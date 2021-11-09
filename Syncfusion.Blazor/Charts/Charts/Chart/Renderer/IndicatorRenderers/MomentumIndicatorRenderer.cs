using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class MomentumIndicatorRenderer : IndicatorBase
    {
        internal override void InitSeriesCollection()
        {
            base.InitSeriesCollection();
            SetSeriesProperties(new ChartSeries(), "UpperLine", Indicator.UpperLine.Color, Indicator.UpperLine.Width);
        }

        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> upperCollection = new List<Point>();
            List<Point> signalCollection = new List<Point>();
            if (Points != null && Points.Count != 0)
            {
                int length = Convert.ToInt32(Indicator.Period);
                if (Points.Count >= length)
                {
                    for (int i = 0; i < Points.Count; i++)
                    {
                        upperCollection.Add(GetDataPoint(Points[i].X, 100.0, Points[i].XValue, Indicator.TargetSeries[1], upperCollection.Count));
                        if (!(i < length))
                        {
                            signalCollection.Add(GetDataPoint(Points[i].X, Convert.ToDouble(Points[i].Close, null) / Convert.ToDouble(Points[i - length].Close, null) * 100, Points[i].XValue, Indicator.TargetSeries[0], signalCollection.Count));
                        }
                    }
                }

                SetSeriesRange(signalCollection, Indicator.TargetSeries[0]);
                SetSeriesRange(upperCollection, Indicator.TargetSeries[1]);
            }
        }
    }
}