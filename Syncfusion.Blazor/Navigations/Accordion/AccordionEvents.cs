using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Configure event handlers to handle the events with the Accordion component.
    /// </summary>
    public partial class AccordionEvents : SfBaseComponent
    {
        [CascadingParameter]
        private SfAccordion Parent { get; set; }

        /// <summary>
        /// The event triggers when clicking anywhere within the Accordion.
        /// </summary>
        [Parameter]
        public EventCallback<AccordionClickArgs> Clicked { get; set; }

        /// <summary>
        /// The event triggers once the control rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// The event triggers when the control gets destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// The event triggers after the item gets expanded.
        /// </summary>
        [Parameter]
        public EventCallback<ExpandedEventArgs> Expanded { get; set; }

        /// <summary>
        /// The event triggers before the item gets expanded.
        /// </summary>
        [Parameter]
        public EventCallback<ExpandEventArgs> Expanding { get; set; }

        /// <summary>
        /// The event triggers after the item gets collapsed.
        /// </summary>
        [Parameter]
        public EventCallback<CollapsedEventArgs> Collapsed { get; set; }

        /// <summary>
        /// The event triggers before the item gets collapsed.
        /// </summary>
        [Parameter]
        public EventCallback<CollapseEventArgs> Collapsing { get; set; }

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