using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.DataTables.Configuration;

namespace Soenneker.Blazor.DataTables.Abstract;

public interface IDataTable
{
    DataTableOptions Options { get; set; }

    /// <summary>
    /// Creates the DataTables component with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration for the DataTables component. (Optional)</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation. (Optional)</param>
    ValueTask Initialize(DataTableOptions? configuration = null, CancellationToken cancellationToken = default);
}