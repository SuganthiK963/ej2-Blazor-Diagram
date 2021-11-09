using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart axis line.
    /// </summary>
    public partial class StockChartCommonLineStyle : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartCommonAxis Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The color of the axis line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// The dash array of the axis line.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// The width of the line in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}