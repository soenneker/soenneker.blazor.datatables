using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Dtos;

public sealed class DataTablesServerSideRequest
{
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("searchTerm")]
    public string? SearchTerm { get; set; }

    [JsonPropertyName("orderColumn")]
    public int? OrderColumn { get; set; }

    [JsonPropertyName("orderDirection")]
    public string? OrderDirection { get; set; }
}