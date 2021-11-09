using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// DropDownButton component is used to toggle contextual overlays for displaying list of action items.
    /// It can contain both text and images.
    /// </summary>
    public partial class SfDropDownButton : SfBaseComponent
    {
        /// <summary>
        /// Sets content for button element including HTML and its customizations.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the text content of the button element and it will support only string type.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the DropDownButton element. The
        /// DropDownButton size and styles can be customized by using this.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space for the DropDownButton that is used to
        /// include an icon. DropDownButton can also include font icon and sprite image.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Positions the icon before/top of the text content in the DropDownButton. The possible values are:
        /// - Left: The icon will be positioned to the left of the text content.
        /// - Top: The icon will be positioned to the top of the text content.
        /// </summary>
        [Parameter]
        public SplitButtonIconPosition IconPosition { get; set; }

        /// <summary>
        /// Specifies action items with its properties which will be rendered in DropDownButton popup.
        /// </summary>
        [Parameter]
        public List<DropDownMenuItem> Items { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the DropDownButton is `Disabled` or not.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// To specify the custom popup content instead of Items.
        /// </summary>
        [Parameter]
        public RenderFragment PopupContent { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the dropdown button element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes; }
            set { htmlAttributes = value; }
        }
    }
}