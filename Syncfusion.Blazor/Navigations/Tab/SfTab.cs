using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Navigations.Internal;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Tab is a content panel to show multiple contents in a compact space. Also, only one tab is active at a time. Each Tab item has an associated content, that will be displayed based on the active Tab.
    /// </summary>
    public partial class SfTab : SfBaseComponent
    {
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string TAB_HEADER = "e-tab-header";
        private const string VERTICAL_CLASS = "e-vertical";
        private const string VERTICAL_TAB = "e-vertical-tab";
        private const string VERTICAL_LEFT = "e-vertical-left";
        private const string VERTICAL_RIGHT = "e-vertical-right";
        private const string HORIZONTAL_BOTTOM = "e-horizontal-bottom";
        private const string DISABLE = "e-disable";
        private const string OVERLAY = "e-overlay";
        private const string AUTO = "auto";
        private const string HUNDRED_PERCENT = "100%";
        private const string CLOSE_SHOW = "e-close-show";
        private const string VERTICAL = "vertical";
        private const string HORIZONTAL = "horizontal";
        private const string ADDING = "Adding";
        private const string ADDED = "Added";
        private const string REMOVING = "Removing";
        private const string REMOVED = "Removed";
        private const string ANIMATION = "Animation";
        private const string CSSCLASS = "CssClass";
        private const string ENABLE_RTL = "EnableRtl";
        private const string HEADER_PLACEMENT = "HeaderPlacement";
        private const string HEIGHT = "Height";
        private const string ITEMS = "Items";
        private const string OVERFLOWMODE = "OverflowMode";
        private const string SCROLLSTEP = "ScrollStep";
        private const string SHOWCLOSEBUTTON = "ShowCloseButton";
        private const string WIDTH = "Width";
        private const string SELECTED_ITEM = "SelectedItem";
        private const string CHILD_ANIMATION = "animation";
        private const string CSSCLASS_NAME = "cssClass";
        private const string ENABLEPERSISTENCE = "enablePersistence";
        private const string RTL_ENABLE = "enableRtl";
        private const string HEADERPLACEMENT = "headerPlacement";
        private const string WIDTHNAME = "width";
        private const string LOAD_ON = "loadOn";
        private const string HEIGHTNAME = "height";
        private const string LOCALENAME = "locale";
        private const string OVERFLOW = "overflowMode";
        private const string SCROLL = "scrollStep";
        private const string SELECTEDITEM = "selectedItem";
        private const string SHOWCLOSE = "showCloseButton";
        private const string TABPREFIX = "tab-";
        private const string TABITEMPREFIX = "tabitem_";
        private const string CLASS = "class";
        private const string ITEM = "e-item";
        private const string ACTIVE = "e-active";
        private const string ITEM_SUFFIX = "e-item-";
        private const string CONTENT_SUFFIX = "e-content-";
        private const string UNDERSCO = "_";
        private const string VERTICAL_ICON = "e-vertical-icon";
        private const string TOP = "top";
        private const string BOTTOM = "bottom";
        private const string ALLOWDRAGANDDROP = "allowDragAndDrop";
        private const string DRAGAREA = "dragArea";
        private List<ToolbarItem> toolbarItems = new List<ToolbarItem>();
        private List<TabItem> visibleItems = new List<TabItem>();
        private int previousIndex;
        private Dictionary<string, object> containerAttributes = new Dictionary<string, object>();

        #region Private properties
        private string TabClass { get; set; } = "e-control e-tab e-lib";

        private string ToolbarHeight { get; set; } = "auto";

        private string ToolbarWidth { get; set; } = "100%";

        private string ToolbarCssClass { get; set; }

        private bool IsVerticalIcon { get; set; }

        internal bool IsCreatedEvent { get; set; }

        private bool IsTabScriptLoaded { get; set; }
        #endregion

        #region Internal variables
        internal bool IsLoaded { get; set; }

        internal bool IsTabItemChanged { get; set; }
        internal bool IsSelectedItemChanged { get; set; }

        internal TabEvents Delegates { get; set; }

        internal string Orientation { get; set; } = HORIZONTAL;
        #endregion

        internal static void SetUniqueID(TabItem item)
        {
            if (item != null && item.UniqueID == null)
            {
                item.UniqueID = TABITEMPREFIX + Guid.NewGuid().ToString();
            }
        }

        private static TabItemModel GetTabItemModel(TabItem tabItem)
        {
            TabItemModel item = new TabItemModel();
            item.Content = tabItem.Content;
            item.CssClass = tabItem.CssClass;
            item.Disabled = tabItem.Disabled;
            if (tabItem.Header != null)
            {
                if (item.Header == null)
                {
                    item.Header = new HeaderModel();
                }

                item.Header.IconCss = tabItem.Header.IconCss;
                item.Header.IconPosition = tabItem.Header.IconPosition;
                item.Header.Text = tabItem.Header.Text;
            }

            item.HeaderTemplate = tabItem.HeaderTemplate;
            item.Visible = tabItem.Visible;
            return item;
        }

        #region Private Methods
        private async Task SetToolbarItems()
        {
            IsVerticalIcon = false;
            visibleItems.Clear();
            toolbarItems = new List<ToolbarItem>();
            if (Items == null)
            {
                return;
            }

            if (OverflowMode == OverflowMode.MultiRow)
            {
                IsLoaded = true;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                TabItem item = Items[i];
                if (item.Visible)
                {
                    if (LoadOn == ContentLoad.Init)
                    {
                        item.IsContentRendered = true;
                    }

                    visibleItems.Add(item);
                    SetUniqueID(item);
                }
            }

            if (SelectedItem <= 0)
            {
                if (Items.Count == 0)
                {
                    SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(-1, selectedItem, SelectedItemChanged);
                }
                else
                {
                    SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(0, selectedItem, SelectedItemChanged);
                }
            }

            if (visibleItems.Count > 0 && visibleItems.Count - 1 < SelectedItem)
            {
                SelectedItem = selectedItem = visibleItems.Count - 1;
                SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(SelectedItem, selectedItem, SelectedItemChanged);
            }

            for (int i = 0; i < visibleItems.Count; i++)
            {
                TabItem item = visibleItems[i];
                List<string> classList = new List<string>();
                if (item.Header != null && (item.Header.IconPosition == TOP || item.Header.IconPosition == BOTTOM))
                {
                    IsVerticalIcon = true;
                }

                if (!string.IsNullOrEmpty(item.CssClass))
                {
                    classList.Add(item.CssClass);
                }

                if (item.Disabled)
                {
                    classList.Add(DISABLE + SPACE + OVERLAY);
                }

                if (item.Header != null)
                {
                    classList.Add("e-i" + item.Header.IconPosition);
                }
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                toolbarItems.Add(new ToolbarItem()
                {
                    Id = "e-item-" + ID + "_" + i,
                    CssClass = string.Join(SPACE, classList.ToArray()),
                    Template = getTabHeader(item)
                });
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            }

            if (IsVerticalIcon && !IsLoaded)
            {
                TabClass += SPACE + VERTICAL_ICON;
            }

            if (LoadOn == ContentLoad.Demand && visibleItems.Count > 0)
            {
                if (visibleItems.Count > SelectedItem && visibleItems[SelectedItem] != null && !visibleItems[SelectedItem].IsContentRendered)
                {
                    visibleItems[SelectedItem].IsContentRendered = true;
                }
            }
        }

        private void UpdateHtmlAttributes()
        {
            if (HtmlAttributes != null)
            {
                foreach (var item in HtmlAttributes)
                {
                    if (item.Key == CLASS)
                    {
                        TabClass += SPACE + item.Value;
                    }
                    else
                    {
                        SfBaseUtils.UpdateDictionary(item.Key, item.Value, containerAttributes);
                    }
                }
            }
        }

        private void UpdateLocalProperties()
        {
            Orientation = HORIZONTAL;
            ToolbarCssClass = TAB_HEADER;
            if (HeaderPlacement == HeaderPosition.Left || HeaderPlacement == HeaderPosition.Right)
            {
                Orientation = VERTICAL;
                TabClass += SPACE + VERTICAL_TAB;
                if (HeaderPlacement == HeaderPosition.Left)
                {
                    TabClass += SPACE + VERTICAL_LEFT;
                }
                else if (HeaderPlacement == HeaderPosition.Right)
                {
                    TabClass += SPACE + VERTICAL_RIGHT;
                }

                ToolbarWidth = AUTO;
                ToolbarHeight = HUNDRED_PERCENT;
            }

            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                TabClass += SPACE + RTL;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                TabClass += SPACE + CssClass;
            }

            if (ShowCloseButton)
            {
                ToolbarCssClass += SPACE + CLOSE_SHOW;
            }

            if (HeaderPlacement == HeaderPosition.Left)
            {
                ToolbarCssClass += SPACE + VERTICAL_CLASS + SPACE + VERTICAL_LEFT;
            }
            else if (HeaderPlacement == HeaderPosition.Right)
            {
                ToolbarCssClass += SPACE + VERTICAL_CLASS + SPACE + VERTICAL_RIGHT;
            }
            else if (HeaderPlacement == HeaderPosition.Bottom)
            {
                ToolbarCssClass += SPACE + HORIZONTAL_BOTTOM;
            }

            UpdateHtmlAttributes();
        }

