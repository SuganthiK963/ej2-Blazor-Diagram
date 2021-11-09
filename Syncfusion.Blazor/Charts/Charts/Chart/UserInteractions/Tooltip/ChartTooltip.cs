using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using System.Timers;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    internal class ChartTooltip : TooltipBase
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private const string BREAKTAG = "<br/>";
        private Timer timer = new Timer();
        private Size elementSize;
        private bool isSharedRemove;
        private bool isDispose;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal ChartTooltip(SfChart sfchart)
            : base(sfchart)
        {
            Chart = sfchart;
        }

        private static string GetIndicatorTooltipFormat(ChartSeries series, string format)
        {
            if (series.Renderer.SeriesType() == SeriesValueType.XY)
            {
                return series.Name + " : <b>${point.y}</b>";
            }
            else
            {
                return format;
            }
        }

        private static bool IsSelected()
        {
            // TODO: Need to change after selection feature included.
            return false;
        }

        internal void MouseMoveHandler()
        {
            if (Chart.IsPointMouseDown)
            {
                RemoveTooltip(100);
                return;
            }

            if (!Chart.DisableTrackTooltip && !IsSelected())
            {
                if ((!Chart.Tooltip.Shared && (!Chart.IsTouch || Chart.StartMove)) || (ChartHelper.WithInBounds(Chart.MouseX, Chart.MouseY, Chart.AxisContainer.AxisLayout.SeriesClipRect) && Chart.Tooltip.Shared && (!Chart.IsTouch || Chart.StartMove)))
                {
                    Tooltip();
                }
                else if (Chart.Tooltip.Shared && isSharedRemove)
                {
                    RemoveTooltip(Chart.Tooltip.FadeOutDuration);
                }
            }
        }

        internal void MouseLeaveHandler()
        {
            RemoveTooltip(Chart.Tooltip.FadeOutDuration);
        }

        private void RemoveTooltip(double fadeOutDuration)
        {
            timer.Stop();
            timer.Interval = fadeOutDuration;
            timer.AutoReset = false;
            if ((Chart.TemplateTooltip != null && PreviousPoints.Count > 0) || Chart.Tooltip.Template != null)
            {
                isSharedRemove = false;
                timer.Elapsed += OnTimeOut;
                timer.Start();
            }
        }

        internal void LongPress()
        {
            if (Chart.Crosshair.Enable && ChartHelper.WithInBounds(Chart.MouseX, Chart.MouseY, Chart.AxisContainer.AxisLayout.SeriesClipRect))
            {
                Tooltip();
            }
        }

        private void Tooltip()
        {
            GetTooltipElement(false, "Chart");
            if (!Chart.Tooltip.Shared)
            {
                RenderSeriesTooltip(true);
            }
            else
            {
                RenderGroupedTooltipAsync(true);
            }
        }

        private string GetTooltipText(PointData pointData)
        {
            return ParseTemplate(pointData.Point, pointData.Series, GetFormat(Chart, pointData.Series), pointData.Series.Renderer.XAxisRenderer.Axis, pointData.Series.Renderer.YAxisRenderer.Axis);
        }

        private string GetFormat(SfChart chart, ChartSeries series)
        {
            string separator = (chart.IsStockChart && chart.Tooltip.Shared && string.IsNullOrEmpty(series.Name)) ? string.Empty : BREAKTAG;
            SeriesValueType seriesType = series.Renderer.SeriesType();
            SeriesCategories category = series.Renderer.Category();
            if (!string.IsNullOrEmpty(series.TooltipFormat))
            {
                if (seriesType == SeriesValueType.XY && category == SeriesCategories.Indicator)
                {
                    return GetIndicatorTooltipFormat(series, chart.Tooltip.Format);
                }

                return series.TooltipFormat;
            }

            if (string.IsNullOrEmpty(series.TooltipFormat) && !chart.IsStockChart && !string.IsNullOrEmpty(chart.Tooltip.Format))
            {
                if (seriesType == SeriesValueType.XY && category == SeriesCategories.Indicator)
                {
                    return GetIndicatorTooltipFormat(series, chart.Tooltip.Format);
                }

                return chart.Tooltip.Format;
            }

            string format = (!chart.Tooltip.Shared || (chart.IsStockChart && !seriesType.ToString().Contains("HighLow", StringComparison.CurrentCulture))) ? (series.Type == ChartSeriesType.Histogram ? "${point.minimum}" + '-' + "${point.maximum}" : "${point.x}") : "${series.name}";

            switch (seriesType)
            {
                case SeriesValueType.XY:
                    if (category == SeriesCategories.Indicator)
                    {
                        GetIndicatorTooltipFormat(series, chart.Tooltip.Format);
                    }

                    return format + " : " + (series.Type == ChartSeriesType.Bubble ? "<b>${point.y}</b>  Size : <b>${point.size}</b>" : "<b>${point.y}</b>");
                case SeriesValueType.HighLow:
                    return format + separator + "High : <b>${point.high}</b><br/>Low : <b>${point.low}</b>";
                case SeriesValueType.HighLowOpenClose:
                    return format + separator + "High : <b>${point.high}</b><br/>Low : <b>${point.low}</b><br/>" + "Open : <b>${point.open}</b><br/>Close : <b>${point.close}</b>";
                case SeriesValueType.BoxPlot:
                    return format + separator + (LierIndex > 3 ? "Outliers : <b>${point.outliers}</b>" : "Maximum : <b>${point.maximum}</b><br/>Q3 : <b>${point.upperquartile}</b><br/>" +
                        "Median : <b>${point.median}</b><br/>Q1 : <b>${point.lowerquartile}</b><br/>Minimum : <b>${point.minimum}</b>");
            }

            return string.Empty;
        }

        private string ParseTemplate(Point point, ChartSeries series, string format, ChartAxis x_Axis, ChartAxis y_Axis)
        {
            PropertyInfo[] pointInfo = point.GetType().GetProperties(), seriesInfo = series.GetType().GetProperties();
            string regexStr, pointValue;
            bool isXPoint, isYPoint;
            foreach (PropertyInfo dataValue in pointInfo)
            {
                regexStr = new Regex("${point" + '.' + dataValue.Name.ToLower(culture) + '}', RegexOptions.Multiline).ToString();
                isXPoint = regexStr == "${point.x}";
                isYPoint = regexStr == "${point.high}" || regexStr == "${point.open}" || regexStr == "${point.close}" || regexStr == "${point.low}" || regexStr == "${point.y}" || regexStr == "${point.minimum}" || regexStr == "${point.maximum}" || regexStr == "${point.outliers}" || regexStr == "${point.upperquartile}" || regexStr == "${point.lowerquartile}" || regexStr == "${point.median}";
                pointValue = (series.Type == ChartSeriesType.BoxAndWhisker && dataValue.Name.ToLower(culture) == "y") ? string.Empty : FormatPointValue(point, regexStr == "${point.x}" ? x_Axis : y_Axis, dataValue, isXPoint, isYPoint);
                format = format.Replace(regexStr, pointValue, StringComparison.InvariantCulture);
            }

            foreach (PropertyInfo dataValue in seriesInfo)
            {
                regexStr = new Regex("${series." + dataValue.Name.ToLower(culture) + '}', RegexOptions.Multiline).ToString();
                format = format.Replace(regexStr, Convert.ToString(dataValue.GetValue(series), culture), StringComparison.InvariantCulture);
            }

            return format;
        }

        private void RenderSeriesTooltip(bool isFirst)
        {
            PointData data = GetData();
            data.LierIndex = LierIndex;
            CurrentPoints = new List<PointData>();
            if (FindData(data, PreviousPoints.Count > 0 ? PreviousPoints[0] : null))
            {
                timer.Stop();
                bool check = PreviousPoints.Count > 0 ? (PreviousPoints[0] != null && data.Point.Index == PreviousPoints[0].Point.Index && data.Series.Renderer.Index == PreviousPoints[0].Series.Renderer.Index) : false;
                if (!(Chart.DataEditingModule != null ? Chart.DataEditingModule.IsPointDragging : false) && check)
                {
                    IsRemove = true;
                    return;
                }

                if (PushData(data, true))
                {
                    TriggerTooltipRender(data, isFirst, GetTooltipText(data), FindHeader(data));
                }
            }
            else
            {
                if (data.Point == null && IsRemove)
                {
                    RemoveTooltip(Chart.Tooltip.FadeOutDuration);
                    IsRemove = false;
                }
                else
                {
                    foreach (ChartSeriesRenderer seriesRenderer in Chart.VisibleSeriesRenderers)
                    {
                        if (seriesRenderer.Series.Visible && !(seriesRenderer.Category() == SeriesCategories.TrendLine))
                        {
                            data = GetClosestX(seriesRenderer.Series) ?? data;
                        }
                    }
                }
            }

            if (data != null && data.Point != null)
            {
                FindMouseValue(data);
            }
        }

        private async void TriggerTooltipRender(PointData point, bool isFirst, string textCollection, string headerText)
        {
            string template = string.Empty;
            TooltipRenderEventArgs argsData = new TooltipRenderEventArgs(
                Constants.TOOLTIPRENDER,
                false,
                new PointInfo()
                {
                    PointX = point.Point.X,
                    PointY = point.Point.Y,
                    SeriesIndex = point.Series.Renderer.Index,
                    SeriesName = point.Series.Name,
                    PointIndex = point.Point.Index,
                    PointText = point.Point.Text
                },
                headerText,
                point.Point,
                point.Series,
                template,
                textCollection,
                Chart.Tooltip.TextStyle);
#pragma warning disable CS0612
            if (Chart.ChartEvents?.TooltipRender != null)
            {
                Chart.ChartEvents.TooltipRender.Invoke(argsData);
            }
#pragma warning restore CS0612
            if (!argsData.Cancel)
            {
                if (point.Series.Type == ChartSeriesType.BoxAndWhisker)
                {
                    isFirst = true;
                }

                HeaderText = argsData.HeaderText;
                FormattedText.Add(argsData.Text);
                Text = FormattedText;
                ChartInternalLocation clipLocation = new ChartInternalLocation(point.Series.Renderer.ClipRect.X, point.Series.Renderer.ClipRect.Y);
                if (Chart.Tooltip.Template != null && CurrentPoints.Count > 0)
                {
                    Point pointInfo = point.Point;
                    FinancialPoint financialPointInfo = point.Point as FinancialPoint;
                    ChartTooltipInfo tooltipTemp = new ChartTooltipInfo()
                    {
                        X = pointInfo.X,
                        Y = pointInfo.Y,
                        Text = pointInfo.Text,
                        High = financialPointInfo?.High,
                        Low = financialPointInfo?.Low,
                        Open = financialPointInfo?.Open,
                        Close = financialPointInfo?.Close,
                        Volume = financialPointInfo?.Volume,
                        PointX = point.Point.X,
                        PointY = point.Point.Y,
                        SeriesIndex = point.Series.Renderer.Index,
                        SeriesName = point.Series.Name,
                        PointIndex = point.Point.Index,
                        PointText = point.Point.Text
                    };
                    ChartInternalLocation symbolLocation = GetSymbolLocation(point);
                    ChartSeries series = CurrentPoints[0].Series;
                    bool isRectSeries = series.Renderer.IsRectSeries();
                    bool isNegative = isRectSeries && series.Type != ChartSeriesType.Waterfall && point.Point != null && (Convert.ToDouble(point.Point.Y, null) < 0);
                    bool inverted = Chart.RequireInvertedAxis && isRectSeries;
                    ChartInternalLocation templateLocation = GetTemplateLocation(Chart.AxisContainer.AxisLayout.SeriesClipRect, symbolLocation, 0, 0, FindMarkerHeight(CurrentPoints[0]), clipLocation, inverted, isNegative);
                    Chart.TemplateTooltip.ChangeContent(Chart.Tooltip.Template, templateLocation, tooltipTemp, null, false);
                    await SetTooltipTemplateElementSizeAsync(symbolLocation, clipLocation, inverted, isNegative, tooltipTemp);
                }
                else if (Chart.Tooltip.Template == null)
                {
                    CreateTooltip(Chart, null, isFirst, GetSymbolLocation(point), clipLocation, point.Point, FindShapes(), FindMarkerHeight(CurrentPoints[0]), Chart.AxisContainer.AxisLayout.SeriesClipRect, new List<PointData>(), null, point.Point, string.Empty);
                }
            }
            else
            {
                RemoveHighlight();
            }

            IsRemove = true;
        }

        private async Task SetTooltipTemplateElementSizeAsync(ChartInternalLocation symbolLocation, ChartInternalLocation clipLocation, bool inverted, bool isNegative, ChartTooltipInfo tooltipTemp)
        {
            elementSize = await JSRuntimeExtensions.InvokeAsync<Size>(JSRuntime, Constants.GETTEMPLATESIZE, new object[] { Chart.ID + "_tooltip" });
            if (elementSize != null && CurrentPoints.Count > 0)
            {
                ChartInternalLocation templateLocation = GetTemplateLocation(Chart.AxisContainer.AxisLayout.SeriesClipRect, symbolLocation, elementSize.Width, elementSize.Height, FindMarkerHeight(CurrentPoints[0]), clipLocation, inverted, isNegative);
                Chart?.TemplateTooltip?.ChangeContent(Chart.Tooltip.Template, templateLocation, tooltipTemp);
                RemoveHighlight();
                HighlightPoints();
                UpdatePreviousPoint(CurrentPoints, null);
            }
        }

        private void RemoveHighlightedMarker()
        {
            Chart?.MarkerExplode?.RemoveHighlightedMarker();
            PreviousPoints = new List<PointData>();
        }

        private async void OnTimeOut(object source, ElapsedEventArgs e)
        {
            if (Chart?.Tooltip?.Template != null)
            {
                Chart.TemplateTooltip?.TemplateFadeOut();
                TooltipAnimationComplete();
            }
            else
            {
                await Chart.InvokeMethod(Constants.TOOLTIPFADEOUT, new object[] { Chart?.Element });
            }

            timer?.Stop();
        }

        private double FindMarkerHeight(PointData pointData)
        {
            if (!Chart.Tooltip.EnableMarker)
            {
                return 0;
            }

            ChartSeries series = pointData.Series;
            return ((series.Marker.Visible || (Chart.Tooltip.Shared && (!series.Renderer.IsRectSeries() || series.Marker.Visible)) || series.Type == ChartSeriesType.Scatter || series.DrawType == ChartDrawType.Scatter)
                && !(series.Type == ChartSeriesType.Candle || series.Type == ChartSeriesType.Hilo || series.Type == ChartSeriesType.HiloOpenClose)) ? (((series.Marker.Height + 2) / 2) + (2 * series.Marker.Border.Width)) : 0;
        }

        private TooltipShape[] FindShapes()
        {
            if (!Chart.Tooltip.EnableMarker)
            {
                return Array.Empty<TooltipShape>();
            }

            List<TooltipShape> marker = new List<TooltipShape>();
            CurrentPoints.ForEach(x => marker.Add((TooltipShape)Enum.Parse(TooltipShape.Circle.GetType(), /*x.Point.Marker.Shape.ToString()*/ChartShape.Circle.ToString())));
            return marker.ToArray();
        }

        private ChartInternalLocation GetSymbolLocation(PointData data)
        {
            ChartInternalLocation location = Chart.IsStockChart ? new ChartInternalLocation(0, Chart.GetTooltipTop.Invoke()) : new ChartInternalLocation(0, 0);
            if (data.Series.Type != ChartSeriesType.BoxAndWhisker)
            {
                if (data.Point.SymbolLocations.Count == 0)
                {
                    return null;
                }

                location = new ChartInternalLocation(data.Point.SymbolLocations[0].X, data.Point.SymbolLocations[0].Y + location.Y);
            }

            switch (data.Series.Type)
            {
                case ChartSeriesType.BoxAndWhisker:
                    return GetBoxLocation(data);
                case ChartSeriesType.Waterfall:
                    return GetWaterfallRegion(data, location);
                case ChartSeriesType.RangeArea:
                    return GetRangeArea(data, location);
                default:
                    return location;
            }
        }

        private ChartInternalLocation GetRangeArea(PointData data, ChartInternalLocation location)
        {
            if (data.Point.Regions.Count > 0 && data.Point.Regions[0] != null)
            {
                if (!Inverted)
                {
                    location.Y = data.Point.Regions[0].Y + (data.Point.Regions[0].Height / 2);
                }
                else
                {
                    location.X = data.Point.Regions[0].X + (data.Point.Regions[0].Width / 2);
                }
            }

            return location;
        }

        private ChartInternalLocation GetWaterfallRegion(PointData data, ChartInternalLocation location)
        {
            if (!Inverted)
            {
                location.Y = ((double)data.Point.Y < 0) ? location.Y - data.Point.Regions[0].Height : location.Y;
            }
            else
            {
                location.X = ((double)data.Point.Y < 0) ? location.X + data.Point.Regions[0].Width : location.X;
            }

            return location;
        }

        private ChartInternalLocation GetBoxLocation(PointData data)
        {
            return LierIndex > 3 ? data.Point.SymbolLocations[LierIndex - 4] : new ChartInternalLocation(data.Point.Regions[0].X + (data.Point.Regions[0].Width / 2), data.Point.Regions[0].Y + (data.Point.Regions[0].Height / 2));
        }

        private string FindHeader(PointData data)
        {
            if (string.IsNullOrEmpty(Header))
            {
                return string.Empty;
            }

            Header = ParseTemplate(data.Point, data.Series, Header, data.Series.Renderer.XAxisRenderer.Axis, data.Series.Renderer.YAxisRenderer.Axis);
            if (!string.IsNullOrEmpty(Header.Replace("<b>", string.Empty, StringComparison.InvariantCulture).Replace("</b>", string.Empty, StringComparison.InvariantCulture).Trim()))
            {
                return Header;
            }

            return string.Empty;
        }

        private bool FindData(PointData data, PointData previous)
        {
            return data.Point != null && ((previous == null || (previous.Point != data.Point)) || (previous.LierIndex > 3 && previous.LierIndex != LierIndex) || (previous.Point == data.Point));
        }

        private string FormatPointValue(Point point, ChartAxis axis, PropertyInfo dataValue, bool isXPoint, bool isYPoint)
        {
            string textValue, formatValue;
            bool customLabelFormat;
            double[] outliers;
            ChartAxisRenderer axisRenderer = axis.Renderer;
            if (axis.ValueType == ValueType.DateTime && isXPoint)
            {
                textValue = Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(point.XValue), axisRenderer.DateFormat);
            }
            else if (axis.ValueType != ValueType.Category && isXPoint)
            {
                customLabelFormat = !string.IsNullOrEmpty(axis.LabelFormat) && axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture);
                _ = double.TryParse(Convert.ToString(dataValue.GetValue(point), CultureInfo.InvariantCulture), out double data);
                object pointValue = (axis.ValueType != ValueType.DateTimeCategory) ? data : dataValue.GetValue(point);
                textValue = customLabelFormat ? axis.LabelFormat.Replace("{value}", ChartAxisRenderer.FormatAxisValue(pointValue, customLabelFormat, axis.LabelFormat), StringComparison.InvariantCulture) : ChartAxisRenderer.FormatAxisValue(pointValue, customLabelFormat, axis.LabelFormat);
            }
            else if (isYPoint && (dataValue.GetValue(point) != null))
            {
                customLabelFormat = !string.IsNullOrEmpty(axis.LabelFormat) && axis.LabelFormat.Contains("{value}", StringComparison.InvariantCulture);
                if (dataValue.Name == "Outliers")
                {
                    outliers = dataValue.GetValue(point) as double[];
                    formatValue = (LierIndex - 4 >= 0) && (outliers.Length > LierIndex - 4) ? ChartAxisRenderer.FormatAxisValue(outliers[LierIndex - 4], customLabelFormat, axis.LabelFormat) : "NAN";
                }
                else
                {
                    formatValue = axisRenderer.FormatValue(Convert.ToDouble(dataValue.GetValue(point), null));
                }

                textValue = formatValue;
            }
            else
            {
                textValue = Convert.ToString(dataValue.GetValue(point), null);
            }

            return textValue;
        }

        private void RenderGroupedTooltipAsync(bool isFirst)
        {
            PointData data = null, lastData = null;
            PointData pointData = Chart.ChartAreaType == ChartAreaType.PolarAxes ? GetData() : null;
            CurrentPoints = new List<PointData>();
            List<PointData> extraPoints = new List<PointData>(), pointsInfo = new List<PointData>();
            List<string> text = new List<string>();
            List<PointInfo> argsData = new List<PointInfo>();
            string headerText = string.Empty;
            foreach (ChartSeriesRenderer seriesRenderer in Chart.VisibleSeriesRenderers)
            {
                ChartSeries series = seriesRenderer.Series;
                if (!series.EnableTooltip || !series.Visible)
                {
                    continue;
                }

                if (Chart.ChartAreaType == ChartAreaType.CartesianAxes && series.Visible)
                {
                    data = GetClosestX(series);
                }
                else if (Chart.ChartAreaType == ChartAreaType.PolarAxes && series.Visible && pointData.Point != null)
                {
                    data = new PointData(seriesRenderer.Points[pointData.Point.Index], series);
                }

                if (data != null)
                {
                    pointsInfo.Add(data);
                }
            }

            pointsInfo = GetSharedPoints(pointsInfo);
            pointsInfo = SortPointsInfo(pointsInfo);
            foreach (PointData dataPoint in pointsInfo)
            {
                argsData.Add(new PointInfo()
                {
                    PointX = dataPoint.Point.X,
                    PointY = dataPoint.Point.Y,
                    SeriesIndex = dataPoint.Series.Renderer.Index,
                    SeriesName = dataPoint.Series.Name,
                    PointIndex = dataPoint.Point.Index,
                    PointText = dataPoint.Point.Text
                });
                headerText = FindHeader(dataPoint);
                CurrentPoints.Add(dataPoint);
                text.Add(GetTooltipText(dataPoint));
                if (Chart.IsStockChart)
                {
                    _ = text[0].Replace(BREAKTAG, string.Empty, StringComparison.InvariantCulture);
                    text.Add(string.Empty);
                }

                lastData = (dataPoint.Series.Renderer.Category() == SeriesCategories.TrendLine && Chart.Tooltip.Shared) ? lastData : dataPoint;
            }

            SharedTooltipRenderEventArgs argument = new SharedTooltipRenderEventArgs(Constants.SHAREDTOOLTIPRENDER, false, text, Chart.Tooltip.TextStyle, headerText, argsData);
#pragma warning disable CS0612
            if (Chart.ChartEvents?.SharedTooltipRender != null)
            {
                Chart.ChartEvents.SharedTooltipRender.Invoke(argument);
            }
#pragma warning restore CS0612
            if (!argument.Cancel && lastData != null && CurrentPoints.Count > 0)
            {
                Text = argument.Text;
                HeaderText = argument.HeaderText;
                FindMouseValue(lastData);
                ChartInternalLocation clipLocation = CurrentPoints.Count == 1 ? new ChartInternalLocation(CurrentPoints[0].Series.Renderer.ClipRect.X, CurrentPoints[0].Series.Renderer.ClipRect.Y) : new ChartInternalLocation(0, 0);
                CreateTooltip(Chart, null, isFirst, FindSharedLocation(), clipLocation, lastData.Point, FindShapes(), FindMarkerHeight(CurrentPoints[0]), Chart.AxisContainer.AxisLayout.SeriesClipRect, extraPoints, null, lastData.Point, string.Empty);
            }
            else
            {
                extraPoints.Add(data);
            }

            isSharedRemove = true;
        }

        private static List<PointData> SortPointsInfo(List<PointData> pointsInfo)
        {
            return pointsInfo.OrderBy(pointInfo => pointInfo.Series.Renderer.Container is ChartIndicatorContainer).ToList();
        }

        private ChartInternalLocation FindSharedLocation()
        {
            if (Chart.IsStockChart)
            {
                return new ChartInternalLocation(
                    Chart.AxisContainer.AxisLayout.SeriesClipRect.X + 5,
                    Chart.AxisContainer.AxisLayout.SeriesClipRect.Y + Chart.GetTooltipTop.Invoke() + 5);
            }
            else
            {
                if (CurrentPoints.Count > 1)
                {
                    return new ChartInternalLocation(ValueX, ValueY);
                }
                else
                {
                    return GetSymbolLocation(CurrentPoints[0]);
                }
            }
        }

        private void FindMouseValue(PointData data)
        {
            ChartAxisRenderer x_AxisRenderer = data.Series.Renderer.XAxisRenderer;
            if (!Chart.RequireInvertedAxis)
            {
                if (Chart.ChartAreaType == ChartAreaType.PolarAxes)
                {
                    ValueX = (ChartHelper.ValueToPolarCoefficient(data.Point.XValue, x_AxisRenderer) * x_AxisRenderer.Rect.Width) + x_AxisRenderer.Rect.X;
                }
                else
                {
                    ValueX = (data.Series.Renderer.Category() == SeriesCategories.TrendLine && Chart.Tooltip.Shared) ? ValueX : (ChartHelper.ValueToCoefficient(data.Point.XValue, x_AxisRenderer) * x_AxisRenderer.Rect.Width) + x_AxisRenderer.Rect.X;
                }

                ValueY = Chart.MouseY;
            }
            else
            {
                ValueY = ((1 - ChartHelper.ValueToCoefficient(data.Point.XValue, x_AxisRenderer)) * x_AxisRenderer.Rect.Height) + x_AxisRenderer.Rect.Y;
                ValueX = Chart.MouseX;
            }
        }

        private void RemoveHighlight()
        {
            for (int i = 0, len = PreviousPoints.Count; i < len; i++)
            {
                PointData item = PreviousPoints[i];
                if (item.Series.Renderer.IsRectSeries())
                {
                    if (item.Series.Visible)
                    {
                        HighlightPoint(item.Series, item.Point.Index, Chart.Tooltip.Shared && CurrentPoints.Count > 0 ? (CurrentPoints[0].Point.Index == PreviousPoints[0].Point.Index) : false);
                    }

                    continue;
                }

                if (!item.Series.Marker.Visible && item.Series.Type != ChartSeriesType.Scatter && item.Series.Type != ChartSeriesType.Bubble)
                {
                    PreviousPoints.RemoveAt(0);
                    len -= 1;
                }
            }
        }

        private void HighlightPoint(ChartSeries series, int pointIndex, bool highlight)
        {
            (Chart.SvgRenderer?.PathElementList.Find(item => item.Id == (Chart.ID + "_Series_" + series.Renderer.Index + "_Point_" + pointIndex + (series.Renderer.SeriesType() == SeriesValueType.BoxPlot ? "_BoxPath" : string.Empty))))?.ChangeOpacity(highlight ? series.Opacity / 2 : series.Opacity);
        }

        private void HighlightPoints()
        {
            foreach (PointData item in CurrentPoints)
            {
                if (item.Series.Renderer.IsRectSeries() && item.Series.Renderer.Category() == SeriesCategories.Series)
                {
                    HighlightPoint(item.Series, item.Point.Index, true);
                }
            }
        }

        internal void FadeOut()
        {
            if (isDispose)
            {
                return;
            }

            ValueX = double.NaN;
            ValueY = double.NaN;
            CurrentPoints = new List<PointData>();
            RemoveHighlight();
            RemoveHighlightedMarker();
            SvgTooltip = null;
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void TooltipAnimationComplete()
        {
            FadeOut();
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void TooltipRender()
        {
            RemoveHighlight();
            HighlightPoints();
            UpdatePreviousPoint(extraPoint, null);
        }

        internal override void Dispose()
        {
            base.Dispose();
            isDispose = true;
            elementSize = null;
            timer.Dispose();
        }
    }
}