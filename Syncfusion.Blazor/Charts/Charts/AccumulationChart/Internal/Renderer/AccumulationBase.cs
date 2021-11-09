using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class AccumulationBase
    {
        internal SfAccumulationChart AccumulationChartInstance { get; set; }

        internal SvgPath HoverBorderElement { get; set; }

        protected static CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal ChartInternalLocation Center
        {
            get
            {
                return center != null ? center : AccumulationChartInstance.VisibleSeries.First().Type == AccumulationType.Pie ? AccumulationChartInstance.PieSeriesModule.CenterLoc : null;
            }

            set
            {
                center = value;
            }
        }

        private ChartInternalLocation center { get; set; }

        public double Radius
        {
            get
            {
                return !double.IsNaN(radius) ? radius : AccumulationChartInstance.PieSeriesModule.Radius;
            }

            set
            {
                radius = value;
            }
        }

        private double radius { get; set; } = double.NaN;

        internal double LabelRadius
        {
            get
            {
                return !double.IsNaN(labelRadius) ? labelRadius : AccumulationChartInstance.PieSeriesModule.LabelRadius;
            }

            set
            {
                labelRadius = value;
            }
        }

        private double labelRadius { get; set; } = double.NaN;

        internal AccumulationBase(SfAccumulationChart chart)
        {
            AccumulationChartInstance = chart;
        }

        internal bool IsCircular()
        {
            return AccumulationChartInstance.Series.Count != 0 && AccumulationChartInstance.Series[0].Type == AccumulationType.Pie;
        }

        internal bool IsVariableRadius()
        {
            return AccumulationChartInstance.PieSeriesModule.IsRadiusMapped;
        }

        internal void ProcessExplode(ChartInternalMouseEventArgs args)
        {
            AccumulationChartInstance.Rendering.PathElementList.Find(item => item.Id == AccumulationChartInstance.ID + "PointHoverBorder")?.ChangeDirection(string.Empty);
            if (args.Target.Contains("_Series_", StringComparison.Ordinal) || args.Target.Contains("_datalabel_", StringComparison.Ordinal))
            {
                int pointIndex = short.Parse(args.Target.Split('_').Last(), AccumulationChartInstance.NumberFormatter);
                if (double.IsNaN(pointIndex) || (args.Target.Contains("_datalabel_", StringComparison.Ordinal) && AccumulationChartInstance.VisibleSeries[0].Points[pointIndex].LabelPosition == AccumulationLabelPosition.Outside))
                {
                    return;
                }
                else
                {
                    ExplodePoints(pointIndex);
                    DeExplodeAll(pointIndex, AccumulationChartInstance.EnableAnimation ? 300 : 0);
                }
            }
        }

        internal void InvokeExplode()
        {
            foreach (AccumulationPoints point in AccumulationChartInstance.VisibleSeries[0].Points)
            {
                if (point.IsExplode)
                {
                    PointExplode(point.Index, point, AccumulationChartInstance.EnableAnimation ? 300 : 0);
                }
            }

            if (AccumulationChartInstance?.AccumulationSelectionModule != null && AccumulationChartInstance.SelectionMode != AccumulationSelectionMode.None && AccumulationChartInstance.AccumulationSelectionModule.SelectedDataIndexes.Count != 0)
            {
                AccumulationChartInstance.AccumulationSelectionModule.SelectedDataIndexes.ForEach(x =>
                {
                    ExplodePoints(x.Point, true);
                    DeExplodeAll(x.Point, AccumulationChartInstance.EnableAnimation ? 300 : 0);
                });
            }
        }

        internal void ExplodePoints(int index, bool explode = false)
        {
            AccumulationChartSeries series = AccumulationChartInstance.Series.First();
            AccumulationPoints point = series.Points.Find(item => item.Index == index);
            bool explodePoints = true;
            if (point != null)
            {
                bool clubPointsExploded = !explode && (point.IsSliced || (series.ClubbedPoints.Count != 0 &&
                      series.Points[series.Points.Count - 1].Index == series.ClubbedPoints[series.ClubbedPoints.Count - 1].Index));
                if (series.Type == AccumulationType.Pie && (clubPointsExploded || point.IsClubbed))
                {
                    explodePoints = ClubPointExplode(index, point, series, series.Points, AccumulationChartInstance.EnableAnimation ? 300 : 0, clubPointsExploded);
                }

                if (explodePoints)
                {
                    PointExplode(index, point, AccumulationChartInstance.EnableAnimation ? 300 : 0, explode);
                }
            }
        }

        private bool ClubPointExplode(int index, AccumulationPoints point, AccumulationChartSeries series, List<AccumulationPoints> points, double duration, bool clubPointsExploded = false)
        {
            if (point.IsClubbed)
            {
                AccumulationChartInstance.animateSeries = false;
                points.RemoveAt(points.Count - 1);
                series.ClubbedPoints.ForEach(item =>
                {
                    item.IsExplode = true;
                    item.Visible = true;
                    series.ProcessPointTextEvents(item);
                });
                series.Points.AddRange(series.ClubbedPoints);
                DeExplodeAll(index, duration);
                series.SumOfPoints = series.Points.Sum(item => double.IsNaN((double)item.Y) ? 0 : (double)item.Y);
                AccumulationChartInstance.RefreshChart();
                return false;
            }
            else if (clubPointsExploded || point.IsSliced)
            {
                AccumulationChartInstance.animateSeries = false;
                series.Points.RemoveRange(points.Count - series.ClubbedPoints.Count, series.ClubbedPoints.Count);
                AccumulationPoints clubPoint = series.GenerateClubPoint();
                clubPoint.Index = points.Count;
                clubPoint.Color = series.ClubbedPoints[0].Color;
                points.Add(clubPoint);
                series.SumOfPoints = series.Points.Sum(item => double.IsNaN((double)item.Y) ? 0 : (double)item.Y);
                DeExplodeAll(index, duration);
                clubPoint.IsExplode = false;
                AccumulationChartInstance.VisibleSeries[0].Points = points;
                AccumulationChartInstance.RefreshChart();
                PointExplode(clubPoint.Index, points[clubPoint.Index], 0, true);
                clubPoint.IsExplode = false;
                DeExplodeSlice(clubPoint.Index, AccumulationChartInstance.ID + "_Series_0_Point_", duration);
                if (point.IsSliced)
                {
                    return false;
                }
            }

            return true;
        }

        internal void DeExplodeAll(int index, double animationDuration)
        {
            foreach (AccumulationPoints currentPoint in AccumulationChartInstance.VisibleSeries[0].Points)
            {
                if ((index != currentPoint.Index && !currentPoint.IsSliced) || currentPoint.IsClubbed)
                {
                    currentPoint.IsExplode = false;
                    DeExplodeSlice(currentPoint.Index, AccumulationChartInstance.ID + "_Series_0_Point_", animationDuration);
                }
            }
        }

        private async void DeExplodeSlice(int index, string sliceId, double animationDuration)
        {
            SvgPath element = AccumulationChartInstance.Rendering.PathElementList.Find(item => item.Id == sliceId + index);
            if (element != null)
            {
                HoverBorderElement = AccumulationChartInstance.Rendering.PathElementList.Find(item => item.Id == AccumulationChartInstance.ID + "PointHoverBorder");
                HoverBorderElement?.ChangePathAttributes(string.Empty, string.Empty, string.Empty);
            }
#pragma warning disable CA2007
            string transform = await AccumulationChartInstance.InvokeMethod<string>(AccumulationChartConstants.GETATTRIBUTE, false, new object[] { sliceId + index, "transform" });
            if (AccumulationChartInstance.EnableAnimation && element != null && !string.IsNullOrEmpty(transform) && transform != "translate(0, 0)" && transform != "translate(0)")
            {
                PerformAnimation(index, sliceId, 0, 0, 0, 0, animationDuration, transform, true);
            }
            else
            {
                PerformAnimation(index, sliceId, 0, 0, 0, 0, animationDuration, string.Empty, true);
            }
        }

        private async void PointExplode(int index, AccumulationPoints point, double duration, bool explode = false)
        {
            ChartInternalLocation translate;
            string pointId = AccumulationChartInstance.ID + "_Series_0_Point_";
            if (!IsCircular())
            {
                translate = new ChartInternalLocation((point.LabelRegion != null && point.LabelRegion.X < point.Region.X) ? -AccumulationChartInstance.ExplodeDistance : AccumulationChartInstance.ExplodeDistance, 0);
            }
            else
            {
                translate = ChartHelper.DegreeToLocation(point.MidAngle, AccumulationChartInstance.ExplodeDistance, Center);
            }

            string transform = await AccumulationChartInstance.InvokeMethod<string>(AccumulationChartConstants.GETATTRIBUTE, false, new object[] { pointId + index, "transform" });
            if (IsExplode(pointId + index, transform) || explode)
            {
                point.IsExplode = true;
                ExplodeSlice(index, translate, pointId, Center != null ? AccumulationChartInstance.PieSeriesModule.CenterLoc : new ChartInternalLocation(0, 0), duration);
            }
            else
            {
                point.IsExplode = false;
                DeExplodeSlice(index, pointId, duration);
            }
        }

        private async void PerformAnimation(int index, string sliceId, double startX, double startY, double endX, double endY, double duration, string transform, bool isReverse = false)
        {
            await AccumulationChartInstance.InvokeMethod(AccumulationChartConstants.PERFORMANIMATION, new object[] { index, sliceId, startX, startY, endX, endY, duration, transform, isReverse });
#pragma warning restore CA2007
        }

        private bool IsExplode(string id, string transform)
        {
            return AccumulationChartInstance.Rendering.PathElementList.Find(item => item.Id == id) != null && (transform == "translate(0, 0)" || transform == null || transform == "translate(0)");
        }

        private void ExplodeSlice(int index, ChartInternalLocation translate, string sliceId, ChartInternalLocation center, double animationDuration)
        {
            PerformAnimation(index, sliceId, 0, 0, translate.X - center.X, translate.Y - center.Y, animationDuration, string.Empty);
        }

        internal virtual void Dispose()
        {
            AccumulationChartInstance = null;
            HoverBorderElement = null;
            Center = null;
            center = null;
        }
    }
}