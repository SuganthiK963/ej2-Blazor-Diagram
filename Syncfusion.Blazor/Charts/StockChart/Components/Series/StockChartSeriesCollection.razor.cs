using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart series collection.
    /// </summary>
    public partial class StockChartSeriesCollection : SfBaseComponent
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartSeries> Series { get; set; } = new List<StockChartSeries>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Series = Series;
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
            ChildContent = null;
            Series = null;
        }
    }
}