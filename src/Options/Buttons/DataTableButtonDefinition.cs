using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Buttons;

public sealed class DataTableButtonDefinition
{
    /// <summary>
    /// Indicate that a button's action processing should be performed asynchronously.
    /// </summary>
    [JsonPropertyName("async")]
    public bool? Async { get; set; }

    /// <summary>
    /// Collection of attribute key / values to set for a button.
    /// </summary>
    [JsonPropertyName("attr")]
    public Dictionary<string, string>? Attr { get; set; }

    /// <summary>
    /// Set the class name for the button.
    /// </summary>
    [JsonPropertyName("className")]
    public string? ClassName { get; set; }

    /// <summary>
    /// Set a button's initial enabled state.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    /// <summary>
    /// Define which button type the button should be based on.
    /// </summary>
    [JsonPropertyName("extend")]
    public string? Extend { get; set; }

    /// <summary>
    /// Define an activation key for a button.
    /// </summary>
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Set a name for each selection.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Unique namespace for every button.
    /// </summary>
    [JsonPropertyName("namespace")]
    public string? Namespace { get; set; }

    /// <summary>
    /// Split dropdown buttons.
    /// </summary>
    [JsonPropertyName("split")]
    public bool? Split { get; set; }

    /// <summary>
    /// Set the tag for the button.
    /// </summary>
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// The text to show in the button.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// Button title attribute text.
    /// </summary>
    [JsonPropertyName("titleAttr")]
    public string? TitleAttr { get; set; }
}