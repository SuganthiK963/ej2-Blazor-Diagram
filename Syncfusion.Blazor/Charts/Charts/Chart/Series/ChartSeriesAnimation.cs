using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of the series animation.
    /// </summary>
    public class ChartSeriesAnimation : ChartDefaultAnimation
    {
        [CascadingParameter]
        private ChartSeries series { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            series = (ChartSeries)Tracker;
            series.UpdateSeriesProperties("Animation", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            series.UpdateSeriesProperties("Animation", this);
        }

        internal override void ComponentDispose()
        {
            Container = null;
            series = null;
        }
    }
}