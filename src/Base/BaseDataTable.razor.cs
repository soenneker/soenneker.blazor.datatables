using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.InteropEventListener.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Blazor.DataTables.Base.Abstract;
using Soenneker.Extensions.Task;

namespace Soenneker.Blazor.DataTables.Base;

public partial class BaseDataTable : ComponentBase, IBaseDataTable
{ 
    protected DotNetObjectReference<BaseDataTable>? DotNetReference;

    protected IDataTablesInterop DataTablesInterop = null!;

    protected IInteropEventListener InteropEventListener = null!;

    /// <summary>
    /// The actual HTML element's id
    /// </summary>
    protected readonly string ElementId = $"datatable-{Guid.NewGuid().ToString()}";

    protected readonly CancellationTokenSource CTs = new();

    protected ElementReference ElementReference;

    protected ILogger<BaseDataTable> Logger = null!;

    /// <summary>
    /// Destroys the element.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        DotNetReference?.Dispose();
        InteropEventListener.DisposeForElement(ElementId);

        await CTs.CancelAsync().NoSync();
    }

    protected void LogWarning(string message)
    {
        if (Debug)
            Console.WriteLine(message);
    }

    protected void LogDebug(string message)
    {
        if (Debug)
            Console.WriteLine(message);
    }
}