using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Inputs.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows to select a date and time value.
    /// </summary>
#pragma warning disable CA1501 // Mark members as hierarchy
    public partial class SfDateTimePicker<TValue> : SfDatePicker<TValue>
#pragma warning restore CA1501 // Mark members as hierarchy
    {
        internal const string TIME_ICON = "e-time-icon e-icons";

        internal const string LIST_ITEM = "e-list-item";

        internal const string SELECTED = "e-active";

        internal const string NAVIGATION = "e-navigation";

        internal const string HOVER = "e-hover";

        internal const int TAB_INDEX = 0;

        internal DateTimePickerEvents<TValue> DatetimepickerEvents { get; set; }

        /// <summary>
        /// Gets or sets the root class of the component.
        /// </summary>
        /// <exclude/>
        protected override string ROOT { get; set; } = "e-control e-datetimepicker e-lib";
        protected string TimeIcon { get; set; }
        /// <summary>
        /// Gets or sets the container class of the component.
        /// </summary>
        /// <exclude/>
        protected override string CONTAINER_CLASS { get; set; } = "e-control-wrapper e-datetime-wrapper e-control-container e-datetime-container";

        /// <summary>
        /// Triggers when the input gets keydown.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        private List<ListOptions<TValue>> ListData { get; set; }

        private DateTime DatePart { get; set; } = DateTime.Today;

        private bool ShowPopupList { get; set; }

        private bool IsListRendered { get; set; }

        internal ElementReference TimePopupElement { get; set; }

        internal ElementReference TimePopupHolderEle { get; set; }

        private bool IsTimeIconClicked { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (string.IsNullOrEmpty(ID))
            {
                ID = "datetimepicker-" + Guid.NewGuid().ToString();
            }

            if (Value != null)
            {
                await UpdateInput();
            }

            TimeIcon = TIME_ICON;
            if (DateTimePickerParent != null && Convert.ToString(DateTimePickerParent.Type, CultureInfo.CurrentCulture) == "DateTime")
            {
                DateTimePickerParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(DateTimePickerParent, this);
            }
        }

        private async Task KeyActionHandler(KeyboardEventArgs args)
        {
            if (IsCalendarRender && args.AltKey && args.Code == "ArrowDown")
            {
                await Hide();
                if (!IsListRender)
                {
                    await ShowTimePopup();
                }
            }
            if (IsListRender && args != null)
            {
                switch (args.Code)
                {
                    case "ArrowDown":
                    case "Home":
                    case "End":
                    case "ArrowUp":
                        if (!args.AltKey)
                        {
                            await KeyHandler(args);
                            await InvokeMethod("sfBlazor.DatePicker.updateScrollPosition", new object[] { InputElement });
                        }
                        break;
                    case "Enter":
                        var findItems = ListData.Where(item => item.ListClass.Contains(NAVIGATION, StringComparison.CurrentCultureIgnoreCase) || item.ListClass.Contains(SELECTED, StringComparison.Ordinal)).FirstOrDefault();
                        UpdateListSelection(findItems.ItemData, SELECTED);
                        await StrictModeUpdate();
                        await SelectTimeList();
                        await InvokeSelectEvent(new SelectedEventArgs<TValue>() { Value = findItems.DateTimeValue });
                        await ChangeTrigger(args);
                        UpdateErrorClass();
                        await Hide();
                        break;
                    case "Escape":
                        await Hide();
                        break;
                }
            }
            if(OnKeyDown.HasDelegate)
            {
                await OnKeyDown.InvokeAsync(args);
            }
        }

        private async Task KeyHandler(KeyboardEventArgs args)
        {
            if (Step > 0 && ListData == null)
            {
                GenerateList();
            }

            var listCount = ListData?.Count;
            int? activeIndex = null;
            if (string.IsNullOrEmpty(CurrentValueAsString) && Value == null)
            {
                activeIndex = (args.Code == "End") ? (int)listCount - 1 : 0;
            }
            else
            {
                var selectedData = ListData.Where(listItem => listItem.ListClass.Contains(SELECTED, StringComparison.CurrentCulture)).FirstOrDefault();
                var navigationData = ListData.Where(listItem => listItem.ListClass.Contains(NAVIGATION, StringComparison.CurrentCulture)).FirstOrDefault();
                var findItems = navigationData != null ? navigationData : selectedData;
                activeIndex = findItems != null ? ListData.IndexOf(findItems) : 0;
                activeIndex = (args.Code == "ArrowDown") ? ++activeIndex : (args.Code == "ArrowUp") ? --activeIndex : (args.Code == "Home") ? 0 : (int)listCount - 1;
            }

            activeIndex = (activeIndex >= 0) ? activeIndex : ListData.Count + activeIndex;
            var selectItem = ListData.ElementAtOrDefault((int)activeIndex);
            if (selectItem != null)
            {
                UpdateListSelection(selectItem.ItemData, NAVIGATION);
                await UpdateValue(selectItem.DateTimeValue);
                await SelectTimeList();
            }
        }

        /// <summary>
        /// Task used to render the popup.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task ClientPopupRender()
        {
            if (IsDatePickerPopup)
            {
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, DATE_PICKER);
                await base.ClientPopupRender();
            }
            else if (ShowPopupList && IsListRendered)
            {
                IsListRendered = false;
                var options = GetClientProperties();
                await InvokeMethod("sfBlazor.DatePicker.renderPopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, options });
                IsListRender = true;
            }
        }

        /// <summary>
        /// Triggers when popup get opened.
        /// </summary>
        /// <param name="isOpen">True when popup is in opened state.</param>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task{PopupObjectArgs}"/>.</returns>
        /// <exclude/>
        protected override async Task<PopupObjectArgs> InvokeOpenEvent(bool isOpen, EventArgs args = null)
        {
            var openEventArgs = new PopupObjectArgs
            {
                Cancel = false,
                Event = args,
                PreventDefault = false
            };
            if (!IsDatePickerPopup && isOpen)
            {
                GenerateList();
            }

            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = args, PreventDefault = false };
            await SfBaseUtils.InvokeEvent<PopupObjectArgs>(isOpen ? DatetimepickerEvents?.OnOpen : DatetimepickerEvents?.OnClose, openEventArgs);
            return openEventArgs;
        }

        /// <summary>
        /// Handles the time icon process.
        /// </summary> 
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task TimeIconHandler(EventArgs eventArgs)
        {
            if (Enabled && eventArgs != null)
            {
                var isDisabled = (inputAttr != null ? inputAttr.ContainsKey("disabled") : false) || (HtmlAttributes != null ? HtmlAttributes.ContainsKey("disabled") : false);
                if (isDisabled)
                {
                    return;
                }

                await Task.Delay(10); // set the delay for prevent the icon click action.
                if (IsDevice)
                {
                    SfBaseUtils.UpdateDictionary(READ_ONLY, true, inputAttr);
                    await FocusOut();
                }
                else
                {
                    PreventIconHandler = true;
                }

                IsTimeIconClicked = true;
                if (IsListRender)
                {
                    await Hide(eventArgs);
                }
                else
                {
                    if (IsCalendarRender)
                    {
                        await Hide(eventArgs);
                    }

                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUT_FOCUS);
                    if (!((Enabled && Readonly) || !Enabled))
                    {
                        TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
                    }

                    await FocusIn();
                    await Show(eventArgs);
                }
            }
        }

        /// <summary>
        /// Task used to hide the time popup.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task HideTimePopup()
        {
            await Hide();
            ShowPopupList = false;
        }

        /// <summary>
        /// Task used to update the properties of calendar.
        /// </summary>
        /// <param name="key">Specifies the key value.</param>
        /// <param name="dateTimeValue">Specifies the date value.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        internal override async Task UpdateCalendarProperty(string key, object dateTimeValue)
        {
            if (key == nameof(Value))
            {
                object dateValue;
                if (!IsDateTimeType())
                {
                    var year = ((DateTimeOffset)dateTimeValue).Year;
                    var month = ((DateTimeOffset)dateTimeValue).Month;
                    var day = ((DateTimeOffset)dateTimeValue).Day;
                    var offset = ((DateTimeOffset)dateTimeValue).Offset;
                    dateValue = new DateTimeOffset(year, month, day, TimePart.Hour, TimePart.Minute, TimePart.Second, TimePart.Millisecond, offset);
                }
                else
                {
                    dateValue = new DateTime(((DateTime)dateTimeValue).Ticks, DateTimeKind.Local);
                }
                await UpdateValue(dateValue);
                if (IsCalendarRender)
                {
                    await Hide();
                }
            }
        }

        /// <summary>
        /// Triggered when the value of the component get changed.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <exclude/>
        protected override void ChangeEvent(EventArgs args)
        {
            if (!SfBaseUtils.Equals(Value, PreviousDate))
            {
                _ = SelectCalendar();
                ChangedEventArgs = new ChangedEventArgs<TValue>()
                {
                    Value = ChangedArgs.Value,
                    Event = args,
                    IsInteracted = args != null
                };
                _ = SfBaseUtils.InvokeEvent<ChangedEventArgs<TValue>>(DatetimepickerEvents?.ValueChange, ChangedEventArgs);
                if (EnablePersistence)
                {
                    _ = SetLocalStorage(ID, Value);
                }

                PreviousDate = Value;
            }
        }

        /// <summary>
        /// Triggers when the value is selected in popups.
        /// </summary>
        /// <param name="args">The args<see cref="SelectedEventArgs{TValue}"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        internal override async Task InvokeSelectEvent(SelectedEventArgs<TValue> args)
        {
            await SfBaseUtils.InvokeEvent<SelectedEventArgs<TValue>>(DatetimepickerEvents?.Selected, args);
        }

        /// <summary>
        /// Task used to update the date and time popup states.
        /// </summary>
        /// <param name="isOpen">The isOpen<see cref="bool"/>.</param>
        /// <exclude/>
        protected override void UpdateDateTimePopupState(bool isOpen)
        {
            IsListRender = ShowPopupList = IsListRendered = isOpen;
            if (isOpen)
            {
                SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, inputAttr);
            }
            else
            {
                TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
            }

            _ = InvokeAsync(() => StateHasChanged());
        }

        private void OnMouseOver(ListOptions<TValue> listItem)
        {
            UpdateListSelection(listItem.ItemData, HOVER);
        }

        /// <summary>
        /// Triggers when the component get focused.
        /// </summary>
        /// <param name="args">The args<see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task InvokeFocusEvent(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            var focusEventArgs = new FocusEventArgs();
            await SfBaseUtils.InvokeEvent<FocusEventArgs>(DatetimepickerEvents?.Focus, focusEventArgs);
        }

        /// <summary>
        /// triggers when the component get focused out.
        /// </summary>
        /// <param name="args">The args <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task InvokeBlurEvent(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            DateIcon = SfBaseUtils.RemoveClass(DateIcon, ACTIVE);
            TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
            var blurEventArgs = new BlurEventArgs();
            await SfBaseUtils.InvokeEvent<BlurEventArgs>(DatetimepickerEvents?.Blur, blurEventArgs);
        }

        /// <summary>
        /// Triggers when the mouse is clicked on the component.
        /// </summary>
        /// <param name="listItem">The listItem<see cref="ListOptions{TValue}"/>.</param>
        /// <param name="eventArgs">The eventArgs<see cref="MouseEventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task OnMouseClick(ListOptions<TValue> listItem, MouseEventArgs eventArgs)
        {
            if (listItem != null && listItem.ItemData != null)
            {
                if (IsListRender)
                {
                    await Hide(eventArgs);
                }

                UpdateListSelection(listItem.ItemData, SELECTED);
                await InvokeSelectEvent(new SelectedEventArgs<TValue>() { Value = listItem.DateTimeValue });
                await UpdateValue(listItem.DateTimeValue);
            }

            await SelectTimeList();
            await ChangeTrigger(eventArgs);
        }

        private void UpdateListSelection(string item, string className)
        {
            if (ListData != null)
            {
                var hoverData = ListData.Where(listItem => listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture)).FirstOrDefault();
                if (hoverData != null)
                {
                    hoverData.ListClass = SfBaseUtils.RemoveClass(hoverData.ListClass, HOVER);
                }

                var navData = ListData.Where(listItem => listItem.ListClass.Contains(NAVIGATION, StringComparison.CurrentCulture)).FirstOrDefault();
                if (navData != null)
                {
                    navData.ListClass = SfBaseUtils.RemoveClass(navData.ListClass, HOVER);
                }

                var selectedData = ListData.Where(listItem => listItem.ListClass.Contains(className, StringComparison.CurrentCulture)).FirstOrDefault();
                if (selectedData != null)
                {
                    selectedData.ListClass = SfBaseUtils.RemoveClass(selectedData.ListClass, className);
                }

                foreach (var listItem in ListData)
                {
                    if (SfBaseUtils.Equals(listItem.ItemData, item))
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, className);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Triggers when the value get changed in the component.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task InvokeChangeEvent(EventArgs args = null)
        {
            ChangedEventArgs = new ChangedEventArgs<TValue>()
            {
                Value = Value,
                Event = args,
                IsInteracted = args != null
            };
            await SfBaseUtils.InvokeEvent<ChangedEventArgs<TValue>>(DatetimepickerEvents?.ValueChange, ChangedEventArgs);
        }
        /// <summary>
        /// Opens the popup to show the list items.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ShowDatePopupAsync()
        {
            await Show();
        }

        /// <summary>
        /// Opens the popup to show the calendar.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ShowDatePopup()
        {
            IsDatePickerPopup = true;
            await Show(null);
        }

        /// <summary>
        /// Opens the popup to show the list items.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ShowTimePopupAsync()
        {
            await ShowTimePopup();
        }
        /// <summary>
        /// Opens the popup to show the list items.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ShowTimePopup()
        {
            IsDatePickerPopup = false;
            if (TimeIcon != null)
            {
                TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
            }

            await Show(null);
        }

        private async Task ChangeTrigger(EventArgs args = null)
        {
            if (CurrentValueAsString != PreviousElementValue && SfBaseUtils.CompareValues<TValue>(PreviousDate, Value) != 0)
            {
                var changedEventArgs = new ChangedEventArgs<TValue>()
                {
                    Value = Value,
                    Event = args,
                    IsInteracted = args != null,
                    Element = InputElement
                };
                await SfBaseUtils.InvokeEvent<ChangedEventArgs<TValue>>(DatetimepickerEvents?.ValueChange, changedEventArgs);
                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, Value);
                }

                PreviousElementValue = CurrentValueAsString;
                PreviousDate = Value;
            }

            UpdateValidateClass();
        }

        private async Task SelectTimeList()
        {
            var date = string.Empty;
            if (Value != null)
            {
                var formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortDatePattern + " " + CurrentCulture.DateTimeFormat.ShortTimePattern;
                date = FormatDateValue(Value, formatString);
            }

            await UpdateInputValue(date);
        }

        private string FormatDateValue(TValue timeValue, string formatString)
        {
            return Intl.GetDateFormat(timeValue, formatString, CalendarLocale);
        }

        private async Task UpdateInputValue(string timeValue)
        {
            await SetValue(timeValue, FloatLabelType, ShowClearButton);

        }

        internal override void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
            _ = SfBaseUtils.InvokeEvent<NavigatedEventArgs>(DatetimepickerEvents?.Navigated, eventArgs);
        }

        internal override async Task BindRenderDayEvent(RenderDayCellEventArgs eventArgs)
        {
            await SfBaseUtils.InvokeEvent<RenderDayCellEventArgs>(DatetimepickerEvents?.OnRenderDayCell, eventArgs);
        }

        private DateTime StartTime(DateTime startDate)
        {
            var tempMin = Min;
            bool start = false;
            var tempStartValue = default(DateTime);
            if ((startDate.Date == tempMin.Date && startDate.Month == tempMin.Month && startDate.Year == tempMin.Year) || (new DateTime(startDate.Year, startDate.Month, startDate.Day) <= new DateTime(tempMin.Year, tempMin.Month, tempMin.Day)))
            {
                start = false;
                tempStartValue = Min;
            }
            else if (startDate.Ticks < Max.Ticks && startDate.Ticks > Min.Ticks)
            {
                start = true;
                tempStartValue = startDate;
            }
            else if (startDate.Ticks >= Max.Ticks)
            {
                start = true;
                tempStartValue = Max;
            }

            return CalculateStartEnd(tempStartValue, start, "starttime");
        }

        private DateTime EndTime(DateTime endDate)
        {
            var tempMax = Max;
            bool end = false;
            var tempEndValue = default(DateTime);
            if ((endDate.Date == tempMax.Date && endDate.Month == tempMax.Month && endDate.Year == tempMax.Year) || (new DateTime(endDate.Year, endDate.Month, endDate.Day) >= new DateTime(tempMax.Year, tempMax.Month, tempMax.Day)))
            {
                end = false;
                tempEndValue = Max;
            }
            else if (endDate.Ticks < Max.Ticks && endDate.Ticks > Min.Ticks)
            {
                end = true;
                tempEndValue = endDate;
            }
            else if (endDate.Ticks <= Min.Ticks)
            {
                end = true;
                tempEndValue = Min;
            }

            return CalculateStartEnd(tempEndValue, end, "endtime");
        }
