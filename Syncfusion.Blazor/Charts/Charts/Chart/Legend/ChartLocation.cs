using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartLocation : ChartDefaultLocation
    {
        internal ChartLegendSettings ChartLegend { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ChartLegend = (ChartLegendSettings)Tracker;
            ChartLegend.UpdateLegendProperties("Location", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Chart.LegendRenderer?.ProcessRenderQueue();
        }
    }
}
