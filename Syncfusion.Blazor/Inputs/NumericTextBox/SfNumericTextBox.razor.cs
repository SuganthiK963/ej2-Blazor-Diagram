using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Inputs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection;
using System.Linq;
using System.Threading;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The NumericTextBox is used to get the number inputs from the user. The input values can be incremented or decremented by a predefined step value.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of SfNumericTextBox.</typeparam>
    public partial class SfNumericTextBox<TValue> : SfInputTextBase<TValue>
    {
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string BLUR = "Blur";
        private const string VALUECHANGE = "ValueChange";
        private const string STEP = "Step";
        private const string MIN = "Min";
        private const string MAX = "Max";
        private const string CONTAINER_CLASS = "e-numeric";
        private const string ROOT_CLASS = "e-control e-numerictextbox e-lib";
        private const string ROLE = "role";
        private const string SPIN_BUTTON = "spinbutton";
        private const string ARIA_LIVE = "aria-live";
        private const string ASSERTIVE = "assertive";
        private const string INCREMENT_CONTENT = "increment";
        private const string ADD = "add";
        private const string SUB = "sub";
        private const string ARIA_INVALID = "aria-invalid";
        private const string ARIA_VALUE_NOW = "aria-valuenow";
        private const string ARIA_VALUE_MIN = "aria-valuemin";
        private const string ARIA_VALUE_MAX = "aria-valuemax";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string MIN_VALUE = "MinValue";
        private const string MAX_Value = "MaxValue";
        private const string ENABLED = "Enabled";
        private const string VALIDATE_DECIMAL_TYPE = "ValidateDecimalOnType";
        private const string READ_ONLY = "Readonly";
        private const string SHOW_SPIN_BUTTON = "ShowSpinButton";
        private const string DECIMALS = "Decimals";
        private const string ARABIC = "ar";
        private const string THAILAND = "th";
        private const string PERSIAN = "fa";
         

        internal NumericTextBoxEvents<TValue> NumericEvents { get; set; }

        private bool ClearBtnStopPropagation { get; set; }

        /// <summary>
        /// Specifies the class value that is appended to container of TextBox.
        /// </summary>
        /// <exclude/>
        protected override string ContainerClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Set the component class to element.
        /// </summary>
        /// <exclude/>
        protected override string RootClass { get; set; } = "e-control e-numerictextbox e-lib";

        private TValue PrevValue { get; set; }

        private bool IsDevice { get; set; }

        /// <summary>
        /// Specifies the input is focused state.
        /// </summary>
        private bool IsFocus { get; set; }

        private bool IsPrevFocused { get; set; }

        private bool IsValidState { get; set; }

        private bool IsSpinButtonChanged { get; set; }

        private bool isNumberCulture { get; set; }

        private string ValidClass { get; set; }

        private string FocusInputValue { get; set; }
        private bool IsPasteValue { get; set; }
        /// <summary>
        /// Set the min and max validation value to the property.
        /// </summary>
        private string minMaxValue { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = "numeric-" + Guid.NewGuid().ToString();
            }

            RootClass = ROOT_CLASS;
            ContainerClass = CONTAINER_CLASS;
            await base.OnInitializedAsync();
            isNumberCulture = CultureInfo.CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal) ||
                CultureInfo.CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal) ||
                CultureInfo.CurrentCulture.Name.StartsWith(PERSIAN, StringComparison.Ordinal);
            PropertyInitialized();
            ScriptModules = SfScriptModules.SfNumericTextBox;
            DependentScripts.Add(Blazor.Internal.ScriptModules.SfTextBox);
            InvokeInputEvent();
            SetCssClass();
            UpdateDecimalType();
            ValidateMinMax();
            ValidateStep();
            await ChangeValue((Value == null) ? default : StrictMode ? TrimValue(Value) : Value);
            if (NumericTextBoxParent != null && Convert.ToString(NumericTextBoxParent.Type, CultureInfo.CurrentCulture) == "Numeric")
            {
                NumericTextBoxParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(NumericTextBoxParent, this);
            }
        }
        /// <summary>
        /// Set the css class to component container element.
        /// </summary>
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = ContainerClass.Contains(CssClass, StringComparison.Ordinal) ? ContainerClass : SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }
        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await PropertyUpdate();
            BaseInputAttributes = SfBaseUtils.UpdateDictionary(ROLE, SPIN_BUTTON, BaseInputAttributes);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await OnPropertyChanged(PropertyChanges);
            }
            await base.OnParametersSetAsync();
            UpdateValidateClass();
            BaseInputAttributes = SfBaseUtils.UpdateDictionary(ARIA_LIVE, ASSERTIVE, BaseInputAttributes);
        }
        private async Task OnPropertyChanged(Dictionary<string, object> newProps)
        {
            var newProperties = newProps.ToList();
            foreach (var prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                    case nameof(Format):
                        this.PrevValue = (this.PropertyChanges.ContainsKey(nameof(Value)) && this.PrevChanges != null && this.PrevChanges.Any()) ? (TValue)this.PrevChanges[nameof(Value)] : PrevValue;
                        this.PropertyChanges.Remove(nameof(Value));
                        await ChangeValue((Value == null) ? default : StrictMode ? TrimValue(Value) : Value);
                        break;
                    case nameof(Min):
                    case nameof(Max):
                        ValidateMinMax();
                        if (!StrictMode)
                        {
                            ValidateState();
                        }
                        break;
                    case nameof(Step):
                        ValidateStep();
                        break;
                    case nameof(Enabled):
                    case nameof(Decimals):
                    case nameof(Readonly):
                    case nameof(ValidateDecimalOnType):
                        await InvokeMethod("sfBlazor.NumericTextBox.propertyChanges", new object[] { InputElement, new NumericClientProps { Readonly = Readonly, Enabled = Enabled, Locale = CultureInfo.CurrentCulture.Name, ValidateDecimalOnType = ValidateDecimalOnType, Decimals = Decimals, DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator } });
                        break;
                    case nameof(ShowSpinButton):
                        IsSpinButtonChanged = true;
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, _cssClass);
                        _cssClass = CssClass;
                        SetCssClass();
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRendered();
                        break;
                }
            }
        }

        protected override string FormatValueAsString(TValue formatValue)
        {
            if (formatValue != null)
            {
                formatValue = StrictMode ? TrimValue(formatValue) : formatValue;
                var value = FormatNumber();
                var isExponential = Convert.ToString(formatValue, CultureInfo.CurrentCulture).Contains("E", StringComparison.Ordinal);
                var isNumberFormat = Format != null && Format.ToLower(CultureInfo.CurrentCulture).Contains("n", StringComparison.Ordinal);
                var formatString = (isExponential && isNumberFormat) ? null : Format;
                var elemValue = IsFocus ? SfBaseUtils.RemoveClass(value, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
                        : Intl.GetNumericFormat<TValue>(formatValue, formatString, NumericLocale, Currency);
                if (IsIgnoreDecimal() && IsFocus)
                {
                    elemValue = SfBaseUtils.RemoveClass(elemValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
                SfBaseUtils.UpdateDictionary(ARIA_VALUE_NOW, value, BaseInputAttributes);
                return elemValue;
            } else
            {
                return default!;
            }
            
        }
        protected override TValue FormatValue(string genericValue)
        {
            if (IsFocus)
            {
                var inputTextValue = genericValue;
                FocusInputValue = inputTextValue;
                inputTextValue = (isNumberCulture) ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                var inputValue = string.IsNullOrEmpty(inputTextValue) ? default : ChangeType(decimal.Parse(inputTextValue, CultureInfo.CurrentCulture));
                int? numberOfDecimals = GetNumberOfDecimals(Value);
                var maximumFraction = (numberOfDecimals != null) ? numberOfDecimals.ToString() : string.Empty;
                inputTextValue = (inputValue == null) ? inputTextValue : Intl.GetNumericFormat<TValue>(inputValue, GetFormatString(inputValue, maximumFraction), NumericLocale, Currency);
                inputTextValue = (isNumberCulture) ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                if (IsIgnoreDecimal())
                {
                    inputTextValue = SfBaseUtils.RemoveClass(inputTextValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }
                return (inputValue == null) ? default : ChangeType(decimal.Parse(inputTextValue, CultureInfo.CurrentCulture));
            } else
            {
                return genericValue != null ? InputTextValue : default;
            }
            
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
            if (IsSpinButtonChanged)
            {
                IsSpinButtonChanged = false;
                await InvokeMethod("sfBlazor.NumericTextBox.spinButtonEvents", new object[] { InputElement });
            }

            if (firstRender)
            {
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
                    localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                    if (!(localStorageValue == null && Value != null))
                    {
                        var persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue, typeof(TValue), false, true);
                        InputTextValue = persistValue;
                    }

                    await ChangeValue((Value == null) ? default : StrictMode ? TrimValue(Value) : Value);
                }

                PrevValue = Value;
                await SfBaseUtils.InvokeEvent<object>(NumericEvents?.Created, null);
            }
        }

        private void PropertyInitialized()
        {
            //_value = Value;
            _step = Step;
            _max = Max;
            _cssClass = CssClass;
            _min = Min;
            _enabled = Enabled;
            _readonly = Readonly;
            _validateDecimalOnType = ValidateDecimalOnType;
            _showSpinButton = ShowSpinButton;
        }

        private async Task PropertyUpdate()
        {
            _step = NotifyPropertyChanges(STEP, Step, _step);
            _decimals = NotifyPropertyChanges(DECIMALS, Decimals, _decimals);
            NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            _max = NotifyPropertyChanges(MAX, Max, _max);
            _min = NotifyPropertyChanges(MIN, Min, _min);
            format = NotifyPropertyChanges(nameof(Format), Format, format);
            _enabled = NotifyPropertyChanges(ENABLED, Enabled, _enabled);
            _readonly = NotifyPropertyChanges(READ_ONLY, Readonly, _readonly);
            _showSpinButton = NotifyPropertyChanges(SHOW_SPIN_BUTTON, ShowSpinButton, _showSpinButton);
            _validateDecimalOnType = NotifyPropertyChanges(VALIDATE_DECIMAL_TYPE, ValidateDecimalOnType, _validateDecimalOnType);
            await Task.CompletedTask;
        }
        
        /// <summary>
        /// Bind the input event to the input element for enabled clear button and floatlabel to the component.
        /// </summary>
        private void InvokeInputEvent()
        {
            if (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Always || ShowClearButton || OnInput.HasDelegate || ValidateOnInput)
            {
                var createInputEvent = EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputHandler);
                inputAttr = SfBaseUtils.UpdateDictionary("oninput", createInputEvent, inputAttr);
            }
        }
        private TValue TrimValue(TValue value)
        {
            if (CompareValue(value, Max) >= 1)
            {
                return Max;
            }

            if (CompareValue(value, Min) <= -1)
            {
                return Min;
            }

            return value;
        }

#pragma warning disable CA1822 // Mark members as static
        private int CompareValue(TValue value1, TValue value2)
        {
            return Comparer<TValue>.Default.Compare(value1, value2);
        }

        private async Task ChangeValue(TValue value)
        {
            if (value != null)
            {
                int? numberOfDecimals = GetNumberOfDecimals(value);
                var roundValue = RoundNumber(value, numberOfDecimals);
                InputTextValue = roundValue;
            }

            await ModifyText();
            if (!StrictMode)
            {
                ValidateState();
            }
        }

        private void ValidateState()
        {
            IsValidState = true;
            if (Value != null)
            {
                IsValidState = !(CompareValue(Value, Max) >= 1 || CompareValue(Value, Min) <= -1);
            }

            CheckErrorClass();
        }

        private void CheckErrorClass()
        {
            if (IsValidState)
            {
                ContainerClass = ContainerClass.Replace(ERROR_CLASS, string.Empty, StringComparison.Ordinal);
            }
            else
            {
                ContainerClass = ContainerClass.Contains(ERROR_CLASS, StringComparison.Ordinal) ? ContainerClass : ContainerClass + " " + ERROR_CLASS;
            }

            SfBaseUtils.UpdateDictionary(ARIA_INVALID, IsValidState ? FALSE : TRUE, BaseInputAttributes);
        }

        private TValue RoundNumber(TValue value, int? precision)
        {
            TValue result = value;
            Type propertyType = typeof(TValue);
            int decimals = precision == null ? 0 : (int)precision;
            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            result = valueString.Contains("E", StringComparison.Ordinal) ? result : (TValue)SfBaseUtils.ChangeType(decimal.Round(Convert.ToDecimal(result, CultureInfo.CurrentCulture), (int)decimals), propertyType, false, true);
            return result;
        }

        private async Task ModifyText()
        {
            if (Value != null)
            {
                var elemValue = FormatValueAsString(Value);
                await SetValue(elemValue, FloatLabelType, ShowClearButton);
            }
            else
            {

                await SetValue(null, FloatLabelType, ShowClearButton);
                BaseInputAttributes.Remove(ARIA_VALUE_NOW);
            }
        }

        private string FormatNumber()
        {
            int? numberOfDecimals = GetNumberOfDecimals(Value);
            var maximumFraction = (numberOfDecimals != null) ? numberOfDecimals.ToString() : string.Empty;
            var formatString = GetFormatString(Value, maximumFraction);
            return Value == null ? null : Intl.GetNumericFormat<TValue>(Value, formatString, NumericLocale, Currency);
        }

        private void ValidateMinMax()
        {
            var minValue = SfInputTextBase<TValue>.GetNumericValue<TValue>(MIN_VALUE);
            var maxValue = SfInputTextBase<TValue>.GetNumericValue<TValue>(MAX_Value);
            Min = (Min == null) ? minValue : Min;
            Max = (Max == null) ? maxValue : Max;
            if (Decimals != null)
            {
                if (CompareValue(Min, minValue) != 0)
                {
                    var inputValue = FormattedValue(Decimals, Min);
                    if (isNumberCulture)
                    {
                        inputValue = RemoveCultureDigits(inputValue);
                    }
                    Min = ChangeType(decimal.Parse(inputValue, CultureInfo.CurrentCulture));
                }

                if (CompareValue(Max, maxValue) != 0)
                {
                    var inputValue = FormattedValue(Decimals, Max);
                    if (isNumberCulture)
                    {
                        inputValue = RemoveCultureDigits(inputValue);
                    }
                    Max = ChangeType(decimal.Parse(inputValue, CultureInfo.CurrentCulture));
                }
            }

            Min = (CompareValue(Min, Max) >= 1) ? Max : Min;
            SfBaseUtils.UpdateDictionary(ARIA_VALUE_MIN, Min.ToString(), BaseInputAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_VALUE_MAX, Max.ToString(), BaseInputAttributes);
        }

        private void ValidateStep()
        {
            if (Decimals != null)
            {
                var inputValue = FormattedValue(Decimals, Step);
                if (isNumberCulture)
                {
                    inputValue = RemoveCultureDigits(inputValue);
                }
                Step = ChangeType(decimal.Parse(inputValue, CultureInfo.CurrentCulture));
            }
        }

        private string FormattedValue(int? decimals, TValue value)
        {
            var maximumFraction = decimals != null ? decimals.ToString() : string.Empty;
            return Intl.GetNumericFormat<TValue>(value, "n" + maximumFraction, NumericLocale, Currency);
        }

        private int? GetNumberOfDecimals(TValue value)
        {
            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            var decimalPart = valueString.Split(new char[] { '.' }).Length > 1 && !valueString.Contains("E", StringComparison.Ordinal) ? valueString.Split(new char[] { '.' })[1] : string.Empty;
            int? numberOfDecimals = string.IsNullOrEmpty(decimalPart) || decimalPart.Length < 0 ? 0 : decimalPart.Length;
            if (Decimals != null)
            {
                numberOfDecimals = numberOfDecimals < Decimals ? numberOfDecimals : Decimals;
            }

            return numberOfDecimals;
        }

        internal async override Task OnAfterScriptRendered()
        {
            await InvokeMethod("sfBlazor.TextBox.initialize", new object[] { InputElement, DotnetObjectReference, ContainerElement });
            await InvokeMethod("sfBlazor.NumericTextBox.initialize", new object[] { ContainerElement, InputElement, DotnetObjectReference, new NumericClientProps { Readonly = Readonly, Enabled = Enabled, Locale = CultureInfo.CurrentCulture.Name, ValidateDecimalOnType = ValidateDecimalOnType, Decimals = Decimals, DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator } });
            IsDevice = SyncfusionService.IsDeviceMode;
        }

        protected override async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            NumericFocusEventArgs<TValue> eventArgs = new NumericFocusEventArgs<TValue>()
            {
                Container = ContainerElement,
                Event = args,
                Value = Value,
                Name = "Focus",
            };
            await SfBaseUtils.InvokeEvent(NumericEvents?.Focus, eventArgs);
            if (Enabled && !Readonly)
            {
                IsFocus = true;
                if (ValueExpression != null)
                {
                    UpdateValidateClass();
                }
                else
                {
                    ContainerClass = ContainerClass.Replace(ERROR_CLASS, string.Empty, StringComparison.Ordinal);
                }

                PrevValue = Value;
                if (Value != null)
                {
                    var formatNumbers = FormatNumber();
                    if (IsIgnoreDecimal())
                    {
                        formatNumbers = SfBaseUtils.RemoveClass(formatNumbers, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }

                    var formatValue = SfBaseUtils.RemoveClass(formatNumbers, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
                    await SetValue(formatValue, FloatLabelType, ShowClearButton);

                    if (!IsPrevFocused && IsRendered)
                    {
                        await InvokeMethod("sfBlazor.NumericTextBox.selectRange", new object[] { InputElement, FormatValueAsString(InputTextValue), DotNetObjectReference.Create(this) });
                    }
                }
            }

        }

        protected override async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            NumericBlurEventArgs<TValue> eventArgs = new NumericBlurEventArgs<TValue>()
            {
                Container = ContainerElement,
                Event = args,
                Value = Value,
                Name = BLUR
            };
            await SfBaseUtils.InvokeEvent(NumericEvents?.Blur, eventArgs);
            if (Enabled && !Readonly)
            {
                if (IsPrevFocused)
                {
                    if (IsDevice)
                    {
                        var value = FocusInputValue;
                        await FocusIn();
                        IsPrevFocused = false;
                        await Task.Delay(200);    // Remove the clear button on time delay in component.
                        await SetValue(value, FloatLabelType, ShowClearButton);
                    }
                }
                else
                {
                    IsFocus = false;
                    var inputValue = FocusInputValue;
                    if (string.IsNullOrEmpty(inputValue))
                    {
                        InputTextValue = default!;
                    }
                    if (isNumberCulture)
                    {
                        inputValue = RemoveCultureDigits(inputValue);
                    }

                    decimal decimalValue;
                    var changedValue = default(TValue);
                    if (!string.IsNullOrEmpty(inputValue) && decimal.TryParse(inputValue, NumberStyles.Any, CultureInfo.CurrentCulture, out decimalValue))
                    {
                        changedValue = ChangeType(decimalValue);
                    }
                    else
                    {
                        changedValue = default;
                    }

                    await UpdateValue(changedValue, args);
                }
            }

        }

        private string RemoveCultureDigits(string inputValue)
        {
            string outValue = string.Empty;
            if (!string.IsNullOrEmpty(inputValue))
            {
                for (int index = 0; index < inputValue.Length; index++)
                {
                    outValue += char.IsDigit(inputValue[index]) ? Convert.ToString(char.GetNumericValue(inputValue, index), CultureInfo.InvariantCulture) : inputValue[index].ToString();
                }
            }

            return outValue;
        }

        private void UpdateDecimalType()
        {
            if (IsIgnoreDecimal())
            {
                Decimals = _decimals = Decimals == null ? 0 : Decimals;
                ValidateDecimalOnType = _validateDecimalOnType = true;
                Format = Format == "n2" ? "n0" : Format;
            }
        }

        private bool IsIgnoreDecimal()
        {
            Type type = typeof(TValue);
            return type == typeof(int) || Nullable.GetUnderlyingType(type) == typeof(int) || Nullable.GetUnderlyingType(type) == typeof(byte) || type == typeof(byte) || type == typeof(long) || Nullable.GetUnderlyingType(type) == typeof(long) || type == typeof(short) || Nullable.GetUnderlyingType(type) == typeof(short);
        }

        private async Task UpdateValue(TValue value, EventArgs args = null)
        {
            if (value != null)
            {
                if (Decimals != null)
                {
                    value = RoundNumber(value, Decimals);
                }
            }

            await ChangeValue((value == null) ? default : StrictMode ? TrimValue(value) : value);
            await RaiseChangeEvent(args);
        }
        
        protected override async Task ChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            var formattedVal = args != null ? (string)args.Value : null;
            if (string.IsNullOrEmpty(formattedVal))
            {
                formattedVal = default;
                InputTextValue = default;
            }
            FocusInputValue = formattedVal;
            IsFocus = false;
            decimal decimalValue;
            if (!(decimal.TryParse(formattedVal, NumberStyles.Any, CultureInfo.CurrentCulture, out decimalValue)))
            {
                await RaiseChangeEvent(args);
                return;
            }
            var inputValue = string.IsNullOrEmpty(formattedVal) ? default : ChangeType(decimal.Parse(formattedVal, CultureInfo.CurrentCulture));
            int? numberOfDecimals = GetNumberOfDecimals(inputValue);
            var maximumFraction = (numberOfDecimals != null) ? numberOfDecimals.ToString() : string.Empty;
            var changedValue = string.IsNullOrEmpty(formattedVal) ? default : ChangeType(decimal.Parse(formattedVal, CultureInfo.CurrentCulture));
            var formatString = GetFormatString(changedValue, maximumFraction);
            var formattedValue = Intl.GetNumericFormat<TValue>(changedValue, formatString, NumericLocale, Currency);
            var parseInput = changedValue == null ? default : ChangeType(decimal.Parse(formattedValue, CultureInfo.CurrentCulture));
            var validateValue = (parseInput == null) ? default : StrictMode ? TrimValue(parseInput) : parseInput;
            if (StrictMode && SfBaseUtils.Equals(InputTextValue, validateValue) && !IsPasteValue)
            {
                minMaxValue = formattedVal;
                CurrentValueAsString = formattedVal;
                InputTextValue = parseInput;
                await Task.Delay(30);
            }
            minMaxValue = null;
            IsPasteValue = false;
            await UpdateValue(validateValue, args);
        }

        private void MouseDownOnSpinner()
        {
            if (IsFocus)
            {
                IsPrevFocused = true;
                MouseDowSpinnerPrevent = true;
            }
        }

        private async Task MouseUpOnSpinner()
        {
            if (IsPrevFocused)
            {
                if (!IsDevice)
                {
                    IsPrevFocused = false;
                }
            }

            MouseDowSpinnerPrevent = true;
            var changeArgs = new Microsoft.AspNetCore.Components.ChangeEventArgs() { Value = Value };
            await SfBaseUtils.InvokeEvent<Microsoft.AspNetCore.Components.ChangeEventArgs>(OnChange, changeArgs);
        }

        private async Task Action(string action, EventArgs args = null, string currentInputValue = null)
        {
            TValue value;
            if (IsFocus)
            {
                var inputTextValue = !string.IsNullOrEmpty(currentInputValue) ? currentInputValue : FormatValueAsString(InputTextValue);
                inputTextValue = (isNumberCulture) ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                var inputValue = string.IsNullOrEmpty(inputTextValue) ? default : ChangeType(decimal.Parse(inputTextValue, CultureInfo.CurrentCulture));
                int? numberOfDecimals = GetNumberOfDecimals(Value);
                var maximumFraction = (numberOfDecimals != null) ? numberOfDecimals.ToString() : string.Empty;
                inputTextValue = (inputValue == null) ? inputTextValue : Intl.GetNumericFormat<TValue>(inputValue, GetFormatString(inputValue, maximumFraction), NumericLocale, Currency);
                inputTextValue = (isNumberCulture) ? RemoveCultureDigits(inputTextValue) : inputTextValue;
                if (IsIgnoreDecimal())
                {
                    inputTextValue = SfBaseUtils.RemoveClass(inputTextValue, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                }

                value = (inputValue == null) ? default : ChangeType(decimal.Parse(inputTextValue, CultureInfo.CurrentCulture));
            }
            else
            {
                value = Value;
            }

            await ChangeValue(PerformAction(value, Step, action));
            await RaiseChangeEvent(args);
            UpdateValidateClass();
            StateHasChanged();
        }

        private string GetFormatString(TValue numericValue, string maximumFraction)
        {
            var isExponential = Convert.ToString(numericValue, CultureInfo.CurrentCulture).Contains("E", StringComparison.Ordinal);
            var isNumberFormat = Format != null && Format.ToLower(CultureInfo.CurrentCulture).Contains("n", StringComparison.Ordinal);
            var formatValue = (IsFocus && !isNumberFormat) ? (Decimals != null ? "n" + Decimals : "n") : Format;
            return isExponential ? formatValue : "n" + maximumFraction;
        }

        private async Task RaiseChangeEvent(EventArgs args = null)
        {
            if (CompareValue(PrevValue, InputTextValue) != 0)
            {
                ChangeEventArgs<TValue> eventArgs = new ChangeEventArgs<TValue>()
                {
                    Value = InputTextValue,
                    PreviousValue = PrevValue,
                    Event = args,
                    IsInteracted = args != null ? true : false,
                    Name = VALUECHANGE
                };
                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, Value);
                }

                await SfBaseUtils.InvokeEvent(NumericEvents?.ValueChange, eventArgs);
                PrevValue = InputTextValue;
            }
        }

        private async Task SetLocalStorage(string persistId, TValue dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        private TValue ChangeType(object value)
        {
            try
            {
                return (TValue)SfBaseUtils.ChangeType(value, typeof(TValue), false, true);
            }
            catch
            {
                return Max;
            }
        }

        private TValue PerformAction(TValue value, TValue step, string operation)
        {
            if (value == null)
            {
                value = ChangeType("0");
            }

            TValue updatedValue = (operation == INCREMENT_CONTENT) ? NumberOperate(value, step, ADD) : NumberOperate(value, step, SUB);
            updatedValue = CorrectRounding(value, step, updatedValue);
            return StrictMode ? TrimValue(updatedValue) : updatedValue;
        }

        private Regex NumericRegex()
        {
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var fractionRule = "*";
            if (Decimals == 0 && ValidateDecimalOnType)
            {
                return new Regex(@"^(-)?(\d*)$");
            }

            if (Decimals != null && ValidateDecimalOnType)
            {
                fractionRule = "{0," + (int)Decimals + "}";
            }

            return new Regex(@"^(-)?(((\d+(" + decimalSeparator + "\\d" + fractionRule + ")?)|(" + decimalSeparator + "\\d" + fractionRule + ")))?$");
        }

        private async Task UpdatePasteInput(string inputValue)
        {
            await SetValue(inputValue, FloatLabelType, ShowClearButton);
        }

        private TValue CorrectRounding(TValue value, TValue step, TValue result)
        {
            Regex regex = new Regex(@"[,.](.*)");
            Regex exponential = new Regex(@"[eE](.*)");
            var valueText = Convert.ToString(value, CultureInfo.InvariantCulture);
            var stepText = Convert.ToString(step, CultureInfo.InvariantCulture);
            var floatValue = regex.Match(valueText).Success;
            var floatStep = regex.Match(stepText).Success;
            var isExponential = exponential.Match(valueText).Success;
            if ((floatValue || floatStep) && !isExponential)
            {
                var valueCount = floatValue ? regex.Matches(valueText)[0].Length : 0;
                var stepCount = floatStep ? regex.Matches(stepText)[0].Length : 0;
                var max = Math.Max(valueCount, stepCount);
                value = RoundValue(result, max);
                return value;
            }

            return result;
        }

        private TValue RoundValue(TValue result, double? precision)
        {
            var roundVal = result;
            precision = precision == null ? 0 : precision;
            var divide = Math.Pow(10, (double)precision);
            roundVal = MultipleValue(roundVal, divide);
            roundVal = DivideValue(roundVal, divide);
            return roundVal;
        }

        private TValue MultipleValue(TValue value, double divide)
        {
            dynamic result = value;
            dynamic devideVal = ChangeType(divide);
            return (TValue)(result * devideVal);
        }

        private TValue DivideValue(TValue value, double divide)
        {
            dynamic result = value;
            dynamic devideVal = ChangeType(divide);
            return (TValue)(Math.Round(result) / devideVal);
        }

        private TValue NumberOperate(TValue value1, TValue value2, string action)
#pragma warning restore CA1822 // Mark members as static
        {
            dynamic changeVal1 = value1;
            dynamic changeVal2 = value2;
            var numberValue = (action.ToLower(CultureInfo.CurrentCulture) == ADD) ? (changeVal1 + changeVal2) : (changeVal1 - changeVal2);
            Type type = typeof(TValue);
            if (Nullable.GetUnderlyingType(type) == typeof(byte) || type == typeof(byte))
            {
                var byteValue = byte.MinValue > numberValue ? byte.MinValue : byte.MaxValue < numberValue ? byte.MaxValue : numberValue;
                return (TValue)byteValue;
            }
            return (TValue)numberValue;
        }

        internal async Task BindClearBtnTouchEvents(EventArgs args)
        {
            await InvokeClearBtnEvent(args);
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
         

        internal async Task BindClearBtnEvents(EventArgs args)
        {
            await InvokeClearBtnEvent(args);
        }

        private async Task InvokeClearBtnEvent(EventArgs args)
        {
            await SetValue(null, FloatLabelType, ShowClearButton);
            await RaiseChangeEvent(args);
            ClearBtnStopPropagation = true;
            await FocusIn();
        }
        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                var destroyArgs = new object[] { InputElement };
                InvokeMethod("sfBlazor.NumericTextBox.destroy", destroyArgs).ContinueWith(t =>
                {
                    _ = SfBaseUtils.InvokeEvent<object>(NumericEvents?.Destroyed, null);
                }, TaskScheduler.Current); 
            }
             
        }
    }
}