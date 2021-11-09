using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Accordion is a vertically collapsible panel that displays one or more panels at a time.
    /// </summary>
    public partial class SfAccordion : SfBaseComponent
    {
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string ACCORDIONPREFIX = "accordion-";
        private const string ARIA_MULTISELECTABLE = "aria-multiselectable";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string ANIMATION = "animation";
        private const string ENABLE_PERSISTENCE = "enablePersistence";
        private const string EXPAND_MODE = "expandMode";
        private const string EXPANDED_INDICES = "expandedIndices";
        private const string CLASS = "class";
        private const string ACCORDION_CLICKED = "clicked";
        private const string ACCORDION_EXPANDING = "expanding";
        private const string ACCORDION_EXPANDED = "expanded";
        private const string ACCORDION_COLLAPSING = "collapsing";
        private const string ACCORDION_COLLAPSED = "collapsed";
        private const string STYLE_WIDTH = "width";
        private const string STYLE_HEIGHT = "height";
        private const string STYLE = "style";
        private Dictionary<string, object> rootAttributes = new Dictionary<string, object>();

        internal List<AccordionItem> ExpandedItem { get; set; }

        internal AccordionEvents Delegates { get; set; }

        internal bool IsItemChanged { get; set; }

        private string AccordionClass { get; set; } = "e-control e-accordion";

        private static AccordionItemModel GetItem(AccordionItem accordionItem)
        {
            AccordionItemModel item = new AccordionItemModel();
            if (accordionItem != null)
            {
                item.Id = accordionItem.Id;
                item.Content = accordionItem.Content;
                item.CssClass = accordionItem.CssClass;
                item.Disabled = accordionItem.Disabled;
                item.Expanded = accordionItem.Expanded;
                item.Header = accordionItem.Header;
                item.IconCss = accordionItem.IconCss;
                item.Visible = accordionItem.Visible;
            }

            return item;
        }

        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> accordionObj = new Dictionary<string, object>();
            accordionObj.Add(ANIMATION, AnimationSettings);
            accordionObj.Add(ENABLE_PERSISTENCE, EnablePersistence);
            accordionObj.Add(EXPAND_MODE, ExpandMode);
            accordionObj.Add(EXPANDED_INDICES, ExpandedIndices);
            return accordionObj;
        }

        private void SetItems()
        {
            int i = 0;
            ExpandedItem = new List<AccordionItem>();
            foreach (AccordionItem item in Items)
            {
                item.IsExpandedFromIndex = false;
                if (ExpandedIndices != null && ExpandedIndices.Contains(i))
                {
                    item.IsExpandedFromIndex = true;
                    item.IsContentRendered = true;
                }

                if ((item.Expanded || item.IsExpandedFromIndex) && (!string.IsNullOrEmpty(item.Content) || item.ContentTemplate != null))
                {
                    ExpandedItem.Add(item);
                }

                i++;
            }
        }

        private void UpdateLocalProperties()
        {
            ExpandedItem = new List<AccordionItem>();
            rootAttributes.Add(ARIA_MULTISELECTABLE, TRUE);
            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                AccordionClass += SPACE + RTL;
            }
            UpdateHtmlAttributes();
        }

        private void UpdateHtmlAttributes()
        {
            string style = $"{STYLE_WIDTH}: {Width};{STYLE_HEIGHT}: {Height};";
            SfBaseUtils.UpdateDictionary(STYLE, style, rootAttributes);
            if (HtmlAttributes != null)
            {
                foreach (var item in HtmlAttributes)
                {
                    if (item.Key == CLASS)
                    {
                        AccordionClass += SPACE + item.Value;
                    }
                    else if (item.Key == STYLE)
                    {
                        if (rootAttributes.ContainsKey(STYLE))
                        {
                            rootAttributes[item.Key] += item.Value.ToString();
                        }
                    }
                    else
                    {
                        SfBaseUtils.UpdateDictionary(item.Key, item.Value, rootAttributes);
                    }
                }
            }
        }

        internal async Task OnPropertyChangeHandler()
        {
            if (PropertyChanges.ContainsKey(nameof(Width)) || PropertyChanges.ContainsKey(nameof(Height)))
            {
                UpdateHtmlAttributes();
            }
            if (PropertyChanges.ContainsKey(nameof(EnableRtl)) || PropertyChanges.ContainsKey(nameof(ExpandMode)))
            {
                bool isRtlChanged = PropertyChanges.ContainsKey(nameof(EnableRtl));
                bool isExpandModeChanged = PropertyChanges.ContainsKey(nameof(ExpandMode));
                await InvokeMethod("sfBlazor.Accordion.setExpandModeAndRTL", Element, EnableRtl, ExpandMode, isRtlChanged, isExpandModeChanged);
            }

            if (PropertyChanges.ContainsKey(nameof(ExpandedIndices)) && !IsExpandIndicesChanged)
            {
                UpdateExpandedIndices();
            }

            IsExpandIndicesChanged = false;
        }

        private void UpdateExpandedIndices()
        {
            rootAttributes[ARIA_MULTISELECTABLE] = ExpandMode == ExpandMode.Single ? FALSE : TRUE;
            if (Items != null && Items.Count > 0)
            {
                SetItems();
            }

            StateHasChanged();
        }

        private async Task SetTaskYield()
        {
            IJSInProcessRuntime runtime = this.JSRuntime as IJSInProcessRuntime;
            if (runtime != null)
            {
                await Task.Yield();
            }
        }

        internal async Task TriggerClickedEvent(MouseEventArgs e, AccordionItem item)
        {
            AccordionClickArgs args = new AccordionClickArgs()
            {
                Name = ACCORDION_CLICKED,
                OriginalEvent = e,
            };
            args.Item = GetItem(item);
            await SfBaseUtils.InvokeEvent<AccordionClickArgs>(Delegates?.Clicked, args);
        }

        internal async Task AfterContentRender(ElementReference headerElement)
        {
            await InvokeMethod("sfBlazor.Accordion.afterContentRender", new object[] { Element, headerElement, AnimationSettings });
        }

        #region JSInterop methods
