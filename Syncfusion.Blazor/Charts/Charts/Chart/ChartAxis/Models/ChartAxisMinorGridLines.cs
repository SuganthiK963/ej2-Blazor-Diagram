using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxisMinorGridLines : ChartSubComponent
    {
        [CascadingParameter]
        private ChartAxis axis { get; set; }

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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            axis = (ChartAxis)Tracker;
            axis.UpdateAxisProperties("MinorGridLines", this);
        }
    }
}
