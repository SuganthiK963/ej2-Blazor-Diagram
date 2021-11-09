using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Partial Class SfSidebar.
    /// </summary>
    public partial class SfSidebar : SfBaseComponent
    {
        /// <summary>
        /// Triggers when the state(expand/collapse) of the component is changed.
        /// </summary>
        [Parameter]
        public EventCallback<ChangeEventArgs> Changed { get; set; }

        /// <summary>
        /// Triggers when the component is ready to close.
        /// </summary>
        [Parameter]
        public EventCallback<EventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the component gets destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the component is ready to open.
        /// </summary>
        [Parameter]
        public EventCallback<EventArgs> OnOpen { get; set; }
    }
}