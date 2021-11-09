using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Syncfusion.Blazor.Charts.BulletChart.Internal
{
    /// <summary>
    /// Specifies helper methods of the component rendering.
    /// </summary>
    public class BulletChartHelper
    {
        private StringComparison comparison = StringComparison.InvariantCulture;

        internal static string TextFormatter(string format, string data)
        {
            if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(data))
            {
                string[] split = format.Split("{value}");
                if (split.Length > 1)
                {
                    return string.Join(string.Format(CultureInfo.InvariantCulture, "{0}", data), split);
                }
            }

            return data;
        }

        internal static Style GetThemeStyle(Theme theme)
        {
            string darkBackground = (theme == Theme.MaterialDark) ? "#303030" : ((theme == Theme.FabricDark) ? "#201F1F" : "#1A1A1A");
            Style style;
            switch (theme)
            {
                case Theme.Fabric:
                    style = new Style("#424242", "#424242", "#FFFFFF", "#666666", "#666666", "SegoeUI", "rgba(0, 8, 22, 0.75)", "#353535", "#FFFFFF", "#181818", "#181818", "#333333", "#FFFFFF", "SegoeUI", "#666666", "SegoeUI", new string[] { "#959595", "#BDBDBD", "#E3E2E2" });
                    break;
                case Theme.Bootstrap:
                    style = new Style("#424242", "#424242", "#FFFFFF", "rgba(0,0,0,0.54)", "rgba(0,0,0,0.54)", "Helvetica", "rgba(0, 0, 0, 0.9)", "#212529", "rgba(255,255,255, 1)", "#181818", "#181818", "rgba(0,0,0,0.87)", "#FFFFFF", "Helvetica-Bold", "rgba(0,0,0,0.54)", "Helvetica", new string[] { "#959595", "#BDBDBD", "#E3E2E2" });
                    break;
                case Theme.HighContrast:
                    style = new Style("#FFFFFF", "#FFFFFF", "#000000", "#FFFFFF", "#FFFFFF", "SegoeUI", "#FFFFFF", "#FFFFFF", "#000000", "#000000", "#000000", "#FFFFFF", "#FFFFFF", "HelveticaNeue", "#FFFFFF", "SegoeUI", new string[] { "#757575", "#BDBDBD", "#EEEEEE" });
                    break;
                case Theme.MaterialDark:
                case Theme.FabricDark:
                case Theme.BootstrapDark:
                    style = new Style("#F0F0F0", "#F0F0F0", darkBackground, "#FFFFFF", "#FFFFFF", "Helvetica", "#F4F4F4", "#DADADA", "#282727", "#181818", "#181818", "#FFFFFF", "#FFFFFF", "Helvetica-Bold", "#FFFFFF", "Helvetica", new string[] { "#8D8D8D", "#ADADAD", "#EEEEEE" });
                    break;
                case Theme.Bootstrap4:
                    style = new Style("#424242", "#424242", "#FFFFFF", "#202528", "#202528", "HelveticaNeue", "rgba(0, 0, 0, 0.9)", "#212529", "rgba(255,255,255, 1)", "#181818", "#181818", "#202528", "#FFFFFF", "HelveticaNeue-Bold", "HelveticaNeue", "#202528", new string[] { "#959595", "#BDBDBD", "#E3E2E2" });
                    break;
                case Theme.Bootstrap5:
                    style = new Style("#CED4DA", "#CED4DA", "transparent", "#495057", "#495057", "HelveticaNeue", "#212529", "#343A40", "#F9FAFB", "#1F2937", "#1F2937", "#343A40", "#495057", "HelveticaNeue", "#343A40", "HelveticaNeue", new string[] { "#9CA3AF", "#D1D5DB", "#E5E7EB" });
                    break;
                case Theme.Bootstrap5Dark:
                    style = new Style("#6C757D", "#6C757D", "transparent", "#CED4DA", "#CED4DA", "HelveticaNeue", "#E9ECEF", "#E9ECEF", "#212529", "#ADB5BD", "#ADB5BD", "#E9ECEF", "#CED4DA", "HelveticaNeue", "#E9ECEF", "HelveticaNeue", new string[] { "#6C757D", "#495057", "#343A40" });
                    break;
                case Theme.Tailwind:
                    style = new Style("#D1D5DB", "#D1D5DB", "transparent", "#6B7280", "#6B7280", "Inter", "#111827", "#374151", "#F9FAFB", "#1F2937", "#1F2937", "#374151", "#F9FAFB", "Inter", "#374151", "Inter", new string[] { "#9CA3AF", "#D1D5DB", "#E5E7EB" });
                    break;
                case Theme.TailwindDark:
                    style = new Style("#4B5563", "#4B5563", "transparent", "#9CA3AF", "#9CA3AF", "Inter", "#F9FAFB", "#D1D5DB", "#1F2937", "#1F2937", "#1F2937", "#D1D5DB", "#D1D5DB", "Inter", "#D1D5DB", "Inter", new string[] { "#6B7280", "#4B5563", "#374151" });
                    break;
                default:
                    style = new Style("#424242", "#424242", "#FFFFFF", "rgba(0,0,0,0.54)", "#666666", "SegoeUI", "rgba(0, 8, 22, 0.75)", "#353535", "#ffffff", "#181818", "#181818", "rgba(0,0,0,0.87)", "#ffffff", "SegoeUI", "rgba(0,0,0,0.54)", "SegoeUI", new string[] { "#959595", "#BDBDBD", "#E3E2E2" });
                    break;
            }

            return style;
        }

        internal double StringToNumber(string sizeValue, double containerSize, bool enable = false)
        {
            if (!string.IsNullOrEmpty(sizeValue))
            {
                double size = sizeValue.Contains('%', comparison) ? enable ? containerSize : (containerSize / 100) * int.Parse(sizeValue.Replace("%", string.Empty, comparison), null) : int.Parse(sizeValue.Replace("px", string.Empty, comparison), null);
                return size != 0 ? size : double.NaN;
            }

            return double.NaN;
        }

        internal string GetText(string text, string labelFormat, string format, bool enableGroupSeparator)
        {
            double textFormat = double.Parse(text ?? string.Empty, null);
            string formatText = TextFormatter(labelFormat, Intl.GetNumericFormat(textFormat, format));
            return !enableGroupSeparator ? formatText.Replace(",", string.Empty, comparison) : formatText;
        }

        internal SizeInfo MeasureTextSize(string text, TextStyle style)
        {
            double width = 0, height = 0, fontSize = double.Parse(SizeConverter(style.Size), null);
            for (int i = 0; i < text?.Length; i++)
            {
                SizeInfo charSize = GetCharSize(text[i], style);
                width += charSize.Width;
                height = Math.Max(charSize.Height, height);
            }

            return new SizeInfo() { Width = (width * fontSize) / 100, Height = (height * fontSize) / 100 };
        }

        private static SizeInfo GetCharSize(char character, TextStyle style)
        {
            FontInfo font = new FontInfo();
            string key = character + "_" + style.FontWeight + "_" + style.FontStyle + "_" + style.FontFamily;
            if (!BulletChartRender.SizePerCharacter.TryGetValue(key, out SizeInfo charSize))
            {
                font.Chars.TryGetValue(character, out double charWidth);
                BulletChartRender.SizePerCharacter[key] = new SizeInfo { Width = charWidth * 6.25, Height = 130 };
                BulletChartRender.SizePerCharacter.TryGetValue(key, out charSize);
            }

            return charSize;
        }

        private string SizeConverter(string sizeValue)
        {
            if (!string.IsNullOrEmpty(sizeValue))
            {
                return sizeValue.Contains("px", comparison) ? sizeValue.Replace("px", string.Empty, comparison) : sizeValue.Contains("%", comparison) ? sizeValue.Replace("%", string.Empty, comparison) : sizeValue;
            }

            return string.Empty;
        }

        internal string TrimText(double width, string text, TextStyle style)
        {
            if (MeasureTextSize(text, style).Width > width)
            {
                for (int i = text.Length - 1; i >= 0; --i)
                {
                    string label = text.Substring(0, i) + Constant.DOT;
                    if (MeasureTextSize(label, style).Width <= width || label.Length < 4)
                    {
                        if (label.Length < 4)
                        {
                            label = Constant.SPACE;
                        }

                        return label;
                    }
                }
            }

            return text;
        }

        internal List<LabelModel> GetTitle(string text, TextStyle titleStyle)
        {
            List<LabelModel> listCollection = new List<LabelModel>();
            switch (titleStyle.TextOverflow)
            {
                case TextOverflow.Wrap:
                    listCollection = TextWrap(text, titleStyle);
                    break;
                case TextOverflow.Trim:
                    listCollection.Add(new LabelModel() { Text = TrimText(titleStyle.MaximumTitleWidth, text, titleStyle) });
                    break;
                default:
                    listCollection.Add(new LabelModel() { Text = text });
                    break;
            }

            return listCollection;
        }

        private List<LabelModel> TextWrap(string title, TextStyle titleStyle)
        {
            List<string> textCollection = title.Split(Constant.SPACE).ToList();
            string label = string.Empty, text = string.Empty;
            List<LabelModel> labelCollection = new List<LabelModel>();
            for (int i = 0, len = textCollection.Count; i < len; i++)
            {
                text = textCollection[i];
                if (MeasureTextSize(string.Concat(label, text).ToString(), titleStyle).Width < titleStyle.MaximumTitleWidth)
                {
                    label = string.Concat(label, (string.IsNullOrEmpty(label) ? string.Empty : Constant.SPACE) + text).ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(label))
                    {
                        labelCollection.Add(new LabelModel() { Text = TrimText(titleStyle.MaximumTitleWidth, label, titleStyle) });
                        label = text;
                    }
                    else
                    {
                        labelCollection.Add(new LabelModel() { Text = TrimText(titleStyle.MaximumTitleWidth, text, titleStyle) });
                        text = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(label) && i == len - 1)
                {
                    labelCollection.Add(new LabelModel() { Text = TrimText(titleStyle.MaximumTitleWidth, label, titleStyle) });
                }
            }

            return labelCollection;
        }
    }
}