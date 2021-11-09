using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Inputs.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DatePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>
    {

        internal DatePickerEvents<TValue> DatepickerEvents { get; set; }

        internal bool ClearBtnStopPropagation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isDevice.
        /// </summary>
        /// <exclude/>
        protected bool IsDevice { get; set; }

        /// <summary>
        /// Gets or sets the dateIcon.
        /// </summary>
        /// <exclude/>
        protected string DateIcon { get; set; }

        /// <summary>
        /// Gets or sets the calendarClass.
        /// </summary>
        /// <exclude/>
        protected string CalendarClass { get; set; }

        /// <summary>
        /// Gets or sets the popupElement.
        /// </summary>
        /// <exclude/>
        protected ElementReference PopupElement { get; set; }

        /// <summary>
        /// Gets or sets the popupHolderEle.
        /// </summary>
        /// <exclude/>
        protected ElementReference PopupHolderEle { get; set; }

        /// <summary>
        /// Gets or sets the modelYear.
        /// </summary>
        /// <exclude/>
        protected string ModelYear { get; set; }

        /// <summary>
        /// Gets or sets the modelDay.
        /// </summary>
        /// <exclude/>
        protected string ModelDay { get; set; }

        /// <summary>
        /// Gets or sets the modelMonth.
        /// </summary>
        /// <exclude/>
        protected string ModelMonth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether calendar is rendered.
        /// </summary>
        /// <exclude/>
        protected bool IsCalendarRendered { get; set; }

        /// <summary>
        /// Gets or sets the popupEventArgs.
        /// </summary>
        /// <exclude/>
        protected DatePickerPopupArgs PopupEventArgs { get; set; }

        /// <summary>
        /// Gets or sets the changedEventArgs.
        /// </summary>
        /// <exclude/>
        protected ChangedEventArgs<TValue> ChangedEventArgs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowPopupCalendar.
        /// </summary>
        /// <exclude/>
        protected bool ShowPopupCalendar { get; set; }

        private bool IsDateIconClicked { get; set; }

        /// <summary>
        /// Gets or sets the previousElementValue.
        /// </summary>
        /// <exclude/>
        protected string PreviousElementValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isCalendarRender.
        /// </summary>
        /// <exclude/>
        protected bool IsCalendarRender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isListRender.
        /// </summary>
        /// <exclude/>
        protected bool IsListRender { get; set; }

        /// <summary>
        /// Gets or sets the popupContainer.
        /// </summary>
        /// <exclude/>
        protected string PopupContainer { get; set; }

        /// <summary>
        /// Gets or sets the currentCulture.
        /// </summary>
        /// <exclude/>
        protected CultureInfo CurrentCulture { get; set; }

        internal CalendarBaseRender<TValue> CalendarBase { get; set; }

        /// <summary>
        /// Gets or sets the CalendarBaseInstance.
        /// </summary>
        /// <exclude/>
        protected CalendarBaseRender<TValue> CalendarBaseInstance { get; set; }

        protected DateTime TimePart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isDatePickerPopup.
        /// </summary>
        /// <exclude/>
        protected bool IsDatePickerPopup { get; set; } = true;

        private string ValidClass { get; set; }

        private bool IsCleared { get; set; }
        private string StrictValue { get; set; }
        private bool IsValideValue { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            PropertyInit();
            await base.OnInitializedAsync();
            DependencyScripts();
            PropertyInitialized();
            SfDateTimePicker<TValue> sfDateTimePicker = (this as SfDateTimePicker<TValue>);
            IsValideValue = true;
            if (sfDateTimePicker == null)
            {
                if (string.IsNullOrEmpty(ID))
                {
                    ID = "datepicker-" + Guid.NewGuid().ToString();
                }

                if (Value != null)
                {
                    await StrictModeUpdate(true);
                    await UpdateInput();
                }
            }

            if (DatePickerParent != null && Convert.ToString(DatePickerParent.Type, CultureInfo.CurrentCulture) == "Date")
            {
                Type parentType = DatePickerParent.GetType();
                DatePickerParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(DatePickerParent, this);
            }
        }

        private void PropertyInit()
        {
            RootClass = ROOT;
            ContainerClass = CONTAINER_CLASS;
            DateIcon = DATE_ICON;
            PopupContainer = POPUP_CONTAINER;

            // Unique class added for dynamically rendered Inplace-editor components
            if (DatePickerParent != null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, "e-editable-elements");
            }

            CalendarClass = CALENDAR_ROOT;
            CurrentCulture = GetDefaultCulture();
            ChangedEventArgs = new ChangedEventArgs<TValue>();
            IsDateIconClicked = false;
        }

        private void DependencyScripts()
        {
            ScriptModules = SfScriptModules.SfDatePicker;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.SfTextBox);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
        }

        /// <summary>
        /// Triggers if any of the component property get changed dynamically.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await PropertyParametersSet();
            SetRTL();
            SetDayHeaderFormat();
            SetAllowEdit();
            UpdateAriaAttributes();
            if (PropertyChanges.Count > 0)
            {
                await OnPropertyChange(PropertyChanges);  
            }
            SetCssClass();
            UpdateValidateClass();
        }

        private async Task OnPropertyChange(Dictionary<string, object> newProps)
        {
            var newProperties = newProps.ToList();
            foreach (var prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Format):
                        if (Value != null)
                        {
                            CurrentValueAsString = Intl.GetDateFormat(Value, GetDefaultFormat(), CalendarLocale);
                        }
                        break;
                    case nameof(Value):
                        UpdateErrorClass();
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, _cssClass);
                        PopupContainer = string.IsNullOrEmpty(PopupContainer) ? PopupContainer : SfBaseUtils.RemoveClass(PopupContainer, _cssClass);
                        _cssClass = CssClass;
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRendered();
                        break;
                    case nameof(StrictMode):
                        if  (Value != null)
                        {
                            await StrictModeUpdate();
                            await UpdateInput();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Triggered after the component is rendered.
        /// </summary>
        /// <param name="firstRender">The firstRender<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await ClientPopupRender();
            if (firstRender)
            {
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
                    localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                    if (!(localStorageValue == null && Value != null))
                    {
                        var persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue, typeof(TValue));
                        InputTextValue = persistValue;
                    }

                    await UpdateInput();
                }

                PreviousDate = Value;
                PreviousElementValue = CurrentValueAsString;
                await SfBaseUtils.InvokeEvent<object>(DatepickerEvents?.Created, null);
                IsValideValue = false;
            }
        }

        private void SetDayHeaderFormat()
        {
            CalendarClass = (DayHeaderFormat == DayHeaderFormats.Wide) ? SfBaseUtils.AddClass(CalendarClass, DAY_HEADER_WIDE) : SfBaseUtils.RemoveClass(CalendarClass, DAY_HEADER_WIDE);
        }

        private void SetRTL()
        {
            CalendarClass = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(CalendarClass, RTL) : SfBaseUtils.RemoveClass(CalendarClass, RTL);
            PopupContainer = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
        }

        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = (ContainerClass != null && !ContainerClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(ContainerClass, CssClass) : ContainerClass;
                PopupContainer = (PopupContainer != null && !PopupContainer.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupContainer, CssClass) : PopupContainer;
            }
        }

        internal async override Task OnAfterScriptRendered()
        {
            var options = GetClientProperties();
            await InvokeMethod("sfBlazor.TextBox.initialize", new object[] { InputElement, DotnetObjectReference, ContainerElement });
            await InvokeMethod("sfBlazor.DatePicker.initialize", new object[] { ContainerElement, InputElement, DotnetObjectReference, options });
            IsDevice = SyncfusionService.IsDeviceMode;
        }

        /// <summary>
        /// Method which updates the client properties.
        /// </summary>
        /// <returns>The <see cref="DatePickerClientProps{TValue}"/>.</returns>
        /// <exclude/>
        protected DatePickerClientProps<TValue> GetClientProperties()
        {
            return new DatePickerClientProps<TValue>
            {
                Readonly = Readonly,
                Enabled = Enabled,
                Locale = CalendarLocale,
                EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ZIndex = ZIndex,
                KeyConfigs = KeyConfigs,
                ShowClearButton = ShowClearButton,
                Value = Value,
                Width = Width,
                IsDatePopup = IsDatePickerPopup,
                AllowEdit = AllowEdit,
                Depth = Depth.ToString()
            };
        }

        internal void SetPopupVisibility(bool args)
        {
            ShowPopupCalendar = args;
        }

        internal override async Task UpdateCalendarProperty(string key, object dateTimeValue)
        {
            if (key == VALUE)
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

        private void PropertyInitialized()
        {
            _cssClass = CssClass;
            CalendarBase_Max = Max;
            CalendarBase_Min = Min;
            CalendarBase_Depth = Depth;
            CalendarBase_Value = Value;
            _format = Format;
            _locale = CalendarLocale;
            DateStrictMode = StrictMode;
        }

        private async Task PropertyParametersSet()
        {
            _format = NotifyPropertyChanges(FORMAT, Format, _format);
            NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            _locale = NotifyPropertyChanges(LOCALE, CalendarLocale, _locale);
            _value = NotifyPropertyChanges(nameof(Value), Value, _value);
            CalendarBase_Max = NotifyPropertyChanges(MAX, Max, CalendarBase_Max);
            CalendarBase_Min = NotifyPropertyChanges(MIN, Min, CalendarBase_Min);
            CalendarBase_Depth = NotifyPropertyChanges(DEPTH, Depth, CalendarBase_Depth);
            DateStrictMode = NotifyPropertyChanges(nameof(StrictMode), StrictMode, DateStrictMode);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Task used to update the value of the component.
        /// </summary>
        /// <param name="dateValue">Specifies the date value.<see cref="object"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task UpdateValue(object dateValue)
        {
            TValue tempValue = (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
            InputTextValue = tempValue;
            await Task.CompletedTask;
        }

        private string GetDefaultFormat()
        {
            var datePattern = CurrentCulture.DateTimeFormat.ShortDatePattern;
            var timePattern = datePattern + SPACE + CurrentCulture.DateTimeFormat.ShortTimePattern;
            var isDatePick = RootClass.Contains(DATE_PICKER, StringComparison.Ordinal);
            return !string.IsNullOrEmpty(Format) ? Format : isDatePick ? datePattern : timePattern;
        }

        /// <summary>
        /// Method to get the default cultureinfo.
        /// </summary>
        /// <returns>Cultureinfo.</returns>
        /// <exclude/>
        protected CultureInfo GetDefaultCulture()
        {
            string locale = string.IsNullOrEmpty(CalendarLocale) ? null : CalendarLocale;
            return Intl.GetCulture(locale);
        }
        protected override string FormatValueAsString(TValue formatValue)
        {
            if (formatValue != null)
            {
                var formatString = GetDefaultFormat();
                var dateFormatValue = Intl.GetDateFormat(formatValue, formatString);
                dateFormatValue = (StrictMode && !(IsFocused && ValidateOnInput)) && StrictValue != null ? StrictValue : dateFormatValue;
                StrictValue = null;
                return dateFormatValue;
            }
            else
            {
                return (!StrictMode || (IsFocused && ValidateOnInput)) ? StrictValue! : default;
            }
        }
        protected override TValue FormatValue(string genericValue)
        {
            TValue date = default!;
            if (string.IsNullOrEmpty(genericValue))
            {
                StrictValue = null;
                return date;
            }
            else
            {
                var format = GetDefaultFormat();
                var inputValue = genericValue.Trim();
                var isArabicCulture = CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
                var isThailandCulture = CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
                if (isArabicCulture || isThailandCulture)
                {
                    inputValue = !string.IsNullOrEmpty(inputValue) ? RemoveCultureDigits(isArabicCulture, inputValue) : inputValue;
                }

                if (IsTryParse(inputValue, format))
                {
                    date = ParseDate(inputValue, format);
                } else
                {
                    if (IsFocused && ValidateOnInput)
                    {
                        StrictValue = inputValue;
                    } else
                    {
                        Type type = typeof(TValue);
                        bool isNullable = Nullable.GetUnderlyingType(type) != null;
                        if (Value != null && (!isNullable || IsValideValue))
                        {
                            date = Value;
                        }
                        if (StrictMode && !SfBaseUtils.Equals(date, default))
                        {
                            StrictValue = inputValue;
                        }
                        else if (!StrictMode)
                        {
                            if (PreviousElementValue != inputValue && !IsValideValue)
                            {
                                date = default;
                                StrictValue = inputValue;
                            }
                        }

                        if (StrictMode && SfBaseUtils.Equals(date, default))
                        {
                            StrictValue = inputValue;
                            var dateValue = (Value == null || string.IsNullOrEmpty(inputValue)) ? default : Value;
                            date = dateValue;
                        }
                    }
                }
            }
            return date!;
        }
        /// <summary>
        /// Task which updates the strict mode.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task StrictModeUpdate(bool isInit = false)
        {
            var format = GetDefaultFormat();
            var inputValue = CurrentValueAsString?.Trim();
            var date = default(TValue);
            var isArabicCulture = CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            var isThailandCulture = CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
            if (isArabicCulture || isThailandCulture)
            {
                inputValue = !string.IsNullOrEmpty(inputValue) ? RemoveCultureDigits(isArabicCulture, inputValue) : inputValue;
            }

            if (IsTryParse(inputValue, format))
            {
                date = ParseDate(inputValue, format);
            }

            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            if (Value != null && (!isNullable || isInit))
            {
                date = Value;
                if (!isInit) 
                {
                    if (ValueChanged.HasDelegate)
                    {
                        await UpdateInputValue(Intl.GetDateFormat(date, format, CalendarLocale));
                    } else
                    {
                        InputTextValue = date;
                    }
                }
            }

            if ((StrictMode && !(IsFocused && ValidateOnInput)) && date != null)
            {
                await UpdateInputValue(Intl.GetDateFormat(date, format, CalendarLocale));
                if (PreviousElementValue != inputValue)
                {
                    await UpdateValue(date);
                }
            }
            else if (!StrictMode || (IsFocused && ValidateOnInput))
            {
                if (PreviousElementValue != inputValue)
                {
                    await UpdateValue(date);
                }
            }

            if ((StrictMode && !(IsFocused && ValidateOnInput)) && date == null)
            {
                var dateValue = (Value == null || string.IsNullOrEmpty(inputValue)) ? default : Value;
                await UpdateValue(dateValue);
            }
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
                return (TValue)SfBaseUtils.ChangeType(DateTime.ParseExact(dateValue, format, (IFormatProvider)CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal), type);
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(DateTimeOffset.ParseExact(dateValue, format, (IFormatProvider)CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal), type);
            }
        }

        /// <summary>
        /// Task used to update the popup state.
        /// </summary>
        /// <param name="isOpen">true if the popup is in opened state, otherwise false.</param>
        protected virtual void UpdateDateTimePopupState(bool isOpen)
        {
        }

        internal async Task ClosePopupElement()
        {
            if (IsDateIconClicked)
            {
                await FocusIn();
            }

            if (IsDevice)
            {
                inputAttr = RemoveAttr(READ_ONLY, inputAttr);
            }

            IsCalendarRender = false;
            SetPopupVisibility(false);
            UpdateDateTimePopupState(false);
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, inputAttr);
            await InvokeAsync(() => StateHasChanged());
            await Task.CompletedTask;
        }

        /// <summary>
        /// Method to trigger the client-side actions once the popup is displayed when date icon is clicked.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected virtual async Task ClientPopupRender()
        {
            if (ShowPopupCalendar && IsCalendarRendered)
            {
                IsCalendarRendered = false;
                var options = GetClientProperties();
                await InvokeMethod("sfBlazor.DatePicker.renderPopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, options });
                IsCalendarRender = true;
            }
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

#pragma warning disable CA1822 // Mark members as static
        private string RemoveCultureDigits(bool isArabic, string dateValue)
#pragma warning restore CA1822 // Mark members as static
        {
            string outDate = string.Empty;
            foreach (char item in dateValue)
            {
                char startVal = isArabic ? ARABIC_START_DIGIT : THAILAND_START_DIGIT;
                char endVal = isArabic ? ARABIC_END_DIGIT : THAILAND_END_DIGIT;
                outDate += (item >= startVal && item <= endVal) ? char.GetNumericValue(item).ToString(CultureInfo.CurrentCulture) : item.ToString();
            }

            return outDate;
        }

        /// <summary>
        /// Checks whether the value type is DateTime.
        /// </summary>
        /// <returns>True or false based on the Type.</returns>
        /// <exclude/>
#pragma warning disable CA1822 // Mark members as static
        protected bool IsDateTimeType()
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
        /// <exclude/>
#pragma warning disable CA1822 // Mark members as static
        protected bool IsDateTimeOffsetType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return type == typeof(DateTimeOffset) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Persists the value's time part to avoid unwanted change events.
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        private void MaintainTimePart(object dateValue)
#pragma warning restore CA1822 // Mark members as static
        {
            if (IsDateTimeType())
            {
                TimePart = (DateTime)dateValue;
            }
            else if (IsDateTimeOffsetType())
            {
                TimePart = ((DateTimeOffset)dateValue).DateTime;
            }
        }

        /// <summary>
        /// Triggers when the value of the component get changed.
        /// </summary>
        /// <param name="args">The <see cref="ChangeEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task ChangeHandler(ChangeEventArgs args)
        {
            StrictValue = string.IsNullOrEmpty((string)args?.Value) ? null : StrictValue;
            CurrentValueAsString = (string)args?.Value;
            IsCleared = false;
            await Task.CompletedTask;
        }

        protected override async Task InputHandler(ChangeEventArgs args)
        {
            IsValideValue = false;
            await Task.CompletedTask;
        }
        /// <summary>
        /// Triggers when the component get focused.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task InvokeFocusEvent(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            var focusEventArgs = new FocusEventArgs();
            await SfBaseUtils.InvokeEvent<FocusEventArgs>(DatepickerEvents?.Focus, focusEventArgs);
        }

        /// <summary>
        /// Triggers when the component get focused out.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task InvokeBlurEvent(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            var blurEventArgs = new BlurEventArgs();
            await SfBaseUtils.InvokeEvent<BlurEventArgs>(DatepickerEvents?.Blur, blurEventArgs);
        }

        /// <summary>
        /// Triggers when the component get focused.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            await InvokeFocusEvent(args);
        }

        /// <summary>
        /// Triggers when the component get focused out.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            DateIcon = SfBaseUtils.RemoveClass(DateIcon, ACTIVE);
            IsDateIconClicked = false;
            await StrictModeUpdate();
            if (Value == null && StrictMode)
            {
                await UpdateInputValue(null);
            }

            await UpdateInput();
            await ChangeTrigger(args);
            UpdateErrorClass();
            UpdateValidateClass(); 
            await InvokeBlurEvent(args);
        }
        protected async Task InvokeClearBtnEvent(EventArgs args)
        {
            if (!IsDevice)
            {
                ClearBtnStopPropagation = true;
            }
            IsCleared = true;
            await Task.Delay(10); // set the delay for update the cleared value to the input.
            await UpdateValue(null);
            await UpdateInputValue(null);
            var clearEventArgs = new ClearedEventArgs()
            {
                Event = args
            };
            await SfBaseUtils.InvokeEvent<ClearedEventArgs>(DatepickerEvents?.Cleared, clearEventArgs);
            await UpdateInput();
            UpdateValidateClass();
            await FocusIn();
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(new ChangeEventArgs() { Value = string.Empty });
            }
            ChangeEvent(args);
            if (IsCalendarRender)
            {
                await Hide(args);
            }
            IsCleared = false;
        }

        /// <summary>
        /// Method which updates the valid class based on the value .
        /// </summary>
        protected void UpdateValidateClass()
        {
            if (ValueExpression != null && InputEditContext != null)
            {
                var fieldIdentifier = FieldIdentifier.Create(ValueExpression);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
                ValidClass = InputEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.AddClass(ContainerClass, ValidClass) : ContainerClass;
                this.ContainerClass = Regex.Replace(this.ContainerClass, @"\s+", " ");
                if (ValidClass == INVALID || ValidClass == MODIFIED_INVALID)
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

        /// <summary>
        /// Triggers while mouse icon performs an action.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="iconName"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task MouseIconHandler(EventArgs args, string iconName)
        {
            if (args != null && iconName != null)
            {
                if (iconName.Contains(DATE_ICON, StringComparison.Ordinal))
                {
                    IsDatePickerPopup = true;
                    await DateIconHandler();
                }
                else
                {
                    IsDatePickerPopup = false;
                    await TimeIconHandler(args);
                }
            }
        }

        /// <summary>
        /// Handles the time icon process.
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task TimeIconHandler(EventArgs eventArgs)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers while popup get opened.
        /// </summary>
        /// <param name="isOpen">The isOpen<see cref="bool"/>.</param>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task{PopupObjectArgs}"/>.</returns>
        /// <exclude/>
        protected virtual async Task<PopupObjectArgs> InvokeOpenEvent(bool isOpen, EventArgs args = null)
        {
            var openEventArgs = new PopupObjectArgs
            {
                Cancel = false,
                Event = args,
                PreventDefault = false
            };
            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = args, PreventDefault = false };
            await SfBaseUtils.InvokeEvent<PopupObjectArgs>(isOpen ? DatepickerEvents?.OnOpen : DatepickerEvents?.OnClose, openEventArgs);
            return openEventArgs;
        }

        /// <summary>
        /// Handles the date icon process.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task DateIconHandler()
        {
            if (Enabled)
            {
                var isDisabled = (inputAttr != null ? inputAttr.ContainsKey("disabled") : false) || (HtmlAttributes != null ? HtmlAttributes.ContainsKey("disabled") : false);
                if (isDisabled)
                {
                    return;
                }

                if (IsDevice)
                {
                    await Task.Delay(10);      // add delay for update the read only attributes for device mode.
                    SfBaseUtils.UpdateDictionary(READ_ONLY, true, inputAttr);
                    await FocusOut();
                }

                IsDateIconClicked = true;
                if (!Readonly)
                {
                    if (IsCalendarRender)
                    {
                        await Hide();
                    }
                    else
                    {
                        if (IsListRender)
                        {
                            await HideTimePopup();
                            await Task.Delay(20);    // Added delay for take time to hide the time popup
                        }

                        await FocusIn();
                        await Task.Delay(5);    // Added delay for take time to hide the time popup
                        await Show();
                        ContainerClass = SfBaseUtils.AddClass(ContainerClass.Trim(), INPUT_FOCUS);
                        if (DateIcon != null)
                        {
                            DateIcon = SfBaseUtils.AddClass(DateIcon, ACTIVE);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method used to hide the time popup.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task HideTimePopup()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Method used to update the value in the input element.
        /// </summary>
        /// <param name="dateValue">The dateValue<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        private async Task UpdateInputValue(string dateValue)
        {
            await SetValue(dateValue, FloatLabelType, ShowClearButton);
        }

        /// <summary>
        /// Triggers when the value of the component get changed.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task InvokeChangeEvent(EventArgs args = null)
        {
            ChangedEventArgs = new ChangedEventArgs<TValue>()
            {
                Value = Value,
                Event = args,
                IsInteracted = args != null
            };
            await SfBaseUtils.InvokeEvent<ChangedEventArgs<TValue>>(DatepickerEvents?.ValueChange, ChangedEventArgs);
        }

        private async Task ChangeTrigger(EventArgs args = null)
        {
            if (CurrentValueAsString != PreviousElementValue)
            {
                if (CompareValue(PreviousDate, Value) != 0)
                {
                    ChangedArgs.Value = Value;
                    await InvokeChangeEvent(args);
                    if (EnablePersistence)
                    {
                        await SetLocalStorage(ID, Value);
                    }

                    PreviousElementValue = CurrentValueAsString;
                    PreviousDate = Value;
                }
            }
        }

        /// <summary>
        /// Triggers when the value of the component get changed.
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
                if (EnablePersistence)
                {
                    _ = SetLocalStorage(ID, Value);
                }

                _ = SfBaseUtils.InvokeEvent<ChangedEventArgs<TValue>>(DatepickerEvents?.ValueChange, ChangedEventArgs);
                PreviousDate = Value;
                PreviousElementValue = CurrentValueAsString;
            }
        }

        internal override async Task InvokeSelectEvent(SelectedEventArgs<TValue> args)
        {
            await SfBaseUtils.InvokeEvent<SelectedEventArgs<TValue>>(DatepickerEvents?.Selected, args);
        }

        /// <summary>
        /// Triggers when change event is triggered to update the selected value in input element .
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task SelectCalendar()
        {
            var date = string.Empty;
            IsValideValue = true;
            if (Value != null)
            {
                var formatString = GetDefaultFormat();
                date = Intl.GetDateFormat(ChangedArgs.Value, formatString, CalendarLocale);
            }

            if (!string.IsNullOrEmpty(date))
            {
                await UpdateInputValue(date);
            }
        }

        /// <summary>
        /// Method used to update the value in input element.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task UpdateInput()
        {
            if (Value == null)
            {
                await UpdateValue(null);
            } 
            else
            {
                if (StrictMode && !(IsFocused && ValidateOnInput))
                {
                    await MinMaxUpdates();
                }

                var dateValue = Value;
                var formatString = GetDefaultFormat();
                string dateString = Intl.GetDateFormat(Value, formatString);
                var checkValue = (ConvertDateValue(dateValue) >= Max) || (ConvertDateValue(dateValue) <= Min);
                if ((Max.Ticks >= ConvertDateValue(dateValue).Ticks && Min.Ticks <= ConvertDateValue(dateValue).Ticks) || (!StrictMode && checkValue))
                {
                    if (dateString != CurrentValueAsString)
                    {
                        await UpdateInputValue(dateString);
                    }
                }
            }

            await UpdateStrictModeValue();
            ChangedArgs.Value = Value;
            UpdateErrorClass();
            UpdateIconState();
        }

        private async Task UpdateStrictModeValue()
        {
            if (Value == null)
            {
                if (StrictMode && !(IsFocused && ValidateOnInput))
                {
                    await UpdateInputValue(null);
                }
                else if (!string.IsNullOrEmpty(CurrentValueAsString))
                {
                    await UpdateInputValue(CurrentValueAsString);
                }
            }
        }

        /// <summary>
        /// Method used to update the error class to the component.
        /// </summary>
        /// <exclude/>
        protected void UpdateErrorClass()
        {
            if ((Value != null && !(ConvertDateValue(Value) >= Min && ConvertDateValue(Value).Date <= Max.Date)) ||
                ((!StrictMode || (IsFocused && ValidateOnInput)) && !string.IsNullOrEmpty(CurrentValueAsString) && Value == null))
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

        /// <summary>
        /// Method update the properties Min and Max .
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        private async Task MinMaxUpdates()
        {
            if (CompareValue(ConvertGeneric(Min), Value) == 1 && Min <= Max && (StrictMode && !(IsFocused && ValidateOnInput)))
            {
                await UpdateValue(Min);
                ChangedArgs.Value = Value;
            }
            else if (CompareValue(Value, ConvertGeneric(Max)) == 1 && Min <= Max && (StrictMode && !(IsFocused && ValidateOnInput)))
            {
                await UpdateValue(Max);
                ChangedArgs.Value = Value;
            }
        }

        private void SetAllowEdit()
        {
            inputAttr = AllowEdit ? !Readonly ? RemoveAttr(READ_ONLY, inputAttr) :
                inputAttr : SfBaseUtils.UpdateDictionary(READ_ONLY, true, inputAttr);
        }

        internal override void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
            _ = SfBaseUtils.InvokeEvent<NavigatedEventArgs>(DatepickerEvents?.Navigated, eventArgs);
        }

        internal override async Task BindRenderDayEvent(RenderDayCellEventArgs eventArgs)
        {
            await SfBaseUtils.InvokeEvent<RenderDayCellEventArgs>(DatepickerEvents?.OnRenderDayCell, eventArgs);
            if (eventArgs.IsDisabled && eventArgs.Date.Date == DateTime.Now.Date && CalendarBaseInstance != null)
            {
                CalendarBaseInstance.TodayEleClass = SfBaseUtils.AddClass(CalendarBaseInstance.TodayEleClass, DISABLE);
            }
        }
#pragma warning disable CA1822 // Mark members as static
        private Dictionary<string, object> RemoveAttr(string removeClass, Dictionary<string, object> attr)
#pragma warning restore CA1822 // Mark members as static
        {
            attr.Remove(removeClass);
            return attr;
        }

        private void UpdateIconState()
        {
            ContainerClass = (!AllowEdit && !Readonly) ? string.IsNullOrEmpty(CurrentValueAsString) ?
                    SfBaseUtils.RemoveClass(ContainerClass, NOEDIT) : SfBaseUtils.AddClass(ContainerClass, NOEDIT) : SfBaseUtils.RemoveClass(ContainerClass, NOEDIT);
        }
#pragma warning disable CA1822 // Mark members as static
        private int CompareValue(TValue value1, TValue value2)
#pragma warning restore CA1822 // Mark members as static
        {
            return Comparer<TValue>.Default.Compare(value1, value2);
        }

        internal async Task InputKeyActionHandler(KeyActions args)
        {
            switch (args.Action)
            {
                case ALT_UP_ARROW:
                    await Hide(args.Events);
                    await FocusIn();
                    break;
                case ALT_DOWN_ARROW:
                    if (IsListRender)
                    {
                        await Hide();
                    }
                    if (!IsCalendarRender)
                    {
                        IsDatePickerPopup = true;
                        await StrictModeUpdate();
                        await UpdateInput();
                        await ChangeTrigger(args.Events);
                        await Show(args.Events);
                    }
                    break;
                case ESCAPE:
                    await Hide(args.Events);
                    break;
                case ENTER:
                    await StrictModeUpdate();
                    await UpdateInput();
                    await ChangeTrigger(args.Events);
                    UpdateErrorClass();
                    if (!IsCalendarRender)
                    {
                        await Hide(args.Events);
                    }

                    break;
                case TAB:
                case SHIFT_TAB:
                    await StrictModeUpdate();
                    await UpdateInput();
                    await ChangeTrigger(args.Events);
                    UpdateErrorClass();
                    if (IsCalendarRender)
                    {
                        await Hide(args.Events);
                    }
                    break;
                default:
                    if (CalendarBaseInstance != null)
                    {
                        await CalendarBaseInstance.KeyActionHandler(args);
                    }

                    break;
            }
        }

        private DateTime ConvertDateValue(TValue dateValue)
        {
            return IsDateTimeOffsetType() ? ((DateTimeOffset)SfBaseUtils.ChangeType(dateValue, typeof(TValue))).DateTime : (DateTime)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
        }

        /// <summary>
        /// Method allows to convert the value to generic type.
        /// </summary>
        /// <param name="dateValue">The dateValue<see cref="DateTime"/>.</param>
        /// <exclude/>
        protected virtual TValue ConvertGeneric(DateTime dateValue)
        {
            if (IsDateTimeOffsetType())
            {
                var year = ((DateTimeOffset)dateValue).Year;
                var month = ((DateTimeOffset)dateValue).Month;
                var day = ((DateTimeOffset)dateValue).Day;
                var offset = ((DateTimeOffset)dateValue).Offset;
                var offsetValue = new DateTimeOffset(year, month, day, TimePart.Hour, TimePart.Minute, TimePart.Second, TimePart.Millisecond, offset);
                return (TValue)SfBaseUtils.ChangeType(offsetValue, typeof(TValue));
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
            }
        }

        /// <summary>
        /// Triggers while disposing the component.
        /// </summary>
        /// <exclude/>
        internal override void ComponentDispose()
        {
            var destroyArgs = new object[] { InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetClientProperties() };
            InvokeMethod("sfBlazor.DatePicker.destroy", destroyArgs).ContinueWith(t =>
              {
                  _ = SfBaseUtils.InvokeEvent<object>(DatepickerEvents?.Destroyed, null);
              }, TaskScheduler.Current);
            DateIcon = null;
            PopupEventArgs = null;
            DatepickerEvents = null;
        }
    }
}
