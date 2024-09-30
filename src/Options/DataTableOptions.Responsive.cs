using System.Text.Json.Serialization;
using Soenneker.Blazor.DataTables.Options.Responsive;

namespace Soenneker.Blazor.DataTables.Options;

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