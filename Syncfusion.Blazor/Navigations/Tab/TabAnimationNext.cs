using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the animation to appear when moving to the next Tab content.
    /// </summary>
    public partial class TabAnimationNext : SfBaseComponent
    {
        private int duration;
        private string easing;
        private AnimationEffect effect;

        [CascadingParameter]
        internal TabAnimationSettings Parent { get; set; }

        [CascadingParameter]
        internal SfTab BaseParent { get; set; }

        /// <summary>
        /// Specifies the time duration to transform content.
        /// </summary>
        [Parameter]
        public int Duration { get; set; } = 600;

        /// <summary>
        /// Specifies the easing effect applied when transforming the content.
        /// </summary>
        [Parameter]
        public string Easing { get; set; } = "ease";

        /// <summary>
        /// Specifies the animation effect for displaying the next Tab content.
        /// Default animation is given as SlideRightIn for next tab animation. You can also disable the animation by setting the animation effect as none.
        /// </summary>
        [Parameter]
        public AnimationEffect Effect { get; set; } = AnimationEffect.SlideRightIn;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            duration = Duration;
            easing = Easing;
            effect = Effect;
            Parent.UpdateNextProperties(this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            duration = NotifyPropertyChanges(nameof(Duration), Duration, duration);
            easing = NotifyPropertyChanges(nameof(Easing), Easing, easing);
            effect = NotifyPropertyChanges(nameof(Effect), Effect, effect);
            if (PropertyChanges.Count > 0)
            {
                BaseParent.IsTabItemChanged = true;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }
    }
}