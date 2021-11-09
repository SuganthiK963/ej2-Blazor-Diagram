using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class Trendline_SplineSeriesRenderer : SplineBaseSeriesRenderer
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
            options = new PathOptions()
            {
                Id = SeriesID(),
                Fill = Constants.TRANSPARENT,
                StrokeWidth = Series.Width,
                Stroke = Series.Fill,
                Opacity = Series.Opacity,
                StrokeDashArray = Series.DashArray,
                Direction = Direction.ToString()
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            Point firstPoint = null;
            bool isInverted = Owner.RequireInvertedAxis;
            string startPoint = "M";
            Direction = new System.Text.StringBuilder();
            List<Point> points = Series.Renderer.Points;

            foreach (var point in points)
            {
                int previous = GetPreviousIndex(points, point.Index - 1, Series), next = GetNextIndex(points, point.Index - 1, Series);
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(previous > -1 ? points[previous] : null, point, (previous > -1) && (next < points.Count) ? points[next] : null, XAxisRenderer))
                {
                    if (firstPoint != null)
                    {
                        Direction.Append(GetSplineDirection(DrawPoints[previous], firstPoint, point, isInverted, Series, startPoint));
                        startPoint = "L";
                    }

                    firstPoint = point;
                    StorePointLocation(point, Series, isInverted);
                }
                else
                {
                    startPoint = "M";
                    firstPoint = null;
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
            if (ClipRect == null || builder == null)
            {
                return;
            }

            CreateTrendlineElement(builder);
            RendererShouldRender = false;
        }
    }
}
