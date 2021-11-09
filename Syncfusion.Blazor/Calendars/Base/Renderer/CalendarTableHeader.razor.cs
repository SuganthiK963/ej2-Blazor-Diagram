using System;
using Microsoft.AspNetCore.Components;
using System.Linq;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Calendars.Internal
{
    /// <summary>
    /// The Calendar day is show in header.
    /// </summary>
    /// <typeparam name="TCalendarHeader">Specifies the type of CalendarTableHeader.</typeparam>
    public partial class CalendarTableHeader<TCalendarHeader> : CalendarBase<TCalendarHeader>
    {
        internal const string WEEK_NUMBER = "e-week-number";

        internal const string WEEK_HEADER = "e-week-header";

        internal const int DAYCOUNT = 7;

        private int DaysCount { get; set; }

        private string[] ShortNames { get; set; }

        [CascadingParameter]
        internal CalendarBase<TCalendarHeader> Parent { get; set; }

        /// <summary>
        /// Specifies the calendar view .
        /// </summary>
        /// <exclude/>
        [Parameter]
        public CalendarView CalendarRenderView { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        protected override void OnInitialized()
        {
            CreateContentHeader();
        }

        /// <summary>
        /// Triggers while dynamically changing the component properties.
        /// </summary>
        protected override void OnParametersSet()
        {
            CreateContentHeader();
        }

        /// <summary>
        /// Method used to create the content of the header.
        /// </summary>
        protected void CreateContentHeader()
        {
            DaysCount = DAYCOUNT;
            UpdateHeaderName();
        }

        private void UpdateHeaderName()
        {
            var currentCulture = Intl.GetCulture();
            var isFirstDayOfWeek = (int)Parent.FirstDayOfWeek != 0;
            switch (Parent.DayHeaderFormat)
            {
                case DayHeaderFormats.Short:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = currentCulture.DateTimeFormat.ShortestDayNames;
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = currentCulture.DateTimeFormat.ShortestDayNames;
                    }

                    break;
                case DayHeaderFormats.Abbreviated:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = currentCulture.DateTimeFormat.AbbreviatedDayNames;
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = currentCulture.DateTimeFormat.AbbreviatedDayNames;
                    }

                    break;
                case DayHeaderFormats.Narrow:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = Intl.GetNarrowDayNames().ToArray();
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = Intl.GetNarrowDayNames().ToArray();
                    }

                    break;
                case DayHeaderFormats.Wide:
                    if (isFirstDayOfWeek)
                    {
                        string[] totalDays = currentCulture.DateTimeFormat.DayNames;
                        GetFirstDayOfHeader(totalDays);
                    }
                    else
                    {
                        ShortNames = currentCulture.DateTimeFormat.DayNames;
                    }

                    break;
            }
        }

        private void GetFirstDayOfHeader(string[] totalDays)
        {
            var firstDay = (int)Parent.FirstDayOfWeek;
            ArraySegment<string> val = new ArraySegment<string>(totalDays);
            string[] sliceItems = val.Slice(firstDay).ToArray<string>();
            string[] remainItem = val.Slice(0, firstDay).ToArray<string>();
            ShortNames = sliceItems.Concat(remainItem).ToArray<string>();
        }
    }
}