using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Internal;
using System.Linq;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Calendars.Internal
{
    /// <summary>
    /// The Calendar base is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    public partial class CalendarBaseRender<TValue> : CalendarBase<TValue>
    {
        internal ElementReference PrevElement { get; set; }

        internal ElementReference NextElement { get; set; }

        internal ElementReference TableBodyEle { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ScriptModules = SfScriptModules.SfCalendarBase;
            CalendarBase_MultiValues = MultiValues;
            await Render();
        }

        /// <summary>
        /// Triggers when any of components property get changed dynamically.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            MultiValues = CalendarBase_MultiValues = await SfBaseUtils.UpdateProperty(MultiValues, CalendarBase_MultiValues, MultiValuesChanged, null, null);
            if (Parent.PropertyChanges.Count > 0)
            {
                var listOfKeys = new List<string>(Parent.PropertyChanges.Keys);
                string[] keys = new string[] { "Start", "Depth", "FirstDayOfWeek", "Min", "Max", "Values" };
                foreach (var key in listOfKeys)
                {
                    if (keys.Contains(key))
                    {
                        if (Parent as SfCalendar<TValue> != null)
                        {
                            Parent.PropertyChanges.Remove(key);
                        }
                        IsNavigation = true;
                        await Update();
                    }
                }

                if (!SfBaseUtils.Equals(Parent.Value, GenericValue(CurrentDate)))
                {
                    if (Parent as SfCalendar<TValue> != null)
                    {
                        ValidateDate();
                        await MinMaxUpdate(Parent.Value);
                        int currentView = GetViewNumber(CurrentView());
                        SwitchView(currentView);
                    }
                    else
                    {
                        await Update();
                    }
                }
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            var parentComp = Parent as SfCalendar<TValue>;
            if (parentComp != null)
            {
                await InvokeMethod("sfBlazor.CalendarBase.initialize", new object[] { Parent.Element, DotnetObjectReference, Parent.KeyConfigs, Parent.Value, MultiSelection });
                IsDeviceMode = await InvokeMethod<bool>("sfBlazor.isDevice", false, null);
            }
        }

        /// <summary>
        /// Invoke the keyboard action handler.
        /// </summary>
        /// <param name="args"><see cref="KeyActions"/> arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnCalendarKeyboardEvent(KeyActions args)
        {
            if (args != null)
            {
                await KeyActionHandler(args);
            }
        }

        private async Task Render()
        {
            IsNavigation = false;
            PropertyType = typeof(TValue);
            var timeSpan = new TimeSpan(0, 0, 0);
            TodayDate = DateTime.Now.Date + timeSpan;
            CurrentDate = (Parent != null && Parent.Value != null && !SfBaseUtils.Equals(Parent.Value, default)) ? ConvertDate(Parent.Value) : (CurrentDateValue != null) ? ConvertDate(CurrentDateValue) : DateTime.Now.Date + timeSpan;
            if ((Parent as SfDateRangePicker<TValue>) == null)
            {
                ValidateDate();
                await MinMaxUpdate(Parent.Value);
            }

            CreateHeader();
            CreateContent();
        }

        private void CreateHeader()
        {
            TitleClass = LINK + SPACE + TITLE;
            ContentHeader = HEADER;
            PrevIconClass = PREV_ICON;
            NextIconClass = NEXT_ICON;
            PrevIconAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, FALSE, PrevIconAttr);
            NextIconAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, FALSE, NextIconAttr);
        }

        private void ValidateDate()
        {
            CurrentDate = (CurrentDate != default) ? CurrentDate : DateTime.Now.Date + new TimeSpan(0, 0, 0);
            if (Parent.Value != null && !SfBaseUtils.Equals(Parent.Value, default(TValue)))
            {
                DateTime currentVal = ConvertDate(Parent.Value);
                if (Parent.Min <= Parent.Max && currentVal >= Parent.Min && currentVal <= Parent.Max)
                {
                    CurrentDate = currentVal;
                }
            }
        }

        /// <summary>
        /// Method used to update the minimum and maximum value.
        /// </summary>
        /// <param name="minMaxValue">.</param>
        /// <returns>Task.</returns>
        protected async Task MinMaxUpdate(TValue minMaxValue)
        {
            DateTime getValue = (minMaxValue == null || SfBaseUtils.Equals(minMaxValue, default)) ? CurrentDate : ConvertDate(minMaxValue);
            if ((getValue.Date != Parent.Min.Date) && getValue <= Parent.Min && Parent.Min <= Parent.Max)
            {
                CurrentDate = Parent.Min;
            }
            else if ((getValue.Date != Parent.Max.Date) && getValue >= Parent.Max && Parent.Min <= Parent.Max)
            {
                CurrentDate = Parent.Max;
            }

            if (minMaxValue != null && !SfBaseUtils.Equals(minMaxValue, default))
            {
                DateTime dateValue = ConvertDate(minMaxValue);
                Type type = typeof(TValue);
                bool isNullable = Nullable.GetUnderlyingType(type) != null;
                if (dateValue < Parent.Min && Parent.Min <= Parent.Max)
                {
                    SetMinMaxValue(Parent.Min, type, isNullable);
                }
                else if (dateValue > Parent.Max && Parent.Min <= Parent.Max)
                {
                    SetMinMaxValue(Parent.Max, type, isNullable);
                }
            }
            else
            {
                UpdateMinMax(minMaxValue);
            }
            await Task.CompletedTask;
        }

        private void UpdateMinMax(TValue dateValue)
        {
            DateTime currentVal = (dateValue == null || SfBaseUtils.Equals(dateValue, default)) ? CurrentDate : ConvertDate(dateValue);
            if (Parent.Min <= Parent.Max && dateValue != null && !SfBaseUtils.Equals(dateValue, default) && currentVal <= Parent.Max && currentVal >= Parent.Min)
            {
                CurrentDate = currentVal;
            }
            else
            {
                var isMaxVal = Parent.Min <= Parent.Max && (dateValue == null || SfBaseUtils.Equals(dateValue, default)) && CurrentDate > Parent.Max;
                var isMinVal = CurrentDate < Parent.Min;
                CurrentDate = isMaxVal ? Parent.Max : isMinVal ? Parent.Min : CurrentDate;
            }
        }

        private void SetMinMaxValue(DateTime dateValue, Type type, bool isNullable)
        {
            bool isDateTimeOffset = type == typeof(DateTimeOffset) || (isNullable && Nullable.GetUnderlyingType(type) == typeof(DateTimeOffset));
            TValue val = isDateTimeOffset ? (TValue)SfBaseUtils.ChangeType(new DateTimeOffset(dateValue), type) : (TValue)SfBaseUtils.ChangeType(dateValue, type);
            UpdateMinMax(val);
        }

        private void CreateContentBody()
        {
            switch (Parent.Start)
            {
                case CalendarView.Year:
                    RenderYears();
                    break;
                case CalendarView.Decade:
                    RenderDecades();
                    break;
                case CalendarView.Month:
                    RenderMonths();
                    break;
            }
        }

        internal void RenderMonths(MouseEventArgs args = null)
        {
            LocalMainDate = new List<DateTime>();
            CalendarView = CalendarView.Month;
            NumCells = WEEK_NUMBER;
            RenderDays(CurrentDate);
            RenderTemplate(NumCells, MONTH, args);
        }

        private void RenderYears(MouseEventArgs args = null)
        {
            LocalMainDate = new List<DateTime>();
            CalendarView = CalendarView.Year;
            CellsCount = YEAR_NUMBER;
            NumCells = CELL_ROW;
            LocalDate = new DateTime(CurrentDate.Year, 1, CurrentDate.Day, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
            TitleUpdate(CurrentDate, MONTHS);
            RenderTemplate(NumCells, YEAR, args);
        }

        private void RenderDecades(MouseEventArgs args = null)
        {
            LocalMainDate = new List<DateTime>();
            CalendarView = CalendarView.Decade;
            CellsCount = YEAR_NUMBER;
            NumCells = CELL_ROW;
            LocalDate = new DateTime(CurrentDate.Year, 1, 1, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
            UpdateDecadeTitle();
            RenderTemplate(NumberCell, DECADE, args);
        }

        private void UpdateDecadeTitle()
        {
            int localYr = LocalDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - (localYr % 10);
            int endYr = startYr + (10 - 1);
            string startHdrYr = Intl.GetDateFormat(new DateTime(startYr, 1, 1), FORMAT_YEAR, CalendarLocale);
            string endHdrYr = Intl.GetDateFormat(new DateTime(endYr, 1, 1), FORMAT_YEAR, CalendarLocale);
            DateTime start = new DateTime(localYr - (localYr % 10) - 1, 1, 1);
            LocalDate = start;
            HeaderTitle = startHdrYr + TITLE_SEPARATOR + endHdrYr;
        }

        private string StartHeadYr(DateTime date)
        {
            DateTime datevalue = new DateTime(date.Year, 1, 1);
            int localYr = datevalue.Year;
            int startYr = localYr - (localYr % 10);
            string startHdrYr = Intl.GetDateFormat(new DateTime(startYr, 1, 1), FORMAT_YEAR, CalendarLocale);
            return startHdrYr;
        }

        private string EndHeadYr(DateTime date)
        {
            DateTime datevalue = new DateTime(date.Year, 1, 1);
            int localYr = datevalue.Year;
            int endYr = localYr - (localYr % 10) + (10 - 1);
            string endHdrYr = Intl.GetDateFormat(new DateTime(endYr, 1, 1), FORMAT_YEAR, CalendarLocale);
            return endHdrYr;
        }

        private void RenderDays(DateTime currentDate)
        {
            CellsCount = CELLCOUNT;
            LocalDate = currentDate;
            NumCells = WEEK_NUMBER;
            TitleUpdate(currentDate, DAYS);
            var firstDayValue = (int)Parent.FirstDayOfWeek;
            if (firstDayValue != 0)
            {
                DateTime firstDayOfMonth = new DateTime(LocalDate.Year, LocalDate.Month, 1, LocalDate.Hour, LocalDate.Minute, LocalDate.Second);
                while ((int)firstDayOfMonth.DayOfWeek != firstDayValue)
                {
                    firstDayOfMonth = firstDayOfMonth.AddDays(-1);
                }

                LocalDate = firstDayOfMonth;
            }
            else
            {
                DateTime firstDayOfMonth = new DateTime(LocalDate.Year, LocalDate.Month, 1, LocalDate.Hour, LocalDate.Minute, LocalDate.Second);
                int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
                LocalDate = firstDayOfMonth.Date == default(DateTime).Date ? firstDayOfMonth : firstDayOfMonth.AddDays(-1 * dayOfWeek);
            }
        }

        private void RenderTemplate(int count, string classNm, MouseEventArgs args = null)
        {
            ContentElementClass = CONTENT + SPACE + classNm;
            ContentHeader = HEADER + SPACE + classNm;
            Row = count;
            Count = count;
            IconHandler();
            TValue tempValue = Parent.Value == null ? default(TValue) : (TValue)SfBaseUtils.ChangeType(Parent.Value, PropertyType);
            Parent.ChangedArgs = new ChangedEventArgs<TValue> { Value = tempValue, Values = MultiValues };
            Parent.ChangeHandler(args, MultiValues, MultiSelection);
        }

        private void IconHandler()
        {
            switch (CurrentView())
            {
                case MONTH_VIEW:
                    PreviousIconHandler(CompareMonth(CurrentDate, Parent.Min) < 1);
                    NextIconHandler(CompareMonth(CurrentDate, Parent.Max) > -1);
                    break;
                case YEAR_VIEW:
                    PreviousIconHandler(CompareDateVal(CurrentDate, Parent.Min, 0) < 1);
                    NextIconHandler(CompareDateVal(CurrentDate, Parent.Max, 0) > -1);
                    break;
                case DECADE_VIEW:
                    PreviousIconHandler(CompareDateVal(CurrentDate, Parent.Min, 10) < 1);
                    NextIconHandler(CompareDateVal(CurrentDate, Parent.Max, 10) > -1);
                    break;
            }
        }

        private void PreviousIconHandler(bool disabled)
        {
            PrevIconClass = disabled ? SfBaseUtils.AddClass(PrevIconClass, DISABLED) : SfBaseUtils.RemoveClass(PrevIconClass, DISABLED);
            PrevIconClass = disabled ? SfBaseUtils.AddClass(PrevIconClass, OVERLAY) : SfBaseUtils.RemoveClass(PrevIconClass, OVERLAY);
            PrevIconAttr[ARIA_DISABLED] = disabled ? TRUE : FALSE;
        }

        /// <summary>
        /// Gets the current view of the Calendar.
        /// </summary>
        internal string CurrentView()
        {
            if (ContentElementClass.Contains(YEAR, StringComparison.Ordinal))
            {
                return YEAR_VIEW;
            }
            else if (ContentElementClass.Contains(DECADE, StringComparison.Ordinal))
            {
                return DECADE_VIEW;
            }
            else
            {
                return MONTH_VIEW;
            }
        }

        /// <summary>
        /// Checks whether the value type is DateTime.
        /// </summary>
        /// <returns>True or false based on the Type.</returns>
#pragma warning disable CA1822 // Mark members as static
        internal bool IsDateTimeType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return type == typeof(DateTime) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Checks whether the value type is DateTimeOffset.
        /// </summary>
        /// <returns>True or false based on the Type.</returns>
#pragma warning disable CA1822 // Mark members as static
        internal bool IsDateTimeOffsetType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return type == typeof(DateTimeOffset) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(type));
        }

        private void NextIconHandler(bool disabled)
        {
            NextIconClass = disabled ? SfBaseUtils.AddClass(NextIconClass, DISABLED) : SfBaseUtils.RemoveClass(NextIconClass, DISABLED);
            NextIconClass = disabled ? SfBaseUtils.AddClass(NextIconClass, OVERLAY) : SfBaseUtils.RemoveClass(NextIconClass, OVERLAY);
            NextIconAttr[ARIA_DISABLED] = disabled ? TRUE : FALSE;
        }

