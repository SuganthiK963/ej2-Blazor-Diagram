using Microsoft.AspNetCore.Components.Rendering;
using System;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;
using Syncfusion.PdfExport;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class CartesianAxisLayout : AxisLayout
    {
        private Rect initialClipRect;

        private double leftSize;

        private double rightSize;

        private double topSize;

        private double bottomSize;

        internal List<ChartAxis> Axes { get; set; } = new List<ChartAxis>();

        internal bool IsAxisLabelRender { get; set; }

        private static double GetAxisOffsetValue(double position1, double position2, double plotOffset)
        {
            return !double.IsNaN(position1) ? (position1 + (!double.IsNaN(position2) ? position2 : plotOffset)) : !double.IsNaN(position2) ? position2 + plotOffset : 2 * plotOffset;
        }

        private static void MeasureRowDefinition(ChartRowRenderer renderer, Size size)
        {
            foreach (ChartAxis axis in renderer.Axes)
            {
                axis.Renderer.ComputeSize(size);
                renderer.ComputeSize(axis, axis.ScrollBarHeight);
            }

            if (renderer.FarSizes.Count > 0)
            {
                renderer.FarSizes[renderer.FarSizes.Count - 1] -= 10;
            }

            if (renderer.NearSizes.Count > 0)
            {
                renderer.NearSizes[renderer.NearSizes.Count - 1] -= 10;
            }
        }

        private static void MeasureColumnDefinition(ChartColumnRenderer renderer, Size size)
        {
            foreach (ChartAxis axis in renderer.Axes)
            {
                axis.Renderer.ComputeSize(size);
                renderer.ComputeSize(axis, axis.ScrollBarHeight);
            }

            if (renderer.FarSizes.Count > 0)
            {
                renderer.FarSizes[renderer.FarSizes.Count - 1] -= 10;
            }

            if (renderer.NearSizes.Count > 0)
            {
                renderer.NearSizes[renderer.NearSizes.Count - 1] -= 10;
            }
        }

        internal static bool FindAxisPosition(ChartAxis axis)
        {
            return !double.IsNaN(axis.Renderer.CrossAt) && axis.Renderer.IsInside(axis.Renderer.CrossInAxis.Renderer.VisibleRange);
        }

        private static double FindLogNumeric(ChartAxis axis, double logPosition, double interval, int labelIndex)
        {
            if (axis.ValueType == ValueType.DateTime)
            {
                interval += axis.Renderer.DateTimeInterval / (axis.MinorTicksPerInterval + 1);
            }
            else if (axis.ValueType == ValueType.Logarithmic)
            {
                interval = ChartHelper.LogBase(logPosition, axis.LogBase);
            }
            else if (axis.ValueType == ValueType.DateTimeCategory)
            {
                var padding = axis.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0;
                interval += ((labelIndex + 1 < axis.Renderer.VisibleLabels.Count ? axis.Renderer.VisibleLabels[labelIndex + 1].Value - padding : axis.Renderer.VisibleRange.End) -
                (labelIndex < axis.Renderer.VisibleLabels.Count ? axis.Renderer.VisibleLabels[labelIndex].Value - padding : axis.Renderer.VisibleRange.Start)) / (axis.MinorTicksPerInterval + 1);
            }
            else
            {
                interval += axis.Renderer.VisibleInterval / (axis.MinorTicksPerInterval + 1);
            }

            return interval;
        }

        private static ChartFontOptions GetFontOptions(ChartFontOptions style)
        {
            return new ChartFontOptions { Color = style.Color, Size = style.Size, FontFamily = style.FontFamily, FontWeight = style.FontWeight, FontStyle = style.FontStyle, TextAlignment = style.TextAlignment, TextOverflow = style.TextOverflow };
        }

        private static bool IsRotatedRectIntersect(ChartInternalLocation[] a, ChartInternalLocation[] b)
        {
            List<ChartInternalLocation[]> polygons = new List<ChartInternalLocation[]> { a, b };
            double minA, maxA, projected, minB, maxB;
            for (int i = 0; i < polygons.Count; i++)
            {
                for (int k = 0; k < polygons[i].Length; k++)
                {
                    ChartInternalLocation p1 = polygons[i][k];
                    ChartInternalLocation p2 = polygons[i][(k + 1) % polygons[i].Length];
                    ChartInternalLocation normal = new ChartInternalLocation(p2.Y - p1.Y, p1.X - p2.X);
                    minA = maxA = double.NaN;
                    for (int j = 0; j < a.Length; j++)
                    {
                        projected = (normal.X * a[j].X) + (normal.Y * a[j].Y);
                        if (double.IsNaN(minA) || projected < minA)
                        {
                            minA = projected;
                        }

                        if (double.IsNaN(maxA) || projected > maxA)
                        {
                            maxA = projected;
                        }
                    }

                    minB = maxB = double.NaN;
                    for (int j = 0; j < b.Length; j++)
                    {
                        projected = (normal.X * b[j].X) + (normal.Y * b[j].Y);
                        if (double.IsNaN(minB) || projected < minB)
                        {
                            minB = projected;
                        }

                        if (double.IsNaN(maxB) || projected > maxB)
                        {
                            maxB = projected;
                        }
                    }

                    if (maxA < minB || maxB < minA)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static List<string> GetLabelText(VisibleLabels label, ChartAxis axis, double intervalLength)
        {
            if (ChartHelper.IsBreakLabel(label.OriginalText) || (axis.LabelIntersectAction == LabelIntersectAction.Wrap))
            {
                List<string> result = new List<string>();
                for (int index = 0; index < label.TextArr.Length; index++)
                {
                    result.Add(FindAxisLabel(axis, label.TextArr[index], intervalLength));
                }

                return result;
            }
            else
            {
                return new List<string>() { FindAxisLabel(axis, label.Text, intervalLength) };
            }
        }

        private static ChartInternalLocation[] GetRotatedRectangleCoordinates(ChartInternalLocation[] actualPoints, double centerX, double centerY, double angle)
        {
            List<ChartInternalLocation> coordinatesAfterRotation = new List<ChartInternalLocation>();
            for (int i = 0; i < 4; i++)
            {
                double tempX = actualPoints[i].X - centerX;
                double tempY = actualPoints[i].Y - centerY;
                actualPoints[i].X = ((tempX * Math.Cos(DegreeToRadian(angle))) - (tempY * Math.Sin(DegreeToRadian(angle)))) + centerX;
                actualPoints[i].Y = ((tempX * Math.Sin(DegreeToRadian(angle))) + (tempY * Math.Cos(DegreeToRadian(angle)))) + centerY;
                coordinatesAfterRotation.Add(new ChartInternalLocation(actualPoints[i].X, actualPoints[i].Y));
            }

            return coordinatesAfterRotation.ToArray();
        }

        private static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        private static ChartInternalLocation[] GetRectanglePoints(Rect rect)
        {
            return new ChartInternalLocation[] { new ChartInternalLocation(rect.X, rect.Y), new ChartInternalLocation(rect.X + rect.Width, rect.Y), new ChartInternalLocation(rect.X + rect.Width, rect.Y + rect.Height), new ChartInternalLocation(rect.X, rect.Y + rect.Height) };
        }

        private static string FindAxisLabel(ChartAxis axis, string label, double width)
        {
            ChartFontOptions axisLabelStyle = axis.LabelStyle.GetChartFontOptions();
            if (axis.LabelIntersectAction == LabelIntersectAction.Trim && axis.Renderer.Angle % 360 == 0 && !axis.EnableTrim)
            {
                return ChartHelper.TextTrim(width, label, axisLabelStyle);
            }

            return label;
        }

        internal override void AddAxis(ChartAxis axis)
        {
            Axes.Add(axis);
        }

        internal override void RemoveAxis(ChartAxis axis)
        {
            throw new NotImplementedException();
        }

        internal override void ComputePlotAreaBounds(Rect rect)
        {
            CrossAt();
            SeriesClipRect = new Rect() { X = rect.X, Y = rect.Y, Height = rect.Height, Width = rect.Width };
            initialClipRect = rect;
            leftSize = rightSize = topSize = bottomSize = 0;
            MeasureRowAxis();
            initialClipRect = ChartHelper.SubtractThickness(initialClipRect, new Thickness(leftSize, rightSize, 0, 0));
            MeasureColumnAxis();
            initialClipRect = ChartHelper.SubtractThickness(initialClipRect, new Thickness(0, 0, topSize, bottomSize));
            CalculateAxisSize(initialClipRect);
            leftSize = rightSize = topSize = bottomSize = 0;
            MeasureRowAxis();
            SeriesClipRect = ChartHelper.SubtractThickness(SeriesClipRect, new Thickness(leftSize, rightSize, 0, 0));
            MeasureColumnAxis();
            SeriesClipRect = ChartHelper.SubtractThickness(SeriesClipRect, new Thickness(0, 0, topSize, bottomSize));
            RefreshAxis();
            CalculateAxisSize(SeriesClipRect);
        }

        private void CrossAt()
        {
            foreach (ChartAxis axis in Axes)
            {
                if (axis.CrossesAt == null)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(axis.CrossesInAxis))
                {
                    if (Chart.RequireInvertedAxis)
                    {
                        axis.Renderer.CrossInAxis = axis.Renderer.Orientation == Orientation.Horizontal ? Axes[0] : Axes[1];
                    }
                    else
                    {
                        axis.Renderer.CrossInAxis = axis.Renderer.Orientation == Orientation.Horizontal ? Axes[1] : Axes[0];
                    }

                    continue;
                }
                else
                {
                    for (int i = 2, len = Axes.Count; i < len; i++)
                    {
                        if (axis.CrossesInAxis == Axes[i].Name)
                        {
                            axis.Renderer.CrossInAxis = Axes[i];
                            continue;
                        }
                    }
                }
            }
        }

        private void CalculateAxisSize(Rect rect)
        {
            ChartAxis axis;
            double nearCount, farCount, size, x, y, axisOffset;
            Chart.RowContainer.HandleChartSizeChange(rect);
            for (int i = 0, len = Chart.RowContainer.Renderers.Count; i < len; i++)
            {
                ChartRowRenderer renderer = (ChartRowRenderer)Chart.RowContainer.Renderers[i];
                nearCount = 0;
                farCount = 0;
                for (int j = 0, len1 = renderer.Axes.Count; j < len1; j++)
                {
                    axis = renderer.Axes[j];
                    axisOffset = axis.PlotOffset;
                    if (double.IsNaN(axis.Renderer.Rect.Height))
                    {
                        axis.Renderer.Rect.Height = renderer.ComputedHeight;
                        size = 0;
                        for (int k = i + 1, len2 = i + axis.Span; k < len2; k++)
                        {
                            size += ((ChartRowRenderer)Chart.RowContainer.Renderers[k]).ComputedHeight;
                        }

                        axis.Renderer.Rect.Y = (renderer.ComputedTop - size) + (axis.PlotOffsetTop > 0 ? axis.PlotOffsetTop : axisOffset);
                        axis.Renderer.Rect.Height = (axis.Renderer.Rect.Height + size) - GetAxisOffsetValue(axis.PlotOffsetTop, axis.PlotOffsetBottom, axis.PlotOffset);
                        axis.Renderer.Rect.Width = 0;
                    }

                    if (axis.OpposedPosition)
                    {
                        x = rect.X + rect.Width + renderer.FarSizes.GetRange(0, Convert.ToInt32(farCount)).Sum();
                        axis.Renderer.Rect.X = axis.Renderer.Rect.X >= x ? axis.Renderer.Rect.X : x;
                        farCount++;
                    }
                    else
                    {
                        x = rect.X - renderer.NearSizes.GetRange(0, Convert.ToInt32(nearCount)).Sum();
                        axis.Renderer.Rect.X = axis.Renderer.Rect.X <= x ? axis.Renderer.Rect.X : x;
                        nearCount++;
                    }
                }
            }

            Chart.ColumnContainer.HandleChartSizeChange(rect);
            for (int i = 0, len = Chart.ColumnContainer.Renderers.Count; i < len; i++)
            {
                ChartColumnRenderer renderer = (ChartColumnRenderer)Chart.ColumnContainer.Renderers[i];
                nearCount = 0;
                farCount = 0;
                for (int j = 0, len2 = renderer.Axes.Count; j < len2; j++)
                {
                    axis = renderer.Axes[j];
                    axisOffset = axis.PlotOffset;
                    if (double.IsNaN(axis.Renderer.Rect.Width))
                    {
                        size = 0;
                        axis.Renderer.Rect.Width = 0;
                        for (int k = i, len3 = i + axis.Span; k < len3; k++)
                        {
                            size += ((ChartColumnRenderer)Chart.ColumnContainer.Renderers[k]).ComputedWidth;
                        }

                        axis.Renderer.Rect.X = renderer.ComputedLeft + (axis.PlotOffsetLeft > 0 ? axis.PlotOffsetLeft : axisOffset);
                        axis.Renderer.Rect.Width = (axis.Renderer.Rect.Width + size) - GetAxisOffsetValue(axis.PlotOffsetLeft, axis.PlotOffsetRight, axis.PlotOffset);
                        axis.Renderer.Rect.Height = 0;
                    }

                    if (axis.OpposedPosition)
                    {
                        y = rect.Y - renderer.FarSizes.GetRange(0, Convert.ToInt32(farCount)).Sum();
                        axis.Renderer.Rect.Y = axis.Renderer.Rect.Y <= y ? axis.Renderer.Rect.Y : y;
                        farCount++;
                    }
                    else
                    {
                        y = rect.Y + rect.Height + renderer.NearSizes.GetRange(0, Convert.ToInt32(nearCount)).Sum();
                        axis.Renderer.Rect.Y = axis.Renderer.Rect.Y >= y ? axis.Renderer.Rect.Y : y;
                        nearCount++;
                    }
                }
            }
        }

        internal void RefreshAxis()
        {
            foreach (ChartAxis axis in Axes)
            {
                if (axis.Renderer != null)
                {
                   axis.Renderer.Rect = new Rect() { X = double.NaN, Y = double.NaN, Height = double.NaN, Width = double.NaN };
                }
            }
        }

        private void MeasureRowAxis()
        {
            foreach (ChartRowRenderer renderer in Chart.RowContainer.Renderers)
            {
                renderer.NearSizes = new List<double>();
                renderer.FarSizes = new List<double>();
                MeasureRowDefinition(renderer, new Size(Chart.AvailableSize.Width, renderer.ComputedHeight));
                if (leftSize < renderer.NearSizes.Sum())
                {
                    leftSize = renderer.NearSizes.Sum();
                }

                if (rightSize < renderer.FarSizes.Sum())
                {
                    rightSize = renderer.FarSizes.Sum();
                }
            }
        }

        private void MeasureColumnAxis()
        {
            foreach (ChartColumnRenderer renderer in Chart.ColumnContainer.Renderers)
            {
                renderer.FarSizes = new List<double>();
                renderer.NearSizes = new List<double>();
                MeasureColumnDefinition(renderer, new Size(renderer.ComputedWidth, Chart.AvailableSize.Height));
                if (bottomSize < renderer.NearSizes.Sum())
                {
                    bottomSize = renderer.NearSizes.Sum();
                }

                if (topSize < renderer.FarSizes.Sum())
                {
                    topSize = renderer.FarSizes.Sum();
                }
            }
        }

        internal override void AxisRenderingCalculation(ChartAxisRenderer renderer)
        {
            ChartAxis axis = renderer.Axis;
            renderer.UpdateCrossValue();
            renderer.IsAxisInside = FindAxisPosition(renderer.Axis);
            renderer.IsTickInside = renderer.IsAxisInside || axis.TickPosition == AxisPosition.Inside;
            renderer.IsAxisLabelInside = renderer.IsAxisInside || axis.LabelPosition == AxisPosition.Inside;
            Rect rect = axis.PlaceNextToAxisLine ? renderer.UpdatedRect : renderer.Rect;
            if (axis.Visible && axis.LineStyle.Width > 0)
            {
                if (renderer.Orientation == Orientation.Vertical)
                {
                    renderer.AxisRenderInfo.AxisLine = new PathOptions { Id = renderer.Owner.ID + "AxisLine_" + renderer.Index, Direction = "M " + renderer.UpdatedRect.X.ToString(Culture) + SPACE + (renderer.UpdatedRect.Y - axis.PlotOffset).ToString(Culture) + " L " + (renderer.UpdatedRect.X + renderer.UpdatedRect.Width).ToString(Culture) + SPACE + (renderer.UpdatedRect.Y + renderer.UpdatedRect.Height + axis.PlotOffset).ToString(Culture), StrokeDashArray = axis.LineStyle.DashArray, StrokeWidth = axis.LineStyle.Width, Stroke = axis.LineStyle.Color ?? Chart.ChartThemeStyle.AxisLine };
                }
                else
                {
                    renderer.AxisRenderInfo.AxisLine = new PathOptions { Id = renderer.Owner.ID + "AxisLine_" + renderer.Index, Direction = "M " + (renderer.UpdatedRect.X - axis.PlotOffset).ToString(Culture) + SPACE + renderer.UpdatedRect.Y.ToString(Culture) + " L " + (renderer.UpdatedRect.X + renderer.UpdatedRect.Width + axis.PlotOffset).ToString(Culture) + SPACE + (renderer.UpdatedRect.Y + renderer.UpdatedRect.Height).ToString(Culture), StrokeDashArray = axis.LineStyle.DashArray, StrokeWidth = axis.LineStyle.Width, Stroke = axis.LineStyle.Color ?? Chart.ChartThemeStyle.AxisLine };
                }
            }

            if (renderer.Orientation == Orientation.Horizontal)
            {
                if (axis.MajorGridLines.Width > 0 || axis.MajorTickLines.Width > 0)
                {
                    CalculateXAxisGridLine(axis, renderer.Index, renderer.UpdatedRect);
                }

                if (axis.Visible)
                {
                    CalculateXAxisLabels(axis, renderer.Index, rect);
                    CalculateXAxisBorder(axis, renderer.Index, rect);
                    if (!string.IsNullOrEmpty(axis.Title))
                    {
                        CalculateXAxisTitle(axis, renderer.Index, rect);
                    }
                }
            }
            else
            {
                if (axis.MajorGridLines.Width > 0 || axis.MajorTickLines.Width > 0)
                {
                    CalculateYAxisGridLine(axis, renderer.Index, renderer.UpdatedRect);
                }

                if (axis.Visible)
                {
                    CalculateYAxisLabels(axis, renderer.Index, rect);
                    CalculateYAxisBorder(axis, renderer.Index, rect);
                    if (!string.IsNullOrEmpty(axis.Title))
                    {
                        CalculateYAxisTitle(axis, renderer.Index, rect);
                    }
                }
            }

            // Only grid lines drawing should happen here. Axis labels and ticks should be drawn in axis build render tree method.
            // throw new NotImplementedException();
        }

        private static void LoadDictionaryValue(ChartAxisRenderer renderer, string key, PathOptions value)
        {
            if (!renderer.AxisRenderInfo.AxisGridOptions.ContainsKey(key))
            {
                renderer.AxisRenderInfo.AxisGridOptions.Add(key, new List<PathOptions>());
            }

            renderer.AxisRenderInfo.AxisGridOptions[key].Add(value);
        }

        private void CalculateYAxisGridLine(ChartAxis axis, int index, Rect rect)
        {
            string[] minorGridDirection = Array.Empty<string>();
            string majorGridId, majorGridDirection, majorTickId, majorTickDirection, minorId;
            double tickSize = axis.OpposedPosition ? axis.MajorTickLines.Height : -axis.MajorTickLines.Height;
            double axisLineSize = axis.OpposedPosition ? axis.LineStyle.Width * 0.5 : -axis.LineStyle.Width * 0.5;
            double ticksbwtLabel = (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0.5 : 0;
            double scrollBarHeight = (axis.CrossesAt == null) ? axis.OpposedPosition ? axis.ScrollBarHeight : -axis.ScrollBarHeight : 0;
            bool isTickInside = axis.TickPosition == AxisPosition.Inside;
            double ticks = isTickInside ? (rect.X - tickSize - axisLineSize) : (rect.X + tickSize + axisLineSize + scrollBarHeight);
            double length = axis.Renderer.VisibleLabels.Count;
            if (axis.ValueType.ToString().Contains("Category", StringComparison.InvariantCulture) && axis.LabelPlacement == LabelPlacement.BetweenTicks && length > 0)
            {
                length += 1;
            }

            for (int i = 0; i < length; i++)
            {
                double tempInterval = i < axis.Renderer.VisibleLabels.Count ? (axis.Renderer.VisibleLabels[i] != null ? axis.Renderer.VisibleLabels[i].Value - ticksbwtLabel : (axis.Renderer.VisibleLabels[i - 1].Value + axis.Renderer.VisibleInterval) - ticksbwtLabel) : axis.Renderer.VisibleLabels[i - 1].Value + axis.Renderer.VisibleInterval - ticksbwtLabel;
                double pointY = ((ChartHelper.ValueToCoefficient(tempInterval, axis.Renderer) * rect.Height) * -1) + (rect.Y + rect.Height);
                if (pointY >= rect.Y && (rect.Y + rect.Height) >= pointY)
                {
                    if (ChartHelper.Inside(tempInterval, axis.Renderer.VisibleRange) || IsBorder(axis, i, pointY))
                    {
                        majorGridId = axis.Renderer.Owner.ID + "_MajorGridLine_" + index + "_" + i;
                        majorGridDirection = "M " + SeriesClipRect.X.ToString(Culture) + SPACE + pointY.ToString(Culture) + " L " + (SeriesClipRect.X + SeriesClipRect.Width).ToString(Culture) + SPACE + pointY.ToString(Culture);
                        ChartHelper.AppendPathElements(axis.Renderer.Owner, majorGridDirection, majorGridId);
                        if (axis.MajorGridLines.Width > 0 && axis.Visible)
                        {
                            LoadDictionaryValue(axis.Renderer, Constants.MAJORGRIDLINE, new PathOptions { Id = majorGridId, Direction = majorGridDirection, StrokeDashArray = axis.MajorGridLines.DashArray, StrokeWidth = axis.MajorGridLines.Width, Stroke = axis.MajorGridLines.Color ?? Chart.ChartThemeStyle.MajorGridLine });
                        }
                    }

                    majorTickId = axis.Renderer.Owner.ID + "_MajorTickLine_" + index + "_" + i;
                    majorTickDirection = "M " + (rect.X + axisLineSize + (isTickInside ? scrollBarHeight : 0)).ToString(Culture) + SPACE + pointY.ToString(Culture) + " L " + ticks.ToString(Culture) + SPACE + pointY.ToString(Culture);
                    ChartHelper.AppendPathElements(axis.Renderer.Owner, majorTickDirection, majorTickId);
                    if (axis.MajorTickLines.Width > 0 && axis.Visible)
                    {
                        LoadDictionaryValue(axis.Renderer, Constants.MAJORTICKLINE, new PathOptions { Id = majorTickId, Direction = majorTickDirection, StrokeWidth = axis.MajorTickLines.Width, Stroke = axis.MajorTickLines.Color ?? Chart.ChartThemeStyle.MajorTickLine });
                    }

                    if ((axis.MinorGridLines.Width > 0 || axis.MinorTickLines.Width > 0) && axis.MinorTicksPerInterval > 0)
                    {
                        minorGridDirection = CalculateAxisMinorLine(axis, tempInterval, rect, i);
                        minorId = axis.Renderer.Owner.ID + "_MinorGridLine_" + index + "_" + i;
                        ChartHelper.AppendPathElements(axis.Renderer.Owner, minorGridDirection[0], minorId);
                        if (axis.MinorGridLines.Width > 0 && axis.Visible && !string.IsNullOrEmpty(minorGridDirection[0]))
                        {
                            LoadDictionaryValue(axis.Renderer, Constants.MINORGRIDLINE, new PathOptions { Id = minorId, Direction = minorGridDirection[0], StrokeDashArray = axis.MinorGridLines.DashArray, StrokeWidth = axis.MinorGridLines.Width, Stroke = axis.MinorGridLines.Color ?? Chart.ChartThemeStyle.MinorGridLine });
                        }

                        minorId = axis.Renderer.Owner.ID + "_MinorTickLine_" + index + "_" + i;
                        ChartHelper.AppendPathElements(axis.Renderer.Owner, minorGridDirection[1], minorId);
                        if (axis.MinorTickLines.Width > 0 && axis.Visible && !string.IsNullOrEmpty(minorGridDirection[1]))
                        {
                            LoadDictionaryValue(axis.Renderer, Constants.MINORTICKLINE, new PathOptions { Id = minorId, Direction = minorGridDirection[1], StrokeWidth = axis.MinorTickLines.Width, Stroke = axis.MinorTickLines.Color ?? Chart.ChartThemeStyle.MinorTickLine });
                        }
                    }
                }
            }
        }

        internal void CalculateYAxisLabels(ChartAxis axis, int index, Rect rect)
        {
            double labelSpace = axis.LabelPadding;
            bool isLabelInside = axis.LabelPosition == AxisPosition.Inside;
            double tickSpace = axis.LabelPosition == axis.TickPosition ? axis.MajorTickLines.Height : 0;
            double padding = tickSpace + labelSpace + (axis.LineStyle.Width * 0.5);
            padding = axis.OpposedPosition ? padding : -padding;
            ChartFontOptions labelStyle = axis.LabelStyle.GetChartFontOptions();
            for (int i = 0, len = axis.Renderer.VisibleLabels.Count; i < len; i++)
            {
                bool isAxisBreakLabel = ChartHelper.IsBreakLabel(axis.Renderer.VisibleLabels[i].OriginalText);
                double pointX = isLabelInside ? (rect.X - padding) : (rect.X + padding + (axis.CrossesAt == null ? axis.ScrollBarHeight * (axis.OpposedPosition ? 1 : -1) : 0));
                Size elementSize = isAxisBreakLabel ? axis.Renderer.VisibleLabels[i].BreakLabelSize : axis.Renderer.VisibleLabels[i].Size;
                double pointY = (ChartHelper.ValueToCoefficient(axis.Renderer.VisibleLabels[i].Value, axis.Renderer) * rect.Height) + 0;
                pointY = Math.Floor((pointY * -1) + (rect.Y + rect.Height));
                double textHeight = (elementSize.Height / 8) * (axis.Renderer.VisibleLabels[i].TextArr.Length / 2);
                double textPadding = ((elementSize.Height / 4) * 3) + 3;
                pointY = isAxisBreakLabel ? (axis.LabelPosition == AxisPosition.Inside ? (pointY - (elementSize.Height / 2) - textHeight + textPadding)
                    : (pointY - textHeight)) : (axis.LabelPosition == AxisPosition.Inside ? (pointY + textPadding) : pointY + (elementSize.Height / 4));
                bool isEndAnchor = ((axis.OpposedPosition && isLabelInside) || (!axis.OpposedPosition && !isLabelInside));
                isEndAnchor = Chart.EnableRTL ? !isEndAnchor : isEndAnchor;
                TextOptions options = new TextOptions(pointX.ToString(Culture), pointY.ToString(Culture), axis.LabelStyle.Color ?? Chart.ChartThemeStyle.AxisLabel, GetFontOptions(labelStyle), axis.Renderer.VisibleLabels[i].Text, isEndAnchor ? "end" : "start", axis.Renderer.Owner.ID + index + "_AxisLabel_" + i);
                if (isAxisBreakLabel)
                {
                    foreach (string text in axis.Renderer.VisibleLabels[i].TextArr)
                    {
                        options.TextCollection.Add(text);
                    }
                }

                switch (axis.EdgeLabelPlacement)
                {
                    case EdgeLabelPlacement.None:
                        break;
                    case EdgeLabelPlacement.Hide:
                        if (((i == 0 || (axis.IsInversed && i == len - 1)) && Convert.ToDouble(options.Y, Culture) > rect.Y + rect.Height) || (((i == len - 1) || (axis.IsInversed && i == 0)) && (Convert.ToDouble(options.Y, Culture) - (elementSize.Height * 0.5)) < rect.Y))
                        {
                            options.Text = string.Empty;
                        }

                        break;
                    case EdgeLabelPlacement.Shift:
                        if ((i == 0 || (axis.IsInversed && i == len - 1)) && Convert.ToDouble(options.Y, Culture) > rect.Y + rect.Height)
                        {
                            pointY = rect.Y + rect.Height;
                            options.Y = pointY.ToString(Culture);
                        }
                        else if (((i == len - 1) || (axis.IsInversed && i == 0)) && ((Convert.ToDouble(options.Y, Culture) - (elementSize.Height * 0.5)) < rect.Y))
                        {
                            pointY = rect.Y + (elementSize.Height * 0.5);
                            options.Y = pointY.ToString(Culture);
                        }

                        break;
                }

                string[] locations = ChartHelper.AppendTextElements(axis.Renderer.Owner, options.Id, Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture));
                options.X = locations[0];
                options.Y = locations[1];
                options.Font = labelStyle;
                axis.Renderer.AxisRenderInfo.AxisLabelOptions.Add(options);
            }
        }

        private void CalculateYAxisTitle(ChartAxis axis, int index, Rect rect)
        {
            ChartFontOptions titleStyle = axis.TitleStyle.GetChartFontOptions();
            double labelRotation = axis.OpposedPosition ? 90 : -90;
            double scrollBarHeight = axis.ScrollBarHeight;
            double padding = (axis.TickPosition == AxisPosition.Inside ? 0 : axis.MajorTickLines.Height + 5) + (axis.LabelPosition == AxisPosition.Inside ? 0 : axis.Renderer.MaxLabelSize.Width + axis.Renderer.MultiLevelLabelHeight + 5);
            padding = axis.OpposedPosition ? padding + scrollBarHeight : -padding - scrollBarHeight;
            double titleSize = ChartHelper.MeasureText(axis.Title, titleStyle).Height * (axis.Renderer.TitleCollection.Count - 1);
            double x = (rect.X + padding);
            double y = (rect.Y + (rect.Height * 0.5));
            axis.Renderer.AxisRenderInfo.AxisTitleOption = new TextOptions(
                x.ToString(Culture),
                (y - axis.LabelPadding - titleSize).ToString(Culture),
                axis.TitleStyle.Color ?? Chart.ChartThemeStyle.AxisTitle,
                GetFontOptions(titleStyle),
                axis.Title,
                "middle",
                axis.Renderer.Owner.ID + "_AxisTitle_" + index,
                "rotate(" + labelRotation.ToString(Culture) + "," + x.ToString(Culture) + "," + y.ToString(Culture) + ")",
                labelRotation.ToString(Culture),
                "undefined",
                !string.IsNullOrEmpty(axis.Description) ? axis.Description : axis.Title,
                axis.TabIndex.ToString(Culture));
            axis.Renderer.AxisRenderInfo.AxisTitleOption.TextCollection = axis.Renderer.TitleCollection;
            axis.Renderer.AxisRenderInfo.AxisTitleOption.Font = titleStyle;
        }

        private void CalculateXAxisTitle(ChartAxis axis, int index, Rect rect)
        {
            ChartFontOptions titleStyle = axis.TitleStyle.GetChartFontOptions();
            Size elementSize = ChartHelper.MeasureText(axis.Title, titleStyle);
            double scrollBarHeight = axis.CrossesAt == null ? axis.ScrollBarHeight : 0;
            double titleSize = ChartHelper.MeasureText(axis.Title, titleStyle).Height * (axis.Renderer.TitleCollection.Count - 1);
            double padding = (axis.TickPosition == AxisPosition.Inside ? 5 : axis.MajorTickLines.Height + 5) + (axis.LabelPosition == AxisPosition.Inside ? 5 : axis.Renderer.MaxLabelSize.Height + axis.Renderer.MultiLevelLabelHeight + 5);
            padding = axis.OpposedPosition ? -(padding + (elementSize.Height * 0.25) + scrollBarHeight + titleSize) : (padding + ((elementSize.Height * 0.75) + scrollBarHeight));
            TextOptions titleOption = new TextOptions(
                (rect.X + (rect.Width * 0.5)).ToString(Culture),
                (rect.Y + padding).ToString(Culture),
                axis.TitleStyle.Color != null ? axis.TitleStyle.Color : Chart.ChartThemeStyle.AxisTitle,
                GetFontOptions(titleStyle),
                axis.Title,
                "middle",
                axis.Renderer.Owner.ID + "_AxisTitle_" + index,
                string.Empty,
                "0",
                "undefined",
                !string.IsNullOrEmpty(axis.Description) ? axis.Description : axis.Title,
                axis.TabIndex.ToString(Culture));
            titleOption.TextCollection = axis.Renderer.TitleCollection;
            titleOption.Font = titleStyle;
            axis.Renderer.AxisRenderInfo.AxisTitleOption = titleOption;
        }

        internal void CalculateXAxisLabels(ChartAxis axis, int index, Rect rect)
        {
            double pointY, labelWidth;
            SfChart chart = axis.Renderer.Owner;
            bool islabelInside = axis.LabelPosition == AxisPosition.Inside;
            double tickSpace = axis.LabelPosition == axis.TickPosition ? axis.MajorTickLines.Height : 0;
            double padding = tickSpace + axis.LabelPadding + (axis.LineStyle.Width * 0.5);
            double angle = axis.Renderer.Angle % 360;
            double previousEnd = axis.IsInversed ? (rect.X + rect.Width) : rect.X;

            double scrollBarHeight = axis.ScrollbarSettings.Enable || (!islabelInside && axis.CrossesAt == null && (axis.ZoomFactor < 1 || axis.ZoomPosition > 0)) ? axis.ScrollBarHeight : 0;
            List<ChartInternalLocation[]> newPoints = new List<ChartInternalLocation[]>();
            bool isRotatedLabelIntersect = false;
            padding += (angle == 90 || angle == 270 || angle == -90 || angle == -270) ? ((islabelInside && !axis.OpposedPosition) && (!islabelInside && axis.OpposedPosition) ? 5 : -5) : 0;
            bool isEndAnchor =  (((!axis.OpposedPosition && !islabelInside) || (axis.OpposedPosition && islabelInside)) ? ((angle <= 360 && angle >= 180) || (angle <= -1 && angle >= -180)) : ((angle >= 1 && angle <= 180) || (angle <= -181 && angle >= -360)));
            isEndAnchor = Chart.EnableRTL ? !isEndAnchor : isEndAnchor;
            for (int i = 0, len = axis.Renderer.VisibleLabels.Count; i < len; i++)
            {
                VisibleLabels label = axis.Renderer.VisibleLabels[i];
                ChartFontOptions labelStyle = label.LabelStyle.GetChartFontOptions();
                bool isAxisBreakLabel = ChartHelper.IsBreakLabel(label.OriginalText);
                double pointX = (ChartHelper.ValueToCoefficient(label.Value, axis.Renderer) * rect.Width) + rect.X;
                double intervalLength = rect.Width / axis.Renderer.VisibleLabels.Count;
                labelWidth = isAxisBreakLabel ? label.BreakLabelSize.Width : label.Size.Width;
                double width = ((axis.LabelIntersectAction == LabelIntersectAction.Trim || axis.LabelIntersectAction == LabelIntersectAction.Wrap) && angle == 0 && labelWidth > intervalLength) ? intervalLength : labelWidth;
                double labelHeight = label.Size.Height / 4;
                pointX -= (angle == 0) ? (width / 2) : (angle == -90 || angle == 270 ? -labelHeight : (angle == 90 || angle == -270) ? labelHeight : 0);
                if (islabelInside && angle != 0)
                {
                    pointY = axis.OpposedPosition ? (rect.Y + padding + labelHeight) : (rect.Y - padding - labelHeight);
                }
                else
                {
                    double labelPadding = ((axis.OpposedPosition && !islabelInside) || (!axis.OpposedPosition && islabelInside)) ?
                        (-(padding + (angle != 0 ? labelHeight : (label.Index > 1 ? (2 * labelHeight) : 0))) * label.Index) - scrollBarHeight :
                    ((padding + ((angle != 0 ? 1 : 3) * labelHeight)) * label.Index) + scrollBarHeight;
                    pointY = rect.Y + labelPadding;
                }

                TextOptions options = new TextOptions(pointX.ToString(Culture), pointY.ToString(Culture), label.LabelStyle.Color ?? Chart.ChartThemeStyle.AxisLabel, GetFontOptions(labelStyle), string.Empty, isEndAnchor ? "end" : string.Empty, chart.ID + index + "_AxisLabel_" + i);
                if (angle == 0)
                {
                    switch (axis.EdgeLabelPlacement)
                    {
                        case EdgeLabelPlacement.None:
                            break;
                        case EdgeLabelPlacement.Hide:
                            if (((i == 0 || (axis.IsInversed && i == len - 1)) && Convert.ToDouble(options.X, Culture) < rect.X) || ((i == len - 1 || (axis.IsInversed && i == 0)) && (Convert.ToDouble(options.X, Culture) + width > rect.X + rect.Width)))
                            {
                                continue;
                            }

                            break;
                        case EdgeLabelPlacement.Shift:
                            if ((i == 0 || (axis.IsInversed && i == len - 1)) && Convert.ToDouble(options.X, Culture) < rect.X)
                            {
                                intervalLength -= rect.X - Convert.ToDouble(options.X, Culture);
                                options.X = rect.X.ToString(Culture);
                                pointX = rect.X;
                            }
                            else if ((i == len - 1 || (axis.IsInversed && i == 0)) && ((Convert.ToDouble(options.X, Culture) + width) > rect.X + rect.Width))
                            {
                                if (label.Size.Width > intervalLength && axis.LabelIntersectAction == LabelIntersectAction.Trim)
                                {
                                    intervalLength -= Convert.ToDouble(options.X, Culture) + width - (rect.X + rect.Width);
                                }
                                else
                                {
                                    intervalLength = width;
                                }

                                options.X = (rect.X + rect.Width - intervalLength).ToString(Culture);
                                pointX = rect.X + rect.Width - intervalLength;
                            }

                            break;
                    }
                }

                List<string> text = GetLabelText(label, axis, intervalLength);
                options.Text = text.FirstOrDefault();
                if (ChartHelper.IsBreakLabel(label.OriginalText) || (axis.LabelIntersectAction == LabelIntersectAction.Wrap))
                {
                    options.TextCollection = text;
                }

                if (angle == 0 && axis.LabelIntersectAction == LabelIntersectAction.Hide && i != 0 && (!axis.IsInversed ? Convert.ToDouble(options.X, Culture) <= previousEnd : Convert.ToDouble(options.X, Culture) + width >= previousEnd))
                {
                    continue;
                }

                previousEnd = axis.IsInversed ? Convert.ToDouble(options.X, Culture) : Convert.ToDouble(options.X, Culture) + width;
                if (angle != 0)
                {
                    options.Transform = "rotate(" + angle.ToString(Culture) + "," + pointX.ToString(Culture) + "," + pointY.ToString(Culture) + ')';
                    options.Y = (isAxisBreakLabel ? Convert.ToDouble(options.Y, Culture) + (axis.OpposedPosition ? (4 * label.TextArr.Length) : -(4 * label.TextArr.Length)) : Convert.ToDouble(options.Y, Culture)).ToString(Culture);
                    double height = pointY - Convert.ToDouble(options.Y, Culture) - ((label.Size.Height / 2) + 10);
                    newPoints.Add(GetRotatedRectangleCoordinates(GetRectanglePoints(new Rect(Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture) - ((label.Size.Height / 2) - 5), label.Size.Width, height)), pointX, pointY - (height / 2), angle));
                    isRotatedLabelIntersect = false;
                    for (int index1 = i; index1 > 0; index1--)
                    {
                        if (newPoints[i] != null && newPoints[index1 - 1] != null && IsRotatedRectIntersect(newPoints[i], newPoints[index1 - 1]))
                        {
                            isRotatedLabelIntersect = true;
                            newPoints[i] = null;
                            break;
                        }
                    }
                }

                string[] locations = ChartHelper.AppendTextElements(chart, options.Id, Convert.ToDouble(options.X, Culture), Convert.ToDouble(options.Y, Culture));
                options.X = locations[0];
                options.Y = locations[1];
                options.Font = labelStyle;
                options.IsMinus = axis.OpposedPosition != (axis.LabelPosition == AxisPosition.Inside);
                options.IsRotatedLabelIntersect = isRotatedLabelIntersect;
                axis.Renderer.AxisRenderInfo.AxisLabelOptions.Add(options);
            }
        }

        private void CalculateXAxisGridLine(ChartAxis axis, int index, Rect rect)
        {
            double tempInterval, pointX;
            double tickSize = axis.OpposedPosition ? -axis.MajorTickLines.Height : axis.MajorTickLines.Height;
            double axisLineSize = axis.OpposedPosition ? -axis.LineStyle.Width * 0.5 : axis.LineStyle.Width * 0.5;
            double scrollBarHeight = axis.CrossesAt == null ? axis.OpposedPosition ? -axis.ScrollBarHeight : axis.ScrollBarHeight : 0;
            bool isCategoryType = axis.ValueType.ToString().Contains("Category", StringComparison.InvariantCulture);
            double ticksbwtLabel = (isCategoryType && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0.5 : 0;
            List<VisibleLabels> visibleLabels = axis.Renderer.VisibleLabels;
            int length = visibleLabels.Count;
            bool isTickInside = axis.TickPosition == AxisPosition.Inside;
            double ticks = isTickInside ? (rect.Y - tickSize - axisLineSize) : (rect.Y + tickSize + axisLineSize + scrollBarHeight);
            if (isCategoryType && length > 0 && axis.LabelPlacement == LabelPlacement.BetweenTicks)
            {
                length += 1;
            }

            for (int i = 0; i < length; i++)
            {
                if (axis.ValueType != ValueType.DateTimeCategory)
                {
                    tempInterval = i < visibleLabels.Count ? (visibleLabels[i] != null ? visibleLabels[i].Value - ticksbwtLabel
                        : visibleLabels[i - 1].Value + axis.Renderer.VisibleInterval - ticksbwtLabel) : visibleLabels[i - 1].Value + axis.Renderer.VisibleInterval - ticksbwtLabel;
                }
                else
                {
                    tempInterval = i < visibleLabels.Count ? (visibleLabels[i] != null ? visibleLabels[i].Value - ticksbwtLabel : axis.Renderer.VisibleRange.End) : axis.Renderer.VisibleRange.End;
                }

                pointX = (ChartHelper.ValueToCoefficient(tempInterval, axis.Renderer) * rect.Width) + rect.X;
                if (pointX >= rect.X && (rect.X + rect.Width) >= pointX)
                {
                    if (ChartHelper.Inside(tempInterval, axis.Renderer.VisibleRange) || IsBorder(axis, i, pointX))
                    {
                        string majorGridId = axis.Renderer.Owner.ID + "_MajorGridLine_" + index + "_" + i;
                        string majorGridDirection = "M " + pointX.ToString(Culture) + SPACE + (SeriesClipRect.Y + SeriesClipRect.Height).ToString(Culture) + " L " + pointX.ToString(Culture) + SPACE + SeriesClipRect.Y.ToString(Culture);
                        ChartHelper.AppendPathElements(axis.Renderer.Owner, majorGridDirection, majorGridId);
                        if (axis.MajorGridLines.Width > 0 && axis.Visible)
                        {
                            LoadDictionaryValue(axis.Renderer, Constants.MAJORGRIDLINE, new PathOptions { Id = majorGridId, Direction = majorGridDirection, StrokeDashArray = axis.MajorGridLines.DashArray, StrokeWidth = axis.MajorGridLines.Width, Stroke = axis.MajorGridLines.Color ?? Chart.ChartThemeStyle.MajorGridLine });
                        }
                    }

                    string majorTickId = axis.Renderer.Owner.ID + "_MajorTickLine_" + index + "_" + i;
                    string majorTickDirection = "M " + pointX.ToString(Culture) + SPACE + (rect.Y + axisLineSize + (isTickInside ? scrollBarHeight : 0)).ToString(Culture) + " L " + pointX.ToString(Culture) + SPACE + ticks.ToString(Culture);
                    ChartHelper.AppendPathElements(axis.Renderer.Owner, majorTickDirection, majorTickId);
                    if (axis.MajorTickLines.Width > 0 && axis.Visible)
                    {
                        LoadDictionaryValue(axis.Renderer, Constants.MAJORTICKLINE, new PathOptions { Id = majorTickId, Direction = majorTickDirection, StrokeWidth = axis.MajorTickLines.Width, Stroke = axis.MajorTickLines.Color ?? Chart.ChartThemeStyle.MajorTickLine });
                    }

                    if (axis.MinorTicksPerInterval > 0 && (axis.MinorGridLines.Width > 0 || axis.MinorTickLines.Width > 0))
                    {
                        string[] minorDirection = CalculateAxisMinorLine(axis, tempInterval, rect, i);
                        string minorId;
                        minorId = axis.Renderer.Owner.ID + "_MinorGridLine_" + index + "_" + i;
                        ChartHelper.AppendPathElements(axis.Renderer.Owner, minorDirection[0], minorId);
                        if (axis.MinorGridLines.Width > 0 && axis.Visible && !string.IsNullOrEmpty(minorDirection[0]))
                        {
                            LoadDictionaryValue(axis.Renderer, Constants.MINORGRIDLINE, new PathOptions { Id = minorId, Direction = minorDirection[0], StrokeWidth = axis.MinorGridLines.Width, Stroke = axis.MinorGridLines.Color ?? Chart.ChartThemeStyle.MinorGridLine, StrokeDashArray = axis.MinorGridLines.DashArray });
                        }

                        minorId = axis.Renderer.Owner.ID + "_MinorTickLine_" + index + "_" + i;
                        ChartHelper.AppendPathElements(axis.Renderer.Owner, minorDirection[1], minorId);
                        if (axis.MinorTickLines.Width > 0 && axis.Visible && !string.IsNullOrEmpty(minorDirection[1]))
                        {
                            LoadDictionaryValue(axis.Renderer, Constants.MINORTICKLINE, new PathOptions { Id = minorId, Direction = minorDirection[1], StrokeWidth = axis.MinorTickLines.Width, Stroke = axis.MinorTickLines.Color ?? Chart.ChartThemeStyle.MinorTickLine });
                        }
                    }
                }
            }
        }

        private string[] CalculateAxisMinorLine(ChartAxis axis, double tempInterval, Rect rect, int labelIndex)
        {
            double coor, position;
            bool isTickInside = axis.TickPosition == AxisPosition.Inside;
            List<string> direction = new List<string>();
            double tickSize = axis.OpposedPosition ? -axis.MinorTickLines.Height : axis.MinorTickLines.Height;
            double logInterval = 1;
            double logPosition = 1;
            string minorGird = string.Empty;
            string minorTick = string.Empty;
            if (axis.ValueType == ValueType.Logarithmic)
            {
                double logStart = Math.Pow(axis.LogBase, tempInterval - axis.Renderer.VisibleInterval);
                logInterval = (Math.Pow(axis.LogBase, tempInterval) - logStart) / (axis.MinorTicksPerInterval + 1);
                logPosition = logStart + logInterval;
            }

            if (axis.Renderer.Orientation == Orientation.Horizontal)
            {
                for (int j = 0; j < axis.MinorTicksPerInterval; j++)
                {
                    tempInterval = FindLogNumeric(axis, logPosition, tempInterval, labelIndex);
                    logPosition += logInterval;
                    if (ChartHelper.Inside(tempInterval, axis.Renderer.VisibleRange))
                    {
                        position = (tempInterval - axis.Renderer.VisibleRange.Start) / (axis.Renderer.VisibleRange.End - axis.Renderer.VisibleRange.Start);
                        position = Math.Ceiling((axis.IsInversed ? (1 - position) : position) * rect.Width);
                        coor = Math.Floor(position + rect.X);
                        minorGird = string.Concat(minorGird, "M " + coor.ToString(Culture) + SPACE + SeriesClipRect.Y.ToString(Culture) + "L " + coor.ToString(Culture) + SPACE + (SeriesClipRect.Y + SeriesClipRect.Height).ToString(Culture));
                        coor = Math.Floor(position + rect.X);
                        minorTick = string.Concat(minorTick, "M " + coor.ToString(Culture) + SPACE + rect.Y.ToString(Culture) + "L " + coor.ToString(Culture) + SPACE + (isTickInside ? (rect.Y - tickSize) : (rect.Y + tickSize) + axis.ScrollBarHeight).ToString(Culture));
                    }
                }
            }
            else
            {
                for (int j = 0; j < axis.MinorTicksPerInterval; j++)
                {
                    tempInterval = FindLogNumeric(axis, logPosition, tempInterval, labelIndex);
                    if (ChartHelper.Inside(tempInterval, axis.Renderer.VisibleRange))
                    {
                        position = Math.Ceiling((tempInterval - axis.Renderer.VisibleRange.Start) / (axis.Renderer.VisibleRange.End - axis.Renderer.VisibleRange.Start) * rect.Height) * -1;
                        coor = Math.Floor(position + rect.Y + rect.Height);
                        minorGird = string.Concat(minorGird, "M " + SeriesClipRect.X.ToString(Culture) + SPACE + coor.ToString(Culture) + "L " + (SeriesClipRect.X + SeriesClipRect.Width).ToString(Culture) + SPACE + coor.ToString(Culture) + SPACE);
                        coor = Math.Floor(position + rect.Y + rect.Height);
                        minorTick = string.Concat(minorTick, "M " + rect.X.ToString(Culture) + SPACE + coor.ToString(Culture) + "L " + (isTickInside ? (rect.X + tickSize) : (rect.X - tickSize) - axis.ScrollBarHeight).ToString(Culture) + SPACE + coor.ToString(Culture) + SPACE);
                    }

                    logPosition += logInterval;
                }
            }

            direction.Add(minorGird);
            direction.Add(minorTick);
            return direction.ToArray();
        }

        private void RenderGridLine(RenderTreeBuilder builder, PathOptions gridOption, ChartAxis axis)
        {
            if (gridOption.StrokeWidth > 0 && axis.Visible && !string.IsNullOrEmpty(gridOption.Direction))
            {
                SvgRenderer.RenderPath(builder, gridOption.Id, gridOption.Direction, gridOption.StrokeDashArray, gridOption.StrokeWidth, gridOption.Stroke);
            }
        }

        private bool IsBorder(ChartAxis axis, int index, double point)
        {
            bool isHorizontal = axis.Renderer.Orientation == Orientation.Horizontal;
            double start = isHorizontal ? SeriesClipRect.X : SeriesClipRect.Y;
            double size = isHorizontal ? SeriesClipRect.Width : SeriesClipRect.Height;
            ChartAreaBorder border = Chart.ChartAreaRenderer.Area.Border;
            if (axis.PlotOffset > 0)
            {
                return true;
            }
            else if ((point == start || point == (start + size)) && (border.Width <= 0 || border.Color == Constants.TRANSPARENT)) 
            {
                return true;
            }
            else if ((point != start && index == (isHorizontal ? 0 : axis.Renderer.VisibleLabels.Count - 1)) || (point != (start + size) && index == (isHorizontal ? axis.Renderer.VisibleLabels.Count - 1 : 0)))
            {
                return true;
            }

            return false;
        }

        private void CalculateXAxisBorder(ChartAxis axis, int index, Rect axisRect)
        {
            if (axis.Border.Width > 0)
            {
                ChartAxisRenderer axisRenderer = axis.Renderer;
                double scrollBarHeight = axis.LabelPosition == AxisPosition.Outside ? axis.ScrollBarHeight : 0;
                double startX, endX, pointX;
                double startY = axisRect.Y + ((axis.OpposedPosition ? -1 : 1) * scrollBarHeight), padding = 10;
                double gap = (axisRect.Width / axisRenderer.VisibleRange.Delta) * (axis.ValueType == ValueType.DateTime ? axisRenderer.DateTimeInterval : axisRenderer.VisibleInterval);
                double length = axisRenderer.MaxLabelSize.Height + ((axis.TickPosition == axis.LabelPosition) ? axis.MajorTickLines.Height : 0);
                string labelBorder = string.Empty;
                double ticksbwtLabel = (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? -0.5 : 0;
                double endY = ((axis.OpposedPosition && axis.LabelPosition == AxisPosition.Inside) || (!axis.OpposedPosition && axis.LabelPosition == AxisPosition.Outside)) ?
                    (axisRect.Y + length + padding + scrollBarHeight) : (axisRect.Y - length - padding - scrollBarHeight);
                for (int i = 0, len = axisRenderer.VisibleLabels.Count; i < len; i++)
                {
                    pointX = ChartHelper.ValueToCoefficient(axisRenderer.VisibleLabels[i].Value + ticksbwtLabel, axisRenderer);
                    pointX = (axis.IsInversed ? (1 - pointX) : pointX) * axisRect.Width;
                    if (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks)
                    {
                        startX = pointX + axisRect.X;
                        endX = pointX + gap + axisRect.X;
                    }
                    else
                    {
                        startX = pointX - (gap * 0.5) + axisRect.X;
                        endX = pointX + (gap * 0.5) + axisRect.X;
                    }

                    switch (axis.Border.Type)
                    {
                        case BorderType.Rectangle:
                        case BorderType.WithoutTopBorder:
                            if (startX < axisRect.X)
                            {
                                labelBorder += "M" + SPACE + axisRect.X.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "L" + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                            }
                            else if (Math.Floor(endX) > axisRect.Width + axisRect.X && !(axisRenderer.VisibleLabels.Count == 1))
                            {
                                labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE + startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "L" + SPACE + (axisRect.Width + axisRect.X).ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                            }
                            else
                            {
                                labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE + startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "L" + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                                if (i == 0)
                                {
                                    labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE + startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "M " + startX.ToString(Culture) + SPACE + endY.ToString(Culture) + " L " + axisRect.X.ToString(Culture) + SPACE + endY.ToString(Culture);
                                }

                                if (i == axisRenderer.VisibleLabels.Count - 1)
                                {
                                    labelBorder += "M" + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "M " + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + " L " + (axisRect.Width + axisRect.X).ToString(Culture) + SPACE + endY.ToString(Culture);
                                }
                            }

                            break;
                        case BorderType.WithoutTopandBottomBorder:
                            if (!(startX < axisRect.X) && !(Math.Floor(endX) > axisRect.Width + axisRect.X))
                            {
                                labelBorder += "M" + SPACE + startX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + "L" + SPACE + startX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + "M " + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + " L " + endX.ToString(Culture) + SPACE + endY.ToString(Culture);
                            }

                            break;
                    }
                }

                labelBorder += axis.Border.Type == BorderType.Rectangle ? ("M " + SPACE + axisRect.X.ToString(Culture) + SPACE + startY.ToString(Culture) + "L" + SPACE + (axisRect.X + axisRect.Width).ToString(Culture) + SPACE + startY.ToString(Culture)) : string.Empty;
                if (!string.IsNullOrEmpty(labelBorder))
                {
                    axisRenderer.AxisRenderInfo.AxisBorder = new PathOptions(Chart.ID + "_BorderLine_" + index, labelBorder, string.Empty, axis.Border.Width, !string.IsNullOrEmpty(axis.Border.Color) ? axis.Border.Color : Chart.ChartThemeStyle.AxisLine, 1, Constants.TRANSPARENT);
                }
            }

            if (axis.Renderer.MultiLevelLabelRenderer != null)
            {
                if (axis.MultiLevelLabels.Count > 0)
                {
                   axis.Renderer.MultiLevelLabelRenderer.CalculateXAxisMultiLevelLabels(axis.Renderer.Index, axisRect);
                }
                else
                {
                    axis.Renderer.MultiLevelLabelRenderer.ClearPathOptions();
                }
            }
        }

        private void CalculateYAxisBorder(ChartAxis axis, int index, Rect rect)
        {
            if (axis.Border.Width > 0)
            {
                ChartAxisRenderer axisRenderer = axis.Renderer;
                double startY, pointY, endY;
                double scrollBarHeight = (axis.OpposedPosition ? 1 : -1) * (axis.LabelPosition == AxisPosition.Outside ? axis.ScrollBarHeight : 0);
                double gap = (rect.Height / axisRenderer.VisibleRange.Delta) * (axis.ValueType == ValueType.DateTime ? axisRenderer.DateTimeInterval : axisRenderer.VisibleInterval);
                double length = axisRenderer.MaxLabelSize.Width + 10 + ((axis.TickPosition == axis.LabelPosition) ? axis.MajorTickLines.Height : 0);
                string labelBorder = string.Empty;
                double ticksbwtLabel = (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks) ? -0.5 : 0;
                double endX = ((axis.OpposedPosition && axis.LabelPosition == AxisPosition.Inside) || (!axis.OpposedPosition
                    && axis.LabelPosition == AxisPosition.Outside)) ? rect.X - length + scrollBarHeight : rect.X + length + scrollBarHeight;
                for (int i = 0, len = axisRenderer.VisibleLabels.Count; i < len; i++)
                {
                    pointY = ChartHelper.ValueToCoefficient(axisRenderer.VisibleLabels[i].Value + ticksbwtLabel, axisRenderer);
                    pointY = (axis.IsInversed ? (1 - pointY) : pointY) * rect.Height;
                    if (axis.ValueType == ValueType.Category && axis.LabelPlacement == LabelPlacement.BetweenTicks)
                    {
                        startY = (pointY * -1) + (rect.Y + rect.Height);
                        endY = (pointY * -1) - gap + (rect.Y + rect.Height);
                    }
                    else
                    {
                        startY = (pointY * -1) + (gap / 2) + (rect.Y + rect.Height);
                        endY = (pointY * -1) - (gap / 2) + (rect.Y + rect.Height);
                    }

                    switch (axis.Border.Type)
                    {
                        case BorderType.Rectangle:
                        case BorderType.WithoutTopBorder:
                            if (startY > (rect.Y + rect.Height))
                            {
                                labelBorder += 'M' + SPACE + endX.ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                            }
                            else if (Math.Floor(rect.Y) > endY)
                            {
                                labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE;
                            }
                            else
                            {
                                labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                                if (i == axisRenderer.VisibleLabels.Count - 1)
                                {
                                    labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE;
                                }
                            }

                            break;
                        case BorderType.WithoutTopandBottomBorder:
                            if (!(startY > rect.Y + rect.Height) && !(endY < Math.Floor(rect.Y)))
                            {
                                labelBorder += 'M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'L' + SPACE + endX.ToString(Culture) + SPACE + startY.ToString(Culture) + SPACE + 'M' + SPACE + endX.ToString(Culture) + SPACE + endY.ToString(Culture) + SPACE + 'L' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + endY.ToString(Culture);
                            }

                            break;
                    }
                }

                labelBorder += (axis.Border.Type == BorderType.Rectangle) ? ('M' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + 'L' + SPACE + (rect.X + scrollBarHeight).ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE) : string.Empty;
                if (!string.IsNullOrEmpty(labelBorder))
                {
                    axisRenderer.AxisRenderInfo.AxisBorder = new PathOptions(Chart.ID + "_BorderLine_" + index, labelBorder, string.Empty, axis.Border.Width, !string.IsNullOrEmpty(axis.Border.Color) ? axis.Border.Color : Chart.ChartThemeStyle.AxisLine, 1, Constants.TRANSPARENT);
                }
            }

            if (axis.Renderer.MultiLevelLabelRenderer != null)
            {
                if (axis.MultiLevelLabels.Count > 0)
                {
                    axis.Renderer.MultiLevelLabelRenderer.CalculateYAxisMultiLevelLabels(axis.Renderer.Index, rect);
                }
                else
                {
                    axis.Renderer.MultiLevelLabelRenderer.ClearPathOptions();
                }
            }
        }

        internal override void ClearAxes()
        {
        }
    }
}
