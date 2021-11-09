using Syncfusion.Blazor.Charts.Chart.Internal;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.RangeNavigator.Internal
{
    internal class RangeNavigatorHelper
    {
        internal static bool WithInRange(DataPoint previousPoint, DataPoint currentPoint, DataPoint nextPoint, RangeNavigatorSeries series)
        {
            double x2 = LogWithIn(currentPoint.XValue, series.XAxisRenderer),
            x1 = previousPoint != null ? LogWithIn(previousPoint.XValue, series.XAxisRenderer) : x2,
            x3 = nextPoint != null ? LogWithIn(nextPoint.XValue, series.XAxisRenderer) : x2,
            start = Math.Floor(series.XAxisRenderer.VisibleRange.Start),
            end = Math.Ceiling(series.XAxisRenderer.VisibleRange.End);
            return (x1 >= start && x1 <= end) || (x2 >= start && x2 <= end) || (x3 >= start && x3 <= end) || (start >= x1 && start <= x3);
        }

        internal static double GetNearestValue(double[] lowerValues, double point)
        {
            return lowerValues.Aggregate((double prev, double curr) =>
            {
                return Math.Abs(curr - point) < Math.Abs(prev - point) ? curr : prev;
            });
        }

        internal static double RangeValueToCoefficient(double xvalue, DoubleRange range, bool inversed)
        {
            double result = (xvalue - range.Start) / range.Delta;
            return inversed ? (1 - result) : result;
        }

        internal static ChartInternalLocation GetPoint(double x, double y, ChartAxisRenderer XAxis, ChartAxisRenderer YAxis, bool isInverted = false)
        {
            x = (XAxis.Axis.ValueType == ValueType.Logarithmic) ? LogBase((x > 0) ? x : 1, XAxis.Axis.LogBase) : x;
            y = (YAxis.Axis.ValueType == ValueType.Logarithmic) ? LogBase((y > 0) ? y : 1, YAxis.Axis.LogBase) : y;
            x = ValueToCoefficient(x, XAxis);
            y = ValueToCoefficient(y, YAxis);
            double xLength = isInverted ? XAxis.Rect.Height : XAxis.Rect.Width,
            yLength = isInverted ? YAxis.Rect.Width : YAxis.Rect.Height;
            return new ChartInternalLocation(isInverted ? y * yLength : x * xLength, isInverted ? (1 - x) * xLength : (1 - y) * yLength);
        }

        internal static double ValueToCoefficient(object point, ChartAxisRenderer axis)
        {
            point = point.GetType() == typeof(System.DateTime) ? (long)((DateTime)point - new DateTime(1970, 1, 1)).TotalMilliseconds : Convert.ToDouble(point, null);
            double result = ((double)point - axis.VisibleRange.Start) / axis.VisibleRange.Delta;
            return axis.Axis.IsInversed ? (1 - result) : result;
        }

        private static double LogWithIn(double xvalue, ChartAxisRenderer axis)
        {
            if (axis.Axis.ValueType == ValueType.Logarithmic)
            {
                xvalue = LogBase(xvalue, axis.Axis.LogBase);
            }

            return xvalue;
        }

        internal static double LogBase(double xvalue, double baseValue)
        {
            return Math.Log(xvalue) / Math.Log(baseValue);
        }

        internal static List<DataPoint> GetVisiblePoints(RangeNavigatorSeries series)
        {
            List<DataPoint> tempPoints = new List<DataPoint>();
            int pointIndex = 0;
            for (int i = 0; i < series.Points.Count; i++)
            {
                DataPoint tempPoint = series.Points[i];
                if (tempPoint.X == null || string.IsNullOrEmpty(Convert.ToString(tempPoint.X, CultureInfo.InvariantCulture)))
                {
                    continue;
                }
                else
                {
                    tempPoint.Index = pointIndex++;
                    tempPoints.Add(tempPoint);
                }
            }

            return tempPoints;
        }

        internal static double GetXLocation(double x, DoubleRange range, double size, bool inversed)
        {
            x = RangeValueToCoefficient(x, range, inversed);
            return x * size;
        }

        internal static List<DataPoint> GetExactData(List<DataPoint> points, double start, double end)
        {
            List<DataPoint> selectedData = new List<DataPoint>();
            points.ForEach((DataPoint point) =>
            {
                if (point.XValue >= start && point.XValue <= end)
                {
                    selectedData.Add(new DataPoint { X = point.X, Y = point.Y });
                }
            });
            return selectedData;
        }

        internal static double GetRangeValueXByPoint(double xvalue, double size, DoubleRange range, bool inversed)
        {
            return ((!inversed ? xvalue / size : (1 - (xvalue / size))) * range.Delta) + range.Start;
        }

        internal static RangeThemeStyle GetChartThemeStyle(Theme theme, SfRangeNavigator chart)
        {
            RangeNavigatorThumbSettings thumbSize = chart.NavigatorStyleSettings.Thumb;
            double thumbWidth = double.IsNaN(thumbSize.Width) ? 20 : thumbSize.Width;
            double thumbHeight = double.IsNaN(thumbSize.Height) ? 20 : thumbSize.Height;
            string darkBackground = theme == Theme.MaterialDark ? "#303030" : (theme == Theme.FabricDark ? "#201F1F" : "#1A1A1A");
            RangeThemeStyle style = GetStyle("#E0E0E0", "#000000", "#686868", chart.Series.Count != 0 ? "rgba(255, 255, 255, 0.6)" : "#EEEEEE", "rgba(189, 189, 189, 1)", "rgba(250, 250, 250, 1)", "#757575", "#FFFFFF", "#EEEEEE", chart.Series.Count != 0 ? "transparent" : "#FF4081", "rgb(0, 8, 22)", "#dbdbdb", thumbWidth, thumbHeight);
            switch (theme)
            {
                case Theme.Fabric:
                    style.SelectedRegionColor = chart.Series.Count != 0 ? "transparent" : "#007897";
                    break;
                case Theme.Bootstrap:
                    style.SelectedRegionColor = chart.Series.Count != 0 ? "transparent" : "#428BCA";
                    break;
                case Theme.HighContrastLight:
                    style = GetStyle("#bdbdbd", "#969696", "#ffffff", chart.Series.Count != 0 ? "rgba(255, 255, 255, 0.3)" : "#EEEEEE", "#ffffff", "#262626", "#ffffff", darkBackground, "#BFBFBF", chart.Series.Count != 0 ? "transparent" : "#FFD939", "#ffffff", "#000000", thumbWidth, thumbHeight);
                    break;
                case Theme.HighContrast:
                    style = GetStyle("#4A4848", "#969696", "#DADADA", chart.Series.Count != 0 ? "rgba(43, 43, 43, 0.6)" : "#514F4F", "#969696", "#333232", "#DADADA", "#000000", "#BFBFBF", chart.Series.Count != 0 ? "rgba(22, 22, 22, 0.6)" : "#FFD939", "#F4F4F4", "#282727", thumbWidth, thumbHeight);
                    break;
                case Theme.MaterialDark:
                case Theme.FabricDark:
                case Theme.BootstrapDark:
                    style = GetStyle("#414040", "#6F6C6C", "#DADADA", chart.Series.Count != 0 ? "rgba(43, 43, 43, 0.6)" : "#514F4F", "#969696", "#333232", "#DADADA", darkBackground, "#BFBFBF", chart.Series.Count != 0 ? "rgba(22, 22, 22, 0.6)" : theme == Theme.FabricDark ? "#007897" : theme == Theme.BootstrapDark ? "#428BCA" : "#FF4081", "#F4F4F4", "#333232", thumbWidth, thumbHeight);
                    break;
                case Theme.Bootstrap4:
                    style = GetStyle("#E0E0E0", "#CED4DA", "#212529", chart.Series.Count != 0 ? "rgba(255, 255, 255, 0.6)" : "#514F4F", "rgba(189, 189, 189, 1)", "#FFFFFF", "#495057", "rgba(255, 255, 255, 0.6)", "#EEEEEE", chart.Series.Count != 0 ? "transparent" : "#FFD939", "rgba(0, 0, 0, 0.9)", "rgba(255, 255, 255)", thumbWidth, thumbHeight);
                    break;
                case Theme.Tailwind:
                    style = GetStyle("#E5E7EB", "#D1D5DB", "#6B7280", chart.Series.Count != 0 ? "transparent" : "#E5E7EB", "#9CA3AF", "#FFFFFF", "#6B7280", "rgba(255, 255, 255, 0.6)", "#374151", chart.Series.Count != 0 ? "rgba(79, 70, 229, 0.3)" : "#4F46E5", "#111827", "#F9FAFB", thumbWidth, thumbHeight);
                    break;
                case Theme.TailwindDark:
                    style = GetStyle("#374151", "#4B5563", "#9CA3AF", chart.Series.Count != 0 ? "transparent" : "#4B5563", "#6B7280", "#1F2937", "#D1D5DB", "#1F2937", "#E5E7EB", chart.Series.Count != 0 ? "rgba(22, 22, 22, 0.6)" : "#22D3EE", "#F9FAFB", "#1F2937", thumbWidth, thumbHeight);
                    break;
                case Theme.Bootstrap5:
                    style = GetStyle("#E5E7EB", "#E5E7EB", "#495057", chart.Series.Count != 0 ? "transparent" : "#E5E7EB", "#9CA3AF", "#FFFFFF", "#6C757D", "rgba(255, 255, 255)", "#374151", chart.Series.Count != 0 ? "rgba(38, 46, 11, 0.3)" : "#4F46E5", "#212529", "#F9FAFB", thumbWidth, thumbHeight);
                    break;
                case Theme.Bootstrap5Dark:
                    style = GetStyle("#343A40", "#343A40", "#CED4DA", chart.Series.Count != 0 ? "transparent" : "#E5E7EB", "#6C757D", "#333232", "#ADB5BD", "#212529", "#374151", chart.Series.Count != 0 ? "rgba(22, 22, 22, 0.6)" : "#ADB5BD", "#E9ECEF", "#212529", thumbWidth, thumbHeight);
                    break;
                default:
                    style.SelectedRegionColor = chart.Series.Count != 0 ? "transparent" : "#FF4081";
                    break;
            }

            return style;
        }

        private static RangeThemeStyle GetStyle(string gridLineColor, string axisLineColor, string labelFontColor, string unselectedRectColor, string thumpLineColor, string thumbBackground, string gripColor, string background, string thumbHoverColor, string selectedRegionColor, string tooltipBackground, string tooltipFontColor, double thumbWidth, double thumbHeight)
        {
            return new RangeThemeStyle
            {
                GridLineColor = gridLineColor,
                AxisLineColor = axisLineColor,
                LabelFontColor = labelFontColor,
                UnselectedRectColor = unselectedRectColor,
                ThumpLineColor = thumpLineColor,
                ThumbBackground = thumbBackground,
                GripColor = gripColor,
                Background = background,
                ThumbHoverColor = thumbHoverColor,
                SelectedRegionColor = selectedRegionColor,
                TooltipBackground = tooltipBackground,
                TooltipFontColor = tooltipFontColor,
                ThumbWidth = thumbWidth,
                ThumbHeight = thumbHeight
            };
        }
    }
}