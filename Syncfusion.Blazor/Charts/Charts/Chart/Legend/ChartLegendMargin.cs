using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartLegendMargin : ChartDefaultMargin
    {
        [CascadingParameter]
        internal SfChart Owner { get; set; }

        internal ChartLegendSettings ChartLegend { get; set; }

        [Parameter]
        public override double Left { get; set; }

        [Parameter]
        public override double Right { get; set; }

        [Parameter]
        public override double Top { get; set; }

        [Parameter]
        public override double Bottom { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ChartLegend = (ChartLegendSettings)Tracker;
            ChartLegend.UpdateLegendProperties("Margin", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.LegendRenderer?.ProcessRenderQueue();
        }
    }
}
