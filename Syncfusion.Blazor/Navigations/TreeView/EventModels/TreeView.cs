using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies class for DataSourceChanged event arguments.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class DataSourceChangedEventArgs<T>
    {
        /// <summary>
        /// Return the updated TreeView data. The data source will be updated after performing some operation like
        /// drag and drop, node editing, adding and removing node. If you want to get updated data source after performing operation like
        /// selecting/unSelecting, checking/unChecking, expanding/collapsing the node, then you can use getTreeData method.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public List<T> Data { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for DataBound event arguments.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class DataBoundEventArgs<T>
    {
        /// <summary>
        /// Return the updated TreeView data. The data source will be updated after performing some operation like
        /// drag and drop, node editing, adding and removing node. If you want to get updated data source after performing operation like
        /// selecting/unSelecting, checking/unChecking, expanding/collapsing the node, then you can use getTreeData method.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public List<T> Data { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for DragAndDrop event arguments.
    /// </summary>
    public class DragAndDropEventArgs
    {
        /// <summary>
        /// If you want to cancel this event then, set cancel as true. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Return the cloned element.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM ClonedNode { get; set; }

        /// <summary>
        /// Return the currently dragged TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM DraggedNode { get; set; }

        /// <summary>
        /// Return the currently dragged node as array of JSON object from data source.
        /// </summary>
        public NodeData DraggedNodeData { get; set; }

        /// <summary>
        /// Return the dragged element's source parent.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM DraggedParentNode { get; set; }

        /// <summary>
        /// Returns the dragged/dropped element's target index position.
        /// </summary>
        public int? DropIndex { get; set; }

        /// <summary>
        /// Return the cloned element's drop status icon while dragging.
        /// </summary>
        public string DropIndicator { get; set; }

        /// <summary>
        /// Returns the dragged/dropped element's target level.
        /// </summary>
        public int? DropLevel { get; set; }

        /// <summary>
        /// Return the dragged element's destination parent.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM DropTarget { get; set; }

        /// <summary>
        /// Return the dropped TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM DroppedNode { get; set; }

        /// <summary>
        /// Return the dropped node as array of JSON object from data source.
        /// </summary>
        public NodeData DroppedNodeData { get; set; }

        /// <summary>
        /// Return the actual event.
        /// </summary>
        public object Event { get; set; }

        /// <summary>
        /// Return boolean value for preventing auto-expanding of parent node.
        /// </summary>
        public bool PreventTargetExpand { get; set; }

        /// <summary>
        /// Return the target element from which drag starts/end.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Target { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the Client X value of target element.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Return the Client Y value of target element.
        /// </summary>
        public double Top { get; set; }
    }

    /// <summary>
    /// Specifies class for node render event arguments.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class NodeRenderEventArgs<T>
    {
        /// <summary>
        /// Return the current rendering node.
        /// </summary>
        public ElementReference Node { get; set; }

        /// <summary>
        /// Return the current rendering node as JSON object.
        /// </summary>
        public T NodeData { get; set; }

        /// <summary>
        /// Return the current rendering node text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for Failure event arguments.
    /// </summary>
    public class FailureEventArgs : ActionEventArgs
    {
        /// <summary>
        /// Defines the error information.
        /// </summary>
        public Exception Error { get; set; }
    }

    /// <summary>
    /// Specifies class for NodeCheck event arguments.
    /// </summary>
    public class NodeCheckEventArgs
    {
        /// <summary>
        /// Return the name of action like check or un-check.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// If you want to cancel this event then, set cancel as true. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Return the currently checked node as JSON object from data source.
        /// </summary>
        public NodeData NodeData { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Return the currently checked TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Node { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for NodeClick event arguments.
    /// </summary>
    public class NodeClickEventArgs
    {
        /// <summary>
        /// Return the actual event.
        /// </summary>
        public ClickEventArgs Event { get; set; }

        /// <summary>
        /// Return the current clicked TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Node { get; set; }

        /// <summary>
        /// Return the current clicked TreeView node data.
        /// </summary>
        public NodeData NodeData { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the Client X value of target element.
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// Return the Client Y value of target element.
        /// </summary>
        public double Top { get; set; }
    }

    /// <summary>
    /// Specifies class that holds the node details.
    /// </summary>
    public class NodeData
    {
        /// <summary>
        /// Specifies the mapping field for expand state of the TreeView node.
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Specifies the mapping field for hasChildren to check whether a node has child nodes or not.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Specifies the ID field mapped in dataSource.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Specifies the field for checked state of the TreeView node.
        /// </summary>
        public string IsChecked { get; set; }

        /// <summary>
        /// Specifies the parent ID field mapped in dataSource.
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// Specifies the mapping field for selected state of the TreeView node.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Specifies the mapping field for text displayed as TreeView node's display text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for NodeEdit event arguments.
    /// </summary>
    public class NodeEditEventArgs
    {
        /// <summary>
        /// If you want to cancel this event then, set cancel as true. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets the inner HTML of TreeView node while editing.
        /// </summary>
        public string InnerHtml { get; set; }

        /// <summary>
        /// Return the current TreeView node new text.
        /// </summary>
        public string NewText { get; set; }

        /// <summary>
        /// Return the current TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Node { get; set; }

        /// <summary>
        /// Return the current node as JSON object from data source.
        /// </summary>
        public NodeData NodeData { get; set; }

        /// <summary>
        /// Return the current TreeView node old text.
        /// </summary>
        public string OldText { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for NodeExpand event arguments.
    /// </summary>
    public class NodeExpandEventArgs
    {
        /// <summary>
        /// If you want to cancel this event then, set cancel as true. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Event argument.
        /// </summary>
        public ClickEventArgs Event { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Return the expanded/collapsed TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Node { get; set; }

        /// <summary>
        /// Return the expanded/collapsed node as JSON object from data source.
        /// </summary>
        public NodeData NodeData { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for NodeKeyPress event arguments.
    /// </summary>
    public class NodeKeyPressEventArgs
    {
        /// <summary>
        /// If you want to cancel this event then, set cancel as true. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Return the actual event.
        /// </summary>
        public KeyboardEventArgs Event { get; set; }

        /// <summary>
        /// Return the current active TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Node { get; set; }

        /// <summary>
        /// Return the current active node as JSON object from data source.
        /// </summary>
        public NodeData NodeData { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the Key Action of Event.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Return the Key value of Event.
        /// </summary>
        public string Key { get; set; }
    }

    /// <summary>
    /// Specifies class for NodeSelect event arguments.
    /// </summary>
    public class NodeSelectEventArgs
    {
        /// <summary>
        /// Return the name of action like select or un-select.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// If you want to cancel this event then, set cancel as true. Otherwise, false.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Return the currently selected TreeView node.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Node { get; set; }

        /// <summary>
        /// Return the currently selected node as JSON object from data source.
        /// </summary>
        public NodeData NodeData { get; set; }

        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies class for denotes the event name.
    /// </summary>
    public class ActionEventArgs
    {
        /// <summary>
        /// Return the Event name.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// class for persistence values.
    /// </summary>
    internal class TreePersistenceValues
    {
        /// <summary>
        /// Gets or sets SelectedNodes of the TreeView component.
        /// </summary>
        public List<string> SelectedNodes { get; set; }

        /// <summary>
        /// Gets or sets CheckedNodes of the TreeViewcomponent.
        /// </summary>
        public Dictionary<string, object> CheckedNodes { get; set; }

        /// <summary>
        /// Gets or sets ExpandedNodes of the TreeViewcomponent.
        /// </summary>
        public List<string> ExpandedNodes { get; set; }
    }
}