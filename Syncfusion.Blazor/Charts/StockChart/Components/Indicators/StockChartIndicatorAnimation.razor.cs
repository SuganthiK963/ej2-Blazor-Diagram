using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart indicator animation.
    /// </summary>
    public partial class StockChartIndicatorAnimation : StockChartCommonAnimation
    {
        [CascadingParameter]
        internal StockChartIndicator Indicator { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Indicator.Animation = this;
        }

        internal override void ComponentDispose()
        {
            Indicator = null;
        }
    }
}