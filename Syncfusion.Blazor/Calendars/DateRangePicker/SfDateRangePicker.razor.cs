using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Inputs.Internal;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DateRangePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDateRangePicker<TValue> : CalendarBase<TValue>, IDateRangePicker
    {
        internal DateRangePickerEvents<TValue> DaterangepickerEvents { get; set; }

        internal bool ClearBtnStopPropagation { get; set; }

        private bool IsDevice { get; set; }

        private bool PreventOpen { get; set; }

        private string DateRangeIcon { get; set; }

        private string CalendarClass { get; set; }

        internal ElementReference PopupElement { get; set; }

        internal ElementReference PopupHolderEle { get; set; }

        private string ModelStartValue { get; set; }

        private string ModelEndValue { get; set; }

        private string ModelDaySpanValue { get; set; }

        private string PopupRootClass { get; set; }

        private string StartBtnClass { get; set; }

        private string EndBtnClass { get; set; }

        private bool IsCalendarRendered { get; set; }

        private DatePickerPopupArgs PopupEventArgs { get; set; }

        private ChangedEventArgs<TValue[]> ChangedEventArgs { get; set; }

        private bool ShowPopupCalendar { get; set; }

        private bool ShowPresets { get; set; }

        private bool IsDateRangeIconClicked { get; set; }

        private string PreviousElementValue { get; set; }

        private bool IsCalendarRender { get; set; }

        private bool IsPresetRender { get; set; }

        private string PopupContainer { get; set; }

        private CultureInfo CurrentCulture { get; set; }

        private TValue StartValue { get; set; }

        private TValue EndValue { get; set; }

        private TValue PreviousStartValue { get; set; }

        private TValue PreviousEndValue { get; set; }

        private bool ApplyDisable { get; set; }

        private TValue StartCurrentDate { get; set; }

        private TValue EndCurrentDate { get; set; }

        private string LeftPrevIcon { get; set; }

        private string LeftNextIcon { get; set; }

        private string RightPrevIcon { get; set; }

        private string RightNextIcon { get; set; }

        private bool IsCustomPopup { get; set; } = true;

        private CalendarBaseRender<TValue> DeviceCalendar { get; set; }

        private CalendarBaseRender<TValue> LeftCalendarBase { get; set; }

        private CalendarBaseRender<TValue> CalendarInstance { get; set; }

        private CalendarBaseRender<TValue> RightCalendarBase { get; set; }

        private DateTime CurrentDate { get; set; }

        private DateTime LeftCalCurrentDate { get; set; }

        private DateTime RightCalCurrentDate { get; set; }

        private bool RightCalFocusToday { get; set; }

        private string ApplyBtnText { get; set; }

        private string CancelBtnText { get; set; }

        private bool LeftCalFocusToday { get; set; } = true;

        private bool LeftCalFocus { get; set; } = true;

        private bool RightCalFocus { get; set; }

        private bool PresetFocus { get; set; }

        private string ValidClass { get; set; }

        private List<PresetsOptions> PresetsItem { get; set; }

        private string FormatDateValue { get; set; }
        protected override string RootClass { get; set; } = ROOT;
        /// <summary>
        /// Invoke state change of the component.
        /// </summary>
        /// <exclude/>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CallStateHasChangedAsync() => await InvokeAsync(StateHasChanged);

        /// <summary>
        /// Update the children properties.
        /// </summary>
        /// <param name="presetValue">Specifies the preset values.</param>
        public void UpdateChildProperties(object presetValue)
        {
            if (presetValue != null)
            {
                var presetVal = (List<DateRangePickerPreset>)presetValue;
                List<Presets> presetItems = new List<Presets>();
                foreach (var item in presetVal)
                {
                    var presetItem = new Presets() { End = item.End, Label = item.Label, Start = item.Start };
                    presetItems.Add(presetItem);
                }

                Presets = presetItems;
            }
        }

        /// <summary>
        /// Update the header properties.
        /// </summary>
        public void UpdateHeaders()
        {
            var isStartValue = (StartValue != null);
            var isEndValue = (EndValue != null);
            int range = 0;
            if (isStartValue && isEndValue)
            {
                range = (int)Math.Round(Math.Abs((decimal)(ConvertDateValue(StartValue) - ConvertDateValue(EndValue)).TotalMilliseconds / (1000 * 60 * 60 * 24))) + 1;
            }
            var startDateText = Localizer.GetText(START_LABEL) != null ? Localizer.GetText(START_LABEL) : START_DATE_VALUE;
            var endDateText = Localizer.GetText(END_LABEL) != null ? Localizer.GetText(END_LABEL) : END_DATE_VALUE;
            var selectDaysText = Localizer.GetText(SELECTED_DAYS_LABEL) != null ? Localizer.GetText(SELECTED_DAYS_LABEL) : SELECTED_DAYS_VALUE;
            var rangeDaysText = Localizer.GetText(RANGE_DAYS_LABEL) != null ? Localizer.GetText(RANGE_DAYS_LABEL) : RANGE_DAYS_VALUE;
            ModelStartValue = isStartValue ? Intl.GetDateFormat(StartValue, HEADER_FORMAT, CalendarLocale) : startDateText;
            ModelEndValue = isEndValue ? Intl.GetDateFormat(EndValue, HEADER_FORMAT, CalendarLocale) : endDateText;
            ModelDaySpanValue = isStartValue && isEndValue ? range.ToString(CurrentCulture) + SPACE + rangeDaysText : selectDaysText;
        }

        internal override void ComponentDispose()
        {
            var options = new DateRangePickerClientProps<TValue>
            {
                EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ZIndex = ZIndex,
                Presets = Presets,
                IsCustomWindow = IsCustomPopup,
            };
            var destroyArgs = new object[] { InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, options };
            InvokeMethod("sfBlazor.DateRangePicker.destroy", destroyArgs).ContinueWith(t =>
            {
                  _ = SfBaseUtils.InvokeEvent<object>(DaterangepickerEvents?.Destroyed, null);
            }, TaskScheduler.Current);
            DateRangeIcon = null;
            PopupEventArgs = null;
            DaterangepickerEvents = null;
            PresetsItem = null;
        }

        internal async Task InputHandler(KeyActions args)
        {
            switch (args.Action)
            {
                case ALT_DOWN_ARROW:
                    await Show();
                    break;
                case ENTER:
                    await InputValueUpdate();
                    break;
                case ESCAPE:
                    await Hide();
                    break;
            }
            await SfBaseUtils.InvokeEvent<ChangeEventArgs>(OnInput, null);
        }

        internal async Task SetLocalStorage(string persistId, TValue startDate, TValue endDate)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId + "_startDate", startDate});
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId + "_endDate", endDate });
        }

        private async Task SelectDateRangeValue(KeyActions args, bool levelRestrict, CellDetails eventArgs, int view)
        {
            if (!string.IsNullOrEmpty(args.TargetClassList) && args.TargetClassList.Contains(APPLY, StringComparison.Ordinal))
            {
                await ApplyFunction();
            }
            else if (!string.IsNullOrEmpty(args.TargetClassList) && args.TargetClassList.Contains(CANCEL, StringComparison.Ordinal))
            {
                await CancelFunction();
            }
            else if (args.ClassList != null && !args.ClassList.Contains(DISABLED, StringComparison.Ordinal))
            {
                if (levelRestrict)
                {
                    var cellData = new CellDetails() { CellID = args.ID };
                    await SelectRange(cellData);
                }
                else
                {
                    await CalendarInstance.ContentClick(eventArgs, --view);
                }

                StateHasChanged();
            }
        }
        private void UpdateCurrentDate(KeyActions args)
        {
            if (args.FocusedDate == null)
            {
                if (EndValue != null)
                {
                    CurrentDate = ConvertDateValue(EndValue);
                }
                else if (StartValue != null)
                {
                    CurrentDate = ConvertDateValue(StartValue);
                }
            }
        }
        internal async Task InputKeyActionHandler(KeyActions args)
        {
            var isLeftCalendar = args.IsLeftCalendar;
            CalendarInstance = isLeftCalendar ? LeftCalendarBase : RightCalendarBase;
            int view = CalendarInstance.GetViewNumber(CalendarInstance.CurrentView());
            var rightDate = ConvertDateValue(EndCurrentDate);
            var leftDate = ConvertDateValue(StartCurrentDate);
            var rightDateLimit = new DateTime(rightDate.Year, rightDate.Month, 1);
            var lastDay = DateTime.DaysInMonth(leftDate.Year, leftDate.Month);
            var leftDateLimit = new DateTime(leftDate.Year, leftDate.Month, lastDay);
            CurrentDate = CalendarInstance.CurrentDate;
            int depthValue = CalendarInstance.GetViewNumber(Depth.ToString());
            bool levelRestrict = view == depthValue && CalendarInstance.GetViewNumber(Start.ToString()) >= depthValue;
            var eventArgs = new CellDetails() { CellID = args.ID, ClassList = args.ClassList, CurrentDate = CurrentDate, Element = null, EventArgs = args.Events };
            var propertyType = typeof(TValue);
            UpdateCurrentDate(args);

            switch (args.Action)
            {
                case ALT_UP_ARROW:
                case ESCAPE:
                    await Hide();
                    await FocusIn();
                    break;
                case TAB:
                    await Hide();
                    break;
                case SELECT:
                    await SelectDateRangeValue(args, levelRestrict, eventArgs, view);
                    break;
                case CONTROL_UP:
                    CalendarInstance.NavigateTitle();
                    break;
                case CONTROL_DOWN:
                    if (args.FocusedDate != null || (args.SelectDate != null && !levelRestrict))
                    {
                        await CalendarInstance.ContentClick(eventArgs, --view);
                        StateHasChanged();
                    }

                    break;
                case HOME:
                    HomeKeyAction(view);
                    break;
                case END:
                    EndKeyAction(view);
                    break;
                case PAGE_UP:
                    await PageUpKeyAction(propertyType);
                    break;
                case PAGE_DOWN:
                    await PageDownKeyAction(propertyType);
                    break;
                case SHIFT_PAGE_UP:
                    CalendarInstance.AddYears(CalendarInstance.CurrentDate, -1);
                    await NavigateMonth(propertyType);
                    break;
                case SHIFT_PAGE_DOWN:
                    CalendarInstance.AddYears(CalendarInstance.CurrentDate, 1);
                    await NavigateMonth(propertyType);
                    break;
                case CONTROL_HOME:
                    await CalendarInstance.NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(new DateTime(CalendarInstance.CurrentDate.Year, 1, 1), propertyType));
                    break;
                case CONTROL_END:
                    await CalendarInstance.NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(new DateTime(CalendarInstance.CurrentDate.Year, YEAR_NUMBER, 31), propertyType));
                    break;
                default:
                   await NavInCalendar(args, isLeftCalendar, leftDateLimit, rightDateLimit, view);
                    break;
            }
        }
        private void HomeKeyAction(int view)
        {
            int localYr = CalendarInstance.CurrentDate.Year;
            localYr = localYr < 10 ? 10 : localYr;
            int startYr = localYr - (localYr % 10);
            var homeDatetime = view == 1 ? new DateTime(CalendarInstance.CurrentDate.Year, 1, 1) : view == 2 ? new DateTime(startYr, 1, 1) : new DateTime(CalendarInstance.CurrentDate.Year, CalendarInstance.CurrentDate.Month, 1);
            CalendarInstance.CurrentDate = homeDatetime;
            CalendarInstance.SwitchView(view);
        }
        private void EndKeyAction(int view)
        {
            DateTime firstDayOfNextMonth = new DateTime(CalendarInstance.CurrentDate.Year, CalendarInstance.CurrentDate.Month, 1).AddMonths(1);
            DateTime lastDayThisMonth = firstDayOfNextMonth.AddDays(-1);
            int localEndYear = CalendarInstance.CurrentDate.Year;
            localEndYear = localEndYear < 10 ? 10 : localEndYear;
            int startYear = localEndYear - (localEndYear % 10);
            int endYr = startYear + (10 - 1);
            var yearView = view == 1 ? new DateTime(CalendarInstance.CurrentDate.Year, 12, 1) : view == 2 ? new DateTime(endYr, 1, 1) : lastDayThisMonth;
            CalendarInstance.CurrentDate = yearView;
            CalendarInstance.SwitchView(view);
        }
        private async Task PageDownKeyAction(Type propertyType)
        {
            if (CompareView(Start, Depth) == "Year")
            {
                CalendarInstance.AddYears(CalendarInstance.CurrentDate, 1);
                await CalendarInstance.NavigateTo(CalendarView.Year, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
            }
            else if (CompareView(Start, Depth) == "Decade")
            {
                CalendarInstance.AddYears(CalendarInstance.CurrentDate, 10);
                await CalendarInstance.NavigateTo(CalendarView.Decade, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
            }
            else
            {
                CalendarInstance.AddMonths(CurrentDate, 1);
                await CalendarInstance.NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
            }
        }
        private async Task PageUpKeyAction(Type propertyType)
        {
            if (CompareView(Start, Depth) == "Year")
            {
                CalendarInstance.AddYears(CalendarInstance.CurrentDate, -1);
                await CalendarInstance.NavigateTo(CalendarView.Year, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
            }
            else if (CompareView(Start, Depth) == "Decade")
            {
                CalendarInstance.AddYears(CalendarInstance.CurrentDate, -10);
                await CalendarInstance.NavigateTo(CalendarView.Decade, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
            }
            else
            {
                CalendarInstance.AddMonths(CalendarInstance.CurrentDate, -1);
                await CalendarInstance.NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
            }
        }

        internal override void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
            _ = SfBaseUtils.InvokeEvent<NavigatedEventArgs>(DaterangepickerEvents?.Navigated, eventArgs);
        }
#pragma warning disable CA1822 // Mark members as static
        private string CompareView(CalendarView start, CalendarView depth)
#pragma warning restore CA1822 // Mark members as static
        {
            if (start == CalendarView.Year && depth == CalendarView.Year)
                return "Year";
            else if (start == CalendarView.Decade && depth == CalendarView.Decade)
                return "Decade";
            else if (start == CalendarView.Month && depth == CalendarView.Month)
                return "Month";
            else
                return "NotMatch";
        }
        private bool CheckDateValue(TValue date, DateTime eventDate)
        {
            if (CompareView(this.Start, this.Depth) == "Year")
                return (ConvertDateValue(date).Month == eventDate.Month) && (ConvertDateValue(date).Year == eventDate.Year);
            else if (CompareView(this.Start, this.Depth) == "Decade")
                return (ConvertDateValue(date).Year == eventDate.Year);
            else
                return (eventDate == ConvertDateValue(date).Date);
        }
        private RenderDayCellEventArgs InvokeRenderDayEvent(RenderDayCellEventArgs eventArgs)
        {
            var itemData = eventArgs.CellData;
            CellListData = CellListData != null ? CellListData : new List<CellDetails>();
            if (!SfBaseUtils.Equals(StartValue, EndValue))
            {
                itemData.ClassList = SfBaseUtils.AddClass(itemData.ClassList, RANGE_HOVER);
            }

            var isHoverList = CellListData.Where(item => item.CellID == itemData.CellID)?.FirstOrDefault();
            if (CellListData.Count > 0 && CellListData[0].ClassList.Contains(START_DATE, StringComparison.Ordinal) && !CellListData[0].ClassList.Contains(DATE_DISABLED, StringComparison.Ordinal) && !SfBaseUtils.Equals(StartValue, EndValue))
            {
                CellListData[0].ClassList = SfBaseUtils.AddClass(CellListData[0].ClassList, RANGE_HOVER);
            }

            if (isHoverList == null)
            {
                var isStartDate = this.CheckDateValue(StartValue, eventArgs.Date.Date);
                var lastDayOfMonth = DateTime.DaysInMonth(eventArgs.Date.Year, eventArgs.Date.Month);
                var isEndDate = this.CheckDateValue(EndValue, eventArgs.Date.Date);
                if (isStartDate || isEndDate)
                {
                    if (Start == CalendarView.Month)
                    {
                        itemData.ClassList = SfBaseUtils.RemoveClass(itemData.ClassList, RANGE_HOVER);
                    }

                    itemData.ClassList = SfBaseUtils.AddClass(itemData.ClassList, SELECTED);
                    itemData.ClassList = SfBaseUtils.AddClass(itemData.ClassList, isStartDate ? START_DATE : END_DATE);
                }

                CellListData.Add(itemData);
                StateHasChanged();
            }
            return eventArgs;
        }
        internal override async Task BindRenderDayEvent(RenderDayCellEventArgs eventArgs)
        {
            if (StartValue != null && EndValue != null)
            {
                var endDateValue = Start == CalendarView.Year ? new DateTime(ConvertDateValue(EndValue).Year, ConvertDateValue(EndValue).Month, 1)
                    : Start == CalendarView.Decade ? new DateTime(ConvertDateValue(EndValue).Year, 1, 1) : ConvertDateValue(EndValue);
                var isValidMinMaxRange = !(ConvertDateValue(StartValue).Ticks < this.Min.Ticks) && !(ConvertDateValue(StartValue).Ticks > this.Max.Ticks) && !(endDateValue.Ticks < this.Min.Ticks) && !(endDateValue.Ticks > this.Max.Ticks);
                var isValidDateRange = (ConvertDateValue(StartValue).Ticks <= eventArgs.Date.Ticks) && (ConvertDateValue(EndValue).Ticks >= eventArgs.Date.Ticks);
                if (isValidMinMaxRange && (CompareView(this.Start, this.Depth) == "Decade" ? (ConvertDateValue(StartValue).Year <= eventArgs.Date.Year && endDateValue.Year >= eventArgs.Date.Year) : isValidDateRange) && (!IsDevice && ((this.Start.ToString() == LeftCalendarBase.CurrentView() && this.Start.ToString() == RightCalendarBase.CurrentView())) || this.Start.ToString() == DeviceCalendar?.CurrentView()))
                {
                    eventArgs = InvokeRenderDayEvent(eventArgs);
                }
            }

            await SfBaseUtils.InvokeEvent<RenderDayCellEventArgs>(DaterangepickerEvents?.OnRenderDayCell, eventArgs);
        }

        internal async Task ClosePopupAction()
        {
            if (IsDateRangeIconClicked)
            {
                await FocusIn();
            }

            IsCalendarRender = false;
            SetPopupVisibility(false);
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, inputAttr);
            StateHasChanged();
            await Task.CompletedTask;
        }

        internal override async Task OnAfterScriptRendered()
        {
            var options = new DateRangePickerClientProps<TValue>
            {
                EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ZIndex = ZIndex,
                Presets = Presets,
            };
            await InvokeMethod("sfBlazor.TextBox.initialize", new object[] { InputElement, DotnetObjectReference, ContainerElement });
            await InvokeMethod("sfBlazor.DateRangePicker.initialize", new object[] { ContainerElement, InputElement, DotnetObjectReference, options });
            IsDevice = SyncfusionService.IsDeviceMode;
        }

        internal override async Task HoverSelection(CellDetails args)
        {
            var hoverCellDetails = args;
            var currentDate = GetIdValue(hoverCellDetails);
            if (StartValue != null && ConvertDateValue(StartValue).Ticks >= Min.Ticks && ConvertDateValue(StartValue).Ticks <= Max.Ticks)
            {
                if (!(EndValue != null && !SfBaseUtils.Equals(EndValue, default)))
                {
                    var tdData = new List<CellDetails>(CellDetailsData);
                    foreach (var hoverDate in tdData)
                    {
                        var isDisabledCell = !hoverDate.ClassList.Contains(DISABLED, StringComparison.Ordinal) || hoverDate.ClassList.Contains(DATE_DISABLED, StringComparison.Ordinal);
                        if (!args.ClassList.Contains(WEEKNUMBER, StringComparison.Ordinal) && isDisabledCell)
                        {
                            var startDateVal = ConvertDateValue(StartValue);
                            if (GetIdValue(hoverDate).Date >= startDateVal.Date && GetIdValue(hoverDate).Ticks <= currentDate.Ticks)
                            {
                                var itemData = hoverDate;
                                if (MinDays != null && MinDays > 0)
                                {
                                    itemData.ClassList = !hoverCellDetails.ClassList.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.AddClass(itemData.ClassList, RANGE_HOVER) : itemData.ClassList;
                                }
                                else if (!itemData.ClassList.Contains(DISABLED, StringComparison.Ordinal) && !SfBaseUtils.Equals(StartValue, EndValue))
                                {
                                    itemData.ClassList = SfBaseUtils.AddClass(itemData.ClassList, RANGE_HOVER);
                                }

                                var isHoverList = CellListData.Where(item => item.CellID == itemData.CellID)?.FirstOrDefault();
                                if (!CellListData[0].ClassList.Contains(DISABLED, StringComparison.Ordinal) && !SfBaseUtils.Equals(StartValue, EndValue))
                                {
                                    CellListData[0].ClassList = SfBaseUtils.AddClass(CellListData[0].ClassList, RANGE_HOVER);
                                }

                                if (isHoverList == null)
                                {
                                    CellListData.Add(itemData);
                                }
                            }
                            else if (hoverDate.ClassList.Contains(RANGE_HOVER, StringComparison.Ordinal))
                            {
                                hoverDate.ClassList = SfBaseUtils.RemoveClass(hoverDate.ClassList, RANGE_HOVER);
                                CellListData.Remove(hoverDate);
                            }

                            StateHasChanged();
                        }
                    }
                }
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await PropertyParametersSet();
            SetRTL();
            SetDayHeaderFormat();
            SetPresetHeight();
            //if (!EnablePersistence)
            //{
            //    await SetSeparatorFormat();
            //}
            SetAllowEdit();
            UpdateAriaAttributes();
            if (PropertyChanges.Count > 0)
            {
                await OnPropertyChange(PropertyChanges);
            }
            SetCssClass();
            UpdateErrorClass();
            UpdateValidateClass();
        }
        private async Task OnPropertyChange(Dictionary<string, object> newProps)
        {
            var newProperties = newProps.ToList();
            foreach (var prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(StartDate):
                        PreviousStartValue = PrevChanges != null ? StartDate : PreviousStartValue;
                        PropertyChanges?.Remove(nameof(StartDate));
                        if (StartDate == null)
                        {
                            StartValue = StartDate;
                            await UpdateInputValue(null);
                        }
                        await UpdateInput();
                        break;
                    case nameof(EndDate):
                        PreviousEndValue = PrevChanges != null ? EndDate : PreviousEndValue;
                        PropertyChanges?.Remove(nameof(EndDate));
                        if (EndDate == null)
                        {
                            EndValue = EndDate;
                            await UpdateInputValue(null);
                        }
                        await UpdateInput();
                        break;
                    case nameof(Format):
                    case nameof(Separator):
                        if (StartDate != null && EndDate != null)
                        {
                            var tempStartDate = Intl.GetDateFormat(startDate, GetDefaultFormat(), CalendarLocale);
                            var tempEndDate = Intl.GetDateFormat(endDate, GetDefaultFormat(), CalendarLocale);
                            await UpdateInputValue(tempStartDate + " " + Separator + " " + tempEndDate);
                        }
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRendered();
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, cssClass);
                        PopupRootClass = string.IsNullOrEmpty(PopupRootClass) ? PopupRootClass : SfBaseUtils.RemoveClass(PopupRootClass, cssClass);
                        cssClass = CssClass;
                        break;
                    case nameof(Presets):
                        Presets = (List<Presets>)PropertyChanges[PRESETS];
                        ProcessPresets();
                        break;
                    case nameof(MinDays):
                    case nameof(MaxDays):
                        CheckMinMaxDays();
                        break;
                }
            }
        }
        /// <summary>
        /// Triggers when the preset items get clicked.
        /// </summary>
        /// <param name="args"><see cref="PresetsOptions"/> arguments.</param>
        /// <returns>Task.</returns>
        protected async Task OnPresetItemClick(PresetsOptions args)
        {
            if (args != null)
            {
                var activeItem = PresetsItem.Where(item => item.ListClass.Contains(ACTIVE, StringComparison.Ordinal)).FirstOrDefault();
                if (activeItem != null)
                {
                    activeItem.ListClass = SfBaseUtils.RemoveClass(activeItem.ListClass, ACTIVE);
                }

                args.ListClass = SfBaseUtils.AddClass(args.ListClass, ACTIVE);
                if (!args.IsCustomRange)
                {
                    RemoveSelection();
                    StartValue = ConvertGeneric(args.Start);
                    EndValue = ConvertGeneric(args.End);
                    StartDate = ConvertGeneric(args.Start);
                    EndDate = ConvertGeneric(args.End);
                    var formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortDatePattern;
                    var dateText = Intl.GetDateFormat(StartValue, formatString, CalendarLocale).ToString() + SPACE + Separator + SPACE + Intl.GetDateFormat(EndValue, formatString, CalendarLocale).ToString();
                    await UpdateInputValue(dateText);
                    await ChangeTrigger();
                    await Hide();
                    await FocusIn();
                    IsCustomPopup = false;
                }
                else
                {
                    IsCustomPopup = true;
                    PresetFocus = false;
                    LeftCalFocus = true;
                    SetPresetHeight();
                    await Task.Delay(1);
                    await InvokeMethod("sfBlazor.DateRangePicker.refreshPopup", new object[] { InputElement });
                }
            }
        }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("daterangepicker");
            }

            PropertyInit();
            await base.OnInitializedAsync();
            DependencyScripts();
            if (DateRangePickerParent != null && Convert.ToString(DateRangePickerParent.Type, CultureInfo.CurrentCulture) == "DateRange")
            {
                DateRangePickerParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(DateRangePickerParent, this);
            }

            PropertyInitialized();
            if (!EnablePersistence)
            {
                await UpdateInput();
            }
            LeftPrevIcon = RightPrevIcon = PREV_ICON;
            LeftNextIcon = RightNextIcon = NEXT_ICON;
            StartValue = StartDate;
            EndValue = EndDate;
            cssClass = CssClass;
            PreviousStartValue = StartValue;
            PreviousEndValue = EndValue;
            ApplyBtnText = Localizer.GetText(APPLY_LOCALE_KEY) != null ? Localizer.GetText(APPLY_LOCALE_KEY) : APPLY_BTN_TXT;
            CancelBtnText = Localizer.GetText(CANCEL_LOCALE_KEY) != null ? Localizer.GetText(CANCEL_LOCALE_KEY) : CANCEL_BTN_TXT;
        }

        /// <summary>
        /// Triggers after the component was rendered.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the first time.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await ClientPopupRender();
            if (firstRender)
            {
                if (EnablePersistence)
                {
                    var localStorageStartValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID + "_startDate" });
                    localStorageStartValue = (localStorageStartValue != "null") ? localStorageStartValue : null;
                    var localStorageEndValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID + "_endDate" });
                    localStorageEndValue = (localStorageEndValue != "null") ? localStorageEndValue : null;
                    var localStorageValue = (localStorageStartValue != null && localStorageEndValue != null) ? localStorageStartValue + " " + Separator + " " + localStorageEndValue : null;
                    if (localStorageValue != null && localStorageValue != Separator)
                    {
                        var presetvalue = localStorageValue.Split(" "+Separator+" ");
                        StartValue = (TValue)SfBaseUtils.ChangeType(presetvalue[0], typeof(TValue));
                        EndValue = (TValue)SfBaseUtils.ChangeType(presetvalue[1], typeof(TValue));
                        await UpdateValue();
                    }
                }
                if (StartDate != null && EndDate != null)
                {
                    await UpdateInputValue(RangeArgs().Text);
                    PreviousStartValue = StartDate;
                    PreviousEndValue = EndDate;
                }
                ApplyDisable = (StartValue != null && EndValue != null) ? false : true;
                PreviousElementValue = FormatDateValue;
                await SfBaseUtils.InvokeEvent<object>(DaterangepickerEvents?.Created, null);
            }
        }

        /// <summary>
        /// Triggeres while hovering the mouse pointer on preset items.
        /// </summary>
        /// <param name="args">Specifies the PresetsOptions arguments.</param>
        /// <exclude/>
        protected void OnPresetItemMouseOver(PresetsOptions args)
        {
            if (args != null)
            {
                var activeItem = GetPresetHoverItem();
                if (activeItem != null)
                {
                    activeItem.ListClass = SfBaseUtils.RemoveClass(activeItem.ListClass, HOVER);
                }

                args.ListClass = SfBaseUtils.AddClass(args.ListClass, HOVER);
            }
        }

        /// <summary>
        /// Triggers when the mouse pointer moved out of the preset items.
        /// </summary>
        /// <exclude/>
        protected void OnPresetItemMouseOut()
        {
            var activeItem = GetPresetHoverList();
            if (activeItem != null)
            {
                foreach (var item in activeItem)
                {
                    item.ListClass = SfBaseUtils.RemoveClass(item.ListClass, HOVER);
                }
            }
        }

        private void PropertyInit()
        {
            RootClass = ROOT;
            PopupRootClass = ROOT + SPACE + POPUP;
            ContainerClass = CONTAINER_CLASS;
            DateRangeIcon = RANGE_ICON;
            PopupContainer = POPUP_CONTAINER;

            // Unique class added for dynamically rendered Inplace-editor components
            if (DateRangePickerParent != null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
                PopupRootClass = SfBaseUtils.AddClass(PopupRootClass, "e-editable-elements");
            }

            CalendarClass = CALENDAR_ROOT;
            CurrentCulture = GetDefaultCulture();
            ChangedEventArgs = new ChangedEventArgs<TValue[]>();
            IsDateRangeIconClicked = false;
        }

        private void DependencyScripts()
        {
            ScriptModules = SfScriptModules.SfDateRangePicker;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.SfTextBox);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
        }

        private async Task SetSeparatorFormat()
        {
            if (StartValue != null && EndValue != null)
            {
                var formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortDatePattern;
                string dateformat = Intl.GetDateFormat(StartValue, formatString, CalendarLocale).ToString() + SPACE + Separator + SPACE + Intl.GetDateFormat(EndValue, formatString, CalendarLocale).ToString();
                await UpdateInputValue(dateformat);
            }
        }

        private void SetPresetHeight()
        {
            PopupRootClass = (Presets.Count > 0 && IsCustomPopup) ? SfBaseUtils.AddClass(PopupRootClass, PRESET_WRAPPER) : SfBaseUtils.RemoveClass(PopupRootClass, PRESET_WRAPPER);
        }

        private void SetDayHeaderFormat()
        {
            CalendarClass = (DayHeaderFormat == DayHeaderFormats.Wide) ? SfBaseUtils.AddClass(CalendarClass, CALENDAR_DAY_HEADER_WIDE) : SfBaseUtils.RemoveClass(CalendarClass, CALENDAR_DAY_HEADER_WIDE);
            PopupRootClass = (DayHeaderFormat == DayHeaderFormats.Wide) ? SfBaseUtils.AddClass(PopupRootClass, RANGE_DAY_HEADER_WIDE) : SfBaseUtils.RemoveClass(PopupRootClass, RANGE_DAY_HEADER_WIDE);
        }

        private void SetRTL()
        {
            CalendarClass = EnableRtl || SyncfusionService.options.EnableRtl ? SfBaseUtils.AddClass(CalendarClass, RTL) : SfBaseUtils.RemoveClass(CalendarClass, RTL);
            PopupContainer = EnableRtl || SyncfusionService.options.EnableRtl ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
            PopupRootClass = EnableRtl || SyncfusionService.options.EnableRtl ? SfBaseUtils.AddClass(PopupRootClass, RTL) : SfBaseUtils.RemoveClass(PopupRootClass, RTL);
        }

        private async Task NavigateMonth(Type propertyType)
        {
            await CalendarInstance.NavigateTo(CalendarView.Month, (TValue)SfBaseUtils.ChangeType(CalendarInstance.CurrentDate, propertyType));
        }

        private async Task RemoveFocusDate(string cellId)
        {
            await InvokeMethod("sfBlazor.DateRangePicker.removeFocusDate", new object[] { InputElement, PopupElement, cellId });
        }

        private async Task MoveLeftAction(DateTime date, bool isLeftCalendar, DateTime leftLimit, int view)
        {
            date = CurrentDate;
            date = date.AddDays(-1);
            if (!isLeftCalendar && date.Ticks == leftLimit.Ticks)
            {
                await KeyCalendarUpdate(true, date);
            }

            if (!isLeftCalendar)
            {
                await KeyCalendarUpdate(false, date);
                CalendarInstance = RightCalendarBase;
            }

            CalendarInstance.KeyboardNavigate(-1, view);
        }
        private async Task MoveRightKeyAction(DateTime date, bool isLeftCalendar, DateTime rightLimit, int view)
        {
            date = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
            date = date.AddDays(1);
            if (isLeftCalendar && date.Ticks == rightLimit.Ticks)
            {
                await KeyCalendarUpdate(false, date);
            }

            if (!isLeftCalendar)
            {
                isLeftCalendar = false;
                await KeyCalendarUpdate(true, date);
                CalendarInstance = RightCalendarBase;
            }

            CalendarInstance.KeyboardNavigate(1, view);
        }
        private async Task NavInCalendar(KeyActions args, bool isLeftCalendar, DateTime leftLimit, DateTime rightLimit, int view)
        {
            CalendarInstance = isLeftCalendar ? LeftCalendarBase : RightCalendarBase;
            var date = default(DateTime);
            switch (args.Action)
            {
                case MOVE_LEFT:
                    await MoveLeftAction(date, isLeftCalendar, leftLimit, view);
                    break;
                case MOVE_RIGHT:
                    await MoveRightKeyAction(date, isLeftCalendar, rightLimit, view);
                    break;
                case ALT_RIGHT_ARROW:
                    if (LeftCalFocus)
                    {
                        LeftCalFocus = false;
                        RightCalFocus = true;
                        date = new DateTime(RightCalendarBase.CurrentDate.Year, RightCalendarBase.CurrentDate.Month, RightCalendarBase.CurrentDate.Day);
                        date = date.AddDays(1);
                        CurrentDate = date;
                        await KeyCalendarUpdate(true, date);
                        RightCalendarBase.KeyboardNavigate(1, view);
                    }
                    else if (IsPresetRender && RightCalFocus)
                    {
                        RightCalFocus = false;
                        PresetFocus = true;
                    }

                    break;
                case ALT_LEFT_ARROW:
                    if (IsPresetRender && PresetFocus)
                    {
                        PresetFocus = false;
                        RightCalFocus = true;
                    }
                    else
                    {
                        RightCalFocus = false;
                        LeftCalFocus = true;
                        date = new DateTime(LeftCalendarBase.CurrentDate.Year, LeftCalendarBase.CurrentDate.Month, LeftCalendarBase.CurrentDate.Day);
                        date = date.AddDays(1);
                        CurrentDate = date;
                        await KeyCalendarUpdate(false, date);
                        LeftCalendarBase.KeyboardNavigate(1, view);
                    }

                    break;
                case MOVE_UP:
                    if (view == 0)
                    {
                        date = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                        date = date.AddDays(-WEEK_NUMBER);
                        if (date.Ticks <= leftLimit.Ticks && !isLeftCalendar)
                        {
                            await KeyCalendarUpdate(true, date);
                        }

                        if (!isLeftCalendar)
                        {
                            await KeyCalendarUpdate(true, date);
                            CalendarInstance = RightCalendarBase;
                        }

                        CalendarInstance.KeyboardNavigate(-WEEK_NUMBER, view);
                    }
                    else
                    {
                        CalendarInstance.KeyboardNavigate(-CELL_ROW, view);
                    }

                    break;
                case MOVE_DOWN:
                    if (view == 0)
                    {
                        date = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day);
                        date = date.AddDays(WEEK_NUMBER);
                        if (isLeftCalendar && date.Ticks >= rightLimit.Ticks)
                        {
                            await KeyCalendarUpdate(false, date);
                        }

                        if (!isLeftCalendar)
                        {
                            await KeyCalendarUpdate(true, date);
                            CalendarInstance = RightCalendarBase;
                        }

                        CalendarInstance.KeyboardNavigate(WEEK_NUMBER, view);
                    }
                    else
                    {
                        CalendarInstance.KeyboardNavigate(CELL_ROW, view);
                    }

                    break;
            }
        }

        private async Task KeyCalendarUpdate(bool isLeftCalendar, DateTime date)
        {
            if (isLeftCalendar)
            {
                RightCalFocusToday = true;
                RightCalendarBase.SetFocusToday(true);
                CalendarInstance = LeftCalendarBase;
            }
            else
            {
                CalendarInstance = RightCalendarBase;
                RightCalFocusToday = true;
                CalendarInstance.SetFocusToday(true);
            }

            CalendarInstance.CurrentDate = CurrentDate;
            await RemoveFocusDate(date.Ticks.ToString(CurrentCulture));
        }
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = (ContainerClass != null && !ContainerClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(ContainerClass, CssClass) : ContainerClass;
                PopupContainer = (PopupContainer != null && !PopupContainer.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupContainer, CssClass) : PopupContainer;
                PopupRootClass = (PopupRootClass != null && !PopupRootClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupRootClass, CssClass) : PopupRootClass;
            }
        }

        private async Task KeyActionHandler(KeyActions args)
        {
            if (IsPresetRender && args != null)
            {
                switch (args.Key)
                {
                    case "ArrowDown":
                    case "Home":
                    case "End":
                    case "ArrowUp":
                        await KeyHandler(args);
                        await InvokeMethod("sfBlazor.DatePicker.updateScrollPosition", new object[] { InputElement });
                        break;
                    case "Enter":
                        var findItems = PresetsItem.Where(item => item.ListClass.Contains(HOVER, StringComparison.Ordinal)).FirstOrDefault();
                        if (findItems != null)
                        {
                            await OnPresetItemClick(findItems);
                        }

                        break;
                    case "Escape":
                        await Hide();
                        break;
                    case "ArrowLeft":
                        if (args.Action == "altLeftArrow")
                        {
                            PresetFocus = false;
                            RightCalFocus = true;
                        }

                        break;
                }

                StateHasChanged();
            }
        }

        private async Task KeyHandler(KeyActions args)
        {
            if (PresetsItem == null)
            {
                ProcessPresets();
            }

            var listCount = PresetsItem?.Count;
            int? activeIndex = null;
            var selectedData = PresetsItem.Where(listItem => listItem.ListClass.Contains(ACTIVE, StringComparison.CurrentCulture)).FirstOrDefault();
            var hoverItem = PresetsItem.Where(listItem => listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture)).FirstOrDefault();
            var findItems = hoverItem != null ? hoverItem : selectedData;
            activeIndex = findItems != null ? PresetsItem.IndexOf(findItems) : 0;
            activeIndex = (args.Key == "ArrowDown") ? ((activeIndex == (int)listCount - 1) ? (int)listCount - 1 : ++activeIndex) : (args.Key == "ArrowUp") ? ((activeIndex == 0) ? 0 : --activeIndex) : (args.Key == "Home") ? 0 : (int)listCount - 1;
            activeIndex = (activeIndex >= 0) ? activeIndex : PresetsItem.Count + activeIndex;
            var selectItem = PresetsItem.ElementAtOrDefault((int)activeIndex);
            if (selectItem != null)
            {
                if (hoverItem != null)
                {
                    hoverItem.ListClass = SfBaseUtils.RemoveClass(hoverItem.ListClass, HOVER);
                }

                selectItem.ListClass = SfBaseUtils.AddClass(selectItem.ListClass, HOVER);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Sets popup visibility based on the provided arguments.
        /// </summary>
        /// <param name="args">Boolean.</param>
        private void SetPopupVisibility(bool args)
        {
            ShowPopupCalendar = args;
        }

        /// <summary>
        /// Initialize the properties during render.
        /// </summary>
        private void PropertyInitialized()
        {
            startDate = StartDate;
            endDate = EndDate;
            format = Format;
            ShowTodayButton = false;
            separator = Separator;
        }

        /// <summary>
        /// Update the properties during parametersSet.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        private async Task PropertyParametersSet()
        {
            format = NotifyPropertyChanges(FORMAT, Format, format);
            startDate = NotifyPropertyChanges(nameof(StartDate), StartDate, startDate, true);
            endDate = NotifyPropertyChanges(nameof(EndDate), EndDate, endDate, true);
            NotifyPropertyChanges(nameof(MaxDays), MaxDays, maxDays);
            NotifyPropertyChanges(nameof(MinDays), MinDays, minDays);
            NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            separator = NotifyPropertyChanges(nameof(Separator), Separator, separator);
            await Task.CompletedTask;
        }

        private async Task UpdateValue()
        {
            var privateStart = startDate;
            StartDate = startDate = StartValue;
            StartDate = startDate = await SfBaseUtils.UpdateProperty(StartValue, privateStart, StartDateChanged, CalendarEditContext, StartDateExpression);
            var privateEnd = endDate;
            EndDate = endDate = EndValue;
            EndDate = endDate = await SfBaseUtils.UpdateProperty(EndValue, privateEnd, EndDateChanged, CalendarEditContext, EndDateExpression);
        }

        /// <summary>
        /// Method to get the default format based on culture, if none specified.
        /// </summary>
        /// <returns>Returns string format.</returns>
        private string GetDefaultFormat()
        {
            CultureInfo currentCulture = GetDefaultCulture();
            return string.IsNullOrEmpty(Format) ? currentCulture.DateTimeFormat.ShortDatePattern : Format;
        }

        /// <summary>
        /// Method to get the default cultureinfo.
        /// </summary>
        /// <returns>Cultureinfo.</returns>
        private CultureInfo GetDefaultCulture()
        {
            string locale = string.IsNullOrEmpty(CalendarLocale) ? null : CalendarLocale;
            return Intl.GetCulture(locale);
        }

        protected override async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            var focusEventArgs = new FocusEventArgs();
            await SfBaseUtils.InvokeEvent<FocusEventArgs>(DaterangepickerEvents?.Focus, focusEventArgs);
        }

        protected override async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            DateRangeIcon = SfBaseUtils.RemoveClass(DateRangeIcon, ACTIVE);
            IsDateRangeIconClicked = false;
            await InputValueUpdate(args);
            UpdateValidateClass();

            var blurEventArgs = new BlurEventArgs();
            await SfBaseUtils.InvokeEvent<BlurEventArgs>(DaterangepickerEvents?.Blur, blurEventArgs);
        }

        private void UpdateAriaAttributes()
        {
            SfBaseUtils.UpdateDictionary(ARIA_LIVE, ASSERTIVE, inputAttr);

            SfBaseUtils.UpdateDictionary(ARIA_AUTOMIC, TRUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, TRUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, NULL_VALUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + OPTIONS, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(ROLE, COMBOBOX, inputAttr);
            SfBaseUtils.UpdateDictionary(AUTO_CORRECT, OFF, inputAttr);
            SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_INVALID, FALSE, inputAttr);
        }

        /// <summary>
        /// Method to trigger the client-side actions once the popup is displayed when date icon is clicked.
        /// </summary>
        private async Task ClientPopupRender()
        {
            if (ShowPopupCalendar && IsCalendarRendered)
            {
                IsCalendarRendered = false;
                var options = new DateRangePickerClientProps<TValue>
                {
                    EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                    ZIndex = ZIndex,
                    Presets = Presets,
                    IsCustomWindow = IsCustomPopup,
                };
                await InvokeMethod("sfBlazor.DateRangePicker.renderPopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, options });
                IsCalendarRender = true;
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUT_FOCUS);
                if (PresetsItem != null && PresetsItem[0] != null && PresetsItem[0].Text != null)
                {
                    IsPresetRender = true;
                }
            }
        }

        private async Task BindClearBtnTouchEvents(EventArgs args)
        {
            await InvokeClearBtnEvent(args);
        }

        private async Task BindClearBtnEvents(EventArgs args)
        {
            await InvokeClearBtnEvent(args);
        }

        private async Task InvokeClearBtnEvent(EventArgs args)
        {
            if (!IsDevice)
            {
                ClearBtnStopPropagation = true;
            }

            await UpdateInputValue(null);
            RemoveSelection();
            var clearEventArgs = new ClearedEventArgs()
            {
                Event = args,
            };
            await FocusIn();
            await SfBaseUtils.InvokeEvent<ClearedEventArgs>(DaterangepickerEvents?.Cleared, clearEventArgs);
            await ChangeTrigger();
            await Hide();
        }

        private void UpdateValidateClass()
        {
            if (StartDateExpression != null && EndDateExpression != null && InputEditContext != null)
            {
                var fieldIdentifier = FieldIdentifier.Create(StartDateExpression);
                fieldIdentifier = FieldIdentifier.Create(EndDateExpression);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
                ValidClass = InputEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.AddClass(ContainerClass, ValidClass) : ContainerClass;
                this.ContainerClass = Regex.Replace(this.ContainerClass, @"\s+", " ");
                if (ValidClass == INVALID || (ValidClass == MODIFIED_INVALID && Value == null))
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                }
                else if (ValidClass == MODIFIED_VALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
                }
            }
        }
        protected override string FormatValueAsString(TValue formatValue)
        {
            return (StartDate != null && EndDate != null) ? RangeArgs().Text : default!;
        }
        protected override TValue FormatValue(string genericValue = null)
        {
            UpdateStartEndDate(genericValue);
            return default!;
        }
        private async Task DateRangeIconHandler()
        {
            if (Enabled)
            {
                var isFloatLabel = FloatLabelType == Inputs.FloatLabelType.Auto;
                if (isFloatLabel)
                {
                    await Task.Delay(2); // set the delay for prevent the icon click action.
                }
                if (IsDevice)
                {
                    SfBaseUtils.UpdateDictionary(READ_ONLY, string.Empty, inputAttr);
                    await FocusOut();
                }
                else
                {
                    PreventIconHandler = true;
                }

                IsDateRangeIconClicked = true;
                if (!Readonly)
                {
                    if (IsCalendarRender)
                    {
                        RemoveSelection();
                        await Hide();
                    }
                    else
                    {
                        if (isFloatLabel)
                        {
                            ContainerClass = SfBaseUtils.AddClass(ContainerClass.Trim(), INPUT_FOCUS);
                            await FocusIn();
                        }
                        await Show();
                        if (!isFloatLabel)
                        {
                            ContainerClass = SfBaseUtils.AddClass(ContainerClass.Trim(), INPUT_FOCUS);
                        }
                        if (PreventOpen)
                        {
                            await FocusIn();
                        }
                        if (DateRangeIcon != null)
                        {
                            DateRangeIcon = SfBaseUtils.AddClass(DateRangeIcon, ACTIVE);
                        }
                    }
                }
            }
        }

        private async Task UpdateInputValue(string dateValue)
        {
            FormatDateValue = dateValue;
            await SetValue(dateValue, FloatLabelType, ShowClearButton);
        }

        private async Task ChangeTrigger(EventArgs args = null)
        {
            if (CompareValue(PreviousStartValue, StartValue) != 0 || CompareValue(PreviousEndValue, EndValue) != 0)
            {
                await UpdateValue();
                await SfBaseUtils.InvokeEvent<RangePickerEventArgs<TValue>>(DaterangepickerEvents?.ValueChange, RangeArgs(args));
                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, StartValue, EndValue);
                }

                PreviousElementValue = FormatDateValue;

                // previousDate = Value;
                PreviousStartValue = StartValue;
                PreviousEndValue = EndValue;
            }
        }

        private void ValidateMinMaxDays()
        {
            if (StartValue != null && EndValue != null)
            {
                var range = (int)Math.Round(Math.Abs((decimal)(ConvertDateValue(StartValue) - ConvertDateValue(EndValue)).TotalMilliseconds / (1000 * 60 * 60 * 24))) + 1;
                if ((MinDays != null && MinDays > 0) && !(range > MinDays))
                {
                    if ((StrictMode && !(IsFocused && ValidateOnInput)))
                    {
                        var date = ConvertDateValue(StartValue);
                        date = new DateTime(date.Year, date.Month, (int)date.Day + ((int)MinDays - 1));
                        EndValue = (date.Ticks > Max.Ticks) ? ConvertGeneric(Max) : ConvertGeneric(date);
                    }
                    else
                    {
                        StartValue = default;
                        EndValue = default;
                    }
                }

                if ((MaxDays != null && MaxDays > 0) && !(range <= MaxDays))
                {
                    if ((StrictMode && !(IsFocused && ValidateOnInput)))
                    {
                        var date = ConvertDateValue(StartValue);
                        date = new DateTime(date.Year, date.Month, (int)date.Day + ((int)MaxDays - 1));
                        EndValue = (date.Ticks > Max.Ticks) ? ConvertGeneric(Max) : ConvertGeneric(date);
                    }
                    else
                    {
                        StartValue = default;
                        EndValue = default;
                    }
                }
            }
        }
        private void UpdateStartEndDate(string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                var range = inputValue.Split(" " + Separator + " ");
                if (range.Length == 2)
                {
                    var formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortDatePattern : Format;
                    if (IsTryParse(range[0].Trim(), formatString) && IsTryParse(range[1].Trim(), formatString))
                    {
                        StartValue = ParseDate(range[0].Trim(), formatString);
                        EndValue = ParseDate(range[1].Trim(), formatString);
                        return;
                    }
                    else if (!StrictMode || (IsFocused && ValidateOnInput))
                    {
                        StartValue = default;
                        EndValue = default;
                    }
                }
                else if (!StrictMode || (IsFocused && ValidateOnInput))
                {
                    StartValue = default;
                    EndValue = default;
                }
            }
        }
        private async Task InputValueUpdate(EventArgs args = null)
        {
            var inputValue = FormatDateValue;
            if (!string.IsNullOrEmpty(inputValue))
            {
                var range = inputValue.Split(" " + Separator + " ");
                if (range.Length == 2)
                {
                    var formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortDatePattern : Format;
                    if (IsTryParse(range[0].Trim(), formatString) && IsTryParse(range[1].Trim(), formatString))
                    {
                        StartValue = ParseDate(range[0].Trim(), formatString);
                        EndValue = ParseDate(range[1].Trim(), formatString);
                        ValidateMinMaxDays();
                        if (inputValue != PreviousElementValue)
                        {
                            await ChangeTrigger(args);
                        }

                        await UpdateInput();
                        UpdateErrorClass();
                        return;
                    }
                    else if (!StrictMode || (IsFocused && ValidateOnInput))
                    {
                        StartValue = default;
                        EndValue = default;
                    }
                }
                else if (!StrictMode || (IsFocused && ValidateOnInput))
                {
                    StartValue = default;
                    EndValue = default;
                }
            }

            bool isNullable = Nullable.GetUnderlyingType(typeof(TValue)) != null;
            if (StartValue != null && EndValue != null && string.IsNullOrEmpty(FormatDateValue) && !isNullable)
            {
                await UpdateInput();
            }
            else if (!StrictMode || (IsFocused && ValidateOnInput))
            {
                StartValue = EndValue = default;
                await UpdateValue();
                RemoveSelection();
            }
            else
            {
                if (string.IsNullOrEmpty(inputValue?.Trim()))
                {
                    StartValue = EndValue = default;
                }

                await UpdateInput();
            }

            await ChangeTrigger();
            UpdateErrorClass();
        }
        protected override async Task InputHandler(ChangeEventArgs args)
        {
            FormatDateValue = args != null ? (string)args.Value : null;
            if (ValidateOnInput && InputEditContext != null)
            {
                await UpdateValue();
            }
            await Task.CompletedTask;
        }
        protected override async Task ChangeHandler(ChangeEventArgs args)
        {
            CurrentValueAsString = FormatDateValue = args != null ? (string)args.Value : null;
            await Task.CompletedTask;
        }
        private void ValidateRangeStrict()
        {
            if (StartValue != null)
            {
                var startDateVal = ConvertDateValue(StartValue);
                if (startDateVal <= Min)
                {
                    StartValue = ConvertGeneric(Min);
                }
                else if (startDateVal >= Min && startDateVal >= Max)
                {
                    StartValue = ConvertGeneric(Max);
                }
            }

            if (EndValue != null)
            {
                var endDateVal = ConvertDateValue(EndValue);
                if (endDateVal <= Max)
                {
                    EndValue = ConvertGeneric(Max);
                }
                else if (endDateVal <= Min)
                {
                    EndValue = ConvertGeneric(Min);
                }
            }
        }
