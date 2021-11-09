using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// TimePicker is an intuitive component which provides an options to select a time value from popup list or to set a desired time value.
    /// </summary>
    public partial class SfTimePicker<TValue> : SfInputTextBase<TValue>
    {
        private const string INVALID = "invalid";

        private const string MODIFIED_INVALID = "modified invalid";

        private const string MODIFIED_VALID = "modified valid";

        private const string ERROR_CLASS = "e-error";

        private const string SUCCESS_CLASS = "e-success";

        private const string CONTAINER_CLASS = "e-time-wrapper";

        private const string ROOT = "e-control e-timepicker e-lib";

        private const string POPUP = "e-popup";

        private const string NON_EDIT = "e-non-edit";

        private const string TIME_ICON = "e-time-icon e-icons";

        private const string FALSE = "false";

        private const string TRUE = "true";

        private const string ARIA_HAS_POPUP = "aria-haspopup";

        private const string ARIA_ACTIVE_DESCENDANT = "aria-activedescendant";

        private const string NULL_VALUE = "null";

        private const string ARIA_OWN = "aria-owns";

        private const string OPTIONS = "_options";

        private const string ROLE = "role";

        private const string COMBOBOX = "combobox";

        private const string AUTO_CORRECT = "autocorrect";

        private const string OFF = "off";

        private const string SPELL_CHECK = "spellcheck";

        private const string ARIA_INVALID = "aria-invalid";

        private const string ARIA_AUTOCOMPLETE = "aria-autocomplete";

        private const string LIST = "list";

        private const string AUTO_CAPITAL = "autocapitalize";

        private const string ARIA_EXPANDED = "aria-expanded";

        private const string POPUP_CONTENT = "e-content";

        private const string DISABLED = "e-disabled";

        private const string RTL = "e-rtl";

        private const string READ_ONLY = "readonly";

        private const string INPUT_FOCUS = "e-input-focus";

        private const string POPUP_CONTAINER = "e-popup-wrapper";

        private const string ACTIVE = "e-active";

        private const string TIME_PICKER = "timepicker";

        private const string LIST_ITEM = "e-list-item";

        private const string SELECTED = "e-active";

        private const string HOVER = "e-hover";

        private const string NAVIGATION = "e-navigation";

        private string TimeIcon { get; set; }

        private string PopupContainer { get; set; }

        private string PreviousElementValue { get; set; }

        private bool IsDevice { get; set; }

        internal TimePickerEvents<TValue> TimepickerEvents { get; set; }

        private bool ClearBtnStopPropagation { get; set; }

        private bool IsCleared { get; set; }

        private bool ShowPopupList { get; set; }

        private bool IsListRendered { get; set; }

        private bool IsListRender { get; set; }

        private DatePickerPopupArgs PopupEventArgs { get; set; }

        internal ElementReference PopupElement { get; set; }

        internal ElementReference PopupHolderEle { get; set; }

        private bool IsTimeIconClicked { get; set; }

        private CultureInfo CurrentCulture { get; set; }

        private bool ListUpdated { get; set; }

        private DateTime DatePart { get; set; }

        private List<ListOptions<TValue>> ListData { get; set; }

        private int? ActiveIndex { get; set; }

        private TValue PreviousDateTime { get; set; }

        private string ValidClass { get; set; }

        private bool IsNavigate { get; set; }
        private string currentInputValue { get; set; }
        /// <summary>
        /// Specifies the valid time value within a specified range or else it will resets to previous value.
        /// </summary>
        private string strictValue { get; set; }
        /// <summary>
        /// Specifies the boolean value whether the input value is valid.
        /// </summary>
        private bool isValideValue { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnInitializedAsync()
        {
            RootClass = ROOT;
            ContainerClass = CONTAINER_CLASS;
            PopupContainer = POPUP_CONTAINER;

            // Unique class added for dynamically rendered Inplace-editor components
            if (TimePickerParent != null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, "e-editable-elements");
            }

            TimeIcon = TIME_ICON;
            await base.OnInitializedAsync(); 
            DependencyScripts();
            PropertyInitialized();
            CurrentCulture = GetDefaultCulture();
            PreviousDateTime = Value;
            isValideValue = true;
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID(TIME_PICKER);
            }

            if (TimePickerParent != null && Convert.ToString(TimePickerParent.Type, CultureInfo.CurrentCulture) == "Time")
            {
                TimePickerParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(TimePickerParent, this);
            }
        }

        private void PropertyInitialized()
        {
            _enableRtl = EnableRtl;
            _keyConfigs = KeyConfigs;
            _scrollTo = ScrollTo;
            _step = Step;
            _format = Format;
            _width = Width;
            _zIndex = ZIndex;
            _cssClass = CssClass;
            _value = Value;
        }

        /// <summary>
        /// Triggers while dynamically changing the properties of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await PropertyParametersSet();
            SetRTL();
            SetTimeAllowEdit();
            UpdateAriaAttributes();
            await SetHtmlAttributes();
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
                    case nameof(Value):
                        await ChangeTrigger();
                        break;
                    case nameof(Format):
                        if (Value != null)
                        {
                            CurrentValueAsString = FormatDateValue(Value, GetDefaultFormat());
                        }
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, _cssClass);
                        PopupContainer = string.IsNullOrEmpty(PopupContainer) ? PopupContainer : SfBaseUtils.RemoveClass(PopupContainer, _cssClass);
                        _cssClass = CssClass;
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRendered();
                        break;
                }
            }
        }
        private void DependencyScripts()
        {
            ScriptModules = SfScriptModules.SfTimePicker;
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.SfTextBox);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
        }

        private void SetRTL()
        {
            PopupContainer = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
        }

        private void UpdateAriaAttributes()
        {
            SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, TRUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_AUTOCOMPLETE, LIST, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, NULL_VALUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + OPTIONS, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(ROLE, COMBOBOX, inputAttr);
            SfBaseUtils.UpdateDictionary(AUTO_CORRECT, OFF, inputAttr);
            SfBaseUtils.UpdateDictionary(AUTO_CAPITAL, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_INVALID, FALSE, inputAttr);
        }

        private async Task SetHtmlAttributes()
        {
            if (BaseHtmlAttributes != null)
            {
                foreach (var item in (Dictionary<string, object>)BaseHtmlAttributes)
                {
                    switch (item.Key)
                    {
                        case "step":
                            Step = Convert.ToInt32(item.Value, CultureInfo.CurrentCulture);
                            break;
                        case "min":
                            DateTime minValue;
                            if (DateTime.TryParse(item.Value.ToString(), GetDefaultCulture(), DateTimeStyles.None, out minValue))
                            {
                                Min = minValue;
                            }

                            break;
                        case "max":
                            DateTime maxValue;
                            if (DateTime.TryParse(item.Value.ToString(), GetDefaultCulture(), DateTimeStyles.None, out maxValue))
                            {
                                Max = maxValue;
                            }

                            break;
                        case "disabled":
                            if (item.Value.ToString() == "disabled" || item.Value.ToString() == "true")
                            {
                                Enabled = false;
                            }

                            break;
                        case "readonly":
                            if (item.Value.ToString() == "readonly" || item.Value.ToString() == "true")
                            {
                                Readonly = true;
                                AllowEdit = false;
                            }

                            break;
                        case "tabindex":
                            TabIndex = Convert.ToInt32(item.Value, CultureInfo.CurrentCulture);
                            break;
                        case "value":
                            await CheckValue(item.Value.ToString());
                            break;
                    }
                }
            }
        }

        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = (ContainerClass != null && !ContainerClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(ContainerClass, CssClass) : ContainerClass;
                PopupContainer = (PopupContainer != null && !PopupContainer.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupContainer, CssClass) : PopupContainer;
            }
        }

        private void SetTimeAllowEdit()
        {
            inputAttr = AllowEdit ? !Readonly ? RemoveAttributes(READ_ONLY, inputAttr) : inputAttr
                    : SfBaseUtils.UpdateDictionary(READ_ONLY, string.Empty, inputAttr);
        }
