using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Partial Class SfSidebar.
    /// </summary>
    public partial class SfSidebar : SfBaseComponent
    {
        /// <summary>
        /// Hide the Sidebar component, if it is in an open state.
        /// </summary>
        /// <returns>"Task".</returns>
        [JSInvokable]
        [Obsolete("This method is deprecated and will no longer be used. Use the IsOpen property to achieve this case.")]
        public async Task Hide()
        {
            if (!sidebarClass.Contains(CLOSE, StringComparison.Ordinal))
            {
                EventArgs eventArgs = SidebarEvent("OnClose");
                await SfBaseUtils.InvokeEvent<EventArgs>(OnClose, eventArgs);
                if (!eventArgs.Cancel)
                {
                    openState = false;
                    IsOpen = SidebarIsOpen = await SfBaseUtils.UpdateProperty<bool>(false, SidebarIsOpen, IsOpenChanged);
                    UpdateClass();
                    await InvokeMethod("sfBlazor.Sidebar.hide", new object[] { element, GetProperties() });
                }
                else
                {
                  await InvokeMethod("sfBlazor.Sidebar.show", new object[] { element, GetProperties(), openState });
                }
            }
        }

        /// <summary>
        /// Hide the Sidebar component, if it is in an open state.
        /// </summary>
        /// <returns>"Task".</returns>
        [JSInvokable]
        internal async Task SidebarHide()
        {
            if (!sidebarClass.Contains(CLOSE, StringComparison.Ordinal))
            {
                EventArgs eventArgs = SidebarEvent("OnClose");
                await SfBaseUtils.InvokeEvent<EventArgs>(OnClose, eventArgs);
                if (!eventArgs.Cancel)
                {
                    openState = false;
                    IsOpen = SidebarIsOpen = await SfBaseUtils.UpdateProperty<bool>(false, SidebarIsOpen, IsOpenChanged);
                    UpdateClass();
                    await InvokeMethod("sfBlazor.Sidebar.hide", new object[] { element, GetProperties() });
                }
            }
        }

        /// <summary>
        /// Shows the Sidebar component, if it is in closed state.
        /// </summary>
        /// <returns>"Task".</returns>
        [JSInvokable]
        [Obsolete("This method is deprecated and will no longer be used. Use the IsOpen property to achieve this case.")]
        public async Task Show()
        {
            if (!sidebarClass.Contains(OPEN, StringComparison.Ordinal))
            {
                EventArgs eventArgs = SidebarEvent("OnOpen");
                await SfBaseUtils.InvokeEvent<EventArgs>(OnOpen, eventArgs);
                if (!eventArgs.Cancel)
                {
                    openState = true;
                    IsOpen = SidebarIsOpen = await SfBaseUtils.UpdateProperty<bool>(true, SidebarIsOpen, IsOpenChanged);
                    UpdateClass();
                    await InvokeMethod("sfBlazor.Sidebar.show", new object[] { element, GetProperties(), openState });
                }
            }
        }

        /// <summary>
        /// Shows the Sidebar component, if it is in closed state.
        /// </summary>
        /// <returns>"Task".</returns>
        [JSInvokable]
        internal async Task SidebarShow()
        {
            if (!sidebarClass.Contains(OPEN, StringComparison.Ordinal))
            {
                EventArgs eventArgs = SidebarEvent("OnOpen");
                await SfBaseUtils.InvokeEvent<EventArgs>(OnOpen, eventArgs);
                if (!eventArgs.Cancel)
                {
                    openState = true;
                    IsOpen = SidebarIsOpen = await SfBaseUtils.UpdateProperty<bool>(true, SidebarIsOpen, IsOpenChanged);
                    UpdateClass();
                    await InvokeMethod("sfBlazor.Sidebar.show", new object[] { element, GetProperties(), openState });
                }
            }
        }

        /// <summary>
        /// Shows or hides the Sidebar based on the current state.
        /// </summary>
        /// <returns>"Task".</returns>
        [Obsolete("This method is deprecated and will no longer be used. Use the IsOpen property to achieve this case.")]
        public async Task Toggle()
        {
            if (sidebarClass.Contains(OPEN, StringComparison.Ordinal))
            {
                await Hide();
            }
            else
            {
                await SidebarShow();
            }
        }
    }
}
