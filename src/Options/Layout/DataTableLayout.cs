using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Layout;

public class DataTableLayout
{
    [JsonPropertyName("topStart")]
    public string? TopStart { get; set; }

    [JsonPropertyName("topEnd")]
    public string? TopEnd { get; set; }

    [JsonPropertyName("top")]
    public string? Top { get; set; }

    [JsonPropertyName("bottomStart")]
    public string? BottomStart { get; set; }

    [JsonPropertyName("bottomEnd")]
    public string? BottomEnd { get; set; }

    [JsonPropertyName("bottom")]
    public string? Bottom { get; set; }
}