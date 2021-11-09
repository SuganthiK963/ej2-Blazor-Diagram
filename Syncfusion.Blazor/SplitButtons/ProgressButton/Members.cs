using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Buttons;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// ProgressButton visualizes the progression of an operation to indicate the user that a process is happening in the background with visual representation.
    /// </summary>
    public partial class SfProgressButton : SfBaseComponent
    {
        /// <summary>
        /// Sets content for progress button element including HTML and its customizations.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the text content of the progress button element.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Specifies the root CSS class of the progress button that allows customization of component’s appearance.
        /// The progress button types, styles, and size can be achieved by using this property.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Enables or disables the progress button.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Specifies the duration of progression in the progress button.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 2000;

        /// <summary>
        /// Enables or disables the background filler UI in the progress button.
        /// </summary>
        [Parameter]
        public bool EnableProgress { get; set; }

        /// <summary>
        /// Enables or disables the Rtl support.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the progress button element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes; }
            set { htmlAttributes = value; }
        }

        /// <summary>
        /// Defines class/multiple classes separated by a space for the progress button that is used to include an icon.
        /// Progress button can also include font icon and sprite image.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Positions an icon in the progress button. The possible values are:
        ///  Left: The icon will be positioned to the left of the text content.
        ///  Right: The icon will be positioned to the right of the text content.
        ///  Top: The icon will be positioned at the top of the text content.
        ///  Bottom: The icon will be positioned at the bottom of the text content.
        /// </summary>
        [Parameter]
        public IconPosition IconPosition { get; set; }

        /// <summary>
        /// Allows the appearance of the progress button to be enhanced and visually appealing when set to true.
        /// </summary>
        [Parameter]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Makes the progress button toggle, when set to true. When you click it, the state changes from normal to active.
        /// </summary>
        [Parameter]
        public bool IsToggle { get; set; }
        internal void UpdateChildProperties(string key, object result)
        {
            if (key == "SpinSettings")
            {
                spinSettings = (ProgressButtonSpinSettings)result;
            }
            else
            {
                animationSettings = (ProgressButtonAnimationSettings)result;
            }
        }
    }
}
