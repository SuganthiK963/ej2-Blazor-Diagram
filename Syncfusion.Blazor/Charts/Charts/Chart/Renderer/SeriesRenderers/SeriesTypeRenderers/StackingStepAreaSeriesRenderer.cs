using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;
using System.Text;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class StackingStepAreaSeriesRenderer : LineBaseSeriesRenderer
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
                Direction = Direction.ToString()
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            bool isInverted = Owner.RequireInvertedAxis;
            Direction = new System.Text.StringBuilder();
            if (!Series.Visible)
            {
                return;
            }

            ChartInternalLocation currentPoint, secondPoint, start = null, point2 = null;
            StackValues stackedvalue = Series.Renderer.StackedValues;
            List<Point> visiblePoint = EnableComplexProperty();
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, stackedvalue.StartValues[0]);
            int pointsLength = visiblePoint.Count, pointIndex, startPoint = 0;
            Point nextPoint, prevPoint = null;
            double x_Value;
            double lineLength = 0;

            if (XAxisRenderer.Axis.ValueType == ValueType.Category && XAxisRenderer.Axis.LabelPlacement == LabelPlacement.BetweenTicks)
            {
                lineLength = 0.5;
            }

            for (int i = 0; i < pointsLength; i++)
            {
                Point point = visiblePoint[i];
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                x_Value = point.XValue;
                pointIndex = point.Index;
                nextPoint = i + 1 < visiblePoint.Count ? visiblePoint[i + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoint[i - 1] : null, point, nextPoint, XAxisRenderer))
                {
                    if (start == null)
                    {
                        start = new ChartInternalLocation(x_Value, 0);
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(x_Value - lineLength), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("M" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(stackedvalue.EndValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }

                    if (prevPoint != null)
                    {
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(prevPoint.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[prevPoint.Index]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + secondPoint.Y.ToString(Culture) + " L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }
                    else if (Series.EmptyPointSettings.Mode == EmptyPointMode.Gap)
                    {
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }

                    point.SymbolLocations.Add(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted));
                    point.Regions.Add(new Rect(point.SymbolLocations[0].X - Series.Marker.Width, point.SymbolLocations[0].Y - Series.Marker.Height, 2 * Series.Marker.Width, 2 * Series.Marker.Height));
                    prevPoint = point;
                }

                if ((nextPoint != null ? !nextPoint.Visible : false) && (Series.EmptyPointSettings.Mode != EmptyPointMode.Drop))
                {
                    for (int j = i; j >= startPoint; j--)
                    {
                        pointIndex = visiblePoint[j].Index;
                        int previousPointIndex = j == 0 ? 0 : visiblePoint[j - 1].Index;
                        if (j != 0 && (stackedvalue.StartValues[pointIndex] < stackedvalue.StartValues[previousPointIndex] || stackedvalue.StartValues[pointIndex] > stackedvalue.StartValues[previousPointIndex]))
                        {
                            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                            Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[previousPointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        }
                        else
                        {
                            currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[pointIndex].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        }

                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }

                    startPoint = i + 1;
                    start = null;
                    prevPoint = null;
                }
            }

            if (Direction.Length > 0)
            {
                if (pointsLength > 1)
                {
                    pointIndex = visiblePoint[pointsLength - 1].Index;
                    start = new ChartInternalLocation(visiblePoint[pointsLength - 1].XValue + lineLength, stackedvalue.EndValues[pointIndex]);
                    secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(start.X), YAxisRenderer.GetPointValue(start.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    Direction.Append("L" + SPACE + secondPoint.X.ToString(Culture) + SPACE + secondPoint.Y.ToString(Culture) + SPACE);
                    start = new ChartInternalLocation(visiblePoint[pointsLength - 1].XValue + lineLength, stackedvalue.StartValues[pointIndex]);
                    secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(start.X), YAxisRenderer.GetPointValue(start.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    Direction.Append("L" + SPACE + secondPoint.X.ToString(Culture) + SPACE + secondPoint.Y.ToString(Culture) + SPACE);
                }

                for (int j = pointsLength - 1; j >= startPoint; j--)
                {
                    int index = 0;
                    if (visiblePoint[j].Visible)
                    {
                        pointIndex = visiblePoint[j].Index;
                        point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[j].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
                    }

                    if (j != 0 && (j != 0) ? !visiblePoint[j - 1].Visible : false)
                    {
                        index = GetNextVisiblePointIndex(visiblePoint, j);
                    }

                    if (j != 0)
                    {
                        pointIndex = index != 0 ? visiblePoint[index].Index : visiblePoint[j - 1].Index;
                        Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoint[index != 0 ? index : j - 1].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[pointIndex]), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted).Y.ToString(Culture) + SPACE);
                    }
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

        private static int GetNextVisiblePointIndex(List<Point> points, int j)
        {
            for (int index = j - 1; index >= 0; index--)
            {
                if (!points[index].Visible)
                {
                    continue;
                }
                else
                {
                    return index;
                }
            }

            return 0;
        }
    }
}
