using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the ChartArea border of the chart.
    /// </summary>
    public class ChartAreaBorder : ChartDefaultBorder
    {
        internal ChartArea ChartArea { get; set; }

        /// <summary>
        /// The width of the border in pixels.
        /// </summary>
        [Parameter]
        public override double Width { get; set; } = 0.5;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ChartArea = (ChartArea)Tracker;
            ChartArea.UpdateBorder(this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.ChartAreaRenderer.RendererShouldRender = true;
            Owner.ChartAreaRenderer.ProcessRenderQueue();
        }
    }
}