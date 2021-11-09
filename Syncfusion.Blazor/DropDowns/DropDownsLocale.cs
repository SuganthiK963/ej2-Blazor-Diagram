using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// Defines the <see cref="LocaleData" />.
    /// </summary>
    public partial class LocaleData
    {
        /// <summary>
        /// Gets or sets the Dropdowns property.
        /// </summary>
        [JsonPropertyName("dropdowns")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DropdownsLocale Dropdowns { get; set; } = new DropdownsLocale();

        /// <summary>
        /// Gets or sets the DropDownList property.
        /// </summary>
        [JsonPropertyName("drop-down-list")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DropDownListLocale DropDownList { get; set; } = new DropDownListLocale();

        /// <summary>
        /// Gets or sets the ComboBox property.
        /// </summary>
        [JsonPropertyName("combo-box")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ComboBoxLocale ComboBox { get; set; } = new ComboBoxLocale();

        /// <summary>
        /// Gets or sets the AutoComplete property.
        /// </summary>
        [JsonPropertyName("auto-complete")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public AutoCompleteLocale AutoComplete { get; set; } = new AutoCompleteLocale();

        /// <summary>
        /// Gets or sets the MultiSelect property.
        /// </summary>
        [JsonPropertyName("multi-select")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public MultiSelectLocale MultiSelect { get; set; } = new MultiSelectLocale();

        /// <summary>
        /// Gets or sets the ListBox property.
        /// </summary>
        [JsonPropertyName("listbox")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListBoxLocale ListBox { get; set; } = new ListBoxLocale();
    }

    /// <summary>
    /// Defines the <see cref="DropdownsLocale" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DropdownsLocale
    {
        /// <summary>
        /// Gets or sets the NoRecordsTemplate property.
        /// </summary>
        [JsonPropertyName("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Gets or sets the ActionFailureTemplate property.
        /// </summary>
        [JsonPropertyName("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";

        /// <summary>
        /// Gets or sets the OverflowCountTemplate property.
        /// </summary>
        [JsonPropertyName("overflowCountTemplate")]
        public string OverflowCountTemplate { get; set; } = "+${count} more..";

        /// <summary>
        /// Gets or sets the SelectAllText property.
        /// </summary>
        [JsonPropertyName("selectAllText")]
        public string SelectAllText { get; set; } = "Select All";

        /// <summary>
        /// Gets or sets the UnSelectAllText property.
        /// </summary>
        [JsonPropertyName("unSelectAllText")]
        public string UnSelectAllText { get; set; } = "Unselect All";

        /// <summary>
        /// Gets or sets the TotalCountTemplate property.
        /// </summary>
        [JsonPropertyName("totalCountTemplate")]
        public string TotalCountTemplate { get; set; } = "${count} selected";
    }

    /// <summary>
    /// Defines the <see cref="DropDownListLocale" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DropDownListLocale
    {
        /// <summary>
        /// Gets or sets the NoRecordsTemplate property.
        /// </summary>
        [JsonPropertyName("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Gets or sets the ActionFailureTemplate property.
        /// </summary>
        [JsonPropertyName("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";
    }

    /// <summary>
    /// Defines the <see cref="ComboBoxLocale" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ComboBoxLocale
    {
        /// <summary>
        /// Gets or sets the NoRecordsTemplate property.
        /// </summary>
        [JsonPropertyName("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Gets or sets the ActionFailureTemplate property.
        /// </summary>
        [JsonPropertyName("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";
    }

    /// <summary>
    /// Defines the <see cref="AutoCompleteLocale" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class AutoCompleteLocale
    {
        /// <summary>
        /// Gets or sets the NoRecordsTemplate property.
        /// </summary>
        [JsonPropertyName("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Gets or sets the ActionFailureTemplate property.
        /// </summary>
        [JsonPropertyName("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";
    }

    /// <summary>
    /// Defines the <see cref="MultiSelectLocale" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MultiSelectLocale
    {
        /// <summary>
        /// Gets or sets the NoRecordsTemplate property.
        /// </summary>
        [JsonPropertyName("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Gets or sets the ActionFailureTemplate property.
        /// </summary>
        [JsonPropertyName("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";

        /// <summary>
        /// Gets or sets the OverflowCountTemplate property.
        /// </summary>
        [JsonPropertyName("overflowCountTemplate")]
        public string OverflowCountTemplate { get; set; } = "+${count} more..";

        /// <summary>
        /// Gets or sets the SelectAllText property.
        /// </summary>
        [JsonPropertyName("selectAllText")]
        public string SelectAllText { get; set; } = "Select All";

        /// <summary>
        /// Gets or sets the UnSelectAllText property.
        /// </summary>
        [JsonPropertyName("unSelectAllText")]
        public string UnSelectAllText { get; set; } = "Unselect All";

        /// <summary>
        /// Gets or sets the TotalCountTemplate property.
        /// </summary>
        [JsonPropertyName("totalCountTemplate")]
        public string TotalCountTemplate { get; set; } = "${count} selected";
    }

    /// <summary>
    /// Defines the <see cref="ListBoxLocale" />.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class ListBoxLocale
    {
        /// <summary>
        /// Gets or sets the NoRecordsTemplate property.
        /// </summary>
        [JsonPropertyName("noRecordsTemplate")]
        public string NoRecordsTemplate { get; set; } = "No records found";

        /// <summary>
        /// Gets or sets the ActionFailureTemplate property.
        /// </summary>
        [JsonPropertyName("actionFailureTemplate")]
        public string ActionFailureTemplate { get; set; } = "Request failed";

        /// <summary>
        /// Gets or sets the SelectAllText property.
        /// </summary>
        [JsonPropertyName("selectAllText")]
        public string SelectAllText { get; set; } = "Select All";

        /// <summary>
        /// Gets or sets the UnSelectAllText property.
        /// </summary>
        [JsonPropertyName("unSelectAllText")]
        public string UnSelectAllText { get; set; } = "Unselect All";

        /// <summary>
        /// Gets or sets the MoveUp property.
        /// </summary>
        [JsonPropertyName("moveUp")]
        public string MoveUp { get; set; } = "Move Up";

        /// <summary>
        /// Gets or sets the MoveDown property.
        /// </summary>
        [JsonPropertyName("moveDown")]
        public string MoveDown { get; set; } = "Move Down";

        /// <summary>
        /// Gets or sets the MoveTo property.
        /// </summary>
        [JsonPropertyName("moveTo")]
        public string MoveTo { get; set; } = "Move To";

        /// <summary>
        /// Gets or sets the MoveFrom property.
        /// </summary>
        [JsonPropertyName("moveFrom")]
        public string MoveFrom { get; set; } = "Move From";

        /// <summary>
        /// Gets or sets the MoveAllTo property.
        /// </summary>
        [JsonPropertyName("moveAllTo")]
        public string MoveAllTo { get; set; } = "Move All To";

        /// <summary>
        /// Gets or sets the MoveAllFrom property.
        /// </summary>
        [JsonPropertyName("moveAllFrom")]
        public string MoveAllFrom { get; set; } = "Move All From";
    }
}
