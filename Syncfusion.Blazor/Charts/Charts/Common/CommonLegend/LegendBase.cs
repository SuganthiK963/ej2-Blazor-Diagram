using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class LegendBase : ChartRenderer
    {
        protected List<LegendOption> LegendCollection { get; set; } = new List<LegendOption>();

        internal LegendPosition Position { get; set; }

        protected double TotalPages { get; set; }

        internal Rect LegendBounds { get; set; } = new Rect();

        protected bool IsVertical { get; set; }

        protected double MaxItemHeight { get; set; }

        protected bool IsPaging { get; set; }

        protected List<double> PageXCollections { get; set; } = new List<double>();

        protected List<Rect> PagingRegions { get; set; } = new List<Rect>();

        internal string LegendID { get; set; }

        protected double MaxWidth { get; set; }

        protected double MaxColumns { get; set; }

        private double PageButtonSize { get; set; } = 8;

        protected double ClipPathHeight { get; set; }

        protected double CurrentPageNumber { get; set; } = 1;

        protected string PageUpID { get; set; }

        protected string PageDownID { get; set; }

        protected string PageNumberID { get; set; }

        protected string LegendTranslateID { get; set; }

        protected ILegendMethods BaseLegendRef { get; set; }

        protected string Transform { get; set; } = string.Empty;

        protected double TotalPageCount { get; set; }

        private double rowCount { get; set; }

        protected ChartThemeStyle ThemeStyle { get; set; }

        private string baseControl { get; set; } = string.Empty;

        private string pagingTransform { get; set; } = string.Empty;

        protected CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal List<LegendSymbols> LegendOptions { get; set; } = new List<LegendSymbols>();

        protected List<LegendSymbols> PagingOptions { get; set; } = new List<LegendSymbols>();

        internal ILegendBase Legend { get; set; }

        protected string ChartId { get; set; }

        /// <summary>
        /// Enable / Disable the legend inverse.
        /// </summary>
        protected bool IsInverse { get; set; }

        /// <summary>
        /// Specifies the legend border width.
        /// </summary>
        protected double BorderWidth { get; set; }

        // This method moved and renamed from DrawSymbol in Charthelper
        internal static SymbolOptions CalculateSymbol(ChartInternalLocation location, string shape, Size size, string url, PathOptions option)
        {
            SymbolOptions shapeoption = ChartHelper.CalculateShapes(location, size, shape, url, option, false);
            if (shapeoption.ShapeName == ShapeName.path)
            {
                shapeoption.PathOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.ellipse)
            {
                shapeoption.EllipseOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.image)
            {
                shapeoption.ImageOption.Visibility = option.Visibility;
            }

            return shapeoption;
        }

        internal void CalculateRenderTreeBuilderOptions()
        {
            int FirstLegend = FindFirstLegendPosition();
            MaxItemHeight = Math.Max(LegendCollection[0].TextSize.Height, Legend.ShapeHeight);

            if (FirstLegend != LegendCollection.Count)
            {
                double x_Location = (!IsInverse) ? LegendBounds.X + Legend.Padding + (Legend.ShapeWidth / 2) : (LegendBounds.X + LegendBounds.Width) - (Legend.Padding + (Legend.ShapeWidth / 2));
                ChartInternalLocation Start = new ChartInternalLocation(x_Location, LegendBounds.Y + Legend.Padding + (MaxItemHeight / 2));
                TotalPages = baseControl != "AccumulationChart" ? TotalPages : 0;
                double TextPadding = Legend.ShapePadding + Legend.Padding + Legend.ShapeWidth;
                int count = 0;
                string legendLabel = ThemeStyle.LegendLabel;
                LegendCollection[FirstLegend].Location = Start;
                LegendOption PreviousLegend = LegendCollection[FirstLegend];
                string[] Access = new string[] { "Legend of bullet chart", "Click to show or hide the ", " series" };
                foreach (LegendOption legendOption in LegendCollection)
                {
                    if (legendOption.Render && !string.IsNullOrEmpty(legendOption.Text))
                    {
                        BaseLegendRef.GetRenderPoint(legendOption, Start, TextPadding, PreviousLegend, count, FirstLegend);
                        List<SymbolOptions> symbols = CalculateLegendOptions(legendOption, count);
                        LegendOptions.Add(new LegendSymbols() { 
                            FirstSymbol = symbols[0], SecondSymbol = symbols[1], 
                            TextOption = CalculateText(legendOption, count, legendLabel, string.IsNullOrEmpty(Legend.Description) ? Legend.Description : (baseControl == "BulletChart") ? Access[0] + string.Empty + legendOption.Text : Access[1] + legendOption.Text + Access[2]), 
                            Index = count
                        });
                        PreviousLegend = legendOption;
                    }

                    count++;
                }

                if (IsPaging)
                {
                    TextOptions textOption = new TextOptions() {
                        FontSize = LegendCollection[0].TextStyle.Size, FontFamily = LegendCollection[0].TextStyle.FontFamily,
                        FontStyle = LegendCollection[0].TextStyle.FontStyle, FontWeight = LegendCollection[0].TextStyle.FontWeight,
                        TextAnchor = IsInverse ? "end":"start",
                        Fill = Owner.ChartThemeStyle.LegendLabel
                    };
                    CalculatePagingElements(textOption);
                }
                else
                {
                    TotalPages = 1;
                }
            }
        }

        private void CalculatePagingElements(TextOptions textOption)
        {
            PagingRegions = new List<Rect>();
            ChartFontOptions font = new ChartFontOptions { Size = textOption.FontSize, Color = textOption.Fill, FontFamily = textOption.FontFamily, FontStyle = textOption.FontStyle, FontWeight = textOption.FontWeight };
            if (baseControl != "AccumulationChart" || !IsVertical)
            {
                TotalPageCount = Math.Ceiling(TotalPages / Math.Max(1, rowCount - 1));
            }
            else
            {
                TotalPageCount = Math.Ceiling(TotalPages / MaxColumns);
            }

            PathOptions symbolOption = new PathOptions(PageUpID, string.Empty, string.Empty, 5, "#545454", 1, Constants.TRANSPARENT);
            ClipPathHeight = Math.Max(1, rowCount - 1) * (MaxItemHeight + Legend.Padding);
            double iconSize = PageButtonSize, x = LegendBounds.X + (iconSize / 2), y = LegendBounds.Y + ClipPathHeight + ((LegendBounds.Height - ClipPathHeight) / 2);
            Size size = ChartHelper.MeasureText(TotalPageCount + "/" + TotalPageCount, font);
            double transformX = IsInverse ? BorderWidth + (iconSize / 2) : LegendBounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width);
            pagingTransform = "translate(" + transformX.ToString(culture) + ", " + 0 + ")";
            PagingOptions.Add(new LegendSymbols() { FirstSymbol = CalculateSymbol(new ChartInternalLocation(x, y), "LeftArrow", new Size(iconSize, iconSize), string.Empty, symbolOption) });
            PagingRegions.Add(new Rect(x + LegendBounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width) - (iconSize * 0.5), y - (iconSize * 0.5), iconSize, iconSize));
            textOption.X = Convert.ToString(x + (iconSize / 2) + 8, culture);
            textOption.Y = Convert.ToString(y + (size.Height / 4), culture);
            textOption.Id = PageNumberID;
            textOption.Text = CurrentPageNumber + "/" + TotalPageCount;
            PagingOptions[0].TextOption = textOption;
            x = Convert.ToDouble(textOption.X, null) + 8 + (iconSize / 2) + size.Width;
            symbolOption = new PathOptions(PageDownID, string.Empty, string.Empty, 5, Constants.TRANSPARENT, 1, "#545454");
            PagingOptions[0].SecondSymbol = CalculateSymbol(new ChartInternalLocation(x, y), "RightArrow", new Size(iconSize, iconSize), string.Empty, symbolOption);
            PagingRegions.Add(new Rect(x + (LegendBounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width) - (iconSize * 0.5)), y - (iconSize * 0.5), iconSize, iconSize));
        }

        internal string GenerateId(LegendOption option, string prefix, int count)
        {
            return baseControl == "Chart" ? prefix + count : prefix + option.PointIndex;
        }

        private void RenderSymbol(RenderTreeBuilder builder, SymbolOptions symbolOption)
        {
            if (symbolOption.ShapeName == ShapeName.ellipse)
            {
                Owner.SvgRenderer.RenderEllipse(builder, symbolOption.EllipseOption);
            }
            else if (symbolOption.ShapeName == ShapeName.path)
            {
                Owner.SvgRenderer.RenderPath(builder, symbolOption.PathOption);
            }
            else if (symbolOption.ShapeName == ShapeName.image)
            {
                Owner.SvgRenderer.RenderImage(builder, symbolOption.ImageOption);
            }
        }

        internal void RenderLegend(RenderTreeBuilder builder, SvgRendering svgRenderer, ChartDefaultBorder border)
        {
            CreateLegendElements(builder, svgRenderer, border);
            foreach (LegendSymbols legendOption in LegendOptions)
            {
                svgRenderer.OpenGroupElement(builder, LegendID + GenerateId(LegendCollection[0], "_g_", legendOption.Index), string.Empty, string.Empty, "cursor: " + (!Legend.ToggleVisibility ? "default" : "pointer"), !Legend.ToggleVisibility ? string.Empty : Legend.TabIndex.ToString(CultureInfo.InvariantCulture), "accessbilityText");
                if (legendOption.FirstSymbol != null)
                {
                    RenderSymbol(builder, legendOption.FirstSymbol);
                }

                if (legendOption.SecondSymbol != null)
                {
                    RenderSymbol(builder, legendOption.SecondSymbol);
                }

                if (legendOption.TextOption != null)
                {
                    svgRenderer.RenderText(builder, legendOption.TextOption);
                }

                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();
            if (IsPaging)
            {
                RenderPagingElements(builder, svgRenderer);
            }
        }

        private void RenderPagingElements(RenderTreeBuilder builder, SvgRendering svgRenderer)
        {
            svgRenderer.OpenGroupElement(builder, LegendID + "_navigation", pagingTransform, string.Empty, "cursor: pointer");
            foreach (LegendSymbols pagingOption in PagingOptions)
            {
                RenderSymbol(builder, pagingOption.FirstSymbol);
                RenderSymbol(builder, pagingOption.SecondSymbol);
                svgRenderer.RenderText(builder, pagingOption.TextOption);
            }

            builder.CloseElement();
        }

        protected TextOptions CalculateText(LegendOption legendOption, int i, string legendLabelColor, string accessbilityText)
        {
            if (legendOption == null)
            {
                return new TextOptions();
            }

            string fill = legendOption.Visible ? ChartHelper.FindThemeColor(legendOption.TextStyle.Color, legendLabelColor) : "#D3D3D3";
            double textWidth = IsInverse ? (legendOption.Text.Contains("...", StringComparison.InvariantCulture) ? ChartHelper.MeasureText(legendOption.Text, legendOption.TextStyle.GetChartFontOptions()).Width : legendOption.TextSize.Width) : 0;
            string x_Loc = Convert.ToString((!IsInverse) ? legendOption.Location.X + (Legend?.ShapeWidth / 2) + Legend.ShapePadding : legendOption.Location.X - (textWidth + (Legend?.ShapeWidth / 2) + Legend.ShapePadding), culture);
            return new TextOptions() {
                Id = LegendID + GenerateId(legendOption, "_text_", i), Text = legendOption.Text,
                X = x_Loc,
                Y = Convert.ToString(legendOption.Location.Y + (MaxItemHeight / 4), culture),
                Fill = !string.IsNullOrEmpty(fill) ? fill : "black",
                FontFamily = legendOption.TextStyle.FontFamily,
                FontSize = legendOption.TextStyle.Size, FontStyle = legendOption.TextStyle.FontStyle,
                FontWeight = legendOption.TextStyle.FontWeight, AccessibilityText = accessbilityText,
                TextAnchor = (!Owner.EnableRTL) ? string.Empty : "end"
            };
        }

        private void CreateLegendElements(RenderTreeBuilder builder, SvgRendering svgRenderer, ChartDefaultBorder legendBorder)
        {
            string clipPath = LegendID + "_clipPath";
            RectOptions Option = new RectOptions(LegendID + "_element", LegendBounds.X, LegendBounds.Y, LegendBounds.Width, LegendBounds.Height, legendBorder.Width, legendBorder.Color, Legend.Background, 0, 0, Legend.Opacity);
            svgRenderer.RenderRect(builder, Option);
            svgRenderer.OpenClipPath(builder, clipPath);
            Option.Id = clipPath + "_rect";
            Option.Y += Legend.Padding;
            Option.Width = (baseControl == "AccumulationChart" && IsVertical) ? MaxWidth - Legend.Padding : LegendBounds.Width;
            if (IsPaging)
            {
                Option.Height = Math.Max(1, rowCount - 1) * (MaxItemHeight + Legend.Padding);
            }

            svgRenderer.RenderRect(builder, Option);
            builder.CloseElement();
            svgRenderer.OpenGroupElement(builder, LegendID + "_collections", string.Empty, "url(#" + clipPath + ")");
            svgRenderer.OpenGroupElement(builder, LegendTranslateID, Transform);
        }

        internal List<SymbolOptions> CalculateLegendOptions(LegendOption legendOption, int i)
        {
            SymbolOptions legendSymbol = null, legendMarkerSymbol = null;
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
            bool isStrokeWidth = legendOption.Shape == LegendShape.SeriesType && legendOption.Type.ToString(culture).ToLower(culture).Contains("line", StringComparison.InvariantCulture) && !legendOption.Type.ToString(culture).ToLower(culture).Contains("area", StringComparison.InvariantCulture),
            isCustomBorder = legendOption.Type == ChartSeriesType.Scatter.ToString() || legendOption.Type == ChartSeriesType.Bubble.ToString();
            double strokewidth = isStrokeWidth ? legendOption.SeriesWidth : 1;
            if (isCustomBorder)
            {
                borderColor = !string.IsNullOrEmpty(legendOption.SeriesBorderColor) ? legendOption.SeriesBorderColor : symbolColor;
                strokewidth = !double.IsNaN(legendOption.SeriesBorderWidth) ? legendOption.SeriesBorderWidth : 1;
            }

            PathOptions SymbolOption = new PathOptions(LegendID + GenerateId(legendOption, "_shape_", i), string.Empty, string.Empty, strokewidth, symbolColor, 1, isCustomBorder ? borderColor : symbolColor);
            legendSymbol = CalculateSymbol(legendOption.Location, shape, new Size(Legend.ShapeWidth, Legend.ShapeHeight), string.Empty, SymbolOption);

            if ((shape == "Line" && legendOption.MarkerVisibility && legendOption.MarkerShape != ChartShape.Image) || legendOption.AccType == "Doughnut")
            {
                shape = Convert.ToString(legendOption.AccType == "Doughnut" ? "Circle" : legendOption.MarkerShape.ToString(), null);
                PathOptions markerOption = new PathOptions(LegendID + GenerateId(legendOption, "_shape_" + "marker_", i), string.Empty, string.Empty, strokewidth, symbolColor, 1, legendOption.AccType == "Doughnut" ? "#FFFFFF" : SymbolOption.Fill);
                legendMarkerSymbol = CalculateSymbol(legendOption.Location, shape, new Size(Legend.ShapeWidth / 2, Legend.ShapeHeight / 2), string.Empty, markerOption);
            }

            return new List<SymbolOptions>() { legendSymbol, legendMarkerSymbol };
        }

        private int FindFirstLegendPosition()
        {
            int count = 0;
            foreach (LegendOption Legend in LegendCollection)
            {
                if (Legend.Render && !string.IsNullOrEmpty(Legend.Text))
                {
                    break;
                }

                count++;
            }

            return count;
        }

        internal void CalculateLegendBounds(Rect rect, Size availableSize, ChartDefaultMargin chartMargin, string baseComponent, string seriesType)
        {
            baseControl = baseComponent;
            LegendID = ChartId + "_chart_legend";
            PageUpID = LegendID + "_pageup";
            PageDownID = LegendID + "_pagedown";
            PageNumberID = LegendID + "_pagenumber";
            LegendTranslateID = LegendID + "_translate_g";
            GetPosition(availableSize, baseControl, seriesType);
            LegendBounds = new Rect() { X = rect.X, Y = rect.Y, Width = 0, Height = 0 };
            string DefaultValue = (baseControl == "BulletChart") ? "40%" : "20%";
            IsVertical = Position == LegendPosition.Left || Position == LegendPosition.Right;
            if (IsVertical)
            {
                LegendBounds.Height = DataVizCommonHelper.StringToNumber(Legend.Height, availableSize.Height - (rect.Y - chartMargin.Top));
                LegendBounds.Height = !double.IsNaN(LegendBounds.Height) ? LegendBounds.Height : rect.Height;
                LegendBounds.Width = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(Legend.Width) ? Legend.Width : DefaultValue, availableSize.Width);
            }
            else
            {
                LegendBounds.Width = DataVizCommonHelper.StringToNumber(Legend.Width, availableSize.Width);
                LegendBounds.Width = !double.IsNaN(LegendBounds.Width) ? LegendBounds.Width : rect.Width;
                LegendBounds.Height = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(Legend.Height) ? Legend.Height : DefaultValue, availableSize.Height);
            }

            BaseLegendRef.GetLegendBounds(availableSize, rect, null);
        }

        private void GetPosition(Size availableSize, string baseControl, string seriesType)
        {
            if (baseControl == "Chart")
            {
                Position = (Position != LegendPosition.Auto) ? Position : LegendPosition.Bottom;
            }
            else
            {
                if (Position == LegendPosition.Auto && (seriesType == "Funnel" || seriesType == "Pyramid"))
                {
                    Position = LegendPosition.Top;
                }

                Position = (Position != LegendPosition.Auto) ? Position : (availableSize.Width > availableSize.Height ? LegendPosition.Right : LegendPosition.Bottom);
            }
        }

        internal void GetLocation(Rect rect, Size availableSize, ChartDefaultMargin legendMargin, ChartDefaultMargin chartMargin, ChartDefaultBorder legendBorder, ChartDefaultLocation location)
        {
            double padding = legendBorder.Width, legendHeight = LegendBounds.Height + padding + legendMargin.Top + legendMargin.Bottom,
            legendWidth = LegendBounds.Width + padding + legendMargin.Left + legendMargin.Right, marginBottom = chartMargin.Bottom;
            if (Position == LegendPosition.Bottom)
            {
                LegendBounds.X = AlignLegend(LegendBounds.X, availableSize.Width, LegendBounds.Width, Legend.Alignment);
                LegendBounds.Y = rect.Y + (rect.Height - legendHeight) + padding + legendMargin.Top;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, 0, legendHeight));
            }
            else if (Position == LegendPosition.Top)
            {
                LegendBounds.X = AlignLegend(LegendBounds.X, availableSize.Width, LegendBounds.Width, Legend.Alignment);
                LegendBounds.Y = rect.Y + (padding / 2) + legendMargin.Top;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, legendHeight, 0));
            }
            else if (Position == LegendPosition.Right)
            {
                LegendBounds.X = rect.X + (rect.Width - LegendBounds.Width) - legendMargin.Right;
                LegendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + chartMargin.Bottom), LegendBounds.Height, Legend.Alignment);
                ChartHelper.SubtractThickness(rect, new Thickness(0, legendWidth, 0, 0));
            }
            else if (Position == LegendPosition.Left)
            {
                LegendBounds.X = LegendBounds.X + legendMargin.Left;
                LegendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + chartMargin.Bottom), LegendBounds.Height, Legend.Alignment);
                ChartHelper.SubtractThickness(rect, new Thickness(legendWidth, 0, 0, 0));
            }
            else
            {
                LegendBounds.X = location.X;
                LegendBounds.Y = location.Y;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, 0, 0));
            }
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

        internal void SetBounds(double computedWidth, double computedHeight)
        {
            computedWidth = computedWidth < LegendBounds.Width ? computedWidth : LegendBounds.Width;
            computedHeight = computedHeight < LegendBounds.Height ? computedHeight : LegendBounds.Height;
            LegendBounds.Width = string.IsNullOrEmpty(Legend.Width) ? computedWidth : LegendBounds.Width;
            LegendBounds.Height = string.IsNullOrEmpty(Legend.Height) ? computedHeight : LegendBounds.Height;
            rowCount = Math.Max(1, Math.Ceiling((LegendBounds.Height - Legend.Padding) / (MaxItemHeight + Legend.Padding)));
        }
    }
}
