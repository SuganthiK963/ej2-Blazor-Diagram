using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class MultiColoredLineSeriesRenderer : MultiColoredBaseSeriesRenderer
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
            bool isInverted = Owner.RequireInvertedAxis;
            List<Point> visiblePoints = EnableComplexProperty();
            string startPoint = "M";
            Direction = new System.Text.StringBuilder();
            Point previous = null, previousPoint, nextPoint;
            segments = SortSegments(Series, Series.Segments);
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.Regions = new List<Rect>();
                previousPoint = i - 1 > -1 ? visiblePoints[i - 1] : null;
                nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(previousPoint, point, nextPoint, XAxisRenderer))
                {
                    if (previous != null)
                    {
                        GetLineDirection(previous.XValue, previous.YValue, point.XValue, point.YValue, isInverted, startPoint);
                        if (SetPointColor(point, previous, Series, Series.SegmentAxis == Segment.X, segments))
                        {
                            options.Add(new PathOptions(Owner.ID + "_Series_" + Series.Renderer.Index + "_Point_" + previous.Index, Direction.ToString(), Series.DashArray, Series.Width, SetPointColor(previous, Interior), Series.Opacity, "none"));
                            startPoint = "M";
                            Direction = new System.Text.StringBuilder();
                        }
                        else
                        {
                            startPoint = "L";
                        }
                    }
                    else
                    {
                        SetPointColor(point, null, Series, Series.SegmentAxis == Segment.X, segments);
                    }

                    previous = point;
                    StorePointLocation(point, Series, isInverted);
                }
                else
                {
                    previous = (Series.EmptyPointSettings.Mode == EmptyPointMode.Drop) ? previous : null;
                    startPoint = (Series.EmptyPointSettings.Mode == EmptyPointMode.Drop) ? startPoint : "M";
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }
            }

            if (!string.IsNullOrEmpty(Direction.ToString()))
            {
                options.Add(new PathOptions(Owner.ID + "_Series_" + Series.Renderer.Index, Direction.ToString(), Series.DashArray, Series.Width, SetPointColor(visiblePoints[count - 1], Interior), Series.Opacity, "none"));
            }

            return segments;
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
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
            if (options.Count != 0)
            {
                ApplySegmentAxis(builder, Series, options, segments);
            }

            builder.CloseElement();
        }
    }
}
