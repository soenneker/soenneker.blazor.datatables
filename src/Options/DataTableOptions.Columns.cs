using System.Collections.Generic;
using System.Text.Json.Serialization;
using Soenneker.Blazor.DataTables.Options.Columns;
using Soenneker.DataTables.Dtos.Column;

namespace Soenneker.Blazor.DataTables.Options;

public sealed partial class DataTableOptions
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