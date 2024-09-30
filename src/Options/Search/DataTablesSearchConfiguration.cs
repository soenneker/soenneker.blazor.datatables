using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Search;

public class DataTablesSearchConfiguration
{
    /// <summary>
    /// Control case-sensitive filtering option.
    /// </summary>
    [JsonPropertyName("caseInsensitive")]
    public bool? CaseInsensitive { get; set; }

    /// <summary>
    /// Enable/disable escaping of regular expression characters in the search term.
    /// </summary>
    [JsonPropertyName("regex")]
    public bool? Regex { get; set; }

    /// <summary>
    /// Enable/disable DataTables' search on return.
    /// </summary>
    [JsonPropertyName("return")]
    public bool? Return { get; set; }

    /// <summary>
    /// Set an initial filtering condition on the table.
    /// </summary>
    [JsonPropertyName("search")]
    public string? Search { get; set; }

    /// <summary>
    /// Enable/disable DataTables' smart filtering.
    /// </summary>
    [JsonPropertyName("smart")]
    public bool? Smart { get; set; }

    /// <summary>
    /// Define an initial search for individual columns.
    /// </summary>
    [JsonPropertyName("searchCols")]
    public List<string>? SearchCols { get; set; }
}