#pragma warning disable 4014 // call is not awaited
#pragma warning disable CA1801 // Review unused parameters
        private void ToolbarInitialLoad(ToolbarEventArgs args)
        {
            Task.Yield().GetAwaiter().OnCompleted(async () =>
            {
                if (!IsLoaded)
                {
                    IsLoaded = true;
                    await InvokeAsync(StateHasChanged);
                }

                if (IsTabScriptLoaded)
                {
                    InvokeMethod("sfBlazor.Tab.headerReady", Element, IsCreatedEvent);
                }
            });
        }

        private void ToolbarClickedHandler(ToolbarEventArgs args)
        {
            if (args.TargetParentDataIndex == null)
            {
                return;
            }

            if (args.IsCloseIcon)
            {
                RemoveTab(args.TargetParentDataIndex.Value);
            }
            else
            {
                bool isPopup = false;
                if (args.TargetParentDataIndex.HasValue && args.TargetParentDataIndex != SelectedItem)
                {
                    ServerSelect(args.TargetParentDataIndex.Value, isPopup, args.ToolbarItemIndex.Value, args.IsPopupElement);
                }
            }
        }

        private void ItemChangeHandler(ToolbarEventArgs args)
        {
            Task.Yield().GetAwaiter().OnCompleted(() =>
            {
                IsTabItemChanged = false;
                InvokeMethod("sfBlazor.Tab.serverItemsChanged", new object[] { Element, SelectedItem, Animation, IsVerticalIcon });
            });
        }

        private void OverflowModeChangeHandler(ToolbarEventArgs args)
        {
            InvokeMethod("sfBlazor.Tab.overflowMode", Element, OverflowMode);
        }
