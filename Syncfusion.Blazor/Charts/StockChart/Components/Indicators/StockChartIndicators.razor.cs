using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart indicators.
    /// </summary>
    public partial class StockChartIndicators : SfBaseComponent
    {
        [CascadingParameter]
        private SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartIndicator> Indicators { get; set; } = new List<StockChartIndicator>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Indicators = Indicators;
        }

        internal override void ComponentDispose()
        {
            Indicators = null;
            StockChartInstance = null;
            ChildContent = null;
        }
    }
}