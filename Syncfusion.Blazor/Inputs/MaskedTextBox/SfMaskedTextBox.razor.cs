using System;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.Inputs.Internal;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The MaskedTextBox is an input element that allows to get input from the user.
    /// </summary>
    public partial class SfMaskedTextBox : SfBaseComponent, IInputBase
    {
        private const string PROMPT_CHAR = "PromptChar";
        private const string VALUE = "Value";
        private const string CONTAINER_CLASS = "e-mask";
        private const string INVALID = "invalid";
        private const string MODIFIED_INVALID = "modified invalid";
        private const string MODIFIED_VALID = "modified valid";
        private const string ERROR_CLASS = "e-error";
        private const string SUCCESS_CLASS = "e-success";
        private const string ROOT_CLASS = "e-control e-maskedtextbox e-lib";
        private const string BACKSPACE = "Backspace";
        private const char OPENSQUAREBRACKET = '[';
        private const char CLOSESQUAREBRACKET = ']';
        private const string CLEARICONHIDE = "e-clear-icon-hide";

        internal SfInputBase InputBaseObj { get; set; }

        private int SelectionStart { get; set; }

        private int SelectionEnd { get; set; }

        private string PreviousValue { get; set; }

        private bool ClearBtnStopPropagation { get; set; }

        private string HiddenMask { get; set; }

        private bool IsUpper { get; set; }

        private bool IsLower { get; set; }

        private bool IsDynamicVal { get; set; }

        private bool IsPasteVal { get; set; }

        private string Regx { get; set; } = string.Empty;

        private string PropsValue { get; set; }

        private string MaskedValue { get; set; }

        private string MaskValue { get; set; }

        private string StrippedValue { get; set; }

        private string PropsValueChar { get; set; }

        private string PasteValue { get; set; }

        private string PromptMask { get; set; }

        private char PromptCharacter { get; set; } = '_';

        private string KeyPressed { get; set; } = string.Empty;

        private string RegularExpression { get; set; } = string.Empty;

        private bool IsMask { get; set; }

        private List<string> SplitMask { get; set; }

        private List<string> CustomRegExpCollec { get; set; }

        private List<string> Props { get; set; }

        private bool IsMultipleDelete { get; set; }

        internal string RootClass { get; set; }

        internal string ContainerClass { get; set; }

        private bool IsRender { get; set; } = true;

        private string ValidClass { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfMaskedTextBox;
            ContainerClass = CONTAINER_CLASS;
            await base.OnInitializedAsync();

            InitializeProps();
            if (EnablePersistence)
            {
                var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
                localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                if (localStorageValue != null)
                {
                    var beforeValue = value;
                    value = localStorageValue;
                    Value = await SfBaseUtils.UpdateProperty(localStorageValue, beforeValue, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
                }
            }

            if (!string.IsNullOrEmpty(Mask))
            {
                HiddenMask = string.Empty;
                if (CustomRegExpCollec != null)
                {
                    CustomRegExpCollec.Clear();
                }

                CreateMask();
                if (!string.IsNullOrEmpty(Value))
                {
                    SelectionStart = SelectionEnd = 0;
                    await DynamicValueTest(Value);
                }
                else
                {
                    MaskedValue = PromptMask;
                }
            }
            else
            {
                MaskedValue = Value;
            }

            if (!string.IsNullOrEmpty(Value) || (FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)))
            {
                MaskValue = MaskedValue;
            }
            else
            {
                MaskValue = null;
            }

            PreviousValue = Value;
            if (MaskedTextBoxParent != null && Convert.ToString(MaskedTextBoxParent.Type, CultureInfo.CurrentCulture) == "Mask") // Used for In-place Editor Component
            {
                MaskedTextBoxParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(MaskedTextBoxParent, this);
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
            await PropertyParametersSet();
            if (PropertyChanges.Count > 0)
            {
                var changedProps = new Dictionary<string, object>(PropertyChanges);
                PropertyChanges.Remove(nameof(Value));
                await PropertyChange(changedProps);
                var options = GetProperty();
                if (InputBaseObj?.InputElement != null && IsRendered)
                {
                    await InvokeMethod("sfBlazor.MaskedTextBox.propertyChange", new object[] { InputBaseObj?.InputElement, options });
                }
            }

            UpdateValidateClass();
        }

        private async Task PropertyChange(Dictionary<string, object> newProps)
        {
            var newProperties = newProps.ToList();
            foreach (var prop in newProperties)
            {
                switch (prop.Key)
                {
                    case nameof(Value):
                        PreviousValue = (PrevChanges != null && PrevChanges.Any()) ? (string)PrevChanges[nameof(Value)] : PreviousValue;
                        if (!string.IsNullOrEmpty(Mask))
                        {
                            SelectionStart = SelectionEnd = 0;
                            MaskValue = MaskedValue = PromptMask;
                            await DynamicValueTest(Value);
                        }
                        else
                        {
                            MaskValue = MaskedValue = Value;
                            InputBaseObj?.SetValue(MaskedValue, FloatLabelType, ShowClearButton);
                        }

                        // await RaiseChangeEvent(null);
                        break;
                    case nameof(Mask):
                    case nameof(PromptChar):
                        if (!string.IsNullOrEmpty(Mask))
                        {
                            HiddenMask = string.Empty;
                            if (CustomRegExpCollec != null)
                            {
                                CustomRegExpCollec.Clear();
                            }

                            CreateMask();
                            MaskedValue = PromptMask;
                            MaskValue = MaskedValue;
                            if (!string.IsNullOrEmpty(Value))
                            {
                                SelectionStart = SelectionEnd = 0;
                                await DynamicValueTest(Value);
                            }
                        }
                        else
                        {
                            MaskedValue = Value;
                            InputBaseObj?.SetValue(MaskedValue, FloatLabelType, ShowClearButton);
                        }

                        break;
                    case nameof(CssClass):
                        ContainerClass = string.IsNullOrEmpty(ContainerClass) ? ContainerClass : SfBaseUtils.RemoveClass(ContainerClass, cssClass);
                        cssClass = CssClass;
                        break;
                    case nameof(FloatLabelType):
                        if (!string.IsNullOrEmpty(Value) || (FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)))
                        {
                            MaskValue = MaskedValue;
                        }
                        else
                        {
                            MaskValue = null;
                        }

                        await OnAfterScriptRendered();
                        break;
                }
            }
        }

        private async Task PropertyParametersSet()
        {
            value = NotifyPropertyChanges(VALUE, Value, value, true);
            promptChar = NotifyPropertyChanges(PROMPT_CHAR, PromptChar, promptChar);
            NotifyPropertyChanges(nameof(CssClass), CssClass, cssClass);
            floatLabelType = NotifyPropertyChanges(nameof(FloatLabelType), FloatLabelType, floatLabelType);
            mask = NotifyPropertyChanges(nameof(Mask), Mask, mask);
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
                await SfBaseUtils.InvokeEvent<object>(Created, null);
            }
        }

        private void InitializeProps()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("mask");
            }

            RootClass = ROOT_CLASS;
            ContainerClass = string.Empty;
            value = Value;
            cssClass = CssClass;
            floatLabelType = FloatLabelType;
            promptChar = PromptChar;
            mask = Mask;
            PreviousValue = Value;
        }

        private async Task InvokeClearBtnEvent(EventArgs args)
        {
            ClearBtnStopPropagation = true;
            await Task.Delay(100);
            string elementValue = (!string.IsNullOrEmpty(Mask)) ? PromptMask : null;
            await InputBaseObj.SetValue(elementValue, FloatLabelType, ShowClearButton);
            Value = value = await SfBaseUtils.UpdateProperty(null, value, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
            if (PromptMask != null)
            {
                MaskValue = MaskedValue = PromptMask;
            }
            else
            {
                MaskValue = MaskedValue = null;
            }

            await RaiseChangeEvent(args);
            await FocusIn();
            var options = GetProperty();
            if (InputBaseObj != null)
            {
                InputBaseObj.ClearIconClass = SfBaseUtils.AddClass(InputBaseObj.ClearIconClass, CLEARICONHIDE);
                await InvokeMethod("sfBlazor.MaskedTextBox.propertyChange", new object[] { InputBaseObj.InputElement, options });
            }
        }

        private async Task RaiseChangeEvent(EventArgs args = null)
        {
            if (PreviousValue != Value)
            {
                MaskChangeEventArgs eventArgs = new MaskChangeEventArgs()
                {
                    Container = InputBaseObj != null ? InputBaseObj.ContainerElement : new ElementReference(),
                    //Event = args,
                    MaskedValue = MaskedValue,
                    //Name = VALUECHANGE,
                    Value = Value,
                    IsInteracted = args != null
                };

                if (EnablePersistence)
                {
                    await SetLocalStorage(ID, Value);
                }

                PreviousValue = Value;
                await SfBaseUtils.InvokeEvent(ValueChange, eventArgs);
            }
        }

        private async Task SetLocalStorage(string persistId, string dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        private void UpdateClearIconStatus(string inputValue)
        {
            if (!string.IsNullOrEmpty(Mask))
            {
                if (string.IsNullOrEmpty(inputValue) && InputBaseObj != null)
                {
                    InputBaseObj.ClearIconClass = SfBaseUtils.AddClass(InputBaseObj.ClearIconClass, CLEARICONHIDE);
                }
                else
                {
                    if (InputBaseObj != null && Enabled && !Readonly)
                    {
                        InputBaseObj.ClearIconClass = SfBaseUtils.RemoveClass(InputBaseObj.ClearIconClass, CLEARICONHIDE);
                    }
                }
            }
        }

        private async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            UpdateClearIconStatus(Value);
            var options = GetProperty();
            if (!string.IsNullOrEmpty(Mask))
            {
                if (!(!string.IsNullOrEmpty(Value) || (FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder))))
                {
                    MaskValue = MaskedValue;
                }
            }

            MaskFocusEventArgs eventArgs = new MaskFocusEventArgs()
            {
                Container = InputBaseObj != null ? InputBaseObj.ContainerElement : new ElementReference(),
                //Event = args,
                MaskedValue = MaskedValue,
                //Name = FOCUS,
                SelectionEnd = SelectionEnd,
                SelectionStart = SelectionStart,
                Value = Value
            };
            await SfBaseUtils.InvokeEvent(Focus, eventArgs);
            await SfBaseUtils.InvokeEvent<Microsoft.AspNetCore.Components.Web.FocusEventArgs>(OnFocus, args);
        }

        private async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (InputBaseObj != null)
            {
                InputBaseObj.ClearIconClass = SfBaseUtils.AddClass(InputBaseObj.ClearIconClass, CLEARICONHIDE);
                if (!string.IsNullOrEmpty(Mask))
                {
                    if (!string.IsNullOrEmpty(Value) || (FloatLabelType == FloatLabelType.Always || string.IsNullOrEmpty(Placeholder)))
                    {
                        MaskValue = MaskedValue;
                    }
                    else
                    {
                        MaskValue = null;
                    }
                }

                await RaiseChangeEvent(args);
                MaskBlurEventArgs eventArgs = new MaskBlurEventArgs()
                {
                    Container = InputBaseObj.ContainerElement,
                    //Event = args,
                    MaskedValue = MaskedValue,
                    //Name = BLUR,
                    Value = Value
                };

                await SfBaseUtils.InvokeEvent(Blur, eventArgs);
                await SfBaseUtils.InvokeEvent<Microsoft.AspNetCore.Components.Web.FocusEventArgs>(OnBlur, args);
            }
        }

        private async Task InputHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (string.IsNullOrEmpty(Mask))
            {
                MaskValue = (string)args.Value;
                var beforeValue = value;
                value = MaskValue;
                Value = await SfBaseUtils.UpdateProperty(MaskValue, beforeValue, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
            }

            await SfBaseUtils.InvokeEvent(OnInput, args);
        }

        private async Task ChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (!(KeyPressed == BACKSPACE))
            {
                var inputValue = string.IsNullOrEmpty((string)args.Value) ? null : (string)args.Value;
                if (!string.IsNullOrEmpty(Mask))
                {
                    MaskedValue = await SfBaseUtils.UpdateProperty(MaskedValue, MaskedValue, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
                    MaskValue = MaskedValue;
                    Value = value = StripValue(MaskedValue);
                }
                else
                {
                    Value = value = await SfBaseUtils.UpdateProperty(inputValue, value, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
                }

                await RaiseChangeEvent(args);
                await SfBaseUtils.InvokeEvent<Microsoft.AspNetCore.Components.ChangeEventArgs>(OnChange, args);
            }
        }

        /// <summary>
        /// Update the parent component class to the element.
        /// </summary>
        /// <param name="rootClass">Specifies the root class of the component.</param>
        /// <param name="containerClass">Specifies the container class o the component.</param>
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
            if (ValueExpression != null && SfMaskedTextBoxEditContext != null)
            {
                var fieldIdentifier = FieldIdentifier.Create(ValueExpression);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
                ValidClass = SfMaskedTextBoxEditContext.FieldCssClass(fieldIdentifier);
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

        private void CreateMask()
        {
            if (!string.IsNullOrEmpty(PromptChar.ToString()))
            {
                PromptCharacter = PromptChar;
            }

            if (Mask != null)
            {
                PromptMask = null;
                SplitMask = Mask.Split(CLOSESQUAREBRACKET).ToList();

                for (var i = 0; i < SplitMask.Count; i++)
                {
                    if (SplitMask[i].Length != 0)
                    {
                        var splitInnerMask = SplitMask[i].Split(OPENSQUAREBRACKET);
                        if (splitInnerMask.Length > 1)
                        {
                            bool checkSpace = false;

                            for (var j = 0; j < splitInnerMask.Length; j++)
                            {
                                if (splitInnerMask[j] == "\\")
                                {
                                    if (CustomRegExpCollec == null)
                                    {
                                        CustomRegExpCollec = new List<string> { OPENSQUAREBRACKET.ToString() };
                                    }
                                    else
                                    {
                                        CustomRegExpCollec.Add(OPENSQUAREBRACKET.ToString());
                                    }

                                    HiddenMask += splitInnerMask[j] + OPENSQUAREBRACKET;
                                }
                                else if (string.IsNullOrEmpty(splitInnerMask[j]))
                                {
                                    checkSpace = true;
                                }
                                else if ((!string.IsNullOrEmpty(splitInnerMask[j]) && checkSpace) || j == splitInnerMask.Length - 1)
                                {
                                    if (CustomRegExpCollec == null)
                                    {
                                        CustomRegExpCollec = new List<string> { OPENSQUAREBRACKET + splitInnerMask[j] + CLOSESQUAREBRACKET };
                                    }
                                    else
                                    {
                                        CustomRegExpCollec.Add(OPENSQUAREBRACKET + splitInnerMask[j] + CLOSESQUAREBRACKET);
                                    }

                                    HiddenMask += PromptCharacter;
                                    checkSpace = false;
                                }
                                else
                                {
                                    PushIntoRegExpCollec(splitInnerMask[j]);
                                }
                            }
                        }
                        else
                        {
                            PushIntoRegExpCollec(splitInnerMask[0]);
                        }
                    }
                }
            }

            if (CustomCharacters != null)
            {
                dynamic customcharkey = CustomCharacters;

                foreach (var objkey in customcharkey)
                {
                    if (Props == null)
                    {
                        Props = new List<string> { objkey.Key };
                    }
                    else
                    {
                        Props.Add(objkey.Key);
                    }
                }

                for (int l = 0; l < Props.Count; l++)
                {
                    PropsValue = CustomCharacters[Props[l]];
                    PropsValueChar = Regex.Replace(PropsValue, @"[, ]", string.Empty);
                    for (int m = 0; m < CustomRegExpCollec.Count; m++)
                    {
                        if (CustomRegExpCollec[m] == Props[l].ToString())
                        {
                            CustomRegExpCollec[m] = OPENSQUAREBRACKET.ToString() + PropsValueChar.Trim() + CLOSESQUAREBRACKET.ToString();
                        }
                    }

                    for (int n = 0; n < HiddenMask.Length; n++)
                    {
                        if (HiddenMask[n].ToString() == Props[l].ToString())
                        {
                            string temp = HiddenMask.Replace(HiddenMask[n], PromptCharacter);
                            HiddenMask = temp;
                        }
                    }
                }
            }

            string maskValue = Regex.Replace(HiddenMask, @"[09?LCAa#&]", PromptCharacter.ToString());
            for (int i = 0; i < maskValue.Length; i++)
            {
                StringBuilder temp = new StringBuilder(maskValue);
                if (HiddenMask[i] == '\\' && i != HiddenMask.Length - 1)
                {
                    temp[i + 1] = HiddenMask[i + 1];
                    maskValue = temp.ToString();
                }
            }

            maskValue = Regex.Replace(maskValue, @"[\\]", string.Empty);
            HiddenMask = Regex.Replace(HiddenMask, @"[\\]", string.Empty);
            PromptMask = Regex.Replace(maskValue, "[>|<]", string.Empty);
        }

        private void PushIntoRegExpCollec(string splitValue)
        {
            for (int k = 0; k < splitValue.Length; k++)
            {
                HiddenMask += splitValue[k];
                if (splitValue[k] != '\\' && splitValue[k] != '<' && splitValue[k] != '>' && splitValue[k] != '|')
                {
                    string regxsplitValue = splitValue[k].ToString();
                    if (CustomRegExpCollec == null)
                    {
                        CustomRegExpCollec = new List<string> { regxsplitValue };
                    }
                    else
                    {
                        CustomRegExpCollec.Add(regxsplitValue);
                    }
                }
            }
        }

        private MaskClientProps GetProperty()
        {
            return new MaskClientProps
            {
                Readonly = Readonly,
                Enabled = Enabled,
                Locale = MaskLocale,
                SelectionStart = SelectionStart,
                SelectionEnd = SelectionEnd,
                Value = Value,
                Mask = PromptMask,
                IsMultipleDelete = IsMultipleDelete,
                PasteValue = PasteValue,
                PromptCharacter = PromptChar.ToString(),
                PlaceHolder = Placeholder,
                MaskedValue = MaskedValue,
                FloatLabelType = FloatLabelType.ToString(),
                CustomRegExpCollec = CustomRegExpCollec,
                HiddenMask = HiddenMask,
                PromptMask = PromptMask
            };
        }

        /// <summary>
        /// Update the respective value property without mask literals.
        /// </summary>
        /// <param name="inputval">Specifies the value with mask literals.</param>
        /// <param name="isPaste">true if the user performs paste action , otherwise false.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task UpdateInputValue(string inputval, bool isPaste = false)
        {
            MaskedValue = inputval;
            StrippedValue = StripValue(MaskedValue);
            if (!IsDynamicVal)
            {
                StrippedValue = string.IsNullOrEmpty(StrippedValue) ? null : StrippedValue;
                UpdateClearIconStatus(StrippedValue);
                if ((isPaste && SfMaskedTextBoxEditContext == null) || ValueChanged.HasDelegate)
                {
                    var beforeValue = value;
                    Value = value = StrippedValue;
                    Value = await SfBaseUtils.UpdateProperty(StrippedValue, beforeValue, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
                    if (SfMaskedTextBoxEditContext != null && ValueChanged.HasDelegate && !string.Equals(Value, beforeValue, StringComparison.Ordinal))
                    {
                        string finalValue = Value;
                        await ValueChanged.InvokeAsync(finalValue);
                        SfBaseUtils.ValidateExpression(SfMaskedTextBoxEditContext, ValueExpression);
                        UpdateValidateClass();
                    }
                }
                else
                {
                    Value = value = await SfBaseUtils.UpdateProperty(StrippedValue, value, ValueChanged, SfMaskedTextBoxEditContext, ValueExpression);
                }
            }
            else
            {
                MaskValue = MaskedValue;
                if (InputBaseObj != null)
                {
                    await InputBaseObj.SetValue(inputval, FloatLabelType, ShowClearButton);
                }
            }
        }

        private string StripValue(string maskVal)
        {
            string stripVal = string.Empty;
            if (PromptMask != null && maskVal != null)
            {
                for (int i = 0; i < PromptMask.Length; i++)
                {
                    if (PromptMask[i] != maskVal[i])
                    {
                        stripVal += maskVal[i];
                    }
                }
            }

            return stripVal;
        }

        private async Task DynamicValueTest(string keyValue, bool isPaste = false)
        {
            if (keyValue != null)
            {
                for (int i = 0; i < keyValue.Length; i++)
                {
                    IsDynamicVal = true;
                    await UpdateMask(keyValue[i].ToString());
                }

                IsDynamicVal = false;

                await UpdateInputValue(MaskedValue, isPaste);
            }
        }

        private async Task UpdateMask(string key)
        {
            if (!IsMultipleDelete)
            {
                var options = GetProperty();
                int startIndex = SelectionStart;
                string inputChar = key;
                string elementValue = MaskedValue != null ? MaskedValue : PromptMask;
                if (startIndex >= 0 && startIndex < CustomRegExpCollec.Count)
                {
                    Regx = CustomRegExpCollec[startIndex].Length == 1 ? GetRegex(CustomRegExpCollec[startIndex]) : CustomRegExpCollec[startIndex];
                }

                Regex checkRegex = new Regex(Regx);

                if (inputChar != null)
                {
                    if (startIndex < PromptMask.Length)
                    {
                        if (PromptMask[startIndex] == PromptCharacter)
                        {
                            if (checkRegex.IsMatch(inputChar))
                            {
                                string modifiedinputchar = GetLetterCase(inputChar, startIndex);
                                string modifiedValue = elementValue.Substring(0, startIndex) + modifiedinputchar + elementValue.Substring(startIndex + 1);
                                SelectionStart = SelectionEnd = SelectionStart + 1;
                                await UpdateInputValue(modifiedValue);
                                IsMask = false;
                            }
                        }
                        else
                        {
                            SelectionStart = SelectionEnd = SelectionStart + 1;
                            IsMask = true;
                            await UpdateMask(inputChar);
                        }
                    }
                    }
                }
            }

        private string GetLetterCase(string inputChar, int startIndex)
        {
            bool isLower = false;
            bool isUpper = false;
            bool isIndependent = false;
            bool isCasing = false;
            string hiddenMaskChar = HiddenMask;
            int length = HiddenMask.Length;
            bool[] lowerArray = new bool[length];
            bool[] upperArray = new bool[length];

            for (var i = 0; i < hiddenMaskChar.Length; i++)
            {
                if ((hiddenMaskChar[i] == '<' || isLower) && hiddenMaskChar[i] != '>' && hiddenMaskChar[i] != '|')
                {
                    isLower = isCasing = true;
                    isUpper = isIndependent = false;
                    lowerArray[i] = true;
                    upperArray[i] = false;
                    if (hiddenMaskChar[i] == '<')
                    {
                        hiddenMaskChar = hiddenMaskChar.Remove(i, 1);
                    }
                }
                else if ((hiddenMaskChar[i] == '>' || isUpper) && hiddenMaskChar[i] != '<' && hiddenMaskChar[i] != '|')
                {
                    isUpper = isCasing = true;
                    isLower = isIndependent = false;
                    lowerArray[i] = false;
                    upperArray[i] = true;
                    if (hiddenMaskChar[i] == '>')
                    {
                        hiddenMaskChar = hiddenMaskChar.Remove(i, 1);
                    }
                }
                else if ((hiddenMaskChar[i] == '|' || isIndependent) && hiddenMaskChar[i] != '<' && hiddenMaskChar[i] != '>')
                {
                    isIndependent = isCasing = true;
                    isUpper = isLower = false;
                    lowerArray[i] = false;
                    upperArray[i] = false;
                    if (hiddenMaskChar[i] == '|')
                    {
                        hiddenMaskChar = hiddenMaskChar.Remove(i, 1);
                    }
                }
                else if (!isCasing)
                {
                    lowerArray[i] = false;
                    upperArray[i] = false;
                }
            }
            var localChar = inputChar.ToLowerInvariant();
            var inputCharacter = upperArray[startIndex] ? inputChar.ToUpperInvariant() : lowerArray[startIndex] ? localChar : inputChar;
            return inputCharacter;
        }

        private string GetRegex(string maskChar)
        {
            switch (maskChar)
            {
                case "0":
                    RegularExpression = @"\d";
                    break;
                case "9":
                    RegularExpression = "[0-9 ]";
                    break;
                case "L":
                    RegularExpression = "[A-Za-z]";
                    break;
                case "A":
                    RegularExpression = "[A-Za-z0-9]";
                    break;
                case "a":
                    RegularExpression = "[A-Za-z0-9 ]";
                    break;
                case "C":
                    RegularExpression = "[^\x7f]+";
                    break;
                case "#":
                    RegularExpression = "[0-9 +-]";
                    break;
                case "?":
                    RegularExpression = "[A-Za-z ]";
                    break;
                case "&":
                    RegularExpression = "[^\x7f ]+";
                    break;
                default:
                    if (maskChar == "(" || maskChar == "+" || maskChar == ")")
                    {
                        RegularExpression = @"\" + maskChar;
                    }
                    else
                    {
                        RegularExpression = maskChar;
                    }

                    break;
            }

            return RegularExpression;
        }

        private async Task PasteHandler(MaskClientProps args)
        {
            if (!string.IsNullOrEmpty(args.PasteValue))
            {
                if (SelectionStart != SelectionEnd)
                {
                    string elementValue = MaskedValue;
                    string modifiedValue = string.Empty;
                    int endIndex = SelectionEnd;
                    for (int i = SelectionStart; i < SelectionEnd; i++)
                    {
                        elementValue = elementValue.Substring(0, i) + PromptMask[i] + elementValue.Substring(i + 1);
                    }

                    MaskedValue = elementValue;
                    for (int i = 0; i < args.PasteValue.Length; i++)
                    {
                        IsDynamicVal = true;
                        if (!(SelectionStart == endIndex))
                        {
                            await UpdateMask(args.PasteValue[i].ToString());
                        }
                    }

                    IsDynamicVal = false;
                    await UpdateInputValue(MaskedValue, true);
                }
                else
                {
                    await DynamicValueTest(args.PasteValue, true);
                }
            }
        }

        internal async override Task OnAfterScriptRendered()
        {
            var options = GetProperty();
            if (InputBaseObj?.InputElement != null)
            {
                await InvokeMethod("sfBlazor.MaskedTextBox.initialize", new object[] { InputBaseObj?.ContainerElement, InputBaseObj?.InputElement, DotnetObjectReference, options });
            }
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                _ = SfBaseUtils.InvokeEvent<object>(Destroyed, null);
                SplitMask = null;
                CustomRegExpCollec = new List<string>();
                Props = new List<string>();
                InputBaseObj = null;
            }
        }
    }
}
