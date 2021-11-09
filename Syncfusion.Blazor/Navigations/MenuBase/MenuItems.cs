using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A list of items that is used to configure menu items.
    /// </summary>
    public partial class MenuItems : SfBaseComponent
    {
        [CascadingParameter]
        private IMenu Menu { get; set; }

        [CascadingParameter]
        private MenuItem ParentItem { get; set; }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        private List<MenuItem> items;

        internal void UpdateChildProperty(MenuItem item)
        {
            items.Add(item);
            if (ParentItem == null)
            {
                Menu.UpdateChildProperties("Items", items);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            items = new List<MenuItem>();
            if (ParentItem != null)
            {
                ParentItem.UpdateChildProperty(items);
            }
            else if (Menu != null)
            {
                Menu.UpdateChildProperties("Items", items);
            }
        }

        internal void RemoveChildProperty(MenuItem item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
        }

        internal override void ComponentDispose()
        {
            Menu = null;
            ParentItem = null;
            ChildContent = null;
        }
    }
}