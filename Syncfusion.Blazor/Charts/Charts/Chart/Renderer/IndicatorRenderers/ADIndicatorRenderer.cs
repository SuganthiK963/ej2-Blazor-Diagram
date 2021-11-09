using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class AccumulationDistributionIndicatorRenderer : IndicatorBase
    {
        internal override void InitDataSource()
        {
            base.InitDataSource();
            List<Point> ad_Points = new List<Point>();
            if (Points.Count > 0 && Points.Count > Indicator.Period)
            {
                double sum = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    double high = Points[i].High != null ? Convert.ToDouble(Points[i].High, null) : double.NaN;
                    double low = Points[i].Low != null ? Convert.ToDouble(Points[i].Low, null) : double.NaN;
                    double close = Points[i].Close != null ? Convert.ToDouble(Points[i].Close, null) : double.NaN;
                    double diff = ((close - low) - (high - close)) / (high - low);
                    sum = sum + (diff * (Points[i].Volume != null ? Convert.ToDouble(Points[i].Volume, null) : double.NaN));
                    ad_Points.Add(GetDataPoint(Points[i].X, sum, Points[i].XValue, Indicator.TargetSeries[0], ad_Points.Count));
                }
            }

            SetSeriesRange(ad_Points, Indicator.TargetSeries[0]);
        }
    }
}