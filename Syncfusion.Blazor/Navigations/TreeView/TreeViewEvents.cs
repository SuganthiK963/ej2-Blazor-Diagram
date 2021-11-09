using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// The TreeView allows us to control the component by using events.
    /// </summary>
    /// <typeparam name="TValue">"Tvalue paramter".</typeparam>
    public partial class TreeViewEvents<TValue> : SfBaseComponent
    {
        [CascadingParameter]
        private SfTreeView<TValue> Parent { get; set; }

        /// <summary>
        /// Triggers when any TreeView action failed to fetch the desired results.
        /// </summary>
        [Parameter]
        public EventCallback<FailureEventArgs> OnActionFailure { get; set; }

        /// <summary>
        /// Triggers when the TreeView control is created successfully.
        /// </summary>
        [Parameter]
        public EventCallback<ActionEventArgs> Created { get; set; }

        /// <summary>
        /// Triggers when data source is populated in the TreeView.
        /// </summary>
        [Parameter]
        public EventCallback<DataBoundEventArgs<TValue>> DataBound { get; set; }

        /// <summary>
        /// Triggers when data source is changed in the TreeView. The data source will be changed after performing some operation like
        /// drag and drop, node editing, adding and removing node.
        /// </summary>
        [Parameter]
        public EventCallback<DataSourceChangedEventArgs<TValue>> DataSourceChanged { get; set; }

        /// <summary>
        /// Triggers when the TreeView control is destroyed successfully.
        /// </summary>
        [Parameter]
        public EventCallback<ActionEventArgs> Destroyed { get; set; }

        /// <summary>
        /// Triggers before the TreeView node is appended to the TreeView element. It helps to customize specific nodes.
        /// </summary>
        [Parameter]
        public EventCallback<NodeRenderEventArgs<TValue>> OnNodeRender { get; set; }

        /// <summary>
        /// Triggers when key press is successful. It helps to customize the operations at key press.
        /// </summary>
        [Parameter]
        public EventCallback<NodeKeyPressEventArgs> OnKeyPress { get; set; }

        /// <summary>
        /// Triggers when the TreeView node is checked/unchecked successfully.
        /// </summary>
        [Parameter]
        public EventCallback<NodeCheckEventArgs> NodeChecked { get; set; }

        /// <summary>
        /// Triggers before the TreeView node is to be checked/unchecked.
        /// </summary>
        [Parameter]
        public EventCallback<NodeCheckEventArgs> NodeChecking { get; set; }

        /// <summary>
        /// Triggers when the TreeView node is clicked successfully.
        /// </summary>
        [Parameter]
        public EventCallback<NodeClickEventArgs> NodeClicked { get; set; }

        /// <summary>
        /// Triggers when the TreeView node collapses successfully.
        /// </summary>
        [Parameter]
        public EventCallback<NodeExpandEventArgs> NodeCollapsed { get; set; }

        /// <summary>
        /// Triggers before the TreeView node collapses.
        /// </summary>
        [Parameter]
        public EventCallback<NodeExpandEventArgs> NodeCollapsing { get; set; }

        /// <summary>
        /// Triggers when the TreeView node drag (move) starts.
        /// </summary>
        [Parameter]
        public EventCallback<DragAndDropEventArgs> OnNodeDragStart { get; set; }

        /// <summary>
        /// Triggers when the TreeView node dragging (move) stops.
        /// </summary>
        [Parameter]
        public EventCallback<DragAndDropEventArgs> OnNodeDragStop { get; set; }

        /// <summary>
        /// Triggers when the TreeView node drag (move) is stopped.
        /// </summary>
        [Parameter]
        public EventCallback<DragAndDropEventArgs> OnNodeDragged { get; set; }

        /// <summary>
        /// Triggers when the TreeView node is dropped on target element successfully.
        /// </summary>
        [Parameter]
        public EventCallback<DragAndDropEventArgs> NodeDropped { get; set; }

        /// <summary>
        /// Triggers when the TreeView node is renamed successfully.
        /// </summary>
        [Parameter]
        public EventCallback<NodeEditEventArgs> NodeEdited { get; set; }

        /// <summary>
        /// Triggers before the TreeView node is renamed.
        /// </summary>
        [Parameter]
        public EventCallback<NodeEditEventArgs> NodeEditing { get; set; }

        /// <summary>
        /// Triggers when the TreeView node expands successfully.
        /// </summary>
        [Parameter]
        public EventCallback<NodeExpandEventArgs> NodeExpanded { get; set; }

        /// <summary>
        /// Triggers before the TreeView node is to be expanded.
        /// </summary>
        [Parameter]
        public EventCallback<NodeExpandEventArgs> NodeExpanding { get; set; }

        /// <summary>
        /// Triggers when the TreeView node is selected/unselected successfully.
        /// </summary>
        [Parameter]
        public EventCallback<NodeSelectEventArgs> NodeSelected { get; set; }

        /// <summary>
        /// Triggers before the TreeView node is selected/unselected.
        /// </summary>
        [Parameter]
        public EventCallback<NodeSelectEventArgs> NodeSelecting { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.TreeviewEvents = this;
        }
    }
}
