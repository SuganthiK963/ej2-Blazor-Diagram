using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the animation settings for the menu open.
    /// </summary>
    public class MenuAnimationSettings : SfBaseComponent
    {
        [CascadingParameter]
        private IMenu Parent { get; set; }

        /// <summary>
        /// Specifies the time duration to transform object.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 400;

        /// <summary>
        /// Specifies the easing effect applied while transform.
        /// </summary>
        [Parameter]
        public string Easing { get; set; } = "ease";

        /// <summary>
        /// Specifies the effect that shown in the sub menu transform.
        /// The possible effects are:
        ///  None: Specifies the sub menu transform with no animation effect.
        ///  SlideDown: Specifies the sub menu transform with slide down effect.
        ///  ZoomIn: Specifies the sub menu transform with zoom in effect.
        ///  FadeIn: Specifies the sub menu transform with fade in effect.
        /// </summary>
        [Parameter]
        public MenuEffect Effect { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("AnimationSettings", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}