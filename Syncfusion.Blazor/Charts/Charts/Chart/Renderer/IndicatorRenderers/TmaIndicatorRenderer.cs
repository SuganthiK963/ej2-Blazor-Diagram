using System;
using System.Collections.Generic;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class TmaIndicatorRenderer : IndicatorBase
    {
        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> tmaPoints = new List<Point>();
            string field = Indicator.Field.ToString();
            if (Points != null && Points.Count != 0 && Points.Count >= Indicator.Period)
            {
                ChartSeries signalSeries = Indicator.TargetSeries[0];
                double sum = 0;
                List<double> smaValues = new List<double>();
                int length = Points.Count;
                double period = Indicator.Period;
                Type type = Points.ToArray().First().GetType();
                while (length >= period)
                {
                    sum = 0;
                    int index = Points.Count - length;
                    for (int j = index; j < index + period; j++)
                    {
                        sum = sum + Convert.ToDouble(type.GetProperty(field).GetValue(Points[j]), null);
                    }

                    sum = sum / period;
                    smaValues.Add(sum);
                    length--;
                }

                for (int k = 0; k < period - 1; k++)
                {
                    sum = 0;
                    for (int j = 0; j < k + 1; j++)
                    {
                        sum = sum + Convert.ToDouble(type.GetProperty(field).GetValue(Points[j]), null);
                    }

                    sum = sum / (k + 1);
                    smaValues.Insert(k, sum);
                }

                int posInc = Convert.ToInt32(period), pos = Convert.ToInt32(period);
                while (posInc <= smaValues.Count)
                {
                    sum = 0;
                    for (int j = posInc - pos; j < posInc; j++)
                    {
                        sum = sum + smaValues[j];
                    }

                    sum = sum / period;
                    tmaPoints.Add(GetDataPoint(type.GetProperty("X").GetValue(Points[posInc - 1]), sum, Points[posInc - 1].XValue, signalSeries, tmaPoints.Count));
                    posInc++;
                }
            }

            SetSeriesRange(tmaPoints, Indicator.TargetSeries[0]);
        }
    }
}