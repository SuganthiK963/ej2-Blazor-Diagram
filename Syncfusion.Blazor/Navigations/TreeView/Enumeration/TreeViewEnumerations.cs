using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Defines the expand type of the TreeView node.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExpandAction
    {
        /// <summary>
        /// Specifies the action on 'DoubleClick' the node expands or collapses. The expand/collapse operation happens when you double-click the node.
        /// </summary>
        [EnumMember(Value = "DoubleClick")]
        DoubleClick,

        /// <summary>
        /// Specifies the action on 'Click' the node expands or collapses. The expand/collapse operation happens when you single-click the node.
        /// </summary>
        [EnumMember(Value = "Click")]
        Click,

        /// <summary>
        /// The expand/collapse operation will not happen when you single-click or double-click the node.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }

    /// <summary>
    /// Defines the sorting order type for TreeView.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Specifies the nodes are not sorted.
        /// </summary>
        None,

        /// <summary>
        /// Specifies the nodes are sorted in the 'Ascending' order.
        /// </summary>
        Ascending,

        /// <summary>
        /// Specifies the nodes are sorted in the 'Descending' order.
        /// </summary>
        Descending
    }

    /// <summary>
    /// TreeView animation effects.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TreeEffect
    {
        /// <summary>
        /// Specifies 'SlideDown' type of animation.
        /// </summary>
        [EnumMember(Value = "SlideDown")]
        SlideDown,

        /// <summary>
        /// Specifies animation type 'None'.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Specifies 'ZoomIn' type of animation.
        /// </summary>
        [EnumMember(Value = "ZoomIn")]
        ZoomIn,

        /// <summary>
        /// Specifies 'FadeIn' type of animation.
        /// </summary>
        [EnumMember(Value = "FadeIn")]
        FadeIn
    }
}