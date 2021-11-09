using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarColumnSeriesRenderer : PolarRadarColumnSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
               ColumnDrawTypeRender(Series, XAxisRenderer.Axis, YAxisRenderer.Axis);
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions = new List<PathOptions>();
            ColumnDrawTypeRender(Series, XAxisRenderer.Axis, YAxisRenderer.Axis);
            base.UpdateDirection();
        }

        public void ColumnDrawTypeRender(ChartSeries series, ChartAxis x_Axis, ChartAxis y_Axis)
        {
            double startAngle, endAngle, itemCurrentXPos, radius, inversedValue, pointStartAngle, pointEndAngle,
               x1, x2, y1, y2, startValue, endValue, innerRadius, d_StartX, d_StartY, d_EndX, d_EndY;
            double min = XAxisRenderer.ActualRange.Start;
            double centerX = (series.Renderer.ClipRect.Width / 2) + series.Renderer.ClipRect.X;
            double centerY = (series.Renderer.ClipRect.Height / 2) + series.Renderer.ClipRect.Y;
            string direction, arcValue;
            double sumofYValues = 0;
            double interval = (series.Renderer.Points.Count > 1 ? series.Renderer.Points[1].XValue : 2 * series.Renderer.Points[0].XValue) - series.Renderer.Points[0].XValue;
            double ticks = (x_Axis.ValueType == ValueType.Category && x_Axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0 : x_Axis.IsInversed ? -interval / 2 : interval / 2;
            double rangeInterval = x_Axis.ValueType == ValueType.DateTime ? XAxisRenderer.DateTimeInterval : 1;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            GetSeriesPosition();
            do
            {
                sumofYValues += rangeInterval;
                min += rangeInterval;
            }
            while (min <= XAxisRenderer.ActualRange.End - (x_Axis.ValueType == ValueType.Category ? 0 : 1));
            PathOptions option;
            foreach (Point point in series.Renderer.Points)
            {
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? series.Renderer.Points[point.Index - 1] : null, point, point.Index + 1 < series.Renderer.Points.Count ? series.Renderer.Points[point.Index + 1] : null, Series.Renderer.XAxisRenderer))
                {
                    inversedValue = x_Axis.IsInversed ? (XAxisRenderer.VisibleRange.End - point.XValue) : point.XValue - XAxisRenderer.VisibleRange.Start;
                    itemCurrentXPos = inversedValue + (((interval / series.Renderer.RectCount) * (x_Axis.IsInversed ? (series.Renderer.RectCount - 1 - series.Renderer.Position) : series.Renderer.Position)) - ticks) + (sumofYValues / 360 * x_Axis.StartAngle);
                    itemCurrentXPos = itemCurrentXPos / sumofYValues;
                    startAngle = 2 * Math.PI * (itemCurrentXPos + x_Axis.StartAngle);
                    endAngle = 2 * Math.PI * (itemCurrentXPos + x_Axis.StartAngle + ((interval / series.Renderer.RectCount) / sumofYValues));
                    if (startAngle == 0 && endAngle == 0)
                    {
                        endAngle = 2 * Math.PI;
                        arcValue = "1";
                    }
                    else
                    {
                        arcValue = "0";
                    }

                    pointStartAngle = startAngle;
                    pointEndAngle = endAngle;
                    startAngle = (startAngle - (0.5 * Math.PI)) + (series.ColumnSpacing / 2);
                    endAngle = ((endAngle - (0.5 * Math.PI)) - 0.000001) - (series.ColumnSpacing / 2);
                    if (series.DrawType == ChartDrawType.StackingColumn)
                    {
                        startValue = series.Renderer.StackedValues.StartValues[point.Index];
                        endValue = series.Renderer.StackedValues.EndValues[point.Index];
                        endValue = y_Axis.ValueType == ValueType.Logarithmic ? ChartHelper.LogBase(endValue == 0 ? 1 : endValue, y_Axis.LogBase) : endValue;
                        endValue = endValue > YAxisRenderer.ActualRange.End ? YAxisRenderer.ActualRange.End : endValue;
                    }
                    else
                    {
                        startValue = YAxisRenderer.VisibleRange.Start;
                        endValue = point.YValue > YAxisRenderer.ActualRange.End ? YAxisRenderer.ActualRange.End : point.YValue;
                    }

                    radius = startValue == endValue ? 0 : Series.Renderer.Owner.AxisContainer.AxisLayout.Radius * ChartHelper.ValueToCoefficient(endValue, YAxisRenderer);
                    x1 = centerX + (radius * Math.Cos(startAngle));
                    x2 = centerX + (radius * Math.Cos(endAngle));
                    y1 = centerY + (radius * Math.Sin(startAngle));
                    y2 = centerY + (radius * Math.Sin(endAngle));
                    innerRadius = Series.Renderer.Owner.AxisContainer.AxisLayout.Radius * ChartHelper.ValueToCoefficient(startValue == 0 && YAxisRenderer.VisibleRange.Start != 0 ? YAxisRenderer.VisibleRange.Start : startValue, YAxisRenderer);
                    d_StartX = centerX + (innerRadius * Math.Cos(startAngle));
                    d_StartY = centerY + (innerRadius * Math.Sin(startAngle));
                    d_EndX = centerX + (innerRadius * Math.Cos(endAngle));
                    d_EndY = centerY + (innerRadius * Math.Sin(endAngle));
                    if (series.Type == ChartSeriesType.Polar)
                    {
                        direction = "M" + SPACE + x1.ToString(Culture) + SPACE + y1.ToString(Culture) + SPACE + "A" + SPACE + radius.ToString(Culture) + SPACE + radius.ToString(Culture) + SPACE + "0" + SPACE + arcValue.ToString(Culture) + SPACE + 1 + SPACE + x2.ToString(Culture) + SPACE + y2.ToString(Culture) + SPACE + "L" + SPACE + d_EndX.ToString(Culture) + SPACE + d_EndY.ToString(Culture) + SPACE +
                            "A" + SPACE + innerRadius.ToString(Culture) + SPACE + innerRadius.ToString(Culture) + SPACE + "1" + SPACE + "0" + SPACE + "0" + SPACE + d_StartX.ToString(Culture) + SPACE + d_StartY.ToString(Culture) + SPACE + "Z";
                    }
                    else
                    {
                        direction = "M" + SPACE + x1.ToString(Culture) + SPACE + y1.ToString(Culture) + SPACE + "L" + SPACE + x2.ToString(Culture) + SPACE + y2.ToString(Culture) + SPACE + "L " + d_EndX.ToString(Culture) + SPACE + d_EndY.ToString(Culture) + SPACE + "L" + SPACE + d_StartX.ToString(Culture) + SPACE + d_StartY.ToString(Culture) + SPACE + "Z";
                    }

                    point.RegionData = new PolarArc(pointStartAngle, pointEndAngle, innerRadius, radius, itemCurrentXPos);
                    PointRenderEventArgs argsData = TriggerEvent(point, Interior, new BorderModel() { Width = Series.Border.Width, Color = (YAxisRenderer.VisibleRange.Start >= point.YValue) ? string.Empty : Series.Border.Color });
                    if (!argsData.Cancel)
                    {
                        if (series.Type == ChartSeriesType.Polar)
                        {
                            point.SymbolLocations.Add(new ChartInternalLocation(centerX + (radius * Math.Cos(startAngle + ((endAngle - startAngle) / 2))), centerY + (radius * Math.Sin(startAngle + ((endAngle - startAngle) / 2)))));
                        }
                        else
                        {
                            point.SymbolLocations.Add(new ChartInternalLocation((x1 + x2) / 2, (y1 + y2) / 2));
                        }

                        option = new PathOptions(Owner.ID + "_Series_" + series.Renderer.Index + "_Point_" + point.Index, direction, series.DashArray, argsData.Border.Width, argsData.Border.Color, series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText);
                        option.Visibility = visibility;
                        ColumnPathOptions.Add(option);
                    }
                }
            }
        }

        private void GetSeriesPosition()
        {
            List<ChartSeries> seriesCollection = new List<ChartSeries>();
            Dictionary<string, double> stackingGroup = new Dictionary<string, double>();
            RectPosition v_Series = new RectPosition { RectCount = 0, Position = double.NaN };
            foreach (ChartSeries series1 in Owner.SeriesContainer.Elements)
            {
                if (series1.Visible && (series1.Type == ChartSeriesType.Polar || series1.Type == ChartSeriesType.Radar) && series1.DrawType.ToString().Contains("Column", StringComparison.InvariantCulture))
                {
                    seriesCollection.Add(series1);
                }
            }

            for (int i = 0; i < seriesCollection.Count; i++)
            {
                ChartSeries series2 = seriesCollection[i];
                if (series2.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    if (!string.IsNullOrEmpty(series2.StackingGroup))
                    {
                        if (!stackingGroup.ContainsKey(series2.StackingGroup))
                        {
                            series2.Renderer.Position = v_Series.RectCount;
                            stackingGroup.Add(series2.StackingGroup, v_Series.RectCount++);
                        }
                        else
                        {
                            series2.Renderer.Position = stackingGroup[series2.StackingGroup];
                        }
                    }
                    else
                    {
                        if (double.IsNaN(v_Series.Position))
                        {
                            series2.Renderer.Position = v_Series.RectCount;
                            v_Series.Position = v_Series.RectCount++;
                        }
                        else
                        {
                            series2.Renderer.Position = v_Series.Position;
                        }
                    }
                }
                else
                {
                    series2.Renderer.Position = v_Series.RectCount++;
                }
            }

            for (int i = 0; i < seriesCollection.Count; i++)
            {
                seriesCollection[i].Renderer.RectCount = v_Series.RectCount;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in ColumnPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
    }
}
