using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartLegendBorder : ChartDefaultBorder
    {
        internal ChartLegendSettings ChartLegend { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ChartLegend = (ChartLegendSettings)Tracker;
            ChartLegend.UpdateLegendProperties("Border", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.LegendRenderer?.ProcessRenderQueue();
        }
    }
}
