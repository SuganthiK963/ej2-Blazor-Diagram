using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Specifies the animation to appear while showing the Toast.
    /// </summary>
    public partial class ToastShowAnimationSettings : SfBaseComponent
    {
        [CascadingParameter]
        private ToastAnimationSettings Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the Spinner element.
        /// </summary>
        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the duration to animate.
        /// </summary>
        [Parameter]
        public int Duration { get; set; }

        /// <summary>
        /// Specifies the animation timing function.
        /// </summary>
        [Parameter]
        public ToastEasing Easing { get; set; }

        /// <summary>
        /// Specifies the animation name that should be applied on while opening and closing the toast.
        /// If the user sets Fade animation, the toast will open with the `FadeIn` effect and close with the `FadeOut` effect.
        /// The following are the list of animation effects available to configure to the toast:
        /// 1. Fade
        /// 2. FadeZoom
        /// 3. FlipLeftDown
        /// 4. FlipLeftUp
        /// 5. FlipRightDown
        /// 6. FlipRightUp
        /// 7. FlipXDown
        /// 8. FlipXUp
        /// 9. FlipYLeft
        /// 10. FlipYRight
        /// 11. SlideBottom
        /// 12. SlideLeft
        /// 13. SlideRight
        /// 14. SlideTop
        /// 15. Zoom
        /// 16. None.
        /// </summary>
        [Parameter]
        public ToastEffect Effect { get; set; }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Parent.UpdateAnimationSettings(false, this, null);
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
            Parent = null;
        }
    }
}