#pragma warning restore 4014 // call is not awaited
#pragma warning restore CA1801 // Review unused parameters

        private void DraggedPopupItem(int droppingIndex, int draggingIndex)
        {
            if (Items != null)
            {
                TabItem item = Items[draggingIndex];
                Items.RemoveAt(draggingIndex);
                Items.Insert(droppingIndex, item);
            }
        }

        private void SelectContent()
        {
            if (LoadOn == ContentLoad.Demand)
            {
                if (visibleItems.Count > SelectedItem && visibleItems[SelectedItem] != null && !visibleItems[SelectedItem].IsContentRendered)
                {
                    visibleItems[SelectedItem].IsContentRendered = true;
                }
            }
        }

        #endregion

        #region Internal methods

        internal async Task OnPropertyChangeHandler()
        {
            if (PropertyChanges.ContainsKey(CSSCLASS))
            {
                await InvokeMethod("sfBlazor.Tab.setCssClass", Element, CssClass);
            }

            if (PropertyChanges.ContainsKey(SHOWCLOSEBUTTON))
            {
                if (ShowCloseButton && !ToolbarCssClass.Contains(CLOSE_SHOW, StringComparison.CurrentCulture))
                {
                    ToolbarCssClass += SPACE + CLOSE_SHOW;
                }
                else if (ToolbarCssClass.Contains(CLOSE_SHOW, StringComparison.CurrentCulture))
                {
                    ToolbarCssClass = SfBaseUtils.RemoveClass(ToolbarCssClass, SPACE + CLOSE_SHOW);
                }

                await Toolbar.RefreshOverflow();
                await InvokeMethod("sfBlazor.Tab.showCloseButton", Element, ShowCloseButton);
            }

            if (PropertyChanges.ContainsKey(HEADER_PLACEMENT))
            {
                ToolbarCssClass = TAB_HEADER;
                if (ShowCloseButton)
                {
                    ToolbarCssClass += SPACE + CLOSE_SHOW;
                }

                bool previousOrientation = Toolbar.IsVertical;
                if (HeaderPlacement == HeaderPosition.Left || HeaderPlacement == HeaderPosition.Right)
                {
                    Orientation = VERTICAL;
                    ToolbarCssClass += SPACE + VERTICAL_CLASS + SPACE + (HeaderPlacement == HeaderPosition.Right ? VERTICAL_RIGHT : VERTICAL_LEFT);
                    ToolbarHeight = HUNDRED_PERCENT;
                    ToolbarWidth = AUTO;
                    Toolbar.IsVertical = true;
                }
                else if (HeaderPlacement == HeaderPosition.Bottom || HeaderPlacement == HeaderPosition.Top)
                {
                    Orientation = HORIZONTAL;
                    if (HeaderPlacement == HeaderPosition.Bottom)
                    {
                        ToolbarCssClass += SPACE + HORIZONTAL_BOTTOM;
                    }

                    ToolbarHeight = AUTO;
                    ToolbarWidth = HUNDRED_PERCENT;
                    Toolbar.IsVertical = false;
                }

                Toolbar.PreventPropChange = Toolbar.IsVertical != previousOrientation;
                await InvokeMethod("sfBlazor.Tab.headerPlacement", new object[] { Element, HeaderPlacement, SelectedItem, Toolbar.Element, ToolbarCssClass, Toolbar.IsVertical, Toolbar.PreventPropChange });
            }

            if (PropertyChanges.ContainsKey("EnableRtl"))
            {
                Toolbar.SetRtl(EnableRtl);
                await InvokeMethod("sfBlazor.Tab.enableRtl", Element, EnableRtl);
            }

            if (PropertyChanges.ContainsKey(ALLOWDRAGANDDROP))
            {
                await InvokeMethod("sfBlazor.Tab.allowDragAndDrop", Element, AllowDragAndDrop);
            }
        }

        internal Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> tabObj = new Dictionary<string, object>();
            tabObj.Add(CHILD_ANIMATION, Animation);
            tabObj.Add(CSSCLASS_NAME, CssClass);
            tabObj.Add(ENABLEPERSISTENCE, EnablePersistence);
            tabObj.Add(RTL_ENABLE, EnableRtl || SyncfusionService.options.EnableRtl);
            tabObj.Add(HEADERPLACEMENT, HeaderPlacement);
            tabObj.Add(HEIGHTNAME, Height);
            tabObj.Add(LOAD_ON, LoadOn);
            tabObj.Add(LOCALENAME, Locale);
            tabObj.Add(OVERFLOW, OverflowMode);
            tabObj.Add(SCROLL, ScrollStep);
            tabObj.Add(SELECTEDITEM, SelectedItem);
            tabObj.Add(SHOWCLOSE, ShowCloseButton);
            tabObj.Add(WIDTHNAME, Width);
            tabObj.Add(ALLOWDRAGANDDROP, AllowDragAndDrop);
            tabObj.Add(DRAGAREA, DragArea);
            return tabObj;
        }

        internal async Task ServerSelect(int targetIndex, bool isPopup, int tabItemIndex, bool isPopupElement)
        {
            previousIndex = SelectedItem;
            SelectingEventArgs eventArgs = new SelectingEventArgs()
            {
                PreviousIndex = previousIndex,
                SelectedIndex = SelectedItem,
                SelectingIndex = targetIndex,
                IsSwiped = false,
                Cancel = false
            };
            await SfBaseUtils.InvokeEvent<SelectingEventArgs>(Delegates?.Selecting, eventArgs);
            if (!eventArgs.Cancel)
            {
                await OnClientChanged(targetIndex);
                await InvokeMethod("sfBlazor.Tab.contentReady", new object[] { Element, targetIndex, isPopup });
                if (AllowDragAndDrop && isPopupElement)
                {
                    DraggedPopupItem(tabItemIndex, targetIndex);
                }

                SelectEventArgs selectArgs = new SelectEventArgs()
                {
                    PreviousIndex = previousIndex,
                    SelectedIndex = targetIndex,
                    IsSwiped = eventArgs.IsSwiped
                };
                await SfBaseUtils.InvokeEvent<SelectEventArgs>(Delegates?.Selected, selectArgs);
            }
        }

        internal async Task OnClientChanged(int selectedValue)
        {
            IsSelectedItemChanged = SelectedItemChanged.HasDelegate;
            SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(selectedValue, selectedItem, SelectedItemChanged);
            SelectContent();
            if (LoadOn != ContentLoad.Init)
            {
                StateHasChanged();
            }
        }

        internal async Task AddItems(List<TabItem> items, int index)
        {
            var temp = index;
            if (Items == null)
            {
                Items = tabitems = new List<TabItem>();
            }

            for (var i = 0; i < items.Count; i++)
            {
                Items.Insert(index, items[i]);
                SetUniqueID(items[i]);
                var visibleIndex = index > visibleItems.Count ? visibleItems.Count : index;
                if (items[i].Visible)
                {
                    visibleItems.Insert(visibleIndex, items[i]);
                }

                index = index + 1;
            }

            SfBaseUtils.UpdateDictionary(ITEMS, Items, PropertyChanges);
            if (temp < SelectedItem)
            {
                SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(SelectedItem + items.Count, selectedItem, SelectedItemChanged);
            }
        }

        internal async Task RemoveItem(int index)
        {
            if (Items != null && index < Items.Count)
            {
                if (visibleItems.Contains(Items[index]))
                {
                    visibleItems.Remove(Items[index]);
                }

                Items.RemoveAt(index);
                if (index < SelectedItem)
                {
                    SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(SelectedItem - 1, selectedItem, SelectedItemChanged);
                }
            }

            await SetToolbarItems();
            if (Toolbar != null)
            {
                Toolbar.IsItemChanged = true;
            }

            PropertyChanges?.Clear();
            StateHasChanged();
            RemoveEventArgs removedEventArgs = new RemoveEventArgs()
            {
                Name = REMOVED,
                RemovedIndex = index
            };
            await SfBaseUtils.InvokeEvent<RemoveEventArgs>(Delegates?.Removed, removedEventArgs);
        }
        #endregion

        #region JSInterop methods
