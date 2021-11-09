using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the data sources that are bound together with the objects in the diagram.
    /// </summary>
    public class DiagramDataBinding
    {
        internal DiagramObjectCollection<Node> nodes;
        internal DiagramObjectCollection<Connector> connectors;
        private Dictionary<object, object> dataTable = new Dictionary<object, object>();
        internal void InitData(DataSourceSettings data, SfDiagramComponent diagram)
        {
            object dataSource = data.DataManager.Data ?? data.DataManager.Json ?? data.DataSource;
            if (dataSource != null && ((IList)dataSource).Count == 0)
            {
                dataSource = data.DataManager.Data;
            }
            if (dataSource != null && ((IList)dataSource).Count > 0)
            {
                this.ApplyDataSource(data, dataSource as IList, diagram);
                _ = SfBaseUtils.InvokeEvent<object>(diagram.DataLoaded, null);
            }
        }

        internal async Task InitSource(DataSourceSettings data, SfDiagramComponent diagram)
        {
            if (data.DataManager is SfDataManager dataManager)
            {
                object result = await dataManager.ExecuteQuery<object>(data.Query?.Clone() ?? new Data.Query());
                this.ApplyDataSource(data, result as IList, diagram);
                await SfBaseUtils.InvokeEvent<object>(diagram.DataLoaded, null);
            }
        }
        private void ApplyDataSource(DataSourceSettings mapper, IList data, SfDiagramComponent diagram)
        {
            nodes = new DiagramObjectCollection<Node>();
            connectors = new DiagramObjectCollection<Connector>();
            this.dataTable = new Dictionary<object, object>();
            object firstNode = null; object firstNodeParentId = null;
            DataItems rootNodes = new DataItems();
            List<object> firstLevel = new List<object>();
            object nextLevel = null;

            if (data != null)
            {
                rootNodes.Items = new Dictionary<object, object>();
                for (int r = 0; r < data.Count; r++)
                {
                    object obj = SfBaseUtils.ChangeType((object)data[r], data[r].GetType());
                    JObject jObject = obj as JObject;
                    Type objType = obj.GetType();

                    PropertyInfo objIdProperty = objType.GetProperty(mapper.ID);
                    PropertyInfo objParentIdProperty = objType.GetProperty(mapper.ParentID);
                    object objIdValue = jObject != null ? jObject.GetValue(mapper.ID, StringComparison.InvariantCulture) : objIdProperty?.GetValue(obj);
                    object objParentIdValue = jObject != null ? jObject.GetValue(mapper.ParentID, StringComparison.InvariantCulture) : objParentIdProperty?.GetValue(obj);
                    if (objParentIdValue == null || !string.IsNullOrEmpty(objParentIdValue.ToString()) || objParentIdValue.GetType() != typeof(object))
                    {
                        if (objParentIdValue != null && !string.IsNullOrEmpty(objParentIdValue.ToString()) && !rootNodes.Items.ContainsKey(objParentIdValue))
                        {
                            DataItems childItem = new DataItems();
                            childItem.Items.Add(objIdValue, obj);
                            rootNodes.Items.Add(objParentIdValue, childItem);
                        }
                        else
                        {
                            if (objParentIdValue == null || string.IsNullOrEmpty(objParentIdValue.ToString()))
                            {
                                DataItems childItem = new DataItems();
                                childItem.Items.Add(objIdValue, obj);
                                rootNodes.Items.Add("undefined" + r.ToString(CultureInfo.InvariantCulture), childItem);
                            }
                            else
                                (rootNodes.Items[objParentIdValue] as DataItems).Items.Add(objIdValue, obj);
                        }
                    }
                    else
                    {
                        rootNodes = UpdateMultipleRootNodes(obj, objParentIdValue as IList, rootNodes);
                    }
                    if (object.ReferenceEquals(mapper.Root, objIdValue))
                    {
                        firstNode = obj;
                        firstNodeParentId = objParentIdValue;
                    }
                }

                if (firstNode != null)
                {
                    DataItems firstObj = new DataItems();
                    firstObj.Items.Add(firstNodeParentId, firstNode);
                    firstLevel.Add(firstObj);
                }
                else
                {
                    int i = 0;
                    foreach (object n in rootNodes.Items.Keys)
                    {
                        string val = "undefined" + i.ToString(CultureInfo.InvariantCulture);
                        if (n.ToString() == val)
                        {
                            firstLevel.Add(rootNodes.Items[n]);
                        }
                        i++;
                    }
                }
                for (int i = 0; i < firstLevel.Count; i++)
                {
                    for (int j = 0; j < (firstLevel[i] as DataItems).Items.Count; j++)
                    {
                        foreach (object n in (firstLevel[i] as DataItems).Items.Keys)
                        {
                            object item = (firstLevel[i] as DataItems).Items[n];
                            Node node = ApplyNodeTemplate(mapper, item, diagram);
                            nodes.Add(node);
                            diagram.NodeCreating.InvokeAsync(node);
                            PropertyInfo objIdProperty = item.GetType().GetProperty(mapper.ID);
                            object objIdValue = !(item is JObject jObject) ? objIdProperty?.GetValue(item) : jObject.GetValue(mapper.ID, StringComparison.InvariantCulture);
                            this.dataTable[objIdValue] = node;
                            if (data.Count > 1)
                                nextLevel = rootNodes.Items[objIdValue];
                            if (nextLevel != null)
                            {
                                this.RenderChildNodes(mapper, nextLevel as DataItems, node.ID, rootNodes, diagram);
                            }
                        }
                    }
                }
                diagram.NodeCollection = nodes;
                diagram.ConnectorCollection = connectors;
                diagram.NodeCollection.Parent = diagram;
                diagram.ConnectorCollection.Parent = diagram;
                this.dataTable = null;
            }
        }

        private void RenderChildNodes(DataSourceSettings mapper, DataItems parent, string value, DataItems rootNodes, SfDiagramComponent diagram)
        {
            foreach (object key in parent.Items.Keys)
            {
                object child = parent.Items[key];
                PropertyInfo childIdPropertyInfo = child.GetType().GetProperty(mapper.ID);
                object childIdValue = child is JObject childObj ? childObj.GetValue(mapper.ID, StringComparison.InvariantCulture) : childIdPropertyInfo?.GetValue(child);
                Node node = ApplyNodeTemplate(mapper, child, diagram);
                var canBreak = false;
                if (!this.CollectionContains(node, mapper.ID, mapper.ParentID))
                {
                    if (childIdValue != null) this.dataTable[childIdValue] = node;
                    nodes.Add(node);
                }
                else
                {
                    canBreak = true;
                }
                if (!ContainsConnector(connectors, value, node.ID))
                {
                    connectors.Add(ApplyConnectorTemplate(value, node.ID, diagram));
                }
                if (!canBreak)
                {
                    object nodeData = node.Data;
                    PropertyInfo nodeDataIdPropertyInfo = nodeData.GetType().GetProperty(mapper.ID);
                    object nodeDataIdValue = node.Data is JObject nodeJObj ? nodeJObj.GetValue(mapper.ID, StringComparison.InvariantCulture) : nodeDataIdPropertyInfo?.GetValue(nodeData);
                    object nextLevel = nodeDataIdValue != null && rootNodes.Items.ContainsKey(nodeDataIdValue) ? rootNodes.Items[nodeDataIdValue] : null;
                    if (nextLevel != null)
                    {
                        this.RenderChildNodes(mapper, nextLevel as DataItems, node.ID, rootNodes, diagram);
                    }
                }
            }
        }
        private static Connector ApplyConnectorTemplate(string sourceNodeId, string targetNodeId, SfDiagramComponent diagram)
        {
            Connector connModel = new Connector()
            {
                ID = BaseUtil.RandomId(),
                SourceID = sourceNodeId,
                TargetID = targetNodeId
            };
            diagram.ConnectorCreating.InvokeAsync(connModel);
            return connModel;
        }
        private static bool ContainsConnector(DiagramObjectCollection<Connector> connectors, string sourceNode, string targetNode)
        {
            if (!string.IsNullOrEmpty(sourceNode) && !string.IsNullOrEmpty(targetNode))
            {
                for (int i = 0; i < connectors.Count; i++)
                {
                    var connector = connectors[i];
                    if (connector != null && (connector.SourceID == sourceNode && connector.TargetID == targetNode))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CollectionContains(Node node, string id, string parentId)
        {
            JObject nodeObj = node.Data as JObject;
            PropertyInfo idNodeProperty = node.Data.GetType().GetProperty(id);
            PropertyInfo parentIdNodeProperty = node.Data.GetType().GetProperty(parentId);
            if (idNodeProperty != null)
            {
                object idNodeVal = nodeObj != null ? nodeObj.GetValue(id, StringComparison.InvariantCulture) : idNodeProperty.GetValue(node.Data);
                if (parentIdNodeProperty != null)
                {
                    object parentIdNodeVal = nodeObj != null ? nodeObj.GetValue(parentId, StringComparison.InvariantCulture) : parentIdNodeProperty.GetValue(node.Data);

                    Node obj = idNodeVal != null && dataTable.ContainsKey(idNodeVal) ? this.dataTable[idNodeVal] as Node : null;

                    if (obj == null)
                        return false;
                    JObject jObject = obj.Data as JObject;
                    PropertyInfo idObjProperty = obj.Data.GetType().GetProperty(id);
                    PropertyInfo parentIdObjProperty = obj.Data.GetType().GetProperty(parentId);
                    if (idObjProperty != null)
                    {
                        object idObjVal = jObject != null ? jObject.GetValue(id, StringComparison.InvariantCulture) : idObjProperty.GetValue(obj.Data);
                        if (parentIdObjProperty != null)
                        {
                            object parentIdObjVal = jObject != null ? jObject.GetValue(parentId, StringComparison.InvariantCulture) : parentIdObjProperty.GetValue(obj.Data);

                            return idObjVal == idNodeVal && parentIdObjVal == parentIdNodeVal;
                        }
                    }
                }
            }
            return false;
        }
        private static DataItems UpdateMultipleRootNodes(object obj, IList objParentId, DataItems rootNodes)
        {
            if (objParentId != null)
            {
                for (int i = 0; i < objParentId.Count; i++)
                {
                    string parent = objParentId[i].ToString();
                    if (rootNodes.Items[parent] != null)
                    {
                        rootNodes.Items.Add(parent, obj);
                    }
                    else
                    {
                        rootNodes.Items[parent] = obj;
                    }
                }
            }
            return rootNodes;
        }

        private static Node ApplyNodeTemplate(DataSourceSettings mapper, object item, SfDiagramComponent diagram)
        {
            object root = item;
            string id = BaseUtil.RandomId();
            Node nodeModel = new Node() { ID = id, Data = item };
            diagram.NodeCreating.InvokeAsync(nodeModel);
            if (mapper.SymbolBinding != null)
                nodeModel = mapper.SymbolBinding.Invoke(nodeModel, item);
            return nodeModel;
        }

        internal void Dispose()
        {
            if (dataTable != null)
            {
                dataTable.Clear();
                dataTable = null;
            }

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Dispose();
                    nodes[i] = null;
                }
                nodes.Clear();
                nodes = null;
            }

            if (connectors != null)
            {
                for (int i = 0; i < connectors.Count; i++)
                {
                    connectors[i].Dispose();
                    connectors[i] = null;
                }
                connectors.Clear();
                connectors = null;
            }
        }
    }

    internal class DataItems
    {
        internal Dictionary<object, object> Items = new Dictionary<object, object>();
    }
}
