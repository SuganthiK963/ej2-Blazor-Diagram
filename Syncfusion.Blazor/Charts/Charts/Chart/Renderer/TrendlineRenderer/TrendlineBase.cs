using Syncfusion.Blazor.Charts.Chart.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class TrendlineBase
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private ChartSeries series;
        private SfChart chart;
        private List<Point> points;
        private ChartSeries trendLineSeries;
        private double?[] polynomialSlopes;

        internal ChartTrendline Trendline { get; set; }

        private static double GetPolynomialYValue(double?[] slopes, double x)
        {
            double sum = 0;
            for (int index = 0; index < slopes.Length; index++)
            {
                sum += (double)slopes[index] * Math.Pow(x, index);
            }

            return sum;
        }

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
        private static bool GaussJordanElimination(double?[,] matrix, double?[] polynomialSlopes)
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional
        {
            int length = matrix.GetLength(0), index = 0, index1 = 0, index5, iindex4, iandex4, iindex1, iindex2;
            double?[] numArray1 = new double?[length];
            double?[] numArray2 = new double?[length];
            double?[] numArray3 = new double?[length];
            double num, num1, num2, num3, num4;
            while (index < length)
            {
                numArray3[index] = 0;
                ++index;
            }

            while (index1 < length)
            {
                num1 = 0;
                int index2 = 0, index3 = 0, index4 = 0;
                while (index4 < length)
                {
                    if (numArray3[index4] != 1)
                    {
                        index5 = 0;
                        while (index5 < length)
                        {
                            if (numArray3[index5] == 0 && Math.Abs((double)matrix[index4, index5]) >= num1)
                            {
                                num1 = Math.Abs((double)matrix[index4, index5]);
                                index2 = index4;
                                index3 = index5;
                            }

                            ++index5;
                        }
                    }

                    ++index4;
                }

                ++numArray3[index3];
                if (index2 != index3)
                {
                    index4 = 0;
                    while (index4 < length)
                    {
                        num2 = (double)matrix[index2, index4];
                        matrix[index2, index4] = matrix[index3, index4];
                        matrix[index3, index4] = num2;
                        ++index4;
                    }

                    num3 = (double)polynomialSlopes[index2];
                    polynomialSlopes[index2] = polynomialSlopes[index3];
                    polynomialSlopes[index3] = num3;
                }

                numArray2[index1] = index2;
                numArray1[index1] = index3;
                if (matrix[index3, index3] == 0.0)
                {
                    return false;
                }

                num4 = Convert.ToDouble(1.0 / matrix[index3, index3], null);
                matrix[index3, index3] = 1.0;
                iindex4 = 0;
                while (iindex4 < length)
                {
                    matrix[index3, iindex4] *= num4;
                    ++iindex4;
                }

                polynomialSlopes[index3] *= num4;
                iandex4 = 0;
                while (iandex4 < length)
                {
                    if (iandex4 != index3)
                    {
                        num2 = (double)matrix[iandex4, index3];
                        matrix[iandex4, index3] = 0.0;
                        index5 = 0;
                        while (index5 < length)
                        {
                            matrix[iandex4, index5] -= matrix[index3, index5] * num2;
                            ++index5;
                        }

                        polynomialSlopes[iandex4] -= polynomialSlopes[index3] * num2;
                    }

                    ++iandex4;
                }

                ++index1;
            }

            iindex1 = length - 1;
            while (iindex1 >= 0)
            {
                if (numArray2[iindex1] != numArray1[iindex1])
                {
                    iindex2 = 0;
                    while (iindex2 < length)
                    {
                        num = Convert.ToDouble(matrix[iindex2, (int)numArray2[iindex1]], null);
                        matrix[iindex2, (int)numArray2[iindex1]] = matrix[iindex2, (int)numArray1[iindex1]];
                        matrix[iindex2, (int)numArray1[iindex1]] = num;
                        ++iindex2;
                    }
                }

                --iindex1;
            }

            return true;
        }

        private void InitPriavteInstances()
        {
            series = Trendline.Parent.Series;
            chart = series.Container;
        }

        internal void InitSeriesCollection()
        {
            InitPriavteInstances();
            trendLineSeries = new ChartSeries();
            if (Trendline.Type == TrendlineTypes.Linear || Trendline.Type == TrendlineTypes.MovingAverage)
            {
                trendLineSeries.SetTrendlineType(ChartSeriesType.Line);
                trendLineSeries.RendererType = typeof(Trendline_LineSeriesRenderer);
            }
            else
            {
                trendLineSeries.SetTrendlineType(ChartSeriesType.Spline);
                trendLineSeries.RendererType = typeof(Trendline_SplineSeriesRenderer);
            }

            SetSeriesProperties();
        }

        private void SetSeriesProperties()
        {
            if (trendLineSeries == null)
            {
                return;
            }

            string fill = !string.IsNullOrEmpty(Trendline.Fill) ? Trendline.Fill : "blue";
            LegendShape legendShape = LegendShape.HorizontalLine;
            ChartSeriesBorder border = new ChartSeriesBorder { };
            ChartSeriesConnector connector = new ChartSeriesConnector { };
            trendLineSeries.SetTrendlineValues(Trendline.Name, "x", "y", Trendline.DashArray, Trendline.Width, fill, legendShape, Trendline.EnableTooltip, border, connector);
            trendLineSeries.Container = chart;
            trendLineSeries.RendererKey += Trendline.RendererKey;
            Trendline.TargetSeries = trendLineSeries;
        }

        internal void UpdateTrendlineMarker()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            ChartMarker marker = new ChartMarker();
