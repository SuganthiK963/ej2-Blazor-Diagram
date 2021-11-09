using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Configure event handlers to handle the events with the Tabs component.
    /// </summary>
    public partial class TabEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal SfTab Parent { get; set; }

        /// <summary>
        /// The event triggers after adding the item to the Tabs.
        /// </summary>
        [Parameter]
        public EventCallback<AddEventArgs> Added { get; set; }

        /// <summary>
        /// The event triggers before adding the tab item to the Tabs.
        /// </summary>
        [Parameter]
        public EventCallback<AddEventArgs> Adding { get; set; }

        /// <summary>
        /// The event triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// The event triggers when the component gets destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// The event triggers after removing the item from the Tabs.
        /// </summary>
        [Parameter]
        public EventCallback<RemoveEventArgs> Removed { get; set; }

        /// <summary>
        /// The event triggers before removing the item from the Tabs.
        /// </summary>
        [Parameter]
        public EventCallback<RemoveEventArgs> Removing { get; set; }

        /// <summary>
        /// The event triggers after the tab item gets selected.
        /// </summary>
        [Parameter]
        public EventCallback<SelectEventArgs> Selected { get; set; }

        /// <summary>
        /// The event triggers before the tab item gets selected.
        /// </summary>
        [Parameter]
        public EventCallback<SelectingEventArgs> Selecting { get; set; }

        /// <summary>
        /// The event triggers when the Tab item drag starts.
        /// </summary>
        [Parameter]
        public EventCallback<DragEventArgs> OnDragStart { get; set; }

        /// <summary>
        /// The event triggers after the tab item gets dropped.
        /// </summary>
        [Parameter]
        public EventCallback<DragEventArgs> Dragged { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Delegates = this;
        }
    }
}