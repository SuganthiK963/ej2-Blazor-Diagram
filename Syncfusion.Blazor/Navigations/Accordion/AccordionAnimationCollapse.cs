using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the animation to appear while collapsing the Accordion item.
    /// </summary>
    public partial class AccordionAnimationCollapse : SfBaseComponent
    {
        [CascadingParameter]
        private AccordionAnimationSettings Parent { get; set; }

        /// <summary>
        /// Specifies the time duration to transform content.
        /// </summary>
        [Parameter]
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Specifies the easing effect applied when transforming the content.
        /// </summary>
        [Parameter]
        public string Easing { get; set; } = "linear";

        /// <summary>
        /// Specifies the animation effect for collapsing the Accordion item.
        /// Default animation is given as SlideUp for collapsing accordion animation. You can also disable the animation by setting the animation effect as none.
        /// </summary>
        [Parameter]
        public AnimationEffect Effect { get; set; } = AnimationEffect.SlideUp;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateCollapseProperties(this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}