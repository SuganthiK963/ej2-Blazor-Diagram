using System;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The TextBox is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextBox : SfInputTextBase<string>
    {
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string ROOT_CLASS = "e-control e-textbox e-lib"; 
        private const string TYPE = "type";
        private const string AUTOCOMPLETE = "autocomplete";
        protected override string RootClass { get; set; } = "e-control e-textbox e-lib";
        protected override string ContainerClass { get; set; } = string.Empty;
        private string InputPreviousValue { get; set; }

        private string PreviousValue { get; set; }

        private bool ClearBtnStopPropagation { get; set; }

        private bool ClearClicked { get; set; }

        private string ValidClass { get; set; }
        protected override string BaseAutocomplete { get; set; }
        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfTextBox;
            BaseAutocomplete = SfBaseUtils.GetEnumValue(this.Autocomplete);
            await base.OnInitializedAsync();
            InvokeInputEvent();
            _cssClass = CssClass;
            InputTextValue = Value;
            _value = Value;
            _autocomplete = Autocomplete;
            _type = Type;
            InitializeProps();
            inputAttr = SfBaseUtils.UpdateDictionary(AUTOCOMPLETE, SfBaseUtils.GetEnumValue(this.Autocomplete), inputAttr);
            if (!Multiline)
            {
                inputAttr = SfBaseUtils.UpdateDictionary(TYPE, SfBaseUtils.GetEnumValue(this.Type), inputAttr);
            }
            SetCssClass();
            if (TextBoxParent != null && Convert.ToString(TextBoxParent.Type, CultureInfo.CurrentCulture) == "Text")
            {
                TextBoxParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(TextBoxParent, this);
            }
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await PropertyUpdate();
            if (PropertyChanges.Count > 0)
            {
                await OnPropertyChange(PropertyChanges);
            }
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
                        if (PropertyChanges.ContainsKey(nameof(Value)) && PreviousValue != Value)
                        {
                            PreviousValue = (PrevChanges != null && PrevChanges.Any()) ? (string)PrevChanges[nameof(Value)] : PreviousValue;
                            await SetValue(Value, FloatLabelType, ShowClearButton);
                            await RaiseChangeEvent(null, false);
                        }
                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, _cssClass);
                        _cssClass = CssClass;
                        SetCssClass();
                        break;
                    case nameof(FloatLabelType):
                        await OnAfterScriptRendered();
                        break;
                    case nameof(Autocomplete):
                        inputAttr = SfBaseUtils.UpdateDictionary(AUTOCOMPLETE, SfBaseUtils.GetEnumValue(this.Autocomplete), inputAttr);
                        break;
                    case nameof(Type):
                        inputAttr = SfBaseUtils.UpdateDictionary(TYPE, SfBaseUtils.GetEnumValue(this.Type), inputAttr);
                        break;
                }
            }
        }
        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = ContainerClass.Contains(CssClass, StringComparison.Ordinal) ? ContainerClass : SfBaseUtils.AddClass(ContainerClass, CssClass);
            }
        }
        private async Task PropertyUpdate()
        {
            NotifyPropertyChanges(nameof(CssClass), CssClass, _cssClass);
            _autocomplete = NotifyPropertyChanges(nameof(Autocomplete), Autocomplete, _autocomplete);
            _type = NotifyPropertyChanges(nameof(Type), Type, _type);
            _value = NotifyPropertyChanges(nameof(Value), Value, _value);
            await Task.CompletedTask;
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
            if (firstRender)
            {
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
                    localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                    if (!(localStorageValue == null && Value != null))
                    {
                        await SetValue(localStorageValue, FloatLabelType, ShowClearButton);
                    }
                }
                PreviousValue = Value;
                await SfBaseUtils.InvokeEvent<object>(Created, null);
            }
        }
        
        /// <summary>
        /// Bind the input event to the input element for enabled clear button and floatlabel to the component.
        /// </summary>
        private void InvokeInputEvent()
        {
            if (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Always || ShowClearButton || OnInput.HasDelegate || Input.HasDelegate || ValidateOnInput)
            {
                var createInputEvent = EventCallback.Factory.Create<ChangeEventArgs>(this, OnInputHandler);
                inputAttr = SfBaseUtils.UpdateDictionary("oninput", createInputEvent, inputAttr);
            }
        }

        private void InitializeProps()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = "textbox-" + Guid.NewGuid().ToString();
            }

            RootClass = ROOT_CLASS;
            ContainerClass = string.Empty;
            InputPreviousValue = Value;
            PreviousValue = Value;
        }

        private async Task BindClearBtnTouchEvents(EventArgs args)
        {
            await InvokeClearBtnEvent(args);
        }

        private async Task BindClearBtnEvents(EventArgs args)
        {
            await InvokeClearBtnEvent(args);
            await FocusIn();
        }

        private async Task InvokeClearBtnEvent(EventArgs args)
        {
            this.CurrentValueAsString = null;
            await SetValue(null, FloatLabelType, ShowClearButton);
            InputEventArgs eventArgs = new InputEventArgs()
            {
                Container = ContainerElement,
                Value = InputTextValue,
                Event = args,
                PreviousValue = InputPreviousValue,
            };
            ClearClicked = true;
            await SfBaseUtils.InvokeEvent(Input, eventArgs);
            await RaiseChangeEvent(args, true);
            PreviousValue = InputTextValue;
            ClearBtnStopPropagation = true;
            await FocusIn();
        }

        private async Task RaiseChangeEvent(EventArgs args = null, bool isInteraction = false)
        {
            ChangedEventArgs eventArgs = new ChangedEventArgs()
            {
                Container = ContainerElement,
                Event = args,
                Value = Value,
                PreviousValue = PreviousValue,
                IsInteracted = isInteraction,
                IsInteraction = isInteraction,
            };
            if (EnablePersistence)
            {
                await SetLocalStorage(ID, Value);
            }

            await SfBaseUtils.InvokeEvent(ValueChange, eventArgs);
            PreviousValue = Value;
        }

        private async Task SetLocalStorage(string persistId, string dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        protected override async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            FocusInEventArgs eventArgs = new FocusInEventArgs()
            {
                Container = ContainerElement,
                Event = args,
                Value = Value
            };
            await SfBaseUtils.InvokeEvent(Focus, eventArgs);
            if (OnFocus.HasDelegate)
            {
                _ = OnFocus.InvokeAsync(args);
            }
        }

        protected override async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (!(string.IsNullOrEmpty(PreviousValue) && string.IsNullOrEmpty(Value) && string.IsNullOrEmpty(InputTextValue)) && PreviousValue != InputTextValue)
            {
                await RaiseChangeEvent(args, true);
            }

            FocusOutEventArgs eventArgs = new FocusOutEventArgs()
            {
                Container = ContainerElement,
                Event = args,
                Value = Value
            };
            await SfBaseUtils.InvokeEvent(Blur, eventArgs);
        }

        protected override async Task InputHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            InputEventArgs eventArgs = new InputEventArgs()
            {
                Container = ContainerElement,
                Value = args != null ? (string)args.Value : null,
                Event = args,
                PreviousValue = InputPreviousValue
            };
            await SfBaseUtils.InvokeEvent(Input, eventArgs);
            InputPreviousValue = args != null ? (string)args.Value : null;
        }

        protected override async Task ChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            var changeVal = args != null ? (string)args.Value : null;
            this.CurrentValueAsString = changeVal;
            SfBaseUtils.ValidateExpression(InputEditContext, ValueExpression);
            await RaiseChangeEvent(args, true);
        }

        /// <summary>
        /// Update the parent component class to the element.
        /// </summary>
        /// <param name="rootClass">Specifies the root class of the component.</param>
        /// <param name="containerClass">Specifies the container class of the component.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateParentClass(string rootClass, string containerClass)
        {
            RootClass = rootClass;
            ContainerClass = containerClass;
            UpdateValidateClass();
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
    }
}