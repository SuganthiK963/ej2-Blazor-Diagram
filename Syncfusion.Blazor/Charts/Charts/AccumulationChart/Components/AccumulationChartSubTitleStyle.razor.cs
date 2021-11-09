using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the configurartion of the subTitle style in the Accumulation chart.
    /// </summary>
    public partial class AccumulationChartSubTitleStyle
    {
        [CascadingParameter]
        private IAccumulationChart accumulationChart { get; set; }

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
        public override string Size { get; set; } = "11px";

        /// <summary>
        /// FontFamily for the text.
        /// </summary>
        [Parameter]
        public override string FontFamily { get; set; } = "Segoe UI";

        /// <summary>
        /// FontFamily for the text.
        /// </summary>
        [Parameter]
        public override string FontWeight { get; set; } = "500";

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            accumulationChart.UpdateChildProperties("SubTitleStyle", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)accumulationChart).PropertyChanges.TryAdd(nameof(IAccumulationChart.SubTitleStyle), this);
                await accumulationChart.OnAccumulationChartParametersSet();
#pragma warning restore CA2007
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            accumulationChart = null;
            ChildContent = null;
        }
    }
}