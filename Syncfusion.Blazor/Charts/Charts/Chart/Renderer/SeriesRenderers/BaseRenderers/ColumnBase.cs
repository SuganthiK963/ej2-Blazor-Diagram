using System;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    public abstract class ColumnBaseRenderer : ChartSeriesRenderer
    {
        protected const string SPACE = " ";

        internal ChartHelper Helper { get; set; } = new ChartHelper();

        protected List<PathOptions> ColumnPathOptions { get; set; } = new List<PathOptions>();

        protected string AccessText { get; set; }

        private static void FindRectPosition(List<ChartSeries> seriesCollection)
        {
            Dictionary<string, double> stackingGroup = new Dictionary<string, double>();
            RectPosition visibleSeries = new RectPosition() { RectCount = 0, Position = double.NaN };
            for (int i = 0; i < seriesCollection.Count; i++)
            {
                if (seriesCollection[i].Type.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    if (!string.IsNullOrEmpty(seriesCollection[i].StackingGroup))
                    {
                        if (!stackingGroup.ContainsKey(seriesCollection[i].StackingGroup))
                        {
                            stackingGroup.Add(seriesCollection[i].StackingGroup, double.NaN);
                        }

                        if (double.IsNaN(stackingGroup[seriesCollection[i].StackingGroup]))
                        {
                            seriesCollection[i].Renderer.Position = visibleSeries.RectCount;
                            stackingGroup[seriesCollection[i].StackingGroup] = visibleSeries.RectCount++;
                        }
                        else
                        {
                            seriesCollection[i].Renderer.Position = stackingGroup[seriesCollection[i].StackingGroup];
                        }
                    }
                    else
                    {
                        if (double.IsNaN(visibleSeries.Position))
                        {
                            seriesCollection[i].Renderer.Position = visibleSeries.RectCount;
                            visibleSeries.Position = visibleSeries.RectCount++;
                        }
                        else
                        {
                            seriesCollection[i].Renderer.Position = visibleSeries.Position;
                        }
                    }
                }
                else
                {
                    seriesCollection[i].Renderer.Position = visibleSeries.RectCount++;
                }
            }

            for (int i = 0; i < seriesCollection.Count; i++)
            {
                seriesCollection[i].Renderer.RectCount = visibleSeries.RectCount;
            }
        }

        private void GetRegion(Point point, Rect rect)
        {
            if (point.Y != null ? Convert.ToDouble(point.Y, null) == 0 : false)
            {
                double markerWidth = (Series.Marker.Width > 0) ? Series.Marker.Width : 0;
                double markerHeight = (Series.Marker.Height > 0) ? Series.Marker.Height : 0;
                point.Regions.Add(new Rect(point.SymbolLocations[0].X - markerWidth, point.SymbolLocations[0].Y - markerHeight, 2 * markerWidth, 2 * markerHeight));
            }
            else
            {
                point.Regions.Add(rect);
            }
        }

        private void GetSideBySidePositions()
        {
            foreach (ChartColumnRenderer columnRenderer in Series.Container.ColumnContainer.Renderers)
            {
                foreach (ChartRowRenderer rowRenderer in Series.Container.RowContainer.Renderers)
                {
                    FindRectPosition(FindSeriesCollection(columnRenderer, rowRenderer));
                }
            }
        }

        private string CalculateRoundedRectPath(Rect rect, double topLeft, double topRight, double bottomLeft, double bottomRight)
        {
            return "M" + SPACE + rect.X.ToString(Culture) + SPACE + (topLeft + rect.Y).ToString(Culture) + " Q " + rect.X.ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + (rect.X + topLeft).ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + "L" + SPACE + (rect.X + rect.Width - topRight).ToString(Culture) + SPACE + rect.Y.ToString(Culture) +
                " Q " + (rect.X + rect.Width).ToString(Culture) + SPACE + rect.Y.ToString(Culture) + SPACE + (rect.X + rect.Width).ToString(Culture) + SPACE + (rect.Y + topRight).ToString(Culture) + SPACE + "L " + (rect.X + rect.Width).ToString(Culture) + SPACE + (rect.Y + rect.Height - bottomRight).ToString(Culture) + " Q " +
                (rect.X + rect.Width).ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + (rect.X + rect.Width - bottomRight).ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + SPACE + "L " + (rect.X + bottomLeft).ToString(Culture) + SPACE + (rect.Y + rect.Height).ToString(Culture) + " Q " + rect.X.ToString(Culture) + SPACE +
                (rect.Y + rect.Height).ToString(Culture) + SPACE + rect.X.ToString(Culture) + SPACE + (rect.Y + rect.Height - bottomLeft).ToString(Culture) + SPACE + "L" + SPACE + rect.X.ToString(Culture) + SPACE + (topLeft + rect.Y).ToString(Culture) + SPACE + "Z";
        }

        protected DoubleRange GetSideBySideInfo()
        {
            if (Series.Container.EnableSideBySidePlacement && (Series.Renderer.Position == 0 || double.IsNaN(Series.Renderer.Position)))
            {
                GetSideBySidePositions();
            }

            double rectCount = !Series.Container.EnableSideBySidePlacement ? 1 : Series.Renderer.RectCount;

            // IsRectSeries = true;
            double width = ChartHelper.GetMinPointsDelta(XAxisRenderer.Axis, Series.Container.SeriesContainer.Renderers.Cast<ChartSeriesRenderer>().ToList()) * (ChartHelper.IsNaNOrZero(Series.ColumnWidth) ? ((Series.Type == ChartSeriesType.Histogram) ? 1 : 0.7) : Series.ColumnWidth);
            double location = ((!Series.Container.EnableSideBySidePlacement ? 0 : Series.Renderer.Position) / rectCount) - 0.5;
            DoubleRange doubleRange = new DoubleRange(location, location + (1 / rectCount));
            if (!(double.IsNaN(doubleRange.Start) || double.IsNaN(doubleRange.End)))
            {
                doubleRange = new DoubleRange(doubleRange.Start * width, doubleRange.End * width);
                double radius = (Series.Container.EnableSideBySidePlacement ? Series.ColumnSpacing : 0) * doubleRange.Delta;
                doubleRange = new DoubleRange(doubleRange.Start + (radius / 2), doubleRange.End - (radius / 2));
            }

            return doubleRange;
        }

        protected Rect GetRectangle(double x1, double y1, double x2, double y2)
        {
            ChartInternalLocation point1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(x1), YAxisRenderer.GetPointValue(y1), XAxisRenderer, YAxisRenderer, Series.Container.RequireInvertedAxis);
            ChartInternalLocation point2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(x2), YAxisRenderer.GetPointValue(y2), XAxisRenderer, YAxisRenderer, Series.Container.RequireInvertedAxis);
            return new Rect(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y), Math.Abs(point2.X - point1.X), Math.Abs(point2.Y - point1.Y));
        }

        protected PointRenderEventArgs TriggerEvent(Point point, string fill, BorderModel border)
        {
            if (point != null && border != null)
            {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                PointRenderEventArgs argsData = new PointRenderEventArgs(Constants.POINTRENDER, false, point, Series, SetPointColor(point, fill), SetBorderColor(point, new ChartEventBorder() { Color = border.Color, Width = border.Width }));
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.

                if (Series?.Container.ChartEvents?.OnPointRender != null)
                {
                    Series.Container.ChartEvents?.OnPointRender.Invoke(argsData);
                }

                point.Interior = argsData.Fill;
                point.Visible = !argsData.Cancel;
                return argsData;
            }

            return null;
        }

        protected void UpdateSymbolLocation(Point point, Rect rect)
        {
            if (!Series.Container.RequireInvertedAxis)
            {
                UpdateXRegion(point, rect);
            }
            else
            {
                UpdateYRegion(point, rect);
            }

            if (Series.Type == ChartSeriesType.Histogram && point != null)
            {
                point.Minimum = (double)point.X - (HistogramValues.BinWidth / 2);
                point.Maximum = (double)point.X + (HistogramValues.BinWidth / 2);
            }
        }

        protected void UpdateXRegion(Point point, Rect rect)
        {
            if (point != null && rect != null)
            {
                point.SymbolLocations.Add(new ChartInternalLocation(rect.X + (rect.Width / 2), (SeriesType() == SeriesValueType.BoxPlot || SeriesType().ToString().Contains("HighLow", StringComparison.InvariantCulture) || (point.YValue >= 0 == !YAxisRenderer.Axis.IsInversed)) ? rect.Y : (rect.Y + rect.Height)));
                GetRegion(point, rect);
                if (Series.Type == ChartSeriesType.RangeColumn)
                {
                    point.SymbolLocations.Add(new ChartInternalLocation(rect.X + (rect.Width / 2), rect.Y + rect.Height));
                }
            }
        }

        protected void UpdateYRegion(Point point, Rect rect)
        {
            if (point != null && rect != null)
            {
                point.SymbolLocations.Add(new ChartInternalLocation((SeriesType() == SeriesValueType.BoxPlot || SeriesType().ToString().Contains("HighLow", StringComparison.InvariantCulture) || (point.YValue >= 0 == !YAxisRenderer.Axis.IsInversed)) ? rect.X + rect.Width : rect.X, rect.Y + (rect.Height / 2)));
                GetRegion(point, rect);
                if (Series.Type == ChartSeriesType.RangeColumn)
                {
                    point.SymbolLocations.Add(new ChartInternalLocation(rect.X, rect.Y + (rect.Height / 2)));
                }
            }
        }

        protected string CalculateRectangle(Point point, Rect rect, string id)
        {
            if (point != null && rect != null)
            {
                if ((Series.Container.RequireInvertedAxis ? rect.Height : rect.Width) <= 0)
                {
                    return null;
                }

                string direction = CalculateRoundedRectPath(rect, Series.CornerRadius.TopLeft, Series.CornerRadius.TopRight, Series.CornerRadius.BottomLeft, Series.CornerRadius.BottomRight);
                DynamicOptions.PathId.Add(id);
                DynamicOptions.CurrentDirection.Add(direction);
                AccessText = string.Empty;
                if (SeriesType() == SeriesValueType.XY)
                {
                    AccessText = point.X + ":" + point.YValue;
                }
                else if (SeriesType() == SeriesValueType.HighLow)
                {
                    FinancialPoint financialPoint = point as FinancialPoint;
                    AccessText = financialPoint.X + ":" + financialPoint.High + ":" + financialPoint.Low;
                }
                else if (SeriesType() == SeriesValueType.BoxPlot)
                {
                    BoxPoint boxPoint = point as BoxPoint;
                    AccessText = boxPoint.X + ":" + boxPoint.Maximum + ":" + boxPoint.Minimum + ":" + boxPoint.LowerQuartile + ":" + boxPoint.UpperQuartile;
                }

                return ChartHelper.AppendPathElements(Series.Container, direction, id, SeriesElementId());
            }

            return null;
        }

        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            ColumnPathOptions.ForEach(option => option.Visibility = visibility);
            switch (property)
            {
                case "Fill":
                    ColumnPathOptions.ForEach(option => option.Fill = Interior);
                    break;
                case "DashArray":
                    ColumnPathOptions.ForEach(option => option.StrokeDashArray = Series.DashArray);
                    break;
                case "Width":
                    ColumnPathOptions.ForEach(option => option.StrokeWidth = Series.Border.Width);
                    break;
                case "Color":
                    ColumnPathOptions.ForEach(option => option.Stroke = Series.Border.Color);
                    break;
                case "Opacity":
                    ColumnPathOptions.ForEach(option => option.Opacity = Series.Opacity);
                    break;
            }
        }

        internal override bool IsRectSeries()
        {
            return true;
        }

        protected virtual void Animate()
        {
            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(SeriesElementId(), AnimationType.Rect);
            }
        }
    }
}