using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    public class ChartMarker: ChartCommonMarker, ISubcomponentTracker
    {
        private int pendingParametersSetCount;

        [CascadingParameter]
        internal ChartSeries Series { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties("Marker", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Series.UpdateSeriesProperties("Marker", this);
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
                Series.Container.SeriesContainer.Prerender();
            }
        }

    }
}
