using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Columns;

public class DataTableColumnDef
{
    /// <summary>
    /// Alias of targets.
    /// </summary>
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    /// <summary>
    /// Assign a column definition to one or more columns.
    /// </summary>
    [JsonPropertyName("targets")]
    public object? Targets { get; set; } // Can be int, string, or array
}