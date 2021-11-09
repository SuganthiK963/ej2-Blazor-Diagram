using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <summary>
    /// Specifies the complex list items.
    /// </summary>
    /// <typeparam name="TValue">"TypeParam".</typeparam>
    public partial class ComplexListItems<TValue> : TreeViewListItems<TValue>
    {
        internal const string LISTITEM = "e-list-item";
        internal const string NAVIGABLE = "e-navigable";
        internal const string ACTIVE = "e-active";
        internal const string COMPLEXLISTITEMSHASCHILD = "e-has-child";
        internal const string DISABLE = "e-disable";
        internal const string NODELEVEL = "e-level-";

        private bool IsTemplate { get; set; }

        /// <summary>
        /// Specifies the TreeOption field values.
        /// </summary>
        [Parameter]
        public TreeOptions<TValue> TreeOptions { get; set; }

        /// <summary>
        /// Specifies the treeview node has child or not.
        /// </summary>
        [Parameter]
        public bool HasChild { get; set; }

        /// <summary>
        /// Specifies the Treeview mapped data values.
        /// </summary>
        [Parameter]
        public new FieldsValueMapping<List<TValue>> MappedData { get; set; }

        /// <summary>
        /// Specifies the Treeview node data id.
        /// </summary>
        [Parameter]
        public string RandomID { get; set; }

        [CascadingParameter]
        internal SfTreeView<TValue> Parent { get; set; }

        /// <summary>
        /// Specifies the index position of Treeview node.
        /// </summary>
        [Parameter]
        public int Index { get; set; }

        /// <summary>
        /// Specifies the treeview list base option model.
        /// </summary>
        [Parameter]
        public ListModel ListModel { get; set; }

        /// <summary>
        /// Specifies the datasource of list element.
        /// </summary>
        [Parameter]
        public TValue ListData { get; set; }

        /// <summary>
        /// Specifies the tree node level of treeview nodes.
        /// </summary>
        [Parameter]
        public new int TreeNodeLevel { get; set; } = 1;

        // Returns the list item classes for the list.
        private string GetListItemClass()
        {
            string classNames = string.Empty;
            classNames = SfBaseUtils.AddClass(classNames, LISTITEM);
            classNames = SfBaseUtils.AddClass(classNames, NODELEVEL + TreeNodeLevel.ToString(CultureInfo.CurrentCulture));
            if (MappedData != null)
            {
                if (MappedData.HtmlAttributes != null && MappedData.HtmlAttributes.ContainsKey("class"))
                {
                    classNames = SfBaseUtils.AddClass(classNames, $"{MappedData.HtmlAttributes["class"]}");
                }

                if (MappedData.Url != null && ListModel.ItemNavigable)
                {
                    classNames = SfBaseUtils.AddClass(classNames, NAVIGABLE);
                }
            }

            if (TreeOptions != null)
            {
                SfBaseUtils.RemoveClass(classNames, ACTIVE);
                if (TreeOptions.IsSelected)
                {
                    classNames = SfBaseUtils.AddClass(classNames, ACTIVE);
                }

                if (TreeOptions.FullRowNavigate)
                {
                    classNames = SfBaseUtils.AddClass(classNames, NAVIGABLE);
                }

                if (TreeOptions.ChildData != null)
                {
                    classNames = SfBaseUtils.AddClass(classNames, COMPLEXLISTITEMSHASCHILD);
                }

                if (TreeOptions.IsDisabled)
                {
                    classNames = SfBaseUtils.AddClass(classNames, DISABLE);
                }
            }

            return classNames;
        }

        // Returns the list item attributes for the list.
        private IDictionary<string, object> GetAttributes()
        {
            string itemId = MappedData?.Id;
            string id = itemId ?? RandomID + "-" + Index;
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("class", GetListItemClass(), attributes);
            SfBaseUtils.UpdateDictionary("role", "treeitem", attributes);
            SfBaseUtils.UpdateDictionary("data-uid", id, attributes);
            if (TreeOptions != null)
            {
                SfBaseUtils.UpdateDictionary("aria-level", TreeNodeLevel.ToString(CultureInfo.CurrentCulture), attributes);
                if(TreeOptions.ChildData != null || MappedData.HasChildren || MappedData.Child?.ToList().Count > 0) {
                    SfBaseUtils.UpdateDictionary("aria-expanded", TreeOptions.IsExpanded ? "true" : "false", attributes);
                }
                if (TreeOptions.IsSelected || Parent.AllowMultiSelection) {
                    SfBaseUtils.UpdateDictionary("aria-selected", TreeOptions.IsSelected ? "true" : "false", attributes);
                }
            }

            if (MappedData.HtmlAttributes != null)
            {
                foreach (string key in MappedData.HtmlAttributes.Keys)
                {
                    if (key != "class")
                    {
                        SfBaseUtils.UpdateDictionary(key, MappedData.HtmlAttributes[key].ToString(), attributes);
                    }
                }
            }

            if (MappedData.Tooltip != null)
            {
                SfBaseUtils.UpdateDictionary("title", MappedData.Tooltip, attributes);
            }

            return attributes;
        }

        // Returns the classes for the tree element of the generating list.
        private string GetContainerClass()
        {
            string listClasses = "e-text-content";
            if (TreeOptions.IconClass != null || MappedData.HasChildren || (MappedData.Child != null && MappedData.Child.ToList().Count > 0))
            {
                listClasses = SfBaseUtils.AddClass(listClasses, "e-icon-wrapper");
            }

            return listClasses;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override Task OnInitializedAsync()
        {
            IsTemplate = TreeOptions != null && TreeOptions.NodeTemplate != null;
            return base.OnInitializedAsync();
        }

        internal override void ComponentDispose()
        {
            if (TreeOptions?.ChildData != null)
            {
                TreeOptions.ChildData = null;
            }

            TreeOptions = null;
            MappedData = null;
            ListModel = null;
        }
    }
}