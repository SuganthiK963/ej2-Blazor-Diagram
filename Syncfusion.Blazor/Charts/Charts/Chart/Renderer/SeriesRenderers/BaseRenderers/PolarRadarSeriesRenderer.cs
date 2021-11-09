using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarRadarSeriesRenderer : LineBaseSeriesRenderer
    {
        private CircleOptions markerCircularPath;

        internal override string ClipRectId()
        {
            return ClipPathId() + "_Circle";
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            double axisMin = YAxisRenderer.Axis.Minimum != null ? Convert.ToDouble(YAxisRenderer.Axis.Minimum, CultureInfo.InvariantCulture) : double.NaN;
            double axisMax = YAxisRenderer.Axis.Maximum != null ? Convert.ToDouble(YAxisRenderer.Axis.Maximum, CultureInfo.InvariantCulture) : double.NaN;
            foreach (Point point in Series.Renderer.Points)
            {
                point.Visible = point.Visible && !((YAxisRenderer.Axis.Minimum != null && point.YValue < axisMin) || (YAxisRenderer.Axis.Maximum != null && point.YValue > axisMax));
            }
        }

        protected override void CreateSeriesElements(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            CircleOptions options = new CircleOptions
            {
                Id = ClipRectId(),
                Fill = Constants.TRANSPARENT,
                Stroke = "Gray",
                StrokeWidth = 1,
                Opacity = 1,
                Cx = ((ClipRect.Width / 2) + ClipRect.X).ToString(culture),
                Cy = ((ClipRect.Height / 2) + ClipRect.Y).ToString(culture),
                R = (Series.Renderer.Owner.AxisContainer.AxisLayout.Radius + (Series.DrawType == ChartDrawType.Scatter ? Math.Max(Series.Marker.Width, Series.Marker.Height) : 0)).ToString(culture),
                Visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible"
            };
            SvgRenderer.OpenGroupElement(builder, SeriesElementId(), string.Empty, SeriesClipPath());
            SvgRenderer.RenderCircularClipPath(builder, ClipPathId(), options);
        }

        protected override void StorePointLocation(Point point, ChartSeries series, bool isInverted)
        {
            double markerWidth = !double.IsNaN(Series.Marker.Width) ? Series.Marker.Width : 0;
            double markerHeight = !double.IsNaN(Series.Marker.Height) ? Series.Marker.Height : 0;
            point?.SymbolLocations.Add(ChartHelper.TransformToVisible(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer.Axis, YAxisRenderer.Axis, series));
            point.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
        }

        internal override string MarkerClipRectId()
        {
            return MarkerClipPathID() + "_Circle";
        }

        internal override void CalculateMarkerClipPath()
        {
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            double explodeValue = Series.Marker.Border.Width + 13;
            bool isZoomed = false;

            // chart.ZoomingModule != null ? chart.ZoomingModule.IsZoomed : false;
            if (Series.Marker.Visible)
            {
                double markerHeight = isZoomed ? 0 : ((Series.Marker.Height + explodeValue) / 2);
                double markerWidth = isZoomed ? 0 : ((Series.Marker.Width + explodeValue) / 2);
                markerCircularPath = new CircleOptions(
                    MarkerClipRectId(),
                    ((ClipRect.Width / 2) + ClipRect.X).ToString(Culture),
                    ((ClipRect.Height / 2) + ClipRect.Y).ToString(Culture),
                    Convert.ToString(Owner.AxisContainer.AxisLayout.Radius + Math.Max(markerHeight, markerWidth), Culture),
                    null,
                    1,
                    "Gray",
                    1,
                    Constants.TRANSPARENT,
                    visibility);
            }
        }

        internal override void RenderMarkerClipPath(RenderTreeBuilder builder)
        {
            Owner.SvgRenderer.OpenGroupElement(builder, SeriesSymbolId(), string.Empty, MarkerClipPath());
            Owner.SvgRenderer.RenderCircularClipPath(builder, MarkerClipPath(), markerCircularPath);
        }
    }

    internal class PolarRadarAreaSeriesRenderer : AreaBaseSeriesRenderer
    {
        private CircleOptions markerCircularPath;

        internal override string ClipRectId()
        {
            return ClipPathId() + "_Circle";
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            double axisMin = YAxisRenderer.Axis.Minimum != null ? Convert.ToDouble(YAxisRenderer.Axis.Minimum, CultureInfo.InvariantCulture) : double.NaN;
            double axisMax = YAxisRenderer.Axis.Maximum != null ? Convert.ToDouble(YAxisRenderer.Axis.Maximum, CultureInfo.InvariantCulture) : double.NaN;
            foreach (Point point in Series.Renderer.Points)
            {
                point.Visible = point.Visible && !((YAxisRenderer.Axis.Minimum != null && point.YValue < axisMin) || (YAxisRenderer.Axis.Maximum != null && point.YValue > axisMax));
            }
        }

        protected override void CreateSeriesElements(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            CircleOptions options = new CircleOptions
            {
                Id = ClipRectId(),
                Fill = Constants.TRANSPARENT,
                Stroke = "Gray",
                StrokeWidth = 1,
                Opacity = 1,
                Cx = ((ClipRect.Width / 2) + ClipRect.X).ToString(culture),
                Cy = ((ClipRect.Height / 2) + ClipRect.Y).ToString(culture),
                R = (Series.Renderer.Owner.AxisContainer.AxisLayout.Radius + (Series.DrawType == ChartDrawType.Scatter ? Math.Max(Series.Marker.Width, Series.Marker.Height) : 0)).ToString(culture),
                Visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible"
            };
            SvgRenderer.OpenGroupElement(builder, SeriesElementId(), string.Empty, SeriesClipPath());
            SvgRenderer.RenderCircularClipPath(builder, ClipPathId(), options);
        }

        protected override void StorePointLocation(Point point, ChartSeries series, bool isInverted)
        {
            double markerWidth = !double.IsNaN(Series.Marker.Width) ? Series.Marker.Width : 0;
            double markerHeight = !double.IsNaN(Series.Marker.Height) ? Series.Marker.Height : 0;
            point?.SymbolLocations.Add(ChartHelper.TransformToVisible(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer.Axis, YAxisRenderer.Axis, series));
            point.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
        }

        internal override void GetAreaPathDirection(double x, double y, ChartSeries series, bool isInverted, ChartInternalLocation startPoint, string startPath)
        {
            if (startPoint == null)
            {
                ChartInternalLocation firstPoint = ChartHelper.TransformToVisible(x, y, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
                Direction.Append(string.Join(string.Empty, startPath, SPACE, Convert.ToInt32(firstPoint.X).ToString(Culture), SPACE, Convert.ToInt32(firstPoint.Y).ToString(Culture), SPACE));
            }
        }

        internal override string MarkerClipRectId()
        {
            return MarkerClipPathID() + "_Circle";
        }

        internal override void CalculateMarkerClipPath()
        {
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            double explodeValue = Series.Marker.Border.Width + 13;
            bool isZoomed = false;

            // chart.ZoomingModule != null ? chart.ZoomingModule.IsZoomed : false;
            if (Series.Marker.Visible)
            {
                double markerHeight = isZoomed ? 0 : ((Series.Marker.Height + explodeValue) / 2);
                double markerWidth = isZoomed ? 0 : ((Series.Marker.Width + explodeValue) / 2);
                markerCircularPath = new CircleOptions(
                    MarkerClipRectId(),
                    ((ClipRect.Width / 2) + ClipRect.X).ToString(Culture),
                    ((ClipRect.Height / 2) + ClipRect.Y).ToString(Culture),
                    Convert.ToString(Owner.AxisContainer.AxisLayout.Radius + Math.Max(markerHeight, markerWidth), Culture),
                    null,
                    1,
                    "Gray",
                    1,
                    Constants.TRANSPARENT,
                    visibility);
            }
        }

        internal override void RenderMarkerClipPath(RenderTreeBuilder builder)
        {
            Owner.SvgRenderer.OpenGroupElement(builder, SeriesSymbolId(), string.Empty, MarkerClipPath());
            Owner.SvgRenderer.RenderCircularClipPath(builder, MarkerClipPath(), markerCircularPath);
        }
    }

    internal class PolarRadarSplineSeriesRenderer : SplineBaseSeriesRenderer
    {
        private CircleOptions markerCircularPath;

        internal override string ClipRectId()
        {
            return ClipPathId() + "_Circle";
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            double axisMin = YAxisRenderer.Axis.Minimum != null ? Convert.ToDouble(YAxisRenderer.Axis.Minimum, CultureInfo.InvariantCulture) : double.NaN;
            double axisMax = YAxisRenderer.Axis.Maximum != null ? Convert.ToDouble(YAxisRenderer.Axis.Maximum, CultureInfo.InvariantCulture) : double.NaN;
            foreach (Point point in Series.Renderer.Points)
            {
                point.Visible = point.Visible && !((YAxisRenderer.Axis.Minimum != null && point.YValue < axisMin) || (YAxisRenderer.Axis.Maximum != null && point.YValue > axisMax));
            }
        }

        protected override void CreateSeriesElements(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            CircleOptions options = new CircleOptions
            {
                Id = ClipRectId(),
                Fill = Constants.TRANSPARENT,
                Stroke = "Gray",
                StrokeWidth = 1,
                Opacity = 1,
                Cx = ((ClipRect.Width / 2) + ClipRect.X).ToString(culture),
                Cy = ((ClipRect.Height / 2) + ClipRect.Y).ToString(culture),
                R = (Series.Renderer.Owner.AxisContainer.AxisLayout.Radius + (Series.DrawType == ChartDrawType.Scatter ? Math.Max(Series.Marker.Width, Series.Marker.Height) : 0)).ToString(culture),
                Visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible"
            };
            SvgRenderer.OpenGroupElement(builder, SeriesElementId(), string.Empty, SeriesClipPath());
            SvgRenderer.RenderCircularClipPath(builder, ClipPathId(), options);
        }

        protected override void StorePointLocation(Point point, ChartSeries series, bool isInverted)
        {
            double markerWidth = !double.IsNaN(Series.Marker.Width) ? Series.Marker.Width : 0;
            double markerHeight = !double.IsNaN(Series.Marker.Height) ? Series.Marker.Height : 0;
            point?.SymbolLocations.Add(ChartHelper.TransformToVisible(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer.Axis, YAxisRenderer.Axis, series));
            point.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
        }

        internal override string MarkerClipRectId()
        {
            return MarkerClipPathID() + "_Circle";
        }

        internal override void CalculateMarkerClipPath()
        {
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            double explodeValue = Series.Marker.Border.Width + 13;
            bool isZoomed = false;

            // chart.ZoomingModule != null ? chart.ZoomingModule.IsZoomed : false;
            if (Series.Marker.Visible)
            {
                double markerHeight = isZoomed ? 0 : ((Series.Marker.Height + explodeValue) / 2);
                double markerWidth = isZoomed ? 0 : ((Series.Marker.Width + explodeValue) / 2);
                markerCircularPath = new CircleOptions(
                    MarkerClipRectId(),
                    ((ClipRect.Width / 2) + ClipRect.X).ToString(Culture),
                    ((ClipRect.Height / 2) + ClipRect.Y).ToString(Culture),
                    Convert.ToString(Owner.AxisContainer.AxisLayout.Radius + Math.Max(markerHeight, markerWidth), Culture),
                    null,
                    1,
                    "Gray",
                    1,
                    Constants.TRANSPARENT,
                    visibility);
            }
        }

        internal override void RenderMarkerClipPath(RenderTreeBuilder builder)
        {
            Owner.SvgRenderer.OpenGroupElement(builder, SeriesSymbolId(), string.Empty, MarkerClipPath());
            Owner.SvgRenderer.RenderCircularClipPath(builder, MarkerClipPath(), markerCircularPath);
        }
    }

    internal class PolarRadarColumnSeriesRenderer : ColumnBaseRenderer
    {
        private CircleOptions markerCircularPath;

        internal override string ClipRectId()
        {
            return ClipPathId() + "_Circle";
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            double axisMin = YAxisRenderer.Axis.Minimum != null ? Convert.ToDouble(YAxisRenderer.Axis.Minimum, CultureInfo.InvariantCulture) : double.NaN;
            double axisMax = YAxisRenderer.Axis.Maximum != null ? Convert.ToDouble(YAxisRenderer.Axis.Maximum, CultureInfo.InvariantCulture) : double.NaN;
            foreach (Point point in Series.Renderer.Points)
            {
                point.Visible = point.Visible && !((YAxisRenderer.Axis.Minimum != null && point.YValue < axisMin) || (YAxisRenderer.Axis.Maximum != null && point.YValue > axisMax));
            }
        }

        protected override void CreateSeriesElements(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            CultureInfo culture = CultureInfo.InvariantCulture;
            CircleOptions options = new CircleOptions
            {
                Id = ClipRectId(),
                Fill = Constants.TRANSPARENT,
                Stroke = "Gray",
                StrokeWidth = 1,
                Opacity = 1,
                Cx = ((ClipRect.Width / 2) + ClipRect.X).ToString(culture),
                Cy = ((ClipRect.Height / 2) + ClipRect.Y).ToString(culture),
                R = (Series.Renderer.Owner.AxisContainer.AxisLayout.Radius + (Series.DrawType == ChartDrawType.Scatter ? Math.Max(Series.Marker.Width, Series.Marker.Height) : 0)).ToString(culture),
                Visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible"
            };
            SvgRenderer.OpenGroupElement(builder, SeriesElementId(), string.Empty, SeriesClipPath());
            SvgRenderer.RenderCircularClipPath(builder, ClipPathId(), options);
        }

        internal override string MarkerClipRectId()
        {
            return MarkerClipPathID() + "_Circle";
        }

        internal override void CalculateMarkerClipPath()
        {
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            double explodeValue = Series.Marker.Border.Width + 13;
            bool isZoomed = false;

            // chart.ZoomingModule != null ? chart.ZoomingModule.IsZoomed : false;
            if (Series.Marker.Visible)
            {
                double markerHeight = isZoomed ? 0 : ((Series.Marker.Height + explodeValue) / 2);
                double markerWidth = isZoomed ? 0 : ((Series.Marker.Width + explodeValue) / 2);
                markerCircularPath = new CircleOptions(
                    MarkerClipRectId(),
                    ((ClipRect.Width / 2) + ClipRect.X).ToString(Culture),
                    ((ClipRect.Height / 2) + ClipRect.Y).ToString(Culture),
                    Convert.ToString(Owner.AxisContainer.AxisLayout.Radius + Math.Max(markerHeight, markerWidth), Culture),
                    null,
                    1,
                    "Gray",
                    1,
                    Constants.TRANSPARENT,
                    visibility);
            }
        }

        internal override void RenderMarkerClipPath(RenderTreeBuilder builder)
        {
            Owner.SvgRenderer.OpenGroupElement(builder, SeriesSymbolId(), string.Empty, MarkerClipPath());
            Owner.SvgRenderer.RenderCircularClipPath(builder, MarkerClipPath(), markerCircularPath);
        }

        protected override void Animate()
        {
            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(SeriesElementId(), AnimationType.PolarRadar);
            }
        }
    }
}
