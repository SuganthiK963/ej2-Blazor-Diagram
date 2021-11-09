using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of animation for the trendlines.
    /// </summary>
    public class ChartTrendlineAnimation : ChartDefaultAnimation
    {
        [CascadingParameter]
        private ChartTrendline Trendline { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Trendline = (ChartTrendline)Tracker;
            Trendline.UpdateTrendlineProperty("Animation", this);
            Trendline.TrendlineInitiator.UpdateTrendlineAnimation();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Trendline.UpdateTrendlineProperty("Animation", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Trendline = null;
        }
    }
}
