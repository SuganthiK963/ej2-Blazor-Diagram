using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class NumericAxisRenderer : ChartAxisRenderer
    {
        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            double min = range.Start;
            double max = range.End;
            if (!Axis.StartFromZero && IsColumn > 0)
            {
                max = max + interval;
                if ((min - interval) < 0 && min > 0)
                {
                    min = 0;
                }
                else
                {
                    min = min - interval;
                }
            }

            if (!ChartHelper.SetRange(Axis))
            {
                ChartRangePadding padding = GetRangePadding();
                if (padding == ChartRangePadding.Additional || padding == ChartRangePadding.Round)
                {
                    return FindAdditional(min, max, interval);
                }
                else if (padding == ChartRangePadding.Normal)
                {
                    return FindNormal(min, max, interval);
                }
            }

            return range;
        }

        private DoubleRange FindAdditional(double start, double end, double interval)
        {
            double minimum = start;
            double maximum = end;
            if (Axis.Minimum == null)
            {
                minimum = Math.Floor(start / interval) * interval;
                if (Axis.RangePadding == ChartRangePadding.Additional)
                {
                    minimum -= interval;
                }
            }

            if (Axis.Maximum == null)
            {
                maximum = Math.Ceiling(end / interval) * interval;
                if (Axis.RangePadding == ChartRangePadding.Additional)
                {
                    maximum += interval;
                }
            }

            return new DoubleRange(minimum, maximum);
        }

        private DoubleRange FindNormal(double start, double end, double interval)
        {
            double minimum = start;
            double maximum = end;
            double max = end;
            double startValue = start;
            if (Axis.Minimum == null)
            {
                if (start < 0)
                {
                    startValue = 0;
                    minimum = start + (start * 0.05);
                    if ((0.365 * interval) >= (interval + (minimum % interval)))
                    {
                        minimum -= interval;
                    }

                    if (minimum % interval < 0)
                    {
                        minimum = Convert.ToDouble((decimal)((minimum - interval) - (minimum % interval)));
                    }
                }
                else
                {
                    minimum = start < ((5.0 / 6.0) * end) ? 0 : (start - ((end - start) * 0.5));
                    if (minimum % interval > 0)
                    {
                        minimum -= minimum % interval;
                    }
                }
            }

            max = end > 0 ? end + ((end - startValue) * 0.05) : end - ((end - startValue) * 0.05);
            if ((0.365 * interval) >= (interval - (max % interval)))
            {
                max += interval;
            }

            if (max % interval > 0)
            {
                max = (max + interval) - (max % interval);
            }

            if (Axis.Maximum == null)
            {
                maximum = max;
            }

            if (Axis.Minimum == null && minimum == 0)
            {
                if (ChartHelper.IsNaNOrZero(Axis.Interval))
                {
                    interval = CalculateNumericNiceInterval(max - minimum);
                }

                if (Axis.Maximum == null)
                {
                    maximum = Math.Ceiling(max / interval) * interval;
                }
            }

            VisibleInterval = ActualInterval = interval;
            return new DoubleRange(minimum, maximum);
        }

        internal override double CalculateActualInterval(DoubleRange range)
        {
            return !ChartHelper.IsNaNOrZero(Axis.Interval) ? Axis.Interval : CalculateNumericNiceInterval(range.Delta);
        }

        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = new List<VisibleLabels>();
            double tempInterval = VisibleRange.Start;
            bool isPolarRadar = Chart != null && Chart.ChartAreaType == ChartAreaType.PolarAxes;
            if (!isPolarRadar && (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0 || PaddingInterval > 0))
            {
                tempInterval = VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }

            int intervalDigits = 0, formatDigits = 0;
            if (!string.IsNullOrEmpty(Axis.LabelFormat) && Axis.LabelFormat.ToLower(culture).Contains('n', StringComparison.InvariantCulture))
            {
                _ = int.TryParse(Axis.LabelFormat.Substring(1, Axis.LabelFormat.Length - 1), out formatDigits);
            }

            if (VisibleInterval > 0 && VisibleInterval.ToString(culture).Contains('.', StringComparison.InvariantCulture))
            {
                intervalDigits = VisibleInterval.ToString(culture).Split('.')[1].Length;
            }

            while (tempInterval <= VisibleRange.End)
            {
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }

                tempInterval += VisibleInterval;
            }

            string tempString = tempInterval.ToString(culture);
            if (!double.IsNaN(tempInterval) && tempInterval > 0 && tempString.Contains('.', StringComparison.InvariantCulture) && tempString.Split('.')[1].Length > 10)
            {
                tempInterval = tempString.Split('.')[1].Length > Math.Max(formatDigits, intervalDigits) ? Math.Round(tempInterval, Math.Max(formatDigits, intervalDigits)) : tempInterval;
                if (tempInterval <= VisibleRange.End)
                {
                    TriggerLabelRender(tempInterval, FormatValue(tempInterval));
                }
            }

            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }
    }

    public class PrimaryXAxisRenderer : NumericAxisRenderer
    {
        internal override bool IsDefaultRenderer()
        {
            return true;
        }
    }

    public class PrimaryYAxisRenderer : NumericAxisRenderer
    {
        internal override bool IsDefaultRenderer()
        {
            return true;
        }
    }

    public class ParetoAxisRenderer : NumericAxisRenderer
    {
        internal override bool IsDefaultRenderer()
        {
            return true;
        }
    }
}
