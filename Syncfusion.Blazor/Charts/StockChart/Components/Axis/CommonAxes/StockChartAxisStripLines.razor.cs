using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization values for stockchart axis striplines.
    /// </summary>
    public partial class StockChartAxisStripLines : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartCommonAxis Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<StockChartCommonStripLine> StripLines { get; set; } = new List<StockChartCommonStripLine>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.StripLines = StripLines;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}