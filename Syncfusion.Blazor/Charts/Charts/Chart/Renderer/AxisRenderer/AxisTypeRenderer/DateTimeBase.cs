using System;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class DateTimeBase : ChartAxisRenderer
    {
        protected double CalculateDateTimeNiceInterval(double start, double end, bool isChart = true)
        {
            double totalDays = Math.Abs((GetTime(new DateTime(1970, 1, 1).AddMilliseconds(start)) - GetTime(new DateTime(1970, 1, 1).AddMilliseconds(end))) / 86400000);
            double interval = 0;
            ActualIntervalType = Axis.IntervalType;
            switch ((RangeIntervalType)Enum.Parse(typeof(RangeIntervalType), Axis.IntervalType.ToString()))
            {
                case RangeIntervalType.Years:
                    interval = CalculateNumericNiceInterval(totalDays / 365);
                    break;
                case RangeIntervalType.Quarter:
                    interval = CalculateNumericNiceInterval((totalDays / 365) * 4);
                    break;
                case RangeIntervalType.Months:
                    interval = CalculateNumericNiceInterval(totalDays / 30);
                    break;
                case RangeIntervalType.Weeks:
                    interval = CalculateNumericNiceInterval(totalDays / 7);
                    break;
                case RangeIntervalType.Days:
                    interval = CalculateNumericNiceInterval(totalDays);
                    break;
                case RangeIntervalType.Hours:
                    interval = CalculateNumericNiceInterval(totalDays * 24);
                    break;
                case RangeIntervalType.Minutes:
                    interval = CalculateNumericNiceInterval(totalDays * 24 * 60);
                    break;
                case RangeIntervalType.Seconds:
                    interval = CalculateNumericNiceInterval(totalDays * 24 * 60 * 60);
                    break;
                case RangeIntervalType.Auto:
                    interval = CalculateNumericNiceInterval(totalDays / 365);
                    if (interval >= 1)
                    {
                        ActualIntervalType = IntervalType.Years;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval((totalDays / 365) * 4);
                    if (interval >= 1 && !isChart)
                    {
                        ActualIntervalType = IntervalType.Months;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval(totalDays / 30);
                    if (interval >= 1)
                    {
                        ActualIntervalType = IntervalType.Months;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval(totalDays / 7);
                    if (interval >= 1 && !isChart)
                    {
                        ActualIntervalType = IntervalType.Days;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval(totalDays);
                    if (interval >= 1)
                    {
                        ActualIntervalType = IntervalType.Days;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval(totalDays * 24);
                    if (interval >= 1)
                    {
                        ActualIntervalType = IntervalType.Hours;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval(totalDays * 24 * 60);
                    if (interval >= 1)
                    {
                        ActualIntervalType = IntervalType.Minutes;
                        return interval;
                    }

                    interval = CalculateNumericNiceInterval(totalDays * 24 * 60 * 60);
                    ActualIntervalType = IntervalType.Seconds;
                    return interval;
            }

            return interval;
        }

#pragma warning disable CA1822
        internal double GetTime(DateTime current)
        {
            return (current - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        private string GetMonthFormat(double currentValue, double previousValue)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(currentValue).Year == new DateTime(1970, 1, 1).AddMilliseconds(previousValue).Year ? IsIntervalInDecimal ? "MMM" : "MMM d" : "yyyy MMM";
        }

        protected string GetSkeleton()
#pragma warning restore CA1822
        {
            RangeIntervalType intervalType = (RangeIntervalType)Enum.Parse(typeof(RangeIntervalType), ActualIntervalType.ToString());
            if (!string.IsNullOrEmpty(Axis.Format))
            {
                return Axis.Format;
            }

            if (intervalType == RangeIntervalType.Years || intervalType == RangeIntervalType.Quarter)
            {
                return "y";
            }
            else if (intervalType == RangeIntervalType.Months || intervalType == RangeIntervalType.Weeks)
            {
                return "m";
            }
            else if (intervalType == RangeIntervalType.Days)
            {
                return "d";
            }
            else if (intervalType == RangeIntervalType.Hours)
            {
                return "t";
            }
            else
            {
                return "T";
            }
        }

        protected string FindCustomFormats(double currentValue, double previousValue)
        {
            string labelFormat = !string.IsNullOrEmpty(Axis.LabelFormat) ? Axis.LabelFormat : string.Empty;
            if (string.IsNullOrEmpty(Axis.Format) && ActualIntervalType == IntervalType.Months && string.IsNullOrEmpty(labelFormat))
            {
                labelFormat = Axis.ValueType == ValueType.DateTime ? GetMonthFormat(currentValue, previousValue) : "yMMM";
            }

            if (string.IsNullOrEmpty(labelFormat))
            {
                labelFormat = ActualIntervalType == IntervalType.Years ? (IsIntervalInDecimal ? "yyyy" : "MMM y") :
                     (ActualIntervalType == IntervalType.Days && !IsIntervalInDecimal) ? "ddd HH tt" : string.Empty;
            }

            return string.IsNullOrEmpty(labelFormat) ? GetSkeleton() : labelFormat;
        }
    }
}