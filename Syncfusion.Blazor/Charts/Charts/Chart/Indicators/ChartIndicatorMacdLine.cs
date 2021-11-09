using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the Macd line of the indicator.
    /// </summary>
    public class ChartIndicatorMacdLine : ChartSubComponent
    {
        [CascadingParameter]
        private ChartIndicator Indicator { get; set; }

        /// <summary>
        /// Defins the width of the Macd line of the indicator.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 2;

        /// <summary>
        /// Defins the color of the Macd line of the indicator.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#ff9933";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Indicator = (ChartIndicator)Tracker;
            Indicator.UpdateIndicatorProperties("MacdLine", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Indicator = null;
            ChildContent = null;
        }
    }
}