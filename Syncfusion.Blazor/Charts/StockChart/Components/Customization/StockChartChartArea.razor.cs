using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart chart area.
    /// </summary>
    public partial class StockChartChartArea : SfBaseComponent
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The background of the chart area that accepts value in hex and rgba as a valid CSS color string..
        /// </summary>
        [Parameter]
        public string Background { get; set; } = "transparent";

        /// <summary>
        /// The background image of the chart area that accepts value in string as url link or location of an image.
        /// </summary>
        [Parameter]
        public string BackgroundImage { get; set; }

        internal StockChartChartAreaBorder Border { get; set; } = new StockChartChartAreaBorder();

        /// <summary>
        /// The opacity for background.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.ChartArea = this;
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
            ChildContent = null;
            Border = null;
        }
    }
}