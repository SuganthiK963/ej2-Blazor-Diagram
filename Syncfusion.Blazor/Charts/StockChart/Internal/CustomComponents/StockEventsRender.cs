using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class StockEventsRender: OwningComponentBase
    {
        private bool shouldRender = true;
        [CascadingParameter]
        private SfStockChart stockChart { get; set; }

        internal void InvalidateRenderer()
        {
            StateHasChanged();
        }
        internal void UpdateRenderer()
        {
            shouldRender = true;
            InvalidateRenderer();
        }
        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (stockChart.ChartSettings != null && stockChart.ChartSettings.SeriesContainer != null)
            {
                stockChart.CalculateStockEvents(builder);
                shouldRender = false;
            }
        }
    }
}
