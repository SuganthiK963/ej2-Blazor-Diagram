using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class HiloOpenCloseSeriesRenderer : FinancialBaseRenderer
    {
        private static void UpdateTickRegion(bool horizontal, Rect region, double borderWidth)
        {
            if (horizontal)
            {
                region.X -= borderWidth / 2;
                region.Width = borderWidth;
            }
            else
            {
                region.Y -= borderWidth / 2;
                region.Height = borderWidth;
            }
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                CalculateHiloOpenClosePathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateHiloOpenClosePathOption();
            base.UpdateDirection();
        }

        private void CalculateHiloOpenClosePathOption()
        {
            int index1, index2;
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            double borderWidth = Series.Border.Width;
            string direction;
            string pointId = Series.Container.ID + "_Series_" + Index + "_Point_", id;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            PathOptions option;
            foreach (FinancialPoint point in Points)
            {
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? Points[point.Index - 1] : null, point, point.Index + 1 < Points.Count ? Points[point.Index + 1] : null, XAxisRenderer))
                {
                    Rect highLowRect = GetRectangle(point.XValue + sideBySideInfo.Start, Math.Max(Convert.ToDouble(point.High, Culture), Convert.ToDouble(point.Low, Culture)), point.XValue + sideBySideInfo.End, Math.Min(Convert.ToDouble(point.High, Culture), Convert.ToDouble(point.Low, Culture)));
                    point.Regions.Add(GetRectangle(point.XValue + sideBySideInfo.Median, Math.Max(Convert.ToDouble(point.High, Culture), Convert.ToDouble(point.Low, Culture)), point.XValue + sideBySideInfo.Median, Math.Min(Convert.ToDouble(point.High, Culture), Convert.ToDouble(point.Low, Culture))));
                    UpdateTickRegion(!Series.Container.RequireInvertedAxis, point.Regions[0], borderWidth);
                    point.Regions.Add(GetRectangle(point.XValue + sideBySideInfo.Start, Math.Max(Convert.ToDouble(point.Open, Culture), Convert.ToDouble(point.Close, Culture)), point.XValue + sideBySideInfo.Median, Math.Max(Convert.ToDouble(point.Open, Culture), Convert.ToDouble(point.Close, Culture))));
                    point.Regions.Add(GetRectangle(point.XValue + sideBySideInfo.Median, Math.Min(Convert.ToDouble(point.Open, Culture), Convert.ToDouble(point.Close, Culture)), point.XValue + sideBySideInfo.End, Math.Min(Convert.ToDouble(point.Open, Culture), Convert.ToDouble(point.Close, Culture))));
                    PointRenderEventArgs argsData = TriggerEvent(point, (Convert.ToDouble(point.Open, Culture) <= Convert.ToDouble(point.Close, Culture)) ? Series.BearFillColor : Series.BullFillColor, new BorderModel { Width = borderWidth, Color = string.Empty });
                    if (!argsData.Cancel)
                    {
                        UpdateSymbolLocation(point, point.Regions[0]);
                        index1 = Convert.ToDouble(point.Open, Culture) > Convert.ToDouble(point.Close, Culture) ? 1 : 2;
                        index2 = Convert.ToDouble(point.Open, Culture) > Convert.ToDouble(point.Close, Culture) ? 2 : 1;
                        id = pointId + point.Index;
                        direction = DrawHiloOpenClosePath(new ChartInternalLocation(point.Regions[index1].X, point.Regions[index1].Y), new ChartInternalLocation(point.Regions[index2].X, point.Regions[index2].Y), highLowRect);
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, borderWidth, argsData.Fill, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText);
                            option.Visibility = visibility;
                            ColumnPathOptions.Add(option);
                        }
                    }

                    UpdateTickRegion(Series.Container.RequireInvertedAxis, point.Regions[1], borderWidth);
                    UpdateTickRegion(Series.Container.RequireInvertedAxis, point.Regions[2], borderWidth);
                }
            }
        }

        protected string DrawHiloOpenClosePath(ChartInternalLocation open, ChartInternalLocation close, Rect rect)
        {
            string direction = string.Empty;
            if (rect != null && open != null && close != null)
            {
                if (Series.Container.RequireInvertedAxis)
                {
                    direction = string.Join(string.Empty, "M", SPACE, rect.X.ToString(Culture), SPACE, (rect.Y + (rect.Height / 2)).ToString(Culture), SPACE, "L", SPACE, (rect.X + rect.Width).ToString(Culture), SPACE, (rect.Y + (rect.Height / 2)).ToString(Culture), SPACE);
                    direction = string.Join(string.Empty, direction, "M", SPACE, open.X.ToString(Culture), SPACE, (rect.Y + (rect.Height / 2)).ToString(Culture), SPACE, "L", SPACE, open.X.ToString(Culture), SPACE, (rect.Y + rect.Height).ToString(Culture), SPACE);
                    direction = string.Join(string.Empty, direction, "M", SPACE, close.X.ToString(Culture), SPACE, (rect.Y + (rect.Height / 2)).ToString(Culture), SPACE, "L", SPACE, close.X.ToString(Culture), SPACE, rect.Y.ToString(Culture), SPACE);
                }
                else
                {
                    direction = string.Join(string.Empty, "M", SPACE, (rect.X + (rect.Width / 2)).ToString(Culture), SPACE, (rect.Y + rect.Height).ToString(Culture), SPACE, "L", SPACE, (rect.X + (rect.Width / 2)).ToString(Culture), SPACE, rect.Y.ToString(Culture), SPACE);
                    direction = string.Join(string.Empty, direction, "M", SPACE, rect.X.ToString(Culture), SPACE, open.Y.ToString(Culture), SPACE, "L", SPACE, (rect.X + (rect.Width / 2)).ToString(Culture), SPACE, open.Y.ToString(Culture), SPACE);
                    direction = string.Join(string.Empty, direction, "M", SPACE, (rect.X + (rect.Width / 2)).ToString(Culture), SPACE, close.Y.ToString(Culture), SPACE, "L", SPACE, (rect.X + rect.Width).ToString(Culture), SPACE, close.Y.ToString(Culture), SPACE);
                }
            }

            return direction;
        }

        internal override SeriesValueType SeriesType()
        {
            return SeriesValueType.HighLowOpenClose;
        }

        internal override bool FindVisibility(Point point)
        {
            FinancialPoint financialpoint = point as FinancialPoint;
            SetHiloMinMax(Convert.ToDouble(financialpoint.XValue), Convert.ToDouble(financialpoint.High, Culture), Convert.ToDouble(financialpoint.Low, Culture));
            return point.X.Equals(null) || financialpoint.Low.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Low, Culture)) || financialpoint.Open.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Open, Culture)) || financialpoint.Close.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Close, Culture)) || financialpoint.High.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.High, Culture));
        }

        internal override void SetEmptyPoint(Point point, int i, Type type)
        {
            if (!FindVisibility(point))
            {
                point.Visible = true;
                return;
            }

            FinancialPoint financialpoint = point as FinancialPoint;
            point.IsEmpty = true;
            switch (Series.EmptyPointSettings.Mode)
            {
                case EmptyPointMode.Zero:
                    financialpoint.Visible = true;
                    financialpoint.High = financialpoint.Low = financialpoint.Open = financialpoint.Close = Convert.ToDouble(0);
                    break;
                case EmptyPointMode.Average:
                    financialpoint.High = financialpoint.High.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.High, Culture)) ? GetAverage(type, Series.High, i) : financialpoint.High;
                    financialpoint.Low = financialpoint.Low.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Low, Culture)) ? GetAverage(type, Series.Low, i) : financialpoint.Low;
                    financialpoint.Open = financialpoint.Open.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Open, Culture)) ? GetAverage(type, Series.Open, i) : financialpoint.Open;
                    financialpoint.Close = financialpoint.Close.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Close, Culture)) ? GetAverage(type, Series.Close, i) : financialpoint.Close;
                    financialpoint.Visible = true;
                    break;
                case EmptyPointMode.Drop:
                case EmptyPointMode.Gap:
                    financialpoint.Visible = false;
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