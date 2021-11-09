using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A list of items that is used to configure Toolbar commands.
    /// </summary>
    public partial class ToolbarItems : SfBaseComponent
    {
        [CascadingParameter]
        internal SfToolbar Parent { get; set; }

        /// <summary>
        /// Child Content for Toolbar items.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// A list of items that is used to configure Toolbar commands.
        /// </summary>
        public List<ToolbarItem> Items { get; set; } = new List<ToolbarItem>();

        internal int UpdateChildProperty(ToolbarItem item)
        {
            if (item != null)
            {
                Items.Add(item);
                Parent.UpdateChildProperties(Items);
            }

            return Items.Count - 1;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties(Items);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}