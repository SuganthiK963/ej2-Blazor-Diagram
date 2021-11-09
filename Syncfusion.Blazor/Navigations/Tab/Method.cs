using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Tab is a content panel to show multiple contents in a compact space. Also, only one tab is active at a time. Each Tab item has an associated content, that will be displayed based on the active Tab.
    /// </summary>
    public partial class SfTab : SfBaseComponent
    {
        /// <summary>
        /// Adds new items to the Tab that accepts a list of Tab items.
        /// </summary>
        /// <param name="items">A list of items that are added to the Tab.</param>
        /// <param name="index">Specifies an index value that determines where the items to be added.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task AddTab(List<TabItem> items, int index)
        {
            if (Items != null && (index <= Items.Count || index <= 0))
            {
                List<TabItemModel> addedTabItems = new List<TabItemModel>();
                if (items != null)
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        TabItemModel item = GetTabItemModel(items[i]);
                        addedTabItems.Add(item);
                    }
                }

                AddEventArgs addingEventArgs = new AddEventArgs()
                {
                    Name = ADDING,
                    Cancel = false,
                    AddedItems = addedTabItems
                };
                await SfBaseUtils.InvokeEvent<AddEventArgs>(Delegates?.Adding, addingEventArgs);
                if (items != null && !addingEventArgs.Cancel)
                {
                    await AddItems(items, index);
                    AddEventArgs addEventArgs = new AddEventArgs()
                    {
                        Name = ADDED,
                        AddedItems = addedTabItems
                    };
                    await SfBaseUtils.InvokeEvent<AddEventArgs>(Delegates?.Added, addEventArgs);
                }
            }
        }

        /// <summary>
        /// Removes a particular Tab based on index from the Tabs.
        /// </summary>
        /// <param name="index">Index of tab item that is going to be removed.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        [JSInvokable]
        public async Task RemoveTab(int index)
        {
            if (Items != null && index < Items.Count)
            {
                RemoveEventArgs removingEventArgs = new RemoveEventArgs()
                {
                    Name = REMOVING,
                    Cancel = false,
                    RemovedIndex = index
                };
                await SfBaseUtils.InvokeEvent<RemoveEventArgs>(Delegates?.Removing, removingEventArgs);
                if (!removingEventArgs.Cancel)
                {
                    await RemoveItem(index);
                }
            }
        }

        /// <summary>
        /// Enables or disables a particular tab item. On passing the value as `false`, the tab will be disabled.
        /// </summary>
        /// <param name="index">Index value of target Tab item.</param>
        /// <param name="isEnable">Specify a Boolean value that determines whether the command should be enabled or disabled. By default, isEnable has true.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task EnableTab(int index, bool isEnable)
        {
            await EnableTabAsync(index, isEnable);
        }

        /// <summary>
        /// Enables or disables a particular tab item. On passing the value as `false`, the tab will be disabled.
        /// </summary>
        /// <param name="index">Index value of target Tab item.</param>
        /// <param name="isEnable">Specify a Boolean value that determines whether the command should be enabled or disabled. By default, isEnable has true.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task EnableTabAsync(int index, bool isEnable)
        {
            if (Items[index] != null)
            {
                Items[index].EnableItem(!isEnable);
            }

            await InvokeMethod("sfBlazor.Tab.enableTab", new object[] { Element, index, isEnable });
        }

        /// <summary>
        /// Shows or hides a particular Tab based on the specified index.
        /// </summary>
        /// <param name="index">Index value of target item.</param>
        /// <param name="isHide">Based on this Boolean value, item will be hide (false) or show (true).</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HideTab(int index, bool? isHide = null)
        {
            await HideTabAsync(index, isHide);
        }

        /// <summary>
        /// Shows or hides a particular Tab based on the specified index.
        /// </summary>
        /// <param name="index">Index value of target item.</param>
        /// <param name="isHide">Based on this Boolean value, item will be hide (false) or show (true).</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task HideTabAsync(int index, bool? isHide = null)
        {
            await InvokeMethod("sfBlazor.Tab.hideTab", new object[] { Element, index, isHide });
            if (OverflowMode == OverflowMode.Popup)
            {
                await Toolbar.RefreshOverflow();
            }
        }

        /// <summary>
        /// Select (activate) a particular tab based on the specified index.
        /// </summary>
        /// <param name="index">Index is used for selecting an item from the Tab.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Select(int index)
        {
            await SelectAsync(index);
        }

        /// <summary>
        /// Select (activate) a particular tab based on the specified index.
        /// </summary>
        /// <param name="index">Index is used for selecting an item from the Tab.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task SelectAsync(int index)
        {
            await InvokeMethod("sfBlazor.Tab.select", new object[] { Element, index });
        }

        /// <summary>
        /// Specifies the value to disable or enable the Tabs component. When set to `true`, the component will be disabled.
        /// </summary>
        /// <param name="disable">Based on this Boolean value, Tab will be enabled (false) or disabled (true).</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Disable(bool disable)
        {
            await DisableAsync(disable);
        }

        /// <summary>
        /// Specifies the value to disable or enable the Tabs component. When set to `true`, the component will be disabled.
        /// </summary>
        /// <param name="disable">Based on this Boolean value, Tab will be enabled (false) or disabled (true).</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task DisableAsync(bool disable)
        {
            await InvokeMethod("sfBlazor.Tab.disable", new object[] { Element, disable });
        }

        /// <summary>
        /// Return a tab item element based on the specified index.
        /// </summary>
        /// <param name="index">Index is used for accessing tab header item element from the Tab.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task<DOM> GetTabItem(int index)
        {
            var dom = await InvokeMethod<DOM>("sfBlazor.Tab.getTabItem", true, new object[] { Toolbar.Element, index });
            dom.JsRuntime = JSRuntime;
            return dom;
        }

        /// <summary>
        /// Returns the tab content element based on the specified index.
        /// </summary>
        /// <param name="index">Index is used for accessing tab content element from the Tab.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task<DOM> GetTabContent(int index)
        {
            var dom = await InvokeMethod<DOM>("sfBlazor.Tab.getTabContent", true, new object[] { Element, index });
            dom.JsRuntime = JSRuntime;
            return dom;
        }

        /// <summary>
        /// Refresh the entire tabs component.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Refresh()
        {
            await RefreshAsync();
        }

        /// <summary>
        /// Refresh the entire tabs component.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task RefreshAsync()
        {
            if (Toolbar != null)
            {
                await Toolbar.RefreshOverflow();
            }

            await InvokeMethod("sfBlazor.Tab.refresh", new object[] { Element });
        }
    }
}
