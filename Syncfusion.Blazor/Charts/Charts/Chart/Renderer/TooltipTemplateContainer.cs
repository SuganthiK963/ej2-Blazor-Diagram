using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class TooltipContainer : OwningComponentBase
    {
        [CascadingParameter]
        internal SfChart Owner { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            foreach (ChartSeries series in Owner.SeriesContainer?.Elements)
            {
            }
        }
    }
}
