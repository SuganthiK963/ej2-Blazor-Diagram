using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart periods.
    /// </summary>
    public partial class StockChartPeriods
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartPeriod> Periods { get; set; } = new List<StockChartPeriod>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Periods = Periods;
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
            ChildContent = null;
            Periods = null;
        }
    }
}