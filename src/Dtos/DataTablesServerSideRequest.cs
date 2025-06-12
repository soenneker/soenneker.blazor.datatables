namespace Soenneker.Blazor.DataTables.Dtos;

public class DataTablesServerSideRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SearchTerm { get; set; }
    public int? OrderColumn { get; set; }
    public string? OrderDirection { get; set; }
}