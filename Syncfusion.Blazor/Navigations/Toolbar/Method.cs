using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The Toolbar control contains a group of commands that are aligned horizontally.
    /// </summary>
    public partial class SfToolbar : SfBaseComponent
    {
        /// <summary>
        /// Adds new items to the Toolbar that accepts an list of Toolbar items.
        /// </summary>
        /// <param name="items">A list of items to be added to the Toolbar.</param>
        /// <param name="index">Number value that determines where the command is to be added.</param>
        public void AddItems(List<ToolbarItem> items, int index)
        {
            if (ToolbarItems == null)
            {
                ToolbarItems = new List<ItemModel>();
            }

            if (items != null)
            {
                int itemIndex = Items.Count;
                for (var i = 0; i < items.Count; i++)
                {
                    ToolbarItem dynamicItem = new ToolbarItem();
                    if (string.IsNullOrEmpty(items[i].Id))
                    {
                        dynamicItem = ToolbarItem.SetId(items[i]);
                    }

                    ItemModel item = GetItem(dynamicItem);
                    Items.Insert(index, dynamicItem);
                    index++;
                    item.Index = itemIndex;
                    ToolbarItems.Add(item);
                    itemIndex++;
                }

                IsItemChanged = true;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Removes the items from the Toolbar at the specified index.
        /// </summary>
        /// <param name="index">Index of item which is to be removed from the Toolbar.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task RemoveItems(int index)
        {
            if (Items != null && index < Items.Count)
            {
                Items[index].VisibleItem(false);
                await InvokeMethod("sfBlazor.Toolbar.hideItem", new object[] { Element, Items });
            }
        }

        /// <summary>
        /// Enables or disables the specified Toolbar item.
        /// </summary>
        /// <param name="items">A list of toolbar item index to be enabled or disabled.</param>
        /// <param name="isEnable">Boolean value that determines whether the command should be enabled or disabled. By default, `isEnable` is set to true.</param>
        public void EnableItems(List<int> items, bool? isEnable = null)
        {
            if (items != null && Items.Count > 0)
            {
                if (isEnable.HasValue)
                {
                    foreach (var item in items)
                    {
                        Items[item].EnableItem(!isEnable.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Shows or hides the Toolbar item that is in the specified index.
        /// </summary>
        /// <param name="index">Index value of target item to be hidden or shown.</param>
        /// <param name="isHide">Based on this Boolean value, item will be hide (true) or show (false). By default, isHide is false.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HideItem(int index, bool? isHide = null)
        {
            await HideItemAsync(index, isHide);
        }

        /// <summary>
        /// Shows or hides the Toolbar item that is in the specified index.
        /// </summary>
        /// <param name="index">Index value of target item to be hidden or shown.</param>
        /// <param name="isHide">Based on this Boolean value, item will be hide (true) or show (false). By default, isHide is false.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task HideItemAsync(int index, bool? isHide = null)
        {
            if (isHide.HasValue)
            {
                Items[index].VisibleItem(!isHide.Value);
            }

            await InvokeMethod("sfBlazor.Toolbar.hideItem", new object[] { Element, Items });
        }

        /// <summary>
        /// Specifies the value to disable/enable the Toolbar component.
        /// When set to `True`, the component will be disabled.
        /// </summary>
        /// <param name="disable">Based on this Boolean value, Toolbar will be enabled (false) or disabled (true).</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Disable(bool disable)
        {
            await DisableAsync(disable);
        }

        /// <summary>
        /// Specifies the value to disable/enable the Toolbar component.
        /// When set to `True`, the component will be disabled.
        /// </summary>
        /// <param name="disable">Based on this Boolean value, Toolbar will be enabled (false) or disabled (true).</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task DisableAsync(bool disable)
        {
            await InvokeMethod("sfBlazor.Toolbar.disable", new object[] { Element, disable });
        }

        /// <summary>
        /// Refresh the whole Toolbar component without re-rendering.
        /// - It is used to manually refresh the Toolbar overflow modes such as scrollable, popup, multi row, and extended.
        /// - It will refresh the Toolbar component after loading items dynamically.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RefreshOverflow()
        {
            await RefreshOverflowAsync();
        }

        /// <summary>
        /// Refresh the whole Toolbar component without re-rendering.
        /// - It is used to manually refresh the Toolbar overflow modes such as scrollable, popup, multi row, and extended.
        /// - It will refresh the Toolbar component after loading items dynamically.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task RefreshOverflowAsync()
        {
            await InvokeMethod("sfBlazor.Toolbar.refreshOverflow", new object[] { Element });
        }

        /// <summary>
        /// Applies all the pending property changes and render the component again.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Refresh()
        {
            await RefreshAsync();
        }

        /// <summary>
        /// Applies all the pending property changes and render the component again.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task RefreshAsync()
        {
            await InvokeMethod("sfBlazor.Toolbar.refresh", new object[] { Element, GetInstance() });
        }
    }
}
