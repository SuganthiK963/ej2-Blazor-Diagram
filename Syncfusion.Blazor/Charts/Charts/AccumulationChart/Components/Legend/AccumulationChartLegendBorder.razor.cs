using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the border for that Accumulation Chart's legend.
    /// </summary>
    public partial class AccumulationChartLegendBorder
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
            accumulationChartLegend.UpdateLegendProperties("Border", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                ((SfBaseComponent)accumulationChartLegend).PropertyChanges.TryAdd(nameof(accumulationChartLegend.Border), this);
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