using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class CustomLegendRenderer: ChartRenderer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.CustomLegendRenderer = this;
        }

        protected override bool ShouldRender()
        {
            return RendererShouldRender;
        }
        public override void HandleChartSizeChange(Rect rect)
        {
            if (Owner.LegendRenderer?.availableRect != null)
            {
                RendererShouldRender = true;
            }
        }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            if (Owner.LegendRenderer != null && Owner.LegendRenderer.availableRect != null && Owner.LegendRenderer.LegendOptions.Count != 0 && Owner.LegendRenderer.Position == LegendPosition.Custom)
            {
                base.BuildRenderTree(builder);
                Owner.SvgRenderer.OpenGroupElement(builder, Owner.LegendRenderer.LegendID + "_g");
                Owner.LegendRenderer.RenderLegend(builder, Owner.SvgRenderer, Owner.LegendRenderer.LegendSettings.Border);
                builder.CloseElement();
            }

            RendererShouldRender = false;
        }
    }
}
