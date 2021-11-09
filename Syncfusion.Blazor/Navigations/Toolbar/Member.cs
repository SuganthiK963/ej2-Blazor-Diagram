using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The Toolbar control contains a group of commands that are aligned horizontally.
    /// </summary>
    public partial class SfToolbar : SfBaseComponent
    {
        private bool allowKeyboard;
        private string cssClass;
        private bool enableCollision;
        private bool enableRtl;
        private string height;
        private OverflowMode overflowMode;
        private int scrollStep;
        private string width;

        /// <summary>
        /// Sets ID attribute for toolbar element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Child Content for Toolbar.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// When this property is set to true, it allows the keyboard interaction in toolbar.
        /// </summary>
        [Parameter]
        public bool AllowKeyboard { get; set; } = true;

        /// <summary>
        /// Sets the CSS classes to root element of the Toolbar that helps to customize component styles.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable the popup collision.
        /// </summary>
        [Parameter]
        public bool EnableCollision { get; set; } = true;

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the height of the Toolbar in pixels/number/percentage. Number value is considered as pixels.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// A list of items that is used to configure Toolbar commands.
        /// </summary>
        [Parameter]
        public List<ToolbarItem> Items { get; set; }

        /// <summary>
        /// Specifies the Toolbar display mode when Toolbar content exceeds the viewing area.
        /// Possible modes are:
        /// - Scrollable: All the elements are displayed in a single line with horizontal scrolling enabled.
        /// - Popup: Prioritized elements are displayed on the Toolbar and the rest of elements are moved to the popup.
        /// - MultiRow: Displays the overflow toolbar items as an in-line of a toolbar.
        /// - Extended: Hide the overflowing toolbar items in the next row.  Show the overflowing toolbar items when you click the expand icons.
        /// If the popup content overflows the height of the page, the rest of the elements will be hidden.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OverflowMode OverflowMode { get; set; }

        /// <summary>
        /// Specifies the scrolling distance in scroller.
        /// </summary>
        [Parameter]
        public int ScrollStep { get; set; }

        /// <summary>
        /// Specifies the width of the Toolbar in pixels/numbers/percentage. Number value is considered as pixels.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "auto";

        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the toolbar element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        internal void UpdateChildProperties(List<ToolbarItem> toolbarItems)
        {
            Items = toolbarItems;
        }
    }
}