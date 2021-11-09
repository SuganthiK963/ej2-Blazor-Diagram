using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Models;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Dynamic;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the chart series of the accumulation chart component.
    /// </summary>
    public partial class AccumulationChartSeries
    {
        internal IEnumerable<object> DataModule { get; set; }

        internal double SumOfPoints { get; set; }

        internal List<AccumulationPoints> Points { get; set; } = new List<AccumulationPoints>();

        internal List<AccumulationPoints> ClubbedPoints { get; set; } = new List<AccumulationPoints>();

        internal string LastGroupTo { get; set; }

        internal Rect AccumulationBound { get; set; }

        internal Rect LabelBound { get; set; }

        internal Size TriangleSize { get; set; }

        internal Size NeckSize { get; set; }

        internal int Index { get; set; }

        internal List<AccumulationPoints> LeftSidePoints { get; set; } = new List<AccumulationPoints>();

        internal List<AccumulationPoints> RightSidePoints { get; set; } = new List<AccumulationPoints>();

        private double sumOfClub { get; set; }

        private const string JOBJECT = "JObject";

        private const string EXPANDOOBJECT = "ExpandoObject";

        private static ChartCommonBorder CreateBorderInstance(string color, double width)
        {
            ChartCommonBorder border = new ChartCommonBorder();
            border.SetBorderValue(color, width);
            return border;
        }

        internal void GetPoints(IEnumerable<object> result, IAccumulationChart accChart)
        {
            int pointsLength = result.Count();
            Type type = result.First().GetType();
            SumOfPoints = 0;
            double clubValue = 0;
            AccumulationPoints accPoint;
            if (pointsLength != 0)
            {
                FindSumOfPoints(result, type.GetProperty(YName));
                Points.Clear();
                ClubbedPoints.Clear();
                sumOfClub = 0;
                clubValue = DataVizCommonHelper.StringToNumber(GroupTo, SumOfPoints);
                string[] colors = Palettes.Length != 0 ? Palettes : ChartHelper.GetSeriesColor(accChart.Theme.ToString());
                foreach (object point in result)
                {
                    accPoint = SetPoints(point, result.IndexOf(point), type);
                    if (!IsClub(accPoint, clubValue, result.IndexOf(point)))
                    {
                        PushPoints(accPoint, colors);
                    }
                    else
                    {
                        accPoint.Index = ClubbedPoints.Count;
                        accPoint.IsExplode = true;
                        ClubbedPoints.Add(accPoint);
                        accPoint.IsSliced = true;
                    }
                }

                LastGroupTo = GroupTo;
                if (sumOfClub > 0)
                {
                    AccumulationPoints clubPoint = GenerateClubPoint();
                    PushPoints(clubPoint, colors);
                    ClubbedPoints.ForEach((AccumulationPoints point) =>
                    {
                        point.Index += Points.Count - 1;
                        point.Color = clubPoint.Color;
                    });
                }

                if (ClubbedPoints.Count != 0 && Explode && Type == AccumulationType.Pie && (ExplodeAll || Points[Points.Count - 1].Index == ExplodeIndex))
                {
                    Points.RemoveAt(Points.Count - 1);
                    Points.AddRange(ClubbedPoints);
                }
            }
        }

        private void FindSumOfPoints(IEnumerable<object> result, PropertyInfo yValue)
        {
            foreach (object point in result)
            {
                double currentY = double.NaN;
                if (result.First().GetType().Name == JOBJECT)
                {
                    JObject o = (JObject)point;
                    currentY = (double)o.GetValue(YName, StringComparison.Ordinal);
                }
                else if (result.First().GetType().Name == EXPANDOOBJECT)
                {
                    foreach (KeyValuePair<string, object> property in (IDictionary<string, object>)point)
                    {
                        if (YName == property.Key)
                        {
                            currentY = (property.Value != null) ? Convert.ToDouble(property.Value, CultureInfo.InvariantCulture) : double.NaN;
                        }
                    }
                }
                else if (result.First().GetType().BaseType.Equals(typeof(DynamicObject)))
                {
                    currentY = Convert.ToDouble(ChartHelper.GetDynamicMember(point, yName), CultureInfo.InvariantCulture);
                }
                else
                {
                    currentY = Convert.ToDouble(yValue.GetValue(point), null);
                }

                if (point != null && !double.IsNaN(currentY))
                {
                    SumOfPoints += Math.Abs(currentY);
                }
            }
        }

        internal void ProcessPointTextEvents(AccumulationPoints point)
        {
            AccumulationPointRenderEventArgs pointRender = new AccumulationPointRenderEventArgs
            (
                AccumulationChartConstants.ONPOINTRENDER,
                false,
                this,
                point,
                point.Color,
                CreateBorderInstance(
                    IsEmpty(point) ? EmptyPointSettings.Border.Color : Border.Color,
                    IsEmpty(point) ? EmptyPointSettings.Border.Width : Border.Width));
            accumulationChart.AccumulationChartEvents?.OnPointRender?.Invoke(pointRender);
            point.AccPointArgs = pointRender;
            AccumulationTextRenderEventArgs textRenderEventArgs = new AccumulationTextRenderEventArgs
            (
                AccumulationChartConstants.ONTEXTRENDER,
                false,
                this,
                point,
#pragma warning disable CA1305
                !string.IsNullOrEmpty(point.OriginalText) ? point.OriginalText : point.Y.ToString(),
#pragma warning restore CA1305
                DataLabel.Fill,
                DataLabel.Border,
                DataLabel.Font);
            accumulationChart.AccumulationChartEvents?.OnDataLabelRender?.Invoke(textRenderEventArgs);
            point.AccTextArgs = textRenderEventArgs;
        }

        private AccumulationPoints SetPoints(object currentResult, int pointIndex, Type type)
        {
            AccumulationPoints currentPoint = new AccumulationPoints();
            if (currentResult.GetType().Name == JOBJECT)
            {
                JObject jsonObject = (JObject)currentResult;
                currentPoint.X = jsonObject.GetValue(XName, StringComparison.Ordinal);
                currentPoint.Y = (double)jsonObject.GetValue(YName, StringComparison.Ordinal);
                currentPoint.Color = (string)jsonObject.GetValue(PointColorMapping, StringComparison.Ordinal);
                currentPoint.SliceRadius = (string)jsonObject.GetValue(Radius, StringComparison.Ordinal);
                currentPoint.Tooltip = (string)jsonObject.GetValue(TooltipMappingName, StringComparison.Ordinal);
                currentPoint.Text = (string)jsonObject.GetValue(DataLabel.Name, StringComparison.Ordinal);
                currentPoint.OriginalText = (string)jsonObject.GetValue(DataLabel.Name, StringComparison.Ordinal);
            }
            else if (currentResult.GetType().Name == EXPANDOOBJECT)
            {
                foreach (KeyValuePair<string, object> property in (IDictionary<string, object>)currentResult)
                {
                    if (XName == property.Key)
                    {
                        currentPoint.X = property.Value;
                    }

                    if (YName == property.Key)
                    {
                        currentPoint.Y = (property.Value != null) ? Convert.ToDouble(property.Value, CultureInfo.InvariantCulture) : double.NaN;
                    }

                    if (PointColorMapping == property.Key)
                    {
                        currentPoint.Color = (string)property.Value;
                    }

                    if (Radius == property.Key)
                    {
                        currentPoint.SliceRadius = (string)property.Value;
                    }

                    if (TooltipMappingName == property.Key)
                    {
                        currentPoint.Tooltip = (string)property.Value;
                    }

                    if (DataLabel.Name == property.Key)
                    {
                        currentPoint.Text = (string)property.Value;
                    }

                    if (DataLabel.Name == property.Key)
                    {
                        currentPoint.OriginalText = (string)property.Value;
                    }
                }
            }
            else if (currentResult.GetType().BaseType.Equals(typeof(DynamicObject)))
            {
                currentPoint.X = ChartHelper.GetDynamicMember(currentResult, xName);
                currentPoint.Y = Math.Abs(Convert.ToDouble(ChartHelper.GetDynamicMember(currentResult, yName), CultureInfo.InvariantCulture));
                currentPoint.Color = Convert.ToString(ChartHelper.GetDynamicMember(currentResult, PointColorMapping), CultureInfo.InvariantCulture);
                currentPoint.SliceRadius = !radius.Contains("%", StringComparison.InvariantCulture) ? Convert.ToString(ChartHelper.GetDynamicMember(currentResult, Radius), CultureInfo.InvariantCulture) : Radius;
                currentPoint.Tooltip = Convert.ToString(Convert.ToString(ChartHelper.GetDynamicMember(currentResult, TooltipMappingName), CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                currentPoint.Text = Convert.ToString(ChartHelper.GetDynamicMember(currentResult, DataLabel.Name), CultureInfo.InvariantCulture);
                currentPoint.OriginalText = Convert.ToString(ChartHelper.GetDynamicMember(currentResult, DataLabel.Name), CultureInfo.InvariantCulture);
            }
            else
            {
                PropertyInfo xProp = type.GetProperty(XName),
                yProp = type.GetProperty(YName),
                colorProp = type.GetProperty(PointColorMapping),
                tooltipProp = type.GetProperty(TooltipMappingName),
                textProp = type.GetProperty(DataLabel.Name),
                radiusProp = type.GetProperty(Radius);
                currentPoint.X = xProp.GetValue(currentResult);
                currentPoint.Y = Math.Abs(Convert.ToDouble(yProp.GetValue(currentResult), null));
                currentPoint.Color = colorProp != null ? Convert.ToString(colorProp.GetValue(currentResult), CultureInfo.InvariantCulture) : null;
                currentPoint.SliceRadius = radiusProp != null ? Convert.ToString(radiusProp.GetValue(currentResult), CultureInfo.InvariantCulture) : null;
                currentPoint.Tooltip = tooltipProp != null ? Convert.ToString(tooltipProp.GetValue(currentResult), CultureInfo.InvariantCulture) : null;
                currentPoint.Text = textProp != null ? Convert.ToString(textProp.GetValue(currentResult), CultureInfo.InvariantCulture) : null;
                currentPoint.OriginalText = textProp != null ? Convert.ToString(textProp.GetValue(currentResult), CultureInfo.InvariantCulture) : null;
            }

            currentPoint.Percentage = Math.Round((double)currentPoint.Y / SumOfPoints * 100, 2);
            currentPoint.SliceRadius = string.IsNullOrEmpty(currentPoint.SliceRadius) ? "80%" : currentPoint.SliceRadius;
            SetAccEmptyPoint(currentPoint, pointIndex, type.GetProperty(YName));
            return currentPoint;
        }

        private void SetAccEmptyPoint(AccumulationPoints point, int i, PropertyInfo yProp)
        {
            if (point != null && yProp != null && (double.IsNaN((double)point.Y) || point.Y == 0 || point.Y == null))
            {
                point.Color = string.IsNullOrEmpty(EmptyPointSettings.Fill) ? point.Color : EmptyPointSettings.Fill;
                object prevPoint = i != 0 ? DataModule.ElementAt(i - 1) : null;
                object nextPoint = i != DataModule.Count() - 1 ? DataModule.ElementAt(i + 1) : null;
                switch (EmptyPointSettings.Mode)
                {
                    case EmptyPointMode.Zero:
                        point.Y = 0;
                        point.Visible = true;
                        break;
                    case EmptyPointMode.Average:
                        double previous = prevPoint != null ? (double)yProp.GetValue(prevPoint) : 0;
                        double next = nextPoint != null ? (double)yProp.GetValue(nextPoint) : 0;
                        point.Y = ((!double.IsNaN(previous) ? Math.Abs(previous) : 0) + (!double.IsNaN(next) ? Math.Abs(next) : 0)) / 2;
                        SumOfPoints += (double)point.Y;
                        point.Visible = true;
                        break;
                    default:
                        point.Visible = false;
                        point.LegendVisible = false;
                        break;
                }
            }
        }

        private bool IsEmpty(AccumulationPoints point)
        {
            return point.Color == EmptyPointSettings.Fill;
        }

        private bool IsClub(AccumulationPoints point, double clubValue, int index)
        {
            if (!double.IsNaN(clubValue))
            {
                if ((GroupMode == GroupMode.Value && Math.Abs((double)point.Y) <= clubValue) || (GroupMode == GroupMode.Point && index >= clubValue))
                {
                    sumOfClub += Math.Abs((double)point.Y);
                    return true;
                }
            }

            return false;
        }

        private void PushPoints(AccumulationPoints point, string[] colors)
        {
            point.Index = Points.Count;
            point.IsExplode = ExplodeAll ? ExplodeAll : point.Index == ExplodeIndex;
            point.Color = !string.IsNullOrEmpty(point.Color) ? point.Color : colors[point.Index % colors.Length];
            Points.Add(point);
            ProcessPointTextEvents(point);
        }

        internal AccumulationPoints GenerateClubPoint()
        {
            AccumulationPoints clubPoint = new AccumulationPoints()
            {
                IsClubbed = true,
                X = "Others",
                Y = sumOfClub,
                SliceRadius = "80%",
                AccTextArgs = new AccumulationTextRenderEventArgs()
            };
            clubPoint.AccTextArgs.Border = new ChartCommonBorder();
            clubPoint.AccTextArgs.Font = new ChartCommonFont();
            clubPoint.Text = clubPoint.OriginalText = clubPoint.X + ": " + sumOfClub;
            return clubPoint;
        }

        internal void ProcessingDataLabels(RenderTreeBuilder builder)
        {
            accumulationChart.DataLabelModule.FindAreaRect();
            LeftSidePoints.Clear();
            RightSidePoints.Clear();
            List<AccumulationPoints> firstQuarter = new List<AccumulationPoints>();
            List<AccumulationPoints> secondQuarter = new List<AccumulationPoints>();
            foreach (AccumulationPoints point in Points)
            {
                if (point.Visible)
                {
                    if (DataLabel.ShowZero || (!DataLabel.ShowZero && (point.Y != 0 || EmptyPointSettings.Mode == EmptyPointMode.Zero)))
                    {
                        accumulationChart.DataLabelModule.RenderDataLabel(point, DataLabel, Points, this);
                    }
                }

                if (point.MidAngle >= 90 && point.MidAngle <= 270)
                {
                    LeftSidePoints.Add(point);
                }
                else
                {
                    if (point.MidAngle >= 0 && point.MidAngle <= 90)
                    {
                        secondQuarter.Add(point);
                    }
                    else
                    {
                        firstQuarter.Add(point);
                    }
                }
            }
#pragma warning disable CA1806
            firstQuarter.OrderBy(x => x.MidAngle);
            secondQuarter.OrderBy(y => y.MidAngle);
            LeftSidePoints.OrderBy(z => z.MidAngle);
#pragma warning restore CA1806
            firstQuarter.AddRange(secondQuarter);
            RightSidePoints.AddRange(firstQuarter);
            if (builder != null)
            {
                accumulationChart.DataLabelModule.DrawDataLabels(this, builder);
            }
        }

#pragma warning disable CA1822
        internal void FindMaxBounds(Rect totalBound, Rect bound)
        {
            totalBound.X = bound.X < totalBound.X ? bound.X : totalBound.X;
            totalBound.Y = bound.Y < totalBound.Y ? bound.Y : totalBound.Y;
            totalBound.Height = (bound.Y + bound.Height) > totalBound.Height ? bound.Y + bound.Height : totalBound.Height;
            totalBound.Width = (bound.X + bound.Width) > totalBound.Width ? bound.X + bound.Width : totalBound.Width;
        }

        internal void RefreshPoints(List<AccumulationPoints> points)
#pragma warning restore CA1822
        {
            points.ForEach(x =>
            {
                x.LabelPosition = null;
                x.LabelRegion = null;
                x.LabelVisible = true;
            });
        }
    }
}