using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Data;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal partial class DataLabelModule : AccumulationBase
    {
        private bool isIncreasingAngle { get; set; }

        private List<AccumulationPoints> rightSideRenderingPoints { get; set; }

        private List<AccumulationPoints> leftSideRenderingPoints { get; set; }

        private void ExtendedLabelsCalculation()
        {
            AccumulationChartSeries series = AccumulationChartInstance.Series.First();
            foreach (AccumulationPoints point in series.RightSidePoints)
            {
                point.InitialLabelRegion = point.LabelRegion;
                point.IsLabelUpdated = 0;
                SkipPoints(point, series.RightSidePoints, series.RightSidePoints.IndexOf(point));
            }

            foreach (AccumulationPoints point in series.LeftSidePoints)
            {
                point.InitialLabelRegion = point.LabelRegion;
                point.IsLabelUpdated = 0;
                SkipPoints(point, series.RightSidePoints, series.RightSidePoints.IndexOf(point));
            }

            ArrangeLeftSidePoints(series);
            isIncreasingAngle = false;
            ArrangeRightSidePoints(series);
        }

        private static void SkipPoints(AccumulationPoints currentPoint, List<AccumulationPoints> halfSidePoints, int pointIndex)
        {
            if (pointIndex > 0 && ((currentPoint.MidAngle < 285 && currentPoint.MidAngle > 255) || (currentPoint.MidAngle < 105 && currentPoint.MidAngle > 75)))
            {
                AccumulationPoints previousPoint = halfSidePoints.ElementAt(pointIndex - 1);
                double angleDiff = (currentPoint.EndAngle % 360) - (currentPoint.StartAngle % 360);
                if (((previousPoint.EndAngle % 360) - (previousPoint.StartAngle % 360)) <= angleDiff && angleDiff < 5 && previousPoint.LabelVisible)
                {
                    SetPointVisibileFalse(currentPoint);
                }
            }
            else if (pointIndex > 1 && ((currentPoint.MidAngle < 300 && currentPoint.MidAngle > 240) || (currentPoint.MidAngle < 120 && currentPoint.MidAngle > 60)))
            {
                AccumulationPoints prevPoint = halfSidePoints[pointIndex - 1];
                AccumulationPoints secondPrevPoint = halfSidePoints[pointIndex - 2];
                if (((currentPoint.EndAngle % 360) - (currentPoint.StartAngle % 360)) < 3 && ((prevPoint.EndAngle % 360) - (prevPoint.StartAngle % 360)) < 3 && ((secondPrevPoint.EndAngle % 360) - (secondPrevPoint.StartAngle % 360)) < 3 && prevPoint.LabelVisible && currentPoint.LabelVisible)
                {
                    SetPointVisibileFalse(currentPoint);
                }
            }
        }

        private static void SetPointVisibileFalse(AccumulationPoints point)
        {
            point.LabelVisible = false;
            point.LabelRegion = null;
        }

        private void ArrangeRightSidePoints(AccumulationChartSeries series)
        {
            bool? startFresh = null;
            bool? angleChanged = null;
            rightSideRenderingPoints = series.RightSidePoints.FindAll(point => point.LabelVisible && point.LabelPosition == AccumulationLabelPosition.Outside);
            AccumulationPoints lastPoint = rightSideRenderingPoints.Count != 0 ? rightSideRenderingPoints.Last() : null;
            if (lastPoint != null && lastPoint.LabelAngle > 90 && lastPoint.LabelAngle < 270)
            {
                isIncreasingAngle = true;
                ChangeLabelAngle(lastPoint, 89);
            }

            for (var i = rightSideRenderingPoints.Count - 1; i >= 0; i--)
            {
                AccumulationPoints currentPoint = rightSideRenderingPoints.ElementAt(i);
                AccumulationPoints nextPoint = i == rightSideRenderingPoints.Count - 1 ? null : rightSideRenderingPoints.ElementAt(i + 1);
                if (IsOverLapWithNext(currentPoint, rightSideRenderingPoints, i) && (currentPoint.LabelVisible || !(currentPoint.LabelAngle <= 90 || currentPoint.LabelAngle >= 270)))
                {
                    double checkAngle = lastPoint.LabelAngle + 10;
                    angleChanged = true;
                    if (startFresh == true)
                    {
                        isIncreasingAngle = false;
                    }
                    else if (checkAngle > 90 && checkAngle < 270 && nextPoint.IsLabelUpdated == 1)
                    {
                        isIncreasingAngle = true;
                    }

                    if (!isIncreasingAngle)
                    {
                        for (var k = i + 1; k < rightSideRenderingPoints.Count; k++)
                        {
                            IncreaseAngle(rightSideRenderingPoints.ElementAt(k - 1), rightSideRenderingPoints.ElementAt(k), series, true);
                        }
                    }
                    else
                    {
                        for (var k = i + 1; k > 0; k--)
                        {
                            DecreaseAngle(rightSideRenderingPoints.ElementAt(k), rightSideRenderingPoints.ElementAt(k - 1), series, true);
                        }
                    }
                }
                else if (angleChanged == true && nextPoint != null && nextPoint.IsLabelUpdated != 1)
                {
                    startFresh = true;
                }
            }
        }

        private void ArrangeLeftSidePoints(AccumulationChartSeries series)
        {
            leftSideRenderingPoints = series.LeftSidePoints.FindAll(point => point.LabelVisible && point.LabelPosition == AccumulationLabelPosition.Outside);
            AccumulationPoints previousPoint = null;
            bool? angleChanged = null;
            bool? startFresh = null;
            for (var i = 0; i < leftSideRenderingPoints.Count; i++)
            {
                AccumulationPoints currentPoint = leftSideRenderingPoints[i];
                if (i != 0)
                {
                    previousPoint = leftSideRenderingPoints[i - 1];
                }

                if (IsOverLapWithPrevious(currentPoint, leftSideRenderingPoints, i) && currentPoint.LabelVisible || !(currentPoint.LabelAngle < 270))
                {
                    angleChanged = true;
                    if (startFresh == true)
                    {
                        isIncreasingAngle = false;
                    }

                    if (!isIncreasingAngle)
                    {
                        for (var k = i; k > 0; k--)
                        {
                            DecreaseAngle(leftSideRenderingPoints[k], leftSideRenderingPoints[k - 1], series, false);
                            leftSideRenderingPoints.FindAll((valuePoint) => isIncreasingAngle = valuePoint.IsLabelUpdated == 1 && valuePoint.LabelAngle - 10 < 100);
                        }
                    }
                    else
                    {
                        for (var k = i; k < leftSideRenderingPoints.Count; k++)
                        {
                            IncreaseAngle(leftSideRenderingPoints[k - 1], leftSideRenderingPoints[k], series, false);
                        }
                    }
                }
                else if (angleChanged == true && previousPoint != null && previousPoint.IsLabelUpdated == 1)
                {
                    startFresh = true;
                }
            }
        }

        private void ChangeLabelAngle(AccumulationPoints currentPoint, double newAngle)
        {
            AccumulationDataLabelSettings datalabel = AccumulationChartInstance.Series.First().DataLabel;
            double radius = double.NaN;
            if (IsVariableRadius())
            {
                radius = AccumulationChartInstance.PieSeriesModule.GetLabelRadius(AccumulationChartInstance.VisibleSeries.First(), currentPoint);
            }

            double labelRadius = (currentPoint.LabelPosition == AccumulationLabelPosition.Outside && AccumulationChartInstance.EnableSmartLabels &&
                datalabel.Position == AccumulationLabelPosition.Inside) ? Radius + DataVizCommonHelper.StringToNumber(string.IsNullOrEmpty(datalabel.ConnectorStyle.Length) ? "4%" : datalabel.ConnectorStyle.Length, AccumulationChartInstance.PieSeriesModule.Size / 2) :
                (!IsVariableRadius() ? AccumulationChartInstance.PieSeriesModule.LabelRadius + 10 : radius);
            GetLabelRegion(currentPoint, AccumulationLabelPosition.Outside, currentPoint.TextSize, !IsVariableRadius() ? labelRadius : radius, marginValue, newAngle);
            currentPoint.IsLabelUpdated = 1;
            currentPoint.LabelAngle = newAngle;
        }

        private static bool IsOverLapWithNext(AccumulationPoints point, List<AccumulationPoints> points, int pointIndex)
        {
            return points.Where((x, i) => i >= pointIndex && i != points.IndexOf(point) && x.Visible && x.LabelVisible && x.LabelRegion != null && point.LabelRegion != null && point.LabelVisible && IsOverlap(point.LabelRegion, x.LabelRegion)).Any();
        }

        private static bool IsOverLapWithPrevious(AccumulationPoints currentPoint, List<AccumulationPoints> points, int currentPointIndex)
        {
            return points.Where((x, i) => i < currentPointIndex && i != points.IndexOf(currentPoint) && points[i].Visible && points[i].LabelVisible && points[i].LabelRegion != null && currentPoint.LabelRegion != null && currentPoint.LabelVisible && IsOverlap(currentPoint.LabelRegion, points[i].LabelRegion)).Any();
        }

        private static bool IsOverlap(Rect currentRect, Rect rect)
        {
            return currentRect.X < rect.X + rect.Width && currentRect.X + currentRect.Width > rect.X && currentRect.Y < rect.Y + rect.Height && currentRect.Height + currentRect.Y > rect.Y;
        }

        private void IncreaseAngle(AccumulationPoints currentPoint, AccumulationPoints nextPoint, AccumulationChartSeries series, bool isRightSide)
        {
            if (currentPoint != null && nextPoint != null)
            {
                int count = 1;
                if (isRightSide)
                {
                    while (IsOverlap(currentPoint.LabelRegion, nextPoint.LabelRegion) || (!IsVariableRadius() && !((currentPoint.LabelRegion.Y + currentPoint.LabelRegion.Height) < nextPoint.LabelRegion.Y)))
                    {
                        double newAngle = nextPoint.MidAngle + count;
                        if (newAngle < 270 && newAngle > 90)
                        {
                            newAngle = 90;
                            isIncreasingAngle = true;
                            break;
                        }

                        ChangeLabelAngle(nextPoint, newAngle);
                        if (IsOverlap(currentPoint.LabelRegion, nextPoint.LabelRegion) && newAngle + 1 > 90 && newAngle + 1 < 270 && rightSideRenderingPoints.IndexOf(nextPoint) == rightSideRenderingPoints.Count - 1)
                        {
                            ChangeLabelAngle(currentPoint, currentPoint.LabelAngle - 1);
                            nextPoint.LabelRegion = nextPoint.InitialLabelRegion;
                            ArrangeRightSidePoints(series);
                            break;
                        }

                        count++;
                    }
                }
                else
                {
                    while (IsOverlap(currentPoint.LabelRegion, nextPoint.LabelRegion) || (!IsVariableRadius() && (currentPoint.LabelRegion.Y < (nextPoint.LabelRegion.Y + nextPoint.LabelRegion.Height))))
                    {
                        double newAngle = nextPoint.MidAngle + count;
                        if (!(newAngle < 270 && newAngle > 90))
                        {
                            newAngle = 270;
                            isIncreasingAngle = true;
                            break;
                        }

                        ChangeLabelAngle(nextPoint, newAngle);
                        count++;
                    }
                }
            }
        }

        private void DecreaseAngle(AccumulationPoints currentPoint, AccumulationPoints previousPoint, AccumulationChartSeries series, bool isRightSide)
        {
            if (currentPoint != null && previousPoint != null)
            {
                double count = 1;
                if (isRightSide)
                {
                    while (IsOverlap(currentPoint.LabelRegion, previousPoint.LabelRegion) || (!IsVariableRadius() && !((previousPoint.LabelRegion.Height + previousPoint.LabelRegion.Y) < currentPoint.LabelRegion.Y)))
                    {
                        double newAngle = previousPoint.MidAngle - count;
                        if (newAngle < 0)
                        {
                            newAngle = 360 + newAngle;
                        }

                        if (newAngle <= 270 && newAngle >= 90)
                        {
                            newAngle = 270;
                            isIncreasingAngle = true;
                            break;
                        }

                        ChangeLabelAngle(previousPoint, newAngle);
                        count++;
                    }
                }
                else
                {
                    if (currentPoint.LabelAngle > 270)
                    {
                        ChangeLabelAngle(currentPoint, 270);
                        previousPoint.LabelAngle = 270;
                    }

                    while (IsOverlap(currentPoint.LabelRegion, previousPoint.LabelRegion) || (!IsVariableRadius() && ((currentPoint.LabelRegion.Y + currentPoint.LabelRegion.Height) > previousPoint.LabelRegion.Y)))
                    {
                        double newAngle = previousPoint.MidAngle - count;
                        if (!(newAngle <= 270 && newAngle >= 90))
                        {
                            newAngle = 90;
                            isIncreasingAngle = true;
                            break;
                        }

                        ChangeLabelAngle(previousPoint, newAngle);
                        if (IsOverlap(currentPoint.LabelRegion, previousPoint.LabelRegion) && series.LeftSidePoints.IndexOf(previousPoint) == 0 && (newAngle - 1 < 90 && newAngle - 1 > 270))
                        {
                            ChangeLabelAngle(currentPoint, currentPoint.LabelAngle + 1);
                            ArrangeLeftSidePoints(series);
                            break;
                        }

                        count++;
                    }
                }
            }
        }
    }
}