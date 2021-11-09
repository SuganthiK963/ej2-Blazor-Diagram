using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// ListBox component used to display a list of items. Users can select one or more items in the list using a checkbox or by keyboard selection.
    /// It supports sorting, grouping, reordering and drag and drop of items.
    /// </summary>
    public partial class SfListBox<TValue, TItem> : SfDropDownBase<TItem>
    {
        /// <summary>
        /// This method is used to enable or disable the items in the ListBox based on the items and enable argument.
        /// </summary>
        /// <param name = "items">Specifies the list items to be enabled or disabled. You can pass either value(TValue) or data source(TItem) collection.</param>
        /// <param name = "enable">Set false to disable the items. By default the items will be enabled.</param>
        public void EnableItems<T>(T items, bool enable = true)
        {
            Type itemsType = typeof(T);
            var isValueType = itemsType == typeof(TValue);
            bool refresh = false;
            if (isValueType || itemsType == typeof(IEnumerable<TItem>) || itemsType == typeof(List<TItem>))
            {
                IEnumerable<TItem> data = null;
                if (isValueType)
                {
                    data = GetDataByValue((TValue)(object)items);
                }
                else
                {
                    data = (IEnumerable<TItem>)items;
                }

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        var listItem = ListDataSource?.Where(listItem => SfBaseUtils.Equals(listItem.CurItemData, item)).FirstOrDefault();
                        if (listItem != null)
                        {
                            refresh = true;
                            if (enable)
                            {
                                listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, DISABLED);
                            }
                            else
                            {
                                listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, DISABLED);
                            }
                        }
                    }
                }
            }

            if (refresh)
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// Gets the array of data Object that matches the given array of values.
        /// </summary>
        /// <param name = "dataValue">Specifies the value(s).</param>
        public List<TItem> GetDataByValue(TValue dataValue)
        {
            var data = new List<TItem>();
            if (dataValue != null)
            {
                Type valueType = typeof(TValue);
                if (valueType.IsArray || valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (valueType == typeof(int[]) || valueType == typeof(List<int>))
                    {
                        List<int> values = valueType.IsArray ? (dataValue as int[]).ToList() : dataValue as List<int>;
                        foreach (var val in values)
                        {
                            data.Add(GetData(val.ToString(CultureInfo.InvariantCulture)));
                        }
                    }
                    else if (valueType == typeof(string[]) || valueType == typeof(List<string>))
                    {
                        List<string> values = valueType.IsArray ? (dataValue as string[]).ToList() : dataValue as List<string>;
                        foreach (var val in values)
                        {
                            data.Add(GetData(val));
                        }
                    }
                }
                else if (IsSimpleType())
                {
                    data.Add(GetData(dataValue));
                }
            }

            return data;
        }

        /// <summary>
        /// Returns the updated dataSource in ListBox.
        /// </summary>
        public IEnumerable<TItem> GetDataList()
        {
            return ListData;
        }

        /// <summary>
        /// Moves all the values from one ListBox to the scoped ListBox.
        /// </summary>
        /// <param name = "scope">Specifies the destination ListBox reference.</param>
        /// <param name = "index">Specifies the index to place the moved items.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task MoveAllTo(string scope = null, double? index = null)
        {
            await MoveData(null, index, await GetScopedListbox(scope), default, true, false);
        }

        /// <summary>
        /// Moves all the values from one ListBox to the scoped ListBox.
        /// </summary>
        /// <param name = "scope">Specifies the destination ListBox reference.</param>
        /// <param name = "index">Specifies the index to place the moved items.</param>
        public async Task MoveAllAsync(string scope = null, double? index = null)
        {
            await MoveAllTo(scope, index);
        }

        /// <summary>
        /// Moves the given value(s) / selected value(s) downwards.
        /// </summary>
        /// <param name = "values">Specifies the value(s).</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task MoveDown(TValue values = default)
        {
            await MoveUpDown(MOVEDOWN, values, false);
        }

        /// <summary>
        /// Moves the given value(s) / selected value(s) downwards.
        /// </summary>
        /// <param name = "values">Specifies the value(s).</param>
        public async Task MoveDownAsync(TValue values = default)
        {
            await MoveDown(values);
        }

        /// <summary>
        /// Moves the given value(s) / selected value(s) to the given / default scoped ListBox.
        /// </summary>
        /// <param name = "values">Specifies the value(s).</param>
        /// <param name = "index">Specifies the index to place the moved items.</param>
        /// <param name = "scope">Specifies the destination ListBox reference.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task MoveTo(TValue values = default, double? index = null, string scope = null)
        {
            await MoveData(null, index, await GetScopedListbox(scope), values, false, false);
        }

        /// <summary>
        /// Moves the given value(s) / selected value(s) to the given / default scoped ListBox.
        /// </summary>
        /// <param name = "values">Specifies the value(s).</param>
        /// <param name = "index">Specifies the index to place the moved items.</param>
        /// <param name = "scope">Specifies the destination ListBox reference.</param>
        public async Task MoveAsync(TValue values = default, double? index = null, string scope = null)
        {
            await MoveTo(values, index, scope);
        }

        /// <summary>
        /// Moves the given value(s) / selected value(s) upwards.
        /// </summary>
        /// <param name = "values">Specifies the value(s).</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task MoveUp(TValue values = default)
        {
            await MoveUpDown(MOVEUP, values, false);
        }

        /// <summary>
        /// Moves the given value(s) / selected value(s) upwards.
        /// </summary>
        /// <param name = "values">Specifies the value(s).</param>
        public async Task MoveUpAsync(TValue values = default)
        {
            await MoveUp(values);
        }

        /// <summary>
        /// Based on the state parameter, entire list item will be selected/deselected.
        /// </summary>
        /// <param name = "state">Set `true`/`false` to select/ unselect the entire list items.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task SelectAll(bool state = true)
        {
            await SelectAllHandler(state);
        }

        /// <summary>
        /// Based on the state parameter, entire list item will be selected/deselected.
        /// </summary>
        /// <param name = "state">Set `true`/`false` to select/ unselect the entire list items.</param>
        public async Task SelectAllAsync(bool state = true)
        {
            await SelectAll(state);
        }

        /// <summary>
        /// Based on the state parameter, specified list item will be selected/deselected.
        /// </summary>
        /// <param name = "items">Specifies the list items to be selected or unselected. You can pass either value(TValue) or data source(TItem) collection.</param>
        /// <param name = "state">Set false to un select the items. By default the items will be selected.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task SelectItems<T>(T items, bool state = true)
        {
            Type itemsType = typeof(T);
            var isValueType = itemsType == typeof(TValue);
            bool refresh = false;
            if (isValueType || itemsType == typeof(TItem) || itemsType == typeof(IEnumerable<TItem>) || itemsType == typeof(List<TItem>))
            {
                IEnumerable<TItem> data = null;
                if (isValueType)
                {
                    data = GetDataByValue((TValue)(object)items);
                }
                else if (itemsType == typeof(TItem))
                {
                    data = new List<TItem>() { (TItem)(object)items };
                }
                else
                {
                    data = (IEnumerable<TItem>)items;
                }

                if (data != null)
                {
                    if (state)
                    {
                        refresh = true;
                        if (selectionSettings.Mode == SelectionMode.Single)
                        {
                            RemoveSelection();
                        }

                        await UpdateSelectedValue(data, selectedValues.Count != 0, true);
                    }
                    else if (selectedValues.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            var listItem = ListDataSource?.Where(listItem => SfBaseUtils.Equals(listItem.CurItemData, item)).FirstOrDefault();
                            if (listItem != null)
                            {
                                refresh = true;
                                if (selectedValues.Contains(listItem.Value))
                                {
                                    await UnSelectValue(listItem);
                                }
                            }
                        }
                    }
                }

                if (refresh)
                {
                    StateHasChanged();
                    scopeListbox?.StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Based on the state parameter, specified list item will be selected/deselected.
        /// </summary>
        /// <param name = "items">Specifies the list items to be selected or unselected. You can pass either value(TValue) or data source(TItem) collection.</param>
        /// <param name = "state">Set false to un select the items. By default the items will be selected.</param>
        public async Task SelectItemsAsync<T>(T items, bool state = true)
        {
            await SelectItems(items, state);
        }

        /// <summary>
        /// Removes a item or collection of items from the list. By default, removed the last item in the list,
        /// but you can remove based on the index parameter.
        /// </summary>
        /// <param name = "items">Specifies the list of data collection to be removed.</param>
        /// <param name = "itemIndex">Specifies the index to remove the item from the list.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RemoveItem(IEnumerable<TItem> items = default, int? itemIndex = null)
        {
            if (ListData != null && ListData.Any())
            {
                var count = ListData.Count();
                if (items == null)
                {
                    items = new List<TItem> { ListData.ElementAt(itemIndex != null && itemIndex < count ? (int)itemIndex : count - 1) };
                }

                foreach (var item in items)
                {
                    string text = IsSimpleDataType() ? item.ToString() : DataUtil.GetObject(Fields?.Text, item)?.ToString();
                    if (text != null)
                    {
                        var listItem = GetDataByText(text);
                        if (listItem != null)
                        {
                            text = IsSimpleDataType() ? item.ToString() : DataUtil.GetObject(Fields?.Value, item)?.ToString();
                            if (text != null && selectedValues.Contains(text))
                            {
                                await SelectItemsAsync(listItem, false);
                            }
                            ListData = ListData.Where(list => !SfBaseUtils.Equals(list, listItem));
                        }
                    }
                }

                await RenderItems();
                await UpdateSelectedValue(GetDataByValue(Value));
                StateHasChanged();
            }
        }

        /// <summary>
        /// Removes a item or collection of items from the list. By default, removed the last item in the list,
        /// but you can remove based on the index parameter.
        /// </summary>
        /// <param name = "items">Specifies the list of data collection to be removed.</param>
        /// <param name = "itemIndex">Specifies the index to remove the item from the list.</param>
        public async Task RemoveItemAsync(IEnumerable<TItem> items = default, int? itemIndex = null)
        {
            await RemoveItem(items, itemIndex);
        }
    }
}