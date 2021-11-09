using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using System.Globalization;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The MultiSelect component contains a list of predefined values from which a multiple value can be chosen.
    /// </summary>
    public partial class SfMultiSelect<TValue, TItem> : SfDropDownBase<TItem>, IDropDowns
    {
        private bool IsTyped { get; set; }

        private async Task OnFilterUp(KeyboardEventArgs args)
        {
            if (args != null && !(args.CtrlKey && args.Code == "KeyV") && IsValidKey)
            {
                IsValidKey = false;
                TypedString = InputValue;
                if (Mode == VisualMode.CheckBox && AllowFiltering)
                {
                    TypedString = FilterinputObj?.InputTextValue;
                }

                await SearchList(args);
                IsFilterInputChange = true;
            }

            IsValidKey = false;
            await SfBaseUtils.InvokeEvent(OnKeyUp, args);
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

        private async Task SearchList(KeyboardEventArgs args = null)
        {
            IsTyped = true;
            if (IsFilter())
            {
                var filterEventArgs = new FilteringEventArgs()
                {
                    BaseEventArgs = args,
                    Cancel = false,
                    PreventDefaultAction = false,
                    Text = TypedString
                };
                await SfBaseUtils.InvokeEvent<FilteringEventArgs>(MultiselectEvents?.Filtering, filterEventArgs);
                if (!filterEventArgs.Cancel && !filterEventArgs.PreventDefaultAction)
                {
                    await FilteringAction(DataSource, Query, Fields);
                }
            }
            else if (AllowCustomValue)
            {
                await FilteringAction(DataSource, Query, Fields);
            }
            else
            {
                await TypeOnOpen();
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
            if (!IsListRender && OpenOnClick)
            {
                await ShowPopup();
            }

            if (ListData != null)
            {
                await GetFocusItem();
            }
        }

        private async Task<TItem> GetFocusItem()
        {
            var focusItem = default(TItem);
            if (((IsDevice && !IsDropDownClick) || !IsDevice) && ListData != null && ListData.Any())
            {
                focusItem = Search(InputValue, ListData, "startswith", true);
            }

            if (focusItem != null)
            {
                var focusedItem = ListDataSource.Where(item => SfBaseUtils.Equals(item.CurItemData, focusItem)).FirstOrDefault();
                await UpdateListSelection(focusedItem, ITEM_FOCUS, true);
            }
            else if (string.IsNullOrEmpty(InputValue))
            {
                await ListFocus();
            }
            else
            {
                RemoveFocusList();
            }

            return focusItem;
        }

        /// <summary>
        /// Triggers when paste action is performes.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task PasteHandler()
        {
            if (IsFilter())
            {
                await Task.Delay(100);    // Added delay for get the pasted content in seach input.
                TypedString = FilterinputObj?.InputTextValue;
                await SearchList(null);
            }
        }

        /// <summary>
        /// Triggers when filter clear icon is clicked.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task FilterClear()
        {
            if (!IsDevice)
            {
                FilterClearBtnStopPropagation = true;
            }

            IsFilterClearClicked = true;
            if (!string.IsNullOrEmpty(FilterInputValue) || !string.IsNullOrEmpty(TypedString))
            {
                FilterInputValue = null;
                InputValue = null;
                await SetInputValue();
                TypedString = string.Empty;
                if (FilterinputObj != null)
                {
                    await FilterinputObj.SetValue(string.Empty, Inputs.FloatLabelType.Never, false);
                }

                // Added delay for cleared the filtering conent in seach input.
                await Task.Delay(50);
                await SearchList();
                IsFilterInputChange = true;
            }
        }

        /// <summary>
        /// Task which specifies the filtering action.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <param name="fields">Specifies the fields.</param>
        /// <returns>Task.</returns>
        protected async Task FilteringAction(IEnumerable<TItem> dataSource, Query query, FieldSettingsModel fields)
        {
            BeforePopupOpen = true;
            query = string.IsNullOrEmpty(TypedString?.Trim()) ? null : query;
            if (string.IsNullOrEmpty(TypedString))
            {
                ListData = MainData;
                await RenderItems();
                await ListFocus();
                SetReOrder();
            }
            else
            {
                await SetListData(dataSource, fields, query);
                if (!AllowCustomValue)
                {
                    await ListFocus();
                }
            }

            if (AllowFiltering)
            {
                UpdateMaximumLength();
            }
        }

        /// <summary>
        /// Task which gets the query.
        /// </summary>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Query.</returns>
        /// <exclude/>
        protected override Query GetQuery(Query query)
        {
            Query filterQuery = new Query();
            if (AllowFiltering && IsTyped && !IsCustomFilter)
            {
                filterQuery = (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
                string filterType = string.IsNullOrEmpty(TypedString) ? "contains" : FilterType.ToString().ToLower(CultureInfo.CurrentCulture);
                var isSimpleDataType = !DataManager.IsDataManager && IsSimpleDataType();
                string fields = (!string.IsNullOrEmpty(Fields?.Text) && !isSimpleDataType) ? Fields.Text : string.Empty;
                var filterValue = !string.IsNullOrEmpty(TypedString) ? TypedString : string.Empty;
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
            }
            else
            {
                filterQuery = (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
            }

            return filterQuery;
        }
    }
}