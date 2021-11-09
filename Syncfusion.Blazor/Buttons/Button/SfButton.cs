using System;
using System.Globalization;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// The Button is a graphical user interface element that triggers an event on its click action.
    /// It can contain a text, an image, or both.
    /// </summary>
    public partial class SfButton : SfBaseComponent
    {
        internal ElementReference btn;
        private const string SPACE = " ";
        private const string RTL = "e-rtl";
        private const string ACTIVE = "e-active";
        private const string PRIMARY = "e-primary";
        private const string ICON_CLASS = "e-btn-icon";
        private const string ICON_BUTTON = "e-icon-btn";
        private const string ROOT = "e-control e-btn e-lib";
        private Dictionary<string, object> htmlAttributes;
        private string iconCss;
        private string btnClass;

        /// <summary>
        /// Sets content for button element including HTML and its customizations.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the text content of the button element and it will support only string type.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the button element. The Button types, styles, and
        /// size can be defined by using this property.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the Button is enabled or disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Enable or disable rendering Button component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space for the Button that is used to include an icon.
        /// Buttons can also include font icon and sprite image.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Positions the icon based on the text content in the button and its default value is Left.
        /// The possible values are:
        /// - Left: The icon will be positioned to the left of the text content.
        /// - Right: The icon will be positioned to the right of the text content.
        /// - Top: The icon will be positioned to the top of the text content.
        /// - Bottom: The icon will be positioned to the bottom of the text content.
        /// </summary>
        [Parameter]
        public IconPosition IconPosition { get; set; }

        /// <summary>
        /// Allows the appearance of the button to be enhanced and visually appealing when set to true.
        /// </summary>
        [Parameter]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Makes the Button toggle, when set to true. When you click it, the state changes from normal to active or viceversa.
        /// </summary>
        [Parameter]
        public bool IsToggle { get; set; }

        /// <exclude/>
        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the button element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes; }
            set { htmlAttributes = value; }
        }

        /// <summary>
        /// Triggers when button element is clicked.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            InitRender();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Created, null);
            }
        }

        private void InitRender()
        {
            btnClass = ROOT;
            if (!string.IsNullOrEmpty(CssClass))
            {
                btnClass += SPACE + CssClass;
            }

            if (IsPrimary)
            {
                btnClass += SPACE + PRIMARY;
            }

            if (EnableRtl || SyncfusionService.options.EnableRtl)
            {
                btnClass += SPACE + RTL;
            }

            if (!string.IsNullOrEmpty(IconCss))
            {
                iconCss = ICON_CLASS;
                if (string.IsNullOrEmpty(Content) && ChildContent == null)
                {
                    btnClass += SPACE + ICON_BUTTON;
                }
                else
                {
                    iconCss += " e-icon-" + IconPosition.ToString().ToLower(CultureInfo.CurrentCulture);
                    if (IconPosition == IconPosition.Top || IconPosition == IconPosition.Bottom)
                    {
                        btnClass += SPACE + "e-" + IconPosition.ToString().ToLower(CultureInfo.CurrentCulture) + "-icon-btn";
                    }
                }
            }

            if (htmlAttributes != null && htmlAttributes.ContainsKey("class"))
            {
                btnClass += SPACE + (htmlAttributes["class"] as string);
            }
        }

        private async Task OnClickHandler(MouseEventArgs args = null)
        {
            if (IsToggle)
            {
                if (btnClass.Contains(ACTIVE, StringComparison.Ordinal))
                {
                    btnClass = btnClass.Replace(SPACE + ACTIVE, string.Empty, StringComparison.Ordinal);
                }
                else
                {
                    btnClass += SPACE + ACTIVE;
                }
            }

            if (args != null)
            {
                await SfBaseUtils.InvokeEvent(OnClick, args);
            }
        }
        
        /// <summary>
        /// Sets the focus to button element.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.focusButton", btn);
        }

        /// <summary>
        /// Sets the focus to button element.
        /// </summary>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
    }

    /// <summary>
    /// Defines the icon position of Button.
    /// </summary>
    public enum IconPosition
    {
        /// <summary>
        /// To position icon left to content.
        /// </summary>
        Left,

        /// <summary>
        /// To position icon right to content.
        /// </summary>
        Right,

        /// <summary>
        /// To position icon above the content.
        /// </summary>
        Top,

        /// <summary>
        /// To position icon below the content.
        /// </summary>
        Bottom,
    }
}
