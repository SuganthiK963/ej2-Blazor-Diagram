using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Spinner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Calendars")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DropDowns")]

namespace Syncfusion.Blazor.Inputs.Internal
{
    /// <summary>
    /// The SfInputBase is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfInputBase : SfBaseComponent, IInputBase
    {
        private const string CONTROL_CONTAINER = "e-control-container";
        private const string CONTROL_OLD_CONTAINER = "e-control-wrapper";
        private const string INPUTGROUP = "e-input-group";

        private const string CLEARICONHIDE = "e-clear-icon-hide";

        private const string FILLED = "e-filled";

        private const string OUTLINE = "e-outline";

        private const string MULTILINE = "e-multi-line-input";

        private const string DISABLE = "e-disabled";

        private const string RTL = "e-rtl";

        private const string INPUT = "e-input";

        private const string INPUTFOCUS = "e-input-focus";

        private const string VALIDINPUT = "e-valid-input";

        private const string FLOATINPUT = "e-float-input";

        private const string FLOATTEXT = "e-float-text";

        private const string FLOATLABELBOTTOM = "e-label-bottom";

        private const string FLOATLABELTOP = "e-label-top";

        private const string INCREMENT_TITLE = "NumericTextBox_IncrementTitle";

        private const string DECREMENT_TITLE = "NumericTextBox_DecrementTitle";

        private const string INCREMENT = "Increment value";

        private const string DECREMENT = "Decrement value";

        private const string CLASS = "class";

        private const string ROLE = "role";

        private const string NAME = "name";

        private const string TEXTBOX = "textbox";

        private const string TYPE = "type";

        private const string TAB_INDEX = "tabindex";

        private const string STYLE = "style";

        private const string PLACE_HOLDER = "placeholder";

        private const string ARIA_PLACE_HOLDER = "aria-placeholder";

        private const string AUTOCOMPLETE = "autocomplete";

        private const string READONLY = "readonly";

        private const string ARIA_READONLY = "aria-readonly";

        private const string ARIA_LABEL_BY = "aria-labelledby";

        private const string DISABLED = "disabled";

        private const string ARIA_DISABLED = "aria-disabled";

        private const string APPEND = "append";

        private const string CLEAR_ICON_HIDE = "e-clear-icon e-clear-icon-hide";

        private const string GROUP_ICON = "e-input-group-icon";

        private const string SPACE = " ";

        private const string SPIN_DOWN = "e-spin-down";

        private const string SPIN_UP = "e-spin-up";

        private const string INPUT_VALUE = "e-input-value";

        private const string DISABLE_ICON = "e-ddl-disable-icon";

        private Dictionary<string, object> inputAttr = new Dictionary<string, object>();

        private List<string> containerAttributes = new List<string>() { "title", "style", "class" };

        internal string ClearIconClass { get; set; }

        internal string InputTextValue { get; set; }

        private string FloatLabel { get; set; }

        /// <exclude/>
        /// <summary>
        /// Gets or sets the Parent.
        /// </summary>
        [CascadingParameter(Name = "Parent")]
        internal IInputBase Parent { get; set; }

        private ElementReference ClearElement { get; set; }

        internal ElementReference InputElement { get; set; }

        internal ElementReference ContainerElement { get; set; }

        internal SfSpinner SpinnerObj { get; set; }

        [Inject]
        internal ISyncfusionStringLocalizer Localizer { get; set; }

        private bool IsFocused { get; set; }

        private string IconPosition { get; set; }

        private string IncrementTitle { get; set; }

        private string DecrementTitle { get; set; }
        private bool IsAddPrepenButton { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfTextBox;
            await base.OnInitializedAsync();
            InputTextValue = Value;
            IconPosition = APPEND;
            ClearIconClass = CLEAR_ICON_HIDE;
        }

        /// <summary>
        /// Triggers when dynamically changing the component property.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            PreRender();
        }

