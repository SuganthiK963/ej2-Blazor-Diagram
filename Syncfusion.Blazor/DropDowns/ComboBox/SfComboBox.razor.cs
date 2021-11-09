using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Syncfusion.Blazor.Inputs.Internal;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;
using System.Reflection;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The ComboBox component allows the user to type a value or choose an option from the list of predefined options.
    /// </summary>
    /// <typeparam name="TValue">Specifies the value type.</typeparam>
    /// <typeparam name="TItem">Specifies the type of SfComboBox.</typeparam>
    public partial class SfComboBox<TValue, TItem> : SfDropDownList<TValue, TItem>, IInputBase, IDropDowns
    {
        private const string NO_RECORD_LOCALE_KEY = "ComboBox_NoRecordsTemplate";
        private const string NO_RECORD_LOCALE_VALUE = "No records found";
        private const string ACTION_FAILURE_LOCALE_KEY = "ComboBox_ActionFailureTemplate";
        private const string ACTION_FAILURE_LOCALE_VALUE = "The action failure";

        /// <summary>
        /// Specifies the root class of the component.
        /// </summary>
        /// <exclude/>
        protected override string ROOT { get; set; } = "e-control e-combobox e-lib";

        internal ComboBoxEvents<TValue, TItem> ComboBoxEvents { get; set; }

        /// <summary>
        /// Specifies the component name.
        /// </summary>
        /// <exclude/>
        protected override string ComponentName { get; set; } = "SfComboBox";

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (string.IsNullOrEmpty(ID) && SfBaseUtils.Equals(ComponentName, nameof(SfComboBox<TValue, TItem>)))
            {
                ID = SfBaseUtils.GenerateID("combobox");
            }

            // Called In-placeEditor method for value updating
            if (ComboBoxParent != null && Convert.ToString(ComboBoxParent.Type, CultureInfo.CurrentCulture) == "ComboBox")
            {
                ComboBoxParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ComboBoxParent, this);
            }
        }

        /// <summary>
        /// Task which specifies whether filter action is allowed or not.
        /// </summary>
        /// <returns>bool.</returns>
        /// <exclude/>
        protected override bool IsFilter()
        {
            return AllowFiltering;
        }

        /// <summary>
        /// Method which updates main data.
        /// </summary>
        /// <exclude/>
        protected override void UpdateMainData()
        {
            MainData = ListData;
        }

        /// <summary>
        /// Triggers before popup get opened.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<BeforeOpenEventArgs> InvokeBeforeOpen()
        {
            if (IsFilter() && MainData != null)
            {
                ListData = MainData;
            }

            var beforeOpenArgs = new BeforeOpenEventArgs() { Cancel = false };
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, InputAttributes);
            await SfBaseUtils.InvokeEvent<BeforeOpenEventArgs>(ComboBoxEvents?.OnOpen, beforeOpenArgs);
            return beforeOpenArgs;
        }

        /// <summary>
        /// Triggers after the popup get closed.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<ClosedEventArgs> InvokeAfterClosed()
        {
            var closedArgs = new ClosedEventArgs() { };
            await SfBaseUtils.InvokeEvent<ClosedEventArgs>(ComboBoxEvents?.Closed, closedArgs);
            return closedArgs;
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
                if (ComponentName == nameof(SfComboBox<TValue, TItem>) && IsFilter())
                {
                    await Render(DataSource, Fields, Query);
                    MainData = ListData;
                }

                PreviousValue = Value;
                await SfBaseUtils.InvokeEvent<object>(ComboBoxEvents?.Created, null);
                if (ComboBoxParent != null)
                {
                    ComboBoxParent.GetType().GetMethod("UpdatePreviewValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(ComboBoxParent, new object[] { true });
                }
            }
        }

        /// <summary>
        /// Method which update the aria attributes.
        /// </summary>
        /// <exclude/>
        protected override void UpdateAriaAttributes()
        {
            SfBaseUtils.UpdateDictionary(ARIA_LIVE, ASSERTIVE, InputAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, TRUE, InputAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_ACTIVE_DESCENDANT, NULL_VALUE, InputAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_OWN, ID + OPTIONS, InputAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, FALSE, InputAttributes);
            SfBaseUtils.UpdateDictionary(ROLE, LIST_BOX, InputAttributes);
            SfBaseUtils.UpdateDictionary(AUTO_CORRECT, OFF, InputAttributes);
            SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, InputAttributes);
            SfBaseUtils.UpdateDictionary(AUTO_CAPITAL, OFF, InputAttributes);
            SfBaseUtils.UpdateDictionary(AUTO_COMPLETE, OFF, InputAttributes);
        }

        /// <summary>
        /// Triggers when the component get focused in.
        /// </summary>
        /// <param name="args">Specifies the FocusEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            UpdateParentClass(RootClass, ContainerClass);
            if (!IsInternalFocus)
            {
                await SfBaseUtils.InvokeEvent<object>(ComboBoxEvents?.Focus, null);
            }
        }

        /// <summary>
        /// Triggers when component get focused out.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task InvokeBlur()
        {
            if (!IsFilterClearClicked)
            {
                await SfBaseUtils.InvokeEvent<object>(ComboBoxEvents?.Blur, null);
            }

            IsFilterClearClicked = false;
            IsInternalFocus = false;
        }

        /// <summary>
        /// Triggers when value get changed.
        /// </summary>
        /// <param name="args">Specifies ChangeEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task OnChangeHandler(ChangeEventArgs args)
        {
            if (FocusedData != null)
            {
                await OnChangeEvent();
            }
            else
            {
                if (!string.IsNullOrEmpty(InputBaseObj?.InputTextValue) && (InputBaseObj.InputTextValue != DDLText || ValidateOnInput ))
                {
                    await CustomValue();
                }
            }

            if (IsListRender)
            {
                await HidePopup();
            }

            await SfBaseUtils.InvokeEvent<ChangeEventArgs>(OnChange, args);
        }

        /// <summary>
        /// Triggers when the component get focused out.
        /// </summary>
        /// <param name="args">Specifies the FocusEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task FocusOutHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args = null)
        {
            if (IsListRender)
            {
                await HidePopup();
            }

            IsDropDownClick = false;
            if (FocusedData == null && ListData != null && !string.IsNullOrEmpty(InputBaseObj?.InputTextValue) && InputBaseObj.InputTextValue != DDLText)
            {
                await CustomValue();
            }

            await OnChangeEvent();
            UpdateValidateClass();
            await InvokeBlur();
            await SfBaseUtils.InvokeEvent(OnBlur, args);
        }

        /// <summary>
        /// Triggers when any value provided to filter input.
        /// </summary>
        /// <param name="args">Specifies the ChangeEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task FilterInputHandler(ChangeEventArgs args)
        {
            IsValidKey = true;
            if (ValidateOnInput && DropDownsEditContext != null)
            {
                if (AllowCustom && args != null && !string.IsNullOrEmpty((string)args.Value))
                {
                    await CustomValue(true);
                } else if (args == null || string.IsNullOrEmpty((string)args.Value))
                {
                    await UpdateValueSelection(default);
                }
            }
            await SfBaseUtils.InvokeEvent<ChangeEventArgs>(OnInput, args);
        }

        /// <summary>
        /// Invokes the popup event.
        /// </summary>
        /// <param name="isOpen">True if the popup is in open state otherwise false.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<PopupEventArgs> InvokePopupEvents(bool isOpen)
        {
            if (Autofill && InputBaseObj != null && !isOpen)
            {
                await InvokeMethod("sfBlazor.DropDownList.removeFillSelection", new object[] { InputBaseObj.InputElement });
            }

            SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, isOpen ? TRUE : FALSE, InputAttributes);
            var popupEvents = OpenEventArgs();
            await SfBaseUtils.InvokeEvent<PopupEventArgs>(isOpen ? ComboBoxEvents?.Opened : ComboBoxEvents?.OnClose, popupEvents);
            return popupEvents;
        }

        /// <summary>
        /// Triggers when item get selected.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<SelectEventArgs<TItem>> InvokeSelectEvent(TItem item, EventArgs args = null)
        {
            var selectEventArgs = new SelectEventArgs<TItem>()
            {
                Cancel = false,
                IsInteracted = args != null,
                Item = null,
                ItemData = item,
                E = args
            };
            if (InputBaseObj != null && InputBaseObj.InputTextValue != DropdownValue)
            {
                await UpdateInputValue(InputBaseObj.InputTextValue);
                await Task.Delay(10);
            }

            await SfBaseUtils.InvokeEvent<SelectEventArgs<TItem>>(ComboBoxEvents?.OnValueSelect, selectEventArgs);
            return selectEventArgs;
        }

        /// <summary>
        /// Triggers when value get changed.
        /// </summary>
        /// <param name="args">Specifies EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task InvokeChangeEvent(EventArgs args = null)
        {
            var changeEventArgs = new ChangeEventArgs<TValue, TItem>()
            {
                Value = Value,
                ItemData = ItemData,
                PreviousItemData = PreviousItemData,
                Cancel = false,
                IsInteracted = args != null
            };
            await SfBaseUtils.InvokeEvent<ChangeEventArgs<TValue, TItem>>(ComboBoxEvents?.ValueChange, changeEventArgs);
        }

        /// <summary>
        /// Task which updates the value.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task UpdateValue()
        {
            if (Value != null)
            {
                ItemData = GetDataByValue(Value);
                if (ItemData == null && AllowCustom)
                {
                    ItemData = SetItemValue(Value.ToString(), typeof(TValue));
                    await ValueMuteChange();
                }

                if (ItemData == null && EnableVirtualization && DataManager != null && (DataManager.Json?.Count() > ListData?.Count() || TotalCount > ListData?.Count()))
                {
                    await GetScrollValue();
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(DDLText))
            {
                ItemData = GetDataByText(DDLText);
                if (ItemData == null && AllowCustom)
                {
                    ItemData = SetItemValue(DDLText.ToString(), typeof(TValue));
                    await ValueMuteChange();
                }
            }
            else if (Index != null && ListData != null)
            {
                ItemData = ListData.ElementAtOrDefault((int)Index);
            }

            await UpdateValueSelection(ItemData);
        }

        /// <summary>
        /// Task which sets the old value.
        /// </summary>
        /// <param name="ddlValue">Specifies the DropDownList value.</param>
        /// <param name="isValueChanged"></param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task SetOldValue(object ddlValue, bool isValueChanged)
        {
            if (isValueChanged)
            {
                await this.UpdateValue();
            } else
            {
                this.ItemData = (Value != null && AllowCustom) ? SetItemValue(Value.ToString(), typeof(TValue)) : default(TItem);
            }
            await ValueMuteChange();
        }

        private async Task ValueMuteChange()
        {
            await UpdateValueSelection(ItemData, true);
            await OnChangeEvent();
        }

        /// <summary>
        /// Task which update auto fill on down action.
        /// </summary>
        /// <param name="curItem">Specifies the current item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task UpdateAutoFillOnDown(TItem curItem)
        {
            if (Autofill)
            {
                var item = GetItemData(curItem);
                await SetValue(item);
            }
        }

        /// <summary>
        /// Task which specifies the search list.
        /// </summary>
        /// <param name="args">Specifies the KeyboardEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task SearchList(KeyboardEventArgs args = null)
        {
            IsTyped = true;
            ActiveIndex = null;
            if (IsFilter())
            {
                var filterEventArgs = new FilteringEventArgs()
                {
                    BaseEventArgs = args,
                    Cancel = false,
                    PreventDefaultAction = false,
                    Text = InputBaseObj?.InputTextValue
                };
                await SfBaseUtils.InvokeEvent<FilteringEventArgs>(ComboBoxEvents?.Filtering, filterEventArgs);
                if (!filterEventArgs.Cancel && !filterEventArgs.PreventDefaultAction)
                {
                    await FilteringAction(DataSource, Query, Fields);
                }
            }
            else
            {
                await IncrementSearch();
            }
        }

        /// <summary>
        /// Task which incrment the search.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task IncrementSearch()
        {
            if (!IsListRender)
            {
                await ShowPopup();
            }

            if (ListData != null)
            {
                await InlineSearch();
            }
        }

        /// <summary>
        /// Task which performs the in line search.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task InlineSearch()
        {
            var activeItem = await GetFocusItem();
            if (IsTyped && activeItem != null)
            {
                ActiveIndex = ListData.IndexOf(activeItem);
                if (Autofill && !PreventAutoFill)
                {
                    await SetAutoFill(activeItem);
                }
            }
            else
            {
                ActiveIndex = null;
                ItemData = default(TItem);
            }
        }

        private async Task SetAutoFill(TItem activeItem)
        {
            if (Autofill && !PreventAutoFill)
            {
                var currentValue = IsSimpleDataType() ? activeItem.ToString() : DataUtil.GetObject(Fields?.Text, activeItem).ToString();
                if (ComponentName == nameof(SfComboBox<TValue, TItem>))
                {
                    await UpdateSelectedList(activeItem);
                }

                await UpdateFillSelection(currentValue);
            }
        }

        private async Task UpdateFillSelection(string currentText)
        {
            var beforeValue = (string)InputBaseObj?.InputTextValue.Clone();
            var inputValue = beforeValue;
            if (!string.IsNullOrEmpty(inputValue) && SfBaseUtils.Equals(inputValue.ToLower(CultureInfo.CurrentCulture), currentText.Substring(0, inputValue.Length).ToLower(CultureInfo.CurrentCulture)))
            {
                inputValue = inputValue + currentText.Substring(inputValue.Length, currentText.Length - inputValue.Length);
            }
            else
            {
                inputValue = currentText;
            }

            await InvokeMethod("sfBlazor.DropDownList.setAutoFillSelection", new object[] { InputBaseObj.InputElement, currentText });
            DropdownValue = inputValue;
            await InputBaseObj?.SetValue(inputValue, FloatLabelType, ShowClearButton);
        }

        private async Task UpdateSelectedList(TItem item, EventArgs args = null)
        {
            ItemData = item;
            var selectEventArgs = new SelectEventArgs<TItem>()
            {
                Cancel = false,
                IsInteracted = args != null,
                Item = null,
                ItemData = item,
                E = args
            };
            await SfBaseUtils.InvokeEvent<SelectEventArgs<TItem>>(ComboBoxEvents?.OnValueSelect, selectEventArgs);
            if (!selectEventArgs.Cancel)
            {
                ActiveIndex = ListData?.IndexOf(ItemData);
                await UpdateListSelection(ItemData, SELECTED);
            }
        }

        private async Task<TItem> GetFocusItem()
        {
            var dataItem = GetItemData();
            var focusItem = ItemData;
            var isSelected = SfBaseUtils.Equals(dataItem.Text, InputBaseObj?.InputTextValue) && Value != null;
            if (!isSelected && ((IsDevice && !IsDropDownClick) || !IsDevice) && ListData != null && ListData.Any())
            {
                var inputValue = InputBaseObj?.InputTextValue;
                var firstData = new List<TItem>();
                firstData.Add(ListData.FirstOrDefault());
                var listItems = ComponentName == nameof(SfComboBox<TValue, TItem>) ? ListData : firstData;
                focusItem = Search(inputValue, ListData, "startswith", true);
            }

            var searchItem = (focusItem != null && InputBaseObj != null) ? focusItem : default;
            await UpdateListSelection(searchItem, ITEM_FOCUS);
            return focusItem;
        }

        /// <summary>
        /// Task which specifies the custom value event.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task<CustomValueSpecifierEventArgs<TItem>> InvokeCustomValueEvent()
        {
            var customValueEventArgs = new CustomValueSpecifierEventArgs<TItem>()
            {
                Text = InputBaseObj?.InputTextValue
            };
            await SfBaseUtils.InvokeEvent<CustomValueSpecifierEventArgs<TItem>>(ComboBoxEvents?.CustomValueSpecifier, customValueEventArgs);
            return customValueEventArgs;
        }

        /// <summary>
        /// Task which specifies the custom value.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task CustomValue(bool isValidateInput = false)
        {
            var dataItem = (TItem)SfBaseUtils.ChangeType(GetValueByText(InputBaseObj?.InputTextValue, true, true), typeof(TItem));
            if (!AllowCustom && !string.IsNullOrEmpty(InputBaseObj?.InputTextValue))
            {
                await Task.Delay(5);    // added time delay for update the input value to input base component.
                await UpdateValueSelection(dataItem);
            }
            else if (!string.IsNullOrEmpty(InputBaseObj?.InputTextValue))
            {
                if (dataItem == null)
                {
                    var inputValue = InputBaseObj?.InputTextValue;
                    CustomValueSpecifierEventArgs<TItem> customValueEventArgs = await InvokeCustomValueEvent();
                    if (customValueEventArgs.Item != null)
                    {
                        ItemData = customValueEventArgs.Item;
                    }
                    else
                    {
                        ItemData = SetItemValue(inputValue, typeof(TValue));
                    }
                }
                else
                {
                    ItemData = dataItem;
                }

                await UpdateValueSelection(ItemData, true, isValidateInput);
                if (!isValidateInput)
                {
                    await OnChangeEvent();
                }
            }
        }

        /// <summary>
        /// Task which pecifies the action begin event.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<ActionBeginEventArgs> ActionBegin(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actionBeginArgs = new ActionBeginEventArgs
            {
                Cancel = false,
                //Data = dataSource,
                Query = query,
                EventName = "ActionBegin"
            };
            await SfBaseUtils.InvokeEvent<ActionBeginEventArgs>(ComboBoxEvents?.OnActionBegin, actionBeginArgs);
            await ShowSpinner();
            return actionBeginArgs;
        }

        /// <summary>
        /// Task which specifies the action failure event.
        /// </summary>
        /// <param name="args">Specifies the object arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task ActionFailure(object args)
        {
            await HideSpinner();
            if (BeforePopupOpen && !IsListRender)
            {
                await ShowPopup();
            }

            await SfBaseUtils.InvokeEvent<Exception>(ComboBoxEvents?.OnActionFailure, (Exception)args);
        }

        private async Task ClearEventHandler(EventArgs args = null)
        {
            if (IsFilter() && IsListRender)
            {
                await FilterClear();
            }

            if (InputBaseObj != null)
            {
                await UpdateInputValue(InputBaseObj.InputTextValue);
                await Task.Delay(10);
            }

            await ClearAll(args);
            UpdateValidateClass();
        }

        /// <summary>
        /// Task which specifies the action complete event.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task ActionComplete(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actioncompleteArgs = new ActionCompleteEventArgs<TItem>
            {
                Cancel = false,
                Result = dataSource,
                Query = query,
                Count = dataSource != null ? dataSource.ToList().Count : 0,
                EventName = "ActionComplete"
            };
            await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(ComboBoxEvents?.OnActionComplete, actioncompleteArgs);
            if (!actioncompleteArgs.Cancel)
            {
                ListData = actioncompleteArgs.Result;
                if (!AllowCustom)
                {
                    await RenderEqualItems();
                }
                await RenderItems();
                await HideSpinner();
                if (BeforePopupOpen && !IsListRender && IsTyped)
                {
                    await ShowPopup();
                }

                if (Autofill && !PreventAutoFill && IsTyped && ListData != null && ListData.Any())
                {
                    await InlineSearch();
                }
            }
        }

        /// <summary>
        /// Update the combobox fileds.
        /// </summary>
        /// <param name="fieldValue">Specifies the field value.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void UpdateChildProperties(object fieldValue)
        {
            var fields = (ComboBoxFieldSettings)fieldValue;
            Fields = new FieldSettingsModel() { GroupBy = fields?.GroupBy, HtmlAttributes = fields?.HtmlAttributes, IconCss = fields?.IconCss, Text = fields?.Text, Value = fields?.Value };
            SetFields();
        }

        internal override void ComponentDispose()
        {
            if (IsRendered)
            {
                var destroyArgs = new object[] { InputBaseObj?.InputElement, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetProperty() };
                InvokeMethod("sfBlazor.DropDownList.destroy", destroyArgs).ContinueWith(t =>
                {
                    _ = SfBaseUtils.InvokeEvent<object>(ComboBoxEvents?.Destroyed, null);
                }, TaskScheduler.Current);
                ObservableEventDisposed();
            }

            InputBaseObj = null;
            FilterinputObj = null;
            ComboBoxEvents = null;
            ContainerAttr = null;
        }
    }
}