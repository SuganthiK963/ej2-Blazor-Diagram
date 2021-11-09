using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart stockevents.
    /// </summary>
    public partial class StockChartStockEvents
    {
        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartStockEvent> StockEvents { get; set; } = new List<StockChartStockEvent>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.StockEvents = StockEvents;
        }

        internal override void ComponentDispose()
        {
            StockEvents = null;
            Parent = null;
            ChildContent = null;
        }
    }
}