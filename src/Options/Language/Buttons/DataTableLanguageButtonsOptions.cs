using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Configuration.Language.Buttons;

public class DataTablesLanguageButtonsOptions
{
    /// <summary>
    /// Set the text within the StateRestore creation button.
    /// </summary>
    [JsonPropertyName("createState")]
    public string? CreateState { get; set; }

    /// <summary>
    /// Set the text within the StateRestore remove all button.
    /// </summary>
    [JsonPropertyName("removeAllStates")]
    public string? RemoveAllStates { get; set; }

    /// <summary>
    /// Set the text within the StateRestore remove button.
    /// </summary>
    [JsonPropertyName("removeState")]
    public string? RemoveState { get; set; }

    /// <summary>
    /// Set the text within the StateRestore rename button.
    /// </summary>
    [JsonPropertyName("renameState")]
    public string? RenameState { get; set; }

    /// <summary>
    /// Set the text within the StateRestore savedStates button.
    /// </summary>
    [JsonPropertyName("savedStates")]
    public string? SavedStates { get; set; }

    /// <summary>
    /// Set the text within the StateRestore stateRestore split dropdown button.
    /// </summary>
    [JsonPropertyName("stateRestore")]
    public string? StateRestore { get; set; }

    /// <summary>
    /// Set the text within the StateRestore update button.
    /// </summary>
    [JsonPropertyName("updateState")]
    public string? UpdateState { get; set; }
}