using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Configure handlers to handle the events with the Dialog component.
    /// </summary>
    public class DialogEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDialog Parent { get; set; }

        /// <summary>
        /// Event triggers after the dialog has been closed.
        /// </summary>
        [Parameter]
        public EventCallback<CloseEventArgs> Closed { get; set; }

        /// <summary>
        /// Event triggers when the dialog is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Event triggers when the dialog is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Event triggers before the dialog is closed.
        /// If you cancel this event, the dialog remains opened.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeCloseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Event triggers when the user drags the dialog.
        /// </summary>
        [Parameter]
        public EventCallback<DragEventArgs> OnDrag { get; set; }

        /// <summary>
        /// Event triggers when the user begins dragging the dialog.
        /// </summary>
        [Parameter]
        public EventCallback<DragStartEventArgs> OnDragStart { get; set; }

        /// <summary>
        /// Event triggers when the user stops dragging the dialog.
        /// </summary>
        [Parameter]
        public EventCallback<DragStopEventArgs> OnDragStop { get; set; }

        /// <summary>
        /// Event triggers when the dialog is being opened.
        /// If you cancel this event, the dialog remains closed.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Event triggers when the overlay of the dialog is clicked.
        /// </summary>
        [Parameter]
        [Obsolete("This property is deprecated. Use @OnOverlayModalClick property to configure the OnOverlayClick")]
        public EventCallback<MouseEventArgs> OnOverlayClick { get; set; }

        /// <summary>
        /// Event triggers when the overlay of the dialog is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<OverlayModalClickEventArgs> OnOverlayModalClick { get; set; }

        /// <summary>
        /// Event triggers when the user begins to resize a dialog.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnResizeStart { get; set; }

        /// <summary>
        /// Event triggers when the user stops to resize a dialog.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnResizeStop { get; set; }

        /// <summary>
        /// Event triggers when a dialog is opened.
        /// </summary>
        [Parameter]
        public EventCallback<OpenEventArgs> Opened { get; set; }

        /// <summary>
        /// Event triggers when the user resizes the dialog.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> Resizing { get; set; }

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