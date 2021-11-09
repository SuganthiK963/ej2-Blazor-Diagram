using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the properties and methods to render legend.
    /// </summary>
    internal class LegendRenderer
    {
        private const int PAGEBUTTONSIZE = 8;
        private const string SPACE = " ";
        private double padding;
        private bool isLegendVertical;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendRenderer"/> class.
        /// </summary>
        /// <param name="parent">represent the properties of the legend.</param>
        public LegendRenderer(SfCircularGauge parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets or sets the properties of the circular gauge.
        /// </summary>
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render the legend.
        /// </summary>
        internal LegendSetting LegendSetting { get; set; } = new LegendSetting();

        /// <summary>
        /// Creates the properties to render the legend.
        /// </summary>
        /// <param name="axis">Specifies the properties of the axis.</param>
        /// <param name="index">Specifies the index of the axis.</param>
        internal void GetLegendOptions(CircularGaugeAxis axis, double index)
        {
            if (axis.Ranges != null)
            {
                if (Parent.LegendSettings.TextStyle == null)
                {
                    Parent.LegendSettings.TextStyle = new CircularGaugeLegendTextStyle();
#pragma warning disable BL0005
                    Parent.LegendSettings.TextStyle.Size = "12px";
#pragma warning restore BL0005
                }

                bool isNumber = double.TryParse(Parent.LegendSettings.TextStyle.Size, out double sizeValue);
                LegendSetting.LegendFontSize = !string.IsNullOrEmpty(Parent.LegendSettings.TextStyle.Size) ? (Parent.LegendSettings.TextStyle.Size.IndexOf("px", StringComparison.InvariantCulture) > 0 ?
                    Convert.ToDouble(Parent.LegendSettings.TextStyle.Size.Replace("px", string.Empty, StringComparison.InvariantCulture), culture) : (isNumber ? sizeValue : 12)) : 12;

                for (int i = 0; i < axis.Ranges.Count; i++)
                {
                    AxisInternal axisValues = axis.AxisValues;
                    if (axisValues.RangeStart[i] >= 0 && axisValues.RangeEnd[i] >= 0 && axisValues.RangeStart[i] != axisValues.RangeEnd[i])
                    {
                        string legendText = string.IsNullOrEmpty(axis.Ranges[i].LegendText) ? axisValues.RangeStart[i] + "-" + axisValues.RangeEnd[i] : axis.Ranges[i].LegendText;
                        Legend legendItem = new Legend
                        {
                            Text = legendText,
                            Fill = axis.Ranges[i].Color,
                            RangeIndex = i,
                            AxisIndex = (int)index,
                            LegendRender = false,
                            LegendTextSize = AxisRenderer.MeasureText(legendText, LegendSetting.LegendFontSize),
#pragma warning disable CA1508
                            LegendToggleFill = Parent.LegendSettings.TextStyle != null && !string.IsNullOrEmpty(Parent.LegendSettings.TextStyle.Color) ? Parent.LegendSettings.TextStyle.Color : Parent.ThemeStyles.LabelColor,
#pragma warning restore CA1508
                        };
                        LegendSetting.LegendItemCollections.Add(legendItem);
                    }
                }
            }

            LegendBounds();
        }

        /// <summary>
        /// Calculates the bounds of the legend.
        /// </summary>
        internal void GetLegendBounds()
        {
            padding = !double.IsNaN(Parent.LegendSettings.Padding) ? Parent.LegendSettings.Padding : 8;
            double extraWidth = 0;
            double extraHeight = 0;
            double maximumWidth = 0;
            double rowWidth = 0;
            double rowCount = 0;
            double columnHeight = 0;
            List<double> columnWidth = new List<double>();
            if (!isLegendVertical)
            {
                extraHeight = string.IsNullOrEmpty(Parent.LegendSettings.Height) ? ((Parent.AvailableSize.Height / 100) * 5) : 0;
            }
            else
            {
                extraWidth = string.IsNullOrEmpty(Parent.LegendSettings.Width) ? ((Parent.AvailableSize.Width / 100) * 5) : 0;
            }

            LegendSetting.LegendBounds.Width += extraWidth;
            LegendSetting.LegendBounds.Height += extraHeight;
            bool isLegendRender = false;
            LegendSetting.MaximumItemHeight = Math.Max(AxisRenderer.MeasureText("MeasureText", LegendSetting.LegendFontSize).Height, Parent.LegendSettings.ShapeHeight);
            for (int i = 0; i < LegendSetting.LegendItemCollections.Count; i++)
            {
                LegendSetting.LegendItemCollections[i].LegendRender = true;
                SizeD textSize = AxisRenderer.MeasureText(LegendSetting.LegendItemCollections[i].Text, LegendSetting.LegendFontSize);
                if (LegendSetting.LegendItemCollections[i].LegendRender && !string.IsNullOrEmpty(LegendSetting.LegendItemCollections[i].Text))
                {
                    isLegendRender = true;
                    int legendWidth = Convert.ToInt32(Parent.LegendSettings.ShapeWidth + (2 * Parent.LegendSettings.ShapePadding) + textSize.Width + (2 * padding));
                    if (isLegendVertical)
                    {
                        ++rowCount;
                        columnHeight = (rowCount * (LegendSetting.MaximumItemHeight + padding)) + padding;
                        if ((rowCount * (LegendSetting.MaximumItemHeight + padding)) + padding > LegendSetting.LegendBounds.Height)
                        {
                            columnHeight = Math.Max(columnHeight, (rowCount * (LegendSetting.MaximumItemHeight + padding)) + padding);
                            rowWidth = rowWidth + maximumWidth;
                            columnWidth.Add(maximumWidth);
                            LegendSetting.TotalPages = Math.Max(rowCount, LegendSetting.TotalPages);
                            maximumWidth = 0;
                            rowCount = 1;
                        }

                        maximumWidth = Math.Max(legendWidth, maximumWidth);
                    }
                    else
                    {
                        rowWidth = rowWidth + legendWidth;
                        if (LegendSetting.LegendBounds.Width < (padding + rowWidth))
                        {
                            maximumWidth = Math.Max(maximumWidth, rowWidth + padding - legendWidth);
                            if (rowCount == 0 && (legendWidth != rowWidth))
                            {
                                rowCount = 1;
                            }

                            rowWidth = legendWidth;
                            rowCount++;
                            columnHeight = (rowCount * (LegendSetting.MaximumItemHeight + padding)) + padding;
                        }
                    }
                }
            }

            if (isLegendVertical)
            {
                rowWidth = rowWidth + maximumWidth;
                LegendSetting.IsPagingEnabled = LegendSetting.LegendBounds.Width < (rowWidth + padding);
                columnHeight = Math.Max(columnHeight, (LegendSetting.TotalPages * (LegendSetting.MaximumItemHeight + padding)) + padding);
                LegendSetting.IsPagingEnabled = LegendSetting.IsPagingEnabled && (LegendSetting.TotalPages > 1);
                if (columnWidth.Count == 0)
                {
                    columnWidth.Add(maximumWidth);
                }
                else if (columnWidth[columnWidth.Count - 1] != maximumWidth)
                {
                    columnWidth.Add(maximumWidth);
                }
            }
            else
            {
                LegendSetting.IsPagingEnabled = LegendSetting.LegendBounds.Height < columnHeight;
                LegendSetting.TotalPages = LegendSetting.TotalRowCount = (int)rowCount;
                columnHeight = Math.Max(columnHeight, LegendSetting.MaximumItemHeight + (2 * padding));
            }

            LegendSetting.MaximumColumns = 0;
            LegendSetting.MaximumColumnWidth = isLegendVertical ? GetMaxColumn(columnWidth, LegendSetting.LegendBounds.Width, padding) : Math.Max(rowWidth + padding, maximumWidth);
            if (isLegendRender)
            {
                SetBounds(LegendSetting.MaximumColumnWidth, columnHeight, LegendSetting.LegendBounds);
            }
            else
            {
                SetBounds(0, 0, LegendSetting.LegendBounds);
            }
        }

        /// <summary>
        /// Translates the legend pages for circular gauge.
        /// </summary>
        /// <param name="currentPage">Specifies the current page of the legend.</param>
        /// <param name="current">Specifies the current page number.</param>
        internal void TranslatePage(int currentPage, int current)
        {
            double tranlateLegendSize = LegendSetting.ClipPathHeight * currentPage;
            LegendSetting.LegendTranslate = "translate(0,-" + tranlateLegendSize.ToString(culture) + ")";
            if (isLegendVertical)
            {
                tranlateLegendSize = LegendSetting.PageXCollections[currentPage * (int)LegendSetting.MaximumColumns] - LegendSetting.LegendBounds.X;
                tranlateLegendSize = tranlateLegendSize < 0 ? 0 : tranlateLegendSize;
                LegendSetting.LegendTranslate = "translate(-" + tranlateLegendSize.ToString(culture) + ",0)";
            }

            LegendSetting.LegendPageText.Text = current + "/" + LegendSetting.TotalPages.ToString(culture);
            LegendSetting.CurrentPage = current;
        }

        /// <summary>
        /// Renders the legend in the Circular Gauge.
        /// </summary>
        internal void RenderLegend()
        {
            double firstLegend = FindFirstLegendPosition();
            double count = 0;
            double legendPadding = !double.IsNaN(Parent.LegendSettings.Padding) ? Parent.LegendSettings.Padding : 8;
            if (Parent.LegendSettings.TextStyle == null)
            {
#pragma warning disable BL0005
                Parent.LegendSettings.TextStyle = new CircularGaugeLegendTextStyle();
#pragma warning restore BL0005
            }

            string legendFontFamily = !string.IsNullOrEmpty(Parent.LegendSettings.TextStyle.FontFamily) ? Parent.LegendSettings.TextStyle.FontFamily : Parent.ThemeStyles.LabelFontFamily;
            LegendSetting.LegendTextStyle = "font-size:" + Parent.LegendSettings.TextStyle.Size +
              "; font-style:" + Parent.LegendSettings.TextStyle.FontStyle + "; font-weight:" + Parent.LegendSettings.TextStyle.FontWeight +
              "; font-family:" + Parent.LegendSettings.TextStyle.FontFamily + ";opacity:" + Parent.LegendSettings.TextStyle.Opacity + ";user-select:none";
            LegendSetting.MaximumItemHeight = Math.Max(LegendSetting.LegendItemCollections[0].LegendTextSize.Height, Parent.LegendSettings.ShapeHeight);
            LegendSetting.Background = !string.IsNullOrEmpty(Parent.LegendSettings.Background) ? Parent.LegendSettings.Background : "transparent";
            LegendSetting.BorderStroke = Parent.LegendSettings.Border != null && !string.IsNullOrEmpty(Parent.LegendSettings.Border.Color) ? Parent.LegendSettings.Border.Color : "transparent";
            LegendSetting.BorderStrokeWidth = Parent.LegendSettings.Border != null && !double.IsNaN(Parent.LegendSettings.Border.Width) ? Parent.LegendSettings.Border.Width : 0;
            LegendSetting.LegendBorderRect = new Rect
            {
                X = LegendSetting.LegendBounds.X - legendPadding,
                Y = Parent.LegendSettings.Position == LegendPosition.Bottom ? LegendSetting.LegendBounds.Y - (LegendSetting.BorderStrokeWidth / 2) : LegendSetting.LegendBounds.Y,
                Width = isLegendVertical ? LegendSetting.MaximumWidth : LegendSetting.LegendBounds.Width,
                Height = isLegendVertical ? LegendSetting.LegendBounds.Height : LegendSetting.LegendBounds.Height + LegendSetting.BorderStrokeWidth,
            };
            Legend previousLegend = new Legend();
            PointF legendStart = default(PointF);
            double textPadding = 0;
            TextSetting legendText = new TextSetting();
            if (firstLegend != LegendSetting.LegendItemCollections.Count)
            {
                LegendSetting.TotalPages = 0;
                legendStart = new PointF
                {
                    X = (float)(LegendSetting.LegendBounds.X + legendPadding + (Parent.LegendSettings.ShapeWidth / 2)),
                    Y = (float)(LegendSetting.LegendBounds.Y + legendPadding + (LegendSetting.MaximumItemHeight / 2)),
                };
                legendText = new TextSetting
                {
                    X = legendStart.X,
                    Y = legendStart.Y,
                    Anchor = "Start",
                };
                textPadding = (2 * Parent.LegendSettings.ShapePadding) + (2 * legendPadding) + Parent.LegendSettings.ShapeWidth;
                LegendSetting.LegendItemCollections[(int)firstLegend].LegendLocation = legendStart;
                previousLegend = LegendSetting.LegendItemCollections[(int)firstLegend];
                for (int i = 0; i < LegendSetting.LegendItemCollections.Count; i++)
                {
                    if (LegendSetting.LegendItemCollections[i].LegendRender && !string.IsNullOrEmpty(LegendSetting.LegendItemCollections[i].Text))
                    {
                        GetRenderPoint(LegendSetting.LegendItemCollections[i], legendStart, textPadding, previousLegend, LegendSetting.LegendBounds, count, firstLegend);
                        RenderSymbol(LegendSetting.LegendItemCollections[i], LegendSetting.LegendItemCollections[i].AxisIndex, LegendSetting.LegendItemCollections[i].RangeIndex);
                        RenderText(LegendSetting.LegendItemCollections[i], legendText);
                        previousLegend = LegendSetting.LegendItemCollections[i];
                    }

                    count++;
                }

                if (LegendSetting.IsPagingEnabled)
                {
                    RenderPagingElements(legendText);
                }
                else
                {
                    LegendSetting.TotalPages = 1;
                }
            }
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal void Dispose()
        {
            Parent = null;
            LegendSetting = null;
        }

        private static Rect SubtractThickness(Rect rect, int left, int right, int top, double bottom)
        {
            rect.X += left;
            rect.Y += top;
            rect.Width -= left + right;
            rect.Height -= top + bottom;
            return rect;
        }

        private void LegendBounds()
        {
            CircularGaugeLegendSettings settings = Parent.LegendSettings;
            LegendPosition legendPosition = (settings.Position != LegendPosition.Auto) ? settings.Position :
                (Parent.AvailableSize.Width > Parent.AvailableSize.Height ? LegendPosition.Right : LegendPosition.Bottom);
            Rect gaugeRect = Parent.AxisRenderer.AxisSetting.GaugeRect;
            isLegendVertical = legendPosition == LegendPosition.Right || legendPosition == LegendPosition.Left;
            LegendSetting.LegendBounds = new Rect
            {
                X = gaugeRect.X, Y = gaugeRect.Y, Width = 0, Height = 0,
            };
            if (isLegendVertical)
            {
                LegendSetting.LegendBounds.Width = !string.IsNullOrEmpty(settings.Width) ?
                    SfCircularGauge.StringToNumber(settings.Width, Parent.AvailableSize.Width) :
                    SfCircularGauge.StringToNumber("20%", Parent.AvailableSize.Width);
                LegendSetting.LegendBounds.Height = !string.IsNullOrEmpty(settings.Height) ?
                    SfCircularGauge.StringToNumber(settings.Height, Parent.AvailableSize.Height - (gaugeRect.Y - ((Parent.Margin != null) ? Parent.Margin.Top : 10))) :
                    gaugeRect.Height;
            }
            else
            {
                LegendSetting.LegendBounds.Width = !string.IsNullOrEmpty(settings.Width) ?
                    SfCircularGauge.StringToNumber(settings.Width, Parent.AvailableSize.Width) : gaugeRect.Width;
                LegendSetting.LegendBounds.Height = !string.IsNullOrEmpty(settings.Height) ?
                    SfCircularGauge.StringToNumber(settings.Height, Parent.AvailableSize.Height) : SfCircularGauge.StringToNumber("20%", Parent.AvailableSize.Height);
            }

            GetLegendBounds();
            GetLocation();
        }

        private double GetMaxColumn(List<double> columns, double width, double padding)
        {
            SetMaximumColumn(columns);
            double maximumPageColumn = padding;
            for (int i = 0; i < columns.Count; i++)
            {
                maximumPageColumn += LegendSetting.MaximumColumnWidth;
                LegendSetting.MaximumColumns++;
                if (maximumPageColumn + padding > width)
                {
                    maximumPageColumn -= LegendSetting.MaximumColumnWidth;
                    LegendSetting.MaximumColumns--;
                    break;
                }
            }

            if (maximumPageColumn == padding)
            {
                maximumPageColumn = width;
            }

            LegendSetting.MaximumColumns = Math.Max(1, LegendSetting.MaximumColumns);
            LegendSetting.MaximumWidth = maximumPageColumn;
            return maximumPageColumn;
        }

        private void SetMaximumColumn(List<double> columns)
        {
            int maxValue = 0;
            foreach (int column in columns)
            {
                if (column > maxValue)
                {
                    maxValue = column;
                }

                LegendSetting.MaximumColumnWidth = maxValue;
            }
        }

        private void SetBounds(double computedWidth, double computedHeight, Rect legendBounds)
        {
            computedWidth = computedWidth < legendBounds.Width ? computedWidth : legendBounds.Width;
            computedHeight = computedHeight < legendBounds.Height ? computedHeight : legendBounds.Height;
            legendBounds.Width = string.IsNullOrEmpty(Parent.LegendSettings.Width) ? computedWidth : legendBounds.Width;
            legendBounds.Height = string.IsNullOrEmpty(Parent.LegendSettings.Height) ? computedHeight : legendBounds.Height;
            LegendSetting.RowCount = Math.Max(1, Math.Ceiling((legendBounds.Height - Parent.LegendSettings.Padding) / (LegendSetting.MaximumItemHeight + Parent.LegendSettings.Padding)));
        }

        private void GetLocation()
        {
            padding = Parent.LegendSettings.Border != null && Parent.LegendSettings.Border.Width != 0 ?
                Parent.LegendSettings.Border.Width : 1;
            double legendHeight = LegendSetting.LegendBounds.Height + padding + (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Top : 0) +
            (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Bottom : 0);
            double legendWidth = LegendSetting.LegendBounds.Width + padding + (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Left : 0) +
            (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Right : 0);
            double marginBottom = Parent.Margin != null ? Parent.Margin.Bottom : 0;
            if (Parent.LegendSettings.Position == LegendPosition.Bottom)
            {
                LegendSetting.LegendBounds.X = AlignLegend(LegendSetting.LegendBounds.X, Parent.AvailableSize.Width, LegendSetting.LegendBounds.Width);
                LegendSetting.LegendBounds.Y = Parent.AxisRenderer.AxisSetting.GaugeRect.Y + (Parent.AxisRenderer.AxisSetting.GaugeRect.Height - legendHeight)
                    + padding + (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Top : 0);
                SubtractThickness(Parent.AxisRenderer.AxisSetting.GaugeRect, 0, 0, 0, (int)legendHeight);
            }
            else if (Parent.LegendSettings.Position == LegendPosition.Top)
            {
                LegendSetting.LegendBounds.X = AlignLegend(LegendSetting.LegendBounds.X, Parent.AvailableSize.Width, LegendSetting.LegendBounds.Width);
                LegendSetting.LegendBounds.Y = Parent.AxisRenderer.AxisSetting.GaugeRect.Y + padding + (Parent.LegendSettings.Margin != null ?
                    Parent.LegendSettings.Margin.Top : 0);
                SubtractThickness(Parent.AxisRenderer.AxisSetting.GaugeRect, 0, 0, (int)legendHeight, 0);
            }
            else if (Parent.LegendSettings.Position == LegendPosition.Right)
            {
                LegendSetting.LegendBounds.X = Parent.AxisRenderer.AxisSetting.GaugeRect.X + (Parent.AxisRenderer.AxisSetting.GaugeRect.Width - LegendSetting.LegendBounds.Width) +
                    (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Right : 0);
                LegendSetting.LegendBounds.Y = Parent.AxisRenderer.AxisSetting.GaugeRect.Y + AlignLegend(
                    0, Parent.AvailableSize.Height - (Parent.AxisRenderer.AxisSetting.GaugeRect.Y + marginBottom), LegendSetting.LegendBounds.Height);
                SubtractThickness(Parent.AxisRenderer.AxisSetting.GaugeRect, 0, (int)legendWidth, 0, 0);
            }
            else
            {
                LegendSetting.LegendBounds.X = LegendSetting.LegendBounds.X + (Parent.LegendSettings.Margin != null ? Parent.LegendSettings.Margin.Left : 0);
                LegendSetting.LegendBounds.Y = Parent.AxisRenderer.AxisSetting.GaugeRect.Y + AlignLegend(
                    0, Parent.AvailableSize.Height - (Parent.AxisRenderer.AxisSetting.GaugeRect.Y + marginBottom), LegendSetting.LegendBounds.Height);
                SubtractThickness(Parent.AxisRenderer.AxisSetting.GaugeRect, (int)legendWidth, 0, 0, 0);
            }
        }

        private void RenderPagingElements(TextSetting legendText)
        {
            double legendPadding = !double.IsNaN(Parent.LegendSettings.Padding) ? Parent.LegendSettings.Padding : 8;
            double buttonPadding = 12;
            if (!isLegendVertical)
            {
                LegendSetting.TotalPages = Math.Ceiling(LegendSetting.TotalPages / Math.Max(1, LegendSetting.RowCount - 1));
            }
            else
            {
                LegendSetting.TotalPages = Math.Ceiling(LegendSetting.TotalPages / LegendSetting.MaximumColumns);
            }

            double legendFontSize;
            LegendSetting.ClipPathHeight = (LegendSetting.RowCount - 1) * (LegendSetting.MaximumItemHeight + legendPadding);
            PointF legendPageSize = new PointF()
            {
                X = (float)(LegendSetting.LegendBounds.X + (PAGEBUTTONSIZE / 2)),
                Y = isLegendVertical ? (float)(LegendSetting.LegendBounds.Y - Parent.LegendSettings.Border.Width - legendPadding + LegendSetting.ClipPathHeight + ((LegendSetting.LegendBounds.Height - LegendSetting.ClipPathHeight) / 2))
                : (float)(LegendSetting.LegendBounds.Y + LegendSetting.ClipPathHeight + ((LegendSetting.LegendBounds.Height - LegendSetting.ClipPathHeight) / 2)),
            };
            if (Parent.LegendSettings.TextStyle == null)
            {
#pragma warning disable BL0005
                Parent.LegendSettings.TextStyle = new CircularGaugeLegendTextStyle();
#pragma warning restore BL0005
            }

            legendFontSize = !string.IsNullOrEmpty(Parent.LegendSettings.TextStyle.Size) && Parent.LegendSettings.TextStyle.Size.IndexOf("px", StringComparison.InvariantCulture) > 0 ?
                Convert.ToDouble(Parent.LegendSettings.TextStyle.Size.Replace("px", string.Empty, StringComparison.InvariantCulture), culture) : 12;
            SizeD legendPageTextSize = AxisRenderer.MeasureText(LegendSetting.TotalPages + "/" + LegendSetting.TotalPages, legendFontSize);
            SizeD pageButton = new SizeD()
            {
                Width = PAGEBUTTONSIZE,
                Height = PAGEBUTTONSIZE,
            };
            LegendSetting.LegendPageLeftPath = CalculateLegendPageShapes(legendPageSize, "LeftArrow", pageButton);
            LegendSetting.PagingRegions.Add(
                new Rect()
                {
                    X = legendPageSize.X + LegendSetting.LegendBounds.Width - ((2 * (PAGEBUTTONSIZE + LegendSetting.Padding)) + LegendSetting.Padding + legendPageTextSize.Width) - (PAGEBUTTONSIZE * 0.5),
                    Y = legendPageSize.Y - (PAGEBUTTONSIZE * 0.5),
                    Width = PAGEBUTTONSIZE,
                    Height = PAGEBUTTONSIZE,
                });
            LegendSetting.LegendPageText = new TextSetting()
            {
                X = legendPageSize.X + legendPadding,
                Y = legendPageSize.Y + (legendPageTextSize.Height / 4),
                Text = "1/" + LegendSetting.TotalPages.ToString(culture),
            };
            legendPageSize = new PointF()
            {
                X = (int)(legendText.X + legendPadding + (PAGEBUTTONSIZE / 2) + legendPageTextSize.Width),
                Y = legendPageSize.Y,
            };
            LegendSetting.LegendPageRightPath = CalculateLegendPageShapes(legendPageSize, "RightArrow", pageButton);
            LegendSetting.PagingRegions.Add(
                new Rect()
                {
                    X = legendPageSize.X + (LegendSetting.LegendBounds.Width - ((2 * (PAGEBUTTONSIZE + LegendSetting.Padding)) + LegendSetting.Padding + legendPageTextSize.Width) - (PAGEBUTTONSIZE * 0.5)),
                    Y = legendPageSize.Y - (PAGEBUTTONSIZE * 0.5),
                    Width = PAGEBUTTONSIZE,
                    Height = PAGEBUTTONSIZE,
                });
            LegendSetting.LegendPageTranslate = "translate(" + (LegendSetting.LegendBounds.Width - ((2 * (PAGEBUTTONSIZE + buttonPadding)) +
               padding + legendPageTextSize.Width)) + ", " + 0 + ")";
            TranslatePage(LegendSetting.CurrentPage - 1, LegendSetting.CurrentPage);
        }

        private double FindFirstLegendPosition()
        {
            double count = 0;
            for (int i = 0; i < LegendSetting.LegendItemCollections.Count; i++)
            {
                if (LegendSetting.LegendItemCollections[i].LegendRender && !string.IsNullOrEmpty(LegendSetting.LegendItemCollections[i].Text))
                {
                    break;
                }

                count++;
            }

            return count;
        }

        private void GetRenderPoint(Legend legendOption, PointF start, double textPadding, Legend prevLegend, Rect rect, double count, double firstLegend)
        {
            double legendPadding = !double.IsNaN(Parent.LegendSettings.Padding) ? Parent.LegendSettings.Padding : 8;
            if (isLegendVertical)
            {
                if (count == firstLegend || (prevLegend.LegendLocation.Y + (LegendSetting.MaximumItemHeight * 1.5) + (padding * 2) > rect.Y + rect.Height))
                {
                    legendOption.LegendLocation = new PointF
                    {
                        X = (float)(prevLegend.LegendLocation.X + ((count == firstLegend) ? 0 : LegendSetting.MaximumColumnWidth)),
                        Y = start.Y,
                    };
                    LegendSetting.PageXCollections.Add((int)(legendOption.LegendLocation.X - (Parent.LegendSettings.ShapeWidth / 2) - legendPadding));
                    LegendSetting.TotalPages++;
                }
                else
                {
                    legendOption.LegendLocation = new PointF
                    {
                        X = prevLegend.LegendLocation.X,
                        Y = (float)(prevLegend.LegendLocation.Y + LegendSetting.MaximumItemHeight + legendPadding),
                    };
                }
            }
            else
            {
                double previousBound = prevLegend.LegendLocation.X + textPadding + prevLegend.LegendTextSize.Width;
                if ((previousBound + legendOption.LegendTextSize.Width + textPadding) > (rect.X + rect.Width + (Parent.LegendSettings.ShapeWidth / 2)))
                {
                    legendOption.LegendLocation = new PointF
                    {
                        Y = (float)((count == firstLegend) ? prevLegend.LegendLocation.Y
                        : prevLegend.LegendLocation.Y + LegendSetting.MaximumItemHeight + legendPadding),
                        X = start.X,
                    };
                }
                else
                {
                    legendOption.LegendLocation = new PointF
                    {
                        Y = prevLegend.LegendLocation.Y,
                        X = (float)((count == firstLegend) ? prevLegend.LegendLocation.X : previousBound),
                    };
                }

                LegendSetting.TotalPages = LegendSetting.TotalRowCount;
            }

            double availableWidth = GetAvailWidth(rect.Width);
            LegendSetting.LegendLocations.Add(legendOption.LegendLocation);
        }

        private double GetAvailWidth(double width)
        {
            if (isLegendVertical)
            {
                width = LegendSetting.MaximumWidth;
            }

            return width - ((!double.IsNaN(Parent.LegendSettings.Padding) ? (Parent.LegendSettings.Padding * 2) : (8 * 2))
                + (!double.IsNaN(Parent.LegendSettings.ShapeWidth) ? Parent.LegendSettings.ShapeWidth : 10)
                + (!double.IsNaN(Parent.LegendSettings.ShapePadding) ? Parent.LegendSettings.ShapePadding : 5));
        }

        private void RenderText(Legend legendOption, TextSetting textOptions)
        {
            textOptions = new TextSetting
            {
                Text = legendOption.Text,
                X = legendOption.LegendLocation.X + (Parent.LegendSettings.ShapeWidth / 2) + Parent.LegendSettings.ShapePadding,
                Y = legendOption.LegendLocation.Y + (LegendSetting.MaximumItemHeight / 4),
            };
            LegendSetting.LegendTextCollections.Add(textOptions);
        }

        private void RenderSymbol(Legend legendOption, int axisIndex, int rangeIndex)
        {
            legendOption.Fill = !string.IsNullOrEmpty(legendOption.Fill) ? legendOption.Fill : Parent.Axes[axisIndex].Ranges[rangeIndex].Color;
            Legend legendItem = new Legend
            {
                Fill = Parent.AxisRenderer.AxisCollection[axisIndex].RangeCollection[rangeIndex].RangeFillColor,
            };
            SizeD legendShapeSize = new SizeD
            {
                Width = Parent.LegendSettings.ShapeWidth,
                Height = Parent.LegendSettings.ShapeHeight,
            };
            if (Parent.LegendSettings.Shape == GaugeShape.Circle)
            {
                LegendSetting.LegendCircles.Add(new Circle
                {
                    RadiusX = legendShapeSize.Width / 2,
                    RadiusY = legendShapeSize.Height / 2,
                    CenterX = legendOption.LegendLocation.X,
                    CenterY = legendOption.LegendLocation.Y,
                });
            }
            else
            {
                LegendSetting.LegendShapePaths.Add(CalculateLegendShapes(legendOption.LegendLocation, Parent.LegendSettings.Shape, legendShapeSize));
            }

            LegendSetting.LegendColors.Add(legendItem);
            LegendSetting.LegendStroke = Parent.LegendSettings.ShapeBorder != null && !string.IsNullOrEmpty(Parent.LegendSettings.ShapeBorder.Color) ?
                Parent.LegendSettings.ShapeBorder.Color : "transparent";
            LegendSetting.LegendStrokeWidth = Parent.LegendSettings.ShapeBorder != null &&
                Parent.LegendSettings.ShapeBorder.Width != 0 ? Parent.LegendSettings.ShapeBorder.Width : 0;
            LegendSetting.LegendOpacity = !double.IsNaN(Parent.LegendSettings.Opacity) ? Parent.LegendSettings.Opacity : 1;
        }

        private Path CalculateLegendShapes(PointF location, GaugeShape shape, SizeD size)
        {
            Path shapePath = null;
            switch (shape)
            {
                case GaugeShape.Diamond:
                    shapePath = new Path()
                    {
                        ShapePath = "M" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + location.Y.ToString(culture) + SPACE +
                        "L" + SPACE + location.X.ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (size.Width / 2)).ToString(culture) + SPACE + location.Y.ToString(culture) + SPACE +
                        "L" + SPACE + location.X.ToString(culture) + SPACE + (location.Y + (size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + location.Y.ToString(culture) + " Z",
                    };
                    break;
                case GaugeShape.Rectangle:
                    shapePath = new Path()
                    {
                        ShapePath = "M" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (size.Width / 2)).ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (size.Width / 2)).ToString(culture) + SPACE + (location.Y + (size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y + (size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + " Z",
                    };
                    break;
                case GaugeShape.Triangle:
                    shapePath = new Path
                    {
                        ShapePath = "M" + SPACE + (location.X + ((-size.Width / 2) + (size.Width / 2))).ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + SPACE + "L" + SPACE + (location.X + (-size.Width / 2) + size.Width).ToString(culture) + SPACE +
                        (location.Y + (-size.Height / 2) + size.Height).ToString(culture) + "L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y + (-size.Height / 2) + size.Height).ToString(culture) + " Z",
                    };
                    break;
                case GaugeShape.InvertedTriangle:
                    shapePath = new Path
                    {
                        ShapePath = "M" + SPACE + (location.X + (-size.Width / 2) + size.Width).ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + SPACE + "L" + SPACE + (location.X + (-size.Width / 2) + (size.Width / 2)).ToString(culture) + SPACE + (location.Y + (-size.Height / 2) + size.Height).ToString(culture) +
                        "L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y + (-size.Height / 2)).ToString(culture) + " Z",
                    };
                    break;
                case GaugeShape.Image:
                    shapePath = new Path
                    {
                        ImagePath = new Rect()
                        {
                            Height = size.Height,
                            Width = size.Width,
                            X = location.X + (-size.Width / 2),
                            Y = location.Y + (-size.Height / 2),
                        },
                    };
                    break;
            }

            return shapePath;
        }

        private Path CalculateLegendPageShapes(PointF location, string shape, SizeD size)
        {
            double space = 2;
            Path shapePath = null;
            if (shape == "RightArrow")
            {
                shapePath = new Path
                {
                    ShapePath = "M" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y - (size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (size.Width / 2)).ToString(culture) + SPACE + location.Y.ToString(culture) + SPACE + "L" + SPACE +
                        (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y + (size.Height / 2)).ToString(culture) + " L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE +
                        (location.Y + (size.Height / 2) - space).ToString(culture) + SPACE + "L" + SPACE + (location.X + (size.Width / 2) - (2 * space)).ToString(culture) + SPACE + location.Y.ToString(culture) +
                        " L" + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + (location.Y - (size.Height / 2) + space).ToString(culture) + " Z",
                };
            }
            else
            {
                shapePath = new Path
                {
                    ShapePath = "M" + SPACE + (location.X + (size.Width / 2)).ToString(culture) + SPACE + (location.Y - (size.Height / 2)).ToString(culture) + SPACE +
                        "L" + SPACE + (location.X + (-size.Width / 2)).ToString(culture) + SPACE + location.Y.ToString(culture) + SPACE + "L" + SPACE +
                        (location.X + (size.Width / 2)).ToString(culture) + SPACE + (location.Y + (size.Height / 2)).ToString(culture) + SPACE + "L" + SPACE +
                        (location.X + (size.Width / 2)).ToString(culture) + SPACE + (location.Y + (size.Height / 2) - space).ToString(culture) + " L" + SPACE + (location.X + (-size.Width / 2) + (2 * space)).ToString(culture)
                        + SPACE + location.Y.ToString(culture) + " L" + (location.X + (size.Width / 2)).ToString(culture) + SPACE + (location.Y - (size.Height / 2) + space).ToString(culture) + " Z",
                };
            }

            return shapePath;
        }

        private double AlignLegend(double start, double size, double legendSize)
        {
            if (Parent.LegendSettings.Alignment == Alignment.Far)
            {
                start = (size - legendSize) - start;
            }
            else if (Parent.LegendSettings.Alignment == Alignment.Center)
            {
                start = (size - legendSize) / 2;
            }

            return start;
        }
    }
}