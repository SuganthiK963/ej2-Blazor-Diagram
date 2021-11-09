using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class MultiColoredAreaSeriesRenderer : MultiColoredBaseSeriesRenderer
    {
        private new readonly List<PathOptions> options = new List<PathOptions>();
        private List<ChartSegment> segments = new List<ChartSegment>();

        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private List<ChartSegment> CalculateDirection()
        {
            options.Clear();
            bool isInverted = Owner.RequireInvertedAxis, rendered = false;
            List<Point> visiblePoints = EnableComplexProperty();
            ChartInternalLocation startPoint = null;
            Direction = new System.Text.StringBuilder();
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, 0);
            Point previous = null, previousPoint, nextPoint;
            segments = SortSegments(Series, Series.Segments);
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                previousPoint = i - 1 > -1 ? visiblePoints[i - 1] : null;
                nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                rendered = false;
                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    GetAreaPathDirection(point.XValue, origin, Series, isInverted, startPoint, "M");
                    startPoint = startPoint != null ? startPoint : new ChartInternalLocation(point.XValue, origin);
                    ChartInternalLocation firstPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    if (previous != null && SetPointColor(point, previous, Series, Series.SegmentAxis == Segment.X, segments))
                    {
                        rendered = true;
                        ChartInternalLocation startRegion = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(startPoint.X), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE);
                        Direction.Append("L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + startRegion.Y.ToString(Culture) + SPACE);
                        GeneratePathOption(options, Series, previous, Direction.ToString(), "_Point_" + previous.Index);
                        Direction = new System.Text.StringBuilder();
                        Direction.Append("M" + SPACE + firstPoint.X.ToString(Culture) + SPACE + startRegion.Y.ToString(Culture) + SPACE + "L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE);
                    }
                    else
                    {
                        Direction.Append("L" + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE);
                        SetPointColor(point, null, Series, Series.SegmentAxis == Segment.X, segments);
                    }

                    if (i + 1 < visiblePoints.Count ? (visiblePoints[i + 1] != null && !visiblePoints[i + 1].Visible && Series.EmptyPointSettings.Mode != EmptyPointMode.Drop) : false)
                    {
                        GetAreaEmptyDirection(new ChartInternalLocation(point.XValue, origin), startPoint, Series, isInverted);
                        startPoint = null;
                    }

                    previous = point;
                    StorePointLocation(point, Series, isInverted);
                }
            }

            if (!rendered)
            {
                if (count > 1)
                {
                    GetAreaPathDirection(previous.XValue, origin, Series, isInverted, null, "L");
                }

                GeneratePathOption(options, Series, previous, Direction.ToString(), string.Empty);
            }

            return segments;
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            base.UpdateDirection();
        }

        private void GeneratePathOption(List<PathOptions> options, ChartSeries series, Point point, string direction, string id)
        {
            options.Add(new PathOptions(Owner.ID + "_Series_" + series.Renderer.Index + id, direction, series.DashArray, series.Border.Width, series.Border.Color, series.Opacity, SetPointColor(point, Interior)));
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            if (options.Count != 0)
            {
                ApplySegmentAxis(builder, Series, options, segments);
            }

            builder.CloseElement();
        }
    }
}
