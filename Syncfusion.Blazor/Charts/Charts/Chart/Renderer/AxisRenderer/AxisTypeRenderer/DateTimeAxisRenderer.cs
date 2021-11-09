using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Internal;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.RangeNavigator")]
namespace Syncfusion.Blazor.Charts.Internal
{
    public class DateTimeAxisRenderer : DateTimeBase
    {
        private static DateTime GetDecimalInterval(DateTime result, double interval, RangeIntervalType intervalType)
        {
            double roundValue = Math.Floor(interval);
            double decimalValue = interval - roundValue;
            switch (intervalType)
            {
                case RangeIntervalType.Years:
                    result = result.AddYears((int)roundValue).AddMonths((int)Math.Round(12 * decimalValue));
                    return result;
                case RangeIntervalType.Quarter:
                    return result.AddMonths((int)(3 * interval));
                case RangeIntervalType.Months:
                    result = result.AddMonths((int)roundValue).AddDays(Math.Round(30 * decimalValue));
                    return result;
                case RangeIntervalType.Weeks:
                    return result.AddDays(interval * 7);
                case RangeIntervalType.Days:
                    result = result.AddDays(roundValue).AddHours(Math.Round(24 * decimalValue));
                    return result;
                case RangeIntervalType.Hours:
                    result = result.AddHours(roundValue).AddMinutes(Math.Round(60 * decimalValue));
                    return result;
                case RangeIntervalType.Minutes:
                    result = result.AddMinutes(roundValue).AddSeconds(Math.Round(60 * decimalValue));
                    return result;
                case RangeIntervalType.Seconds:
                    result = result.AddSeconds(roundValue).AddMilliseconds(Math.Round(1000 * decimalValue));
                    return result;
            }

            return result;
        }

        internal override DoubleRange InitializeDoubleRange()
        {
            if (Axis.Minimum != null)
            {
                Min = GetTime((DateTime)Axis.Minimum);
            }
            else if (double.IsNaN(Min) || Min == double.PositiveInfinity)
            {
                Min = GetTime(new DateTime(1970, 1, 1));
            }

            if (Axis.Maximum != null)
            {
                Max = GetTime((DateTime)Axis.Maximum);
            }
            else if (double.IsNaN(Max) || Max == double.NegativeInfinity)
            {
                Max = GetTime(new DateTime(1970, 5, 1));
            }

            if (Min == Max)
            {
                Max = Max + 2592000000;
                Min = Min - 2592000000;
            }

            return new DoubleRange(Min, Max);
        }

        internal override double CalculateActualInterval(DoubleRange range)
        {
            double dateTimeInterval = CalculateDateTimeNiceInterval(range.Start, range.End);
            return ChartHelper.IsNaNOrZero(Axis.Interval) ? dateTimeInterval : Axis.Interval;
        }

        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            if (!ChartHelper.SetRange(Axis))
            {
                ChartRangePadding rangePadding = GetRangePadding();
                DateTime minimum = new DateTime(1970, 1, 1).AddMilliseconds(Min);
                DateTime maximum = new DateTime(1970, 1, 1).AddMilliseconds(Max);
                if (rangePadding == ChartRangePadding.None)
                {
                    Min = GetTime(minimum);
                    Max = GetTime(maximum);
                }
                else if (rangePadding == ChartRangePadding.Additional || rangePadding == ChartRangePadding.Round)
                {
                    switch (ActualIntervalType)
                    {
                        case IntervalType.Years:
                            GetYear(minimum, maximum, rangePadding, interval);
                            break;
                        case IntervalType.Months:
                            GetMonth(minimum, maximum, rangePadding, interval);
                            break;
                        case IntervalType.Days:
                            GetDay(minimum, maximum, rangePadding, interval);
                            break;
                        case IntervalType.Hours:
                            GetHour(minimum, maximum, rangePadding, interval);
                            break;
                        case IntervalType.Minutes:
                            int minute = Convert.ToInt32((minimum.Minute / interval) * interval);
                            int endMinute = Convert.ToInt32(maximum.Minute + (minimum.Minute - minute));
                            if (rangePadding == ChartRangePadding.Round)
                            {
                                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, minimum.Hour, minute, 0));
                                Max = GetTime(new DateTime(maximum.Year, maximum.Month, minimum.Day, maximum.Hour, endMinute, 59));
                            }
                            else
                            {
                                Min = GetTime(new DateTime(minimum.Year, maximum.Month, minimum.Day, minimum.Hour, minute + (int)-interval, 0));
                                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, maximum.Hour, endMinute + (int)interval, 0));
                            }

