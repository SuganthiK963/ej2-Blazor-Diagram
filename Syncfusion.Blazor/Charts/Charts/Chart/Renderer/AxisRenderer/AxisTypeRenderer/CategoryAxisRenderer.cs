using System;
using System.Collections.Generic;
using System.Globalization;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class CategoryAxisRenderer : DateTimeBase
    {
        internal override double CalculateActualInterval(DoubleRange range)
        {
            if (double.IsNaN(Axis.Interval))
            {
                return Math.Max(1, Math.Floor(range.Delta / GetActualDesiredIntervalsCount()));
            }
            else
            {
                return Math.Ceiling(Axis.Interval);
            }
        }

        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            double ticks = (Axis.LabelPlacement == LabelPlacement.BetweenTicks && Chart.ChartAreaType != ChartAreaType.PolarAxes) ? 0.5 : 0,
                minimum = range.Start, maximum = range.End;
            if (ticks > 0)
            {
                minimum -= ticks;
                maximum += ticks;
            }
            else
            {
                maximum += !double.IsNaN(maximum) ? 0 : 0.5;
            }

            return new DoubleRange(minimum, maximum);
        }

        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = new List<VisibleLabels>();
            double tempInterval = Math.Ceiling(VisibleRange.Start), position;
            if (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0)
            {
                tempInterval = VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }

            while (tempInterval <= Math.Floor(VisibleRange.End))
            {
                if (ChartHelper.WithIn(tempInterval, VisibleRange) && Labels.Count > 0 && Labels.Count > (int)Math.Floor(VisibleRange.End))
                {
                    position = Math.Round(tempInterval);
                    TriggerLabelRender(position, !string.IsNullOrEmpty(Labels[(int)position]) ? Labels[(int)position].ToString(CultureInfo.InvariantCulture) : position.ToString(CultureInfo.InvariantCulture));
                }

                tempInterval += VisibleInterval;
            }

            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }
    }
}