#pragma warning disable CA1822 // Mark members as static
        private DateTime CalculateStartEnd(DateTime dateValue, bool range, string method)
#pragma warning restore CA1822 // Mark members as static
        {
            var day = dateValue.Day;
            var month = dateValue.Month;
            var year = dateValue.Year;
            var hours = dateValue.Hour;
            var minutes = dateValue.Minute;
            var seconds = dateValue.Second;
            var milliseconds = dateValue.Millisecond;
            if (range)
            {
                return (method == "starttime") ? new DateTime(year, month, day, 0, 0, 0) : new DateTime(year, month, day, 23, 59, 59);
            }
            else
            {
                return new DateTime(year, month, day, hours, minutes, seconds, milliseconds);
            }
        }

        private void GenerateList()
        {
            if (Step > 0)
            {
                var datetimeValue = Value != null ? ConvertDate(Value) : DateTime.Now;
                TimeSpan start = StartTime(datetimeValue).TimeOfDay;
                TimeSpan end = EndTime(datetimeValue).TimeOfDay;
                TimeSpan interval = new TimeSpan(0, Step, 0);
                ListData = new List<ListOptions<TValue>>();
                var formatString = string.IsNullOrEmpty(TimeFormat) ? CurrentCulture.DateTimeFormat.ShortTimePattern : TimeFormat;
                while (end >= start)
                {
                    datetimeValue = StartTime(datetimeValue);
                    var listDate = new DateTime(datetimeValue.Year, datetimeValue.Month, datetimeValue.Day, start.Hours, start.Minutes, start.Seconds, start.Milliseconds, datetimeValue.Kind);
                    var listItem = new ListOptions<TValue>()
                    {
                        DateTimeValue = ConvertGeneric(listDate),
                        ItemData = Intl.GetDateFormat(listDate, formatString, CalendarLocale),
                        ListClass = LIST_ITEM
                    };
                    if (Value != null && SfBaseUtils.Equals(ConvertDate(Value), listDate))
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, SELECTED);
                    }

                    ListData.Add(listItem);
                    start = start.Add(interval);
                }
            }
        }

        /// <summary>
        /// Method used to convert the date value to generic type.
        /// </summary>
        /// <param name="dateValue">The dateValue<see cref="DateTime"/>.</param>
        /// <exclude/>
        protected override TValue ConvertGeneric(DateTime dateValue)
        {
            if (IsDateTimeOffsetType())
            {
                return (TValue)SfBaseUtils.ChangeType(new DateTimeOffset(dateValue), typeof(TValue));
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
            }
        }

        internal override void ComponentDispose()
        {
            var destroyArgs = new object[] { InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetClientProperties() };
            InvokeMethod("sfBlazor.DatePicker.destroy", destroyArgs).ContinueWith(t =>
            {
                _ = SfBaseUtils.InvokeEvent<object>(DatetimepickerEvents?.Destroyed, null);
            }, TaskScheduler.Current);
            DateIcon = null;
            TimeIcon = null;
            PopupEventArgs = null;
            DatetimepickerEvents = null;
            ListData = null;
        }
    }
}
