using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class RSIIndicatorRenderer : IndicatorBase
    {
        internal override void InitSeriesCollection()
        {
            base.InitSeriesCollection();
            if (Indicator.ShowZones)
            {
                SetSeriesProperties(new ChartSeries(), "LowerLine", Indicator.LowerLine.Color, Indicator.LowerLine.Width);
                SetSeriesProperties(new ChartSeries(), "UpperLine", Indicator.UpperLine.Color, Indicator.UpperLine.Width);
            }
        }

        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> signalCollection = new List<Point>(), lowerCollection = new List<Point>(), upperCollection = new List<Point>();
            if (Points.Count != 0 && Points.Count >= Indicator.Period)
            {
                if (Indicator.ShowZones)
                {
                    for (int i = 0; i < Points.Count; i++)
                    {
                        upperCollection.Add(GetDataPoint(Points[i].X, Indicator.OverBought, Points[i].XValue, Indicator.TargetSeries[1], upperCollection.Count));
                        lowerCollection.Add(GetDataPoint(Points[i].X, Indicator.OverSold, Points[i].XValue, Indicator.TargetSeries[2], lowerCollection.Count));
                    }
                }

                double prevClose = Convert.ToDouble(Points[0].Close, null), gain = 0, loss = 0, close, period = Indicator.Period;
                int index = Convert.ToInt32(Indicator.Period);
                for (int i = 1; i <= Indicator.Period; i++)
                {
                    close = Convert.ToDouble(Points[i].Close, null);
                    if (close > prevClose)
                    {
                        gain += close - prevClose;
                    }
                    else
                    {
                        loss += prevClose - close;
                    }

                    prevClose = close;
                }

                gain = gain / period;
                loss = loss / period;
                signalCollection.Add(GetDataPoint(Points[index].X, 100 - (100 / (1 + (gain / loss))), Points[index].XValue, Indicator.TargetSeries[0], signalCollection.Count));
                for (int j = index + 1; j < Points.Count; j++)
                {
                    close = Convert.ToDouble(Points[j].Close, null);
                    if (close > prevClose)
                    {
                        gain = ((gain * (period - 1)) + (close - prevClose)) / period;
                        loss = loss * (period - 1) / period;
                    }
                    else if (close < prevClose)
                    {
                        loss = ((loss * (period - 1)) + (prevClose - close)) / period;
                        gain = gain * (period - 1) / period;
                    }

                    prevClose = close;
                    signalCollection.Add(GetDataPoint(Points[j].X, 100 - (100 / (1 + (gain / loss))), Points[j].XValue, Indicator.TargetSeries[0], signalCollection.Count));
                }
            }

            SetSeriesRange(signalCollection, Indicator.TargetSeries[0]);
            if (Indicator.ShowZones)
            {
                SetSeriesRange(upperCollection, Indicator.TargetSeries[1]);
                SetSeriesRange(lowerCollection, Indicator.TargetSeries[2]);
            }
        }
    }
}