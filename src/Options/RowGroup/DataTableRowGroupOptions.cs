using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.RowGroup;

public class DataTableRowGroupOptions
{
    /// <summary>
    /// Set the class name to be used for the grouping rows.
    /// </summary>
    [JsonPropertyName("className")]
    public string? ClassName { get; set; }

    /// <summary>
    /// Set the data point to use as the grouping data source.
    /// </summary>
    [JsonPropertyName("dataSrc")]
    public string? DataSrc { get; set; }

    /// <summary>
    /// Text to show for rows which have null, undefined or empty string group data.
    /// </summary>
    [JsonPropertyName("emptyDataGroup")]
    public string? EmptyDataGroup { get; set; }

    /// <summary>
    /// Provides the ability to disable row grouping at initialization.
    /// </summary>
    [JsonPropertyName("enable")]
    public bool? Enable { get; set; }

    /// <summary>
    /// Set the class name to be used for the grouping end rows.
    /// </summary>
    [JsonPropertyName("endClassName")]
    public string? EndClassName { get; set; }

    /// <summary>
    /// Provide a function that can be used to control the data shown in the end grouping row.
    /// </summary>
    //[JsonPropertyName("endRender")]
    //public Func<object, object>? EndRender { get; set; }

    /// <summary>
    /// Set the class name to be used for the grouping start rows.
    /// </summary>
    [JsonPropertyName("startClassName")]
    public string? StartClassName { get; set; }

    /// <summary>
    /// Provide a function that can be used to control the data shown in the start grouping row.
    /// </summary>
    //[JsonPropertyName("startRender")]
    //public Func<object, object>? StartRender { get; set; }
}