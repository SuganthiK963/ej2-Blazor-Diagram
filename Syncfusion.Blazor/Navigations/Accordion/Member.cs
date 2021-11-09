using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Accordion is a vertically collapsible panel that displays one or more panels at a time.
    /// </summary>
    public partial class SfAccordion : SfBaseComponent
    {
        /// <summary>
        /// Sets ID attribute for the accordion element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Child Content for Accordion.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the accordion items.
        /// </summary>
        public List<AccordionItem> Items { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable content of all the accordion are rendered on the initial load and maintained in the DOM.
        /// </summary>
        [Parameter]
        public bool LoadOnDemand { get; set; } = true;

        /// <summary>
        /// Enable or disable rendering component in the right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the options to expand a single or multiple panels at a time.
        /// The possible values are:
        /// - Single: Sets to expand only one Accordion item at a time.
        /// - Multiple: Sets to expand more than one Accordion item at a time.
        /// </summary>
        [Parameter]
        public ExpandMode ExpandMode { get; set; } = ExpandMode.Multiple;

        /// <summary>
        /// Specifies the index of items that is expanded on the initial load.
        /// </summary>
        [Parameter]
        public int[] ExpandedIndices { get; set; }

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<int[]> ExpandedIndicesChanged { get; set; }

        /// <summary>
        /// Specifies the height of the Accordion that can be represented in pixels/percentage.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// Specifies the width of the Accordion that can be represented as pixels/percentage.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <exclude/>
        /// <summary>
        /// Gets or sets the html attributes.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; }
        
        internal AccordionAnimationSettings AnimationSettings { get; set; }

        internal bool IsExpandIndicesChanged { get; set; }

        internal void UpdateItemProperties(List<AccordionItem> items)
        {
            Items = items;
        }

        internal void UpdateAnimationProperties(AccordionAnimationSettings animationSettings)
        {
            var animation = animationSettings;
            if (animation == null)
            {
                animation = new AccordionAnimationSettings();
                animation.UpdateExpandProperties(animation.Expand);
                animation.UpdateCollapseProperties(animation.Collapse);
            }

            AnimationSettings = animation;
        }
    }
}