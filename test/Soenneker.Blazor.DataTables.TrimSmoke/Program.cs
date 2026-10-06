using System.Text.Json;
using Soenneker.Blazor.DataTables;

var configuration = JsonSerializer.Deserialize("{}", LibraryJsonContext.Default.DataTableOptions)!;
var payload = JsonSerializer.SerializeToElement(configuration, LibraryJsonContext.Default.DataTableOptions);
Check(payload.ValueKind == JsonValueKind.Object, "configuration wire object");
var options = LibraryJsonContext.WithContext(SmokeJsonContext.Default);
var custom = JsonSerializer.SerializeToElement<object>(new SmokePayload { Value = "custom" }, (System.Text.Json.Serialization.Metadata.JsonTypeInfo<object>)options.GetTypeInfo(typeof(object)));
Check(custom.GetProperty("value").GetString() == "custom", "application-generated metadata");

Console.WriteLine("Trimmed JSON smoke checks passed.");

static void Check(bool condition, string name)
{
    if (!condition) throw new InvalidOperationException(name);
}
