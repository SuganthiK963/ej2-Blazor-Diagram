using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart axis major tick lines.
    /// </summary>
    public partial class StockChartCommonMajorTickLines : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartCommonAxis Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The color of the major tick line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// The height of the ticks in pixels.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 5;

        /// <summary>
        /// The width of the tick lines in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
            ChildContent = null;
        }
    }
}