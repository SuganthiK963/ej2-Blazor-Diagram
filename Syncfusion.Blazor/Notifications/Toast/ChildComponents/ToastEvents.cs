using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Configure the event handlers to handle the events with Toast component.
    /// </summary>
    public class ToastEvents : SfBaseComponent
    {
        [CascadingParameter]
        private SfToast Parent { get; set; }

        /// <summary>
        /// Triggers the event before the toast shown.
        /// </summary>
        [Parameter]
        public EventCallback<ToastBeforeOpenArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers the event before the toast close.
        /// </summary>
        [Parameter]
        public EventCallback<ToastBeforeCloseArgs> OnClose { get; set; }

        /// <summary>
        /// The event will be fired while clicking on the Toast.
        /// </summary>
        [Parameter]
        public EventCallback<ToastClickEventArgs> OnClick { get; set; }

        /// <summary>
        /// Trigger the event after the Toast hides.
        /// </summary>
        [Parameter]
        public EventCallback<ToastCloseArgs> Closed { get; set; }

        /// <summary>
        /// Triggers the event after the Toast gets created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers the event after the Toast gets destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers the event after the Toast shown on the target container.
        /// </summary>
        [Parameter]
        public EventCallback<ToastOpenArgs> Opened { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Delegates = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}
