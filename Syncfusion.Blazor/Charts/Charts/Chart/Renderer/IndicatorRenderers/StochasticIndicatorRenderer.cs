using System;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class StochasticIndicatorRenderer : IndicatorBase
    {
        internal override void InitSeriesCollection()
        {
            base.InitSeriesCollection();
            SetSeriesProperties(new ChartSeries(), "PeriodLine", Indicator.PeriodLine.Color, Indicator.PeriodLine.Width);
            if (Indicator.ShowZones)
            {
                SetSeriesProperties(new ChartSeries(), "UpperLine", Indicator.UpperLine.Color, Indicator.UpperLine.Width);
                SetSeriesProperties(new ChartSeries(), "LowerLine", Indicator.LowerLine.Color, Indicator.LowerLine.Width);
            }
        }

        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> signalCollection = new List<Point>(), upperCollection = new List<Point>(), lowerCollection = new List<Point>(), periodCollection = new List<Point>(), source = new List<Point>();
            if (Points.Count != 0 && Points.Count >= Indicator.Period)
            {
                if (Indicator.ShowZones)
                {
                    for (int i = 0; i < Points.Count; i++)
                    {
                        upperCollection.Add(GetDataPoint(Points[i].X, Indicator.OverBought, Points[i].XValue, Indicator.TargetSeries[2], upperCollection.Count));
                        lowerCollection.Add(GetDataPoint(Points[i].X, Indicator.OverSold, Points[i].XValue, Indicator.TargetSeries[3], lowerCollection.Count));
                    }
                }

                source = CalculatePeriod(Indicator.Period, Points, Indicator.TargetSeries[1]);
                periodCollection = SmaCalculation(Indicator.Period, Indicator.KPeriod, source, Indicator.TargetSeries[1]);
                signalCollection = SmaCalculation(Indicator.Period + Indicator.KPeriod - 1, Indicator.DPeriod, source, Indicator.TargetSeries[0]);
            }

            SetSeriesRange(signalCollection, Indicator.TargetSeries[0]);
            SetSeriesRange(periodCollection, Indicator.TargetSeries[1]);
            if (Indicator.ShowZones)
            {
                SetSeriesRange(upperCollection, Indicator.TargetSeries[2]);
                SetSeriesRange(lowerCollection, Indicator.TargetSeries[3]);
            }
        }

        private List<Point> CalculatePeriod(double period, List<FinancialPoint> data, ChartSeries series)
        {
            List<object> lowValues = new List<object>(), highValues = new List<object>(), closeValues = new List<object>();
            List<Point> modifiedSource = new List<Point>();
            for (int j = 0; j < data.Count; j++)
            {
                lowValues.Insert(j, data[j].Low);
                highValues.Insert(j, data[j].High);
                closeValues.Insert(j, data[j].Close);
            }

            if (data.Count > period)
            {
                List<object> mins = new List<object>(), maxs = new List<object>();
                for (int i = 0; i < period - 1; ++i)
                {
                    maxs.Add(0);
                    mins.Add(0);
                    modifiedSource.Add(GetDataPoint(data[i].X, data[i].Close, data[i].XValue, series, modifiedSource.Count));
                }

                for (int i = Convert.ToInt32(period - 1); i < data.Count; ++i)
                {
                    double min = double.MaxValue, max = double.MinValue;
                    for (int j = 0; j < period; ++j)
                    {
                        min = Math.Min(min, Convert.ToDouble(lowValues[i - j], null));
                        max = Math.Max(max, Convert.ToDouble(highValues[i - j], null));
                    }

                    maxs.Add(max);
                    mins.Add(min);
                }

                for (int i = Convert.ToInt32(period - 1); i < data.Count; ++i)
                {
                    double top = 0, bottom = 0;
                    top += Convert.ToDouble(closeValues[i], null) - Convert.ToDouble(mins[i], null);
                    bottom += Convert.ToDouble(maxs[i], null) - Convert.ToDouble(mins[i], null);
                    modifiedSource.Add(GetDataPoint(data[i].X, (top / bottom) * 100, data[i].XValue, series, modifiedSource.Count));
                }
            }

            return modifiedSource;
        }

        private List<Point> SmaCalculation(double period, double kperiod, List<Point> data, ChartSeries sourceSeries)
        {
            List<Point> pointCollection = new List<Point>();
            if (data.Count >= period + kperiod)
            {
                double count = period + (kperiod - 1);
                List<object> temp = new List<object>(), values = new List<object>();
                for (int i = 0; i < data.Count; i++)
                {
                    temp.Add(Convert.ToDouble(data[i].Y, null));
                }

                int length = temp.Count;
                while (length >= count)
                {
                    double sum = 0;
                    for (int i = Convert.ToInt32(period - 1); i < (period + kperiod - 1); i++)
                    {
                        sum = sum + Convert.ToDouble(temp[i], null);
                    }

                    sum = sum / kperiod;
                    values.Add(Convert.ToDouble(sum.ToString("N2", CultureInfo.InvariantCulture), null));
                    temp.RemoveRange(0, 1);
                    length = temp.Count;
                }

                int len = Convert.ToInt32(count - 1);
                for (int i = 0; i < data.Count; i++)
                {
                    if (!(i < len))
                    {
                        pointCollection.Add(GetDataPoint(data[i].X, Convert.ToDouble(values[i - len], null), data[i].XValue, sourceSeries, pointCollection.Count));
                        data[i].Y = Convert.ToDouble(values[i - len], null);
                    }
                }
            }

            return pointCollection;
        }
    }
}