using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to configure the Animation  for indicator.
    /// </summary>
    public class ChartIndicatorAnimation : ChartDefaultAnimation
    {
        [CascadingParameter]
        private ChartIndicator Indicator { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Indicator = (ChartIndicator)Tracker;
            Indicator.UpdateIndicatorProperties("Animation", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Indicator = null;
            ChildContent = null;
        }
    }
}