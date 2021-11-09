using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The AutoComplete component provides the matched suggestion list when type into the input, from which the user can select one.
    /// </summary>
    public partial class SfAutoComplete<TValue, TItem> : SfComboBox<TValue, TItem>
    {
        /// <summary>
        /// When set to 'true', highlight the searched characters on suggested list items.
        /// </summary>
        [Parameter]
        public bool Highlight { get; set; }

        /// <summary>
        /// Allows you to set the minimum search character length, the search action will perform after typed minimum characters.
        /// </summary>
        [Parameter]
        public int MinLength { get; set; } = 1;

        /// <summary>
        /// Determines on which filter type, the component needs to be considered on search action.
        /// </summary>
        [Parameter]
        public override FilterType FilterType { get; set; } = FilterType.Contains;

        /// <summary>
        /// <para>Specifies whether to show or hide the clear button.</para>
        /// <para>When the clear button is clicked, `Value` properties are reset to null.</para>
        /// </summary>
        [Parameter]
        public override bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Allows you to either show or hide the popup button on the component.
        /// </summary>
        [Parameter]
        public bool ShowPopupButton { get; set; }

        /// <summary>
        /// Supports the specified number of list items on the suggestion popup.
        /// </summary>
        [Parameter]
        public int SuggestionCount { get; set; } = 20;

        /// <summary>
        /// Parent component of AutoComplete.
        /// </summary>
        /// <exclude/>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic AutoCompleteParent { get; set; }
    }
}