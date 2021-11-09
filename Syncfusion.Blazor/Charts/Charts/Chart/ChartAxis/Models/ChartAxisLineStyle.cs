using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxisLineStyle : ChartSubComponent
    {
        [CascadingParameter]
        private ChartAxis axis { get; set; }

        /// <summary>
        /// The color of the axis line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#b5b5b5";

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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            axis = (ChartAxis)Tracker;
            axis.UpdateAxisProperties("LineStyle", this);
        }
    }
}
