using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarStackingAreaSeriesRenderer : PolarRadarSeriesRenderer
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
                AnimationOptions = new AnimationOptions(SeriesID(), AnimationType.Progressive);
            }
        }

        private void CalculateDirection()
        {
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            if (!Series.Visible)
            {
                return;
            }

            int pointsLength = visiblePoints.Count;
            StackValues stackedvalue = Series.Renderer.StackedValues;
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, stackedvalue.EndValues[0]);
            double startPoint = 0;
            ChartInternalLocation point1, point2;
            if (pointsLength > 0)
            {
                point1 = ChartHelper.TransformToVisible(visiblePoints[0].XValue, origin, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
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
                    point1 = ChartHelper.TransformToVisible(point.XValue, stackedvalue.EndValues[i], XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                    Direction.Append("L" + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                    point.SymbolLocations.Add(point1);
                    point.Regions.Add(new Rect(point.SymbolLocations[0].X - Series.Marker.Width, point.SymbolLocations[0].Y - Series.Marker.Height, 2 * Series.Marker.Width, 2 * Series.Marker.Height));
                }
            }

            if (visiblePoints.Count > 1)
            {
                ConnectPoints connectPoints = GetFirstLastVisiblePoint(Series.Renderer.Points);
                point1 = new ChartInternalLocation(connectPoints.First.XValue, stackedvalue.EndValues[connectPoints.First.Index]);
                point2 = ChartHelper.TransformToVisible(point1.X, point1.Y, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                Direction.Append("L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
            }

            if (Series.Renderer.Index != GetFirstSeriesIndex())
            {
                for (int j = pointsLength - 1; j >= startPoint; j--)
                {
                    if (!visiblePoints[j].Visible)
                    {
                        continue;
                    }

                    ChartSeries previousSeries = GetPreviousSeries(Series);
                    if (previousSeries.EmptyPointSettings.Mode != EmptyPointMode.Drop || !previousSeries.Renderer.Points[j].IsEmpty)
                    {
                        point2 = ChartHelper.TransformToVisible(visiblePoints[j].XValue, stackedvalue.StartValues[j], XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                        Direction.Append(((j == (pointsLength - 1)) ? "M" : "L") + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
                    }
                }
            }
        }

        private double GetFirstSeriesIndex()
        {
            foreach (ChartSeries series in Owner.SeriesContainer.Elements)
            {
                if (series.Visible)
                {
                    return series.Renderer.Index;
                }
            }

            return 0;
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
}