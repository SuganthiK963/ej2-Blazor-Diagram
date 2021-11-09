using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations.Internal
{
    public partial class CreateMenuItem<TValue, TItem> : SfBaseComponent
    {
        [CascadingParameter]
        private SfMenu<TValue> Parent { get; set; }

        [Parameter]
        public List<TItem> Items { get; set; }

        private async Task MenuHoverHandler(TItem item)
        {
            if (Parent.NavIdx == null)
            {
                return;
            }

            if (Parent.HamburgerMode)
            {
                await Parent.OpenCloseSubMenu(item);
                return;
            }

            var index = Items.IndexOf(item);
            if (Parent.NavIdx.Count == 0)
            {
                if (Parent.ShowItemOnClick)
                {
                    if (!Parent.ClsCollection[index].ItemClass.Contains(SELECTED, StringComparison.Ordinal))
                    {
                        await ClearAndUpdate(index, FOCUSED);
                    }
                }
                else if (index > -1)
                {
                    await UpdateState(item, index, FOCUSED);
                }
            }
            else
            {
                if (Parent.NavIdx[0] != index)
                {
                    if (Parent.ShowItemOnClick)
                    {
                        await ClearAndUpdate(index, FOCUSED, true);
                    }
                    else
                    {
                        await UpdateState(item, index, FOCUSED);
                    }
                }
                else if (Parent.ShowItemOnClick)
                {
                    await Parent.DocumentMouseDownAsync(false, true);
                }
            }
        }

        private async Task ClearAndUpdate(int index, string stateCls, bool skipNavIndex = false)
        {
            if (!Parent.ClsCollection[index].ItemClass.Contains(stateCls, StringComparison.Ordinal))
            {
                await Parent.DocumentMouseDownAsync(false, skipNavIndex, false, stateCls == FOCUSED);
                Parent.ClsCollection[index].ItemClass = SfBaseUtils.AddClass(Parent.ClsCollection[index].ItemClass, stateCls);
            }
        }

        private async Task ItemClickHandler(TItem item, System.EventArgs e, bool isEnterKey = false)
        {
            if (Parent.HamburgerMode)
            {
                if (Parent.NavIdx.Count == 1)
                {
                    await Parent.DocumentMouseDownAsync();
                }

                if (Parent.MenuItems == null)
                {
                    await Parent.ClickHandler(Parent.Items as List<TItem>, item, e, false, false, false, true);
                }
                else
                {
                    await Parent.ClickHandler(Parent.MenuItems, item as MenuItemModel, e, false, false, false, true);
                }

                Parent.ComponentRefresh();
                return;
            }

            if (Parent.Delegates == null)
            {
                await SfBaseUtils.InvokeEvent(Parent.SelfRefDelegates?.ItemSelected, new MenuEventArgs<TItem>() { Name = SELECT, Item = item, Event = e });
            }
            else
            {
                await SfBaseUtils.InvokeEvent(Parent.Delegates.ItemSelected, new MenuEventArgs<TItem>() { Name = SELECT, Item = item, Event = e });
            }

            var index = Items.IndexOf(item);
            if (Parent.ShowItemOnClick || isEnterKey)
            {
                if (Parent.ClsCollection[index].ItemClass.Contains(SELECTED, StringComparison.Ordinal))
                {
                    bool cancel = false;
                    if (Parent.NavIdx.Count > 0)
                    {
                        if (Parent.SelfDataSubMenu == null)
                        {
                            cancel = await Parent.SubMenu?.CloseMenuAsync();
                        }
                        else
                        {
                            cancel = await Parent.SelfDataSubMenu.CloseMenuAsync();
                        }
                    }

                    if (!cancel)
                    {
                        Parent.NavIdx.Clear();
                        Parent.ClsCollection[index].ItemClass = SfBaseUtils.RemoveClass(Parent.ClsCollection[index].ItemClass, SELECTED);
                    }
                }
                else
                {
                    await UpdateState(item, index, SELECTED);
                }
            }
            else
            {
                if (Parent.ClsCollection[index].ItemClass.Contains(FOCUSED, StringComparison.Ordinal))
                {
                    await Parent.DocumentMouseDownAsync();
                    Parent.ClsCollection[index].ItemClass = SfBaseUtils.AddClass(Parent.ClsCollection[index].ItemClass, SELECTED);
                }
            }
        }

        internal async Task KeyDownHandler(TItem item, KeyboardEventArgs e, bool isUl = false)
        {
            if (Parent.HamburgerMode)
            {
                if (!isUl && e.Code == ENTER && Parent.NavIdx.Count == 1)
                {
                    await Parent.DocumentMouseDownAsync();
                }

                if (Parent.MenuItems == null)
                {
                    await Parent.KeyActionHandler(Parent.Items as List<TItem>, item, e, isUl, false, true);
                }
                else
                {
                    await Parent.KeyActionHandler(Parent.MenuItems, item as MenuItemModel, e, isUl, false, true);
                }

                Parent.ComponentRefresh();
            }
            else
            {
                if (Parent.Orientation == Orientation.Vertical)
                {
                    if (e.Code == ARROWUP || e.Code == ARROWDOWN)
                    {
                        await Parent.KeyActionHandler(Items, item, e, isUl, true);
                    }
                    else if (!isUl && (Parent.EnableRtl ? e.Code == ARROWLEFT : e.Code == ARROWRIGHT))
                    {
                        await ItemClickHandler(item, e, true);
                    }
                }
                else if (e.Code == ARROWLEFT || e.Code == ARROWRIGHT)
                {
                    if (e.Code == ARROWLEFT)
                    {
                        e.Code = ARROWUP;
                    }
                    else
                    {
                        e.Code = ARROWDOWN;
                    }

                    await Parent.KeyActionHandler(Items, item, e, isUl, true);
                }

                if (!isUl && (e.Code == ENTER || e.Key == ARROWDOWN))
                {
                    await ItemClickHandler(item, e, true);
                }
                if (e.Code == HOME || e.Code == END)
                {
                    await Parent.KeyActionHandler(Items, item, e, isUl, true);
                }
            }
        }

        private async Task UpdateState(TItem item, int index, string stateCls)
        {
            var subItems = Utils.GetItemProperties<List<TItem>, TItem>(item, Parent.Fields.Children);
            bool cancel = false;
            if (Parent.NavIdx.Count > 0)
            {
                if (Parent.SelfDataSubMenu == null && Parent.SubMenu != null)
                {
                    cancel = await Parent.SubMenu.CloseMenuAsync();
                }
                else if(Parent.SelfDataSubMenu != null)
                {
                    cancel = await Parent.SelfDataSubMenu.CloseMenuAsync();
                }
            }

            if (!cancel)
            {
                if (subItems == null)
                {
                    Parent.NavIdx.Clear();
                    await ClearAndUpdate(index, stateCls);
                }
                else
                {
                    Parent.NavIdx = new List<int>() { index };
                    if (Parent.MenuItems == null)
                    {
                        Parent.SubMenuItems = subItems as List<TValue>;
                    }
                    else
                    {
                        Parent.SubMenuItemsModel = subItems as List<MenuItemModel>;
                    }

                    await Parent.DocumentMouseDownAsync();
                    Parent.ClsCollection[Parent.NavIdx[0]].ItemClass = SfBaseUtils.AddClass(Parent.ClsCollection[Parent.NavIdx[0]].ItemClass, SELECTED);
                    if (Parent.SubMenuOpen)
                    {
                        Parent.ComponentRefresh();
                        Parent.SubMenu?.Open();
                        Parent.SelfDataSubMenu?.Open();
                    }
                    else
                    {
                        Parent.SubMenuOpen = true;
                        Parent.ComponentRefresh();
                    }
                }
            }
        }

        private CurrentNavProps GetCurrentNavProps()
        {
            List<TItem> items;
            if (Parent.MenuItems == null)
            {
                items = Parent.Items as List<TItem>;
            }
            else
            {
                items = Parent.MenuItems as List<TItem>;
            }

            List<ClassCollection> itemClasses = Parent.ClsCollection;
            int index = -1;
            int ulIndex = -1;
            for (var i = 1; i < Parent.NavIdx.Count; i++)
            {
                ulIndex++;
                if (items == Items)
                {
                    index = Parent.NavIdx[i];
                    break;
                }

                itemClasses = itemClasses[Parent.NavIdx[i]].ClassList;
                items = Utils.GetItemProperties<List<TItem>, TItem>(items[Parent.NavIdx[i]], Parent.Fields.Children);
            }

            return new CurrentNavProps { ItemIndex = index, ItemClasses = itemClasses, UlIndex = ulIndex };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (!Parent.isMenuRendered)
            {
                Parent.isMenuRendered = true;
                var args = new MenuOptions() { Element = Parent.Element, EnableScrolling = Parent.EnableScrolling, IsRtl = Parent.EnableRtl || SyncfusionService.options.EnableRtl };
                await InvokeMethod(UPDATESCROLL, args.Element, args.EnableScrolling, args.IsRtl);
            }
        }
    }
}