#pragma warning restore CA2000 // Dispose objects before losing scope
            marker.SetMarkerValues(Trendline.Marker);
        }

        internal void UpdateTrendlineAnimation()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            ChartDefaultAnimation animation = new ChartDefaultAnimation();
#pragma warning restore CA2000 // Dispose objects before losing scope
            animation.SetTrendlineAnimation(Trendline.Animation.Enable, Trendline.Animation.Duration, Trendline.Animation.Delay);
        }

        internal void InitDataSource()
        {
            points = series.Renderer.Points;
            if (points != null && points.Count > 0)
            {
                switch (Trendline.Type)
                {
                    case TrendlineTypes.Linear:
                        SetLinearRange();
                        break;
                    case TrendlineTypes.Exponential:
                        SetExponentialRange();
                        break;
                    case TrendlineTypes.MovingAverage:
                        SetMovingAverageRange();
                        break;
                    case TrendlineTypes.Polynomial:
                        SetPolynomialRange();
                        break;
                    case TrendlineTypes.Power:
                        SetPowerRange();
                        break;
                    case TrendlineTypes.Logarithmic:
                        SetLogarithmicRange();
                        break;
                }
            }

            if (Trendline.Type != TrendlineTypes.Linear && Trendline.Type != TrendlineTypes.MovingAverage)
            {
                trendLineSeries.Renderer.FindSplinePoint();
            }
        }

        internal void InitiateAxis()
        {
            ChartAxisRenderer x_AxisRender = series.Renderer.XAxisRenderer;
            ChartAxisRenderer y_AxisRender = series.Renderer.YAxisRenderer;
            ChartSeriesRenderer trendlineSeriesRenderer = trendLineSeries.Renderer;
            trendlineSeriesRenderer.XAxisRenderer = x_AxisRender;
            trendlineSeriesRenderer.YAxisRenderer = y_AxisRender;
            x_AxisRender.SeriesRenderer.Add(trendlineSeriesRenderer);
            y_AxisRender.SeriesRenderer.Add(trendlineSeriesRenderer);
        }

        private void SetLinearRange()
        {
            List<double> x_Values = new List<double>();
            List<double> y_Values = new List<double>();
            for (int index = 0; index < points.Count; index++)
            {
                x_Values.Add(points[index].XValue);
                y_Values.Add(points[index].YValue);
            }

            trendLineSeries.Renderer.Points = GetLinearPoints(x_Values, FindSlopeIntercept(x_Values, y_Values));
        }

        private List<Point> GetLinearPoints(List<double> x_Values, SlopeIntercept slopeInterceptLinear)
        {
            List<Point> pts = new List<Point>();
            int max = x_Values.IndexOf(x_Values.Max());
            int min = x_Values.IndexOf(x_Values.Min());
            double x1Linear = x_Values[min] - Trendline.BackwardForecast;
            double x2Linear = x_Values[max] + Trendline.ForwardForecast;
            pts.Add(GetDataPoint(x1Linear, (slopeInterceptLinear.Slope * x1Linear) + slopeInterceptLinear.Intercept, pts.Count));
            pts.Add(GetDataPoint(x2Linear, (slopeInterceptLinear.Slope * x2Linear) + slopeInterceptLinear.Intercept, pts.Count));
            return pts;
        }

        private void SetExponentialRange()
        {
            List<double> x_Value = new List<double>();
            List<double> y_Value = new List<double>();
            for (int index = 0; index < points.Count; index++)
            {
                x_Value.Add(points[index].XValue);
                y_Value.Add(points[index].YValue != 0 ? Math.Log(points[index].YValue) : 0);
            }

            trendLineSeries.Renderer.Points = GetExponentialPoints(x_Value, FindSlopeIntercept(x_Value, y_Value));
        }

        private List<Point> GetExponentialPoints(List<double> x_Values, SlopeIntercept slopeInterceptExp)
        {
            int midPoint = Convert.ToInt32(Math.Round((double)points.Count / 2));
            List<Point> ptsExp = new List<Point>();
            double x1 = x_Values[0] - Trendline.BackwardForecast;
            double x2 = x_Values[midPoint - 1];
            double x3 = x_Values[x_Values.Count - 1] + Trendline.ForwardForecast;
            ptsExp.Add(GetDataPoint(x1, slopeInterceptExp.Intercept * Math.Exp(slopeInterceptExp.Slope * x1), ptsExp.Count));
            ptsExp.Add(GetDataPoint(x2, slopeInterceptExp.Intercept * Math.Exp(slopeInterceptExp.Slope * x2), ptsExp.Count));
            ptsExp.Add(GetDataPoint(x3, slopeInterceptExp.Intercept * Math.Exp(slopeInterceptExp.Slope * x3), ptsExp.Count));
            return ptsExp;
        }

        private void SetMovingAverageRange()
        {
            List<double> x_Values = new List<double>();
            List<double> y_Values = new List<double>();
            List<double> x_AvgValues = new List<double>();
            for (int index = 0; index < points.Count; index++)
            {
                x_AvgValues.Add(points[index].XValue);
                x_Values.Add(index + 1);
                y_Values.Add(points[index].YValue);
            }

            trendLineSeries.Renderer.Points = GetMovingAveragePoints(x_AvgValues, y_Values);
        }

        private List<Point> GetMovingAveragePoints(List<double> x_Values, List<double> y_Values)
        {
            List<Point> pts = new List<Point>();
            double period = Math.Max(2, Trendline.Period >= points.Count ? points.Count - 1 : Trendline.Period);
            int index = 0;
            while ((period + index) <= points.Count)
            {
                double y = 0, count = 0, nullCount = 0;
                for (int j = index; count < period; j++)
                {
                    count++;
                    if (y_Values[j] == 0 || double.IsNaN(y_Values[j]))
                    {
                        nullCount++;
                    }

                    y += y_Values[j];
                }

                y = period - nullCount <= 0 ? double.NaN : y / (period - nullCount);
                if (y != 0 && !double.IsNaN(y))
                {
                    pts.Add(GetDataPoint(x_Values[Convert.ToInt32(period - 1 + index)], y, pts.Count));
                }

                index++;
            }

            return pts;
        }

        private void SetPolynomialRange()
        {
            List<double> x_PolyValues = new List<double>();
            List<double> y_PolyValues = new List<double>();
            for (int index = 0; index < points.Count; index++)
            {
                x_PolyValues.Add(points[index].XValue);
                y_PolyValues.Add(points[index].YValue);
            }

            trendLineSeries.Renderer.Points = GetPolynomialPoints(x_PolyValues, y_PolyValues);
        }

        private Point GetDataPoint(double x, double y, int index)
        {
            Point trendPoint = new Point()
            {
                X = x,
                Y = y,
                XValue = x,
                Interior = trendLineSeries.Fill,
                Index = index,
                YValue = y,
                Visible = true
            };
            trendLineSeries.Renderer.XMin = Math.Min(trendLineSeries.Renderer.XMin, trendPoint.XValue);
            trendLineSeries.Renderer.YMin = Math.Min(trendLineSeries.Renderer.YMin, trendPoint.YValue);
            trendLineSeries.Renderer.XMax = Math.Max(trendLineSeries.Renderer.XMax, trendPoint.XValue);
            trendLineSeries.Renderer.YMax = Math.Max(trendLineSeries.Renderer.YMax, trendPoint.YValue);
            trendLineSeries.Renderer.XData.Add(trendPoint.XValue);
            return trendPoint;
        }

        private SlopeIntercept FindSlopeIntercept(List<double> x_Values, List<double> y_Values)
        {
            double x_Avg = 0, y_Avg = 0, xy_Avg = 0, xx_Avg = 0, yy_Avg = 0, slope = 0, intercept;
            for (int index = 0; index < points.Count; index++)
            {
                if (double.IsNaN(y_Values[index]))
                {
                    y_Values[index] = (y_Values[index - 1] + y_Values[index + 1]) / 2;
                }

                x_Avg += x_Values[index];
                y_Avg += y_Values[index];
                xy_Avg += x_Values[index] * y_Values[index];
                xx_Avg += x_Values[index] * x_Values[index];
                yy_Avg += y_Values[index] * y_Values[index];
            }

            if (Trendline.Intercept != 0 && !double.IsNaN(Trendline.Intercept) && (Trendline.Type == TrendlineTypes.Linear || Trendline.Type == TrendlineTypes.Exponential))
            {
                intercept = Trendline.Intercept;
                switch (Trendline.Type)
                {
                    case TrendlineTypes.Linear:
                        slope = (xy_Avg - (Trendline.Intercept * x_Avg)) / xx_Avg;
                        break;
                    case TrendlineTypes.Exponential:
                        slope = (xy_Avg - (Math.Log(Math.Abs(Trendline.Intercept)) * x_Avg)) / xx_Avg;
                        break;
                }
            }
            else
            {
                slope = ((points.Count * xy_Avg) - (x_Avg * y_Avg)) / ((points.Count * xx_Avg) - (x_Avg * x_Avg));
                slope = Trendline.Type == TrendlineTypes.Linear ? slope : Math.Abs(slope);
                if (Trendline.Type == TrendlineTypes.Exponential || Trendline.Type == TrendlineTypes.Power)
                {
                    intercept = Math.Exp((y_Avg - (slope * x_Avg)) / points.Count);
                }
                else
                {
                    intercept = (y_Avg - (slope * x_Avg)) / points.Count;
                }
            }

            return new SlopeIntercept { Slope = slope, Intercept = intercept };
        }

        private List<Point> GetPolynomialPoints(List<double> x_Values, List<double> y_Values)
        {
            double polynomialOrder = points.Count <= Trendline.PolynomialOrder ? points.Count : Trendline.PolynomialOrder;
            polynomialOrder = Math.Max(2, polynomialOrder);
            polynomialOrder = Math.Min(6, polynomialOrder);
#pragma warning disable CA2000 // Dispose objects before losing scope
            ChartTrendline chartTrendline = new ChartTrendline();
#pragma warning restore CA2000 // Dispose objects before losing scope
            chartTrendline.PolynomialOrderValue(polynomialOrder);
            polynomialSlopes = new double?[Convert.ToInt32(Trendline.PolynomialOrder + 1)];
            int index = 0, subIndex;
            while (index < x_Values.Count)
            {
                subIndex = 0;
                while (subIndex <= Trendline.PolynomialOrder)
                {
                    if (!polynomialSlopes[subIndex].HasValue)
                    {
                        polynomialSlopes[subIndex] = 0;
                    }

                    polynomialSlopes[subIndex] += Math.Pow(x_Values[index], subIndex) * y_Values[index];
                    ++subIndex;
                }

                index++;
            }

            double?[] numArray = new double?[Convert.ToInt32(1 + (2 * Trendline.PolynomialOrder))];
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
            double?[,] matrix = new double?[Convert.ToInt32(Trendline.PolynomialOrder + 1, null), 3];
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional
            int index1 = 0, num1 = 0, index2, index3 = 0, index4;
            while (index1 < x_Values.Count)
            {
                double d = x_Values[index1], num2 = 1.0;
                index2 = 0;
                while (index2 < numArray.Length)
                {
                    if (!numArray[index2].HasValue)
                    {
                        numArray[index2] = 0;
                    }

                    numArray[index2] += num2;
                    num2 *= d;
                    ++num1;
                    ++index2;
                }

                ++index1;
            }

            while (index3 <= Trendline.PolynomialOrder)
            {
                index4 = 0;
                while (index4 <= Trendline.PolynomialOrder)
                {
                    matrix[index3, index4] = numArray[index3 + index4];
                    ++index4;
                }

                ++index3;
            }

            if (!GaussJordanElimination(matrix, polynomialSlopes))
            {
                polynomialSlopes = null;
            }

            return GetPoints(x_Values);
        }

        private List<Point> GetPoints(List<double> x_Values)
        {
            List<Point> pts = new List<Point>();
            double x1 = 1, x_Value, y_Value;
            for (int index = 1; index <= polynomialSlopes.Length; index++)
            {
                if (index == 1)
                {
                    x_Value = x_Values[0] - Trendline.BackwardForecast;
                    y_Value = GetPolynomialYValue(polynomialSlopes, x_Value);
                    pts.Add(GetDataPoint(x_Value, y_Value, pts.Count));
                }
                else if (index == polynomialSlopes.Length)
                {
                    x_Value = x_Values[points.Count - 1] + Trendline.ForwardForecast;
                    y_Value = GetPolynomialYValue(polynomialSlopes, x_Value);
                    pts.Add(GetDataPoint(x_Value, y_Value, pts.Count));
                }
                else
                {
                    x1 += (points.Count + Trendline.ForwardForecast) / polynomialSlopes.Length;
                    x_Value = x_Values[Convert.ToInt32(Math.Round(x1)) - 1];
                    y_Value = GetPolynomialYValue(polynomialSlopes, x_Value);
                    pts.Add(GetDataPoint(x_Value, y_Value, pts.Count));
                }
            }

            return pts;
        }

        private void SetPowerRange()
        {
            List<double> x_Values = new List<double>();
            List<double> y_Values = new List<double>();
            List<double> powerPoints = new List<double>();
            for (int index = 0; index < points.Count; index++)
            {
                powerPoints.Add(points[index].XValue);
                x_Values.Add(points[index].XValue != 0 && !double.IsNaN(points[index].XValue) ? Math.Log(points[index].XValue) : 0);
                y_Values.Add(points[index].YValue != 0 && !double.IsNaN(points[index].YValue) ? Math.Log(points[index].YValue) : 0);
            }

            trendLineSeries.Renderer.Points = GetPowerPoints(powerPoints, FindSlopeIntercept(x_Values, y_Values));
        }

        private List<Point> GetPowerPoints(List<double> x_Values, SlopeIntercept slopeInterceptPower)
        {
            int midPoint = Convert.ToInt32(Math.Round((double)(points.Count / 2)));
            List<Point> pts = new List<Point>();
            double x1 = x_Values[0] - Trendline.BackwardForecast;
            x1 = x1 > -1 ? x1 : 0;
            double x2 = x_Values[midPoint - 1];
            double x3 = x_Values[x_Values.Count - 1] + Trendline.ForwardForecast;
            pts.Add(GetDataPoint(x1, slopeInterceptPower.Intercept * Math.Pow(x1, slopeInterceptPower.Slope), pts.Count));
            pts.Add(GetDataPoint(x2, slopeInterceptPower.Intercept * Math.Pow(x2, slopeInterceptPower.Slope), pts.Count));
            pts.Add(GetDataPoint(x3, slopeInterceptPower.Intercept * Math.Pow(x3, slopeInterceptPower.Slope), pts.Count));
            return pts;
        }

        private void SetLogarithmicRange()
        {
            List<double> x_LogValue = new List<double>();
            List<double> y_LogValue = new List<double>();
            List<double> x_PointsLgr = new List<double>();
            for (int index = 0; index < points.Count; index++)
            {
                x_PointsLgr.Add(points[index].XValue);
                x_LogValue.Add(points[index].XValue != 0 && !double.IsNaN(points[index].XValue) ? Math.Log(points[index].XValue) : 0);
                y_LogValue.Add(points[index].YValue);
            }

            trendLineSeries.Renderer.Points = GetLogarithmicPoints(x_PointsLgr, FindSlopeIntercept(x_LogValue, y_LogValue));
        }

        private List<Point> GetLogarithmicPoints(List<double> x_Values, SlopeIntercept slopeInterceptLog)
        {
            int midPoint = Convert.ToInt32(Math.Round((double)(points.Count / 2)));
            List<Point> pts = new List<Point>();
            double x1Log = x_Values[0] - Trendline.BackwardForecast;
            double y1Log = slopeInterceptLog.Intercept + (slopeInterceptLog.Slope * (x1Log != 0 && !double.IsNaN(x1Log) ? Math.Log(x1Log) : 0));
            double x2Log = x_Values[midPoint - 1];
            double y2Log = slopeInterceptLog.Intercept + (slopeInterceptLog.Slope * (x2Log != 0 && !double.IsNaN(x2Log) ? Math.Log(x2Log) : 0));
            double x3Log = x_Values[x_Values.Count - 1] + Trendline.ForwardForecast;
            double y3Log = slopeInterceptLog.Intercept + (slopeInterceptLog.Slope * (x3Log != 0 && !double.IsNaN(x3Log) ? Math.Log(x3Log) : 0));
            pts.Add(GetDataPoint(x1Log, y1Log, pts.Count));
            pts.Add(GetDataPoint(x2Log, y2Log, pts.Count));
            pts.Add(GetDataPoint(x3Log, y3Log, pts.Count));
            return pts;
        }
    }
}
