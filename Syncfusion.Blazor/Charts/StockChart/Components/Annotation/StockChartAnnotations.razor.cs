using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies annotation collection for stockchart.
    /// </summary>
    public partial class StockChartAnnotations
    {
        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartAnnotation> Annotations { get; set; } = new List<StockChartAnnotation>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            StockChartInstance.Annotations = Annotations;
        }

        internal override void ComponentDispose()
        {
            Annotations = null;
            StockChartInstance = null;
            ChildContent = null;
        }
    }
}