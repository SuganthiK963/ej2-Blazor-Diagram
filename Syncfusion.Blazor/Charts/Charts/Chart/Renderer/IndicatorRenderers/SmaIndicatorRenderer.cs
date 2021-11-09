using System.Collections.Generic;
using System;
using System.Linq;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class SmaIndicatorRenderer : IndicatorBase
    {
        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> smaPoints = new List<Point>();
            if (Points != null && Points.Count != 0 && Points.Count >= Indicator.Period)
            {
                string field = Indicator.Field.ToString();
                double period = Indicator.Period;
                Type type = Points.ToArray().First().GetType();
                double sum = 0;
                PropertyAccessor indicatorField = new PropertyAccessor(type.GetProperty(field));
                for (int i = 0; i < period; i++)
                {
                    sum += Convert.ToDouble(indicatorField.GetValue(Points[i]), null);
                }

                double average = sum / period;
                int pos1 = Convert.ToInt32(period - 1);
                smaPoints.Add(GetDataPoint(type.GetProperty("X").GetValue(Points[pos1]), average, Points[pos1].XValue, Indicator.TargetSeries[0], smaPoints.Count));
                int posInc = Convert.ToInt32(period), pos2 = Convert.ToInt32(period);
                while (posInc < Points.Count)
                {
                    sum -= Convert.ToDouble(indicatorField.GetValue(Points[posInc - pos2]), null);
                    sum += Convert.ToDouble(indicatorField.GetValue(Points[posInc]), null);
                    average = sum / period;
                    smaPoints.Add(GetDataPoint(indicatorField.GetValue(Points[posInc]), average, Points[posInc].XValue, Indicator.TargetSeries[0], smaPoints.Count));
                    posInc++;
                }

                SetSeriesRange(smaPoints, Indicator.TargetSeries[0]);
            }
        }
    }
}