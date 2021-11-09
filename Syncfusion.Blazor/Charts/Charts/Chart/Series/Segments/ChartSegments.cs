using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the collection of regions that helps to differentiate a line Type series.
    /// </summary>
    public class ChartSegments : ChartSubComponent
    {
        internal List<ChartSegment> Segments = new List<ChartSegment>();

        [CascadingParameter]
        private ChartSeries Series { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties(nameof(Segments), Segments);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        internal override void ComponentDispose()
        {
            Series = null;
            Segments = null;
        }
    }
}
