using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    public partial class SfTooltip : SfBaseComponent
    {
        /// <summary>
        /// It is used to show the Tooltip on the specified target with specific animation settings.
        /// You can also pass the additional arguments like target element in which the tooltip should appear and animation settings for the tooltip open action.
        /// </summary>
        /// <param name="element">element.</param>
        /// <param name="animation">animation.</param>
        /// <returns>="Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Open(ElementReference? element = null, TooltipAnimationSettings animation = null)
        {
            if (animation == null)
            {
                animation = Animation.Open;
            }

            if (element == null)
            {
                element = tooltipElement;
            }
            if (isScriptRendered)
            {
                await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.showTooltip", new object[] { tooltipElement, element, animation, Target });
            }
        }

        /// <summary>
        /// It is used to show the Tooltip on the specified target with specific animation settings.
        /// You can also pass the additional arguments like target element in which the tooltip should appear and animation settings for the tooltip open action.
        /// </summary>
        /// <param name="element">element.</param>
        /// <param name="animation">animation.</param>
        /// <returns>="Task".</returns>
        public async Task OpenAsync(ElementReference? element = null, TooltipAnimationSettings animation = null)
        {
            await Open(element, animation);
        }

        /// <summary>
        /// It is used to hide the Tooltip with a specific animation effect. You can pass the animation settings for tooltip close action.
        /// </summary>
        /// <param name="animation">animation.</param>
        /// <returns>="Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Close(TooltipAnimationSettings animation = null)
        {
            if (animation == null)
            {
                animation = Animation.Close;
            }
            if (isScriptRendered)
            {
                await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.hideTooltip", tooltipElement, animation);
            }
        }

        /// <summary>
        /// It is used to hide the Tooltip with a specific animation effect. You can pass the animation settings for tooltip close action.
        /// </summary>
        /// <param name="animation">animation.</param>
        /// <returns>="Task".</returns>
        public async Task CloseAsync(TooltipAnimationSettings animation = null)
        {
            await Close(animation);
        }

        /// <summary>
        /// Refresh the tooltip component when the target element is dynamically used.
        /// </summary>
        /// <returns>="Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Refresh()
        {
            await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.refresh", tooltipElement);
        }

        /// <summary>
        /// Refresh the tooltip component when the target element is dynamically used.
        /// </summary>
        /// <returns>="Task".</returns>
        public async Task RefreshAsync()
        {
            await Refresh();
        }

        /// <summary>
        /// Dynamically refreshes the tooltip element position based on the target element.
        /// </summary>
        /// <param name="target">target.</param>
        /// <returns>="Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RefreshPosition(ElementReference? target = null)
        {
            await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.refreshPosition", tooltipElement, target, Target);
        }

        /// <summary>
        /// Dynamically refreshes the tooltip element position based on the target element.
        /// </summary>
        /// <param name="target">target.</param>
        /// <returns>="Task".</returns>
        public async Task RefreshPositionAsync(ElementReference? target = null)
        {
            await RefreshPosition(target);
        }
    }
}
