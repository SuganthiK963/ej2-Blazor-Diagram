using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Globalization;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Calendars.Internal
{
    /// <summary>
    /// The Calendar base is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    /// <typeparam name="TCalendarCell">Specifies the type of CalendarDayCell.</typeparam>
    public partial class CalendarDayCell<TCalendarCell> : CalendarBase<TCalendarCell>
    {
        internal const string OTHERMONTH = "e-other-month";
        internal const string OTHERDECADE = "e-other-year";
        internal const string DISABLED = "e-disabled";
        internal const string OVERLAY = "e-overlay";
        internal const string WEEKEND = "e-weekend";
        internal const string WEEKNUMBER = "e-week-number";
        internal const string LINK = "e-day";
        internal const string CELL = "e-cell";
        internal const string TODAY = "e-today";
        internal const string SELECTED = "e-selected";
        internal const string FOCUSEDDATE = "e-focused-date";
        internal const string VALUE = "Value";
        internal const string RENDERDAYCELL = "OnRenderDayCell";
        internal const string FORMATFULLDATE = "dddd, MMMM dd, yyyy";
        internal const string FORMATDATE = " d ";
        internal const string FORMATYEAR = "yyyy";
        internal const string FORMATSHORTDATE = "M/d/yy";
        internal const int CELLCOUNT = 42;
        internal const int WEEKCOUNT = 7;
        internal const string FORMATMONTH = "MMM";

        private ElementReference dayCell;

        [CascadingParameter]
        internal CalendarBase<TCalendarCell> Parent { get; set; }

        /// <summary>
        /// Bind the cell click for calendar.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<CellDetails> OnCellClick { get; set; }

        /// <summary>
        /// Get or Set the current date.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public DateTime CurrentCellDate { get; set; }

        /// <summary>
        /// Get or Set the local date.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public DateTime LocalDates { get; set; }

        /// <summary>
        /// Specifies the class of cell.
        /// </summary>
        [Parameter]
        public string CellClass { get; set; }

        /// <summary>
        /// Specifies whether the current date is focused or not.
        /// </summary>
        [Parameter]
        public bool IsFocusTodayDate { get; set; } = true;

        /// <summary>
        /// Get or Set Cell value.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public int Cells { get; set; }

        /// <summary>
        /// Get or Set the today date value.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public DateTime TodayCellDate { get; set; }

        /// <summary>
        /// Get or Set calendar navigation.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsNavigation { get; set; }

        /// <summary>
        /// Get or Set current calendar view.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public CalendarView CalendarRenderView { get; set; }

        /// <summary>
        /// Get or Set the calendar cell selection.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsSelect { get; set; }

        /// <summary>
        /// Specifies the option to enable the multiple dates selection of the calendar.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Get or Set calendar values.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public DateTime[] MultiselectValues { get; set; }
        /// <exclude/>
        internal string RowEleClass { get; set; }

        private string TdEleClass { get; set; }

        private string DayTitle { get; set; }

        private string DayLink { get; set; }

        private bool RangeDisabled { get; set; }

        private bool IsDisabled { get; set; }

        /// <summary>
        /// Triggers when the component is rendered for the first time.
        /// </summary>
        /// <returns>Task.</returns>
        protected async override Task OnInitializedAsync()
        {
            await RenderCell(CurrentCellDate, Parent.Value, IsMultiSelect, MultiselectValues, CalendarRenderView);
        }

        /// <summary>
        /// Triggers when any of the property is changed dynamically.
        /// </summary>
        /// <returns>Task.</returns>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (IsNavigation || (Parent.PropertyChanges.Count > 0 && !SfBaseUtils.Equals(GenericValue(CurrentCellDate), Parent.Value)))
            {
                await RenderCell(CurrentCellDate, Parent.Value, IsMultiSelect, MultiselectValues, CalendarRenderView);
            }
        }

        /// <summary>
        /// Triggers after the component is rendered.
        /// </summary>
        /// <param name="firstRender">true if the component is rendered for the first time,otherwise false.</param>
        protected override void OnAfterRender(bool firstRender)
        {
            if (IsNavigation)
            {
                UpdateCellDetails();
            }

            if (firstRender)
            {
                UpdateCellDetails();
            }
        }

        private void UpdateCellDetails()
        {
            var isCellPresent = Parent.CellDetailsData.Where(i => i.CellID == LocalDates.Ticks + "_" + Cells).FirstOrDefault();
            if (isCellPresent == null)
            {
                Parent.CellDetailsData.Add(new CellDetails { CellID = LocalDates.Ticks + "_" + Cells, ClassList = !string.IsNullOrEmpty(CellClass) ? CellClass : TdEleClass, Element = dayCell, EventArgs = new MouseEventArgs(), CurrentDate = CurrentCellDate });
            }
        }
        private void UpdateTitle()
        {
            string title = Intl.GetDateFormat(LocalDates, FORMATFULLDATE, CalendarLocale);
            RangeDisabled = (Parent.Min.Date > LocalDates) || (Parent.Max.Date < LocalDates.Date);
            if (RangeDisabled)
            {
                DayTitle = string.Empty;
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
            else
            {
                DayTitle = title;
            }
        }
        private void UpdateWeekEnds(int currentMonth)
        {
            if (currentMonth != LocalDates.Month)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OTHERMONTH);
            }

            if ((int)LocalDates.DayOfWeek == 0 || (int)LocalDates.DayOfWeek == 6)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, WEEKEND);
            }
        }
        private async Task UpdateDisableCells(RenderDayCellEventArgs eventArgs, TCalendarCell dateValue, bool multiSelection, DateTime[] values)
        {
            bool isDisabledCell = TdEleClass != null && TdEleClass.Contains(DISABLED, StringComparison.Ordinal);
            if (eventArgs.IsDisabled || (IsSelect && (isDisabledCell || RangeDisabled)))
            {
                if (multiSelection && values != null && values.Length > 0)
                {
                    for (int index = 0; index < values.Length; index++)
                    {
                        List<DateTime> val = values.ToList();
                        if (eventArgs.Date.Ticks == values[index].Ticks)
                        {
                            val.RemoveAt(index);
                            values = val.ToArray();
                            index = -1;
                        }
                    }
                }
                else if (dateValue != null && ConvertDate(dateValue) == eventArgs.Date)
                {
                    await Parent.UpdateCalendarProperty(VALUE, default(TCalendarCell));
                }
            }
            if ((eventArgs.IsDisabled && (LocalDates != DateTime.Now)) || (IsSelect && (isDisabledCell || RangeDisabled)))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
        }
        private void UpdateMultiValues(DateTime[] values, TCalendarCell dateValue, bool otherMnthBool, bool disabledCls)
        {
            dateValue = (dateValue != null) ? dateValue : GenericValue(values[0]);
            DateTime getValue = ConvertDate(dateValue);
            for (int tempValue = 0; tempValue < values.Length; tempValue++)
            {
                string localDateString = Intl.GetDateFormat(LocalDates, FORMATSHORTDATE, CalendarLocale);
                string tempDateString = Intl.GetDateFormat(values[tempValue], FORMATSHORTDATE, CalendarLocale);
                if ((localDateString == tempDateString && GetDateVal(LocalDates, values[tempValue])) || GetDateVal(LocalDates, getValue))
                {
                    TdEleClass = TdEleClass.Contains(FOCUSEDDATE, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(TdEleClass, FOCUSEDDATE) : TdEleClass;
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, SELECTED);
                }
                else
                {
                    UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
                }
            }

            if (values.Length <= 0)
            {
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
                if (dateValue != null)
                {
                    Parent.Value = default(TCalendarCell);
                }
            }
        }
        private async Task RenderDayCell(DateTime currentDate, TCalendarCell dateValue, bool multiSelection, DateTime[] values)
        {
            int currentMonth = currentDate.Month;
            DateTime date = LocalDates;
            if (IsSelect || Parent.IsTodayClick)
            {
                TdEleClass = SfBaseUtils.RemoveClass(TdEleClass, TODAY);
                TdEleClass = SfBaseUtils.RemoveClass(TdEleClass, SELECTED);
                TdEleClass = SfBaseUtils.RemoveClass(TdEleClass, FOCUSEDDATE);
                TdEleClass = SfBaseUtils.RemoveClass(TdEleClass, OTHERMONTH);
            } else
            {
                TdEleClass = CELL;
            }
            DayLink = Intl.GetDateFormat(LocalDates, FORMATDATE, CalendarLocale).Trim();
            UpdateTitle();
            UpdateWeekEnds(currentMonth);
            RenderDayCellEventArgs eventArgs = await TriggerDayCellEvent();
            if ((Parent as SfDateRangePicker<TCalendarCell>) == null)
            {
                TdEleClass = !string.IsNullOrEmpty(eventArgs?.CellData?.ClassList) ? eventArgs.CellData.ClassList : TdEleClass;
            }
            await UpdateDisableCells(eventArgs, dateValue, multiSelection, values);
            bool otherMnthBool = TdEleClass.Contains(OTHERMONTH, StringComparison.Ordinal);
            bool disabledCls = TdEleClass.Contains(DISABLED, StringComparison.Ordinal);
            if (multiSelection && values != null && values.Length > 0 && !otherMnthBool && !disabledCls)
            {
                UpdateMultiValues(values, dateValue, otherMnthBool, disabledCls);
            }
            else if (multiSelection && values != null && values.Length <= 0)
            {
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
                if (dateValue != null)
                {
                    Parent.Value = default(TCalendarCell);
                }
            }
            else if (dateValue != null)
            {
                DateTime dateVal = ConvertDate(dateValue);
                if (!disabledCls && GetDateVal(LocalDates, dateVal))
                {
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, SELECTED);
                }
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
            }
            else
            {
                UpdateFocus(otherMnthBool, disabledCls, LocalDates, CurrentCellDate);
            }

            if (date.Month == DateTime.Now.Month && date.Day == DateTime.Now.Day && date.Year == DateTime.Now.Year)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, TODAY);
            }

            if (Cells == CELLCOUNT)
            {
                IsNavigation = false;
            }
        }

        private async Task<RenderDayCellEventArgs> TriggerDayCellEvent()
        {
            var cellData = new CellDetails { CellID = LocalDates.Ticks + "_" + Cells, ClassList = CellClass != null ? CellClass : TdEleClass, Element = dayCell, EventArgs = new MouseEventArgs(), CurrentDate = CurrentCellDate };
            RenderDayCellEventArgs eventArgs = new RenderDayCellEventArgs()
            {
                Date = LocalDates,
                IsDisabled = IsDisabled,
                IsOutOfRange = RangeDisabled,
                Name = RENDERDAYCELL,
                CellData = cellData
            };
            var isEqualVal = Parent.PropertyChanges != null && Parent.PropertyChanges.Count > 0 && Parent.PropertyChanges.ContainsKey(VALUE);
            if (!IsSelect && !isEqualVal)
            {
                await Parent.BindRenderDayEvent(eventArgs);
            }

            return eventArgs;
        }

        /// <exclude/>
        internal async Task RenderCell(DateTime currentDate, TCalendarCell dateValue, bool multiSelection, DateTime[] values, CalendarView calendarView)
        {
            switch (calendarView)
            {
                case CalendarView.Year:
                    await RenderMonthCell(currentDate, dateValue);
                    break;
                case CalendarView.Decade:
                    await RenderYearCell(currentDate, dateValue);
                    break;
                case CalendarView.Month:
                    await RenderDayCell(currentDate, dateValue, multiSelection, values);
                    break;
            }
        }

        private async Task RenderMonthCell(DateTime currentDate, TCalendarCell dateValue)
        {
            DateTime curDate = currentDate;
            int curYrs = LocalDates.Year;
            int minYr = Parent.Min.Year;
            int maxYr = Parent.Max.Year;
            int month = Cells + 1;
            DateTime dateVal = dateValue != null ? ConvertDate(dateValue) : DateTime.Now;
            bool localMonth = dateValue != null && dateVal.Month == LocalDates.Month;
            bool select = dateValue != null && dateVal.Year == curDate.Year && localMonth;
            TdEleClass = CELL;
            DayLink = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Intl.GetDateFormat(LocalDates, FORMATMONTH, CalendarLocale));
            if ((curYrs < minYr || (month < Parent.Min.Month && curYrs == minYr)) || (curYrs > maxYr || (month > Parent.Max.Month && curYrs >= maxYr)))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
            }
            else if (dateValue != null && select)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, SELECTED);
            }
            else if (LocalDates.Month == curDate.Month && currentDate.Month == curDate.Month)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
            }

            RenderDayCellEventArgs eventArgs = await TriggerDayCellEvent();
            if (eventArgs.IsDisabled && (LocalDates != DateTime.Now))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
        }

        private async Task RenderYearCell(DateTime currentDate, TCalendarCell dateValue)
        {
            int localYr = currentDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYear = localYr - (localYr % 10);
            int endYear = localYr - (localYr % 10) + (10 - 1);
            DateTime startYr = new DateTime(startYear, 1, 1);
            DateTime endYr = new DateTime(endYear, 1, 1);
            DateTime start = new DateTime(localYr - (localYr % 10) - 1, 1, 1);
            int year = start.Year + Cells;
            DateTime localDate = new DateTime(year, LocalDates.Month, LocalDates.Day);
            TdEleClass = CELL;
            DayLink = Intl.GetDateFormat(localDate, FORMATYEAR, CalendarLocale);
            if ((year < startYr.Year) || (year > endYr.Year))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OTHERDECADE);
                if (year < Parent.Min.Year || year > Parent.Max.Year)
                {
                    TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                }
            }
            else if (year < Parent.Min.Year || year > Parent.Max.Year)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
            }
            else if (dateValue != null)
            {
                DateTime dateTimeVal = ConvertDate(dateValue);
                var isLocalyear = localDate.Year == currentDate.Year && !TdEleClass.Contains(DISABLED, StringComparison.Ordinal);
                TdEleClass = (localDate.Year == dateTimeVal.Year) ? SfBaseUtils.AddClass(TdEleClass, SELECTED) : isLocalyear ? SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE) : TdEleClass;
            }
            else if (localDate.Year == currentDate.Year && !TdEleClass.Contains(DISABLED, StringComparison.Ordinal))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
            }

            RenderDayCellEventArgs eventArgs = await TriggerDayCellEvent();

            if (eventArgs.IsDisabled && (LocalDates != DateTime.Now))
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, DISABLED);
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, OVERLAY);
            }
        }

#pragma warning disable CA1822 // Mark members as static
        private bool GetDateVal(DateTime date, DateTime dateValue)
#pragma warning restore CA1822 // Mark members as static
        {
            return date.Day == dateValue.Day && date.Month == dateValue.Month && date.Year == dateValue.Year;
        }

        private void UpdateFocus(bool otherMonth, bool disabled, DateTime localDate, DateTime currentDate)
        {
            if (currentDate.Day == localDate.Day && !otherMonth && !disabled && IsFocusTodayDate)
            {
                TdEleClass = SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE);
            }
            else
            {
                var checkMinFocus = currentDate >= Parent.Max && localDate == Parent.Max && !otherMonth && !disabled;
                var checkMaxFocus = currentDate <= Parent.Min && localDate == Parent.Min && !otherMonth && !disabled;
                TdEleClass = (checkMinFocus || (checkMaxFocus && IsFocusTodayDate)) ? SfBaseUtils.AddClass(TdEleClass, FOCUSEDDATE) : TdEleClass;
            }
        }
    }
}