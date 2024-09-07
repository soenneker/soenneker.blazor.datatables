using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.DataTables.Configuration;
using Soenneker.Extensions.ValueTask;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.DataTables.Base;
using Soenneker.Extensions.String;
using System.Collections.Generic;

namespace Soenneker.Blazor.DataTables;

public partial class DataTable : BaseDataTable
{
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? Attributes { get; set; }

    [Parameter]
    public DataTablesConfiguration Configuration { get; set; } = new();

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

    public async ValueTask Initialize(DataTablesConfiguration? configuration = null, CancellationToken cancellationToken = default)
    {
        if (configuration != null)
            Configuration = configuration;

        DotNetReference = DotNetObjectReference.Create((BaseDataTable)this);

        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CTs.Token);
        await DataTablesInterop.Create(ElementReference, ElementId, DotNetReference, Configuration, linkedCts.Token);
        await DataTablesInterop.CreateObserver(ElementReference, ElementId, cancellationToken);

        await AddEventListeners().NoSync();
    }

    [JSInvokable("OnInitializedJs")]
    public async Task OnInitializedJs()
    {
        if (OnInitialize.HasDelegate)
            await OnInitialize.InvokeAsync();
    }

    private async ValueTask AddEventListeners()
    {
        if (OnDestroy.HasDelegate)
        {
            await AddEventListener<string>(
                GetJsEventName(nameof(OnDestroy)),
                async e => { await OnDestroy.InvokeAsync(); });
        }
    }

    private static string GetJsEventName(string callback)
    {
        // Remove first two characters
        string subStr = callback[2..];
        string result = subStr.ToSnakeCaseFromPascal();
        return result;
    }

    private ValueTask AddEventListener<T>(string eventName, Func<T, ValueTask> callback)
    {
        return InteropEventListener.Add("DataTablesInterop.addEventListener", ElementId, eventName, callback, CTs.Token);
    }
}