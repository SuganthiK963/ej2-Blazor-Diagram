using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns.Internal;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Defines the ListBox tool bar settings.
    /// </summary>
    public class ListBoxToolbarSettings : SfBaseComponent
    {
        [CascadingParameter]
        private IListBox Parent { get; set; }

        /// <summary>
        /// Specifies the list of tools for dual ListBox.
        /// The predefined tools are 'MoveUp', 'MoveDown', 'MoveTo', 'MoveFrom', 'MoveAllTo', and 'MoveAllFrom'.
        /// </summary>
        [Parameter]
        public string[] Items { get; set; }

        /// <summary>
        /// Positions the toolbar before/after the ListBox.
        /// The possible values are:
        /// - Left: The toolbar will be positioned to the left of the ListBox.
        /// - Right: The toolbar will be positioned to the right of the ListBox.
        /// </summary>
        [Parameter]
        public ToolBarPosition Position { get; set; } = ToolBarPosition.Right;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("toolbarSettings", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}