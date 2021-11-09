using System;
using System.Reflection;
using Syncfusion.Blazor.Data;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <summary>
    /// Specifies the ComplexCreateList.
    /// </summary>
    /// <typeparam name="TValue">"TypeParam".</typeparam>
    public partial class CreateListFromComplex<TValue> : TreeViewListItems<TValue>
    {
        internal const string IDPREFIX = "sftreeview-";
        private Dictionary<string, object> fieldProp = new Dictionary<string, object>();
        private FieldsMapping fieldsMap = new FieldsMapping();

        /// <summary>
        /// Specifies the datasource of list element.
        /// </summary>
        [Parameter]
        public IEnumerable<TValue> ListData { get; set; }

        /// <summary>
        /// Specifies the TreeOption field values.
        /// </summary>
        [Parameter]
        public TreeOptions<TValue> TreeOptions { get; set; }

        /// <summary>
        /// Specifies the TreeItemCreating event.
        /// </summary>
        [Parameter]
        public new Action<TreeItemCreatedArgs<TValue>> TreeItemCreating { get; set; }

        /// <summary>
        /// Specifies the tree node level of treeview nodes.
        /// </summary>
        [Parameter]
        public new int TreeNodeLevel { get; set; } = 1;

        /// <summary>
        /// Specifies the treeview list base option model.
        /// </summary>
        [Parameter]
        public ListModel ListModel { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ListModel = GetDefaultListOptions();
            if (TreeParent.TreeViewFields != null)
            {
                MapSettings(ListModel, false);
            }
        }

        internal void DisposeTreeOptions()
        {
            TreeOptions = null;
        }

        // Returns the display attribute for the list item.
        private string GetTreeClass()
        {
            return TreeOptions != null && !TreeOptions.IsExpanded ? "e-display-none" : string.Empty;
        }

        // Return  Ul element attribute details.
        internal Dictionary<string, object> GetAttributes()
        {
            string parentUl = "e-list-parent e-ul";
            if (TreeOptions != null)
            {
                string isLoaded = TreeOptions.IsLoaded ? string.Empty : "e-display-none";
                parentUl = SfBaseUtils.AddClass(parentUl, isLoaded);
            }
            Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            parentUl = SfBaseUtils.AddClass(parentUl, GetTreeClass());
            SfBaseUtils.UpdateDictionary("class", parentUl, htmlAttributes);
            SfBaseUtils.UpdateDictionary("role", "group", htmlAttributes);
            return htmlAttributes;
        }

        private string GetConvertedData(string propertyName, TValue data)
        {
            object nodeData = DataUtil.GetObject(fieldProp[propertyName].ToString(), data) != null ? DataUtil.GetObject(fieldProp[propertyName].ToString(), data).ToString() : null;
            return nodeData != null ? nodeData.ToString() : null;
        }

        // Maps the user given field values with the listbase fields.
        internal FieldsValueMapping<List<TValue>> GetMappedData(TValue fieldData)
        {
            try
            {
                FieldsValueMapping<List<TValue>> mappedData;
                string idValue = GetConvertedData("Id", fieldData);
                return mappedData = fieldProp.Count > 0 ? new FieldsValueMapping<List<TValue>>
                {
                    Child = fieldProp["Child"] != null ? (List<TValue>)DataUtil.GetObject(fieldProp["Child"].ToString(), fieldData) : null,
                    HtmlAttributes = fieldProp["HtmlAttributes"] != null ? (Dictionary<string, object>)DataUtil.GetObject(fieldProp["HtmlAttributes"].ToString(), fieldData) : null,
                    IconCss = fieldProp["IconCss"] != null ? (string)DataUtil.GetObject(fieldProp["IconCss"].ToString(), fieldData) : null,
                    Id = idValue,
                    IsChecked = fieldProp["IsChecked"] != null ? (bool?)DataUtil.GetObject(fieldProp["IsChecked"]?.ToString(), fieldData) : null,
                    Text = (string)DataUtil.GetObject(fieldProp["Text"].ToString(), fieldData),
                    Tooltip = fieldProp["Tooltip"] != null ? (string)DataUtil.GetObject(fieldProp["Tooltip"]?.ToString(), fieldData) : null,
                    HasChildren = fieldProp["HasChildren"] != null ? Convert.ToBoolean(DataUtil.GetObject(fieldProp["HasChildren"]?.ToString(), fieldData), CultureInfo.InvariantCulture) : false,
                    Expanded = fieldProp["Expanded"] != null ? (DataUtil.GetObject(fieldProp["Expanded"]?.ToString(), fieldData) != null ? (bool)DataUtil.GetObject(fieldProp["Expanded"].ToString(), fieldData) : false) : false,
                    ImageUrl = fieldProp["ImageUrl"] != null ? (string)DataUtil.GetObject(fieldProp["ImageUrl"]?.ToString(), fieldData) : null,
                    Selected = fieldProp["Selected"] != null ? DataUtil.GetObject(fieldProp["Selected"]?.ToString(), fieldData) != null ? (bool)DataUtil.GetObject(fieldProp["Selected"].ToString(), fieldData) : false : false,
                    Url = fieldProp["Url"] != null ? (string)DataUtil.GetObject(fieldProp["Url"]?.ToString(), fieldData) : null
                }
                : new FieldsValueMapping<List<TValue>>();
            }
            catch (NullReferenceException e)
            {
                throw new InvalidCastException("Invalid mapping in List field settings. Please provide valid fields mapping for your Datasource.", e);
            }
        }

        /// <summary>
        /// Returns TreeItemCreatedArgs for a list item for which TreeItemCreating event invoked.
        /// </summary>
        /// <param name="item">"Specifies the item".</param>
        /// <param name="mappedData">"Specifies the mappedData".</param>
        /// <param name="nodeLevel">"Specifies the nodeLevel".</param>
        /// <returns>"Task".</returns>
        protected TreeItemCreatedArgs<TValue> InvokeTreeItemCreating(TValue item, FieldsValueMapping<List<TValue>> mappedData = null, int nodeLevel = 1)
        {
            try
            {
                TreeItemCreatingArgs = new TreeItemCreatedArgs<TValue> { ItemData = item, Item = null, Text = mappedData?.Text ?? item?.ToString(), TreeOptions = new TreeOptions<TValue>(), NodeLevel = nodeLevel, Options = ListModel };
                TreeItemCreating?.Invoke(TreeItemCreatingArgs);
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Exception", e);
            }

            return TreeItemCreatingArgs;
        }

        /// <summary>
        /// Maps the default setting for the list generated.
        /// </summary>
        /// <param name="options">"Specifies the options field".</param>
        /// <param name="isField">"Specifies the isField attribute".</param>
        protected void MapSettings(ListModel options, bool isField)
        {
            TreeViewFieldsSettings<TValue> parentField = TreeParent.TreeViewFields;
            fieldsMap = ListBasePropertyMapper<FieldsMapping>.PropertyMapper(options?.Fields, fieldsMap)[1];
            fieldProp.Clear();
            SfBaseUtils.UpdateDictionary(nameof(parentField.Child), isField ? fieldsMap.Child : parentField.Child, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.Expanded), isField ? fieldsMap.Expanded : parentField.Expanded, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.HasChildren), isField ? fieldsMap.HasChildren : parentField.HasChildren, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.HtmlAttributes), isField ? fieldsMap.HtmlAttributes : parentField.HtmlAttributes, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.ImageUrl), isField ? fieldsMap.ImageUrl : parentField.ImageUrl, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.IconCss), isField ? fieldsMap.IconCss : parentField.IconCss, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.Id), isField ? fieldsMap.Id : parentField.Id, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.IsChecked), isField ? fieldsMap.IsChecked : parentField.IsChecked, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.Selected), parentField.Selected ?? fieldsMap.Selected, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.Text), isField ? fieldsMap.Text : parentField.Text, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(parentField.Tooltip), isField ? fieldsMap.Tooltip : parentField.Tooltip, fieldProp);
            SfBaseUtils.UpdateDictionary(nameof(fieldsMap.Url), isField ? fieldsMap.Url : parentField.NavigateUrl, fieldProp);
        }

        /// <summary>
        /// ListBase Property.
        /// </summary>
        /// <typeparam name="T">"T".</typeparam>
        internal static class ListBasePropertyMapper<T>
        {
            /// <summary>
            /// Specifies the property mapper.
            /// </summary>
            /// <param name="customizedProp">"Specifes the customised prop".</param>
            /// <param name="mappedProp">"Specifies the mapped property".</param>
            /// <returns>"Task".</returns>
            public static List<T> PropertyMapper(T customizedProp, T mappedProp)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                Type customizedType = customizedProp.GetType();
                foreach (PropertyInfo property in properties)
                {
                    PropertyInfo mappedProperty = customizedType.GetProperty(property.Name);
                    if (mappedProperty != null)
                    {
                        object mappedPropVal = mappedProperty.GetValue(customizedProp);
                        mappedProperty.SetValue(mappedProp, mappedPropVal);
                    }
                }

                return new List<T> { customizedProp, mappedProp };
            }
        }

        /// <summary>
        /// Sets and returns default listbase properties values to the listbase options.
        /// </summary>
#pragma warning disable CA1024 // Use properties where appropriate
        public ListModel GetDefaultListOptions()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            return new ListModel { Fields = new FieldsMapping() };
        }
    }
}