using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class CandleSeriesRenderer : FinancialBaseRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                CalculateCandlePathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateCandlePathOption();
            base.UpdateDirection();
        }

        private void CalculateCandlePathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            double borderWidth = Series.Border.Width;
            string direction;
            string pointId = Series.Container.ID + "_Series_" + Index + "_Point_", id;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            PathOptions option;
            foreach (FinancialPoint point in Points)
            {
                point.Regions = new List<Rect>();
                point.SymbolLocations = new List<ChartInternalLocation>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? Points[point.Index - 1] : null, point, point.Index + 1 < Points.Count ? Points[point.Index + 1] : null, XAxisRenderer))
                {
                    Rect tickRegion = GetRectangle(point.XValue + sideBySideInfo.Median, Math.Max(Convert.ToDouble(point.High, Culture), Convert.ToDouble(point.Low, Culture)), point.XValue + sideBySideInfo.Median, Math.Min(Convert.ToDouble(point.High, Culture), Convert.ToDouble(point.Low, Culture)));
                    if (!Series.Container.RequireInvertedAxis)
                    {
                        tickRegion.X -= borderWidth / 2;
                        tickRegion.Width = borderWidth;
                    }
                    else
                    {
                        tickRegion.Y -= borderWidth / 2;
                        tickRegion.Height = borderWidth;
                    }

                    Rect centerRegion = GetRectangle(point.XValue + sideBySideInfo.Start, Math.Max(Convert.ToDouble(point.Open, Culture), Convert.ToDouble(point.Close, Culture)), point.XValue + sideBySideInfo.End, Math.Min(Convert.ToDouble(point.Open, Culture), Convert.ToDouble(point.Close, Culture)));
                    PointRenderEventArgs argsData = TriggerEvent(point, GetCandleColor(point, Series), new BorderModel { Color = Series.Border.Color, Width = Series.Border.Width });
                    id = pointId + point.Index;
                    direction = GetPathString(tickRegion, centerRegion, Series);
                    if (!argsData.Cancel)
                    {
                        UpdateSymbolLocation(point, tickRegion);
                        UpdateSymbolLocation(point, centerRegion);
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, argsData.Border.Width, argsData.Fill, Series.Opacity, !Series.EnableSolidCandles ? (Convert.ToDouble(point.Open, Culture) > Convert.ToDouble(point.Close, Culture) ? argsData.Fill : "transparent") : argsData.Fill, string.Empty, string.Empty, AccessText);
                            option.Visibility = visibility;
                            ColumnPathOptions.Add(option);
                        }
                    }
                }
            }
        }

        internal override SeriesValueType SeriesType()
        {
            return SeriesValueType.HighLowOpenClose;
        }

        private string GetPathString(Rect topRect, Rect midRect, ChartSeries series)
        {
            string direction = string.Empty;
            bool isInverted = series.Container.RequireInvertedAxis;
            double center = isInverted ? topRect.Y + (topRect.Height / 2) : topRect.X + (topRect.Width / 2);

            if (isInverted)
            {
                direction = string.Join(string.Empty, "M", SPACE, topRect.X.ToString(Culture), SPACE, center.ToString(Culture), SPACE, "L", SPACE, midRect.X.ToString(Culture), SPACE, center.ToString(Culture));
                direction = string.Join(string.Empty, direction, " M", SPACE, midRect.X.ToString(Culture), SPACE, midRect.Y.ToString(Culture), SPACE, "L", SPACE, (midRect.X + midRect.Width).ToString(Culture), SPACE, midRect.Y.ToString(Culture), SPACE, "L", SPACE, (midRect.X + midRect.Width).ToString(Culture), SPACE, (midRect.Y + midRect.Height).ToString(Culture), SPACE, "L", SPACE, midRect.X.ToString(Culture), SPACE, (midRect.Y + midRect.Height).ToString(Culture), SPACE, "Z");
                direction = string.Join(string.Empty, direction, " M", SPACE, (midRect.X + midRect.Width).ToString(Culture), SPACE, center.ToString(Culture), SPACE, "L", SPACE, (topRect.X + topRect.Width).ToString(Culture), SPACE, center.ToString(Culture));
            }
            else
            {
                direction = string.Join(string.Empty, "M", SPACE, center.ToString(Culture), SPACE, topRect.Y.ToString(Culture), SPACE, "L", SPACE, center.ToString(Culture), SPACE, midRect.Y.ToString(Culture));
                direction = string.Join(string.Empty, direction, " M", SPACE, midRect.X.ToString(Culture), SPACE, midRect.Y.ToString(Culture), SPACE, "L", SPACE, (midRect.X + midRect.Width).ToString(Culture), SPACE, midRect.Y.ToString(Culture), SPACE, "L", SPACE, (midRect.X + midRect.Width).ToString(Culture), SPACE, (midRect.Y + midRect.Height).ToString(Culture), SPACE, "L", SPACE, midRect.X.ToString(Culture), SPACE, (midRect.Y + midRect.Height).ToString(Culture), SPACE, "Z");
                direction = string.Join(string.Empty, direction, " M", SPACE, center.ToString(Culture), SPACE, (midRect.Y + midRect.Height).ToString(Culture), SPACE, "L", SPACE, center.ToString(Culture), SPACE, (topRect.Y +
                         topRect.Height).ToString(Culture));
            }

            return direction;
        }

        private string GetCandleColor(Point point, ChartSeries series)
        {
            Point previousPoint = point.Index - 1 > -1 ? Points[point.Index - 1] : null;
            FinancialPoint earlierPoint = previousPoint as FinancialPoint;
            FinancialPoint currentPoint = point as FinancialPoint;
            if (series.EnableSolidCandles == false)
            {
                if (earlierPoint == null)
                {
                    return series.BearFillColor;
                }
                else
                {
                    return (Convert.ToDouble(earlierPoint.Close, Culture) > Convert.ToDouble(currentPoint.Close, Culture)) ? series.BullFillColor : series.BearFillColor;
                }
            }
            else
            {
                return Convert.ToDouble(currentPoint.Open, Culture) > Convert.ToDouble(currentPoint.Close, Culture) ? series.BullFillColor : series.BearFillColor;
            }
        }

        internal override bool FindVisibility(Point point)
        {
            FinancialPoint candlePoint = point as FinancialPoint;
            SetHiloMinMax(candlePoint.XValue, Convert.ToDouble(candlePoint.High, Culture), Convert.ToDouble(candlePoint.Low, Culture));
            return point.X.Equals(null) || candlePoint.Low.Equals(null) || double.IsNaN(Convert.ToDouble(candlePoint.Low, Culture)) || candlePoint.Open.Equals(null) || double.IsNaN(Convert.ToDouble(candlePoint.Open, Culture)) || candlePoint.Close.Equals(null) || double.IsNaN(Convert.ToDouble(candlePoint.Close, Culture)) || candlePoint.High.Equals(null) || double.IsNaN(Convert.ToDouble(candlePoint.High, Culture));
        }

        internal override void SetEmptyPoint(Point point, int i, Type type)
        {
            if (!FindVisibility(point))
            {
                point.Visible = true;
                return;
            }

            FinancialPoint candlePoint = point as FinancialPoint;
            point.IsEmpty = true;
            switch (Series.EmptyPointSettings.Mode)
            {
                case EmptyPointMode.Zero:
                    candlePoint.Visible = true;
                    candlePoint.High = candlePoint.Low = candlePoint.Open = candlePoint.Close = 0;
                    break;
                case EmptyPointMode.Average:
                    candlePoint.High = candlePoint.High.Equals(null) || double.IsNaN((double)candlePoint.High) ? GetAverage(type, Series.High, i) : candlePoint.High;
                    candlePoint.Low = candlePoint.Low.Equals(null) || double.IsNaN((double)candlePoint.Low) ? GetAverage(type, Series.Low, i) : candlePoint.Low;
                    candlePoint.Open = candlePoint.Open.Equals(null) || double.IsNaN((double)candlePoint.Open) ? GetAverage(type, Series.Open, i) : candlePoint.Open;
                    candlePoint.Close = candlePoint.Close.Equals(null) || double.IsNaN((double)candlePoint.Close) ? GetAverage(type, Series.Close, i) : candlePoint.Close;
                    candlePoint.Visible = true;
                    break;
                case EmptyPointMode.Drop:
                case EmptyPointMode.Gap:
                    YData[i] = double.NaN;
                    point.Visible = false;
                    break;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in ColumnPathOptions)
            {
                RenderSeriesElement(builder, option);
            }

            builder.CloseElement();
        }
    }
}