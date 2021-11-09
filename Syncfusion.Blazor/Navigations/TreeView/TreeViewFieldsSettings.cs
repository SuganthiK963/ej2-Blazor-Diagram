using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;
using System.Linq;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    ///  A class used for configuring the TreeView fields setting properties.
    /// </summary>
    /// <typeparam name="TValue">"Specifies the TValue parameter".</typeparam>
    public partial class TreeViewFieldsSettings<TValue> : TreeViewCommonFieldsSettings<TValue>
    {
        [CascadingParameter]
        private SfTreeView<TValue> Parent { get; set; }

        /// <summary>
        /// Specifies the child content.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Invokes when data source changes.
        /// </summary>
        [Parameter]
        public EventCallback<IEnumerable<TValue>> DataSourceChanged { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("fields", this);
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            TreeViewCommonFieldsSettingsdataSource = NotifyPropertyChanges(nameof(DataSource), DataSource?.ToList(), TreeViewCommonFieldsSettingsdataSource?.ToList());
            if (PropertyChanges.ContainsKey(nameof(DataSource))) { Parent.IsDataSourceChanged = true; }            
            if (!Parent.IsDataSourceChanged && (Parent.CheckedNodesChanged.HasDelegate || Parent.ExpandedNodesChanged.HasDelegate))
            {
                return;
            }

            if (Parent.ListReference != null)
            {
                Parent.UpdateChildProperties("fields", this);
                List<TValue> datasource = new List<TValue>();
                if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataSource != null)
                {
                    datasource = ((Parent.IsNumberTypeId || Parent.IsNodeDropped || Parent.ListReference.IsRefreshNode || Parent.AllowDragAndDrop) && !PropertyChanges.ContainsKey(nameof(DataSource))) ? Parent.ListReference.DataSource.ToList() : Parent.TreeViewFields.DataSource.ToList();
                }
                else
                {
                    datasource = null;
                }

                await Parent.UpdateData(datasource, PropertyChanges.ContainsKey(nameof(DataSource)));
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}