#pragma warning disable CA1822 // Mark members as static
        private Dictionary<string, object> RemoveAttributes(string removeClass, Dictionary<string, object> attr)
#pragma warning restore CA1822 // Mark members as static
        {
            attr.Remove(removeClass);
            return attr;
        }

        private async Task SetLocalStorage(string persistId, TValue dataValue)
        {
            if (EnablePersistence)
            {
                await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
            }
        }

        private void UpdateIconState()
        {
            ContainerClass = (!AllowEdit && !Readonly) ? string.IsNullOrEmpty(CurrentValueAsString) ?
                    SfBaseUtils.RemoveClass(ContainerClass, NON_EDIT) : SfBaseUtils.AddClass(ContainerClass, NON_EDIT) : SfBaseUtils.RemoveClass(ContainerClass, NON_EDIT);
        }

        private async Task PropertyParametersSet()
        {
            _format = NotifyPropertyChanges(nameof(Format), Format, _format);
            _enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, _enableRtl);
            _keyConfigs = NotifyPropertyChanges(nameof(KeyConfigs), KeyConfigs, _keyConfigs);
            _scrollTo = NotifyPropertyChanges(nameof(ScrollTo), ScrollTo, _scrollTo);
            _step = NotifyPropertyChanges(nameof(Step), Step, _step);
            _width = NotifyPropertyChanges(nameof(Width), Width, _width);
            _zIndex = NotifyPropertyChanges(nameof(ZIndex), ZIndex, _zIndex);
            NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            _value = NotifyPropertyChanges(nameof(Value), Value, _value);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers after the component get rendered.
        /// </summary>
        /// <param name="firstRender">true if the component rendered for the first time.</param>
        /// <returns>The <see cref="Task"/>.</returns>
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

                PreviousElementValue = CurrentValueAsString;
                await SfBaseUtils.InvokeEvent<object>(TimepickerEvents?.Created, null);
                isValideValue = true;
            }
        }

        internal async override Task OnAfterScriptRendered()
        {
            var options = new TimePickerClientProps<TValue>
            {
                EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                ZIndex = ZIndex,
                KeyConfigs = KeyConfigs,
                Value = Value,
                Width = Width,
                Step = Step,
                ScrollTo = ScrollTo
            };
            await base.OnAfterScriptRendered();
            await InvokeMethod<bool>("sfBlazor.TimePicker.initialize", false, new object[] { ContainerElement, InputElement, DotnetObjectReference, options });
            IsDevice = SyncfusionService.IsDeviceMode;
        }
        protected override string FormatValueAsString(TValue formatValue)
        {
            if (formatValue != null)
            {
                if (StrictMode)
                {
                    MinMaxUpdates(formatValue);
                }
                var formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortTimePattern : Format;
                var dateFormatValue = FormatDateValue(formatValue, formatString);
                dateFormatValue = (StrictMode && !(IsFocused && ValidateOnInput)) && strictValue != null ? strictValue : dateFormatValue;
                strictValue = null;
                return dateFormatValue;
            } else
            {
                
                return (!StrictMode || (IsFocused && ValidateOnInput)) ? strictValue! : default!;
            }
            
        }
        protected override TValue FormatValue(string genericValue = null)
        {
            if (string.IsNullOrEmpty(genericValue))
            {
                strictValue = null;
                return default;
            } else
            {
                var format = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortTimePattern;
                var inputValue = genericValue.Trim();
                var date = default(TValue);
                if (IsTryParse(inputValue, format))
                {
                    if (StrictMode)
                    {
                        MinMaxUpdates(Value);
                    }
                    date = CreateDateObj(inputValue, Value);
                }  else
                {
                    if (IsFocused && ValidateOnInput)
                    {
                        strictValue = inputValue;
                    } else
                    {
                        Type type = typeof(TValue);
                        bool isNullable = Nullable.GetUnderlyingType(type) != null;
                        if (Value != null && (!isNullable || isValideValue))
                        {
                            date = Value;
                        }

                        if (StrictMode && !SfBaseUtils.Equals(date, default))
                        {
                            strictValue = inputValue;
                        }
                        else if ((!StrictMode || (IsFocused && ValidateOnInput)) && (PreviousElementValue != inputValue) && !isValideValue)
                        {
                            strictValue = inputValue;
                            UpdateValue(date);
                        }

                        if (StrictMode && date == null && string.IsNullOrEmpty(inputValue))
                        {
                            UpdateValue(null);
                        }
                    }
                }
                return date;
            }
        }
        protected override async Task InputHandler(ChangeEventArgs args)
        {
            isValideValue = false;
            await Task.CompletedTask;
        }
        private async Task ClientPopupRender()
        {
            if (ShowPopupList && IsListRendered)
            {
                IsListRendered = false;
                var options = new TimePickerClientProps<TValue>
                {
                    EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                    ZIndex = ZIndex,
                    KeyConfigs = KeyConfigs,
                    Value = Value,
                    Width = Width,
                    Step = Step,
                    ScrollTo = ScrollTo
                };
                await InvokeMethod("sfBlazor.TimePicker.renderPopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, options });
                IsListRender = true;
            }
        }

        protected override async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            await InvokeMethod("sfBlazor.TimePicker.selectInputText", new object[] { InputElement });
            await SfBaseUtils.InvokeEvent<FocusEventArgs>(TimepickerEvents?.Focus, new FocusEventArgs());
        }

        protected override async Task ChangeHandler(ChangeEventArgs args)
        {
            var changeValue = args != null ? (string)args.Value : null;
            strictValue = string.IsNullOrEmpty(changeValue) ? null : strictValue;
            currentInputValue = changeValue;
            if (!IsCleared)
            {
                await SetValue(changeValue, FloatLabelType, ShowClearButton);
            }

            IsCleared = false;
            await Task.CompletedTask;
        }

        protected override async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
            IsTimeIconClicked = false;
            await StrictModeUpdate();
            if (string.IsNullOrEmpty(currentInputValue) && Value == null)
            {
                await UpdateInputValue(null);
            }

            await UpdateInput(true);
            await ChangeTrigger(args);
            if (IsListRender)
            {
                await Hide(args);
            }
            await SfBaseUtils.InvokeEvent<BlurEventArgs>(TimepickerEvents?.Blur, new BlurEventArgs());
        }

        private async Task StrictModeUpdate()
        {
            var format = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortTimePattern;
            var inputValue = currentInputValue?.Trim();
            var date = default(TValue);
            if (IsTryParse(inputValue, format))
            {
                date = CreateDateObj(inputValue);
            }

            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            if (Value != null && string.IsNullOrEmpty(inputValue) && !isNullable)
            {
                date = Value;
                await UpdateInputValue(FormatDateValue(date, format));
            }

            if ((StrictMode && !(IsFocused && ValidateOnInput)) && date != null)
            {
                await UpdateInputValue(FormatDateValue(date, format));
                if (PreviousElementValue != inputValue)
                {
                    UpdateValue(date);
                }
            }
            else if ((!StrictMode || (IsFocused && ValidateOnInput)) && (PreviousElementValue != inputValue))
            {
                UpdateValue(date);
            }

            if ((StrictMode && !(IsFocused && ValidateOnInput)) && date == null && string.IsNullOrEmpty(inputValue))
            {
                UpdateValue(null);
            }
        }

        private async Task InvokeClearBtnEvent(EventArgs args)
        {
            if (!IsDevice)
            {
                ClearBtnStopPropagation = true;
            }

            IsCleared = true;
            currentInputValue = null;
            UpdateValue(null);
            await UpdateInputValue(null);
            await SfBaseUtils.InvokeEvent<ClearedEventArgs>(TimepickerEvents?.Cleared, new ClearedEventArgs() { Event = args });
            await FocusIn();
            await ChangeTrigger(args);
            await Hide(args);
        }

        private async Task MouseIconHandler(EventArgs eventArgs)
        {
            await TimeIconHandler(eventArgs);
        }
        private async Task TimeIconHandler(EventArgs args = null)
        {
            if (Enabled)
            {
                await Task.Delay(10); // set the delay for prevent the icon click action.
                if (IsDevice)
                {
                    SfBaseUtils.UpdateDictionary(READ_ONLY, string.Empty, inputAttr);
                    await FocusOut();
                }

                IsTimeIconClicked = true;
                if (IsListRender)
                {
                    await Hide(args);
                }
                else
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUT_FOCUS);
                    if (TimeIcon != null)
                    {

                        TimeIcon = SfBaseUtils.AddClass(TimeIcon, ACTIVE);
                    }
                    await FocusIn();
                    await Show(args);
                }
            }
        }

        private async Task GenerateList()
        {
            if (Step > 0)
            {
                TimeSpan start = Min.TimeOfDay;
                TimeSpan end = Max.TimeOfDay;
                TimeSpan interval = new TimeSpan(0, Step, 0);
                ListData = new List<ListOptions<TValue>>();
                var formatString = GetDefaultFormat();
                while (end >= start)
                {
                    var listDateTime = new DateTime(DatePart.Year, DatePart.Month, DatePart.Day, start.Hours, start.Minutes, start.Seconds, start.Milliseconds, DatePart.Kind);
                    var timeFormatValue = IsTimeSpanType() ? Intl.GetDateFormat(listDateTime.TimeOfDay, formatString, TimePickerLocale) : Intl.GetDateFormat(listDateTime, formatString, TimePickerLocale);
                    var listItem = new ListOptions<TValue>()
                    {
                        DateTimeValue = ConvertGeneric(listDateTime),
                        ItemData = timeFormatValue,
                        ListClass = LIST_ITEM
                    };
                    var itemEventArgs = new ItemEventArgs<TValue>
                    {
                        Name = "OnItemRender",
                        Value = listItem.DateTimeValue,
                        Text = listItem.ItemData,
                        IsDisabled = false
                    };
                    await SfBaseUtils.InvokeEvent<ItemEventArgs<TValue>>(TimepickerEvents?.OnItemRender, itemEventArgs);
                    if (itemEventArgs.IsDisabled)
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, DISABLED);
                    }

                    ListData.Add(listItem);
                    start = start.Add(interval);
                }

                ListUpdated = true;
            }
        }

        private string GetDefaultFormat()
        {
            CultureInfo currentCulture = GetDefaultCulture();
            return string.IsNullOrEmpty(Format) ? currentCulture.DateTimeFormat.ShortTimePattern : Format;
        }

        private CultureInfo GetDefaultCulture()
        {
            string locale = string.IsNullOrEmpty(TimePickerLocale) ? null : TimePickerLocale;
            return Intl.GetCulture(locale);
        }
        private async Task ClosePopupAction()
        {
            if (IsTimeIconClicked)
            {
                await FocusIn();
            }

            IsListRender = false;
            ShowPopupList = false;

            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, inputAttr);
            await InvokeAsync(() => StateHasChanged());
        }

        private async Task ListItemClick(ListOptions<TValue> item, EventArgs args = null)
        {
            isValideValue = true;
            if (!item.ListClass.Contains(DISABLED, StringComparison.Ordinal) && !item.ListClass.Contains(SELECTED, StringComparison.Ordinal))
            {
                UpdateListSelection(item.ItemData, SELECTED);
                await CheckValue(item.ItemData, args);
            }

            if (IsListRender || item.ListClass.Contains(DISABLED, StringComparison.Ordinal) || item.ListClass.Contains(SELECTED, StringComparison.Ordinal))
            {
                await Hide(args);
            }
        }

        private void OnMouseOut()
        {
            var hoverData = ListData.Where(listItem => listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture)).FirstOrDefault();
            if (hoverData != null)
            {
                hoverData.ListClass = SfBaseUtils.RemoveClass(hoverData.ListClass, HOVER);
            }
        }

        private void OnMouseOver(ListOptions<TValue> listItem)
        {
            var hoverData = ListData.Where(listItem => listItem.ListClass.Contains(HOVER, StringComparison.CurrentCulture)).FirstOrDefault();
            if (hoverData != null)
            {
                hoverData.ListClass = SfBaseUtils.RemoveClass(hoverData.ListClass, HOVER);
            }

            listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, HOVER);
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
                    navData.ListClass = SfBaseUtils.RemoveClass(navData.ListClass, NAVIGATION);
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

        private async Task CheckValue(string inputValue, EventArgs args = null)
        {
            var timeValue = default(TValue);
            if (!string.IsNullOrEmpty(inputValue))
            {
                timeValue = GetDateObject(inputValue);
            }

            UpdateValue(timeValue);
            await SelectTimeList();
            if (args != null)
            {
                await SfBaseUtils.InvokeEvent<SelectedEventArgs<TValue>>(TimepickerEvents?.Selected, new SelectedEventArgs<TValue>() { Value = timeValue });
            }

            await ChangeTrigger(args);
        }

        private async Task ChangeTrigger(EventArgs args = null)
        {
            var inputValue = CurrentValueAsString;
            if (inputValue != PreviousElementValue && SfBaseUtils.CompareValues<TValue>(PreviousDateTime, Value) != 0)
            {
                var changedEventArgs = new ChangeEventArgs<TValue>()
                {
                    Value = Value,
                    Event = args,
                    IsInteracted = args != null,
                    Text = inputValue,
                    Element = InputElement
                };
                await SfBaseUtils.InvokeEvent<ChangeEventArgs<TValue>>(TimepickerEvents?.ValueChange, changedEventArgs);
                PreviousElementValue = inputValue;
                PreviousDateTime = Value;
                await SetLocalStorage(ID, Value);
            }

            UpdateErrorClass();
            UpdateValidateClass();
        }

        private async Task SelectTimeList()
        {
            var date = string.Empty;
            if (Value != null)
            {
                var formatString = !string.IsNullOrEmpty(Format) ? Format : CurrentCulture.DateTimeFormat.ShortTimePattern;
                date = FormatDateValue(Value, formatString);
            }

            await UpdateInputValue(date);
        }

        private string FormatDateValue(TValue timeValue, string formatString)
        {
            return timeValue != null ? Intl.GetDateFormat(timeValue, formatString, TimePickerLocale) : null;
        }

        private async Task UpdateInput(bool isInit = false)
        {
            var dateValue = isInit ? Value : PreviousDateTime;
            if (dateValue != null)
            {
                if (StrictMode && !(IsFocused && ValidateOnInput))
                {
                    MinMaxUpdates(dateValue);
                }
                var formatString = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortTimePattern : Format;
                string dateString = FormatDateValue(dateValue, formatString);
                await UpdateInputValue(dateString);
                var checkValue = (ConvertDate(dateValue) >= Max) || (ConvertDate(dateValue) <= Min);
                if ((SfBaseUtils.CompareValues<TValue>(ConvertGeneric(Max), dateValue) == 1 && SfBaseUtils.CompareValues<TValue>(dateValue, ConvertGeneric(Min)) == 1) || ((!StrictMode || (IsFocused && ValidateOnInput)) && checkValue))
                {
                    await UpdateInputValue(dateString);
                }
            } else
            {
                UpdateValue(null);
                if (StrictMode && !(IsFocused && ValidateOnInput))
                {
                    await UpdateInputValue(null);
                }
                else if (!string.IsNullOrEmpty(CurrentValueAsString))
                {
                    await UpdateInputValue(CurrentValueAsString);
                }
            }

            UpdateErrorClass();
            UpdateIconState();
        }

        private void MinMaxUpdates(TValue timeValue)
        {
            if (timeValue != null && SfBaseUtils.CompareValues<TValue>(ConvertGeneric(Min), timeValue) == 1 && Min <= Max && StrictMode)
            {
                UpdateValue(Min);
            }
            else if (timeValue != null && SfBaseUtils.CompareValues<TValue>(timeValue, ConvertGeneric(Max)) == 1 && Min <= Max && StrictMode)
            {
                UpdateValue(Max);
            }
        }

        private void UpdateValue(object timeValue)
        {
            TValue tempValue = (TValue)SfBaseUtils.ChangeType(timeValue, typeof(TValue));
            InputTextValue = tempValue;
        }

        private async Task UpdateInputValue(string timeValue)
        {
            currentInputValue = timeValue;
            await SetValue(timeValue, FloatLabelType, ShowClearButton);
        }

        private void UpdateErrorClass()
        {
            if ((Value != null && !(ConvertDate(Value) >= Min && ConvertDate(Value) <= Max)) ||
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

        private TValue GetDateObject(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var dateValue = CreateDateObj(text);
                var isValue = Value != null;
                if (dateValue != null)
                {
                    var day = isValue ? ConvertDate(Value).Day : DateTime.Now.Day;
                    var month = isValue ? ConvertDate(Value).Month : DateTime.Now.Month;
                    var year = isValue ? ConvertDate(Value).Year : DateTime.Now.Year;
                    var dateVal = ConvertDate(dateValue);
                    var dateTimeVal = new DateTime(year, month, day, dateVal.Hour, dateVal.Minute, dateVal.Second, dateVal.Millisecond, dateVal.Kind);
                    return ConvertGeneric(dateTimeVal);
                }
            }

            return default(TValue);
        }

        private DateTime ConvertDate(TValue timeValue)
        {
            if (IsDateTimeOffsetType())
            {
                var offsetValue = (DateTimeOffset)SfBaseUtils.ChangeType(timeValue, typeof(TValue));
                return offsetValue.DateTime;
            }
            else if (IsDateTimeType())
            {
                return (DateTime)SfBaseUtils.ChangeType(timeValue, typeof(TValue));
            }
            else
            {
                return DateTime.Now.Date + (TimeSpan)SfBaseUtils.ChangeType(timeValue, typeof(TValue));
            }
        }

        private TValue ConvertGeneric(DateTime timeValue)
        {
            if (IsDateTimeOffsetType())
            {
                var offsetDate = (DateTimeOffset)timeValue;
                var offsetValue = new DateTimeOffset(offsetDate.Year, offsetDate.Month, offsetDate.Day, offsetDate.Hour, offsetDate.Minute, offsetDate.Second, offsetDate.Millisecond, offsetDate.Offset);
                return (TValue)SfBaseUtils.ChangeType(offsetValue, typeof(TValue));
            }
            else if (IsDateTimeType())
            {
                return (TValue)SfBaseUtils.ChangeType(timeValue, typeof(TValue));
            }
            else if (IsTimeSpanType())
            {
                return (TValue)SfBaseUtils.ChangeType(timeValue.TimeOfDay, typeof(TValue));
            }

            return default;
        }

        private TValue CreateDateObj(string val, TValue defaultTimeValue = default)
        {
            var formatString = CurrentCulture.DateTimeFormat.ShortDatePattern;
            var timeValue = default(TValue);
            var timeItemValue = defaultTimeValue != null ? ConvertDate(defaultTimeValue) : DateTime.Now;
            var today = Intl.GetDateFormat(timeItemValue, formatString, TimePickerLocale);
            if (val.ToUpperInvariant().IndexOf("AM", StringComparison.Ordinal) > -1 || val.ToUpperInvariant().IndexOf("PM", StringComparison.Ordinal) > -1)
            {
                DateTime dateTime;
                if (DateTime.TryParse(today + SPACE + val, out dateTime))
                {
                    timeValue = ConvertGeneric(dateTime);
                }

                if (timeValue == null)
                {
                    timeValue = TimeParse(today, val);
                }
            }
            else
            {
                timeValue = TimeParse(today, val);
            }

            return timeValue;
        }

        private TValue TimeParse(string today, string val)
        {
            Type propertyType = typeof(TValue);
            var dateTimeFormat = CurrentCulture.DateTimeFormat.ShortDatePattern + SPACE + CurrentCulture.DateTimeFormat.ShortTimePattern;
            var formatTime = CurrentCulture.DateTimeFormat.ShortDatePattern + SPACE + Format;
            var formatString = string.IsNullOrEmpty(Format) ? dateTimeFormat : formatTime;
            if (IsDateTimeType())
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(today + SPACE + val, formatString, CurrentCulture, DateTimeStyles.None, out dateTime))
                {
                    return (TValue)SfBaseUtils.ChangeType(dateTime, propertyType);
                }
            }
            else if (IsDateTimeOffsetType())
            {
                DateTimeOffset offsetValue;
                if (DateTimeOffset.TryParseExact(today + SPACE + val, formatString, CurrentCulture, DateTimeStyles.None, out offsetValue))
                {
                    return (TValue)SfBaseUtils.ChangeType(offsetValue, propertyType);
                }
            }
            else if (IsTimeSpanType())
            {
                TimeSpan timeSpanValue;
                var timeformat = string.IsNullOrEmpty(Format) ? CurrentCulture.DateTimeFormat.ShortTimePattern : Format;
                if (TimeSpan.TryParseExact(val, timeformat, CurrentCulture, TimeSpanStyles.None, out timeSpanValue))
                {
                    return (TValue)SfBaseUtils.ChangeType(timeSpanValue, typeof(TValue));
                }
            }

            return default;
        }

        private bool IsTryParse(string timeValue, string format)
        {
            if (IsDateTimeType())
            {
                DateTime dateTimeVal;
                return DateTime.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dateTimeVal);
            }
            else if (IsDateTimeOffsetType())
            {
                DateTimeOffset dateTimeOffsetVal;
                return DateTimeOffset.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dateTimeOffsetVal);
            }
            else
            {
                TimeSpan timeSpanValue;
                return TimeSpan.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, out timeSpanValue);
   
            }
        }

        private TValue ParseDateTimeVal(string timeValue, string format)
        {
            Type propertyType = typeof(TValue);
            if (IsDateTimeType())
            {
                DateTime dateTimeVal;
                if (DateTime.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dateTimeVal))
                {
                    return (TValue)SfBaseUtils.ChangeType(dateTimeVal, propertyType);
                }
            }
            else
            {
                DateTimeOffset dateTimeOffsetVal;
                if (DateTimeOffset.TryParseExact(timeValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out dateTimeOffsetVal))
                {
                    return (TValue)SfBaseUtils.ChangeType(dateTimeOffsetVal, propertyType);
                }
            }

            return default;
        }

        private async Task KeyboardActions(KeyActions args)
        {
            if (!Readonly && Enabled)
            {
                switch (args.Action)
                {
                    case "home":
                    case "end":
                    case "up":
                    case "down":
                        await KeyHandler(args);
                        break;
                    case "enter":
                        var selectItem = IsNavigate ? ListData.ElementAtOrDefault((int)ActiveIndex)?.ItemData : CurrentValueAsString;
                        if (IsListRender)
                        {
                            UpdateListSelection(selectItem, SELECTED);
                        }

                        await CheckValue(selectItem, args.Events);
                        await Hide(args.Events);
                        await FocusIn();
                        await InvokeMethod("sfBlazor.TimePicker.selectInputText", new object[] { InputElement });
                        break;
                    case "open":
                        if (!IsListRender)
                            {
                            await Show(args.Events);
                        }

                        break;
                    case "escape":
                        await UpdateInput();
                        await Hide(args.Events);
                        break;
                    case "close":
                        await Hide(args.Events);
                        break;
                    default:
                        IsNavigate = false;
                        break;
                }
            }
        }

        private async Task KeyHandler(KeyActions args)
        {
            if (Step > 0 && ListData == null)
            {
                await GenerateList();
            }

            var listCount = ListData?.Count;
            if (string.IsNullOrEmpty(CurrentValueAsString) && Value == null && ActiveIndex == null)
            {
               var findItems = ListData.Where(item => !item.ListClass.Contains(DISABLED, StringComparison.CurrentCulture)).FirstOrDefault();
                ActiveIndex = (args.Action == "end") ? listCount - 1 : ListData.IndexOf(findItems);
            }
            else
            {
                var findItems = ListData.Where(item => item.ItemData == CurrentValueAsString).FirstOrDefault();
                var isFindItems = findItems != null;
                findItems = (!isFindItems && Value != null) ? ListData.Where(item => ConvertDate(item.DateTimeValue).Ticks >= ConvertDate(Value).Ticks).FirstOrDefault() : findItems;
                ActiveIndex = findItems != null ? ListData.IndexOf(findItems) : 0;
                ActiveIndex = (args.Action == "down") ? ActiveIndex < listCount - 1 ? (!isFindItems ? ActiveIndex : ++ActiveIndex) : ActiveIndex : (args.Action == "up") ? --ActiveIndex : (args.Action == "home") ? 0 : listCount - 1;
                var disabledItem = ListData.ElementAtOrDefault((int)ActiveIndex);
                if (disabledItem != null && disabledItem.ListClass.Contains(DISABLED, StringComparison.CurrentCulture))
                {
                    ActiveIndex = (args.Action == "down") ? ActiveIndex < listCount - 1 ? ++ActiveIndex : ActiveIndex : (args.Action == "up") ? --ActiveIndex : (args.Action == "home") ? 0 : listCount - 1;
                }
            }

            ActiveIndex = (ActiveIndex >= 0) ? ActiveIndex : ListData.Count + ActiveIndex;
            var selectItem = ListData.ElementAtOrDefault((int)ActiveIndex);
            if (selectItem != null)
            {
                if (IsListRender)
                {
                    var navData = ListData.Where(listItem => listItem.ListClass.Contains(NAVIGATION, StringComparison.CurrentCulture)).FirstOrDefault();
                    if (navData != null)
                        {
                        navData.ListClass = SfBaseUtils.RemoveClass(navData.ListClass, NAVIGATION);
                    }

                    selectItem.ListClass = SfBaseUtils.AddClass(selectItem.ListClass, NAVIGATION);
                }

                await UpdateInputValue(selectItem.ItemData);
                UpdateValidateClass();
                await InvokeMethod("sfBlazor.TimePicker.selectInputText", new object[] { InputElement, true, ActiveIndex });
            }

            IsNavigate = true;
        }
