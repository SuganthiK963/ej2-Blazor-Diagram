using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns.Internal;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Defines the selection settings of List Box.
    /// </summary>
    public class ListBoxSelectionSettings : SfBaseComponent
    {
        [CascadingParameter]
        private IListBox Parent { get; set; }

        /// <summary>
        /// Set the position of the checkbox.
        /// </summary>
        [Parameter]
        public CheckBoxPosition CheckboxPosition { get; set; } = CheckBoxPosition.Left;

        /// <summary>
        /// Specifies the selection modes. The possible values are
        ///  `Single`: Allows you to select a single item in the ListBox.
        ///  `Multiple`: Allows you to select more than one item in the ListBox.
        /// </summary>
        [Parameter]
        public SelectionMode Mode { get; set; } = SelectionMode.Multiple;

        /// <summary>
        /// If 'showCheckbox' is set to true, then 'checkbox' will be visualized in the list item.
        /// </summary>
        [Parameter]
        public bool ShowCheckbox { get; set; }

        /// <summary>
        /// Allows you to either show or hide the selectAll option on the component.
        /// </summary>
        [Parameter]
        public bool ShowSelectAll { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("selectionSettings", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}