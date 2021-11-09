using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Lists.Internal
{
    /// <exclude/>
    /// <summary>
    /// A list base component for all the Syncfusion Blazor List dependant components to implement the common functionalities.
    /// </summary>
    /// <typeparam name="T">The first generic type parameter.</typeparam>
    public class ListBaseFoundation<T> : ListCommonBase<T>
    {
        private Dictionary<string, object> fieldProp = new Dictionary<string, object>();
        private ListBaseFields<T> fields = new ListBaseFields<T>();
        private DefaultListBaseOptions<T> defaultListBaseOptions = new DefaultListBaseOptions<T>();

        internal bool IsGroupedMode { get; set; }

        internal bool IsItemTemplate { get; set; }

        internal bool IsGroupTemplate { get; set; }

        internal ClassList ClassNames { get; set; }

        internal IEnumerable<T> DataDetails => GetSortedData();

        internal IEnumerable<T> PrimitiveDataSource => GetSortedArrayData();

        internal List<ComposedItemModel<T>> ComposedDataSource => ComposeGroupData();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (ListParent != null)
            {
                Type type = (!string.IsNullOrEmpty(ItemTemplatePropertyName)) ? ListParent.GetType() : null;
                if (!string.IsNullOrEmpty(ItemTemplatePropertyName))
                {
                    RenderFragment<T> template = (RenderFragment<T>)type?.GetProperty(ItemTemplatePropertyName).GetValue(ListParent);
                    IsItemTemplate = template != null;
                    if (IsItemTemplate)
                    {
                        Template = template;
                    }
                }

                if (!string.IsNullOrEmpty(GroupTemplatePropertyName))
                {
                    RenderFragment<ComposedItemModel<T>> groupTemplate = (RenderFragment<ComposedItemModel<T>>)type?.GetProperty(GroupTemplatePropertyName).GetValue(ListParent);
                    IsGroupTemplate = groupTemplate != null;
                    if (IsGroupTemplate)
                    {
                        GroupTemplate = groupTemplate;
                    }
                }
            }

            ClassNames = defaultListBaseOptions.GetModuleClassList(ListBaseOptionModel?.ModuleName);
        }

        /// <summary>
        /// maps the default setting for the list generated.
        /// </summary>
        /// <param name="options">Specifies mapping options.</param>
        protected void MapSettings(ListBaseOptionModel<T> options)
        {
            fields = ListBaseOptionModel.Fields;
            if (ListBaseOptionModel?.Fields.GroupBy != null)
            {
                IsGroupedMode = true;
            }

            if (!string.IsNullOrEmpty(options?.ModuleName))
            {
                ClassNames = defaultListBaseOptions.GetModuleClassList(options.ModuleName);
            }

            fieldProp.Clear();
            SfBaseUtils.UpdateDictionary(nameof(fields.Child), fields.Child, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.Enabled), fields.Enabled, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.GroupBy), fields.GroupBy, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.HtmlAttributes), fields.HtmlAttributes, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.IconCss), fields.IconCss, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.Id), fields.Id, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.IsChecked), fields.IsChecked, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.Text), fields.Text, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.IsVisible), fields.IsVisible, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.Tooltip), fields.Tooltip, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fields.Value), fields.Value, fieldProp);
        }

        /// <summary>
        /// get the converted data using field values.
        /// </summary>
        internal string GetConvertedData(T data, string propertyName)
        {
            if (fieldProp[propertyName] != null)
            {
                object propertValue = DataUtil.GetObject(fieldProp[propertyName].ToString(), data);
                if (propertValue != null)
                {
                    return propertValue.ToString();
                }

                return null;
            }

            return null;
        }

        // maps the user given field values with the listbase fields
        internal FieldsValueMapping<List<T>> GetMappedData(T data)
        {
            FieldsValueMapping<List<T>> mappedData;
            return mappedData = fieldProp.Count > 0 ? new FieldsValueMapping<List<T>>
            {
                Child = (List<T>)DataUtil.GetObject(fieldProp["Child"].ToString(), data),
                Enabled = (bool?)DataUtil.GetObject(fieldProp["Enabled"].ToString(), data),
                GroupBy = GetConvertedData(data, "GroupBy"),
                HtmlAttributes = (Dictionary<string, object>)DataUtil.GetObject(fieldProp["HtmlAttributes"].ToString(), data),
                IconCss = (string)DataUtil.GetObject(fieldProp["IconCss"].ToString(), data),
                Id = GetConvertedData(data, "Id"),
                IsChecked = (bool?)DataUtil.GetObject(fieldProp["IsChecked"].ToString(), data),
                IsVisible = (bool?)DataUtil.GetObject(fieldProp["IsVisible"].ToString(), data),
                Text = GetConvertedData(data, "Text"),
                Tooltip = GetConvertedData(data, "Tooltip"),
                Value = DataUtil.GetObject(fieldProp["Value"].ToString(), data) != null ? DataUtil.GetObject(fieldProp["Value"].ToString(), data).ToString() : null
            }
            : new FieldsValueMapping<List<T>>();
        }

        /// <summary>
        /// Sorts the given list items with complex datasource.
        /// </summary>
        /// <returns> Returns the sorted data.</returns>
        protected IEnumerable<T> GetSortedData()
        {
            IEnumerable<T> sortedData = DataSource;
            if (sortedData.Any())
            {
                if (ListBaseOptionModel?.SortOrder != SortOrder.None && DataSource.ElementAt(0).GetType().GetProperty(ListBaseOptionModel?.Fields.Text) != null)
                {
                    sortedData = DataOperations.PerformSorting<T>(DataSource, new List<Sort>() { new Sort { Direction = ListBaseOptionModel?.SortOrder.ToString(), Name = ListBaseOptionModel?.Fields.Text } });
                }
            }

            return sortedData;
        }

        /// <summary>
        /// Sorts the given list items with primitive datasource.
        /// </summary>
        /// <returns> Returns the sorted array data.</returns>
        protected IEnumerable<T> GetSortedArrayData()
        {
            if (ListBaseOptionModel?.SortOrder != SortOrder.None)
            {
                DataSource = ListBaseOptionModel?.SortOrder == SortOrder.Ascending ? DataSource.OrderBy(x => x) : DataSource.OrderByDescending(x => x);
            }

            return DataSource;
        }

        /// <summary>
        /// groups the given list items with complex datasource.
        /// </summary>
        /// <returns> Returns the grouped data.</returns>
        protected List<ComposedItemModel<T>> ComposeGroupData()
        {
            List<ComposedItemModel<T>> groupData = new List<ComposedItemModel<T>>();
            if (DataSource != null && DataSource.Any())
            {
                if (ListBaseOptionModel?.SortOrder != SortOrder.None && ListBaseOptionModel?.Fields.GroupBy != null)
                {
                    DataSource = DataOperations.PerformSorting<T>(DataSource, new List<Sort>() { new Sort { Direction = ListBaseOptionModel?.SortOrder.ToString(), Name = ListBaseOptionModel?.Fields.GroupBy } }).ToList();
                }

                if (ListBaseOptionModel?.Fields.GroupBy != null)
                {
                    IsGroupedMode = true;
                    IEnumerable<GroupResult> groupedItems = (IEnumerable<GroupResult>)DataOperations.PerformGrouping(DataSource, new List<string>() { ListBaseOptionModel?.Fields.GroupBy });
                    foreach (GroupResult groupItem in groupedItems)
                    {
                        groupData.Add(new ComposedItemModel<T> { Key = groupItem.Key?.ToString() ?? string.Empty, IsGroupItem = true, Items = groupItem.Items.Cast<T>().ToList<T>() });
                        foreach (T item in (IEnumerable<T>)groupItem?.Items)
                        {
                            groupData.Add(new ComposedItemModel<T> { Data = item, Key = groupItem.Key?.ToString() ?? string.Empty, IsGroupItem = false });
                        }
                    }
                }
            }

            return groupData;
        }

        /// <summary>
        /// ItemCreated event invoke method.
        /// </summary>
        /// <param name="item">Specifies the item details.</param>
        /// <param name="mappedData">Specifies the mapped data.</param>
        /// <param name="isCreated">Specified the created parameter.</param>
        protected void InvokeItemCreate(T item, FieldsValueMapping<List<T>> mappedData = null, bool isCreated = false)
        {
            ItemCreatedArgs<T> itemCreatedArgs = new ItemCreatedArgs<T> { ListsDataSource = DataSource, CurData = item, ListsFields = ListBaseOptionModel?.Fields, ListsItem = null, Text = mappedData?.Text ?? item?.ToString(), ListsOptions = ListBaseOptionModel, Name = isCreated ? "ItemCreated" : "ItemCreating" };
            if (isCreated)
            {
                ItemCreated?.Invoke(itemCreatedArgs);
            }
            else
            {
                ItemCreating?.Invoke(itemCreatedArgs);
            }
        }

        internal override void ComponentDispose()
        {
            ClassNames = null;
            fieldProp = null;
            DataSource = null;
            ListParent = null;
            ListBaseOptionModel = null;
            ItemCreated = null;
            ItemCreating = null;
            fields = null;
            defaultListBaseOptions = null;
        }
    }
}