#pragma warning disable CA1822 // Mark members as static
        private bool IsDateTimeType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type propertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) != null;
            return propertyType == typeof(DateTime) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(propertyType));
        }

#pragma warning disable CA1822 // Mark members as static
        private bool IsTimeSpanType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type propertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) != null;
            return propertyType == typeof(TimeSpan) || (isNullable && typeof(TimeSpan) == Nullable.GetUnderlyingType(propertyType));
        }

#pragma warning disable CA1822 // Mark members as static
        private bool IsDateTimeOffsetType()
#pragma warning restore CA1822 // Mark members as static
        {
            Type propertyType = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) != null;
            return propertyType == typeof(DateTimeOffset) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(propertyType));
        }

        private void UpdateValidateClass()
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

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                var options = new TimePickerClientProps<TValue>
                {
                    EnableRtl = EnableRtl || SyncfusionService.options.EnableRtl,
                    ZIndex = ZIndex,
                    KeyConfigs = KeyConfigs,
                    Value = Value,
                    Width = Width,
                    Step = Step,
                    ScrollTo = ScrollTo
                };
                var destroyArgs = new object[] { InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, options };
                InvokeMethod("sfBlazor.TimePicker.destroy", destroyArgs).ContinueWith(t =>
                {
                    _ = SfBaseUtils.InvokeEvent<object>(TimepickerEvents?.Destroyed, null);
                }, TaskScheduler.Current);
                TimeIcon = null;
                PopupEventArgs = null;
                TimepickerEvents = null;
            }
        }
    }
}
