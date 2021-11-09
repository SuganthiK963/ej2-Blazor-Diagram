using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Syncfusion.Blazor.Buttons;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// ButtonGroupButton  creates a button element that triggers an event on its click action.
    /// It can contain a text, an image, or both.
    /// </summary>
    public partial class ButtonGroupButton : SfBaseComponent
    {
        private const string SPACE = " ";
        private const string ROOT = "e-control e-btn e-lib";
        private const string ICON_CLASS = "e-btn-icon";
        private const string ICON_BUTTON = "e-icon-btn";
        private string btnClass;
        private string iconCss;

        [CascadingParameter]
        internal SfButtonGroup ButtonGroup { get; set; }

        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the button element. The button types and
        /// styles can be defined by using this property.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space for the button that is used to include an icon.
        /// Button can also include font icon and sprite image.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the button is enabled or disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the button is selected or not.
        /// </summary>
        [Parameter]
        public bool Selected { get; set; }
        private bool selected { get; set; }

        /// <summary>
        /// Defines name attribute for the input element.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Defines value attribute for the input element.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        /// <summary>
        /// Defines the text content of the button element and it will support only string type.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Makes the button as toggleable, when set to true. When you click it, the state changes from normal to active and viceversa.
        /// </summary>
        [Parameter]
        public bool IsToggle { get; set; }

        /// <summary>
        /// Allows the appearance of the button to be enhanced and visually appealing when set to true.
        /// </summary>
        [Parameter]
        public bool IsPrimary { get; set; }

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

        /// <exclude/>
        /// <summary>
        /// You can add the additional html attributes such as id, native events etc., to the ButtonGroup element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<bool> SelectedChanged { get; set; }

        private bool buttonSelected;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            selected = Selected;
            buttonSelected = Selected;
            ButtonGroup.UpdateChildProperty(this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (ButtonGroup.Mode == SelectionMode.Single && IsRendered)
            {
                await UpdateButtonState(buttonSelected);
            }
            InitRender();
        }

        private void InitRender()
        {
            btnClass = ROOT;
            if (!string.IsNullOrEmpty(CssClass))
            {
                btnClass += SPACE + CssClass;
            }

            if (!string.IsNullOrEmpty(IconCss))
            {
                iconCss = ICON_CLASS;
                if (ChildContent == null && string.IsNullOrEmpty(Content))
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
        }

        private async Task ClickHandler()
        {
            if (ButtonGroup.Mode == SelectionMode.Multiple)
            {
                await Task.Yield();
                await UpdateButtonState(!Selected);
            }
            else
            {
                buttonSelected = !Selected;
                for (var i = 0; i < ButtonGroup.btnItems.Count; i++)
                {
                    if (!SfBaseUtils.Equals(this, ButtonGroup.btnItems[i]))
                    {
                        ButtonGroup.btnItems[i].buttonSelected = false;
                    }
                }
                await UpdateButtonState(buttonSelected);
            }
        }

        protected async Task UpdateButtonState(bool state)
        {
            Selected = selected = await SfBaseUtils.UpdateProperty(state, selected, SelectedChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
