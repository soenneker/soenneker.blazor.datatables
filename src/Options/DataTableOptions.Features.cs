using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options;

public partial class DataTableOptions
{
    /// <summary>
    /// Feature control DataTables' smart column width handling.
    /// </summary>
    [JsonPropertyName("autoWidth")]
    public bool? AutoWidth { get; set; }

    /// <summary>
    /// Set a caption for the table.
    /// </summary>
    [JsonPropertyName("caption")]
    public string? Caption { get; set; }

    /// <summary>
    /// Feature control deferred rendering for additional speed of initialization.
    /// </summary>
    [JsonPropertyName("deferRender")]
    public bool? DeferRender { get; set; }

    /// <summary>
    /// Feature control table information display field.
    /// </summary>
    [JsonPropertyName("info")]
    public bool? Info { get; set; }

    /// <summary>
    /// Feature control the end user's ability to change the paging display length of the table.
    /// </summary>
    [JsonPropertyName("lengthChange")]
    public bool? LengthChange { get; set; }

    /// <summary>
    /// Feature control ordering (sorting) abilities in DataTables.
    /// </summary>
    [JsonPropertyName("ordering")]
    public bool? Ordering { get; set; }

    /// <summary>
    /// Enable or disable table pagination.
    /// </summary>
    [JsonPropertyName("paging")]
    public bool? Paging { get; set; }

    /// <summary>
    /// Feature control the processing indicator.
    /// </summary>
    [JsonPropertyName("processing")]
    public bool? Processing { get; set; }

    /// <summary>
    /// Enable horizontal scrolling.
    /// </summary>
    [JsonPropertyName("scrollX")]
    public bool? ScrollX { get; set; }

    /// <summary>
    /// Enable vertical scrolling.
    /// </summary>
    [JsonPropertyName("scrollY")]
    public string? ScrollY { get; set; }

    /// <summary>
    /// Feature control search (filtering) abilities.
    /// </summary>
    [JsonPropertyName("searching")]
    public bool? Searching { get; set; }

    /// <summary>
    /// Feature control DataTables' server-side processing mode.
    /// </summary>
    [JsonPropertyName("serverSide")]
    public bool? ServerSide { get; set; }

    /// <summary>
    /// State saving - restore table state on page reload.
    /// </summary>
    [JsonPropertyName("stateSave")]
    public bool? StateSave { get; set; }
}