using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class RangeColumnSeriesRenderer : FinancialBaseRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                CalculateColumnPathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateColumnPathOption();
            base.UpdateDirection();
        }

        private void CalculateColumnPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            string direction;
            string pointId = Series.Container.ID + "_Series_" + Index + "_Point_", id;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            PathOptions option;
            foreach (FinancialPoint rangePoint in Points)
            {
                rangePoint.SymbolLocations = new List<ChartInternalLocation>();
                rangePoint.Regions = new List<Rect>();
                if (rangePoint.Visible && ChartHelper.WithInRange(rangePoint.Index - 1 > -1 ? Series.Renderer.Points[rangePoint.Index - 1] : null, rangePoint, rangePoint.Index + 1 < Series.Renderer.Points.Count ? Series.Renderer.Points[rangePoint.Index + 1] : null, Series.Renderer.XAxisRenderer))
                {
                    Rect rect = GetRectangle(rangePoint.XValue + sideBySideInfo.Start, Convert.ToDouble(rangePoint.High, Culture), rangePoint.XValue + sideBySideInfo.End, Convert.ToDouble(rangePoint.Low, Culture));
                    PointRenderEventArgs argsData = TriggerEvent(rangePoint, Interior, new BorderModel { Width = Series.Border.Width, Color = Series.Border.Color });
                    if (!argsData.Cancel)
                    {
                        UpdateSymbolLocation(rangePoint, rect);
                        id = pointId + rangePoint.Index;
                        direction = CalculateRectangle(rangePoint, rect, id);
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, argsData.Border.Width, argsData.Border.Color, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText);
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
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }

        internal override SeriesValueType SeriesType()
        {
            return SeriesValueType.HighLow;
        }

        internal override bool ShouldRenderMarker()
        {
            return true;
        }

        internal override bool FindVisibility(Point point)
        {
            FinancialPoint rangepoint = point as FinancialPoint;
            if (rangepoint.High == null || rangepoint.Low == null)
            {
                YMin = Math.Min(YMin, 0);
                YMax = Math.Max(YMax, 0);
                XMin = Math.Min(XMin, (double)rangepoint.XValue);
                XMax = Math.Max(XMax, (double)rangepoint.XValue);
                return rangepoint.High == null || rangepoint.Low == null;
            }

            SetHiloMinMax((double)rangepoint.XValue, (double)rangepoint.High, (double)rangepoint.Low);
            return point.X.Equals(null) || double.IsNaN((double)rangepoint.Low) || double.IsNaN((double)rangepoint.High);
        }

        internal override void SetEmptyPoint(Point point, int i, Type type)
        {
            FinancialPoint rangepoint = point as FinancialPoint;

            if (!FindVisibility(rangepoint))
            {
                rangepoint.Visible = true;
                return;
            }

            rangepoint.IsEmpty = true;
            switch (Series.EmptyPointSettings.Mode)
            {
                case EmptyPointMode.Zero:
                    rangepoint.Visible = true;
                    rangepoint.High = rangepoint.Low = 0;
                    break;
                case EmptyPointMode.Average:
                    rangepoint.High = rangepoint.High == null || double.IsNaN((double)rangepoint.High) ? GetAverage(type, Series.High, i) : rangepoint.High;
                    rangepoint.Low = rangepoint.Low == null || double.IsNaN((double)rangepoint.Low) ? GetAverage(type, Series.Low, i) : rangepoint.Low;
                    rangepoint.Visible = true;
                    break;
                case EmptyPointMode.Drop:
                case EmptyPointMode.Gap:
                    rangepoint.Visible = false;
                    break;
            }
        }
    }
}