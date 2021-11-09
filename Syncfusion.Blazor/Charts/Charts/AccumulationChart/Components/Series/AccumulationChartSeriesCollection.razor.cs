using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Series represents the collection of data in the Accumulation chart.
    /// </summary>
    public partial class AccumulationChartSeriesCollection
    {
        [CascadingParameter]
        private IAccumulationChart accumulationChart { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        internal List<AccumulationChartSeries> SeriesCollection { get; set; } = new List<AccumulationChartSeries>();

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            accumulationChart.UpdateChildProperties("Series", SeriesCollection);
        }

        internal override void ComponentDispose()
        {
            accumulationChart = null;
            SeriesCollection = null;
            ChildContent = null;
        }
    }
}