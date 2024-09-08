using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration.Responsive;

public class DataTableResponsiveDetails
{
    /// <summary>
    /// Define how the hidden information should be displayed to the end user.
    /// </summary>
    // [JsonPropertyName("display")]
    //public Func<object, object>? Display { get; set; }

    /// <summary>
    /// Define the renderer used to display the child rows.
    /// </summary>
    // [JsonPropertyName("renderer")]
    // public Func<object, object>? Renderer { get; set; }

    /// <summary>
    /// Column/selector for child row display control when using column details type.
    /// </summary>
    [JsonPropertyName("target")]
    public string? Target { get; set; }

    /// <summary>
    /// Set the child row display control type.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}