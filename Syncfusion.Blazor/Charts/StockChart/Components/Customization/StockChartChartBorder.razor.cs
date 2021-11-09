using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart border.
    /// </summary>
    public partial class StockChartChartBorder : ChartCommonBorder
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public override string Color { get; set; } = "#DDDDDD";

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Border = this;
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
            ChildContent = null;
        }
    }
}