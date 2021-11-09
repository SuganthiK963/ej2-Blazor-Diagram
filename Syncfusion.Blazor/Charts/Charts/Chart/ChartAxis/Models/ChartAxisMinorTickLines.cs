using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxisMinorTickLines : ChartSubComponent
    {
        [CascadingParameter]
        private ChartAxis axis { get; set; }

        /// <summary>
        /// The color of the minor tick line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// The height of the ticks in pixels.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 5;

        /// <summary>
        /// The width of the tick line in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 0.7;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            axis = (ChartAxis)Tracker;
            axis.UpdateAxisProperties("MinorTickLines", this);
        }
    }
}