#pragma warning disable CA1822 // Mark members as static
        internal int GetViewNumber(string stringVal)
        {
            return (stringVal == MONTH_VIEW) ? MONTH_VIEW_VAL : (stringVal == YEAR_VIEW) ? YEAR_VIEW_VAL : DECADE_VIEW_VAL;
        }

        internal void CreateContent()
        {
            ContentElement = true;
            ContentElementClass = CONTENT;
            CreateContentBody();
            if (Parent.ShowTodayButton)
            {
                CreateContentFooter();
            }
        }

        internal async Task Update()
        {
            IsSelect = false;
            ValidateDate();
            await MinMaxUpdate(Parent.Value);
            CreateContentBody();
        }

        private void TitleUpdate(DateTime currentDate, string view)
        {
            var fromtTitle = (view == DAYS) ? FORMAT_MONTHS : FORMAT_YEAR;
            HeaderTitle = Intl.GetDateFormat(currentDate, fromtTitle, CalendarLocale);
        }

        private void NavigatePreviousHandler(MouseEventArgs args)
        {
            if (Enabled && args != null)
            {
                NextPrevIconHandler(false);
            }
        }

        private void NavigateNextHandler(MouseEventArgs args)
        {
            if (Enabled && args != null)
            {
                NextPrevIconHandler(true);
            }
        }

        private void TriggerNavigate(MouseEventArgs args)
        {
            NavigatedEventArgs eventArgs = new NavigatedEventArgs()
            {
                Date = CurrentDate,
                View = CurrentView(),
                Event = args,
                Name = NAVIGATED,
            };
            IsSelect = false;
            Parent.BindNavigateEvent(eventArgs);
        }

        internal void AddMonths(DateTime date, int index)
        {
            DateTime currentVal = date.AddMonths(index);
            if (Parent.Min <= Parent.Max && currentVal.Month >= Parent.Min.Month && currentVal.Month <= Parent.Max.Month)
            {
                CurrentDate = currentVal;
            }
        }

        internal void AddYears(DateTime date, int index)
        {
            DateTime currentVal = date.AddYears(index);
            if (Parent.Min <= Parent.Max && currentVal.Year >= Parent.Min.Year && currentVal.Year <= Parent.Max.Year)
            {
                CurrentDate = currentVal;
            }
        }

        internal void SwitchView(int view, MouseEventArgs args = null)
        {
            switch (view)
            {
                case MONTH_VIEW_VAL:
                    IsNavigation = true;
                    RenderMonths(args);
                    break;
                case YEAR_VIEW_VAL:
                    IsNavigation = true;
                    RenderYears(args);
                    SetAnimation();
                    break;
                case DECADE_VIEW_VAL:
                    IsNavigation = true;
                    RenderDecades(args);
                    SetAnimation();
                    break;
            }

            InvokeAsync(() => StateHasChanged());
            TriggerNavigate(args);
        }

        private void SetAnimation()
        {
            if (!Parent.IsTodayClick && !IsKeyboardSelect)
            {
                _ = SfBaseUtils.Animate(JSRuntime, TableBodyEle, Animate);
            }
        }

        private void NextPrevIconHandler(bool isNext)
        {
            int currentView = GetViewNumber(CurrentView());
            switch (CurrentView())
            {
                case MONTH_VIEW:
                    AddMonths(CurrentDate, isNext ? 1 : -1);
                    SwitchView(currentView);
                    break;
                case YEAR_VIEW:
                    AddYears(CurrentDate, isNext ? 1 : -1);
                    SwitchView(currentView);
                    break;
                case DECADE_VIEW:
                    AddYears(CurrentDate, isNext ? 10 : -10);
                    SwitchView(currentView);
                    break;
            }
        }

        internal void NavigateTitle()
        {
            if (Enabled)
            {
                IsSelect = false;
                int currentView = GetViewNumber(CurrentView());
                IsKeyboardSelect = false;
                SwitchView(++currentView);
            }
        }

        private async Task ClickHandler(CellDetails args)
        {
            if (Enabled)
            {
                if (IsDeviceMode)
                {
                    IsCellClicked = true;
                    ContentElement = false;
                    StateHasChanged();
                }

                ValidateDate();
                await MinMaxUpdate(Parent.Value);
                await CellClick(args);
            }
        }

        private async Task CellClick(CellDetails args)
        {
            string classList = args.ClassList;
            int view = GetViewNumber(CurrentView());
            if (classList.Contains(OTHER_MONTH, StringComparison.Ordinal))
            {
                await ContentClick(args, MONTH_VIEW_VAL);
            }
            else if (view == GetViewNumber(Parent.Depth.ToString()) && GetViewNumber(Parent.Start.ToString()) >= GetViewNumber(Parent.Depth.ToString()))
            {
                await ContentClick(args, YEAR_VIEW_VAL);
            }
            else if (view == DECADE_VIEW_VAL)
            {
                await ContentClick(args, YEAR_VIEW_VAL);
            }
            else if (!classList.Contains(OTHER_MONTH, StringComparison.Ordinal) && view == MONTH_VIEW_VAL)
            {
                await SelectDate(args.EventArgs, args.CellID, MultiSelection, MultiValues, args);
            }
            else
            {
                await ContentClick(args, MONTH_VIEW_VAL);
            }
        }

        internal async Task ContentClick(CellDetails args, int view)
        {
            int currentView = GetViewNumber(CurrentView());
            DateTime curDate = IsKeyboardSelect ? CurrentDate : GetIdValue(args);
            var depth = Parent.Depth.ToString();
            var start = Parent.Start.ToString();
            var isDepthView = currentView == GetViewNumber(depth) && GetViewNumber(start) >= GetViewNumber(depth);
            if (view == MONTH_VIEW_VAL)
            {
                if (isDepthView)
                {
                    CurrentDate = curDate;
                    if (MultiSelection && !Parent.CheckPresentDate(curDate, MultiValues))
                    {
                        List<DateTime> copyValues = Parent.CopyValues(MultiValues).ToList();
                        var isDefaultValue = Parent.Value != null && !SfBaseUtils.Equals(Parent.Value, default(TValue));
                        var changeValue = isDefaultValue ? (DateTime)SfBaseUtils.ChangeType(Parent.Value, typeof(TValue)) : DateTime.Now;
                        if (isDefaultValue && !copyValues.Contains(changeValue))
                        {
                            copyValues.Add(changeValue);
                        }

                        copyValues.Add(curDate);
                        MultiValues = copyValues.ToArray();
                        await Parent.UpdateCalendarProperty(CALENDAR_BASE_VALUES, copyValues.ToArray());
                    }

                    await Parent.UpdateCalendarProperty(VALUE, GenericValue(curDate));
                }
                else
                {
                    DateTime contentDate = new DateTime(CurrentDate.Year, curDate.Month, curDate.Day, curDate.Hour, curDate.Minute, curDate.Second, curDate.Millisecond);
                    if (curDate.Month > 0 && CurrentDate.Month != curDate.Month)
                    {
                        contentDate = new DateTime(contentDate.Year, contentDate.Month, curDate.Day, curDate.Hour, curDate.Minute, curDate.Second, curDate.Millisecond);
                    }

                    contentDate = new DateTime(curDate.Year, contentDate.Month, contentDate.Day, curDate.Hour, curDate.Minute, curDate.Second, curDate.Millisecond);
                    CurrentDate = contentDate;
                }

                await SfBaseUtils.Animate(JSRuntime, TableBodyEle, Animate);
                IsNavigation = true;
                RenderMonths();
                TriggerNavigate(args.EventArgs);
            }
            else if (view == YEAR_VIEW_VAL)
            {
                if (isDepthView)
                {
                    await SelectDate(args.EventArgs, args.CellID, MultiSelection, MultiValues, args);
                }
                else
                {
                    CurrentDate = new DateTime(curDate.Year, CurrentDate.Month, CurrentDate.Day, CurrentDate.Hour, CurrentDate.Minute, CurrentDate.Second, CurrentDate.Millisecond);
                    await SfBaseUtils.Animate(JSRuntime, TableBodyEle, Animate);
                    IsNavigation = true;
                    RenderYears();
                    TriggerNavigate(args.EventArgs);
                }
            }
        }

        private async Task SetDateDecade(DateTime date, int year)
        {
            date = new DateTime(year, date.Month, date.Day);
            await Parent.UpdateCalendarProperty(VALUE, GenericValue(date));
        }

        private async Task SetDateYear(DateTime dateValue)
        {
            var dayValue = Parent.Value != null ? dateValue.Day : DateTime.Now.Day;
            DateTime date = new DateTime(dateValue.Year, dateValue.Month, dayValue);
            date = (dateValue.Month != date.Month) ? new DateTime(dateValue.Year, dateValue.Month, 0) : date;
            await Parent.UpdateCalendarProperty(VALUE, GenericValue(date));
        }

        private async Task SelectedDateValue(TValue selectedValue)
        {
            if (!SfBaseUtils.Equals(selectedValue, Parent.PreviousSelectedDate) || SfBaseUtils.Equals(selectedValue, Parent.PreviousDeSelectedDate))
            {
                var selectEventArgs = new SelectedEventArgs<TValue>() { Value = selectedValue };
                await Parent.InvokeSelectEvent(selectEventArgs);
                Parent.PreviousSelectedDate = selectedValue;
            }
        }

        private async Task SelectDate(MouseEventArgs events, string cellId, bool multiSelection, DateTime[] values, CellDetails args = null)
        {
            if (CellClickHandler.HasDelegate)
            {
                if (args == null)
                {
                    args = new CellDetails() { CellID = cellId };
                }

                CurrentDate = args.CurrentDate;
                await SfBaseUtils.InvokeEvent<CellDetails>(CellClickHandler, args);
            }
            else
            {
                long id = long.Parse(cellId.Split(new char[] { '_' })[0], CultureInfo.CurrentCulture);
                DateTime date = new DateTime(id);
                Parent.IsTodayClick = false;
                if (CurrentView() == DECADE_VIEW)
                {
                    await SetDateDecade(CurrentDate, date.Year);
                }
                else if (CurrentView() == YEAR_VIEW)
                {
                    await SetDateYear(date);
                }
                else
                {
                    var selectedValue = default(TValue);
                    if (multiSelection && !Parent.CheckPresentDate(date, values))
                    {
                        List<DateTime> copyValues = Parent.CopyValues(values).ToList();
                        var isDefaultValue = Parent.Value != null && !SfBaseUtils.Equals(Parent.Value, default(TValue));
                        var changeValue = isDefaultValue ? (DateTime)SfBaseUtils.ChangeType(Parent.Value, typeof(TValue)) : DateTime.Now;
                        if (isDefaultValue && !copyValues.Contains(changeValue))
                        {
                            copyValues.Add(changeValue);
                        }

                        copyValues.Add(date);
                        MultiValues = copyValues.ToArray();
                        await Parent.UpdateCalendarProperty(CALENDAR_BASE_VALUES, copyValues.ToArray());
                        var selectValues = copyValues.ToArray();
                        selectedValue = GenericValue(selectValues[selectValues.Length - 1]);
                        await SelectedDateValue(selectedValue);
                        await Parent.UpdateCalendarProperty(VALUE, GenericValue(selectValues[selectValues.Length - 1]));
                    }
                    else
                    {
                        selectedValue = GenericValue(new DateTime(id));
                        if (!multiSelection)
                        {
                            await SelectedDateValue(selectedValue);
                        }

                        await Parent.UpdateCalendarProperty(VALUE, GenericValue(new DateTime(id)));
                    }
                }

                if (multiSelection && values != null && values.Length > 0 && values.Contains(date))
                {
                    List<DateTime> copyValues = Parent.CopyValues(values).ToList();
                    for (int item = 0; item < copyValues.Count; item++)
                    {
                        string localDateString = Intl.GetDateFormat(date, FORMAT_SHORT_DATE, CalendarLocale);
                        string tempDateString = Intl.GetDateFormat(copyValues[item], FORMAT_SHORT_DATE, CalendarLocale);
                        if (localDateString == tempDateString)
                        {
                            var deselectedValue = GenericValue(copyValues[item]);
                            if (!SfBaseUtils.Equals(deselectedValue, Parent.PreviousDeSelectedDate) || SfBaseUtils.Equals(deselectedValue, Parent.PreviousSelectedDate))
                            {
                                var deselectEventArgs = new DeSelectedEventArgs<TValue>()
                                {
                                    Value = deselectedValue
                                };
                                await Parent.InvokeDeSelectEvent(deselectEventArgs);
                                Parent.PreviousDeSelectedDate = deselectedValue;
                            }

                            copyValues.RemoveAt(item);
                        }
                    }

                    MultiValues = copyValues.ToArray();
                    await Parent.UpdateCalendarProperty(CALENDAR_BASE_VALUES, copyValues.ToArray());
                    var selectValues = MultiValues.Length > 0 ? GenericValue(MultiValues[MultiValues.Length - 1]) : default(TValue);
                    await Parent.UpdateCalendarProperty(VALUE, selectValues);
                }

                IsSelect = true;
                IsNavigation = true;
                CurrentDate = date;
                TValue tempValue = IsNullValue(Parent.Value) ? default : (TValue)SfBaseUtils.ChangeType(Parent.Value, PropertyType);
                Parent.ChangedArgs = new ChangedEventArgs<TValue> { Value = tempValue, Values = MultiValues };
                Parent.ChangeHandler(events, MultiValues, MultiSelection);
            }
        }

        private bool IsNullValue(TValue DateValue)
        {
            return DateValue == null;
        }

        private DateTime GetIdValue(CellDetails args)
        {
            long id = long.Parse(args.CellID.Split(new char[] { '_' })[0], CultureInfo.CurrentCulture);
            string dateString = Intl.GetDateFormat(new DateTime(id), FORMAT_FULL_DATE, CalendarLocale);
            var cultureInfo = Intl.GetCulture(CalendarLocale);
            DateTime date;
            var checkDateValue = DateTime.TryParseExact(dateString, FORMAT_FULL_DATE, cultureInfo, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out date);
            date = checkDateValue ? DateTime.ParseExact(dateString, FORMAT_FULL_DATE, cultureInfo) : new DateTime(id);
            return date;
        }

        private void CreateContentFooter()
        {
            TodayEleContent = Localizer.GetText(TODAY_LOCALE_KEY) == null ? TODAY_LOCALE_VALUE : Localizer.GetText(TODAY_LOCALE_KEY);
            TodayEleClass = BTN + SPACE + SPACE + TODAY + SPACE + FLAT + SPACE + PRIMARY + SPACE + CSS;
            if (!(Parent.Min.Date <= TodayDate && TodayDate <= Parent.Max.Date))
            {
                TodayEleClass = SfBaseUtils.AddClass(TodayEleClass, DISABLED);
            }
        }

        private async Task TodayButtonClick(MouseEventArgs args = null)
        {
            if (Enabled)
            {
                if (CurrentView() != Parent.Depth.ToString())
                {
                    await SfBaseUtils.Animate(JSRuntime, TableBodyEle, Animate);
                }

                Parent.IsTodayClick = true;
                DateTime tempValue = GenerateTodayVal(Parent.Value);
                await Parent.UpdateCalendarProperty(VALUE, GenericValue(tempValue));
                if (MultiSelection)
                {
                    List<DateTime> copyValues = Parent.CopyValues(MultiValues).ToList();
                    if (!Parent.CheckPresentDate(tempValue, MultiValues))
                    {
                        copyValues.Add(tempValue);
                        MultiValues = copyValues.ToArray();
                        await Parent.UpdateCalendarProperty(CALENDAR_BASE_VALUES, copyValues.ToArray());
                    }
                }

                await BaseTodayButtonClick(tempValue, args);
            }
        }

        private async Task BaseTodayButtonClick(DateTime dateValue, MouseEventArgs args = null)
        {
            IsSelect = false;
            TValue dateVal = GenericValue(dateValue);
            if (GetViewNumber(Parent.Start.ToString()) >= GetViewNumber(Parent.Depth.ToString()))
            {
                await NavigateTo(Parent.Depth, dateVal, args);
            }
            else
            {
                await NavigateTo(Parent.Depth, dateVal, args);
            }
        }

        /// <summary>
        /// This method is used to navigate to the month/year/decade view of the Calendar.
        /// </summary>
        internal async Task NavigateTo(CalendarView view, TValue dateValue, MouseEventArgs args = null)
        {
            DateTime date = ConvertDate(dateValue);
            var depth = Parent.Depth.ToString();
            var calView = view.ToString();
            await MinMaxUpdate(Parent.Value);
            if (date >= Parent.Min && date <= Parent.Max)
            {
                CurrentDate = date;
            }

            CurrentDate = (date <= Parent.Min) ? Parent.Min : (date > Parent.Max) ? Parent.Max : CurrentDate;
            if (GetViewNumber(depth) >= GetViewNumber(calView))
            {
                if ((GetViewNumber(depth) <= GetViewNumber(Parent.Start.ToString()))
                    || GetViewNumber(depth) == GetViewNumber(calView))
                {
                    view = Parent.Depth;
                }
            }

            SwitchView(GetViewNumber(calView), args);
        }

        private DateTime GenerateTodayVal(TValue dateValue)
        {
            DateTime changeValue = (dateValue != null) ? ConvertDate(dateValue) : DateTime.Now;
            if (Parent.Value != null && !SfBaseUtils.Equals(Parent.Value, default(TValue)))
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, changeValue.Hour, changeValue.Minute, changeValue.Second, changeValue.Millisecond);
            }
            else
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
            }
        }

        private int CompareMonth(DateTime start, DateTime end)
        {
            var getStartVal = (start.Month == end.Month) ? 0 : (start.Month > end.Month) ? 1 : -1;
            return (start.Year > end.Year) ? 1 : (start.Year < end.Year) ? -1 : getStartVal;
        }

        private int CompareDateVal(DateTime startDate, DateTime endDate, int modifier)
