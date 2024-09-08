using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration.Responsive;

public class DataTableResponsiveBreakpoint
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