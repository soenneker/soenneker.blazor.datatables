using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration.Language.Paginate;

public class DataTableLanguagePaginateOptions
{
    /// <summary>
    /// Pagination 'first' button string.
    /// </summary>
    [JsonPropertyName("first")]
    public string? First { get; set; }

    /// <summary>
    /// Pagination 'last' button string.
    /// </summary>
    [JsonPropertyName("last")]
    public string? Last { get; set; }

    /// <summary>
    /// Pagination 'next' button string.
    /// </summary>
    [JsonPropertyName("next")]
    public string? Next { get; set; }

    /// <summary>
    /// Pagination 'previous' button string.
    /// </summary>
    [JsonPropertyName("previous")]
    public string? Previous { get; set; }
}