using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart indicator lower line.
    /// </summary>
    public partial class StockChartLowerLine : StockChartCommonConnector
    {
        [CascadingParameter]
        internal StockChartIndicator Indicator { get; set; }

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
            Indicator.LowerLine = this;
        }

        internal override void ComponentDispose()
        {
            Indicator = null;
            ChildContent = null;
        }
    }
}