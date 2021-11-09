using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the TreeView component.
    /// </summary>
    public partial class SfTreeView<TValue> : SfBaseComponent, ITreeView
    {
        internal IEnumerable<TValue> InternalDataSource { get; set; } = new List<TValue>();

        [CascadingParameter]
        private ITreeView DynamicParent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfTreeView;
            SetRootAttributes();
            await base.OnInitializedAsync();
            IsDevice = await SyncfusionService.IsDevice();
            DynamicParent?.UpdateAnimationProperties(TreeViewAnimation);
            TreeSortOrder = SortOrder;
            TreeAllowDragAndDrop = AllowDragAndDrop;
            TreeAllowEditing = AllowEditing;
            TreeAllowTextWrap = AllowTextWrap;
            TreeDropArea = DropArea;
            TreeAllowMultiSelection = AllowMultiSelection;
            TreeShowCheckBox = ShowCheckBox;
            TreeAutoCheck = AutoCheck;
            TreeCssClass = CssClass;
            TreeDisabled = Disabled;
            TreeFullRowSelect = FullRowSelect;
            TreeExpandOn = ExpandOn;
            TreeViewAnimation = AnimationSettings = TreeViewAnimation != null ? TreeViewAnimation : AnimationSettings;
            TreeSelectedNodes = SelectedNodes;
            TreeExpandedNodes = ExpandedNodes;
            TreeCheckedNodes = CheckedNodes;
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">"First render parameter".</param>
        /// <returns>"Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                UpdateChildProperties(TREEVIEWFIELD, TreeViewFields);
                TreeFields = TreeViewFields;
                if (TreeViewFields?.DataSource != null && ListReference != null)
                {
                    InternalData = ListGeneration<TValue>.GetSortedData(TreeViewFields.DataSource.ToList(), SortOrder.ToString(), TreeViewFields.Text);
                    ListReference.ListData = InternalData;
                    ListReference.DataSource = InternalData;
                    if (InternalData != null)
                    {
                        ListReference.ItemsData = InternalData;
                        await ListReference.IdentifyDataSource();
                        ListReference.ListUpdated();
                    }
                }

                if (TreeviewEvents != null && TreeviewEvents.Created.HasDelegate)
                {
                    await SfBaseUtils.InvokeEvent<ActionEventArgs>(TreeviewEvents.Created, new ActionEventArgs() { Name = "Created" });
                }

                if (ListReference != null && ListReference.DataType == TreeViewDataType.RemoteData && !IsCompleteltyRendered)
                {
                    IsCompleteltyRendered = true;
                    IsDataSourceChanged = true;
                }
            } else if (AllowTextWrap && IsDataSourceChanged)
            {
                await InvokeMethod("sfBlazor.TreeView.updateTextWrap", new object[] { Element });
                IsDataSourceChanged = false;
            } else if (IsTreeNodeExpandingCall && ExpandedNodesChanged.HasDelegate)
            {
                await InvokeMethod("sfBlazor.TreeView.updateSpinnerClass", new object[] { Element });
                SpinnerRef?.HideAsync();
                SpinnerRef?.Dispose();
                IsTreeNodeExpandingCall = false;
            }
        }

        // Update the necessary property values to client side.
        internal override async Task OnAfterScriptRendered()
        {
            if (ListReference != null && ListReference.DataType != TreeViewDataType.RemoteData)
            {
                IsCompleteltyRendered = true;
                if (!IsCurrentExpandedUpdated && !SfBaseUtils.Equals(CurrentExpandedNodes, ExpandedNodes?.ToList()))
                {
                    IsCurrentExpandedUpdated = true;
                    if (EnablePersistence)
                    {
                        TreePersistenceValues localStorageValue = await InvokeMethod<TreePersistenceValues>("window.localStorage.getItem", true, new object[] { ID });
                        InternalExpandedNodes = localStorageValue.ExpandedNodes;
                    }
                    else
                    {
                        InternalExpandedNodes = CurrentExpandedNodes;
                    }

                    await UpdateExpandedNodes();
                    ListReference?.ListUpdated();
                }
            }

            await InvokeMethod("sfBlazor.TreeView.initialize", new object[] { Element, GetInstance(), DotnetObjectReference });
            if (ExpandedNodes != null)
            {
                AllExpandedNodes = ExpandedNodes.ToList();
            }
        }

#pragma warning disable CS0618 // Type or member is obsolete
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            TreeAllowMultiSelection = NotifyPropertyChanges(nameof(AllowMultiSelection), AllowMultiSelection, TreeAllowMultiSelection);
            TreeSortOrder = NotifyPropertyChanges(nameof(SortOrder), SortOrder, TreeSortOrder);
            TreeShowCheckBox = NotifyPropertyChanges(nameof(ShowCheckBox), ShowCheckBox, TreeShowCheckBox);
            TreeEnableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, TreeEnableRtl);
            TreeAutoCheck = NotifyPropertyChanges(nameof(AutoCheck), AutoCheck, TreeAutoCheck);
            TreeCssClass = NotifyPropertyChanges(nameof(CssClass), CssClass, TreeCssClass);
            TreeDisabled = NotifyPropertyChanges(nameof(Disabled), Disabled, TreeDisabled);
            TreeAllowDragAndDrop = NotifyPropertyChanges(nameof(AllowDragAndDrop), AllowDragAndDrop, TreeAllowDragAndDrop);
            TreeAllowEditing = NotifyPropertyChanges(nameof(AllowEditing), AllowEditing, TreeAllowEditing);
            TreeAllowTextWrap = NotifyPropertyChanges(nameof(AllowTextWrap), AllowTextWrap, TreeAllowTextWrap);
            TreeDropArea = NotifyPropertyChanges(nameof(DropArea), DropArea, TreeDropArea);
            TreeFields = NotifyPropertyChanges(nameof(Fields), Fields, TreeFields);
            TreeFullRowSelect = NotifyPropertyChanges(nameof(FullRowSelect), FullRowSelect, TreeFullRowSelect);
            TreeExpandOn = NotifyPropertyChanges(nameof(ExpandOn), ExpandOn, TreeExpandOn);
            TreeViewAnimation = NotifyPropertyChanges(nameof(Animation), Animation, TreeViewAnimation);
            ExpandedNodes = TreeExpandedNodes = NotifyPropertyChanges(nameof(ExpandedNodes), ExpandedNodes, TreeExpandedNodes);
            SelectedNodes = TreeSelectedNodes = NotifyPropertyChanges(nameof(SelectedNodes), SelectedNodes, TreeSelectedNodes);
            CheckedNodes = TreeCheckedNodes = NotifyPropertyChanges(nameof(CheckedNodes), CheckedNodes, TreeCheckedNodes);
            if (PropertyChanges.Count > 0)
            {
                await OnPropertyChangeHandler();
            }
        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}