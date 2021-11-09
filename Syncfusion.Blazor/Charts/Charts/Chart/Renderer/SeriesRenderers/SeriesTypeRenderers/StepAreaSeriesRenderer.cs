using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;
using System.Text;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class StepAreaSeriesRenderer : LineBaseSeriesRenderer
    {
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
                Direction = Direction.ToString()
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            bool isInverted = Owner.RequireInvertedAxis;
            ChartInternalLocation currentPoint, secondPoint, start = null;
            List<Point> visiblePoints = EnableComplexProperty();
            int pointsLength = visiblePoints.Count;
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, 0);
            Point nextPoint, prevPoint = null;
            double lineLength = 0;
            Direction = new System.Text.StringBuilder();

            if (XAxisRenderer.Axis.ValueType == ValueType.Category && XAxisRenderer.Axis.LabelPlacement == LabelPlacement.BetweenTicks)
            {
                lineLength = 0.5;
            }

            for (int i = 0; i < pointsLength; i++)
            {
                Point point = visiblePoints[i];
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoints[i - 1] : null, point, nextPoint, XAxisRenderer))
                {
                    if (start == null)
                    {
                        start = new ChartInternalLocation(point.XValue, 0);
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("M" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue - lineLength), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }

                    if (prevPoint != null)
                    {
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(prevPoint.XValue), YAxisRenderer.GetPointValue(prevPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + secondPoint.Y.ToString(Culture) + " L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }
                    else if (Series.EmptyPointSettings.Mode == EmptyPointMode.Gap)
                    {
                        currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                        Direction.Append("L" + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture) + SPACE);
                    }

                    StorePointLocation(point, Series, isInverted);
                    prevPoint = point;
                }

                if ((nextPoint != null ? !nextPoint.Visible : false) && (Series.EmptyPointSettings.Mode != EmptyPointMode.Drop))
                {
                    currentPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue + lineLength), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    Direction.Append(((start == null) ? "M" : "L") + SPACE + currentPoint.X.ToString(Culture) + SPACE + currentPoint.Y.ToString(Culture));
                    start = null;
                    prevPoint = null;
                }
            }

            if ((pointsLength > 1) && Direction.Length > 0)
            {
                start = new ChartInternalLocation(visiblePoints[pointsLength - 1].XValue + lineLength, visiblePoints[pointsLength - 1].YValue);
                secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(start.X), YAxisRenderer.GetPointValue(start.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                Direction.Append("L" + SPACE + secondPoint.X.ToString(Culture) + SPACE + secondPoint.Y.ToString(Culture) + SPACE);
                start = new ChartInternalLocation(visiblePoints[pointsLength - 1].XValue + lineLength, origin);
                secondPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(start.X), YAxisRenderer.GetPointValue(start.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                Direction.Append("L" + SPACE + secondPoint.X.ToString(Culture) + SPACE + secondPoint.Y.ToString(Culture) + SPACE);
            }
            else
            {
                Direction = new StringBuilder();
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
