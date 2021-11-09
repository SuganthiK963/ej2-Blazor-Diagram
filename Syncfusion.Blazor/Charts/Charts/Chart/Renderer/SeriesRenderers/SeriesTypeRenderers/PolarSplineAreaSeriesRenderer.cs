
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarSplineAreaSeriesRenderer : PolarRadarSplineSeriesRenderer
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
                AnimationOptions = new AnimationOptions(name, AnimationType.PolarRadar);
            }
        }

        private void CalculateDirection()
        {
            Point firstPoint = null;
            Point point;
            Direction = new System.Text.StringBuilder();
            bool isInverted = Owner.RequireInvertedAxis;
            ChartInternalLocation pt1 = null, startPoint = null, startPoint1 = null;
            List<Point> visiblePoints = Series.Renderer.Points;
            double origin = Series.Renderer.Points[0].YValue;

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                point = visiblePoints[i];
                int previous = GetPreviousIndex(visiblePoints, point.Index - 1, Series), next = GetNextIndex(visiblePoints, point.Index - 1, Series);
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(previous > -1 ? visiblePoints[previous] : null, point, (next > -1) && next < visiblePoints.Count ? visiblePoints[next] : null, XAxisRenderer))
                {
                    if (firstPoint != null)
                    {
                        pt1 = ChartHelper.TransformToVisible(point.XValue, point.YValue, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                        Direction.Append(GetPolarSplineAreaDirection(DrawPoints[previous], pt1, isInverted, Series));
                    }
                    else
                    {
                        startPoint = ChartHelper.TransformToVisible(point.XValue, origin, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                        Direction.Append("M " + startPoint.X.ToString(Culture) + SPACE + startPoint.Y.ToString(Culture) + SPACE);
                        startPoint1 = ChartHelper.TransformToVisible(point.XValue, point.YValue, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
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

                if (((i + 1 < visiblePoints.Count && !visiblePoints[i + 1].Visible) || i == visiblePoints.Count - 1) && pt1 != null && startPoint != null)
                {
                    startPoint = ChartHelper.TransformToVisible(point.XValue, origin, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
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
