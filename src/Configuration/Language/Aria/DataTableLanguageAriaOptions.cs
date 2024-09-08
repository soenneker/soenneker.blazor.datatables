using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration.Language.Aria;

public class DataTableLanguageAriaOptions
{
    /// <summary>
    /// Language string used for WAI-ARIA column orderable label.
    /// </summary>
    [JsonPropertyName("orderable")]
    public string? Orderable { get; set; }

    /// <summary>
    /// Language string used for WAI-ARIA column label to alter column's ordering.
    /// </summary>
    [JsonPropertyName("orderableRemove")]
    public string? OrderableRemove { get; set; }

    /// <summary>
    /// Language string used for WAI-ARIA column label to alter column's ordering.
    /// </summary>
    [JsonPropertyName("orderableReverse")]
    public string? OrderableReverse { get; set; }

    /// <summary>
    /// WAI-ARIA labels for pagination buttons.
    /// </summary>
    [JsonPropertyName("paginate")]
    public DataTableLanguageAriaPaginateOptions? Paginate { get; set; }

    /// <summary>
    /// Language strings used for WAI-ARIA specific attributes for sort ascending.
    /// </summary>
    [JsonPropertyName("sortAscending")]
    public string? SortAscending { get; set; }

    /// <summary>
    /// Language strings used for WAI-ARIA specific attributes for sort descending.
    /// </summary>
    [JsonPropertyName("sortDescending")]
    public string? SortDescending { get; set; }
}