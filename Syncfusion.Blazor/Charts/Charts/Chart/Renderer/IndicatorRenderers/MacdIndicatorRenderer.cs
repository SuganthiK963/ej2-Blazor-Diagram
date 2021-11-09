using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class MacdIndicatorRenderer : IndicatorBase
    {
        internal override void InitSeriesCollection()
        {
            base.InitSeriesCollection();
            if (Indicator.MacdType == MacdType.Line || Indicator.MacdType == MacdType.Both)
            {
                SetSeriesProperties(new ChartSeries(), "MacdLine", Indicator.MacdLine.Color, Indicator.MacdLine.Width);
            }

            if (Indicator.MacdType == MacdType.Histogram || Indicator.MacdType == MacdType.Both)
            {
#pragma warning disable BL0005
                SetSeriesProperties(new ChartSeries() { Type = ChartSeriesType.Column }, "Histogram", Indicator.MacdPositiveColor, Indicator.Width);
#pragma warning disable BL0005
            }
        }

        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> signalCollection = new List<Point>();
            double fastPeriod = Indicator.FastPeriod, slowPeriod = Indicator.SlowPeriod, trigger = Indicator.Period, length = fastPeriod + trigger;
            List<Point> macdCollection = new List<Point>(), histogramCollection = new List<Point>();
            ChartSeries histogramSeries = null, macdLineSeries = null;
            if (Indicator.MacdType == MacdType.Histogram)
            {
                histogramSeries = Indicator.TargetSeries[1];
            }
            else
            {
                macdLineSeries = Indicator.TargetSeries[1];
                if (Indicator.MacdType == MacdType.Both)
                {
                    histogramSeries = Indicator.TargetSeries[2];
                }
            }

            List<Point> point = Points.Cast<Point>().ToList();
            if (Points != null && length < Points.Count && slowPeriod <= fastPeriod && slowPeriod > 0 && (length - 2) >= 0)
            {
                List<double> macdValues = GetMACDVales(CalculateEMAValues(slowPeriod, point, "Close"), CalculateEMAValues(fastPeriod, point, "Close"));
                macdCollection = GetMACDPoints(macdValues, point, (macdLineSeries != null) ? macdLineSeries : Indicator.TargetSeries[0]);
                List<double> signalEMA = CalculateEMAValues(trigger, macdCollection, "Y");
                signalCollection = GetSignalPoints(signalEMA, point, Indicator.TargetSeries[0]);
                if (histogramSeries != null)
                {
                    histogramCollection = GetHistogramPoints(macdValues, signalEMA, point, histogramSeries);
                }
            }

            SetSeriesRange(signalCollection, Indicator.TargetSeries[0]);
            if (histogramSeries != null)
            {
                SetSeriesRange(histogramCollection, histogramSeries);
            }

            if (macdLineSeries != null)
            {
                SetSeriesRange(macdCollection, macdLineSeries);
            }
        }

        private static List<double> CalculateEMAValues(double period, List<Point> validData, string field)
        {
            double sum = 0;
            List<double> emaValues = new List<double>();
            Type type = validData.ToArray().First().GetType();
            for (int i = 0; i < period; i++)
            {
                sum += Convert.ToDouble(type.GetProperty(field).GetValue(validData[i]), null);
            }

            double initialEMA = sum / period;
            emaValues.Add(initialEMA);
            double emaAvg = initialEMA;
            for (int j = Convert.ToInt32(period); j < validData.Count; j++)
            {
                emaAvg = ((Convert.ToDouble(type.GetProperty(field).GetValue(validData[j]), null) - emaAvg) * (2 / (period + 1))) + emaAvg;
                emaValues.Add(emaAvg);
            }

            return emaValues;
        }

        private List<double> GetMACDVales(List<double> shortEma, List<double> longEma)
        {
            List<double> macdPoints = new List<double>();
            int diff = Convert.ToInt32(Indicator.FastPeriod - Indicator.SlowPeriod);
            for (int i = 0; i < longEma.Count; i++)
            {
                macdPoints.Add(shortEma[i + diff] - longEma[i]);
            }

            return macdPoints;
        }

        private List<Point> GetMACDPoints(List<double> macdPoints, List<Point> validData, ChartSeries series)
        {
            List<Point> macdCollection = new List<Point>();
            int dataMACDIndex = Convert.ToInt32(Indicator.FastPeriod - 1);
            int macdIndex = 0;
            while (dataMACDIndex < validData.Count)
            {
                macdCollection.Add(GetDataPoint(validData[dataMACDIndex].X, macdPoints[macdIndex], validData[dataMACDIndex].XValue, series, macdCollection.Count));
                dataMACDIndex++;
                macdIndex++;
            }

            return macdCollection;
        }

        private List<Point> GetSignalPoints(List<double> signalEma, List<Point> validData, ChartSeries series)
        {
            int dataSignalIndex = Convert.ToInt32(Indicator.FastPeriod + Indicator.Period - 2), signalIndex = 0;
            List<Point> signalCollection = new List<Point>();
            while (dataSignalIndex < validData.Count)
            {
                signalCollection.Add(GetDataPoint(validData[dataSignalIndex].X, signalEma[signalIndex], validData[dataSignalIndex].XValue, series, signalCollection.Count));
                dataSignalIndex++;
                signalIndex++;
            }

            return signalCollection;
        }

        private List<Point> GetHistogramPoints(List<double> macdPoints, List<double> signalEma, List<Point> validData, ChartSeries series)
        {
            int dataHistogramIndex = Convert.ToInt32(Indicator.FastPeriod + Indicator.Period - 2);
            int histogramIndex = 0;
            List<Point> histogramCollection = new List<Point>();
            while (dataHistogramIndex < validData.Count)
            {
                histogramCollection.Add(GetDataPoint(validData[dataHistogramIndex].X, macdPoints[histogramIndex + Convert.ToInt32(Indicator.Period - 1)] - signalEma[histogramIndex], validData[dataHistogramIndex].XValue, series, histogramCollection.Count));
                dataHistogramIndex++;
                histogramIndex++;
            }

            return histogramCollection;
        }
    }
}