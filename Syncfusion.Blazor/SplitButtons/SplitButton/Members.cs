using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// SplitButton component has primary and secondary button. Primary button is used to select 
    /// default action and secondary button is used to toggle contextual overlays for displaying list of 
    /// action items. It can contain both text and images.
    /// </summary>
    public partial class SfSplitButton : SfBaseComponent
    {
        /// <summary>
        /// Sets content for primary button element including HTML and its customizations.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the text `Content` of the Button element and it will support only string type.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the SplitButton element. The SplitButton
        /// size and styles can be customized by using this.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the SplitButton is disabled or not.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space for the SplitButton that is used to include an
        /// icon. SplitButton can also include font icon and sprite image.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Positions the icon before/top of the text content in the SplitButton. The possible values are
        /// - Left: The icon will be positioned to the left of the text content.
        /// - Top: The icon will b  positioned to the top of the text content.
        /// </summary>
        [Parameter]
        public SplitButtonIconPosition IconPosition { get; set; }

        /// <summary>
        /// Specifies action items with its properties which will be rendered as SplitButton secondary button popup.
        /// </summary>
        [Parameter]
        public List<DropDownMenuItem> Items { get; set; }

        /// <summary>
        /// Allows to specify the custom popup content instead of Items.
        /// </summary>
        [Parameter]
        public RenderFragment PopupContent { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the primary button element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes; }
            set { htmlAttributes = value; }
        }
    }
}