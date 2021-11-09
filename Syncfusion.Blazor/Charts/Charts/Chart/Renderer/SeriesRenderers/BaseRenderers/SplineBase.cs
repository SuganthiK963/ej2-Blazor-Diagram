using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System.Runtime.InteropServices;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal abstract class SplineBaseSeriesRenderer : LineBaseSeriesRenderer
    {
        internal const double ONETHIRD = 1 / 3.0;

        public SplineType SplineType { get; set; } = SplineType.Natural;

        internal List<ControlPoints> DrawPoints { get; set; } = new List<ControlPoints>();

        private double[] SplinePoints { get; set; }

        internal static List<Point> FilterEmptyPoints(ChartSeries series, [Optional] List<Point> seriesPoints)
        {
            List<Point> points = seriesPoints != null ? seriesPoints : series.Renderer.Points;

            if (series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
            {
                return points;
            }

            for (int i = 0; i < points.Count; i++)
            {
                points[i].Index = i;
                if (points[i].IsEmpty)
                {
                    points[i].SymbolLocations = new List<ChartInternalLocation>();
                    points[i].Regions = new List<Rect>();
                    points.RemoveRange(i, 1);
                    i--;
                }
            }

            return points;
        }

        internal override void OnParentParameterSet()
        {
            base.OnParentParameterSet();

            SplineType = Series.SplineType;
        }

        internal override void FindSplinePoint()
        {
            ControlPoints pointValue;
            bool isPolar = Owner.GetAreaType() == ChartAreaType.PolarAxes;
            List<Point> points = FilterEmptyPoints(Series);

            SplinePoints = FindSplineCoefficients(points, Series);
            if (points.Count > 1)
            {
                DrawPoints = new List<ControlPoints>();
                foreach (Point data in points)
                {
                    if (data.Index != 0)
                    {
                        int previous = GetPreviousIndex(points, data.Index - 1, Series);
                        pointValue = GetControlPoints(points[previous], data, SplinePoints[previous], SplinePoints[data.Index], Series);
                        DrawPoints.Add(pointValue);
                        if (data.YValue != 0 && !double.IsNaN(data.YValue) && pointValue.ControlPoint1.Y != 0 && !double.IsNaN(pointValue.ControlPoint1.Y) && pointValue.ControlPoint2.Y != 0 && !double.IsNaN(pointValue.ControlPoint2.Y) && (Series.Renderer.YMax - Series.Renderer.YMin) > 1)
                        {
                            Series.Renderer.YMin = Math.Floor(Math.Min(Math.Min(Series.Renderer.YMin, data.YValue), Math.Min(pointValue.ControlPoint1.Y, pointValue.ControlPoint2.Y)));
                            Series.Renderer.YMax = Math.Ceiling(Math.Max(Math.Max(Series.Renderer.YMax, data.YValue), Math.Max(pointValue.ControlPoint1.Y, pointValue.ControlPoint2.Y)));
                        }
                    }
                }

                if (isPolar && Series.IsClosed)
                {
                    pointValue = GetControlPoints(
                        new Point { XValue = points[points.Count - 1].XValue, YValue = points[points.Count - 1].YValue },
                        new Point { XValue = points[points.Count - 1].XValue + 1, YValue = points[0].YValue },
                        SplinePoints[0],
                        SplinePoints[points[points.Count - 1].Index],
                        Series);
                    DrawPoints.Add(pointValue);
                }
            }
        }

        internal virtual string GetSplineDirection(ControlPoints data, Point firstPoint, Point point, bool isInverted, ChartSeries series, string startPoint)
        {
            ChartInternalLocation pt1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(firstPoint.XValue), YAxisRenderer.GetPointValue(firstPoint.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartInternalLocation pt2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(point.YValue), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartInternalLocation bpt1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint1.X), YAxisRenderer.GetPointValue(data.ControlPoint1.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartInternalLocation bpt2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint2.X), YAxisRenderer.GetPointValue(data.ControlPoint2.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            return startPoint + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE + "C" + SPACE + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt2.X.ToString(Culture) + SPACE + pt2.Y.ToString(Culture) + SPACE;
        }

        internal virtual string GetPolarSplineDirection(ControlPoints data, Point firstPoint, Point point, bool isInverted, ChartSeries series, string startPoint)
        {
            ChartInternalLocation pt1 = ChartHelper.TransformToVisible(firstPoint.XValue, firstPoint.YValue, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
            ChartInternalLocation pt2 = ChartHelper.TransformToVisible(point.XValue, point.YValue, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
            ChartInternalLocation bpt1 = ChartHelper.TransformToVisible(data.ControlPoint1.X, data.ControlPoint1.Y, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
            ChartInternalLocation bpt2 = ChartHelper.TransformToVisible(data.ControlPoint2.X, data.ControlPoint2.Y, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
            return startPoint + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE + "C" + SPACE + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt2.X.ToString(Culture) + SPACE + pt2.Y.ToString(Culture) + SPACE;
        }

        internal virtual string GetSplineAreaDirection(ControlPoints data, ChartInternalLocation pt1, bool isInverted, ChartSeries series)
        {
            ChartInternalLocation bpt1 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint1.X), YAxisRenderer.GetPointValue(data.ControlPoint1.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            ChartInternalLocation bpt2 = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(data.ControlPoint2.X), YAxisRenderer.GetPointValue(data.ControlPoint2.Y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
            return "C " + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE;
        }

        internal virtual string GetPolarSplineAreaDirection(ControlPoints data, ChartInternalLocation pt1, bool isInverted, ChartSeries series)
        {
            ChartInternalLocation bpt1 = ChartHelper.TransformToVisible(data.ControlPoint1.X, data.ControlPoint1.Y, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
            ChartInternalLocation bpt2 = ChartHelper.TransformToVisible(data.ControlPoint2.X, data.ControlPoint2.Y, XAxisRenderer.Axis, YAxisRenderer.Axis, series);
            return "C " + bpt1.X.ToString(Culture) + SPACE + bpt1.Y.ToString(Culture) + SPACE + bpt2.X.ToString(Culture) + SPACE + bpt2.Y.ToString(Culture) + SPACE + pt1.X.ToString(Culture) + SPACE + pt1.Y.ToString(Culture) + SPACE;
        }

        protected static int GetPreviousIndex(List<Point> points, int i, ChartSeries series)
        {
            if (series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
            {
                return i;
            }

            bool isNull = i > -1 ? points[i] == null : true;
            while (isNull && i > -1)
            {
                i = i - 1;
            }

            return i;
        }

        internal static int GetNextIndex(List<Point> points, int i, ChartSeries series)
        {
            if (series.EmptyPointSettings.Mode != EmptyPointMode.Drop)
            {
                return i;
            }

            bool isNull = i > -1 ? (i < points.Count ? points[i] == null : true) : true;
            while (isNull && i < points.Count)
            {
                i = i + 1;
            }

            return i;
        }

        protected double DateTimeInterval()
        {
            IntervalType interval = XAxisRenderer.ActualIntervalType;
            double intervalInMilliseconds = 86400000;
            if (interval == IntervalType.Years)
            {
                intervalInMilliseconds = 365 * intervalInMilliseconds;
            }
            else if (interval == IntervalType.Months)
            {
                intervalInMilliseconds = 30 * intervalInMilliseconds;
            }
            else if (interval == IntervalType.Days)
            {
                return intervalInMilliseconds;
            }
            else if (interval == IntervalType.Hours)
            {
                intervalInMilliseconds = 60 * 60 * 1000;
            }
            else if (interval == IntervalType.Minutes)
            {
                intervalInMilliseconds = 60 * 1000;
            }
            else if (interval == IntervalType.Seconds)
            {
                intervalInMilliseconds = 1000;
            }
            else
            {
                intervalInMilliseconds = 30 * intervalInMilliseconds;
            }

            return intervalInMilliseconds;
        }

        public static double[] FindSplineCoefficients(List<Point> points, ChartSeries series)
        {
            int count = points.Count;
            double[] y_Spline = new double[count];
            if (count > 0)
            {
                double[] y_SplineDuplicate = new double[count];
                int slopeSize = (count - 1) > -1 ? (count - 1) : 0;
                double[] dx = new double[slopeSize], dy = new double[slopeSize], slope = new double[slopeSize];
                double cardinalSplineTension = series.CardinalSplineTension > 0 ? series.CardinalSplineTension : 0.5;
                cardinalSplineTension = cardinalSplineTension < 0 ? 0 : cardinalSplineTension > 1 ? 1 : cardinalSplineTension;
                switch (series.SplineType)
                {
                    case SplineType.Monotonic:
                        for (int i = 0; i < count - 1; i++)
                        {
                            dx[i] = !double.IsNaN(points[i + 1].XValue - points[i].XValue) ? points[i + 1].XValue - points[i].XValue : 0;
                            dy[i] = !double.IsNaN(points[i + 1].YValue - points[i].YValue) ? points[i + 1].YValue - points[i].YValue : 0;
                            slope[i] = dy[i] / dx[i];
                        }

                        y_Spline[0] = slope[0];
                        y_Spline[count - 1] = slope[slope.Length - 1];
                        for (int j = 0; j < dx.Length; j++)
                        {
                            if (slope.Length > j + 1)
                            {
                                if (slope[j] * slope[j + 1] <= 0)
                                {
                                    y_Spline[j + 1] = 0;
                                }
                                else
                                {
                                    double interPoint = dx[j] + dx[j + 1];
                                    y_Spline[j + 1] = 3 * interPoint / (((interPoint + dx[j + 1]) / slope[j]) + ((interPoint + dx[j]) / slope[j + 1]));
                                }
                            }
                        }

                        break;

                    case SplineType.Cardinal:
                        for (int i = 0; i < count; i++)
                        {
                            if (i == 0)
                            {
                                y_Spline[i] = (count > 2) ? (cardinalSplineTension * (points[i + 2].XValue - points[i].XValue)) : 0;
                            }
                            else if (i == (count - 1))
                            {
                                y_Spline[i] = (count > 2) ? (cardinalSplineTension * (points[count - 1].XValue - points[count - 3].XValue)) : 0;
                            }
                            else
                            {
                                y_Spline[i] = cardinalSplineTension * (points[i + 1].XValue - points[i - 1].XValue);
                            }
                        }

                        break;

                    default:
                        if (series.SplineType == SplineType.Clamped)
                        {
                            y_Spline[0] = ((3 * (points[1].YValue - points[0].YValue)) / (points[1].XValue - points[0].XValue)) - 3;
                            y_SplineDuplicate[0] = 0.5;
                            y_Spline[points.Count - 1] = (3 * (points[points.Count - 1].YValue - points[points.Count - 2].YValue)) / (points[points.Count - 1].XValue - points[points.Count - 2].XValue);
                            y_Spline[0] = y_SplineDuplicate[0] = double.IsInfinity(Math.Abs(y_Spline[0])) ? 0 : y_Spline[0];
                            y_Spline[points.Count - 1] = y_SplineDuplicate[points.Count - 1] = double.IsInfinity(Math.Abs(y_Spline[points.Count - 1])) ? 0 : y_Spline[points.Count - 1];
                        }
                        else
                        {
                            y_Spline[0] = y_SplineDuplicate[0] = 0;
                            y_Spline[points.Count - 1] = 0;
                        }

                        for (int i = 1; i < count - 1; i++)
                        {
                            y_Spline[0] = y_SplineDuplicate[0] = 0;
                            y_Spline[points.Count - 1] = 0;
                            double coefficient1 = points[i].XValue - points[i - 1].XValue, coefficient2 = points[i + 1].XValue - points[i - 1].XValue, coefficient3 = points[i + 1].XValue - points[i].XValue;
                            double dy1 = !double.IsNaN(points[i + 1].YValue - points[i].YValue) ? points[i + 1].YValue - points[i].YValue : 0;
                            double dy2 = !double.IsNaN(points[i].YValue - points[i - 1].YValue) ? points[i].YValue - points[i - 1].YValue : 0;
                            if (coefficient1 == 0 || coefficient2 == 0 || coefficient3 == 0)
                            {
                                y_Spline[i] = 0;
                                y_SplineDuplicate[i] = 0;
                            }
                            else
                            {
                                double p = 1 / ((coefficient1 * y_Spline[i - 1]) + (2 * coefficient2));
                                y_Spline[i] = -p * coefficient3;
                                y_SplineDuplicate[i] = p * ((6 * ((dy1 / coefficient3) - (dy2 / coefficient1))) - (coefficient1 * y_SplineDuplicate[i - 1]));
                            }
                        }

                        for (int k = count - 2; k >= 0; k--)
                        {
                            y_Spline[k] = (y_Spline[k] * y_Spline[k + 1]) + y_SplineDuplicate[k];
                        }

                        break;
                }
            }

            return y_Spline;
        }

        private ControlPoints GetControlPoints(Point prevdata, Point nextData, double y_Spline1, double y_Spline2, ChartSeries series)
        {
            ControlPoints point;
            double y_SplineDuplicate1 = y_Spline1;
            double y_SplineDuplicate2 = y_Spline2;
            switch (series.SplineType)
            {
                case SplineType.Cardinal:
                    if (XAxisRenderer.Axis.ValueType == ValueType.DateTime)
                    {
                        y_SplineDuplicate1 = y_Spline1 / DateTimeInterval();
                        y_SplineDuplicate2 = y_Spline2 / DateTimeInterval();
                    }

                    point = new ControlPoints(new ChartInternalLocation(prevdata.XValue + (y_Spline1 / 3), prevdata.YValue + (y_SplineDuplicate1 / 3)), new ChartInternalLocation(nextData.XValue - (y_Spline2 / 3), nextData.YValue - (y_SplineDuplicate2 / 3)));
                    break;
                case SplineType.Monotonic:
                    double pointValue = (nextData.XValue - prevdata.XValue) / 3;
                    point = new ControlPoints(new ChartInternalLocation(prevdata.XValue + pointValue, prevdata.YValue + (y_Spline1 * pointValue)), new ChartInternalLocation(nextData.XValue - pointValue, nextData.YValue - (y_Spline2 * pointValue)));
                    break;
                default:
                    double deltaX2 = nextData.XValue - prevdata.XValue;
                    deltaX2 = deltaX2 * deltaX2;

                    // issue fix for dateTime category
                    point = new ControlPoints(
                        new ChartInternalLocation(((2 * prevdata.XValue) + nextData.XValue) * ONETHIRD, ONETHIRD * (((2 * prevdata.YValue) + nextData.YValue) - (ONETHIRD * deltaX2 * (y_Spline1 + (0.5 * y_Spline2))))),
                        new ChartInternalLocation((prevdata.XValue + (2 * nextData.XValue)) * ONETHIRD, ONETHIRD * ((prevdata.YValue + (2 * nextData.YValue)) - (ONETHIRD * deltaX2 * ((0.5 * y_Spline1) + y_Spline2)))));
                    break;
            }

            return point;
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            SplinePoints = null;
        }
    }
}
