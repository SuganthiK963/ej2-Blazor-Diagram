using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Tab is a content panel to show multiple contents in a compact space. Also, only one tab is active at a time. Each Tab item has an associated content, that will be displayed based on the active Tab.
    /// </summary>
    public partial class SfTab : SfBaseComponent
    {
        private TabAnimationSettings animation;
        private string cssClass;
        private bool allowDragAndDrop;
        private bool enableRtl;
        private HeaderPosition headerPlacement;
        private string height;
        private List<TabItem> tabitems;
        private OverflowMode overflowMode;
        private int scrollStep;
        private int selectedItem;
        private bool showCloseButton;
        private string width;

        /// <summary>
        /// Sets ID attribute for the tab element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Child Content for Tab.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the animation settings of the Tabs component. The animation effect can be applied to activate the tab with duration and delay.
        /// </summary>
        [Parameter]
        public TabAnimationSettings Animation { get; set; }

        /// <summary>
        /// Sets the CSS classes to the root element of the Tabs that helps to customize the component styles.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// If enabled, the tab’s selected item will be persisted.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Indicates whether the Tab allows drag and drop over tab items. Tab item has been reordered based on the dropped item.
        /// </summary>
        [Parameter]
        public bool AllowDragAndDrop { get; set; }

        /// <summary>
        /// Defines the area in which the draggable element movement will be occurring. Outside that area will be restricted
        /// for the draggable element movement. By default, the draggable element movement occurs with Tabitems.
        /// </summary>
        [Parameter]
        public string DragArea { get; set; }

        /// <summary>
        /// Enable or disable rendering component in the right to left (RTL) direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the orientation of the Tab header.
        /// The possible values are:
        /// - Top: Places the Tab header on the top.
        /// - Bottom: Places the Tab header at the bottom.
        /// - Left: Places the Tab header at the left.
        /// - Right: Places the Tab header at the right.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HeaderPosition HeaderPlacement { get; set; }

        /// <summary>
        /// Specifies the height of the Tabs component. By default, Tab height is set based on the height of its parent.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// A list of items that are used to configure the Tabs component.
        /// </summary>
        [Parameter]
        public List<TabItem> Items { get; set; }

        /// <summary>
        /// Specifies the modes for the Tab content.
        /// The possible modes are:
        /// `Dynamic` Load the Tab content dynamically, which is rendering its content when switching its header.
        /// `Init` Loads all the tab contents on initial loading.
        /// `Demand` Loads the Tab content when required but keeps the content once it is rendered.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContentLoad LoadOn { get; set; }

        /// <summary>
        /// Overrides the global culture and localization value for this component. Default global culture is 'en-US'.
        /// </summary>
        [Parameter]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the display mode which will be applied when the Tabs exceeds the viewing area.
        /// The possible modes are:
        /// - Scrollable: All the elements will be displayed in a single line with horizontal scrolling enabled.
        /// - Popup: Tab container will hold the items that can be placed within the available space and the rest of the items will be moved to the popup.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OverflowMode OverflowMode { get; set; }

        /// <summary>
        /// Specifies the scrolling distance that applies when scrolling in Tab and enabled Scrollable mode.
        /// </summary>
        [Parameter]
        public int ScrollStep { get; set; }

        /// <summary>
        /// Specifies the index for activating the Tab item.
        /// </summary>
        [Parameter]
        public int SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<int> SelectedItemChanged { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether to show the close button in the Tab header or not.
        /// </summary>
        [Parameter]
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Specifies the width of the Tabs component. By default, Tab width sets based on the width of its parent.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the tab element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        internal void UpdateItemProperties(List<TabItem> item)
        {
            Items = tabitems = item;
        }

        internal void UpdateAnimationProperties(TabAnimationSettings animationSettings)
        {
            var animate = animationSettings;
            if (animationSettings == null)
            {
                animate = new TabAnimationSettings();
                animate.UpdateNextProperties(animate.Next);
                animate.UpdatePreviousProperties(animate.Previous);
            }

            Animation = animation = animate;
        }
    }
}