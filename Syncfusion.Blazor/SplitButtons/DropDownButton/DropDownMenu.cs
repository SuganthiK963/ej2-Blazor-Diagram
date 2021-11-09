using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.SplitButtons.Internal
{
    public partial class DropDownMenu
    {
        private const string SELECTED = "ItemSelected";
        private const string ENTER = "Enter";
        private const string ONITEMRENDER = "OnItemRender";
        private const string ITEM = "e-item";
        private const string DISABLED = " e-disabled";
        private const string BLANK = " e-blank-icon";
        private const string SEPARATOR = " e-separator";
        private const string URL = " e-url";
        private const string MENUURL = "e-menu-url";
        private const string TEXT = "e-menu-text";
        private const string SPACE = " ";
        private const string ICON = "e-menu-icon";
        private const string DROPDOWN = "e-dropdown-menu";
        private const string ID = "id";
        private const string ROLE = "role";
        private const string MENUITEM = "menuitem";
        private const string TABINDEX = "tabindex";
        private const string MINUSONE = "-1";
        private const string CLASS = "class";
        private const string TARGET = "target";
        private const string HREF = "href";

        [CascadingParameter]
        private SfDropDownButton Parent { get; set; }

        private MenuItemAttributes GetMenuItemAttributes(DropDownMenuItem item, int index)
        {
            if (!Parent.itemsRendered)
            {
                Task.Run(async delegate
                {
                    await SfBaseUtils.InvokeEvent(
                        Parent.Delegates?.OnItemRender,
                        new MenuEventArgs() { Name = ONITEMRENDER, Element = Parent.popup, Item = item });
                });
            }

            if (index == Parent.Items.Count - 1)
            {
                Parent.itemsRendered = true;
            }

            var itemCls = ITEM;
            var menuItemAttributes = new MenuItemAttributes();
            if (item.Disabled)
            {
                itemCls += DISABLED;
            }

            if (item.Separator)
            {
                itemCls += SEPARATOR;
            }
            else
            {
                if (Parent.hasIcon && string.IsNullOrEmpty(item.IconCss))
                {
                    itemCls += BLANK;
                }

                if (!string.IsNullOrEmpty(item.Url))
                {
                    itemCls += URL;
                    menuItemAttributes.UrlAttributes.Add(CLASS, TEXT + SPACE + MENUURL);
                    menuItemAttributes.UrlAttributes.Add(HREF, item.Url);
                }
            }

            var attributes = item.HtmlAttributes;
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    if (attribute.Key == TARGET)
                    {
                        menuItemAttributes.UrlAttributes.Add(TARGET, attribute.Value);
                        continue;
                    }

                    if (attribute.Key == CLASS)
                    {
                        menuItemAttributes.HtmlAttributes.Add(CLASS, itemCls + SPACE + attribute.Value);
                    }
                    else
                    {
                        menuItemAttributes.HtmlAttributes.Add(attribute.Key, attribute.Value);
                    }
                }
            }

            if (!menuItemAttributes.HtmlAttributes.ContainsKey(CLASS))
            {
                menuItemAttributes.HtmlAttributes.Add(CLASS, itemCls);
            }

            if (!menuItemAttributes.HtmlAttributes.ContainsKey(ID) && !string.IsNullOrEmpty(item.Id))
            {
                menuItemAttributes.HtmlAttributes.Add(ID, item.Id);
            }

            if (!menuItemAttributes.HtmlAttributes.ContainsKey(ROLE))
            {
                menuItemAttributes.HtmlAttributes.Add(ROLE, MENUITEM);
            }

            if (!menuItemAttributes.HtmlAttributes.ContainsKey(TABINDEX))
            {
                menuItemAttributes.HtmlAttributes.Add(TABINDEX, MINUSONE);
            }

            return menuItemAttributes;
        }

        private async Task ClickHandler(MouseEventArgs e, DropDownMenuItem item)
        {
            await TriggerEvent(e, item);
        }

        private async Task TriggerEvent(EventArgs e, DropDownMenuItem item)
        {
            await SfBaseUtils.InvokeEvent(
                Parent.Delegates?.ItemSelected,
                new MenuEventArgs() { Name = SELECTED, Element = Parent.popup, Item = item, Event = e });
            await Parent.ButtonClickAsync(null);
        }

        private async Task KeyDownHandler(KeyboardEventArgs e, DropDownMenuItem item)
        {
            if (e.Code == ENTER)
            {
                await TriggerEvent(e, item);
            }
        }

        private class MenuItemAttributes
        {
            public Dictionary<string, object> HtmlAttributes { get; set; } = new Dictionary<string, object>();

            public Dictionary<string, object> UrlAttributes { get; set; } = new Dictionary<string, object>();
        }
    }
}