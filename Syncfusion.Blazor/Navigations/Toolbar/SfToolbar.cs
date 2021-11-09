using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Navigations.Internal;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Internal;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Gantt")]

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The Toolbar control contains a group of commands that are aligned horizontally.
    /// </summary>
    public partial class SfToolbar : SfBaseComponent
    {
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string VERTICAL_CLASS = "e-vertical";
        private const string VERTICAL_LEFT_CLASS = "e-vertical-left";
        private const string ROLE = "role";
        private const string TOOLBAR = "toolbar";
        private const string ARIA_DISABLED = "aria-disabled";
        private const string ARIA_ORIENTATION = "aria-orientation";
        private const string VERTICAL = "vertical";
        private const string HORIZONTAL = "horizontal";
        private const string CSSCLASS = "CssClass";
        private const string WIDTH = "Width";
        private const string HEIGHT = "Height";
        private const string OVERFLOWMODE = "OverflowMode";
        private const string ENABLERTL = "EnableRtl";
        private const string SCROLLSTEP = "ScrollStep";
        private const string ENABLECOLLISION = "EnableCollision";
        private const string ALLOWKEYBOARD = "AllowKeyboard";
        private const string OVERFLOWMODECHANGED = "OverflowModeChanged";
        private const string TOOLBARCLICKED = "ToolbarClicked";
        private const string CSSCLASS_NAME = "cssClass";
        private const string WIDTHNAME = "width";
        private const string HEIGHTNAME = "height";
        private const string OVERFLOW = "overflowMode";
        private const string RTL_ENABLE = "enableRtl";
        private const string SCROLL = "scrollStep";
        private const string COLLISION = "enableCollision";
        private const string KEYBOARD = "allowKeyboard";
        private const string CLIENT_ITEMS = "items";
        private const string ISVERTICAl = "isVertical";
        private const string ISVERTICAL_LEFT = "isVerticalLeft";
        private const string ITEM_CLICK = "click";
        private const string TOOLBAR_CLICKED = "clicked";
        private const string ITEMS_CHANGED = "OnItemsChanged";
        private const string INITIAL_LOAD = "InitialLoad";
        private Dictionary<string, object> containerAttributes = new Dictionary<string, object>();

        #region Internal variables
        internal bool PreventPropChange { get; set; }

        internal List<ItemModel> ToolbarItems { get; set; } = new List<ItemModel>();

        internal EventAggregator EventAggregator { get; set; } = new EventAggregator();

        internal ToolbarEvents Delegates { get; set; }

        internal bool IsItemChanged { get; set; }

        internal bool IsLoaded { get; set; }

        internal bool IsVertical { get; set; }

        internal bool IsVerticalLeft { get; set; }
        #endregion

        #region Private variables

        private string ToolbarClass { get; set; } = "e-control e-toolbar e-lib";

        private bool IsInitialModeMultiRow { get; set; }

        #endregion

        private bool SetItems()
        {
            if (Items != null)
            {
                ToolbarItems = new List<ItemModel>();
                for (var i = 0; i < Items.Count; i++)
                {
                    if (!Items[i].ItemFromTag)
                    {
                        if (string.IsNullOrEmpty(Items[i].Id))
                        {
                            Items[i] = ToolbarItem.SetId(Items[i]);
                        }

                        ItemModel item = new ItemModel()
                        {
                            Align = Items[i].Align,
                            CssClass = Items[i].CssClass,
                            HtmlAttributes = Items[i].HtmlAttributes,
                            Id = Items[i].Id,
                            Overflow = Items[i].Overflow,
                            PrefixIcon = Items[i].PrefixIcon,
                            Disabled = Items[i].Disabled,
                            ShowAlwaysInPopup = Items[i].ShowAlwaysInPopup,
                            ShowTextOn = Items[i].ShowTextOn,
                            SuffixIcon = Items[i].SuffixIcon,
                            Template = Items[i].Template,
                            Text = Items[i].Text,
                            TooltipText = Items[i].TooltipText,
                            Type = Items[i].Type,
                            Visible = Items[i].Visible,
                            Width = Items[i].Width,
                            Index = i
                        };
                        ToolbarItems.Add(item);
                    }
                }
            }

            return ToolbarItems.Count != 0;
        }

        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> toolbarObj = new Dictionary<string, object>();
            toolbarObj.Add(KEYBOARD, AllowKeyboard);
            toolbarObj.Add(CSSCLASS_NAME, CssClass);
            toolbarObj.Add(COLLISION, EnableCollision);
            toolbarObj.Add(RTL_ENABLE, EnableRtl || SyncfusionService.options.EnableRtl);
            toolbarObj.Add(HEIGHTNAME, Height);
            toolbarObj.Add(CLIENT_ITEMS, Items);
            toolbarObj.Add(OVERFLOW, OverflowMode);
            toolbarObj.Add(SCROLL, ScrollStep);
            toolbarObj.Add(WIDTHNAME, Width);
            toolbarObj.Add(ISVERTICAl, IsVertical);
            toolbarObj.Add(ISVERTICAL_LEFT, IsVerticalLeft);
            return toolbarObj;
        }

        private void UpdateHtmlAttributes()
        {
            if (HtmlAttributes != null)
            {
                foreach (var item in HtmlAttributes)
                {
                    if (item.Key == "class")
                    {
                        ToolbarClass += " " + item.Value;
                    }
                    else
                    {
                        SfBaseUtils.UpdateDictionary(item.Key, item.Value, containerAttributes);
                    }
                }
            }
        }

        #region Internal methods
        internal static ItemModel GetItem(ToolbarItem toolbarItem)
        {
            ItemModel item = new ItemModel();
            if (toolbarItem != null)
            {
                item.Align = toolbarItem.Align;
                item.CssClass = toolbarItem.CssClass;
                item.HtmlAttributes = toolbarItem.HtmlAttributes;
                item.Id = toolbarItem.Id;
                item.Overflow = toolbarItem.Overflow;
                item.PrefixIcon = toolbarItem.PrefixIcon;
                item.Disabled = toolbarItem.Disabled;
                item.ShowAlwaysInPopup = toolbarItem.ShowAlwaysInPopup;
                item.ShowTextOn = toolbarItem.ShowTextOn;
                item.SuffixIcon = toolbarItem.SuffixIcon;
                item.Template = toolbarItem.Template;
                item.Text = toolbarItem.Text;
                item.TooltipText = toolbarItem.TooltipText;
                item.Type = toolbarItem.Type;
                item.Visible = toolbarItem.Visible;
                item.Width = toolbarItem.Width;
            }

            return item;
        }

        internal void UpdateLocalProperties()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ToolbarClass = ToolbarClass + SPACE + CssClass;
            }

            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                ToolbarClass += SPACE + RTL;
            }

            if (ToolbarClass.Contains(VERTICAL_CLASS, StringComparison.CurrentCulture))
            {
                IsVertical = true;
            }

            if (ToolbarClass.Contains(VERTICAL_LEFT_CLASS, StringComparison.CurrentCulture))
            {
                IsVerticalLeft = true;
            }

            containerAttributes.Add(ROLE, TOOLBAR);
            containerAttributes.Add(ARIA_DISABLED, false);
            containerAttributes.Add(ARIA_ORIENTATION, IsVertical ? VERTICAL : HORIZONTAL);
            UpdateHtmlAttributes();
        }

        internal void SetRtl(bool isEnableRtl)
        {
            EnableRtl = enableRtl = NotifyPropertyChanges(ENABLERTL, isEnableRtl, enableRtl);
            StateHasChanged();
        }

        internal async Task OnPropertyChangeHandler()
        {
            if (PropertyChanges.ContainsKey(CSSCLASS) && !PreventPropChange)
            {
                await InvokeMethod("sfBlazor.Toolbar.setCssClass", Element, CssClass);
            }

            if (PropertyChanges.ContainsKey(WIDTH) && !PreventPropChange)
            {
                await InvokeMethod("sfBlazor.Toolbar.setWidth", Element, Width);
            }

            if (PropertyChanges.ContainsKey(HEIGHT) && !PreventPropChange)
            {
                await InvokeMethod("sfBlazor.Toolbar.setHeight", Element, Height);
            }

            PreventPropChange = false;
            if (PropertyChanges.ContainsKey(OVERFLOWMODE))
            {
                await InvokeMethod("sfBlazor.Toolbar.setOverflowMode", Element, OverflowMode);
                EventAggregator.Notify(OVERFLOWMODECHANGED, null);
            }

            if (PropertyChanges.ContainsKey(ENABLERTL))
            {
                await InvokeMethod("sfBlazor.Toolbar.setEnableRTL", Element, EnableRtl);
            }

            if (PropertyChanges.ContainsKey(SCROLLSTEP))
            {
                await InvokeMethod("sfBlazor.Toolbar.setScrollStep", Element, ScrollStep);
            }

            if (PropertyChanges.ContainsKey(ENABLECOLLISION))
            {
                await InvokeMethod("sfBlazor.Toolbar.setEnableCollision", Element, EnableCollision);
            }

            if (PropertyChanges.ContainsKey(ALLOWKEYBOARD))
            {
                await InvokeMethod("sfBlazor.Toolbar.setAllowKeyboard", Element, AllowKeyboard);
            }
        }
        #endregion

        #region JSInterop methods
