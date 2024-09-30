using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Responsive;

public class DataTableResponsiveOptions
{
    /// <summary>
    /// Set the breakpoints for a Responsive instance.
    /// </summary>
    [JsonPropertyName("breakpoints")]
    public List<DataTableResponsiveBreakpoint>? Breakpoints { get; set; }

    /// <summary>
    /// Enable and configure the child rows shown by Responsive for collapsed tables.
    /// </summary>
    [JsonPropertyName("details")]
    public DataTableResponsiveDetails? Details { get; set; }

    /// <summary>
    /// Set the orthogonal data request type for the hidden information display.
    /// </summary>
    [JsonPropertyName("orthogonal")]
    public string? Orthogonal { get; set; }
}