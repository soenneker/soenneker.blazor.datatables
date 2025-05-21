using System.Text.Json.Serialization;

namespace Soenneker.Blazor.DataTables.Options.Language.StateRestore;

public sealed class DataTablesLanguageStateRestoreOptions
{
    /// <summary>
    /// Set the text for the elements that are shown within the creation modal.
    /// </summary>
    [JsonPropertyName("creationModal")]
    public DataTablesLanguageStateRestoreCreationModalOptions? CreationModal { get; set; }

    /// <summary>
    /// Set the error message shown when attempting to rename a state to one that already exists.
    /// </summary>
    [JsonPropertyName("duplicateError")]
    public string? DuplicateError { get; set; }

    /// <summary>
    /// Set the error message shown when incorrectly renaming a state to an empty string.
    /// </summary>
    [JsonPropertyName("emptyError")]
    public string? EmptyError { get; set; }

    /// <summary>
    /// Set the text that is shown in the savedStates collection when there are no saved states.
    /// </summary>
    [JsonPropertyName("emptyStates")]
    public string? EmptyStates { get; set; }

    /// <summary>
    /// Set the confirmation message shown within the StateRestore remove modal.
    /// </summary>
    [JsonPropertyName("removeConfirm")]
    public string? RemoveConfirm { get; set; }

    /// <summary>
    /// Set the error message shown when incorrectly removing a state.
    /// </summary>
    [JsonPropertyName("removeError")]
    public string? RemoveError { get; set; }

    /// <summary>
    /// Set the joiner used between states within the StateRestore remove modal.
    /// </summary>
    [JsonPropertyName("removeJoiner")]
    public string? RemoveJoiner { get; set; }

    /// <summary>
    /// Set the text within the StateRestore remove button.
    /// </summary>
    [JsonPropertyName("removeSubmit")]
    public string? RemoveSubmit { get; set; }

    /// <summary>
    /// Set the title shown in the StateRestore remove modal.
    /// </summary>
    [JsonPropertyName("removeTitle")]
    public string? RemoveTitle { get; set; }

    /// <summary>
    /// Set the text within the StateRestore remove button.
    /// </summary>
    [JsonPropertyName("renameButton")]
    public string? RenameButton { get; set; }

    /// <summary>
    /// Set the label next to the input element within the StateRestore rename modal.
    /// </summary>
    [JsonPropertyName("renameLabel")]
    public string? RenameLabel { get; set; }

    /// <summary>
    /// Set the title shown in the StateRestore rename modal.
    /// </summary>
    [JsonPropertyName("renameTitle")]
    public string? RenameTitle { get; set; }
}