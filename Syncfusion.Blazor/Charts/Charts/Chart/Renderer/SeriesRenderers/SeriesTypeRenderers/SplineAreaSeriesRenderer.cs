using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class SplineAreaSeriesRenderer : SplineBaseSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            options = new PathOptions()
            {
                Id = name,
                Fill = Interior,
                StrokeWidth = Series.Border.Width,
                Stroke = Series.Border.Color,
                Opacity = Series.Opacity,
                StrokeDashArray = Series.DashArray,
                Direction = Series.Renderer.Points.Count > 1 && Direction.Length != 0 ? Direction.ToString() : string.Empty
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            Point firstPoint = null;
            Point point;
            bool isInverted = Owner.RequireInvertedAxis;
            ChartInternalLocation data = null, startPoint = null, startPoint1 = null;
            List<Point> points = Series.Renderer.Points;
            Direction = new System.Text.StringBuilder();
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, 0);
            for (int i = 0; i < points.Count; i++)
            {
                point = points[i];
                int previous = GetPreviousIndex(points, point.Index - 1, Series), next = GetNextIndex(points, point.Index - 1, Series);
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(previous > -1 ? points[previous] : null, point, (next > -1) && next < points.Count ? points[next] : null, XAxisRenderer))
                {
                    if (firstPoint != null)
                    {
                        data = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append(GetSplineAreaDirection(DrawPoints[previous], data, isInverted, Series));
                    }
                    else
                    {
                        startPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("M " + startPoint.X.ToString(Culture) + SPACE + startPoint.Y.ToString(Culture) + SPACE);
                        startPoint1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L " + startPoint1.X.ToString(Culture) + SPACE + startPoint1.Y.ToString(Culture) + SPACE);
                    }

                    firstPoint = point;
                    StorePointLocation(point, Series, isInverted);
                }
                else
                {
                    firstPoint = null;
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }

                if (((i + 1 < points.Count && !points[i + 1].Visible) || i == points.Count - 1) && data != null && startPoint != null)
                {
                    startPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    Direction.Append("L " + startPoint.X.ToString(Culture) + SPACE + startPoint.Y.ToString(Culture));
                }
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
