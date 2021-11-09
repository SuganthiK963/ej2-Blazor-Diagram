using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the series collection of the chart.
    /// </summary>
    public partial class SmithChartSeriesCollection
    {
        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<SmithChartSeries> Series { get; set; } = new List<SmithChartSeries>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Series = Series;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Series = null;
        }
    }
}