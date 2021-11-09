using System;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Data;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// SfTreeView component.
    /// </summary>
    public partial class SfTreeView<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Adds the collection of TreeView nodes based on target and index position. If target node is not specified,
        /// then the nodes are added as children of the given parentID or in the root level of TreeView.
        /// </summary>
        /// <param name="nodes">A list of nodes to be added to the TreeView.</param>
        /// <param name="target">Based on target nodes are added as children of the given parentID or in the root level of TreeView.</param>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public void AddNodes(List<TValue> nodes, string? target = null)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            ListReference.AddNodeData(nodes, target);
        }

        /// <summary>
        /// Instead of clicking on the TreeView node for editing, we can enable it by using
        /// `BeginEdit` property. On passing the node ID or element through this property, the edit textBox
        /// will be created for the particular node thus allowing us to edit it.
        /// </summary>
        /// <param name="node">Specifies ID of TreeView node.</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task BeginEdit(string node)
        {
            await InvokeMethod("sfBlazor.TreeView.beginEdit", new object[] { Element, node });
        }

        /// <summary>
        /// Instead of clicking on the TreeView node for editing, we can enable it by using
        /// `BeginEdit` property. On passing the node ID or element through this property, the edit textBox
        /// will be created for the particular node thus allowing us to edit it.
        /// </summary>
        /// <param name="node">Specifies ID of TreeView node.</param>
        /// <returns>"Task".</returns>
        public async Task BeginEditAsync(string node)
        {
            await BeginEdit(node);
        }

        /// <summary>
        /// Checks all the unchecked nodes. You can also check specific nodes by passing array of unchecked nodes
        /// as argument to this method.
        /// </summary>
        /// <param name="nodesId">"Specifies the NodeId".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CheckAll(string[] nodesId = null)
        {
            Dictionary<string, object> fieldProp = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary(TreeViewFields.Id, TreeViewFields.Id.ToString(), fieldProp);
            if (ShowCheckBox)
            {
                NodeCheckEventArgs nodeArgs = new NodeCheckEventArgs();
                nodeArgs.NodeData = new NodeData();
                if (nodesId == null)
                {
                    ListReference.GetAllNodeId(ListReference.DataSource);
                    AllCheckedNodes.Clear();
                }

                int nodeCount = nodesId == null ? ListReference.CheckNodeId.Count : nodesId.Length;
                for (int i = 0; i < nodeCount; i++)
                {
                    nodeArgs.NodeData.Id = nodesId != null ? nodesId[i] : ListReference.CheckNodeId[i];
                    nodeArgs.Action = CHECK;
                    if (nodeArgs.NodeData.Id != null)
                    {
                        await TriggerNodeCheckingEvent(nodeArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Checks all the unchecked nodes. You can also check specific nodes by passing array of unchecked nodes
        /// as argument to this method.
        /// </summary>
        /// <param name="nodesId">"Specifies the NodeId".</param>
        /// <returns>"Task".</returns>
        public async Task CheckAllAsync(string[] nodesId = null)
        {
            await CheckAll(nodesId);
        }

        /// <summary>
        /// This method clears the Expanded, Selected and Checked interaction states in the TreeView. This method is useful when changing the data source dynamically.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ClearState()
        {
            AllSelectedNodes?.Clear();
            AllCheckedNodes?.Clear();
            AllExpandedNodes = new List<string>();
            InternalExpandedNodes = new List<string>();
            SelectedNodes = TreeSelectedNodes = await SfBaseUtils.UpdateProperty<string[]>(null, TreeSelectedNodes, SelectedNodesChanged);
            CheckedNodes = TreeCheckedNodes = await SfBaseUtils.UpdateProperty<string[]>(null, TreeCheckedNodes, CheckedNodesChanged);
            ExpandedNodes = TreeExpandedNodes =  await SfBaseUtils.UpdateProperty<string[]>(null, TreeExpandedNodes, ExpandedNodesChanged);
            ListReference?.ListUpdated();
            IsClearStateCall = true;
        }

        /// <summary>
        /// This method clears the Expanded, Selected and Checked interaction states in the TreeView. This method is useful when changing the data source dynamically.
        /// </summary>
        public async Task ClearStateAsync()
        {
            await ClearState();
        }

        /// <summary>
        /// Collapses all the expanded TreeView nodes. You can collapse specific nodes by passing array of nodes as argument to this method.
        /// </summary>
        /// <param name="nodesId">"Specifes the NodeID".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CollapseAll(string[] nodesId = null)
        {
            List<string> expandedNodes = new List<string>();
            expandedNodes = nodesId == null ? InternalExpandedNodes : nodesId.ToList();
            for (int i = expandedNodes.Count - 1; i >= 0; i--)
            {
                NodeExpandEventArgs nodeArgs = new NodeExpandEventArgs();
                nodeArgs.NodeData = new NodeData();
                nodeArgs.NodeData.Id = expandedNodes[i];
                await TriggerNodeCollapsingEvent(nodeArgs);
            }
        }

        /// <summary>
        /// Collapses all the expanded TreeView nodes. You can collapse specific nodes by passing array of nodes as argument to this method.
        /// </summary>
        /// <param name="nodesId">"Specifes the NodeID".</param>
        /// <returns>"Task".</returns>
        public async Task CollapseAllAsync(string[] nodesId = null)
        {
            await CollapseAll(nodesId);
        }

        /// <summary>
        /// Disables the collection of nodes by passing the ID of nodes or node elements in the array.
        /// </summary>
        /// <param name="nodes">Specifies the array of TreeView nodes ID.</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task DisableNodes(string[] nodes)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (nodes != null)
            {
                int nodeLength = nodes.Length;
                if (nodeLength > 0)
                {
                    for (int i = 0; i < nodeLength; i++)
                    {
                        if (!AllDisabledNodes.Contains(nodes[i]))
                        {
                            AllDisabledNodes.Add(nodes[i]);
                        }
                    }
                }

                ListReference.ListUpdated();
            }
        }

        /// <summary>
        /// Disables the collection of nodes by passing the ID of nodes or node elements in the array.
        /// </summary>
        /// <param name="nodes">Specifies the array of TreeView nodes ID.</param>
        /// <returns>"Task".</returns>
        public async Task DisableNodesAsync(string[] nodes)
        {
            await DisableNodes(nodes);
        }

        /// <summary>
        /// Enables the collection of disabled nodes by passing the ID of nodes or node elements in the array.
        /// </summary>
        /// <param name="nodes">Specifies the array of TreeView nodes ID.</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task EnableNodes(string[] nodes)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (nodes != null)
            {
                int nodeLength = nodes.Length;
                if (nodeLength > 0)
                {
                    for (int i = 0; i < nodeLength; i++)
                    {
                        if (AllDisabledNodes.Contains(nodes[i]))
                        {
                            AllDisabledNodes.Remove(nodes[i]);
                        }
                    }
                }

                ListReference.ListUpdated();
            }
        }

        /// <summary>
        /// Enables the collection of disabled nodes by passing the ID of nodes or node elements in the array.
        /// </summary>
        /// <param name="nodes">Specifies the array of TreeView nodes ID.</param>
        /// <returns>"Task".</returns>
        public async Task EnableNodesAsync(string[] nodes)
        {
            await EnableNodes(nodes);
        }

        /// <summary>
        /// Ensures visibility of the TreeView node by using node ID or node element.
        /// When many TreeView nodes are present and we need to find a particular node, `EnsureVisible` property
        /// helps bring the node to visibility by expanding the TreeView and scrolling to the specific node.
        /// </summary>
        /// <param name="node">Specifies ID of TreeView node.</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task EnsureVisible(string node)
        {
            await InvokeMethod("sfBlazor.TreeView.ensureVisible", new object[] { Element, node });
        }

        /// <summary>
        /// Ensures visibility of the TreeView node by using node ID or node element.
        /// When many TreeView nodes are present and we need to find a particular node, `EnsureVisible` property
        /// helps bring the node to visibility by expanding the TreeView and scrolling to the specific node.
        /// </summary>
        /// <param name="node">Specifies ID of TreeView node.</param>
        /// <returns>"Task".</returns>
        public async Task EnsureVisibleAsync(string node)
        {
            await EnsureVisible(node);
        }

        /// <summary>
        /// Expands all the collapsed TreeView nodes. You can expand the specific nodes by passing the array of collapsed nodes.
        /// </summary>
        /// <param name="nodesId">"Specifies the NodeId".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ExpandAll(string[] nodesId = null)
        {
            Internal.ExpandEventArgs nodeArgs = new Internal.ExpandEventArgs();
            nodeArgs.NodeData = new NodeData();
            List<string> expandedNodes = new List<string>();
            List<string> expandingNodes = new List<string>();
            if (nodesId == null)
            {
                ListReference.GetAllNodeId(ListReference.DataSource);
                expandingNodes = ListReference.AllPrentNodeId;
            }
            else
            {
                expandingNodes = nodesId.ToList();
            }

            for (int i = 0; i < expandingNodes.Count; i++)
            {
                if (InternalExpandedNodes != null && !InternalExpandedNodes.Contains(expandingNodes[i]))
                {
                    expandedNodes.Add(expandingNodes[i]);
                }
            }

            int nodeCount = expandedNodes.Count;
            for (int i = 0; i < nodeCount; i++)
            {
                nodeArgs.NodeData.Id = expandedNodes[i];
                nodeArgs.IsLoaded = false;
                if (nodeArgs.NodeData.Id != null)
                {
                    TreeExpandAll = true;
                    await TriggerNodeExpandingEvent(nodeArgs);
                }
            }
        }

        /// <summary>
        /// Expands all the collapsed TreeView nodes. You can expand the specific nodes by passing the array of collapsed nodes.
        /// </summary>
        /// <param name="nodesId">"Specifies the NodeId".</param>
        /// <returns>"Task".</returns>
        public async Task ExpandAllAsync(string[] nodesId = null)
        {
            await ExpandAll(nodesId);
        }

        /// <summary>
        /// Gets all the checked nodes including child, whether it is loaded or not.
        /// </summary>
        /// <returns>"Task".</returns>
        [Obsolete("This method is deprecated and will no longer be used.")]
        public List<string> GetAllCheckedNodes()
        {
            Dictionary<string, object> checkedNode = new Dictionary<string, object>();
            checkedNode = AllCheckedNodes;
            return checkedNode.Keys.ToList();
        }

        /// <summary>
        /// Gets all the disabled nodes including child, whether it is loaded or not.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CA1024 // Use properties where appropriate
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<string>> GetDisabledNodes()
#pragma warning restore CA1024 // Use properties where appropriate
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return AllDisabledNodes;
        }

        /// <summary>
        /// Gets all the disabled nodes including child, whether it is loaded or not.
        /// </summary>
        /// <returns>"Task".</returns>
        public async Task<List<string>> GetDisabledNodesAsync()
        {
            return await GetDisabledNodes();
        }

        /// <summary>
        /// Get the node's data such as id, text, parentID, selected, isChecked, and expanded by passing the node element or it's ID.
        /// </summary>
        /// <param name="node">Specifies ID of TreeView node.</param>
        /// <returns>"Return TreeData".</returns>
        public NodeData GetNode(string node)
        {
            NodeData getNodeData = new NodeData();
            if (node != null)
            {
                getNodeData = ListReference.GetNodeDetails(node);
            }

            return getNodeData;
        }

        /// <summary>
        /// To get the updated data source of TreeView after performing some operation like drag and drop, node editing,
        /// node selecting/unSelecting, node expanding/collapsing, node checking/unChecking, adding and removing node.
        ///  If you pass the ID of TreeView node as arguments for this method then it will return the updated data source
        /// of the corresponding node otherwise it will return the entire updated data source of TreeView.
        ///  The updated data source also contains custom attributes if you specified in data source.
        /// </summary>
        /// <param name="node">Specifies ID of TreeView node.</param>
        /// <returns>"Return TreeData".</returns>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public List<TValue> GetTreeData(string? node = null)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            List<TValue> treeData = ListReference.GetTreeViewData(node);
            return treeData;
        }

        /// <summary>
        /// Removes the collection of TreeView nodes by passing the array of node details as argument to this method.
        /// </summary>
        /// <param name="nodes">Specifies the array of TreeView nodes ID.</param>
        public void RemoveNodes(string[] nodes)
        {
            ListReference.RemoveNodes(nodes);
        }

        /// <summary>
        /// Unchecks all the checked nodes. You can also uncheck the specific nodes by passing array of checked nodes
        /// as argument to this method.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <param name="nodesId">"Specifies the Id of the node".</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UncheckAll(string[] nodesId = null)
        {
            if (ShowCheckBox)
            {
                List<string> checkedNodes = AllCheckedNodes.Keys?.ToList();
                NodeCheckEventArgs nodeArgs = new NodeCheckEventArgs();
                int nodeCount = checkedNodes.Count;
                for (int i = 0; i < nodeCount; i++)
                {
                    nodeArgs.NodeData = new NodeData();
                    if (nodesId != null && nodesId.Contains(checkedNodes[i]))
                    {
                        nodeArgs.NodeData.Id = checkedNodes[i];
                        nodeArgs.Action = "uncheck";
                    }
                    else if (nodesId == null && AllCheckedNodes.ToList().Count > 0)
                    {
                        nodeArgs.NodeData.Id = checkedNodes[i];
                    }

                    if (nodeArgs.NodeData.Id != null)
                    {
                        await TriggerNodeCheckingEvent(nodeArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Unchecks all the checked nodes. You can also uncheck the specific nodes by passing array of checked nodes
        /// as argument to this method.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <param name="nodesId">"Specifies the Id of the node".</param>
        public async Task UncheckAllAsync(string[] nodesId = null)
        {
            await UncheckAll(nodesId);
        }

        /// <summary>
        /// Replaces the text of the TreeView node with the given text.
        /// </summary>
        /// <param name="target">Specifies ID of TreeView node.</param>
        /// <param name="newText">Specifies the new text of TreeView node.</param>
        /// <returns>"Task".</returns>
        [Obsolete("This method is deprecated and will no longer be used.")]
        public async Task UpdateNode(string target, string newText)
        {
            ListReference.UpdateSelfNodeText(target, newText);
            await TriggerNodeEdittedEvent(newText);
        }

        /// <summary>
        /// Applies all the pending property changes and render the component again.
        /// </summary>
        /// <returns>"Task".</returns>
        [Obsolete("This method is deprecated and will no longer be used.")]
        public async Task Refresh()
        {
            List<TValue> dataSource = ListReference.DataSource.ToList();
            await UpdateData(dataSource);
        }

        /// <summary>
        /// Replaces the text of the TreeView node with the given text.
        /// </summary>
        /// <param name="target">Specifies ID of TreeView node.</param>
        /// <param name="newData">Specifies the new Data of TreeView node.</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RefreshNode(string target, List<TValue> newData)
        {
            if (target != null && newData != null)
            {
                ListReference.IsRefreshNode = true;
                ListReference.RefreshTreeNodes(target, newData);
                await TriggerDataSourceChangedEvent();
                ListReference.IsRefreshNode = false;
            }
        }

        /// <summary>
        /// Replaces the text of the TreeView node with the given text.
        /// </summary>
        /// <param name="target">Specifies ID of TreeView node.</param>
        /// <param name="newData">Specifies the new Data of TreeView node.</param>
        /// <returns>"Task".</returns>
        public async Task RefreshNodeAsync(string target, List<TValue> newData)
        {
            await RefreshNode(target, newData);
        }

        /// <summary>
        /// Returns the route element reference of the component.
        /// </summary>
        /// <returns>"Task".</returns>
        [Obsolete("This method is deprecated and will no longer be used.")]
#pragma warning disable CA1024 // Use properties where appropriate
        public ElementReference GetRootElement()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            ElementReference ele = Element;
            return ele;
        }

        /// <summary>
        /// Replaces the text of the TreeView node with the given text.
        /// </summary>
        /// <param name="sourceNodes">Specifies the array of TreeView nodes ID.</param>
        /// <param name="target">Specifies ID of TreeView node.</param>
        /// <param name="index">Specifies the index to place the moved nodes in the target element.</param>
        /// <param name="preventTargetExpand">If set to true, the target parent node will be prevented from auto expanding.</param>
        /// <returns>"Task".</returns>
        [Obsolete("This method is deprecated and will no longer be used.")]
        public async Task MoveNodes(string[] sourceNodes, string target, int index, bool? preventTargetExpand)
        {
            if (sourceNodes != null)
            {
                if (target != null && sourceNodes.Length > 0)
                {
                    for (int i = 0; i < sourceNodes.Length; i++)
                    {
                        DropTreeArgs nodeArgs = new DropTreeArgs();
                        nodeArgs.DragLi = sourceNodes[i];
                        nodeArgs.DropLi = target;
                        await DropNodeAsChild(nodeArgs);
                    }
                }
            }
        }
    }
}
