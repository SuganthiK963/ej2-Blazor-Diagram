using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the margin of the Accumulation chart.
    /// </summary>
    public partial class AccumulationChartMargin
    {
        [CascadingParameter]
        private IAccumulationChart accumulationChart { get; set; }

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
            accumulationChart.UpdateChildProperties("Margin", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)accumulationChart).PropertyChanges.TryAdd(nameof(IAccumulationChart.Margin), this);
                PropertyChanges.Clear();
                await accumulationChart.OnAccumulationChartParametersSet();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            accumulationChart = null;
            ChildContent = null;
        }
    }
}