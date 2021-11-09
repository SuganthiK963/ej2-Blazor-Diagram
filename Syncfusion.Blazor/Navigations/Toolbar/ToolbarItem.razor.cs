using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// An item object that is used to configure Toolbar commands.
    /// </summary>
    public partial class ToolbarItem : SfBaseComponent
    {
        private const string ALIGN = "Align";
        private const string CSSCLASS = "CssClass";
        private const string HTMLATTRIBUTES = "HtmlAttributes";
        private const string OVERFLOW = "Overflow";
        private const string PREFIXICON = "PrefixIcon";
        private const string SHOWALWAYSINPOPUP = "ShowAlwaysInPopup";
        private const string SHOWTEXTON = "ShowTextOn";
        private const string SUFFIXICON = "SuffixIcon";
        private const string TEXT = "Text";
        private const string TOOLTIPTEXT = "TooltipText";
        private const string TYPE = "Type";
        private const string VISIBLE = "Visible";
        private const string WIDTH = "Width";
        private const string TOOLBARITEM = "e-toolbar-item";
        private ItemAlign align;
        private string cssClass;
        private Dictionary<string, object> htmlAttributes;
        private OverflowOption overflow;
        private string prefixIcon;
        private bool showAlwaysInPopup;
        private DisplayMode showTextOn;
        private string suffixIcon;
        private string text;
        private string tooltipText;
        private ItemType type;
        private bool visible;
        private string width;

        [CascadingParameter]
        [JsonIgnore]
        internal ToolbarItems Parent { get; set; }

        [CascadingParameter]
        [JsonIgnore]
        internal SfToolbar BaseParent { get; set; }

        internal int Index { get; set; } = -1;

        internal bool ItemFromTag { get; set; }

        private ItemModel Item { get; set; }

        /// <summary>
        /// Child Content for Toolbar item.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Event triggers when click the toolbar item.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<ClickEventArgs> OnClick { get; set; }

        /// <summary>
        /// Specifies the location for aligning Toolbar items on the Toolbar. Each command will be aligned according to the `Align` property.
        /// Possible values are:
        /// - Left: To align commands to the left side of the Toolbar.
        /// - Center: To align commands at the center of the Toolbar.
        /// - Right: To align commands to the right side of the Toolbar.
        ///
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemAlign Align { get; set; }

        /// <summary>
        /// Defines single/multiple classes (separated by space) to be used for customization of commands.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies whether an item should be disabled or not.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Defines htmlAttributes used to add custom attributes to Toolbar command.
        /// Supports HTML attributes such as style, class, etc.
        /// </summary>
        [Parameter]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// Specifies the unique ID to be used with button or input element of Toolbar items.
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the Toolbar command display area when an element's content is too large to fit available space.
        /// This is applicable only to `Popup` mode. Possible values are:
        /// - Show:  Always shows the item as the primary priority on the Toolbar.
        /// - Hide: Always shows the item as the secondary priority on the popup.
        /// - None: No priority for display, and as per normal order moves to popup when content exceeds.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OverflowOption Overflow { get; set; }

        /// <summary>
        /// Defines single/multiple classes separated by space used to specify an icon for the button.
        /// The icon will be positioned before the text content if text is available, otherwise the icon alone will be rendered.
        /// </summary>
        [Parameter]
        public string PrefixIcon { get; set; } = string.Empty;

        /// <summary>
        /// Defines the priority of items to display it in popup always.
        /// It allows to maintain toolbar item on popup always but it does not work for toolbar priority items.
        /// </summary>
        [Parameter]
        public bool ShowAlwaysInPopup { get; set; }

        /// <summary>
        /// Specifies where the button text will be displayed on popup mode of the Toolbar.
        /// Possible values are:
        /// - Toolbar:  Text will be displayed on Toolbar only.
        /// - Overflow: Text will be displayed only when content overflows to popup.
        /// - Both: Text will be displayed on popup and Toolbar.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DisplayMode ShowTextOn { get; set; }

        /// <summary>
        /// Defines single/multiple classes separated by space used to specify an icon for the button.
        /// The icon will be positioned after the text content if text is available.
        /// </summary>
        [Parameter]
        public string SuffixIcon { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the HTML element/element ID as a string that can be added as a Toolbar command.
        ///
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Specifies the text to be displayed on the Toolbar button.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the text to be displayed on hovering the Toolbar button.
        /// </summary>
        [Parameter]
        public string TooltipText { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the types of command to be rendered in the Toolbar.
        /// Supported types are:
        /// - Button: Creates the Button control with its given properties like text, prefixIcon, etc.
        /// - Separator: Adds a horizontal line that separates the Toolbar commands.
        /// - Input: Creates an input element that is applicable to template rendering with Syncfusion controls like DropDownList,
        /// AutoComplete, etc.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ItemType Type { get; set; }

        /// <summary>
        /// Specifies whether an item should be hidden or not.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Specifies the width of the Toolbar button commands.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "auto";

        internal static ToolbarItem SetId(ToolbarItem item)
        {
            item.Id = SfBaseUtils.GenerateID(TOOLBARITEM);
            return item;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Index = Parent.UpdateChildProperty(this);
            UpdateIdAttribute();
            align = Align;
            cssClass = CssClass;
            htmlAttributes = HtmlAttributes;
            overflow = Overflow;
            prefixIcon = PrefixIcon;
            showAlwaysInPopup = ShowAlwaysInPopup;
            showTextOn = ShowTextOn;
            suffixIcon = SuffixIcon;
            text = Text;
            tooltipText = TooltipText;
            type = Type;
            visible = Visible;
            width = Width;
            BaseParent.IsItemChanged = true;
            ItemFromTag = true;
            if (string.IsNullOrEmpty(Id))
            {
                Id = SfBaseUtils.GenerateID(TOOLBARITEM);
            }
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            UpdateIdAttribute();
            align = NotifyPropertyChanges(ALIGN, Align, align);
            cssClass = NotifyPropertyChanges(CSSCLASS, CssClass, cssClass);
            htmlAttributes = NotifyPropertyChanges(HTMLATTRIBUTES, HtmlAttributes, htmlAttributes);
            overflow = NotifyPropertyChanges(OVERFLOW, Overflow, overflow);
            prefixIcon = NotifyPropertyChanges(PREFIXICON, PrefixIcon, prefixIcon);
            showAlwaysInPopup = NotifyPropertyChanges(SHOWALWAYSINPOPUP, ShowAlwaysInPopup, showAlwaysInPopup);
            showTextOn = NotifyPropertyChanges(SHOWTEXTON, ShowTextOn, showTextOn);
            suffixIcon = NotifyPropertyChanges(SUFFIXICON, SuffixIcon, suffixIcon);
            text = NotifyPropertyChanges(TEXT, Text, text);
            tooltipText = NotifyPropertyChanges(TOOLTIPTEXT, TooltipText, tooltipText);
            type = NotifyPropertyChanges(TYPE, Type, type);
            visible = NotifyPropertyChanges(VISIBLE, Visible, visible);
            width = NotifyPropertyChanges(WIDTH, Width, width);
            if (PropertyChanges.Count > 0)
            {
                BaseParent.IsItemChanged = true;
            }

            if (string.IsNullOrEmpty(Id))
            {
                Id = SfBaseUtils.GenerateID(TOOLBARITEM);
            }

            Item = SfToolbar.GetItem(this);
        }

        private void UpdateIdAttribute()
        {
            if (HtmlAttributes != null)
            {
                foreach (var item in HtmlAttributes)
                {
                    if (item.Key == "id")
                    {
                        Id = item.Value.ToString();
                    }
                }
            }
        }

        internal void EnableItem(bool isDisabled)
        {
            Disabled = isDisabled;
        }

        internal void VisibleItem(bool isVisible)
        {
            Visible = isVisible;
        }

        internal override void ComponentDispose()
        {
            if (Parent.Items != null && Parent.Items.Contains(this))
            {
                Parent.Items.Remove(this);
                BaseParent.IsItemChanged = true;
            }

            Parent = null;
            BaseParent = null;
            ChildContent = null;
        }
    }
}