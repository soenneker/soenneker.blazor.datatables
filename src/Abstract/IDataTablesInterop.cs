using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Threading;
using Soenneker.Blazor.Utils.EventListeningInterop.Abstract;
using System;
using Soenneker.Blazor.DataTables.Base;
using Soenneker.Blazor.DataTables.Options;

namespace Soenneker.Blazor.DataTables.Abstract;

/// <summary>
/// Provides JavaScript interop functionality for working with DataTables in Blazor.
/// </summary>
public interface IDataTablesInterop : IEventListeningInterop, IAsyncDisposable
{
    /// <summary>
    /// Creates a DOM mutation observer to monitor and respond to the removal of the specified element.
    /// </summary>
    /// <param name="elementReference">The ElementReference to observe.</param>
    /// <param name="elementId">The ID of the element being observed.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask CreateObserver(ElementReference elementReference, string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes the DataTables JavaScript environment. Should be called once before any other interop operations.
    /// </summary>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates and initializes a DataTable instance on the specified HTML table element.
    /// </summary>
    /// <param name="elementReference">The ElementReference to the table element.</param>
    /// <param name="elementId">The ID of the table element.</param>
    /// <param name="dotNetObjectRef">A .NET object reference used for JS-to-.NET callback handling.</param>
    /// <param name="configuration">Optional configuration options for the DataTable instance.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask Create(ElementReference elementReference, string elementId, DotNetObjectReference<BaseDataTable> dotNetObjectRef,
        DataTableOptions? configuration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys the DataTable instance associated with the specified element and cleans up related resources.
    /// </summary>
    /// <param name="elementReference">The ElementReference of the table to destroy.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default);

    ValueTask Refresh(string elementId, CancellationToken cancellationToken = default);
}