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
/// A Blazor interop library for DataTables
/// </summary>
public interface IDataTablesInterop : IEventListeningInterop, IAsyncDisposable
{
    ValueTask CreateObserver(ElementReference elementReference, string elementId, CancellationToken cancellationToken = default);

    ValueTask Initialize(CancellationToken cancellationToken = default);

    ValueTask Create(ElementReference elementReference, string elementId, DotNetObjectReference<BaseDataTable> dotNetObjectRef, DataTableOptions? configuration = null, CancellationToken cancellationToken = default);

    ValueTask Destroy(ElementReference elementReference, CancellationToken cancellationToken = default);
}
