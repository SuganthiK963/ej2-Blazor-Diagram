using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarAreaSeriesRenderer : PolarRadarAreaSeriesRenderer
    {
        private double origin;

        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            options = new PathOptions()
            {
                Id = name,
                Fill = Interior,
                StrokeWidth = Series.Border.Width,
                Stroke = Series.Border.Color,
                Opacity = Series.Opacity,
                StrokeDashArray = Series.DashArray,
                Direction = Series.Renderer.Points.Count > 1 && Direction.Length != 0 ? Direction.ToString() : string.Empty
            };
            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(name, AnimationType.PolarRadar);
            }
        }

        private void CalculateDirection()
        {
            bool isClosed = Series.IsClosed;
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            double pointX, pointY;
            ChartInternalLocation startPoint = null;
            origin = GetFirstLastVisiblePoint(Series.Renderer.Points).First.YValue;
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange((i - 1 > -1) && (i - 1 < count) ? visiblePoints[i - 1] : null, point, i + 1 < count ? visiblePoints[i + 1] : null, XAxisRenderer))
                {
                    pointX = point.XValue;
                    pointY = point.YValue;
                    GetPolarAreaPathDirection(pointX, origin, Series, Owner.RequireInvertedAxis, startPoint, "M");
                    startPoint = startPoint != null ? startPoint : new ChartInternalLocation(pointX, origin);
                    GetPolarAreaPathDirection(
                        pointX, point.YValue, Series, Owner.RequireInvertedAxis, null, "L");
                    if (i + 1 < visiblePoints.Count ? (visiblePoints[i + 1] != null && (!visiblePoints[i + 1].Visible && WithinYRange(visiblePoints[i + 1], YAxisRenderer))) && !(Series.EmptyPointSettings.Mode == EmptyPointMode.Drop) : false)
                    {
                        GetAreaEmptyDirection(new ChartInternalLocation(pointX, origin), startPoint, Series, Owner.RequireInvertedAxis);
                        startPoint = null;
                    }

                    StorePointLocation(point, Series, Owner.RequireInvertedAxis);
                }
            }

            if (Direction.Length != 0)
            {
                Direction.Append(" Z");
            }

            if (Series.Renderer.Points.Count > 1 && Direction.Length != 0)
            {
                GetPolarAreaPathDirection(Series.Renderer.Points[Series.Renderer.Points.Count - 1].XValue, origin, Series, Owner.RequireInvertedAxis, null, "L");
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