        /// <summary>
        /// Triggers after the component get rendered.
        /// </summary>
        /// <param name="firstRender">True if the component rendered for the first time.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (this.PrependButtons != null && PrependButtons.Count > 0 && IsAddPrepenButton)
            {
                IsAddPrepenButton = false;
                await OnAfterScriptRendered();
            }
        }

        private void PreRender()
        {
            inputAttr = SfBaseUtils.UpdateDictionary(CLASS, RootClass, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(NAME, ID, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(ROLE, TEXTBOX, inputAttr);
            var tabIndex = Enabled ? this.TabIndex : -1;
            inputAttr = SfBaseUtils.UpdateDictionary(TAB_INDEX, tabIndex, inputAttr);
            if (FloatLabelType == FloatLabelType.Never)
            {
                inputAttr = SfBaseUtils.UpdateDictionary(CLASS, RootClass + " " + INPUT, inputAttr);
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, FLOATINPUT);
            }

            ContainerAttr = SfBaseUtils.UpdateDictionary(STYLE, "width:" + Width + ";", ContainerAttr);
            if (!Multiline)
            {
                inputAttr = SfBaseUtils.UpdateDictionary(TYPE, Type, inputAttr);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTGROUP);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_CONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_OLD_CONTAINER);
            }
            else
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_CONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_OLD_CONTAINER);
                ContainerClass =  SfBaseUtils.AddClass(ContainerClass, MULTILINE);
                if (FloatLabelType == FloatLabelType.Never)
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTGROUP);
                }
            }

            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = ContainerClass.Contains(CssClass, StringComparison.Ordinal) ? ContainerClass : SfBaseUtils.AddClass(ContainerClass, CssClass);
            }

            inputAttr = SfBaseUtils.UpdateDictionary(PLACE_HOLDER, Placeholder, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(ARIA_PLACE_HOLDER, Placeholder, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(AUTOCOMPLETE, Autocomplete, inputAttr);
            SetReadOnly();
            SetEnabled();
            SetRtl();
            CreateFloatingLabel();
            CheckInputValue(FloatLabelType, InputTextValue);
            UpdateHtmlAttr();
            UpdateInputAttr();
            if (SpinButton)
            {
                var incrementLocale = Localizer.GetText(INCREMENT_TITLE);
                var decrementLocale = Localizer.GetText(DECREMENT_TITLE);
                IncrementTitle = incrementLocale == null ? INCREMENT : incrementLocale;
                DecrementTitle = decrementLocale == null ? DECREMENT : decrementLocale;
            }
        }

        /// <summary>
        /// The setReadOnly.
        /// </summary>
        private void SetReadOnly()
        {
            if (Readonly || IsReadOnlyInput)
            {
                inputAttr = SfBaseUtils.UpdateDictionary(READONLY, true, inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_READONLY, "true", inputAttr);
            }
            else
            {
                inputAttr.Remove(READONLY);
                inputAttr.Remove(ARIA_READONLY);
            }
        }

        /// <summary>
        /// The createFloatingLabel.
        /// </summary>
        private void CreateFloatingLabel()
        {
            if (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Always)
            {
                inputAttr.Remove(PLACE_HOLDER);
                inputAttr.Remove(ARIA_PLACE_HOLDER);
            }

            if (FloatLabelType == FloatLabelType.Auto && !ContainerClass.Contains(INPUTFOCUS, StringComparison.Ordinal) && !IsFocused)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, inputAttr);
                FloatLabel = string.IsNullOrEmpty(Value) ? FLOATTEXT + " " + FLOATLABELBOTTOM : FLOATTEXT + " " + FLOATLABELTOP;
            }
            else if (FloatLabelType == FloatLabelType.Always)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                ContainerClass = ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) ? SfBaseUtils.AddClass(ContainerClass, VALIDINPUT) : ContainerClass;
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, inputAttr);
                FloatLabel = FLOATTEXT + " " + FLOATLABELTOP;
            }
        }

        /// <summary>
        /// The setEnabled.
        /// </summary>
        private void SetEnabled()
        {
            if (!Enabled)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, DISABLE);
                inputAttr = SfBaseUtils.UpdateDictionary(CLASS, inputAttr[CLASS] + " " + DISABLE, inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary(DISABLED, DISABLED, inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, "true", inputAttr);
            }
            else
            {
                ContainerClass = ContainerClass.Replace(DISABLE, string.Empty, StringComparison.Ordinal);
                inputAttr[CLASS] = inputAttr[CLASS].ToString().Replace(DISABLE, string.Empty, StringComparison.Ordinal);
                inputAttr.Remove(DISABLED);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, "false", inputAttr);
            }
        }

        private void UpdateInputAttr()
        {
            if (InputAttributes.Count > 0)
            {
                foreach (var attr in InputAttributes)
                {
                    SfBaseUtils.UpdateDictionary(attr.Key, attr.Value, inputAttr);
                }
            }
        }

        private void SetRtl()
        {
            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, RTL);
            }
            else
            {
                ContainerClass = ContainerClass.Replace(RTL, string.Empty, StringComparison.Ordinal);
            }
        }

        private void UpdateHtmlAttr()
        {
            if (HtmlAttributes != null)
            {
                foreach (var item in (Dictionary<string, object>)HtmlAttributes)
                {
                    if (containerAttributes.IndexOf(item.Key) < 0)
                    {
                        inputAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, inputAttr);
                    }
                    else
                    {
                        if (item.Key == CLASS)
                        {
                            ContainerClass = SfBaseUtils.AddClass(ContainerClass, (string)item.Value);
                        }
                        else if (item.Key == STYLE)
                        {
                            if (ContainerAttr.ContainsKey(STYLE))
                            {
                                ContainerAttr[item.Key] += item.Value.ToString();
                            }
                            else
                            {
                                ContainerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, ContainerAttr);
                            }
                        }
                        else
                        {
                            ContainerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, ContainerAttr);
                        }
                    }
                }
            }
        }

        private async Task OnInputHandler(ChangeEventArgs args)
        {
            InputTextValue = (string)args.Value;
            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState((string)args.Value);
            }

            CheckInputValue(FloatLabelType, InputTextValue);
            UpdateIconState(InputTextValue);
            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }
            else if (Parent != null)
            {
                await SfBaseUtils.InvokeEvent(Parent.OnInput, args);
            }
        }

        private async Task InvokeMouseIconHandler(MouseEventArgs args, string iconName)
        {
            var eventArgs = new IconHandlerArgs() { eventArgs = args, IconName = iconName };
            await SfBaseUtils.InvokeEvent<IconHandlerArgs>(MouseIconHandler, eventArgs);
        }

        private async Task InvokeTouchIconHandler(TouchEventArgs args, string iconName)
        {
            var eventArgs = new IconHandlerArgs() { eventArgs = args, IconName = iconName };
            await SfBaseUtils.InvokeEvent<IconHandlerArgs>(TouchIconHandler, eventArgs);
        }

        private async Task OnPasteHandler(ClipboardEventArgs args)
        {
            await SfBaseUtils.InvokeEvent<ClipboardEventArgs>(OnPaste, args);
        }

        internal async Task OnFocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (Enabled)
            {
                if (ContainerClass.Contains(INPUTGROUP, StringComparison.Ordinal) || ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || ContainerClass.Contains(FILLED, StringComparison.Ordinal))
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTFOCUS);
                }

                IsFocused = true;
                FocusFn();
                if (!Readonly)
                {
                    UpdateIconState(InputTextValue);
                }

                Parent?.UpdateParentClass(RootClass, ContainerClass);
                await SfBaseUtils.InvokeEvent<Microsoft.AspNetCore.Components.Web.FocusEventArgs>(OnFocus, args);
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            await InvokeMethod("sfBlazor.TextBox.initialize", new object[] { InputElement, DotnetObjectReference, ContainerElement });
        }

        /// <summary>
        /// triggers while the component get focused out.
        /// </summary>
        /// <returns>Task.</returns>
        [JSInvokable]
        public async Task BlurHandler()
        {
            await OnBlurHandler();
        }

        internal async Task OnBlurHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args = null)
        {
            BlurFn();
            if (ContainerClass.Contains(INPUTGROUP, StringComparison.Ordinal) || ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || ContainerClass.Contains(FILLED, StringComparison.Ordinal))
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, INPUTFOCUS);
            }

            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(InputTextValue);
            }

            IsFocused = false;
            Parent?.UpdateParentClass(RootClass, ContainerClass);
            args = args == null ? new Microsoft.AspNetCore.Components.Web.FocusEventArgs() : args;
            ClearIconClass = ShowClearButton ? SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE) : ClearIconClass;
            await SfBaseUtils.InvokeEvent<Microsoft.AspNetCore.Components.Web.FocusEventArgs>(OnBlur, args);
        }

        private void FocusFn()
        {
            if (FloatLabelType == FloatLabelType.Auto)
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELTOP;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELBOTTOM, string.Empty, StringComparison.Ordinal) : FloatLabel;
            }
        }

        private void BlurFn()
        {
            if (FloatLabelType == FloatLabelType.Auto)
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELTOP, string.Empty, StringComparison.Ordinal) : FloatLabel;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELBOTTOM;
            }
        }

        private void OnChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState((string)args.Value);
            }
            Value = (string)args.Value;
            CheckInputValue(FloatLabelType, InputTextValue);
            Parent?.UpdateParentClass(RootClass, ContainerClass);
            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(args);
            }

        }

        private void UpdateLabelState(string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELTOP;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELBOTTOM, string.Empty, StringComparison.Ordinal) : FloatLabel;
            }
            else
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELTOP, string.Empty, StringComparison.Ordinal) : FloatLabel;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELBOTTOM;
            }
        }

        private void CheckInputValue(FloatLabelType floatLabelType, string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, VALIDINPUT);
            }
            else if (floatLabelType != FloatLabelType.Always)
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, VALIDINPUT);
            }
        }

        private void ValidateLabel(string value)
        {
            if (ContainerClass.Contains(FLOATINPUT, StringComparison.Ordinal) && FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(value);
            }
        }

        internal async Task SetValue(string value, FloatLabelType floatLabelType, bool clearButton = false, bool isValueTemp = false)
        {
            Value = _value = await SfBaseUtils.UpdateProperty(value, _value, ValueChanged, InputEditContext, ValueExpression);
            InputTextValue = value;
            IsValueTemplate = isValueTemp;
            if (floatLabelType == FloatLabelType.Auto)
            {
                ValidateLabel(value);
            }

            if (clearButton)
            {
                if (!string.IsNullOrEmpty(value) && ContainerClass.Contains(INPUTFOCUS, StringComparison.Ordinal))
                {
                    ClearIconClass = ClearIconClass.Replace(CLEARICONHIDE, string.Empty, StringComparison.Ordinal);
                }
                else
                {
                    ClearIconClass = SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
                }
            }

            CheckInputValue(floatLabelType, value);
            await InvokeAsync(() => StateHasChanged());
        }

        private void UpdateIconState(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                ClearIconClass = ClearIconClass.Replace(CLEARICONHIDE, string.Empty, StringComparison.Ordinal);
            }
            else
            {
                ClearIconClass = SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
            }
        }

        private async Task WireClearBtnEvents()
        {
            if (!(inputAttr[CLASS].ToString().Contains(DISABLE, StringComparison.Ordinal) || inputAttr.ContainsKey(READONLY)))
            {
                // focuse the input
                Value = null;
                InputTextValue = string.Empty;
                ClearIconClass = SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
            }

            await Task.CompletedTask;
        }

        internal async Task AddIcons(string position = APPEND, string icons = null)
        {
            IconPosition = position.ToLower(CultureInfo.CurrentCulture);
            if (IconPosition == APPEND)
            {
                if (Buttons == null)
                {
                    Buttons = new List<string>() { icons };
                }
                else
                {
                    var icon = Buttons;
                    icon.Add(icons);
                    Buttons = icon;
                }
            }
            else
            {
                if (PrependButtons == null)
                {
                    PrependButtons = new List<string>() { icons };
                }
                else
                {
                    var icon = PrependButtons;
                    icon.Add(icons);
                    PrependButtons = icon;
                }
                IsAddPrepenButton = true;
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Update the parent component class to the element.
        /// </summary>
        /// <param name="rootClass">The rootClass<see cref="string"/>.</param>
        /// <param name="containerClass">The containerClass<see cref="string"/>.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateParentClass(string rootClass, string containerClass)
        {
            RootClass = rootClass;
            ContainerClass = containerClass;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal static T GetNumericValue<T>(string property)
        {
            Type propertyType = typeof(T);
            bool isNullable = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            propertyType = isNullable ? Nullable.GetUnderlyingType(propertyType) : propertyType;
            if (property == "Step")
            {
                return (T)SfBaseUtils.ChangeType(decimal.One, propertyType);
            }
            else
            {
                switch (propertyType?.Name)
                {
                    case nameof(Int32):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(int.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(int.MaxValue, propertyType);
                    case nameof(Int64):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(long.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(long.MaxValue, propertyType);
                    case nameof(Int16):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(short.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(short.MaxValue, propertyType);
                    case nameof(Single):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(float.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(float.MaxValue, propertyType);
                    case nameof(Double):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(double.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(double.MaxValue, propertyType);
                    case nameof(Decimal):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(decimal.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(decimal.MaxValue, propertyType);
                    case nameof(UInt16):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(UInt16.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(UInt16.MaxValue, propertyType);
                    case nameof(UInt32):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(UInt32.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(UInt32.MaxValue, propertyType);
                    case nameof(UInt64):
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(UInt64.MinValue, propertyType) : (T)SfBaseUtils.ChangeType(UInt64.MaxValue, propertyType);
                    default:
                        return (property == "MinValue") ? (T)SfBaseUtils.ChangeType(propertyType?.GetField("MinValue")?.GetValue(null), propertyType) : (T)SfBaseUtils.ChangeType(propertyType?.GetField("MaxValue")?.GetValue(null), propertyType);
                }
            }
        }

        private void UpdateInputAttributes()
        {
            if (InputAttributes?.Count > 0)
            {
                foreach (var item in InputAttributes)
                {
                    SfBaseUtils.UpdateDictionary(item.Key, item.Value, inputAttr);
                }
            }
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                _ = InvokeMethod("sfBlazor.TextBox.destroyInput", new object[] { InputElement });
            }
        }
    }
}
