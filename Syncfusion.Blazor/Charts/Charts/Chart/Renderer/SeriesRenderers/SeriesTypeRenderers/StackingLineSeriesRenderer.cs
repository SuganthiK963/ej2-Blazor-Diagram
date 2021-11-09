using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class StackingLineSeriesRenderer : LineBaseSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            options = new PathOptions()
            {
                Id = name,
                Fill = Constants.TRANSPARENT,
                StrokeWidth = Series.Width,
                Stroke = Interior,
                Opacity = Series.Opacity,
                StrokeDashArray = Series.DashArray,
                Direction = Direction.ToString()
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        internal void CalculateDirection()
        {
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            if (!Series.Visible)
            {
                return;
            }

            StackValues stackedvalue = Series.Renderer.StackedValues;
            ChartInternalLocation point1;
            int j = 0;
            for (int i = 0; i < visiblePoints.Count; i++)
            {
                Point point = visiblePoints[i];
                point.Regions = new List<Rect>();
                point.SymbolLocations = new List<ChartInternalLocation>();
                Point nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoints[i - 1] : null, point, nextPoint, XAxisRenderer))
                {
                    point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[i]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                    Direction.Append((j != 0 ? "L" : "M") + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                    point.SymbolLocations.Add(point1);
                    point.Regions.Add(new Rect(point.SymbolLocations[0].X - Series.Marker.Width, point.SymbolLocations[0].Y - Series.Marker.Height, 2 * Series.Marker.Width, 2 * Series.Marker.Height));
                    j++;
                }
                else if (Series.EmptyPointSettings.Mode != EmptyPointMode.Drop && nextPoint != null && nextPoint.Visible)
                {
                    point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(nextPoint.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[i + 1]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                    Direction.Append("M" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
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

    internal class StackingLine100SeriesRenderer : StackingLineSeriesRenderer
    {
    }
}