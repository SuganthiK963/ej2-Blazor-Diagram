using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartTrendlineContainer : ChartRendererContainer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.TrendlineContainer = this;
        }

        protected override void OnElementAdded(IChartElement element)
        {
            if(Owner.InitialRect != null)
            {
                StateHasChanged();
            }
        }

        protected override void OnElementRemoved(IChartElement element)
        {
            if (element != null && !IsDisposed)
            {
                RemoveRenderer((element as ChartTrendline).TargetSeries.Renderer);
                StateHasChanged();
            }
        }

        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            ChartSeriesRenderer trendLineSeries = renderer as ChartSeriesRenderer;
            if (trendLineSeries != null && element != null)
            {
                ChartTrendline trendLine = element as ChartTrendline;
                trendLine.Renderer = trendLineSeries;
                trendLineSeries.SourceIndex = trendLine.Parent.Series.Renderer.Index;
                trendLineSeries.Index = Renderers.IndexOf(renderer);
                trendLineSeries.Interior = !string.IsNullOrEmpty(trendLine.Fill) ? trendLine.Fill : "blue";
                trendLine.Renderer.TrendLineLegendVisibility = trendLine.Visible;
                Owner.VisibleSeriesRenderers.Add(trendLineSeries);
                if (Owner.InitialRect != null)
                {
                    AddToRenderQueue(renderer);
                    Owner.ProcessOnLayoutChange();
                }
            }
        }

        protected override void OnRendererRemoved(IChartElementRenderer renderer)
        {
            if (!IsDisposed)
            {
                Owner.VisibleSeriesRenderers.Remove(renderer as ChartSeriesRenderer);
                Owner.ProcessOnLayoutChange();
            }
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                renderer.HandleChartSizeChange(rect);
            }
        }

        internal void ProcessData()
        {
            foreach (ChartTrendline element in Elements)
            {
                element.TrendlineInitiator.InitDataSource();
            }
        }

        internal void AssignAxisToTrendline()
        {
            foreach (ChartTrendline trendline in Elements)
            {
                trendline.TrendlineInitiator.InitiateAxis();
            }
        }

        internal void PerformAnimation(List<InitialAnimationInfo> animationInfo)
        {
            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                ChartSeries series = renderer.Series;
                if (series.Visible && series.Animation.Enable)
                {
                    renderer.PerformInitialAnimation(animationInfo);
                }
            }
        }

        public override void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        internal void RemoveRenderer(ChartSeriesRenderer renderer)
        {
            Renderers.Remove(renderer);
            Owner.VisibleSeriesRenderers.Remove(renderer);
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            int seq = 0;
            foreach (ChartTrendline element in Elements)
            {
                if (element.TargetSeries.RendererType != null)
                {
                    builder.OpenComponent(seq++, element.TargetSeries.RendererType);
                    builder.AddAttribute(seq++, "Trendlineseries", element.TargetSeries);
                    builder.SetKey(element.RendererKey + "-" + element.TargetSeries.RendererKey);
                    builder.CloseComponent();
                    if (element.TargetSeries.Marker.Visible)
                    {
                        builder.OpenComponent(seq++, typeof(ChartMarkerRenderer));
                        builder.AddAttribute(seq++, "Series", element.TargetSeries);
                        builder.SetKey(element.RendererKey + "-" + element.TargetSeries.RendererKey);
                        builder.CloseComponent();
                    }
                }
                
            }
            RendererShouldRender = false;
        }
    }
}
