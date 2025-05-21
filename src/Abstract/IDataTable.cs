using Microsoft.AspNetCore.Components;
using Soenneker.Blazor.DataTables.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.DataTables.Abstract;

/// <summary>
/// Defines the contract for a Blazor DataTable component with initialization, lifecycle management, and auto-render support.
/// </summary>
public interface IDataTable
{
    /// <summary>
    /// Gets or sets additional attributes to apply to the underlying &lt;table&gt; element.
    /// </summary>
    Dictionary<string, object?>? Attributes { get; set; }

    /// <summary>
    /// Gets or sets the DataTable configuration options.
    /// </summary>
    DataTableOptions Options { get; set; }

    /// <summary>
    /// Gets or sets the table content to render inside the component.
    /// </summary>
    RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Initializes the DataTable instance on the current table element.
    /// </summary>
    /// <param name="configuration">Optional configuration to apply before initialization.</param>
    /// <param name="cancellationToken">A cancellation token for the operation.</param>
    /// <returns>A ValueTask representing the async initialization process.</returns>
    ValueTask Initialize(DataTableOptions? configuration = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invoked from JavaScript when the DataTable initialization is complete.
    /// </summary>
    /// <returns>A Task representing the callback logic.</returns>
    Task OnInitializedJs();

    /// <summary>
    /// Destroys and reinitializes the DataTable after executing an asynchronous DOM update operation,
    /// such as modifying the rendered table body. Blazor render is triggered before reinitialization.
    /// </summary>
    /// <param name="domMutator">An asynchronous delegate that modifies the DOM or data backing the table rows.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask RecreateWithDomUpdate(Func<ValueTask> domMutator, CancellationToken cancellationToken = default);

    /// <summary>
    /// Destroys and reinitializes the DataTable after executing a synchronous DOM update operation,
    /// such as modifying the rendered table body. Blazor render is triggered before reinitialization.
    /// </summary>
    /// <param name="domMutator">A delegate that modifies the DOM or data backing the table rows.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask RecreateWithDomUpdate(Action domMutator, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the internal DataTable rows from the DOM without destroying the instance, after executing an asynchronous DOM update operation.
    /// This maintains paging, sorting, and filtering state while updating displayed rows.
    /// </summary>
    /// <param name="domMutator">An asynchronous delegate that modifies the DOM or data backing the table rows.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask RefreshWithDomUpdate(Func<ValueTask> domMutator, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the internal DataTable rows from the DOM without destroying the instance, after executing a synchronous DOM update operation.
    /// This maintains paging, sorting, and filtering state while updating displayed rows.
    /// </summary>
    /// <param name="domMutator">A delegate that modifies the DOM or data backing the table rows.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    ValueTask RefreshWithDomUpdate(Action domMutator, CancellationToken cancellationToken = default);

}