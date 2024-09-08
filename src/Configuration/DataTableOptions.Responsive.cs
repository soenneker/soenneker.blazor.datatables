using System.Text.Json.Serialization;
using Soenneker.Blazor.DataTables.Configuration.Responsive;

namespace Soenneker.Blazor.DataTables.Configuration;

public partial class DataTableOptions
{
    /// <summary>
    /// Set the column's visibility priority.
    /// </summary>
    [JsonPropertyName("columns.responsivePriority")]
    public int? ResponsivePriority { get; set; }

    /// <summary>
    /// Enable and configure the Responsive extension for DataTables.
    /// </summary>
    [JsonPropertyName("responsive")]
    public DataTableResponsiveOptions? Responsive { get; set; }
}