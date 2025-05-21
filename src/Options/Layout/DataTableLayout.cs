using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Layout;

/// <summary>
/// Represents the layout configuration for a DataTable, allowing customization of control positioning within the table.
/// </summary>
public sealed class DataTableLayout
{
    /// <summary>
    /// Gets or sets the layout content positioned at the top start of the DataTable.
    /// </summary>
    [JsonPropertyName("topStart")]
    public string? TopStart { get; set; }

    /// <summary>
    /// Gets or sets the layout content positioned at the top end of the DataTable.
    /// </summary>
    [JsonPropertyName("topEnd")]
    public string? TopEnd { get; set; }

    /// <summary>
    /// Gets or sets the layout content positioned across the full top area of the DataTable.
    /// </summary>
    [JsonPropertyName("top")]
    public string? Top { get; set; }

    /// <summary>
    /// Gets or sets the layout content positioned at the bottom start of the DataTable.
    /// </summary>
    [JsonPropertyName("bottomStart")]
    public string? BottomStart { get; set; }

    /// <summary>
    /// Gets or sets the layout content positioned at the bottom end of the DataTable.
    /// </summary>
    [JsonPropertyName("bottomEnd")]
    public string? BottomEnd { get; set; }

    /// <summary>
    /// Gets or sets the layout content positioned across the full bottom area of the DataTable.
    /// </summary>
    [JsonPropertyName("bottom")]
    public string? Bottom { get; set; }
}