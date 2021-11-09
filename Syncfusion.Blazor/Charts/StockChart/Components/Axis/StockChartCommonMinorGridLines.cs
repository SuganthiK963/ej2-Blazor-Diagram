using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart axis minor gridlines.
    /// </summary>
    public class StockChartCommonMinorGridLines : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartCommonAxis Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The color of the minor grid line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// The dash array of grid lines.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// The width of the line in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 0.7;

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}