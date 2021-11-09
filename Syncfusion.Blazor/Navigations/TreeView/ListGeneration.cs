using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Internal;
using System.Reflection;
using System.Runtime.Serialization;
using System.Globalization;

namespace Syncfusion.Blazor.Navigations.Internal
{
    /// <summary>
    /// An enum type that denotes the Treeview data source types.
    /// </summary>
    /// <exclude/>
    internal enum TreeViewDataType
    {
        /// <summary>
        /// Specifies 'SelfReferential' Data type.
        /// </summary>
        SelfReferential,

        /// <summary>
        /// Specifies 'Hierarchical' Data type.
        /// </summary>
        Hierarchical,

        /// <summary>
        /// Specifies 'RemoteData' Data type.
        /// </summary>
        RemoteData
    }

    /// <summary>
    /// List generation of TreeView component.
    /// </summary>
    /// <typeparam name="TValue">"TValue paramater".</typeparam>
    public partial class ListGeneration<TValue> : CreateListFromComplex<TValue>
    {
        internal const string TRUE = "true";
        internal const string ISTRUE = "True";
        internal const string FALSE = "false";
        internal const string INDETERMEDIATE = "intermediate";
        internal const string EXPANDED = "Expanded";
        internal const string SELECTED = "Selected";
        internal const string CHECK = "check";
        internal const string UNCHECK = "uncheck";
        internal const string CHECKED = "Checked";
        internal const string LISTGENERATIONDATASOURCE = "DataSource";
        internal const string ICONEXPANDCLASS = "e-icon-expandable";

        private string iconClass;

        internal List<TValue> ChildItems { get; set; } = new List<TValue>();

        internal IEnumerable<TValue> DataSource { get; set; } = new List<TValue>();

        internal List<string> AllPrentNodeId { get; set; } = new List<string>();

        internal List<string> CheckNodeId { get; set; } = new List<string>();

        [CascadingParameter]
        internal SfTreeView<TValue> Parent { get; set; }

        internal new IEnumerable<TValue> ListData { get; set; }

        internal List<string> RemoteExpandedValues { get; set; } = new List<string>() { };

        internal Query Query { get; set; }

        private bool ListGenerationIsExpanded { get; set; }

        private bool ListGenerationIsTextUpdated { get; set; }

        private int NodeLevel { get; set; } = 1;

        private IEnumerable<TValue> ListGenerationChild { get; set; }

        private DataManager ListGenerationDataManager { get; set; }

        private List<RemoteFieldsData> ListGenerationRemoteData { get; set; } = new List<RemoteFieldsData>();

        private TreeFieldsMapping ListGenerationFieldsMapper { get; set; }

        internal IEnumerable<TValue> ItemsData { get; set; }

        internal TreeViewDataType DataType { get; set; }

        internal bool MultiSelectFlag { get; set; } = true;

        internal bool IsRefreshNode { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DataSource = (IEnumerable<TValue>)DataUtil.GetObject(LISTGENERATIONDATASOURCE, Parent.TreeViewFields);

            MultiSelectFlag = true;
            List<TValue> data = DataSource?.ToList();
            if (data != null && data.Count > 0 && data[0].GetType()?.GetProperty(Parent?.TreeViewFields?.Id)?.GetValue(data[0])?.GetType() == typeof(int))
            {
                Parent.IsNumberTypeId = true;
            }
            ItemsData = GetSortedData(((TreeViewFieldsSettings<TValue>)Parent.TreeViewFields).DataSource?.ToList(), Parent.SortOrder.ToString(), Parent.TreeViewFields.Text);
            await IdentifyDataSource();
        }

        // Sorting operations for provided data source.
        internal static List<TValue> GetSortedData(List<TValue> dataSource, string sortOrder, string fieldValue)
        {
            return sortOrder != "None" && dataSource != null ? DataOperations.PerformSorting<TValue>(dataSource, new List<Sort>() { new Sort { Direction = sortOrder, Name = fieldValue } }).ToList() : dataSource;
        }

        // Identify the Binded data source type and update the selected and checked options values.
        internal async Task IdentifyDataSource(bool isUpdateChecked = false)
        {
            if (Parent?.TreeViewFields?.DataManager != null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(Parent?.TreeViewFields?.ParentID) || (!string.IsNullOrEmpty(Parent?.TreeViewFields?.HasChildren) && string.IsNullOrEmpty(Parent.TreeViewFields.Child)))
            {
                DataType = TreeViewDataType.SelfReferential;
                if (ItemsData != null)
                {
                    if (Parent.ExpandedNodes != null && Parent.ExpandedNodes.Length > 0)
                    {
                        Parent.InternalExpandedNodes = Parent.ExpandedNodes.ToList();
                        if (!Parent.IsCompleteltyRendered)
                        {
                            Parent.AllExpandedNodes = Parent.ExpandedNodes.ToList();
                        }
                    }
                    else
                    {
                        UpdateHierarchicalAndSelfProps(DataSource, EXPANDED);
                        Parent.InternalExpandedNodes = Parent.AllExpandedNodes.ToList();
                        await Parent.UpdateExpandedNodes();
                    }

                    TypeCheck(GroupingData(null, null), isUpdateChecked);
                }
            }
            else
            {
                DataType = TreeViewDataType.Hierarchical;
                if (Parent != null && Parent.ExpandedNodes != null)
                {
                    Parent.InternalExpandedNodes = Parent.ExpandedNodes.ToList();
                    if (!Parent.IsCompleteltyRendered)
                    {
                        Parent.AllExpandedNodes = Parent.ExpandedNodes.ToList();
                    }
                }
                else if (DataSource != null && DataSource.Any())
                {
                    UpdateHierarchicalAndSelfProps(DataSource, EXPANDED);
                }

                TypeCheck(ItemsData, isUpdateChecked);
            }
        }

        private async void TypeCheck(IEnumerable<TValue> itemData, bool isUpdateChecked = false)
        {
            if (ItemsData != null)
            {
                await UpdateSelectedNodes();
                await UpdateCheckedNodes(isUpdateChecked);
            }

            ListData = itemData;
        }

        internal void RefreshTreeNodes(string target, List<TValue> newData)
        {
            List<TValue> childs;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            if (DataType == TreeViewDataType.SelfReferential)
            {
                TValue item = GetRemovedSelfData(target, true);
                List<TValue> dataSource = DataSource.ToList();
                int pos = Parent.TreeViewFields.DataSource.IndexOf(item);
                dataSource.Insert(pos, newData[0]);
                ItemsData = (IEnumerable<TValue>)dataSource;
                DataSource = ItemsData;
            }
            else if (DataType == TreeViewDataType.Hierarchical)
            {
                bool refreshChild = false;
                childs = (List<TValue>)GetChild(fields.Child.ToString(), newData[0]);
                if (childs?.Count > 0)
                {
                    refreshChild = true;
                }

                GetHierarchicalData(target, DataSource.ToList());
                if (!refreshChild)
                {
                    newData[0].GetType().GetProperty(fields?.Child.ToString())?.SetValue(newData[0], GetChild(fields.Child.ToString(), Parent.TreeviewRemovedData));
                }

                GetRemovedHierData(target, DataSource.ToList(), true, default(TValue), newData[0], false);
            }

            ListUpdated();
        }

        // Update the TreeNode Text for Self Referential data source.
        internal void UpdateSelfNodeText(string nodeId, string newText)
        {
            bool offlineHier = false;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            bool offline = false;
            if (DataType == TreeViewDataType.RemoteData)
            {
                offline = Parent.TreeViewFields.DataManager?.Offline ?? false;
                if (offline && fields.Child != null)
                {
                    offlineHier = true;
                }
            }

            if (DataType == TreeViewDataType.SelfReferential || (DataType == TreeViewDataType.RemoteData && !offline) || (offline && !offlineHier))
            {
                List<TValue> updatedData = ItemsData?.ToList();
                int dataLength = updatedData.Count;
                for (int i = 0; i < dataLength; i++)
                {
                    UpdateFields(0);
                    if (UpdateNodeText(nodeId, updatedData[i], ListGenerationFieldsMapper, newText))
                    {
                        break;
                    }

                    if (DataType == TreeViewDataType.RemoteData && !ListGenerationIsTextUpdated)
                    {
                        UpdateRemoteNodeText(ListGenerationFieldsMapper, updatedData[i], nodeId, newText, 1);
                    }
                }

                Parent?.TreeViewFields?.GetType().GetProperty(LISTGENERATIONDATASOURCE)?.SetValue(fields, updatedData);
                ItemsData = (IEnumerable<TValue>)updatedData;
                DataSource = ItemsData;
            }
            else if (DataType == TreeViewDataType.Hierarchical || (offline && offlineHier))
            {
                List<TValue> data = DataSource?.ToList();
                UpdateFields(0);
                UpdatedHierarchicalText(nodeId, newText, data);
            }

            ListGenerationIsTextUpdated = false;
        }

        private void UpdateRemoteNodeText(TreeFieldsMapping fields, TValue updatedData, string nodeId, string newText, int level)
        {
            UpdateFields(level);
            string idAttrValue = GetAttrValue(fields?.Id, updatedData);
            IEnumerable<TValue> childData = (IEnumerable<TValue>)GetChildRemoteData(idAttrValue);
            List<TValue> updatedChildData = childData?.ToList();
            for (int j = 0; j < updatedChildData?.Count; j++)
            {
                if (UpdateNodeText(nodeId, updatedChildData[j], ListGenerationFieldsMapper, newText))
                {
                    break;
                }

                int childLevel = level + 1;
                UpdateRemoteNodeText(ListGenerationFieldsMapper, updatedChildData[j], nodeId, newText, childLevel);
            }
        }

        internal void DropNodeAsSiblingNodeHier(string dragLi, string dropLi, bool? pre, TValue removedData, IEnumerable<TValue> listData, TValue parentNode, bool isExternalDrop = false)
        {
            if (isExternalDrop)
            {
                List<TValue> dataList = listData?.ToList();
                dataList.Insert(dataList.Count, removedData);
                ListData = dataList;
                ItemsData = (IEnumerable<TValue>)ListData;
            }
            else
            {
                string idAttrValue;bool IsDropped = false;
                List<TValue> dataList = listData?.ToList();
                int dataLength = dataList.Count;
                IEnumerable<TValue> childs;
                for (int i = 0; i < dataLength; i++)
                {
                    idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, dataList[i]);
                    childs = GetChild(Parent.TreeViewFields.Child.ToString(), dataList[i]);
                    if (idAttrValue == dropLi)
                    {
                        if (pre != null && pre == true)
                        {
                            dataList.Insert(i, removedData);
                        }
                        else
                        {
                            dataList.Insert(i + 1, removedData);
                        }

                        if (parentNode != null)
                        {
                            PropertyInfo property = parentNode.GetType().GetProperty(Parent.TreeViewFields?.Child?.ToString());
                            FieldInfo field = parentNode.GetType().GetField(Parent.TreeViewFields?.Child?.ToString());
                            if (property != null)
                            {
                                property.SetValue(parentNode, dataList);
                            }
                            else if (field != null)
                            {
                                field.SetValue(parentNode, dataList);
                            }
                        }
                        IsDropped = true;
                        break;
                    }

                    if (childs != null)
                    {
                        DropNodeAsSiblingNodeHier(dragLi, dropLi, pre, removedData, childs, dataList[i]);
                    }
                }
                if (!IsDropped)
                {
                    dataList.Insert(dataList.Count, removedData);
                }
                ListData = dataList;
                ItemsData = (IEnumerable<TValue>)ListData;
            }
        }

