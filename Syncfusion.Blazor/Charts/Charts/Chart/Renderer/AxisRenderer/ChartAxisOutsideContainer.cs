using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartAxisOutsideContainer : ChartRendererContainer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.AxisOutSideContainer = this;
        }

        protected override bool ShouldRender()
        {
            return RendererShouldRender || ContainerUpdate;
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder != null && Owner.AxisContainer != null)
            {
                int seq = 0;
                foreach (ChartAxisRenderer axisRenderer in Owner.AxisContainer.Renderers)
                {
                    builder.OpenComponent(seq++, typeof(ChartAxisOutsideRenderer));
                    builder.AddAttribute(seq++, "AxisRenderer", axisRenderer);
                    builder.CloseComponent();
                }
            }
        }
    }

    public class ChartAxisOutsideRenderer : ChartRenderer, IChartElementRenderer
    {
        [Parameter]
        public ChartAxisRenderer AxisRenderer { get; set; }

        protected override void OnInitialized()
        {
            Owner.AxisOutSideContainer.AddRenderer(this);
            SvgRenderer = Owner.SvgRenderer;
            AxisRenderer.OutSideRenderer = this;
        }

        public void InvalidateRender()
        {
        }

        public void HandleLayoutChange()
        {
        }

        public override void ProcessRenderQueue()
        {
            RendererShouldRender = true;
            base.ProcessRenderQueue();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (builder != null)
            {
                if (Owner.AxisContainer.AxisLayout is CartesianAxisLayout)
                {
                    RenderCartesianAxisOutsideCollection(builder);
                }
                else
                {
                    RenderPolarRadarAxisOutsideCollection(builder);
                }

                RendererShouldRender = false;
            }
        }

        private void RenderPolarRadarAxisOutsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer.OpenGroupElement(builder, Owner.ID + "AxisCollection" + 0);
            foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderer.AxisRenderInfo.AxisGridOptions)
            {
                if (keyValue.Key.Equals(Constants.MAJORTICKLINE, StringComparison.Ordinal))
                {
                    AxisRenderer.DrawLine(builder, keyValue.Value);
                }

                if (keyValue.Key.Equals(Constants.MINORTICKLINE, StringComparison.Ordinal))
                {
                    AxisRenderer.DrawLine(builder, keyValue.Value);
                }
            }

            if (AxisRenderer.AxisRenderInfo.AxisLine != null)
            {
                SvgRenderer.RenderPath(builder, AxisRenderer.AxisRenderInfo.AxisLine);
            }

            SvgRenderer.OpenGroupElement(builder, Owner.ID + "AxisLabels" + 0);
            foreach (TextOptions option in AxisRenderer.AxisRenderInfo.AxisLabelOptions)
            {
                ChartHelper.TextElement(builder, SvgRenderer, option);
            }

            builder.CloseElement();
            builder.CloseElement();
        }

        private void RenderCartesianAxisOutsideCollection(RenderTreeBuilder builder)
        {
            SvgRenderer.OpenGroupElement(builder, Owner.ID + "AxisGroup" + AxisRenderer.Index + "Outside");
            if (AxisRenderer.AxisRenderInfo.AxisLine != null && AxisRenderer.IsAxisInside)
            {
                AxisRenderer.DrawLine(builder, AxisRenderer.AxisRenderInfo.AxisLine);
            }

            if (AxisRenderer.IsTickInside)
            {
                foreach (KeyValuePair<string, List<PathOptions>> keyValue in AxisRenderer.AxisRenderInfo.AxisGridOptions)
                {
                    if (keyValue.Key.Equals(Constants.MAJORTICKLINE, StringComparison.Ordinal))
                    {
                        AxisRenderer.DrawLine(builder, keyValue.Value);
                    }

                    if (keyValue.Key.Equals(Constants.MINORTICKLINE, StringComparison.Ordinal))
                    {
                        AxisRenderer.DrawLine(builder, keyValue.Value);
                    }
                }
            }

            if (AxisRenderer.IsAxisLabelInside)
            {
                SvgRenderer.OpenGroupElement(builder, Owner.ID + "AxisLabels" + AxisRenderer.Index);
                foreach (TextOptions option in AxisRenderer.AxisRenderInfo.AxisLabelOptions)
                {
                    ChartHelper.TextElement(builder, SvgRenderer, option);
                }

                builder.CloseElement();
                if (AxisRenderer.AxisRenderInfo.AxisBorder != null)
                {
                    SvgRenderer.RenderPath(builder, AxisRenderer.AxisRenderInfo.AxisBorder, "pointer-events: none");
                }

                AxisRenderer.MultiLevelLabelRenderer?.RenderMultilevelLabel(builder);
            }

            if (AxisRenderer.AxisRenderInfo.AxisTitleOption != null && AxisRenderer.IsAxisInside)
            {
                ChartHelper.TextElement(builder, SvgRenderer, AxisRenderer.AxisRenderInfo.AxisTitleOption);
            }

            builder.CloseElement();
        }
    }
}
