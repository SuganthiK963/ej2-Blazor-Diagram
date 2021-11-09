using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the helper methods of smith chart component.
    /// </summary>
    public class SmithChartHelper
    {
        private const string SPACE = " ";

        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal static Dictionary<string, Size> SizePerCharacter { get; set; } = new Dictionary<string, Size>();

        internal static Rect SubtractRect(Rect rect, Rect thickness)
        {
            rect.X += thickness.X;
            rect.Y += thickness.Y;
            rect.Width -= thickness.X + thickness.Width;
            rect.Height -= thickness.Y + thickness.Height;
            return rect;
        }

        internal static double GetEpsilonValue()
        {
            double e = 1.0;
            while ((1.0 + (0.5 * e)) != 1.0)
            {
                e *= 0.5;
            }

            return e;
        }

        internal static string TextTrim(double maxWidth, string text, SmithChartCommonFont font)
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

        internal static Size MeasureText(string text, SmithChartCommonFont font)
        {
            double width = 0, height = 0, fontSize = DataVizCommonHelper.PixelToNumber(font.Size, new System.Text.RegularExpressions.Regex(SmithChartConstants.NUMPATTERN));
            for (int i = 0; i < text?.Length; i++)
            {
                Size charSize = GetCharSize(text[i], font);
                width += charSize.Width;
                height = Math.Max(charSize.Height, height);
            }

            return new Size((width * fontSize) / 100, (height * fontSize) / 100);
        }

        internal static void TextElement(RenderTreeBuilder builder, TextOptions option, SmithChartCommonFont font, SvgRendering svgRenderer)
        {
            int count = option.TextCollection.Count;
            option.Font = font;
            option.TextLocationCollection = new List<TextLocation>();
            option.Text = count == 0 ? option.Text : option.TextCollection[0];
            if (count > 0)
            {
                for (int i = 1; i < count; i++)
                {
                    double height = MeasureText(option.TextCollection[i], (SmithChartCommonFont)option.Font).Height;
                    option.TextLocationCollection.Add(new TextLocation(option.TextCollection[i], Convert.ToDouble(option.Y, CultureInfo.InvariantCulture) + (i * height)));
                }
            }

            option.ChildContent = DataVizCommonHelper.RenderTSpan(option);
            svgRenderer?.RenderText(builder, option);
        }

        internal static SmithChartThemeStyle GetSmithChartThemeStyle(Theme theme)
        {
            string darkBackground = theme == Theme.MaterialDark ? "#303030" : (theme == Theme.FabricDark ? "#201F1F" : "#1A1A1A");
            switch (theme)
            {
                case Theme.HighContrastLight:
                case Theme.HighContrast:
                    return new SmithChartThemeStyle("#ffffff", "#ffffff", "#BFBFBF", "#969696", "#ffffff", "#ffffff", "#000000", "#ffffff", "#ffffff", "#ffffff", "#000000", "#000000", "#969696");
                case Theme.MaterialDark:
                case Theme.FabricDark:
                case Theme.BootstrapDark:
                    return new SmithChartThemeStyle("#DADADA", "#6F6C6C", "#414040", "#514F4F", "#ffffff", "#DADADA", darkBackground, "#9A9A9A", "#F4F4F4", "#DADADA", "#282727", "#333232", "#9A9A9A");
                case Theme.Bootstrap4:
                    return new SmithChartThemeStyle("#212529", "#ADB5BD", "#CED4DA", "#DEE2E6", "#212529", "#212529", "#FFFFFF", "#DEE2E6", "#000000", "#212529", "#FFFFFF", "#FFFFFF", "#FFFFFF");
                case Theme.Tailwind:
                    return new SmithChartThemeStyle("#6B7280", "#D1D5DB", "#E5E7EB", "#D1D5DB", "#374151", "#374151", "#FFFFFF", "#D1D5DB6", "#111827", "#F9FAFB", "#F9FAFB", "#F9FAFB", "#9CA3AF");
                case Theme.TailwindDark:
                    return new SmithChartThemeStyle("#9CA3AF", "#4B5563", "#374151", "#4B5563", "#D1D5DB", "#D1D5DB", "#1F2937", "#4B5563", "#F9FAFB", "#D1D5DB", "#1F2937", "#1F2937", "#9CA3AF");
                case Theme.Bootstrap5:
                    return new SmithChartThemeStyle("#495057", "#D1D5DB", "#E5E7EB", "#E5E7EB", "#343A40", "#343A40", "rgba(255, 255, 255, 0.0)", "#DEE2E6", "#212529", "#D1D5DB", "#D1D5DB", "#F9FAFB", "#6B7280");
                case Theme.Bootstrap5Dark:
                    return new SmithChartThemeStyle("#CED4DA", "#495057", "#495057", "#495057", "#E9ECEF", "#E9ECEF", "rgba(255, 255, 255, 0.0)", "#495057", "#212529", "#D1D5DB", "#D1D5DB", "#F9FAFB", "#6B7280");
                default:
                    return new SmithChartThemeStyle("#686868", "#b5b5b5", "#dbdbdb", "#eaeaea", "#424242", "#353535", "#FFFFFF", "Gray", "rgba(0, 8, 22, 0.75)", "#424242", "#ffffff", "#dbdbdb", "#ffffff");
            }
        }

        internal static double StringToNumber(string sizeValue, double containerSize)
        {
            if (!string.IsNullOrEmpty(sizeValue))
            {
                double size = sizeValue.Contains('%', StringComparison.InvariantCulture) ? (containerSize / 100) * int.Parse(sizeValue.Replace("%", string.Empty, StringComparison.InvariantCulture), null) : int.Parse(sizeValue.Replace("px", string.Empty, StringComparison.InvariantCulture), null);
                return size != 0 ? size : containerSize;
            }

            return containerSize;
        }

        internal void DrawSymbol(RenderTreeBuilder builder, SvgRendering svgRenderer, Point location, string shape, Size size, PathOptions option)
        {
            SymbolOptions shapeoption = CalculateShapes(location, size, shape, option);
            if (shapeoption.ShapeName == ShapeName.path)
            {
                shapeoption.PathOption.Visibility = option.Visibility;
                svgRenderer.RenderPath(builder, shapeoption.PathOption);
            }

            if (shapeoption.ShapeName == ShapeName.ellipse)
            {
                shapeoption.EllipseOption.Visibility = option.Visibility;
                svgRenderer.RenderEllipse(builder, shapeoption.EllipseOption);
            }
        }

        internal SymbolOptions CalculateShapes(Point location, Size size, string shape, PathOptions option)
        {
            double width = size.Width,
            height = size.Height, lx = location.X, ly = location.Y, y = location.Y + (-height / 2), x = location.X + (-width / 2);
            SymbolOptions symbolOption = new SymbolOptions();
            symbolOption.ShapeName = ShapeName.path;
            switch (shape)
            {
                case "Circle":
                    symbolOption.ShapeName = ShapeName.ellipse;
                    symbolOption.EllipseOption = new EllipseOptions(option.Id, Convert.ToString(width / 2, culture), Convert.ToString(height / 2, culture), Convert.ToString(lx, culture), Convert.ToString(ly, culture), option.StrokeDashArray, option.StrokeWidth, option.Stroke, option.Opacity, option.Fill);
                    break;
                case "Diamond":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + ly.ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + ly.ToString(culture) + " z";
                    break;
                case "Rectangle":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + " z";
                    break;
                case "Triangle":
                    option.Direction = "M" + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (-height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + x.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + " z";
                    break;
                case "InvertedTriangle":
                    option.Direction = "M" + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + lx.ToString(culture) + SPACE + (ly + (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx - (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + SPACE + 'L' + SPACE + (lx + (width / 2)).ToString(culture) + SPACE + (ly - (height / 2)).ToString(culture) + " z";
                    break;
                case "Pentagon":
                    string dir = string.Empty;
                    for (int i = 0; i <= 5; i++)
                    {
                        double xval = (width / 2) * Math.Cos((Math.PI / 180) * (i * 72)),
                        yval = (height / 2) * Math.Sin((Math.PI / 180) * (i * 72));
                        if (i == 0)
                        {
                            dir = "M" + SPACE + (lx + xval).ToString(culture) + SPACE + (ly + yval).ToString(culture) + SPACE;
                        }
                        else
                        {
                            dir = dir + 'L' + SPACE + (lx + xval).ToString(culture) + SPACE + (ly + yval).ToString(culture) + SPACE;
                        }
                    }

                    dir = dir + 'Z';
                    option.Direction = dir;
                    break;
            }

            symbolOption.PathOption = option;
            return symbolOption;
        }

        private static Size GetCharSize(char character, SmithChartCommonFont font)
        {
            string key = character + "_" + font.FontWeight + "_" + font.FontStyle + "_" + font.FontFamily;
            if (!SizePerCharacter.TryGetValue(key, out Size charSize))
            {
                FontInfo fontInfo = new FontInfo();
                fontInfo.Chars.TryGetValue(character, out double charWidth);
                SizePerCharacter[key] = new Size(charWidth * 6.25, 130);
                SizePerCharacter.TryGetValue(key, out charSize);
            }

            return charSize;
        }
    }
}