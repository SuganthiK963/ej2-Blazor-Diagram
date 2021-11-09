using System.Collections.Generic;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Sparkline.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue>
    {
        internal void RenderLabels(RenderTreeBuilder builder, List<SparklineValues> points)
        {
            string color = DataLabelSettings.TextStyle != null && !string.IsNullOrEmpty(DataLabelSettings.TextStyle.Color) ? DataLabelSettings.TextStyle.Color : ThemeStyle.DataLabel;
            if (Type == SparklineType.WinLoss || DataLabelSettings.Visible?.Count < 0)
            {
                return;
            }

            SparklineFont labelStyle = DataLabelSettings.TextStyle != null ? DataLabelSettings.TextStyle : new SparklineFont();
            FontOptions font = new FontOptions()
            {
                Size = labelStyle.Size,
                Color = color,
                FontFamily = string.IsNullOrEmpty(labelStyle.FontFamily) ? ThemeStyle.LabelFontFamily : labelStyle.FontFamily,
                FontWeight = labelStyle.FontWeight,
                FontStyle = labelStyle.FontStyle
            };
            TextOptions option = new TextOptions("0", "0", color, font, string.Empty, "middle", string.Empty, string.Empty, "0", "middle");
            double[] pointsYPos = points.Select(a => a.MarkerPosition).ToArray();
            double highPos = pointsYPos.Min(),
            lowPos = pointsYPos.Max(),
            padding = DataLabelSettings.Fill != "transparent" || DataLabelSettings.Border?.Width <= 0 ? 2 : 0;
            bool isElementOpen = false;
            for (int i = 0, length = points.Count; i < length; i++)
            {
                bool renderLabel = DataLabelSettings.Visible != null && string.Join(',', DataLabelSettings.Visible).Contains("All", comparison);
                option.Id = ID + "_Sparkline_Label_Text_" + i;
                option.Text = !string.IsNullOrEmpty(DataLabelSettings.Format) ? TextFormatter(DataLabelSettings.Format, processedData.ElementAt(i)) : points[i].YVal.ToString(culture);
                RectInfo size = MeasureTextSize(option.Text, labelStyle);
                double x = points[i].Location.X + (DataLabelSettings.Offset != null ? DataLabelSettings.Offset.X : 0),
                y = Type == SparklineType.Pie ? points[i].Location.Y : points[i].MarkerPosition > AxisHeight ?
                    (points[i].Location.Y + (size.Height / 2) + padding) : ((points[i].Location.Y - (size.Height / 2) - padding) + (DataLabelSettings.Offset != null ? DataLabelSettings.Offset.Y : 0));
                DataLabelRenderingEventArgs labelArgs = new DataLabelRenderingEventArgs()
                {
                    Border = new Border() { Color = DataLabelSettings.Border?.Color, Width = DataLabelSettings.Border != null ? DataLabelSettings.Border.Width : 0 },
                    Fill = DataLabelSettings.Fill,
                    PointIndex = i,
                    X = x,
                    Y = y,
                    Text = option.Text,
                    Color = color
                };
                Events?.OnDataLabelRendering?.Invoke(labelArgs);
                size = MeasureTextSize(labelArgs.Text, labelStyle);
                option.Text = labelArgs.Text;
                renderLabel = GetLabelVisible(renderLabel, points[i], i, DataLabelSettings, length, highPos, lowPos);
                EdgeLabelOption labelOption = ArrangeLabelPosition(DataLabelSettings.EdgeLabelMode, renderLabel, labelArgs.X + (DataLabelSettings.Offset != null ? DataLabelSettings.Offset.X : 0), i, length, size, padding);
                if (renderLabel && !labelArgs.Cancel && labelOption.Render)
                {
                    if (i == 0)
                    {
                        Rendering.OpenGroupElement(builder, ID + "_Sparkline_Label_G", string.Empty, string.Empty);
                        isElementOpen = true;
                    }

                    option.X = labelOption.X.ToString(culture);
                    option.Y = labelArgs.Y.ToString(culture);
                    option.Fill = labelArgs.Color;
                    Rendering.OpenGroupElement(builder, ID + "_Sparkline_Label_G" + i, string.Empty, string.Empty);
                    DrawRectangle(builder, new RectOptions(ID + "_Sparkline_Label_Rect_" + i, labelOption.X - ((size.Width / 2) + padding), labelArgs.Y - padding - (size.Height / 1.75), size.Width + (padding * 2), size.Height + (padding * 2), labelArgs.Border.Width, labelArgs.Border.Color, labelArgs.Fill, 0, 0, DataLabelSettings.Opacity));
                    Rendering.RenderText(builder, option);
                    builder.CloseElement();
                    if (i == length - 1 && isElementOpen)
                    {
                        builder.CloseElement();
                    }
                }
                else if (i == length - 1 && isElementOpen)
                {
                    builder.CloseElement();
                }
            }
        }

        private static string TextFormatter(string format, IDictionary<string, object> data)
        {
            if (string.IsNullOrEmpty(format))
            {
                return null;
            }

            foreach (string key in data.Keys)
            {
                string[] splitText = format.Split("${" + key + "}");
                if (splitText.Length > 1)
                {
                    data.TryGetValue(key, out object keyValue);
                    format = string.Join((keyValue ?? string.Empty).ToString(), splitText);
                }
            }

            return format;
        }

        private bool GetLabelVisible(bool render, SparklineValues data, int i, SparklineDataLabelSettings label, int length, double highPos, double lowPos)
        {
            string labelVisible = string.Join(",", label.Visible != null ? label.Visible : new List<VisibleType>());
            if (data.MarkerPosition > AxisHeight)
            {
                render |= labelVisible.Contains("Negative", comparison);
            }

            if (i == 0)
            {
                render |= labelVisible.Contains("Start", comparison);
            }
            else if (i == length - 1)
            {
                render |= labelVisible.Contains("End", comparison);
            }

            if (data.MarkerPosition == highPos)
            {
                render |= labelVisible.Contains("High", comparison);
            }
            else if (data.MarkerPosition == lowPos)
            {
                render |= labelVisible.Contains("Low", comparison);
            }

            if (string.Join(",", labelVisible).Contains("None", comparison))
            {
                render = false;
            }

            return render;
        }

        private EdgeLabelOption ArrangeLabelPosition(EdgeLabelMode edgeLabel, bool render, double x, int index, int length, RectInfo size, double padding)
        {
            if (edgeLabel == EdgeLabelMode.None)
            {
                return new EdgeLabelOption(x, render);
            }

            if (index == 0 && ((x - (size.Width / 2) - padding) <= 0))
            {
                if (edgeLabel == EdgeLabelMode.Hide)
                {
                    render = false;
                }
                else
                {
                    x = Padding.Left + padding + (size.Width / 2);
                }
            }
            else if (index == length - 1 && ((x + (size.Width / 2) + padding) >= AvailableSize.Width))
            {
                if (edgeLabel == EdgeLabelMode.Hide)
                {
                    render = false;
                }
                else
                {
                    x -= (size.Width / 2) + padding;
                }
            }

            return new EdgeLabelOption(x, render);
        }
    }
}