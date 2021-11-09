using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class TooltipBase : ChartData
    {
        internal TooltipBase(SfChart sfchart)
            : base(sfchart)
        {
            JSRuntime = sfchart.JSRuntime;
        }

        internal TooltipBase(SfAccumulationChart accChart)
           : base(accChart)
        {
            JSRuntime = accChart.JSRuntime;
        }

        internal ChartTooltipComponent TemplateTooltip { get; set; }

        internal bool IsRemove { get; set; }

        internal bool Inverted { get; set; }

        internal List<string> FormattedText { get; set; } = new List<string>();

        internal double ValueX { get; set; }

        internal double ValueY { get; set; }

        internal List<string> Text { get; set; }

        internal SVGTooltip SvgTooltip { get; set; }

        internal string HeaderText { get; set; }

        internal string Header { get; set; }

        internal IJSRuntime JSRuntime { get; set; }

        internal List<PointData> extraPoint { get; set; }

        private static bool IsNaN(object value)
        {
            return double.TryParse(value.ToString(), out double valid) && double.IsNaN(valid);
        }

        internal virtual void GetTooltipElement(bool isTooltip, string baseControl = "AccumulationChart")
        {
            if (baseControl == "AccumulationChart")
            {
                Header = AccChartInstance.Tooltip.Header == null ? AccChartInstance.Tooltip.Shared ? "<b>${point.x}</b>" : "<b>${series.name}</b>" : AccChartInstance.Tooltip.Header;
            }
            else
            {
                Inverted = Chart.RequireInvertedAxis;
                Header = Chart.Tooltip.Header == null ? Chart.Tooltip.Shared ? "<b>${point.x}</b>" : "<b>${series.name}</b>" : Chart.Tooltip.Header;
            }

            FormattedText = new List<string>();
        }

        internal bool PushData(PointData data, bool isChart)
        {
            if (data.Series.EnableTooltip)
            {
                if (isChart)
                {
                    CurrentPoints.Add(data);
                }

                return true;
            }

            return false;
        }

        internal async void CreateTooltip(SfChart chart, SfAccumulationChart accChart, bool isFirst, ChartInternalLocation location, ChartInternalLocation clipLocation, Point point, TooltipShape[] shapes, double offset, Rect bounds, List<PointData> extraPoints = null, List<AccPointData> accExtraPoints = null, Point templatePoint = null, string customTemplate = "")
        {
            ChartSeries series = accChart != null ? null : CurrentPoints[0].Series;
            ChartDefaultFont textStyle = chart != null ? chart.Tooltip.TextStyle : null;
            TooltipBorderModel border = new TooltipBorderModel
            {
                Color = chart != null ? chart.Tooltip.Border.Color : accChart.Tooltip.Border.Color,
                Width = chart != null ? chart.Tooltip.Border.Width : accChart.Tooltip.Border.Width
            };

            ChartCommonFont accTextstyle = accChart != null ? accChart.Tooltip.TextStyle : null;
            DomRect tooltipParentOffset = chart != null ? chart.SecondaryElementOffset : accChart.SecondaryElementOffset;
            if (isFirst)
            {
                SvgTooltip = new SVGTooltip()
                {
                    Opacity = accChart != null ? accChart.Tooltip.Opacity : chart.Tooltip.Opacity,
                    Header = HeaderText,
                    Content = Text?.ToArray(),
                    Fill = accChart != null ? accChart.Tooltip.Fill : chart.Tooltip.Fill,
                    Border = border,
                    EnableAnimation = accChart != null ? accChart.Tooltip.EnableAnimation : chart.Tooltip.EnableAnimation,
                    Location = (location != null) ? new ToolLocationModel { X = location.X + tooltipParentOffset.Left, Y = location.Y + tooltipParentOffset.Top } : null,
                    Shared = chart != null ? chart.Tooltip.Shared : false,
                    Shapes = shapes,
                    ClipBounds = chart != null ? (Chart.ChartAreaType == ChartAreaType.PolarAxes ? new ToolLocationModel { X = 0, Y = 0 } : new ToolLocationModel { X = clipLocation.X, Y = clipLocation.Y }) : new ToolLocationModel { X = 0, Y = 0 },
                    AreaBounds = new AreaBoundsModel { X = bounds.X + tooltipParentOffset.Left, Y = bounds.Y + ((chart != null && chart.IsStockChart) ? chart.GetTooltipTop.Invoke() : 0) + tooltipParentOffset.Top, Height = bounds.Height, Width = bounds.Width },
                    Palette = FindPalette(),
                    Template = customTemplate ?? chart.Tooltip.Template.ToString(),
                    Data = GetData(templatePoint, series),
                    Theme = chart != null ? chart.Theme.ToString() : accChart.Theme.ToString(),
                    Offset = offset,
                    TextStyle = textStyle != null ? GetTextStyle(textStyle) : GetAccTextStyle(accTextstyle),
                    IsNegative = chart != null ? (series.Renderer.IsRectSeries() && series.Type != ChartSeriesType.Waterfall && point != null && (point.Y != null) ? (series.Renderer.SeriesType() != SeriesValueType.BoxPlot) ? Convert.ToDouble(point.Y, null) < 0 : false : false) : false,
                    Inverted = chart != null ? chart.RequireInvertedAxis && series.Renderer.IsRectSeries() : false,
                    ArrowPadding = Text.Count > 1 ? 0 : 12,
                    AvailableSize = chart != null ? chart.AvailableSize : accChart.AvailableSize,
                    Duration = chart != null ? chart.Tooltip.Duration : accChart.Tooltip.Duration,
                    IsCanvas = false,
                    IsTextWrap = chart != null ? chart.Tooltip.EnableTextWrap : accChart.Tooltip.EnableTextWrap,
                    EnableRTL = chart != null ? chart.EnableRTL : accChart.EnableRTL
                };
                string options = JsonSerializer.Serialize(SvgTooltip);
                if (chart != null)
                {
                    extraPoint = extraPoints;
                    await chart.InvokeMethod(Constants.RENDERTOOLTIP, new object[] { options, chart.ID + "_tooltip", DotNetObjectReference.Create<object>(chart.TooltipModule), chart.Element });
                }
                else
                {
                     accChart.AccumulationTooltipModule.RemoveHighlight();
                     await JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, AccumulationChartConstants.RENDERTOOLTIP, new object[] { options, accChart.ID + "_tooltip", DotNetObjectReference.Create<object>(accChart.AccumulationTooltipModule), accChart.Element });
                     UpdatePreviousPoint(null, accExtraPoints);
                }
            }
            else if (SvgTooltip != null)
            {
                SvgTooltip.Location = new ToolLocationModel { X = location.X, Y = location.Y };
                SvgTooltip.Content = Text.ToArray();
                SvgTooltip.Header = HeaderText;
                SvgTooltip.Offset = offset;
                SvgTooltip.Palette = FindPalette();
                SvgTooltip.Shapes = shapes;
                SvgTooltip.Data = GetData(templatePoint, series);
                SvgTooltip.Template = customTemplate ?? chart.Tooltip.Template.ToString();
                SvgTooltip.TextStyle = textStyle != null ? GetTextStyle(textStyle) : GetAccTextStyle(accTextstyle);
                SvgTooltip.IsNegative = chart != null ? (series.Renderer.IsRectSeries() && series.Type != ChartSeriesType.Waterfall && point != null && (double)point.Y < 0) : false;
                SvgTooltip.ClipBounds = chart != null ? Chart.ChartAreaType == ChartAreaType.PolarAxes ? new ToolLocationModel { X = 0, Y = 0 } : new ToolLocationModel { X = clipLocation.X, Y = clipLocation.Y } : new ToolLocationModel { X = 0, Y = 0 };
                SvgTooltip.ArrowPadding = Text.Count > 1 ? 0 : 12;
                string options = JsonSerializer.Serialize(SvgTooltip);
                if (chart != null)
                {
                    extraPoint = extraPoints;
                    await chart.InvokeMethod(Constants.RENDERTOOLTIP, new object[] { options, chart.ID + "_tooltip", DotNetObjectReference.Create<object>(chart.TooltipModule), chart.Element });
                }
                else
                {
                     accChart.AccumulationTooltipModule.RemoveHighlight();
                     await JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, AccumulationChartConstants.RENDERTOOLTIP, new object[] { options, accChart.ID + "_tooltip", DotNetObjectReference.Create<object>(accChart.AccumulationTooltipModule), AccChartInstance.Element });
                     UpdatePreviousPoint(null, accExtraPoints);
                }
            }
        }

        internal void UpdatePreviousPoint(List<PointData> extraPoints, List<AccPointData> accExtraPoints)
        {
            if (extraPoints != null)
            {
                if (extraPoints.Count > 0)
                {
                    CurrentPoints = CurrentPoints.Concat(extraPoints).ToList();
                }

                PreviousPoints = new List<PointData>(CurrentPoints.Count);
                PreviousPoints.AddRange(CurrentPoints.Select(i => new PointData(i.Point, i.Series, i.LierIndex)));
            }
            else if (accExtraPoints != null)
            {
                if (accExtraPoints.Count > 0)
                {
                    AccCurrentPoints = AccCurrentPoints.Concat(accExtraPoints).ToList();
                }

                AccPreviousPoints = new List<AccPointData>(AccCurrentPoints.Count);
                AccPreviousPoints.AddRange(AccCurrentPoints.Select(i => new AccPointData(i.Point, i.Series, i.LierIndex)));
            }
        }

        private static string FindColor(PointData data, ChartSeries series)
        {
            if (series.Renderer.IsRectSeries() && (series.Type == ChartSeriesType.Candle || series.Type == ChartSeriesType.Hilo || series.Type == ChartSeriesType.HiloOpenClose))
            {
                return data.Point.Interior;
            }
            else
            {
                string color = data.Point.Interior;
                return !string.IsNullOrEmpty(color) ? color : (!string.IsNullOrEmpty(series.Marker.Fill) ? series.Marker.Fill : series.Renderer.Interior);
            }
        }

        private string[] FindPalette()
        {
            List<string> colors = new List<string>();

            if (AccCurrentPoints.Count != 0)
            {
                AccCurrentPoints.ForEach(x => colors.Add(x.Point.Color));
            }
            else
            {
                CurrentPoints.ForEach(x => colors.Add(FindColor(x, x.Series)));
            }

            return colors.ToArray();
        }

        internal static ChartInternalLocation GetTemplateLocation(Rect bounds, ChartInternalLocation symbolLocation, double width, double height, double markerHeight, ChartInternalLocation clipBounds, bool inverted, bool isNegative)
        {
            ChartInternalLocation location = new ChartInternalLocation(symbolLocation.X, symbolLocation.Y);
            double elementWidth = width + 10, elementHeight = height + 10, clipX = clipBounds.X, clipY = clipBounds.Y, boundsX = bounds.X, boundsY = bounds.Y;
            if (!inverted)
            {
                location = new ChartInternalLocation(location.X + clipX - (width / 2), location.Y + clipY - height - 12 - markerHeight);
                if (location.Y < boundsY || isNegative)
                {
                    location.Y = (symbolLocation.Y < 0 ? 0 : symbolLocation.Y) + clipY + markerHeight;
                }

                if (location.Y + elementHeight + 12 > boundsY + bounds.Height)
                {
                    location.Y = (symbolLocation.Y > bounds.Height ? bounds.Height : symbolLocation.Y) + clipY - height - 12 - markerHeight;
                }

                if (location.X < boundsX)
                {
                    location.X = boundsX;
                }

                if (location.X + elementWidth > boundsX + bounds.Width)
                {
                    location.X -= (location.X + elementWidth) - (boundsX + bounds.Width);
                }
            }
            else
            {
                location = new ChartInternalLocation(location.X + clipX + markerHeight, location.Y + clipY - (height / 2));
                if ((location.X + elementWidth + 12 > boundsX + bounds.Width) || isNegative)
                {
                    location.X = (symbolLocation.X > bounds.Width ? bounds.Width : symbolLocation.X) + clipX - markerHeight - (elementWidth + 12);
                }

                if (location.X < boundsX)
                {
                    location.X = (symbolLocation.X < 0 ? 0 : symbolLocation.X) + clipX + markerHeight;
                }

                if (location.Y <= boundsY)
                {
                    location.Y = boundsY;
                }

                if (location.Y + elementHeight >= boundsY + bounds.Height)
                {
                    location.Y -= (location.Y + elementHeight) - (boundsY + bounds.Height);
                }
            }

            return new ChartInternalLocation(location.X, location.Y);
        }

        private static TemplateData GetData(Point templatePoint, ChartSeries series)
        {
            return new TemplateData
            {
                X = templatePoint?.X,
                Y = templatePoint?.Y != null && (series.Type != ChartSeriesType.BoxAndWhisker) ? (!IsNaN(templatePoint.Y) ? templatePoint.Y : null) : null,

                // High = templatePoint?.High != null && !IsNaN(templatePoint.High) ? templatePoint.High : null,
                // Low = templatePoint?.Low != null && !IsNaN(templatePoint.Low) ? templatePoint.Low : null,
                // Open = templatePoint?.Open != null && !IsNaN(templatePoint.Open) ? templatePoint.Open : null,
                // Close = templatePoint?.Close != null && !IsNaN(templatePoint.Close) ? templatePoint.Close : null,
                // Volume = templatePoint?.Volume != null && !IsNaN(templatePoint.Volume) ? templatePoint.Volume : null,
                Text = templatePoint?.Text
            };
        }

        private static TextStyleModel GetTextStyle(ChartDefaultFont textStyle)
        {
            return new TextStyleModel
            {
                Size = textStyle.Size,
                Color = textStyle.Color,
                FontFamily = textStyle.FontFamily,
                FontWeight = textStyle.FontWeight,
                FontStyle = textStyle.FontStyle,
                Opacity = textStyle.Opacity
            };
        }

        private static TextStyleModel GetAccTextStyle(ChartCommonFont textStyle)
        {
            return new TextStyleModel
            {
                Size = textStyle.Size,
                Color = textStyle.Color,
                FontFamily = textStyle.FontFamily,
                FontWeight = textStyle.FontWeight,
                FontStyle = textStyle.FontStyle,
                Opacity = textStyle.Opacity
            };
        }

        internal override void Dispose()
        {
            base.Dispose();
            TemplateTooltip = null;
            FormattedText = null;
            SvgTooltip = null;
            Text = null;
        }
    }
}