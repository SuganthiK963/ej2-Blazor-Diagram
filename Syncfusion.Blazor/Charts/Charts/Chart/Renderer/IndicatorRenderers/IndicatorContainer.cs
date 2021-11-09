using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartIndicatorContainer : ChartRendererContainer
    {
        private int seq;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.IndicatorContainer = this;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            foreach (ChartIndicator indicator in Elements)
            {
                UpdateClipRect(indicator.TargetSeries[0].Renderer.XAxisRenderer, indicator.TargetSeries[0].Renderer.YAxisRenderer, indicator.ClipRect);
            }

            foreach (ChartSeriesRenderer renderer in Renderers)
            {
                renderer.HandleChartSizeChange(rect);
            }
        }

        public override void AddRenderer(IChartElementRenderer renderer)
        {
            if (renderer != null && !Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Add(renderer);
                int index = Renderers.IndexOf(renderer);
                if (index <= Elements.Count - 1 && index != -1)
                {
                    OnRendererAdded(renderer, Elements[index]);
                }
                else
                {
                    string renderkey = (renderer as ChartSeriesRenderer).Series.RendererKey;
                    IChartElement element = FindIndicator(renderkey);
                    OnRendererAdded(renderer, element);
                }
            }
        }

        private IChartElement FindIndicator(string renderkey)
        {
            foreach (ChartIndicator indicator in Elements)
            {
                if (renderkey.IndexOf(indicator.RendererKey, System.StringComparison.InvariantCulture) > -1)
                {
                    return indicator;
                }
            }

            return null;
        }

        internal void AssignAxisToIndicator()
        {
            foreach (ChartIndicator indicator in Elements)
            {
                indicator.IndicatorRenderer.InitiateAxis();
            }
        }

        internal void UpdateClipRect(ChartAxisRenderer x_AxisRenderer, ChartAxisRenderer y_AxisRenderer, Rect clipRect)
        {
            RendererShouldRender = true;
            if (Owner.RequireInvertedAxis)
            {
                clipRect.X = y_AxisRenderer.Rect.X;
                clipRect.Y = x_AxisRenderer.Rect.Y;
                clipRect.Width = y_AxisRenderer.Rect.Width;
                clipRect.Height = x_AxisRenderer.Rect.Height;
            }
            else
            {
                clipRect.X = x_AxisRenderer.Rect.X;
                clipRect.Y = y_AxisRenderer.Rect.Y;
                clipRect.Width = x_AxisRenderer.Rect.Width;
                clipRect.Height = y_AxisRenderer.Rect.Height;
            }

            StateHasChanged();
        }

        internal void ProcessData()
        {
            foreach (ChartIndicator indicator in Elements)
            {
                indicator.IndicatorRenderer.InitDataSource();
            }
        }

        protected override void OnElementAdded(IChartElement element)
        {
            StateHasChanged();
        }

        protected override void OnElementRemoved(IChartElement element)
        {
            if (element != null)
            {
                foreach (ChartSeries series in (element as ChartIndicator).TargetSeries)
                {
                    Renderers.Remove(series.Renderer);
                }

                if (!Owner.ChartDisposed())
                {
                    StateHasChanged();
                }
            }
        }

        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            ChartSeriesRenderer indicatorSeries = renderer as ChartSeriesRenderer;
            if (indicatorSeries != null)
            {
               indicatorSeries.Index = Renderers.IndexOf(renderer);
               indicatorSeries.SourceIndex = Elements.IndexOf(element);
               Owner.VisibleSeriesRenderers.Add(indicatorSeries);
            }

            ChartIndicator chartIndicator = (ChartIndicator)element;
            if (Owner.InitialRect != null && element != null && chartIndicator.TargetSeries.Count > 0 && chartIndicator.TargetSeries[chartIndicator.TargetSeries.Count - 1].Renderer != null)
            {
                Owner.ProcessOnLayoutChange();
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

        private void CreateIndicatorElements(RenderTreeBuilder builder, ChartIndicator element)
        {
            int index = Elements.IndexOf(element);
            string clipPathId = Owner.ID + "_ChartIndicatorClipRect_" + index, 
            visibility = (element.Animation.Enable && Owner.ShouldAnimateSeries) ? "hidden" : "visible";
            RectOptions rectOption = new RectOptions(clipPathId + "_Rect", 0, 0, element.ClipRect.Width, element.ClipRect.Height, 1, "transparent", "transparent", 0, 0, 1, visibility);
            builder.OpenElement(seq++, "g");
            builder.AddAttribute(seq++, "id", Owner.ID + "IndicatorGroup" + index);
            builder.AddAttribute(seq++, "transform", "translate(" + element.ClipRect.X + "," + element.ClipRect.Y + ")");
            builder.AddAttribute(seq++, "clip-path", "url(#" + clipPathId + ")");
            builder.OpenElement(seq++, "defs");
            builder.OpenElement(seq++, "clipPath");
            builder.AddAttribute(seq++, "id", clipPathId);
            builder.OpenComponent<SvgRect>(seq++);
            builder.AddMultipleAttributes(seq++, Owner.SvgRenderer.GetOptions(rectOption));
            builder.CloseComponent();
            builder.CloseElement();
            builder.CloseElement();
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            seq = 0;
            builder.OpenElement(seq++, "g");
            builder.AddAttribute(seq++, "id", Owner.ID + "IndicatorCollection");
            foreach (ChartIndicator element in Elements)
            {
                CreateIndicatorElements(builder, element);
                foreach (ChartSeries series in element.TargetSeries)
                {
                    if (series.RendererType != null)
                    {
                        builder.OpenComponent(seq++, series.RendererType);
                        builder.AddAttribute(seq++, "Indicatorseries", series);
                        builder.SetKey(element.RendererKey + "-" + series.RendererKey);
                        builder.CloseComponent();
                    }
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        }
    }
}
