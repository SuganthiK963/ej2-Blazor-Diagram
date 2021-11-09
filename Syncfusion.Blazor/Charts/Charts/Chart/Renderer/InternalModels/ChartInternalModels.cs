using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the color and width of the border.
    /// </summary>
    public class ChartEventBorder
    {
        /// <summary>
        /// Sets and gets the color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the the width of the border in pixels.
        /// </summary>
        public double Width { get; set; }
    }

    /// <summary>
    /// Sets and gets the options for customizing the bottom, left, right, top margin of the chart component.
    /// </summary>
    public class ChartEventMargin
    {
        /// <summary>
        /// Sets and gets the bottom margin for the chart component.
        /// </summary>
        public double Bottom { get; set; }

        /// <summary>
        /// Sets and gets the left margin for the chart component.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Sets and gets the right margin for the chart component.
        /// </summary>
        public double Right { get; set; }

        /// <summary>
        /// Sets and gets the top margin for the chart component.
        /// </summary>
        public double Top { get; set; }
    }
}