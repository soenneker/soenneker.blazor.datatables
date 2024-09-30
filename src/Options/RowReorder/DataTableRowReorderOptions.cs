using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.RowReorder;

public class DataTableRowReorderOptions
{
    /// <summary>
    /// Enable or disable the canceling of the drag and drop interaction.
    /// </summary>
    [JsonPropertyName("cancelable")]
    public bool? Cancelable { get; set; }

    /// <summary>
    /// Configure the data point that will be used for the reordering data.
    /// </summary>
    [JsonPropertyName("dataSrc")]
    public string? DataSrc { get; set; }

    /// <summary>
    /// Attach an Editor instance for database updating.
    /// </summary>
    [JsonPropertyName("editor")]
    public object? Editor { get; set; } // Using object as a placeholder for actual editor instance type

    /// <summary>
    /// Enable or disable RowReorder's user interaction.
    /// </summary>
    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }

    /// <summary>
    /// Set the options for the Editor form when submitting data.
    /// </summary>
    [JsonPropertyName("formOptions")]
    public object? FormOptions { get; set; } // This can be further specified based on Editor form options

    /// <summary>
    /// Define the selector used to pick the elements that will start a drag.
    /// </summary>
    [JsonPropertyName("selector")]
    public string? Selector { get; set; }

    /// <summary>
    /// Control the horizontal position of the row being dragged.
    /// </summary>
    [JsonPropertyName("snapX")]
    public bool? SnapX { get; set; }

    /// <summary>
    /// Control automatic update of data when a row is dropped.
    /// </summary>
    [JsonPropertyName("update")]
    public bool? Update { get; set; }
}