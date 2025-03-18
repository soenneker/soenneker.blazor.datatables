using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.DataTables.Base;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.DataTables;

public partial class DataTable : BaseDataTable
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? Attributes { get; set; }

    [Parameter]
    public DataTableOptions Options { get; set; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool _elementInitialized;

    protected override async Task OnInitializedAsync()
    {
        await DataTablesInterop.Initialize().NoSync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            InteropEventListener.Initialize(DataTablesInterop);
        }

        // This is needed because ElementReference may be null when inside a delayed render scenario
        if (!_elementInitialized && ElementReference.Id.HasContent())
        {
            _elementInitialized = true;
            await Initialize().NoSync();
        }
    }

    public async ValueTask Initialize(DataTableOptions? configuration = null, CancellationToken cancellationToken = default)
    {
        if (configuration != null)
            Options = configuration;

        DotNetReference = DotNetObjectReference.Create<BaseDataTable>(this);

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CTs.Token);
        await DataTablesInterop.Create(ElementReference, ElementId, DotNetReference, Options, linkedCts.Token).NoSync();
        await DataTablesInterop.CreateObserver(ElementReference, ElementId, linkedCts.Token).NoSync();

        await AddEventListeners(linkedCts.Token).NoSync();
    }

    [JSInvokable("OnInitializedJs")]
    public async Task OnInitializedJs()
    {
        if (OnInitialize.HasDelegate)
            await OnInitialize.InvokeAsync().NoSync();
    }

    private async ValueTask AddEventListeners(CancellationToken cancellationToken = default)
    {
        if (OnDestroy.HasDelegate)
        {
            await AddEventListener<string>(GetJsEventName(nameof(OnDestroy)), async _ => { await OnDestroy.InvokeAsync().NoSync(); }).NoSync();
        }
    }

    private static string GetJsEventName(string callback)
    {
        // Remove first two characters
        string subStr = callback[2..];
        return subStr.ToSnakeCaseFromPascal();
    }

    private ValueTask AddEventListener<T>(string eventName, Func<T, ValueTask> callback)
    {
        return InteropEventListener.Add("DataTablesInterop.addEventListener", ElementId, eventName, callback, CTs.Token);
    }
}