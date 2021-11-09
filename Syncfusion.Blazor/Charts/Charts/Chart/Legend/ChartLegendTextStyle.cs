using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartLegendTextStyle : ChartDefaultFont
    {
        [CascadingParameter]
        internal SfChart Owner { get; set; }

        internal ChartLegendSettings ChartLegend { get; set; }

        [Parameter]
        public override string Size { get; set; } = "13px";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ChartLegend = (ChartLegendSettings)Tracker;
            ChartLegend.UpdateLegendProperties("TextStyle", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.LegendRenderer?.ProcessRenderQueue();
        }
    }
}
