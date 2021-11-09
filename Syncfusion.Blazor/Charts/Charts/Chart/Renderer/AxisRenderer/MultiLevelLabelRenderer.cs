using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class MultiLevelLabelRenderer
    {
        private const string SPACE = " ";
        private double[] y_AxisMultiLabelHeight;
        private double[] y_AxisPrevHeight;
        private double[] x_AxisMultiLabelHeight;
        private double[] x_AxisPrevHeight;
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private ChartAxis axis;
        private ChartAxisRenderer axisRenderer;
        private SfChart chart;
        private string clipPathID;
        private string groupID;
        private Rect clipRect;

        internal List<PathOptions> BorderOptions { get; set; } = new List<PathOptions>();

        internal List<TextOptions> TextOptions { get; set; } = new List<TextOptions>();

        internal void IniMultilevelLabel(ChartAxisRenderer renderer)
        {
            chart = renderer.Owner;
            axis = renderer.Axis;
            axisRenderer = renderer;
            GetMultilevelLabelsHeight();
        }

        private void GetMultilevelLabelsHeight()
        {
            bool isVertical = axisRenderer.Orientation == Orientation.Vertical;
            double axisValue = isVertical ? axisRenderer.Rect.Height : axisRenderer.Rect.Width;
            axisValue = double.IsNaN(axisValue) ? 0 : axisValue;
            List<ChartMultiLevelLabel> multiLevelLabelCollection = axisRenderer.Axis.MultiLevelLabels;
            int labelCount = multiLevelLabelCollection.Count;
            double height, gap, data = 0, padding = 10;
            double[] multiLevelLabelsHeight = new double[labelCount];
            double[] prevHeight = new double[labelCount];
            ChartHelper helper = new ChartHelper();
            for (int index = 0; index < labelCount; index++)
            {
                ChartMultiLevelLabel multiLevel = multiLevelLabelCollection[index];
                foreach (ChartCategory categoryLabel in multiLevel.Categories)
                {
                    if (!string.IsNullOrEmpty(categoryLabel.Text) && categoryLabel.Start != null && categoryLabel.End != null)
                    {
                        Size labelSize = ChartHelper.MeasureText(categoryLabel.Text, multiLevel.TextStyle.GetChartFontOptions());
                        height = isVertical ? labelSize.Width : labelSize.Height + ((2 * multiLevel.Border.Width) + (multiLevel.Border.Type == BorderType.CurlyBrace ? padding : 0));
                        gap = categoryLabel.MaximumTextWidth != 0 ? categoryLabel.MaximumTextWidth : (ChartHelper.ValueToCoefficient(Convert.ToDouble(categoryLabel.End, null), axisRenderer) * axisValue) - (ChartHelper.ValueToCoefficient(Convert.ToDouble(categoryLabel.Start, null), axisRenderer) * axisValue);
                        if (labelSize.Width > (gap - padding) && gap > 0 && multiLevel.Overflow == TextOverflow.Wrap && !isVertical)
                        {
                            height = height * ChartHelper.TextWrap(categoryLabel.Text, gap - padding, /*multiLevel.TextStyle*/new ChartFontOptions { Size = multiLevel.TextStyle.Size }).Count;
                        }

                        multiLevelLabelsHeight[index] = Math.Max(height, multiLevelLabelsHeight[index]);
                    }
                }

                prevHeight[index] = data;
                data += !double.IsNaN(multiLevelLabelsHeight[index]) ? (multiLevelLabelsHeight[index] + padding) : 0;
            }

            axisRenderer.MultiLevelLabelHeight = data + (!string.IsNullOrEmpty(axisRenderer.Axis.Title) || chart.LegendRenderer.LegendSettings.Visible ? padding / 2 : 0);
            if (isVertical)
            {
                y_AxisMultiLabelHeight = multiLevelLabelsHeight;
                y_AxisPrevHeight = prevHeight;
            }
            else
            {
                x_AxisMultiLabelHeight = multiLevelLabelsHeight;
                x_AxisPrevHeight = prevHeight;
            }
        }

        internal void ClearPathOptions()
        {
            BorderOptions.Clear();
            TextOptions.Clear();
        }

        private void CalculateBorderElement(int borderIndex, int axisIndex, string path, int pointIndex)
        {
            PathOptions pathOption = new PathOptions()
            {
                Id = chart.ID + axisIndex + "_Axis_MultiLevelLabel_Rect_" + borderIndex + '_' + pointIndex,
                Fill = Constants.TRANSPARENT,
                StrokeWidth = axis.MultiLevelLabels[borderIndex].Border.Width,
                Stroke = !string.IsNullOrEmpty(axis.MultiLevelLabels[borderIndex].Border.Color) ? axis.MultiLevelLabels[borderIndex].Border.Color : chart.ChartThemeStyle.AxisLine,
                Opacity = 1,
                StrokeDashArray = string.Empty,
                Direction = path
            };
            pathOption.Direction = ChartHelper.AppendPathElements(chart, pathOption.Direction, pathOption.Id);
            BorderOptions.Add(pathOption);
        }

        private string CalculateXAxisLabelBorder(int labelIndex, double gap, double startX, double startY, Size labelSize, TextOptions textOptions, Rect axisRect, Alignment alignment, string path, bool isOutside, bool opposedPosition, int categoryIndex)
        {
            double padding = 10, padding1, padding2, data, value1;
            ChartMultiLevelLabel groupLabel = axis.MultiLevelLabels[labelIndex];
            BorderType categoryType = (groupLabel.Categories[categoryIndex].Type == BorderType.Auto) ? groupLabel.Border.Type : groupLabel.Categories[categoryIndex].Type;
            double width = gap + padding;
            double height = x_AxisMultiLabelHeight[labelIndex] + padding;
            double scrollBarHeight = axis.LabelPosition == AxisPosition.Outside ? axis.ScrollBarHeight : 0;
            double x = startX + axisRect.X;
            double y = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? (startY + axisRect.Y + x_AxisPrevHeight[labelIndex] + scrollBarHeight) : (axisRect.Y - startY - x_AxisPrevHeight[labelIndex] - scrollBarHeight);
            switch (categoryType)
            {
                case BorderType.WithoutTopandBottomBorder:
                case BorderType.Rectangle:
                case BorderType.WithoutTopBorder:
                    height = ((!opposedPosition && isOutside) || (opposedPosition && !isOutside)) ? height : -height;
                    path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + x.ToString(culture) + SPACE + (y + height).ToString(culture) + " M " + (x + width).ToString(culture) + SPACE + y.ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + (y + height).ToString(culture) + (categoryType != BorderType.WithoutTopandBottomBorder ? (" L" + SPACE + x.ToString(culture) + SPACE + (y + height).ToString(culture) + SPACE) : SPACE) + (categoryType == BorderType.Rectangle ? ("M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + y.ToString(culture)) : SPACE);
                    break;
                case BorderType.Brace:
                    if (alignment == Alignment.Near)
                    {
                        data = Convert.ToDouble(textOptions.X, null);
                        value1 = Convert.ToDouble(textOptions.X, null) + labelSize.Width + 2;
                    }
                    else if (alignment == Alignment.Center)
                    {
                        data = Convert.ToDouble(textOptions.X, null) - (labelSize.Width / 2) - 2;
                        value1 = Convert.ToDouble(textOptions.X, null) + (labelSize.Width / 2) + 2;
                    }
                    else
                    {
                        data = Convert.ToDouble(textOptions.X, null) - labelSize.Width - 2;
                        value1 = Convert.ToDouble(textOptions.X, null);
                    }

                    height = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? height : -height;
                    path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + x.ToString(culture) + SPACE + (y + (height / 2)).ToString(culture) + " M " + x.ToString(culture) + SPACE + (y + (height / 2)).ToString(culture) + " L " + (data - 2).ToString(culture) + SPACE + (y + (height / 2)).ToString(culture) +
                        " M " + value1.ToString(culture) + SPACE + (y + (height / 2)).ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + (y + (height / 2)).ToString(culture) + " M " + (x + width).ToString(culture) + SPACE + (y + (height / 2)).ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + y.ToString(culture);
                    break;
                case BorderType.CurlyBrace:
                    if ((!opposedPosition && isOutside) || (opposedPosition && !isOutside))
                    {
                        padding = 10;
                        padding1 = 15;
                        padding2 = 5;
                    }
                    else
                    {
                        padding = -10;
                        padding1 = -15;
                        padding2 = -5;
                    }

                    if (alignment == Alignment.Center)
                    {
                        path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " C " + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + 5).ToString(culture) + SPACE + (y + padding).ToString(culture) + SPACE + (x + 10).ToString(culture) + SPACE +
                            (y + padding).ToString(culture) + " L " + (x + (width / 2) - 5).ToString(culture) + SPACE + (y + padding).ToString(culture) + " L " + (x + (width / 2)).ToString(culture) + SPACE + (y + padding1).ToString(culture) +
                            " L " + (x + (width / 2) + 5).ToString(culture) + SPACE + (y + padding).ToString(culture) + " L " + (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + " C " +
                            (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + SPACE + (x + width).ToString(culture) + SPACE + (y + padding2).ToString(culture) + SPACE + (x + width).ToString(culture) + SPACE + y.ToString(culture);
                    }
                    else if (alignment == Alignment.Near)
                    {
                        path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " C " + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + 5).ToString(culture) + SPACE + (y + padding).ToString(culture) + SPACE + (x + 10).ToString(culture) + SPACE +
                            (y + padding).ToString(culture) + " L " + (x + 15).ToString(culture) + SPACE + (y + padding1).ToString(culture) + " L " + (x + 20).ToString(culture) + SPACE + (y + padding).ToString(culture) + " L " +
                            (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + " C " + (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + SPACE + (x + width).ToString(culture) + SPACE + (y + padding2).ToString(culture) + SPACE + (x + width).ToString(culture) + SPACE + y.ToString(culture);
                    }
                    else
                    {
                        path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " C " + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + 5).ToString(culture) + SPACE + (y + padding).ToString(culture) + SPACE + (x + 10).ToString(culture) + SPACE +
                            (y + padding).ToString(culture) + " L " + (x + width - 20).ToString(culture) + SPACE + (y + padding).ToString(culture) + " L " + (x + width - 15).ToString(culture) + SPACE + (y + padding1).ToString(culture) +
                            " L " + (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + " L " + (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + " C "
                            + (x + width - 10).ToString(culture) + SPACE + (y + padding).ToString(culture) + SPACE + (x + width).ToString(culture) + SPACE + (y + padding2).ToString(culture) + SPACE + (x + width).ToString(culture) + SPACE + y.ToString(culture);
                    }

                    break;
            }

            return path;
        }

        internal void CalculateYAxisMultiLevelLabels(int index, Rect rect)
        {
            ClearPathOptions();
            bool isOutside = axis.LabelPosition == AxisPosition.Outside;
            double startX = (axis.TickPosition == axis.LabelPosition ? axis.MajorTickLines.Height : 0) + axisRenderer.MaxLabelSize.Width + 10;
            double scrollBarHeight = (isOutside && axis.CrossesAt == null ? axis.ScrollBarHeight : 0) * (axis.OpposedPosition ? 1 : -1);
            double clipX = (axis.OpposedPosition && !isOutside) || (!axis.OpposedPosition && isOutside) ? (rect.X - axisRenderer.MultiLevelLabelHeight - startX - 10) : (rect.X + startX);
            clipPathID = chart.ID + "_YAxis_Clippath_" + index;
            groupID = chart.ID + "YAxisMultiLevelLabel" + index;
            clipRect = new Rect { X = clipX + scrollBarHeight, Y = rect.Y - axis.MajorTickLines.Width, Height = rect.Height + (2 * axis.MajorTickLines.Width), Width = axisRenderer.MultiLevelLabelHeight + 10 };
            ChartHelper helper = new ChartHelper();
            for (int level = 0; level < axis.MultiLevelLabels.Count; level++)
            {
                ChartMultiLevelLabel multiLevel = axis.MultiLevelLabels[level];
                int pointIndex = 0;
                for (int i = 0; i < multiLevel.Categories.Count; i++)
                {
                    ChartCategory categoryLabel = multiLevel.Categories[i];
                    string pathRect = string.Empty;
                    object end = categoryLabel.End.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.End, null), null) : categoryLabel.End;
                    object start = categoryLabel.Start.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.Start, null), null) : categoryLabel.Start;
                    double startY = ChartHelper.ValueToCoefficient(Convert.ToDouble(start, null), axisRenderer) * rect.Height;
                    double endY = axis.IsInversed ? startY : ChartHelper.ValueToCoefficient(Convert.ToDouble(end, null), axisRenderer) * rect.Height;

                    AxisMultiLabelRenderEventArgs argsData = new AxisMultiLabelRenderEventArgs(Constants.AXISMULTILABELRENDER, false, axis, categoryLabel.CustomAttributes, categoryLabel.Text, multiLevel.TextStyle, multiLevel.Alignment);
                    if (chart.ChartEvents?.OnAxisMultiLevelLabelRender != null)
                    {
                        chart.ChartEvents.OnAxisMultiLevelLabelRender.Invoke(argsData);
                    }

                    if (!argsData.Cancel)
                    {
                        Size labelSize = ChartHelper.MeasureText(argsData.Text, argsData.TextStyle.GetChartFontOptions());
                        double gap = endY - startY;
                        double x = rect.X - startX - y_AxisPrevHeight[level] - (y_AxisMultiLabelHeight[level] / 2) - (10 / 2);
                        double y = rect.Height + rect.Y - startY - (gap / 2);
                        if (axis.OpposedPosition)
                        {
                            x = isOutside ? rect.X + startX + (10 / 2) + (y_AxisMultiLabelHeight[level] / 2) + y_AxisPrevHeight[level] + scrollBarHeight : rect.X - startX - (y_AxisMultiLabelHeight[level] / 2) - y_AxisPrevHeight[level] - (10 / 2);
                        }
                        else
                        {
                            x = isOutside ? x + scrollBarHeight : rect.X + startX + (10 / 2) + (y_AxisMultiLabelHeight[level] / 2) + y_AxisPrevHeight[level];
                        }

                        if (argsData.Alignment == Alignment.Center)
                        {
                            y += labelSize.Height / 4;
                        }
                        else if (argsData.Alignment == Alignment.Far)
                        {
                            y += (gap / 2) - (labelSize.Height / 2);
                        }
                        else
                        {
                            y = y - (gap / 2) + labelSize.Height;
                        }

                        x = multiLevel.Border.Type == BorderType.CurlyBrace ? ((!axis.OpposedPosition && isOutside) || (axis.OpposedPosition && !isOutside) ? x - 10 : x + 10) : x;
#pragma warning disable CA1305
                        TextOptions options = new TextOptions(x.ToString(culture), y.ToString(culture), argsData.TextStyle.Color ?? chart.ChartThemeStyle.AxisLabel, argsData.TextStyle.GetFontOptions(), argsData.Text, "middle", chart.ID + index + "_Axis_MultiLevelLabel_Level_" + level + "_Text_" + i);
                        options.Text = (multiLevel.Overflow == TextOverflow.Trim) ? ChartHelper.TextTrim(ChartHelper.IsNaNOrZero(categoryLabel.MaximumTextWidth) ? y_AxisMultiLabelHeight[level] : categoryLabel.MaximumTextWidth, argsData.Text, /*argsData.TextStyle*/new ChartFontOptions { Size = argsData.TextStyle.Size }) : options.Text;
                        string[] locations = ChartHelper.AppendTextElements(chart, options.Id, Convert.ToDouble(options.X, null), Convert.ToDouble(options.Y, null));
                        options.X = locations[0];
                        options.Y = locations[1];
                        TextOptions.Add(options);
                        if (multiLevel.Border.Width > 0 && multiLevel.Border.Type != BorderType.WithoutBorder)
                        {
                            pathRect = CalculateYAxisLabelBorder(level, endY, startX, startY, labelSize, options, rect, argsData.Alignment, pathRect, isOutside, axis.OpposedPosition, pointIndex);
                            if (!string.IsNullOrEmpty(pathRect))
                            {
                                CalculateBorderElement(level, index, pathRect, pointIndex);
                                pointIndex++;
                            }
                        }
                    }
                }
            }
        }

        internal void CalculateXAxisMultiLevelLabels(int index, Rect axisRect)
        {
            ClearPathOptions();
            double padding = 10, x, y, startX, endX, gap;
            double startY = (axis.LabelPosition == axis.TickPosition ? axis.MajorTickLines.Height : 0) + axisRenderer.MaxLabelSize.Height + padding;
            string anchor;
            Size labelSize;
            bool isOutside = axis.LabelPosition == AxisPosition.Outside;
            AxisMultiLabelRenderEventArgs argsData;
            bool opposedPosition = axis.OpposedPosition;
            double scrollBarHeight = axis.ScrollbarSettings.Enable || (isOutside && (axis.CrossesAt == null)) ? axis.ScrollBarHeight : 0;
            double clipY = (opposedPosition && !isOutside) || (!opposedPosition && isOutside) ? (axisRect.Y + startY - axis.MajorTickLines.Width) : (axisRect.Y - startY - axisRenderer.MultiLevelLabelHeight);
            clipPathID = chart.ID + "_XAxis_Clippath_" + index;
            groupID = chart.ID + "XAxisMultiLevelLabel" + index;
            clipRect = new Rect() { X = axisRect.X - axis.MajorTickLines.Width, Y = clipY + scrollBarHeight, Height = axisRenderer.MultiLevelLabelHeight + padding, Width = axisRect.Width + (2 * axis.MajorTickLines.Width) };
            int labelCount = axis.MultiLevelLabels.Count;
            ChartHelper helper = new ChartHelper();
            for (int level = 0; level < labelCount; level++)
            {
                ChartMultiLevelLabel multiLevel = axis.MultiLevelLabels[level];
                int pointIndex = 0;
                for (int i = 0; i < multiLevel.Categories.Count; i++)
                {
                    ChartCategory categoryLabel = multiLevel.Categories[i];
                    string pathRect = string.Empty;
                    object start = categoryLabel.Start.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.Start, null)) : categoryLabel.Start;
                    object end = categoryLabel.End.GetType() == typeof(string) ? Convert.ToDouble(DateTime.Parse((string)categoryLabel.End, null)) : categoryLabel.End;
                    argsData = new AxisMultiLabelRenderEventArgs(Constants.AXISMULTILABELRENDER, false, axis, categoryLabel.CustomAttributes, categoryLabel.Text, multiLevel.TextStyle, multiLevel.Alignment);
                    if (chart.ChartEvents?.OnAxisMultiLevelLabelRender != null)
                    {
                        chart.ChartEvents.OnAxisMultiLevelLabelRender.Invoke(argsData);
                    }

                    if (!argsData.Cancel)
                    {
                        startX = ChartHelper.ValueToCoefficient(Convert.ToDouble(start, null), axisRenderer) * axisRect.Width;
                        endX = ChartHelper.ValueToCoefficient(Convert.ToDouble(end, null), axisRenderer) * axisRect.Width;
                        endX = axis.IsInversed ? new double[] { startX, startX = endX } [0] : endX;
                        labelSize = ChartHelper.MeasureText(argsData.Text, argsData.TextStyle.GetChartFontOptions());
                        gap = (ChartHelper.IsNaNOrZero(categoryLabel.MaximumTextWidth) ? endX - startX : categoryLabel.MaximumTextWidth) - padding;
                        x = startX + axisRect.X + padding;
                        y = (((opposedPosition && !isOutside) || (!opposedPosition && isOutside)) ? (startY + axisRect.Y + (labelSize.Height / 2) + padding + x_AxisPrevHeight[level]) : (axisRect.Y - startY + (labelSize.Height / 2) -
                                x_AxisMultiLabelHeight[level] - x_AxisPrevHeight[level])) + scrollBarHeight;
                        if (argsData.Alignment == Alignment.Center)
                        {
                            x += (endX - startX - padding) / 2;
                            anchor = "middle";
                        }
                        else if (argsData.Alignment == Alignment.Far)
                        {
                            x = x + (endX - startX - padding) - (multiLevel.Border.Width / 2);
                            anchor = "end";
                        }
                        else
                        {
                            anchor = "start";
                            x += multiLevel.Border.Width / 2;
                        }

                        y = multiLevel.Border.Type == BorderType.CurlyBrace ? (((!opposedPosition && isOutside) || (opposedPosition && !isOutside)) ? y + padding : y - (padding / 2)) : y;
                        TextOptions options = new TextOptions(
                            x.ToString(culture),
                            y.ToString(culture),
                            argsData.TextStyle.Color ?? chart.ChartThemeStyle.AxisLabel,
                            argsData.TextStyle.GetChartFontOptions(),
                            argsData.Text,
                            anchor,
                            chart.ID + index + "_Axis_MultiLevelLabel_Level_" + level + "_Text_" + i);
                        if (multiLevel.Overflow != TextOverflow.None)
                        {
                            options.TextCollection = multiLevel.Overflow == TextOverflow.Wrap ? ChartHelper.TextWrap(argsData.Text, gap, /*argsData.TextStyle*/new ChartFontOptions { Size = argsData.TextStyle.Size }) : new List<string>() { ChartHelper.TextTrim(gap, argsData.Text, /*argsData.TextStyle*/new ChartFontOptions { Size = argsData.TextStyle.Size }) };
                            options.X = (Convert.ToDouble(options.X, null) - (padding / 2)).ToString(culture);
                        }

                        string[] locations = ChartHelper.AppendTextElements(chart, options.Id, Convert.ToDouble(options.X, null), Convert.ToDouble(options.Y, null));
                        options.X = locations[0];
                        options.Y = locations[1];
                        TextOptions.Add(options);
                        if (multiLevel.Border.Width > 0 && multiLevel.Border.Type != BorderType.WithoutBorder)
                        {
                            pathRect = CalculateXAxisLabelBorder(level, endX - startX - padding, startX, startY, labelSize, options, axisRect, argsData.Alignment, pathRect, isOutside, opposedPosition, pointIndex);
                            if (!string.IsNullOrEmpty(pathRect))
                            {
                                CalculateBorderElement(level, index, pathRect, pointIndex);
                                pointIndex++;
                            }
                        }
                    }
                }
            }
        }

        private string CalculateYAxisLabelBorder(int labelIndex, double endY, double startX, double startY, Size labelSize, TextOptions textOptions, Rect rect, Alignment alignment, string path, bool isOutside, bool opposedPosition, int categoryIndex)
        {
            double height = endY - startY;
            double padding = 10, padding1, padding2;
            ChartMultiLevelLabel groupLabel = axis.MultiLevelLabels[labelIndex];
            BorderType categoryType = (groupLabel.Categories[categoryIndex].Type == BorderType.Auto) ? groupLabel.Border.Type : groupLabel.Categories[categoryIndex].Type;
            double y = rect.Y + rect.Height - endY;
            double scrollBarHeight = (isOutside && axis.CrossesAt == null ? axis.ScrollBarHeight : 0) * (opposedPosition ? 1 : -1);
            double width = y_AxisMultiLabelHeight[labelIndex] + padding;
            double x = ((!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? rect.X - startX - y_AxisPrevHeight[labelIndex] : rect.X + startX + y_AxisPrevHeight[labelIndex]) + scrollBarHeight;
            switch (categoryType)
            {
                case BorderType.WithoutTopandBottomBorder:
                case BorderType.Rectangle:
                case BorderType.WithoutTopBorder:
                    width = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? -width : width;
                    path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + y.ToString(culture) + " M " + x.ToString(culture) + SPACE + (y + height).ToString(culture) + " L " + (x + width).ToString(culture) + SPACE + (y + height).ToString(culture) + (categoryType != BorderType.WithoutTopandBottomBorder ? " L" + SPACE + (x + width).ToString(culture) + SPACE + y.ToString(culture) + SPACE : SPACE) + (categoryType == BorderType.Rectangle ? "M " + x.ToString(culture) + SPACE + (y + height).ToString(culture) + "L" + SPACE + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE : SPACE);
                    break;
                case BorderType.Brace:
                    width = (!opposedPosition && isOutside) || (opposedPosition && !isOutside) ? width : -width;
                    path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " L " + (x - (width / 2)).ToString(culture) + SPACE + y.ToString(culture) + " L " + (x - (width / 2)).ToString(culture) + SPACE + (Convert.ToDouble(textOptions.Y, null) - (labelSize.Height / 2) - 4).ToString(culture) + " M " + (x - (width / 2)).ToString(culture) + SPACE + (Convert.ToDouble(textOptions.Y, null) + (labelSize.Height / 4) + 2).ToString(culture) +
                        " L " + (x - (width / 2)).ToString(culture) + SPACE + (y + height).ToString(culture) + " L " + x.ToString(culture) + SPACE + (y + height).ToString(culture);
                    break;
                case BorderType.CurlyBrace:
                    if ((!opposedPosition && isOutside) || (opposedPosition && !isOutside))
                    {
                        padding = -10;
                        padding1 = -15;
                        padding2 = -5;
                    }
                    else
                    {
                        padding = 10;
                        padding1 = 15;
                        padding2 = 5;
                    }

                    if (alignment == Alignment.Center)
                    {
                        path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " C " + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + (y + 10).ToString(culture)
                            + " L " + (x + padding).ToString(culture) + SPACE + (y + ((height - 10) / 2)).ToString(culture) + " L " + (x + padding1).ToString(culture) + SPACE + (y + ((height - 10) / 2) + 5).ToString(culture)
                            + " L " + (x + padding).ToString(culture) + SPACE + (y + ((height - 10) / 2) + 10).ToString(culture) + " L " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) +
                            " C " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) + SPACE + (x + padding2).ToString(culture) + SPACE + (y + height).ToString(culture) + SPACE + x.ToString(culture) + SPACE + (y + height).ToString(culture);
                    }
                    else if (alignment == Alignment.Far)
                    {
                        path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " C " + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + (y + 10).ToString(culture)
                            + " L " + (x + padding).ToString(culture) + SPACE + (y + height - 20).ToString(culture) + SPACE + " L " + (x + padding1).ToString(culture) + SPACE + (y + (height - 15)).ToString(culture) +
                            " L " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) + " L " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) +
                            " C " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + (y + height).ToString(culture) + SPACE + x.ToString(culture) + SPACE + (y + height).ToString(culture);
                    }
                    else
                    {
                        path += "M " + x.ToString(culture) + SPACE + y.ToString(culture) + " C " + x.ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + y.ToString(culture) + SPACE + (x + padding).ToString(culture) + SPACE + (y + 10).ToString(culture)
                            + " L " + (x + padding1).ToString(culture) + SPACE + (y + 15).ToString(culture) + " L " + (x + padding).ToString(culture) + SPACE + (y + 20).ToString(culture) + " L " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) +
                            " C " + (x + padding).ToString(culture) + SPACE + (y + (height - 10)).ToString(culture) + SPACE + (x + padding2).ToString(culture) + SPACE + (y + height).ToString(culture) + SPACE + x.ToString(culture) + SPACE + (y + height).ToString(culture);
                    }

                    break;
            }

            return path;
        }

        internal void RenderMultilevelLabel(RenderTreeBuilder builder)
        {
            chart.SvgRenderer.OpenGroupElement(builder, groupID, string.Empty, "url(#" + clipPathID + ")", "cursor: pointer;");
            chart.SvgRenderer.RenderClipPath(builder, clipPathID, clipRect);
            for (int i = 0; i < TextOptions.Count; i++)
            {
                chart.SvgRenderer.OpenGroupElement(builder, chart.ID + axisRenderer.Index + "_MultiLevelLabel" + i);
                ChartHelper.TextElement(builder, chart.SvgRenderer, TextOptions[i]);
                chart.SvgRenderer.RenderPath(builder, BorderOptions[i], "pointer-events: none");
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        internal void Dispose()
        {
            y_AxisMultiLabelHeight = null;
            x_AxisMultiLabelHeight = null;
            x_AxisPrevHeight = null;
            y_AxisPrevHeight = null;
            chart = null;
        }
    }
}