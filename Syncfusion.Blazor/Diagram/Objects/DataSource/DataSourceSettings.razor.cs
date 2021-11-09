using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// DataSourceSettings is used to specify the data source and defines how the parent and child relationship will be generated in the layout. It is applicable only when the layout is used. 
    /// </summary>
    /// <example>
    /// <code lang="Razor">
    /// <![CDATA[
    /// <SfDiagramComponent Width = "1000px" Height="1000px">
    /// <DataSourceSettings @bind-DataSource="@data" ID="@Id" ParentID="@ParentId" SymbolMapping="@doBinding"></DataSourceSettings>
    /// <Layout Type = "LayoutType" >
    /// </ Layout >
    /// </ SfDiagramComponent >
    /// @code {
    /// public object data { get; set; }
    /// public string Id;
    /// public string ParentId;
    /// public LayoutType LayoutType = LayoutType.HierarchicalTree;
    /// protected override void OnInitialized()
    /// {
    ///  Id = "Name";
    ///  ParentId = "Category";
    ///  data = Data1;
    /// }
    /// //bind the external data with the node
    /// private Node doBinding(Node node, object data)
    /// {
    ///  HierarchicalDetails hierarchicalData = data as HierarchicalDetails;
    ///  node.Annotations = new DiagramObjectCollection<ShapeAnnotation>()
    /// {
    ///  new ShapeAnnotation()
    /// {
    ///  ID = "annotation"+node.ID,
    ///  Content = hierarchicalData.Name,
    /// },
    /// };
    /// return node;
    /// }
    /// public List<HierarchicalDetails> Data1 = new List<HierarchicalDetails>()
    /// {
    ///  new HierarchicalDetails(){ Name ="Diagram",FillColor="#916DAF"},
    ///  new HierarchicalDetails(){ Name ="Layout", Category="Diagram"},
    ///  new HierarchicalDetails(){ Name ="Tree Layout",Category="Layout"},
    ///  new HierarchicalDetails(){ Name ="Organizational Chart", Category="Layout"}
    /// };
    /// public class HierarchicalDetails
    /// {
    ///  public string Name { get; set; }
    ///  public string Category { get; set; }
    ///  public string FillColor { get; set; }
    /// }
    /// }
    /// ]]>
    /// </code>
    /// </example>

    public partial class DataSourceSettings : SfDataBoundComponent
    {
        private bool refreshData;
        private object dataSource;
        private string id;
        private string parentId;
        private string root;

        [CascadingParameter]
        [JsonIgnore]
        internal SfDiagramComponent Parent { get; set; }
        /// <summary>
        /// Gets or sets the child content that the layout is displayed for.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the data source changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<object> DataSourceChanged { get; set; }
        /// <summary>
        /// Gets or sets the data source that the layout is displayed for.
        /// </summary>
        [Parameter]
        [JsonPropertyName("dataSource")]
        public object DataSource { get; set; }

        /// <summary>
        /// Defines the property name that will be mapped to each node id from the data source record.
        /// </summary>
        [Parameter]
        [JsonPropertyName("id")]
        public string ID { get; set; }
        /// <summary>
        /// Defines the property name that will be used to find the parent and child relationship between the nodes from the data source record.
        /// </summary>
        [Parameter]
        [JsonPropertyName("parentId")]
        public string ParentID { get; set; }
        /// <summary>
        /// Gets or sets the root (primary) node of the layout populated from the data source.
        /// </summary>
        [Parameter]
        [JsonPropertyName("root")]
        public string Root { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the id value changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<string> IDChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the parent id value changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<string> ParentIDChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the root value changes.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<string> RootChanged { get; set; }

        /// <summary>
        /// Returns the customized node information. It is a call-back method which is triggered for each node when initializing the layout.
        /// </summary>
        /// <remarks>
        /// <table>
        /// <tr>
        /// <td style = "border:none">For more details, refer <see href="https://blazor.syncfusion.com/documentation/diagram-component/layout/organizational-chart">Organizational Chart</see></td>
        /// </tr>
        /// </table>
        /// </remarks>

        [Parameter]
        [JsonIgnore]
        public Func<Node, object, Node> SymbolBinding { get; set; }
        /// <summary>
        /// Gets or sets the query that is displayed for.
        /// </summary>
        [Parameter]
        [JsonPropertyName("query")]
        public Query Query { get; set; }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            dataSource = DataSource;
            await UpdateLiveData();
        }
        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering, otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && refreshData)
            {
                refreshData = false;
                PropertyChanges.Clear();
                await Parent.DiagramContent.RefreshDataSource();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            this.DataSource = this.dataSource = BaseUtil.UpdateDictionary(nameof(DataSource), dataSource, DataSource, PropertyChanges);
            this.ID = this.id = BaseUtil.UpdateDictionary(nameof(ID), id, ID, PropertyChanges);
            this.ParentID = this.parentId = BaseUtil.UpdateDictionary(nameof(ParentID), parentId, ParentID, PropertyChanges);
            this.Root = this.root = BaseUtil.UpdateDictionary(nameof(Root), root, Root, PropertyChanges);
            if (DataSource != null)
            {
                if (PropertyChanges.ContainsKey(nameof(DataSource)))
                {
                    await UpdateLiveData();
                    refreshData = true;
                }
            }
            if (!this.Parent.FirstRender && PropertyChanges.Any() && DataSource == null)
            {
                this.Parent.ClearObjects();
            }
        }
        private async Task UpdateLiveData()
        {
            await UpdatedLocalData();
            if (DataManager != null)
            {
                DataManager.DataAdaptor.SetRunSyncOnce(true);
                await this.DataManager.ExecuteQuery<object>(new Query());
            }
            Parent.UpdateDataSourceSetting(this);
        }
        internal async Task<IEnumerable<object>> UpdatedLocalData()
        {
            SetDataManager<object>(DataSource);
            DataManager.DataAdaptor.SetRunSyncOnce(true);
            IEnumerable<object> data = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(new Query());
            return data;
        }

        internal async Task PropertyUpdate(DataSourceSettings dataSourceSettings)
        {
            DataSource = await SfBaseUtils.UpdateProperty(dataSourceSettings.DataSource, DataSource, DataSourceChanged, null, null);
            ID = await SfBaseUtils.UpdateProperty(dataSourceSettings.ID, ID, IDChanged, null, null);
            ParentID = await SfBaseUtils.UpdateProperty(dataSourceSettings.ParentID, ParentID, ParentIDChanged, null, null);
            Root = await SfBaseUtils.UpdateProperty(dataSourceSettings.Root, Root, RootChanged, null, null);
            //CrudAction = await SfBaseUtils.UpdateProperty(dataSourceSettings.CrudAction, CrudAction, CrudActionChanged, null, null);
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            if (Parent != null)
            {
                Parent = null;
            }
            if (ChildContent != null)
            {
                ChildContent = null;
            }
            if (DataSource != null)
            {
                DataSource = null;
            }
            if (dataSource != null)
            {
                dataSource = null;
            }
            if (ID != null)
            {
                ID = null;
            }
            if (id != null)
            {
                id = null;
            }
            if (ParentID != null)
            {
                ParentID = null;
            }
            if (parentId != null)
            {
                parentId = null;
            }
            if (Root != null)
            {
                Root = null;
            }
            if (root != null)
            {
                root = null;
            }
        }
    }
}
