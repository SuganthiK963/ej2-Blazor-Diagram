using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// The class provides data for the animation to Dialog.
    /// </summary>
    public class DialogAnimationSettings : SfBaseComponent
    {
        private const string ANIMATION_SETTINGS = "animationSettings";

        [CascadingParameter]
        internal SfDialog Parent { get; set; }

        /// <summary>
        /// Specifies the delay in milliseconds to start the animation.
        /// </summary>
        [Parameter]
        public double Delay { get; set; }

        /// <summary>
        /// Specifies the duration in milliseconds that the animation takes to open or close the dialog.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 400;

        /// <summary>
        /// Specifies the animation name that should be applied on while opening and closing the dialog.
        /// If the user sets Fade animation, the dialog will open with the `FadeIn` effect and close with the `FadeOut` effect.
        /// The following are the list of animation effects available to configure to the dialog:
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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DialogEffect Effect { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties(ANIMATION_SETTINGS, this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}