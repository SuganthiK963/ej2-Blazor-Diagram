using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class AtrIndicatorRenderer : IndicatorBase
    {
        internal override void InitDataSource()
        {
            base.InitDataSource();
            if (Points.Count > 0 && Points.Count > Indicator.Period)
            {
                double average, highClose = 0, lowClose = 0, period = Indicator.Period, sum = 0;
                List<Point> points = new List<Point>();
                List<AtrPoints> atrPoints = new List<AtrPoints>();
                for (int i = 0; i < Points.Count; i++)
                {
                    if (i > 0)
                    {
                        highClose = Math.Abs(Convert.ToDouble(Points[i].High, null) - Convert.ToDouble(Points[i - 1].Close, null));
                        lowClose = Math.Abs(Convert.ToDouble(Points[i].Low, null) - Convert.ToDouble(Points[i - 1].Close, null));
                    }

                    double trueRange = Math.Max(Convert.ToDouble(Points[i].High, null) - Convert.ToDouble(Points[i].Low, null), Math.Max(highClose, lowClose));
                    sum = sum + trueRange;
                    if (i >= period)
                    {
                        average = ((atrPoints[i - 1].Y * (period - 1)) + trueRange) / period;
                        points.Add(GetDataPoint(Points[i].X, average, Points[i].XValue, Indicator.TargetSeries[0], points.Count));
                    }
                    else
                    {
                        average = sum / period;
                        if (i == period - 1)
                        {
                            points.Add(GetDataPoint(Points[i].X, average, Points[i].XValue, Indicator.TargetSeries[0], points.Count));
                        }
                    }

                    atrPoints.Add(new AtrPoints { X = Points[i].X, Y = average });
                }

                SetSeriesRange(points, Indicator.TargetSeries[0]);
            }
        }
    }
}