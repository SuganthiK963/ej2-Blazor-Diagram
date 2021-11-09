using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Used to configure the items which is going to render as menu.
    /// </summary>
    public partial class MenuItem : SfBaseComponent
    {
        [CascadingParameter]
        private MenuItems Parent { get; set; }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space for the menu Item that is used to include an icon.
        /// Menu Item can include font icon and sprite image.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the id for menu item.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the sub menu items that is the array of MenuItem.
        /// </summary>
        [Parameter]
        public List<MenuItem> Items { get; set; }

        /// <summary>
        /// Specifies separator between the menu items. Separator are either horizontal or vertical lines used to group menu items.
        /// </summary>
        [Parameter]
        public bool Separator { get; set; }

        /// <summary>
        /// Used to enable or disable the menu item.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Used to hide or show the menu item.
        /// </summary>
        [Parameter]
        public bool Hidden { get; set; }

        /// <summary>
        /// Specifies text for menu item.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Specifies URL for menu item that creates the anchor link to navigate to the url provided.
        /// </summary>
        [Parameter]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// You can add the additional HTML attributes such as style, title etc., to the menu item.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
        }

        internal void UpdateChildProperty(List<MenuItem> items)
        {
            Items = items;
        }

        internal override void ComponentDispose()
        {
            Parent?.RemoveChildProperty(this);
            Parent = null;
            ChildContent = null;
        }
    }
}