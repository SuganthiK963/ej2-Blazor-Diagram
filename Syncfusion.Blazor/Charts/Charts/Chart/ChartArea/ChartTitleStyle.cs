using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    ///  Options to customize the title style of the chart.
    /// </summary>
    public class ChartTitleStyle : ChartDefaultFont
    {
        [CascadingParameter]
        internal SfChart Owner { get; set; }

        [Parameter]
        public override string Size { get; set; } = "15px";

        [Parameter]
        public override string FontWeight { get; set; } = "500";

        [Parameter]
        public override string FontFamily { get; set; } = "Segoe UI";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.ChartTitleRenderer.TitleStyle = this;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Owner.ChartTitleRenderer.RendererShouldRender = true;
            Owner.ChartTitleRenderer.ProcessRenderQueue();
        }
    }
}