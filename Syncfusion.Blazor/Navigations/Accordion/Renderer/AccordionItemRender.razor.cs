using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <summary>
    /// Specifies the accordion item renderer.
    /// </summary>
    public partial class AccordionItemRender: SfBaseComponent
    {
        private const string ITEM_SELECT = "e-acrdn-item e-select";
        private const string ITEMUNDERSCO = "acrdn_item_";
        private const string HEADERUNDERSCO = "acrdn_header_";
        private const string PANELUNDERSCO = "acrdn_panel_";
        private const string SPACE = " ";
        private const string EXPAND_STATE = "e-expand-state";
        private const string ARIA_DISABLED = "aria-disabled";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string TABINDEX = "tabindex";
        private const string ZERO = "0";
        private const string ARIA_CONTROLS = "aria-controls";
        private const string ARIA_LABELLEDBY = "aria-labelledby";
        private const string ARIA_LEVEL = "aria-level";
        private const string ACCORDION_PANEL = "e-acrdn-panel";
        private const string CONTENT_HIDE = "e-content-hide";
        private const string TGL_COLLAPSE_ICON = "e-tgl-collapse-icon";
        private const string ICONS = "e-icons";
        private const string ARIA_EXPANDED = "aria-expanded";
        private const string ARIA_SELECTED = "aria-selected";
        private const string ARIA_HIDDEN = "aria-hidden";
        private const string SELECTED = "e-selected";
        private const string SELECTED_ACTIVE = "e-selected e-active";
        private const string ACTIVE = "e-active";
        private const string EXPAND_ICON = "e-expand-icon";
        private const string HIDE = "e-hide";
        private const string OVERLAY = "e-overlay";
        private const string ACCORDIONHEADER = "e-acrdn-header";
        private const string HEADING = "heading";
        private const string ACCORDIONHEADERCONTENT = "e-acrdn-header-content";
        private const string TOGGLEICON = "e-toggle-icon";
        private const string DEFINITION = "definition";
        private const string ACCORDIONCONTENT = "e-acrdn-content";
        private const string ACCORDIONHEADERICON = "e-acrdn-header-icon";

        [CascadingParameter]
        private SfAccordion Parent { get; set; }

        private string ToggleCss { get; set; }

        private string ItemCss { get; set; } = ITEM_SELECT;

        private string ContentCss { get; set; }

        private string HeaderIconCss { get; set; }

        private IDictionary<string, object> ItemAttributes { get; set; } = new Dictionary<string, object>();

        private IDictionary<string, object> HeaderAttributes { get; set; } = new Dictionary<string, object>();

        private IDictionary<string, object> ContentAttributes { get; set; } = new Dictionary<string, object>();

        private string ItemId { get; set; } = SfBaseUtils.GenerateID(ITEMUNDERSCO);

        private string HeaderId { get; set; } = SfBaseUtils.GenerateID(HEADERUNDERSCO);

        private string ContentId { get; set; } = SfBaseUtils.GenerateID(PANELUNDERSCO);

        private string CssClass { get; set; }

        private bool? Disabled { get; set; }

        private string IconCss { get; set; }

        private bool? Visible { get; set; }

        private bool? IsExpandedFromIndex { get; set; }

        /// <summary>
        /// Gets or sets the accordion item.
        /// </summary>
        [Parameter]
        public AccordionItem Item { get; set; }

        private bool IsItemClick { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            SetInitialItem(Item);
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SetItemAttributes(Item);
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (IsItemClick)
            {
                IsItemClick = false;
                await Parent.AfterContentRender(HeaderElement);
            }
        }

        private void SetInitialItem(AccordionItem item)
        {
            if (item.Expanded || Parent.ExpandedItem.Contains(item))
            {
                item.IsContentRendered = true;
            }

            if ((Parent.ExpandedItem.Count == 1) && Parent.ExpandedItem[Parent.ExpandedItem.Count - 1] == item)
            {
                ItemCss = SfBaseUtils.AddClass(ItemCss, EXPAND_STATE);
            }

            HeaderAttributes.Add(ARIA_DISABLED, FALSE);
            HeaderAttributes.Add(TABINDEX, ZERO);
            HeaderAttributes.Add(ARIA_CONTROLS, ContentId);
            ContentAttributes.Add(ARIA_LABELLEDBY, HeaderId);
            SetItemAttributes(item);
        }

        private void SetItemAttributes(AccordionItem item)
        {
            if (Parent.Items != null)
            {
                HeaderAttributes[ARIA_LEVEL] = Parent.Items.Count;
            }

            if (item.Expanded)
            {
                item.IsContentRendered = true;
            }

            if (item.Expanded != item.IsExpanded || item.IsExpandedFromIndex != IsExpandedFromIndex)
            {
                item.IsExpanded = item.Expanded;
                IsExpandedFromIndex = item.IsExpandedFromIndex;
                if (!item.Expanded && !item.IsExpandedFromIndex)
                {
                    ContentCss = ACCORDION_PANEL + SPACE + CONTENT_HIDE;
                    ToggleCss = TGL_COLLAPSE_ICON + SPACE + ICONS;
                    ItemAttributes[ARIA_EXPANDED] = FALSE;
                    HeaderAttributes[ARIA_SELECTED] = FALSE;
                    ContentAttributes[ARIA_HIDDEN] = TRUE;
                }
                else
                {
                    if (!ItemCss.Contains(SPACE + SELECTED + SPACE + ACTIVE, StringComparison.CurrentCulture) && (!string.IsNullOrEmpty(item.Content) || item.ContentTemplate != null))
                    {
                        ItemCss = SfBaseUtils.AddClass(ItemCss, SELECTED_ACTIVE);
                    }

                    ContentCss = ACCORDION_PANEL;
                    ToggleCss = TGL_COLLAPSE_ICON + SPACE + ICONS + SPACE + EXPAND_ICON;
                    ItemAttributes[ARIA_EXPANDED] = TRUE;
                    HeaderAttributes[ARIA_SELECTED] = TRUE;
                    ContentAttributes[ARIA_HIDDEN] = FALSE;
                }
            }

            if (item.IconCss != IconCss)
            {
                IconCss = item.IconCss;
                HeaderIconCss = string.Empty;
                if (!string.IsNullOrEmpty(item.IconCss))
                {
                    HeaderIconCss = item.IconCss + SPACE + ICONS;
                }
            }

            if (item.Visible != Visible)
            {
                Visible = item.Visible;
                if (!item.Visible)
                {
                    ItemCss = SfBaseUtils.AddClass(ItemCss, HIDE);
                }
                else
                {
                    ItemCss = SfBaseUtils.RemoveClass(ItemCss, HIDE);
                }
            }

            if (item.Disabled != Disabled)
            {
                Disabled = item.Disabled;
                if (item.Disabled)
                {
                    ItemCss = SfBaseUtils.AddClass(ItemCss, OVERLAY);
                    HeaderAttributes[ARIA_DISABLED] = TRUE;
                    HeaderAttributes[ARIA_SELECTED] = FALSE;
                    HeaderAttributes[TABINDEX] = SPACE;
                }
                else
                {
                    ItemCss = SfBaseUtils.RemoveClass(ItemCss, OVERLAY);
                    HeaderAttributes[ARIA_DISABLED] = FALSE;
                    HeaderAttributes[ARIA_SELECTED] = (item.Expanded || item.IsExpandedFromIndex) ? TRUE : FALSE;
                    HeaderAttributes[TABINDEX] = ZERO;
                }
            }

            if (item.CssClass != CssClass)
            {
                if (!string.IsNullOrEmpty(CssClass))
                {
                    ItemCss = SfBaseUtils.RemoveClass(ItemCss, CssClass);
                }

                CssClass = item.CssClass;
                if (!string.IsNullOrEmpty(item.CssClass))
                {
                    ItemCss = SfBaseUtils.AddClass(ItemCss, item.CssClass);
                }
            }
        }

        private async Task ItemClickHandler(MouseEventArgs e)
        {
            IsItemClick = true;
            if (Item != null)
            {
                Item.IsContentRendered = true;
            }

            await Parent.TriggerClickedEvent(e, Item);
        }

        internal override void ComponentDispose()
        {
            ItemAttributes = null;
            HeaderAttributes = null;
            ContentAttributes = null;
            Parent = null;
        }
    }
}