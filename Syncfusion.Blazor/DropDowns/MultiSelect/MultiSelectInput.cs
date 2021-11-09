using System;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Inputs;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The MultiSelect component contains a list of predefined values from which a single value can be chosen.
    /// </summary>
    public partial class SfMultiSelect<TValue, TItem> : SfDropDownBase<TItem>, IDropDowns
    {
        private bool IsSelectedItem
        {
            get
            {
                List<SelectedData<TItem>> selectedDataItems = SelectedData;
                if (selectedDataItems == null)
                {
                    return false;
                }

                return selectedDataItems.Any<SelectedData<TItem>>();
            }
        }

        private string InputPlaceholder
        {
            get
            {
                if (!IsSelectedItem && !string.IsNullOrEmpty(Placeholder))
                {
                    return Placeholder;
                }

                return string.Empty;
            }
        }

        private int InputSize
        {
            get
            {
                int length = InputPlaceholder != null ? InputPlaceholder.Length : 0;
                int val2 = Math.Max(InputValue != null ? InputValue.Length : 0, 1);
                return length;
            }
        }

        private void UpdateInputAttr()
        {
            if (InputAttributes != null && InputAttributes.Count > 0)
            {
                foreach (var attr in InputAttributes)
                {
                    SfBaseUtils.UpdateDictionary(attr.Key, attr.Value, inputAttr);
                }
            }
        }

        /// <summary>
        /// Method which set RTL to the component.
        /// </summary>
        /// <exclude/>
        protected void SetRTL()
        {
            PopupContainer = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
            ContainerClass = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(ContainerClass, RTL) : SfBaseUtils.RemoveClass(ContainerClass, RTL);
        }

        private void SetReadOnly()
        {
            if (Readonly)
            {
                inputAttr = SfBaseUtils.UpdateDictionary(READONLY, Readonly, inputAttr);
            }
            else
            {
                if (Mode != VisualMode.CheckBox)
                {
                    inputAttr.Remove(READONLY);
                }
            }
        }

        private void SetEnabled()
        {
            if (!Enabled)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, DISABLE);
                inputAttr = SfBaseUtils.UpdateDictionary(DISABLED, DISABLED, inputAttr);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, TRUE, inputAttr);
            }
            else
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, DISABLE);
                inputAttr.Remove(DISABLED);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_DISABLED, FALSE, inputAttr);
            }
        }

        private void CreateFloatingLabel()
        {
            if (FloatLabelType == FloatLabelType.Auto && !ContainerClass.Contains(INPUTFOCUS, StringComparison.Ordinal) && !IsFocused)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, inputAttr);
                FloatLabel = string.IsNullOrEmpty(InputValue) ? FLOATTEXT + " " + FLOATLABELBOTTOM : FLOATTEXT + " " + FLOATLABELTOP;
            }
            else if (FloatLabelType == FloatLabelType.Always)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, FLOATINPUT);
                ContainerClass = ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) ? SfBaseUtils.AddClass(ContainerClass, VALIDINPUT) : ContainerClass;
                inputAttr = SfBaseUtils.UpdateDictionary(ARIA_LABEL_BY, "label_" + ID, inputAttr);
                FloatLabel = FLOATTEXT + " " + FLOATLABELTOP;
            }

            if (FloatLabelType == FloatLabelType.Never)
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, FLOATINPUT);
            }
        }

        private void CheckInputValue(FloatLabelType floatLabelType, string inputValue)
        {
            ContainerClass = !String.IsNullOrEmpty(inputValue) ? SfBaseUtils.AddClass(ContainerClass, VALIDINPUT) : (floatLabelType != FloatLabelType.Always) ?
                    SfBaseUtils.RemoveClass(ContainerClass, VALIDINPUT) : ContainerClass;
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
                        else if (item.Key == STYLE && ContainerAttr.ContainsKey(STYLE))
                        {
                            ContainerAttr[item.Key] += item.Value.ToString();
                        }
                        else
                        {
                            ContainerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, ContainerAttr);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method which set css class to the component.
        /// </summary>
        /// <exclude/>
        protected void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = (ContainerClass != null && !ContainerClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(ContainerClass, CssClass) : ContainerClass;
                PopupContainer = (PopupContainer != null && !PopupContainer.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupContainer, CssClass) : PopupContainer;
            }
        }

        private async Task BlurHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args = null, bool isFilterinput = false)
        {
            IsFocused = false;
            IsDropDownClick = false;
            if (!AllowCustomValue)
            {
                InputValue = null;
                await SetInputValue();
            }

            var inputTextVal = string.IsNullOrEmpty(InputValue) ? GetDelimValues() : InputValue;
            if (FloatLabelType == FloatLabelType.Auto && string.IsNullOrEmpty(inputTextVal))
            {
                UpdateFloatBottom();
            }

            if (ContainerClass.Contains(INPUT_GROUP, StringComparison.Ordinal) || ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || ContainerClass.Contains(FILLED, StringComparison.Ordinal))
            {
                ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, INPUTFOCUS);
            }

            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState(inputTextVal);
            }

            UpdateDefaultView(IsFocused);
            if (!isFilterinput && Mode != VisualMode.Box)
            {
                await UpdateDelimViews();
            }

            if (SelectedData != null && SelectedData.Count > 0)
            {
                RemoveChipSelection();
            }

            if (IsListRender && !(AllowFiltering && !(isFilterinput && !IsDevice)) && !IsFilterClearClicked)
            {
                await HidePopup();
            }

            if (Mode == VisualMode.Box)
            {
                SearchBoxElement = (!string.IsNullOrEmpty(InputValue) && !AllowCustomValue) ? SfBaseUtils.AddClass(SearchBoxElement, ZERO_SIZE) : SfBaseUtils.RemoveClass(SearchBoxElement, ZERO_SIZE);
                StateHasChanged();
            }

            await OnChangeEvent(args);
            if (!isFilterinput && !IsListRender)
            {
                IsInternalFocus = false;
                await SfBaseUtils.InvokeEvent<object>(MultiselectEvents?.Blur, null);
            }

            UpdateValidateClass();
            await SfBaseUtils.InvokeEvent(OnBlur, args);
        }

        private async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            IsFocused = true;
            if (ContainerClass.Contains(INPUT_GROUP, StringComparison.Ordinal) || ContainerClass.Contains(OUTLINE, StringComparison.Ordinal) || ContainerClass.Contains(FILLED, StringComparison.Ordinal))
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, INPUTFOCUS);
            }

            UpdateDelimClass();
            UpdateDefaultView(IsFocused);
            if (Mode == VisualMode.Box)
            {
                SearchBoxElement = SfBaseUtils.RemoveClass(SearchBoxElement, ZERO_SIZE);
            }

            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateFloatTop();
            }

            if (!IsInternalFocus)
            {
                await SfBaseUtils.InvokeEvent<object>(MultiselectEvents?.Focus, null);
            }

            await SfBaseUtils.InvokeEvent(OnFocus, args);
        }

        private async Task InputHandler(ChangeEventArgs args)
        {
            InputValue = (string)args.Value;
            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState((string)args.Value);
            }

            IsValidKey = true;
            CheckInputValue(FloatLabelType, InputValue);
            await SfBaseUtils.InvokeEvent(OnInput, args);
        }

        private async Task ChangeHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (FloatLabelType == FloatLabelType.Auto)
            {
                UpdateLabelState((string)args.Value);
            }

            CheckInputValue(FloatLabelType, InputValue);
            await SfBaseUtils.InvokeEvent(OnChange, args);
        }

        private void ValidateLabel(string textValue = null)
        {
            if (ContainerClass.Contains(FLOATINPUT, StringComparison.Ordinal) && FloatLabelType == FloatLabelType.Auto)
            {
                var inputTextVal = string.IsNullOrEmpty(textValue) ? GetDelimValues() : textValue;
                UpdateLabelState(inputTextVal);
            }
        }

        private void UpdateLabelState(string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                UpdateFloatTop();
            }
            else
            {
                UpdateFloatBottom();
            }
        }

        private void UpdateFloatTop()
        {
            FloatLabel = SfBaseUtils.AddClass(FloatLabel, FLOATLABELTOP);
            FloatLabel = SfBaseUtils.RemoveClass(FloatLabel, FLOATLABELBOTTOM);
        }

        private void UpdateFloatBottom()
        {
            FloatLabel = SfBaseUtils.AddClass(FloatLabel, FLOATLABELBOTTOM);
            FloatLabel = SfBaseUtils.RemoveClass(FloatLabel, FLOATLABELTOP);
        }

        private void UpdateAriaAttributes()
        {
            SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(AUTO_COMPLETE, OFF, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + OPTIONS, inputAttr);
            SfBaseUtils.UpdateDictionary(ROLE, LIST_BOX, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, NULL_VALUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, TRUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, inputAttr);
            SfBaseUtils.UpdateDictionary(AUTO_MULTI_SELECT, TRUE, inputAttr);
            SfBaseUtils.UpdateDictionary(ARIA_DESCRIBE, string.Empty, inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(TYPE, "text", inputAttr);
            inputAttr = SfBaseUtils.UpdateDictionary(TAB_INDEX, TabIndex, inputAttr);
        }

        private void UpdateValidateClass()
        {
            if (ValueExpression != null && DropDownsEditContext != null)
            {
                var fieldIdentifier = FieldIdentifier.Create(ValueExpression);

                ValidClass = DropDownsEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
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