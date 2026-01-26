using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.Blazor.Utils.EventListeningInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using Soenneker.Utils.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.DataTables;

/// <inheritdoc cref="IDataTablesInterop"/>
public sealed class DataTablesInterop : EventListeningInterop, IDataTablesInterop
{
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncInitializer _scriptInitializer;
    private readonly AsyncInitializer _styleInitializer;

    private const string _modulePath = "Soenneker.Blazor.DataTables/js/datatablesinterop.js";
    private const string _moduleName = "DataTablesInterop";

    private readonly CancellationScope _cancellationScope = new();

    public DataTablesInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader)
        : base(jSRuntime)
    {
        _resourceLoader = resourceLoader;
        _scriptInitializer = new AsyncInitializer(InitializeScript);
        _styleInitializer = new AsyncInitializer(InitializeStyle);
    }

    private ValueTask InitializeScript(CancellationToken token)
    {
        return _resourceLoader.ImportModuleAndWaitUntilAvailable(
            _modulePath,
            _moduleName,
            100,
            token);
    }

    private ValueTask InitializeStyle(CancellationToken token)
    {
        return _resourceLoader.LoadStyle(
            "_content/Soenneker.Blazor.DataTables/css/datatables.css",
            cancellationToken: token);
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _styleInitializer.Init(linked);
        }
    }

    public ValueTask CreateObserver(
        ElementReference elementReference,
        string elementId,
        CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return JsRuntime.InvokeVoidAsync(
                "DataTablesInterop.createObserver",
                linked,
                elementReference,
                elementId);
    }

    public async ValueTask Create(
        ElementReference elementReference,
        string elementId,
        DotNetObjectReference<DataTable> dotNetObjectRef,
        DataTableOptions? configuration = null,
        CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await Initialize(linked);

            string? json = null;

            if (configuration != null)
                json = JsonUtil.Serialize(configuration);

            await JsRuntime.InvokeVoidAsync(
                "DataTablesInterop.create",
                linked,
                elementReference,
                elementId,
                json,
                dotNetObjectRef);
        }
    }

    public ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return JsRuntime.InvokeVoidAsync(
                "DataTablesInterop.destroy",
                linked,
                elementReference);
    }

    public ValueTask Refresh(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return JsRuntime.InvokeVoidAsync(
                "DataTablesInterop.refresh",
                linked,
                elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);

        await _scriptInitializer.DisposeAsync();
        await _styleInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
