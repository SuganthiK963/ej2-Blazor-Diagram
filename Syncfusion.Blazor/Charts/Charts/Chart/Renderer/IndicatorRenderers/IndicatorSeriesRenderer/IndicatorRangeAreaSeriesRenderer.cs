using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class IndicatorRangeAreaSeriesRenderer : IndicatorLineBaseRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            options = new PathOptions(SeriesID(), Direction.ToString(), Series.DashArray, Series.Border.Width, Series.Border.Color, Series.Opacity, Series.Fill);

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            Direction = new System.Text.StringBuilder();
            string direction = string.Empty, command = "M";
            bool? closed = null;
            List<FinancialPoint> visiblePoints = EnableComplexProperty().Cast<FinancialPoint>().ToList();
            for (int i = 0, length = visiblePoints.Count; i < length; i++)
            {
                FinancialPoint point = visiblePoints[i];
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                double low = Math.Min((double)point.Low, (double)point.High), high = Math.Max((double)point.Low, (double)point.High), temp;
                if (YAxisRenderer.Axis.IsInversed)
                {
                    temp = low;
                    low = high;
                    high = temp;
                }

                ChartInternalLocation lowPoint = ChartHelper.GetPoint(point.XValue, low, XAxisRenderer, YAxisRenderer, Owner.RequireInvertedAxis), highPoint = ChartHelper.GetPoint(point.XValue, high, XAxisRenderer, YAxisRenderer, Owner.RequireInvertedAxis);
                point.SymbolLocations.Add(highPoint);
                point.SymbolLocations.Add(lowPoint);
                Rect rect = new Rect(Math.Min(lowPoint.X, highPoint.X), Math.Min(lowPoint.Y, highPoint.Y), Math.Max(Math.Abs(highPoint.X - lowPoint.X), Series.Marker.Width), Math.Max(Math.Abs(highPoint.Y - lowPoint.Y), Series.Marker.Width));
                if (!Owner.RequireInvertedAxis)
                {
                    rect.X -= Series.Marker.Width / 2;
                }
                else
                {
                    rect.Y -= Series.Marker.Width / 2;
                }

                point.Regions.Add(rect);
                Point nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(i - 1 > -1 ? visiblePoints[i - 1] : null, point, nextPoint, XAxisRenderer))
                {
                    direction = string.Join(string.Empty, direction, command, " ", lowPoint.X.ToString(Culture), SPACE, lowPoint.Y.ToString(Culture), SPACE);
                    closed = false;
                    if ((i + 1 < visiblePoints.Count && (nextPoint != null) ? !nextPoint.Visible : false) || i == visiblePoints.Count - 1)
                    {
                        direction = CloseRangeAreaPath(visiblePoints, direction, i);
                        command = "M";
                        direction = string.Join(string.Empty, direction, SPACE, "Z");
                        closed = true;
                    }

                    command = "L";
                }
                else
                {
                    if (closed == false && i != 0)
                    {
                        direction = CloseRangeAreaPath(visiblePoints, direction, i);
                        closed = true;
                    }

                    command = "M";
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }
            }

            Direction.Append(direction);
        }

        private string CloseRangeAreaPath(List<FinancialPoint> visiblePoints, string direction, int i)
        {
            Point point;
            for (int j = i; j >= 0; j--)
            {
                if (visiblePoints[j].Visible && (visiblePoints[j].SymbolLocations.Count > 0) ? visiblePoints[j].SymbolLocations[0] != null : false)
                {
                    point = visiblePoints[j];
                    direction = string.Join(string.Empty, direction, "L", SPACE, point.SymbolLocations[0].X.ToString(Culture), SPACE, point.SymbolLocations[0].Y.ToString(Culture), SPACE);
                }
                else
                {
                    break;
                }
            }

            return direction;
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            options.Direction = Direction.ToString();
            base.UpdateDirection();
        }

        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = true;
            switch (property)
            {
                case "Fill":
                    options.Fill = Interior = Series.Fill;
                    break;
            }
        }
    }
}
