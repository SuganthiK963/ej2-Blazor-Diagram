using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the upper line of the indicator.
    /// </summary>
    public class ChartIndicatorUpperLine : ChartSubComponent
    {
        [CascadingParameter]
        private ChartIndicator Indicator { get; set; }

        /// <summary>
        /// Specifies the width the upper line.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// Specifies the color of the upper line.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#ffb735";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Indicator = (ChartIndicator)Tracker;
            Indicator.UpdateIndicatorProperties("UpperLine", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Indicator = null;
            ChildContent = null;
        }
    }
}