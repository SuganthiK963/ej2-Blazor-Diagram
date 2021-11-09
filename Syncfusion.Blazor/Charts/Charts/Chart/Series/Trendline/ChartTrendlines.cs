using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the collection of trendlines that are used to predict the trend.
    /// </summary>
    public class ChartTrendlines : ChartSubComponent, ISubcomponentTracker
    {
        private int pendingParametersSetCount;

        [CascadingParameter]
        internal ChartSeries Series { get; set; }

        internal List<ChartTrendline> Trendlines { get; set; } = new List<ChartTrendline>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties(nameof(Trendlines), Trendlines);
        }

        void ISubcomponentTracker.PushSubcomponent()
        {
            pendingParametersSetCount++;
        }

        void ISubcomponentTracker.PopSubcomponent()
        {
            pendingParametersSetCount--;
            if (pendingParametersSetCount == 0)
            {
                Series.Container.TrendlineContainer.Prerender();
            }
        }
    }
}
