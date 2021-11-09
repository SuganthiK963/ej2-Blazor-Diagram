using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Inputs.Internal;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;
using System.Globalization;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The AutoComplete component provides the matched suggestion list when type into the input, from which the user can select one.
    /// </summary>
#pragma warning disable CA1501 // Mark members as hierarchy
    public partial class SfAutoComplete<TValue, TItem> : SfComboBox<TValue, TItem>, IInputBase, IDropDowns
#pragma warning restore CA1501 // Mark members as hierarchy
    {
        private const string NO_RECORD_LOCALE_KEY = "AutoComplete_NoRecordsTemplate";
        private const string NO_RECORD_LOCALE_VALUE = "No records found";
        private const string ACTION_FAILURE_LOCALE_KEY = "AutoComplete_ActionFailureTemplate";
        private const string ACTION_FAILURE_LOCALE_VALUE = "The action failure";

        /// <summary>
        /// Specifies the root class of the component.
        /// </summary>
        /// <exclude/>
        protected override string ROOT { get; set; } = "e-control e-autocomplete e-lib";

        internal AutoCompleteEvents<TValue, TItem> AutocompleteEvents { get; set; }

        /// <summary>
        /// Specifies the component name.
        /// </summary>
        /// <exclude/>
        protected override string ComponentName { get; set; } = "SfAutoComplete";

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID("autocomplete");
            }

            if (AutoCompleteParent != null && Convert.ToString(AutoCompleteParent.Type, CultureInfo.CurrentCulture) == "AutoComplete")
            {
                AutoCompleteParent.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(AutoCompleteParent, this);
            }
        }

        /// <summary>
        /// Specifies whether filter option is allowed or not.
        /// </summary>
        /// <returns>bool.</returns>
        /// <exclude/>
        protected override bool IsFilter()
        {
            return true;
        }

        /// <summary>
        /// Triggers before the popup get opened.
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
            await SfBaseUtils.InvokeEvent<BeforeOpenEventArgs>(AutocompleteEvents?.OnOpen, beforeOpenArgs);
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
            await SfBaseUtils.InvokeEvent<ClosedEventArgs>(AutocompleteEvents?.Closed, closedArgs);
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
                PreviousValue = Value;
                await SfBaseUtils.InvokeEvent<object>(AutocompleteEvents?.Created, null);

                // Called In-placeEditor method for value updating
                if (AutoCompleteParent != null)
                {
                    AutoCompleteParent.GetType().GetMethod("UpdatePreviewValue", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(AutoCompleteParent, new object[] { true });
                }
            }
        }

        /// <summary>
        /// Invoked when popup get opened.
        /// </summary>
        /// <param name="isOpen">True if the popup in open state.</param>
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
            await SfBaseUtils.InvokeEvent<PopupEventArgs>(isOpen ? AutocompleteEvents?.Opened : AutocompleteEvents?.OnClose, popupEvents);
            return popupEvents;
        }

        /// <summary>
        /// Method which returns the selected event arguments.
        /// </summary>
        /// <param name="item">Specifies the item.</param>
        /// <param name="args">Speciifes the EventArgs arguments.</param>
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

            await SfBaseUtils.InvokeEvent<SelectEventArgs<TItem>>(AutocompleteEvents?.OnValueSelect, selectEventArgs);
            return selectEventArgs;
        }

        /// <summary>
        /// Method which returns the change event arguments.
        /// </summary>
        /// <param name="args">Specifies the EventArgs arguments.</param>
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
            await SfBaseUtils.InvokeEvent<ChangeEventArgs<TValue, TItem>>(AutocompleteEvents?.ValueChange, changeEventArgs);
        }

        /// <summary>
        /// Task which specifies the search list.
        /// </summary>
        /// <param name="args">Specifies the KeyboardEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task SearchList(KeyboardEventArgs args = null)
        {
            ActiveIndex = null;
            IsTyped = true;
            if (IsFilter())
            {
                if (args != null && (args.Code == "ArrowDown" || args.Code == "ArrowUp"))
                {
                    BeforePopupOpen = true;
                    await SetListData(DataSource, Fields, Query);
                    return;
                }

                var filterEventArgs = new FilteringEventArgs()
                {
                    BaseEventArgs = args,
                    Cancel = false,
                    PreventDefaultAction = false,
                    Text = TypedString
                };
                await SfBaseUtils.InvokeEvent<FilteringEventArgs>(AutocompleteEvents?.Filtering, filterEventArgs);
                if (!filterEventArgs.Cancel && !filterEventArgs.PreventDefaultAction)
                {
                    if (!string.IsNullOrEmpty(TypedString) && TypedString.Length >= MinLength)
                    {
                        await FilteringAction(DataSource, Query, Fields);
                    }
                    else
                    {
                        await HidePopup();
                    }
                }
            }
            else
            {
                await IncrementSearch();
            }
        }

        /// <summary>
        /// Triggers when the component get focused in.
        /// </summary>
        /// <param name="args">Specifies the FocusEventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task FocusHandler(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            UpdateParentClass(RootClass, ContainerClass);
            if (!IsInternalFocus)
            {
                await SfBaseUtils.InvokeEvent<object>(AutocompleteEvents?.Focus, null);
            }
        }

        /// <summary>
        /// Triggers when component get focused out.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task InvokeBlur()
        {
            if (!IsFilterClearClicked)
            {
                await SfBaseUtils.InvokeEvent<object>(AutocompleteEvents?.Blur, null);
            }

            IsFilterClearClicked = false;
            IsInternalFocus = false;
        }

        /// <summary>
        /// Specifies whether it is edit textbox.
        /// </summary>
        /// <returns>bool.</returns>
        /// <exclude/>
        protected override bool IsEditTextBox()
        {
            return true && !string.IsNullOrEmpty(InputBaseObj.InputTextValue?.Trim());
        }

        /// <summary>
        /// Task which updates the focus item.
        /// </summary>
        /// <param name="focusItem">Specifies the focus item.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task UpdateFocusItem(TItem focusItem = default)
        {
            if (focusItem != null && IsListRender && InputBaseObj != null)
            {
                await UpdateListSelection(focusItem, ITEM_FOCUS);
            }
        }

        /// <summary>
        /// Method which gets the query.
        /// </summary>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override Query GetQuery(Query query)
        {
            if (FilterinputObj != null && !IsCustomFilter)
            {
                var propertyType = typeof(TValue);
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                var isObjectType = propertyType == typeof(TItem);
                Query filterQuery = (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
                var isFilterValue = string.IsNullOrEmpty(TypedString) && Value != null && !isObjectType;
                string filterType = isFilterValue ? "equal" : FilterType.ToString().ToLower(CultureInfo.CurrentCulture);
                var isSimpleDataType = !DataManager.IsDataManager && IsSimpleDataType();
                string fields = (!string.IsNullOrEmpty(Fields?.Value) && !isSimpleDataType) ? Fields.Value : string.Empty;
                var filterValue = isFilterValue ? Value.ToString() : string.IsNullOrEmpty(TypedString) ? string.Empty : TypedString;
                if (fields.Split(new char[] { '.' }).Length > 1)
                {
                    List<WhereFilter> whereFilters = new List<WhereFilter>();
                    whereFilters.Add(new WhereFilter() { Field = fields, Operator = filterType, value = filterValue, IgnoreCase = IgnoreCase, IgnoreAccent = IgnoreAccent });
                    filterQuery.Where(new WhereFilter() { Condition = "or", IsComplex = true, predicates = whereFilters });
                }
                else
                {
                    filterQuery.Where(new WhereFilter() { Field = fields, Operator = filterType, value = filterValue, IgnoreCase = IgnoreCase, IgnoreAccent = IgnoreAccent });
                }

                if (!(query?.Queries?.Take != null && query?.Queries?.Take > 0))
                {
                    filterQuery.Take(SuggestionCount);
                }

                return filterQuery;
            }
            else
            {
                return (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
            }
        }

        /// <summary>
        /// Specifies the clear all event.
        /// </summary>
        /// <param name="args">Specifies the EventArgs arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task ClearAll(EventArgs args = null)
        {
            await base.ClearAll(args);
            IsInternalFocus = true;
            await FocusIn();
            if (IsListRender)
            {
                await HidePopup();
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
            await SfBaseUtils.InvokeEvent<ActionBeginEventArgs>(AutocompleteEvents?.OnActionBegin, actionBeginArgs);
            await ShowSpinner();
            return actionBeginArgs;
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
                await CreatePopup();
            }

            await SfBaseUtils.InvokeEvent<Exception>(AutocompleteEvents?.OnActionFailure, (Exception)args);
        }

        private async Task ClearEventHandler(EventArgs args = null)
        {
            if (AllowFiltering && IsListRender)
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
            await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(AutocompleteEvents?.OnActionComplete, actioncompleteArgs);
            if (!actioncompleteArgs.Cancel)
            {
                ListData = actioncompleteArgs.Result;
                await RenderItems();
                await HideSpinner();
                if (BeforePopupOpen && !IsListRender)
                {
                    await CreatePopup();
                }

                if (Autofill && !PreventAutoFill && IsTyped && ListData != null && ListData.Any())
                {
                    await InlineSearch();
                }
            }
        }

        /// <summary>
        /// Task which specifies the custom value event.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task<CustomValueSpecifierEventArgs<TItem>> InvokeCustomValueEvent()
        {
            var customValueEventArgs = new CustomValueSpecifierEventArgs<TItem>()
            {
                Text = InputBaseObj?.InputTextValue
            };
            await SfBaseUtils.InvokeEvent<CustomValueSpecifierEventArgs<TItem>>(AutocompleteEvents?.CustomValueSpecifier, customValueEventArgs);
            return customValueEventArgs;
        }

        /// <summary>
        /// Task which specifies the list item created event.
        /// </summary>
        /// <param name="listItem">Specifies the list item.</param>
        /// <returns>ListOptions.</returns>
        /// <exclude/>
        protected override ListOptions<TItem> ListItemCreated(ListOptions<TItem> listItem)
        {
            var item = listItem;
            if (Highlight && item != null)
            {
                item.Text = HighlightSearch(item.Text, InputBaseObj?.InputTextValue, IgnoreCase, FilterType);
            }

            return item;
        }

        /// <summary>
        /// Highlight the searched characters on suggested list items.
        /// </summary>
        /// <param name="textValue">highlight the list item.</param>
        /// <param name="ignoreCase">performing the search  text based on casing.</param>
        /// <param name="filtertype">Determines on which filter type, the highlight text update on the text.</param>
        /// <param name="highLighText"> Higlighted the char based on hightligh text and this is  optional. If not provide the highlightText, it wil get the filter value.</param>
        /// <returns>Returns highlight string.</returns>
        public string HighLightSearch(string textValue, bool ignoreCase, FilterType filtertype, string highLighText = null)
        {
            var searchText = string.IsNullOrEmpty(highLighText) ? FilterinputObj?.InputTextValue : highLighText;
            return HighlightSearch(textValue, searchText, ignoreCase, filtertype);
        }

        /// <summary>
        /// Update the autocomplete fileds.
        /// </summary>
        /// <param name="fieldValue">Specifies the field value.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void UpdateChildProperties(object fieldValue)
        {
            var fields = (AutoCompleteFieldSettings)fieldValue;
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
                    _ = SfBaseUtils.InvokeEvent<object>(AutocompleteEvents?.Destroyed, null);
                }, TaskScheduler.Current);
                ObservableEventDisposed();
            }

            InputBaseObj = null;
            FilterinputObj = null;
            AutocompleteEvents = null;
            ContainerAttr = null;
        }
    }
}