using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    internal partial class SmithChartLegend
    {
        private SmithChartThemeStyle themeStyle;
        private SfSmithChart smithchart;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal SmithChartLegend(SfSmithChart chart, SmithChartThemeStyle smithchartThemeStyle)
        {
            smithchart = chart;
            themeStyle = smithchartThemeStyle;
        }

        internal Rect LegendActualBounds { get; set; }

        internal List<LegendSeries> LegendSeries { get; set; } = new List<LegendSeries>();

        internal Rect CalculateLegendBounds()
        {
            LegendSeries = new List<LegendSeries>();
            SmithChartLegendSettings legend = smithchart.LegendSettings;
            LegendPosition position = legend.Position;
            SmithChartLegendTitleTextStyle font = legend.Title.TextStyle;
            double padding = 10, width = 0, height = 0,
            itemPadding = legend.ItemPadding > 0 ? legend.ItemPadding : 0,
            legendItemWidth = 0, legendItemHeight = 0, legendHeight = 0, length = smithchart.Series.Count,
            svgObjectWidth = smithchart.AvailableSize.Width - ((smithchart.ElementSpacing * 2) - (legend.Border.Width * 2) + (smithchart.Border.Width * 2));
            int rowCount = legend.RowCount, columnCount = legend.ColumnCount;
            Size titleSize = smithchart.LegendSettings.Title.Visible ? SmithChartHelper.MeasureText(smithchart.LegendSettings.Title.Text, font) : new Size(0, 0);
            double maxRowWidth = 0, totalRowHeight = 0, curRowWidth = 0, curRowHeight = 0, allowItems = double.NaN, itemsCountRow = 0;
            Rect legendBounds = new Rect(0, 0, 0, 0);
            string defaultValue = "20%";
            bool isVertical = position == LegendPosition.Left || position == LegendPosition.Right;
            if (isVertical)
            {
                legendBounds.Height = DataVizCommonHelper.StringToNumber(legend.Height, smithchart.AvailableSize.Height - (smithchart.Bounds.Y - smithchart.Margin.Top));
                legendBounds.Height = !double.IsNaN(legendBounds.Height) ? legendBounds.Height : smithchart.Bounds.Height;
                legendBounds.Width = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(legend.Width) ? legend.Width : defaultValue, smithchart.AvailableSize.Width);
            }
            else
            {
                legendBounds.Width = DataVizCommonHelper.StringToNumber(legend.Width, smithchart.AvailableSize.Width);
                legendBounds.Width = !double.IsNaN(legendBounds.Width) ? legendBounds.Width : smithchart.Bounds.Width;
                legendBounds.Height = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(legend.Height) ? legend.Height : defaultValue, smithchart.AvailableSize.Height);
            }

            if (smithchart.LegendSettings.Visible && length != 0)
            {
                if (position == LegendPosition.Bottom || position == LegendPosition.Top || position == LegendPosition.Custom)
                {
                    if (rowCount != 0 && columnCount != 0 && (rowCount <= columnCount))
                    {
                        rowCount = (int)(length / columnCount);
                    }
                    else if (rowCount == 0 && columnCount != 0)
                    {
                        rowCount = (int)(length / columnCount);
                    }
                    else if (rowCount == 0 && columnCount == 0)
                    {
                        rowCount = 1;
                    }

                    if (rowCount != 0)
                    {
                        allowItems = Math.Ceiling(length / rowCount);
                    }
                }
                else
                {
                    if (rowCount != 0 && columnCount != 0 && (rowCount <= columnCount))
                    {
                        columnCount = (int)(length / rowCount);
                    }
                    else if (rowCount != 0 && columnCount == 0)
                    {
                        columnCount = (int)(length / rowCount);
                    }
                    else if (rowCount == 0 && columnCount == 0)
                    {
                        columnCount = 1;
                    }

                    if (columnCount != 0)
                    {
                        allowItems = columnCount;
                    }
                }

                for (int i = 0; i < length; i++)
                {
                    LegendSeries.Add(new LegendSeries
                    {
                        Text = !string.IsNullOrEmpty(smithchart.Series[i].Name) ? smithchart.Series[i].Name : "series" + i,
                        SeriesIndex = i,
                        Shape = smithchart.LegendSettings.Shape,
                        Fill = smithchart.Series[i].Interior,
                        Bounds = new Size(0, 0)
                    });
                    Size legendsize = GetLegendSize(LegendSeries[i], legendBounds.Width);
                    legendItemWidth = Math.Max(legendsize.Width, legendItemWidth);
                    legendItemHeight = Math.Max(legendsize.Height, legendItemHeight);
                    LegendSeries[i].Bounds = new Size(legendItemWidth, legendItemHeight);
                    itemsCountRow = itemsCountRow + 1;
                    curRowWidth = curRowWidth + legendItemWidth + itemPadding;
                    curRowHeight = Math.Max(legendItemHeight, curRowHeight);
                    if (curRowWidth > svgObjectWidth && (position == LegendPosition.Top || position == LegendPosition.Bottom || position == LegendPosition.Custom))
                    {
                        curRowWidth -= legendsize.Width + itemPadding;
                        maxRowWidth = Math.Max(maxRowWidth, curRowWidth);
                        curRowWidth = legendsize.Width + itemPadding;
                        totalRowHeight = totalRowHeight + curRowHeight + itemPadding;
                    }

                    if (itemsCountRow == allowItems || i == length - 1)
                    {
                        maxRowWidth = Math.Max(maxRowWidth, curRowWidth);
                        totalRowHeight = totalRowHeight + curRowHeight + itemPadding;
                        legendHeight = totalRowHeight;
                        itemsCountRow = 0;
                        curRowHeight = 0;
                        curRowWidth = 0;
                    }
                }

                width = titleSize.Width > maxRowWidth - itemPadding ? (titleSize.Width + (padding * 2) + itemPadding) :
                    maxRowWidth + (padding * 2) - (smithchart.Border.Width * 2);
                width = width > legendBounds.Width ? legendBounds.Width : width;
                height = legendHeight + smithchart.ElementSpacing + titleSize.Height;
                legendBounds = new Rect(0, 0, width, height);
            }

            LegendActualBounds = legendBounds;
            return LegendActualBounds;
        }

        private Size GetLegendSize(LegendSeries series, double legendMaxWidth)
        {
            SmithChartLegendSettings legend = smithchart.LegendSettings;
            Size textSize = SmithChartHelper.MeasureText(series.Text, legend.TextStyle);
            return new Size(legend.ItemStyle.Width + (textSize.Width > legendMaxWidth ? legendMaxWidth : textSize.Width) + legend.ShapePadding, Math.Max(legend.ItemStyle.Height, textSize.Height));
        }

        private void DrawLegendTitle(SmithChartLegendSettings legend, Rect legendBounds, RenderTreeBuilder builder)
        {
            Size titleSize = SmithChartHelper.MeasureText(legend.Title.Text, legend.Title.TextStyle);
            double titleWidth = titleSize.Width, titleHeight = titleSize.Height, startX = 0, legendBoundsWidth = legendBounds.Width,
            startY = titleHeight;
            SmithChartAlignment textAlignment = legend.Title.TextAlignment;
            switch (textAlignment)
            {
                case SmithChartAlignment.Far:
                    startX = legendBoundsWidth - titleWidth - startX;
                    break;
                case SmithChartAlignment.Center:
                    startX = (legendBoundsWidth / 2) - (titleWidth / 2);
                    break;
            }

            if (startX < 0)
            {
                startX = 0;
            }

            TextOptions titleOptions = new TextOptions()
            {
                X = Convert.ToString(startX, culture),
                Y = Convert.ToString(startY, culture),
                Text = legend.Title.Text,
                TextAnchor = "start",
                Id = smithchart.ID + "_LegendTitleText",
                FontFamily = legend.Title.TextStyle.FontFamily,
                FontSize = legend.Title.TextStyle.Size,
                FontStyle = legend.Title.TextStyle.FontStyle,
                FontWeight = legend.Title.TextStyle.FontWeight,
                Style = "opacity :" + legend.Title.TextStyle.Opacity,
                Fill = legend.Title.TextStyle.Color ?? themeStyle.LegendLabel,
                AccessibilityText = string.IsNullOrEmpty(legend.Title.Description) ? legend.Title.Text : legend.Title.Description
            };
            titleOptions.Text = SmithChartHelper.TextTrim(Math.Round(LegendActualBounds.Width, 4), titleOptions.Text, smithchart.LegendSettings.Title.TextStyle);
            smithchart.Rendering.RenderText(builder, titleOptions);
        }

        private void DrawLegendBorder(SmithChartLegendSettings legend, Rect legendBounds, RenderTreeBuilder builder)
        {
            smithchart.Rendering.RenderRect(builder, smithchart.ID + "_svg_legendRect", 0, 0, legendBounds.Width, legendBounds.Height, legend.Border.Width, string.IsNullOrEmpty(legend.Border.Color) ? "transparent" : legend.Border.Color, "transparent");
        }

        private void DrawLegendItem(SmithChartLegendSettings legend, LegendSeries legendSeries, double k, double x, double y, RenderTreeBuilder builder)
        {
            double textHeight = SmithChartHelper.MeasureText(legendSeries.Text, legend.TextStyle).Height;
            SmithChartLegendItemStyle symbol = legend.ItemStyle;
            Point location = new Point(x + (symbol.Width / 2), y + ((textHeight > symbol.Height ? textHeight : symbol.Height) / 2));
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + "_svg_Legend" + k, string.Empty, string.Empty, "cursor: " + (legend.ToggleVisibility ? "pointer" : "default"), Convert.ToString(k + 1, culture));
            SmithChartLegendRenderEventArgs legendEventArgs = new SmithChartLegendRenderEventArgs
            {
                Text = legendSeries.Text,
                Fill = legendSeries.Fill,
                Shape = legendSeries.Shape,
                EventName = "LegendRender"
            };
            SfSmithChart.InvokeEvent<SmithChartLegendRenderEventArgs>(smithchart.SmithChartEvents?.LegendRendering, legendEventArgs);
            if (!legendEventArgs.Cancel)
            {
                DrawLegendShape(location.X, location.Y, k, legend, legendEventArgs, builder);
                TextOptions options = new TextOptions()
                {
                    X = Convert.ToString(location.X + (symbol.Width / 2) + legend.ShapePadding, culture),
                    Y = Convert.ToString(location.Y + (textHeight / 4), culture),
                    Text = legendEventArgs.Text,
                    TextAnchor = "start",
                    Id = smithchart.ID + "_LegendItemText" + k,
                    FontFamily = themeStyle.FontFamily ?? legend.TextStyle.FontFamily,
                    FontSize = themeStyle.FontSize ?? legend.TextStyle.Size,
                    FontStyle = legend.TextStyle.FontStyle,
                    Style = "opacity :" + legend.TextStyle.Opacity,
                    FontWeight = legend.TextStyle.FontWeight,
                    Fill = legend.TextStyle.Color ?? themeStyle.LegendLabel,
                    AccessibilityText = string.IsNullOrEmpty(legend.Description) ? "Click to show or hide the " + legendEventArgs.Text + " series" : legend.Description
                };
                double maxWidth = (legendSeries.Bounds.Width + legend.ItemPadding) > LegendActualBounds.Width ? (legendSeries.Bounds.Width - legend.ItemPadding - legend.ItemStyle.Width) : legendSeries.Bounds.Width;
                options.Text = SmithChartHelper.TextTrim(Math.Round(maxWidth, 4), options.Text, smithchart.LegendSettings.TextStyle);
                smithchart.Rendering.RenderText(builder, options);
            }

            builder.CloseElement();
        }

        private void DrawLegendShape(double locX, double locY, double index, SmithChartLegendSettings legend, SmithChartLegendRenderEventArgs legendEventArgs, RenderTreeBuilder builder)
        {
            SmithChartLegendItemStyle symbol = legend.ItemStyle;
            string fill = smithchart.Series[(int)index].Visible ? legendEventArgs.Fill : "grey";
            Shape shape = legendEventArgs.Shape;
            PathOptions symbolOption = new PathOptions(smithchart.ID + "_LegendItemShape" + index, string.Empty, string.Empty, symbol.Border.Width, symbol.Border.Color, 1, fill);
            smithchart.Helper.DrawSymbol(builder, smithchart.Rendering, new Point(locX, locY), shape.ToString(), new Size(symbol.Width, symbol.Height), symbolOption);
        }

        internal void RenderLegend(RenderTreeBuilder builder)
        {
            SmithChartLegendSettings legend = smithchart.LegendSettings;
            SmithChartAlignment alignment = legend.Alignment;
            SmithChartCommonFont titleFont = smithchart.Title.Font ?? (SmithChartCommonFont)smithchart.Title.TextStyle;
            double maxWidth = 0, startX, startY, smithchartTitleHeight = SmithChartHelper.MeasureText(smithchart.Title.Text, titleFont).Height,
            smithchartSubtitleHeight = SmithChartHelper.MeasureText(smithchart.Title.Subtitle.Text, smithchart.Title.Subtitle.TextStyle).Height,
            elementSpacing = smithchart.ElementSpacing,
            offset = smithchartTitleHeight + smithchartSubtitleHeight + elementSpacing + smithchart.Margin.Top,
            itemPadding = legend.ItemPadding > 0 ? legend.ItemPadding : 0,
            svgObjectWidth = smithchart.AvailableSize.Width, svgObjectHeight = smithchart.AvailableSize.Height,
            legendBorder = legend.Border.Width, legendWidth = 0;
            Rect legendBounds = new Rect(LegendActualBounds.X, LegendActualBounds.Y, LegendActualBounds.Width, LegendActualBounds.Height);
            Size titleSize = SmithChartHelper.MeasureText(legend.Title.Text, legend.Title.TextStyle);
            double legendTitleHeight = titleSize.Height, borderSize = smithchart.Border.Width,
            svgWidth = svgObjectWidth - (borderSize * 2), svgHeight = svgObjectHeight - (borderSize * 2);
            LegendPosition legendPosition = legend.Position;
            if (legendPosition != LegendPosition.Custom)
            {
                switch (legendPosition)
                {
                    case LegendPosition.Bottom:
                        legendBounds.Y = svgHeight - (legendBounds.Height + legendBorder + elementSpacing);
                        break;
                    case LegendPosition.Top:
                        legendBounds.Y = borderSize + offset;
                        break;
                    case LegendPosition.Right:
                        legendBounds.X = svgWidth - legendBounds.Width - (elementSpacing * 2);
                        break;
                    case LegendPosition.Left:
                        legendBounds.X = borderSize + (elementSpacing * 2);
                        break;
                }

                if (legendPosition == LegendPosition.Left || legendPosition == LegendPosition.Right)
                {
                    switch (alignment)
                    {
                        case SmithChartAlignment.Center:
                            legendBounds.Y = (svgHeight / 2) - ((legendBounds.Height + (legendBorder * 2)) / 2) + (elementSpacing / 2);
                            break;
                        case SmithChartAlignment.Near:
                            legendBounds.Y = borderSize + (elementSpacing * 2) + offset;
                            break;
                        case SmithChartAlignment.Far:
                            legendBounds.Y = svgHeight - (legendBounds.Height + legendBorder) - (elementSpacing * 2);
                            break;
                    }
                }
                else
                {
                    switch (alignment)
                    {
                        case SmithChartAlignment.Center:
                            legendBounds.X = (svgWidth / 2) - ((legendBounds.Width + (legendBorder * 2)) / 2) + (elementSpacing / 2);
                            break;
                        case SmithChartAlignment.Near:
                            legendBounds.X = borderSize + (elementSpacing * 2);
                            break;
                        case SmithChartAlignment.Far:
                            legendBounds.X = svgWidth - (legendBounds.Width + legendBorder) - (elementSpacing * 2);
                            break;
                    }
                }
            }
            else
            {
                legendBounds.Y = (legend.Location.Y < svgHeight) ? legend.Location.Y : 0;
                legendBounds.X = (legend.Location.X < svgWidth) ? legend.Location.X : 0;
            }

            if (legendPosition == LegendPosition.Bottom || legendPosition == LegendPosition.Top)
            {
                for (int i = 0; i < LegendSeries.Count; i++)
                {
                    legendWidth += LegendSeries[i].Bounds.Width + itemPadding;
                    if (legendWidth > svgWidth)
                    {
                        legendBounds.X = (svgWidth / 2) - ((legendBounds.Width + (legendBorder * 2)) / 2) + (elementSpacing / 2);
                        break;
                    }
                }
            }

            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + "_legend_group", "translate(" + legendBounds.X.ToString(culture) + "," + legendBounds.Y.ToString(culture) + ")");
            double currentX = startX = elementSpacing, currentY = startY = elementSpacing;
            if (!string.IsNullOrEmpty(legend.Title.Text) && legend.Title.Visible)
            {
                DrawLegendTitle(legend, legendBounds, builder);
                currentY = startY = elementSpacing + legendTitleHeight;
            }

            DrawLegendBorder(legend, legendBounds, builder);
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + "legendItem_Group");
            for (int k = 0; k < LegendSeries.Count; k++)
            {
                if ((legend.RowCount < legend.ColumnCount || legend.RowCount == legend.ColumnCount) &&
                    (legendPosition == LegendPosition.Top || legendPosition == LegendPosition.Bottom || legendPosition == LegendPosition.Custom))
                {
                    if ((currentX + LegendSeries[k].Bounds.Width) > legendBounds.Width + startX)
                    {
                        currentX = elementSpacing;
                        currentY += LegendSeries[k].Bounds.Height + itemPadding;
                    }

                    DrawLegendItem(legend, LegendSeries[k], k, currentX, currentY, builder);
                    currentX += LegendSeries[k].Bounds.Width + itemPadding;
                }
                else
                {
                    if ((currentY + LegendSeries[k].Bounds.Height + itemPadding + legendTitleHeight + borderSize) > (legendBounds.Height + startY))
                    {
                        currentY = startY;
                        currentX += maxWidth + itemPadding;
                    }

                    DrawLegendItem(legend, LegendSeries[k], k, currentX, currentY, builder);
                    currentY += LegendSeries[k].Bounds.Height + itemPadding;
                    maxWidth = Math.Max(maxWidth, LegendSeries[k].Bounds.Width);
                }
            }

            builder.CloseElement();
            builder.CloseElement();
        }

        internal void Click(SmithChartInternalMouseEventArgs eventArgs)
        {
            if (!smithchart.LegendSettings.Visible)
            {
                return;
            }

            List<string> legendItemsId = new List<string>() { smithchart.ID + "_svg_Legend", smithchart.ID + "_LegendItemText", smithchart.ID + "_LegendItemShape" };
            foreach (string id in legendItemsId)
            {
                if (eventArgs.Target.Contains(id, StringComparison.InvariantCulture))
                {
                    int seriesIndex = int.Parse(eventArgs.Target.Split(id)[1], null);
                    SmithChartSeries series = smithchart.VisibleSeries[seriesIndex];
                    LegendSeries legend = LegendSeries[seriesIndex];
                    if (series.Fill != null)
                    {
                        smithchart.VisibleSeries[seriesIndex].Interior = series.Fill;
                    }

                    if (smithchart.LegendSettings.ToggleVisibility)
                    {
                        bool visibility = series.Visible;
#pragma warning disable BL0005
                        series.Visible = !visibility;
#pragma warning restore BL0005
                        legend.Visible = visibility;
                        smithchart.AnimateSeries = false;
                        smithchart.RefreshChart();
                    }

                    break;
                }
            }
        }

        internal void Dispose()
        {
            smithchart = null;
            LegendSeries = null;
            themeStyle = null;
            LegendActualBounds = null;
        }
    }
}