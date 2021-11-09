using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Configure handlers to handle the events with the Chip component.
    /// </summary>
    public class ChipEvents : SfBaseComponent
    {
        /// <summary>
        /// Indicates the SfChip component.
        /// </summary>
        [CascadingParameter]
        private SfChip BaseParent { get; set; }

        /// <summary>
        /// This click event will get triggered once the chip is before click.
        /// </summary>
        [Parameter]
        public EventCallback<ChipEventArgs> OnBeforeClick { get; set; }

        /// <summary>
        /// This click event will get triggered once the chip is clicked.
        /// </summary>
        [Parameter]
        public EventCallback<ChipEventArgs> OnClick { get; set; }

        /// <summary>
        /// This created event will get triggered once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// This created event will get triggered once the component successfuly disposed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// This delete event will get triggered before removing the chip.
        /// </summary>
        [Parameter]
        public EventCallback<ChipEventArgs> OnDelete { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.ChipEvents = this;
        }
    }
}