        internal void DropNodeAsSiblingNode(string dropLi, bool? pre, TValue removedData, bool isExternalDrop = false)
        {
            string idAttrValue;
            List<TValue> dataList = DataSource?.ToList();
            int dataLength = dataList.Count;
            if (isExternalDrop)
            {
                dataList.Insert(dataList.Count, removedData);
                DataSource = dataList;
            }
            else
            {
                bool IsDropped = false;
                for (int i = 0; i < dataLength; i++)
                {
                    idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, dataList[i]);
                    if (idAttrValue == dropLi)
                    {
                        if (pre != null && pre == true)
                        {
                            dataList.Insert(i, removedData);
                        }
                        else
                        {
                            dataList.Insert(i + 1, removedData);
                        }
                        IsDropped = true;
                        DataSource = dataList;
                        break;
                    }
                }
                if (!IsDropped)
                {
                    dataList.Insert(dataList.Count, removedData);
                    DataSource = dataList;
                }
            }
        }

        internal TValue GetRemovedSelfData(string id, bool isRemoveCall = false, bool isHasChildUpdate = false)
        {
            TValue removedData = default(TValue);
            string idAttrValue;
            List<TValue> dataList = DataSource?.ToList();
            int dataLength = dataList.Count;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, dataList[i]);
                if (idAttrValue == id)
                {
                    removedData = dataList[i];
                    if (isRemoveCall)
                    {
                        dataList.RemoveAt(i);
                    }

                    if (isHasChildUpdate)
                    {
                        dataList[i].GetType().GetProperty(Parent.TreeViewFields?.HasChildren)?.SetValue(dataList[i], false);
                    }

                    DataSource = dataList;
                    break;
                }
            }

            return removedData;
        }

        internal void GetAndRemovedHierData(string id, List<TValue> dataSource, object parentNode)
        {
            int dataLength = dataSource.Count;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            IEnumerable<TValue> childs;
            if (dataSource == null)
            {
                Parent.TreeviewRemovedData = default(TValue);
            }

            for (int i = 0; i < dataLength; i++)
            {
                string idAttrValue;
                idAttrValue = GetAttrValue(fields.Id, dataSource[i]);
                childs = GetChild(fields.Child.ToString(), dataSource[i]);
                if (dataSource[i] != null && idAttrValue != null && idAttrValue == id)
                {
                    Parent.TreeviewRemovedData = dataSource[i];
                    dataSource.RemoveAt(i);
                    dataLength -= 1;
                    if (parentNode != null)
                    {
                        PropertyInfo property = parentNode.GetType().GetProperty(fields?.Child?.ToString());
                        FieldInfo field = parentNode.GetType().GetField(fields?.Child?.ToString());
                        if (property != null)
                        {
                            property.SetValue(parentNode, dataSource);
                        }
                        else
                        {
                            if (field != null)
                            {
                                field.SetValue(parentNode, dataSource);
                            }
                        }
                    }

                    break;
                }
                else if (childs != null)
                {
                    GetAndRemovedHierData(id, childs.ToList(), dataSource[i]);
                }
            }

            DataSource = dataSource;
            Parent.InternalData = DataSource.ToList();
            ItemsData = DataSource;
        }

        internal void GetRemovedHierData(string id, List<TValue> dataSource, bool isChild = false, TValue parent = default, TValue newData = default, bool isRemove = false)
        {
            int dataLength = dataSource.Count;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            IEnumerable<TValue> childs;
            if (dataSource == null)
            {
                Parent.TreeviewRemovedData = default(TValue);
            }

            for (int i = 0; i < dataLength; i++)
            {
                string idAttrValue;
                idAttrValue = GetAttrValue(fields.Id, dataSource[i]);
                childs = GetChild(fields.Child.ToString(), dataSource[i]);
                if (dataSource[i] != null && idAttrValue != null && idAttrValue == id)
                {
                    Parent.TreeviewRemovedData = dataSource[i];
                    dataSource.RemoveAt(i);
                    if (!isRemove)
                    {
                        dataSource.Insert(i, newData);
                    }

                    if (isChild && parent != null)
                    {
                        PropertyInfo prop = parent.GetType().GetProperty(fields?.Child);
                        FieldInfo field = parent.GetType().GetField(fields?.Child);
                        if (prop != null)
                        {
                            prop.SetValue(parent, dataSource);
                        }
                        else if (field != null)
                        {
                            field.SetValue(parent, dataSource);
                        }
                    }
                }
                else if (childs != null)
                {
                    GetRemovedHierData(id, childs.ToList(), true, dataSource[i], newData, isRemove);
                }
            }

            DataSource = dataSource;
        }

        private bool UpdateNodeText(string nodeId, TValue updatedData, TreeFieldsMapping fields, string newText)
        {
            string idAttrValue = GetAttrValue(fields.Id.ToString(), updatedData);
            if (idAttrValue == nodeId)
            {
                updatedData.GetType().GetProperty(fields?.Text.ToString())?.SetValue(updatedData, newText);
                ListGenerationIsTextUpdated = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        // Update the TreeNode Text for Hierarchical data source.
        private void UpdatedHierarchicalText(string nodeId, string newText, List<TValue> dataSource)
        {
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            List<TValue> updatedData = dataSource?.ToList();
            IEnumerable<TValue> childs;
            int updatedDataLength = updatedData.Count;
            for (int i = 0; i < updatedDataLength; i++)
            {
                childs = GetChild(fields.Child.ToString(), updatedData[i]);
                if (UpdateNodeText(nodeId, updatedData[i], ListGenerationFieldsMapper, newText))
                {
                    break;
                }

                if (childs != null)
                {
                    UpdatedHierarchicalText(nodeId, newText, (List<TValue>)childs);
                }
            }

            dataSource?.ToList().Concat(updatedData);
            Parent?.TreeViewFields?.GetType().GetProperty(LISTGENERATIONDATASOURCE)?.SetValue(fields, dataSource);
            ItemsData = (IEnumerable<TValue>)dataSource;
        }

        internal async Task UpdateCheckedNodes(bool isUpdateChecked = false)
        {
            if (Parent != null && Parent.ShowCheckBox)
            {
                if (Parent.CheckedNodes != null && Parent.CheckedNodes.Length > 0)
                {
                    for (int i = 0; i < Parent.CheckedNodes.Length; i++)
                    {
                        SfBaseUtils.UpdateDictionary(Parent.CheckedNodes[i], TRUE, Parent.AllCheckedNodes);
                    }

                    if (!isUpdateChecked)
                    {
                        await Parent.UpdateCheckedNodes();
                    }
                }

                if (DataType == TreeViewDataType.SelfReferential || DataType == TreeViewDataType.Hierarchical)
                {
                    if (Parent.CheckedNodes == null || Parent.CheckedNodes.Length == 0)
                    {
                        UpdateHierarchicalAndSelfProps(DataSource, CHECKED);
                    }

                    if (Parent.AutoCheck)
                    {
                        foreach (var item in Parent.AllCheckedNodes.Where(y => (string)y.Value == "intermediate").ToList())
                        {
                            Parent.AllCheckedNodes.Remove(item.Key);
                        }

                        List<string> checkedNodes = Parent.AllCheckedNodes.Where(x => x.Value == "true" as object).Select(x => x.Key).ToList();
                        UpdateChildCheckedNodes(checkedNodes, CHECK);
                        await Parent.UpdateCheckedNodes();
                        if (DataType == TreeViewDataType.SelfReferential)
                        {
                            UpdateSelfIntermediateState(checkedNodes);
                        }
                        else
                        {
                            UpdateCheckedDataFromDS(checkedNodes, CHECK);
                        }
                    }
                }
            }
        }

        // Update child checked state for Hierarchical data source.
        private void UpdateChildCheckedState(List<TValue> childItems, TValue treeData, List<string> checkedNodes, string action)
        {
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            string checkedParent = GetAttrVal(fields?.Id, treeData)?.ToString();
            string checkedChild;
            IEnumerable<TValue> childs;
            List<TValue> childData;
            int checkCount = 0;
            int intermediateCount = 0;
            int childLength = childItems.Count;
            for (int i = 0; i < childLength; i++)
            {
                checkedChild = GetAttrVal(fields?.Id, childItems[i])?.ToString();
                childs = GetChild(fields.Child.ToString(), childItems[i]);
                childData = childs != null ? childs.ToList() : null;
                if (childData != null && childData.Count > 0)
                {
                    UpdateChildCheckedState(childData, childItems[i], checkedNodes, action);
                }

                if (Parent.AllCheckedNodes.ContainsKey(checkedChild))
                {
                    if (Parent.AllCheckedNodes[checkedChild].ToString() == TRUE)
                    {
                        checkCount++;
                    }
                    else if (Parent.AllCheckedNodes[checkedChild].ToString() == INDETERMEDIATE)
                    {
                        intermediateCount++;
                    }
                }
            }

            UpdateIntermediateState(checkCount, childLength, checkedParent, intermediateCount);
        }

        private void UpdateIntermediateState(int checkCount, int? childLength, string checkedParent, int intermediateCount)
        {
            if (childLength != null && checkCount == childLength)
            {
                SfBaseUtils.UpdateDictionary(checkedParent, TRUE, Parent.AllCheckedNodes);
            }
            else if (checkCount == 0)
            {
                if (intermediateCount > 0)
                {
                    Parent.AllCheckedNodes[checkedParent] = INDETERMEDIATE;
                }
                else
                {
                    Parent.AllCheckedNodes.Remove(checkedParent);
                }
            }
            else
            {
                SfBaseUtils.UpdateDictionary(checkedParent, INDETERMEDIATE, Parent.AllCheckedNodes);
            }
        }

        // Based on checked nodes values to updated the checked tree node's list.
        internal void UpdateCheckedDataFromDS(List<string> checkedNodes, string action)
        {
            List<TValue> itemsdata = DataSource != null ? DataSource.ToList() : new List<TValue>();
            IEnumerable<TValue> childs;
            List<TValue> childItems;
            int dataLength = itemsdata.Count;
            for (int i = 0; i < dataLength; i++)
            {
                childs = Parent.TreeViewFields != null && Parent.TreeViewFields.Child != null ? GetChild(Parent.TreeViewFields.Child.ToString(), itemsdata[i]) : null;
                childItems = childs != null ? childs.ToList() : null;
                if (childItems != null && childItems.Count > 0)
                {
                    UpdateChildCheckedState(childItems, itemsdata[i], checkedNodes, action);
                }
            }
        }

        // Update intermediate state for self referential data source.
        internal void UpdateSelfIntermediateState(List<string> checkedNodes)
        {
            int checkCount;
            int intermediateCount;
            string parentID;
            List<TValue> siblingNodes;
            int checkedLength = checkedNodes.Count;
            for (int i = 0; i < checkedLength; i++)
            {
                checkCount = 0;
                intermediateCount = 0;
                parentID = GetParentId(checkedNodes[i]);
                if (parentID != null)
                {
                    siblingNodes = GroupingData(parentID, null);
                    int? sibilingLength = siblingNodes?.Count;
                    for (int j = 0; j < sibilingLength; j++)
                    {
                        string idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, siblingNodes[j]);
                        if (Parent.AllCheckedNodes.ContainsKey(idAttrValue))
                        {
                            if (Parent.AllCheckedNodes[idAttrValue].ToString() == TRUE)
                            {
                                ++checkCount;
                            }
                            else if (Parent.AllCheckedNodes[idAttrValue].ToString() == INDETERMEDIATE)
                            {
                                ++intermediateCount;
                            }
                        }
                    }

                    UpdateIntermediateState(checkCount, sibilingLength, parentID, intermediateCount);
                    List<string> parentCheckedNodes = new List<string>();
                    parentCheckedNodes.Add(parentID);
                    UpdateSelfIntermediateState(parentCheckedNodes);
                }
            }
        }

        // Get Parent node id for self referential data source.
        private string GetParentId(string id)
        {
            string parentID = null;
            string idAttrValue;
            List<TValue> dataList = ItemsData?.ToList();
            int dataLength = dataList.Count;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, dataList[i]);
                if (idAttrValue == id)
                {
                    parentID = GetAttrValue(Parent.TreeViewFields?.ParentID, dataList[i]);
                    break;
                }
            }

            return parentID;
        }

        // Update child checked state for Hierarchical data source.
        internal void UpdateHierarchicalChildCheckedNodes(List<string> checkedNodes, string action)
        {
            int checkedLength = checkedNodes.Count;
            for (int i = 0; i < checkedLength; i++)
            {
                GetHierarchicalChild(checkedNodes[i], DataSource);
                if (ChildItems != null && ChildItems.Count > 0)
                {
                    UpdateChildCheckedValues(ChildItems, action);
                }
            }
        }

        // Update child checked state values for tree nodes.
        private void UpdateChildCheckedValues(List<TValue> dataSource, string action)
        {
            string idAttrValue;
            IEnumerable<TValue> childs;
            int dataLength = dataSource.Count;
            for (int j = 0; j < dataLength; j++)
            {
                idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, dataSource[j]);
                if (Parent.AllCheckedNodes.ContainsKey(idAttrValue))
                {
                    if (action == UNCHECK)
                    {
                        Parent.AllCheckedNodes.Remove(idAttrValue);
                    }
                    else
                    {
                        Parent.AllCheckedNodes[idAttrValue] = TRUE;
                    }
                }
                else if (action == CHECK)
                {
                    SfBaseUtils.UpdateDictionary(idAttrValue, TRUE, Parent.AllCheckedNodes);
                }

                childs = GetChild(Parent.TreeViewFields?.Child.ToString(), dataSource[j]);
                if (childs != null)
                {
                    UpdateChildCheckedValues((List<TValue>)childs, action);
                }
            }
        }

        // Update child checked state values for tree nodes.
        internal void UpdateChildCheckedNodes(List<string> checkedNodes, string action)
        {
            List<string> childnodes = new List<string>();
            int nodesLength = checkedNodes.Count;
            for (int i = 0; i < nodesLength; i++)
            {
                if (DataType == TreeViewDataType.SelfReferential)
                {
                    ChildItems = null;
                    GetSelfChild(checkedNodes[i], ItemsData);
                }
                else if (DataType == TreeViewDataType.Hierarchical)
                {
                    GetHierarchicalChild(checkedNodes[i], DataSource);
                }

                if (ChildItems != null && ChildItems.Count > 0)
                {
                    string idAttrValue;
                    int itemLength = ChildItems.Count;
                    for (int j = 0; j < itemLength; j++)
                    {
                        if (ChildItems == null)
                        {
                            return;
                        }

                        idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, ChildItems[j]);
                        childnodes.Add(idAttrValue);
                        if (Parent.AllCheckedNodes.ContainsKey(idAttrValue))
                        {
                            if (action == UNCHECK)
                            {
                                Parent.AllCheckedNodes.Remove(idAttrValue);
                            }
                            else
                            {
                                Parent.AllCheckedNodes[idAttrValue] = TRUE;
                            }
                        }
                        else if (action == CHECK)
                        {
                            SfBaseUtils.UpdateDictionary(idAttrValue, TRUE, Parent.AllCheckedNodes);
                            if (DataType == TreeViewDataType.Hierarchical && Parent.CheckedNodesChanged.HasDelegate)
                            {
                                UpdateChildCheckedNodes(childnodes, action);
                            }
                        }
                    }

                    ChildItems = null;
                }
            }
        }

        // Update selected nodes for tree view.
        private async Task UpdateSelectedNodes()
        {
            if (Parent != null && Parent.SelectedNodes != null && Parent.SelectedNodes.Length > 0)
            {
                if (Parent.AllowMultiSelection)
                {
                    Parent.AllSelectedNodes = Parent.SelectedNodes.ToList();
                }
                else
                {
                    Parent.AllSelectedNodes.Clear();
                    if (Parent.AllSelectedNodes.IndexOf(Parent.SelectedNodes[0]) < 0)
                    {
                        Parent.AllSelectedNodes.Add(Parent.SelectedNodes[0]);
                    }

                    await Parent.UpdateSelectedNodes();
                }
            }
            else
            {
                if (DataType == TreeViewDataType.Hierarchical || DataType == TreeViewDataType.SelfReferential)
                {
                    UpdateHierarchicalAndSelfProps(DataSource, SELECTED);
                    if (Parent.AllSelectedNodes.Count > 0)
                    {
                        await Parent.UpdateSelectedNodes();
                    }
                }
            }
        }

        // Get the attribute value based on current data.
        internal static string GetAttrValue(string propertyName, TValue currentData)
        {
            return DataUtil.GetObject(propertyName, currentData)?.ToString();
        }

        // Update Checked node values based on checked attribute value.
        private static void UpdateCheckedProperty(string attrValue, string idValue, IDictionary<string, object> collections)
        {
            if (attrValue == ISTRUE && !collections.ContainsKey(idValue))
            {
                collections.Add(idValue, TRUE);
            }
        }

        internal static string GetAttrValues(string propertyName, TValue currentData)
        {
            return DataUtil.GetObject(propertyName, currentData)?.ToString();
        }

        // Update Checked, Selected, Expanded node values for Hierarchical data source.
        private void UpdateHierarchicalAndSelfProps(IEnumerable<TValue> dataSource, string attributes)
        {
            string idAttrValue;
            List<TValue> itemsData = dataSource?.ToList();
            IEnumerable<TValue> childs;
            List<TValue> childItems;
            TreeViewFieldsSettings<TValue> fields = Parent?.TreeViewFields;
            int? itemLength = itemsData?.Count;
            for (int i = 0; i < itemLength; i++)
            {
                idAttrValue = GetAttrValue(fields?.Id, itemsData[i]);
                UpdateHierarchicalAndSelfProps_Hierarchical(attributes, idAttrValue, itemsData[i], fields);
                if (DataType == TreeViewDataType.Hierarchical && fields?.Child != null)
                {
                    childs = GetChild(fields?.Child.ToString(), itemsData[i]);
                    childItems = childs != null ? childs.ToList() : null;
                    if (childItems?.Count > 0)
                    {
                        UpdateHierarchicalAndSelfProps(childItems, EXPANDED);
                        UpdateHierarchicalAndSelfProps(childItems, SELECTED);
                        if (Parent.CheckedNodes == null)
                        {
                            UpdateHierarchicalAndSelfProps(childItems, CHECKED);
                        }
                    }
                }
            }
        }

        private void UpdateHierarchicalAndSelfProps_Hierarchical(string attributes, string idAttrValue, TValue itemsData, TreeViewFieldsSettings<TValue> fields)
        {
            string attrValue;
            if (attributes == EXPANDED && fields?.Expanded != null)
            {
                attrValue = GetAttrValue(fields?.Expanded, itemsData);
                UpdatePropertyValues(attrValue, idAttrValue, Parent.AllExpandedNodes, EXPANDED);
            }
            else if (attributes == SELECTED && fields?.Selected != null)
            {
                attrValue = GetAttrValue(fields?.Selected, itemsData);
                UpdatePropertyValues(attrValue, idAttrValue, Parent.AllSelectedNodes, SELECTED);
            }
            else if (attributes == CHECKED && fields?.IsChecked != null)
            {
                attrValue = GetAttrValue(fields?.IsChecked, itemsData);
                UpdateCheckedProperty(attrValue, idAttrValue, Parent.AllCheckedNodes);
            }
        }

        // Update Selected node values based on multi select options.
        private void UpdatePropertyValues(string attrValue, string idValue, List<string> collections, string action)
        {
            if (attrValue == ISTRUE && !collections.Contains(idValue))
            {
                if (!Parent.AllowMultiSelection && collections.Count > 0 && action == SELECTED)
                {
                    collections.Clear();
                }

                collections.Add(idValue);
                if (action == EXPANDED && Parent != null && Parent.ExpandedNodes != null && Parent.ExpandedNodes.Contains(idValue) && !Parent.InternalExpandedNodes.Contains(idValue))
                {
                    Parent.InternalExpandedNodes.Add(idValue);
                }
            }
        }

        // Get child nodes for Hierarchical data source.
        internal void GetHierarchicalChild(string id, IEnumerable<TValue> dataSource)
        {
            List<TValue> data = dataSource.ToList();
            string idAttrValue;
            IEnumerable<TValue> childs;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            int? dataLength = data?.Count;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = GetAttrValue(fields.Id, data[i]);
                childs = fields.Child != null ? GetChild(fields.Child.ToString(), data[i]) : null;
                if (idAttrValue == id)
                {
                    ChildItems = childs != null ? childs.ToList() : null;
                    break;
                }

                if (childs != null)
                {
                    GetHierarchicalChild(id, (List<TValue>)childs);
                }
            }
        }

        // Get child nodes for Self Referential data source.
        private void GetSelfChild(string id, IEnumerable<TValue> dataSource)
        {
            if (ChildItems == null)
            {
                ChildItems = new List<TValue>();
            }

            List<TValue> itemsData = dataSource.ToList();
            List<TValue> childs;
            string idAttrValue;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            int? itemLength = itemsData?.Count;
            for (int i = 0; i < itemLength; i++)
            {
                idAttrValue = GetAttrValue(fields?.Id, itemsData[i]);
                if (idAttrValue == id)
                {
                    childs = GroupingData(idAttrValue, itemsData);
                    int? childLength = childs?.Count;
                    for (int j = 0; j < childLength; j++)
                    {
                        ChildItems.Add(childs[j]);
                        string parentID = DataUtil.GetObject(fields.Id, childs[j])?.ToString();
                        bool? hasChild = (bool?)DataUtil.GetObject(fields.HasChildren?.ToString(), childs[j]);
                        if (parentID != null && hasChild != null && hasChild == true)
                        {
                            GetSelfChild(parentID, itemsData);
                        }
                    }
                }
            }
        }

        internal async Task TriggerDataBoundEvent(List<TValue> dataSource)
        {
            DataBoundEventArgs<TValue> args = new DataBoundEventArgs<TValue>()
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Data = dataSource,
#pragma warning restore CS0618 // Type or member is obsolete
                Name = "DataBound"
            };
            await SfBaseUtils.InvokeEvent<DataBoundEventArgs<TValue>>(Parent.TreeviewEvents?.DataBound, args);
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">"First render".</param>
        /// <returns>"Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (Parent.TreeViewFields?.DataManager != null)
                {
                    DataType = TreeViewDataType.RemoteData;
                    Parent.AllExpandedNodes = Parent != null && Parent.ExpandedNodes != null ? Parent.ExpandedNodes.ToList() : new List<string>() { };
                    ListGenerationDataManager = Parent.TreeViewFields.DataManager;
                    UpdateFields(0);
                    Query = GettingQuery(ListGenerationFieldsMapper);
                    await GetDataManagerData();
                    if (Parent != null && (Parent.TreeViewFields?.HasChildren != null || Parent.TreeViewFields?.ParentID != null) && Parent.TreeViewFields?.Query == null)
                    {
                        Parent.InternalData = ListData.ToList();
                        DataSource = Parent.InternalData;
                        ItemsData = Parent.InternalData;
                        ListData = GroupingData(null, null);
                    }

                    StateHasChanged();
                }

                ChildItems = null;
                Parent.IsNodeRendered = true;
                if (Parent != null && Parent.EnablePersistence)
                {
                    TreePersistenceValues localStorageValue = await InvokeMethod<TreePersistenceValues>("window.localStorage.getItem", true, new object[] { Parent.ID });
                    if (localStorageValue == null)
                    {
                        await SetLocalStorage(Parent.ID, SerializeModel());
                    }
                    else
                    {
                        Parent.AllSelectedNodes = localStorageValue.SelectedNodes;
                        Parent.AllCheckedNodes = localStorageValue.CheckedNodes;
                        Parent.AllExpandedNodes = localStorageValue.ExpandedNodes;
                        Parent.CurrentExpandedNodes = Parent.AllExpandedNodes;
                    }

                    StateHasChanged();
                }

                if (Parent != null && Parent.ShowCheckBox)
                {
                    await UpdateCheckedNodes();
                }

                await TriggerDataBoundEvent(DataSource?.ToList());
            }

            if (DataType == TreeViewDataType.RemoteData && Parent.IsCompleteltyRendered && !SfBaseUtils.Equals(RemoteExpandedValues, Parent.ExpandedNodes?.ToList()))
            {
                Parent.InternalExpandedNodes = RemoteExpandedValues;
                await Parent.UpdateExpandedNodes();
            }
        }

        internal async Task GetDataManagerData()
        {
            try
            {
                object itemsData = (ListGenerationDataManager != null) ? await ListGenerationDataManager.ExecuteQuery<TValue>(Query) : null;
                List<TValue> dataSource = new List<TValue>();
                if (ListGenerationDataManager != null && itemsData != null)
                {
                    dataSource = GetDataSource(itemsData);
                    if (dataSource?.Count > 0)
                    {
                        dataSource = GetSortedData(dataSource.ToList(), Parent.SortOrder.ToString(), Parent.TreeViewFields.Text);
                        ListData = dataSource;
                        Parent.InternalData = ListData.ToList();
                        DataSource = Parent.InternalData;
                        ItemsData = Parent.InternalData;
                        if (!Parent.LoadOnDemand)
                        {
                            await GetChildRemoteData(dataSource, 1);
                        }

                        await EnsureExpandNodes(dataSource);
                    }
                }
            }
            catch (Exception exception)
            {
                await ThorwException(exception);
                throw new InvalidOperationException("Data operation failed", exception);
            }
        }

        private async Task ThorwException(Exception e)
        {
            FailureEventArgs args = new FailureEventArgs()
            {
                Error = e,
                Name = "OnActionFailure"
            };
            await SfBaseUtils.InvokeEvent<FailureEventArgs>(Parent.TreeviewEvents?.OnActionFailure, args);
        }

        // Get child nodes data values for Remote data binding.
        private async Task<IEnumerable<TValue>> GetRemoteDataChild()
        {
            try
            {
                object itemsData = (ListGenerationDataManager != null) ? await ListGenerationDataManager.ExecuteQuery<TValue>(Query) : null;
                List<TValue> dataSource = new List<TValue>();
                if (ListGenerationDataManager != null && itemsData != null)
                {
                    dataSource = GetDataSource(itemsData);
                }

                return (itemsData != null) ? dataSource : null;
            }
            catch (Exception exception)
            {
                await ThorwException(exception);
                return null;
                throw;
            }
        }

        private List<TValue> GetDataSource(object itemsData)
        {
            IEnumerable nodeData;
            if (Query != null && Query.IsCountRequired)
            {
                nodeData = (((DataResult)itemsData).Result == null) ? new List<object>() : ((DataResult)itemsData).Result;
            }
            else
            {
                nodeData = itemsData as IEnumerable;
            }

            return nodeData.Cast<TValue>().ToList();
        }

        // Update child nodes values for Remote data binding.
        private async Task GetChildRemoteData(List<TValue> dataSource, int level)
        {
            IEnumerable<TValue> childData;
            object id;
            int dataLength = dataSource.Count;
            for (int i = 0; i < dataLength; i++)
            {
                ListGenerationDataManager = null;
                UpdateFields(level - 1);
                id = (ListGenerationFieldsMapper.Id != null && dataSource[i] != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, dataSource[i]) : null;
                UpdateFields(level);
                Query = GettingQuery(ListGenerationFieldsMapper, id);
                childData = await GetRemoteDataChild();
                if (childData != null)
                {
                    List<TValue> child = (childData as IEnumerable).Cast<TValue>().ToList();
                    RemoteDataField(id.ToString(), ListGenerationFieldsMapper, child);
                    await RenderingRemoteChild(child, level);
                }

                UpdateFields(level);
            }
        }

        // Rendering remote data source child nodes.
        private async Task RenderingRemoteChild(List<TValue> datas, int level)
        {
            object id;
            IEnumerable<TValue> valueData;
            int dataLength = datas.Count;
            for (int j = 0; j < dataLength; j++)
            {
                ListGenerationDataManager = null;
                UpdateFields(level);
                id = (ListGenerationFieldsMapper.Id != null && datas[j] != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, datas[j]) : null;
                UpdateFields(level + 1);
                Query = GettingQuery(ListGenerationFieldsMapper, id);
                valueData = await GetRemoteDataChild();
                if (valueData != null)
                {
                    List<TValue> child = (valueData as IEnumerable).Cast<TValue>().ToList();
                    RemoteDataField(id.ToString(), ListGenerationFieldsMapper, child);
                    await GetChildRemoteData(child, level + 1);
                }
            }
        }

        private void RemoteDataField(string id, TreeFieldsMapping fields, List<TValue> childData)
        {
            ListGenerationRemoteData.Add(new RemoteFieldsData()
            {
                NodeId = id,
                FieldSettings = fields,
                RemoteData = childData
            });
        }

        /// <summary>
        /// Getting Query values for Remote data source.
        /// </summary>
        /// <param name="mapper">"Specfies the mapper field".</param>
        /// <param name="value">"Specifies the value".</param>
        /// <returns>"Task".</returns>
        protected virtual Query GettingQuery(TreeFieldsMapping mapper, object value = null)
        {
            List<string> columns = new List<string>();
            Query query = new Query();
            if (mapper?.Query == null)
            {
                query = new Query();
                List<string> properties = new List<string>()
            {
                "TableName", "Child", "Text", "Id", "ParentID", "NavigateUrl", "Expanded", "HasChildren", "HtmlAttributes", "ImageUrl", "IconCss", "Selected", "Tooltip"
            };
                int propertiesLength = properties.Count;
                for (int i = 0; i < propertiesLength; i++)
                {
                    if (properties[i] != LISTGENERATIONDATASOURCE && properties[i] != "TableName" && properties[i] != "Child" && properties[i] != null)
                    {
                        string property = (string)DataUtil.GetObject(properties[i], ListGenerationFieldsMapper);
                        if (property != null && !columns.Contains(property))
                        {
                            columns.Add(property);
                        }
                    }
                }

                query.Select(columns);
                if (ListGenerationFieldsMapper != null && ListGenerationFieldsMapper.TableName != null)
                {
                    query.From(mapper.TableName);
                }
            }
            else
            {
                query = GetQuery(mapper.Query);
            }

            if (value != null && mapper.ParentID != null)
            {
                value = GetIdType() ? int.Parse(value.ToString(), CultureInfo.InvariantCulture) : value;
                query.Where(new WhereFilter() { Field = mapper.ParentID, Operator = "equal", value = value });
            }

            return query;
        }

        private bool GetIdType()
        {
            object data = ListData.ElementAt(0);
            if (data != null)
            {
                object propertyValue = (ListGenerationFieldsMapper.Id != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, data) : null;
                return (propertyValue.GetType() == typeof(int)) ? true : false;
            }

            return false;
        }

        internal static Query CloneValue(Query value)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Query));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, value);
                ms.Seek(0, SeekOrigin.Begin);
                return (Query)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// Update Cloned query value for Remote data binding.
        /// </summary>
        /// <param name="query">"Specifies the QUERY parameter".</param>
        /// <returns>"Task".</returns>
        protected virtual Query GetQuery(Query query)
        {
            return (query != null) ? CloneValue(query) : ((Query != null) ? CloneValue(Query) : new Query());
        }

        // Update the child level Fields values for Remote data binding
        private static TreeFieldsMapping FieldSettings(object fields)
        {
            TreeFieldsMapping localMapper = new TreeFieldsMapping();
            if (fields != null)
            {
                localMapper.Children = (object)DataUtil.GetObject("Children", fields);
                localMapper.Id = (string)DataUtil.GetObject("Id", fields);
                localMapper.HasChildren = (string)DataUtil.GetObject("HasChildren", fields);
                localMapper.Text = (string)DataUtil.GetObject("Text", fields);
                localMapper.HtmlAttributes = (string)DataUtil.GetObject("HtmlAttributes", fields);
                localMapper.Expanded = (string)DataUtil.GetObject("Expanded", fields);
                localMapper.ImageUrl = (string)DataUtil.GetObject("ImageUrl", fields);
                localMapper.IconCss = (string)DataUtil.GetObject("IconCss", fields);
                localMapper.Selected = (string)DataUtil.GetObject("Selected", fields);
                localMapper.Tooltip = (string)DataUtil.GetObject("Tooltip", fields);
                localMapper.ParentID = (string)DataUtil.GetObject("ParentID", fields);
                localMapper.Url = (string)DataUtil.GetObject("NavigateUrl", fields);
                localMapper.DataManager = (DataManager)DataUtil.GetObject("DataManager", fields);
                localMapper.Query = (Query)DataUtil.GetObject("Query", fields);
                localMapper.IsChecked = (string)DataUtil.GetObject("IsChecked", fields);
                localMapper.TableName = (string)DataUtil.GetObject("TableName", fields);
            }

            return localMapper;
        }

        // Update Fields values based on provided node level.
        private void UpdateFields(int nodeLevel)
        {
            TreeFieldsMapping localMapper = FieldSettings(Parent.TreeViewFields);
            for (int i = 0; i < nodeLevel; i++)
            {
                if (localMapper.Children != null)
                {
                    localMapper = (TreeFieldsMapping)FieldSettings(localMapper.Children);
                }
            }

            ListGenerationDataManager = localMapper.DataManager;
            Query = localMapper.Query;
            ListGenerationFieldsMapper = localMapper;
        }

        // Customize the li element values.
        internal void BeforeNodeCreate(TreeItemCreatedArgs<TValue> args)
        {
            RenderTreeNodes(args);
        }

        // Trigger Draw node event for TreeView component.
        private async Task AfterNodeCreatedAsync(ItemCreatedArgs<TValue> args)
        {
            NodeRenderEventArgs<TValue> parameter = new NodeRenderEventArgs<TValue>()
            {
                NodeData = (TValue)args.ItemData,
                Text = args.Text,
                Name = "OnNodeRender"
            };
            await SfBaseUtils.InvokeEvent<NodeRenderEventArgs<TValue>>(Parent.TreeviewEvents?.OnNodeRender, parameter);
        }

        // Update the selected, checked, expanded attribute values based on user interactions.
        internal void ListStateChanged(string args)
        {
            if (DataType == TreeViewDataType.Hierarchical)
            {
                GetHierarchicalChild(args, DataSource);
            }
            else if (DataType == TreeViewDataType.SelfReferential)
            {
                MultiSelectFlag = true;
                GetSelfChild(args, DataSource);
            }

            UpdateHierarchicalAndSelfProps(ChildItems, EXPANDED);
            if (!Parent.IsTreeInteracted)
            {
                UpdateHierarchicalAndSelfProps(ChildItems, SELECTED);
            }

            ListUpdated();
        }

        // Based on latest property values update the list elements.
        internal void ListUpdated()
        {
            StateHasChanged();
        }

        // Based on Data Binding customize the tree nodes.
        private void RenderTreeNodes(TreeItemCreatedArgs<TValue> args)
        {
            if (DataType == TreeViewDataType.Hierarchical)
            {
                RenderingHierarchicalData(args);
            }
            else if (DataType == TreeViewDataType.SelfReferential)
            {
                RenderSelfReferencialData(args);
            }
            else if (DataType == TreeViewDataType.RemoteData)
            {
                var offline = Parent.TreeViewFields.DataManager?.Offline ?? false;
                if (!offline)
                {
                    RenderRemoteData(args);
                }
                else
                {
                    TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
                    if (fields.Child != null)
                    {
                        RenderingHierarchicalData(args);
                    }
                    else
                    {
                        RenderSelfReferencialData(args);
                    }
                }
            }
        }

        internal async Task RenderRemoteLi(string parentID, int level, bool getChildValue = false)
        {
            UpdateFields(level);
            IEnumerable<TValue> childData;
            Query = GettingQuery(ListGenerationFieldsMapper, parentID);
            object itemsData = (ListGenerationDataManager != null) ? await ListGenerationDataManager.ExecuteQuery<TValue>(Query) : null;
            List<TValue> dataSource = new List<TValue>();
            if (ListGenerationDataManager != null && itemsData != null)
            {
                dataSource = GetDataSource(itemsData);
                if (dataSource != null && dataSource.Count > 0)
                {
                    dataSource = GetSortedData(dataSource.ToList(), Parent.SortOrder.ToString(), ListGenerationFieldsMapper.Text);
                    childData = dataSource;
                    if (childData != null)
                    {
                        List<TValue> child = (childData as IEnumerable).Cast<TValue>().ToList();
                        if (!getChildValue)
                        {
                            RemoteDataField(parentID.ToString(), ListGenerationFieldsMapper, child);
                        }
                        else
                        {
                            AddChildListData(parentID.ToString(), child);
                        }
                    }

                    await EnsureExpandNodes(dataSource);
                }
            }
        }

        private async Task EnsureExpandNodes(List<TValue> dataSource)
        {
            for (int i = 0; i < dataSource.Count; i++)
            {
                object propertyValue = (ListGenerationFieldsMapper.Id != null && dataSource[i] != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, dataSource[i]) : null;
                if (Parent.AllExpandedNodes?.Count > 0)
                {
                    if (Parent.AllExpandedNodes.Contains(propertyValue?.ToString()))
                    {
                        await UpdateExpandState(propertyValue);
                    }
                }
                else
                {
                    object expandVal = (ListGenerationFieldsMapper.Expanded != null && dataSource[i] != null) ? GetAttrVal(ListGenerationFieldsMapper.Expanded, dataSource[i]) : null;
                    if (expandVal?.ToString() == "True")
                    {
                        await UpdateExpandState(propertyValue);
                    }
                }
            }
        }

        private async Task UpdateExpandState(object propertyValue)
        {
            if (Parent.LoadOnDemand)
            {
                await RenderRemoteLi(propertyValue.ToString(), 1);
            }

            if (Parent != null && Parent.ExpandedNodes != null ? Parent.ExpandedNodes.Contains(propertyValue.ToString()) : true)
            {
                if (!Parent.InternalExpandedNodes.Contains(propertyValue.ToString()))
                {
                    Parent.InternalExpandedNodes.Add(propertyValue.ToString());
                }

                if (!RemoteExpandedValues.Contains(propertyValue.ToString()))
                {
                    RemoteExpandedValues.Add(propertyValue.ToString());
                }

                if (!Parent.AllExpandedNodes.Contains(propertyValue.ToString()))
                {
                    Parent.AllExpandedNodes.Add(propertyValue.ToString());
                }
            }

            if (Parent != null && (!Parent.IsCompleteltyRendered ? Parent.ExpandedNodes == null : true))
            {
                await Parent.UpdateExpandedNodes();
            }
        }

        // Customize the Remote data binding li element data.
        private void RenderRemoteData(TreeItemCreatedArgs<TValue> args)
        {
            UpdateFields(args.NodeLevel - 1);
            if (Parent.LoadOnDemand)
            {
                if (Parent.AllExpandedNodes != null && Parent.AllExpandedNodes.Count > 0)
                {
                    RenderRemoteData_ExpandNodes(args);
                }
                else
                {
                    RenderRemoteData_List(args);
                }
            }
            else
            {
                RenderRemoteData_LoadOnDemand_False(args);
            }

            string isSelected = UpdateSelection(args);
            args.Options.Fields = ListGenerationFieldsMapper;
            object idValue = (ListGenerationFieldsMapper.Id != null && args.ItemData != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, args.ItemData) : null;
            args.TreeOptions = new TreeOptions<TValue>()
            {
                NodeTemplate = (Parent != null && Parent.TreeViewTemplate != null) ? Parent.TreeViewTemplate.NodeTemplate : null,
                ChildData = ListGenerationChild != null ? GetSortedData((List<TValue>)ListGenerationChild, Parent.SortOrder.ToString(), Parent.TreeViewFields.Text) : null,
                IsFullRowSelect = Parent.FullRowSelect,
                IsExpanded = ListGenerationIsExpanded,
                IsTree = true,
                IconClass = iconClass,
                TreeViewFields = ListGenerationFieldsMapper,
                IsSelected = UpdateSelfSelection(idValue?.ToString()),
                IsChecked = UpdateChecked(idValue.ToString()),
                IsEdit = UpdateEditing(idValue.ToString())
            };
            ListGenerationChild = null;
            ListGenerationIsExpanded = false;
            iconClass = null;
        }

        private void RenderRemoteData_List(TreeItemCreatedArgs<TValue> args)
        {
            IEnumerable<TValue> childData;
            object expandVal = (ListGenerationFieldsMapper.Expanded != null && args.ItemData != null) ? GetAttrVal(ListGenerationFieldsMapper.Expanded, args.ItemData) : null;
            if (expandVal != null)
            {
                Type expandType = expandVal.GetType();
                string parentID = (ListGenerationFieldsMapper.Id != null && args.ItemData != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, args.ItemData).ToString() : null;
                childData = (IEnumerable<TValue>)GetChildRemoteData(parentID);
                if (((expandType == typeof(bool) && (bool)expandVal) || (expandType == typeof(string) && (string)expandVal == TRUE)) || childData != null)
                {
                    ListGenerationChild = childData;
                    if (Parent != null && Parent.ExpandedNodes != null ? Array.IndexOf(Parent.ExpandedNodes, parentID.ToString()) >= 0 : true)
                    {
                        ListGenerationIsExpanded = true;
                    }
                }
                else if ((expandType == typeof(bool) && !(bool)expandVal) || (expandType == typeof(string) && (string)expandVal == FALSE))
                {
                    iconClass = ICONEXPANDCLASS;
                }
                else
                {
                    iconClass = null;
                }
            }
        }

        private void RenderRemoteData_ExpandNodes(TreeItemCreatedArgs<TValue> args)
        {
            IEnumerable<TValue> childData;
            object propertyValue = (ListGenerationFieldsMapper.Id != null && args.ItemData != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, args.ItemData) : null;
            if (Parent.AllExpandedNodes.Contains(propertyValue?.ToString()))
            {
                string parentID = (ListGenerationFieldsMapper.Id != null && args.ItemData != null) ? GetAttrVal(Parent.TreeViewFields.Id, args.ItemData).ToString() : null;
                childData = (IEnumerable<TValue>)GetChildRemoteData(parentID);
                ListGenerationChild = childData;
                if (Parent != null && Parent.ExpandedNodes != null ? Array.IndexOf(Parent.ExpandedNodes, propertyValue.ToString()) >= 0 : true)
                {
                    ListGenerationIsExpanded = true;
                }
            }
        }

        private void RenderRemoteData_LoadOnDemand_False(TreeItemCreatedArgs<TValue> args)
        {
            IEnumerable<TValue> childData;
            string parentID = (ListGenerationFieldsMapper.Id != null && args.ItemData != null) ? GetAttrVal(ListGenerationFieldsMapper.Id, args.ItemData).ToString() : null;
            childData = (IEnumerable<TValue>)GetChildRemoteData(parentID);
            ListGenerationChild = childData;
            if (Parent != null && Parent.ExpandedNodes != null ? Parent.ExpandedNodes.Contains(parentID) : Parent.AllExpandedNodes.Contains(parentID))
            {
                ListGenerationIsExpanded = true;
            }
            else if (ListGenerationChild != null)
            {
                ListGenerationIsExpanded = false;
                iconClass = ICONEXPANDCLASS;
            }
        }

        // Get the child data values for Remote data source.
        internal List<TValue> GetChildRemoteData(string id)
        {
            List<TValue> groupData = new List<TValue>();
            int dataLength = ListGenerationRemoteData.Count;
            for (int i = 0; i < dataLength; i++)
            {
                if (ListGenerationRemoteData[i].NodeId == id)
                {
                    groupData = ListGenerationRemoteData[i].RemoteData;
                    break;
                }
            }

            return groupData.Count != 0 ? groupData : null;
        }

        // Customize the Hierarchical data binding li element data.
        private void RenderingHierarchicalData(TreeItemCreatedArgs<TValue> args)
        {
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            object propertyValue = (fields.Id != null && args.ItemData != null) ? GetAttrVal(fields.Id, args.ItemData) : null;
            object childData;
            if (Parent.LoadOnDemand)
            {
                if (Parent.AllExpandedNodes != null && Parent.AllExpandedNodes.Count > 0)
                {
                    if (Parent.AllExpandedNodes.Contains(propertyValue?.ToString()))
                    {
                        childData = (fields.Child != null && args.ItemData != null) ? GetAttrVal(fields.Child.ToString(), args.ItemData) : null;
                        ListGenerationChild = (IEnumerable<TValue>)childData;
                        if ((Parent != null && Parent.ExpandedNodes != null ? Array.IndexOf(Parent.ExpandedNodes, propertyValue.ToString()) >= 0 : true) && ListGenerationChild != null && ListGenerationChild.Any())
                        {
                            ListGenerationIsExpanded = true;
                        }
                        else if (ListGenerationChild == null || !ListGenerationChild.Any())
                        {
                            ListGenerationChild = null;
                        }
                    }
                }
            }
            else
            {
                childData = (fields.Child != null && args.ItemData != null) ? GetAttrVal(fields.Child.ToString(), args.ItemData) : null;
                ListGenerationChild = (IEnumerable<TValue>)childData;
                string parentID = (fields.Id != null && args.ItemData != null) ? GetAttrVal(fields.Id, args.ItemData).ToString() : null;
                if (Parent != null && Parent.ExpandedNodes != null ? Array.IndexOf(Parent.ExpandedNodes, parentID) >= 0 : Parent.AllExpandedNodes.Contains(parentID))
                {
                    ListGenerationIsExpanded = true;
                }
                else if (ListGenerationChild != null)
                {
                    ListGenerationIsExpanded = false;
                    iconClass = ICONEXPANDCLASS;
                }
            }

            string nodeId = DataUtil.GetObject(fields.Id.ToString(), args.ItemData).ToString();
            bool isLoaded = (!Parent.IsLoaded && nodeId == Parent.LoadedId) ? false : true;
            args.TreeOptions = GetOptions(fields, UpdateSelfSelection(propertyValue.ToString()), UpdateChecked(propertyValue.ToString()), UpdateEditing(propertyValue.ToString()), Parent.AllDisabledNodes.Contains(nodeId), isLoaded);
            ListGenerationChild = null;
            ListGenerationIsExpanded = false;
            iconClass = null;
        }

        private bool UpdateEditing(string propertyValue)
        {
            return Parent.EdittedNodeId != null && Parent.EdittedNodeId == propertyValue;
        }

        private TreeOptions<TValue> GetOptions(TreeViewFieldsSettings<TValue> fields, bool isSelected, string isChecked, bool isEdit, bool isDisable, bool isLoaded)
        {
            return new TreeOptions<TValue>()
            {
                NodeTemplate = (Parent.TreeViewTemplate != null) ? Parent.TreeViewTemplate.NodeTemplate : null,
                ChildData = ListGenerationChild != null ? GetSortedData((List<TValue>)ListGenerationChild, Parent.SortOrder.ToString(), fields.Text) : null,
                IsFullRowSelect = Parent.FullRowSelect,
                IsExpanded = ListGenerationIsExpanded,
                IsSelected = isSelected,
                IsTree = true,
                IconClass = iconClass,
                IsChecked = isChecked,
                IsEdit = isEdit,
                FullRowNavigate = Parent.FullRowNavigable,
                IsDisabled = isDisable,
                IsLoaded = isLoaded
            };
        }

        // Update the selected node value based multi select option in Remote data source.
        private string UpdateSelection(TreeItemCreatedArgs<TValue> args)
        {
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            string isSelected = null;
            if (Parent.SelectedNodes != null && Parent.SelectedNodes.Length > 0)
            {
                object propertyValue = (fields.Id != null && args.ItemData != null) ? GetAttrVal(fields.Id, args.ItemData) : null;
                int pos = Array.IndexOf(Parent.SelectedNodes, propertyValue?.ToString());
                if (pos > -1)
                {
                    isSelected = TRUE;
                    if (MultiSelectFlag)
                    {
                        MultiSelectFlag = false;
                    }
                }
            }
            else
            {
                object selectedVal = (fields.Selected != null) ? GetAttrVal(fields.Selected, args.ItemData) : null;
                if (selectedVal != null)
                {
                    Type selectType = selectedVal.GetType();
                    if ((MultiSelectFlag && selectType == typeof(bool) && (bool)selectedVal) || (selectType == typeof(string) && (string)selectedVal == "true"))
                    {
                        isSelected = TRUE;
                        if (!Parent.AllowMultiSelection)
                        {
                            MultiSelectFlag = false;
                        }
                    }
                    else if ((Parent.AllowMultiSelection && selectType == typeof(bool) && !(bool)selectedVal) || (selectType == typeof(string) && (string)selectedVal == FALSE))
                    {
                        isSelected = FALSE;
                    }
                }
            }

            return isSelected;
        }

        // Update the selected node value based multi select option both Self Referential & Hierarchical data source.
        private bool UpdateSelfSelection(string id)
        {
            bool listGenerationIsSelected = false;
            if (Parent.AllSelectedNodes != null)
            {
                listGenerationIsSelected = Parent.AllSelectedNodes.Contains(id);
            }

            return listGenerationIsSelected;
        }

        // Update the checked node value based multi select option both Self Referential & Hierarchical data source.
        private string UpdateChecked(object id)
        {
            string listGenerationIsChecked = "false";
            if (Parent.AllCheckedNodes != null && Parent.AllCheckedNodes.ContainsKey(id.ToString()))
            {
                listGenerationIsChecked = Parent.AllCheckedNodes[id.ToString()].ToString();
            }

            return listGenerationIsChecked;
        }

        // Customize the Self Referential data binding li element data.
        private void RenderSelfReferencialData(TreeItemCreatedArgs<TValue> args)
        {
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            object propertyValue = (fields.Id != null && args.ItemData != null) ? GetAttrVal(fields.Id, args.ItemData) : null;
            if (Parent.LoadOnDemand)
            {
                if (Parent.AllExpandedNodes?.Count > 0)
                {
                    if (Parent.AllExpandedNodes.Contains(propertyValue?.ToString()))
                    {
                        string parentID = (fields.Id != null && args.ItemData != null) ? GetAttrVal(fields.Id, args.ItemData).ToString() : null;
                        IEnumerable<TValue> childData = (IEnumerable<TValue>)GroupingData(parentID, null);
                        ListGenerationChild = childData;
                        if (Parent != null && Parent.ExpandedNodes != null ? Array.IndexOf(Parent.ExpandedNodes, propertyValue.ToString()) >= 0 : true)
                        {
                            ListGenerationIsExpanded = true;
                        }
                    }
                }
            }
            else
            {
                string parentID = (fields.Id != null && args.ItemData != null) ? GetAttrVal(fields.Id, args.ItemData).ToString() : null;
                IEnumerable<TValue> childData = (IEnumerable<TValue>)GroupingData(parentID, null);
                ListGenerationChild = childData;
                if (Parent != null && Parent.ExpandedNodes != null ? Array.IndexOf(Parent.ExpandedNodes, parentID) >= 0 : Parent.AllExpandedNodes.Contains(parentID))
                {
                    ListGenerationIsExpanded = true;
                }
                else if (ListGenerationChild != null)
                {
                    ListGenerationIsExpanded = false;
                    iconClass = ICONEXPANDCLASS;
                }
            }
            string nodeId = DataUtil.GetObject(fields.Id.ToString(), args.ItemData).ToString();
            bool isLoaded = (!Parent.IsLoaded && nodeId == Parent.LoadedId) ? false : true;
            args.TreeOptions = GetOptions(fields, UpdateSelfSelection(propertyValue.ToString()), UpdateChecked(propertyValue.ToString()), UpdateEditing(propertyValue.ToString()), Parent.AllDisabledNodes.Contains(propertyValue.ToString()), isLoaded);
            ListGenerationChild = null;
            ListGenerationIsExpanded = false;
            iconClass = null;
        }

        // Get attribute value for current data source.
        internal static object GetAttrVal(string propertyName, object currentData)
        {
            return DataUtil.GetObject(propertyName, currentData);
        }

        internal static IEnumerable<TValue> GetChild(string field, TValue itemData)
        {
            return (IEnumerable<TValue>)GetAttrVal(field, itemData);
        }

        // Grouping the data for provided Parent node id.
        internal List<TValue> GroupingData(string parentID, List<TValue> dataSource = null)
        {
            string id;
            List<TValue> groupData = new List<TValue>();
            List<TValue> listData = dataSource != null ? dataSource : ItemsData != null ? ItemsData.ToList() : new List<TValue>();
            int listDataLength = listData.Count;
            for (int i = 0; i < listDataLength; i++)
            {
                id = DataUtil.GetObject(Parent.TreeViewFields.ParentID, listData[i])?.ToString();
                if (parentID == id || (parentID != null && parentID.Equals(id, StringComparison.Ordinal)))
                {
                    groupData.Add(listData[i]);
                }
            }

            return groupData.Count != 0 ? groupData : null;
        }

        /// <summary>
        /// Update the Persistence value to local storage.
        /// </summary>
        internal async Task SetLocalStorage(string persistId, string dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        /// <summary>
        /// Updating the persisting values to our component properties.
        /// </summary>
        internal string SerializeModel()
        {
            return JsonSerializer.Serialize(new TreePersistenceValues { CheckedNodes = Parent.AllCheckedNodes, ExpandedNodes = Parent.AllExpandedNodes, SelectedNodes = Parent.AllSelectedNodes });
        }

        internal void AddChildData(string target, TValue node, List<TValue> datasource, bool isChild)
        {
            string idAttrValue;
            List<TValue> dataList = datasource?.ToList();
            int dataLength = dataList.Count;
            IEnumerable<TValue> childs;
            List<TValue> newNodedata = new List<TValue>();
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = GetAttrValue(Parent.TreeViewFields?.Id, dataList[i]);
                childs = GetChild(Parent.TreeViewFields.Child.ToString(), dataList[i]);
                if (idAttrValue == target)
                {
                    if (childs != null)
                    {
                        newNodedata = childs.ToList();
                    }

                    if (isChild)
                    {
                        newNodedata.Add(node);
                        PropertyInfo prop = dataList[i].GetType().GetProperty(Parent.TreeViewFields?.Child?.ToString());
                        FieldInfo field = dataList[i].GetType().GetField(Parent.TreeViewFields?.Child?.ToString());
                        if (prop != null)
                        {
                            prop.SetValue(dataList[i], newNodedata);
                        }
                        else if (field != null)
                        {
                            field.SetValue(dataList[i], newNodedata);
                        }
                    }

                    break;
                }

                if (childs != null)
                {
                    AddChildData(target, node, childs.ToList(), true);
                }
            }

            Parent.InternalData?.ToList().Concat(dataList);
            ItemsData = (IEnumerable<TValue>)Parent.InternalData;
        }

        // updates the child data for remote data if any data is dynamically added
        private void AddChildListData(string id, List<TValue> nodes)
        {
            int index = -1;
            foreach (RemoteFieldsData SubData in ListGenerationRemoteData)
            {
                if (SubData.NodeId == id)
                {
                    index = ListGenerationRemoteData.IndexOf(SubData);
                }
            }
            if (index >= 0 && !SfBaseUtils.Equals(ListGenerationRemoteData[index].RemoteData, nodes))
            {
                ListGenerationRemoteData[index].RemoteData.AddRange(nodes);
            }
        }

        /// <summary>
        /// Adding treeview nodes.
        /// </summary>
        internal async void AddNodeData(List<TValue> nodes, string target = null)
        {
            List<TValue> nodeList = Parent.InternalData;
            if (nodes != null)
            {
                int nodeCount = nodes.Count;
                if (DataType == TreeViewDataType.RemoteData && target != null)
                {
                    string idValue;
                    int index = -1;
                    bool isValueUpdated = false;
                    TreeFieldsMapping settings = new TreeFieldsMapping() { };
                    List<TValue> rootData = ListData.ToList();
                    foreach (TValue data in rootData)
                    {
                        idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data).ToString();
                        if (idValue == target)
                        {
                            foreach (RemoteFieldsData SubData in ListGenerationRemoteData)
                            {
                                if (SubData.NodeId == target)
                                {
                                    isValueUpdated = true;
                                    index = ListGenerationRemoteData.IndexOf(SubData);
                                }
                            }
                            if (!isValueUpdated)
                            {
                                string HasChildren = Parent.TreeViewFields.HasChildren.ToString();
                                data.GetType().GetProperty(HasChildren)?.SetValue(data, true);
                                await Parent.TreeViewFields.DataManager.Insert<TValue>(nodes[0], Query?.FromTable, Query);
                                return;
                            }
                        }
                    }
                    if (index >= 0 && isValueUpdated)
                    {
                        ListGenerationRemoteData[index].RemoteData.AddRange(nodes);
                        if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline)
                        {
                            await Parent.TreeViewFields.DataManager.Insert<TValue>(nodes[0], Query?.FromTable, Query);
                        }
                    }
                    if (!isValueUpdated)
                    {
                        foreach (RemoteFieldsData data in ListGenerationRemoteData)
                        {
                            idValue = data.NodeId;
                            if (idValue == target && !isValueUpdated)
                            {
                                isValueUpdated = true;
                                ListGenerationRemoteData[ListGenerationRemoteData.IndexOf(data)].RemoteData.AddRange(nodes);
                            }
                            else
                            {
                                foreach (TValue NodeData in data.RemoteData)
                                {
                                    idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), NodeData).ToString();
                                    if (idValue == target)
                                    {
                                        isValueUpdated = true;
                                        settings = data.FieldSettings;
                                    }
                                }
                            }
                        }
                        if (isValueUpdated)
                        {
                            isValueUpdated = false;
                            index = -1;
                            foreach (RemoteFieldsData SubData in ListGenerationRemoteData)
                            {
                                if (SubData.NodeId == target)
                                {
                                    isValueUpdated = true;
                                    index = ListGenerationRemoteData.IndexOf(SubData);
                                }
                            }
                            if (index >= 0)
                            {
                                ListGenerationRemoteData[index].RemoteData.AddRange(nodes);
                                if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline)
                                {
                                    await Parent.TreeViewFields.DataManager.Insert<TValue>(nodes[0], Query?.FromTable, Query);
                                }
                            }
                            if (!isValueUpdated)
                            {
                                foreach (RemoteFieldsData SubData in ListGenerationRemoteData)
                                {
                                    foreach (TValue NodeData in SubData.RemoteData)
                                    {
                                        idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), NodeData).ToString();
                                        if (idValue == target)
                                        {
                                            string HasChildren = Parent.TreeViewFields.HasChildren.ToString();
                                            NodeData.GetType().GetProperty(HasChildren)?.SetValue(NodeData, true);
                                        }
                                    }
                                }
                                if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline)
                                {
                                    await Parent.TreeViewFields.DataManager.Insert<TValue>(nodes[0], Query?.FromTable, Query);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (DataType == TreeViewDataType.Hierarchical && target != null)
                    {
                        AddChildData(target, nodes[0], Parent.InternalData, true);
                    }
                    else
                    {
                        for (int i = 0; i < nodeCount; i++)
                        {
                            TValue item = nodes.ElementAt(i);
                            nodeList.Add(item);
                        }
                        if ((DataType == TreeViewDataType.RemoteData && Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline))
                        {
                            await Parent.TreeViewFields.DataManager.Insert<TValue>(nodes[0], Query?.FromTable, Query);
                        }
                        await Parent.UpdateData(nodeList);
                    }
                }
            }
        }

        /// <summary>
        /// Update Has Child after remove operation in Remote Data
        /// </summary>
        private async Task CheckForRemoteHasChild(List<object> parentIds)
        {
            if(parentIds.Count > 0)
            {
                List<TValue> rootData = ListData.ToList();
                int[] rootIndex = Array.Empty<int>();
                bool updateRootChild;
                foreach (TValue data in rootData)
                {
                    updateRootChild = true;
                    if (parentIds.Contains(DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data)))
                    {
                        foreach(RemoteFieldsData SubData in ListGenerationRemoteData)
                        {
                            if(SubData.NodeId == DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data).ToString())
                            {
                                updateRootChild = false;
                            }
                        }
                        if (updateRootChild && parentIds.Contains(DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data)))
                        {
                            string HasChildren = Parent.TreeViewFields.HasChildren.ToString();
                            data.GetType().GetProperty(HasChildren)?.SetValue(data, false);
                            if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline)
                            {
                                await Parent.TreeViewFields.DataManager.Update<TValue>(Parent.TreeViewFields.Id, data, Query?.FromTable, Query);
                            }
                        }
                    }
                }
                bool updateSubChild;
                foreach(RemoteFieldsData data in ListGenerationRemoteData)
                {
                    updateSubChild = true;
                    foreach (TValue NodData in data.RemoteData)
                    {
                        if (parentIds.Contains(DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data)))
                        {
                            foreach (RemoteFieldsData checkData in ListGenerationRemoteData)
                            {
                                if(checkData.NodeId == DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data).ToString())
                                {
                                    updateSubChild = false;
                                }
                            }
                        }
                        if (updateSubChild && parentIds.Contains(DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), NodData)))
                        {
                            string HasChildren = Parent.TreeViewFields.HasChildren.ToString();
                            NodData.GetType().GetProperty(HasChildren)?.SetValue(NodData, false);
                            if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline)
                            {
                                await Parent.TreeViewFields.DataManager.Update<TValue>(Parent.TreeViewFields.Id, NodData, Query?.FromTable, Query);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Removing treeview nodes.
        /// </summary>
        internal async void RemoveNodes(string[] nodes)
        {
            List<TValue> nodeList = Parent.InternalData;
            if (nodes != null)
            {
                if (DataType == TreeViewDataType.RemoteData)
                {
                    List<object> parentCheckValues = new List<object>(){ };
                    List<TValue> updatedData = new List<TValue>() { };
                    foreach (string id in nodes)
                    {
                        updatedData.Add(Parent.GetTreeData(id)[0]);
                    }
                    string idValue;
                    int[] rootIndex = Array.Empty<int>();
                    int[] subIndex = Array.Empty<int>();
                    Dictionary<int, int[]> nodeIndex = new Dictionary<int, int[]>() { };
                    List<TValue> rootData = ListData.ToList();
                    foreach (TValue data in rootData)
                    {
                        idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data).ToString();
                        object parentIdValue = Parent.TreeViewFields.ParentID != null ? DataUtil.GetObject(Parent.TreeViewFields.ParentID.ToString(), data) : null;
                        if (nodes.Contains(idValue))
                        {
                            if (parentIdValue != null)
                            {
                                parentCheckValues.Add(parentIdValue);
                            }
                            rootIndex = SfBaseUtils.AddArrayValue(rootIndex, rootData.IndexOf(data));
                        }
                    }
                    if(rootIndex.Length > 0)
                    {
                        foreach(int index in rootIndex)
                        {
                            rootData.RemoveAt(index);
                        }
                        ListData = Parent.InternalData = rootData;
                    }
                    foreach (RemoteFieldsData SubData in ListGenerationRemoteData)
                    {
                        if (nodes.Contains(SubData.NodeId))
                        {
                            subIndex = SfBaseUtils.AddArrayValue(subIndex, ListGenerationRemoteData.IndexOf(SubData));
                        }
                    }
                    if (subIndex.Length > 0)
                    {
                        foreach (int index in subIndex)
                        {
                            ListGenerationRemoteData.RemoveAt(index);
                        }
                    }
                    foreach (RemoteFieldsData SubData in ListGenerationRemoteData)
                    {
                        foreach(TValue NodeData in SubData.RemoteData)
                        {
                            idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), NodeData).ToString();
                            object parentIdValue = Parent.TreeViewFields.ParentID != null ? DataUtil.GetObject(Parent.TreeViewFields.ParentID.ToString(), NodeData) : null;
                            if (nodes.Contains(idValue))
                            {
                                if (parentIdValue != null)
                                {
                                    parentCheckValues.Add(parentIdValue);
                                }
                                if (!nodeIndex.ContainsKey(ListGenerationRemoteData.IndexOf(SubData)))
                                {
                                    nodeIndex.Add(ListGenerationRemoteData.IndexOf(SubData), new int[] { SubData.RemoteData.IndexOf(NodeData) });
                                }
                                else
                                {
                                    nodeIndex[ListGenerationRemoteData.IndexOf(SubData)] = SfBaseUtils.AddArrayValue(nodeIndex[ListGenerationRemoteData.IndexOf(SubData)], SubData.RemoteData.IndexOf(NodeData));
                                }
                            }
                        }
                    }
                    if (nodeIndex.Count > 0)
                    {
                        foreach(int key in nodeIndex.Keys)
                        {
                            foreach(int value in nodeIndex[key])
                            {
                                ListGenerationRemoteData[key].RemoteData.RemoveAt(value);
                            }
                        }
                    }
                    foreach(TValue data in updatedData)
                    {
                        if (Parent.TreeViewFields != null && Parent.TreeViewFields.DataManager != null && !Parent.TreeViewFields.DataManager.Offline)
                        {
                            await Parent.TreeViewFields.DataManager.Remove<TValue>(Parent.TreeViewFields.Id, DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data), Query?.FromTable, Query);
                        }
                    }
                    await CheckForRemoteHasChild(parentCheckValues);
                }
                else
                {
                    if (DataType == TreeViewDataType.Hierarchical)
                    {
                        int nodeCount = nodes.Length;
                        if (nodeCount > 0)
                        {
                            for (int k = 0; k < nodeCount; k++)
                            {
                                GetHierarchicalData(nodes[k], DataSource.ToList());
                                GetRemovedHierData(nodes[k], nodeList, true, default(TValue), Parent.TreeviewRemovedData, true);
                            }
                        }
                    }
                    else
                    {
                        int dataCount = nodeList.Count;
                        for (int i = 0; i < nodes.Length; i++)
                        {
                            string nodeValue = nodes[i];
                            for (int j = dataCount - 1; j >= 0; j--)
                            {
                                TValue item = nodeList.ElementAt(j);
                                string nodeid = DataUtil.GetObject(Parent.TreeViewFields.Id.ToString(), item).ToString();
                                string parentId = Parent.TreeViewFields.ParentID != null && DataUtil.GetObject(Parent.TreeViewFields.ParentID.ToString(), item) != null ? DataUtil.GetObject(Parent.TreeViewFields.ParentID.ToString(), item).ToString() : null;
                                if (nodeValue == nodeid || nodeValue == parentId)
                                {
                                    nodeList.Remove(nodeList[j]);
                                }
                            }
                        }
                    }
                }
            }

            await Parent.UpdateData(nodeList);
        }

        /// <summary>
        /// Return treeview node data.
        /// </summary>
        internal List<TValue> GetTreeViewData(string node = null)
        {
            List<TValue> treeData = new List<TValue>();
            TValue data;
            if (node != null)
            {
                if (DataType == TreeViewDataType.Hierarchical)
                {
                    GetHierarchicalData(node, DataSource.ToList());
                    data = Parent.TreeviewRemovedData;
                    UpdateData(new List<TValue>() { data });
                    treeData.Add(data);
                }
                else if (DataType == TreeViewDataType.SelfReferential)
                {
                    data = GetRemovedSelfData(node);
                    UpdateData(new List<TValue>() { data });
                    treeData.Add(data);
                }
                else if (DataType == TreeViewDataType.RemoteData)
                {
                    data = GetRemoteNodeData(node);
                    UpdateData(new List<TValue>() { data });
                    treeData.Add(data);
                }
            }
            else
            {
                treeData = Parent.AllowDragAndDrop ? DataSource?.ToList() : Parent.TreeViewFields?.DataSource?.ToList();
                UpdateData(treeData);
            }

            return treeData;
        }

        private void UpdateData(List<TValue> data)
        {
            if (data != null)
            {
                foreach (TValue treeData in data)
                {
                    string idValue = DataUtil.GetObject(Parent.TreeViewFields.Id.ToString(), treeData).ToString();
                    if (Parent.ExpandedNodes != null)
                    {
                        bool isExpanded = Parent.ExpandedNodes.Contains(idValue);
                        treeData.GetType().GetProperty(nameof(Parent.TreeViewFields.Expanded))?.SetValue(treeData, isExpanded);
                    }
                    if (Parent.CheckedNodes != null)
                    {
                        bool isChecked = Parent.CheckedNodes.Contains(idValue);
                        treeData.GetType().GetProperty(nameof(Parent.TreeViewFields.IsChecked))?.SetValue(treeData, isChecked);
                    }
                    if (Parent.SelectedNodes != null)
                    {
                        bool isSelected = Parent.SelectedNodes.Contains(idValue);
                        treeData.GetType().GetProperty(nameof(Parent.TreeViewFields.Selected))?.SetValue(treeData, isSelected);
                    }
                    if (Parent.TreeViewFields.Child != null)
                    {
                        List<TValue> children = GetChild(Parent.TreeViewFields.Child.ToString(), treeData)?.ToList();
                        if (children != null)
                        {
                            UpdateData(children);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the node data for remote data
        /// </summary>
        internal TValue GetRemoteNodeData(string id)
        {
            TValue item = default(TValue);
            string idValue;
            bool isValueUpdated = false;
            foreach (TValue data in ListData)
            {
                idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), data).ToString();
                if (idValue == id)
                {
                    isValueUpdated = true;
                    return data;
                }
            }
            if (!isValueUpdated)
            {
                foreach (RemoteFieldsData data in ListGenerationRemoteData)
                {
                    foreach (TValue NodeData in data.RemoteData)
                    {
                        idValue = DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), NodeData).ToString();
                        if (idValue == id)
                        {
                            isValueUpdated = true;
                            return NodeData;
                        }
                    }
                }
            }
            return item;
        }

        /// <summary>
        /// Get the node's data such as id, text, parentID, selected, isChecked, and expanded by passing the node element or it's ID.
        /// </summary>
        internal NodeData GetNodeDetails(string node)
        {
            NodeData getNodeData = new NodeData();
            TValue item = default(TValue);
            if (node != null)
            {
                if (DataType == TreeViewDataType.Hierarchical)
                {
                    GetHierarchicalData(node, DataSource.ToList());
                    item = Parent.TreeviewRemovedData;
                    if (item != null)
                    {
                        getNodeData.HasChildren = Parent.TreeViewFields.Child != null ? DataUtil.GetObject(Parent.TreeViewFields.Child.ToString(), item) != null : false;
                    }
                }
                else if (DataType == TreeViewDataType.SelfReferential)
                {
                    item = GetRemovedSelfData(node);
                    if (item != null)
                    {
                        getNodeData.HasChildren = Parent.TreeViewFields.HasChildren != null && DataUtil.GetObject(Parent.TreeViewFields?.HasChildren?.ToString(), item) != null ? (bool)DataUtil.GetObject(Parent.TreeViewFields?.HasChildren?.ToString(), item) : false;
                    }
                }
                else if (DataType == TreeViewDataType.RemoteData)
                {
                    GetHierarchicalData(node, DataSource.ToList());
                    item = GetRemoteNodeData(node);
                    if (item != null)
                    {
                        getNodeData.HasChildren = Parent.TreeViewFields.Child != null ? DataUtil.GetObject(Parent.TreeViewFields.Child.ToString(), item) != null : false;
                    }
                }
            }

            if (item != null)
            {
                string nodeValue = DataUtil.GetObject(Parent.TreeViewFields.Id.ToString(), item).ToString();
                if (node.ToString().Contains(nodeValue, StringComparison.Ordinal))
                {
                    getNodeData.Id = (string)DataUtil.GetObject(Parent.TreeViewFields.Id?.ToString(), item).ToString();
                    getNodeData.Text = (string)DataUtil.GetObject(Parent.TreeViewFields.Text?.ToString(), item);
                    getNodeData.ParentID = Parent.TreeViewFields.ParentID != null ? DataUtil.GetObject(Parent.TreeViewFields.ParentID.ToString(), item)?.ToString() : string.Empty;
                    getNodeData.Selected = (bool)Parent.AllSelectedNodes.Contains(nodeValue) ? true : false;
                    getNodeData.IsChecked = Parent.AllCheckedNodes.Keys.ToList().Contains(nodeValue) ? "true" : "false";
                    getNodeData.Expanded = Parent.ExpandedNodes != null && (bool)Parent.ExpandedNodes.Contains(nodeValue) ? true : false;
                }
            }

            return getNodeData;
        }

        internal void GetHierarchicalData(string id, IEnumerable<TValue> dataSource)
        {
            string idAttrValue;
            List<TValue> dataList = dataSource.ToList();
            int dataLength = dataList.Count;
            IEnumerable<TValue> childs;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = GetAttrValue(fields.Id, dataList[i]);
                childs = fields.Child != null ? GetChild(fields.Child.ToString(), dataList[i]) : null;
                if (idAttrValue == id)
                {
                    Parent.TreeviewRemovedData = dataList[i];
                    break;
                }

                if (childs != null)
                {
                    GetHierarchicalData(id, (List<TValue>)childs);
                }
            }
        }

        internal void GetAllNodeId(IEnumerable<TValue> dataSource)
        {
            string idAttrValue;
            List<TValue> dataList = dataSource.ToList();
            int dataLength = dataList.Count;
            IEnumerable<TValue> childs;
            TreeViewFieldsSettings<TValue> fields = Parent.TreeViewFields;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = GetAttrValue(fields.Id, dataList[i]);
                if (!CheckNodeId.Contains(idAttrValue))
                {
                    CheckNodeId.Add(idAttrValue);
                }

                childs = fields.Child != null ? GetChild(fields.Child.ToString(), dataList[i]) : null;
                bool hasChild = fields.HasChildren != null ? (bool)DataUtil.GetObject(fields.HasChildren.ToString(), dataList[i]) : false;
                if (DataType == TreeViewDataType.SelfReferential)
                {
                    List<TValue> childNodes = GroupingData(idAttrValue, DataSource.ToList());
                    if (childNodes != null && childNodes.Count > 0)
                    {
                        hasChild = true;
                    }
                }

                if (hasChild && !AllPrentNodeId.Contains(idAttrValue))
                {
                    AllPrentNodeId.Add(idAttrValue);
                }

                if (childs != null)
                {
                    if (!AllPrentNodeId.Contains(idAttrValue))
                    {
                        AllPrentNodeId.Add(idAttrValue);
                    }

                    GetAllNodeId((List<TValue>)childs);
                }
            }
        }

        internal async Task UpdatePersistence()
        {
            if (Parent.EnablePersistence && Parent.IsRendered)
            {
                await SetLocalStorage(Parent.ID, SerializeModel());
            }
        }

        // Component dispose method.
        internal override void ComponentDispose()
        {
            Parent = null;
            ListData = null;
            Query = null;
            iconClass = null;
            ListGenerationChild = null;
            ListGenerationDataManager = null;
            ListGenerationRemoteData = null;
            ListGenerationFieldsMapper = null;
            AllPrentNodeId = null;
            CheckNodeId = null;
            ItemsData = null;
            ChildItems = null;
            DataSource = null;
            RemoteExpandedValues = null;
        }

        private class RemoteFieldsData
        {
            internal string NodeId { get; set; }

            internal TreeFieldsMapping FieldSettings { get; set; }

            internal List<TValue> RemoteData { get; set; }
        }
    }
}
