using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the Lower line of the indicator.
    /// </summary>
    public class ChartIndicatorLowerLine : ChartSubComponent
    {
        [CascadingParameter]
        private ChartIndicator Indicator { get; set; }

        /// <summary>
        /// Defins the width of the lower line of the indicator.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// Defins the color of the lower line of the indicator.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#f2ec2f";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Indicator = (ChartIndicator)Tracker;
            Indicator.UpdateIndicatorProperties("LowerLine", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Indicator = null;
            ChildContent = null;
        }
    }
}