using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    public class ChartData
    {
        public ChartData(SfChart sfchart)
        {
            Chart = sfchart;
            LierIndex = 0;
        }

        internal ChartData(SfAccumulationChart chart)
        {
            AccChartInstance = chart;
            LierIndex = 0;
        }

        protected SfChart Chart { get; set; }

        protected SfAccumulationChart AccChartInstance { get; set; }

        protected int LierIndex { get; set; }

        protected List<PointData> CurrentPoints { get; set; } = new List<PointData>();

        protected List<AccPointData> AccCurrentPoints { get; set; } = new List<AccPointData>();

        internal List<AccPointData> AccPreviousPoints { get; set; } = new List<AccPointData>();

        internal bool InsideRegion { get; set; }

        internal List<PointData> PreviousPoints { get; set; } = new List<PointData>();

        internal PointData GetData()
        {
            Point point = null;
            ChartSeries series = null;
            ChartSeriesRenderer seriesRenderer;
            bool isRectSeries;
            double width, height, mouseX, mouseY, markerWidth, markerHeight;
            for (int len = Chart.VisibleSeriesRenderers.Count, i = len - 1; i >= 0; i--)
            {
                seriesRenderer = Chart.VisibleSeriesRenderers[i];
                series = seriesRenderer.Series;
                isRectSeries = seriesRenderer.IsRectSeries();
                width = (series.Type == ChartSeriesType.Scatter || series.DrawType == ChartDrawType.Scatter || (!isRectSeries && series.Marker.Visible)) ? (series.Marker.Height + 5) / 2 : 0;
                height = (series.Type == ChartSeriesType.Scatter || series.DrawType == ChartDrawType.Scatter || (!isRectSeries && series.Marker.Visible)) ? (series.Marker.Width + 5) / 2 : 0;
                mouseX = Chart.MouseX;
                mouseY = Chart.MouseY;
                if (series.ChartDataEditSettings.Enable && seriesRenderer.IsRectSeries())
                {
                    if (!(series.Type == ChartSeriesType.Bar && Chart.IsTransposed) && (Chart.IsTransposed || series.Type == ChartSeriesType.Bar))
                    {
                        markerWidth = series.Marker.Width / 2;
                        mouseX = seriesRenderer.YAxisRenderer.Axis.IsInversed ? mouseX + markerWidth : mouseX - markerWidth;
                    }
                    else
                    {
                        markerHeight = series.Marker.Height / 2;
                        mouseY = seriesRenderer.YAxisRenderer.Axis.IsInversed ? mouseY - markerHeight : mouseY + markerHeight;
                    }
                }

                if (series.Visible && ChartHelper.WithInBounds(mouseX, mouseY, seriesRenderer.ClipRect, width, height))
                {
                    point = GetRectPoint(seriesRenderer, seriesRenderer.ClipRect, mouseX, mouseY);
                }

                if (point != null)
                {
                    return new PointData(point, series);
                }
            }

            return new PointData(point, series);
        }

        protected static double GetClosest(ChartSeriesRenderer seriesRenderer, double pointValue)
        {
            double closest = double.NaN;
            if (seriesRenderer == null)
            {
                return closest;
            }

            ChartAxisRenderer x_AxisRenderer = seriesRenderer.XAxisRenderer;
            if (x_AxisRenderer != null && pointValue >= x_AxisRenderer.VisibleRange.Start - 0.5 && pointValue <= x_AxisRenderer.VisibleRange.End + 0.5)
            {
                foreach (double data in seriesRenderer.XData)
                {
                    if (double.IsNaN(closest) || Math.Abs(data - pointValue) < Math.Abs(closest - pointValue))
                    {
                        closest = data;
                    }
                }
            }

            return closest;
        }

        private Point GetRectPoint(ChartSeriesRenderer seriesRenderer, Rect rect, double x, double y)
        {
            ChartSeries series = seriesRenderer.Series;
            foreach (Point point in seriesRenderer.Points)
            {
                if (point.RegionData == null && point.Regions.Count == 0)
                {
                    continue;
                }

                if (point.RegionData != null && Chart.ChartAreaType == ChartAreaType.PolarAxes && series.DrawType.ToString().Contains("Column", StringComparison.InvariantCulture))
                {
                    double fromCenterX = x - ((seriesRenderer.ClipRect.Width / 2) + seriesRenderer.ClipRect.X),
                           fromCenterY = y - ((seriesRenderer.ClipRect.Height / 2) + seriesRenderer.ClipRect.Y),
                           arcAngle = 2 * Math.PI * (point.RegionData.CurrentXPosition < 0 ? 1 + point.RegionData.CurrentXPosition : point.RegionData.CurrentXPosition),
                           startAngle = point.RegionData.StartAngle,
                           endAngle = point.RegionData.EndAngle,
                           distanceFromCenter = Math.Sqrt(Math.Pow(Math.Abs(fromCenterX), 2) + Math.Pow(Math.Abs(fromCenterY), 2)),
                           clickAngle = (Math.Atan2(fromCenterY, fromCenterX) + (0.5 * Math.PI) - arcAngle) % (2 * Math.PI);
                    clickAngle = clickAngle < 0 ? (2 * Math.PI) + clickAngle : clickAngle;
                    clickAngle = clickAngle + (2 * Math.PI * seriesRenderer.XAxisRenderer.Axis.StartAngle);
                    startAngle -= arcAngle;
                    startAngle = startAngle < 0 ? (2 * Math.PI) + startAngle : startAngle;
                    endAngle -= arcAngle;
                    endAngle = endAngle < 0 ? (2 * Math.PI) + endAngle : endAngle;
                    if (clickAngle >= startAngle && clickAngle <= endAngle && ((distanceFromCenter >= point.RegionData.InnerRadius && distanceFromCenter <= point.RegionData.Radius) ||
                          (distanceFromCenter <= point.RegionData.InnerRadius && distanceFromCenter >= point.RegionData.Radius)) && distanceFromCenter <= Chart.AxisContainer.AxisLayout.Radius)
                    {
                        return point;
                    }
                }

                if (series.ChartDataEditSettings.Enable && seriesRenderer.IsRectSeries() && RectRegion(x, y, point, rect, seriesRenderer.Series))
                {
                    InsideRegion = true;
                    return point;
                }

                if (CheckRegionContainsPoint(point.Regions, rect, x, y))
                {
                    return point;
                }
            }

            return null;
        }

        private bool CheckRegionContainsPoint(List<Rect> regionRect, Rect rect, double x, double y)
        {
            Rect result = regionRect.Find(region => ChartHelper.WithInBounds(x, y, new Rect((Chart.ChartAreaType == ChartAreaType.CartesianAxes ? rect.X : 0) + region.X, (Chart.ChartAreaType == ChartAreaType.CartesianAxes ? rect.Y : 0) + region.Y, region.Width, region.Height)));
            if (result != null)
            {
                LierIndex = regionRect.IndexOf(result);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected PointData GetClosestX(ChartSeries series)
        {
            if (series == null)
            {
                return null;
            }

            double pointValue = GetPointValue(series.Renderer);
            double closest = GetClosest(series.Renderer, pointValue);
            foreach (Point point in series.Renderer.Points)
            {
                if (closest == point.XValue && point.Visible)
                {
                    return new PointData(point, series);
                }
            }

            return null;
        }

        protected double GetPointValue(ChartSeriesRenderer seriesRenderer)
        {
            if (seriesRenderer != null && seriesRenderer.ClipRect != null)
            {
                ChartAxisRenderer x_AxisRenderer = seriesRenderer.XAxisRenderer;
                double pointValue = (!Chart.RequireInvertedAxis) ? Chart.MouseX - seriesRenderer.ClipRect.X : Chart.MouseY - seriesRenderer.ClipRect.Y,
                    size = (!Chart.RequireInvertedAxis) ? seriesRenderer.ClipRect.Width : seriesRenderer.ClipRect.Height;
                return ChartHelper.GetValueByPoint(pointValue, size, x_AxisRenderer.Orientation, x_AxisRenderer.VisibleRange, x_AxisRenderer.Axis.IsInversed);
            }

            return 0;
        }

        
        private bool RectRegion(double x, double y, Point point, Rect rect, ChartSeries series)
        {
            double y_Value = 0, x_Value = 0, width = 20, height = 20;
            bool isInversed = series.Renderer.YAxisRenderer.Axis.IsInversed;
            if (isInversed && Chart.IsTransposed)
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    y_Value = point.Regions[0].Height - 10;
                    width = point.Regions[0].Width;
                }
                else
                {
                    x_Value = -10;
                    height = point.Regions[0].Height;
                }
            }
            else if (isInversed || point.YValue < 0)
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    x_Value = -10;
                    height = point.Regions[0].Height;
                }
                else
                {
                    y_Value = point.Regions[0].Height - 10;
                    width = point.Regions[0].Width;
                }
            }
            else if (Chart.IsTransposed)
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    y_Value = -10;
                    width = point.Regions[0].Width;
                }
                else
                {
                    x_Value = point.Regions[0].Width - 10;
                    height = point.Regions[0].Height;
                }
            }
            else
            {
                if (series.Type == ChartSeriesType.Bar)
                {
                    x_Value = point.Regions[0].Width - 10;
                    height = point.Regions[0].Height;
                }
                else
                {
                    y_Value = -10;
                    width = point.Regions[0].Width;
                }
            }

            return point.Regions.Any(region => ChartHelper.WithInBounds(x, y, new Rect((Chart.ChartAreaType == ChartAreaType.CartesianAxes ? rect.X : 0) + region.X + x_Value, (Chart.ChartAreaType == ChartAreaType.CartesianAxes ? rect.Y : 0) + region.Y + y_Value, width, height)));
        }

        protected List<PointData> GetSharedPoints(List<PointData> pointsInfo)
        {
            if (!(Chart.ChartAreaType == ChartAreaType.CartesianAxes && pointsInfo != null && pointsInfo.Count > 0 && pointsInfo.GroupBy(info => info.Point.XValue).Distinct().ToList().Count > 1))
            {
                return pointsInfo;
            }

            double pointValue, closest = double.NaN;
            foreach (PointData point in pointsInfo)
            {
                pointValue = GetPointValue(point.Series.Renderer);
                if (IsCloset(pointValue, point, closest))
                {
                    closest = point.Point.XValue;
                }
            }

            return pointsInfo = pointsInfo.Where(info => closest == info.Point.XValue).ToList();
        }

        private static bool IsCloset(double pointValue, PointData point, double closest)
        {
            ChartAxisRenderer x_AxisRenderer = point.Series.Renderer.XAxisRenderer;
            return pointValue >= x_AxisRenderer.VisibleRange.Start - 0.5 && pointValue <= x_AxisRenderer.VisibleRange.End + 0.5 &&
                (double.IsNaN(closest) || Math.Abs(point.Point.XValue - pointValue) < Math.Abs(closest - pointValue));
        }

        internal virtual void Dispose()
        {
            Chart = null;
            AccChartInstance = null;
            AccCurrentPoints.Clear();
            AccPreviousPoints.Clear();

            CurrentPoints.Clear();
            PreviousPoints.Clear();
        }
    }
}