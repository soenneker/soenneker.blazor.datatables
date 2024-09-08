using Soenneker.Blazor.DataTables.Abstract;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Soenneker.Blazor.Utils.EventListeningInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Extensions.ValueTask;
using System.Threading;
using Microsoft.AspNetCore.Components;
using Soenneker.Utils.Json;
using System;
using Soenneker.Blazor.DataTables.Base;
using Soenneker.Blazor.DataTables.Configuration;

namespace Soenneker.Blazor.DataTables;

/// <inheritdoc cref="IDataTablesInterop"/>
public class DataTablesInterop: EventListeningInterop, IDataTablesInterop
{
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncSingleton<object> _scriptInitializer;

    public DataTablesInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader) : base(jSRuntime)
    {
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton<object>(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable("Soenneker.Blazor.DataTables/datatablesinterop.js", "DataTablesInterop", 100, token).NoSync();

            return new object();
        });
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Get(cancellationToken).NoSync();
    }

    public ValueTask CreateObserver(ElementReference elementReference, string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DataTablesInterop.createObserver", cancellationToken, elementReference, elementId);
    }

    public async ValueTask Create(ElementReference elementReference, string elementId, DotNetObjectReference<BaseDataTable> dotNetObjectRef, DataTableOptions? configuration = null,
        CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Get(cancellationToken).NoSync();

        string? json = null;

        if (configuration != null)
            json = JsonUtil.Serialize(configuration);

        await JsRuntime.InvokeVoidAsync("DataTablesInterop.create", cancellationToken, elementReference, elementId, json, dotNetObjectRef);
    }

    public ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DataTablesInterop.destroy", cancellationToken, elementReference);
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return _resourceLoader.DisposeModule("Soenneker.Blazor.DataTables/datatablesinterop.js");
    }
}
