using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart trendlines.
    /// </summary>
    public partial class StockChartTrendlines : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartSeries Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartTrendline> Trendlines { get; set; } = new List<StockChartTrendline>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Trendlines = Trendlines;
        }

        internal override void ComponentDispose()
        {
            Trendlines = null;
            Parent = null;
            ChildContent = null;
        }
    }
}