using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Data;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;
using System.Runtime.CompilerServices;
using System.Globalization;
using Syncfusion.Blazor.Spinner;
using System;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.PivotView")]

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The TreeView component is used to represent hierarchical data in a tree like structure with advanced functions to edit, drag and drop, select with CheckBox and more.
    /// TreeView can be populated from a data source such as an array of data's or from DataManager.
    /// </summary>
    /// <typeparam name="TValue">"TValue parameter".</typeparam>
    public partial class SfTreeView<TValue> : SfBaseComponent
    {
        internal const string RTLENABLE = "enableRtl";
        internal const string EXPANDONTYPE = "expandOnType";
        internal const string TREEVIEWFIELD = "fields";
        internal const string SPACE = " ";
        internal const string CLASS = "class";
        internal const string IDVALUE = "id";
        internal const string ROLE = "role";
        internal const string TABINDEX = "tabindex";
        internal const string ACTIVEDESCENDENT = "aria-activedescendant";
        internal const string RTL = "e-rtl";
        internal const string FULLROWWRAP = "e-fullrow-wrap";
        internal const string TREEVIEWALLOWMULTISELECTION = "allowMultiSelection";
        internal const string TEXTWRAP = "allowTextWrap";
        internal const string TREEVIEWSHOWCHECKBOX = "showCheckBox";
        internal const string TREEVIEWALLOWEDITING = "allowEditing";
        internal const string TREEVIEWALLOWDRAGANDDROP = "allowDragAndDrop";
        internal const string DRAGAREA = "dropArea";
        internal const string TREEVIEWFULLROWSELECT = "fullRowSelect";
        internal const string TREEVIEWCSSCLASS = "cssClass";
        internal const string TREEVIEWDISABLED = "disabled";
        internal const string EDISABLED = "e-disabled";
        internal const string WRAP = "e-text-wrap";
        internal const string TRUE = "true";
        internal const string CHECK = "check";
        internal const string TEMPLATES = "TreeViewTemplates";
        internal const string HASTEMPLATE = "hasTemplate";
        internal const string TREEVIEWANIMATIONCLASS = "animation";
        internal const string PROPERTYCHANGED = "sfBlazor.TreeView.onPropertyChanged";
        private string treeViewClassList = "e-control e-lib e-treeview";
        private string role = "tree";
        private string tabIndex = "0";
        private string activeDescendant = "_active";
        private bool treeViewHasTemplate;
        private bool treeviewSelectionUpdate;
        internal bool IsLoaded = true;
        internal bool IsTreeNodeExpandingCall;
        internal bool IsClearStateCall; 
        internal string LoadedId;
        private Dictionary<string, object> attributes = new Dictionary<string, object>();

        internal bool IsDestroyed { get; set; }

        internal bool IsNodeRendered { get; set; }
        internal bool TreeExpandAll { get; set; }
        internal bool IsNodeExpanded { get; set; }

        internal int ChildCount { get; set; }

        internal int NodeLen { get; set; }

        internal bool IsDevice { get; set; }

        internal string Target { get; set; }

        internal SfSpinner SpinnerRef { get; set; }

        // Specify the server side private variables for the component.
        internal List<string> AllExpandedNodes { get; set; } = new List<string>();

        internal List<string> InternalExpandedNodes { get; set; } = new List<string>();

        internal List<string> CurrentExpandedNodes { get; set; } = new List<string>();

        internal bool IsCompleteltyRendered { get; set; }

        internal bool IsCurrentExpandedUpdated { get; set; }

        internal List<string> AllSelectedNodes { get; set; } = new List<string>();

        internal List<string> AllDisabledNodes { get; set; } = new List<string>();

        internal Dictionary<string, object> AllCheckedNodes { get; set; } = new Dictionary<string, object>();

        internal List<string> IsRenderedNodes { get; set; } = new List<string>();

        internal string EdittedNodeId { get; set; } = string.Empty;

        internal bool IsTreeInteracted { get; set; }

        internal List<TValue> InternalData { get; set; }

        internal bool IsNumberTypeId { get; set; }

        internal TValue TreeviewRemovedData { get; set; }

        internal bool IsNodeDropped { get; set; }

        internal ElementReference Element { get; set; }

        internal ListGeneration<TValue> ListReference { get; set; }

        internal TreeViewEvents<TValue> TreeviewEvents { get; set; }

        internal bool IsDataSourceChanged { get; set; }

        private bool TreeAllowDragAndDrop { get; set; }

        private bool TreeAllowEditing { get; set; }

        private bool TreeAllowTextWrap { get; set; }

        private bool TreeAllowMultiSelection { get; set; }
        private string[] TreeCheckedNodes { get; set; }

        private bool TreeAutoCheck { get; set; } = true;

        private string TreeCssClass { get; set; } = string.Empty;

        private bool TreeDisabled { get; set; }

        private string TreeDropArea { get; set; }

        private bool TreeEnableRtl { get; set; }

        private ExpandAction TreeExpandOn { get; set; } = ExpandAction.DoubleClick;

        private TreeViewFieldsSettings<TValue> TreeFields { get; set; }

        private string[] TreeExpandedNodes { get; set; }

        private bool TreeFullRowSelect { get; set; } = true;

        private string[] TreeSelectedNodes { get; set; }

        private bool TreeShowCheckBox { get; set; }

        private SortOrder TreeSortOrder { get; set; }

        private void SetRootAttributes()
        {
            UpdateDisbaled();
            SetCssClass();
            SetRTL();
            SetFullRowSelect();
            SetTextWrap();
            UpdateHtmlAttr();
            UpdateAttributes();
        }

        // Update the Disabled property value to TreeView component.
        private void UpdateDisbaled()
        {
            if (Disabled)
            {
                treeViewClassList += SPACE + EDISABLED;
            }
        }

        // Set cssClass for TreeView component.
        private void SetCssClass()
        {
            treeViewClassList += SPACE + CssClass;
        }

        private void SetTextWrap()
        {
            if (AllowTextWrap) treeViewClassList += SPACE + WRAP;
        }

        // Set multi select options to TreeView component.
        private async Task SetMultiSelection()
        {
            if (!AllowMultiSelection && AllSelectedNodes.Count > 1)
            {
                AllSelectedNodes.RemoveRange(1, AllSelectedNodes.Count - 1);
                await UpdateSelectedNodes();
            }

            await InvokeMethod("sfBlazor.TreeView.setMultiSelect", Element, AllowMultiSelection);
        }

        // Update tree view component element attributes.
        private void UpdateAttributes()
        {
            SfBaseUtils.UpdateDictionary(IDVALUE, ID, attributes);
            SfBaseUtils.UpdateDictionary(CLASS, treeViewClassList, attributes);
            SfBaseUtils.UpdateDictionary(ROLE, role, attributes);
            SfBaseUtils.UpdateDictionary(TABINDEX, tabIndex, attributes);
            SfBaseUtils.UpdateDictionary(ACTIVEDESCENDENT, ID + activeDescendant,  attributes);
        }

        // Set Full Row Select options for TreeView component.
        private void SetFullRowSelect()
        {
            if (FullRowSelect)
            {
                treeViewClassList += SPACE + FULLROWWRAP;
            }
        }

        // Set Right to Left or Left to Right directions for TreeView component.
        private void SetRTL()
        {
            if (EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl))
            {
                treeViewClassList += SPACE + RTL;
            }
        }

        // Update the latest property values to TreeView component.
        internal async Task OnPropertyChangeHandler()
        {
            foreach (string key in PropertyChanges.Keys.ToList())
            {
                if (PropertyChanges.ContainsKey(nameof(ExpandedNodes)) && ListReference != null)
                {
                    await OnPropertyChangeHandler_ExpandedNodes();
                }

                if (PropertyChanges.ContainsKey(nameof(SelectedNodes)))
                {
                    await OnPropertyChangeHandler_SelectedNodes();
                }

                if (PropertyChanges.ContainsKey(nameof(CheckedNodes)) && ListReference != null)
                {
                    AllCheckedNodes.Clear();
                    await ListReference.UpdateCheckedNodes(true);
                }

                if (PropertyChanges.ContainsKey(nameof(AllowMultiSelection)))
                {
                    await SetMultiSelection();
                }

                if (PropertyChanges.ContainsKey(nameof(AutoCheck)))
                {
                    await UpdateCheckedState();
                }

                if (PropertyChanges.ContainsKey(nameof(FullRowSelect)))
                {
                    ListReference.ListUpdated();
                    await InvokeMethod(PROPERTYCHANGED, Element, GetPropertyChanges());
                }
                IDictionary<string, object> changedProperties = GetPropertyChanges();
                if (!PropertyChanges.ContainsKey(nameof(FullRowSelect)) && !PropertyChanges.ContainsKey(nameof(AutoCheck)) && !PropertyChanges.ContainsKey(nameof(AllowMultiSelection)) && !PropertyChanges.ContainsKey(nameof(CheckedNodes)) && !PropertyChanges.ContainsKey(nameof(SelectedNodes)) && !PropertyChanges.ContainsKey(nameof(ExpandedNodes)) && changedProperties.Count > 0)
                {
                    await InvokeMethod(PROPERTYCHANGED, Element, changedProperties);
                }

                if (ListReference != null && !AllowDragAndDrop)
                {
                    await OnPropertyChangeHandler_ListReference();
                }
            }
        }

        private async Task OnPropertyChangeHandler_ListReference()
        {
            TreeViewFields.TreeViewCommonFieldsSettingsdataSource = NotifyPropertyChanges("DataSource", TreeViewFields.DataSource?.ToList(), TreeViewFields.TreeViewCommonFieldsSettingsdataSource?.ToList());
            if (!PropertyChanges.ContainsKey("DataSource") && ((TreeviewEvents != null && (TreeviewEvents.NodeSelecting.HasDelegate || TreeviewEvents.NodeSelected.HasDelegate || TreeviewEvents.NodeExpanding.HasDelegate || TreeviewEvents.NodeExpanded.HasDelegate)) || CheckedNodesChanged.HasDelegate || ExpandedNodesChanged.HasDelegate))
            {
                return;
            }

            await UpdateData(TreeViewFields != null && TreeViewFields.DataSource != null ? TreeViewFields.DataSource.ToList() : null);
        }

        private async Task OnPropertyChangeHandler_ExpandedNodes()
        {
            List<string> expandingNodes = new List<string>();
            List<string> collapsingNodes = new List<string>();
            if (ExpandedNodes == null)
            {
                ExpandedNodes = Array.Empty<string>();
            }
            List<string> expandNodesList = ExpandedNodes.ToList();
            collapsingNodes = InternalExpandedNodes.Except(expandNodesList).ToList();
            expandingNodes = expandNodesList.Except(InternalExpandedNodes).ToList();
            if (collapsingNodes.Count > 0)
            {
                await CollapseAll(collapsingNodes.ToArray());
            }

            if (expandingNodes.Count > 0)
            {
                await ExpandAll(expandingNodes.ToArray());
            }

            InternalExpandedNodes = ExpandedNodes.ToList();
        }

        private async Task OnPropertyChangeHandler_SelectedNodes()
        {
            List<string> selectedNodes = new List<string>();
            for (int i = 0; i < SelectedNodes?.Length; i++)
            {
                if (!AllSelectedNodes.Contains(SelectedNodes[i]))
                {
                    selectedNodes.Add(SelectedNodes[i]);
                }
            }

            for (int i = 0; i < selectedNodes.Count; i++)
            {
                await TriggerNodeSelectingEvent(new SelectionEventArgs() { IsMultiSelect = AllowMultiSelection, IsCtrKey = false, IsShiftKey = false, Nodes = null, NodeData = new NodeData() { Id = selectedNodes[i] } });
                if (i == (selectedNodes.Count - 1))
                {
                    treeviewSelectionUpdate = false;
                }
            }
        }

        /// <summary>
        /// Specifies the particular property is changes or not.
        /// </summary>
        /// <returns>"Task".</returns>
        protected IDictionary<string, object> GetPropertyChanges()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            if (PropertyChanges.ContainsKey(nameof(AllowDragAndDrop)))
            {
                properties.Add(TREEVIEWALLOWDRAGANDDROP, AllowDragAndDrop);
            }

            if (PropertyChanges.ContainsKey(nameof(AllowEditing)))
            {
                properties.Add(TREEVIEWALLOWEDITING, AllowEditing);
            }

            if (PropertyChanges.ContainsKey(nameof(AllowTextWrap)))
            {
                properties.Add(TEXTWRAP, AllowTextWrap);
            }

            if (PropertyChanges.ContainsKey(nameof(ShowCheckBox)))
            {
                properties.Add(TREEVIEWSHOWCHECKBOX, ShowCheckBox);
            }

            if (PropertyChanges.ContainsKey(nameof(EnableRtl)))
            {
                properties.Add(RTLENABLE, EnableRtl);
            }

            if (PropertyChanges.ContainsKey(nameof(Disabled)))
            {
                properties.Add(TREEVIEWDISABLED, Disabled);
            }

            if (PropertyChanges.ContainsKey(nameof(DropArea)))
            {
                properties.Add(DRAGAREA, DropArea);
            }

            if (PropertyChanges.ContainsKey(nameof(FullRowSelect)))
            {
                properties.Add(TREEVIEWFULLROWSELECT, FullRowSelect);
            }

            if (PropertyChanges.ContainsKey(nameof(CssClass)))
            {
                properties.Add(TREEVIEWCSSCLASS, CssClass);
            }

            if (PropertyChanges.ContainsKey(nameof(ExpandOn)))
            {
                properties.Add(EXPANDONTYPE, ExpandOn);
            }

