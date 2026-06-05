using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using Soenneker.Utils.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.String;

namespace Soenneker.Blazor.DataTables;

/// <inheritdoc cref="IDataTablesInterop"/>
public sealed class DataTablesInterop : IDataTablesInterop
{
    private const string _modulePath = "_content/Soenneker.Blazor.DataTables/js/datatablesinterop.js";

    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly AsyncInitializer _scriptInitializer;
    private readonly AsyncInitializer _styleInitializer;

    private readonly CancellationScope _cancellationScope = new();

    public DataTablesInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;
        _scriptInitializer = new AsyncInitializer(InitializeScript);
        _styleInitializer = new AsyncInitializer(InitializeStyle);
    }

    private async ValueTask InitializeScript(CancellationToken token)
    {
        _ = await _moduleImportUtil.GetContentModuleReference(_modulePath, token);
    }

    private async ValueTask InitializeStyle(CancellationToken token)
    {
        await _resourceLoader.LoadStyle("_content/Soenneker.Blazor.DataTables/css/datatables.css", cancellationToken: token);
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _styleInitializer.Init(linked);
        }
    }

    public async ValueTask CreateObserver(ElementReference elementReference, string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("createObserver", linked, elementReference, elementId);
        }
    }

    public async ValueTask Create(ElementReference elementReference, string elementId, DotNetObjectReference<DataTable> dotNetObjectRef,
        DataTableOptions? configuration = null, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await Initialize(linked);

            string? json = null;

            if (configuration != null)
                json = JsonUtil.Serialize(configuration);

            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("create", linked, elementReference, elementId, json, dotNetObjectRef);
        }
    }

    public async ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("destroy", linked, elementReference);
        }
    }

    public async ValueTask Refresh(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("refresh", linked, elementId);
        }
    }

    /// <summary>
    /// Adds event listener.
    /// </summary>
    /// <param name="functionName">The function name.</param>
    /// <param name="elementId">The element id.</param>
    /// <param name="eventName">The event name.</param>
    /// <param name="dotNetCallback">The dot net callback.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask AddEventListener(string functionName, string elementId, string eventName, object dotNetCallback,
        CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            string identifier = functionName.Replace("DataTablesInterop.", "", StringComparison.Ordinal);
            await module.InvokeVoidAsync(identifier, linked, elementId, eventName, dotNetCallback);
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);

        await _scriptInitializer.DisposeAsync();
        await _styleInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
