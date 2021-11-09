using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Spinner;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Calendars")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DropDowns")]

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The SfInputBase is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public abstract class SfInputTextBase<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Triggers when the content of input has changed and gets focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Triggers each time when the value of input has changed.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Triggers when the content is paste into an input.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ClipboardEventArgs> OnPaste { get; set; }

        /// <summary>
        /// Triggers when the input has focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnFocus { get; set; }

        /// <summary>
        /// Specifies the id of the TextBox component.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Sets the content of the TextBox.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        protected TValue _value { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

        /// <summary>
        /// Specifies the edit context of the Input.
        /// </summary>
        [CascadingParameter]
        protected EditContext InputEditContext { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the TextBox. One or more custom CSS classes can be added to a TextBox.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Enable or disable the persisting TextBox state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in the right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the TextBox allows the user to interact with it.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Specifies the floating label behavior of the TextBox that the placeholder text floats above the TextBox based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the TextBox when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the TextBox.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the TextBox after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>
        /// </summary>
        protected virtual FloatLabelType BaseFloatLabelType { get; set; }
        protected virtual Dictionary<string, object> BaseHtmlAttributes { get; set; }

        protected virtual Dictionary<string, object> BaseInputAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Specifies the global culture and localization of the TextBox component.
        /// </summary>
        [Obsolete("The Locale is deprecated and will no longer be used. Hereafter, the Locale works based on the current culture.")]
        [Parameter]
        public string Locale { get; set; } = string.Empty;
        /// <summary>
        /// Specifies a boolean value that indicates whether the component validates the input or not.
        /// </summary>
        /// <value>
        /// <c>true</c>, If the ValidateOnInput is enabled for form validation, then the model value will be updated on entering the value to the input. otherwise, <b>false</b>.The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// This property is used to validate the form on typing into the input and updating the model value. The ValueChange event will be fired after the component lost its focus.
        /// </remarks>
        [Parameter]
        public bool ValidateOnInput { get; set; }
        protected virtual bool MultilineInput { get; set; }
        protected virtual string BasePlaceholder { get; set; }
        protected virtual bool BaseReadonly { get; set; }
        protected virtual bool BaseIsReadOnlyInput { get; set; }
        protected virtual bool BaseShowClearButton { get; set; }
        protected virtual string BaseWidth { get; set; }
        protected virtual int BaseTabIndex { get; set; }
        protected virtual string BaseAutocomplete { get; set; } = "off";
        /// <summary>
        /// Specifies the container attrubute of Input.
        /// </summary>
        protected Dictionary<string, object> ContainerAttr { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Specifies a boolean value that enable or disable the spin button on the component.
        /// </summary>
        /// <exclude/>
        protected bool SpinButton { get; set; } = true;


        /// <summary>
        /// Accepts the template design and assigns it to the selected list item in the input element of the component.
        /// </summary>
        /// <exclude/>
        protected RenderFragment ValueTemplate { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the value template is displayed in TextBox.
        /// </summary>
        /// <exclude/> 
        protected bool IsValueTemplate { get; set; }



        /// <summary>
        /// Specifies the prevents the click actions.
        /// </summary>
        /// <exclude/> 
        protected bool ClickStopPropagation { get; set; }

        /// <summary>
        /// Specifies the prevents the mouse actions.
        /// </summary>
        /// <exclude/> 
        protected bool MouseDowSpinnerPrevent { get; set; }

        /// <summary>
        /// Specifies the prevents the icon actions.
        /// </summary>
        /// <exclude/> 
        protected bool PreventIconHandler { get; set; }

        /// <summary>
        /// Specifies the prevents the container actions.
        /// </summary>
        /// <exclude/> 
        protected bool MousePreventContainer { get; set; }
        protected List<ButtonGroups> ListOfButtons { get; set; }

        protected const string CONTROL_CONTAINER = "e-control-container";
        protected const string CONTROL_OLD_CONTAINER = "e-control-wrapper";
        protected const string INPUTGROUP = "e-input-group";

        protected const string CLEARICONHIDE = "e-clear-icon-hide";

        private const string FILLED = "e-filled";

        private const string OUTLINE = "e-outline";

        protected const string MULTILINE = "e-multi-line-input";

        protected const string DISABLE = "e-disabled";

        private const string RTL = "e-rtl";

        protected const string INPUT = "e-input";

        protected const string INPUTFOCUS = "e-input-focus";

        private const string VALIDINPUT = "e-valid-input";

        protected const string FLOATINPUT = "e-float-input";

        protected const string FLOATTEXT = "e-float-text";

        protected const string FLOATLABELBOTTOM = "e-label-bottom";

        protected const string FLOATLABELTOP = "e-label-top";

        protected const string INCREMENT_TITLE = "NumericTextBox_IncrementTitle";

        protected const string DECREMENT_TITLE = "NumericTextBox_DecrementTitle";

        protected const string INCREMENT = "Increment value";

        protected const string DECREMENT = "Decrement value";

        private const string CLASS = "class";

        private const string ROLE = "role";

        private const string NAME = "name";

        private const string TEXTBOX = "textbox";

        private const string TAB_INDEX = "tabindex";

        private const string STYLE = "style";

        private const string PLACE_HOLDER = "placeholder";

        private const string ARIA_PLACE_HOLDER = "aria-placeholder";

        private const string READONLY = "readonly";

        private const string ARIA_READONLY = "aria-readonly";

        private const string ARIA_LABEL_BY = "aria-labelledby";

        protected const string DISABLED_ATTR = "disabled";

        protected const string ARIA_DISABLED = "aria-disabled";

        protected const string APPEND = "append";

        protected const string PREPEND = "prepend";

        protected const string CLEAR_ICON_HIDE = "e-clear-icon e-clear-icon-hide";

        protected const string GROUP_ICON = "e-input-group-icon";

        protected const string SPACE = " ";

        protected const string SPIN_DOWN = "e-spin-down";

        protected const string SPIN_UP = "e-spin-up";

        protected const string DISABLE_ICON = "e-ddl-disable-icon";

        protected Dictionary<string, object> inputAttr { get; set; } = new Dictionary<string, object>();

        protected List<string> containerAttributes { get; set; } = new List<string>() { "title", "style", "class" };

        internal string ClearIconClass { get; set; }

        internal TValue InputTextValue 
        {
            get => Value;
            set
            {
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
                if (hasChanged)
                {
                    Value = _value = value;
                    _ = ValueChanged.InvokeAsync(Value);
                    if (InputEditContext != null && ValueExpression != null)
                    {
                        InputEditContext.NotifyFieldChanged(FieldIdentifier.Create(ValueExpression));
                    }
                }
            }
        }

        internal string CurrentValueAsString
        {
            get => FormatValueAsString(InputTextValue);
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    InputTextValue = default!;
                }
                else
                {
                    InputTextValue = FormatValue(value);
                }
            }
        }


        protected string FloatLabel { get; set; }
         

        protected ElementReference ClearElement { get; set; }

        internal ElementReference InputElement { get; set; }

        internal ElementReference ContainerElement { get; set; }

        internal SfSpinner SpinnerObj { get; set; }

        [Inject]
        internal ISyncfusionStringLocalizer Localizer { get; set; }

        protected bool IsFocused { get; set; }

        private string IconPosition { get; set; }

        protected string IncrementTitle { get; set; }

        protected string DecrementTitle { get; set; }
        private bool IsAddPrepenButton { get; set; }
        /// <summary>
        /// Specifies the class value that is appended to container of TextBox.
        /// </summary>
        /// <exclude/>
        protected virtual string ContainerClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Set the component class to element.
        /// </summary>
        /// <exclude/>
        protected virtual string RootClass { get; set; } = "e-control e-textbox e-lib";
        protected virtual string ComponentReference { get; set; }
        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ComponentReference))
            {
                ScriptModules = SfScriptModules.SfTextBox;
                await base.OnInitializedAsync();
                IconPosition = APPEND;
                ClearIconClass = CLEAR_ICON_HIDE;
            } else
            {
                await base.OnInitializedAsync();
            }
            
        }

        /// <summary>
        /// Triggers when dynamically changing the component property.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (string.IsNullOrEmpty(ComponentReference))
            {
                PreRender();
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (IsAddPrepenButton && this.ListOfButtons != null && ListOfButtons.Count > 0 )
            {
                IsAddPrepenButton = false;
                await OnAfterScriptRendered();
            }
        }
        protected virtual string FormatValueAsString(TValue formatValue)
        {
            return formatValue?.ToString();
        }
        protected virtual TValue FormatValue(string genericValue)
        {
            return string.IsNullOrEmpty(genericValue) ? default : (TValue)SfBaseUtils.ChangeType(genericValue, typeof(TValue));
        }
        private void PreRender()
        {
            inputAttr = SfBaseUtils.UpdateDictionary(CLASS, RootClass, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(NAME, ID, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(ROLE, TEXTBOX, inputAttr);
            var tabIndex = Enabled ? this.BaseTabIndex : -1;
            inputAttr = SfBaseUtils.UpdateDictionary(TAB_INDEX, tabIndex, inputAttr);
            if (BaseFloatLabelType == FloatLabelType.Never)
            {
                inputAttr = SfBaseUtils.UpdateDictionary(CLASS, RootClass + " " + INPUT, inputAttr);
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, FLOATINPUT);
            }

            ContainerAttr = SfBaseUtils.UpdateDictionary(STYLE, "width:" + BaseWidth + ";", ContainerAttr);
            if (!MultilineInput)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTGROUP);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_CONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_OLD_CONTAINER);
            }
            else
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_CONTAINER);
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, CONTROL_OLD_CONTAINER);
                ContainerClass =  SfBaseUtils.AddClass(ContainerClass, MULTILINE);
                if (BaseFloatLabelType == FloatLabelType.Never)
                {
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTGROUP);
                }
            }
            inputAttr = SfBaseUtils.UpdateDictionary("autocomplete", BaseAutocomplete, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(PLACE_HOLDER, BasePlaceholder, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(ARIA_PLACE_HOLDER, BasePlaceholder, inputAttr);
            SetReadOnly();
            SetEnabled();
            SetRtl();
            CreateFloatingLabel();
            CheckInputValue(BaseFloatLabelType, FormatValueAsString(InputTextValue));
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
            if (BaseReadonly || BaseIsReadOnlyInput)
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
            if (BaseFloatLabelType == FloatLabelType.Auto || BaseFloatLabelType == FloatLabelType.Always)
            {
                inputAttr.Remove(PLACE_HOLDER);
                inputAttr.Remove(ARIA_PLACE_HOLDER);
            }

            if (BaseFloatLabelType == FloatLabelType.Auto && !ContainerClass.Contains(INPUTFOCUS, StringComparison.Ordinal) && !IsFocused)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, inputAttr);
                FloatLabel = string.IsNullOrEmpty(FormatValueAsString(InputTextValue)) ? FLOATTEXT + " " + FLOATLABELBOTTOM : FLOATTEXT + " " + FLOATLABELTOP;
            }
            else if (BaseFloatLabelType == FloatLabelType.Always)
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
                inputAttr = SfBaseUtils.UpdateDictionary(DISABLED_ATTR, DISABLED_ATTR, inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, "true", inputAttr);
            }
            else
            {
                ContainerClass = ContainerClass.Replace(DISABLE, string.Empty, StringComparison.Ordinal);
                inputAttr[CLASS] = inputAttr[CLASS].ToString().Replace(DISABLE, string.Empty, StringComparison.Ordinal);
                inputAttr.Remove(DISABLED_ATTR);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, "false", inputAttr);
            }
        }

        private void UpdateInputAttr()
        {
            if (BaseInputAttributes.Count > 0)
            {
                foreach (var attr in BaseInputAttributes)
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
            if (BaseHtmlAttributes != null)
            {
                foreach (var item in (Dictionary<string, object>)BaseHtmlAttributes)
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
        protected virtual async Task InputHandler(ChangeEventArgs args)
        {
            await Task.CompletedTask;
        }
        protected async Task OnInputHandler(ChangeEventArgs args)
        {
            var inputVal = args != null ? (string)args.Value : null;
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(inputVal);
            }

            CheckInputValue(BaseFloatLabelType, inputVal);
            UpdateIconState(inputVal);
            if (ValidateOnInput && InputEditContext != null)
            {
                InputTextValue = FormatValue(inputVal);
            }
            await InputHandler(args);
            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args);
            }
        }

        protected async Task OnPasteHandler(ClipboardEventArgs args)
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
                if (!BaseReadonly)
                {
                    UpdateIconState(FormatValueAsString(InputTextValue));
                }
                await FocusHandler(args);
                if (OnFocus.HasDelegate)
                {
                    _ = OnFocus.InvokeAsync(args);
                }
            }
        }
        protected virtual async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            await Task.CompletedTask;
        }
        internal override async Task OnAfterScriptRendered()
        {
            await base.OnAfterScriptRendered();
            await InvokeMethod("sfBlazor.TextBox.initialize", new object[] { InputElement, DotnetObjectReference, ContainerElement });

        }

        /// <summary>
        /// triggers while the component get focused out.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude />
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

            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(FormatValueAsString(InputTextValue));
            }

            IsFocused = false;
            args = args == null ? new Microsoft.AspNetCore.Components.Web.FocusEventArgs() : args;
            ClearIconClass = BaseShowClearButton ? SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE) : ClearIconClass;
            await FocusOutHandler(args);
            if (OnBlur.HasDelegate)
            {
                _ = OnBlur.InvokeAsync(args);
            } else
            {
                StateHasChanged();
            }
        }

        protected virtual async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            await Task.CompletedTask;
        }

        private void FocusFn()
        {
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELTOP;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELBOTTOM, string.Empty, StringComparison.Ordinal) : FloatLabel;
            }
        }

        private void BlurFn()
        {
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                FloatLabel = FloatLabel.Contains(FLOATLABELTOP, StringComparison.Ordinal) ? FloatLabel.Replace(FLOATLABELTOP, string.Empty, StringComparison.Ordinal) : FloatLabel;
                FloatLabel = FloatLabel.Contains(FLOATLABELBOTTOM, StringComparison.Ordinal) ? FloatLabel : FloatLabel + " " + FLOATLABELBOTTOM;
            }
        }
        protected virtual async Task ChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            await Task.CompletedTask;
        }
        protected async Task OnChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            var changeVal = args != null ? (string)args.Value : null;
            if (BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(changeVal);
            }
            CheckInputValue(BaseFloatLabelType, changeVal);
            await this.ChangeHandler(args);
            if (OnChange.HasDelegate)
            {
                _ = OnChange.InvokeAsync(args);
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
            if (ContainerClass.Contains(FLOATINPUT, StringComparison.Ordinal) && BaseFloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(value);
            }
        }

        internal async Task SetValue(string value, FloatLabelType floatLabelType, bool clearButton = false, bool isValueTemp = false)
        {
            InputTextValue = FormatValue(value);
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

        protected async Task WireClearBtnEvents()
        {
            if (!(inputAttr[CLASS].ToString().Contains(DISABLE, StringComparison.Ordinal) || inputAttr.ContainsKey(READONLY)))
            {
                // focuse the input
                InputTextValue = default;
                ClearIconClass = SfBaseUtils.AddClass(ClearIconClass, CLEARICONHIDE);
            }

            await Task.CompletedTask;
        }

        internal async Task AddIcons(string position = APPEND, string icons = null, Dictionary<string, object> events = null)
        {
            IconPosition = position.ToLower(CultureInfo.CurrentCulture);
            if (ListOfButtons == null)
            {
                ListOfButtons = new List<ButtonGroups>();
            }
            ListOfButtons.Add(new ButtonGroups()
            {
                Icon = icons,
                Position = position,
                Events = events
            });
            if(position == PREPEND)
                IsAddPrepenButton = true;

            await Task.CompletedTask;
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
            if (BaseInputAttributes?.Count > 0)
            {
                foreach (var item in BaseInputAttributes)
                {
                    SfBaseUtils.UpdateDictionary(item.Key, item.Value, inputAttr);
                }
            }
        }
        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                inputAttr = null;
                BaseInputAttributes = null;
                containerAttributes = null;
                InputEditContext = null;
                _ = InvokeMethod("sfBlazor.TextBox.destroyInput", new object[] { InputElement });
            }
        }

        protected class ButtonGroups
        {
            internal string Icon { get; set; } = string.Empty;

            internal string Position { get; set; } = string.Empty;

            internal Dictionary<string, object> Events = new Dictionary<string, object>();
        }
    }
}
