using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class BollingerBandsIndicatorRenderer : IndicatorBase
    {
        internal override void InitSeriesCollection()
        {
            Indicator.TargetSeries = new List<ChartSeries>();
#pragma warning disable BL0005
#pragma warning disable CA2000
            ChartSeries rangeArea = new ChartSeries() { Type = ChartSeriesType.RangeArea };
#pragma warning restore CA2000
#pragma warning restore BL0005
            if (Indicator.BandColor != Constants.TRANSPARENT && Indicator.BandColor != "none")
            {
                SetSeriesProperties(rangeArea, "BollingerBand", Indicator.BandColor, 0);
            }

            SetSeriesProperties(new ChartSeries(), "BollingerBand", Indicator.Fill, Indicator.Width);
            SetSeriesProperties(new ChartSeries(), "UpperLine", Indicator.UpperLine.Color, Indicator.UpperLine.Width);
            SetSeriesProperties(new ChartSeries(), "LowerLine", Indicator.LowerLine.Color, Indicator.LowerLine.Width);
        }

        internal override void InitDataSource()
        {
            base.InitDataSource();
            bool enableBand = Indicator.BandColor != Constants.TRANSPARENT && Indicator.BandColor != "none";
            int start = enableBand ? 1 : 0;
            List<Point> signalCollection = new List<Point>(), upperCollection = new List<Point>(), lowerCollection = new List<Point>(), bandCollection = new List<Point>();
            ChartSeries upperSeries = Indicator.TargetSeries[start + 1], lowerSeries = Indicator.TargetSeries[start + 2], signalSeries = Indicator.TargetSeries[start], rangeAreaSeries = enableBand ? Indicator.TargetSeries[0] : null;
            if (Points.Count != 0 && Points.Count >= Indicator.Period)
            {
                double sum = 0, deviationSum = 0, multiplier = Indicator.StandardDeviation;
                int limit = Points.Count, length = Convert.ToInt32(Math.Round(Indicator.Period));
                List<double> smaPoints = new List<double>(), deviations = new List<double>();
                List<BollingerPoints> bollingerPoints = new List<BollingerPoints>();
                for (int i = 0; i < length; i++)
                {
                    sum += Convert.ToDouble(Points[i].Close, null);
                }

                double diffSum = sum / Indicator.Period;
                for (int l = 0; l < limit; l++)
                {
                    double y = Convert.ToDouble(Points[l].Close, null);
                    if (l >= length - 1 && l < limit)
                    {
                        if (l - Indicator.Period >= 0)
                        {
                            sum = sum + (y - Convert.ToDouble(Points[l - length].Close, null));
                            diffSum = sum / Indicator.Period;
                            smaPoints.Insert(l, diffSum);
                            deviations.Insert(l, Math.Pow(y - diffSum, 2));
                            deviationSum += deviations[l] - deviations[l - length];
                        }
                        else
                        {
                            smaPoints.Insert(l, diffSum);
                            deviations.Insert(l, Math.Pow(y - diffSum, 2));
                            deviationSum += deviations[l];
                        }

                        double range = Math.Sqrt(deviationSum / Indicator.Period);
                        double lowerBand = smaPoints[l] - (multiplier * range);
                        double upperBand = smaPoints[l] + (multiplier * range);
                        if (l + 1 == length)
                        {
                            for (int j = 0; j < length - 1; j++)
                            {
                                bollingerPoints.Insert(j, new BollingerPoints
                                {
                                    MiddleBound = smaPoints[l],
                                    LowerBound = lowerBand,
                                    UpperBound = upperBand
                                });
                            }
                        }

                        bollingerPoints.Insert(l, new BollingerPoints
                        {
                            MiddleBound = smaPoints[l],
                            LowerBound = lowerBand,
                            UpperBound = upperBand
                        });
                    }
                    else
                    {
                        if (l < Indicator.Period - 1)
                        {
                            smaPoints.Insert(l, diffSum);
                            deviations.Insert(l, Math.Pow(y - diffSum, 2));
                            deviationSum += deviations[l];
                        }
                    }
                }

                int i2 = -1, j1 = -1;
                for (int k = 0; k < limit; k++)
                {
                    if (k >= (length - 1))
                    {
                        upperCollection.Add(GetDataPoint(Points[k].X, bollingerPoints[k].UpperBound, Points[k].XValue, upperSeries, upperCollection.Count));
                        lowerCollection.Add(GetDataPoint(Points[k].X, bollingerPoints[k].LowerBound, Points[k].XValue, lowerSeries, lowerCollection.Count));
                        signalCollection.Add(GetDataPoint(Points[k].X, bollingerPoints[k].MiddleBound, Points[k].XValue, signalSeries, signalCollection.Count));
                        if (enableBand)
                        {
                            bandCollection.Add(GetRangePoint(Points[k].X, upperCollection[++i2].Y, lowerCollection[++j1].Y, Points[k].XValue, rangeAreaSeries, bandCollection.Count));
                        }
                    }
                }
            }

            if (enableBand)
            {
                SetSeriesRange(bandCollection, Indicator.TargetSeries[0]);
            }

            SetSeriesRange(signalCollection, Indicator.TargetSeries[start]);
            SetSeriesRange(upperCollection, Indicator.TargetSeries[start + 1]);
            SetSeriesRange(lowerCollection, Indicator.TargetSeries[start + 2]);
        }

        private static Point GetRangePoint(object x, object high, object low, double xvalue, ChartSeries series, int index)
        {
            Point point = new FinancialPoint()
            {
                X = x,
                High = Convert.ToDouble(high, null),
                Low = Convert.ToDouble(low, null),
                XValue = xvalue,
                Interior = series.Fill,
                Index = index,
                Visible = true,
            };

            // series.XData.Add(point.XValue);
            return point;
        }
    }
}