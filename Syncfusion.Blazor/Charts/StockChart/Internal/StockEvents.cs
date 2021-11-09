using System;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    internal class StockEvents : ChartData
    {
        private const string STOCKEVENTS = "_StockEvents";
        private const string STOCKEVENTSID = "_StockEvents_";
        private const string SERIESID = "_Series_";
        private const string SPACE = " ";
        private const string MOVETO = "M ";
        private const string LINETO = "L ";
        private const string ENDPATH = " Z";
        private const double TEXTPADDING = 8;
        private const double ARROWPADDING = 12;
        private string chartId;
        private int seriesIndex;
        private int pointIndex;
        private SfStockChart stockChart;
        private ChartHelper helper;
        private SvgRendering renderer;
        private SVGTooltip stockEventTooltip;
        private List<List<ChartInternalLocation>> symbolLocations;
        private DomRect svgOffset;

        public StockEvents(SfStockChart sfstockchart)
            : base(sfstockchart.ChartSettings)
        {
            stockChart = sfstockchart;
            chartId = sfstockchart.ID;
            renderer = stockChart.SvgRenderer;
            helper = new ChartHelper();
        }

        private static ChartInternalLocation FindClosePoint(ChartSeriesRenderer seriesRenderer, StockChartStockEvent stockEvent)
        {
            DateTime stockEventDate = stockEvent.Date;
            double closeIndex = GetClosest(seriesRenderer, ChartHelper.GetTime(stockEventDate));
            PointData pointData = new PointData();
            Point point;
            double xpixel, ypixel;
            for (int k = 0; k < seriesRenderer.Points.Count; k++)
            {
                point = seriesRenderer.Points[k];
                if (closeIndex == point.XValue && point.Visible)
                {
                    pointData = new PointData(point, seriesRenderer.Series);
                }
                else if (k != 0 && k != seriesRenderer.Points.Count)
                {
                    if (closeIndex > seriesRenderer.Points[k - 1].XValue && closeIndex < seriesRenderer.Points[k + 1].XValue)
                    {
                        pointData = new PointData(point, seriesRenderer.Series);
                    }
                }
            }

            if (double.IsNaN(closeIndex))
            {
                return null;
            }

            xpixel = seriesRenderer.XAxisRenderer.Rect.X + (ChartHelper.ValueToCoefficient(pointData.Point.XValue, seriesRenderer.XAxisRenderer) * seriesRenderer.XAxisRenderer.Rect.Width);
            double yvalue = Convert.ToDouble(pointData.Point.YValue, null);
            ypixel = ChartHelper.ValueToCoefficient(yvalue, seriesRenderer.YAxisRenderer) * seriesRenderer.YAxisRenderer.Rect.Height;
            ypixel = (ypixel * -1) + (seriesRenderer.YAxisRenderer.Rect.Y + seriesRenderer.YAxisRenderer.Rect.Height);
            return new ChartInternalLocation(xpixel, ypixel);
        }

        private static T GetElementById<T>(List<T> elementList, string id)
        {
            PropertyInfo propertyInfo = elementList[0].GetType().GetProperty("Id");
            return elementList.Find(item => (string)propertyInfo.GetValue(item) == id);
        }

        private static string FindArrowPaths(FlagType type)
        {
            switch (type)
            {
                case FlagType.ArrowUp:
                    return "l -10 10 l 5 0 l 0 10 l 10 0 l 0 -10 l 5 0 z";
                case FlagType.ArrowDown:
                    return "l -10 -10 l 5 0 l 0 -10 l 10 0 l 0 10 l 5 0 z";
                case FlagType.ArrowLeft:
                    return "l -10 -10 l 0 5 l -10 0 l 0 10 l 10 0 l 0 5 z";
                case FlagType.ArrowRight:
                    return "l 10 -10 l 0 5 l 10 0 l 0 10 l -10 0 l 0 5 z";
            }

            return string.Empty;
        }

        private static List<List<ChartInternalLocation>> InitialArray(int numRows, int numCols, ChartInternalLocation initial)
        {
            List<List<ChartInternalLocation>> locations = new List<List<ChartInternalLocation>>(numRows);
            for (int i = 0; i < numRows; ++i)
            {
                List<ChartInternalLocation> columns = new List<ChartInternalLocation>(numCols);
                for (int j = 0; j < numCols; ++j)
                {
                    columns.Add(initial);
                }

                locations.Add(columns);
            }

            return locations;
        }

        internal void RenderStockEvents(RenderTreeBuilder builder)
        {
            StockChartStockEvent stockEvent;
            Size textSize;
            renderer.OpenGroupElement(builder, chartId + STOCKEVENTS);
            symbolLocations = InitialArray(stockChart.Series.Count, stockChart.StockEvents.Count, new ChartInternalLocation(0, 0));
            for (int i = 0; i < stockChart.StockEvents.Count; i++)
            {
                stockEvent = stockChart.StockEvents[i];
                foreach (ChartSeriesRenderer seriesRenderer in stockChart.ChartSettings.SeriesContainer.Renderers)
                {
                    textSize = ChartHelper.MeasureText(stockEvent.Text + 'W', stockEvent.TextStyle.GetCommonFont());
                    renderer.OpenGroupElement(builder, chartId + SERIESID + seriesRenderer.Index + STOCKEVENTSID + i);
                    DateTime stockEventDate = stockEvent.Date;
                    if (seriesRenderer.XAxisRenderer != null && ChartHelper.WithIn(ChartHelper.GetTime(stockEventDate), seriesRenderer.XAxisRenderer.VisibleRange))
                    {
                        if (stockEvent.SeriesIndexes != null && stockEvent.SeriesIndexes.Length > 0)
                        {
                            for (int j = 0; j < stockEvent.SeriesIndexes.Length; j++)
                            {
                                if (stockEvent.SeriesIndexes[j] == seriesRenderer.Index)
                                {
                                    CreatEventGroup(builder, seriesRenderer, stockEvent, i, textSize);
                                }
                            }
                        }
                        else
                        {
                            CreatEventGroup(builder, seriesRenderer, stockEvent, i, textSize);
                        }
                    }

                    builder.CloseElement();
                }
            }

            builder.CloseElement();
        }

        private void CreatEventGroup(RenderTreeBuilder builder, ChartSeriesRenderer seriesRenderer, StockChartStockEvent stockEvent, int i, Size textSize)
        {
            ChartInternalLocation symbolLocation = FindClosePoint(seriesRenderer, stockEvent);
            if (symbolLocation == null)
            {
                return;
            }

            if (!stockEvent.ShowOnSeries)
            {
                symbolLocation.Y = seriesRenderer.YAxisRenderer.Rect.Y + seriesRenderer.YAxisRenderer.Rect.Height;
            }

            symbolLocations[seriesRenderer.Index][i] = symbolLocation;
            CreateStockElements(builder, stockEvent, seriesRenderer, i, symbolLocation, textSize);
        }

        internal async void RemoveStockEventTooltip()
        {
            await stockChart.InvokeMethod(StockChartConstants.REMOVETOOLTIP, new object[] { stockChart.Element });
        }

        internal async void RenderStockEventTooltip(string targetId)
        {
            int seriesIndex = int.Parse(targetId.Split(STOCKEVENTSID)[0].Split(chartId + SERIESID)[1], null),
            pointIndex = int.Parse(targetId.Split(STOCKEVENTSID)[1].Split("_")[0], null);
            svgOffset = await stockChart.InvokeMethod<DomRect>(Constants.GETELEMENTBOUNDSBYID, false, new object[] { stockChart.GetId("_svg") });
            Rect bounds = stockChart.ChartSettings.AxisContainer.AxisLayout.SeriesClipRect;
            ChartInternalLocation updatedLocation = symbolLocations[seriesIndex][pointIndex],
            pointLocation = new ChartInternalLocation(
                updatedLocation.X - bounds.X,
                updatedLocation.Y + stockChart.GetSeriesTooltipTop() - bounds.Y);
            ApplyHighlights(pointIndex, seriesIndex);
            ChartDefaultFont textStyle = new ChartDefaultFont();
            stockEventTooltip = new SVGTooltip()
            {
                Opacity = 1,
                Header = string.Empty,
                EnableAnimation = true,
                Inverted = true,
                Content = new string[] { stockChart.StockEvents[pointIndex].Description },
                Shapes = Array.Empty<TooltipShape>(),
                ArrowPadding = ARROWPADDING,
                Location = new ToolLocationModel { X = pointLocation.X, Y = pointLocation.Y },
                Theme = stockChart.Theme.ToString(),
                ClipBounds = new ToolLocationModel() { X = bounds.X, Y = bounds.Y },
                TextStyle = new TextStyleModel() { Size = textStyle.Size, Color = textStyle.Color, FontFamily = textStyle.FontFamily, FontStyle = textStyle.FontStyle, FontWeight = textStyle.FontWeight, Opacity = textStyle.Opacity },
                AreaBounds = new AreaBoundsModel { X = bounds.X, Y = bounds.Y, Height = bounds.Height + stockChart.GetSeriesTooltipTop(), Width = bounds.Width },
            };
            string options = JsonSerializer.Serialize(stockEventTooltip);
            await stockChart.InvokeMethod(StockChartConstants.RENDERTOOLTIP, new object[] { options, stockChart.GetId(StockChartConstants.STOCKEVENTSTOOLTIP), DotNetObjectReference.Create<object>(this), stockChart.Element });
        }

        private void ApplyHighlights(int pointIndex, int seriesIndex)
        {
            if (this.pointIndex != pointIndex || this.seriesIndex != seriesIndex)
            {
                RemoveHighlights();
            }

            this.pointIndex = pointIndex;
            this.seriesIndex = seriesIndex;
            string stockId = chartId + SERIESID + seriesIndex + STOCKEVENTSID + pointIndex;
            SetOpacity(stockId + "_Shape", 0.5);
            SetTextOpacity(stockId + "_Text", 0.5);
        }

        private void RemoveHighlights()
        {
            string stockId = chartId + SERIESID + seriesIndex + STOCKEVENTSID + pointIndex;
            SetOpacity(stockId + "_Shape", 1);
            SetTextOpacity(stockId + "_Text", 1);
        }

        private void SetTextOpacity(string elementId, double opacity)
        {
            SvgText textEle = GetElementById(renderer.TextElementList, elementId);
            if (textEle != null)
            {
                textEle.ChangeOpacity(opacity);
            }
        }

        private void SetOpacity(string elementId, double opacity)
        {
            SvgPath pathEle = GetElementById(renderer.PathElementList, elementId);
            if (pathEle != null)
            {
                pathEle.ChangeOpacity(opacity);
            }

            SvgEllipse ellipseEle = GetElementById(renderer.EllipseElementList, elementId);
            if (ellipseEle != null)
            {
                ellipseEle.ChangeOpacity(opacity);
            }
        }

        private void CreateStockElements(RenderTreeBuilder builder, StockChartStockEvent stockEvent, ChartSeriesRenderer seriesRenderer, int i, ChartInternalLocation symbolLocation, Size textSize)
        {
            Size result = new Size(textSize.Width > 20 ? textSize.Width : 20, textSize.Height > 20 ? textSize.Height : 20);
            string pathString = string.Empty;
            System.Globalization.CultureInfo culture = stockChart.Culture;
            PathOptions pathOption;
            double lx = symbolLocation.X, ly = symbolLocation.Y;
            string stockId = chartId + SERIESID + seriesRenderer.Index + STOCKEVENTSID + i;
            TextOptions textOption = new TextOptions
            {
                Id = stockId + "_Text",
                X = stockEvent.Type != FlagType.Flag ? Convert.ToString(lx, culture) : Convert.ToString(lx + (result.Width / 2), stockChart.Culture),
                Y = Convert.ToString(ly - result.Height, stockChart.Culture),
                TextAnchor = "middle",
                AccessibilityText = stockEvent.Description,
                DominantBaseline = "middle",
                Fill = stockEvent.TextStyle.Color,
                Text = stockEvent.Text
            };
            StockChartStockEventsBorder border = stockEvent.Border;
            switch (stockEvent.Type)
            {
                case FlagType.Flag:
                case FlagType.Circle:
                case FlagType.Square:
                    helper.DrawSymbol(builder, renderer, new ChartInternalLocation(lx, ly), "Circle", new Size(2, 2), string.Empty, new PathOptions(stockId + "_Circle", string.Empty, string.Empty, border.Width, border.Color));
                    helper.DrawSymbol(builder, renderer, new ChartInternalLocation(lx, ly - 5), "VerticalLine", new Size(9, 9), string.Empty, new PathOptions(stockId + "_VerticalLine", string.Empty, string.Empty, border.Width, border.Color));
                    helper.DrawSymbol(builder, renderer, new ChartInternalLocation(stockEvent.Type != FlagType.Flag ? lx : lx + (result.Width / 2), ly - result.Height), stockEvent.Type.ToString(), result, string.Empty, new PathOptions(stockId + "_Shape", string.Empty, string.Empty, border.Width, border.Color, 1, stockEvent.Background));
                    ChartHelper.TextElement(builder, renderer, textOption);
                    break;
                case FlagType.ArrowUp:
                case FlagType.ArrowDown:
                case FlagType.ArrowRight:
                case FlagType.ArrowLeft:
                    pathString = 'M' + SPACE + Convert.ToString(lx, culture) + SPACE + Convert.ToString(ly, culture) + SPACE + FindArrowPaths(stockEvent.Type);
                    pathOption = new PathOptions(stockId + "_Shape", pathString, string.Empty, border.Width, border.Color, 1, stockEvent.Background);
                    renderer.RenderPath(builder, pathOption);
                    break;
                case FlagType.Triangle:
                case FlagType.InvertedTriangle:
                    result.Height = 3 * textSize.Height;
                    result.Width = textSize.Width + (1.5 * textSize.Width);
                    helper.DrawSymbol(builder, renderer, new ChartInternalLocation(lx, ly), stockEvent.Type.ToString(), new Size(20, 20), string.Empty, new PathOptions(stockId + "_Shape", string.Empty, string.Empty, border.Width, border.Color, 1, stockEvent.Background));
                    ChartHelper.TextElement(builder, renderer, textOption);
                    break;
                case FlagType.Text:
                    textSize.Height += TEXTPADDING;
                    pathString = MOVETO + Convert.ToString(lx, culture) + SPACE + Convert.ToString(ly, culture) + SPACE +
                    LINETO + Convert.ToString(lx - 5, culture) + SPACE + Convert.ToString(ly - 5, culture) + SPACE +
                    LINETO + Convert.ToString(lx - (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - 5, culture) + SPACE +
                    LINETO + Convert.ToString(lx - (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - textSize.Height, culture) + SPACE +
                    LINETO + Convert.ToString(lx + (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - textSize.Height, culture) + SPACE +
                    LINETO + Convert.ToString(lx + (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - 5, culture) + SPACE +
                    LINETO + Convert.ToString(lx + 5, culture) + SPACE + Convert.ToString(ly - 5, culture) + ENDPATH;
                    pathOption = new PathOptions(stockId + "_Shape", pathString, string.Empty, border.Width, border.Color, 1, stockEvent.Background);
                    renderer.RenderPath(builder, pathOption);
                    renderer.RenderPath(builder, pathOption);
                    textOption.Y = Convert.ToString(ly - (textSize.Height / 2), stockChart.Culture);
                    ChartHelper.TextElement(builder, renderer, textOption);
                    break;
                default:
                    pathString = MOVETO + Convert.ToString(lx, culture) + SPACE + Convert.ToString(ly, culture) + SPACE +
                    LINETO + Convert.ToString(lx - (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - (textSize.Height / 3), culture) + SPACE +
                    LINETO + Convert.ToString(lx - (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - textSize.Height, culture) + SPACE +
                    LINETO + Convert.ToString(lx + (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - textSize.Height, culture) + SPACE +
                    LINETO + Convert.ToString(lx + (textSize.Width / 2), culture) + SPACE + Convert.ToString(ly - (textSize.Height / 3), culture) + ENDPATH;
                    pathOption = new PathOptions(stockId + "_Shape", pathString, string.Empty, border.Width, border.Color, 1, stockEvent.Background);
                    renderer.RenderPath(builder, pathOption);
                    textOption.Y = Convert.ToString(ly - (textSize.Height / 2), stockChart.Culture);
                    ChartHelper.TextElement(builder, renderer, textOption);
                    break;
            }
        }
    }
}