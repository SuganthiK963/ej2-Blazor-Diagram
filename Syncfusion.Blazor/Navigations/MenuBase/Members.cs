using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations.Internal
{
    public partial class SfMenuBase<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Child content for menu.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the Menu container.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies menu items with its properties which will be rendered as ContextMenu.
        /// </summary>
        [Parameter]
        public List<TValue> Items { get; set; }

        /// <summary>
        /// Specifies whether to show the sub menu or not on click.
        /// When set to true, the sub menu will open only on mouse click.
        /// </summary>
        [Parameter]
        public bool ShowItemOnClick { get; set; }

        /// <summary>
        /// Specifies whether to enable / disable the scrollable option in Menu.
        /// </summary>
        [Parameter]
        public bool EnableScrolling { get; set; }
    }
}