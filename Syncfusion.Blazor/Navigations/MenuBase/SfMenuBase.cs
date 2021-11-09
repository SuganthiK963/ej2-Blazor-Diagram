using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations.Internal
{
    public partial class SfMenuBase<TValue> : SfBaseComponent
    {
        internal MenuEvents<TValue> Delegates { get; set; }

        internal MenuEvents<MenuItemModel> SelfRefDelegates { get; set; }

        internal OpenCloseMenuEventArgs<TValue> CloseEventArgs;
        internal OpenCloseMenuEventArgs<MenuItemModel> CloseMenuEventArgs;
        internal OpenCloseMenuEventArgs<TValue> OpenEventArgs;
        internal OpenCloseMenuEventArgs<MenuItemModel> OpenMenuEventArgs;
        internal MenuAnimationSettings AnimationSettings;
        internal MenuFieldSettings Fields = new MenuFieldSettings();
        internal MenuTemplates<TValue> MenuTemplates;
        internal List<ClassCollection> ClsCollection = new List<ClassCollection>();
        internal List<int> NavIdx = new List<int>();
        internal List<MenuItemModel> MenuItems;
        internal ElementReference Element;
        internal bool IsMenu;
        internal bool IsDevice;
        internal double? Left;
        internal double? Top;
        private string cssClass;
        private bool enableRtl;

        protected string Initialize(string container)
        {
            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                container += RTL;
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                container += SPACE + CssClass;
            }

            return container;
        }

        internal ItemModel<T> BeforeItemCreation<T>(T item, int index, List<ClassCollection> itemClasses, bool triggerEvent = true)
        {
            if (itemClasses.Count == index)
            {
                itemClasses.Add(new ClassCollection { ItemClass = MENUITEM });
                if (triggerEvent)
                {
                    Task.Run(async delegate
                    {
                        if (Delegates == null)
                        {
                            await SfBaseUtils.InvokeEvent(SelfRefDelegates?.OnItemRender, new MenuEventArgs<T>() { Name = BEFORERENDER, Item = item });
                        }
                        else
                        {
                            await SfBaseUtils.InvokeEvent(Delegates?.OnItemRender, new MenuEventArgs<T>() { Name = BEFORERENDER, Item = item });
                        }
                    });
                }
            }

            var menuItem = GetMenuItem(item);
            itemClasses[index].ItemClass = UpdateClass(menuItem.Items != null, itemClasses[index].ItemClass, MENUCARET);
            itemClasses[index].ItemClass = UpdateClass(menuItem.Disabled, itemClasses[index].ItemClass, DISABLED);
            itemClasses[index].ItemClass = UpdateClass(menuItem.Hidden, itemClasses[index].ItemClass, HIDE);
            itemClasses[index].ItemClass = UpdateClass(menuItem.Separator, itemClasses[index].ItemClass, SEPARATOR);
            itemClasses[index].ItemClass = UpdateClass(!string.IsNullOrEmpty(menuItem.Url), itemClasses[index].ItemClass, NAVIGATION);
            var attributes = Utils.GetItemProperties<Dictionary<string, object>, T>(item, Fields.HtmlAttributes);
            menuItem.HtmlAttributes = new Dictionary<string, object>();
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    if (attribute.Key == CLASS)
                    {
                        menuItem.HtmlAttributes.Add(CLASS, itemClasses[index].ItemClass + SPACE + attribute.Value);
                    }
                    else
                    {
                        menuItem.HtmlAttributes.Add(attribute.Key, attribute.Value);
                    }
                }
            }

            if (!menuItem.HtmlAttributes.ContainsKey(CLASS))
            {
                menuItem.HtmlAttributes.Add(CLASS, itemClasses[index].ItemClass);
            }

            if (!menuItem.HtmlAttributes.ContainsKey(ITEMID) && !string.IsNullOrEmpty(menuItem.Id))
            {
                menuItem.HtmlAttributes.Add(ITEMID, menuItem.Id);
            }

            if (menuItem.Items != null)
            {
                menuItem.HtmlAttributes.Add(HASPOPUP, TRUE);
                menuItem.HtmlAttributes.Add(EXPANDED, itemClasses[index].ItemClass.Contains(SELECTED, StringComparison.Ordinal) ? TRUE : FALSE);
            }

            return menuItem;
        }

        private static string UpdateClass(bool add, string prevClass, string className)
        {
            if (add)
            {
                prevClass = SfBaseUtils.AddClass(prevClass, className);
            }
            else
            {
                prevClass = SfBaseUtils.RemoveClass(prevClass, className);
            }

            return prevClass;
        }

        internal bool HandleBlankIcon<T>(List<int> blankIconidxes, bool blankIcon, ItemModel<T> item, List<ClassCollection> itemClasses, int index)
        {
            if (itemClasses[index].ItemClass.Contains(BLANKICON, StringComparison.Ordinal))
            {
                return blankIcon;
            }

            if (!blankIcon)
            {
                if (string.IsNullOrEmpty(item.IconCss))
                {
                    blankIconidxes.Add(index);
                }
                else
                {
                    blankIcon = true;
                    foreach (var idx in blankIconidxes)
                    {
                        itemClasses[idx].ItemClass = SfBaseUtils.AddClass(itemClasses[idx].ItemClass, BLANKICON);
                    }

                    if (blankIconidxes.Count > 0)
                    {
                        blankIconidxes.Clear();
                        StateHasChanged();
                    }
                }
            }
            else if (string.IsNullOrEmpty(item.IconCss))
            {
                itemClasses[index].ItemClass = SfBaseUtils.AddClass(itemClasses[index].ItemClass, BLANKICON);
                item.HtmlAttributes[CLASS] = SfBaseUtils.AddClass((string)item.HtmlAttributes[CLASS], BLANKICON);
            }

            return blankIcon;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<bool> CloseMenuAsync(int startIndex = 0, bool isKey = false, bool refresh = false, bool hamburgerMode = false)
        {
            var cancel = false;
            var i = NavIdx.Count - 1;
            while (i >= startIndex)
            {
                var items = Items;
                var menuItems = MenuItems;
                var itemClasses = ClsCollection;
                TValue parentItem = default;
                MenuItemModel parentMenuItem = default;
                if (i != 0)
                {
                    for (var j = 1; j <= i; j++)
                    {
                        if (j == i)
                        {
                            if (MenuItems == null)
                            {
                                parentItem = items[NavIdx[j]];
                            }
                            else
                            {
                                parentMenuItem = menuItems[NavIdx[j]];
                            }
                        }
                        else
                        {
                            itemClasses = itemClasses[NavIdx[j]].ClassList;
                        }

                        if (MenuItems == null)
                        {
                            items = GetMenuItem(items[NavIdx[j]]).Items;
                        }
                        else
                        {
                            menuItems = GetMenuItem(menuItems[NavIdx[j]]).Items;
                        }
                    }
                }

                if (MenuItems == null)
                {
                    var eventArgs = await TriggerBeforeOpenCloseEvent(parentItem, items, ONCLOSE, i == 0);
                    cancel = eventArgs.Cancel;
                }
                else
                {
                    var eventArgs = await TriggerBeforeOpenCloseEvent(parentMenuItem, menuItems, ONCLOSE, i == 0);
                    cancel = eventArgs.Cancel;
                }

                if (cancel || IsDisposed)
                {
                    if (!IsDisposed && (hamburgerMode || IsMenu))
                    {
                        await InvokeMethod(FOCUSMENU, Element);
                    }

                    break;
                }
                else
                {
                    if (i == 0)
                    {
                        itemClasses.Clear();
                    }
                    else
                    {
                        itemClasses[NavIdx[i]].ClassList.Clear();
                        itemClasses[NavIdx[i]].ItemClass = SfBaseUtils.RemoveClass(itemClasses[NavIdx[i]].ItemClass, SELECTED);
                        if (isKey)
                        {
                            itemClasses[NavIdx[i]].ItemClass = SfBaseUtils.AddClass(itemClasses[NavIdx[i]].ItemClass, FOCUSED);
                        }
                    }

                    if (NavIdx.Count > 0)
                    {
                        NavIdx.RemoveAt(i);
                    }

                    if (MenuItems == null)
                    {
                        CloseEventArgs = new OpenCloseMenuEventArgs<TValue> { Name = CLOSED, Items = items, ParentItem = parentItem, Element = Element };
                    }
                    else
                    {
                        CloseMenuEventArgs = new OpenCloseMenuEventArgs<MenuItemModel> { Name = CLOSED, Items = menuItems, ParentItem = parentMenuItem, Element = Element };
                    }
                }

                i--;
                if (i < startIndex && refresh)
                {
                    StateHasChanged();
                }
            }

            return cancel;
        }

        internal async Task OpenCloseSubMenu<T>(T item, bool isClick = false, bool isUpDownKey = false, bool isRightKey = false)
        {
            List<T> items = MenuItems as List<T>;
            if (items == null)
            {
                items = Items as List<T>;
            }

            List<ClassCollection> itemClasses = ClsCollection;
            RemoveFocus(item, items, itemClasses);
            var itemsPos = 1;
            int curIdx = items.IndexOf(item);
            if (curIdx == -1)
            {
                for (var i = 1; i < NavIdx.Count; i++)
                {
                    items = Utils.GetItemProperties<List<T>, T>(items[NavIdx[i]], Fields.Children);
                    itemClasses = itemClasses[NavIdx[i]].ClassList;
                    RemoveFocus(item, items, itemClasses);
                    itemsPos++;
                    curIdx = items.IndexOf(item);
                    if (curIdx > -1)
                    {
                        break;
                    }
                }
            }

            if (curIdx < 0 || curIdx >= items.Count || curIdx >= itemClasses.Count)
            {
                return;
            }

            if (!isUpDownKey && (!ShowItemOnClick || isClick))
            {
                var subItems = Utils.GetItemProperties<List<T>, T>(item, Fields.Children);
                if (subItems == null)
                {
                    await CloseMenuAsync(itemsPos);
                }
                else
                {
                    if (!itemClasses[curIdx].ItemClass.Contains(SELECTED, StringComparison.Ordinal))
                    {
                        var canceled = await CloseMenuAsync(itemsPos);
                        if (!canceled)
                        {
                            var eventArgs = await TriggerBeforeOpenCloseEvent(items[curIdx], subItems, ONOPEN);
                            if (!eventArgs.Cancel)
                            {
                                NavIdx.Add(curIdx);
                                itemClasses[curIdx].ClassList = new List<ClassCollection>();
                                itemClasses[curIdx].ItemClass = SfBaseUtils.RemoveClass(itemClasses[curIdx].ItemClass, FOCUSED);
                                itemClasses[curIdx].ItemClass = SfBaseUtils.AddClass(itemClasses[curIdx].ItemClass, SELECTED);
                                if (isRightKey)
                                {
                                    InitializeItemClasses(subItems, itemClasses[curIdx].ClassList);
                                    var subIdx = CheckIndex(itemClasses[curIdx].ClassList, true, 0, 0);
                                    if (subIdx != -1)
                                    {
                                        itemClasses[curIdx].ClassList[subIdx].ItemClass = SfBaseUtils.AddClass(itemClasses[curIdx].ClassList[subIdx].ItemClass, FOCUSED);
                                    }
                                }

                                SetOpenEventArgs(subItems, items[curIdx]);
                            }
                        }
                    }
                    else if ((ShowItemOnClick || IsDevice) && isClick && !isRightKey)
                    {
                        await CloseMenuAsync(itemsPos);
                    }
                }
            }

            if (curIdx > -1 && curIdx < itemClasses.Count && !itemClasses[curIdx].ItemClass.Contains(FOCUSED, StringComparison.Ordinal) &&
                !itemClasses[curIdx].ItemClass.Contains(SELECTED, StringComparison.Ordinal))
            {
                itemClasses[curIdx].ItemClass = SfBaseUtils.AddClass(itemClasses[curIdx].ItemClass, FOCUSED);
                if (ShowItemOnClick)
                {
                    for (var i = itemsPos; i < NavIdx.Count; i++)
                    {
                        items = Utils.GetItemProperties<List<T>, T>(items[NavIdx[i]], Fields.Children);
                        itemClasses = itemClasses[NavIdx[i]].ClassList;
                        RemoveFocus(item, items, itemClasses, true);
                    }
                }
            }
        }

        private void InitializeItemClasses<T>(List<T> items, List<ClassCollection> itemClassess)
        {
            for (var idx = 0; idx < items.Count; idx++)
            {
                BeforeItemCreation(items[idx], idx, itemClassess, false);
            }
        }

        private static int RemoveFocus<T>(T item, List<T> items, List<ClassCollection> itemClasses, bool isKey = false)
        {
            var i = 0;
            var index = -1;
            foreach (var itemClass in itemClasses)
            {
                var idx = items.IndexOf(item);
                if (itemClass.ItemClass.Contains(FOCUSED, StringComparison.Ordinal) && (idx != i || isKey))
                {
                    itemClass.ItemClass = SfBaseUtils.RemoveClass(itemClass.ItemClass, FOCUSED);
                    index = i;
                }

                i++;
            }

            return index;
        }

        private int CheckIndex(List<ClassCollection> itemClasses, bool isDown, int index, int count)
        {
            if (count == itemClasses.Count)
            {
                return -1;
            }

            if (itemClasses[index].ItemClass.Contains(SEPARATOR, StringComparison.Ordinal) || itemClasses[index].ItemClass.Contains(HIDE, StringComparison.Ordinal) ||
                itemClasses[index].ItemClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                index = isDown ? (index == itemClasses.Count - 1 ? 0 : index + 1) : (index == 0 ? itemClasses.Count - 1 : index - 1);
                count++;
            }

            if (itemClasses[index].ItemClass.Contains(SEPARATOR, StringComparison.Ordinal) || itemClasses[index].ItemClass.Contains(HIDE, StringComparison.Ordinal)
                || itemClasses[index].ItemClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                index = CheckIndex(itemClasses, isDown, index, count);
            }

            return index;
        }

        internal async Task ClickHandler<T>(List<T> menuItems, T item, System.EventArgs e, bool isEnterKey = false, bool isUl = false, bool header = false, bool hamburgerMode = false)
        {
            await Task.Yield();
            if (!isEnterKey || !isUl)
            {
                if (!header)
                {
                    if (Delegates == null)
                    {
                        await SfBaseUtils.InvokeEvent(SelfRefDelegates?.ItemSelected, new MenuEventArgs<T>() { Name = SELECT, Item = item, Event = e });
                    }
                    else
                    {
                        await SfBaseUtils.InvokeEvent(Delegates.ItemSelected, new MenuEventArgs<T>() { Name = SELECT, Item = item, Event = e });
                    }
                }

                if (IsDisposed)
                {
                    return;
                }

                if (Utils.GetItemProperties<List<T>, T>(item, Fields.Children) == null)
                {
                    if (hamburgerMode)
                    {
                        await CloseMenuAsync(1, isEnterKey, false, hamburgerMode);
                        var idx = menuItems.IndexOf(item);
                        if (idx > -1 && idx < ClsCollection.Count && !ClsCollection[idx].ItemClass.Contains(SELECTED, StringComparison.Ordinal))
                        {
                            ClsCollection[idx].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[idx].ItemClass, FOCUSED);
                            ClsCollection[idx].ItemClass = SfBaseUtils.AddClass(ClsCollection[idx].ItemClass, SELECTED);
                        }
                    }
                    else
                    {
                        await CloseMenuAsync();
                    }
                }
                else if (ShowItemOnClick || IsDevice || isEnterKey)
                {
                    await OpenCloseSubMenu(item, true, false, isEnterKey);
                }
            }
        }

        internal async Task KeyActionHandler<T>(List<T> menuItems, T item, KeyboardEventArgs e, bool isUl, bool isParentMenu = false, bool hamburgerMode = false)
        {
            if (e.Code == ARROWUP || e.Code == ARROWDOWN)
            {
                List<ClassCollection> itemClasses = ClsCollection;
                List<T> items = menuItems;
                var count = isParentMenu ? 1 : NavIdx.Count;
                for (var i = 0; i < count; i++)
                {
                    if (i != 0)
                    {
                        itemClasses = itemClasses[NavIdx[i]].ClassList;
                        items = Utils.GetItemProperties<List<T>, T>(items[NavIdx[i]], Fields.Children);
                    }

                    if (items.IndexOf(item) > -1)
                    {
                        var idx = RemoveFocus(item, items, itemClasses, true);
                        if (idx == -1)
                        {
                            idx = items.IndexOf(item);
                        }
                        else if (isUl)
                        {
                            isUl = false;
                        }

                        if (!isUl)
                        {
                            idx = e.Code == ARROWDOWN ? (idx == items.Count - 1 ? 0 : idx + 1) : (idx == 0 ? items.Count - 1 : idx - 1);
                        }

                        idx = CheckIndex(itemClasses, e.Code == ARROWDOWN, idx, 0);
                        if (idx != -1)
                        {
                            itemClasses[idx].ItemClass = SfBaseUtils.AddClass(itemClasses[idx].ItemClass, FOCUSED);
                        }

                        break;
                    }
                    else if (ShowItemOnClick)
                    {
                        RemoveFocus(item, items, itemClasses, true);
                    }
                }
            }
            else if (e.Code == ESC || (((EnableRtl || SyncfusionService.options.EnableRtl) ? e.Code == ARROWRIGHT : e.Code == ARROWLEFT) && (NavIdx.Count > 1 || IsMenu)))
            {
                await CloseMenuAsync(NavIdx.Count - 1, true, false, hamburgerMode);
            }
            else if ((EnableRtl || SyncfusionService.options.EnableRtl) ? e.Code == ARROWLEFT : e.Code == ARROWRIGHT)
            {
                if (!isUl && Utils.GetItemProperties<List<T>, T>(item, Fields.Children) != null)
                {
                    await OpenCloseSubMenu(item, true, false, true);
                }
            }
            else if (e.Code == HOME || e.Code == END)
            {
                List<ClassCollection> itemClasses = ClsCollection;
                for (var i =0; i<= menuItems.Count-1; i++)
                {
                    ClsCollection[i].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[i].ItemClass, FOCUSED);
                    ClsCollection[i].ItemClass = SfBaseUtils.RemoveClass(ClsCollection[i].ItemClass, SELECTED);
                }
                var idx = e.Code == HOME ? 0 : menuItems.Count-1;
                itemClasses[idx].ItemClass = SfBaseUtils.AddClass(itemClasses[idx].ItemClass, FOCUSED);
            }
            else if (e.Code == ENTER)
            {
                await ClickHandler(menuItems, item, e, true, isUl, false, hamburgerMode);
            }
        }

        protected void SetOpenEventArgs<T>(List<T> items, T parentItem)
        {
            if (MenuItems == null)
            {
                OpenEventArgs = new OpenCloseMenuEventArgs<T>()
                {
                    Name = OPENED,
                    ParentItem = parentItem,
                    Items = items == null ? Items as List<T> : items,
                    Element = Element,
                    NavigationIndex = this.NavIdx.Count - 1
                }

                as OpenCloseMenuEventArgs<TValue>;
            }
            else
            {
                OpenMenuEventArgs = new OpenCloseMenuEventArgs<MenuItemModel>()
                {
                    Name = OPENED,
                    ParentItem = parentItem as MenuItemModel,
                    Items = items == null ? MenuItems : items as List<MenuItemModel>,
                    Element = Element,
                    NavigationIndex = this.NavIdx.Count - 1
                };
            }
        }

        internal ItemModel<T> GetMenuItem<T>(T item)
        {
            ItemModel<T> itemModel = new ItemModel<T>();
            PropertyInfo[] ItemPropsInfo = item.GetType().GetProperties();
            for (int i = 0, len = ItemPropsInfo.Length; i < len; i++)
            {
                PropertyInfo prop = ItemPropsInfo[i];
                if (prop.Name == Fields.Text)
                {
                    itemModel.Text = (string)prop.GetValue(item);
                }
                else if (prop.Name == Fields.IconCss)
                {
                    itemModel.IconCss = (string)prop.GetValue(item);
                }
                else if (prop.Name == Fields.Children)
                {
                    itemModel.Items = (List<T>)prop.GetValue(item);
                }
                else if (prop.Name == Fields.Url)
                {
                    itemModel.Url = (string)prop.GetValue(item);
                }
                else if (prop.Name == Fields.Separator)
                {
                    itemModel.Separator = (bool)prop.GetValue(item);
                }
                else if (prop.Name == Fields.Disabled)
                {
                    itemModel.Disabled = (bool)prop.GetValue(item);
                }
                else if (prop.Name == Fields.Hidden)
                {
                    itemModel.Hidden = (bool)prop.GetValue(item);
                }
                else if (prop.Name == Fields.ItemId)
                {
                    itemModel.Id = (string)prop.GetValue(item);
                }
                else if (prop.Name == Fields.ParentId)
                {
                    itemModel.ParentId = (string)prop.GetValue(item);
                }
            }

            return itemModel;
        }

        internal static void SetMenuItem(TValue item, string propName, object propValue)
        {
            PropertyInfo prop = item.GetType().GetProperty(propName);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(item, propValue, null);
            }
        }

        protected async Task<BeforeOpenCloseMenuEventArgs<T>> TriggerBeforeOpenCloseEvent<T>(T parentItem, List<T> subItems, string name, bool isParent = false, double left = default, double top = default)
        {
            var eventArgs = new BeforeOpenCloseMenuEventArgs<T>() { Cancel = false, Name = name, Element = Element, Items = subItems };
            if (isParent)
            {
                eventArgs.Left = left;
                eventArgs.Top = top;
            }
            else
            {
                eventArgs.ParentItem = parentItem;
            }

            MenuEvents<T> delegates = SelfRefDelegates == null ? Delegates as MenuEvents<T> : SelfRefDelegates as MenuEvents<T>;
            await SfBaseUtils.InvokeEvent(name == ONOPEN ? delegates?.OnOpen : delegates?.OnClose, eventArgs);
            return eventArgs;
        }

        private void UpdateToggleProperty(List<string> items, bool state, string field, bool isUniqueId)
        {
            bool refresh = false;
            List<int> navIdxes;
            TValue item;
            int index;
            bool propValue;
            foreach (var itemText in items)
            {
                navIdxes = new List<int>();
                GetIndex(itemText, Items, navIdxes, isUniqueId);
                item = default;
                index = -1;
                for (var i = 0; i < navIdxes.Count; i++)
                {
                    index = navIdxes[i];
                    if (index != -1)
                    {
                        if (i == 0)
                        {
                            item = Items[index];
                        }
                        else
                        {
                            item = Utils.GetItemProperties<List<TValue>, TValue>(item, Fields.Children)[index];
                        }
                    }
                }

                if (index != -1)
                {
                    refresh = true;
                    propValue = Utils.GetItemProperties<bool, TValue>(item, field);
                    if (state)
                    {
                        if (!propValue)
                        {
                            SetMenuItem(item, field, true);
                        }
                    }
                    else
                    {
                        if (propValue)
                        {
                            SetMenuItem(item, field, false);
                        }
                    }
                }
            }

            if (refresh)
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// To update the child properties
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object result)
        {
            switch (key)
            {
                case ANIMATION:
                    AnimationSettings = (MenuAnimationSettings)result;
                    break;
                case ITEMS:
                    Items = (List<TValue>)result;
                    StateHasChanged();
                    break;
                case FIELDS:
                    Fields = (MenuFieldSettings)result;
                    break;
                case TEMPLATES:
                    MenuTemplates = (MenuTemplates<TValue>)result;
                    break;
                case MENUEVENTS:
                    Delegates = (MenuEvents<TValue>)result;
                    break;
                case SELFREFMENUEVENTS:
                    SelfRefDelegates = (MenuEvents<MenuItemModel>)result;
                    break;
            }
        }

        internal void InsertItems(List<TValue> menuItems, List<int> navIdxes, bool insertBefore)
        {
            List<TValue> items = Items;
            List<ClassCollection> itemClasses = ClsCollection;
            if (navIdxes != null)
            {
                for (var i = 0; i < navIdxes.Count; i++)
                {
                    if (navIdxes[i] != -1)
                    {
                        if (i == navIdxes.Count - 1)
                        {
                            if (itemClasses != null && itemClasses.Count == items.Count)
                            {
                                List<ClassCollection> newClsList = new List<ClassCollection>();
                                if (menuItems != null)
                                {
                                    for (var j = 0; j < menuItems.Count; j++)
                                    {
                                        newClsList.Add(new ClassCollection() { ItemClass = MENUITEM });
                                    }
                                }
                                itemClasses.InsertRange(insertBefore ? navIdxes[i] : navIdxes[i] + 1, newClsList);
                            }

                            items.InsertRange(insertBefore ? navIdxes[i] : navIdxes[i] + 1, menuItems);
                            StateHasChanged();
                            break;
                        }

                        if (itemClasses != null && itemClasses[navIdxes[i]] != null)
                        {
                            itemClasses = itemClasses[navIdxes[i]].ClassList;
                        }
                        items = Utils.GetItemProperties<List<TValue>, TValue>(items[navIdxes[i]], Fields.Children);
                    }
                }
            }
        }

        internal bool RemoveItem(List<int> navIdxes, bool isOpenMenuItem)
        {
            List<TValue> items = Items;
            List<ClassCollection> itemClasses = ClsCollection;
            if (navIdxes == null || navIdxes.Count == 0)
            {
                isOpenMenuItem = false;
            }

            for (var i = 0; i < navIdxes.Count; i++)
            {
                if (isOpenMenuItem && (i > NavIdx.Count - 1 || NavIdx[i] != navIdxes[i]))
                {
                    isOpenMenuItem = false;
                }

                if (navIdxes[i] < 0)
                {
                    break;
                }

                if (i == navIdxes.Count - 1)
                {
                    if (itemClasses != null && navIdxes[i] < itemClasses.Count - 1 && itemClasses.Count == items.Count)
                    {
                        itemClasses.RemoveAt(navIdxes[i]);
                    }

                    items.RemoveAt(navIdxes[i]);
                    break;
                }

                if (itemClasses != null && itemClasses[navIdxes[i]] != null)
                {
                    itemClasses = itemClasses[navIdxes[i]].ClassList;
                }

                items = Utils.GetItemProperties<List<TValue>, TValue>(items[navIdxes[i]], Fields.Children);
            }

            return isOpenMenuItem;
        }

        internal List<int> GetIndex<T>(string item, List<T> items, List<int> navIdx, bool isUniqueId, bool isItemModel = false)
        {
            if (items != null)
            {
                for (var i = 0; i < items.Count; i++)
                {
                    List<T> menuItems;
                    string text;
                    if (isItemModel)
                    {
                        var menuItem = items[i] as MenuItemModel;
                        text = isUniqueId ? menuItem.Id : menuItem.Text;
                        menuItems = menuItem.Items as List<T>;
                    }
                    else
                    {
                        var menuItem = GetMenuItem<T>(items[i]);
                        text = isUniqueId ? menuItem.Id : menuItem.Text;
                        menuItems = menuItem.Items;
                    }

                    if (text == item)
                    {
                        navIdx?.Add(i);
                        break;
                    }
                    else if (menuItems != null)
                    {
                        navIdx = GetIndex(item, menuItems, navIdx, isUniqueId, isItemModel);
                        if (navIdx[navIdx.Count - 1] == -1)
                        {
                            if (i != items.Count - 1)
                            {
                                navIdx.Remove(navIdx[navIdx.Count - 1]);
                            }
                        }
                        else
                        {
                            navIdx.Insert(0, i);
                            break;
                        }
                    }
                    else
                    {
                        if (i == items.Count - 1)
                        {
                            navIdx.Add(-1);
                        }
                    }
                }
            }

            return navIdx;
        }
    }
}