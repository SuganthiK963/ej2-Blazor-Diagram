using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Specifies the show and hide animation settings of Toast.
    /// </summary>
    public partial class ToastAnimationSettings : SfBaseComponent
    {
        [CascadingParameter]
        private SfToast Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the Spinner element.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the animation to appear when hiding the Toast.
        /// </summary>
        [Parameter]
        public ToastHideAnimationSettings HideSettings { get; set; }

        /// <summary>
        /// Specifies the animation to appear when showing the Toast.
        /// </summary>
        [Parameter]
        public ToastShowAnimationSettings ShowSettings { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            UpdateAnimationSettings(true);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Parent.UpdateAnimation(this);
        }

        internal void UpdateShowAnimationSettings(ToastShowAnimationSettings animation)
        {
            ShowSettings = animation == null ? new ToastShowAnimationSettings() : animation;
            Parent.UpdateAnimation(this);
        }

        internal void UpdateHideAnimationSettings(ToastHideAnimationSettings animation)
        {
            HideSettings = animation == null ? new ToastHideAnimationSettings() : animation;
            Parent.UpdateAnimation(this);
        }

        internal void UpdateAnimationSettings(bool isInitial, ToastShowAnimationSettings show = null, ToastHideAnimationSettings hide = null)
        {
            if (isInitial)
            {
                ShowSettings = new ToastShowAnimationSettings();
                HideSettings = new ToastHideAnimationSettings();
            }
            else if (show != null)
            {
                ShowSettings = show;
            }
            else if (hide != null)
            {
                HideSettings = hide;
            }
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
            Parent = null;
        }
    }
}