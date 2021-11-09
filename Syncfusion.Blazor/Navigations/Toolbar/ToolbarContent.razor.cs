using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Buttons;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using System;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <summary>
    /// Specifies toolbar content.
    /// </summary>
    public partial class ToolbarContent : ComponentBase, IDisposable
    {
        private const string SPACE = " ";
        private const string TOOLBAR_ITEM = "e-toolbar-item";
        private const string TOOLBAR_BUTTON = "e-tbar-btn";
        private const string TEMPLATE = "e-template";
        private const string TOOLBAR_TEXT = "e-toolbar-text";
        private const string TOOLBAR_BUTTON_TEXT = "e-tbtn-txt";
        private const string TOOLBAR_BUTTON_ALIGN = "e-tbtn-align";
        private const string ICONS = "e-icons";
        private const string SEPARATOR = "e-separator";
        private const string POPUP_TEXT = "e-popup-text";
        private const string OVERFLOW_SHOW = "e-overflow-show";
        private const string OVERFLOW_HIDE = "e-overflow-hide";
        private const string POPUP_ALONE = "e-popup-alone";
        private const string OVERLAY = "e-overlay";
        private const string HIDDEN = "e-hidden";
        private const string TYPE = "type";
        private const string BUTTON = "button";
        private const string TAB_INDEX = "tabindex";
        private const string NEGATIVE_VALUE = "-1";
        private const string STYLE = "style";
        private const string WIDTH = "width:";
        private const string ICON_BUTTON = "e-icon-btn";
        private const string TITLE = "title";
        private const string ARIA_LABEL = "aria-label";
        private Dictionary<string, object> buttonAttributes = new Dictionary<string, object>();
        private IDictionary<string, object> itemAttributes = new Dictionary<string, object>();
        private ItemModel item;

        /// <summary>
        /// Defines the toolbar item model.
        /// </summary>
        [Parameter]
        public ItemModel Item { get; set; }

        /// <summary>
        /// Defines toolbar item index.
        /// </summary>
        [Parameter]
        public int Index { get; set; }

        private string ItemCss { get; set; }

        private string ButtonCss { get; set; }

        private string ButtonIconCss { get; set; }

        private IconPosition ButtonIconPosition { get; set; }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Item != null && Item != item)
            {
                item = Item;
                ItemCss = TOOLBAR_ITEM;
                ButtonCss = TOOLBAR_BUTTON;
                if (Item.Template != null)
                {
                    ItemCss += SPACE + TEMPLATE;
                }
                else if (Item.Type == ItemType.Button)
                {
                    if (!string.IsNullOrEmpty(Item.Text))
                    {
                        ButtonCss += SPACE + TOOLBAR_BUTTON_TEXT;
                    }
                    else
                    {
                        ItemCss += SPACE + TOOLBAR_BUTTON_ALIGN;
                    }

                    if (!string.IsNullOrEmpty(Item.PrefixIcon) || !string.IsNullOrEmpty(Item.SuffixIcon))
                    {
                        if ((!string.IsNullOrEmpty(Item.PrefixIcon) && !string.IsNullOrEmpty(Item.SuffixIcon)) || !string.IsNullOrEmpty(Item.PrefixIcon))
                        {
                            ButtonIconCss = Item.PrefixIcon + SPACE + ICONS;
                            ButtonIconPosition = IconPosition.Left;
                        }
                        else
                        {
                            ButtonIconCss = Item.SuffixIcon + SPACE + ICONS;
                            ButtonIconPosition = IconPosition.Right;
                        }
                    }
                }
                else if (Item.Type == ItemType.Separator)
                {
                    ItemCss += SPACE + SEPARATOR;
                }

                SetItemCss();
                if (!string.IsNullOrEmpty(ButtonIconCss) && string.IsNullOrEmpty(Item.Text))
                {
                    ButtonCss = SfBaseUtils.AddClass(ButtonCss, ICON_BUTTON);
                }

                if (!string.IsNullOrEmpty(Item.TooltipText))
                {
                    itemAttributes.Clear();
                    itemAttributes.Add(TITLE, Item.TooltipText);
                }

                buttonAttributes.Clear();
                buttonAttributes.Add(TYPE, BUTTON);
                buttonAttributes.Add(TAB_INDEX, NEGATIVE_VALUE);
                buttonAttributes.Add(STYLE, WIDTH + @Item.Width);
                if (!string.IsNullOrEmpty(Item.Text))
                {
                    buttonAttributes.Add(ARIA_LABEL, @Item.Text);
                }
                else if (!string.IsNullOrEmpty(Item.TooltipText))
                {
                    buttonAttributes.Add(ARIA_LABEL, @Item.TooltipText);
                }
            }
        }

        private void SetItemCss()
        {
            if (Item.ShowTextOn == DisplayMode.Toolbar)
            {
                ItemCss += SPACE + TOOLBAR_TEXT + SPACE + TOOLBAR_BUTTON_ALIGN;
            }
            else if (Item.ShowTextOn == DisplayMode.Overflow)
            {
                ItemCss += SPACE + POPUP_TEXT;
            }

            if (Item.Overflow == OverflowOption.Show)
            {
                ItemCss += SPACE + OVERFLOW_SHOW;
            }
            else if (Item.Overflow == OverflowOption.Hide && Item.Type != ItemType.Separator)
            {
                ItemCss += SPACE + OVERFLOW_HIDE;
            }

            if (Item.Overflow != OverflowOption.Show && Item.ShowAlwaysInPopup && Item.Type != ItemType.Separator)
            {
                ItemCss += SPACE + POPUP_ALONE;
            }

            if (Item.Disabled)
            {
                ItemCss += SPACE + OVERLAY;
            }

            if (!Item.Visible)
            {
                ItemCss += SPACE + HIDDEN;
            }

            if (Item.CssClass != null)
            {
                ItemCss = ItemCss + SPACE + Item.CssClass;
            }
        }

        /// <summary>
        /// Disposes unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose unmanaged resources in the Syncfusion Blazor toolbar component.
        /// </summary>
        /// <param name="disposing">Boolean value to dispose the object.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                buttonAttributes = null;
                itemAttributes = null;
            }
        }
    }

    /// <summary>
    /// A class that holds options to control the toolbar item clicked action.
    /// </summary>
    public class ToolbarEventArgs
    {
        /// <summary>
        /// Gets or sets the close icon.
        /// </summary>
        public bool IsCloseIcon { get; set; }

        /// <summary>
        /// Gets or sets the data index.
        /// </summary>
        public int? TargetParentDataIndex { get; set; }

        /// <summary>
        /// Gets or sets the toolbar item index.
        /// </summary>
        public int? ToolbarItemIndex { get; set; }

        /// <summary>
        /// Gets or sets the item from popup element.
        /// </summary>
        public bool IsPopupElement { get; set; }
    }
}