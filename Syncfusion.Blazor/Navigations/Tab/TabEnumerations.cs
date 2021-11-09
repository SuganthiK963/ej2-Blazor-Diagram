using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Options to set the orientation of Tab header.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HeaderPosition
    {
        /// <summary>
        /// Places the Tab header on the top.
        /// </summary>
        [EnumMember(Value = "Top")]
        Top,

        /// <summary>
        /// Places the Tab header at the bottom.
        /// </summary>
        [EnumMember(Value = "Bottom")]
        Bottom,

        /// <summary>
        /// Places the Tab header on the left.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Places the Tab header on the right.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right
    }

    /// <summary>
    /// Specifies the options of Tab content display mode.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContentLoad
    {
        /// <summary>
        /// The content of the selected tab alone will be loaded and available in DOM and it will be replaced with the corresponding content if you select the tab dynamically.
        /// </summary>
        [EnumMember(Value = "Dynamic")]
        Dynamic,

        /// <summary>
        /// The content of all the tabs are rendered on the initial load and maintained in the DOM.
        /// </summary>
        [EnumMember(Value = "Init")]
        Init,

        /// <summary>
        /// The content of the selected tab alone is loaded initially. The content of the tabs which were loaded once will be maintained in the DOM.
        /// </summary>
        [EnumMember(Value = "Demand")]
        Demand
    }
}