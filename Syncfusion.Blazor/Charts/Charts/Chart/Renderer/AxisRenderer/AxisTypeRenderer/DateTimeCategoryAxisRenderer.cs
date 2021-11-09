using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class DateTimeCategoryAxisRenderer : CategoryAxisRenderer
    {
        internal override void GenerateVisibleLabels()
        {
            VisibleLabels = new List<VisibleLabels>();
            if (Labels.Count == 0)
            {
                MaxLabelSize = new Size(0, 0);
                return;
            }

            if (Axis.IntervalType == IntervalType.Auto)
            {
                CalculateDateTimeNiceInterval(Convert.ToDouble(Labels[0], null), Convert.ToDouble(Labels[Labels.Count - 1], null));
            }
            else
            {
                ActualIntervalType = Axis.IntervalType;
            }

            for (int i = 0; i < Labels.Count; i++)
            {
                if ((!SameInterval(Convert.ToDouble(Labels[i], null), Convert.ToDouble(Labels[i == 0 ? 0 : i - 1], null), ActualIntervalType, i) || Axis.IsIndexed) && ChartHelper.WithIn(i - (Axis.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0), VisibleRange))
                {
                    TriggerLabelRender(i, Axis.IsIndexed ? GetIndexedAxisLabel(Labels[i]) : Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(Convert.ToDouble(Labels[i], null)), CustomFormat()));
                }
            }

            GetMaxLabelWidth();
        }

        private string CustomFormat()
        {
            return string.IsNullOrEmpty(Axis.LabelFormat) ? ActualIntervalType == IntervalType.Years ? "yyyy" : GetSkeleton() : Axis.LabelFormat;
        }

        internal string GetIndexedAxisLabel(string axisValue)
        {
            string[] texts = axisValue.Split(',');
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] = Intl.GetDateFormat(new DateTime(1970, 1, 1).AddMilliseconds(Convert.ToDouble(texts[i], null)), CustomFormat());
            }

            return string.Join(", ", texts);
        }

        private static bool SameInterval(double currentDate, double previousDate, IntervalType type, double index)
        {
            if (index != 0)
            {
                switch (type)
                {
                    case IntervalType.Years:
                        return new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Year == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Year;
                    case IntervalType.Months:
                        return new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Year == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Year && new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Month == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Month;
                    case IntervalType.Days:
                        return Math.Abs(currentDate - previousDate) < 24 * 60 * 60 * 1000 && new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Day == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Day;
                    case IntervalType.Hours:
                        return Math.Abs(currentDate - previousDate) < 60 * 60 * 1000 && new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Day == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Day;
                    case IntervalType.Minutes:
                        return Math.Abs(currentDate - previousDate) < 60 * 1000 && new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Minute == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Minute;
                    case IntervalType.Seconds:
                        return Math.Abs(currentDate - previousDate) < 1000 && new DateTime(1970, 1, 1).AddMilliseconds(currentDate).Day == new DateTime(1970, 1, 1).AddMilliseconds(previousDate).Day;
                }
            }

            return false;
        }
    }
}