using System.Collections.Generic;
using System.Text.Json.Serialization;
using Soenneker.Blazor.DataTables.Options.Language;
using Soenneker.Blazor.DataTables.Options.RowGroup;
using Soenneker.Blazor.DataTables.Options.RowReorder;
using Soenneker.Blazor.DataTables.Options.Search;

namespace Soenneker.Blazor.DataTables.Options;

public partial class DataTableOptions
{
    /// <summary>
    /// Delay the loading of server-side data until second draw.
    /// </summary>
    [JsonPropertyName("deferLoading")]
    public int? DeferLoading { get; set; }

    /// <summary>
    /// Destroy any existing table matching the selector and replace it with new options.
    /// </summary>
    //[JsonPropertyName("destroy")]
    //public bool? Destroy { get; set; }

    /// <summary>
    /// Initial paging start point.
    /// </summary>
    [JsonPropertyName("displayStart")]
    public int? DisplayStart { get; set; }

    /// <summary>
    /// Define the table control elements to appear on the page and in what order.
    /// </summary>
    [JsonPropertyName("dom")]
    public string? Dom { get; set; }

    /// <summary>
    /// Define and position the table control elements to appear on the page.
    /// </summary>
    [JsonPropertyName("layout")]
    public object? Layout { get; set; }

    /// <summary>
    /// Change the options in the page length select list.
    /// </summary>
    [JsonPropertyName("lengthMenu")]
    public object? LengthMenu { get; set; }

    /// <summary>
    /// Initial order (sort) to apply to the table.
    /// </summary>
    [JsonPropertyName("order")]
    public List<object>? Order { get; set; }

    /// <summary>
    /// Control which cell the order event handler will be applied to in a column.
    /// </summary>
    [JsonPropertyName("orderCellsTop")]
    public bool? OrderCellsTop { get; set; }

    /// <summary>
    /// Highlight the columns being ordered in the table's body.
    /// </summary>
    [JsonPropertyName("orderClasses")]
    public bool? OrderClasses { get; set; }

    /// <summary>
    /// Control if the initial data order is reversed when desc ordering.
    /// </summary>
    [JsonPropertyName("orderDescReverse")]
    public bool? OrderDescReverse { get; set; }

    /// <summary>
    /// Ordering to always be applied to the table.
    /// </summary>
    [JsonPropertyName("orderFixed")]
    public List<object>? OrderFixed { get; set; }

    /// <summary>
    /// Multiple column ordering ability control.
    /// </summary>
    [JsonPropertyName("orderMulti")]
    public bool? OrderMulti { get; set; }

    /// <summary>
    /// Change the initial page length (number of rows per page).
    /// </summary>
    [JsonPropertyName("pageLength")]
    public int? PageLength { get; set; }

    /// <summary>
    /// Pagination button display options.
    /// </summary>
    [JsonPropertyName("pagingType")]
    public string? PagingType { get; set; }

    /// <summary>
    /// Display component renderer types.
    /// </summary>
    [JsonPropertyName("renderer")]
    public string? Renderer { get; set; }

    /// <summary>
    /// Retrieve an existing DataTables instance.
    /// </summary>
    [JsonPropertyName("retrieve")]
    public bool? Retrieve { get; set; }

    /// <summary>
    /// Data property name that DataTables will use to set tr element DOM IDs.
    /// </summary>
    [JsonPropertyName("rowId")]
    public string? RowId { get; set; }

    /// <summary>
    /// Allow the table to reduce in height when a limited number of rows are shown.
    /// </summary>
    [JsonPropertyName("scrollCollapse")]
    public bool? ScrollCollapse { get; set; }

    /// <summary>
    /// Set an initial search in DataTables and/or search options.
    /// </summary>
    [JsonPropertyName("search")]
    public DataTablesSearchConfiguration? Search { get; set; }

    /// <summary>
    /// Set a delay for search operations.
    /// </summary>
    [JsonPropertyName("searchDelay")]
    public int? SearchDelay { get; set; }

    /// <summary>
    /// Saved state validity duration.
    /// </summary>
    [JsonPropertyName("stateDuration")]
    public int? StateDuration { get; set; }

    /// <summary>
    /// Tab index control for keyboard navigation.
    /// </summary>
    [JsonPropertyName("tabIndex")]
    public int? TabIndex { get; set; }

    /// <summary>
    /// Language configuration options for DataTables.
    /// </summary>
    [JsonPropertyName("language")]
    public DataTableLanguageOptions? Language { get; set; }

    /// <summary>
    /// Buttons configuration object.
    /// </summary>
    [JsonPropertyName("buttons")]
    public List<object>? Buttons { get; set; }

    /// <summary>
    /// Enable and configure the RowGroup extension for DataTables.
    /// </summary>
    [JsonPropertyName("rowGroup")]
    public DataTableRowGroupOptions? RowGroup { get; set; }

    /// <summary>
    /// Enable and configure the RowReorder extension for DataTables.
    /// </summary>
    [JsonPropertyName("rowReorder")]
    public DataTableRowReorderOptions? RowReorder { get; set; }
}