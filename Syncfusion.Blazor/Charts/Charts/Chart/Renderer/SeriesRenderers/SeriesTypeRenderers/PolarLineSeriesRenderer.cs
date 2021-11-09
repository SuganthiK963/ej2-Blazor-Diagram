using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarLineSeriesRenderer : PolarRadarSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            options = new PathOptions(name, Direction.ToString(), Series.DashArray, Series.Width, Interior, Series.Opacity);
            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(name, AnimationType.PolarRadar);
            }
        }

        private void CalculateDirection()
        {
            bool isClosed = Series.IsClosed;
            bool isDrop = Series.EmptyPointSettings.Mode == EmptyPointMode.Drop;
            string startPoint = "M";
            Direction = new System.Text.StringBuilder();
            List<Point> visiblePoints = EnableComplexProperty();
            double prevPointX = double.NaN, prevPointY = double.NaN, pointX, pointY;
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange((i - 1 > -1) && (i - 1 < count) ? visiblePoints[i - 1] : null, point, i + 1 < count ? visiblePoints[i + 1] : null, XAxisRenderer))
                {
                    pointX = point.XValue;
                    pointY = point.YValue;
                    GetPolarLineDirection(prevPointX, prevPointY, pointX, pointY, Owner.RequireInvertedAxis, startPoint);
                    startPoint = !double.IsNaN(prevPointX) ? "L" : startPoint;
                    prevPointX = pointX;
                    prevPointY = pointY;
                    StorePointLocation(point, Series, Owner.RequireInvertedAxis);
                }
                else
                {
                    prevPointX = isDrop ? prevPointX : double.NaN;
                    prevPointY = isDrop ? prevPointY : double.NaN;
                    startPoint = "M";
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }
            }

            if (isClosed)
            {
                ConnectPoints points = GetFirstLastVisiblePoint(visiblePoints);
                ChartInternalLocation point2 = ChartHelper.TransformToVisible(points.Last.XValue, points.Last.YValue, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                ChartInternalLocation point1 = ChartHelper.TransformToVisible(points.First.XValue, points.First.YValue, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                Direction.Append(string.Join(string.Empty, startPoint, SPACE, Convert.ToInt32(point2.X), SPACE, Convert.ToInt32(point2.Y), SPACE, 'L', SPACE, Convert.ToInt32(point1.X), SPACE, Convert.ToInt32(point1.Y), SPACE));
            }
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            options.Direction = Direction.ToString();
            base.UpdateDirection();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElement(builder, options);
            builder.CloseElement();
        }
    }
}
