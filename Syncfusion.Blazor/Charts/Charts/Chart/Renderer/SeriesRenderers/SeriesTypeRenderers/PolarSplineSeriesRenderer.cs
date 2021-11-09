using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarSplineSeriesRenderer : PolarRadarSplineSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            options = new PathOptions()
            {
                Id = name,
                Fill = Constants.TRANSPARENT,
                StrokeWidth = Series.Width,
                Stroke = Interior,
                Opacity = Series.Opacity,
                StrokeDashArray = Series.DashArray,
                Direction = Direction.ToString()
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(name, AnimationType.PolarRadar);
            }
        }

        private void CalculateDirection()
        {
            Point firstPoint = null;
            bool isInverted = Owner.RequireInvertedAxis;
            string startPoint = "M";
            List<Point> points = Series.Renderer.Points;
            Direction = new System.Text.StringBuilder();
            foreach (var point in points)
            {
                int previous = GetPreviousIndex(points, point.Index - 1, Series), next = GetNextIndex(points, point.Index - 1, Series);
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(previous > -1 ? points[previous] : null, point, (previous > -1) && (next < points.Count) ? points[next] : null, XAxisRenderer))
                {
                    if (firstPoint != null)
                    {
                        Direction.Append(GetPolarSplineDirection(DrawPoints[previous], firstPoint, point, isInverted, Series, startPoint));
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

            if (points.Count > 0 && DrawPoints.Count > 0 && Series.IsClosed)
            {
                ConnectPoints connectPoints = GetFirstLastVisiblePoint(points);
                Direction.Append(GetPolarSplineDirection(DrawPoints[DrawPoints.Count - 1], connectPoints.Last, new Point { XValue = connectPoints.First.XValue, YValue = connectPoints.First.YValue }, isInverted, Series, startPoint));
                startPoint = "L";
            }
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            options.Direction = Direction.ToString();
            base.UpdateDirection();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElement(builder, options);
            builder.CloseElement();
        }
    }
}
