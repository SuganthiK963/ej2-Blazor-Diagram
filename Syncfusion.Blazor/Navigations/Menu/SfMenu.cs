using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Menu is a graphical user interface that serve as navigation headers for your application.
    /// </summary>
    public partial class SfMenu<TValue> : SfMenuBase<TValue>
    {
        internal bool SubMenuOpen;
        internal SfContextMenu<TValue> SubMenu;
        internal List<TValue> SubMenuItems;
        internal SfContextMenu<MenuItemModel> SelfDataSubMenu;
        internal List<MenuItemModel> SubMenuItemsModel;
        internal bool isMenuRendered;
        private readonly string id = SfBaseUtils.GenerateID(SFMENU);
        private string containerClass;
        private bool closeMenu;
        private bool enableScrolling;
        internal double scrollHeight;
        private MenuFieldSettings fields;
        private bool hamburgerMode;

        private void Initialize()
        {
            var container = CONTAINER;
            if (HamburgerMode)
            {
                container += HAMBURGER;
                if (string.IsNullOrEmpty(Target) && Orientation == Orientation.Horizontal)
                {
                    closeMenu = true;
                }
                else
                {
                    NavIdx = new List<int> { 0 };
                }
            }

            containerClass = Initialize(container);
        }

        private async Task HeaderClickHandler(bool open = false)
        {
            if (!open)
            {
                open = closeMenu;
            }

            if (open)
            {
                var cancel = await BeforeOpenCloseEvent(ONOPEN, true);
                if (!cancel)
                {
                    NavIdx = new List<int> { 0 };
                    closeMenu = false;
                    SetOpenEventArgs<TValue>(default, default);
                }
            }
            else
            {
                var cancel = await CloseMenuAsync();
                if (!cancel)
                {
                    NavIdx.Clear();
                    closeMenu = true;
                }
            }
        }

        private async Task<bool> BeforeOpenCloseEvent(string eventName, bool isParent = false)
        {
            bool cancel;
            if (MenuItems == null)
            {
                var eventArgs = await TriggerBeforeOpenCloseEvent(Items[NavIdx.Count > 0 ? NavIdx[0] : 0], isParent ? Items : SubMenuItems, eventName, isParent);
                cancel = eventArgs.Cancel;
                scrollHeight = eventArgs.ScrollHeight;
            }
            else
            {
                var eventArgs = await TriggerBeforeOpenCloseEvent(MenuItems[NavIdx.Count > 0 ? NavIdx[0] : 0], isParent ? MenuItems : SubMenuItemsModel, eventName, isParent);
                cancel = eventArgs.Cancel;
                scrollHeight = eventArgs.ScrollHeight;
            }

            return cancel;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DocumentMouseDownAsync(bool refresh = false, bool skipNavIndex = false, bool closeSubMenu = false, bool isFocus = false, bool focusRefresh = false)
        {
            for (var index = 0; index < ClsCollection?.Count; index++)
            {
                if (skipNavIndex && NavIdx.Count > 0 && NavIdx[0] == index)
                {
                    continue;
                }

                ClsCollection[index].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[index].ItemClass, FOCUSED);
                if (!isFocus)
                {
                    ClsCollection[index].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[index].ItemClass, SELECTED);
                }
            }

            if (refresh && ShowItemOnClick && NavIdx.Count > 0)
            {
                if (HamburgerMode)
                {
                    await CloseMenuAsync(1);
                }
                else
                {
                    bool cancel;
                    var index = closeSubMenu ? 1 : 0;
                    if (SelfDataSubMenu == null)
                    {
                        cancel = await SubMenu?.CloseMenuAsync(index);
                    }
                    else
                    {
                        cancel = await SelfDataSubMenu.CloseMenuAsync(index);
                    }

                    if (!cancel)
                    {
                        NavIdx.Clear();
                    }
                }
            }

            if (refresh || focusRefresh)
            {
                StateHasChanged();
            }
        }

        private async Task BeforeOpenHandler<T>(BeforeOpenCloseMenuEventArgs<T> e)
        {
            scrollHeight = e.ScrollHeight = 0;
            if (e.ParentItem == null)
            {
                bool cancel = await BeforeOpenCloseEvent(ONOPEN);
                if (cancel)
                {
                    if (NavIdx.Count > 0)
                    {
                        ClsCollection[NavIdx[0]].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[NavIdx[0]].ItemClass, SELECTED);
                        ClsCollection[NavIdx[0]].ItemClass = SfBaseUtils.AddClass(ClsCollection[NavIdx[0]].ItemClass, FOCUSED);
                        NavIdx.Clear();
                    }

                    SubMenu?.Close();
                    SelfDataSubMenu?.Close();
                    e.Cancel = true;
                }
                else
                {
                    await SetPosition(CALCULATEPOS);
                }
            }
            else
            {
                if (Delegates == null)
                {
                    await SfBaseUtils.InvokeEvent(SelfRefDelegates?.OnOpen, e);
                }
                else
                {
                    await SfBaseUtils.InvokeEvent(Delegates.OnOpen, e);
                }

                scrollHeight = e.ScrollHeight;
            }
        }

        private async Task SetPosition(string name)
        {
            if (NavIdx == null || NavIdx.Count == 0)
            {
                SubMenu?.Close();
                SelfDataSubMenu?.Close();
                return;
            }

            var args = new MenuOptions()
            {
                Element = Element,
                ItemIndex = NavIdx[0],
                ShowItemOnClick = ShowItemOnClick,
                EnableScrolling = EnableScrolling,
                IsVertical = Orientation == Orientation.Vertical,
                IsRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ScrollHeight = scrollHeight
            };
            args.Popup = MenuItems == null ? SubMenu.Element : SelfDataSubMenu.Element;
            await Task.Yield();
            await InvokeMethod(name, args);
        }

        private async Task BeforeCloseHandler<T>(BeforeOpenCloseMenuEventArgs<T> e)
        {
            if (e.ParentItem == null)
            {
                bool cancel = await BeforeOpenCloseEvent(ONCLOSE);
                if (cancel)
                {
                    ClsCollection[NavIdx[0]].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[NavIdx[0]].ItemClass, FOCUSED);
                    ClsCollection[NavIdx[0]].ItemClass = SfBaseUtils.AddClass(ClsCollection[NavIdx[0]].ItemClass, SELECTED);
                    e.Cancel = true;
                }
                else
                {
                    if (NavIdx.Count > 0)
                    {
                        ClsCollection[NavIdx[0]].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[NavIdx[0]].ItemClass, SELECTED);
                        ClsCollection[NavIdx[0]].ItemClass = SfBaseUtils.AddClass(ClsCollection[NavIdx[0]].ItemClass, FOCUSED);
                    }
                    NavIdx.Clear();
                }
            }
            else
            {
                if (Delegates == null)
                {
                    await SfBaseUtils.InvokeEvent(SelfRefDelegates?.OnClose, e);
                }
                else
                {
                    await SfBaseUtils.InvokeEvent(Delegates.OnClose, e);
                }
            }
        }

        private async Task OpenedHandler<T>(OpenCloseMenuEventArgs<T> e)
        {
            if (Delegates == null)
            {
                await SfBaseUtils.InvokeEvent(SelfRefDelegates?.Opened, e);
            }
            else
            {
                await SfBaseUtils.InvokeEvent(Delegates.Opened, e);
            }
        }

        internal void SelfReferentialData()
        {
            if (fields == null)
            {
                fields = Fields;
            }
            else
            {
                Fields = fields;
            }

            MenuItems = new List<MenuItemModel>();
            foreach (var item in Items)
            {
                var itemModel = GetMenuItem(item);
                if (string.IsNullOrEmpty(itemModel.ParentId))
                {
                    MenuItems.Add(new MenuItemModel { Text = itemModel.Text, Disabled = itemModel.Disabled, Hidden = itemModel.Hidden, IconCss = itemModel.IconCss, Id = itemModel.Id, Separator = itemModel.Separator, Url = itemModel.Url });
                }
                else
                {
                    List<int> navIdxes = new List<int>();
                    var SubMenuItems = MenuItems;
                    GetIndex(itemModel.ParentId, MenuItems, navIdxes, true, true);
                    for (var i = 0; i < navIdxes.Count; i++)
                    {
                        if (navIdxes[i] == -1)
                        {
                            break;
                        }

                        if (SubMenuItems[navIdxes[i]].Items == null)
                        {
                            SubMenuItems[navIdxes[i]].Items = new List<MenuItemModel>();
                        }

                        SubMenuItems = SubMenuItems[navIdxes[i]].Items;
                        if (i == navIdxes.Count - 1)
                        {
                            SubMenuItems.Add(new MenuItemModel { Text = itemModel.Text, Disabled = itemModel.Disabled, Hidden = itemModel.Hidden, IconCss = itemModel.IconCss, Id = itemModel.Id, Separator = itemModel.Separator, Url = itemModel.Url });
                        }
                    }
                }
            }

            Fields = new MenuFieldSettings();
            ClsCollection = new List<ClassCollection>();
            StateHasChanged();
        }

        private async Task TriggerOpenCloseEvent<T>(OpenCloseMenuEventArgs<T> e, bool open, bool focus)
        {
            MenuEvents<T> delegates = SelfRefDelegates == null ? Delegates as MenuEvents<T> : SelfRefDelegates as MenuEvents<T>;
            await SfBaseUtils.InvokeEvent(open ? delegates?.Opened : delegates?.Closed, e);
            if (focus)
            {
                await InvokeMethod(FOCUSMENU, Element, false);
            }
        }

        private void InsertMenuItems(List<TValue> items, string text, bool isUniqueId, bool insertBefore)
        {
            List<int> navIdxes = new List<int>();
            GetIndex(text, Items, navIdxes, isUniqueId);
            if (!HamburgerMode && navIdxes.Count > 1 && NavIdx.Count == 1 && navIdxes[0] == NavIdx[0])
            {
                if (SelfDataSubMenu == null)
                {
                    if (insertBefore)
                    {
                        SubMenu?.InsertItems(items, GetIndex(text, Items, new List<int>(), isUniqueId), true);
                    }
                    else
                    {
                        SubMenu?.InsertItems(items, GetIndex(text, Items, new List<int>(), isUniqueId), false);
                    }
                }
            }
            else
            {
                InsertItems(items, navIdxes, insertBefore);
            }
        }

        internal void ComponentRefresh()
        {
            StateHasChanged();
        }
    }
}