#pragma warning disable CA1822 // Mark members as static
        private bool IsDateTimeType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return type == typeof(DateTime) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(type));
        }
        private bool IsTryParse(string dateValue, string format)
        {
            if (IsDateTimeType())
            {
                DateTime dateTimeVal;
                return DateTime.TryParseExact(dateValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dateTimeVal);
            }
            else
            {
                DateTimeOffset dateTimeOffsetVal;
                return DateTimeOffset.TryParseExact(dateValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dateTimeOffsetVal);
            }
        }

        private TValue ParseDate(string dateValue, string format)
        {
            Type type = typeof(TValue);
            if (IsDateTimeType())
            {
                return (TValue)SfBaseUtils.ChangeType(DateTime.ParseExact(dateValue, format, (IFormatProvider)CultureInfo.CurrentCulture), type);
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(DateTimeOffset.ParseExact(dateValue, format, (IFormatProvider)CultureInfo.CurrentCulture), type);
            }
        }

        private async Task UpdateInput()
        {
            var formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortDatePattern : Format;
            if (StartDate != null && EndDate != null)
            {
                string startDate = Intl.GetDateFormat(StartDate, formatString);
                string endDate = Intl.GetDateFormat(EndDate, formatString);
                await UpdateInputValue(startDate + " " + Separator + " " + endDate);
                PreviousStartValue = StartValue = StartDate;
                PreviousEndValue = EndValue = EndDate;
            }
            else if ((StrictMode && !(IsFocused && ValidateOnInput)) && !string.IsNullOrEmpty(FormatDateValue))
            {
                await UpdateInputValue(null);
            }

            UpdateIconState();
        }

        private void UpdateErrorClass()
        {
            var inputStr = FormatDateValue?.Trim();
            if (((StartValue == null && EndValue == null && !string.IsNullOrEmpty(inputStr)) ||
                ((StartValue != null && ConvertDateValue(StartValue).Ticks < Min.Ticks)
                || ((StartValue != null && EndValue != null) && ConvertDateValue(StartValue) > ConvertDateValue(EndValue))
                || (EndValue != null && ConvertDateValue(EndValue) > Max))) && !string.IsNullOrEmpty(inputStr))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERROR_CLASS);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_INVALID, TRUE, inputAttr);
            }
            else
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERROR_CLASS);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_INVALID, FALSE, inputAttr);
            }
        }

        private void SetAllowEdit()
        {
            inputAttr = AllowEdit ? !Readonly ? RemoveAttr(READ_ONLY, inputAttr) :
                inputAttr : SfBaseUtils.UpdateDictionary(READ_ONLY, string.Empty, inputAttr);
        }
