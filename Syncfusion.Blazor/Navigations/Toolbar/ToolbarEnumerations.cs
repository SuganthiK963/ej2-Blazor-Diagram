using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the options of Toolbar display mode. Display option is considered when Toolbar content exceeds the available space.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OverflowMode
    {
        /// <summary>
        ///  Scrollable - All the elements are displayed in a single line with horizontal scrolling enabled.
        /// </summary>
        [EnumMember(Value = "Scrollable")]
        Scrollable,

        /// <summary>
        ///  Popup - Prioritized elements are displayed on the Toolbar and the rest of elements are moved to the popup.
        /// </summary>
        [EnumMember(Value = "Popup")]
        Popup,

        /// <summary>
        ///  MultiRow - Displays the overflow toolbar items as an in-line of a toolbar.
        /// </summary>
        [EnumMember(Value = "MultiRow")]
        MultiRow,

        /// <summary>
        ///  Extended - Hide the overflowing toolbar items in the next row. Show the overflowing toolbar items when you click the expand icons. If the popup content overflows the height of the page, the rest of the elements will be hidden.
        /// </summary>
        [EnumMember(Value = "Extended")]
        Extended
    }

    /// <summary>
    /// Specifies the options of where the text will be displayed in popup mode of the Toolbar.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DisplayMode
    {
        /// <summary>
        ///  Both - Text will be displayed on popup and Toolbar.
        /// </summary>
        [EnumMember(Value = "Both")]
        Both,

        /// <summary>
        ///  Overflow - Text will be displayed only when content overflows to popup.
        /// </summary>
        [EnumMember(Value = "Overflow")]
        Overflow,

        /// <summary>
        ///  Toolbar - Text will be displayed on Toolbar only.
        /// </summary>
        [EnumMember(Value = "Toolbar")]
        Toolbar
    }

    /// <summary>
    /// Specifies the options for aligning the Toolbar items.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemAlign
    {
        /// <summary>
        ///  Left - To align commands to the left side of the Toolbar.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        ///  Center - To align commands at the center of the Toolbar.
        /// </summary>
        [EnumMember(Value = "Center")]
        Center,

        /// <summary>
        ///  Right - To align commands to the right side of the Toolbar.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right
    }

    /// <summary>
    /// Specifies the options for supporting element types of Toolbar command.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemType
    {
        /// <summary>
        ///  Button - Creates the Button control with its given properties like text, prefixIcon, etc.
        /// </summary>
        [EnumMember(Value = "Button")]
        Button,

        /// <summary>
        ///  Separator - Adds a horizontal line that separates the Toolbar commands.
        /// </summary>
        [EnumMember(Value = "Separator")]
        Separator,

        /// <summary>
        ///  Input - Creates an input element that is applicable to template rendering with Syncfusion controls like DropDownList, AutoComplete, etc.
        /// </summary>
        [EnumMember(Value = "Input")]
        Input
    }

    /// <summary>
    /// Specifies the options of the Toolbar item display area when the Toolbar content overflows to available space.Applicable to `Popup` mode.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OverflowOption
    {
        /// <summary>
        ///  None - No priority for display, and as per normal order moves to popup when content exceeds.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        ///  Show - Always shows the item as the primary priority on the Toolbar.
        /// </summary>
        [EnumMember(Value = "Show")]
        Show,

        /// <summary>
        ///  Hide - Always shows the item as the secondary priority on the popup.
        /// </summary>
        [EnumMember(Value = "Hide")]
        Hide
    }
}