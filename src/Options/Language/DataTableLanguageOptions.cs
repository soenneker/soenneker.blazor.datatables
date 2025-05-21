using System.Text.Json.Serialization;
using Soenneker.Blazor.DataTables.Options.Language.Aria;
using Soenneker.Blazor.DataTables.Options.Language.Paginate;
using Soenneker.Blazor.DataTables.Options.Language.StateRestore;

namespace Soenneker.Blazor.DataTables.Options.Language;

public sealed class DataTableLanguageOptions
{
    /// <summary>
    /// Language strings used for WAI-ARIA specific attributes.
    /// </summary>
    [JsonPropertyName("aria")]
    public DataTableLanguageAriaOptions? Aria { get; set; }

    /// <summary>
    /// Decimal place character.
    /// </summary>
    [JsonPropertyName("decimal")]
    public string? Decimal { get; set; }

    /// <summary>
    /// Table has no records string.
    /// </summary>
    [JsonPropertyName("emptyTable")]
    public string? EmptyTable { get; set; }

    /// <summary>
    /// Replacement pluralisation for table data type.
    /// </summary>
    [JsonPropertyName("entries")]
    public string? Entries { get; set; }

    /// <summary>
    /// Table summary information display string.
    /// </summary>
    [JsonPropertyName("info")]
    public string? Info { get; set; }

    /// <summary>
    /// Table summary information string used when the table is empty of records.
    /// </summary>
    [JsonPropertyName("infoEmpty")]
    public string? InfoEmpty { get; set; }

    /// <summary>
    /// Appended string to the summary information when the table is filtered.
    /// </summary>
    [JsonPropertyName("infoFiltered")]
    public string? InfoFiltered { get; set; }

    /// <summary>
    /// String to append to all other summary information strings.
    /// </summary>
    [JsonPropertyName("infoPostFix")]
    public string? InfoPostFix { get; set; }

    /// <summary>
    /// Page length options string.
    /// </summary>
    [JsonPropertyName("lengthMenu")]
    public string? LengthMenu { get; set; }

    /// <summary>
    /// Loading information display string - shown when Ajax loading data.
    /// </summary>
    [JsonPropertyName("loadingRecords")]
    public string? LoadingRecords { get; set; }

    /// <summary>
    /// Pagination specific language strings.
    /// </summary>
    [JsonPropertyName("paginate")]
    public DataTableLanguagePaginateOptions? Paginate { get; set; }

    /// <summary>
    /// Processing indicator string.
    /// </summary>
    [JsonPropertyName("processing")]
    public string? Processing { get; set; }

    /// <summary>
    /// Search input string.
    /// </summary>
    [JsonPropertyName("search")]
    public string? Search { get; set; }

    /// <summary>
    /// Search input element placeholder attribute.
    /// </summary>
    [JsonPropertyName("searchPlaceholder")]
    public string? SearchPlaceholder { get; set; }

    /// <summary>
    /// Thousands separator.
    /// </summary>
    [JsonPropertyName("thousands")]
    public string? Thousands { get; set; }

    /// <summary>
    /// Load language information from remote file.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Table empty as a result of filtering string.
    /// </summary>
    [JsonPropertyName("zeroRecords")]
    public string? ZeroRecords { get; set; }

    /// <summary>
    /// Container for button-specific language options.
    /// </summary>
    [JsonPropertyName("buttons")]
    public object? Buttons { get; set; }

    /// <summary>
    /// Container for state restore-specific language options.
    /// </summary>
    [JsonPropertyName("stateRestore")]
    public DataTablesLanguageStateRestoreOptions? StateRestore { get; set; }
}