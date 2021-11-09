using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Buttons;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Provides data to configure the Dialog button properties.
    /// </summary>
    public partial class DialogButton : SfBaseComponent
    {
        private const string TYPE = "Type";
        private const string CONTENT = "Content";
        private const string ICON_CSS = "IconCss";
        private const string DISABLED = "Disabled";
        private const string IS_TOGGLE = "IsToggle";
        private const string CSS_CLASS = "CssClass";
        private const string IS_PRIMARY = "IsPrimary";
        private const string ENABLE_RTL = "EnableRtl";
        private const string ICON_POSITION = "IconPosition";

        private string content;
        private string cssClass;
        private int tagIndex = -1;
        private bool disabled;
        private ButtonType type;
        private bool isToggle;
        private bool isPrimary;
        private IconPosition iconPosition;
        private string iconCss;
        private bool enableRtl;

        [CascadingParameter]
        internal DialogButtons Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the Spinner element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the text `content` of the Button element.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines the class/multiple classes separated by a space in the Button element.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the Button is `disabled`.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Enable or disable rendering component in the right to left (RTL) direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Defines the class/multiple classes separated by a space for the Button that is used to include an icon.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Positions the icon before or after the text content in the Button.
        /// The possible values are: Left: The icon will be positioned to the left of the text content.
        /// Right: The icon will be positioned to the right of the text content.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public IconPosition IconPosition { get; set; }

        /// <summary>
        /// Allows the appearance of the Button to be enhanced and visually appealing when set to `true`.
        /// </summary>
        [Parameter]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Makes the Button toggle, when set to `true`.
        /// When you click it, the state changes from normal to active.
        /// </summary>
        [Parameter]
        public bool IsToggle { get; set; }

        /// <summary>
        /// Event triggers when `click` the dialog button.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Specifies the type of the button.
        /// Possible values are Button, Submit, and Reset.
        /// </summary>
        [Parameter]
        public ButtonType Type { get; set; }

        /// <summary>
        /// Specifies the Flat appearance of the dialog buttons.
        /// </summary>
        [Parameter]
        public bool IsFlat { get; set; } = true;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            tagIndex = Parent.UpdateChildProperty(this);
            content = Content;
            cssClass = CssClass;
            disabled = Disabled;
            iconCss = IconCss;
            isPrimary = IsPrimary;
            enableRtl = EnableRtl;
            iconPosition = IconPosition;
            isToggle = IsToggle;
            type = Type;
            Parent.Buttons[tagIndex] = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            content = NotifyPropertyChanges(CONTENT, Content, content);
            cssClass = NotifyPropertyChanges(CSS_CLASS, CssClass, cssClass);
            disabled = NotifyPropertyChanges(DISABLED, Disabled, disabled);
            iconCss = NotifyPropertyChanges(ICON_CSS, IconCss, iconCss);
            isPrimary = NotifyPropertyChanges(IS_PRIMARY, IsPrimary, isPrimary);
            enableRtl = NotifyPropertyChanges(ENABLE_RTL, EnableRtl, enableRtl);
            iconPosition = NotifyPropertyChanges(ICON_POSITION, IconPosition, iconPosition);
            isToggle = NotifyPropertyChanges(IS_TOGGLE, IsToggle, isToggle);
            type = NotifyPropertyChanges(TYPE, Type, type);
        }

        internal override void ComponentDispose()
        {
            Parent.Buttons.Remove(this);
            List<DialogButton> btns = Parent.Buttons;
            if (btns != null)
            {
                for (int i = 0; i < btns.Count; i++)
                {
                    Parent.Buttons[i].tagIndex = i;
                }
            }

            ChildContent = null;
            Parent = null;
        }
    }
}