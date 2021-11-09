using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;
using Syncfusion.Blazor.Lists.Internal;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// The SfListView control represents the data in interactive hierarchical structure interface across different layouts or views,
    /// that also has features such as data-binding, template, grouping and virtualization.
    /// </summary>
    public partial class SfListView<TValue> : SfBaseComponent
    {

        /// <summary>
        /// Disables the list items by passing the Id and text fields.
        /// listItem like fields: TValue { fieldName: fieldValue }.
        /// </summary>
        /// <param name="listItem">Specifies the list item arguement.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DisableItem(TValue listItem)
        {
            await InvokeMethod("sfBlazor.ListView.enableState", ElementRef, listItem, false);
        }

        /// <summary>
        /// Disables the list items by passing the Id and text fields.
        /// listItem like fields: TValue { fieldName: fieldValue }.
        /// </summary>
        /// <param name="listItem">Specifies the list item arguement.</param>
        /// <returns>Task.</returns>
        public async Task DisableItemAsync(TValue listItem)
        {
            await DisableItem(listItem);
        }

        /// <summary>
        /// Enables the disabled list items by passing the Id and text fields.
        /// listItem like fields: TValue { fieldName: fieldValue }.
        /// </summary>
        /// <param name="listItem">Specifies the list item arguement.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task EnableItem(TValue listItem)
        {
            await InvokeMethod("sfBlazor.ListView.enableState", ElementRef, listItem, true);
        }

        /// <summary>
        /// Enables the disabled list items by passing the Id and text fields.
        /// listItem like fields: TValue { fieldName: fieldValue }.
        /// </summary>
        /// <param name="listItem">Specifies the list item arguement.</param>
        /// <returns>Task.</returns>
        public async Task EnableItemAsync(TValue listItem)
        {
            await EnableItem(listItem);
        }

        /// <summary>
        /// Gets the details of the currently selected item from the list items.
        /// </summary>
        /// <returns>Task.</returns>
        internal async Task<SelectedItems<TValue>> GetSelectedItem()
        {
            CheckedItemDetails checkedElements = await InvokeMethod<CheckedItemDetails>("sfBlazor.ListView.getCheckedItems", false, new object[] { ElementRef });
            List<TValue> data = new List<TValue>();
            List<string> text = new List<string>();
            ListElementDetails<TValue> details;
            foreach (var id in checkedElements.ElementId)
            {
                details = GetLiElementData(new ListElementReference() { ElementId = id, Key = checkedElements.Key }, false);
                if (details != null)
                {
                    data.Add(details.ItemData);
                    text.Add(details.ItemText);
                }
            }

            return new SelectedItems<TValue>() { Data = data, Text = text };
        }

        /// <summary>
        /// Gets the details of the currently checked item from the list items.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<SelectedItems<TValue>> GetCheckedItems()
        {
            return ShowCheckBox ? await GetSelectedItem() : null;
        }

        /// <summary>
        /// Gets the details of the currently checked item from the list items.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<SelectedItems<TValue>> GetCheckedItemsAsync()
        {
            return await GetCheckedItems();
        }

        /// <exclude/>
        /// <summary>
        /// Remove Element from datasource based on given item details
        /// element like fields: TValue { fieldName: fieldValue }.
        /// </summary>
        /// <param name="listItem">Specifies the list item arguement.</param>
        protected void Remove(TValue listItem)
        {
            List<TValue> list = listViewDataSource[key: DATASOURCEKEY].ToList();
            Type tvalueType = typeof(TValue);
            string selectedId = (string)tvalueType.GetProperty(ListFields.Id).GetValue(listItem, null);
            ListElementDetails<TValue> details = GetLiElementData(new ListElementReference() { ElementId = selectedId, Key = DATASOURCEKEY }, false);
            if (details.Index != null)
            {
                list.RemoveAt(details.Index[0]);
                listViewDataSource[key: DATASOURCEKEY] = DataSource = list;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Removes item(s) from the ListView by passing the array of field objects.
        /// listItems like fields: TValue { fieldName: fieldValue }.
        /// </summary>
        /// <param name="listItems">Specifies the list items arguement.</param>
        public void RemoveItems(IEnumerable<TValue> listItems)
        {
            if (listItems != null && listItems.Any())
            {
                foreach (TValue data in listItems)
                {
                    Remove(data);
                }
            }
        }

        /// <summary>
        /// Check the items in ListView
        /// To check the specific listItem by passing the fields like : TValue { fieldName: fieldValue }
        /// To check all the listItem by passing empty argument.
        /// </summary>
        /// <param name="listItems">Specifies the list items arguement.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CheckItems(IEnumerable<TValue> listItems = null)
        {
            if (listItems != null && listItems.Any())
            {
                if (ShowCheckBox)
                {
                    await InvokeMethod("sfBlazor.ListView.getCheckData", ElementRef, listItems.ToArray(), true, ListFields.Id);
                }
                else
                {
                    await InvokeMethod("sfBlazor.ListView.selectItem", ElementRef, listItems.ToArray());
                }
            }
            else
            {
                await InvokeMethod("sfBlazor.ListView.checkAllItems", ElementRef);
            }
        }

        /// <summary>
        /// Check the items in ListView
        /// To check the specific listItem by passing the fields like : TValue { fieldName: fieldValue }
        /// To check all the listItem by passing empty argument.
        /// </summary>
        /// <param name="listItems">Specifies the list items arguement.</param>
        /// <returns>Task.</returns>
        public async Task CheckItemsAsync(IEnumerable<TValue> listItems = null)
        {
            if (listItems != null)
            {
                foreach (TValue data in listItems)
                {
                    Type tvalueType = typeof(TValue);
                    string selectedId = (string)tvalueType.GetProperty(ListFields.Id).GetValue(data, null);
                    UpdateData(true, selectedId);
                }
            }
            await CheckItems(listItems);
        }

        /// <summary>
        /// Uncheck the items in ListView.
        /// To uncheck the specific listItem by passing the fields like : TValue { fieldName: fieldValue }.
        /// To uncheck all the listItem by passing empty argument.
        /// </summary>
        /// <param name="listItems">Specifies the list item arguement.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UncheckItems(IEnumerable<TValue> listItems = null)
        {
            if (listItems != null && listItems.Any())
            {
                await InvokeMethod("sfBlazor.ListView.getCheckData", ElementRef, listItems.ToArray(), false, ListFields.Id);
            }
            else
            {
                await InvokeMethod("sfBlazor.ListView.uncheckAllItems", ElementRef);
            }
        }

        /// <summary>
        /// Uncheck the items in ListView.
        /// To uncheck the specific listItem by passing the fields like : TValue { fieldName: fieldValue }.
        /// To uncheck all the listItem by passing empty argument.
        /// </summary>
        /// <param name="listItems">Specifies the list item arguement.</param>
        /// <returns>Task.</returns>
        public async Task UncheckItemsAsync(IEnumerable<TValue> listItems = null)
        {
            if (listItems != null)
            {
                foreach (TValue data in listItems)
                {
                    Type tvalueType = typeof(TValue);
                    string selectedId = (string)tvalueType.GetProperty(ListFields.Id).GetValue(data, null);
                    UpdateData(false, selectedId);
                }
            }
            await UncheckItems(listItems);
        }

    }
}