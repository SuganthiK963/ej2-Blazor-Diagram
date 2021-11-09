using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// Specifies the animation settings for the progress button.
    /// </summary>
    public partial class ProgressButtonAnimationSettings : SfBaseComponent
    {
        [CascadingParameter]
        private SfProgressButton progressButton { get; set; }

        /// <summary>
        /// Specifies the duration taken to animate.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 400;

        /// <summary>
        /// Specifies the animation timing function.
        /// </summary>
        [Parameter]
        public string Easing { get; set; } = "ease";

        /// <summary>
        /// Specifies the effect of animation.
        /// </summary>
        [Parameter]
        public AnimationEffect Effect { get; set; } = AnimationEffect.None;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            progressButton.UpdateChildProperties("animationSettings", this);
        }

        internal override void ComponentDispose()
        {
            progressButton = null;
        }
    }
}