using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Used to configure the menu events.
    /// </summary>
    public class MenuEvents<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        private IMenu Parent { get; set; }

        [CascadingParameter]
        private SfContextMenu<TValue> ContextMenu { get; set; }

        /// <summary>
        /// Triggers before closing the menu.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenCloseMenuEventArgs<TValue>> OnClose { get; set; }

        /// <summary>
        /// Triggers while rendering each menu item.
        /// </summary>
        [Parameter]
        public EventCallback<MenuEventArgs<TValue>> OnItemRender { get; set; }

        /// <summary>
        /// Triggers before opening the menu item.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenCloseMenuEventArgs<TValue>> OnOpen { get; set; }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers while closing the menu.
        /// </summary>
        [Parameter]
        public EventCallback<OpenCloseMenuEventArgs<TValue>> Closed { get; set; }

        /// <summary>
        /// Triggers while opening the menu item.
        /// </summary>
        [Parameter]
        public EventCallback<OpenCloseMenuEventArgs<TValue>> Opened { get; set; }

        /// <summary>
        /// Triggers while selecting menu item.
        /// </summary>
        [Parameter]
        public EventCallback<MenuEventArgs<TValue>> ItemSelected { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (ContextMenu == null)
            {
                Parent?.UpdateChildProperties(typeof(TValue) == typeof(MenuItemModel) ? "SelfRefMenuEvents" : "MenuEvents", this);
            }
            else
            {
                ContextMenu.Delegates = this;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ContextMenu = null;
        }
    }
}