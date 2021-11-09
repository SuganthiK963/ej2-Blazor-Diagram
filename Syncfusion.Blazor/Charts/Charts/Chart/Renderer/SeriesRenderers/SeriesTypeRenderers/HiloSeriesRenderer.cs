using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class HiloSeriesRenderer : FinancialBaseRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                CalculateHiloPathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateHiloPathOption();
            base.UpdateDirection();
        }

        private void CalculateHiloPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
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
                    Rect region = GetRectangle(point.XValue + sideBySideInfo.Median, Convert.ToDouble(point.High, Culture), point.XValue + sideBySideInfo.Median, Convert.ToDouble(point.Low, Culture));
                    PointRenderEventArgs argsData = TriggerEvent(point, Interior, new BorderModel() { Width = Series.Width, Color = string.Empty });
                    if (!argsData.Cancel)
                    {
                        if (!Series.Container.RequireInvertedAxis)
                        {
                            region.Width = argsData.Border.Width;
                            region.X = region.X - (region.Width / 2);
                        }
                        else
                        {
                            region.Height = argsData.Border.Width;
                            region.Y = region.Y - (region.Height / 2);
                        }

                        UpdateSymbolLocation(point, region);
                        id = pointId + point.Index;
                        direction = CalculateRectangle(point, region, id);
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, Series.Width, argsData.Fill, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText);
                            option.Visibility = visibility;
                            ColumnPathOptions.Add(option);
                        }
                    }
                }
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

        internal override SeriesValueType SeriesType()
        {
            return SeriesValueType.HighLow;
        }

        internal override bool FindVisibility(Point point)
        {
            FinancialPoint financialpoint = point as FinancialPoint;
            SetHiloMinMax(Convert.ToDouble(financialpoint.XValue), Convert.ToDouble(financialpoint.High, Culture), Convert.ToDouble(financialpoint.Low, Culture));
            return point.X.Equals(null) || financialpoint.Low.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Low, null)) || financialpoint.High.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.High, null));
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
                    financialpoint.High = financialpoint.Low = 0;
                    break;
                case EmptyPointMode.Average:
                    financialpoint.High = financialpoint.High.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.High, Culture)) ? GetAverage(type, Series.High, i) : financialpoint.High;
                    financialpoint.Low = financialpoint.Low.Equals(null) || double.IsNaN(Convert.ToDouble(financialpoint.Low, Culture)) ? GetAverage(type, Series.Low, i) : financialpoint.Low;
                    financialpoint.Visible = true;
                    break;
                case EmptyPointMode.Drop:
                case EmptyPointMode.Gap:
                    point.Visible = false;
                    break;
            }
        }
    }
}