using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class that holds options to control the toolbar clicked action.
    /// </summary>
    public class ClickEventArgs
    {
        /// <summary>
        /// Defines the prevent action.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the current Toolbar Item Object.
        /// </summary>
        public ItemModel Item { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines the current Event arguments.
        /// </summary>
        public MouseEventArgs OriginalEvent { get; set; }
    }

    /// <summary>
    /// A class for a toolbar Item.
    /// </summary>
    public class ItemModel
    {
        /// <summary>
        /// Event triggers when `click` the toolbar item.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("click")]
        public EventCallback<ClickEventArgs> Click { get; set; }

        /// <summary>
        /// Specifies the location for aligning Toolbar items on the Toolbar. Each command will be aligned according to the `Align` property.
        /// Possible values are:
        /// - Left: To align commands to the left side of the Toolbar.
        /// - Center: To align commands at the center of the Toolbar.
        /// - Right: To align commands to the right side of the Toolbar.
        /// </summary>
        [JsonPropertyName("align")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemAlign Align { get; set; }

        /// <summary>
        /// Defines single/multiple classes (separated by space) to be used for customization of commands.
        /// </summary>
        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies whether an item should be disabled or not.
        /// </summary>
        [JsonPropertyName("disabled")]
        public bool Disabled { get; set; }

        /// <summary>
        /// Defines htmlAttributes used to add custom attributes to Toolbar command.
        /// Supports HTML attributes such as style, class, etc.
        /// </summary>
        [JsonPropertyName("htmlAttributes")]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// Specifies the unique ID to be used with button or input element of Toolbar items.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the Toolbar command display area when an element's content is too large to fit available space.
        /// This is applicable only to `Popup` mode. Possible values are:
        /// - Show:  Always shows the item as the primary priority on the Toolbar.
        /// - Hide: Always shows the item as the secondary priority on the popup.
        /// - None: No priority for display, and as per normal order moves to popup when content exceeds.
        /// </summary>
        [JsonPropertyName("overflow")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OverflowOption Overflow { get; set; }

        /// <summary>
        /// Defines single/multiple classes separated by space used to specify an icon for the button.
        /// The icon will be positioned before the text content if text is available, otherwise the icon alone will be rendered.
        /// </summary>
        [JsonPropertyName("prefixIcon")]
        public string PrefixIcon { get; set; } = string.Empty;

        /// <summary>
        /// Defines the priority of items to display it in popup always.
        /// It allows to maintain toolbar item on popup always but it does not work for toolbar priority items.
        /// </summary>
        [JsonPropertyName("showAlwaysInPopup")]
        public bool ShowAlwaysInPopup { get; set; }

        /// <summary>
        /// Specifies where the button text will be displayed on popup mode of the Toolbar.
        /// Possible values are:
        /// - Toolbar:  Text will be displayed on Toolbar only.
        /// - Overflow: Text will be displayed only when content overflows to popup.
        /// - Both: Text will be displayed on popup and Toolbar.
        /// </summary>
        [JsonPropertyName("showTextOn")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DisplayMode ShowTextOn { get; set; }

        /// <summary>
        /// Defines single/multiple classes separated by space used to specify an icon for the button.
        /// The icon will be positioned after the text content if text is available.
        /// </summary>
        [JsonPropertyName("suffixIcon")]
        public string SuffixIcon { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the template that can be added as a Toolbar command.
        /// </summary>
        [JsonIgnore]
        [JsonPropertyName("template")]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Specifies the text to be displayed on the Toolbar button.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the text to be displayed on hovering the Toolbar button.
        /// </summary>
        [JsonPropertyName("tooltipText")]
        public string TooltipText { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the types of command to be rendered in the Toolbar.
        /// Supported types are:
        /// - Button: Creates the Button control with its given properties like text, prefixIcon, etc.
        /// - Separator: Adds a horizontal line that separates the Toolbar commands.
        /// - Input: Creates an input element that is applicable to template rendering with Syncfusion controls like DropDownList,
        /// AutoComplete, etc.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemType Type { get; set; }

        /// <summary>
        /// Specifies whether an item should be hidden or not.
        /// </summary>
        [JsonPropertyName("visible")]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Specifies the width of the Toolbar button commands.
        /// </summary>
        [JsonPropertyName("width")]
        public string Width { get; set; } = "auto";

        internal int Index { get; set; } = -1;
    }
}