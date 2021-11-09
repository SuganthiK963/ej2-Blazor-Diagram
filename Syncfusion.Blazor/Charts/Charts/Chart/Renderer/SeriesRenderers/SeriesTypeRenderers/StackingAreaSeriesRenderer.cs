using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class StackingAreaSeriesRenderer : LineBaseSeriesRenderer
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

        internal void CalculateDirection()
        {
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            if (!Series.Visible)
            {
                return;
            }

            int pointsLength = visiblePoints.Count;
            StackValues stackedvalue = Series.Renderer.StackedValues;
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, stackedvalue.StartValues[0]);
            double startPoint = 0;
            ChartInternalLocation point1, point2;
            if (pointsLength > 0)
            {
                point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[0].XValue), YAxisRenderer.GetPointValue(origin), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                Direction.Append("M" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            }

            for (int i = 0; i < visiblePoints.Count; i++)
            {
                Point point = visiblePoints[i];
                point.Regions = new List<Rect>();
                point.SymbolLocations = new List<ChartInternalLocation>();
                Point nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoints[i - 1] : null, visiblePoints[i], i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null, XAxisRenderer))
                {
                    point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(stackedvalue.EndValues[i]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                    Direction.Append("L" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                    point.SymbolLocations.Add(point1);
                    point.Regions.Add(new Rect(point.SymbolLocations[0].X - Series.Marker.Width, point.SymbolLocations[0].Y - Series.Marker.Height, 2 * Series.Marker.Width, 2 * Series.Marker.Height));
                }
                else if (Series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
                {
                    for (int j = i - 1; j >= startPoint; j--)
                    {
                        point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[j].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[j]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                        Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
                    }

                    if (i + 1 < visiblePoints.Count ? visiblePoints[i + 1] != null && visiblePoints[i + 1].Visible : false)
                    {
                        point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[i + 1].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[i + 1]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                        Direction.Append("M" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                    }

                    startPoint = i + 1;
                }
            }

            for (int j = pointsLength - 1; j >= startPoint; j--)
            {
                ChartSeries previousSeries = GetPreviousSeries(Series);
                if (previousSeries.EmptyPointSettings.Mode != EmptyPointMode.Drop || !previousSeries.Renderer.Points[j].IsEmpty)
                {
                    point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(visiblePoints[j].XValue), YAxisRenderer.GetPointValue(stackedvalue.StartValues[j]), XAxisRenderer, YAxisRenderer, XLength, YLength, Owner.RequireInvertedAxis);
                    Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
                }
            }
        }

        private ChartSeries GetPreviousSeries(ChartSeries series)
        {
            List<ChartSeries> seriesCollection = Owner.SeriesContainer.Elements.Cast<ChartSeries>().ToList();
            for (int i = 0, length = seriesCollection.Count; i < length; i++)
            {
                if (series.Renderer.Index == seriesCollection[i].Renderer.Index && i != 0)
                {
                    return seriesCollection[i - 1];
                }
            }

            return seriesCollection[0];
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

    internal class StackingArea100SeriesRenderer : StackingAreaSeriesRenderer
    {
    }
}