#pragma warning disable SA1604 // Element documentation should have summary
#pragma warning disable SA1611 // Element parameters should be documented
#pragma warning disable SA1615 // Element return value should be documented

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreatedEvent()
        {
            await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, null);
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<bool> OnDragStart(int draggingIndex)
        {
            TabItem draggedItem = null;
            if (Items.Count > 0 && draggingIndex >= 0 && draggingIndex < Items.Count)
            {
                draggedItem = Items[draggingIndex];
            }

            DragEventArgs dragArgs = new DragEventArgs()
            {
                Index = draggingIndex,
                DraggedItem = draggedItem
            };
            await SfBaseUtils.InvokeEvent<DragEventArgs>(Delegates?.OnDragStart, dragArgs);
            return dragArgs.Cancel;
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<bool> Dragged(int droppingIndex, int draggingIndex, int left, int top)
        {
            TabItem draggedItem = null;
            TabItem droppedItem = null;
            if (Items.Count > 0 && draggingIndex >= 0 && draggingIndex >= 0 && draggingIndex < Items.Count && droppingIndex < Items.Count)
            {
                draggedItem = Items[draggingIndex];
                droppedItem = Items[droppingIndex];
            }

            DragEventArgs dropArgs = new DragEventArgs()
            {
                Index = droppingIndex,
                DraggedItem = draggedItem,
                DroppedItem = droppedItem,
                Left = left,
                Top = top
            };
            await SfBaseUtils.InvokeEvent<DragEventArgs>(Delegates?.Dragged, dropArgs);
            if (!dropArgs.Cancel)
            {
                if (Items != null)
                {
                    TabItem item = Items[draggingIndex];
                    Items.RemoveAt(draggingIndex);
                    Items.Insert(droppingIndex, item);
                }

                SelectedItem = selectedItem = await SfBaseUtils.UpdateProperty(droppingIndex, selectedItem, SelectedItemChanged);
                await UpdateToolbarItems();
            }
            else
            {
                await UpdateToolbarItems();
            }

            return dropArgs.Cancel;
        }

        private async Task UpdateToolbarItems()
        {
            await SetToolbarItems();
            if (Toolbar != null)
            {
                Toolbar.IsItemChanged = true;
            }

            PropertyChanges?.Clear();
            StateHasChanged();
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task SelectingEvent(SelectingEventArgs args, int? dataIndex)
        {
            previousIndex = SelectedItem;
            if (args != null)
            {
                await SfBaseUtils.InvokeEvent<SelectingEventArgs>(Delegates?.Selecting, args);
                if (!args.Cancel && dataIndex != null)
                {
                    await OnClientChanged(dataIndex.Value);
                    IJSInProcessRuntime runtime = JSRuntime as IJSInProcessRuntime;
                    if (runtime != null)
                    {
                        await Task.Yield();
                    }

                    await InvokeMethod("sfBlazor.Tab.selectingContent", new object[] { Element, dataIndex.Value });
                    SelectEventArgs selectArgs = new SelectEventArgs()
                    {
                        PreviousIndex = previousIndex,
                        SelectedIndex = dataIndex.Value,
                        IsSwiped = args.IsSwiped
                    };
                    await SfBaseUtils.InvokeEvent<SelectEventArgs>(Delegates?.Selected, selectArgs);
                }
            }
        }
#pragma warning restore SA1604 // Element documentation should have summary
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1615 // Element return value should be documented
        #endregion
    }
}