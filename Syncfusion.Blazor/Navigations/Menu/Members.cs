using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Menu is a graphical user interface that serve as navigation headers for your application.
    /// </summary>
    public partial class SfMenu<TValue> : SfMenuBase<TValue>
    {
        /// <summary>
        /// Specifies whether to enable / disable the hamburger mode in Menu.
        /// </summary>
        [Parameter]
        public bool HamburgerMode { get; set; }

        /// <summary>
        /// Specified the orientation of Menu whether it can be horizontal or vertical.
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Specifies target element to open/close Menu while click in Hamburger mode.
        /// </summary>
        [Parameter]
        public string Target { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the title text for hamburger mode in Menu.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = HEADERTITLE;
    }
}