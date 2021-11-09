using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the text style of the accumulation chart's texts.
    /// </summary>
    public partial class AccumulationChartLegendFont
    {
        [CascadingParameter]
        private AccumulationChartLegendSettings accumulationChartLegend { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Unique size of the axis labels.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "13px";

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            accumulationChartLegend.UpdateLegendProperties("TextStyle", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)accumulationChartLegend).PropertyChanges.TryAdd(nameof(accumulationChartLegend.TextStyle), this);
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