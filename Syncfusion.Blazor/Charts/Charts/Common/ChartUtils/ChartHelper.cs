using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.InteropServices;
using Syncfusion.Blazor.DataVizCommon;
using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop.Infrastructure;
using Syncfusion.Blazor.Charts.Internal;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using System.Reflection;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    public class ChartHelper
    {
        internal static Dictionary<string, Size> sizePerCharacter { get; set; } = new Dictionary<string, Size>();

        internal static IJSRuntime JsRuntime { get; set; }

        internal static List<string> ChartFontKeys { get; set; } = new List<string>();

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        private const int rgbHexCode = 6;

        private const string DEFAULTCOLOR = "white";

        private const string SPACE = " ";

        internal static FontData Font = new FontData()
        {
            Average = 7.95,
            Chars = new Dictionary<char, double>()
            {
                ['0'] = 8.0,
                ['1'] = 8.0,
                ['2'] = 8.0,
                ['3'] = 8.0,
                ['4'] = 8.0,
                ['5'] = 8.0,
                ['6'] = 8.0,
                ['7'] = 8.0,
                ['8'] = 8.0,
                ['9'] = 8.0,
                ['!'] = 5.0,
                ['"'] = 7.0,
                ['#'] = 8.0,
                ['$'] = 8.0,
                ['%'] = 14.0,
                ['&'] = 10.0,
                ['\''] = 4.0,
                ['('] = 5.0,
                [')'] = 5.0,
                ['*'] = 7.0,
                ['+'] = 8.0,
                [','] = 4.0,
                ['-'] = 5.0,
                ['.'] = 4.0,
                ['/'] = 6.0,
                [':'] = 4.0,
                [';'] = 4.0,
                ['<'] = 8.0,
                ['='] = 8.0,
                ['>'] = 8.0,
                ['?'] = 7.0,
                ['@'] = 14.0,
                ['A'] = 9.0,
                ['B'] = 10.0,
                ['C'] = 10.0,
                ['D'] = 10.0,
                ['E'] = 9.0,
                ['F'] = 8.0,
                ['G'] = 10.0,
                ['H'] = 11.0,
                ['I'] = 5.0,
                ['J'] = 8.0,
                ['K'] = 10.0,
                ['L'] = 8.0,
                ['M'] = 12.0,
                ['N'] = 11.0,
                ['O'] = 11.0,
                ['P'] = 10.0,
                ['Q'] = 11.0,
                ['R'] = 10.0,
                ['S'] = 9.0,
                ['T'] = 9.0,
                ['U'] = 11.0,
                ['V'] = 9.0,
                ['W'] = 13.0,
                ['X'] = 9.0,
                ['Y'] = 8.0,
                ['Z'] = 9.0,
                ['['] = 5.0,
                ['\\'] = 6.0,
                [']'] = 5.0,
                ['^'] = 8.0,
                ['_'] = 8.0,
                ['`'] = 9.0,
                ['a'] = 9.0,
                ['b'] = 9.0,
                ['c'] = 8.0,
                ['d'] = 9.0,
                ['e'] = 8.0,
                ['f'] = 5.0,
                ['g'] = 9.0,
                ['h'] = 9.0,
                ['i'] = 4.0,
                ['j'] = 4.0,
                ['k'] = 8.0,
                ['l'] = 5.0,
                ['m'] = 14.0,
                ['n'] = 9.0,
                ['o'] = 9.0,
                ['p'] = 9.0,
                ['q'] = 9.0,
                ['r'] = 6.0,
                ['s'] = 7.0,
                ['t'] = 6.0,
                ['u'] = 9.0,
                ['v'] = 8.0,
                ['w'] = 12.0,
                ['x'] = 8.0,
                ['y'] = 8.0,
                ['z'] = 7.0,
                ['{'] = 5.0,
                ['|'] = 4.0,
                ['}'] = 5.0,
                ['~'] = 8.0,
                [' '] = 5.0
            }
        };

        internal static bool WithInRange(Point previousPoint, Point currentPoint, Point nextPoint, ChartAxisRenderer xAxis)
        {
            double mX2 = xAxis.GetPointValue(currentPoint.XValue),
            mX1 = previousPoint != null ? xAxis.GetPointValue(previousPoint.XValue) : mX2,
            mX3 = nextPoint != null ? xAxis.GetPointValue(nextPoint.XValue) : mX2,
            xStart = Math.Floor(xAxis.VisibleRange.Start),
            xEnd = Math.Ceiling(xAxis.VisibleRange.End);
            return (mX1 >= xStart && mX1 <= xEnd) || (mX2 >= xStart && mX2 <= xEnd) || (mX3 >= xStart && mX3 <= xEnd) || (xStart >= mX1 && xStart <= mX3);
        }

        internal static object GetDynamicMember(object data, string memberName)
        {
            /*
             * Not possible to get the dynamic object data member value using reflection, So, we use "CallSiteBinder" to get the value.
             */
            if (!string.IsNullOrEmpty(memberName))
            {
                CallSiteBinder binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, memberName, data.GetType(),
                new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });
                dynamic callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
                return callsite.Target(callsite, data);
            }

            return null;
        }

        internal static string AppendPathElements(SfChart chart, string direction, string id, string parentId = "")
        {
            bool redraw = chart.Redraw;
            DynamicPathAnimationOptions existElement = redraw ? chart.PathAnimationElements.Find(item => item.Id == id) : null;
            string previous = string.Empty;
            if (existElement != null)
            {
                previous = existElement.CurrentDir != direction ? existElement.CurrentDir : string.Empty;
                existElement.PreviousDir = previous;
                existElement.CurrentDir = direction;
                direction = !string.IsNullOrEmpty(existElement.PreviousDir) && redraw ? existElement.PreviousDir : direction;
            }
            else
            {
                chart.PathAnimationElements.Add(new DynamicPathAnimationOptions { ParentId = parentId, Id = id, CurrentDir = direction, PreviousDir = previous });
            }

            return direction;
        }

        internal static string[] AppendTextElements(SfChart chart, string id, double locationX, double locationY, string x = "x", string y = "y")
        {
            bool redraw = chart.Redraw;
            DynamicTextAnimationOptions existElement = redraw ? chart.TextAnimationElements.Find(item => item.Id == id) : null;
            if (existElement != null)
            {
                double preLocationX = existElement.CurLocationX != locationX ? existElement.CurLocationX : locationX,
                preLocationY = existElement.CurLocationY != locationY ? existElement.CurLocationY : locationY;
                existElement.PreLocationX = preLocationX;
                existElement.PreLocationY = preLocationY;
                existElement.CurLocationX = locationX;
                existElement.CurLocationY = locationY;
                locationX = redraw ? existElement.PreLocationX : locationX;
                locationY = redraw ? existElement.PreLocationY : locationY;
            }
            else if (!chart.TextAnimationElements.Exists(item => item.Id == id))
            {
                chart.TextAnimationElements.Add(new DynamicTextAnimationOptions { CurLocationX = locationX, CurLocationY = locationY, Id = id, X = x, Y = y });
            }
#pragma warning disable CA1305
            return new string[] { locationX.ToString(CultureInfo.InvariantCulture), locationY.ToString(CultureInfo.InvariantCulture) };
        }

        internal static double GetValueByPoint(double pointValue, double size, Orientation orientation, DoubleRange visibleRange, bool isInversed)
        {
            return ((((orientation == Orientation.Horizontal && !isInversed) || (orientation != Orientation.Horizontal && isInversed)) ? pointValue / size : (1 - (pointValue / size))) * visibleRange.Delta) + visibleRange.Start;
        }

        // internal static string[] AppendPieTextElements(SfAccumulationChart chart, string id, double locationX, double locationY, string x = "x", string y = "y")
        //        {
        //            DynamicAccTextAnimationOptions existElement = chart.AnimateTextElements.Find(item => item.Id == id);
        //            if (existElement != null)
        //            {
        //                double preLocationX = existElement.CurLocationX != locationX ? existElement.CurLocationX : locationX,
        //                preLocationY = existElement.CurLocationY != locationY ? existElement.CurLocationY : locationY;
        //                existElement.PreLocationX = preLocationX;
        //                existElement.PreLocationY = preLocationY;
        //                existElement.CurLocationX = locationX;
        //                existElement.CurLocationY = locationY;
        //                locationX = chart.LegendClickRedraw ? existElement.PreLocationX : locationX;
        //                locationY = chart.LegendClickRedraw ? existElement.PreLocationY : locationY;
        //            }
        //            else
        //            {
        //                chart.AnimateTextElements.Add(new DynamicAccTextAnimationOptions { CurLocationX = locationX, CurLocationY = locationY, Id = id, X = x, Y = y });
        //            }
        //            return new string[] { locationX.ToString(CultureInfo.InvariantCulture), locationY.ToString(CultureInfo.InvariantCulture) };
        // #pragma warning restore CA1305 
        // }

        internal static Rect AppendRectElements(SfChart chart, string id, Rect rect)
        {
            Rect previousRect = null;
            DynamicRectAnimationOptions existElement = chart.RectAnimationElements.Find(item => item.Id == id);
            if (existElement != null)
            {
                previousRect = !existElement.CurrentRect.Equals(rect) ? existElement.CurrentRect : rect;
                existElement.PreviousRect = previousRect;
                existElement.CurrentRect = rect;
            }
            else
            {
                chart.RectAnimationElements.Add(new DynamicRectAnimationOptions { Id = id, CurrentRect = rect });
            }

            return (chart.Redraw && previousRect != null) ? previousRect : rect;
        }

        internal static double LogWithIn(double point, ChartAxis axis)
        {
            if (axis.ValueType == ValueType.Logarithmic)
            {
                point = LogBase(point, axis.LogBase);
            }
            return point;
        }

        private static Size MeasureBreakText(string originalText, ChartFontOptions font)
        {
            originalText = originalText.Replace("<br/>", "<br>", StringComparison.InvariantCulture);
            List<string> textCollection = originalText.Split("<br>").ToList();
            double width = 0, height = 0;
            Size size = new Size(width, height);
            foreach (string text in textCollection)
            {
                size = MeasureText(text, font);
                width = Math.Max(width, size.Width);
                height += size.Height;
            }

            return new Size(width, height);
        }

        internal static Size MeasureText(string text, ChartFontOptions font)
        {
            if (text.Contains("<br>", StringComparison.InvariantCulture) || text.Contains("<br/>", StringComparison.InvariantCulture))
            {
                return MeasureBreakText(text, font);
            }
            else
            {
                double width = 0, height = 0, fontSize = PixelToNumber(font.Size);
                Size charSize;
                if (IsRTLText(text))
                {
                    string key = text + Constants.UNDERSCORE + font.FontWeight + Constants.UNDERSCORE + font.FontStyle + Constants.UNDERSCORE + font.FontFamily;
                    if (sizePerCharacter.ContainsKey(key))
                    {
                        charSize = sizePerCharacter[key];
                        return new Size(charSize.Width * (fontSize / 100), charSize.Height * (fontSize / 100));
                    }
                }

                for (int i = 0; i < text.Length; i++)
                {
                    charSize = GetCharSize(text[i], font);
                    width += charSize.Width;
                    height = Math.Max(charSize.Height, height);
                }

                return new Size((width * fontSize) / 100, (height * fontSize) / 100);
            }
        }

        /// <summary>
        /// The method is used to find whether given text is RTL or not.
        /// </summary>
        internal static bool IsRTLText(string text)
        {
            return text.Any(c => c >= 0x600 && c <= 0x6ff);
        }

        private static Size GetCharSize(char character, ChartFontOptions font)
        {
            Size charSize = new Size();
            string key = character + Constants.UNDERSCORE + font.FontWeight + Constants.UNDERSCORE + font.FontStyle + Constants.UNDERSCORE + font.FontFamily;
            if (!sizePerCharacter.TryGetValue(key, out charSize))
            {
                double charWidth;
                if (!Font.Chars.TryGetValue(character, out charWidth))
                {
                    return new Size { Width = charWidth * 6.25, Height = 130 };
                }

                sizePerCharacter[key] = new Size { Width = charWidth * 6.25, Height = 130 };
                sizePerCharacter.TryGetValue(key, out charSize);
            }

            return charSize;
        }

        private static double PixelToNumber(string size)
        {
            if (!string.IsNullOrEmpty(size))
            {
#pragma warning disable CA1304
                switch (Constants.REGNUM.Match(size.ToLower()).ToString())
                {
                    case "px":
                        return Convert.ToDouble(Constants.REGNUM.Replace(size.ToLower(), string.Empty), null);
                    case "rem":
                    case "em":
                        return Convert.ToDouble(Constants.REGNUM.Replace(size.ToLower(), string.Empty), null) * 16;
                    case "pt":
                        return Convert.ToDouble(Constants.REGNUM.Replace(size.ToLower(), string.Empty), null) * 1.3333333333333333;
                    case "%":
                        return Convert.ToDouble(Constants.REGNUM.Replace(size.ToLower(), string.Empty), null) * 0.13;
                    default:
                        return 0;
                }
#pragma warning restore CA1304
            }

            return 0;
        }

        internal static bool IsNaNOrZero(double point)
        {
            return double.IsNaN(point) || point == 0;
        }

        internal static Rect SubtractThickness(Rect rect, Thickness thickness)
        {
            rect.X += thickness.Left;
            rect.Y += thickness.Top;
            rect.Width -= thickness.Left + thickness.Right;
            rect.Height -= thickness.Top + thickness.Bottom;
            return rect;
        }

        internal static string FindThemeColor(string actualColor, string themeColor)
        {
            return string.IsNullOrEmpty(actualColor) ? themeColor : actualColor;
        }

        internal static ChartThemeStyle GetChartThemeStyle(string theme)
        {
            string darkBackground = (theme == "MaterialDark") ? "#303030" : ((theme == "FabricDark") ? "#201F1F" : "#1A1A1A");
            switch (theme)
            {
                case "HighContrastLight":
                case "Highcontrast":
                case "HighContrast":
                    return GetThemeStyle("#ffffff", "#ffffff", "#ffffff", "#BFBFBF", "#969696", "#BFBFBF", "#969696", "#ffffff", "#ffffff", "#000000", "#ffffff", "#ffffff", "#ffffff", "#ffffff", "#000000", "#ffffff", "#000000", "#000000", "#969696", "#BFBFBF", "rgba(255, 217, 57, 0.3)", "#ffffff", "#FFD939");
                case "MaterialDark":
                case "FabricDark":
                case "BootstrapDark":
                    return GetThemeStyle("#DADADA", "#ffffff", "#6F6C6C", "#414040", "#514F4F", "#414040", "#4A4848", "#ffffff", "#DADADA", darkBackground, "#9A9A9A", "#ffffff", "#F4F4F4", "#F4F4F4", "#282727", "#F4F4F4", "#282727", "#333232", "#9A9A9A", null, "rgba(56,169,255, 0.1)", "#38A9FF", "#38A9FF");
                case "Bootstrap4":
                    return GetThemeStyle("#212529", "#ffffff", "#CED4DA", "#CED4DA", "#DEE2E6", "#ADB5BD", "#CED4DA", "#212529", "#212529", "#FFFFFF", "#DEE2E6", "#000000", "#6C757D", "#495057", "#FFFFFF", "rgba(0, 0, 0, 0.9)", "rgba(255,255,255)", "rgba(255,255,255, 0.9)", "rgba(255,255,255, 0.2)", null, "rgba(41, 171, 226, 0.1)", "#29abe2", "#29abe2");
                case "Tailwind":
                    return GetThemeStyle("#6B728", "#374151", "#D1D5DB", "#E5E7EB", "#E5E7EB", "#D1D5DB", "#D1D5DB", "#374151", "#374151", "rgba(255,255,255)", "#E5E7EB", "#374151", "#1F2937", "#111827", "#F9FAFB", "#111827", "#D1D5DB", "#F9FAFB", "#6B7280", null, "rgba(79,70,229, 0.1)", "#4F46E5", "#4F46E5");
                case "TailwindDark":
                    return GetThemeStyle("#9CA3AF", "#9CA3AF", "#4B5563", "#374151", "#374151", "#4B5563", "#4B5563", "#D1D5DB", "#D1D5DB", "#1F2937", "#374151", "#ffffff", "#9CA3AF", "#F9FAFB", "#1F2937", "#F9FAFB", "#6B7280", "#1F2937", "#9CA3AF", null, "rgba(34,211,238, 0.1)", "#22D3EE", "#22D3EE");
                case "Bootstrap5":
                    return GetThemeStyle("#495057", "#343A40", "#D1D5DB", "#E5E7EB", "#E5E7EB", "#D1D5DB", "#D1D5DB", "#343A40", "#343A40", "rgba(255, 255, 255)", "#DEE2E6", "#1F2937", "#1F2937", "#212529", "#F9FAFB", "#212529", "#D1D5DB", "#F9FAFB", "#6B7280", null, "rgba(79,70,229, 0.1)", "#4F46E5", "#4F46E5");
                case "Bootstrap5Dark":
                    return GetThemeStyle("#CED4DA", "#E9ECEF", "#495057", "#343A40", "#343A40", "#495057", "#495057", "#E9ECEF", "#E9ECEF", "#212529", "#444C54", "#ADB5BD", "#ADB5BD", "#E9ECEF", "#212529", "#E9ECEF", "#D1D5DB", "#F9FAFB", "#6B7280", null, "rgba(79,70,229, 0.1)", "#4F46E5", "#4F46E5");
                default:
                    return GetThemeStyle("#686868", "#424242", "#b5b5b5", "#dbdbdb", "#eaeaea", "#b5b5b5", "#d6d6d6", "#424242", "#353535", "#FFFFFF", "Gray", "#000000", "#4f4f4f", "#4f4f4f", "#e5e5e5", "rgba(0, 8, 22, 0.75)", "#ffffff", "#dbdbdb", "#ffffff", null, "rgba(41, 171, 226, 0.1)", "#29abe2", "#29abe2");
            }
        }

        private static ChartThemeStyle GetThemeStyle(string axisLabel, string axisTitle, string axisLine, string majorGridLine, string minorGridLine, string majorTickLine, string minorTickLine, string chartTitle, string legendLabel, string background, string areaBorder, string errorBar, string crosshairLine, string crosshairFill, string crosshairLabel, string tooltipFill, string tooltipBoldLabel, string tooltipLightLabel, string tooltipHeaderLine, string markerShadow, string selectionRectFill, string selectionRectStroke, string selectionCircleStroke)
        {
            return new ChartThemeStyle()
            {
                AxisLabel = axisLabel,
                AxisTitle = axisTitle,
                AxisLine = axisLine,
                MajorGridLine = majorGridLine,
                MinorGridLine = minorGridLine,
                MajorTickLine = majorTickLine,
                MinorTickLine = minorTickLine,
                ChartTitle = chartTitle,
                LegendLabel = legendLabel,
                Background = background,
                AreaBorder = areaBorder,
                ErrorBar = errorBar,
                CrosshairLine = crosshairLine,
                CrosshairFill = crosshairFill,
                CrosshairLabel = crosshairLabel,
                TooltipFill = tooltipFill,
                TooltipBoldLabel = tooltipBoldLabel,
                TooltipLightLabel = tooltipLightLabel,
                TooltipHeaderLine = tooltipHeaderLine,
                MarkerShadow = markerShadow,
                SelectionRectFill = selectionRectFill,
                SelectionRectStroke = selectionRectStroke,
                SelectionCircleStroke = selectionCircleStroke
            };
        }

        internal static string[] GetSeriesColor(string theme)
        {
            switch (theme)
            {
                case "Fabric":
                    return new string[] { "#4472c4", "#ed7d31", "#ffc000", "#70ad47", "#5b9bd5", "#c1c1c1", "#6f6fe2", "#e269ae", "#9e480e", "#997300" };
                case "Bootstrap4":
                    return new string[] { "#a16ee5", "#f7ce69", "#55a5c2", "#7ddf1e", "#ff6ea6", "#7953ac", "#b99b4f", "#407c92", "#5ea716", "#b91c52" };
                case "Bootstrap":
                    return new string[] { "#a16ee5", "#f7ce69", "#55a5c2", "#7ddf1e", "#ff6ea6", "#7953ac", "#b99b4f", "#407c92", "#5ea716", "#b91c52" };
                case "HighContrastLight":
                case "HighContrast":
                    return new string[] { "#79ECE4", "#E98272", "#DFE6B6", "#C6E773", "#BA98FF", "#FA83C3", "#00C27A", "#43ACEF", "#D681EF", "#D8BC6E" };
                case "MaterialDark":
                    return new string[] { "#9ECB08", "#56AEFF", "#C57AFF", "#61EAA9", "#EBBB3E", "#F45C5C", "#8A77FF", "#63C7FF", "#FF84B0", "#F7C928" };
                case "FabricDark":
                    return new string[] { "#4472c4", "#ed7d31", "#ffc000", "#70ad47", "#5b9bd5", "#c1c1c1", "#6f6fe2", "#e269ae", "#9e480e", "#997300" };
                case "BootstrapDark":
                    return new string[] { "#a16ee5", "#f7ce69", "#55a5c2", "#7ddf1e", "#ff6ea6", "#7953ac", "#b99b4f", "#407c92", "#5ea716", "#b91c52" };
                case "Tailwind":
                    return new string[] { "#5A61F6", "#65A30D", "#334155", "#14B8A6", "#8B5CF6", "#0369A1", "#F97316", "#9333EA", "#F59E0B", "#15803D" };
                case "TailwindDark":
                    return new string[] { "#8B5CF6", "#22D3EE", "#F87171", "#4ADE80", "#E879F9", "#FCD34D", "#F97316", "#2DD4BF", "#F472B6", "#10B981" };
                case "Bootstrap5":
                    return new string[] { "#262E0B", "#668E1F", "#AF6E10", "#862C0B", "#1F2D50", "#64680B", "#311508", "#4C4C81", "#0C7DA0", "#862C0B" };
                case "Bootstrap5Dark":
                    return new string[] { "#5ECB9B", "#A860F1", "#EBA844", "#557EF7", "#E9599B", "#BFC529", "#3BC6CF", "#7A68EC", "#74B706", "#EA6266" };
                default:
                    return new string[] { "#00bdae", "#404041", "#357cd2", "#e56590", "#f8b883", "#70ad47", "#dd8abd", "#7f84e8", "#7bb4eb", "#ea7a57" };
            }
        }

        internal static bool SetRange(ChartAxis axis)
        {
            return axis.Minimum != null && axis.Maximum != null;
        }

        internal static double LogBase(double point, double baseValue)
        {
            return Math.Log(point) / Math.Log(baseValue);
        }

        internal static bool WithIn(double interval, DoubleRange range)
        {
            return (interval <= range.End) && (interval >= range.Start);
        }

        internal static bool Inside(double interval, DoubleRange range)
        {
            return (interval < range.End) && (interval > range.Start);
        }

        internal static double ValueToCoefficient(double point, ChartAxisRenderer axisRenderer)
        {
            double result = (point - axisRenderer.VisibleRange.Start) / axisRenderer.VisibleRange.Delta;
            return axisRenderer.Axis.IsInversed ? (1 - result) : result;
        }

        // TODO: Temp method need to remove
        internal static double ValueToCoefficient(double point, double min, double delta, bool isInversed)
        {
            double result = (point - min) / delta;
            return isInversed ? (1 - result) : result;
        }

        // internal static double ValueToCoefficient(object point, ChartAxis axis)
        // {
        //     point = point.GetType() == typeof(System.DateTime) ? (long)((DateTime)point - new DateTime(1970, 1, 1)).TotalMilliseconds : Convert.ToDouble(point, null);
        //     double result = ((double)point - axis.VisibleRange.Min) / axis.VisibleRange.Delta;
        //     return axis.IsInversed ? (1 - result) : result;
        // }

        internal static bool IsBreakLabel(string label)
        {
            return label.Contains("<br>", StringComparison.InvariantCulture);
        }

        internal static ChartInternalLocation GetPoint(double x, double y, ChartAxisRenderer XAxisRenderer, ChartAxisRenderer YAxisRenderer, bool isInverted = false)
        {
            x = ValueToCoefficient(x, XAxisRenderer.VisibleRange.Start, XAxisRenderer.VisibleRange.Delta, XAxisRenderer.Axis.IsInversed);
            y = ValueToCoefficient(y, YAxisRenderer.VisibleRange.Start, YAxisRenderer.VisibleRange.Delta, YAxisRenderer.Axis.IsInversed);
            double xLength = isInverted ? XAxisRenderer.Rect.Height : XAxisRenderer.Rect.Width,
            yLength = isInverted ? YAxisRenderer.Rect.Width : YAxisRenderer.Rect.Height;
            return new ChartInternalLocation(isInverted ? y * yLength : x * xLength, isInverted ? (1 - x) * xLength : (1 - y) * yLength);
        }

        internal static ChartInternalLocation GetPoint(double x, double y, ChartAxisRenderer XAxisRenderer, ChartAxisRenderer YAxisRenderer, double xLength, double yLength, bool isInverted = false)
        {
            x = ValueToCoefficient(x, XAxisRenderer.VisibleRange.Start, XAxisRenderer.VisibleRange.Delta, XAxisRenderer.Axis.IsInversed);
            y = ValueToCoefficient(y, YAxisRenderer.VisibleRange.Start, YAxisRenderer.VisibleRange.Delta, YAxisRenderer.Axis.IsInversed);
            return new ChartInternalLocation(isInverted ? y * yLength : x * xLength, isInverted ? (1 - x) * xLength : (1 - y) * yLength);
        }

        internal static ChartInternalLocation TransformToVisible(double x, double y, ChartAxis xAxis, ChartAxis yAxis, ChartSeries series, double radius = 0)
        {
            x = xAxis.ValueType == ValueType.Logarithmic ? LogBase(x > 1 ? x : 1, xAxis.LogBase) : x;
            y = yAxis.ValueType == ValueType.Logarithmic ? LogBase(y > 1 ? y : 1, yAxis.LogBase) : y;
            x += xAxis.ValueType == ValueType.Category && xAxis.LabelPlacement == LabelPlacement.BetweenTicks && series.Type != ChartSeriesType.Radar ? 0.5 : 0;
            radius = series.Renderer.Owner.AxisContainer.AxisLayout.Radius * ValueToCoefficient(y, yAxis.Renderer);
            ChartInternalLocation point = CoefficientToVector(ValueToPolarCoefficient(x, xAxis.Renderer), xAxis.StartAngle);
            return new ChartInternalLocation(((series.Renderer.ClipRect.Width / 2) + series.Renderer.ClipRect.X) + (radius * point.X), ((series.Renderer.ClipRect.Height / 2) + series.Renderer.ClipRect.Y) + (radius * point.Y));
        }

        internal static ChartInternalLocation CoefficientToVector(double coefficient, double startAngle)
        {
            startAngle = startAngle < 0 ? startAngle + 360 : startAngle;
            double angle = (Math.PI * (1.5 - (2 * coefficient))) + (startAngle * Math.PI) / 180;
            return new ChartInternalLocation(Math.Cos(angle), Math.Sin(angle));
        }

        internal static List<string> GetTitle(string title, ChartFontOptions style, double width)
        {
            List<string> titleCollection = new List<string>();
            switch (style.TextOverflow)
            {
                case TextOverflow.Wrap:
                    titleCollection = TextWrap(title, width, style);
                    break;
                case TextOverflow.Trim:
                    titleCollection.Add(TextTrim(width, title, style));
                    break;
                default:
                    titleCollection.Add(title);
                    break;
            }

            return titleCollection;
        }

        internal static List<string> TextWrap(string currentLabel, double maximumWidth, ChartFontOptions font)
        {
            string[] textCollection = currentLabel.Split(SPACE);
            string label = string.Empty, text;
            List<string> labelCollection = new List<string>();
            for (int i = 0, len = textCollection.Length; i < len; i++)
            {
                text = textCollection[i];
                if (MeasureText(label + text, font).Width < maximumWidth)
                {
                    label = label + ((string.IsNullOrEmpty(label) ? string.Empty : SPACE) as string) + text;
                }
                else
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        labelCollection.Add(TextTrim(maximumWidth, label, font));
                        label = text;
                    }
                    else
                    {
                        labelCollection.Add(TextTrim(maximumWidth, text, font));
                        text = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(label) && i == len - 1)
                {
                    labelCollection.Add(TextTrim(maximumWidth, label, font));
                }
            }

            return labelCollection;
        }

        internal static double MinMax(double factor, double min, double max)
        {
            return factor > max ? max : (factor < min ? min : factor);
        }

        internal void DrawSymbol(RenderTreeBuilder builder, SvgRendering svgRenderer, ChartInternalLocation location, string shape, Size size, string url, PathOptions option, [Optional] bool isbulletChart, [Optional] SfChart chart)
        {
            ChartInternalLocation currentLocation = null;
            if (chart != null && shape == "Circle")
            {
                string[] locations = ChartHelper.AppendTextElements(chart, option.Id, location.X, location.Y, "cx", "cy");
                currentLocation = new ChartInternalLocation(Convert.ToDouble(locations[0], culture), Convert.ToDouble(locations[1], culture));
            }

            SymbolOptions shapeoption = CalculateShapes(currentLocation != null ? currentLocation : location, size, shape, url, option, isbulletChart);
            if (shapeoption.ShapeName == ShapeName.path)
            {
                if (chart != null)
                {
                    shapeoption.PathOption.Direction = ChartHelper.AppendPathElements(chart, shapeoption.PathOption.Direction, shapeoption.PathOption.Id);
                }

                shapeoption.PathOption.Visibility = option.Visibility;
                svgRenderer.RenderPath(builder, shapeoption.PathOption);
            }

            if (shapeoption.ShapeName == ShapeName.ellipse)
            {
                shapeoption.EllipseOption.Visibility = option.Visibility;
                svgRenderer.RenderEllipse(builder, shapeoption.EllipseOption);
            }

            if (shapeoption.ShapeName == ShapeName.image)
            {
                shapeoption.ImageOption.Visibility = option.Visibility;
                svgRenderer.RenderImage(builder, shapeoption.ImageOption);
            }
        }

        internal static SymbolOptions CalculateShapes(ChartInternalLocation location, Size size, string shape, string url, PathOptions option, bool isBulletChart)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            double width = isBulletChart && shape == "Circle" ? (size.Width - 2) : size.Width,
            height = isBulletChart && shape == "Circle" ? (size.Height - 2) : size.Height, lx = location.X, ly = location.Y, y = location.Y + (-height / 2), x = location.X + (-width / 2);
            SymbolOptions symbolOption = new SymbolOptions();
            symbolOption.ShapeName = ShapeName.path;
            switch (shape)
            {
                case "Bubble":
                case "Circle":
                    symbolOption.ShapeName = ShapeName.ellipse;
                    symbolOption.EllipseOption = new EllipseOptions(option.Id, Convert.ToString(width / 2, culture), Convert.ToString(height / 2, culture), Convert.ToString(lx, culture), Convert.ToString(ly, culture), option.StrokeDashArray, option.StrokeWidth, option.Stroke, option.Opacity, option.Fill);
                    break;
                case "Cross":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + "L" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'M' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture);
                    option.Stroke = option.Fill;
                    break;
                case "Multiply":
                    option.Direction = "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + (y + height).ToString(culture) + " M " + (x + width).ToString(culture) + SPACE + y.ToString(culture) + " L " + x.ToString(culture) + SPACE + (y + height).ToString(culture);
                    option.Stroke = option.Fill;
                    break;
                case "HorizontalLine":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture);
                    break;
                case "VerticalLine":
                    option.Direction = "M" + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture);
                    break;
                case "Diamond":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + " z";
                    break;
                case "ActualRect":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 8)).ToString(culture) + SPACE + 'L' + SPACE + (lx).ToString(culture) + SPACE + (ly + (-height / 8)).ToString(culture) + SPACE + 'L' + SPACE + (lx).ToString(culture) + SPACE + (ly + (height / 8)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (height / 8)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 8)).ToString(culture) + " z";
                    break;
                case "TargetRect":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + " z";
                    break;
                case "Rectangle":
                case "Hilo":
                case "HiloOpenClose":
                case "Candle":
                case "Waterfall":
                case "BoxAndWhisker":
                case "StepArea":
                case "StackingStepArea":
                case "Square":
                case "Flag":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + " z";
                    break;
                case "Pyramid":
                case "Triangle":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + " z";
                    break;
                case "Funnel":
                case "InvertedTriangle":
                    option.Direction = "M" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx - (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + " z";
                    break;
                case "Pentagon":
                    string dir = string.Empty;
                    for (int i = 0; i <= 5; i++)
                    {
                        double xVal = (width / 2) * Math.Cos((Math.PI / 180) * (i * 72)),
                        yVal = (height / 2) * Math.Sin((Math.PI / 180) * (i * 72));
                        if (i == 0)
                        {
                            dir = "M" + SPACE + (lx + xVal).ToString(culture) + SPACE + (ly + yVal).ToString(culture) + SPACE;
                        }
                        else
                        {
                            dir = dir + 'L' + SPACE + (lx + xVal).ToString(culture) + SPACE + (ly + yVal).ToString(culture) + SPACE;
                        }
                    }

                    dir = dir + 'Z';
                    option.Direction = dir;
                    break;
                case "Image":
                    symbolOption.ShapeName = ShapeName.image;
                    symbolOption.ImageOption = new ImageOptions(option.Id, x, y, width, height, url);
                    break;
            }

            option = CalculateLegendShapes(location, size, shape, option);
            symbolOption.PathOption = option;
            return symbolOption;
        }

        internal static double ValueToPolarCoefficient(double point, ChartAxisRenderer axisRenderer)
        {
            DoubleRange range = axisRenderer.VisibleRange;
            List<VisibleLabels> visibleLables = axisRenderer.VisibleLabels;
            double delta, length;
            if (visibleLables.Count == 0)
            {
                delta = 1;
                length = 1;
            }
            else if (axisRenderer.Axis.ValueType != ValueType.Category)
            {
                delta = range.End - ((axisRenderer.Axis.ValueType == ValueType.DateTime) ? axisRenderer.DateTimeInterval : axisRenderer.VisibleInterval) - range.Start;
                length = visibleLables.Count - 1;
                delta = delta == 0 ? 1 : delta;
            }
            else
            {
                delta = visibleLables.Count == 1 ? 1 : (visibleLables[visibleLables.Count - 1].Value - visibleLables[0].Value);
                length = visibleLables.Count;
            }

            return axisRenderer.Axis.IsInversed ? (point - range.Start) / delta * (1 - (1 / length)) : 1 - ((point - range.Start) / delta * (1 - (1 / length)));
        }

        internal static double GetMinPointsDelta(ChartAxis axis, List<ChartSeriesRenderer> seriesCollection)
        {
            double minDelta = double.MaxValue, minVal;
            string axisName = axis.GetName();
            for (int index = 0; index < seriesCollection?.Count; index++)
            {
                ChartSeriesRenderer seriesRenderer = seriesCollection[index];
                ChartSeries series = seriesRenderer.Series;
                List<double> xValues = new List<double>();
                if (series.Visible && (axisName == series.XAxisName || (axisName == Constants.PRIMARYXAXIS && series.XAxisName == null)))
                {
                    seriesRenderer.Points.ForEach(x => xValues.Add(x.XValue));
                    xValues.Sort();
                    if (xValues.Count == 1)
                    {
                        double seriesMin = (axis.ValueType == ValueType.DateTime && seriesRenderer.XMin == seriesRenderer.XMax) ? (seriesRenderer.XMin - 2592000000) : seriesRenderer.XMin;
                        minVal = xValues[0] - (!double.IsNaN(seriesMin) ? seriesMin : axis.Renderer.VisibleRange.Start);
                        if (minVal != 0)
                        {
                            minDelta = Math.Min(minDelta, minVal);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < xValues.Count; i++)
                        {
                            if (i > 0 & !double.IsNaN(xValues[i]))
                            {
                                minVal = xValues[i] - xValues[i - 1];
                                if (minVal != 0)
                                {
                                    minDelta = Math.Min(minDelta, minVal);
                                }
                            }
                        }
                    }
                }
            }

            if (minDelta == double.MaxValue)
            {
                minDelta = 1;
            }

            return minDelta;
        }

        private static PathOptions CalculateLegendShapes(ChartInternalLocation location, Size size, string shape, PathOptions options)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            double height = size.Height, width = size.Width, lx = location.X, ly = location.Y;
            switch (shape)
            {
                case "MultiColoredLine":
                case "Line":
                case "StackingLine":
                case "StackingLine100":
                    options.Direction = "M" + SPACE + (lx + (-width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture);
                    break;
#pragma warning restore CA1062 
                case "StepLine":
                    options.Fill = "transparent";
                    options.Direction = "M" + SPACE + (lx + (-width / 2) - 2.5).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 2) + (width / 10)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 2) + (width / 10)).ToString(culture)
                        + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 10)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 10)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 5)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' +
                        SPACE + (lx + (width / 5)).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + string.Empty + (lx + (width / 2) + 2.5).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture);
                    break;
                case "RightArrow":
                    options.Direction = "M" + SPACE + (lx + (-width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + " L" + SPACE + (lx + (-width / 2)).ToString(culture) + SPACE +
                        (ly + (height / 2) - 2).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2) - 4).ToString(culture) + SPACE + ly.ToString(culture) + " L" + (lx + (-width / 2)).ToString(culture) + SPACE + (ly - (height / 2) + 2).ToString(culture) + " Z";
                    break;
                case "LeftArrow":
                    options.Fill = options.Stroke;
                    options.Stroke = "transparent";
                    options.Direction = "M" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE +
                        (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2) - 2).ToString(culture) + " L" + SPACE + (lx + (-width / 2) + 4).ToString(culture) + SPACE + ly.ToString(culture) + " L" + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2) + 2).ToString(culture) + " Z";
                    break;
                case "Column":
                case "Pareto":
                case "StackingColumn":
                case "StackingColumn100":
                case "RangeColumn":
                case "Histogram":
                    options.Direction = "M" + SPACE + (lx - 3 * (width / 5)).ToString(culture) + SPACE + (ly - (height / 5)).ToString(culture) + SPACE + 'L' + SPACE + (lx + 3 * (-width / 10)).ToString(culture) + SPACE + (ly - (height / 5)).ToString(culture) + SPACE + 'L' + SPACE +
                        (lx + 3 * (-width / 10)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx - 3 * (width / 5)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'Z' + SPACE + 'M' + SPACE +
                        (lx + (-width / 10) - (width / 20)).ToString(culture) + SPACE + (ly - (height / 4) - 5).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 10) + (width / 20)).ToString(culture) + SPACE + (ly - (height / 4) -
                        5).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 10) + (width / 20)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 10) - (width / 20)).ToString(culture) + SPACE + (ly +
                        (height / 2)).ToString(culture) + SPACE + 'Z' + SPACE + 'M' + SPACE + (lx + 3 * (width / 10)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + (lx + 3 * (width / 5)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE
                        + (lx + 3 * (width / 5)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + 3 * (width / 10)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'Z';
                    break;
                case "Bar":
                case "StackingBar":
                case "StackingBar100":
                    options.Direction = "M" + SPACE + (lx + (-width / 2) + (-2.5)).ToString(culture) + SPACE + (ly - 3 * (height / 5)).ToString(culture) + SPACE + 'L' + SPACE + (lx + 3 * (width / 10)).ToString(culture) + SPACE + (ly - 3 * (height / 5)).ToString(culture) + SPACE + 'L' + SPACE +
                        (lx + 3 * (width / 10)).ToString(culture) + SPACE + (ly - 3 * (height / 10)).ToString(culture) + SPACE + 'L' + SPACE + (lx - (width / 2) + (-2.5)).ToString(culture) + SPACE + (ly - 3 * (height / 10)).ToString(culture) + SPACE + 'Z' + SPACE
                        + 'M' + SPACE + (lx + (-width / 2) + (-2.5)).ToString(culture) + SPACE + (ly - (height / 5) + 0.5).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2) + 2.5).ToString(culture) + SPACE + (ly
                        - (height / 5) + 0.5).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2) + 2.5).ToString(culture) + SPACE + (ly + (height / 10) + 0.5).ToString(culture) + SPACE + 'L' + SPACE + (lx - (width / 2) + (-2.5)).ToString(culture) + SPACE + (ly + (height / 10) + 0.5).ToString(culture) + SPACE + 'Z' + SPACE + 'M'
                        + SPACE + (lx - (width / 2) + (-2.5)).ToString(culture) + SPACE + (ly + (height / 5) + 1).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 4)).ToString(culture) + SPACE + (ly + (height / 5)
                        + 1).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 4)).ToString(culture) + SPACE + (ly + (height / 2) + 1).ToString(culture) + SPACE + 'L' + SPACE + (lx - (width / 2) + (-2.5)).ToString(culture) + SPACE + (ly + (height / 2) + 1).ToString(culture) + SPACE + 'Z';
                    break;
                case "Spline":
                    options.Fill = "transparent";
                    options.Direction = "M" + SPACE + (lx - (width / 2)).ToString(culture) + SPACE + (ly + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + lx.ToString(culture) + SPACE + (ly - height).ToString(culture) + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 5)).ToString(culture)
                        + SPACE + 'M' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture);
                    break;
                case "Area":
                case "MultiColoredArea":
                case "RangeArea":
                case "StackingArea":
                case "StackingArea100":
                    options.Direction = "M" + SPACE + (lx - (width / 2) - 2.5).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (-width / 4) + (-1.25)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture)
                        + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 4)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 4) + 1.25).ToString(culture) + SPACE + (ly + (-height / 2) + (height / 4)).ToString(culture) + SPACE
                        + 'L' + SPACE + (lx + (height / 2) + 2.5).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'Z';
                    break;
                case "SplineArea":
                    options.Direction = "M" + SPACE + (lx - (width / 2)).ToString(culture) + SPACE + (ly + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + lx.ToString(culture) + SPACE + (ly - height).ToString(culture) + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 5)).ToString(culture) + SPACE + 'Z' + SPACE + 'M'
                        + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 5)).ToString(culture) + SPACE + 'Q' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + " Z";
                    break;
                case "Pie":
                case "Doughnut":
                    options.Stroke = "transparent";
                    options.Direction = GetAccumulationLegend(lx, ly, Math.Min(height, width) / 2, height, width);
                    break;
            }

            return options;
        }

        private static string GetAccumulationLegend(double locX, double locY, double radius, double height, double width)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;
            ChartInternalLocation cartesianlarge = DegreeToLocation(270, radius, new ChartInternalLocation(locX, locY));
            ChartInternalLocation cartesiansmall = DegreeToLocation(270, radius, new ChartInternalLocation(locX + (width / 10), locY));
            return "M" + SPACE + locX.ToString(culture) + SPACE + locY.ToString(culture) + SPACE + 'L' + SPACE + (locX + radius).ToString(culture) + SPACE + locY.ToString(culture) + SPACE + 'A' + SPACE + radius.ToString(culture) + SPACE + radius.ToString(culture) + SPACE + 0 + SPACE + 1 + SPACE + 1 + SPACE + cartesianlarge.X.ToString(culture) + SPACE + cartesianlarge.Y.ToString(culture) + SPACE + 'Z' + SPACE + 'M' + SPACE + (locX +
               (width / 10)).ToString(culture) + SPACE + (locY - (height / 10)).ToString(culture) + SPACE + 'L' + (locX + radius).ToString(culture) + SPACE + (locY - height / 10).ToString(culture) + SPACE + 'A' + SPACE + radius.ToString(culture) + SPACE + radius.ToString(culture) + SPACE + 0 + SPACE + 0 + SPACE + 0 + SPACE + cartesiansmall.X.ToString(culture) + SPACE + cartesiansmall.Y.ToString(culture) + SPACE + 'Z';
        }

        internal static Rect SubtractRect(Rect rect, Rect thickness)
        {
            rect.X += thickness.X;
            rect.Y += thickness.Y;
            rect.Width -= thickness.X + thickness.Width;
            rect.Height -= thickness.Y + thickness.Height;
            return rect;
        }

        internal static double GetTime(DateTime current)
        {
            return (current - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        internal static bool WithInBounds(double x, double y, Rect bounds, double width = 0, double height = 0)
        {
            return bounds != null && x >= bounds.X - width && x <= bounds.X + bounds.Width + width && y >= bounds.Y - height && y <= bounds.Y + bounds.Height + height;
        }

        internal static bool IsOverlap(Rect currentRect, Rect rect)
        {
            return currentRect.X < rect.X + rect.Width && currentRect.X + currentRect.Width > rect.X && currentRect.Y < rect.Y + rect.Height && currentRect.Height + currentRect.Y > rect.Y;
        }

        internal static List<ChartInternalLocation> GetRotatedRectangleCoordinates(List<ChartInternalLocation> actualPoints, double centerX, double centerY, double angle)
        {
            List<ChartInternalLocation> coordinatesAfterRotation = new List<ChartInternalLocation>();
            for (int i = 0; i < 4; i++)
            {
                ChartInternalLocation point = actualPoints[i];
                double tempX = point.X - centerX;
                double tempY = point.Y - centerY;
                point.X = (tempX * Math.Cos(DegreeToRadian(angle)) - tempY * Math.Sin(DegreeToRadian(angle))) + centerX;
                point.Y = (tempX * Math.Sin(DegreeToRadian(angle)) + tempY * Math.Cos(DegreeToRadian(angle))) + centerY;
                coordinatesAfterRotation.Add(new ChartInternalLocation(point.X, point.Y));
            }

            return coordinatesAfterRotation;
        }

        internal static Color GetRBGValue(string color)
        {
            Color rbgValue;
            char[] getChar;
            if (color.Contains('#', StringComparison.InvariantCulture) && !color.Contains("url", StringComparison.InvariantCulture))
            {
                color = color.Replace("#", string.Empty, StringComparison.InvariantCulture);
                if (color.Length < rgbHexCode)
                {
                    getChar = color.ToCharArray();
                    color = string.Empty;
                    for (int i = 0; i < getChar.Length; i++)
                    {
                        color += getChar[i].ToString() + getChar[i].ToString();
                    }
                }

                rbgValue = Color.FromArgb(int.Parse(color.Substring(0, 2), NumberStyles.AllowHexSpecifier, null), int.Parse(color.Substring(2, 2), NumberStyles.AllowHexSpecifier, null), int.Parse(color.Substring(4, 2), NumberStyles.AllowHexSpecifier, null));           
            }
            else
            {
                color = (Color.FromName(color).A != 0) ? color : DEFAULTCOLOR;
                rbgValue = Color.FromName(color);
            }

            return rbgValue;
        }

        internal static int[] IndexFinder(string id, bool isPoint = false)
        {
            string[] ids = new string[] { "-1", "-1" };
            if (id.Contains("_Point_", StringComparison.InvariantCulture))
            {
                ids = id.Split("_Series_")[1].Split("_Point_");
                if (ids[1].Contains("Text", StringComparison.InvariantCulture))
                {
                    ids[1] = ids[1].Split("_Text_")[0];
                }
            }
            else if (id.Contains("_shape_", StringComparison.InvariantCulture) && (!isPoint || (isPoint && !id.Contains("_legend_", StringComparison.InvariantCulture))))
            {
                ids = id.Split("_shape_");
                ids[0] = ids[1];
            }
            else if (id.Contains("_text_", StringComparison.InvariantCulture) && (!isPoint || (isPoint && !id.Contains("_legend_", StringComparison.InvariantCulture))))
            {
                ids = id.Split("_text_");
                if (id.Contains("datalabel", StringComparison.InvariantCulture))
                {
                    ids[0] = ids[0].Split("_Series_")[1];
                }
                else
                {
                    ids[0] = ids[1];
                }
            }
            else if (id.Contains("Series", StringComparison.InvariantCulture) && !id.Contains("points", StringComparison.InvariantCulture))
            {
                ids[0] = id.Split("_Series_")[1].Split("_")[0];
            }
#pragma warning disable CA1806
            Int32.TryParse(ids[0], out int series);
            Int32.TryParse(ids[1].Split("_")[0], out int point);
#pragma warning restore CA1806 
            return new int[] { series, point };
        }

        internal static string FindDirection(double rx, double ry, Rect rect, ChartInternalLocation arrowLocation, int arrowPadding,
            bool top, bool bottom, bool left, double tipX, double tipY, double tipRadius = 0)
        {
            string direction = string.Empty;
            CultureInfo culture = CultureInfo.InvariantCulture;
            double startX = rect.X, startY = rect.Y, width = rect.X + rect.Width, height = rect.Y + rect.Height;
            if (top)
            {
                direction += "M" + SPACE + startX.ToString(culture) + SPACE + (startY + ry).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (startX + rx).ToString(culture) + SPACE + startY.ToString(culture) + SPACE +
                    " L" + SPACE + (width - rx).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (startY + ry).ToString(culture);
                direction += " L" + SPACE + width.ToString(culture) + SPACE + (height - ry).ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (width - rx).ToString(culture) + SPACE + (height).ToString(culture);
                if (arrowPadding != 0)
                {
                    direction += " L" + SPACE + (arrowLocation.X + (arrowPadding / 2)).ToString(culture) + SPACE + height.ToString(culture);
                    direction += " L" + SPACE + (tipX + tipRadius).ToString(culture) + SPACE + (height + arrowPadding - tipRadius).ToString(culture);
                    direction += " Q" + SPACE + (tipX).ToString(culture) + SPACE + (height + arrowPadding).ToString(culture) + SPACE + (tipX - tipRadius).ToString(culture) + SPACE + (height + arrowPadding - tipRadius).ToString(culture);
                }

                if ((arrowLocation.X - (arrowPadding / 2)) > startX)
                {
                    direction += " L" + SPACE + (arrowLocation.X - (arrowPadding / 2)).ToString(culture) + SPACE + height.ToString(culture) + " L" + SPACE + (startX + rx).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (startX).ToString(culture) + SPACE + (height - ry).ToString(culture) + " z";
                }
                else
                {
                    if (arrowPadding == 0)
                    {
                        direction += " L" + SPACE + (startX + rx).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (startX).ToString(culture) + SPACE + (height - ry).ToString(culture) + " z";
                    }
                    else
                    {
                        direction += " L" + SPACE + (startX).ToString(culture) + SPACE + (height + ry).ToString(culture) + " z";
                    }
                }
            }
            else if (bottom)
            {
                direction += "M" + SPACE + startX.ToString(culture) + SPACE + (startY + ry).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (startX + rx).ToString(culture) + SPACE + startY.ToString(culture) + " L" + SPACE + (arrowLocation.X - arrowPadding / 2).ToString(culture) + SPACE + startY.ToString(culture);
                direction += " L" + SPACE + (tipX - tipRadius).ToString(culture) + SPACE + (arrowLocation.Y + tipRadius).ToString(culture) + " Q" + SPACE + tipX.ToString(culture) + SPACE + arrowLocation.Y.ToString(culture) + SPACE + (tipX + tipRadius).ToString(culture) + SPACE + (arrowLocation.Y + tipRadius).ToString(culture);
                direction += " L" + SPACE + (arrowLocation.X + arrowPadding / 2).ToString(culture) + SPACE + startY.ToString(culture) + " L" + SPACE + (width - rx).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (startY + ry).ToString(culture);
                direction += " L" + SPACE + width.ToString(culture) + SPACE + (height - ry).ToString(culture) + " Q " + width.ToString(culture) + SPACE + (height).ToString(culture) + SPACE + (width - rx).ToString(culture) + SPACE + height.ToString(culture) + " L" + SPACE + (startX + rx).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (height - ry).ToString(culture) + " z";
            }
            else if (left)
            {
                direction += "M" + SPACE + startX.ToString(culture) + SPACE + (startY + ry).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (startX + rx).ToString(culture) + SPACE + startY.ToString(culture);
                direction += " L" + SPACE + (width - rx).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (startY + ry).ToString(culture) + " L" + SPACE + width.ToString(culture) + SPACE + (arrowLocation.Y - arrowPadding / 2).ToString(culture);
                direction += " L" + SPACE + (width + arrowPadding - tipRadius).ToString(culture) + SPACE + (tipY - tipRadius).ToString(culture);
                direction += " Q " + (width + arrowPadding).ToString(culture) + SPACE + tipY.ToString(culture) + SPACE + (width + arrowPadding - tipRadius).ToString(culture) + SPACE + (tipY + tipRadius).ToString(culture);
                direction += " L" + SPACE + width.ToString(culture) + SPACE + (arrowLocation.Y + arrowPadding / 2).ToString(culture) + " L" + SPACE + width.ToString(culture) + SPACE + (height - ry).ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (width - rx).ToString(culture) + SPACE + height.ToString(culture);
                direction += " L" + SPACE + (startX + rx).ToString(culture) + SPACE + height.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + (height).ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (height - ry).ToString(culture) + " z";
            }
            else
            {
                direction += "M" + SPACE + (startX + rx).ToString(culture) + SPACE + startY.ToString(culture) + " Q " + startX.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + startX.ToString(culture) + SPACE + (startY + ry).ToString(culture) + " L" + SPACE + startX.ToString(culture) + SPACE + (arrowLocation.Y - arrowPadding / 2).ToString(culture);
                direction += " L" + SPACE + (startX - arrowPadding + tipRadius).ToString(culture) + SPACE + (tipY - tipRadius).ToString(culture) + " Q " + (startX - arrowPadding).ToString(culture) + SPACE + tipY.ToString(culture) + SPACE + (startX - arrowPadding + tipRadius).ToString(culture) + SPACE + (tipY + tipRadius).ToString(culture);
                direction += " L" + SPACE + (startX).ToString(culture) + SPACE + (arrowLocation.Y + arrowPadding / 2).ToString(culture) + " L" + SPACE + startX.ToString(culture) + SPACE + (height - ry).ToString(culture) + " Q " + startX.ToString(culture) + SPACE + height.ToString(culture) + SPACE + (startX + rx).ToString(culture) + SPACE + height.ToString(culture);
                direction += " L" + SPACE + (width - rx).ToString(culture) + SPACE + height.ToString(culture) + " Q " + width.ToString(culture) + SPACE + height.ToString(culture) + SPACE + width.ToString(culture) + SPACE + (height - ry).ToString(culture) + " L" + SPACE + width.ToString(culture) + SPACE + (startY + ry).ToString(culture) + " Q " + width.ToString(culture) + SPACE + startY.ToString(culture) + SPACE + (width - rx).ToString(culture) + SPACE + startY.ToString(culture) + " z";
            }

            return direction;
        }

        internal static double SubArraySum(object[] values, double first, double last, double[] index, ChartSeries series, Type type)
        {
            double sum = 0;
            int start = Convert.ToInt32(first + 1);
            if (index != null)
            {
                for (int i = start; i < last; i++)
                {
                    if (Array.IndexOf(index, i) == -1)
                    {
                        sum += Convert.ToDouble(type.GetProperty(series.YName).GetValue(values[i]));
                    }
                }
            }
            else
            {
                for (int i = start; i < last; i++)
                {
                    if (type.GetProperty(series.YName).GetValue(values[i]) != null)
                    {
                        sum += Convert.ToDouble(type.GetProperty(series.YName).GetValue(values[i]));
                    }
                }
            }

            return sum;
        }

        internal static List<object> Sort(List<object> data, string yname, Type type)
        {
            double first = 0, second = 0;
            data.Sort((a, b) =>
            {
                first = Convert.ToDouble(type.GetProperty(yname).GetValue(a));
                second = Convert.ToDouble(type.GetProperty(yname).GetValue(b));
                if (first > second)
                {
                    return -1;
                }

                if (first == second)
                {
                    return 0;
                }

                return 1;
            });
            return data;
        }

        internal static double GetMedian(double[] points)
        {
            int half = Convert.ToInt32(Math.Floor((double)(points.Length / 2)));
            return points.Length % 2 != 0 ? points[half] : ((points[half - 1] + points[half]) / 2.0);
        }

        internal static string GetSaturationColor(string color, double factor)
        {
            if (!color.Contains('#', StringComparison.InvariantCulture))
            {
                color = string.Format(null, "{0:x6}", Color.FromName(color).ToArgb());
            }

            color = color.Replace("#", string.Empty, StringComparison.InvariantCulture);
            if (color.Length < 6)
            {
                color = string.Empty + color[0] + color[0] + color[1] + color[1] + color[2] + color[2];
            }

            string rgb = "#";
            for (int i = 0; i < 3; i++)
            {
                int colorCode = Int32.Parse(color.Substring(i * 2, 2), NumberStyles.HexNumber, null);
                colorCode = Convert.ToInt32(Math.Round(Math.Min(Math.Max(0, colorCode + (colorCode * factor)), 255)));
                rgb += ("00" + string.Format(null, "{0:x6}", colorCode)).Substring(string.Format(null, "{0:x6}", colorCode).Length);
            }

            return rgb;
        }

        internal static void TextElement(RenderTreeBuilder builder, SvgRendering svgRenderer, TextOptions option)
        {
            int count = option.TextCollection.Count;
            option.TextLocationCollection = new List<TextLocation>();
            option.Text = count == 0 ? option.Text : option.IsMinus ? option.TextCollection[count - 1] : option.TextCollection[0];
            if (count > 0)
            {
                for (int i = 1; i < count; i++)
                {
                    double height = MeasureText(option.TextCollection[i], (ChartFontOptions)option.Font).Height;
                    option.TextLocationCollection.Add(new TextLocation(option.IsMinus ? option.TextCollection[count - (i + 1)] : option.TextCollection[i], Convert.ToDouble(option.Y, CultureInfo.InvariantCulture) + (option.IsMinus ? -(i * height) : (i * height))));
                }
            }

            option.ChildContent = RenderTSpan(option);
            if (!option.IsRotatedLabelIntersect && svgRenderer != null)
            {
                svgRenderer.RenderText(builder, option);
                
                // TODO: Need to include append child function for animation.
            }
        }

        private static RenderFragment RenderTSpan(TextOptions option) => builder =>
        {
            for (int i = 0; i < option.TextLocationCollection.Count; i++)
            {
                builder.OpenElement(SvgRendering.Seq++, "tspan");
                Dictionary<string, object> svgattributes = new Dictionary<string, object> {
                    { "id", option.Id},
                    { "x", option.X.ToString(CultureInfo.InvariantCulture) },
                    { "y", option.TextLocationCollection[i].Y.ToString(CultureInfo.InvariantCulture) }
                };
                builder.AddMultipleAttributes(SvgRendering.Seq++, svgattributes);
                builder.AddContent(SvgRendering.Seq++, option.TextLocationCollection[i].Text);
                builder.CloseElement();
            }
        };

        internal static Size RotateTextSize(ChartFontOptions labelStyle, string text, double angle)
        {
            Size size = MeasureText(text, labelStyle);
            double theta = angle * Math.PI / 180.0;
            while (theta < 0.0)
            {
                theta += 2 * Math.PI;
            }

            double adjacentTop, oppositeTop, adjacentBottom, oppositeBottom;
            if ((theta >= 0.0 && theta < Math.PI / 2.0) || (theta >= Math.PI && theta < (Math.PI + (Math.PI / 2.0))))
            {
                adjacentTop = Math.Abs(Math.Cos(theta)) * size.Width;
                oppositeTop = Math.Abs(Math.Sin(theta)) * size.Width;
                adjacentBottom = Math.Abs(Math.Cos(theta)) * size.Height;
                oppositeBottom = Math.Abs(Math.Sin(theta)) * size.Height;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(theta)) * size.Height;
                oppositeTop = Math.Abs(Math.Cos(theta)) * size.Height;
                adjacentBottom = Math.Abs(Math.Sin(theta)) * size.Width;
                oppositeBottom = Math.Abs(Math.Cos(theta)) * size.Width;
            }

            return new Size((int)Math.Ceiling(adjacentTop + oppositeBottom), (int)Math.Ceiling(adjacentBottom + oppositeTop));
        }

        internal static Rect GetTransform(Rect xAxisRect, Rect yAxisRect, bool invertedAxis)
        {
            if (invertedAxis)
            {
                return new Rect(yAxisRect.X, xAxisRect.Y, yAxisRect.Width, xAxisRect.Height);
            }
            else
            {
                return new Rect(xAxisRect.X, yAxisRect.Y, xAxisRect.Width, yAxisRect.Height);
            }
        }

        // internal void TriggerLabelRender(SfChart chart, double tempInterval, string text, ChartFontoptions labelStyle, ChartAxis axis)
        // {
        //     AxisLabelRenderEventArgs argsData = new AxisLabelRenderEventArgs(Constants.AXISLABELRENDER, false, axis, text, tempInterval, labelStyle);
        //     if (chart?.ChartEvents?.OnAxisLabelRender != null && chart.ChartAxisLayoutPanel.IsAxisLabelRender)
        //         chart.ChartEvents.OnAxisLabelRender.Invoke(argsData);
        //     if (!argsData.Cancel)
        //     {
        //         axis.VisibleLabels.Add(new VisibleLabels(axis.EnableTrim ? TextTrim(axis.MaximumLabelWidth, argsData.Text, axis.LabelStyle) : argsData.Text, argsData.Value, (ChartAxisLabelStyle)argsData.LabelStyle, argsData.Text));
        //     }
        // }
           
        internal static ScrollbarThemeStyle GetScrollbarThemeColor(Theme theme)
        {
            switch (theme)
            {
                case Theme.HighContrastLight:
                    return GetScrollbarStyle("#333", "#bfbfbf", "#fff", "#685708", "#333", "#333", "#fff", "#969696");
                case Theme.Bootstrap:
                    return GetScrollbarStyle("#f5f5f5", "#e6e6e6", "#fff", "#eee", "#8c8c8c", "#8c8c8c");
                case Theme.Fabric:
                    return GetScrollbarStyle("#f8f8f8", "#eaeaea", "#fff", "#eaeaea", "#a6a6a6", "#a6a6a6");
                default:
                    return GetScrollbarStyle("#f5f5f5", "#e0e0e0", "#fff", "#eee", "#9e9e9e", "#9e9e9e");
            }
        }

        private static ScrollbarThemeStyle GetScrollbarStyle(string backRect, string thumb, string circle, string circleHover, string arrow, string grip, string arrowHover = null, string backRectBorder = null)
        {
            return new ScrollbarThemeStyle()
            {
                BackRect = backRect,
                Thumb = thumb,
                Circle = circle,
                CircleHover = circleHover,
                Arrow = arrow,
                Grip = grip,
                ArrowHover = arrowHover,
                BackRectBorder = backRectBorder
            };
        }

        internal static double StringToNumber(string size, double containerSize)
        {
            if (!string.IsNullOrEmpty(size) && size != "auto")
            {
               return size.Contains('%', StringComparison.InvariantCulture) ? (containerSize / 100) * double.Parse(size.Replace("%", SPACE, StringComparison.InvariantCulture), null) : double.Parse(size.ToLower(CultureInfo.CurrentCulture).Replace("px", string.Empty, StringComparison.InvariantCulture), provider: CultureInfo.InvariantCulture);
            }

            return double.NaN;
        }

        internal static string TextTrim(double maxWidth, string text, ChartFontOptions font)
        {
            string label = text;
            double size = MeasureText(text, font).Width;
            if (Math.Round(size) > Math.Round(maxWidth))
            {
                for (int i = text.Length - 1; i >= 0; --i)
                {
                    label = text.Substring(0, i) + "...";
                    size = MeasureText(label, font).Width;
                    if (size <= maxWidth)
                    {
                        return label;
                    }
                }
            }

            return label;
        }

        internal static double TitlePositionX(Rect rect, Alignment textAlignment)
        {
            if (textAlignment == Alignment.Near)
            {
                return rect.X;
            }
            else if (textAlignment == Alignment.Center)
            {
                return rect.X + (rect.Width / 2);
            }
            else
            {
                return rect.X + rect.Width;
            }
        }

        internal static string GetUniCode(string text, string pattern, Regex regExp)
        {
            string title = Regex.Replace(text, pattern, SPACE), convertedText = SPACE;
            MatchCollection digit = regExp.Matches(text);
            Dictionary<char, string> UnicodeSub = new Dictionary<char, string>() { { '0', "\u2080" }, { '1', "\u2081" }, { '2', "\u2082" }, { '3', "\u2083" }, { '4',"\u2084"},
                { '5',"\u2085"}, { '6',"\u2086"}, {'7',"\u2087"}, {'8',"\u2088"}, {'9',"\u2089"}};
            Dictionary<char, string> UnicodeSup = new Dictionary<char, string>() { { '0', "\u2070" }, { '1', "\u00B9" }, { '2', "\u00B2" }, { '3', "\u00B3" }, { '4',"\u2074"},
                { '5',"\u2075"}, {'6',"\u2076"}, {'7',"\u2077"}, {'8',"\u2078"}, {'9',"\u2079"}};
            int k = 0;
            for (int i = 0; i <= title.Length - 1; i++)
            {
                if (title[i].Equals(SPACE))
                {
                    string DigitSpecific = (regExp == Constants.REGSUB) ? Convert.ToString(digit[k], null).Replace("~", string.Empty, StringComparison.Ordinal) : Convert.ToString(digit[k], null).Replace("^", string.Empty, StringComparison.Ordinal);
                    for (int j = 0; j < DigitSpecific.Length; j++)
                    {
                        convertedText += (regExp == Constants.REGSUB) ? UnicodeSub[DigitSpecific[j]] : UnicodeSup[DigitSpecific[j]];
                    }

                    k++;
                }
                else
                {
                    convertedText += title[i];
                }
            }

            return convertedText.Trim();
        }

        internal static void TextElement(RenderTreeBuilder builder, SvgRendering render, TextOptions option, ChartFontOptions font, string color, string accessText)
        {
            render.RenderText(builder, new TextOptions { X = option.X, Y = option.Y, Id = option.Id, Fill = !string.IsNullOrEmpty(color) ? color : "black", FontSize = font.Size, FontFamily = font.FontFamily, FontStyle = font.FontStyle, FontWeight = font.FontWeight, Text = option.Text, TextAnchor = option.TextAnchor, AccessibilityText = accessText });
        }

        internal static double GetActualDesiredIntervalsCount(Size availableSize, double desiredIntervals, Orientation orientation, double maximumLabels)
        {
            if (double.IsNaN(desiredIntervals))
            {
                return Math.Max((orientation == Orientation.Horizontal ? availableSize.Width : availableSize.Height) * (((orientation == Orientation.Horizontal ? 0.533 : 1) * maximumLabels) / 100), 1);
            }
            else
            {
                return desiredIntervals;
            }
        }

        internal static ChartInternalLocation DegreeToLocation(double degree, double radius, ChartInternalLocation center)
        {
            double radian = (degree * Math.PI) / 180;
            return new ChartInternalLocation(Math.Cos(radian) * radius + center.X, Math.Sin(radian) * radius + center.Y);
        }

        internal static List<Point> GetVisiblePoints(List<Point> points)
        {
            List<Point> tempPoints = new List<Point>();
            int pointIndex = 0;
            for (int i = 0; i < points.Count; i++)
            {
                Point tempPoint = points[i];
                if (tempPoint.X == null || string.IsNullOrEmpty(Convert.ToString(tempPoint.X, null)))
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

        internal static List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            string labelFormat = seriesRenderer.YAxisRenderer.Axis.LabelFormat;
            bool customLabelFormat = labelFormat.Contains("{value}", StringComparison.InvariantCulture);

            // TODO: Need to override seriesRenderer.GetLabelText for all series types
            List<string> text = seriesRenderer.GetLabelText(currentPoint, seriesRenderer);
            if (!string.IsNullOrEmpty(labelFormat) && string.IsNullOrEmpty(currentPoint.Text))
            {
                for (int i = 0; i < text.Count; i++)
                {
                    text[i] = customLabelFormat ? labelFormat.Replace("{value}", ChartAxisRenderer.FormatAxisValue(Convert.ToDouble(text[i], null), customLabelFormat, labelFormat), StringComparison.InvariantCulture) : ChartAxisRenderer.FormatAxisValue(Convert.ToDouble(text[i], null), customLabelFormat, labelFormat);
                }
            }

            return text;
        }

        internal static Rect CalculateRect(ChartInternalLocation location, Size textSize, ChartEventMargin margin)
        {
            return new Rect(location.X - (textSize.Width / 2) - margin.Left, location.Y - (textSize.Height / 2) - margin.Top, textSize.Width + margin.Left + margin.Right, textSize.Height + margin.Top + margin.Bottom);
        }

        internal static bool IsCollide(Rect rect, List<Rect> collections, Rect clipRect, bool isCartesianAxes)
        {
            Rect currentRect = new Rect(rect.X + clipRect.X, rect.Y + clipRect.Y, rect.Width, rect.Height);
            return (collections.Count != 0 && collections.Any(rect => currentRect.X < (rect.X + rect.Width) && (currentRect.X + currentRect.Width) > rect.X &&
                     currentRect.Y < (rect.Y + rect.Height) && (currentRect.Height + currentRect.Y) > rect.Y)) || (isCartesianAxes && (currentRect.X + currentRect.Width) > (clipRect.X + clipRect.Width) || (currentRect.Y + currentRect.Height) > (clipRect.Y + clipRect.Height));
        }

        private static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        internal static bool IsRotatedRectIntersect(List<ChartInternalLocation> a, List<ChartInternalLocation> b)
        {
            List<List<ChartInternalLocation>> polygons = new List<List<ChartInternalLocation>>() { a, b };
            double minA, maxA, projected, minB, maxB;
            for (int i = 0; i < polygons.Count; i++)
            {
                List<ChartInternalLocation> polygon = polygons[i];
                for (int k = 0; k < polygon.Count; k++)
                {
                    int i2 = (k + 1) % polygon.Count;
                    ChartInternalLocation normal = new ChartInternalLocation(polygon[i2].Y - polygon[k].Y, polygon[k].X - polygon[i2].X);
                    minA = maxA = 0;
                    for (int j = 0; j < a.Count; j++)
                    {
                        projected = (normal.X * a[j].X) + (normal.Y * a[j].Y);
                        if (projected < minA)
                        {
                            minA = projected;
                        }

                        if (projected > maxA)
                        {
                            maxA = projected;
                        }
                    }

                    minB = maxB = 0;
                    for (int j = 0; j < b.Count; j++)
                    {
                        projected = (normal.X * b[j].X) + (normal.Y * b[j].Y);
                        if (projected < minB)
                        {
                            minB = projected;
                        }

                        if (projected > maxB)
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

        internal static string GetFontStyle(ChartDefaultFont font)
        {
            return "font-size:" + font.Size + "; font-style:" + font.FontStyle + "; font-weight:" + font.FontWeight + "; font-family:" + font.FontFamily + ";opacity:" + font.Opacity + "; color:" + font.Color + ";";
        }

        internal static VisibleRangeModel GetVisibleRangeModel(DoubleRange doubleRange, double interval)
        {
            return new VisibleRangeModel()
            {
                Min = doubleRange.Start,
                Max = doubleRange.End,
                Delta = doubleRange.Delta,
                Interval = interval
            };
        }

        /// <summary>
        /// The method is used to get the textAnchor of title / subTitle.
        /// </summary>
        /// <param name="textAlignment">Specifies the text alignment of title.</param>
        /// /// <param name="enableRTL">Specifies the chart render in RTL mode.</param>
        internal static string GetTextAnchor(Alignment textAlignment, bool enableRTL)
        {
            switch (textAlignment)
            {
                case Alignment.Near:
                    return enableRTL ? "end" : "start";
                case Alignment.Far:
                    return enableRTL ? "start" : "end";
                default:
                    return "middle";
            }
        }

        internal static void Dispose()
        {
            // TODO: This solution has been provided for <BLAZ-10049> and <Incident 313131>. Since "sizePerCharacter" is used as a static property, during dispose its override to both chart & accumulation chart simultaneously when using in dashboard layout panel. In future we may need to change the internal property.
            sizePerCharacter.Clear();
            JsRuntime = null;
        }
    }
}