using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.Blazor.Utils.EventListeningInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.DataTables;

/// <inheritdoc cref="IDataTablesInterop"/>
public sealed class DataTablesInterop : EventListeningInterop, IDataTablesInterop
{
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncSingleton _scriptInitializer;
    private readonly AsyncSingleton _styleInitializer;

    private const string _modulePath = "Soenneker.Blazor.DataTables/js/datatablesinterop.js";
    private const string _moduleName = "DataTablesInterop";

    public DataTablesInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader) : base(jSRuntime)
    {
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleName, 100, token);

            return new object();
        });

        _styleInitializer = new AsyncSingleton(async (token, _) =>
        {
            await _resourceLoader.LoadStyle("_content/Soenneker.Blazor.DataTables/css/datatables.css", cancellationToken: token);
            return new object();
        });
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken);
        await _styleInitializer.Init(cancellationToken);
    }

    public ValueTask CreateObserver(ElementReference elementReference, string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_moduleName}.createObserver", cancellationToken, elementReference, elementId);
    }

    public async ValueTask Create(ElementReference elementReference, string elementId, DotNetObjectReference<DataTable> dotNetObjectRef,
        DataTableOptions? configuration = null, CancellationToken cancellationToken = default)
    {
        await Initialize(cancellationToken);

        string? json = null;

        if (configuration != null)
            json = JsonUtil.Serialize(configuration);

        await JsRuntime.InvokeVoidAsync($"{_moduleName}.create", cancellationToken, elementReference, elementId, json, dotNetObjectRef);
    }

    public ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_moduleName}.destroy", cancellationToken, elementReference);
    }

    public ValueTask Refresh(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_moduleName}.refresh", cancellationToken, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);

        await _scriptInitializer.DisposeAsync();
        await _styleInitializer.DisposeAsync();
    }
}
