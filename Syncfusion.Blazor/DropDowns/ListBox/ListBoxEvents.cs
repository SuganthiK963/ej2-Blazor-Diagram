using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Specifies the ListBox Events of the component.
    /// </summary>
    public class ListBoxEvents<TValue, TItem> : SfBaseComponent
    {
        [CascadingParameter]
        private SfListBox<TValue, TItem> Parent { get; set; }

        /// <summary>
        /// Triggers before fetching data from the remote server.
        /// </summary>
        [Parameter]
        public EventCallback<ActionBeginEventArgs> OnActionBegin { get; set; }

        /// <summary>
        /// Triggers after data is fetched successfully from the remote server.
        /// </summary>
        [Parameter]
        public EventCallback<ActionCompleteEventArgs<TItem>> OnActionComplete { get; set; }

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [Parameter]
        public EventCallback<object> OnActionFailure { get; set; }

        /// <summary>
        /// Triggers before dropping the list item on another list item.
        /// </summary>
        [Parameter]
        public EventCallback<DropEventArgs<TItem>> OnDrop { get; set; }

        /// <summary>
        /// Triggers while rendering each list item.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeItemRenderEventArgs<TItem>> OnItemRender { get; set; }

        /// <summary>
        /// Triggers while select / unselect the list item.
        /// </summary>
        [Parameter]
        public EventCallback<ListBoxChangeEventArgs<TValue, TItem>> ValueChange { get; set; }

        /// <summary>
        /// Triggers when the component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers after dragging the list item.
        /// </summary>
        [Parameter]
        public EventCallback<DragEventArgs<TItem>> DragStart { get; set; }

        /// <summary>
        /// Triggers before dropping the list item on another list item.
        /// </summary>
        [Parameter]
        public EventCallback<DragEventArgs<TItem>> Dropped { get; set; }

        /// <summary>
        /// Triggers on typing a character in the component.
        /// </summary>
        [Parameter]
        public EventCallback<FilteringEventArgs> ItemSelected { get; set; }

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