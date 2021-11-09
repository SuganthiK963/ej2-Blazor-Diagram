using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Annotations are user defined HTML element that can be placed on chart
    /// We can use annotations to pile up the visual elegance of the chart.
    /// </summary>
    public partial class AccumulationChartAnnotations
    {
        [CascadingParameter]
        private IAccumulationChart accumulationChart { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        internal List<AccumulationChartAnnotation> Annotations { get; set; } = new List<AccumulationChartAnnotation>();

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            accumulationChart.UpdateChildProperties("Annotations", Annotations);
        }

        internal override void ComponentDispose()
        {
            accumulationChart = null;
            ChildContent = null;
            Annotations = null;
        }
    }
}