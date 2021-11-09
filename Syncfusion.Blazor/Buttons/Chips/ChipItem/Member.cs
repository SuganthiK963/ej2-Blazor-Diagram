using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Partial Class ChipItem.
    /// </summary>
    public partial class ChipItem : SfBaseComponent
    {
        /// <summary>
        /// Specifies the ChildContent.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the custom classes to be added to the chip element used to customize the Chip.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;
        private string ChipsCssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a value that indicates whether the chip component is enabled or not.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;
        private bool ChipsEnabled { get; set; }
        /// <summary>
        /// Specifies the leading icon CSS class for the chip.
        /// </summary>
        [Parameter]
        public string LeadingIconCss { get; set; } = string.Empty;
        private string ChipsLeadingIconCss { get; set; }

        /// <summary>
        /// Specifies the leading icon url for the chip.
        /// </summary>
        [Parameter]
        public string LeadingIconUrl { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the customized text value for the avatar in the chip.
        /// </summary>
        [Parameter]
        public string LeadingText { get; set; } = string.Empty;
        private string ChipsLeadingText { get; set; }

        /// <summary>
        /// Specifies the text content for the chip.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;
        private string ChipsText { get; set; }

        /// <summary>
        /// Specifies the trailing icon CSS class for the chip.
        /// </summary>
        [Parameter]
        public string TrailingIconCss { get; set; } = string.Empty;
        private string ChipsTrailingIconCss { get; set; }

        /// <summary>
        /// Specifies the leading icon url for the chip.
        /// </summary>
        [Parameter]
        public string TrailingIconUrl { get; set; } = string.Empty;

        /// <summary>
        /// This value property helps to store the chip component values.
        /// </summary>
        [Parameter]
        public string Value { get; set; } = string.Empty;
        private string ChipsValue { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as title to the each chip element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }
}