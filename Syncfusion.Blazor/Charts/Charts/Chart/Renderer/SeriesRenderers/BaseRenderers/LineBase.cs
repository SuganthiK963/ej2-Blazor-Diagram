using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System.Runtime.InteropServices;
using System.Text;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal abstract class LineBaseSeriesRenderer : ChartSeriesRenderer
    {
        protected const string SPACE = " ";
        protected PathOptions options;

        internal LineBaseSeriesRenderer([Optional] SfChart chart)
        {
            Chart = chart;
        }

        internal SfChart Chart { get; set; }

        protected StringBuilder Direction { get; set; } = new StringBuilder();

        internal static ConnectPoints GetFirstLastVisiblePoint(List<Point> points)
        {
            Point first = null;
            Point last = null;
            foreach (var point in points)
            {
                if (first == null && point.Visible)
                {
                    first = last = point;
                }

                last = point.Visible ? point : last;
            }

            return new ConnectPoints() { First = first, Last = last };
        }

        protected List<Point> EnableComplexProperty()
        {
            List<Point> tempPoints = new List<Point>();
            List<Point> tempPoints2 = new List<Point>();
            Rect areaBounds = ClipRect;
            if (Points.Count > 0)
            {
                double x_Tolerance = Math.Abs(XAxisRenderer.VisibleRange.Delta / areaBounds.Width),
                y_Tolerance = Math.Abs(YAxisRenderer.VisibleRange.Delta / areaBounds.Height),
                prevXValue = (Points[0] != null && IsValid(Points[0].X) && (Convert.ToDouble(Points[0].X, null) > x_Tolerance)) ? 0 : x_Tolerance,
                prevYValue = (Points[0] != null && IsValid(Points[0].Y) && (Convert.ToDouble(Points[0].Y, null) > x_Tolerance)) ? 0 : y_Tolerance;
                foreach (Point currentPoint in Points.ToArray())
                {
                    currentPoint.SymbolLocations = new List<ChartInternalLocation>();
                    double x_Val = currentPoint.XValue != 0 ? currentPoint.XValue : XAxisRenderer.VisibleRange.Start;
                    double y_Val = currentPoint.YValue != 0 ? currentPoint.YValue : YAxisRenderer.VisibleRange.Start;
                    if (Math.Abs(prevXValue - x_Val) >= x_Tolerance || Math.Abs(prevYValue - y_Val) >= y_Tolerance)
                    {
                        tempPoints.Add(currentPoint);
                        prevXValue = x_Val;
                        prevYValue = y_Val;
                    }
                }

                for (int i = 0; i < tempPoints.Count; i++)
                {
                    if (tempPoints[i].X == null || string.IsNullOrEmpty(Convert.ToString(tempPoints[i].X, null)))
                    {
                        continue;
                    }
                    else
                    {
                        tempPoints2.Add(tempPoints[i]);
                    }
                }
            }

            return tempPoints2;
        }

        protected virtual void GetLineDirection(double firstPointX, double firstPointY, double secondPointX, double secondPointY, bool isInverted, string startPoint)
        {
            ChartInternalLocation point1, point2;
            if (!double.IsNaN(firstPointX))
            {
                point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(firstPointX), YAxisRenderer.GetPointValue(firstPointY), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(secondPointX), YAxisRenderer.GetPointValue(secondPointY), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                Direction.Append(string.Join(string.Empty, startPoint, SPACE, Convert.ToInt32(point1.X), SPACE, Convert.ToInt32(point1.Y), SPACE, 'L', SPACE, Convert.ToInt32(point2.X), SPACE, Convert.ToInt32(point2.Y), SPACE));
            }
        }

        protected virtual void GetPolarLineDirection(double firstPointX, double firstPointY, double secondPointX, double secondPointY, bool isInverted, string startPoint)
        {
            ChartInternalLocation point1, point2;
            if (!double.IsNaN(firstPointX))
            {
                point1 = ChartHelper.TransformToVisible(firstPointX, firstPointY, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                point2 = ChartHelper.TransformToVisible(secondPointX, secondPointY, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                Direction.Append(string.Join(string.Empty, startPoint, SPACE, Convert.ToInt32(point1.X), SPACE, Convert.ToInt32(point1.Y), SPACE, 'L', SPACE, Convert.ToInt32(point2.X), SPACE, Convert.ToInt32(point2.Y), SPACE));
            }
        }

        protected virtual void StorePointLocation(Point point, ChartSeries series, bool isInverted)
        {
            double markerWidth = !double.IsNaN(Series.Marker.Width) ? Series.Marker.Width : 0;
            double markerHeight = !double.IsNaN(Series.Marker.Height) ? Series.Marker.Height : 0;
            point?.SymbolLocations.Add(ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted));
            point.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
        }

        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            switch (property)
            {
                case "Fill":
                    options.Stroke = Interior;
                    Series.Marker.Renderer?.MarkerColorChanged();
                    break;
                case "DashArray":
                    options.StrokeDashArray = Series.DashArray;
                    break;
                case "Width":
                    options.StrokeWidth = Series.Width;
                    break;
                case "Opacity":
                    options.Opacity = Series.Opacity;
                    break;
            }
        }

        internal static bool WithinYRange(Point point, ChartAxisRenderer y_Axis)
        {
            return point.YValue >= y_Axis.VisibleRange.Start && point.YValue <= y_Axis.VisibleRange.End;
        }

        internal void RenderSeriesElement(RenderTreeBuilder builder, PathOptions options)
        {
            options.Direction = ChartHelper.AppendPathElements(Owner, options.Direction, options.Id, SeriesElementId());
            SvgRenderer.RenderPath(builder, options);
            DynamicOptions.PathId.Add(options.Id);
            DynamicOptions.CurrentDirection.Add(options.Direction);
        }

        internal void AppendLinePath(RenderTreeBuilder builder, PathOptions options)
        {
            options.Direction = ChartHelper.AppendPathElements(Owner, options.Direction, options.Id);
            Owner.SvgRenderer.RenderPath(builder, options);
            DynamicOptions.PathId.Add(options.Id);
            DynamicOptions.CurrentDirection.Add(options.Direction);
        }

        private static bool IsValid(object data)
        {
            return double.TryParse(Convert.ToString(data, null), out double x);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            Chart = null;
        }
    }
}