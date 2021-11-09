using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.RangeNavigator.Internal
{
    internal class RangeAxis : DateTimeAxisRenderer
    {
        protected const string SPACE = " ";

        private string labelsStyle = "cursor: default;user-select: none";
        private SfRangeNavigator chart;
        private RangeIntervalType actualIntervalType;
        private StringComparison comparison = StringComparison.InvariantCulture;
        private string skeleton = string.Empty;

        internal RangeAxis(SfRangeNavigator rangeNavigator)
        {
            chart = rangeNavigator;
        }

        internal List<VisibleLabels> FirstLevelLabels { get; set; } = new List<VisibleLabels>();

        internal List<VisibleLabels> SecondLevelLabels { get; set; } = new List<VisibleLabels>();

        internal List<double> LowerValues { get; set; }

        private static RangeIntervalType GetSecondaryLabelType(RangeIntervalType type)
        {
            RangeIntervalType[] types = new RangeIntervalType[] { RangeIntervalType.Years, RangeIntervalType.Quarter, RangeIntervalType.Months, RangeIntervalType.Weeks, RangeIntervalType.Days, RangeIntervalType.Hours, RangeIntervalType.Minutes, RangeIntervalType.Seconds };
            return type == RangeIntervalType.Years ? RangeIntervalType.Years : types[types.IndexOf(type) - 1];
        }

        private string FindCustomFormats(RangeIntervalType intervalType, string labelFormat)
        {
            if (string.IsNullOrEmpty(labelFormat))
            {
                labelFormat = intervalType == RangeIntervalType.Years ? (IsIntervalInDecimal ? "yyyy" : "MMM y") : (intervalType == RangeIntervalType.Days && !IsIntervalInDecimal) ? "ddd HH tt" : string.Empty;
            }

            return string.IsNullOrEmpty(labelFormat) ? GetSkeleton(skeleton, intervalType) : labelFormat;
        }

        private static double FindLabelY(SfRangeNavigator control, bool isSecondary)
        {
            double reference = control.InitialClipRect.Y + control.InitialClipRect.Height, pointY, tickHeight = control.MajorTickLines.Height, padding = 8,
            textHeight = ChartHelper.MeasureText("Quarter1 2011", SfRangeNavigator.GetFontOptions(control.LabelStyle)).Height;
            if ((control.LabelPosition == AxisPosition.Outside && control.TickPosition == AxisPosition.Outside) || control.Series.Count == 0)
            {
                pointY = reference + tickHeight + padding + (textHeight * 0.75);
            }
            else if (control.LabelPosition == AxisPosition.Inside && control.TickPosition == AxisPosition.Inside)
            {
                pointY = reference - tickHeight - padding;
            }
            else if (control.LabelPosition == AxisPosition.Inside && control.TickPosition == AxisPosition.Outside)
            {
                pointY = reference - padding;
            }
            else
            {
                pointY = reference + padding + (textHeight * 0.75);
            }

            if (isSecondary)
            {
                padding = 15;
                if (control.LabelPosition == AxisPosition.Outside || control.Series.Count == 0)
                {
                    pointY += padding + (textHeight * 0.75);
                }
                else
                {
                    pointY = (control.TickPosition == AxisPosition.Outside || control.Series.Count == 0) ? reference + tickHeight + padding + (textHeight * 0.75) : reference + padding + (textHeight * 0.75);
                }
            }

            return pointY;
        }

        private static bool IsIntersect(bool isInversed, double currentX, double currentWidth, double prevX, double prevWidth)
        {
            return isInversed ? ((currentX + (currentWidth / 2)) > (prevX - (prevWidth / 2))) : ((currentX - (currentWidth / 2)) < (prevX + (prevWidth / 2)));
        }

        protected static string GetSkeleton(string skeleton, RangeIntervalType intervalType)
        {
            if (!string.IsNullOrEmpty(skeleton))
            {
                return skeleton;
            }

            if (intervalType == RangeIntervalType.Years || intervalType == RangeIntervalType.Quarter)
            {
                return "y";
            }
            else if (intervalType == RangeIntervalType.Months || intervalType == RangeIntervalType.Weeks)
            {
                return "m";
            }
            else if (intervalType == RangeIntervalType.Days)
            {
                return "d";
            }
            else if (intervalType == RangeIntervalType.Hours)
            {
                return "t";
            }
            else
            {
                return "T";
            }
        }

        internal void ProcessYAxis(SfRangeNavigator control)
        {
            ChartAxis axis = new ChartAxis
            {
#pragma warning disable BL0005
                ValueType = ValueType.Double,
                MajorGridLines = new ChartAxisMajorGridLines() { Width = 0 },
                MajorTickLines = new ChartAxisMajorTickLines() { Width = 0 },
                RangePadding = ChartRangePadding.None,
                LabelStyle = new ChartAxisLabelStyle() { Size = "0" },
                Visible = false
            };
            control.YAxisRenderer = new NumericAxisRenderer();
            control.YAxisRenderer.Axis = axis;
            control.YAxisRenderer.Orientation = Orientation.Vertical;
            control.YAxisRenderer.Axis.MaximumLabels = 3;
            if(!control.IsStockChart)
            {
                Axis = control.YAxisRenderer.Axis;
                Axis.Renderer = control.YAxisRenderer;
            }
            
            skeleton = control.RangeNavigatorSkeleton;
            control.YAxisRenderer.IntervalDivs = new double[] { 10, 5, 2, 1 };
            control.YAxisRenderer.Rect = control.InitialClipRect;
            AxisAvailabelSize = control.AvailableSize;
            control.YAxisRenderer.VisibleLabels = new List<VisibleLabels>();
            control.YAxisRenderer.Orientation = Orientation.Vertical;
            GetActualRange(control.YAxisRenderer, control.ChartSeries.YMin, control.ChartSeries.YMax);
            control.YAxisRenderer.VisibleRange = control.YAxisRenderer.ActualRange;
        }

        private static DoubleRange InitializeDoubleRange(ChartAxisRenderer axisRenderer, double min, double max)
        {
            min = axisRenderer.Axis.Minimum != null ? Convert.ToDouble(axisRenderer.Axis.Minimum, null) : (double.IsNaN(min) || double.IsPositiveInfinity(min)) ? 0 : min;
            max = axisRenderer.Axis.Maximum != null ? Convert.ToDouble(axisRenderer.Axis.Maximum, null) : (double.IsNaN(max) || double.IsNegativeInfinity(max)) ? 5 : max;
            if (min == max)
            {
                max = min + 1;
            }

            DoubleRange range = new DoubleRange(min, max);
            return range;
        }

        private void GetActualRange(ChartAxisRenderer axis, double min, double max)
        {
            DoubleRange range = InitializeDoubleRange(axis, min, max);
            axis.ActualInterval = axis.Axis.Interval > 0 && !double.IsNaN(axis.Axis.Interval) ? axis.Axis.Interval :CalculateNumericNiceInterval(range.Delta);
            axis.ActualRange = range;
            axis.ActualInterval = !ChartHelper.IsNaNOrZero(axis.Axis.Interval) ? axis.Axis.Interval : axis.ActualInterval;

            axis.ActualRange = new DoubleRange(
                axis.Axis.Minimum != null ? Convert.ToDouble(axis.Axis.Minimum, null) : range.Start,
                axis.Axis.Maximum != null ? Convert.ToDouble(axis.Axis.Maximum, null) : range.End
                );
        }

        internal void ProcessXAxis(SfRangeNavigator control)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            ChartAxis axis = new ChartAxis
            {
                Maximum = control.Maximum,
                Minimum = control.Minimum,
                Interval = control.Interval,
                ValueType = (ValueType)Enum.Parse(typeof(ValueType), control.ValueType.ToString()),
                IsInversed = control.IsRtlEnabled(),
                LabelFormat = string.IsNullOrEmpty(control.LabelFormat) && control.UseGroupingSeparator ? "N0" : control.LabelFormat,
                LogBase = control.LogBase
            };
#pragma warning restore CA2000 // Dispose objects before losing scope
            skeleton = control.RangeNavigatorSkeleton;
            if (control.ValueType == RangeValueType.Double)
            {
                control.XAxisRenderer = new NumericAxisRenderer();
                SetAxisRenderValue(axis);
                GetActualRange(control.XAxisRenderer, control.ChartSeries.XMin, control.ChartSeries.XMax);
                control.XAxisRenderer.VisibleInterval = control.XAxisRenderer.ActualInterval = control.XAxisRenderer.CalculateActualInterval(control.XAxisRenderer.ActualRange);
                control.XAxisRenderer.VisibleRange = control.XAxisRenderer.ActualRange;
                control.XAxisRenderer.GenerateVisibleLabels();
            }

            if (control.ValueType == RangeValueType.Logarithmic)
            {
                control.XAxisRenderer = new LogarithmicAxisRenderer();
                SetAxisRenderValue(axis);
                GetActualLogRange(control.XAxisRenderer, control.ChartSeries.XMin, control.ChartSeries.XMax);
                control.XAxisRenderer.VisibleInterval = control.XAxisRenderer.ActualInterval = control.XAxisRenderer.CalculateActualInterval(control.XAxisRenderer.ActualRange);
                control.XAxisRenderer.VisibleRange = control.XAxisRenderer.ActualRange;
                control.XAxisRenderer.GenerateVisibleLabels();
            }

            if (control.ValueType == RangeValueType.DateTime)
            {
                labelsStyle = "cursor: pointer;user-select: none";
                control.XAxisRenderer = new DateTimeAxisRenderer();
                control.XAxisRenderer.Min = control.ChartSeries.XMin;
                control.XAxisRenderer.Max = control.ChartSeries.XMax;
                SetAxisRenderValue(axis);
                control.XAxisRenderer.ActualRange = control.XAxisRenderer.InitializeDoubleRange();
                control.XAxisRenderer.VisibleInterval = control.XAxisRenderer.ActualInterval = control.XAxisRenderer.CalculateActualInterval(control.XAxisRenderer.ActualRange);
                control.XAxisRenderer.VisibleRange = control.XAxisRenderer.ActualRange;
                control.XAxisRenderer.CalculateVisibleRange(control.XAxisRenderer.ActualRange);
            }
            control.ChartSeries.XAxisRenderer = control.XAxisRenderer;
        }

        private void SetAxisRenderValue(ChartAxis axis)
        {
            chart.XAxisRenderer.Axis = axis;
            chart.XAxisRenderer.Axis.IntervalType = (IntervalType)chart.IntervalType;
            chart.XAxisRenderer.ActualIntervalType = (IntervalType)chart.IntervalType;
            chart.XAxisRenderer.Axis.MaximumLabels = 3;
            Axis = chart.XAxisRenderer.Axis;
            Axis.Renderer = chart.XAxisRenderer;
            chart.XAxisRenderer.Orientation = Orientation.Horizontal;
            chart.XAxisRenderer.IntervalDivs = new double[] { 10, 5, 2, 1 };
            chart.XAxisRenderer.Rect = chart.InitialClipRect;
            AxisAvailabelSize = chart.XAxisRenderer.AxisAvailabelSize = chart.AvailableSize;
            chart.XAxisRenderer.VisibleLabels = new List<VisibleLabels>();
            chart.XAxisRenderer.Orientation = Orientation.Horizontal;
        }
        private void GetActualLogRange(ChartAxisRenderer axis, double min, double max)
        {
            DoubleRange range = InitializeDoubleRange(axis, min, max);
            double logStart = ChartHelper.LogBase(range.Start, axis.Axis.LogBase);
            logStart = double.IsFinite(logStart) ? logStart : range.Start;
            double logEnd = double.IsFinite(logStart) ? range.End == 1 ? 1 : ChartHelper.LogBase(range.End, axis.Axis.LogBase) : range.End;
            axis.ActualInterval = !double.IsNaN(axis.Axis.Interval) ? axis.Axis.Interval : CalculateLogNiceInterval(range.End - range.Start);

            axis.ActualRange = new DoubleRange(Math.Floor(logStart / 1), Math.Ceiling(logEnd / 1));
            chart.XAxisRenderer.Min = axis.ActualRange.Start;
            chart.XAxisRenderer.Max = axis.ActualRange.End;
        }

        private double CalculateLogNiceInterval(double delta)
        {
            double niceInterval = delta;
            double minInterval = Math.Pow(10, Math.Floor(ChartHelper.LogBase(niceInterval, 10)));
            for (int j = 0, len = IntervalDivs.Length; j < len; j++)
            {
                double currentInterval = minInterval * IntervalDivs[j];
                if (ChartHelper.GetActualDesiredIntervalsCount(AxisAvailabelSize, Axis.DesiredIntervals, Orientation, Axis.MaximumLabels) < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }
        private string PlaceAxisLabels(ChartAxisRenderer axis, double pointY, string id, RenderTreeBuilder builder, RangeIntervalType intervalType)
        {
            int maxLabels = axis.VisibleLabels.Count;
            VisibleLabels label, prevLabel = null;
            Rect rect = chart.InitialClipRect;
            string border = string.Empty, disabledColor = chart.DisableRangeSelector ? "transparent" : null;
            double prevX = chart.IsRtlEnabled() ? (rect.X + rect.Width) : rect.X, pointX = 0,
            intervalInTime = chart.ValueType == RangeValueType.DateTime ? (maxLabels > 1) ? (axis.VisibleLabels[1].Value - axis.VisibleLabels[0].Value) : (axis.VisibleRange.End - axis.VisibleLabels[0].Value) / 2 : 0;
            if (chart.ValueType == RangeValueType.DateTime && (intervalType == RangeIntervalType.Quarter || intervalType == RangeIntervalType.Weeks))
            {
                FindSuitableFormat(axis, intervalType);
            }

            for (int i = 0, len = maxLabels; i < len; i++)
            {
                label = axis.VisibleLabels[i];
                label.Size = ChartHelper.MeasureText(label.Text, GetFontOptions(axis.Axis.LabelStyle));
                if (chart.SecondaryLabelAlignment == LabelAlignment.Middle)
                {
                    pointX = (RangeNavigatorHelper.ValueToCoefficient(label.Value + (intervalInTime / 2), axis) * rect.Width) + rect.X;
                }
                else if (id.Contains("Secondary", StringComparison.InvariantCulture))
                {
                    pointX = FindAlignment(axis, i);
                }

                // Here we calcuated for EdgelabelPlacements
                if ((i == 0 || (i == axis.VisibleLabels.Count - 1 && chart.IsRtlEnabled())) && pointX < rect.X)
                {
                    pointX = rect.X + (label.Size.Width / 2);
                }

                if ((i == axis.VisibleLabels.Count - 1 || (i == 0 && chart.IsRtlEnabled())) && ((pointX + (label.Size.Width / 2)) > (rect.X + rect.Width)))
                {
                    pointX = rect.X + rect.Width - (label.Size.Width / 2);
                    label.Text = SPACE;
                }

                // Here we calculated for smart axis label position,
                if (chart.LabelIntersectAction == RangeLabelIntersectAction.Hide && i != 0 && IsIntersect(axis.Axis.IsInversed, pointX, label.Size.Width, prevX, prevLabel.Size.Width))
                {
                    continue;
                }

                // Here we calculated for label alignment for single visible label
                if (chart.SecondaryLabelAlignment == LabelAlignment.Middle && axis.VisibleLabels.Count == 1)
                {
                    pointX = ChartHelper.ValueToCoefficient(label.Value, axis) + (rect.X + (rect.Width / 2));
                }

                ChartCommonFont labelStyle = chart.LabelStyle;
                RangeLabelRenderEventArgs argsData = new RangeLabelRenderEventArgs
                {
                    EventName = RangeConstants.LABELRENDER,
                    Text = chart.UseGroupingSeparator ? label.Text : label.Text.Replace(",", string.Empty, StringComparison.InvariantCulture),
                    Value = label.Value,
                    LabelStyle = new ChartCommonFont
                    {
                        Size = labelStyle.Size,
                        Color = disabledColor ?? labelStyle.Color ?? chart.ThemeStyle.LabelFontColor,
                        FontFamily = labelStyle.FontFamily,
                        FontStyle = labelStyle.FontStyle,
                        FontWeight = labelStyle.FontWeight,
                        Opacity = labelStyle.Opacity,
                        TextAlignment = labelStyle.TextAlignment,
                        TextOverflow = labelStyle.TextOverflow
                    },
                    Region = new Rect(pointX, pointY, label.Size.Width, label.Size.Height)
                };
                if (chart.RangeNavigatorEvents?.LabelRender != null)
                {
                    SfRangeNavigator.InvokeEvent<RangeLabelRenderEventArgs>(chart.RangeNavigatorEvents?.LabelRender, argsData);
                }

                if (!argsData.Cancel)
                {
                    chart.Labels.Add(argsData);
                }
                else
                {
                    return null;
                }

                chart.SvgRenderer.RenderText(builder, new TextOptions(pointX.ToString(culture), pointY.ToString(culture), argsData.LabelStyle.Color ?? chart.ThemeStyle.LabelFontColor, argsData.LabelStyle.GetFontOptions(), argsData.Text, "middle", chart.Id + id + i, string.Empty, "0", "undefined", string.Empty, string.Empty, labelsStyle));
                prevX = pointX;
                prevLabel = label;
            }

            return border;
        }

        internal void RenderGridLines(RenderTreeBuilder builder, ChartAxisRenderer axis)
        {
            RangeNavigatorMajorGridLines majorGridLines = chart.MajorGridLines;
            RangeNavigatorMajorTickLines majorTickLines = chart.MajorTickLines;
            string majorGrid = string.Empty, majorTick = string.Empty;
            Rect rect = chart.InitialClipRect;
            double tempInterval, labelLength, axisLineSize = axis.Axis.OpposedPosition ? -axis.Axis.LineStyle.Width * 0.5 : axis.Axis.LineStyle.Width * 0.5,
            tick = (chart.TickPosition == AxisPosition.Outside || chart.Series.Count == 0) ? rect.Y + rect.Height + majorTickLines.Height : rect.Y + rect.Height - majorTickLines.Height;
            chart.SvgRenderer.OpenGroupElement(builder, chart.Id + "_GridLines");
            FirstLevelLabels = new List<VisibleLabels>();
            // axis.Axis.LabelStyle = JsonSerializer.Deserialize<ChartAxisLabelStyle>(JsonSerializer.Serialize(chart.LabelStyle));
            skeleton = chart.RangeNavigatorSkeleton;
            if (chart.ValueType == RangeValueType.DateTime)
            {
                Axis.IntervalType = (IntervalType)chart.IntervalType;
                CalculateDateTimeNiceInterval(axis.ActualRange.Start, axis.ActualRange.End, false);
                axis.ActualIntervalType = ActualIntervalType;
                actualIntervalType = chart.IntervalType != RangeIntervalType.Auto ? (RangeIntervalType)ActualIntervalType :
                    (RangeIntervalType)Enum.Parse(typeof(RangeIntervalType), ActualIntervalType.ToString());
                FindAxisLabels(axis, actualIntervalType, chart.LabelFormat);
            }

            FirstLevelLabels = axis.VisibleLabels;
            LowerValues = new List<double>();
            labelLength = axis.VisibleLabels.Count;
            for (int i = 0; i < labelLength; i++)
            {
                LowerValues.Add(FirstLevelLabels[i].Value);
                tempInterval = axis.VisibleLabels[i] != null ? axis.VisibleLabels[i].Value : (axis.VisibleLabels[i - 1].Value + axis.VisibleInterval);
                double pointX = (ChartHelper.ValueToCoefficient(FirstLevelLabels[i].Value, axis) * rect.Width) + rect.X;
                if (pointX >= rect.X && (rect.X + rect.Width) >= pointX)
                {
                    if (ChartHelper.Inside(tempInterval, axis.VisibleRange) || IsBorder(axis, i, pointX))
                    {
                        majorGrid = string.Concat(majorGrid, "M" + SPACE + pointX.ToString(culture) + SPACE + (chart.InitialClipRect.Y + chart.InitialClipRect.Height).ToString(culture) + SPACE + "L" + SPACE + pointX.ToString(culture) + SPACE + chart.InitialClipRect.Y.ToString(culture));
                    }

                    majorTick = string.Concat(majorTick, "M" + SPACE + pointX.ToString(culture) + SPACE + (rect.Y + axisLineSize + rect.Height).ToString(culture) + SPACE + "L" + SPACE + pointX.ToString(culture) + SPACE + tick.ToString(culture));
                }
            }

            RenderGridLine(axis, 0, majorGrid, new ChartBorderType { Width = majorGridLines.Width, Color = majorGridLines.Color }, "_MajorGridLine_", 0, builder, majorGridLines.Color ?? chart.ThemeStyle.GridLineColor, majorGridLines.DashArray);
            RenderGridLine(axis, 0, majorTick, new ChartBorderType { Width = majorTickLines.Width, Color = majorTickLines.Color }, "_MajorTickLine_", 0, builder, majorTickLines.Color ?? chart.ThemeStyle.GridLineColor);
            builder.CloseElement();
        }

        internal void RenderAxisLabels(RenderTreeBuilder builder)
        {
            ChartAxisRenderer axis = chart.XAxisRenderer;
            chart.SvgRenderer.OpenGroupElement(builder, chart.Id + "AxisLabels");
            chart.SvgRenderer.OpenGroupElement(builder, chart.Id + "_FirstLevelAxisLabels");
            ChartAxisRenderer secondaryAxis = axis;
            secondaryAxis.Axis.LabelFormat = chart.LabelFormat;
            double pointY = FindLabelY(chart, false);
            PlaceAxisLabels(axis, pointY, "_AxisLabel_", builder, actualIntervalType);
            secondaryAxis.Axis.LabelFormat = string.Empty;
#pragma warning restore BL0005
            if (chart.EnableGrouping && chart.ValueType == RangeValueType.DateTime && actualIntervalType != RangeIntervalType.Years)
            {
                RangeIntervalType secondaryIntervalType = chart.GroupBy != RangeIntervalType.Auto ? chart.GroupBy : GetSecondaryLabelType(actualIntervalType);
                chart.SvgRenderer.OpenGroupElement(builder, chart.Id + "_SecondLevelAxisLabels");
                secondaryAxis.VisibleInterval = 1;
                secondaryAxis.VisibleLabels = new List<VisibleLabels>();
                FindAxisLabels(secondaryAxis, secondaryIntervalType, secondaryAxis.Axis.LabelFormat);
                SecondLevelLabels = secondaryAxis.VisibleLabels;
                pointY = FindLabelY(chart, true);
                RenderSecondaryGridLine(secondaryAxis, pointY, "_SecondaryLabel_", builder);
                PlaceAxisLabels(secondaryAxis, pointY, "_SecondaryLabel_", builder, secondaryIntervalType);
                builder.CloseElement();
            }

            chart.ChartSeries.XAxisRenderer.VisibleLabels = chart.ChartSeries.XAxisRenderer.VisibleLabels.Concat(secondaryAxis.VisibleLabels).Distinct().ToList();
            builder.CloseElement();
            builder.CloseElement();
        }

        private void RenderSecondaryGridLine(ChartAxisRenderer axis, double pointY, string id, RenderTreeBuilder builder)
        {
            int maxLabels = axis.VisibleLabels.Count;
            VisibleLabels label;
            Rect rect = chart.InitialClipRect;
            string border = string.Empty, disabledColor = chart.DisableRangeSelector ? "transparent" : null;
            double pointX = 0, pointXGrid,
            intervalInTime = chart.ValueType == RangeValueType.DateTime ? (maxLabels > 1) ? (axis.VisibleLabels[1].Value - axis.VisibleLabels[0].Value) : (axis.VisibleRange.End - axis.VisibleLabels[0].Value) / 2 : 0;
            for (int i = 0, len = maxLabels; i < len; i++)
            {
                label = axis.VisibleLabels[i];
                label.Size = ChartHelper.MeasureText(label.Text, GetFontOptions(axis.Axis.LabelStyle));
                if (chart.SecondaryLabelAlignment == LabelAlignment.Middle)
                {
                    pointX = (ChartHelper.ValueToCoefficient(label.Value + (intervalInTime / 2), axis) * rect.Width) + rect.X;
                }
                else if (id.Contains("Secondary", StringComparison.InvariantCulture))
                {
                    pointX = FindAlignment(axis, i);
                }

                pointXGrid = (ChartHelper.ValueToCoefficient(label.Value, axis) * rect.Width) + rect.X;
                if ((i == 0 || (i == axis.VisibleLabels.Count - 1 && chart.IsRtlEnabled())) && pointX < rect.X)
                {
                    pointX = rect.X + (label.Size.Width / 2);
                }

                if ((i == axis.VisibleLabels.Count - 1 || (i == 0 && chart.IsRtlEnabled())) && ((pointX + label.Size.Width) > (rect.X + rect.Width)))
                {
                    pointX = rect.X + rect.Width - (label.Size.Width / 2);
                }

                // Here we calculated for secondary axis grid lines
                if (id.Contains("_SecondaryLabel_", StringComparison.InvariantCulture) && pointX >= rect.X && (rect.X + rect.Width) >= pointX)
                {
                    border = border + "M" + SPACE + pointXGrid.ToString(culture) + SPACE + pointY.ToString(culture) + SPACE + "L" + SPACE + pointXGrid.ToString(culture) + SPACE + (pointY - label.Size.Height).ToString(culture);
                }
            }

            RenderGridLine(axis, 1, border, new ChartBorderType { Width = axis.Axis.MajorGridLines.Width, Color = axis.Axis.MajorGridLines.Color }, "_SecondaryMajorLines", 0, builder, chart.MajorGridLines.Color ?? chart.ThemeStyle.GridLineColor, chart.MajorGridLines.DashArray);
        }

        internal void CalculateGroupingBounds()
        {
            double padding = chart.Margin.Bottom, labelHeight = ChartHelper.MeasureText("string", SfRangeNavigator.GetFontOptions(chart.LabelStyle)).Height;
            CalculateDateTimeNiceInterval(chart.XAxisRenderer.VisibleRange.Start, chart.XAxisRenderer.VisibleRange.End, false);
            if (chart.EnableGrouping && chart.ValueType == RangeValueType.DateTime && (ActualIntervalType != IntervalType.Years || chart.Series.Count == 0))
            {
                chart.InitialClipRect.Height -= (chart.LabelPosition == AxisPosition.Outside || chart.Series.Count == 0) ? padding + labelHeight : (labelHeight + (2 * padding));
            }

            if (chart.Series.Count == 0)
            {
                chart.InitialClipRect.Y += chart.InitialClipRect.Height / 4;
                chart.InitialClipRect.Height = chart.InitialClipRect.Height / 2;
            }
        }

        private void RenderGridLine(ChartAxisRenderer axis, int index, string gridDirection, ChartBorderType gridModel, string gridId, int gridIndex, RenderTreeBuilder builder, string themeColor, string dashArray = null)
        {
            if (gridModel.Width > 0 && axis.Axis.Visible && !string.IsNullOrEmpty(gridDirection))
            {
                chart.SvgRenderer.RenderPath(builder, chart.Id + gridId + index + '_' + gridIndex, gridDirection, dashArray, gridModel.Width, gridModel.Color ?? themeColor);
            }
        }

        private static ChartFontOptions GetFontOptions(ChartAxisLabelStyle font)
        {
            return new ChartFontOptions { Color = font.Color, Size = font.Size };
        }

        private void FindSuitableFormat(ChartAxisRenderer axis, RangeIntervalType intervalType)
        {
            List<VisibleLabels> labels = axis.VisibleLabels;
            double labelLength = labels.Count, prevX = 0, currentX,
            interval = chart.ValueType == RangeValueType.DateTime ? labelLength > 1 ? (labels[1].Value - labels[0].Value) : axis.VisibleInterval : 0;
            Rect bounds = chart.InitialClipRect;
            for (int i = 0; i < labelLength; i++)
            {
                currentX = (ChartHelper.ValueToCoefficient(labels[i].Value + (interval / 2), axis) * bounds.Width) + bounds.X;
                labels[i].Size = ChartHelper.MeasureText(labels[i].Text, GetFontOptions(axis.Axis.LabelStyle));

                // Here we calculated for edgelabelPlacements
                if (i == 0 && currentX < bounds.X)
                {
                    currentX = bounds.X + (labels[i].Size.Width / 2);
                }

                if (intervalType == RangeIntervalType.Quarter && i != 0)
                {
                    if (labels[i].Text.Contains("Quarter", comparison) && IsIntersect(axis.Axis.IsInversed, currentX, labels[i].Size.Width, prevX, labels[i - 1].Size.Width))
                    {
                        labels.ForEach((label) =>
                        {
                            label.Text = label.Text.ToString(culture).Replace("Quarter", "QTR", comparison);
                        });
                        axis.VisibleLabels = labels;
                        FindSuitableFormat(axis, intervalType);
                    }
                    else if (IsIntersect(axis.Axis.IsInversed, currentX, labels[i].Size.Width, prevX, labels[i - 1].Size.Width))
                    {
                        labels.ForEach((label) =>
                        {
                            label.Text = label.Text.ToString(culture).Replace("QTR", "Q", comparison);
                        });
                        axis.VisibleLabels = labels;
                    }
                }
                else if (intervalType == RangeIntervalType.Weeks && i != 0 && labels[i].Text.Contains("Week", comparison) && IsIntersect(axis.Axis.IsInversed, currentX, labels[i].Size.Width, prevX, labels[i - 1].Size.Width))
                {
                    labels.ForEach((label) =>
                    {
                        label.Text = label.Text.ToString(culture).Replace("Week", "W", comparison);
                    });
                    axis.VisibleLabels = labels;
                }

                prevX = currentX;
            }
        }

        private void FindAxisLabels(ChartAxisRenderer axis, RangeIntervalType intervalType, string labelFormat)
        {
            axis.VisibleLabels = new List<VisibleLabels>();
            DateTime start = new DateTime(1970, 1, 1).AddMilliseconds(axis.VisibleRange.Start);
            double nextInterval, interval = double.IsNaN(chart.Interval) ? (intervalType == RangeIntervalType.Seconds || intervalType == RangeIntervalType.Hours || intervalType == RangeIntervalType.Minutes) ? axis.VisibleInterval : 1 : chart.Interval;
            switch (intervalType)
            {
                case RangeIntervalType.Years:
                    start = new DateTime(start.Year, 1, 1);
                    break;
                case RangeIntervalType.Quarter:
                    if (start.Month <= 2)
                    {
                        start = new DateTime(start.Year, 1, 1);
                    }
                    else if (start.Month <= 5)
                    {
                        start = new DateTime(start.Year, 4, 1);
                    }
                    else if (start.Month <= 8)
                    {
                        start = new DateTime(start.Year, 7, 1);
                    }
                    else
                    {
                        start = new DateTime(start.Year, 10, 1);
                    }

                    break;
                case RangeIntervalType.Months:
                    start = new DateTime(start.Year, start.Month, 1);
                    break;
                case RangeIntervalType.Weeks:
                    start = start.AddDays(-(int)start.DayOfWeek);
                    break;
                case RangeIntervalType.Days:
                    start = start.Date;
                    break;
                case RangeIntervalType.Hours:
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    break;
                case RangeIntervalType.Minutes:
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, 0);
                    break;
                case RangeIntervalType.Seconds:
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, start.Minute, start.Second);
                    break;
            }

            nextInterval = GetTime(start);
            while (nextInterval <= axis.VisibleRange.End)
            {
                string text = DateFormats(Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(nextInterval), FindCustomFormats(intervalType, labelFormat)), axis.VisibleLabels.Count, intervalType);
                axis.VisibleLabels.Add(new VisibleLabels(text, nextInterval, axis.Axis.LabelStyle, text));
                nextInterval = GetTime(IncreaseDateTimeInterval(nextInterval, interval, intervalType));
            }
        }

        private string DateFormats(string text, double index, RangeIntervalType intervalType)
        {
            bool isFirstLevel = chart.EnableGrouping && FirstLevelLabels.Count == 0;
            switch (intervalType)
            {
                case RangeIntervalType.Quarter:
                    if (text.Contains("Jan", comparison))
                    {
                        return !isFirstLevel ? text.Replace("January", "Quarter1", comparison) : "Quarter1";
                    }
                    else if (text.Contains("Apr", comparison))
                    {
                        return !isFirstLevel ? text.Replace("April", "Quarter2", comparison) : "Quarter2";
                    }
                    else if (text.Contains("Jul", comparison))
                    {
                        return !isFirstLevel ? text.Replace("July", "Quarter3", comparison) : "Quarter3";
                    }
                    else if (text.Contains("Oct", comparison))
                    {
                        return !isFirstLevel ? text.Replace("October", "Quarter4", comparison) : "Quarter4";
                    }

                    break;
                case RangeIntervalType.Weeks:
                    return "Week" + ++index;
                default:
                    return text;
            }

            return text;
        }

        private double FindAlignment(ChartAxisRenderer axis, int index)
        {
            VisibleLabels label = axis.VisibleLabels[index], nextLabel = axis.VisibleLabels[index + 1];
            Rect bounds = chart.InitialClipRect;
            return chart.SecondaryLabelAlignment == LabelAlignment.Near ? (ChartHelper.ValueToCoefficient(label.Value, axis) * bounds.Width) + bounds.X + (label.Size.Width / 2) :
                (ChartHelper.ValueToCoefficient(nextLabel != null ? nextLabel.Value : axis.VisibleRange.End, axis) * bounds.Width) + bounds.X - label.Size.Width;
        }

        private bool IsBorder(ChartAxisRenderer axis, int index, double xvalue)
        {
            Rect rect = chart.InitialClipRect;
            bool isHorizontal = axis.Orientation == Orientation.Horizontal;
            double start = isHorizontal ? rect.X : rect.Y, size = isHorizontal ? rect.Width : rect.Height, startIndex = isHorizontal ? 0 : axis.VisibleLabels.Count - 1,
            endIndex = isHorizontal ? axis.VisibleLabels.Count - 1 : 0;
            if (axis.Axis.PlotOffset > 0 || ((xvalue == start || xvalue == (start + size)) && (chart.NavigatorBorder.Width <= 0 || chart.NavigatorBorder.Color == Constants.TRANSPARENT)) || ((xvalue != start && index == startIndex) || (xvalue != (start + size) && index == endIndex)))
            {
                return true;
            }

            return false;
        }

        internal void Dispose()
        {
            chart = null;
            FirstLevelLabels = null;
            SecondLevelLabels = null;
            LowerValues = null;
        }
    }
}