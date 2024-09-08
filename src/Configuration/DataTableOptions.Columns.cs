using Soenneker.Blazor.DataTables.Configuration.Columns;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration;

public partial class DataTableOptions
{
    /// <summary>
    /// Set column definition initialization properties.
    /// </summary>
    [JsonPropertyName("columnDefs")]
    public List<DataTableColumnDef>? ColumnDefs { get; set; }

    /// <summary>
    /// Set column specific initialization properties.
    /// </summary>
    [JsonPropertyName("columns")]
    public List<DataTableColumn>? Columns { get; set; }
}