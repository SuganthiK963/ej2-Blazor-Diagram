using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarRadarAxisLayout : AxisLayout
    {
        private ChartPrimaryXAxis primaryXAxis;

        private ChartPrimaryYAxis primaryYAxis;

        internal double CenterX { get; set; }

        internal double CenterY { get; set; }

        internal double StartAngle { get; set; }

        internal List<Rect> VisibleAxisLabelRect { get; set; } = new List<Rect>();

        private static void MeasureDefinition(ChartRowRenderer definition, Size size)
        {
            foreach (ChartAxis axis in definition.Axes)
            {
                axis.Renderer.ComputeSize(size);
            }
        }

        private static void MeasureDefinition(ChartColumnRenderer definition, Size size)
        {
            foreach (ChartAxis axis in definition.Axes)
            {
                axis.Renderer.ComputeSize(size);
            }
        }

        private static PathOptions GetPathOptions(string id, string fill, double strokeWidth, string stroke, string direction, int opacity, string dashArray = null)
        {
            return new PathOptions
            {
                Id = id,
                Fill = fill,
                StrokeWidth = strokeWidth,
                Stroke = stroke,
                Direction = direction,
                Opacity = opacity,
                StrokeDashArray = dashArray
            };
        }

        internal override void AddAxis(ChartAxis axis)
        {
        }

        internal override void RemoveAxis(ChartAxis axis)
        {
        }

        internal override void ClearAxes()
        {
        }

        internal override void ComputePlotAreaBounds(Rect rect)
        {
            SeriesClipRect = new Rect(rect.X, rect.Y, rect.Width, rect.Height);

            // TODO: Need to make common
            primaryXAxis = (ChartPrimaryXAxis)Chart.AxisContainer.Axes[Constants.PRIMARYXAXIS];
            primaryYAxis = (ChartPrimaryYAxis)Chart.AxisContainer.Axes[Constants.PRIMARYYAXIS];
            IsPolar = (Chart.SeriesContainer.Renderers[0] as ChartSeriesRenderer).Series.Type == ChartSeriesType.Polar;
            MeasureRowAxis();
            MeasureColumnAxis();
            CalculateAxisSize();
        }

        private void MeasureRowAxis()
        {
            ChartRowRenderer renderer = Chart.RowContainer.Renderers[0] as ChartRowRenderer;
            CalculateRowSize(renderer);
            MeasureDefinition(renderer, new Size(Chart.AvailableSize.Width, renderer.ComputedHeight));
        }

        private void CalculateRowSize(ChartRowRenderer renderer)
        {
            renderer.ComputedHeight = SeriesClipRect.Height / 2;
            renderer.ComputedTop = SeriesClipRect.Y;
        }

        private void MeasureColumnAxis()
        {
            ChartColumnRenderer renderer = Chart.ColumnContainer.Renderers[0] as ChartColumnRenderer;
            CalculateColumnSize(renderer);
            MeasureDefinition(renderer, new Size(renderer.ComputedWidth, Chart.AvailableSize.Height));
        }

        private void CalculateColumnSize(ChartColumnRenderer renderer)
        {
            renderer.ComputedLeft = SeriesClipRect.X;
            renderer.ComputedWidth = SeriesClipRect.Width;
        }

        private void CalculateAxisSize()
        {
            CenterX = (SeriesClipRect.Width * 0.5) + SeriesClipRect.X;
            CenterY = (SeriesClipRect.Height * 0.5) + SeriesClipRect.Y;
            Radius = (Math.Min(SeriesClipRect.Width, SeriesClipRect.Height) / 2) - 5 - primaryXAxis.MajorTickLines.Height - primaryXAxis.Renderer.MaxLabelSize.Height;
            Radius = (primaryXAxis.Coefficient * Radius) / 100;
            SeriesClipRect.Y = CenterY - Radius;
            SeriesClipRect.X = CenterX - Radius;
            SeriesClipRect.Height = 2 * Radius;
            SeriesClipRect.Width = 2 * Radius;
            ChartRowRenderer rowRenderer = Chart.RowContainer.Renderers[0] as ChartRowRenderer;
            CalculateRowSize(rowRenderer);
            primaryYAxis.Renderer.Rect = SeriesClipRect;
            ChartColumnRenderer columnRenderer = Chart.ColumnContainer.Renderers[0] as ChartColumnRenderer;
            CalculateColumnSize(columnRenderer);
            primaryXAxis.Renderer.Rect = SeriesClipRect;
        }

        internal override void AxisRenderingCalculation(ChartAxisRenderer renderer)
        {
            StartAngle = ((ChartPrimaryXAxis)Chart.AxisContainer.Axes[Constants.PRIMARYXAXIS]).StartAngle;
            int i = renderer.Index;
            if (renderer.Orientation == Orientation.Horizontal)
            {
                if (renderer.Axis.MajorGridLines.Width > 0 || renderer.Axis.MajorTickLines.Width > 0)
                {
                    CalculateXAxisGridLine(renderer.Axis, i);
                }

                if (renderer.Axis.Visible /*&& renderer.Axis.InternalVisibility*/)
                {
                    CalculateXAxisLabels(renderer.Axis, i);
                }
            }
            else
            {
                CalculateYAxisGridLine(renderer.Axis, i);
                if (renderer.Axis.LineStyle.Width > 0 && renderer.VisibleLabels.Count > 0)
                {
                    CalculateYAxisLine(renderer.Axis, i);
                }

                if (renderer.Axis.Visible /*&& renderer.Axis.InternalVisibility*/ && renderer.VisibleLabels.Count > 0)
                {
                    CalculateYAxisLabels(renderer.Axis, i);
                }
            }
        }

        private static void LoadDictionaryValue(ChartAxisRenderer renderer, string key, PathOptions value)
        {
            if (!renderer.AxisRenderInfo.AxisGridOptions.ContainsKey(key))
            {
                renderer.AxisRenderInfo.AxisGridOptions.Add(key, new List<PathOptions>());
            }

            renderer.AxisRenderInfo.AxisGridOptions[key].Add(value);
        }

        private static Rect GetLabelRegion(double pointX, double pointY, VisibleLabels label, string anchor)
        {
            if (anchor == "middle")
            {
                pointX -= label.Size.Width / 2;
            }
            else if (anchor == "end")
            {
                pointX -= label.Size.Width;
            }

            pointY -= label.Size.Height / 2;
            return new Rect(pointX, pointY, label.Size.Width, label.Size.Height);
        }

        private static double GetAvailableSpaceToTrim(Rect legendRect, Rect labelRect)
        {
            double legendX2 = legendRect.X + legendRect.Width;
            double labelX2 = labelRect.X + labelRect.Width;
            double width = 0;
            if (labelRect.X > legendRect.X && labelRect.X < legendX2 && labelX2 > legendX2)
            {
                width = labelX2 - legendX2;
            }
            else if (labelRect.X > legendRect.X && labelRect.X < legendX2 && labelX2 < legendX2)
            {
                width = 0;
            }
            else if (labelX2 > legendRect.X && labelX2 < legendX2 && labelRect.X < legendRect.X)
            {
                width = legendRect.X - labelRect.X;
            }
            else if (labelX2 > legendRect.X && labelX2 > legendX2 && labelRect.X < legendRect.X)
            {
                width = legendRect.X - labelRect.X;
            }

            return width;
        }

        private void CalculateXAxisGridLine(ChartAxis axis, int index)
        {
            string minorGirdLine = string.Empty, minorTickLine = string.Empty;
            List<string> minorDirection;
            for (int i = 0; i < axis.Renderer.VisibleLabels.Count; i++)
            {
                ChartInternalLocation vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(axis.Renderer.VisibleLabels[i].Value, axis.Renderer), StartAngle);
                double x2 = CenterX + (Radius * vector.X);
                double y2 = CenterY + (Radius * vector.Y);
                double locationX = x2 + (axis.MajorTickLines.Height * vector.X * (axis.TickPosition == AxisPosition.Inside ? -1 : 1));
                double locationY = y2 + (axis.MajorTickLines.Height * vector.Y * (axis.TickPosition == AxisPosition.Inside ? -1 : 1));
                string majorGrid = "M " + CenterX.ToString(Culture) + SPACE + CenterY.ToString(Culture) + SPACE + "L " + x2.ToString(Culture) + SPACE + y2.ToString(Culture);
                string majorTick = "M " + x2.ToString(Culture) + SPACE + y2.ToString(Culture) + " L " + locationX.ToString(Culture) + SPACE + locationY.ToString(Culture);
                if (axis.MinorTicksPerInterval > 0 && (axis.MinorGridLines.Width > 0 || axis.MinorTickLines.Width > 0) && axis.ValueType != ValueType.Category && /*Chart.VisibleSeries[0].Type == ChartSeriesType.Polar*/true)
                {
                    minorDirection = CalculateAxisMinorLine(axis, axis.Renderer.VisibleLabels[i].Value, minorGirdLine, minorTickLine);
                    minorGirdLine = minorDirection[0];
                    minorTickLine = minorDirection[1];
                }

                RenderTickLine(axis, index, majorTick, minorTickLine, i);
                RenderGridLine(axis, index, majorGrid, minorGirdLine, i);
            }
        }

        private void CalculateYAxisLine(ChartAxis axis, int index)
        {
            ChartInternalLocation vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(axis.Renderer.VisibleLabels[0].Value, axis.Renderer), StartAngle);
            axis.Renderer.AxisRenderInfo.AxisLine = GetPathOptions(Chart.ID + "AxisLine_" + index, null, axis.LineStyle.Width, !string.IsNullOrEmpty(axis.LineStyle.Color) ? axis.LineStyle.Color : Chart.ChartThemeStyle.AxisLine, "M " + CenterX.ToString(Culture) + SPACE + CenterY.ToString(Culture) + "L " + (CenterX + (Radius * vector.X)).ToString(Culture) + SPACE + (CenterY + (Radius * vector.Y)).ToString(Culture), 1, axis.LineStyle.DashArray);
        }

        private string RenderRadarGrid(double radius, string majorGrid)
        {
            ChartInternalLocation vector, vector2;
            for (int i = 0, len = primaryXAxis.Renderer.VisibleLabels.Count; i < len; i++)
            {
                vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(primaryXAxis.Renderer.VisibleLabels[i].Value, primaryXAxis.Renderer), StartAngle);
                if (i + 1 < len)
                {
                    vector2 = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(primaryXAxis.Renderer.VisibleLabels[i + 1].Value, primaryXAxis.Renderer), StartAngle);
                }
                else
                {
                    vector2 = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(primaryXAxis.Renderer.VisibleLabels[0].Value, primaryXAxis.Renderer), StartAngle);
                }

                majorGrid = string.Concat(majorGrid, (i != 0 ? "L " : "M ") + SPACE + (CenterX + (radius * vector.X)).ToString(Culture) + SPACE + (CenterY + (radius * vector.Y)).ToString(Culture) + SPACE + "L " + SPACE + (CenterX + (radius * vector2.X)).ToString(Culture) + SPACE + (CenterY + (radius * vector2.Y)).ToString(Culture) + SPACE);
            }

            return majorGrid;
        }

        private List<string> CalculateAxisMinorLine(ChartAxis axis, double tempInterval, string minorGird, string minorTick)
        {
            double interval = tempInterval;
            List<string> direction = new List<string>();
            for (int j = 0; j < axis.MinorTicksPerInterval; j++)
            {
                interval += (axis.ValueType == ValueType.DateTime ? axis.Renderer.DateTimeInterval : axis.Renderer.VisibleInterval) / (axis.MinorTicksPerInterval + 1);
                if (ChartHelper.Inside(interval, axis.Renderer.VisibleRange))
                {
                    ChartInternalLocation vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(interval, axis.Renderer), StartAngle);
                    double x = CenterX + (Radius * vector.X);
                    double y = CenterY + (Radius * vector.Y);
                    double tickXSize = x + (axis.MinorTickLines.Height * vector.X * (axis.TickPosition == AxisPosition.Inside ? -1 : 1));
                    double tickYSize = y + (axis.MinorTickLines.Height * vector.Y * (axis.TickPosition == AxisPosition.Inside ? -1 : 1));
                    minorGird = string.Concat(minorGird, "M" + SPACE + CenterX.ToString(Culture) + SPACE + CenterY.ToString(Culture) + "L " + x.ToString(Culture) + SPACE + y.ToString(Culture));
                    minorTick = string.Concat(minorTick, "M" + SPACE + x.ToString(Culture) + SPACE + y.ToString(Culture) + "L" + SPACE + tickXSize.ToString(Culture) + SPACE + tickYSize.ToString(Culture));
                }
            }

            direction.Add(minorGird);
            direction.Add(minorTick);
            return direction;
        }

        private void RenderTickLine(ChartAxis axis, double index, string majorTickLine, string minorTickLine, double gridIndex)
        {
            PathOptions tickOptions;
            if (axis.MajorTickLines.Width > 0)
            {
                tickOptions = GetPathOptions(Chart.ID + "_MajorTickLine_" + index + "_" + gridIndex, Constants.TRANSPARENT, axis.MajorTickLines.Width, !string.IsNullOrEmpty(axis.MajorTickLines.Color) ? axis.MajorTickLines.Color : Chart.ChartThemeStyle.MajorTickLine, majorTickLine, 1);
                ChartHelper.AppendPathElements(Chart, tickOptions.Direction, tickOptions.Id);
                LoadDictionaryValue(axis.Renderer, Constants.MAJORTICKLINE, tickOptions);
            }

            if (axis.MinorTickLines.Width > 0)
            {
                tickOptions = GetPathOptions(Chart.ID + "_MinorTickLine_" + index + "_" + gridIndex, Constants.TRANSPARENT, axis.MinorTickLines.Width, !string.IsNullOrEmpty(axis.MinorTickLines.Color) ? axis.MinorTickLines.Color : Chart.ChartThemeStyle.MinorTickLine, minorTickLine, 1);
                ChartHelper.AppendPathElements(Chart, tickOptions.Direction, tickOptions.Id);
                LoadDictionaryValue(axis.Renderer, Constants.MINORTICKLINE, tickOptions);
            }
        }

        private void RenderGridLine(ChartAxis axis, double index, string majorGridLine, string minorGridLines, double gridIndex)
        {
            PathOptions gridOptions;
            if (axis.MajorGridLines.Width > 0)
            {
                gridOptions = GetPathOptions(Chart.ID + "_MajorGridLine_" + index + "_" + gridIndex, Constants.TRANSPARENT, axis.MajorGridLines.Width, !string.IsNullOrEmpty(axis.MajorGridLines.Color) ? axis.MajorGridLines.Color : Chart.ChartThemeStyle.MajorGridLine, majorGridLine, 1, axis.MajorGridLines.DashArray);
                gridOptions.Direction = ChartHelper.AppendPathElements(Chart, gridOptions.Direction, gridOptions.Id);
                LoadDictionaryValue(axis.Renderer, Constants.MAJORGRIDLINE, gridOptions);
            }

            if (axis.MinorGridLines.Width > 0)
            {
                gridOptions = GetPathOptions(Chart.ID + "_MinorGridLine_" + index + "_" + gridIndex, Constants.TRANSPARENT, axis.MinorGridLines.Width, !string.IsNullOrEmpty(axis.MajorTickLines.Color) ? axis.MinorGridLines.Color : Chart.ChartThemeStyle.MinorGridLine, minorGridLines, 1, axis.MinorGridLines.DashArray);
                gridOptions.Direction = ChartHelper.AppendPathElements(Chart, gridOptions.Direction, gridOptions.Id);
                LoadDictionaryValue(axis.Renderer, Constants.MINORGRIDLINE, gridOptions);
            }
        }

        private void CalculateYAxisLabels(ChartAxis axis, int index)
        {
            double angle = StartAngle < 0 ? StartAngle + 360 : StartAngle;
            List<Rect> labelRegions = new List<Rect>();
            List<bool> isLabelVisible = new List<bool>();
            isLabelVisible.AddRange(Enumerable.Repeat(true, axis.Renderer.VisibleLabels.Count));
            ChartInternalLocation vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(axis.Renderer.VisibleLabels[0].Value, axis.Renderer), StartAngle);
            for (int i = 0, len = axis.Renderer.VisibleLabels.Count; i < len; i++)
            {
                bool isIntersect = false;
                Size elementSize = axis.Renderer.VisibleLabels[i].Size;
                double radius = Radius * ChartHelper.ValueToCoefficient(axis.Renderer.VisibleLabels[i].Value, axis.Renderer);
                double pointX = (CenterX + (radius * vector.X)) + ((axis.MajorTickLines.Height + (elementSize.Width / 2) + (5 / 2)) * Math.Cos(angle * Math.PI / 180) * (axis.LabelPosition == AxisPosition.Inside ? 1 : -1));
                double pointY = ((CenterY + (radius * vector.Y)) + ((axis.MajorTickLines.Height + (elementSize.Height / 2)) * Math.Sin(angle * Math.PI / 180) * (axis.LabelPosition == AxisPosition.Inside ? 1 : -1))) + (elementSize.Height / 4);
                labelRegions.Add(GetLabelRegion(pointX, pointY, axis.Renderer.VisibleLabels[i], "middle"));
                if (i != 0 && axis.LabelIntersectAction == LabelIntersectAction.Hide)
                {
                    for (int j = i; j >= 0; j--)
                    {
                        j = (j == 0) ? 0 : (j == i) ? (j - 1) : j;
                        if (isLabelVisible[j] && ChartHelper.IsOverlap(labelRegions[i], labelRegions[j]))
                        {
                            isIntersect = true;
                            isLabelVisible[i] = false;
                            break;
                        }
                        else
                        {
                            isLabelVisible[i] = true;
                        }
                    }

                    if (isIntersect)
                    {
                        continue;
                    }

                    foreach (Rect rect in VisibleAxisLabelRect)
                    {
                        if (ChartHelper.IsOverlap(labelRegions[i], rect))
                        {
                            isIntersect = true;
                            break;
                        }
                    }
                }

                if (isIntersect)
                {
                    continue;
                }

                VisibleAxisLabelRect.Add(labelRegions[i]);
                FontOptions font = new FontOptions
                {
                    Color = !string.IsNullOrEmpty(axis.LabelStyle.Color) ? axis.LabelStyle.Color : Chart.ChartThemeStyle.AxisLabel,
                    Size = axis.LabelStyle.Size,
                    FontFamily = axis.LabelStyle.FontFamily,
                    FontWeight = axis.LabelStyle.FontWeight,
                    FontStyle = axis.LabelStyle.FontStyle
                };
                TextOptions options = new TextOptions(pointX.ToString(Culture), pointY.ToString(Culture), null, font, axis.Renderer.VisibleLabels[i].Text, "middle", Chart.ID + index + "_AxisLabel_" + i);
                string[] locations = ChartHelper.AppendTextElements(Chart, options.Id, Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture));
                options.X = locations[0];
                options.Y = locations[1];
                axis.Renderer.AxisRenderInfo.AxisLabelOptions.Add(options);
            }
        }

        private void CalculateYAxisGridLine(ChartAxis axis, int index)
        {
            PathOptions pathOptions;
            CircleOptions circleOptions;
            double radius = 0;
            string[] locations;
            double angle = StartAngle < 0 ? StartAngle + 360 : StartAngle;
            BorderModel border = new BorderModel
            {
                Color = !string.IsNullOrEmpty(axis.MajorGridLines.Color) ? axis.MajorGridLines.Color : Chart.ChartThemeStyle.MajorGridLine,
                Width = axis.MajorGridLines.Width
            };
            if (axis.MajorGridLines.Width > 0)
            {
                if (IsPolar)
                {
                    for (int j = 0; j < axis.Renderer.VisibleLabels.Count; j++)
                    {
                        radius = Radius * ChartHelper.ValueToCoefficient(axis.Renderer.VisibleLabels[j].Value, axis.Renderer);
                        circleOptions = new CircleOptions
                        {
                            Id = Chart.ID + "_MajorGridLine_" + index + "_" + j,
                            Fill = Constants.TRANSPARENT,
                            Stroke = border.Color,
                            StrokeWidth = border.Width,
                            Cx = CenterX.ToString(Culture),
                            Cy = CenterY.ToString(Culture),
                            R = radius.ToString(Culture),
                            Opacity = 1
                        };
                        locations = ChartHelper.AppendTextElements(Chart, circleOptions.Id, Convert.ToDouble(circleOptions.R, Culture), Convert.ToDouble(circleOptions.R, Culture), "r", "r");
                        circleOptions.R = locations[0];
                        circleOptions.R = locations[1];
                        axis.Renderer.AxisRenderInfo.MajorGridCircleOptions.Add(circleOptions);
                    }

                    if (radius != Radius)
                    {
                        circleOptions = new CircleOptions
                        {
                            Id = Chart.ID + "_MajorGridLine_" + index + "_" + axis.Renderer.VisibleLabels.Count + 1,
                            Fill = Constants.TRANSPARENT,
                            Stroke = border.Color,
                            StrokeWidth = border.Width,
                            Cx = CenterX.ToString(Culture),
                            Cy = CenterY.ToString(Culture),
                            R = radius.ToString(Culture)
                        };
                        locations = ChartHelper.AppendTextElements(Chart, circleOptions.Id, Convert.ToDouble(circleOptions.R, Culture), Convert.ToDouble(circleOptions.R, Culture), "r", "r");
                        circleOptions.R = locations[0];
                        circleOptions.R = locations[1];
                        axis.Renderer.AxisRenderInfo.MajorGridCircleOptions.Add(circleOptions);
                    }
                }
                else
                {
                    for (int j = 0; j < axis.Renderer.VisibleLabels.Count; j++)
                    {
                        radius = Radius * ChartHelper.ValueToCoefficient(axis.Renderer.VisibleLabels[j].Value, axis.Renderer);
                        pathOptions = GetPathOptions(Chart.ID + "_MajorGridLine_" + index + "_" + j, Constants.TRANSPARENT, axis.MajorGridLines.Width, !string.IsNullOrEmpty(axis.MajorGridLines.Color) ? axis.MajorGridLines.Color : Chart.ChartThemeStyle.MajorGridLine, RenderRadarGrid(radius, string.Empty), 1);
                        LoadDictionaryValue(axis.Renderer, Constants.MAJORGRIDLINE, pathOptions);
                    }

                    if (radius != Radius)
                    {
                        pathOptions = GetPathOptions(Chart.ID + "_MajorGridLine_" + index + "_" + axis.Renderer.VisibleLabels.Count, Constants.TRANSPARENT, axis.MajorGridLines.Width, !string.IsNullOrEmpty(axis.MajorGridLines.Color) ? axis.MajorGridLines.Color : Chart.ChartThemeStyle.MajorGridLine, RenderRadarGrid(Radius, string.Empty), 1);
                        LoadDictionaryValue(axis.Renderer, Constants.MAJORGRIDLINE, pathOptions);
                    }
                }
            }

            if (axis.MajorTickLines.Width > 0 && axis.Renderer.VisibleLabels.Count > 0)
            {
                ChartInternalLocation vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(axis.Renderer.VisibleLabels[0].Value, axis.Renderer), StartAngle);
                for (int i = 0; i < axis.Renderer.VisibleLabels.Count; i++)
                {
                    radius = Radius * ChartHelper.ValueToCoefficient(axis.Renderer.VisibleLabels[i].Value, axis.Renderer);
                    double x1 = CenterX + (radius * vector.X);
                    double y1 = CenterY + (radius * vector.Y);
                    RenderTickLine(axis, index, "M " + x1.ToString(Culture) + SPACE + y1.ToString(Culture) + " L " + (x1 + (axis.MajorTickLines.Height * Math.Cos(angle * Math.PI / 180) * (axis.TickPosition == AxisPosition.Inside ? 1 : -1))).ToString(Culture) + SPACE + (y1 + (axis.MajorTickLines.Height * Math.Sin(angle * Math.PI / 180) * (axis.TickPosition == AxisPosition.Inside ? 1 : -1))).ToString(Culture), string.Empty, i);
                }
            }
        }

        private void CalculateXAxisLabels(ChartAxis axis, int index)
        {
            VisibleAxisLabelRect = new List<Rect>();
            Rect legendRect = null;

            if (Chart.LegendRenderer != null && Chart.LegendRenderer.LegendSettings.Visible)
            {
                legendRect = Chart.LegendRenderer.LegendBounds;
            }

            double pointX = 0, pointY = 0, firstLabelX = 0;
            string textAnchor = string.Empty;
            bool islabelInside = axis.LabelPosition == AxisPosition.Inside;
            List<Rect> labelRegions = new List<Rect>();
            List<bool> isLabelVisible = new List<bool>();
            isLabelVisible.AddRange(Enumerable.Repeat(true, axis.Renderer.VisibleLabels.Count));
            double radius = Radius + axis.MajorTickLines.Height;
            radius = islabelInside ? -radius : radius;
            ChartFontOptions axisLabelStyle = axis.LabelStyle.GetChartFontOptions();
            for (int i = 0, len = axis.Renderer.VisibleLabels.Count; i < len; i++)
            {
                bool isIntersect = false;
                VisibleLabels label = axis.Renderer.VisibleLabels[i];
                ChartFontOptions labelStyle = label.LabelStyle.GetChartFontOptions();
                ChartInternalLocation vector = ChartHelper.CoefficientToVector(ChartHelper.ValueToPolarCoefficient(label.Value + (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0), axis.Renderer), StartAngle);
                if (!double.IsNaN(vector.X) && !double.IsNaN(vector.Y))
                {
                    pointX = CenterX + ((radius + axis.MajorTickLines.Height + 5) * vector.X);
                    pointY = CenterY + ((radius + axis.MajorTickLines.Height + 5) * vector.Y);
                    textAnchor = (pointX.ToString("N1", CultureInfo.InvariantCulture) == CenterX.ToString("N1", CultureInfo.InvariantCulture)) ? "middle" : ((pointX < CenterX && !islabelInside) || (pointX > CenterX && islabelInside)) ? "end" : "start";
                }

                string labelText = label.Text;
                if (axis.EnableTrim || axis.LabelIntersectAction == LabelIntersectAction.Trim)
                {
                    string originalText = axis.Renderer.VisibleLabels[i].OriginalText;
                    double chartWidth = Chart.AvailableSize.Width;
                    for (int i1 = originalText.Length - 1; i1 >= 0; --i1)
                    {
                        string trimText = originalText.Substring(0, i1) + "...";
                        double size = ChartHelper.MeasureText(trimText, axisLabelStyle).Width;
                        if (pointX == chartWidth / 2 ? (pointX - (size / 2) >= 0 && pointX + (size / 2) <= chartWidth) : ((axis.LabelPosition == AxisPosition.Outside && ((pointX >= chartWidth / 2 && pointX + size <= chartWidth) ||
                            (pointX <= chartWidth / 2 && pointX - size >= 0))) || (axis.LabelPosition == AxisPosition.Inside && (pointX + size <= chartWidth / 2 || pointX - size >= chartWidth / 2))))
                        {
                            labelText = i1 == originalText.Length - 1 ? originalText : trimText;
                            label.Size.Width = ChartHelper.MeasureText(labelText, axisLabelStyle).Width;
                            label.Text = labelText;
                            break;
                        }
                    }
                }

                labelRegions.Add(GetLabelRegion(pointX, pointY, label, textAnchor));
                if (i == 0)
                {
                    firstLabelX = pointX;
                }
                else if (i == axis.Renderer.VisibleLabels.Count - 1 && axis.ValueType != ValueType.Category)
                {
                    labelText = (ChartHelper.MeasureText(labelText, axisLabelStyle).Height + pointX) > firstLabelX ? string.Empty : labelText;
                }

                if (i != 0 && axis.LabelIntersectAction == LabelIntersectAction.Hide)
                {
                    for (int j = i; j >= 0; j--)
                    {
                        j = (j == 0) ? 0 : ((j == i) ? (j - 1) : j);
                        if (isLabelVisible[j] && ChartHelper.IsOverlap(labelRegions[i], labelRegions[j]))
                        {
                            isIntersect = true;
                            isLabelVisible[i] = false;
                            break;
                        }
                        else
                        {
                            isLabelVisible[i] = true;
                        }
                    }
                }

                if (!isIntersect && legendRect != null)
                {
                    isIntersect = ChartHelper.IsOverlap(labelRegions[i], legendRect);
                    if (isIntersect)
                    {
                        double width = GetAvailableSpaceToTrim(legendRect, labelRegions[i]);
                        if (width > 0)
                        {
                            labelText = ChartHelper.TextTrim(width, axis.Renderer.VisibleLabels[i].OriginalText, /*axis.LabelStyle*/new ChartFontOptions());
                            isIntersect = false;
                        }
                    }
                }

                TextOptions options = new TextOptions(pointX.ToString(Culture), pointY.ToString(Culture), !string.IsNullOrEmpty(label.LabelStyle.Color) ? label.LabelStyle.Color : Chart.ChartThemeStyle.AxisLabel, label.LabelStyle.GetChartFontOptions(), labelText, textAnchor, Chart.ID + index + "_AxisLabel_" + i);
                if (isIntersect)
                {
                    continue;
                }

                string[] locations = ChartHelper.AppendTextElements(Chart, options.Id, Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture));
                options.X = locations[0];
                options.Y = locations[1];
                options.Font = labelStyle;
                VisibleAxisLabelRect.Add(labelRegions[i]);
                axis.Renderer.AxisRenderInfo.AxisLabelOptions.Add(options);
            }
        }
    }
}
