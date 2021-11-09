using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    public partial class SfTooltip : SfBaseComponent
    {
        /// <summary>
        /// The event `Closed` will be fired when the Tooltip component gets closed.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipEventArgs> Closed { get; set; }

        /// <summary>
        /// The event `Opened` will be fired after the Tooltip component gets opened.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipEventArgs> Opened { get; set; }

        /// <summary>
        /// The event `OnClose` will be fired before the Tooltip hides from the screen.
        /// The Tooltip close can be prevented by setting the cancel argument value to true.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnClose { get; set; }

        /// <summary>
        /// The event `OnCollision` will be fired for every collision fit calculation.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnCollision { get; set; }

        /// <summary>
        /// The event `OnOpen` will be fired before the Tooltip is displayed over the target element.
        /// When one of its arguments `cancel` is set to true, the Tooltip display can be prevented.
        /// This event is mainly used to refresh the Tooltip positions dynamically or to set customized styles in it and so on.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnOpen { get; set; }

        /// <summary>
        /// The event `OnRender` will be fired before the Tooltip and its contents will be added to the DOM.
        /// When one of its arguments `cancel` is set to true, the Tooltip can be prevented from rendering on the page.
        /// This event is mainly used to customize the Tooltip before it shows up on the screen.
        /// For example, to load the AJAX content or to set new animation effects on the Tooltip, this event can be opted.
        /// </summary>
        [Parameter]
        public EventCallback<TooltipEventArgs> OnRender { get; set; }

        /// <summary>
        /// The event `Created` will be fired after the Tooltip component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// The event `Destroyed` will be fired when the Tooltip component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }
    }
}