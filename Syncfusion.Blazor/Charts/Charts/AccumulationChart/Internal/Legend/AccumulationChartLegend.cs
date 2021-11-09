using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal partial class AccumulationChartLegend : BaseLegend, ILegendBaseMethods
    {
        private SfAccumulationChart accChart { get; set; }

        private Rect titleRect { get; set; } = new Rect();

        private double totalRowCount { get; set; }

        private double maxColumnWidth { get; set; }

        public string ChartID { get; set; }

        internal AccumulationChartLegend(SfAccumulationChart chart, ChartThemeStyle themeStyle)
            : base(themeStyle)
        {
            accChart = chart;
            ChartID = accChart.ID;
        }

        internal void GetLegendOptions(List<AccumulationChartSeries> series)
        {
            LegendCollection.Clear();
            BaseLegendRef = this;
            IsInverse = accChart.LegendSettings.IsInversed || accChart.EnableRTL;
            for (var i = 0; i < 1; i++)
            {
                string seriesType = series[i].Type.ToString();
                if (seriesType == "Pie")
                {
                    seriesType = series[i].InnerRadius != "0" && series[i].InnerRadius != "0%" ? "Doughnut" : "Pie";
                }

                foreach (AccumulationPoints point in series[i].Points)
                {
                    if (point.X != null && !double.IsNaN((double)point.Y) && point.LegendVisible)
                    {
                        LegendCollection.Add(new LegendOption()
                        {
                            Text = point.X.ToString(),
                            Fill = point.Color,
                            Shape = series[i].LegendShape,
                            Visible = point.Visible,
                            AccType = seriesType,
                            PointIndex = point.Index,
                            SeriesIndex = series.IndexOf(series[i])
                        });
                    }
                }
            }
        }

        public void GetLegendBounds(Size availableSize, Rect legendBounds, Rect rect, Size maxLabelSize)
        {
            AccumulationChartLegendSettings legend = accChart.LegendSettings;
            double padding = legend.Padding;
            if (!IsVertical)
            {
                legendBounds.Height += string.IsNullOrEmpty(legend.Height) ? ((availableSize.Height / 100) * 5) : 0;
            }
            else
            {
                legendBounds.Width += string.IsNullOrEmpty(legend.Width) ? ((availableSize.Width / 100) * 5) : 0;
            }

            List<double> columnWidth = new List<double>();
            double maximumWidth = 0, rowWidth = 0, rowCount = 0, columnHeight = 0, legendWidth = 0;
            BorderWidth = legend.Border.Width;
            LegendTextStyle = SfAccumulationChart.GetFontOptions(legend.TextStyle);
            MaxItemHeight = Math.Max(ChartHelper.MeasureText("MeasureText", LegendTextStyle).Height, legend.ShapeHeight);
            bool render = false;
            foreach (LegendOption legendOption in LegendCollection)
            {
                AccumulationLegendRenderEventArgs legendEventArgs = new AccumulationLegendRenderEventArgs()
                {
                    Fill = legendOption.Fill,
                    Text = legendOption.Text,
                    Shape = legendOption.Shape,
                    Name = AccumulationChartConstants.LEGENDRENDER
                };
                if (accChart.AccumulationChartEvents?.OnLegendItemRender != null)
                {
                    accChart.AccumulationChartEvents.OnLegendItemRender.Invoke(legendEventArgs);
                }

                legendOption.Render = !legendEventArgs.Cancel;
                legendOption.Text = legendEventArgs.Text;
                legendOption.Fill = legendEventArgs.Fill;
                legendOption.Shape = legendEventArgs.Shape;
                legendOption.TextSize = ChartHelper.MeasureText(legendOption.Text, LegendTextStyle);
                if (legendOption.Render && !string.IsNullOrEmpty(legendOption.Text))
                {
                    render = true;
                    legendWidth = legend.ShapeWidth + legend.ShapePadding + legendOption.TextSize.Width + padding;
                    if (IsVertical)
                    {
                        ++rowCount;
                        columnHeight = (rowCount * (MaxItemHeight + padding)) + padding;
                        if ((rowCount * (MaxItemHeight + padding)) + padding > legendBounds.Height)
                        {
                            columnHeight = Math.Max(columnHeight, (rowCount * (MaxItemHeight + padding)) + padding);
                            rowWidth += maximumWidth;
                            columnWidth.Add(maximumWidth);
                            TotalPages = Math.Max(rowCount, TotalPages != 0 ? TotalPages : 1);
                            maximumWidth = 0;
                            rowCount = 1;
                        }

                        maximumWidth = Math.Max(legendWidth, maximumWidth);
                    }
                    else
                    {
                        rowWidth += legendWidth;
                        if (legendBounds.Width < (padding + rowWidth))
                        {
                            maximumWidth = Math.Max(maximumWidth, rowWidth + padding - legendWidth);
                            if (rowCount == 0 && legendWidth != rowWidth)
                            {
                                rowCount = 1;
                            }

                            rowWidth = legendWidth;
                            rowCount++;
                            columnHeight = (rowCount * (MaxItemHeight + padding)) + padding;
                        }
                    }
                }
            }

            if (IsVertical)
            {
                rowWidth += maximumWidth;
                IsPaging = legendBounds.Width < (rowWidth + padding);
                columnHeight = Math.Max(columnHeight, ((TotalPages != 0 ? TotalPages : 1) * (MaxItemHeight + padding)) + padding);
                IsPaging = IsPaging && TotalPages > 1;
                if (columnWidth.Count == 0 || (columnWidth.Count != 0 && columnWidth[columnWidth.Count - 1] != maximumWidth))
                {
                    columnWidth.Add(maximumWidth);
                }
            }
            else
            {
                IsPaging = legendBounds.Height < columnHeight;
                TotalPages = accChart.AccumulationLegendModule.totalRowCount = rowCount;
                columnHeight = Math.Max(columnHeight, MaxItemHeight + padding + padding);
            }

            accChart.AccumulationLegendModule.MaxColumns = 0;
            if (render)
            {
                SetBounds(IsVertical ? accChart.AccumulationLegendModule.GetMaxColumn(columnWidth, legendBounds.Width, padding, rowWidth + padding) : Math.Max(rowWidth + padding, maximumWidth), columnHeight, legend);
            }
            else
            {
                SetBounds(0, 0, legend);
            }

            accChart.AccumulationLegendModule.GetLocation(accChart.LegendSettings, LegendBounds, rect, availableSize, accChart.LegendSettings.Margin, accChart.Margin, accChart.LegendSettings.Border, accChart.LegendSettings.Location);
        }

        private double GetMaxColumn(List<double> columns, double width, double padding, double rowWidth)
        {
            double maxPageColumn = padding;
            if (columns.Count > 0)
            {
                maxColumnWidth = columns.Max();
            }

            foreach (double column in columns)
            {
                maxPageColumn += maxColumnWidth;
                MaxColumns++;
                if (maxPageColumn + padding > width)
                {
                    maxPageColumn -= maxColumnWidth;
                    MaxColumns--;
                    break;
                }
            }

            IsPaging = maxPageColumn < rowWidth && TotalPages > 1;
            if (maxPageColumn == padding)
            {
                maxPageColumn = width;
            }

            MaxColumns = Math.Max(1, MaxColumns);
            MaxWidth = maxPageColumn;
            return maxPageColumn;
        }

        private double GetAvailWidth(double width)
        {
            if (IsVertical)
            {
                width = MaxWidth;
            }

            return width - ((accChart.LegendSettings.Padding * 2) + accChart.LegendSettings.ShapeWidth + accChart.LegendSettings.ShapePadding);
        }

        public void GetRenderPoint(LegendOption legendOption, ChartInternalLocation start, double textPadding, LegendOption prevLegend, Rect rect, int count, int firstLegend)
        {
            if (IsVertical)
            {
                if (count == firstLegend || Math.Round(prevLegend.Location.Y + (MaxItemHeight * 1.5) + (accChart.LegendSettings.Padding * 2)) > Math.Round(rect.Y + rect.Height))
                {
                    legendOption.Location.X = prevLegend.Location.X + (count == firstLegend ? 0 : (!IsInverse) ? maxColumnWidth : -maxColumnWidth);
                    legendOption.Location.Y = start.Y;
                    double textStartLoc = (accChart.LegendSettings.ShapeWidth / 2) + accChart.LegendSettings.Padding;
                    PageXCollections.Add(legendOption.Location.X + ((!IsInverse) ? -textStartLoc : textStartLoc));
                    TotalPages++;
                }
                else
                {
                    legendOption.Location.X = prevLegend.Location.X;
                    legendOption.Location.Y = prevLegend.Location.Y + MaxItemHeight + accChart.LegendSettings.Padding;
                }
            }
            else
            {
                double textWidth = textPadding + prevLegend.TextSize.Width, previousBound = (!IsInverse) ? prevLegend.Location.X + textWidth : (prevLegend.Location.X + 0.5) - textWidth;
                if (IsWithinBounds(previousBound, legendOption.TextSize.Width + textPadding, rect, accChart.LegendSettings.ShapeWidth / 2))
                {
                    legendOption.Location.Y = (count == firstLegend) ? prevLegend.Location.Y : prevLegend.Location.Y + MaxItemHeight + accChart.LegendSettings.Padding;
                    legendOption.Location.X = start.X;
                }
                else
                {
                    legendOption.Location.Y = prevLegend.Location.Y;
                    legendOption.Location.X = (count == firstLegend) ? prevLegend.Location.X : previousBound;
                }

                TotalPages = accChart.AccumulationLegendModule.totalRowCount;
            }

            legendOption.Text = ChartHelper.TextTrim(Math.Round(accChart.AccumulationLegendModule.GetAvailWidth(LegendBounds.Width), 4), legendOption.Text, LegendTextStyle);
        }

        /// <summary>
        /// The method is used to check whether current legend group within the legend bounds.
        /// </summary>
        private bool IsWithinBounds(double previousBound, double textWidth, Rect legendBounds, double shapeWidth)
        {
            if (!IsInverse)
            {
                return (previousBound + textWidth) > (legendBounds.X + legendBounds.Width + shapeWidth);
            }
            else
            {
                return (previousBound - textWidth) < (legendBounds.X - shapeWidth);
            }
        }

        internal void GetSmartLegendLocation(Rect labelBound, Rect legendBound, ChartCommonMargin margin)
        {
            double space;
            switch (Position)
            {
                case LegendPosition.Left:
                    space = (labelBound.X - legendBound.Width - margin.Left) / 2;
                    legendBound.X = (labelBound.X - legendBound.Width) < margin.Left ? legendBound.X : labelBound.X - legendBound.Width - space;
                    break;
                case LegendPosition.Right:
                    space = (accChart.AvailableSize.Width - margin.Right - (labelBound.X + labelBound.Width + legendBound.Width)) / 2;
                    legendBound.X = (labelBound.X + labelBound.Width + legendBound.Width) > (accChart.AvailableSize.Width - margin.Right) ? legendBound.X : labelBound.X + labelBound.Width + space;
                    break;
                case LegendPosition.Top:
                    GetTitleRect();
                    space = (labelBound.Y - legendBound.Height - (titleRect.Y + titleRect.Height)) / 2;
                    legendBound.Y = (labelBound.Y - legendBound.Height) < margin.Top ? legendBound.Y : labelBound.Y - legendBound.Height - space;
                    break;
                case LegendPosition.Bottom:
                    space = (accChart.AvailableSize.Height - margin.Bottom - (labelBound.Y + labelBound.Height + legendBound.Height)) / 2;
                    legendBound.Y = (labelBound.Y + labelBound.Height + legendBound.Height) > (accChart.AvailableSize.Height - margin.Bottom) ? legendBound.Y : labelBound.Y + labelBound.Height + space;
                    break;
            }
        }

        private void GetTitleRect()
        {
            if (!string.IsNullOrEmpty(accChart.Title))
            {
                Size titleSize = ChartHelper.MeasureText(accChart.Title, SfAccumulationChart.GetFontOptions(accChart.TitleStyle));
                titleRect = new Rect((accChart.AvailableSize.Width / 2) - (titleSize.Width / 2), accChart.Margin.Top, titleSize.Width, titleSize.Height);
            }
        }

        internal void Dispose()
        {
            accChart = null;
            titleRect = null;
        }
    }
}