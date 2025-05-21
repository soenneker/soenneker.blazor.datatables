using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Columns;

public class DataTableColumn
{
    /// <summary>
    /// Set the columns' aria-label attribute value.
    /// </summary>
    [JsonPropertyName("ariaTitle")]
    public string? AriaTitle { get; set; }

    /// <summary>
    /// Cell type to be created for a column.
    /// </summary>
    [JsonPropertyName("cellType")]
    public string? CellType { get; set; }

    /// <summary>
    /// Class to assign to each cell in the column.
    /// </summary>
    [JsonPropertyName("className")]
    public string? ClassName { get; set; }

    /// <summary>
    /// Add padding to the text content used when calculating the optimal width for a table.
    /// </summary>
    [JsonPropertyName("contentPadding")]
    public string? ContentPadding { get; set; }

    /// <summary>
    /// Cell created callback to allow DOM manipulation.
    /// </summary>
    [JsonPropertyName("createdCell")]
    public Action<string, string, object>? CreatedCell { get; set; }

    /// <summary>
    /// Set the data source for the column from the rows data object/array.
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; set; }

    /// <summary>
    /// Set default, static, content for a column.
    /// </summary>
    [JsonPropertyName("defaultContent")]
    public string? DefaultContent { get; set; }

    /// <summary>
    /// Set the column footer text.
    /// </summary>
    [JsonPropertyName("footer")]
    public string? Footer { get; set; }

    /// <summary>
    /// Set a descriptive name for a column.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Enable or disable ordering on this column.
    /// </summary>
    [JsonPropertyName("orderable")]
    public bool? Orderable { get; set; }

    /// <summary>
    /// Define multiple column ordering as the default order for a column.
    /// </summary>
    [JsonPropertyName("orderData")]
    public object? OrderData { get; set; } // Can be int or array

    /// <summary>
    /// Live DOM sorting type assignment.
    /// </summary>
    [JsonPropertyName("orderDataType")]
    public string? OrderDataType { get; set; }

    /// <summary>
    /// Order direction application sequence.
    /// </summary>
    [JsonPropertyName("orderSequence")]
    public List<string>? OrderSequence { get; set; }

    /// <summary>
    /// Enable or disable search on the data in this column.
    /// </summary>
    [JsonPropertyName("searchable")]
    public bool? Searchable { get; set; }

    /// <summary>
    /// Set the column title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Set the column type - used for filtering and sorting string processing.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Enable or disable the display of this column.
    /// </summary>
    [JsonPropertyName("visible")]
    public bool? Visible { get; set; }

    /// <summary>
    /// Column width assignment.
    /// </summary>
    [JsonPropertyName("width")]
    public string? Width { get; set; }
}