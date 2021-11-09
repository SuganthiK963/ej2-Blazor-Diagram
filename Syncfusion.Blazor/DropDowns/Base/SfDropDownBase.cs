using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Data;
using System.Runtime.Serialization;
using System.IO;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Dynamic;
using System.ComponentModel;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// The DropDownBase can be created data source and fetch the list of data from data bound component.
    /// </summary>
    public partial class SfDropDownBase<T> : SfDataBoundComponent
    {
        internal const string LIST_ITEM = "e-list-item";

        internal const string LIST_ICON = "e-list-icon";

        internal const string SPACE = " ";

        internal const string GROUP_ITEM = "e-list-group-item";

        /// <summary>
        /// Specifies the list data.
        /// </summary>
        /// <exclude/>
        protected IEnumerable<T> ListData { get; set; }

        /// <summary>
        /// Specifies the main data.
        /// </summary>
        /// <exclude/>
        protected IEnumerable<T> MainData { get; set; }

        /// <summary>
        /// Specifies the list data source.
        /// </summary>
        /// <exclude/>
        protected IEnumerable<ListOptions<T>> ListDataSource { get; set; }

        /// <summary>
        /// Specifies the item data.
        /// </summary>
        /// <exclude/>
        protected T ItemData { get; set; }

        /// <summary>
        /// Specifies the match items.
        /// </summary>
        /// <exclude/>
        private List<T> MatchItems { get; set; }

        /// <summary>
        /// Specifies whether the action get failed or not.
        /// </summary>
        /// <exclude/>
        protected bool IsActionFaiure { get; set; }

        /// <summary>
        /// Specifies the total count.
        /// </summary>
        /// <exclude/>
        protected int TotalCount { get; set; }

        private string PrevString { get; set; }

        /// <summary>
        /// Specifies the list data.
        /// </summary>
        /// <exclude/>
        protected virtual string ComponentName { get; set; } = "SfDropDownBase";

        private bool IsObservableWired { get; set; }

        private string IncrementSearchValue { get; set; } = string.Empty;

        internal bool isToolbarAction;

        /// <summary>
        /// Task which render the component with provided datasource , fields and query.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="fields">Specifies the fields.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exlude/>
        protected async Task Render(IEnumerable<T> dataSource, FieldSettingsModel fields, Query query)
        {
            await SetListData(dataSource, fields, query);
        }

        /// <summary>
        /// Task which sets the list data.
        /// </summary>
        /// <param name="dataSource">Specifies the datasource.</param>
        /// <param name="fields">Specifies the fields.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task SetListData(IEnumerable<T> dataSource, FieldSettingsModel fields, Query query)
        {
            try
            {
                fields = fields != null ? fields : Fields;
                var eventArgs = await ActionBegin(dataSource, query);
                IsActionFaiure = false;
                if (!eventArgs.Cancel)
                {
                    Type dataType = typeof(T);
                    if ((DataManager != null && !DataManager.IsDataManager) && IsSimpleDataType())
                    {
                        var data = SimpleDataExecute(dataSource, GetQuery(eventArgs.Query));
                        await ActionComplete(data, GetQuery(eventArgs.Query));
                    }
                    else
                    {
                        var data = (DataManager != null) ? await DataManager.ExecuteQuery<T>(GetQuery(eventArgs.Query)) : null;
                        var resultData = new List<T>();
                        if (DataManager != null && GetQuery(eventArgs.Query).IsCountRequired)
                        {
                            TotalCount = ((DataResult)data).Count;
                            var dataResult = (((DataResult)data).Result == null) ? new List<object>() : ((DataResult)data).Result;
                            resultData = ((IEnumerable<object>)dataResult).Cast<T>().ToList();
                        }
                        else
                        {
                            var dataResult = (data == null) ? new List<object>() : data;
                            resultData = (dataResult as IEnumerable)?.Cast<T>()?.ToList();
                        }

                        await ActionComplete(resultData, GetQuery(eventArgs.Query));
                    }
                }
            }
            catch (Exception exception)
            {
                IsActionFaiure = true;
                await ActionFailure(exception);
            }
        }

        /// <summary>
        /// Task specifies the action begin.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected virtual async Task<ActionBeginEventArgs> ActionBegin(IEnumerable<T> dataSource, Query query = null)
        {
            return await Task.FromResult(new ActionBeginEventArgs());
        }

        /// <summary>
        /// Task which specifies the action complete.
        /// </summary>
        /// <param name="dataSource">Specifies the data source.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Task.</returns>
        protected virtual async Task ActionComplete(IEnumerable<T> dataSource, Query query = null)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Task which specifies the action failure.
        /// </summary>
        /// <param name="args">Specifies the object arguments.</param>
        /// <returns>Task.</returns>
        protected virtual async Task ActionFailure(object args)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Methods which gets the query.
        /// </summary>
        /// <param name="query">Specifies the query.</param>
        /// <returns>query.</returns>
        /// <exclude/>
        protected virtual Query GetQuery(Query query)
        {
            return (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
        }

        /// <summary>
        /// Method which sets the fields.
        /// </summary>
        protected void SetFields()
        {
            if (!string.IsNullOrEmpty(Fields?.Value) && string.IsNullOrEmpty(Fields?.Text))
            {
                Fields.Text = Fields.Value;
            }
            else if (string.IsNullOrEmpty(Fields?.Value) && !string.IsNullOrEmpty(Fields?.Text))
            {
                Fields.Value = Fields.Text;
            }
            else if (string.IsNullOrEmpty(Fields?.Value) && string.IsNullOrEmpty(Fields?.Text))
            {
                Fields = new FieldSettingsModel();
                Fields.Value = Fields.Text = "text";
            }
        }

        /// <summary>
        /// Specifies the data execute.
        /// </summary>
        /// <param name="data">Specifies the data.</param>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Simple data.</returns>
        protected IEnumerable<T> SimpleDataExecute(IEnumerable<T> data, Query query)
        {
            var simpleData = data;
            string filterType = FilterType.ToString().ToLower(CultureInfo.CurrentCulture);
            StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;
            string filterValue = string.Empty;

            // perform Filtering
            if (query?.Queries?.Where?.Count > 0)
            {
                var whereQuery = query.Queries.Where[0];
                filterType = whereQuery.Operator;
                filterValue = whereQuery.value?.ToString();
                ignoreCase = whereQuery.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            }

            if (!string.IsNullOrEmpty(filterValue))
            {
                switch (filterType)
                {
                    case "startswith":
                        simpleData = simpleData.Where(i => i.ToString().StartsWith(filterValue, ignoreCase)).ToList<T>();
                        break;
                    case "endswith":
                        simpleData = simpleData.Where(i => i.ToString().EndsWith(filterValue, ignoreCase)).ToList<T>();
                        break;
                    case "contains":
                        simpleData = simpleData.Where(i => i.ToString().Contains(filterValue, ignoreCase)).ToList<T>();
                        break;
                    case "equals":
                        simpleData = simpleData.Where(i => i.ToString().Equals(filterValue, ignoreCase)).ToList<T>();
                        break;
                }
            }

            if (query?.Queries?.Take != null && query?.Queries?.Take > 0)
            {
                simpleData = simpleData.Take((int)query?.Queries.Take);
            }

            return simpleData;
        }

        /// <summary>
        /// Method which clones the query.
        /// </summary>
        /// <param name="query">Specifies the query.</param>
        /// <returns>Query.</returns>
        protected Query CloneQuery(Query query)
        {
            return query != null ? query.Clone() : new Query();
        }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            dataSource = DataSource;
            query = Query;
            sortOrder = SortOrder;
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (!object.ReferenceEquals(DataSource, dataSource))
            {
                UpdateObservableEvents(nameof(dataSource), dataSource, true);
                UpdateObservableEvents(nameof(DataSource), DataSource);
            }

            sortOrder = NotifyPropertyChanges(nameof(SortOrder), SortOrder, sortOrder);
            if ((dataSource == null && DataSource != null) ||
                (dataSource != null && DataSource == null) ||
                (dataSource != null && DataSource != null && !SfBaseUtils.Equals(DataSource, dataSource)) ||
                (dataSource != null && DataSource != null &&
                ((!dataSource.Any() && DataSource.Any()) || (dataSource.Any() && !DataSource.Any())))) //Handles delayed data assigning.
            {
                PropertyChanges.TryAdd(nameof(DataSource), DataSource);
                DataSource = dataSource = DataSource;
            }
            query = NotifyPropertyChanges(nameof(Query), Query, query);
            DataSource = dataSource = await UpdateProperty(nameof(DataSource), DataSource, dataSource);
            SetDataManager<T>(DataSource);
            if (DataSource != null && !IsObservableWired)
            {
                UpdateObservableEvents(nameof(DataSource), DataSource, true);
                UpdateObservableEvents(nameof(DataSource), DataSource);
                IsObservableWired = true;
            }
        }

        internal virtual void UpdateDropDownTemplate(string name, RenderFragment template = null, RenderFragment<T> dataTemplate = null, RenderFragment<ComposedItemModel<T>> groupTemp = null)
        {
        }

        /// <summary>
        /// Method which gets data by text.
        /// </summary>
        /// <param name="ddlText">Specifies the text value.</param>
        /// <param name="field">Specifies the field.</param>
        /// <returns>Type.</returns>
        /// <exclude/>
        protected T GetDataByText(string ddlText, string field = null)
        {
            if (ListData != null)
            {
                if (IsSimpleDataType())
                {
                    return ListData.Where(item => SfBaseUtils.Equals(item, ddlText)).FirstOrDefault();
                }
                else
                {
                    var fields = field != null ? field : (!string.IsNullOrEmpty(Fields?.Text) ? Fields.Text : "text");
                    return ListData.Where(item => EqualityComparer<string>.Default.Equals((string)SfBaseUtils.ChangeType(DataUtil.GetObject(fields, item), typeof(string)), ddlText)).FirstOrDefault();
                }
            }

            return default;
        }

        /// <summary>
        /// Method which specifies the incremental search.
        /// </summary>
        /// <param name="queryString">Specifies the query string.</param>
        /// <param name="items">Specifies the items.</param>
        /// <param name="selectedIndex">Specifies the selectes index.</param>
        /// <param name="ignoreCase">Specifies the ignore case.</param>
        /// <returns>Type.</returns>
        /// <exclude/>
        protected T IncrementalSearch(string queryString, IEnumerable<T> items, int? selectedIndex, bool ignoreCase)
        {
            IncrementSearchValue += ignoreCase ? queryString?.ToLower(CultureInfo.CurrentCulture) : queryString;
            int itemIndex = 0;
#pragma warning disable CA2000 // Dispose objects before losing scope
            var cancellationTokenSource = new CancellationTokenSource();
#pragma warning restore CA2000 // Dispose objects before losing scope
            var cancellationToken = cancellationTokenSource.Token;
            _ = Task.Delay(200).ContinueWith(
                (t) =>
            {
                IncrementSearchValue = string.Empty;
            }, cancellationToken, TaskContinuationOptions.None, TaskScheduler.Current);
            var listItems = items;
            var strLength = IncrementSearchValue.Length;
            selectedIndex = selectedIndex != null ? selectedIndex + 1 : 0;
            int i = (int)selectedIndex;
            MatchItems = new List<T>();
            var listCount = listItems?.Count();
            do
            {
                if (i == listCount)
                {
                    i = -1;
                }

                itemIndex = (i == -1) ? 0 : i;
                var item = listItems.ElementAtOrDefault(itemIndex);
                var matchText = IsSimpleDataType() ? item?.ToString() : DataUtil.GetObject(Fields?.Text, item)?.ToString();
                matchText = ignoreCase ? matchText?.ToLower(CultureInfo.CurrentCulture) : matchText;
                if (!string.IsNullOrEmpty(matchText) && matchText.Length >= strLength && matchText.Substring(0, strLength) == IncrementSearchValue)
                {
                    MatchItems.Add(item);
                }

                i++;
            }
            while (i != selectedIndex);
            PrevString = IncrementSearchValue;
            return MatchItems.FirstOrDefault();
        }

        /// <summary>
        /// Method which specifies the search action.
        /// </summary>
        /// <param name="inputValue">Specifies the input element value.</param>
        /// <param name="items">Specifies the items.</param>
        /// <param name="searchType">Specifies the search type.</param>
        /// <param name="ignoreCase">Specifies whether the case can be ignored or not. </param>
        /// <returns>Type.</returns>
        /// <exclude/>
        protected T Search(string inputValue, IEnumerable<T> items, string searchType, bool ignoreCase)
        {
            var listItems = items;
            T itemData = default;
            if (!string.IsNullOrEmpty(inputValue))
            {
                if (IsSimpleDataType())
                {
                    var query = new Query().Where(new WhereFilter() { Field = string.Empty, Operator = searchType, value = inputValue, IgnoreCase = ignoreCase });
                    itemData = SimpleDataExecute(listItems, query).FirstOrDefault();
                }
                else
                {
                    var strLength = inputValue.Length;
                    var listCount = listItems.Count();
                    var queryStr = ignoreCase ? inputValue.ToLower(CultureInfo.CurrentCulture) : inputValue;
                    for (int index = 0; index < listCount; index++)
                    {
                        var item = DataUtil.GetObject(Fields.Text, listItems.ElementAtOrDefault(index)).ToString();
                        item = ignoreCase ? item.ToLower(CultureInfo.CurrentCulture) : item;
                        var inputText = Regex.Replace(item, "@/^s+|s+$/g", string.Empty);
                        var typedLength = inputText.Length < strLength ? inputText.Length : strLength;
                        if ((searchType == "equals" && inputText == queryStr) || (searchType == "startswith" && !string.IsNullOrEmpty(inputText) && inputText.Substring(0, typedLength) == queryStr))
                        {
                            itemData = listItems.ElementAtOrDefault(index);
                            break;
                        }
                    }
                }
            }

            return itemData;
        }

        /// <summary>
        /// Method which set item value.
        /// </summary>
        /// <param name="itemValue">Specifies the item value.</param>
        /// <param name="valueType"></param>
        /// <returns>Type.</returns>
        protected T SetItemValue(string itemValue, Type valueType = null)
        {
            Type itemType = typeof(T);
            if (IsSimpleDataType())
            {
                return (T)SfBaseUtils.ChangeType(itemValue, itemType);
            }
            else
            {
                var items = (T)Activator.CreateInstance(typeof(T));
                if (items is ExpandoObject)
                {
                    dynamic currentItem = items;
                    ((IDictionary<string, object>)currentItem)[Fields.Value] = itemValue;
                    ((IDictionary<string, object>)currentItem)[Fields.Text] = itemValue;
                    items = (T)currentItem;
                }
                else
                {
                    var getValueProperty = itemType.GetProperty(Fields?.Value);
                    var getTextProperty = itemType.GetProperty(Fields?.Text);
                    if (getValueProperty != null && getValueProperty.CanWrite && getValueProperty.SetMethod != null)
                    {
                        if (valueType != null)
                        {
                            itemType.GetProperty(Fields?.Value)?.SetValue(items, SfBaseUtils.ChangeType(itemValue, valueType));
                        } else
                        {
                            var propertyType = itemType.GetProperty(this.Fields?.Value).PropertyType;
                            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {
                                propertyType = Nullable.GetUnderlyingType(propertyType);
                            }
                            if (propertyType == typeof(string))
                            {
                                itemType.GetProperty(this.Fields?.Value)?.SetValue(items, itemValue);
                            }
                        }
                    }

                    if (getTextProperty != null && getTextProperty.CanWrite && getValueProperty.SetMethod != null)
                    {
                        itemType.GetProperty(Fields?.Text)?.SetValue(items, itemValue);
                    }
                }

                return items;
            }
        }

        /// <summary>
        /// Method speciifes whether the data is simple data type.
        /// </summary>
        /// <returns>Bool.</returns>
        protected bool IsSimpleDataType()
        {
            Type type = typeof(T);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return isNullable || type == typeof(string) || type == typeof(int) || type == typeof(int) || type == typeof(long) || type == typeof(double) || type == typeof(decimal) || type == typeof(bool);
        }

        /// <summary>
        /// Add new items to the popup list. By default, new items append to the list as the last item, but you can insert based on the index parameter.
        /// </summary>
        /// <param name="items">Specifies the items append to the list based on index.</param>
        /// <param name="itemIndex">Specifies the index to place the newly added item in the popup list.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddItems(IEnumerable<T> items, int? itemIndex = null)
        {
            await InsertItem(items, itemIndex);
        }
		/// <summary>
        /// Add new items to the popup list. By default, new items append to the list as the last item, but you can insert based on the index parameter.
        /// </summary>
        /// <param name="items">Specifies the items append to the list based on index.</param>
        /// <param name="itemIndex">Specifies the index to place the newly added item in the popup list.</param>
        public async Task AddItemsAsync(IEnumerable<T> items, int? itemIndex = null)
        {
            await AddItems(items, itemIndex);
        }

        /// <summary>
        /// Method which inserts the item.
        /// </summary>
        /// <param name="items">Specifies the items.</param>
        /// <param name="itemIndex">Specifies the item index.</param>
        /// <param name="preventInit">Specifies whether it  preventInit or not.</param>
        /// <returns>Task.</returns>
        protected async Task InsertItem(IEnumerable<T> items, int? itemIndex = null, bool preventInit = false)
        {
            if (!preventInit && (ListData == null || !ListData.Any()))
            {
                await Render(DataSource, Fields, Query);
            }

            ListData = ListData == null ? new List<T>() : ListData;
            var itemCount = ListData.Count();
            int index = (itemIndex == null || itemIndex < 0 || itemIndex > itemCount - 1) ? itemCount : (int)itemIndex;
            var listItems = new List<T>(ListData);
            listItems.InsertRange(index, items);
            ListData = listItems;
            if (IsFilter() && ComponentName == "SfComboBox")
            {
                var mainItems = new List<T>(MainData);
                index = (itemIndex == null && MainData != null) ? MainData.Count() : index;
                mainItems.InsertRange(index, items);
                MainData = mainItems;
            }

            await RenderItems();
        }

        /// <summary>
        /// Method specifies whether  filter mode or not.
        /// </summary>
        /// <returns>bool.</returns>
        protected virtual bool IsFilter()
        {
            return false;
        }

        /// <summary>
        /// Task which render the component with provided datasource.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected async Task RenderItems()
        {
            if (Fields?.GroupBy != null)
            {
                ListDataSource = await GetGroupedDataSource(ListData, SortOrder);
            }
            else
            {
                ListDataSource = await GetDataSource(ListData, AddSorting(SortOrder, Fields?.Text));
            }
        }

        private async Task<object> DataExecute(IEnumerable<T> dataSource, Query query)
        {
            IEnumerable<object> dataItems = (DataManager != null && GetQuery(query).IsCountRequired) ? (IEnumerable<object>)dataSource
                : (dataSource as IEnumerable)?.Cast<object>()?.ToList();
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            DataManager dm = new DataManager { Json = dataItems };
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
#pragma warning restore CA2000 // Dispose objects before losing scope
            return await dm.ExecuteQuery<T>(query);
        }

        private async Task<IEnumerable<ListOptions<T>>> GetDataSource(IEnumerable<T> dataSource, Query query)
        {
            IEnumerable<T> resultData = IsSimpleDataType() ? dataSource : await DataExecute(dataSource, query) as IEnumerable<T>;
            if (ComponentName == "SfListBox")
            {
                ListData = resultData;
                if (isToolbarAction)
                {
                    ListData = resultData = dataSource;
                }
            }
            var dataItems = new List<ListOptions<T>>();
            if (resultData != null)
            {
                foreach (T listItemData in resultData as IEnumerable<T>)
                {
                    if (listItemData != null)
                    {
                        var childItem = new ListOptions<T>()
                        {
                            Text = IsSimpleDataType() ? listItemData.ToString() : DataUtil.GetObject(Fields?.Text, listItemData)?.ToString(),
                            Value = IsSimpleDataType() ? listItemData.ToString() : DataUtil.GetObject(Fields?.Value, listItemData)?.ToString(),
                            CurItemData = listItemData,
                            ListClass = LIST_ITEM,
                            IconCss = LIST_ICON + SPACE + DataUtil.GetObject(Fields?.IconCss, listItemData)?.ToString(),
                            ShowIcon = Fields?.IconCss != null,
                            ListAttribute = Fields?.HtmlAttributes != null ? DataUtil.GetObject(Fields?.HtmlAttributes, listItemData) as Dictionary<string, object> : null
                        };
                        childItem = ListItemCreated(childItem);
                        dataItems.Add(childItem);
                    }
                }
            }

            return dataItems;
        }

        private async Task<IEnumerable<ListOptions<T>>> GetGroupedDataSource(IEnumerable<T> dataSource, SortOrder sortOrder)
        {
            var curQuery = new Query().Group(Fields.GroupBy);
            curQuery = AddSorting(sortOrder, Fields.Text, curQuery);
            var data = await DataExecute(dataSource, curQuery) as IEnumerable<Group<T>>;
            var dataItems = new List<ListOptions<T>>();
            foreach (Group<T> groupData in data)
            {
                if (groupData != null)
                {
                    var itemObj = groupData.Items;
                    var grpItem = new ListOptions<T>()
                    {
                        Text = groupData.Key.ToString(),
                        IsHeader = true,
                        Items = itemObj,
                        ListClass = GROUP_ITEM,
                        GroupItems = new ComposedItemModel<T>() { IsGroupItem = true, Items = (List<T>)itemObj, Key = groupData.Key.ToString() }
                    };
                    dataItems.Add(grpItem);
                    foreach (T itemdata in itemObj)
                    {
                        if (itemdata != null)
                        {
                            var childItem = new ListOptions<T>()
                            {
                                Text = DataUtil.GetObject(Fields?.Text, itemdata).ToString(),
                                Value = DataUtil.GetObject(Fields?.Value, itemdata).ToString(),
                                CurItemData = itemdata,
                                ListClass = LIST_ITEM,
                                IconCss = LIST_ICON + SPACE + DataUtil.GetObject(Fields?.IconCss, itemdata).ToString(),
                                ShowIcon = Fields?.IconCss != null,
                                ListAttribute = Fields?.HtmlAttributes != null ? DataUtil.GetObject(Fields?.HtmlAttributes, itemdata) as Dictionary<string, object> : null
                            };
                            childItem = ListItemCreated(childItem);
                            dataItems.Add(childItem);
                        }
                    }
                }
            }

            return dataItems;
        }

        private Query AddSorting(SortOrder sortOrder, string sortBy, Query query = null)
        {
            var sortQuery = query != null ? query : new Query();
            if (IsSimpleDataType())
            {
                ListData = (sortOrder == SortOrder.None) ? ListData : (sortOrder == SortOrder.Ascending) ? from sortData in ListData orderby sortData select sortData : from sortData in ListData orderby sortData descending select sortData;
            }
            else
            {
                sortQuery = sortOrder != SortOrder.None ? sortQuery.Sort(sortBy, sortOrder.ToString()) : sortQuery;
            }

            return sortQuery;
        }

        /// <summary>
        /// Method specifies the higlight search action.
        /// </summary>
        /// <param name="liConent">Specifies the list content.</param>
        /// <param name="searchText">Specifies the search text.</param>
        /// <param name="ignoreCase">Specifies whether cases can be ignored or not.</param>
        /// <param name="highlightType">Specifies the highlight type.</param>
        /// <returns>string.</returns>
        /// <exclude/>
        protected string HighlightSearch(string liConent, string searchText, bool ignoreCase, FilterType highlightType = DropDowns.FilterType.Contains)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                searchText = new Regex("@/^[a-zA-Z0-9- ]*$/").IsMatch(searchText) ? searchText : Regex.Replace(searchText, "@/[\\-\\[\\]\\/\\{\\}\\(\\)\\*\\+\\?\\.\\\\^\\$\\|]/g", "@\\$&");
                var replaceQuery = highlightType == FilterType.StartsWith ? @"^(" + searchText + ")" : highlightType == FilterType.EndsWith ? @"(" + searchText + ")$" : @"(" + searchText + ")";
                var ignoreText = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
                var hightLightText = Regex.Replace(liConent, replaceQuery, @"<span class='e-highlight'>$1</span>", ignoreText);
                return hightLightText;
            }

            return liConent;
        }

        /// <summary>
        /// Triggers while the list item get created.
        /// </summary>
        /// <param name="listItem">Specifies the list item.</param>
        /// <returns>ListOptions.</returns>
        protected virtual ListOptions<T> ListItemCreated(ListOptions<T> listItem)
        {
            return listItem;
        }

        internal void ObservableEventDisposed()
        {
            UpdateObservableEvents(nameof(DataSource), DataSource, true);
        }
    }

    /// <summary>
    /// Specifies the list options.
    /// </summary>
    /// <typeparam name="T">Specifies the type of ListOptions.</typeparam>
    public class ListOptions<T>
    {
        /// <summary>
        /// Specifies the Text property.
        /// </summary>
        /// <exclude/>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the Value property.
        /// </summary>
        /// <exclude/>
        public string Value { get; set; }

        /// <summary>
        /// Specifies the GroupBy  property.
        /// </summary>
        /// <exclude/>
        public string GroupBy { get; set; }

        /// <summary>
        /// Specifies the IsHeader  property.
        /// </summary>
        /// <exclude/>
        public bool IsHeader { get; set; }

        /// <summary>
        /// Specifies the ShowIcon  property.
        /// </summary>
        /// <exclude/>
        public bool ShowIcon { get; set; }

        /// <summary>
        /// Specifies the Items  property.
        /// </summary>
        /// <exclude/>
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Specifies the  CurItemData property.
        /// </summary>
        /// <exclude/>
        public T CurItemData { get; set; }

        /// <summary>
        /// Specifies the ListClass  property.
        /// </summary>
        /// <exclude/>
        public string ListClass { get; set; }

        /// <summary>
        /// Specifies the IconCss  property.
        /// </summary>
        /// <exclude/>
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies the GroupItems property.
        /// </summary>
        /// <exclude/>
        public ComposedItemModel<T> GroupItems { get; set; }

        /// <summary>
        /// Specifies the ListAttribute property.
        /// </summary>
        /// <exclude/>
        public Dictionary<string, object> ListAttribute { get; set; }
    }

    /// <summary>
    /// Specifies the key action.
    /// </summary>
    public class KeyActions
    {
        /// <summary>
        /// Specifies the  action property.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Specifies the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Specifies the events.
        /// </summary>
        public EventArgs Events { get; set; }

        /// <summary>
        /// Specifies the type.
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// Specifies the data items.
    /// </summary>
    /// <typeparam name="TVal">Specifies the type of DataItems.</typeparam>
    public class DataItems<TVal>
    {
        /// <summary>
        /// Specifies the text.
        /// </summary>
        public object Text { get; set; }

        /// <summary>
        /// Specifies the value.
        /// </summary>
        public TVal Value { get; set; }
    }

    /// <summary>
    /// common class model for grouped list.
    /// </summary>
    /// <typeparam name="T">Specifies the type of ComposedItemModel.</typeparam>
    /// <exclude/>
    public class ComposedItemModel<T>
    {
        /// <summary>
        /// Specifies whether it is a group item or not.
        /// </summary>
        public bool IsGroupItem { get; set; }

        /// <summary>
        /// Returns the grouped data items.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Returns the grouped key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Returns the list item is group.
        /// </summary>
        public bool IsHeader => IsGroupItem;

        /// <summary>
        /// Returns the id of the group list item.
        /// </summary>
        public string Id => $"group-list-item-{Key}";

        /// <summary>
        /// Returns the text of the grouping field.
        /// </summary>
        public string Text => Key;

        /// <summary>
        /// Returns the grouped items.
        /// </summary>
        public List<T> Items { get; set; }
    }

    internal class DropDownClientProperties
    {
        /// <summary>
        /// Specifies whether the component is in rtl mode or not.
        /// </summary>
        /// <exclude/>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the z-index.
        /// </summary>
        public double ZIndex { get; set; }

        /// <summary>
        /// Specifies the popup width.
        /// </summary>
        public string PopupWidth { get; set; }

        /// <summary>
        /// Specifies the popup height.
        /// </summary>
        public string PopupHeight { get; set; }

        /// <summary>
        /// Specifies the width of the component.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Specifies whether filtering is allowed or not.
        /// </summary>
        public bool AllowFiltering { get; set; }

        /// <summary>
        /// Specifies the module name.
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Specifies whether the virtualization is enabled or not.
        /// </summary>
        public bool EnableVirtualization { get; set; }

        /// <summary>
        /// Specifies the delimeter character.
        /// </summary>
        public string DelimiterChar { get; set; }

        /// <summary>
        /// Specifies the over flow content.
        /// </summary>
        public string OverFlowContent { get; set; }

        /// <summary>
        /// Specifies the total count of the content.
        /// </summary>
        public string TotalCountContent { get; set; }

        /// <summary>
        /// Specifies the delimeter value.
        /// </summary>
        public List<string> DelimValue { get; set; }

        /// <summary>
        /// Specifies the mode.
        /// </summary>
        public string Mode { get; set; }
    }

    /// <summary>
    /// Specifies the selected data.
    /// </summary>
    /// <typeparam name="TItem">Specifies the type of SelectedData.</typeparam>
    public class SelectedData<TItem>
    {
        /// <summary>
        /// Specifies the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Specifies the item data.
        /// </summary>
        public TItem ItemData { get; set; }

        /// <summary>
        /// Specifies the chip class.
        /// </summary>
        public string ChipClass { get; set; } = "e-chips";
    }
}