                            break;
                        case IntervalType.Seconds:
                            int second = Convert.ToInt32((minimum.Second / interval) * interval);
                            int endSecond = Convert.ToInt32(maximum.Second + (minimum.Second - second));
                            if (rangePadding == ChartRangePadding.Round)
                            {
                                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, minimum.Hour, minimum.Minute, second, 0));
                                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, maximum.Hour, maximum.Minute, endSecond, 0));
                            }
                            else
                            {
                                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, minimum.Hour, minimum.Minute, second + (int)-interval, 0));
                                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, maximum.Hour, maximum.Minute, endSecond + (int)interval, 0));
                            }

                            break;
                    }
                }
            }

            return new DoubleRange(Min, Max);
        }

        internal override DoubleRange CalculateVisibleRange(DoubleRange actualRange)
        {
            if (Axis.ZoomFactor < 1 || Axis.ZoomPosition > 0)
            {
                actualRange = CalculateVisibleRangeOnZooming();
                if (Axis.EnableAutoIntervalOnZooming)
                {
                    CalculateAutoIntervalOnBothAxisRange(actualRange);
                    VisibleInterval = CalculateDateTimeNiceInterval(actualRange.Start, actualRange.End);
                }
            }

            RangeIntervalType intervalType = (RangeIntervalType)Enum.Parse(typeof(RangeIntervalType), ActualIntervalType.ToString());
            DateTimeInterval = GetTime(IncreaseDateTimeInterval(VisibleRange.Start, VisibleInterval, intervalType)) - VisibleRange.Start;
            if (Chart?.ChartEvents?.OnAxisActualRangeCalculated != null)
            {
                actualRange = TriggerRangeRender(actualRange);
            }

            return actualRange;
        }

        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = new List<VisibleLabels>();
            if (!SeriesRenderer.Any(series => series.Series.Visible) && (Axis.Minimum == null || Axis.Maximum == null))
            {
                return;
            }

            double tempInterval = VisibleRange.Start;
            RangeIntervalType intervalType = (RangeIntervalType)Enum.Parse(typeof(RangeIntervalType), ActualIntervalType.ToString());
            if (!ChartHelper.SetRange(Axis))
            {
                tempInterval = GetTime(AlignRangeStart(tempInterval, VisibleInterval));
            }

            while (tempInterval <= VisibleRange.End && VisibleInterval > 0)
            {
                VisibleLabels[] axisLabels = VisibleLabels.ToArray();
                double previousValue = !double.IsNaN(axisLabels.Length) && (axisLabels.Length >= 1) ? VisibleLabels[axisLabels.Length - 1].Value : tempInterval;

                DateFormat = FindCustomFormats(tempInterval, previousValue);
                if (ChartHelper.WithIn(tempInterval, VisibleRange))
                {
                    TriggerLabelRender(tempInterval, Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(tempInterval), FindCustomFormats(tempInterval, previousValue)));
                }

                tempInterval = GetTime(IncreaseDateTimeInterval(tempInterval, VisibleInterval, intervalType));
            }

            if (ActualIntervalType == IntervalType.Months || ActualIntervalType == IntervalType.Days)
            {
                DateFormat = !string.IsNullOrEmpty(Axis.LabelFormat) ? Axis.LabelFormat : (ActualIntervalType == IntervalType.Months) ? "yyyy MMM" : "d";
            }

            GetMaxLabelWidth();
            base.GenerateVisibleLabels();
        }

        private void GetYear(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            if (rangePadding == ChartRangePadding.Additional)
            {
                Min = GetTime(new DateTime(minimum.Year - (int)interval, 1, 1, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year + (int)interval, 1, 1, 0, 0, 0));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, 0, 0, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, 11, 30, 23, 59, 59));
            }
        }

        private void GetMonth(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, 1, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, new DateTime(maximum.Year, maximum.Month, 1).Day, 23, 59, 59));
            }
            else
            {
                int month = minimum.Month + (int)-interval, year = month > 0 ? minimum.Year : minimum.Year - 1;
                month = month <= 0 ? 12 + month : month;
                Min = GetTime(new DateTime(year, month, 1, 0, 0, 0));
                int maxmonth = maximum.Month + (int)interval, maxyear = maxmonth < 12 ? maximum.Year : maximum.Year + 1;
                maxmonth = maxmonth > 12 ? maxmonth - 12 : maxmonth;
                Max = GetTime(new DateTime(maxyear, maxmonth, maxmonth == 2 ? 28 : 30, 0, 0, 0));
            }
        }

        private void GetDay(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, 0, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, minimum.Day, 23, 59, 59));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, 0, 0, 0).AddDays((int)-interval));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, 0, 0, 0).AddDays((int)interval));
            }
        }

        private void GetHour(DateTime minimum, DateTime maximum, ChartRangePadding rangePadding, double interval)
        {
            int hour = (minimum.Hour / (int)interval) * (int)interval;
            int endHour = maximum.Hour + (minimum.Hour - hour);
            if (rangePadding == ChartRangePadding.Round)
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, hour, 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, endHour, 59, 59));
            }
            else
            {
                Min = GetTime(new DateTime(minimum.Year, minimum.Month, minimum.Day, hour + (int)(-interval), 0, 0));
                Max = GetTime(new DateTime(maximum.Year, maximum.Month, maximum.Day, endHour + (int)interval, 0, 0));
            }
        }

        internal DateTime IncreaseDateTimeInterval(double minValue, double interval, RangeIntervalType intervalType)
        {
            DateTime result = new DateTime(1970, 1, 1).AddMilliseconds(minValue);
            if (!ChartHelper.IsNaNOrZero(Axis.Interval))
            {
                IsIntervalInDecimal = (interval % 1) == 0;
                VisibleInterval = interval;
            }
            else
            {
                interval = Math.Ceiling(interval);
                VisibleInterval = interval;
            }

            if (IsIntervalInDecimal)
            {
                switch (intervalType)
                {
                    case RangeIntervalType.Years:
                        return result.AddYears((int)interval);
                    case RangeIntervalType.Quarter:
                        return result.AddMonths((int)(3 * interval));
                    case RangeIntervalType.Months:
                        return result.AddMonths((int)interval);
                    case RangeIntervalType.Weeks:
                        return result.AddDays(interval * 7);
                    case RangeIntervalType.Days:
                        return result.AddDays(interval);
                    case RangeIntervalType.Hours:
                        return result.AddHours(interval);
                    case RangeIntervalType.Minutes:
                        return result.AddMinutes(interval);
                    case RangeIntervalType.Seconds:
                        return result.AddSeconds(interval);
                }
            }
            else
            {
                result = GetDecimalInterval(result, interval, intervalType);
            }

            return result;
        }

        private DateTime AlignRangeStart(double startDate, double intervalSize)
        {
            DateTime dateTime = new DateTime(1970, 1, 1).AddMilliseconds(startDate);
            switch (ActualIntervalType)
            {
                case IntervalType.Years:
                    return new DateTime((int)Math.Floor(Math.Floor(dateTime.Year / intervalSize) * intervalSize), dateTime.Month, dateTime.Day, 0, 0, 0);
                case IntervalType.Months:
                    int month = (int)Math.Floor(Math.Floor(dateTime.Month / intervalSize) * intervalSize);
                    month = month <= 1 ? 1 : (month == 2 && dateTime.Day > 28) ? (DateTime.IsLeapYear(dateTime.Year) && dateTime.Day == 29) ? month : 3 : month;
                    return new DateTime(dateTime.Year, month, 1, 0, 0, 0).AddDays(dateTime.Day - 1);
                case IntervalType.Days:
                    int day = (int)Math.Floor(Math.Floor(dateTime.Day / intervalSize) * intervalSize);
                    return (day <= 0) ? new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0).AddDays(-1) : new DateTime(dateTime.Year, dateTime.Month, day, 0, 0, 0);
                case IntervalType.Hours:
                    double hours = Math.Floor(Math.Floor(dateTime.Hour / intervalSize) * intervalSize);
                    hours = (hours <= 0 || double.IsNaN(hours)) ? 0 : hours;
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, (int)hours, 0, 0);
                case IntervalType.Minutes:
                    double minutes = Math.Floor(Math.Floor(dateTime.Minute / intervalSize) * intervalSize);
                    minutes = (minutes <= 0 || double.IsNaN(minutes)) ? 0 : minutes;
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, (int)minutes, 0, 0);
                case IntervalType.Seconds:
                    double seconds = Math.Floor(Math.Floor(dateTime.Second / intervalSize) * intervalSize);
                    seconds = (seconds <= 0 || double.IsNaN(seconds)) ? 0 : seconds;
                    return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, (int)seconds, 0);
            }

            return dateTime;
        }
    }
}