using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <summary>
    ///  A class used for configuring the TreeView fields setting properties.
    /// </summary>
    /// <typeparam name="TValue">"Specifies the Tvalue parameter".</typeparam>
    public partial class TreeViewCommonFieldsSettings<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Binds the field settings for child nodes or mapping field for nested nodes objects that contain array of JSON objects.
        /// </summary>
        [Parameter]
        public string Child { get; set; }

        /// <summary>
        /// Specifies the array of JavaScript objects or instance of DataManager to populate the nodes.
        /// </summary>
        [Parameter]
        public IEnumerable<TValue> DataSource { get; set; }

        internal IEnumerable<TValue> TreeViewCommonFieldsSettingsdataSource { get; set; }

        /// <summary>
        /// Specifies the mapping field for expand state of the TreeView node.
        /// </summary>
        [Parameter]
        public string Expanded { get; set; }

        /// <summary>
        /// Specifies the mapping field for hasChildren to check whether a node has child nodes or not.
        /// </summary>
        [Parameter]
        public string HasChildren { get; set; }

        /// <summary>
        /// Specifies the mapping field for htmlAttributes to be added to the TreeView node.
        /// </summary>
        [Parameter]
        public string HtmlAttributes { get; set; }

        /// <summary>
        /// The DataManager is used to performing data operations in applications.
        /// It acts as an abstraction for using local data source - IEnumerable and remote data source - web services returning JSON or oData.
        /// </summary>
        public DataManager DataManager { get; set; }

        /// <summary>
        /// Specifies the mapping field for icon class of each TreeView node that will be added before the text.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies the ID field mapped in dataSource.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Specifies the mapping field for image URL of each TreeView node where image will be added before the text.
        /// </summary>
        [Parameter]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Specifies the field for checked state of the TreeView node.
        /// </summary>
        [Parameter]
        public string IsChecked { get; set; }

        /// <summary>
        /// Specifies the mapping field for navigateUrl to be added as hyper-link of the TreeView node.
        /// </summary>
        [Parameter]
        public string NavigateUrl { get; set; }

        /// <summary>
        /// Specifies the parent ID field mapped in dataSource.
        /// </summary>
        [Parameter]
        public string ParentID { get; set; }

        /// <summary>
        /// This paramter will execute along with data processing.
        /// </summary>
        [Parameter]
        public Query Query { get; set; }

        /// <summary>
        /// Specifies the mapping field for selected state of the TreeView node.
        /// </summary>
        [Parameter]
        public string Selected { get; set; }

        /// <summary>
        /// Specifies the table name used to fetch data from a specific table in the server.
        /// </summary>
        [Parameter]
        public string TableName { get; set; }

        /// <summary>
        /// Specifies the mapping field for text displayed as TreeView node's display text.
        /// </summary>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Specifies the mapping field for tooltip that will be displayed as hovering text of the TreeView node.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Specifies the child.
        /// </summary>
        public object Children { get; set; }

        /// <summary>
        /// Specifies the child property update.
        /// </summary>
        /// <param name="prop">"Specifies the prop details".</param>
        /// <param name="details">"Specifies the details".</param>
        public void UpdateChildProperties(string prop, object details)
        {
            if (!string.IsNullOrEmpty(prop))
            {
                Children = details;
            }

            Dictionary<string, object> child = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("children", details, child);
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            TreeViewCommonFieldsSettingsdataSource = DataSource;
            UpdateChildProperties("child", Child);
        }

        internal override void ComponentDispose()
        {
            Tooltip = null;
            Children = null;
            Query = null;
            DataManager = null;
        }
    }
}
