using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Blazor.DataTables.Base.Abstract;

public interface IBaseDataTable 
{
    #region Events

    /// <summary>
    /// Event triggered when the component is initialized.
    /// </summary>
    EventCallback OnInitialize { get; set; }


    /// <summary>
    /// Event triggered when the component is destroyed.
    /// </summary>
    EventCallback OnDestroy { get; set; }

    /// <summary>
    /// For debugging to log messages
    /// </summary>
    bool Debug { get; set; }

    #endregion Events

    /// <summary>
    /// Destroys the component.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation. (Optional)</param>
    ValueTask Destroy(CancellationToken cancellationToken = default);
}