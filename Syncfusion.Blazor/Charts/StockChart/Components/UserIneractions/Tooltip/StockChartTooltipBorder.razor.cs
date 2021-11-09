using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart tooltip border.
    /// </summary>
    public partial class StockChartTooltipBorder : ChartCommonBorder
    {
        [CascadingParameter]
        internal StockChartTooltipSettings TooltipSettings { get; set; }

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
            TooltipSettings.Border = this;
        }

        internal override void ComponentDispose()
        {
            TooltipSettings = null;
            ChildContent = null;
        }
    }
}