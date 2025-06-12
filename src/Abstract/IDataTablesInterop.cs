using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.Blazor.Utils.EventListeningInterop.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    ValueTask Create(ElementReference elementReference, string elementId, DotNetObjectReference<DataTable> dotNetObjectRef,
        DataTableOptions? configuration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys the DataTable instance associated with the specified element and cleans up related resources.
    /// </summary>
    /// <param name="elementReference">The ElementReference of the table to destroy.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the DataTable instance, re-reading the data from the DOM.
    /// </summary>
    /// <param name="elementId">The ID of the table element.</param>
    /// <param name="cancellationToken">An optional token to cancel the operation.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask Refresh(string elementId, CancellationToken cancellationToken = default);
}