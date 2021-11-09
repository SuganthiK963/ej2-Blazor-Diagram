using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Drawing;
using System.Linq;
using System.Globalization;
using Syncfusion.Blazor.Charts.Internal;
using Point = Syncfusion.Blazor.Charts.Chart.Internal.Point;
using Size = Syncfusion.Blazor.Charts.Chart.Internal.Size;

namespace Syncfusion.Blazor.Charts
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class MarkerExplode : ChartData
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private bool isRemove;
        private Timeout markerExplode = new Timeout();
        private string trackBallClass = "EJ2-TrackBall";
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal MarkerExplode(SfChart chart)
            : base(chart)
        {
            Chart = chart;
        }

        internal void MarkerMove(bool remove)
        {
            // NOTE: Issue fix for "DataEditing" flickering.
            if (Chart.IsPointMouseDown)
            {
                RemoveHighlightedMarker();
                return;
            }

            PointData data = new PointData();
            bool explodeSeries;
            ChartSeries series = null;
            if (!Chart.Tooltip.Shared || !Chart.Tooltip.Enable)
            {
                data = GetData();
                series = data.Series;
                PointData previous = PreviousPoints.Count > 0 ? PreviousPoints[0] : null;
                explodeSeries = (series != null) && ((series.Type == ChartSeriesType.Bubble || series.DrawType == ChartDrawType.Scatter || series.Type == ChartSeriesType.Scatter) ||
                    (series.Type != ChartSeriesType.Candle && (series.Type != ChartSeriesType.Hilo) && (series.Type != ChartSeriesType.HiloOpenClose) && series.Marker.Visible && series.Marker.Width != 0 && series.Marker.Height != 0));
                data.LierIndex = LierIndex;
                if (data.Point != null && explodeSeries && (previous == null || previous.Point != data.Point || (previous.LierIndex > 3 && previous.LierIndex != LierIndex)))
                {
                    CurrentPoints.Add(data);
                }

                if (data.Point != null && explodeSeries && Chart.IsPointMouseDown)
                {
                    CurrentPoints.Add(data);
                }
            }
            else
            {
                if (!ChartHelper.WithInBounds(Chart.MouseX, Chart.MouseY, Chart.AxisContainer.AxisLayout.SeriesClipRect))
                {
                    return;
                }

                if (Chart.Tooltip.Enable)
                {
                    PointData pointData = Chart.ChartAreaType == ChartAreaType.PolarAxes ? GetData() : null;
                    foreach (ChartSeriesRenderer seriesRenderer in Chart.VisibleSeriesRenderers)
                    {
                        ChartSeries chartSeries = seriesRenderer.Series;
                        if (!chartSeries.EnableTooltip || seriesRenderer.Category() == SeriesCategories.Indicator || !chartSeries.Visible)
                        {
                            continue;
                        }

                        if (Chart.ChartAreaType == ChartAreaType.CartesianAxes && chartSeries.Visible)
                        {
                            data = GetClosestX(chartSeries);
                        }
                        else if (Chart.ChartAreaType == ChartAreaType.PolarAxes && chartSeries.Visible && pointData.Point != null)
                        {
                            data = new PointData(seriesRenderer.Points[pointData.Point.Index], chartSeries);
                        }

                        if (data != null)
                        {
                            CurrentPoints.Add(data);
                        }
                    }

                    CurrentPoints = GetSharedPoints(CurrentPoints);
                }
            }

            if (CurrentPoints.Count > 0)
            {
                if (PreviousPoints.Count == 0 || Chart.IsPointMouseDown || (PreviousPoints.Count > 0 && PreviousPoints[0].Point != CurrentPoints[0].Point))
                {
                    if (PreviousPoints.Count > 0)
                    {
                        RemoveHighlightedMarker();
                    }

                    foreach (PointData pointData in CurrentPoints)
                    {
                        ChartSeries pointDataSeries = pointData.Series;
                        Point point = pointData.Point;
                        point.Marker = point.Marker != null ? point.Marker : new MarkerSettingModel { Visible = false };
                        if ((pointData != null && point != null) || (series != null && (series.Type != ChartSeriesType.Candle) && (series.Type != ChartSeriesType.Hilo) && (series.Type != ChartSeriesType.HiloOpenClose)))
                        {
                            markerExplode.Stop();
                            isRemove = true;
                            foreach (ChartInternalLocation location in point.SymbolLocations)
                            {
                                int index = point.SymbolLocations.IndexOf(location);
                                if (!pointDataSeries.Renderer.IsRectSeries() || point.Marker.Visible)
                                {
                                    DrawTrackBall(pointDataSeries, point, location, index);
                                }
                            }
                        }
                    }

                    PreviousPoints = new List<PointData>(CurrentPoints.Count);
                    PreviousPoints.AddRange(CurrentPoints.Select(i => new PointData(i.Point, i.Series, i.LierIndex)));
                }
            }

            if (!Chart.Tooltip.Enable && ((CurrentPoints.Count == 0 && isRemove) || (remove && isRemove) || !ChartHelper.WithInBounds(Chart.MouseX, Chart.MouseY, Chart.AxisContainer.AxisLayout.SeriesClipRect)))
            {
                isRemove = false;
                markerExplode = new Timeout(() => { RemoveHighlightedMarker(); }, 2000);
            }

            CurrentPoints = new List<PointData>();
        }

        private void DrawTrackBall(ChartSeries series, Point point, ChartInternalLocation location, double index)
        {
            MarkerSettingModel marker = point.Marker;
            ChartMarker seriesMarker = series.Marker;
            ChartSeriesRenderer seriesRenderer = series.Renderer;
            string symbolId = Chart.ID + "_Series_" + seriesRenderer.Index + "_Point_" + point.Index + "_Trackball" + (!double.IsNaN(index) && index != 0 ? "_" + Convert.ToString(index, null) : string.Empty);
            Size size = new Size((!double.IsNaN(marker.Width) ? marker.Width : seriesMarker.Width) + 5, (!double.IsNaN(marker.Height) ? marker.Height : seriesMarker.Height) + 5);
#pragma warning disable CA2000
#pragma warning disable BL0005
            ChartEventBorder seriesBorder = new ChartEventBorder { Width = series.Border.Width, Color = series.Border.Color };
#pragma warning disable BL0005
#pragma warning restore CA2000
            ChartEventBorder border = marker.Border ?? seriesBorder;
            string borderShadow = (!string.IsNullOrEmpty(border.Color) && border.Color != "transparent") ? border.Color :
               !string.IsNullOrEmpty(marker.Fill) ? marker.Fill : !string.IsNullOrEmpty(point.Interior) ? point.Interior : seriesRenderer.Interior;
            Color borderColor = ChartHelper.GetRBGValue(borderShadow);
            double borderWidth = (marker.Border != null) ? marker.Border.Width : seriesMarker.Border.Width;
            string markerShadow = "rgba(" + borderColor.R + ',' + borderColor.G + ',' + borderColor.B + ",0.2)";
            string transform = Chart.ChartAreaType == ChartAreaType.CartesianAxes ? "translate(" + seriesRenderer.ClipRect.X.ToString(culture) + "," + seriesRenderer.ClipRect.Y.ToString(culture) + ")" : string.Empty;
            string clipPath = (series.Type == ChartSeriesType.Bubble || series.Type == ChartSeriesType.Scatter) ? seriesRenderer.SeriesClipPath() : seriesRenderer.MarkerClipPath();
            for (int i = 0; i < 2; i++)
            {
                PathOptions option = new PathOptions(
                    symbolId + '_' + i,
                    string.Empty,
                    string.Empty,
                    borderWidth + (i > 0 ? 0 : 8),
                    i > 0 ? borderShadow : markerShadow,
                    !double.IsNaN(marker.Opacity) ? marker.Opacity : seriesMarker.Opacity,
                    i > 0 ? (!string.IsNullOrEmpty(marker.Fill) ? marker.Fill : !string.IsNullOrEmpty(point.Interior) ? point.Interior : "#ffffff") : "transparent");
                SymbolOptions symbolOption = ChartHelper.CalculateShapes(location, size, (!string.IsNullOrEmpty(Convert.ToString(marker.Shape, null)) ? marker.Shape : seriesMarker.Shape).ToString(), seriesMarker.ImageUrl, option, false);
                Render(symbolOption.ShapeName == ShapeName.path ? symbolOption.PathOption : (symbolOption.ShapeName == ShapeName.ellipse) ? (object)symbolOption.EllipseOption : symbolOption.ImageOption, symbolOption.ShapeName.ToString(), transform, clipPath);
            }
        }

        internal void RemoveHighlightedMarker()
        {
            RemoveTrackBall();
            PreviousPoints = new List<PointData>();
        }

        private async void Render(object option, string shapeName, string transform, string clipPath)
        {
            await Chart.InvokeMethod(Constants.DRAWTRACKBALL, new object[] { Chart.GetSvgId(), option, shapeName, trackBallClass, clipPath, transform });
        }

        private async void RemoveTrackBall()
        {
            if (!Chart.ChartDisposed())
            {
                await Chart.InvokeMethod(Constants.REMOVEHIGHLIGHTEDMARKER, new object[] { trackBallClass });
            }
        }
    }

    internal class Timeout : System.Timers.Timer
    {
        internal Timeout(Action action, double delay)
        {
            AutoReset = false;
            Interval = delay;
            Elapsed += (sender, args) => action();
            Start();
        }

        internal Timeout()
        {
        }
    }
}