using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Spectified the co-ordinates of the legend in the chart.
    /// </summary>
    public partial class AccumulationChartLocation
    {
        [CascadingParameter]
        private AccumulationChartLegendSettings accumulationChartLegend { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            accumulationChartLegend.UpdateLegendProperties("Location", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)accumulationChartLegend).PropertyChanges.TryAdd(nameof(accumulationChartLegend.Location), this);
                PropertyChanges.Clear();
                await accumulationChartLegend.LegendPropertyChanged();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            accumulationChartLegend = null;
            ChildContent = null;
        }
    }
}