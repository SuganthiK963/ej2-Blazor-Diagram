using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the configurartion of the title style in the Accumulation chart.
    /// </summary>
    public partial class AccumulationChartTitleStyle
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
        public override string Size { get; set; } = "15px";

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
            accumulationChart.UpdateChildProperties("TitleStyle", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Count != 0 && IsRendered)
            {
                ((SfBaseComponent)accumulationChart).PropertyChanges.TryAdd(nameof(IAccumulationChart.TitleStyle), this);
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