using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Partial Class SfSidebar.
    /// </summary>
    public partial class SfSidebar : SfBaseComponent
    {
        /// <summary>
        /// Sets id attribute for the sidebar element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Specifies the child content.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Enable or disable the animation transitions on expanding or collapsing the Sidebar.
        /// </summary>
        [Parameter]
        public bool Animate { get; set; } = true;

        /// <summary>
        /// Specifies whether the Sidebar needs to be closed or not when the document area is clicked.
        /// </summary>
        [Parameter]
        public bool CloseOnDocumentClick { get; set; }
        private bool SidebarCloseOnDocumentClick { get; set; }

        /// <summary>
        /// Specifies the size of the Sidebar in dock state. Dock size can be set in pixel values.
        /// </summary>
        [Parameter]
        public string DockSize { get; set; } = "auto";

        /// <summary>
        /// Specifies the docking state of the component.
        /// </summary>
        [Parameter]
        public bool EnableDock { get; set; }

        /// <summary>
        /// Enables the expand or collapse while swiping in the touch devices.
        /// </summary>
        [Parameter]
        public bool EnableGestures { get; set; } = true;

        /// <summary>
        /// Enable or disable the persisting component's state between page reloads. If enabled, isOpen state will be persisted.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering Sidebar in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets the Sidebar component is open or close.
        /// When the Sidebar type is set to `Auto`,
        /// the component will be expanded in the desktop and collapsed in the mobile mode regardless of the isOpen property.
        /// </summary>
        [Parameter]
        public bool IsOpen { get; set; }

        private bool SidebarIsOpen { get; set; }

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<bool> IsOpenChanged { get; set; }

        /// <summary>
        /// Specifies the media query string for resolution, when opens the Sidebar.
        /// Example: assigning media query value to '(min-width: 600px)' will open the sidebar component only when the provided resolution is met else the sidebar will be in closed state.
        /// </summary>
        [Parameter]
        public string MediaQuery { get; set; }

        /// <summary>
        /// Specifies the position of the Sidebar.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SidebarPosition Position { get; set; }
        private SidebarPosition SidebarPosition { get; set; }

        /// <summary>
        /// Specifies whether to apply overlay options to the main content or not when the Sidebar is in an open state.
        /// </summary>
        [Parameter]
        public bool ShowBackdrop { get; set; }
        private bool SliderShowBackdrop { get; set; }

        /// <summary>
        /// Allows to place the sidebar inside the target element.
        /// </summary>
        [Parameter]
        public string Target { get; set; }

        /// <summary>
        /// Specifies the expanding types of the Sidebar.
        /// `Over` - The sidebar floats over the main content area.
        /// `Push` - The sidebar pushes the main content area to appear side-by-side and shrinks the main content within the screen width.
        /// `Slide` - The sidebar translates the x and y positions of the main content area based on the sidebar width.
        /// The main content area will not be adjusted within the screen width.
        ///  `Auto` - Sidebar with `Over` type in mobile resolution and `Push` type in other higher resolutions.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SidebarType Type { get; set; } = SidebarType.Auto;
        private SidebarType SidebarType { get; set; }

        /// <summary>
        /// Specifies the width of the Sidebar. By default, the width of the Sidebar sets based on the size of its content.
        /// Width can also be set in pixel values.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "auto";
        private string SidebarWidth { get; set; }

        /// <summary>
        /// Specifies the z-index of the Sidebar. It is applicable only when sidebar act as the overlay type.
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [Obsolete("This property is deprecated.Use @attributes to set additional attributes for sidebar element.")]
        public Dictionary<string, object> HtmlAttributes { get { return SidebarHtmlAttributes; } set { SidebarHtmlAttributes = value; } }

        internal Dictionary<string, object> SidebarHtmlAttributes { get; set; }
    }
}