#pragma warning disable SA1604 // Element documentation should have summary
#pragma warning disable SA1611 // Element parameters should be documented
#pragma warning disable SA1615 // Element return value should be documented
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreatedEvent()
        {
            await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, null);
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnAccordionClick(int index)
        {
            if (Items != null && Items.Count > 0 && Items[index] != null)
            {
                Items[index].IsContentRendered = true;
            }

            StateHasChanged();
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerExpandingEvent(int? targetIndex)
        {
            ExpandEventArgs args = new ExpandEventArgs()
            {
                Name = ACCORDION_EXPANDING,
                Index = targetIndex.Value,
                IsExpanded = true,
                Cancel = false
            };
            AccordionItem item = null;
            if (Items != null && Items.Count > 0 && targetIndex != null && targetIndex >= 0 && targetIndex < Items.Count)
            {
                item = Items[targetIndex.Value];
            }

            args.Item = GetItem(item);
            await SfBaseUtils.InvokeEvent<ExpandEventArgs>(Delegates?.Expanding, args);
            if (!args.Cancel)
            {
                await InvokeMethod("sfBlazor.Accordion.expandingItem", new object[] { Element, args });
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerExpandedEvent(ExpandEventArgs args)
        {
            if (args != null)
            {
                ExpandedEventArgs expandedArgs = new ExpandedEventArgs()
                {
                    Name = ACCORDION_EXPANDED,
                    Index = args.Index,
                    IsExpanded = args.IsExpanded,
                    Item = args.Item
                };
                await SfBaseUtils.InvokeEvent<ExpandedEventArgs>(Delegates?.Expanded, expandedArgs);
                UpdateExpandedIndices(expandedArgs.Index, expandedArgs.IsExpanded);
            }
        }

        private async void UpdateExpandedIndices(int index, bool isExpanded)
        {
            if (ExpandedIndices != null && !ExpandedIndices.Contains(index))
            {
                IsExpandIndicesChanged = true;
                if (Items != null && Items.Count > index)
                {
                    Items[index].IsExpandedFromIndex = true;
                }

                List<int> indices = ExpandedIndices.ToList();
                indices.Add(index);
                ExpandedIndices = indices.ToArray();
                await SetTaskYield(); // To resolve animation lag issue in WASM application
                ExpandedIndices = expandedIndices = await SfBaseUtils.UpdateProperty(ExpandedIndices, expandedIndices, ExpandedIndicesChanged);
            }
            else
            {
                if (Items != null && Items.Count > index)
                {
                    await Items[index].UpdateExpandedValue(isExpanded);
                }
            }
        }

        private async void UpdateCollapsedIndices(int index, bool isExpanded)
        {
            if (ExpandedIndices != null)
            {
                IsExpandIndicesChanged = true;
                if (Items != null && Items.Count > index)
                {
                    Items[index].IsExpandedFromIndex = false;
                }

                ExpandedIndices = ExpandedIndices.Where(val => val != index).ToArray();
                await SetTaskYield(); // To resolve animation lag issue in WASM application
                ExpandedIndices = expandedIndices = await SfBaseUtils.UpdateProperty(ExpandedIndices, expandedIndices, ExpandedIndicesChanged);
            }
            else
            {
                if (Items != null && Items.Count > index)
                {
                    await Items[index].UpdateExpandedValue(isExpanded);
                }
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerCollapsingEvent(int? targetIndex)
        {
            CollapseEventArgs args = new CollapseEventArgs()
            {
                Name = ACCORDION_COLLAPSING,
                Index = targetIndex.Value,
                IsExpanded = false,
                Cancel = false
            };
            AccordionItem item = null;
            if ((Items != null && Items.Count > 0) && (targetIndex != null && targetIndex >= 0 && targetIndex < Items.Count))
            {
                item = Items[targetIndex.Value];
            }

            args.Item = GetItem(item);
            await SfBaseUtils.InvokeEvent<CollapseEventArgs>(Delegates?.Collapsing, args);
            if (!args.Cancel)
            {
                await InvokeMethod("sfBlazor.Accordion.collapsingItem", new object[] { Element, args });
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerCollapsedEvent(ExpandEventArgs args)
        {
            if (args != null)
            {
                CollapsedEventArgs collapsedArgs = new CollapsedEventArgs()
                {
                    Name = ACCORDION_COLLAPSED,
                    Index = args.Index,
                    IsExpanded = args.IsExpanded,
                    Item = args.Item
                };
                await SfBaseUtils.InvokeEvent<CollapsedEventArgs>(Delegates?.Collapsed, collapsedArgs);
                UpdateCollapsedIndices(collapsedArgs.Index, collapsedArgs.IsExpanded);
            }
        }
#pragma warning restore SA1604 // Element documentation should have summary
#pragma warning restore SA1611 // Element parameters should be documented
#pragma warning restore SA1615 // Element return value should be documented
        #endregion
    }
}