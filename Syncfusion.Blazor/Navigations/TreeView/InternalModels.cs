using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Navigations.Internal
{
    ///<exclude/>
    public class SelectionEventArgs
    {
        /// <summary>
        /// Specifies the MultiSelect argument.
        /// </summary>
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Specifies the control key is pressed or not.
        /// </summary>
        public bool IsCtrKey { get; set; }

        /// <summary>
        /// Specifies the shift key is pressed or not.
        /// </summary>
        public bool IsShiftKey { get; set; }

        /// <summary>
        /// Specifies the Node data.
        /// </summary>
        public string[] Nodes { get; set; }

        /// <summary>
        /// Specifies the nodes are selected in manual interaction or dynamic interaction.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies the action to be performed.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Specifies the Node Data.
        /// </summary>
        public NodeData NodeData { get; set; }
    }

    /// <summary>
    /// Expand event args.
    /// </summary>
    /// <exclude/>
    public class ExpandEventArgs : NodeExpandEventArgs
    {
        /// <summary>
        /// Specifies the loaded argument.
        /// </summary>
        public bool IsLoaded { get; set; }

        /// <summary>
        /// Specifies the Node level.
        /// </summary>
        public int NodeLevel { get; set; }
    }

    ///<exclude/>
    public class DropTreeArgs
    {
        /// <summary>
        /// Specifies the Drageed List item.
        /// </summary>
        public string DragLi { get; set; }

        /// <summary>
        /// Specifies the Dropped List item.
        /// </summary>
        public string DropLi { get; set; }

        /// <summary>
        /// Specifies the Dropped parent List item.
        /// </summary>
        public string DropParentLi { get; set; }

        /// <summary>
        /// Specifies the Dragged parent List item.
        /// </summary>
        public string DragParentLi { get; set; }

        /// <summary>
        /// Specifies the value.
        /// </summary>
        public bool Pre { get; set; }

        /// <summary>
        /// Specifies the Source Tree.
        /// </summary>
        public DotNetObjectReference<object> SrcTree { get; set; }

        /// <summary>
        /// Specifies the external drag and drop is true or not.
        /// </summary>
        public bool IsExternalDrag { get; set; }

    }
}
