using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart rows.
    /// </summary>
    public partial class StockChartRows : SfBaseComponent
    {
        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartRow> Rows { get; set; } = new List<StockChartRow>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Rows = Rows;
        }

        internal override void ComponentDispose()
        {
            Rows = null;
            Parent = null;
            ChildContent = null;
        }
    }
}