using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of the marker in trendline.
    /// </summary>
    public class ChartTrendlineMarker : ChartCommonMarker
    {
        [CascadingParameter]
        private ChartTrendline Trendline { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Trendline = (ChartTrendline)Tracker;
            Trendline.UpdateTrendlineProperty("Marker", this);
            Trendline.TrendlineInitiator.UpdateTrendlineMarker();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Trendline.UpdateTrendlineProperty("Marker", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Trendline = null;
        }
    }
}
