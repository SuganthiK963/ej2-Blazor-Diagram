using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class LineSeriesRenderer : LineBaseSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            options = new PathOptions(SeriesID(), Direction.ToString(), Series.DashArray, Series.Width, Interior, Series.Opacity);
            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(SeriesID(), AnimationType.Progressive);
            }
        }

        internal void CalculateDirection()
        {
            string startPoint = "M";
            bool isDrop = Series.EmptyPointSettings.Mode == EmptyPointMode.Drop;
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
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
                    GetLineDirection(prevPointX, prevPointY, pointX, pointY, Owner.RequireInvertedAxis, startPoint);
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

    internal class ParetoLineSeriesRenderer : LineSeriesRenderer
    {
    }
}
