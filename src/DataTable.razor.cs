using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Extensions.ValueTask;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.DataTables.Base;
using Soenneker.Extensions.String;
using System.Collections.Generic;
using Soenneker.Blazor.DataTables.Options;

namespace Soenneker.Blazor.DataTables;

public partial class DataTable : BaseDataTable
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? Attributes { get; set; }

    [Parameter]
    public DataTableOptions Options { get; set; } = new();

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await DataTablesInterop.Initialize();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            InteropEventListener.Initialize(DataTablesInterop);
            await Initialize();
        }
    }

    public async ValueTask Initialize(DataTableOptions? configuration = null, CancellationToken cancellationToken = default)
    {
        if (configuration != null)
            Options = configuration;

        DotNetReference = DotNetObjectReference.Create<BaseDataTable>(this);

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CTs.Token);
        await DataTablesInterop.Create(ElementReference, ElementId, DotNetReference, Options, linkedCts.Token);
        await DataTablesInterop.CreateObserver(ElementReference, ElementId, cancellationToken);

        await AddEventListeners(linkedCts.Token).NoSync();
    }

    [JSInvokable("OnInitializedJs")]
    public async Task OnInitializedJs()
    {
        if (OnInitialize.HasDelegate)
            await OnInitialize.InvokeAsync();
    }

    private async ValueTask AddEventListeners(CancellationToken cancellationToken = default)
    {
        if (OnDestroy.HasDelegate)
        {
            await AddEventListener<string>(
                GetJsEventName(nameof(OnDestroy)),
                async (_, _) => { await OnDestroy.InvokeAsync(); });
        }
    }

    private static string GetJsEventName(string callback)
    {
        // Remove first two characters
        string subStr = callback[2..];
        return subStr.ToSnakeCaseFromPascal();
    }

    private ValueTask AddEventListener<T>(string eventName, Func<T, CancellationToken, ValueTask> callback)
    {
        return InteropEventListener.Add("DataTablesInterop.addEventListener", ElementId, eventName, callback, CTs.Token);
    }
}