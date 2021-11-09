using System;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class BaseLegend
    {
        public BaseLegend(ChartThemeStyle theme)
        {
            themeStyle = theme;
        }

        internal static ChartHelper Helper { get; set; } = new ChartHelper();

        internal List<LegendOption> LegendCollection { get; set; } = new List<LegendOption>();

        internal LegendPosition Position { get; set; }

        internal double TotalPages { get; set; }

        internal Rect LegendBounds { get; set; } = new Rect();

        protected bool IsVertical { get; set; }

        protected double MaxItemHeight { get; set; }

        protected bool IsPaging { get; set; }

#pragma warning disable CA2227
        protected List<double> PageXCollections { get; set; } = new List<double>();

        protected List<Rect> PagingRegions { get; set; } = new List<Rect>();

#pragma warning restore CA2227

        protected string LegendID { get; set; }

        protected double MaxWidth { get; set; }

        protected double MaxColumns { get; set; }

        private double PageButtonSize { get; set; } = 8;

        protected double ClipPathHeight { get; set; }

        protected double TotalNoOfPages { get; set; }

        protected double CurrentPageNumber { get; set; } = 1;

        protected double CurrentPage { get; set; } = 1;

        protected string PageUpID { get; set; }

        protected string PageDownID { get; set; }

        protected string PageNumberID { get; set; }

        protected string LegendTranslateID { get; set; }

        protected bool EnableCanvas { get; set; }

        protected ILegendBaseMethods BaseLegendRef { get; set; }

        protected string Transform { get; set; } = string.Empty;

        protected double TotalPageCount { get; set; }

        private ChartThemeStyle themeStyle { get; set; }

        private double rowCount { get; set; }

        private bool calTotalPage { get; set; }

        private string accessbilityText { get; set; }

        private string baseControl { get; set; } = string.Empty;

        protected CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        /// Enable / Disable the legend inverse.
        /// </summary>
        protected bool IsInverse { get; set; }

        /// <summary>
        /// Specifies the legend textStyle.
        /// </summary>
        protected ChartFontOptions LegendTextStyle { get; set; }

        /// <summary>
        /// Specifies the legend border width.
        /// </summary>
        protected double BorderWidth { get; set; }

        private static ChartFontOptions GetFontOptions(ChartCommonFont font)
        {
            return new ChartFontOptions { Color = font.Color, Size = font.Size, FontFamily = font.FontFamily, FontWeight = font.FontWeight, FontStyle = font.FontStyle, TextAlignment = font.TextAlignment, TextOverflow = font.TextOverflow };
        }

        internal void CalculateLegendBounds(Rect rect, Size availableSize, ILegend legendSettings, ChartCommonMargin chartMargin, string baseComponent, string seriesType)
        {
            baseControl = baseComponent;
            LegendID = BaseLegendRef.ChartID + "_chart_legend";
            PageUpID = LegendID + "_pageup";
            PageDownID = LegendID + "_pagedown";
            PageNumberID = LegendID + "_pagenumber";
            LegendTranslateID = LegendID + "_translate_g";
            GetPosition(legendSettings.Position, availableSize, baseComponent, seriesType);
            LegendBounds = new Rect() { X = rect.X, Y = rect.Y, Width = 0, Height = 0 };
            string defaultValue = (baseControl == "BulletChart") ? "40%" : "20%";
            IsVertical = this.Position == LegendPosition.Left || this.Position == LegendPosition.Right;
            if (IsVertical)
            {
                LegendBounds.Height = DataVizCommonHelper.StringToNumber(legendSettings.Height, availableSize.Height - (rect.Y - chartMargin.Top));
                LegendBounds.Height = !double.IsNaN(LegendBounds.Height) ? LegendBounds.Height : rect.Height;
                LegendBounds.Width = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(legendSettings.Width) ? legendSettings.Width : defaultValue, availableSize.Width);
            }
            else
            {
                LegendBounds.Width = DataVizCommonHelper.StringToNumber(legendSettings.Width, availableSize.Width);
                LegendBounds.Width = !double.IsNaN(LegendBounds.Width) ? LegendBounds.Width : rect.Width;
                LegendBounds.Height = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(legendSettings.Height) ? legendSettings.Height : defaultValue, availableSize.Height);
            }

            BaseLegendRef.GetLegendBounds(availableSize, LegendBounds, rect, null);
        }

        private void GetPosition(LegendPosition position, Size availableSize, string baseControl, string seriesType)
        {
            if (baseControl == "Chart")
            {
                this.Position = (position != LegendPosition.Auto) ? position : LegendPosition.Bottom;
            }
            else
            {
                if (position == LegendPosition.Auto && (seriesType == "Funnel" || seriesType == "Pyramid"))
                {
                    position = LegendPosition.Top;
                }

                this.Position = (position != LegendPosition.Auto) ? position : (availableSize.Width > availableSize.Height ? LegendPosition.Right : LegendPosition.Bottom);
            }
        }

        internal void GetLocation(ILegend legendSettings, Rect legendBounds, Rect rect, Size availableSize, ChartCommonMargin legendMargin, ChartCommonMargin chartMargin, ChartCommonBorder legendBorder, ChartCommonLocation location)
        {
            double padding = legendBorder.Width, legendHeight = legendBounds.Height + padding + legendMargin.Top + legendMargin.Bottom,
            legendWidth = legendBounds.Width + padding + legendMargin.Left + legendMargin.Right, marginBottom = chartMargin.Bottom;
            if (Position == LegendPosition.Bottom)
            {
                legendBounds.X = AlignLegend(legendBounds.X, availableSize.Width, legendBounds.Width, legendSettings.Alignment);
                legendBounds.Y = rect.Y + (rect.Height - legendHeight) + padding + legendMargin.Top;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, 0, legendHeight));
            }
            else if (Position == LegendPosition.Top)
            {
                legendBounds.X = AlignLegend(legendBounds.X, availableSize.Width, legendBounds.Width, legendSettings.Alignment);
                legendBounds.Y = rect.Y + padding + legendMargin.Top;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, legendHeight, 0));
            }
            else if (Position == LegendPosition.Right)
            {
                legendBounds.X = rect.X + (rect.Width - legendBounds.Width) - legendMargin.Right;
                legendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + chartMargin.Bottom), legendBounds.Height, legendSettings.Alignment);
                ChartHelper.SubtractThickness(rect, new Thickness(0, legendWidth, 0, 0));
            }
            else if (Position == LegendPosition.Left)
            {
                legendBounds.X = legendBounds.X + legendMargin.Left;
                legendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + chartMargin.Bottom), legendBounds.Height, legendSettings.Alignment);
                ChartHelper.SubtractThickness(rect, new Thickness(legendWidth, 0, 0, 0));
            }
            else
            {
                legendBounds.X = location.X;
                legendBounds.Y = location.Y;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, 0, 0));
            }
        }

        private static int FindFirstLegendPosition(List<LegendOption> legendCollection)
        {
            int count = 0;
            foreach (LegendOption Legend in legendCollection)
            {
                if (Legend.Render && !string.IsNullOrEmpty(Legend.Text))
                {
                    break;
                }

                count++;
            }

            return count;
        }

        private static double AlignLegend(double start, double size, double legendSize, Alignment alignment)
        {
            if (alignment == Alignment.Far)
            {
                start = size - legendSize - start;
            }
            else if (alignment == Alignment.Center)
            {
                start = (size - legendSize) / 2;
            }

            return start;
        }

        protected void SetBounds(double computedWidth, double computedHeight, ILegend legend)
        {
            computedWidth = computedWidth < LegendBounds.Width ? computedWidth : LegendBounds.Width;
            computedHeight = computedHeight < LegendBounds.Height ? computedHeight : LegendBounds.Height;
#pragma warning disable CA1062
            LegendBounds.Width = string.IsNullOrEmpty(legend.Width) ? computedWidth : LegendBounds.Width;
            LegendBounds.Height = string.IsNullOrEmpty(legend.Height) ? computedHeight : LegendBounds.Height;
            rowCount = Math.Max(1, Math.Ceiling((LegendBounds.Height - legend.Padding) / (MaxItemHeight + legend.Padding)));
        }

        internal void RenderLegend(RenderTreeBuilder builder, SvgRendering svgRenderer, ILegend legend, Rect legendBounds, ChartCommonBorder border, ChartCommonFont legendTextStyle, [Optional] List<ChartSeries> VisibleSeries, [Optional] List<AccumulationChartSeries> accVisibleSeries)
        {
            int FirstLegend = FindFirstLegendPosition(LegendCollection);
            MaxItemHeight = Math.Max(LegendCollection[0].TextSize.Height, legend.ShapeHeight);
            svgRenderer?.OpenGroupElement(builder, LegendID + "_g");
            if (FirstLegend != LegendCollection.Count)
            {
                CreateLegendElements(builder, svgRenderer, border, legendBounds, legend, LegendID);
                double x_Location = (!IsInverse) ? legendBounds.X + legend.Padding + (legend.ShapeWidth / 2) : ((baseControl == "AccumulationChart" && IsVertical) ? legendBounds.X + MaxWidth : legendBounds.X + legendBounds.Width ) - (legend.Padding + (legend.ShapeWidth / 2));
                ChartInternalLocation Start = new ChartInternalLocation(x_Location, legendBounds.Y + legend.Padding + MaxItemHeight / 2);
                TextOptions TextOption = new TextOptions() { Id = string.Empty, X = Convert.ToString(Start.X, culture), Y = Convert.ToString(Start.Y, culture), TextAnchor = (!IsInverse) ? "start" : "end", Fill = themeStyle.LegendLabel };
                TotalPages = TotalPages = baseControl != "AccumulationChart" ? TotalPages : 0;
                double TextPadding = legend.ShapePadding + legend.Padding + legend.ShapeWidth;
                int count = 0;
                LegendCollection[FirstLegend].Location = Start;
                LegendOption PreviousLegend = LegendCollection[FirstLegend];
                string[] Access = new string[] { "Legend of bullet chart", "Click to show or hide the ", " series" };
                foreach (LegendOption legendOption in LegendCollection)
                {
                    if (baseControl == "AccumulationChart")
                    {
                        legendOption.Fill = accVisibleSeries[0].Points[(int)legendOption.PointIndex].Color;
                    }

                    accessbilityText = !string.IsNullOrEmpty(legend.Description) ? legend.Description : (baseControl == "BulletChart") ? Access[0] + string.Empty + legendOption.Text : Access[1] + legendOption.Text + Access[2];
                    if (legendOption.Render && !string.IsNullOrEmpty(legendOption.Text))
                    {
                        svgRenderer.OpenGroupElement(builder, LegendID + GenerateId(legendOption, "_g_", count), string.Empty, string.Empty, "cursor: " + (!legend.ToggleVisibility ? "default" : "pointer"), !legend.ToggleVisibility ? string.Empty : legend.TabIndex.ToString(culture), accessbilityText);
                        BaseLegendRef.GetRenderPoint(legendOption, Start, TextPadding, PreviousLegend, legendBounds, count, FirstLegend);
                        RenderSymbol(builder, svgRenderer, legendOption, count, legend, VisibleSeries, accVisibleSeries);
                        RenderText(builder, svgRenderer, legend, legendTextStyle, legendOption, TextOption, count);
                        builder.CloseElement();
                        PreviousLegend = legendOption;
                    }

                    count++;
                }

                builder.CloseElement();
                builder.CloseElement();
                if (IsPaging)
                {
                    RenderPagingElements(builder, svgRenderer, legend, legendBounds, TextOption, legendTextStyle);
                }
                else
                {
                    TotalPages = 1;
                }
            }

            builder.CloseElement();
        }

        private void RenderSymbol(RenderTreeBuilder builder, SvgRendering svgRenderer, LegendOption legendOption, int i, ILegend legendSettings, [Optional] List<ChartSeries> VisibleSeries, [Optional] List<AccumulationChartSeries> accVisibleSeries)
        {
            string borderColor = string.Empty, shape, symbolColor = legendOption.Visible ? legendOption.Fill : "#D3D3D3";
            if (baseControl == "Chart")
            {
                shape = legendOption.Shape == LegendShape.SeriesType ? legendOption.Type : Convert.ToString(legendOption.Shape, null);
            }
            else
            {
                shape = legendOption.Shape == LegendShape.SeriesType ? Convert.ToString(legendOption.AccType, null) : Convert.ToString(legendOption.Shape, null);
            }

            shape = shape == "Scatter" ? Convert.ToString(legendOption.MarkerShape, null) : shape;
            bool isStrokeWidth = legendOption.Shape == LegendShape.SeriesType && VisibleSeries != null && legendOption.Type.ToString(culture).ToLower(culture).Contains("line", StringComparison.InvariantCulture) && !legendOption.Type.ToString(culture).ToLower(culture).Contains("area", StringComparison.InvariantCulture),
            isCustomBorder = legendOption.Type == ChartSeriesType.Scatter.ToString(culture) || legendOption.Type == ChartSeriesType.Bubble.ToString(culture);
            double strokewidth = isStrokeWidth ? VisibleSeries[i].Width : legendOption.Shape == LegendShape.Multiply ? 4 : 1;
            if (isCustomBorder && i < (baseControl == "Chart" ? VisibleSeries.Count : accVisibleSeries?.Count))
            {
                ChartCommonBorder SeriesBorder = baseControl != "Chart" ? accVisibleSeries?[i].Border : null;
                borderColor = !string.IsNullOrEmpty(SeriesBorder.Color) ? SeriesBorder.Color : symbolColor;
                strokewidth = !double.IsNaN(SeriesBorder.Width) ? SeriesBorder.Width : 1;
            }

            PathOptions SymbolOption = new PathOptions(LegendID + this.GenerateId(legendOption, "_shape_", i), string.Empty, string.Empty, strokewidth, symbolColor, 1, isCustomBorder ? borderColor : symbolColor);
            bool isCanvas = false;
            if (!isCanvas)
            {
                Helper.DrawSymbol(builder, svgRenderer, legendOption.Location, shape, new Size(legendSettings.ShapeWidth, legendSettings.ShapeHeight), string.Empty, SymbolOption, baseControl == "BulletChart");
            }

            if ((shape == "Line" && legendOption.MarkerVisibility && legendOption.MarkerShape != ChartShape.Image) || legendOption.AccType == "Doughnut")
            {
                SymbolOption.Id = this.LegendID + this.GenerateId(legendOption, "_shape_" + "marker_", i);
                shape = Convert.ToString(legendOption.AccType == "Doughnut" ? "Circle" : legendOption.MarkerShape.ToString(), null);
                SymbolOption.Fill = legendOption.AccType == "Doughnut" ? "#FFFFFF" : SymbolOption.Fill;
                if (!isCanvas)
                {
                    Helper.DrawSymbol(builder, svgRenderer, legendOption.Location, shape, new Size(legendSettings.ShapeWidth / 2, legendSettings.ShapeHeight / 2), string.Empty, SymbolOption, baseControl == "BulletChart");
                }
            }
        }

        protected void RenderText(RenderTreeBuilder builder, SvgRendering svgRenderer, ILegend legendSettings, ChartCommonFont legendTextStyle, LegendOption legendOption, TextOptions textOption, int i)
        {
            textOption.Id = LegendID + GenerateId(legendOption, "_text_", i);
            textOption.Text = legendOption.Text;
            double textWidth = (IsInverse) ? (legendOption.Text.Contains("...", StringComparison.InvariantCulture) ? ChartHelper.MeasureText(legendOption.Text, LegendTextStyle).Width : legendOption.TextSize.Width) : 0;
            string x_Loc = Convert.ToString((!IsInverse) ? legendOption.Location.X + (legendSettings?.ShapeWidth / 2) + legendSettings.ShapePadding : legendOption.Location.X - (textWidth + (legendSettings?.ShapeWidth / 2) + legendSettings.ShapePadding), culture);
            textOption.X = Convert.ToString(x_Loc, culture);
            textOption.Y = Convert.ToString(legendOption.Location.Y + (MaxItemHeight / 4), culture);
            ChartHelper.TextElement(builder, svgRenderer, textOption, GetFontOptions(legendTextStyle), legendOption.Visible ? ChartHelper.FindThemeColor(legendTextStyle.Color, themeStyle.LegendLabel) : "#D3D3D3", accessbilityText);
#pragma warning restore CA1062
        }

        private string GenerateId(LegendOption option, string prefix, int count)
        {
            return baseControl == "Chart" ? prefix + count : prefix + option.PointIndex;
        }

        private void CreateLegendElements(RenderTreeBuilder builder, SvgRendering svgRenderer, ChartCommonBorder legendBorder, Rect legendBounds, ILegend legend, string id)
        {
            string clipPath = id + "_clipPath";
            bool isVertical = baseControl == "AccumulationChart" && IsVertical;
            RectOptions Option = new RectOptions(id + "_element", legendBounds.X, legendBounds.Y, 0, legendBounds.Height, legendBorder.Width, legendBorder.Color, legend.Background);
            Option.Width = (isVertical && IsInverse) ? MaxWidth : legendBounds.Width;
            svgRenderer.RenderRect(builder, Option);
            svgRenderer.OpenClipPath(builder, clipPath);
            Option.Id = clipPath + "_rect";
            Option.Y += legend.Padding;
            Option.Width = (isVertical) ? MaxWidth - legend.Padding : legendBounds.Width;
            if (IsPaging)
            {
                Option.Height = Math.Max(1, rowCount - 1) * (MaxItemHeight + (legend.Padding - 1));
            }

            svgRenderer.RenderRect(builder, Option);
            builder.CloseElement();
            svgRenderer.OpenGroupElement(builder, id + "_collections", string.Empty, "url(#" + clipPath + ")");
            svgRenderer.OpenGroupElement(builder, LegendTranslateID, Transform);
        }

        private void RenderPagingElements(RenderTreeBuilder builder, SvgRendering svgRenderer, ILegend legend, Rect bounds, TextOptions textOption, ChartCommonFont legendTextStyle)
        {
            this.PagingRegions = new List<Rect>();
            bool isCanvas = false;
            if (baseControl != "AccumulationChart" || !IsVertical)
            {
                TotalPageCount = Math.Ceiling(TotalPages / Math.Max(1, rowCount - 1));
            }
            else
            {
                TotalPageCount = Math.Ceiling(TotalPages / MaxColumns);
            }

            PathOptions symbolOption = new PathOptions(PageUpID, string.Empty, string.Empty, 5, "#545454", 1, Constants.TRANSPARENT);
            ClipPathHeight = Math.Max(1, rowCount - 1) * (MaxItemHeight + legend.Padding);
            string transform = string.Empty;
            double iconSize = PageButtonSize, x = bounds.X + (iconSize / 2), y = bounds.Y + ClipPathHeight + ((bounds.Height - ClipPathHeight) / 2);
            Size size = ChartHelper.MeasureText(TotalPageCount + "/" + TotalPageCount, GetFontOptions(legendTextStyle));
            if (!isCanvas)
            {
                double transformX = IsInverse ? BorderWidth + (iconSize / 2) : bounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width);
                transform = "translate(" + transformX.ToString(culture) + ", " + 0 + ")";
            }
            else if (CurrentPageNumber == 1 && calTotalPage)
            {
                this.TotalNoOfPages = TotalPageCount;
                calTotalPage = false;
            }

            svgRenderer.OpenGroupElement(builder, LegendID + "_navigation", transform, string.Empty, "cursor: pointer");
            if (!isCanvas)
            {
                Helper.DrawSymbol(builder, svgRenderer, new ChartInternalLocation(x, y), "LeftArrow", new Size(iconSize, iconSize), string.Empty, symbolOption);
            }

            PagingRegions.Add(new Rect(x + bounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width) - (iconSize * 0.5), y - (iconSize * 0.5), iconSize, iconSize));
            textOption.X = Convert.ToString(x + (iconSize / 2) + 8, culture);
            textOption.Y = Convert.ToString(y + (size.Height / 4), culture);
            textOption.Id = PageNumberID;
            textOption.Text = CurrentPageNumber + "/" + TotalPageCount;
            ChartHelper.TextElement(builder, svgRenderer, textOption, GetFontOptions(legendTextStyle), textOption.Fill, string.Empty);
            x = Convert.ToDouble(textOption.X, null) + 8 + (iconSize / 2) + size.Width;
            symbolOption.Id = PageDownID;
            if (!isCanvas)
            {
                Helper.DrawSymbol(builder, svgRenderer, new ChartInternalLocation(x, y), "RightArrow", new Size(iconSize, iconSize), string.Empty, symbolOption);
            }

            PagingRegions.Add(new Rect(x + (bounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width) - (iconSize * 0.5)), y - (iconSize * 0.5), iconSize, iconSize));
            builder.CloseElement();
        }
    }
}