#pragma warning disable SA1604 // Element documentation should have summary
#pragma warning disable SA1611 // Element parameters should be documented
#pragma warning disable SA1615 // Element return value should be documented

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerClickEvent(MouseEventArgs e, bool isPopupElement, bool isCloseIcon, int? trgParentDataIndex, string id, int? tabItemIndex)
        {
            ClickEventArgs args = new ClickEventArgs()
            {
                Name = ITEM_CLICK,
                OriginalEvent = e,
            };
            ToolbarEventArgs tbarArgs = new ToolbarEventArgs()
            {
                IsCloseIcon = isCloseIcon,
                TargetParentDataIndex = trgParentDataIndex,
                ToolbarItemIndex = tabItemIndex,
                IsPopupElement = isPopupElement
            };
            ToolbarItem item = null;
            if (trgParentDataIndex != null && trgParentDataIndex >= 0 && !string.IsNullOrEmpty(id) && Items.Exists(x => x.Id == id))
            {
                item = Items.Find(x => x.Id.Contains(id, StringComparison.Ordinal));
            }

            if (item != null)
            {
                args.Item = new ItemModel()
                {
                    Align = item.Align,
                    CssClass = item.CssClass,
                    Disabled = item.Disabled,
                    HtmlAttributes = item.HtmlAttributes,
                    Id = item.Id,
                    Overflow = item.Overflow,
                    PrefixIcon = item.PrefixIcon,
                    ShowAlwaysInPopup = item.ShowAlwaysInPopup,
                    ShowTextOn = item.ShowTextOn,
                    SuffixIcon = item.SuffixIcon,
                    Template = item.Template,
                    Text = item.Text,
                    TooltipText = item.TooltipText,
                    Type = item.Type,
                    Visible = item.Visible,
                    Width = item.Width
                };
                if (item.OnClick.HasDelegate)
                {
                    await item.OnClick.InvokeAsync(args);
                }
            }

            if (!args.Cancel)
            {
                args.Name = TOOLBAR_CLICKED;
                await SfBaseUtils.InvokeEvent<ClickEventArgs>(Delegates?.Clicked, args);
                EventAggregator.Notify(TOOLBARCLICKED, tbarArgs);
                if (isPopupElement && !args.Cancel && OverflowMode == OverflowMode.Popup && args.Item != null && args.Item.Type != ItemType.Input)
                {
                    await InvokeMethod("sfBlazor.Toolbar.hidePopup", new object[] { Element });
                }
            }
        }
#pragma warning restore SA1604 // Element documentation should have summary
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1615 // Element return value should be documented
        #endregion
    }
}