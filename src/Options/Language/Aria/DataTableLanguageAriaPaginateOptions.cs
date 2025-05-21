using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Language.Aria;

public sealed class DataTableLanguageAriaPaginateOptions
{
    /// <summary>
    /// WAI-ARIA label for the first pagination button.
    /// </summary>
    [JsonPropertyName("first")]
    public string? First { get; set; }

    /// <summary>
    /// WAI-ARIA label for the last pagination button.
    /// </summary>
    [JsonPropertyName("last")]
    public string? Last { get; set; }

    /// <summary>
    /// WAI-ARIA label for the next pagination button.
    /// </summary>
    [JsonPropertyName("next")]
    public string? Next { get; set; }

    /// <summary>
    /// WAI-ARIA label for the number pagination buttons.
    /// </summary>
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    /// <summary>
    /// WAI-ARIA label for the previous pagination button.
    /// </summary>
    [JsonPropertyName("previous")]
    public string? Previous { get; set; }
}