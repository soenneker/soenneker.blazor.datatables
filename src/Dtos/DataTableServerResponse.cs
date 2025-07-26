using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Dtos;

/// <summary>
/// Represents a server response for DataTables server-side processing.
/// </summary>
public sealed class DataTableServerResponse
{
    /// <summary>
    /// Gets or sets the draw counter that DataTables is expecting back from the server.
    /// </summary>
    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    /// <summary>
    /// Gets or sets the total number of records before filtering.
    /// </summary>
    [JsonPropertyName("totalRecords")]
    public int TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the total number of records after filtering.
    /// </summary>
    [JsonPropertyName("totalFilteredRecords")]
    public int TotalFilteredRecords { get; set; }

    /// <summary>
    /// Gets or sets the data to be displayed in the table.
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; set; }

    /// <summary>
    /// Gets or sets an optional error message to be displayed by DataTables.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// Creates a success response for DataTables server-side processing.
    /// </summary>
    /// <param name="draw">The draw counter from the request.</param>
    /// <param name="recordsTotal">Total number of records before filtering.</param>
    /// <param name="recordsFiltered">Total number of records after filtering.</param>
    /// <param name="data">The data to be displayed.</param>
    /// <returns>A configured DataTablesServerResponse.</returns>
    public static DataTableServerResponse Success(int draw, int recordsTotal, int recordsFiltered, object data)
    {
        return new DataTableServerResponse
        {
            Draw = draw,
            TotalRecords = recordsTotal,
            TotalFilteredRecords = recordsFiltered,
            Data = data
        };
    }

    /// <summary>
    /// Creates an error response for DataTables server-side processing.
    /// </summary>
    /// <param name="draw">The draw counter from the request.</param>
    /// <param name="errorMessage">The error message to display.</param>
    /// <returns>A configured DataTablesServerResponse with the error message.</returns>
    public static DataTableServerResponse Fail(int draw, string errorMessage)
    {
        return new DataTableServerResponse
        {
            Draw = draw,
            Error = errorMessage
        };
    }
}