#pragma warning disable CA1822 // Mark members as static
        private Dictionary<string, object> RemoveAttr(string removeClass, Dictionary<string, object> attr)
#pragma warning restore CA1822 // Mark members as static
        {
            attr.Remove(removeClass);
            return attr;
        }

        private DateTime GetIdValue(CellDetails args)
        {
            long id = long.Parse(args.CellID.Split(new char[] { '_' })[0], CurrentCulture);
            string dateString = Intl.GetDateFormat(new DateTime(id), FORMAT_FULL_DATE, CalendarLocale);
            DateTime date;
            var checkDateValue = DateTime.TryParseExact(dateString, FORMAT_FULL_DATE, CurrentCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out date);
            date = checkDateValue ? DateTime.ParseExact(dateString, FORMAT_FULL_DATE, CurrentCulture) : new DateTime(id);
            return date;
        }

        private async Task SelectRange(CellDetails args)
        {
            await RemoveFocusDate(args.CellID);
            var celldata = args;
            var dateValue = GetIdValue(args);
            var year = (int)dateValue.Year;
            var month = dateValue.Month;
            var firstDay = new DateTime(year, month, 1);
            var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            var firstMonth = new DateTime(year, 1, 1);
            var lastMonth = new DateTime(year, 12, 31);
            if (StartValue != null && EndValue != null && !SfBaseUtils.Equals(StartValue, default) && !SfBaseUtils.Equals(EndValue, default))
            {
                RemoveSelection();
            }
            if (args.ClassList.Contains("e-disabled", StringComparison.Ordinal) && args.ClassList.Contains("e-date-disabled", StringComparison.Ordinal))
            {
                return;
            }
            if (this.StartValue != null && !SfBaseUtils.Equals(StartValue, default))
            {
                if (dateValue.Ticks >= ConvertDate(StartValue).Ticks)
                {
                    if (!(dateValue.Ticks == ConvertDate(StartValue).Ticks && this.MinDays != null && MinDays > 1))
                    {
                        var endDateVal = this.CellListData.Where(item => item.CellID == celldata.CellID)?.FirstOrDefault();
                        if (endDateVal != null)
                        {
                            endDateVal.ClassList += SPACE + END_DATE;
                            endDateVal.ClassList += SPACE + SELECTED;
                        }
                        else
                        {
                            celldata.ClassList += SPACE + END_DATE;
                            celldata.ClassList += SPACE + SELECTED;
                            this.CellListData.Add(celldata);
                        }
                        EndValue = default;
                        EndValue = (Depth == CalendarView.Month) ? ConvertGeneric(dateValue) : (Depth == CalendarView.Year) ? ConvertGeneric(lastDay) : ConvertGeneric(lastMonth);

                        // update attributes
                        // update othermonth 
                        ApplyDisable = false;
                        RemoveSelection(true);
                        // invoke select event

                    }
                }
                else if (dateValue.Ticks <= ConvertDate(StartValue).Ticks)
                {
                    CellListData = new List<CellDetails>();
                    StartValue = (Depth == CalendarView.Month) ? ConvertGeneric(dateValue) :
                        (Depth == CalendarView.Year) ? ConvertGeneric(firstDay) : ConvertGeneric(firstMonth);
                    EndValue = default;
                    celldata.ClassList += SPACE + START_DATE;
                    celldata.ClassList += SPACE + SELECTED;
                    CellListData.Add(celldata);
                    CheckMinMaxDays();
                }
            }
            else
            {
                CellListData = new List<CellDetails>();
                StartValue = (Depth == CalendarView.Month) ? ConvertGeneric(dateValue) :
                    (Depth == CalendarView.Year) ? ConvertGeneric(firstDay) : ConvertGeneric(firstMonth);
                EndValue = default;
                celldata.ClassList += SPACE + START_DATE;

                // update attributes
                // update othermonth
                CheckMinMaxDays();
                ApplyDisable = true;

                // invoke select event
                celldata.ClassList += SPACE + SELECTED;
                CellListData.Add(celldata);
            }

            if (IsDevice)
            {
                StartBtnClass = SfBaseUtils.RemoveClass(StartBtnClass, ACTIVE);
                EndBtnClass = SfBaseUtils.AddClass(EndBtnClass, ACTIVE);
            }

            CurrentCellDetail = celldata;
            UpdateHeaders();
            await SfBaseUtils.InvokeEvent<RangePickerEventArgs<TValue>>(DaterangepickerEvents?.RangeSelected, RangeArgs(CurrentCellDetail.EventArgs));
        }

        private void ProcessPresets()
        {
            PresetsItem = new List<PresetsOptions>();
            int i = 0;
            Regex reg = new Regex("[*'\",_&#^@]");
            var inputValue = FormatDateValue;
            var formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortDatePattern : Format;
            string[] rangeValue = !string.IsNullOrEmpty(inputValue) ? inputValue.Split(" " + Separator + " ") : null;
            DateTime startDate = StartValue != null ? ConvertDateValue(StartValue) : rangeValue != null && rangeValue.Length > 0 && rangeValue[0] != null && IsTryParse(rangeValue[0].Trim(), formatString) ? ConvertDateValue(ParseDate(rangeValue[0].Trim(), formatString)) : default;
            DateTime endDate = EndValue != null ? ConvertDateValue(EndValue) : rangeValue != null && rangeValue.Length > 0 && rangeValue[0] != null && IsTryParse(rangeValue[1].Trim(), formatString) ? ConvertDateValue(ParseDate(rangeValue[1].Trim(), formatString)) : default;
            if (Presets != null && Presets[0] != null && Presets[0].Label != null)
            {
                foreach (var range in Presets)
                {
                    string id = reg.Replace(range.Label, string.Empty) + "_" + (++i);
                    PresetsItem.Add(new PresetsOptions
                    {
                        Id = id,
                        Text = range.Label,
                        Start = range.Start,
                        End = range.End,
                        ListClass = (range.Start.Date == startDate.Date && range.End.Date == endDate.Date) ? LIST_ITEM + SPACE + ACTIVE : LIST_ITEM,
                        IsCustomRange = false,
                    });
                }

                var activeItem = PresetsItem.Where(item => item.ListClass.Contains(ACTIVE, StringComparison.Ordinal)).FirstOrDefault();
                PresetsItem.Add(new PresetsOptions
                {
                    Id = "custom_range",
                    Text = Localizer.GetText(CUSTOM_RANGE) != null ? Localizer.GetText(CUSTOM_RANGE) : CUSTOM_RANGE_VALUE,
                    Start = startDate,
                    End = endDate,
                    ListClass = activeItem != null ? LIST_ITEM : LIST_ITEM + SPACE + ACTIVE,
                    IsCustomRange = true,
                });
                if (activeItem != null || IsDevice)
                {
                    IsCustomPopup = false;
                }
            }

            UpdateErrorClass();
        }

        private PresetsOptions GetPresetActiveItem()
        {
            return PresetsItem.Where(item => item.ListClass.Contains(ACTIVE, StringComparison.Ordinal)).FirstOrDefault();
        }

        private PresetsOptions GetPresetHoverItem()
        {
            return PresetsItem.Where(item => item.ListClass.Contains(HOVER, StringComparison.Ordinal)).FirstOrDefault();
        }

        private List<PresetsOptions> GetPresetHoverList()
        {
            return PresetsItem.Where(item => item.ListClass.Contains(HOVER, StringComparison.Ordinal)).ToList();
        }

        private void SelectStartMonth()
        {
            if (StartValue != null)
            {
                var convertStartDate = ConvertDateValue(StartValue);
                if (IsSameMonth(convertStartDate, Max))
                {
                    var maxDate = new DateTime(Max.Year, Max.Month - 1, 1);
                    StartCurrentDate = ConvertGeneric(maxDate);
                }
                else if (!(convertStartDate >= Min && convertStartDate <= Max))
                {
                    StartCurrentDate = ConvertGeneric(DateTime.Now.Date);
                }
                else
                {
                    StartCurrentDate = StartValue;
                }
                if ((EndValue != null && ConvertDateValue(EndValue).Ticks > Max.Ticks) || (ConvertDateValue(StartValue).Ticks < Min.Ticks) || (ConvertDateValue(EndValue).Ticks > ConvertDateValue(EndValue).Ticks))
                {
                    StartCurrentDate = ConvertGeneric(DateTime.Now.Date);
                }
            }
            else
            {
                StartCurrentDate = ConvertGeneric(DateTime.Now.Date);
            }

            StartMonthCurrentDate();
        }

        private void SelectNextMonth()
        {
            var currentDate = ConvertDateValue(StartCurrentDate);
            if (StartValue != null && EndValue != null)
            {
                if (!IsSameMonth(ConvertDateValue(EndValue), currentDate))
                {
                    EndCurrentDate = EndValue;
                    currentDate = ConvertDateValue(EndValue);
                }
                else
                {
                    SetNextMonthDate(currentDate);
                }
                if ((ConvertDateValue(StartValue).Ticks < Min.Ticks) || (ConvertDateValue(EndValue).Ticks > Max.Ticks) || (ConvertDateValue(StartValue).Ticks > ConvertDateValue(EndValue).Ticks))
                {
                    var endDateVal = DateTime.Now.Date;
                    var isDefaultValue = SfBaseUtils.Equals(EndCurrentDate, default);
                    endDateVal = isDefaultValue ? endDateVal.AddMonths(1) : new DateTime(currentDate.Month == 12 ? endDateVal.Year + 1 : endDateVal.Year, currentDate.Month == 12 ? 1 : currentDate.Month + 1, 1);
                    EndCurrentDate = ConvertGeneric(endDateVal);
                }
            }
            else
            {
                SetNextMonthDate(currentDate);
            }
        }

        private void SetNextMonthDate(DateTime currentDate)
        {
            EndCurrentDate = ConvertGeneric(new DateTime(currentDate.Month == 12 ? currentDate.Year + 1 : currentDate.Year, currentDate.Month == 12 ? 1 : currentDate.Month + 1, 1));
            return;
        }

        private void SelectNextYear()
        {
            var currentDate = ConvertDateValue(StartCurrentDate);
            if (StartValue != null && EndValue != null)
            {
                if (!IsSameYear(ConvertDateValue(EndValue), currentDate))
                {
                    EndCurrentDate = EndValue;
                    currentDate = ConvertDateValue(EndValue);
                } else
                {
                    var nextYear = new DateTime(currentDate.Year + 1, 1, currentDate.Day);
                    EndCurrentDate = ConvertGeneric(nextYear);
                }
                if ((ConvertDateValue(StartValue).Ticks < Min.Ticks) || (ConvertDateValue(EndValue).Ticks > Max.Ticks) || (ConvertDateValue(StartValue).Ticks > ConvertDateValue(EndValue).Ticks))
                {
                    var endDateVal = DateTime.Now.Date;
                    endDateVal = new DateTime(endDateVal.Year + 1, 1, 1);
                    EndCurrentDate = ConvertGeneric(endDateVal);
                }
            }
            else
            {
                EndCurrentDate = ConvertGeneric(new DateTime(currentDate.Year + 1, 1, 1));
                return;
            }
        }

        private void SelectNextDecade()
        {
            var currentDate = ConvertDateValue(StartCurrentDate);
            if (StartValue != null && EndValue != null)
            {
                if (!IsSameDecade(ConvertDateValue(EndValue), currentDate))
                {
                    EndCurrentDate = EndValue;
                    currentDate = ConvertDateValue(EndValue);
                }
                else
                {
                    var nextYear = new DateTime(currentDate.Year + 10, 1, 1);
                    EndCurrentDate = ConvertGeneric(nextYear);
                }
                if ((ConvertDateValue(StartValue).Ticks < Min.Ticks) || (ConvertDateValue(EndValue).Ticks > Max.Ticks) || (ConvertDateValue(StartValue).Ticks > ConvertDateValue(EndValue).Ticks))
                {
                    var endDateVal = DateTime.Now.Date;
                    endDateVal = new DateTime(endDateVal.Year + 10, currentDate.Month, currentDate.Day);
                    EndCurrentDate = ConvertGeneric(endDateVal);
                }
            }
            else
            {
                EndCurrentDate = ConvertGeneric(new DateTime(currentDate.Year + 10, currentDate.Month, currentDate.Day));
                return;
            }
        }

        private void StartMonthCurrentDate()
        {
            if (IsSameMonth(Min, Max) || ConvertDateValue(StartCurrentDate).Ticks > Max.Ticks || IsSameMonth(ConvertDateValue(StartCurrentDate), Max))
            {
                var maxMonth = Max.Month > 1 ? Max.Month - 1 : Max.Month;
                var maxDate = new DateTime(Max.Year, maxMonth, 1);
                StartCurrentDate = ConvertGeneric(maxDate);
            }
            else if (ConvertDateValue(StartCurrentDate).Ticks < Min.Ticks)
            {
                StartCurrentDate = ConvertGeneric(Min);
            }
        }

