using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Responsive;

public sealed class DataTableResponsiveBreakpoint
{
    /// <summary>
    /// The width at which the breakpoint occurs.
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// A name for the breakpoint.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}