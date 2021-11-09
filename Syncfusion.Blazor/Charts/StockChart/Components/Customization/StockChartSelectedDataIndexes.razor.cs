using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart selected data indexes.
    /// </summary>
    public partial class StockChartSelectedDataIndexes
    {
        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartSelectedDataIndex> SelectedDataIndexes { get; set; } = new List<StockChartSelectedDataIndex>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.SelectedDataIndexes = SelectedDataIndexes;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}