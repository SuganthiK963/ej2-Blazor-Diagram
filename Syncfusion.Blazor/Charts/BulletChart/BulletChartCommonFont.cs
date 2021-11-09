using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// BulletChartCommonFont Class.
    /// </summary>
    public class BulletChartCommonFont : SfBaseComponent
    {
        /// <summary>
        /// Sets and gets the color of the text.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Sets and gets to enable trim of the text.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool EnableTrim { get; set; } = true;

        /// <summary>
        /// Sets and gets the font family of the text.
        /// </summary>
        [Parameter]
        public string FontFamily { get; set; } = "Roboto-Regular";

        /// <summary>
        /// Sets and gets the font style of the text.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary>
        /// Sets and gets the font weight of the text.
        /// </summary>
        [Parameter]
        public string FontWeight { get; set; } = "Normal";

        /// <summary>
        /// Sets and gets the maximum label width of the text.
        /// </summary>
        [Parameter]
        public double MaximumTitleWidth { get; set; } = -1;

        /// <summary>
        /// Sets and gets the opacity of the text.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets the font size of the text.
        /// </summary>
        [Parameter]
        public string Size { get; set; } = "12px";

        /// <summary>
        /// Sets and gets the text alignment of the text.
        /// </summary>
        [Parameter]
        public Alignment TextAlignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Sets and gets the chart text overflow option.
        /// </summary>
        [Parameter]
        public TextOverflow TextOverflow { get; set; } = TextOverflow.None;

        /// <summary>
        /// Sets and gets the range color of the text.
        /// </summary>
        [Parameter]
        public bool EnableRangeColor { get; set; }
    }
}