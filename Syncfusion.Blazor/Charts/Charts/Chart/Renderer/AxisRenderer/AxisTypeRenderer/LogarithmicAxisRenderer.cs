using System;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class LogarithmicAxisRenderer : ChartAxisRenderer
    {
        internal override DoubleRange InitializeDoubleRange()
        {
            Min = Axis.Minimum != null ? Convert.ToDouble(Axis.Minimum, null) : (double.IsNaN(Min) || double.IsPositiveInfinity(Min)) ? 0 : Min;
            Max = Axis.Maximum != null ? Convert.ToDouble(Axis.Maximum, null) : (double.IsNaN(Max) || double.IsNegativeInfinity(Max)) ? 5 : Max;
            if (Min == Max)
            {
                Max = Min + 1;
            }

            Min = Min < 0 ? 0 : Min;
            double logStart = ChartHelper.LogBase(Min, Axis.LogBase);
            logStart = double.IsFinite(logStart) ? logStart : Min;
            double logEnd = double.IsFinite(logStart) ? Max == 1 ? 1 : ChartHelper.LogBase(Max, Axis.LogBase) : Max;
            Min = Math.Floor(logStart / 1);
            Max = Math.Ceiling(logEnd / 1);

            return new DoubleRange(Min, Max);
        }

        internal override double CalculateActualInterval(DoubleRange range)
        {
            return !double.IsNaN(Axis.Interval) ? Axis.Interval : CalculateLogNiceInterval(Max - Min);
        }

        internal override DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0)
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming)
                {
                    double interval = CalculateLogNiceInterval(actualRange.Delta);
                    VisibleInterval = Math.Floor(interval) == 0 ? 1 : Math.Floor(interval);
                }
            }

            if (Chart.ChartEvents?.OnAxisActualRangeCalculated != null)
            {
                actualRange = TriggerRangeRender(actualRange);
            }

            return actualRange;
        }

        internal override void GenerateVisibleLabels()
        {
            double tempInterval = VisibleRange.Start;
            VisibleLabels = new List<VisibleLabels>();
            if (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0)
            {
                tempInterval = VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }

            while (tempInterval <= VisibleRange.End)
            {
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                   TriggerLabelRender(tempInterval, FormatValue(Math.Pow(Axis.LogBase, tempInterval)));
                }

                tempInterval += VisibleInterval;
            }

            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }

        private double CalculateLogNiceInterval(double delta)
        {
            double niceInterval = delta;
            double minInterval = Math.Pow(10, Math.Floor(ChartHelper.LogBase(niceInterval, 10)));
            for (int j = 0, len = IntervalDivs.Length; j < len; j++)
            {
                double currentInterval = minInterval * IntervalDivs[j];
                if (ChartHelper.GetActualDesiredIntervalsCount(AxisAvailabelSize, Axis.DesiredIntervals, Orientation, Axis.MaximumLabels) < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }
        internal override double GetPointValue(double x)
        {
            return ChartHelper.LogBase((x > 0) ? x : 1, Axis.LogBase);
        }
    }
}
