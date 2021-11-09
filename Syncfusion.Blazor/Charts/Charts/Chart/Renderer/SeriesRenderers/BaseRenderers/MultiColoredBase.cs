using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class MultiColoredBaseSeriesRenderer : AreaBaseSeriesRenderer
    {
        private double MaxSegmentValue { get; set; } = double.NegativeInfinity;

        internal List<ChartSegment> SortSegments(ChartSeries series, List<ChartSegment> chartSegments)
        {
            ChartAxis axis = series.SegmentAxis == Segment.X ? XAxisRenderer.Axis : YAxisRenderer.Axis;
            chartSegments.Where((x) => !(axis.ValueType == ValueType.Category || axis.ValueType == ValueType.DateTimeCategory)).ToList().ForEach((a) =>
            {
                MaxSegmentValue = Math.Max(MaxSegmentValue, Convert.ToDouble((a.Value != null) ? (axis.ValueType == ValueType.Double) ? a.Value : ChartHelper.GetTime((DateTime)a.Value) : 0, Culture));
            });
            chartSegments.Sort(delegate (ChartSegment a, ChartSegment b)
            {
                return GetAxisValue(a.Value, axis).CompareTo(GetAxisValue(b.Value, axis));
            });
            return chartSegments;
        }

        private string CreateClipRect(RenderTreeBuilder builder, double startValue, double endValue, int index, bool isX)
        {
            bool isInverted = Owner.RequireInvertedAxis;
            string clipPathId = Owner.ID + "_ChartSegmentClipRect_" + index;
            ChartInternalLocation startPointLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(isX ? startValue : XAxisRenderer.VisibleRange.Start), YAxisRenderer.GetPointValue(isX ? YAxisRenderer.VisibleRange.End : endValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartInternalLocation endPointLocation = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(isX ? endValue : XAxisRenderer.VisibleRange.End), YAxisRenderer.GetPointValue(isX ? YAxisRenderer.VisibleRange.Start : startValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            if ((endPointLocation.X - startPointLocation.X > 0) && (endPointLocation.Y - startPointLocation.Y > 0))
            {
                Owner.SvgRenderer.OpenClipPath(builder, clipPathId);
                Owner.SvgRenderer.RenderRect(builder, new RectOptions(clipPathId + "_Rect", startPointLocation.X, startPointLocation.Y, endPointLocation.X - startPointLocation.X, endPointLocation.Y - startPointLocation.Y, 1, "Gray", Constants.TRANSPARENT, 0, 0, 1));
                builder.CloseElement();
                return "url(#" + clipPathId + ")";
            }

            return null;
        }

        private double GetAxisValue(object segmentValue, ChartAxis axis)
        {
            if (segmentValue == null && axis.ValueType != ValueType.DateTime)
            {
                segmentValue = (axis.ValueType == ValueType.Double) ? Math.Max(axis.Renderer.VisibleRange.End, MaxSegmentValue) : axis.Renderer.VisibleRange.End;
            }

            if (axis.ValueType == ValueType.DateTime)
            {
                return ChartHelper.GetTime(segmentValue != null ? (DateTime)segmentValue : new DateTime(1970, 1, 1).AddMilliseconds(Math.Max(axis.Renderer.VisibleRange.End, MaxSegmentValue)));
            }
            else if (axis.ValueType.ToString().Contains("Category", StringComparison.InvariantCulture))
            {
                string xValue;
                DateTime validDate;
                if (axis.ValueType == ValueType.DateTimeCategory)
                {
                    xValue = DateTime.TryParse(segmentValue.ToString(), out validDate) ? ChartHelper.GetTime(validDate).ToString(Culture) : segmentValue.ToString();
                }
                else
                {
                    xValue = segmentValue.ToString();
                }

                return (axis.Renderer.Labels.IndexOf(xValue) == -1) ? axis.Renderer.Labels.Count : axis.Renderer.Labels.IndexOf(xValue);
            }

            return Convert.ToDouble(segmentValue, null);
        }

        internal bool SetPointColor(Point currentPoint, Point previous, ChartSeries series, bool isXSegment, List<ChartSegment> segments)
        {
            if (string.IsNullOrEmpty(series.PointColorMapping))
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    if ((isXSegment ? currentPoint.XValue : currentPoint.YValue) <= GetAxisValue(segments[i].Value, isXSegment ? XAxisRenderer.Axis : YAxisRenderer.Axis) || segments[i].Value == null)
                    {
                        currentPoint.Interior = segments[i].Color;
                        break;
                    }
                }

                if (currentPoint.Interior == null)
                {
                    currentPoint.Interior = Interior;
                }

                return false;
            }
            else
            {
                if (previous != null)
                {
                    return SetPointColor(currentPoint, Interior) != SetPointColor(previous, Interior);
                }
                else
                {
                    return false;
                }
            }
        }

        internal void ApplySegmentAxis(RenderTreeBuilder builder, ChartSeries series, List<PathOptions> options, List<ChartSegment> segments)
        {
            if (!string.IsNullOrEmpty(series.PointColorMapping))
            {
                foreach (PathOptions option in options)
                {
                    AppendLinePath(builder, option);
                }

                return;
            }

            ChartAxis axis = series.SegmentAxis == Segment.X ? XAxisRenderer.Axis : YAxisRenderer.Axis;
            IncludeSegment(segments, axis, segments.Count);
            PathOptions attributeOptions = null;
            for (int index = 0; index < segments.Count; index++)
            {
                ChartSegment segment = segments[index];
                string clipPath = CreateClipRect(builder, index > 0 ? GetAxisValue(segments[index - 1].Value, axis) : axis.Renderer.VisibleRange.Start, GetAxisValue(segment.Value, axis), index, series.SegmentAxis == Segment.X);
                if (!string.IsNullOrEmpty(clipPath))
                {
                    foreach (PathOptions option in options)
                    {
                        attributeOptions = new PathOptions()
                        {
                            ClipPath = clipPath,
                            StrokeDashArray = segment.DashArray,
                            Opacity = option.Opacity,
                            Stroke = series.Type.ToString().Contains("Line", StringComparison.InvariantCulture) ? string.IsNullOrEmpty(segment.Color) ? Interior : segment.Color : series.Border.Color,
                            Fill = series.Type.ToString().Contains("Line", StringComparison.InvariantCulture) ? "none" : string.IsNullOrEmpty(segment.Color) ? Interior : segment.Color,
                            Id = option.Id + "_Segment_" + index,
                            Direction = option.Direction,
                            StrokeWidth = option.StrokeWidth
                        };
                    }

                    attributeOptions.Direction = ChartHelper.AppendPathElements(Owner, attributeOptions.Direction, attributeOptions.Id);
                    Owner.SvgRenderer.RenderPath(builder, attributeOptions);
                    DynamicOptions.PathId.Add(attributeOptions.Id);
                    DynamicOptions.CurrentDirection.Add(attributeOptions.Direction);
                }
            }
        }

        private void IncludeSegment(List<ChartSegment> segments, ChartAxis axis, int length)
        {
            if (length <= 0)
            {
                ChartSegment chartSegment = new ChartSegment();
                chartSegment.SetSegmentValue(axis.Renderer.VisibleRange.End, Interior);
                segments.Add(chartSegment);
                return;
            }

            if (GetAxisValue(segments[length - 1].Value, axis) < axis.Renderer.VisibleRange.End)
            {
                ChartSegment chartSegment = new ChartSegment();
                chartSegment.SetSegmentValue(axis.Renderer.VisibleRange.End, Interior);
                segments.Add(chartSegment);
            }
        }
    }
}