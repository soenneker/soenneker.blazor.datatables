using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Responsive;

public sealed class DataTableResponsiveDetails
{

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