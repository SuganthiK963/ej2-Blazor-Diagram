using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class Trendline_LineSeriesRenderer : LineBaseSeriesRenderer
    {
        [Parameter]
        public ChartSeries Trendlineseries { get; set; }

        protected override void OnInitialized()
        {
            InitSeriesRendererFields();
            Series = Trendlineseries;
            Series.Renderer = this;
            SvgRenderer = Owner.SvgRenderer;
            Owner.TrendlineContainer.AddRenderer(this);
            InitDynamicAnimationProperty();
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            SeriesRenderer();
            Series.Marker.Renderer?.HandleChartSizeChange(rect);
        }

        protected override string SeriesID()
        {
            return Owner.ID + "_Series_" + SourceIndex + "_TrendLine_" + Index;
        }

        internal override string ClipPathId()
        {
            return Owner.ID + "_ChartTrendlineClipRect_" + SourceIndex;
        }

        internal override string SeriesElementId()
        {
            return Owner.ID + "TrendlineSeriesGroup" + SourceIndex;
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            options = new PathOptions(SeriesID(), Direction.ToString(), Series.DashArray, Series.Width, Series.Fill, Series.Opacity);
            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        internal void CalculateDirection()
        {
            string startPoint = "M";
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            double prevPointX = double.NaN, prevPointY = double.NaN, pointX, pointY;
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange((i - 1 > -1) && (i - 1 < count) ? visiblePoints[i - 1] : null, point, i + 1 < count ? visiblePoints[i + 1] : null, XAxisRenderer))
                {
                    pointX = point.XValue;
                    pointY = point.YValue;
                    GetLineDirection(prevPointX, prevPointY, pointX, pointY, Owner.RequireInvertedAxis, startPoint);
                    startPoint = !double.IsNaN(prevPointX) ? "L" : startPoint;
                    prevPointX = pointX;
                    prevPointY = pointY;
                    StorePointLocation(point, Series, Owner.RequireInvertedAxis);
                }
                else
                {
                    prevPointX = prevPointY = double.NaN;
                    startPoint = "M";
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }
            }
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            options.Direction = Direction.ToString();
            base.UpdateDirection();
        }

        private void CreateTrendlineElement(RenderTreeBuilder builder)
        {
            Rect rect = new Rect { X = 0, Y = 0, Width = ClipRect.Width, Height = ClipRect.Height };
            SvgRenderer.OpenGroupElement(builder, SeriesElementId(), "translate(" + ClipRect.X.ToString(Culture) + "," + ClipRect.Y.ToString(Culture) + ")", "url(#" + ClipPathId() + ")");
            SvgRenderer.RenderClipPath(builder, ClipPathId(), rect, Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible");
            SvgRenderer.RenderPath(builder, options);
            builder.CloseElement();
        }

        internal override SeriesCategories Category()
        {
            return SeriesCategories.TrendLine;
        }

        public override void ProcessRenderQueue()
        {
            StateHasChanged();
            Series.Marker.Renderer?.ProcessRenderQueue();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !TrendLineLegendVisibility)
            {
                return;
            }

            CreateTrendlineElement(builder);
            RendererShouldRender = false;
        }
    }
}
