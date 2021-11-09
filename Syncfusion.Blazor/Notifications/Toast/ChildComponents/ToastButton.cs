using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Buttons;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Provides data to configure the Toast button properties.
    /// </summary>
    public partial class ToastButton : SfBaseComponent
    {
        [CascadingParameter]
        private ToastButtons Parent { get; set; }

        /// <summary>
        /// Specifies the click event binding of action buttons created within Toast.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        /// <summary>
        /// Defines the text `Content` of the Button element.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines the class/multiple classes separated by a space in the Button element.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the Button is `Disabled`.
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
        /// Specifies the type of the button.
        /// Possible values are Button, Submit, and Reset.
        /// </summary>
        [Parameter]
        public ButtonType Type { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
        }

        internal override void ComponentDispose()
        {
            Parent.ActionButtons.Remove(this);
            Parent = null;
        }
    }
}
