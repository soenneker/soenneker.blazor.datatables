namespace Soenneker.Blazor.DataTables.Options;

/// <summary>
/// Base class for DataTable event arguments
/// </summary>
public abstract class DataTableEventArgs
{
    /// <summary>
    /// The current page number (1-based)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// The number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The total number of records available
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// The total number of filtered records
    /// </summary>
    public int TotalFilteredRecords { get; set; }
} 