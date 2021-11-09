using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class StepLineSeriesRenderer : LineBaseSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            options = new PathOptions(SeriesID(), Direction.ToString(), Series.DashArray, Series.Width, Interior, Series.Opacity);

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            bool isInverted = Owner.RequireInvertedAxis;
            string startPoint = "M";
            Point prevPoint = null;
            double lineLength = 0;
            ChartInternalLocation point1, point2;
            List<Point> visiblePoints = Series.Renderer.Points;
            Direction = new System.Text.StringBuilder();

            if (XAxisRenderer.Axis.ValueType == ValueType.Category && XAxisRenderer.Axis.LabelPlacement == LabelPlacement.BetweenTicks)
            {
                lineLength = 0.5;
            }

            foreach (Point point in visiblePoints)
            {
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? visiblePoints[point.Index - 1] : null, point, point.Index + 1 < visiblePoints.Count ? visiblePoints[point.Index + 1] : null, XAxisRenderer))
                {
                    if (prevPoint != null)
                    {
                        point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(prevPoint.XValue), YAxisRenderer.GetPointValue(prevPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append(startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE + "L" + SPACE + point2.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + " L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
                        startPoint = "L";
                    }
                    else
                    {
                        point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append(startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                        startPoint = "L";
                    }

                    StorePointLocation(point, Series, isInverted);
                    prevPoint = point;
                }
                else
                {
                    prevPoint = Series.EmptyPointSettings.Mode == EmptyPointMode.Drop ? prevPoint : null;
                    startPoint = Series.EmptyPointSettings.Mode == EmptyPointMode.Drop ? startPoint : "M";
                }
            }

            if (visiblePoints.Count > 0)
            {
                point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[visiblePoints.Count - 1].XValue + lineLength), YAxisRenderer.GetPointValue(visiblePoints[visiblePoints.Count - 1].YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                Direction.Append(startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
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
