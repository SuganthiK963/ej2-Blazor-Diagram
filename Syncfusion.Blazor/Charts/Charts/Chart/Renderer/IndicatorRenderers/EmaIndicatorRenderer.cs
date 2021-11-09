using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class EmaIndicatorRenderer : IndicatorBase
    {
        internal override void InitDataSource()
        {
            base.InitDataSource();
            string field = Indicator.Field.ToString();
            List<Point> emaPoints = new List<Point>();
            double period = Indicator.Period;
            if (Points != null && Points.Count != 0 && Points.Count >= period)
            {
                double sum = 0;
                double k = 2 / (period + 1);
                Type type = Points.ToArray().First().GetType();
                for (int i = 0; i < period; i++)
                {
                    sum += Convert.ToDouble(type.GetProperty(field).GetValue(Points[i]), null);
                }

                double average = sum / period;
                int pos1 = Convert.ToInt32(period - 1);
                emaPoints.Add(GetDataPoint(type.GetProperty("X").GetValue(Points[pos1]), average, Points[pos1].XValue, Indicator.TargetSeries[0], emaPoints.Count));
                int posInc = Convert.ToInt32(period);
                while (posInc < Points.Count)
                {
                    double prevAverage = Convert.ToDouble(emaPoints.ToArray().First().GetType().GetProperty(Indicator.TargetSeries[0].YName).GetValue(emaPoints[posInc - Convert.ToInt32(period)]), null);
                    emaPoints.Add(GetDataPoint(type.GetProperty(field).GetValue(Points[posInc]), ((Convert.ToDouble(type.GetProperty(field).GetValue(Points[posInc]), null) - prevAverage) * k) + prevAverage, Points[posInc].XValue, Indicator.TargetSeries[0], emaPoints.Count));
                    posInc++;
                }
            }

            SetSeriesRange(emaPoints, Indicator.TargetSeries[0]);
        }
    }
}