#pragma warning disable CA1822 // Mark members as static
        private bool IsSameMonth(DateTime start, DateTime end)
#pragma warning restore CA1822 // Mark members as static
        {
            return start.Month == end.Month && start.Year == end.Year;
        }
#pragma warning disable CA1822 // Mark members as static
        private bool IsSameYear(DateTime start, DateTime end)
#pragma warning restore CA1822 // Mark members as static
        {
            return start.Year == end.Year;
        }
#pragma warning disable CA1822 // Mark members as static
        private bool IsSameDecade(DateTime start, DateTime end)
#pragma warning restore CA1822 // Mark members as static
        {
            var startYear = start.Year;
            var endYear = end.Year;
            return (startYear - (startYear % 10)) == (endYear - (endYear % 10));
        }

        private void RemoveSelection(bool isDisable = false)
        {
            var cellDatas = CellListData != null ? CellListData : new List<CellDetails>();
            foreach (var cell in cellDatas)
            {
                cell.ClassList = SfBaseUtils.RemoveClass(cell.ClassList, !isDisable ? RANGE_HOVER : DATE_DISABLED);
                cell.ClassList = SfBaseUtils.RemoveClass(cell.ClassList, !isDisable ? SELECTED : DISABLED);
                cell.ClassList = !isDisable ? cell.ClassList : SfBaseUtils.RemoveClass(cell.ClassList, OVERLAY);
            }

            var cellDetails = CellDetailsData != null ? CellDetailsData : new List<CellDetails>();
            foreach (var item in cellDetails)
            {
                item.ClassList = SfBaseUtils.RemoveClass(item.ClassList, !isDisable ? RANGE_HOVER : DATE_DISABLED);
                item.ClassList = SfBaseUtils.RemoveClass(item.ClassList, !isDisable ? SELECTED : DISABLED);
                item.ClassList = !isDisable ? item.ClassList : SfBaseUtils.RemoveClass(item.ClassList, OVERLAY);
            }

            if (!isDisable)
            {
                StartValue = default;
                EndValue = default;
                CellListData = new List<CellDetails>();
            }
            else
            {
                RightNextIcon = RightNextIcon != null && RightNextIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(RightNextIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : RightNextIcon;
                RightPrevIcon = RightPrevIcon != null && RightPrevIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(RightPrevIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : RightPrevIcon;
                LeftNextIcon = LeftNextIcon != null && LeftNextIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(LeftNextIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : LeftNextIcon;
                LeftPrevIcon = LeftPrevIcon != null && LeftPrevIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.RemoveClass(LeftPrevIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : LeftPrevIcon;
            }
        }

        private async Task DeviceStartBtnClick()
        {
            if (StartValue != null)
            {
                EndBtnClass = SfBaseUtils.RemoveClass(EndBtnClass, ACTIVE);
                StartBtnClass = SfBaseUtils.AddClass(StartBtnClass, ACTIVE);
                StartCurrentDate = StartValue;
                await DeviceCalendar.NavigateTo(Start, StartValue);
            }
        }

        private async Task DeviceEndBtnClick()
        {
            if (EndValue != null)
            {
                StartBtnClass = SfBaseUtils.RemoveClass(StartBtnClass, ACTIVE);
                EndBtnClass = SfBaseUtils.AddClass(EndBtnClass, ACTIVE);
                StartCurrentDate = EndValue;
                await DeviceCalendar.NavigateTo(Start, EndValue);
            }
        }

        private async Task ApplyFunction(MouseEventArgs args = null)
        {
            if (StartValue != null && EndValue != null)
            {
                await UpdateInputValue(RangeArgs(args).Text);
                await ChangeTrigger(args);
            }

            RemoveSelection();
            await Hide();
            if (args != null)
            {
                await FocusIn();
            }

            UpdateErrorClass();
        }

        private RangePickerEventArgs<TValue> RangeArgs(EventArgs args = null)
        {
            var inputVal = string.Empty;
            var range = 0;
            var formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortDatePattern;
            var startDate = StartValue != null ? Intl.GetDateFormat(StartValue, formatString) : null;
            var endDate = EndValue != null ? Intl.GetDateFormat(EndValue, formatString) : null;
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                inputVal = startDate + SPACE + Separator + SPACE + endDate;
                range = (int)Math.Round(Math.Abs((decimal)(ConvertDateValue(StartValue) - ConvertDateValue(EndValue)).TotalMilliseconds / (1000 * 60 * 60 * 24))) + 1;
            }

            return new RangePickerEventArgs<TValue>()
            {
                DaySpan = range,
                Element = InputElement,
                EndDate = EndValue,
                StartDate = StartValue,
                Event = args,
                IsInteracted = args != null,
                Text = inputVal,
                Name = "RangePickerEvent",
            };
        }

        private async Task CancelFunction()
        {
            await FocusIn();
            RemoveSelection();
            ApplyDisable = (StartValue != null && EndValue != null) ? false : true;
            await Hide();
        }

        private void UpdateStartEndValue()
        {
            StartValue = PreviousStartValue != null ? PreviousStartValue : default;
            EndValue = PreviousEndValue != null ? PreviousEndValue : default;
        }

        private void CheckMinMaxDays()
        {
            if ((MinDays != null && MinDays > 0) || (MaxDays != null && MaxDays > 0))
            {
               UpdateMinMaxDays();
            }
        }

        private void UpdateMinMaxDays()
        {
            if (StartValue != null && EndValue == null)
            {
                if ((MinDays != null && MinDays > 0) || (MaxDays != null && MaxDays > 0))
                {
                    var starDays = ConvertDateValue(StartValue).Day;
                    var minDay = MinDays == null ? -1 : MinDays - 1;
                    var maxDay = MaxDays == null ? -1 : MaxDays - 1;
                    DateTime? minDate = ConvertDateValue(StartValue).AddDays((double)minDay);
                    DateTime? maxDate = ConvertDateValue(StartValue).AddDays((double)maxDay);
                    minDate = MinDays != null && MinDays > 0 ? minDate : null;
                    maxDate = MaxDays != null && MaxDays > 0 ? maxDate : null;
                    if (CurrentView() == YEAR)
                    {
                        minDate = minDate != null ? (DateTime?)new DateTime(((DateTime)minDate).Year, ((DateTime)minDate).Month, 0) : null;
                        maxDate = maxDate != null ? (DateTime?)new DateTime(((DateTime)maxDate).Year, ((DateTime)maxDate).Month, 1) : null;
                    }
                    else if (CurrentView() == DECADE)
                    {
                        minDate = minDate != null ? (DateTime?)new DateTime(((DateTime)minDate).Year - 1, 12, 1) : null;
                        maxDate = maxDate != null ? (DateTime?)new DateTime(((DateTime)maxDate).Year, 0, 1) : null;
                    }

                    DateTime? maxDayEle = UpdateMinMaxDaysCell(minDate, maxDate);
                    ChangedIconState(maxDayEle);
                    StateHasChanged();
                }
            }
        }
        private DateTime? UpdateMinMaxDaysCell(DateTime? minDate, DateTime? maxDate)
        {
            DateTime? maxDayEle = null;
            foreach (var cellItem in CellDetailsData)
            {
                cellItem.ClassList = SfBaseUtils.RemoveClass(cellItem.ClassList, DISABLED + SPACE + DATE_DISABLED + SPACE + OVERLAY);
                if (!cellItem.ClassList.Contains(START_DATE, StringComparison.Ordinal) && !cellItem.ClassList.Contains(WEEKNUMBER, StringComparison.Ordinal))
                {
                    var eleDate = GetIdValue(cellItem);
                    if (minDate != null && eleDate == minDate && cellItem.ClassList.Contains(DISABLED, StringComparison.Ordinal))
                    {
                        minDate = ((DateTime)minDate).AddDays(((DateTime)minDate).Day + 1);
                    }

                    if (!cellItem.ClassList.Contains(DISABLED, StringComparison.Ordinal))
                    {
                        if (eleDate > ConvertDateValue(StartValue))
                        {
                            if (minDate != null && eleDate < minDate)
                            {
                                cellItem.ClassList = SfBaseUtils.AddClass(cellItem.ClassList, DISABLED + SPACE + DATE_DISABLED + SPACE + OVERLAY);
                                CellListData.Add(cellItem);
                                CellListData.Add(cellItem);
                            }

                            if (maxDate != null && eleDate > maxDate)
                            {
                                cellItem.ClassList = SfBaseUtils.AddClass(cellItem.ClassList, DISABLED + SPACE + DATE_DISABLED + SPACE + OVERLAY);
                                maxDayEle = maxDayEle == null && !cellItem.ClassList.Contains(OTHER_MONTH, StringComparison.Ordinal) ? eleDate : maxDayEle;
                                CellListData.Add(cellItem);
                            }
                        }
                    }
                }
            }
            return maxDayEle;
        }
        private void ChangedIconState(DateTime? maxDayEle)
        {
            if (maxDayEle != null)
            {
                if (((DateTime)maxDayEle).Month == ConvertDateValue(StartCurrentDate).Month)
                {
                    RightNextIcon = RightNextIcon != null && !RightNextIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.AddClass(RightNextIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : RightNextIcon;
                    RightPrevIcon = RightPrevIcon != null && !RightPrevIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.AddClass(RightPrevIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : RightPrevIcon;
                    LeftNextIcon = LeftNextIcon != null && !LeftNextIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.AddClass(LeftNextIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : LeftNextIcon;
                }
                else
                {
                    RightNextIcon = RightNextIcon != null && !RightNextIcon.Contains(DISABLED, StringComparison.Ordinal) ? SfBaseUtils.AddClass(RightNextIcon, ICON_DISABLED + SPACE + DISABLED + SPACE + OVERLAY) : RightNextIcon;
                }
            }
        }
        private void UpdateIconState()
        {
            ContainerClass = (!AllowEdit && !Readonly) ? string.IsNullOrEmpty(FormatDateValue) ?
                    SfBaseUtils.RemoveClass(ContainerClass, NO_EDIT) : SfBaseUtils.AddClass(ContainerClass, NO_EDIT) : SfBaseUtils.RemoveClass(ContainerClass, NO_EDIT);
        }
#pragma warning disable CA1822 // Mark members as static
        private int CompareValue(TValue value1, TValue value2)
        {
            return Comparer<TValue>.Default.Compare(value1, value2);
        }
        private bool IsDateTimeOffsetType()
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return type == typeof(DateTimeOffset) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(type));
        }
        private DateTime ConvertDateValue(TValue dateValue)
        {
            return (IsDateTimeOffsetType()) ? ((DateTimeOffset)SfBaseUtils.ChangeType(dateValue, typeof(TValue))).DateTime : (DateTime)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
        }

        private TValue ConvertGeneric(DateTime dateValue)
#pragma warning restore CA1822 // Mark members as static
        {
            if (IsDateTimeOffsetType())
            {
                var year = ((DateTimeOffset)dateValue).Year;
                var month = ((DateTimeOffset)dateValue).Month;
                var day = ((DateTimeOffset)dateValue).Day;
                var offset = ((DateTimeOffset)dateValue).Offset;
                var offsetValue = new DateTimeOffset(year, month, day, 0, 0, 0, 0, offset);
                return (TValue)SfBaseUtils.ChangeType(offsetValue, typeof(TValue));
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
            }
        }
    }
}