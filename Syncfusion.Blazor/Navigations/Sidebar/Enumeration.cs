using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the Sidebar positions.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SidebarPosition
    {
        /// <summary>
        /// Specifies the position of the Left Sidebar corresponding to the main content.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Specifies the position of the Right Sidebar corresponding to the main content.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right
    }

    /// <summary>
    /// Specifies the Sidebar types.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SidebarType
    {
        /// <summary>
        /// The sidebar translates the x and y positions of the main content area based on the sidebar width.
        /// </summary>
        [EnumMember(Value = "Slide")]
        Slide,

        /// <summary>
        /// The sidebar floats over the main content area.
        /// </summary>
        [EnumMember(Value = "Over")]
        Over,

        /// <summary>
        /// The sidebar pushes the main content area to appear side-by-side, and shrinks the main content within the screen width.
        /// </summary>
        [EnumMember(Value = "Push")]
        Push,

        /// <summary>
        /// Sidebar with `Over` type in mobile resolution and `Push` type in other higher resolutions.
        /// </summary>
        [EnumMember(Value = "Auto")]
        Auto
    }
}