#pragma warning disable CS0618 // Type or member is obsolete
            if (PropertyChanges.ContainsKey(nameof(Animation)))
#pragma warning restore CS0618 // Type or member is obsolete
            {
                properties.Add(TREEVIEWANIMATIONCLASS, TreeViewAnimation = AnimationSettings = TreeViewAnimation != null ? TreeViewAnimation : AnimationSettings);
            }

            return properties;
        }

        // Update checked nodes state for TreeView component.
        private async Task UpdateCheckedState()
        {
            List<string> checkedNodes = GetCheckedNodes();
            foreach (var item in AllCheckedNodes.Where(y => (string)y.Value == "intermediate").ToList())
            {
                AllCheckedNodes.Remove(item.Key);
            }

            if (AutoCheck)
            {
                ListReference.ChildItems = new List<TValue>();
                await UpdateCheckedNodeState(checkedNodes, CHECK, false);
            }

            ListReference.ListUpdated();
        }

        private async Task CheckNodes(List<string> checkedNodes)
        {
            AllCheckedNodes.Clear();
            int nodesLength = checkedNodes.Count;
            for (int i = 0; i < nodesLength; i++)
            {
                SfBaseUtils.UpdateDictionary(checkedNodes[i], TRUE, AllCheckedNodes);
            }

            await UpdateCheckedNodes();
        }

        // Update the htmlattribute values to TreeView component.
        private void UpdateHtmlAttr()
        {
            if (SfHtmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> item in SfHtmlAttributes)
                {
                    if (item.Key == CLASS)
                    {
                        treeViewClassList += SPACE + item.Value;
                        SfBaseUtils.UpdateDictionary(CLASS, treeViewClassList, attributes);
                    }
                    else if (item.Key == ROLE)
                    {
                        role = item.Value.ToString();
                    }
                    else if (item.Key == TABINDEX)
                    {
                        tabIndex = item.Value.ToString();
                    }
                    else if (item.Key == ACTIVEDESCENDENT)
                    {
                        activeDescendant = item.Value.ToString();
                    }
                    else
                    {
                        SfBaseUtils.UpdateDictionary(item.Key, item.Value, attributes);
                    }
                }
            }
        }

        // Get the TreeView component instance.
        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> treeObj = new Dictionary<string, object>();
            treeObj.Add(RTLENABLE, EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl));
            treeObj.Add(EXPANDONTYPE, ExpandOn);
            treeObj.Add(TREEVIEWANIMATIONCLASS, AnimationSettings);
            treeObj.Add(TREEVIEWALLOWMULTISELECTION, AllowMultiSelection);
            treeObj.Add(TREEVIEWSHOWCHECKBOX, ShowCheckBox);
            treeObj.Add(TREEVIEWALLOWEDITING, AllowEditing);
            treeObj.Add(TEXTWRAP, AllowTextWrap);
            treeObj.Add(TREEVIEWALLOWDRAGANDDROP, AllowDragAndDrop);
            treeObj.Add(DRAGAREA, DropArea);
            treeObj.Add(TREEVIEWFULLROWSELECT, FullRowSelect);
            treeObj.Add(TREEVIEWCSSCLASS, CssClass);
            treeObj.Add(TREEVIEWDISABLED, Disabled);
            treeObj.Add(HASTEMPLATE, treeViewHasTemplate);
            return treeObj;
        }

        /// <summary>
        /// Drop Node as Sibling for TreeView component.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <param name="args">"Specifies the DropTree argument".</param>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DropNodeAsSibling(DropTreeArgs args)
        {
            TValue removedData = default(TValue);
            SfTreeView<TValue> DraggedTree = null;
            bool isExternalDrag = (bool)args?.IsExternalDrag;
            string dragLi = args.DragLi;
            string dropLi = args.DropLi;
            string dragParentLi = args.DragParentLi;
            string dropParentLi = args.DropParentLi;
            bool pre = args.Pre;
            if (isExternalDrag)
            {
                DraggedTree = (SfTreeView<TValue>)args.SrcTree.Value;
            }

            if (ListReference.DataType == TreeViewDataType.SelfReferential)
            {
                removedData = isExternalDrag ? DraggedTree.ListReference.GetRemovedSelfData(dragLi?.ToString(), true) : ListReference.GetRemovedSelfData(dragLi?.ToString(), true);
                if (removedData != null)
                {
                    removedData.GetType().GetProperty(TreeViewFields?.ParentID).SetValue(removedData, IsNumberTypeId ? (object)Convert.ToInt32(dropParentLi?.ToString(), CultureInfo.InvariantCulture) : dropParentLi);
                    ListReference.DropNodeAsSiblingNode(dropLi, pre, removedData, dropLi == null && isExternalDrag ? true : false);
                    if (dragParentLi != null)
                    {
                        List<TValue> childNodes = isExternalDrag ? DraggedTree?.ListReference.GroupingData(dragParentLi, DraggedTree.ListReference?.DataSource.ToList()) : ListReference.GroupingData(dragParentLi, ListReference?.DataSource.ToList());
                        if (childNodes == null || childNodes.Count == 0)
                        {
                            if (isExternalDrag)
                            {
                                DraggedTree.ListReference.GetRemovedSelfData(dragParentLi, false, true);
                            }
                            else
                            {
                                ListReference.GetRemovedSelfData(dragParentLi, false, true);
                            }
                        }
                    }
                    if (isExternalDrag)
                    {
                        List<TValue> TargetDataList = ListReference.DataSource?.ToList();
                        await UpdateDraggedTree(DraggedTree, dragLi, TargetDataList);
                    }
                    else
                    {
                        await UpdateData(ListReference?.DataSource.ToList());
                    }
                }
            }
            else if (ListReference.DataType == TreeViewDataType.Hierarchical)
            {
                removedData = GetDraggedData(dragLi, DraggedTree, isExternalDrag);
                if (removedData != null)
                {
                    ListReference.DropNodeAsSiblingNodeHier(dragLi, dropLi, pre, removedData, ListReference.ItemsData.ToList(), default(TValue), dropLi == null && isExternalDrag ? true : false);
                    await UpdateData(ListReference.ItemsData.ToList());
                    if (isExternalDrag)
                    {
                        await UpdateDraggedTree(DraggedTree);
                    }
                }
            }
        }

        private async Task UpdateDraggedTree(SfTreeView<TValue> draggedTree, string dragLi = null, List<TValue> dataList = null)
        {
            List<TValue> draggeddataList = draggedTree?.ListReference.DataSource?.ToList();
            if (ListReference.DataType == TreeViewDataType.SelfReferential)
            {
                List<TValue> childData;
                childData = draggedTree.ListReference.GroupingData(dragLi, null);
                for (int i = 0; i < childData?.Count; i++)
                {
                    dataList.Add(childData[i]);
                    draggeddataList.Remove(childData[i]);
                    UpdateNestedChild(draggedTree, childData[i], dataList);
                }

                await UpdateData(dataList);
            }

            await draggedTree?.UpdateData(draggeddataList);
        }

        private void UpdateNestedChild(SfTreeView<TValue> draggedTree, TValue childData, List<TValue> dataList)
        {
            List<TValue> draggeddataList = draggedTree?.ListReference.DataSource?.ToList();
            List<TValue> subChildData;
            string parentLi = draggedTree.TreeViewFields.Id != null && DataUtil.GetObject(draggedTree.TreeViewFields.Id.ToString(), childData) != null ? DataUtil.GetObject(draggedTree.TreeViewFields.Id.ToString(), childData).ToString() : null;
            subChildData = draggedTree.ListReference.GroupingData(parentLi, null);
            for (int j = 0; j < subChildData?.Count; j++)
            {
                dataList.Add(subChildData[j]);
                draggeddataList.Remove(subChildData[j]);
                UpdateNestedChild(draggedTree, subChildData[j], dataList);
            }
        }

        private TValue GetDraggedData(string dragLi, SfTreeView<TValue> draggedTree, bool externalDrag)
        {
            List<TValue> itemsData;
            TValue removedData = default(TValue);
            if (externalDrag)
            {
                itemsData = draggedTree.ListReference.ItemsData.ToList();
                draggedTree.ListReference.GetAndRemovedHierData(dragLi, itemsData, null);
                removedData = draggedTree.TreeviewRemovedData;
            }
            else
            {
                itemsData = ListReference.ItemsData.ToList();
                ListReference.GetAndRemovedHierData(dragLi, itemsData, null);
                removedData = TreeviewRemovedData;
            }

            return removedData;
        }

        private void UpdateDragExpanded(string dropLi)
        {
            if (!AllExpandedNodes.Contains(dropLi))
            {
                AllExpandedNodes.Add(dropLi.ToString());
            }
            if (!InternalExpandedNodes.Contains(dropLi))
            {
                InternalExpandedNodes.Add(dropLi);
            }
        }

        private async Task UpdateDragData(bool isExternalDrag, string dragLi, SfTreeView<TValue> draggedTree, List<TValue> dataList)
        {
            if (isExternalDrag)
            {
                await UpdateDraggedTree(draggedTree, dragLi, dataList);
            }
            else
            {
                await UpdateData(dataList);
            }
        }

        /// <summary>
        /// Drop Node as Sibling for TreeView component.
        /// </summary>
        /// <param name="args">"Specifies the DropTree argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task DropNodeAsChild(DropTreeArgs args)
        {
            SfTreeView<TValue> DraggedTree = null;
            bool isExternalDrag = (bool)args?.IsExternalDrag;
            string dragLi = args.DragLi;
            string dropLi = args.DropLi;
            string dragParentLi = args.DragParentLi;
            string dropParentLi = args.DropParentLi;
            bool pre = args.Pre;
            if (isExternalDrag)
            {
                DraggedTree = (SfTreeView<TValue>)args.SrcTree.Value;
            }

            TValue removedData = default(TValue);
            if (ListReference.DataType == TreeViewDataType.SelfReferential)
            {
                removedData = isExternalDrag ? DraggedTree.ListReference.GetRemovedSelfData(dragLi, true) : ListReference.GetRemovedSelfData(dragLi, true);
                if (removedData != null)
                {
                    removedData.GetType().GetProperty(TreeViewFields?.ParentID).SetValue(removedData, IsNumberTypeId ? (object)Convert.ToInt32(dropLi?.ToString(), CultureInfo.InvariantCulture) : dropLi);
                    UpdateDragExpanded(dropLi);
                    if (TreeViewFields.HasChildren != null)
                    {
                        TValue addedData = default(TValue);
                        addedData = ListReference.GetRemovedSelfData(dropLi.ToString());
                        addedData.GetType().GetProperty(TreeViewFields?.HasChildren)?.SetValue(addedData, true);
                    }
                    ExpandedNodes = InternalExpandedNodes.ToArray();
                    if (dragParentLi != null)
                    {
                        List<TValue> childNodes = isExternalDrag ? DraggedTree?.ListReference.GroupingData(dragParentLi.ToString(), DraggedTree.ListReference?.DataSource.ToList()) : ListReference.GroupingData(dragParentLi.ToString(), ListReference?.DataSource.ToList());
                        if (childNodes == null || childNodes.Count == 0)
                        {
                            if (isExternalDrag) { DraggedTree.ListReference.GetRemovedSelfData(dragParentLi.ToString(), false, true); }
                            else { ListReference.GetRemovedSelfData(dragParentLi.ToString(), false, true); }
                        }
                    }
                    List<TValue> dataList = ListReference.DataSource?.ToList();
                    dataList.Add(removedData);
                    await UpdateDragData(isExternalDrag, dragLi, DraggedTree, dataList);
                }
            }
            else if (ListReference.DataType == TreeViewDataType.Hierarchical)
            {
                removedData = GetDraggedData(dragLi, DraggedTree, isExternalDrag);
                if (removedData != null)
                {
                    ListReference.AddChildData(dropLi?.ToString(), removedData, InternalData, true);
                    UpdateDragExpanded(dropLi);
                    ExpandedNodes = InternalExpandedNodes.ToArray();
                    await UpdateData(ListReference.ItemsData.ToList());
                    if (isExternalDrag)
                    {
                        await UpdateDraggedTree(DraggedTree);
                    }
                }
            }
        }

        /// <summary>
        /// Trigger Node Drag Start Event for TreeView component.
        /// </summary>
        /// <param name="args">"Node Drag Start argument".</param>
        /// <param name="left">"Dragged Node position".</param>
        /// <param name="top">"Dragged Node top position".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerDragStartEvent(DragAndDropEventArgs args, double left, double top)
        {
            if (args != null)
            {
                args.Name = "OnNodeDragStart";
                args.Left = left;
                args.Top = top;
                await SfBaseUtils.InvokeEvent<DragAndDropEventArgs>(TreeviewEvents?.OnNodeDragStart, args);
                await InvokeMethod("sfBlazor.TreeView.dragStartActionContinue", new object[] { Element, args.Cancel });
            }
        }

        /// <summary>
        /// Trigger Node Dragging Event for TreeView component.
        /// </summary>
        /// <param name="args">"Node Drag Start argument".</param>
        /// <param name="left">"Dragging Node position".</param>
        /// <param name="top">"Dragging Node top position".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeDraggingEvent(DragAndDropEventArgs args, double left, double top)
        {
            if (args != null)
            {
                args.Name = "OnNodeDragged";
                args.Left = left;
                args.Top = top;
                if (ListReference.DataType != TreeViewDataType.RemoteData && args.DraggedNodeData.Id != null)
                {
                    args.DraggedNodeData = ListReference.GetNodeDetails(args.DraggedNodeData.Id);
                }

                await SfBaseUtils.InvokeEvent<DragAndDropEventArgs>(TreeviewEvents?.OnNodeDragged, args);
                if (!args.Cancel)
                {
                    await InvokeMethod("sfBlazor.TreeView.nodeDragging", new object[] { Element });
                }
            }
        }

        /// <summary>
        /// Trigger Node Drag Stop Event for TreeView component.
        /// </summary>
        /// <param name="args">"Node Drop Start argument".</param>
        /// <param name="left">"Drag stop Node position".</param>
        /// <param name="top">"Drag stop Node top position".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerDragStopEvent(DragAndDropEventArgs args, double left, double top)
        {
            if (args != null)
            {
                args.Name = "OnNodeDragStop";
                args.Left = left;
                args.Top = top;
                if (ListReference.DataType != TreeViewDataType.RemoteData && args.DraggedNodeData.Id != null)
                {
                    args.DraggedNodeData = ListReference.GetNodeDetails(args.DraggedNodeData.Id);
                }
                await SfBaseUtils.InvokeEvent<DragAndDropEventArgs>(TreeviewEvents?.OnNodeDragStop, args);
                await InvokeMethod("sfBlazor.TreeView.dragNodeStop", new object[] { Element, args });
            }
        }

        /// <summary>
        /// Trigger Node Drag Stop Event for TreeView component.
        /// </summary>
        /// <param name="args">"Dropped argument".</param>
        /// <param name="left">"Dropped stop Node position".</param>
        /// <param name="top">"Dropped stop Node top position".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeDropped(DragAndDropEventArgs args, double left, double top)
        {
            if (args != null)
            {
                args.Name = "NodeDropped";
                args.Left = left;
                args.Top = top;
                IsNodeDropped = true;
                await SfBaseUtils.InvokeEvent<DragAndDropEventArgs>(TreeviewEvents?.NodeDropped, args);
                IsNodeDropped = false;
            }
        }

        /// <summary>
        /// Trigger Node Drag Stop Event for TreeView component.
        /// </summary>
        /// <param name="parentNodes">"Parent node".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateExpandedNode(string[] parentNodes)
        {
            if (parentNodes != null)
            {
                for (int i = 0; i < parentNodes.Length; i++)
                {
                    if (!InternalExpandedNodes.Contains(parentNodes[i]))
                    {
                        Internal.ExpandEventArgs nodeArgs = new Internal.ExpandEventArgs();
                        nodeArgs.NodeData = new NodeData();
                        nodeArgs.NodeData.Id = parentNodes[i];
                        nodeArgs.NodeData.HasChildren = true;
                        nodeArgs.IsLoaded = false;
                        await TriggerNodeExpandingEvent(nodeArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Trigger Node Selecting Event for TreeView component.
        /// </summary>
        /// <param name="selectEventArgs">"Select event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeSelectingEvent(SelectionEventArgs selectEventArgs)
        {
            NodeSelectEventArgs args = new NodeSelectEventArgs() { Action = selectEventArgs?.Action, IsInteracted = selectEventArgs.IsInteracted, NodeData = selectEventArgs.NodeData };
            string parentId = args.NodeData.ParentID;
            args.Name = args.Action == "select" ? "NodeSelecting" : "NodeUnSelecting";
            if (ListReference.DataType != TreeViewDataType.RemoteData)
            {
                args.NodeData = ListReference.GetNodeDetails(args.NodeData.Id);
            }
            args.NodeData.ParentID = parentId;
            await SfBaseUtils.InvokeEvent<NodeSelectEventArgs>(TreeviewEvents?.NodeSelecting, args);
            if (!args.Cancel)
            {
                args.Name = args.Action == "select" ? "NodeSelected" : "NodeUnSelected";
                if (args.Action == "un-select" && AllSelectedNodes.Contains(args.NodeData.Id))
                {
                    AllSelectedNodes.Remove(args.NodeData.Id);
                }
                else
                {
                    await SelectionActionAsync(args, selectEventArgs.IsMultiSelect, selectEventArgs.IsCtrKey, selectEventArgs.IsShiftKey, selectEventArgs.Nodes);
                }
                if (selectEventArgs.IsInteracted) { IsTreeInteracted = true; }
                await UpdateSelectedNodes();
                if ((TreeviewEvents == null) || !(TreeviewEvents.NodeSelecting.HasDelegate))
                {
                    ListReference.ListUpdated();
                }
                await SfBaseUtils.InvokeEvent<NodeSelectEventArgs>(TreeviewEvents?.NodeSelected, args);
                await ListReference.UpdatePersistence();
            }
        }

        // User interactions to update the selected node values in TreeView component.
        private async Task SelectionActionAsync(NodeSelectEventArgs args, bool multiSelect, bool ctrKey, bool shiftKey, string[] array)
        {
            if (!AllowMultiSelection || (!multiSelect && !ctrKey))
            {
                AllSelectedNodes.Clear();
            }

            if (AllowMultiSelection && shiftKey && array.Length > 0)
            {
                AllSelectedNodes = array.ToList();
            }
            else if (AllowMultiSelection && array == null && !AllSelectedNodes.Contains(args.NodeData.Id))
            {
                if (!treeviewSelectionUpdate)
                {
                    AllSelectedNodes.Clear();
                    treeviewSelectionUpdate = true;
                }

                AllSelectedNodes.Add(args.NodeData.Id);
            }
            else if (!AllSelectedNodes.Contains(args.NodeData.Id))
            {
                AllSelectedNodes.Add(args.NodeData.Id);
            }

            await UpdateSelectedNodes();
        }

        /// <summary>
        /// Trigger Node Editing Event for TreeView component.
        /// </summary>
        /// <param name="args">"Node Edit event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeEditingEvent(NodeEditEventArgs args)
        {
            EdittedNodeId = args?.NodeData.Id;
            args.NodeData = GetNode(args.NodeData.Id);
            args.OldText = args.NodeData.Text;
            args.Name = "NodeEditing";
            await InvokeMethod("sfBlazor.TreeView.updateOldText", new object[] { Element, args.OldText });
            if (args != null)
            {
                await SfBaseUtils.InvokeEvent<NodeEditEventArgs>(TreeviewEvents?.NodeEditing, args);
                if (args.Cancel)
                {
                    EdittedNodeId = null;
                }

                ListReference.ListUpdated();
            }
        }

        internal async Task TriggerNodeEdittedEvent(string newText)
        {
            NodeEditEventArgs eventArgs = new NodeEditEventArgs();
            eventArgs.NewText = newText;
            eventArgs.NodeData = ListReference.GetNodeDetails(EdittedNodeId);
            eventArgs.OldText = eventArgs.NodeData.Text;
            eventArgs.Name = "NodeEditted";
            await SfBaseUtils.InvokeEvent<NodeEditEventArgs>(TreeviewEvents?.NodeEdited, eventArgs);
            newText = eventArgs.Cancel ? eventArgs.OldText : eventArgs.NewText;
            ListReference.UpdateSelfNodeText(EdittedNodeId, newText);
            EdittedNodeId = null;
            ListReference.ListUpdated();
            await TriggerDataSourceChangedEvent();
            if (ListReference.DataType == TreeViewDataType.RemoteData)
            {
                if (TreeViewFields != null && TreeViewFields.DataManager != null && !TreeViewFields.DataManager.Offline)
                {
                    await TreeViewFields.DataManager.Update<TValue>(TreeViewFields.Id, GetTreeData(eventArgs.NodeData.Id)[0], TreeViewFields.Query?.FromTable, TreeViewFields.Query);
                }
            }
            if (AllowTextWrap)
            {
                await InvokeMethod("sfBlazor.TreeView.updateTextWrap", new object[] { Element });
            }
        }

        internal async Task TriggerDataSourceChangedEvent()
        {
            await InvokeMethod("sfBlazor.TreeView.dataSourceChanged", Element);
            DataSourceChangedEventArgs<TValue> args = new DataSourceChangedEventArgs<TValue>()
            {
                Name = "DataSourceChanged",
#pragma warning disable CS0618 // Type or member is obsolete
                Data = TreeViewFields?.DataSource?.ToList()
#pragma warning restore CS0618 // Type or member is obsolete
            };
            await SfBaseUtils.InvokeEvent<DataSourceChangedEventArgs<TValue>>(TreeviewEvents?.DataSourceChanged, args);
        }

        /// <summary>
        /// Trigger TreeView created event.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreatedEvent()
        {
            await SfBaseUtils.InvokeEvent<ActionEventArgs>(TreeviewEvents?.Created, null);
        }

        /// <summary>
        /// Trigger Node Expanding Event for TreeView component.
        /// </summary>
        /// <param name="arguments">"Expand event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeExpandingEvent(Internal.ExpandEventArgs arguments)
        {
            bool isUpdated = false;
            if (arguments != null)
            {
                NodeExpandEventArgs args = new NodeExpandEventArgs() { Event = arguments.Event, IsInteracted = arguments.IsInteracted, NodeData = arguments.NodeData };
                var offline = TreeViewFields.DataManager?.Offline ?? false;
                args.Name = "NodeExpanding";
                Target = args.NodeData?.Id;
                if (ListReference.DataType != TreeViewDataType.RemoteData)
                {
                    args.NodeData = ListReference.GetNodeDetails(args.NodeData.Id);
                }

                await SfBaseUtils.InvokeEvent<NodeExpandEventArgs>(TreeviewEvents?.NodeExpanding, args);
                if (!args.Cancel)
                {
                    if (ListReference.DataType == TreeViewDataType.RemoteData)
                    {
                        IEnumerable<TValue> childData = (IEnumerable<TValue>)ListReference.GetChildRemoteData(args.NodeData?.Id);
                        if (LoadOnDemand && !offline && !arguments.IsLoaded)
                        {
                            await ListReference.RenderRemoteLi(args.NodeData?.Id, arguments.NodeLevel, childData != null);
                        }
                    }

                    if (!arguments.IsLoaded)
                    {
                        IsNodeExpanded = true;
                        if (args.IsInteracted || TreeExpandAll)
                        {
                            IsLoaded = false;
                            LoadedId = args.NodeData?.Id;
                        }
                        NodeLen = 0;
                    } else
                    {
                        IsLoaded = true;
                    }

                    if (!InternalExpandedNodes.Contains(args.NodeData.Id))
                    {
                        InternalExpandedNodes.Add(args.NodeData.Id);
                        isUpdated = true;
                    }

                    if (ListReference != null && ListReference.DataType == TreeViewDataType.RemoteData && ListReference.RemoteExpandedValues != null && !ListReference.RemoteExpandedValues.Contains(args.NodeData.Id))
                    {
                        ListReference.RemoteExpandedValues.Add(args.NodeData.Id);
                    }

                    if (!AllExpandedNodes.Contains(args.NodeData.Id))
                    {
                        AllExpandedNodes.Add(args.NodeData.Id);
                        isUpdated = true;
                    }

                    if (isUpdated)
                    {
                        await UpdateExpandedNodes();
                        ListReference.ListStateChanged(args.NodeData.Id);
                    }
                    IsLoaded = true;
                    TreeExpandAll = false;
                    if (args.IsInteracted == false && ExpandedNodesChanged.HasDelegate)
                    {
                        IsTreeNodeExpandingCall = true;
                    }
                    if (UpdateIcon(args.NodeData.Id))
                    {
                        await InvokeMethod("sfBlazor.TreeView.expandedNode", new object[] { Element, args });
                    }
                }

                if (ListReference != null)
                {
                    await ListReference.UpdatePersistence();
                }
            }
        }

        private List<string> GetCheckedNodes()
        {
            List<string> checkedNodes = new List<string>();
            foreach (KeyValuePair<string, object> checkedNode in AllCheckedNodes)
            {
                if (checkedNode.Value.ToString() == TRUE)
                {
                    checkedNodes.Add(checkedNode.Key);
                }
            }

            return checkedNodes;
        }

        /// <summary>
        /// Trigger Node Checking Event for TreeView component.
        /// </summary>
        /// <param name="args">"NodeCheck event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeCheckingEvent(NodeCheckEventArgs args)
        {
            if (args != null)
            {
                args.Name = args.Action == "check" ? "NodeChecking" : "NodeUnChecking";
                if (ListReference.DataType != TreeViewDataType.RemoteData)
                {
                    args.NodeData = ListReference.GetNodeDetails(args.NodeData.Id);
                }

                await UpdateCheckedNodes();
                await SfBaseUtils.InvokeEvent<NodeCheckEventArgs>(TreeviewEvents?.NodeChecking, args);
                if (!args.Cancel)
                {
                    if (AllCheckedNodes.ContainsKey(args.NodeData.Id) && args.Action != CHECK)
                    {
                        AllCheckedNodes.Remove(args.NodeData.Id);
                    }
                    else
                    {
                        SfBaseUtils.UpdateDictionary(args.NodeData.Id, TRUE, AllCheckedNodes);
                    }

                    if (AutoCheck)
                    {
                        if (args.Action == CHECK)
                        {
                            if (AllCheckedNodes.ContainsKey(args.NodeData.Id))
                            {
                                AllCheckedNodes[args.NodeData.Id] = TRUE;
                            }
                            else
                            {
                                SfBaseUtils.UpdateDictionary(args.NodeData.Id, TRUE, AllCheckedNodes);
                            }

                            List<string> checkedNodes = GetCheckedNodes();
                            ListReference.ChildItems = new List<TValue>();
                            await UpdateCheckedNodeState(checkedNodes, args.Action, false);
                        }
                        else
                        {
                            AllCheckedNodes.Remove(args.NodeData.Id);
                            await UpdateCheckedNodeState(new List<string>() { args.NodeData.Id }, args.Action, true);
                        }
                    }

                    await UpdateCheckedNodes();
                    ListReference.ListUpdated();
                    args.Name = args.Action == "check" ? "NodeChecked" : "NodeUnChecked";
                    await SfBaseUtils.InvokeEvent<NodeCheckEventArgs>(TreeviewEvents?.NodeChecked, args);
                    await ListReference.UpdatePersistence();
                }
            }
        }

        private void UpdateCheckedValueToDatasource()
        {
            List<string> checkedNodes = AllCheckedNodes.Where(x => x.Value == "true" as object).Select(x => x.Key)?.ToList();
            if (TreeViewFields?.IsChecked != null)
            {
                if (ListReference?.DataType == TreeViewDataType.Hierarchical)
                {
                    UpdateHierarchicalData(checkedNodes, TreeViewFields?.DataSource?.ToList(), TreeViewFields?.IsChecked);
                }
                else if (ListReference?.DataType == TreeViewDataType.SelfReferential)
                {
                    UpdateSelfReferentialData(checkedNodes, TreeViewFields?.DataSource?.ToList(), TreeViewFields?.IsChecked);
                }
            }
        }

        internal void UpdateSelfReferentialData(List<string> id, IEnumerable<TValue> dataSource, string propertyName)
        {
            string idAttrValue;
            List<TValue> dataList = dataSource?.ToList();
            int dataLength = dataList != null ? dataList.Count : 0;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = ListGeneration<TValue>.GetAttrValues(TreeViewFields?.Id, dataList[i]);
                if (CheckedNodesChanged.HasDelegate)
                {
                    if (id.Contains(idAttrValue))
                    {
                        dataList[i].GetType().GetProperty(propertyName).SetValue(dataList[i], true);
                    }
                    else
                    {
                        dataList[i].GetType().GetProperty(propertyName).SetValue(dataList[i], false);
                    }
                }
            }

            ListReference.DataSource = dataList;
            InternalData = ListReference.DataSource?.ToList();
            TreeViewFields.GetType().GetProperty("DataSource").SetValue(TreeViewFields, InternalData);
            ListReference.ItemsData = ListReference.DataSource;
        }

        internal void UpdateHierarchicalData(List<string> id, IEnumerable<TValue> dataSource, string propertyName)
        {
            string idAttrValue;
            List<TValue> dataList = dataSource?.ToList();
            int dataLength = dataList != null ? dataList.Count : 0;
            IEnumerable<TValue> childs;
            TreeViewFieldsSettings<TValue> fields = TreeViewFields;
            for (int i = 0; i < dataLength; i++)
            {
                idAttrValue = ListGeneration<TValue>.GetAttrValues(fields.Id, dataList[i]);
                childs = fields.Child != null ? ListGeneration<TValue>.GetChild(fields.Child.ToString(), dataList[i]) : null;
                if (CheckedNodesChanged.HasDelegate)
                {
                    if (id.Contains(idAttrValue))
                    {
                        dataList[i].GetType().GetProperty(propertyName).SetValue(dataList[i], true);
                    }
                    else
                    {
                        dataList[i].GetType().GetProperty(propertyName).SetValue(dataList[i], false);
                    }
                }

                if (childs != null)
                {
                    UpdateHierarchicalData(id, (List<TValue>)childs, propertyName);
                }
            }

            ListReference.DataSource = dataList;
            InternalData = ListReference.DataSource?.ToList();
            TreeViewFields.GetType().GetProperty("DataSource").SetValue(TreeViewFields, InternalData);
            ListReference.ItemsData = ListReference.DataSource;
        }

        // Update checked Nodes state for TreeView component.
        private async Task UpdateCheckedNodeState(List<string> checkedNodes, string action, bool flag)
        {
            if (ListReference.DataType == TreeViewDataType.SelfReferential)
            {
                ListReference.UpdateChildCheckedNodes(checkedNodes, action);
                ListReference.UpdateSelfIntermediateState(checkedNodes);
            }
            else if (ListReference.DataType == TreeViewDataType.Hierarchical)
            {
                ListReference.UpdateHierarchicalChildCheckedNodes(checkedNodes, action);
                ListReference.UpdateCheckedDataFromDS(flag ? GetCheckedNodes() : checkedNodes, action);
            }
            else
            {
                await CheckNodes(checkedNodes);
            }
        }

        /// <summary>
        /// Trigger Node Click Event for TreeView component.
        /// </summary>
        /// <param name="eventArgs">"NodeClick event argument".</param>
        /// <param name="id">"Clicked node id".</param>
        /// <param name="left">"Clicked node Left position".</param>
        /// <param name="top">"Clicked node Top position".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeClickingEvent(ClickEventArgs eventArgs, string id, double left, double top)
        {
            if (eventArgs != null)
            {
                NodeClickEventArgs args = new NodeClickEventArgs()
                {
                    Name = "NodeClicked",
                    NodeData = GetNode(id),
                    Event = eventArgs,
                    Left = left,
                    Top = top
                };
                await SfBaseUtils.InvokeEvent<NodeClickEventArgs>(TreeviewEvents?.NodeClicked, args);
            }
        }

        private bool UpdateIcon(string id)
        {
            bool hasChild = true;
            if (ListReference.DataType == TreeViewDataType.SelfReferential)
            {
                List<TValue> childNodes = ListReference.GroupingData(id, ListReference?.DataSource.ToList());
                if (childNodes == null || childNodes.Count == 0)
                {
                    ListReference.GetRemovedSelfData(id, false, true);
                    ListReference.ListUpdated();
                    hasChild = false;
                }
            }

            return hasChild;
        }

        /// <summary>
        /// Trigger Node Expanded Event for TreeView component.
        /// </summary>
        /// <param name="args">"Node Expanded event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeExpandedEvent(NodeExpandEventArgs args)
        {
            if (args != null)
            {
                args.Name = "NodeExpanded";
                if (ListReference.DataType != TreeViewDataType.RemoteData)
                {
                    args.NodeData = ListReference.GetNodeDetails(args.NodeData.Id);
                }
                Target = null;
                await SpinnerRef?.HideAsync();
                SpinnerRef?.Dispose();
                IsTreeNodeExpandingCall = false;
                await SfBaseUtils.InvokeEvent<NodeExpandEventArgs>(TreeviewEvents?.NodeExpanded, args);
            }
        }

        /// <summary>
        /// Update the latest data source values to TreeView component (Drag and drop).
        /// </summary>
        /// <param name="dataSource">"Specifies the datasource".</param>
        /// <param name="isUpdateChecked">"Specifies the checked is true or not".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateData(List<TValue> dataSource, bool isUpdateChecked = false)
        {
            if (!SfBaseUtils.Equals(InternalData, dataSource) && ListReference != null && isUpdateChecked)
            {
                if (IsClearStateCall) { 
                    await ClearStateAsync();
                    IsClearStateCall = false;
                }
                await ListReference.TriggerDataBoundEvent(dataSource);
            }

            if (TreeViewFields?.DataSource != null && TreeViewFields?.DataManager == null)
            {
                InternalData = dataSource != null ? ListGeneration<TValue>.GetSortedData(dataSource.ToList(), SortOrder.ToString(), TreeViewFields.Text) : new List<TValue>();
            }

            ListReference.ListData = InternalData;
            ListReference.DataSource = InternalData;
            if (InternalData != null)
            {
                ListReference.ItemsData = InternalData;
                await ListReference.IdentifyDataSource(isUpdateChecked);
                ListReference.ListUpdated();
            }
            if (isUpdateChecked)
            {
                await TriggerDataSourceChangedEvent();
            }
        }

        /// <summary>
        /// Collapse Action for TreeView.
        /// </summary>
        /// <param name="args">"NodeCollapsing event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeCollapsingEvent(NodeExpandEventArgs args)
        {
            bool isUpdated = false;
            if (args != null)
            {
                args.Name = "NodeCollapsing";
                if (ListReference.DataType != TreeViewDataType.RemoteData)
                {
                    args.NodeData = ListReference.GetNodeDetails(args.NodeData.Id);
                }

                await SfBaseUtils.InvokeEvent<NodeExpandEventArgs>(TreeviewEvents?.NodeCollapsing, args);
                if (!args.Cancel)
                {
                    if (InternalExpandedNodes.Contains(args.NodeData.Id))
                    {
                        InternalExpandedNodes.RemoveAt(InternalExpandedNodes.IndexOf(args.NodeData.Id));
                        isUpdated = true;
                    }
                    if (AllExpandedNodes.Contains(args.NodeData.Id) && !ExpandedNodesChanged.HasDelegate)
                    {
                        AllExpandedNodes.RemoveAt(AllExpandedNodes.IndexOf(args.NodeData.Id));
                        isUpdated = true;
                    }
                    if (isUpdated)
                    {
                        await UpdateExpandedNodes();
                    }
                    if (args.IsInteracted == true && ExpandedNodesChanged.HasDelegate)
                    {
                        IsTreeNodeExpandingCall = true;
                    }
                    await InvokeMethod("sfBlazor.TreeView.collapsedNode", new object[] { Element, args });
                }

                if (ListReference != null)
                {
                    await ListReference.UpdatePersistence();
                }
            }
        }

        /// <summary>
        /// Collapsed Action for TreeView.
        /// </summary>
        /// <param name="args">"Key press event argument".</param>
        /// <param name="id">"Specifies the Id".</param>
        /// <param name="keyAction">"Specifies the key action".</param>
        /// <param name="keyValue">"Specifies the Key value".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerKeyboardEvent(NodeKeyPressEventArgs args, string id, string keyAction, string keyValue)
        {
            if (args != null)
            {
                args = new NodeKeyPressEventArgs() { Name = "OnKeyPress", Action = keyAction, Key = keyValue };
                if (ListReference.DataType != TreeViewDataType.RemoteData)
                {
                    if (id != null)
                    {
                        args.NodeData = ListReference.GetNodeDetails(id);
                    }
                }

                await SfBaseUtils.InvokeEvent<NodeKeyPressEventArgs>(TreeviewEvents?.OnKeyPress, args);
                if (!args.Cancel)
                {
                    await InvokeMethod("sfBlazor.TreeView.KeyActionHandler", new object[] { Element, args, id });
                }
            }
        }

        /// <summary>
        /// Collapsed Action for TreeView.
        /// </summary>
        /// <param name="args">"NodeCollapsed event argument".</param>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerNodeCollapsedEvent(NodeExpandEventArgs args)
        {
            if (args != null)
            {
                args.Name = "NodeCollapsed";
                await SfBaseUtils.InvokeEvent<NodeExpandEventArgs>(TreeviewEvents?.NodeCollapsed, args);
            }
        }

        /// <summary>
        /// Update child property values to TreeView component instance.
        /// </summary>
        /// <param name="key">"Specifies the key field".</param>
        /// <param name="details">"Specifies the details field".</param>
        public void UpdateChildProperties(string key, object details)
        {
            switch (key)
            {
                case TREEVIEWFIELD:
                    TreeViewFields = (TreeViewFieldsSettings<TValue>)details;
                    break;
                case TEMPLATES:
                    TreeViewTemplate = (TreeViewTemplates<TValue>)details;
                    if (TreeViewTemplate != null)
                    {
                        treeViewHasTemplate = true;
                    }

                    break;
            }

            Dictionary<string, object> child = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary(key, details, child);
        }

        internal async Task UpdateSelectedNodes()
        {
            SelectedNodes = TreeSelectedNodes = await SfBaseUtils.UpdateProperty<string[]>(AllSelectedNodes?.Count > 0 ? AllSelectedNodes.ToArray() : null, TreeSelectedNodes, SelectedNodesChanged);
        }

        internal async Task UpdateExpandedNodes()
        {
            ExpandedNodes = TreeExpandedNodes = await SfBaseUtils.UpdateProperty<string[]>(InternalExpandedNodes.Count > 0 ? InternalExpandedNodes?.ToArray() : null, TreeExpandedNodes, ExpandedNodesChanged);
        }

        internal async Task UpdateCheckedNodes()
        {
            string[] checkedNodes = AllCheckedNodes.Where(x => x.Value == "true" as object).Select(x => x.Key).ToArray();
            string[] checkNodes = checkedNodes.Length > 0 ? checkedNodes : null;
            UpdateCheckedValueToDatasource();
            CheckedNodes = TreeCheckedNodes = await SfBaseUtils.UpdateProperty<string[]>(checkNodes, TreeCheckedNodes, CheckedNodesChanged);
        }

        internal async override void ComponentDispose()
        {
            if (IsRendered && !IsDestroyed)
            {
                if (TreeviewEvents != null && TreeviewEvents.Destroyed.HasDelegate)
                {
                    await SfBaseUtils.InvokeEvent<ActionEventArgs>(TreeviewEvents.Destroyed, null);
                }

                IsDestroyed = true;
                attributes = null;
                if (ListReference != null)
                {
                    ListReference.DisposeTreeOptions();
                    ListReference.DataSource = null;
                    ListReference.ItemsData = null;
                    ListReference.ChildItems = null;
                    ListReference.ListData = null;
                    ListReference = null;
                }
                SpinnerRef?.Dispose();
                InternalData = null;
                TreeviewEvents = null;
                AllExpandedNodes = null;
                InternalExpandedNodes = null;
                CurrentExpandedNodes = null;
                AllSelectedNodes = null;
                AllCheckedNodes = null;
                AllDisabledNodes = null;
            }
        }
    }
}