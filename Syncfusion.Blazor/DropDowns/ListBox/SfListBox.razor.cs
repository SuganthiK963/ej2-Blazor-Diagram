using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns.Internal;
using System.Threading.Tasks;
using Syncfusion.Blazor.Data;
using System.Linq;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Inputs.Internal;
using System.Globalization;
using System.Text.Json;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// ListBox component used to display a list of items. Users can select one or more items in the list using a checkbox or by keyboard selection.
    /// It supports sorting, grouping, reordering and drag and drop of items.
    /// </summary>
    public partial class SfListBox<TValue, TItem> : SfDropDownBase<TItem>, IListBox
    {
        protected override string ComponentName { get; set; } = "SfListBox";
        private Dictionary<string, object> htmlAttributes;
        private string id = SfBaseUtils.GenerateID(LISTBOX);
        private List<string> selectedValues = new List<string>();
        private int? previousIndex;
        private ListBoxSelectionSettings selectionSettings;
        private ListBoxToolbarSettings toolbarSettings;
        private TValue listboxValue;
        private string cssClass;
        private bool enableRtl;
        private bool allowDragAndDrop;
        private bool enabled;
        private string container;
        private ElementReference element;
        private SfInputBase filterInputObj;
        private string filterValue;
        private bool render;
        private string toolbarCls = " e-listboxtool-wrapper";
        private IEnumerable<TItem> tempData;
        private IEnumerable<TItem> filteredData;
        private SfListBox<TValue, TItem> scopeListbox;
        internal ListBoxEvents<TValue, TItem> Delegates;

        private void Initialize()
        {
            var containerCls = CONTAINER;
            if (!string.IsNullOrEmpty(CssClass))
            {
                containerCls += SPACE + CssClass;
            }

            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                containerCls += RTL;
            }

            if (!Enabled)
            {
                containerCls += DISABLE;
            }

            if (toolbarSettings != null)
            {
                containerCls += TOOLBAR;
                if (AllowFiltering)
                {
                    containerCls = SfBaseUtils.RemoveClass(containerCls, TOOLBAR);
                    containerCls = SfBaseUtils.RemoveClass(containerCls, CONTAINER);
                    containerCls = SfBaseUtils.AddClass(containerCls, toolbarCls);
                }
                if (toolbarSettings.Position == ToolBarPosition.Right)
                {
                    containerCls += RIGHT;
                }
                else
                {
                    containerCls += LEFT;
                }
            }
            else
            {
                if (AllowFiltering)
                {
                    containerCls += " e-filter-list";
                }
            }

            if (selectionSettings != null && selectionSettings.ShowCheckbox && selectionSettings.CheckboxPosition == CheckBoxPosition.Right)
            {
                containerCls = SfBaseUtils.AddClass(containerCls, RIGHT);
            }

            container = containerCls;
        }

        private ToolbarProps CheckBtnDisabled(string item)
        {
            var prop = new ToolbarProps();
            var localeKey = string.Empty;
            var localeValue = string.Empty;
            switch (item)
            {
                case MOVEUP:
                    prop.Disabled = ListDataSource == null || !ListDataSource.Any() || !selectedValues.Any() || selectedValues.Contains(ListDataSource.ElementAt(0).Value);
                    localeKey = MOVEUPKEY; localeValue = MOVEUPVALUE;
                    break;
                case MOVEDOWN:
                    prop.Disabled = ListDataSource == null || !ListDataSource.Any() || !selectedValues.Any() || selectedValues.Contains(ListDataSource.ElementAt(ListDataSource.Count() - 1).Value);
                    localeKey = MOVEDOWNKEY; localeValue = MOVEDOWNVALUE;
                    break;
                case MOVETO:
                    prop.Disabled = Scope == null || ListDataSource == null || !ListDataSource.Any() || !selectedValues.Any();
                    localeKey = MOVETOKEY; localeValue = MOVETOVALUE;
                    break;
                case MOVEFROM:
                    prop.Disabled = scopeListbox != null ? scopeListbox.ListDataSource == null || !scopeListbox.ListDataSource.Any() || !scopeListbox.selectedValues.Any() : true;
                    localeKey = MOVEFROMKEY; localeValue = MOVEFROMVALUE;
                    break;
                case MOVEALLTO:
                    prop.Disabled = Scope == null || ListDataSource == null || !ListDataSource.Any();
                    localeKey = MOVEALLTOKEY; localeValue = MOVEALLTOVALUE;
                    break;
                case MOVEALLFROM:
                    prop.Disabled = scopeListbox != null ? scopeListbox.ListDataSource == null || !scopeListbox.ListDataSource.Any() : true;
                    localeKey = MOVEALLFROMKEY; localeValue = MOVEALLFROMVALUE;
                    break;
                default:
                    prop.Disabled = false; prop.ItemText = item;
                    break;
            }

            if (!string.IsNullOrEmpty(localeKey))
            {
                var localeText = Localizer.GetText(localeKey);
                prop.ItemText = string.IsNullOrEmpty(localeText) ? localeValue : localeText;
            }

            return prop;
        }

        private void UpdateSize()
        {
            if (htmlAttributes == null)
            {
                htmlAttributes = new Dictionary<string, object>();
            }

            if (!htmlAttributes.ContainsKey(STYLE) || htmlAttributes[STYLE] == null || !htmlAttributes[STYLE].ToString().Contains(HEIGHT, StringComparison.Ordinal))
            {
                var styleValue = string.Empty;
                if (AllowFiltering)
                {
                    styleValue += SPACE + MINHEIGHT;
                }

                if (!string.IsNullOrEmpty(Height))
                {
                    styleValue += SPACE + HEIGHT + SPACE + Height;
                }

                if (!string.IsNullOrEmpty(styleValue))
                {
                    if (htmlAttributes.ContainsKey(STYLE))
                    {
                        htmlAttributes[STYLE] += styleValue;
                    }
                    else
                    {
                        htmlAttributes.Add(STYLE, styleValue);
                    }
                }
            }
        }

        private Dictionary<string, object> GetAttributes(string type)
        {
            var attr = new Dictionary<string, object>();
            switch(type)
            {
                case "ul":
                    {
                        if (htmlAttributes.ContainsKey("ul"))
                        {
                            attr = (Dictionary<string, object>)htmlAttributes["ul"];
                        }
                        break;
                    }
                case "li":
                    {
                        if (htmlAttributes.ContainsKey("li"))
                        {
                            attr = (Dictionary<string, object>)htmlAttributes["li"];
                        }
                        break;
                    }
                default:
                    {
                        attr = htmlAttributes.ToDictionary(entry => entry.Key, entry => entry.Value);
                        attr.Remove("ul");
                        attr.Remove("li");
                        break;
                    }
            }
            return attr;
        }

        private Dictionary<string, object> GetListAttributes(ListOptions<TItem> item)
        {
            var attr = new Dictionary<string, object>();
            if (item.ListAttribute != null)
            {
                attr = item.ListAttribute;
            }
            else
            {
                attr = this.GetAttributes("li");
            }
            SfBaseUtils.UpdateDictionary(ROLE, item.IsHeader ? PRESENTATION : OPTION, attr);
            SfBaseUtils.UpdateDictionary(DATAVALUE, item.Value, attr);
            if (!item.IsHeader)
            {
                SfBaseUtils.UpdateDictionary(TABINDEX, -1, attr);
            }

            SfBaseUtils.UpdateDictionary(ARIASELECTED, selectedValues.Contains(item.Value) ? TRUE : FALSE, attr);
            return attr;
        }

        private async Task MouseDownHandler(ListOptions<TItem> item)
        {
            var index = ListDataSource?.IndexOf(item);
            if (index == null || index < 0)
            {
                return;
            }

            var items = GetDataByValue(Value);
            if (!items.Contains(item.CurItemData))
            {
                items.Insert(0, item.CurItemData);
            }

            var eventArgs = new DragEventArgs<TItem> { CurrentIndex = (double)index, Items = items, Source = new SourceDestinationModel<TItem> { CurrentData = ListData } };
            await SfBaseUtils.InvokeEvent<DragEventArgs<TItem>>(Delegates?.DragStart, eventArgs);
            if (eventArgs.Cancel)
            {
                item.ListClass = SfBaseUtils.AddClass(item.ListClass, PREVENTDRAG);
            }
            else if (item.ListClass.Contains(PREVENTDRAG, StringComparison.Ordinal))
            {
                item.ListClass = SfBaseUtils.RemoveClass(item.ListClass, PREVENTDRAG);
            }
        }

        private async Task ClickHandler(ListOptions<TItem> item, bool ShiftKey, bool CtrlKey)
        {
            if (!ShiftKey && !CtrlKey)
            {
                for (var i = 0; i <= tempData.Count() - 1; i++)
                {
                    var listItem = ListDataSource.ElementAt(i);
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, SELECTED);
                }
            }
            if (item.IsHeader)
            {
                return;
            }

            if (!selectionSettings.ShowCheckbox && selectionSettings.Mode == SelectionMode.Multiple && ShiftKey && previousIndex != null && previousIndex < ListDataSource.Count())
            {
                int curIndex = ListDataSource.IndexOf(item);
                if (curIndex > -1)
                {
                    if (selectedValues.Contains(ListDataSource.ElementAt((int)previousIndex).Value))
                    {
                        var selectedData = new List<TItem>();
                        int startIndex = Math.Min(curIndex, (int)previousIndex);
                        int endIndex = Math.Max(curIndex, (int)previousIndex);
                        while (startIndex <= endIndex)
                        {
                            if (startIndex < ListDataSource.Count())
                            {
                                selectedData.Add(ListDataSource.ElementAt(startIndex).CurItemData);
                            }

                            startIndex++;
                        }

                        RemoveSelection(selectedData);
                        await UpdateSelectedValue(selectedData, true, true);
                    }
                }
            }
            else
            {
                if (!selectedValues.Contains(item.Value) || (selectedValues.Count > 1 && !CtrlKey && !selectionSettings.ShowCheckbox))
                {
                    if (selectionSettings.ShowCheckbox && selectedValues.Count == MaximumSelectionLength)
                    {
                        return;
                    }

                    if (selectionSettings.Mode == SelectionMode.Single || (!CtrlKey && !selectionSettings.ShowCheckbox))
                    {
                        RemoveSelection();
                    }

                    await UpdateSelectedValue(new List<TItem>() { item.CurItemData }, false, true, (selectionSettings.Mode == SelectionMode.Multiple && CtrlKey) || selectionSettings.ShowCheckbox);
                }
                else if (CtrlKey || selectionSettings.ShowCheckbox)
                {
                    await UnSelectValue(item);
                }
            }
        }

        private async Task UnSelectValue(ListOptions<TItem> item)
        {
            RemoveSelectedValue(item);
            await UpdateValue(new List<string>(), true, item.Value);
            if (previousIndex != null && selectedValues.Count == 0)
            {
                previousIndex = null;
            }
        }

        private async Task KeyActionHandler(KeyboardEventArgs e, ListOptions<TItem> item, bool isContainer)
        {
            if (e.Code == ARROWUP || e.Code == ARROWDOWN)
            {
                var selecteditem = ListDataSource.Where(listItem => selectedValues.Contains(listItem.Value)).FirstOrDefault();
                var index = ListDataSource.IndexOf(selecteditem);
                if (index == -1)
                {
                    index = 0;
                    isContainer = true;
                }
                if (!isContainer || selectedValues.Count != 0)
                {
                    if (selectionSettings.ShowCheckbox ||e.CtrlKey)
                    {
                        index = ListDataSource.IndexOf(item);
                    }
                    if (index == -1)
                    {
                        return;
                    }
                    index = e.Code == ARROWUP ? index - 1 : index + 1;
                    if (index < 0 || index > ListDataSource.Count() - 1)
                    {
                        return;
                    }
                }

                var listItem = ListDataSource.ElementAt(index);
                if (selectionSettings.ShowCheckbox || e.CtrlKey)
                {
                    if (!listItem.ListClass.Contains(SELECTED, StringComparison.Ordinal))
                    {
                        listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, SELECTED);
                    }
                    else
                    {
                        index = ListDataSource.IndexOf(item);
                        listItem = ListDataSource.ElementAt(index);
                        listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, SELECTED);
                    }
                }
                else
                {
                    await ClickHandler(listItem, e.ShiftKey, e.CtrlKey);
                }
            }
            else if (e.CtrlKey && e.Code == KEYA)
            {
                await SelectAll();
            }
            else if (e.Code == SPACEKEY && !isContainer)
            {
                await ClickHandler(item, e.ShiftKey, e.CtrlKey);
            }
            else if (e.CtrlKey && e.ShiftKey && e.Code == HOME)
            {
                var selecteditem = ListDataSource.Where(listItem => selectedValues.Contains(listItem.Value)).FirstOrDefault();
                var index = ListDataSource.IndexOf(selecteditem);
                for (var i = 0; i <= index; i++)
                {
                    var listItem = ListDataSource.ElementAt(i);
                    listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, SELECTED);
                }
            }
            else if (e.CtrlKey && e.ShiftKey && e.Code == END)
            {
                var selecteditem = ListDataSource.Where(listItem => selectedValues.Contains(listItem.Value)).FirstOrDefault();
                var index = ListDataSource.IndexOf(selecteditem);
                for (var i = index; i <= ListDataSource.Count()-1; i++)
                {
                    var listItem = ListDataSource.ElementAt(i);
                    listItem.ListClass = SfBaseUtils.AddClass(listItem.ListClass, SELECTED);
                }
            }
            else if (e.Code == HOME || e.Code == END)
            {
                if (selectedValues.Count != 0)
                {
                    RemoveSelection();
                }
                var index = e.Code == HOME ? 0 : ListDataSource.Count() - 1;
                var listItem = ListDataSource.ElementAt(index);
                UpdateFocus(listItem);
            }
        }

        private void UpdateFocus(ListOptions<TItem> item, bool remove = false)
        {
            var focusedData = ListDataSource.Where(listItem => listItem.ListClass.Contains(FOCUSED, StringComparison.Ordinal));
            if (focusedData != null)
            {
                foreach (var listItem in focusedData)
                {
                    listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, FOCUSED);
                }
            }

            if (remove)
            {
                return;
            }

            item.ListClass = SfBaseUtils.AddClass(item.ListClass, FOCUSED);
        }

        private void RemoveSelection(List<TItem> list = default)
        {
            UpdateFocus(default, true);
            var selectedData = ListDataSource.Where(listItem => selectedValues.Contains(listItem.Value));
            if (selectedData == null)
            {
                return;
            }

            foreach (var listItem in selectedData)
            {
                if (list == null || !list.Contains(listItem.CurItemData))
                {
                    RemoveSelectedValue(listItem);
                }
            }
        }

        private void RemoveSelectedValue(ListOptions<TItem> listItem)
        {
            listItem.ListClass = SfBaseUtils.RemoveClass(listItem.ListClass, SELECTED);
            if (!string.IsNullOrEmpty(listItem.Value) && selectedValues.Contains(listItem.Value))
            {
                selectedValues.Remove(listItem.Value);
            }
        }

        private async Task SelectAllHandler(bool state, bool isAction = false)
        {
            if (ListDataSource == null || !ListDataSource.Any())
            {
                return;
            }

            if (isAction && selectedValues.Count == ListDataSource.Count())
            {
                state = false;
            }

            if (state)
            {
                await UpdateSelectedValue(ListData, false, true);
            }
            else
            {
                RemoveSelection();
                previousIndex = null;
                await UpdateSelectedValue(new List<TItem>(), false, true);
            }
        }

        private async Task UpdateSelectedValue(IEnumerable<TItem> selectedData, bool isShiftKey = false, bool isAction = false, bool isCtrlKey = false)
        {
            ListOptions<TItem> curItem;
            List<string> values = new List<string> { };
            if (!isCtrlKey)
            {
                RemoveSelection();
            }
            var len = selectedData.Count();
            for (var i = 0; i < len; i++)
            {
                if (selectionSettings.ShowCheckbox && selectedValues.Count == MaximumSelectionLength)
                {
                    break;
                }

                curItem = ListDataSource?.Where(listItem => !listItem.IsHeader && SfBaseUtils.Equals(listItem.CurItemData, selectedData.ElementAt(i))).FirstOrDefault();
                if (curItem != null)
                {
                    if (!isShiftKey && i == len - 1)
                    {
                        previousIndex = ListDataSource?.IndexOf(curItem);
                    }

                    if (selectionSettings == null || !selectionSettings.ShowCheckbox)
                    {
                        curItem.ListClass = SfBaseUtils.AddClass(curItem.ListClass, SELECTED);
                    }

                    if (!string.IsNullOrEmpty(curItem.Value))
                    {
                        values.Add(curItem.Value);
                        if (!selectedValues.Contains(curItem.Value))
                        {
                            selectedValues.Add(curItem.Value);
                        }
                    }
                }
            }

            if (isAction)
            {
                await UpdateValue(values, isCtrlKey);
            }
        }

        private async Task ChangeHandler()
        {
            if (filterInputObj != null)
            {
                await filterInputObj.SetValue(filterInputObj.InputTextValue, Inputs.FloatLabelType.Never, false);
            }
        }

        private async Task UpdateValue(List<string> values, bool isCtrlKey, string removeValue = null)
        {
            TValue curValue = GetValue(values, isCtrlKey, removeValue);
            listboxValue = Value = await SfBaseUtils.UpdateProperty(curValue, listboxValue, ValueChanged, DropDownsEditContext, ValueExpression);
            scopeListbox?.StateHasChanged();
            if (EnablePersistence)
            {
                await SetLocalStorage(id, values);
            }
            await SfBaseUtils.InvokeEvent<ListBoxChangeEventArgs<TValue, TItem>>(
                Delegates?.ValueChange,
                new ListBoxChangeEventArgs<TValue, TItem>() { Value = Value, Name = VALUECHANGE, Items = ListData });
        }

        private TValue GetValue(List<string> values, bool isCtrlKey, string removeValue)
        {
            Type valueType = typeof(TValue);
            if (valueType.IsArray || valueType.IsGenericType)
            {
                var isIntType = valueType == typeof(int[]) || valueType == typeof(int?[]) || valueType == typeof(List<int>) || valueType == typeof(List<int?>);
                var isLongType = valueType == typeof(long[]) || valueType == typeof(long?[]) || valueType == typeof(List<long>) || valueType == typeof(List<long?>);
                var isDecimalType = valueType == typeof(decimal[]) || valueType == typeof(decimal?[]) || valueType == typeof(List<decimal>) || valueType == typeof(List<decimal?>);
                var isDoubleType = valueType == typeof(double[]) || valueType == typeof(double?[]) || valueType == typeof(List<double>) || valueType == typeof(List<double?>);
                var isGuidType = valueType == typeof(Guid[]) || valueType == typeof(Guid?[]) || valueType == typeof(List<Guid>) || valueType == typeof(List<Guid?>);
                var isEnumType = valueType == typeof(Enum[]) || valueType == typeof(Enum[]) || valueType == typeof(List<Enum>) || valueType == typeof(List<Enum>);
                var isObjectType = valueType == typeof(TItem[]) || valueType == typeof(List<TItem>);
                if (isIntType)
                {
                    List<int> value = new List<int> { };
                    foreach (var val in values)
                    {
                        value.Add(int.Parse(val, CultureInfo.InvariantCulture));
                    }

                    if (isCtrlKey && Value != null)
                    {
                        value.InsertRange(0, Value as IEnumerable<int>);
                    }

                    if (removeValue != null && value.Contains(int.Parse(removeValue, CultureInfo.InvariantCulture)))
                    {
                        value.Remove(int.Parse(removeValue, CultureInfo.InvariantCulture));
                    }

                    if (value.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return valueType == typeof(int?[]) ? (TValue)(object)value.ToArray().Cast<int?>().ToArray() : (TValue)(object)value.ToArray();
                    }
                    else
                    {
                        return valueType == typeof(List<int?>) ? (TValue)(object)value.ToList().Cast<int?>().ToList() : (TValue)(object)value;
                    }
                }
                else if (valueType == typeof(string[]) || valueType == typeof(List<string>))
                {
                    if (isCtrlKey && Value != null)
                    {
                        values.InsertRange(0, Value as IEnumerable<string>);
                    }

                    if (removeValue != null && values.Contains(removeValue))
                    {
                        values.Remove(removeValue);
                    }

                    if (values.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return (TValue)(object)values.ToArray();
                    }
                    else
                    {
                        return (TValue)(object)values;
                    }
                }
                else if (isLongType)
                {
                    List<long> value = new List<long> { };
                    foreach (var val in values)
                    {
                        value.Add(long.Parse(val, CultureInfo.InvariantCulture));
                    }

                    if (Value == null && value.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return valueType == typeof(long?[]) ? (TValue)(object)value.ToArray().Cast<long?>().ToArray() : (TValue)(object)value.ToArray();
                    }
                    else
                    {
                        return valueType == typeof(List<long?>) ? (TValue)(object)value.ToList().Cast<long?>().ToList() : (TValue)(object)value;
                    }
                }
                else if (isDecimalType)
                {
                    List<decimal> value = new List<decimal> { };
                    foreach (var val in values)
                    {
                        value.Add(decimal.Parse(val, CultureInfo.InvariantCulture));
                    }
                    if (Value == null && value.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return valueType == typeof(decimal?[]) ? (TValue)(object)value.ToArray().Cast<decimal?>().ToArray() : (TValue)(object)value.ToArray();
                    }
                    else
                    {
                        return valueType == typeof(List<decimal?>) ? (TValue)(object)value.ToList().Cast<decimal?>().ToList() : (TValue)(object)value;
                    }
                }
                else if (isDoubleType)
                {
                    List<double> value = new List<double> { };
                    foreach (var val in values)
                    {
                        value.Add(double.Parse(val, CultureInfo.InvariantCulture));
                    }
                    if (Value == null && value.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return valueType == typeof(double?[]) ? (TValue)(object)value.ToArray().Cast<double?>().ToArray() : (TValue)(object)value.ToArray();
                    }
                    else
                    {
                        return valueType == typeof(List<double?>) ? (TValue)(object)value.ToList().Cast<double?>().ToList() : (TValue)(object)value;
                    }
                }
                else if (isGuidType)
                {
                    List<Guid> value = new List<Guid> { };
                    foreach (var val in values)
                    {
                        value.Add(Guid.Parse(val));
                    }
                    if (Value == null && value.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return valueType == typeof(Guid?[]) ? (TValue)(object)value.ToArray().Cast<Guid?>().ToArray() : (TValue)(object)value.ToArray();
                    }
                    else
                    {
                        return valueType == typeof(List<Guid?>) ? (TValue)(object)value.ToList().Cast<Guid?>().ToList() : (TValue)(object)value;
                    }
                }
                else if (isEnumType)
                {
                    List<Enum> value = new List<Enum> { };
                    foreach (var val in values)
                    {
                        var selectedValue = selectedValues.Count > 0 ? selectedValues[0] : val;
                        value.Add((Enum)Enum.Parse(selectedValue.GetType(), val));
                    }
                    if (Value == null && value.Count == 0)
                    {
                        return default;
                    }

                    if (valueType.IsArray)
                    {
                        return valueType == typeof(Enum[]) ? (TValue)(object)value.ToArray().Cast<Enum>().ToArray() : (TValue)(object)value.ToArray();
                    }
                    else
                    {
                        return valueType == typeof(List<Enum>) ? (TValue)(object)value.ToList().Cast<Enum>().ToList() : (TValue)(object)value;
                    }
                }
                else if (isObjectType)
                {
                    if (values == null && values.Count == 0)
                    {
                        return default;
                    }

                    return (valueType.IsArray) ? (TValue)(object)values.ToArray() : (TValue)(object)values.ToList();
                }
            }
            else if (IsSimpleType() && values.Count > 0)
            {
                return (TValue)SfBaseUtils.ChangeType(values[0], typeof(TValue));
            }

            return default;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DragEndAsync(bool isOutsideListbox, string draggedValue, bool sameListBox, bool scopedListBox, int previousIndex, int currentIndex, DotNetObjectReference<object> scopeObj)
        {
            SfListBox<TValue, TItem> scope = default;
            if (scopeObj != null)
            {
                scope = (SfListBox<TValue, TItem>)scopeObj.Value;
            }

            if (sameListBox || scopedListBox || isOutsideListbox)
            {
                var eventArgs = new DropEventArgs<TItem> { Cancel = false, CurrentIndex = currentIndex, PreviousIndex = previousIndex, Target = scope.element };
                TValue value = default;
                var select = true;
                if (draggedValue != null && !selectedValues.Contains(draggedValue))
                {
                    value = GetValue(new List<string> { draggedValue }, false, null);
                    select = false;
                    if (value == null)
                    {
                        return;
                    }

                    eventArgs.Items = GetDataByValue(value);
                }
                else
                {
                    eventArgs.Items = GetDataByValue(Value);
                }

                var previousSrcData = ListData;
                var previousDestData = scope?.ListData;
                await SfBaseUtils.InvokeEvent<DropEventArgs<TItem>>(Delegates?.OnDrop, eventArgs);
                if (eventArgs.Cancel || (sameListBox && eventArgs.CurrentIndex == eventArgs.PreviousIndex))
                {
                    return;
                }

                if (scopedListBox && !isOutsideListbox)
                {
                    await MoveData(null, currentIndex, scope, value, false, false, null, true, select);
                }
                else if (!isOutsideListbox)
                {
                    await MoveWithInListBox(currentIndex, value, select);
                }
                if (sameListBox)
                {
                    currentIndex = ((currentIndex - previousIndex) > 0) ? (currentIndex - 1) : currentIndex;
                }
                if (filterInputObj == null || string.IsNullOrEmpty(filterInputObj.InputTextValue))
                {
                    tempData = ListData;
                }
                await SfBaseUtils.InvokeEvent<DragEventArgs<TItem>>(Delegates?.Dropped, new DragEventArgs<TItem> { CurrentIndex = currentIndex, PreviousIndex = previousIndex, Items = eventArgs.Items, Source = new SourceDestinationModel<TItem> { PreviousData = previousSrcData, CurrentData = ListData }, Destination = new SourceDestinationModel<TItem> { PreviousData = previousDestData, CurrentData = scope?.ListData } });
            }
        }

        private async Task MoveWithInListBox(int index, TValue draggedValue, bool select)
        {
            if (draggedValue == null)
            {
                draggedValue = Value;
            }

            List<TItem> selectedList = GetDataByValue(draggedValue);
            int? addIndex = GetValidIndex(index, selectedList);
            ListData = ListData.Where(list => !selectedList.Contains(list));
            await InsertItem(selectedList, addIndex);
            await UpdateSelectedValue(select ? selectedList : GetDataByValue(Value));
            StateHasChanged();
        }

        private int? GetValidIndex(int index, List<TItem> selectedList)
        {
            int? addIndex = index;
            if (index == ListData.Count())
            {
                return null;
            }
            else
            {
                foreach (var listItem in selectedList)
                {
                    var dataIndex = ListData.IndexOf(listItem);
                    if (dataIndex > -1 && dataIndex <= index)
                    {
                        addIndex--;
                    }
                }
            }

            return addIndex;
        }

        private async Task ToolbarClickHandler(string item)
        {
            isToolbarAction = true;
            switch (item)
            {
                case MOVEUP:
                case MOVEDOWN:
                    await MoveUpDown(item, Value);
                    break;
                case MOVETO:
                    await MoveData(item, null, scopeListbox, Value, false);
                    break;
                case MOVEFROM:
                    await MoveScopedData(item, false);
                    break;
                case MOVEALLTO:
                    await MoveData(item, null, scopeListbox);
                    break;
                case MOVEALLFROM:
                    await MoveScopedData(item);
                    break;
                default:
                    await TriggerBeginActionEvent(item, true);
                    break;
            }
            isToolbarAction = true;
        }

        private async Task MoveScopedData(string item, bool isAll = true)
        {
            if (scopeListbox != null)
            {
                IEnumerable<TItem> listData = isAll ? scopeListbox.ListData : scopeListbox.GetDataByValue(scopeListbox.Value);
                var cancel = await TriggerBeginActionEvent(item, true);
                if (!cancel)
                {
                    await scopeListbox.MoveData(item, null, this, scopeListbox.Value, isAll, false, listData);
                    await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(Delegates?.OnActionComplete, new ActionCompleteEventArgs<TItem> { Result = listData, EventName = item });
                }
            }
        }

        private async Task MoveUpDown(string item, TValue values, bool isAction = true)
        {
            var selectedList = GetDataByValue(values == null ? Value : values);
            var cancel = await TriggerBeginActionEvent(item, isAction);
            if (!cancel)
            {
                int index = 0;
                List<int> indexes = new List<int>();
                if (item == MOVEDOWN)
                {
                    selectedList.Reverse();
                }

                foreach (var listItem in selectedList)
                {
                    index = ListData.IndexOf(listItem);
                    if (index > -1)
                    {
                        index = item == MOVEUP ? index - 1 : index + 1;
                        if (index < 0 || index >= ListData.Count())
                        {
                            continue;
                        }

                        ListData = ListData.Where(list => !SfBaseUtils.Equals(list, listItem));
                        await InsertItem(new List<TItem> { listItem }, index);
                        indexes.Add(index);
                    }
                }

                if (selectionSettings == null || !selectionSettings.ShowCheckbox)
                {
                    foreach (var idx in indexes)
                    {
                        ListDataSource.ElementAt(idx).ListClass = SfBaseUtils.AddClass(ListDataSource.ElementAt(idx).ListClass, SELECTED);
                    }
                }

                await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(Delegates?.OnActionComplete, new ActionCompleteEventArgs<TItem> { Result = selectedList, EventName = item });
            }
        }

        private async Task MoveData(string item, double? index, SfListBox<TValue, TItem> scope = default, TValue values = default, bool isAll = true, bool isAction = true, IEnumerable<TItem> listData = null, bool isDragDrop = false, bool select = true)
        {
            if (scope == null)
            {
                scope = scopeListbox;
            }

            if (listData == null)
            {
                listData = isAll ? ListData : GetDataByValue(values == null ? Value : values);
            }

            var cancel = await TriggerBeginActionEvent(item, isAction);
            if (!cancel)
            {
                if (ListData != null)
                {
                    if (scope != null)
                    {
                        if (scope.AllowFiltering && !string.IsNullOrEmpty(scope.filterInputObj.InputTextValue))
                        {
                            var currentViewData = scope.ListData;
                            scope.ListData = scope.tempData;
                            await scope.InsertItem(listData, (int?)index, scope.ListData != null);
                            scope.filteredData = scope.ListData;
                            scope.ListData = currentViewData;
                        }
                        await scope.InsertItem(listData, (int?)index, scope.ListData != null);
                        scope.tempData = scope.ListData;
                    }

                    if (listData.Count() == 1)
                    {
                        index = ListData.IndexOf(listData.ElementAt(0));
                    }

                    if (isAll)
                    {
                        ListData = new List<TItem>();
                    }
                    else
                    {
                        ListData = ListData.Where(list => !listData.Contains(list)).ToList();
                    }
                    if (filterInputObj == null || string.IsNullOrEmpty(filterInputObj.InputTextValue))
                    {
                        tempData = ListData;
                    }
                    else
                    {
                        tempData = tempData.Where(list => !listData.Contains(list)).ToList();
                    }
                    await RenderItems();
                    if (select)
                    {
                        selectedValues.Clear();
                        previousIndex = null;
                        if (!isDragDrop && listData.Count() == 1 && ListData.Any() && index != default && index > -1 && index <= ListData.Count())
                        {
                            await UpdateSelectedValue(new List<TItem> { ListData.ElementAt((int)(index == ListData.Count() ? index - 1 : index)) }, false, true);
                        }
                        else
                        {
                            await UpdateValue(new List<string>(), false);
                        }
                    }
                    else
                    {
                        await UpdateSelectedValue(GetDataByValue(Value));
                    }

                    if (scope != null && scope.selectedValues.Any())
                    {
                        await scope.UpdateSelectedValue(scope.GetDataByValue(scope.Value));
                    }

                    StateHasChanged();
                    scope?.StateHasChanged();
                }

                await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(Delegates?.OnActionComplete, new ActionCompleteEventArgs<TItem> { Result = listData, EventName = item });
            }
        }

        private async Task<bool> TriggerBeginActionEvent(string actionName, bool isAction)
        {
            if (isAction)
            {
                var actionEventArgs = new ActionBeginEventArgs { EventName = actionName };
                await SfBaseUtils.InvokeEvent<ActionBeginEventArgs>(Delegates?.OnActionBegin, actionEventArgs);
                return actionEventArgs.Cancel;
            }

            return false;
        }

        private static bool IsSimpleType()
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            return isNullable || type == typeof(String) || type == typeof(int) || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Double) || type == typeof(Decimal) || type == typeof(Boolean);
        }

        private async Task PasteHandler()
        {
            await Task.Delay(100);  // Added delay for get the pasted content in search input.
            await SearchList();
        }

        private async Task InvokeClearBtnEvent()
        {
            filterValue = null;
            await filterInputObj.SetValue(string.Empty, Inputs.FloatLabelType.Never, false);
            await Task.Delay(50); // Added delay for cleared the filtering conent in seach input.
            await SearchList();
        }

        private async Task SearchList()
        {
            var typedString = filterInputObj.InputTextValue;
            var filterEventArgs = new FilteringEventArgs() { Cancel = false, PreventDefaultAction = false, Text = typedString };
            await SfBaseUtils.InvokeEvent<FilteringEventArgs>(Delegates?.ItemSelected, filterEventArgs);
            if (filterEventArgs.Cancel || filterEventArgs.PreventDefaultAction)
            {
                return;
            }

            tempData = filteredData ?? tempData;

            Query query = string.IsNullOrEmpty(typedString?.Trim()) ? null : Query;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
            DataManager.Json = (IEnumerable<object>)tempData;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            if (string.IsNullOrEmpty(typedString))
            {
                ListData = tempData;
                await RenderItems();
            }
            else
            {
                await SetListData(DataSource, Fields, query);
            }

            await UpdateSelectedValue(GetDataByValue(Value));
        }

        private TItem GetData(object curValue)
        {
            if (IsSimpleDataType())
            {
                if (curValue.GetType().IsEnum)
                {
                    return ListData.Where(item => SfBaseUtils.Equals(Enum.Parse(curValue.GetType(), item.ToString()), curValue)).FirstOrDefault();
                }
                else
                {
                    return ListData.Where(item => SfBaseUtils.Equals(item, (TItem)curValue)).FirstOrDefault();
                }
            }
            else
            {
                var fields = !string.IsNullOrEmpty(Fields?.Value) ? Fields.Value : "value";
                curValue = curValue.GetType().IsEnum ? Enum.Parse(curValue.GetType(), curValue.ToString()) : curValue;
                return ListData.Where(item => SfBaseUtils.Equals(SfBaseUtils.ChangeType(DataUtil.GetObject(fields, item), curValue.GetType()), curValue)).FirstOrDefault();
            }
        }

        private async Task UpdateDataSource()
        {
            await Render(DataSource, Fields, Query);
            if (ListData != null)
            {
                tempData = ListData;
                if (AllowFiltering)
                {
                    MainData = ListData;
                }

                if (Value != null)
                {
                    await UpdateSelectedValue(GetDataByValue(Value));
                }

                if (!render)
                {
                    render = true;
                }

                StateHasChanged();
                scopeListbox?.StateHasChanged();
            }
        }

        private async Task<SfListBox<TValue,TItem>> GetScopedListbox(string scope)
        {
            if (scope != null && toolbarSettings != null)
            {
                DotNetObjectReference<object> dotnetObj = await InvokeMethod<DotNetObjectReference<object>>(GETSCOPEDLISTBOX, false, element, Scope);
                if (dotnetObj != null)
                {
                    scopeListbox = (SfListBox<TValue, TItem>)dotnetObj.Value;
                }

                return scopeListbox;
            }
            else
            {
                return null;
            }
        }

        protected override Query GetQuery(Query query)
        {
            Query filterQuery = new Query();
            if (AllowFiltering && filterInputObj != null)
            {
                var typedString = filterInputObj.InputTextValue;
                filterQuery = (query != null) ? CloneQuery(query) : ((Query != null) ? CloneQuery(Query) : new Query());
                string filterType = string.IsNullOrEmpty(typedString) ? "contains" : FilterType.ToString().ToLower(CultureInfo.CurrentCulture);
                var IsSimpleDataType = !DataManager.IsDataManager && this.IsSimpleDataType();
                string fields = (!string.IsNullOrEmpty(Fields?.Text) && !IsSimpleDataType) ? Fields.Text : string.Empty;
                var filterValue = !string.IsNullOrEmpty(typedString) ? typedString : string.Empty;
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

        protected override async Task<ActionBeginEventArgs> ActionBegin(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actionBeginArgs = new ActionBeginEventArgs { Query = query, EventName = ACTIONBEGIN };
            await SfBaseUtils.InvokeEvent<ActionBeginEventArgs>(Delegates?.OnActionBegin, actionBeginArgs);
            return actionBeginArgs;
        }

        protected override async Task ActionComplete(IEnumerable<TItem> dataSource, Query query = null)
        {
            var actionCompleteArgs = new ActionCompleteEventArgs<TItem> { Result = dataSource, Query = query, Count = dataSource != null ? dataSource.Count() : 0, EventName = ACTIONCOMPLETE };
            await SfBaseUtils.InvokeEvent<ActionCompleteEventArgs<TItem>>(Delegates?.OnActionComplete, actionCompleteArgs);
            if (!actionCompleteArgs.Cancel)
            {
                ListData = actionCompleteArgs.Result;
                await RenderItems();
            }
        }

        protected override async Task ActionFailure(object args)
        {
            await SfBaseUtils.InvokeEvent<object>(Delegates?.OnActionFailure, args);
        }

        protected override bool IsFilter()
        {
            return AllowFiltering;
        }

        protected async Task SetLocalStorage(string persistId, List<string> values)
        {
            string serializeValues = JsonSerializer.Serialize(values);
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, serializeValues });
        }

        protected async Task<string> GetPersistData()
        {
            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { id });
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string propKey, object propValue)
        {
            switch (propKey)
            {
                case nameof(Fields):
                    var fields = (ListBoxFieldSettings)propValue;
                    Fields = new FieldSettingsModel() { GroupBy = fields?.GroupBy, HtmlAttributes = fields?.HtmlAttributes, IconCss = fields?.IconCss, Text = fields?.Text, Value = fields?.Value };
                    SetFields();
                    if (selectionSettings == null)
                    {
                        selectionSettings = new ListBoxSelectionSettings();
                    }

                    if (ListData == null || !ListData.Any())
                    {
                        UpdateDataSource().GetAwaiter();
                    }
                    break;
                case nameof(selectionSettings):
                    selectionSettings = (ListBoxSelectionSettings)propValue;
                    Initialize();
                    break;
                case nameof(toolbarSettings):
                    toolbarSettings = (ListBoxToolbarSettings)propValue;
                    Initialize();
                    break;
                case nameof(ItemTemplate):
                    ItemTemplate = (RenderFragment<TItem>)propValue;
                    break;
                case nameof(NoRecordsTemplate):
                    NoRecordsTemplate = (RenderFragment)propValue;
                    break;
            }
        }

        private class ToolbarProps
        {
            public string ItemText { get; set; }

            public bool Disabled { get; set; } = true;
        }
    }
}