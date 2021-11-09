using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Lists
{
    /// <summary>
    /// Configure handlers to handle the events with the ListView component.
    /// </summary>
    /// <typeparam name="TValue">The generic type parameter.</typeparam>
    public partial class ListViewEvents<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Specifies the base parent.
        /// </summary>
        [CascadingParameter]
        protected SfListView<TValue> BaseParent { get; set; }

        /// <summary>
        /// Triggers when each ListView action begins.
        /// </summary>
        [Parameter]
        public EventCallback<ActionEventsArgs> OnActionBegin { get; set; }

        /// <summary>
        /// Triggers when each ListView action is completed.
        /// </summary>
        [Parameter]
        public EventCallback<ActionEventsArgs> OnActionComplete { get; set; }

        /// <summary>
        /// Triggers when the data fetch request from the remote server fails.
        /// </summary>
        [Parameter]
        public EventCallback<ActionFailureEventsArgs> OnActionFailure { get; set; }

        /// <summary>
        /// Triggers when a list item in the component is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<ClickEventArgs<TValue>> Clicked { get; set; }

        /// <summary>
        /// Triggers when the back icon is clicked in the nested list item.
        /// </summary>
        [Parameter]
        public EventCallback<BackEventArgs<TValue>> OnBack { get; set; }

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
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.ListViewEvents = this;
        }
    }
}
