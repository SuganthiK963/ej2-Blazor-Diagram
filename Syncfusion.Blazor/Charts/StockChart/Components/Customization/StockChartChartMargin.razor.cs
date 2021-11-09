using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart margin.
    /// </summary>
    public partial class StockChartChartMargin : ChartCommonMargin
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Margin = this;
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
            ChildContent = null;
        }
    }
}