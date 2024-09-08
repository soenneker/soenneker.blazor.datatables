using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration.Language.StateRestore
{
    public class DataTablesLanguageStateRestoreCreationModalOptions
    {
        /// <summary>
        /// Set the text within the StateRestore creation modal button.
        /// </summary>
        [JsonPropertyName("button")]
        public string? Button { get; set; }

        /// <summary>
        /// Set the text for the label of the columns search checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("columns.search")]
        public string? ColumnsSearch { get; set; }

        /// <summary>
        /// Set the text for the label of the column visibility checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("columns.visible")]
        public string? ColumnsVisible { get; set; }

        /// <summary>
        /// Set the text for the label of the page length checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("length")]
        public string? Length { get; set; }

        /// <summary>
        /// Set the text for the label of the name input within the creation modal.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Set the text for the label of the order checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("order")]
        public string? Order { get; set; }

        /// <summary>
        /// Set the text for the label of the paging checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("paging")]
        public string? Paging { get; set; }

        /// <summary>
        /// Set the text for the label of the scroller checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("scroller")]
        public string? Scroller { get; set; }

        /// <summary>
        /// Set the text for the label of the search checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("search")]
        public string? Search { get; set; }

        /// <summary>
        /// Set the text for the label of the searchBuilder checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("searchBuilder")]
        public string? SearchBuilder { get; set; }

        /// <summary>
        /// Set the text for the label of the searchPanes checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("searchPanes")]
        public string? SearchPanes { get; set; }

        /// <summary>
        /// Set the text for the label of the select checkbox within the creation modal.
        /// </summary>
        [JsonPropertyName("select")]
        public string? Select { get; set; }

        /// <summary>
        /// Set the text for the title of the creation modal.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Set the label text shown beside the toggle check boxes.
        /// </summary>
        [JsonPropertyName("toggleLabel")]
        public string? ToggleLabel { get; set; }
    }
}
