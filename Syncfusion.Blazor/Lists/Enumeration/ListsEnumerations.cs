using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// An enum type that denotes the animation effects of the ListView. Available options are as follows None, SlideLeft, SlideDown, Zoom, Fade.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ListViewEffect
    {
        /// <summary>
        /// Navigation of the nested list item occurs with out any animation effect.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Navigation of the nested list item occurs with a slide left animation effect.
        /// </summary>
        [EnumMember(Value = "SlideLeft")]
        SlideLeft,

        /// <summary>
        /// Navigation of the nested list item occurs with a slide down animation effect.
        /// </summary>
        [EnumMember(Value = "SlideDown")]
        SlideDown,

        /// <summary>
        /// Navigation of the nested list item occurs with a zooming animation effect.
        /// </summary>
        [EnumMember(Value = "Zoom")]
        Zoom,

        /// <summary>
        /// Navigation of the nested list item occurs with a fading animation effect.
        /// </summary>
        [EnumMember(Value = "Fade")]
        Fade
    }

     /// <summary>
    /// An enum type that denotes the position of checkbox of the ListView. Available options are as follows Left and Right.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CheckBoxPosition
    {
        /// <summary>
        /// Positions the checkbox to the left of the text content.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Positions the checkbox to the right of the text content.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right
    }

    /// <summary>
    /// An enum type that denotes the sort order of the ListView. Available options are as follows None, Ascending, Descending.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortOrder
    {
        /// <summary>
        /// The list view items will be displayed without performing any sorting.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Sorts the list view items will be displayed in the ascending order.
        /// </summary>
        [EnumMember(Value = "Ascending")]
        Ascending,

        /// <summary>
        /// Sorts the list view items will be displayed in the descending order.
        /// </summary>
        [EnumMember(Value = "Descending")]
        Descending
    }
}