#pragma warning restore CA1822 // Mark members as static
        {
            int start = endDate.Year;
            int end = start;
            if (modifier > 0)
            {
                start = start - (start % modifier);
                end = start - (start % modifier) + modifier - 1;
            }

            return (startDate.Year > end) ? 1 : (startDate.Year < start) ? -1 : 0;
        }

        /// <exclude/>
        internal async Task KeyActionHandler(KeyActions args)
        {
            if (Enabled)
            {
            IsSelect = false;
            IsKeyboardSelect = true;
            int view = GetViewNumber(CurrentView());
            int depthValue = GetViewNumber(Parent.Depth.ToString());
            bool levelRestrict = view == depthValue && GetViewNumber(Parent.Start.ToString()) >= depthValue;
            var eventArgs = new CellDetails() { CellID = args.ID, ClassList = args.ClassList, CurrentDate = CurrentDate, Element = null, EventArgs = args.Events };
            switch (args.Action)
            {
                case MOVE_LEFT:
                case MOVE_RIGHT:
                    var mouseNavValue = args.Action == MOVE_LEFT ? -1 : 1;
                    KeyboardNavigate(mouseNavValue, view);
                    break;
                case MOVE_UP:
                case MOVE_DOWN:
                    var cellRow = args.Action == MOVE_UP ? -CELL_ROW : CELL_ROW;
                    var weekNumber = args.Action == MOVE_UP ? -WEEK_NUMBER : WEEK_NUMBER;
                    KeyboardNavigate((view == 0) ? weekNumber : cellRow, view);
                    break;
                case SELECT:
                    await SelectKeyAction(args, eventArgs, levelRestrict, view);
                    break;
                case CONTROL_UP:
                    NavigateTitle();
                    break;
                case CONTROL_DOWN:
                    await ControlDownKeyAction(args, levelRestrict, eventArgs, view);
                    break;
                case HOME:
                    HomeKeyAction(view);
                    break;
                case END:
                    EndKeyAction(view);
                    break;
                case PAGE_UP:
                case PAGE_DOWN:
                    await PageKeyAction(args.Action);
                    break;
                case SHIFT_PAGE_UP:
                case SHIFT_PAGE_DOWN:
                    var shiftPageValue = args.Action == SHIFT_PAGE_UP ? -1 : 1;
                    AddYears(CurrentDate, shiftPageValue);
                    await NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType));
                    break;
                case CONTROL_HOME:
                case CONTROL_END:
                    var homeEndDate = args.Action == CONTROL_HOME ? new DateTime(CurrentDate.Year, 1, 1) : new DateTime(CurrentDate.Year, YEAR_NUMBER, 31);
                    await NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(homeEndDate, PropertyType));
                    break;
            }
            }
        }

        private async Task ControlDownKeyAction(KeyActions args, bool levelRestrict, CellDetails eventArgs, int view)
        {
            if (args.FocusedDate != null || (args.SelectDate != null && !levelRestrict))
            {
                await ContentClick(eventArgs, --view);
                StateHasChanged();
            }
        }

        private void HomeKeyAction(int view)
        {
            int localYr = CurrentDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - (localYr % 10);
            var homeDatetime = view == 1 ? new DateTime(CurrentDate.Year, 1, 1) : view == 2 ? new DateTime(startYr, 1, 1) : new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            CurrentDate = homeDatetime;
            SwitchView(view);
        }
        private void EndKeyAction(int view)
        {
            DateTime firstDayOfNextMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1).AddMonths(1);
            DateTime lastDayThisMonth = firstDayOfNextMonth.AddDays(-1);
            int localEndYear = CurrentDate.Year;
            localEndYear = localEndYear < 10 ? 10 : localEndYear;
            int startYear = localEndYear - (localEndYear % 10);
            int endYr = startYear + (10 - 1);
            var yearView = view == 1 ? new DateTime(CurrentDate.Year, 12, 1) : view == 2 ? new DateTime(endYr, 1, 1) : lastDayThisMonth;
            CurrentDate = yearView;
            SwitchView(view);
        }
        private async Task SelectKeyAction(KeyActions args, CellDetails eventArgs, bool levelRestrict, int view)
        {
            if (args.TargetClassList != null && args.TargetClassList.Contains(TODAY, StringComparison.Ordinal) && args.TargetClassList.Contains(BTN, StringComparison.Ordinal))
            {
                await TodayButtonClick(new MouseEventArgs());
            }
            else
            {
                if (args.ClassList != null && !args.ClassList.Contains(DISABLED, StringComparison.Ordinal))
                {
                    if (levelRestrict)
                    {
                        await SelectDate(args.Events, args.ID, MultiSelection, MultiValues);
                    }
                    else
                    {
                        await ContentClick(eventArgs, --view);
                    }

                    StateHasChanged();
                }
            }
        }

        private async Task PageKeyAction(string action)
        {
            if (Parent.Start == CalendarView.Year && Parent.Depth == CalendarView.Year)
            {
                var dayNavValue = action == PAGE_DOWN ? 1 : -1;
                AddMonths(CurrentDate, dayNavValue);
                await NavigateTo(CalendarView.Year, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType));
            }
            else if (Parent.Start == CalendarView.Decade && Parent.Depth == CalendarView.Decade)
            {
                var monthNavValue = action == PAGE_DOWN ? 10 : 10;
                AddYears(CurrentDate, monthNavValue);
                await NavigateTo(CalendarView.Decade, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType));
            }
            else
            {
                var yearNavValue = action == PAGE_DOWN ? 1 : -1;
                AddMonths(CurrentDate, yearNavValue);
                await NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(CurrentDate, PropertyType));
            }
        }

        private void UpdateKeyNavigation(DateTime date)
        {
            if (IsMonthYearRange(date))
            {
                IsNavigation = true;
                StateHasChanged();
            }
            else
            {
                CurrentDate = date;
            }
        }

        internal void SetFocusToday(bool isFocus = false)
        {
            IsFocusTodayCell = isFocus;
        }
        internal void KeyboardNavigate(int number, int currentView)
        {
            DateTime date = CurrentDate;
            switch (currentView)
            {
                case DECADE_VIEW_VAL:
                    AddYears(CurrentDate, number);
                    HeaderTitle = StartHeadYr(CurrentDate) + TITLE_SEPARATOR + EndHeadYr(CurrentDate);
                    UpdateKeyNavigation(CurrentDate);
                    break;
                case YEAR_VIEW_VAL:
                    AddMonths(CurrentDate, number);
                    TitleUpdate(CurrentDate, CurrentView());
                    UpdateKeyNavigation(CurrentDate);
                    break;
                case MONTH_VIEW_VAL:
                    var addDate = date.AddDays(number);
                    CurrentDate = (addDate.Date >= Parent.Min.Date && addDate.Date <= Parent.Max.Date) ? addDate : date;
                    if (date.Date >= Parent.Min.Date && date.Date <= Parent.Max.Date)
                    {
                        SwitchView(0);
                    }

                    break;
            }
        }

        private bool IsMonthYearRange(DateTime date)
        {
            return (date.Month >= Parent.Min.Month || date.Year >= Parent.Min.Year) && (date.Month <= Parent.Max.Month || date.Year <= Parent.Max.Year);
        }
    }
}