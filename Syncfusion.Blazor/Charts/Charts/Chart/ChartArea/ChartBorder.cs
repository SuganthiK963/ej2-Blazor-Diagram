using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the Border of the chart.
    /// </summary>
    public class ChartBorder : ChartDefaultBorder
    {
        /// <summary>
        /// The width of the border in pixels.
        /// </summary>
        [Parameter]
        public override double Width { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.ChartBorderRenderer.ChartBorder = this;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.ChartBorderRenderer.RendererShouldRender = true;
            Owner.ChartBorderRenderer.ProcessRenderQueue();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Owner = null;
            ChildContent = null;
        }
    }
}