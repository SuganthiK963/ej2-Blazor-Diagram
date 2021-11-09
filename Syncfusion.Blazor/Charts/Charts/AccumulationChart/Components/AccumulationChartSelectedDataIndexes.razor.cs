using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// SelectedData in the accumulation chart appears inimitable from the rest of the data points.
    /// </summary>
    public partial class AccumulationChartSelectedDataIndexes
    {
        [CascadingParameter]
        private IAccumulationChart Chart { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        internal List<AccumulationChartSelectedDataIndex> SelectedData { get; set; } = new List<AccumulationChartSelectedDataIndex>();

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            Chart.UpdateChildProperties("SelectedDataIndexes", SelectedData);
        }

        internal override void ComponentDispose()
        {
            Chart = null;
            ChildContent = null;
            SelectedData = null;
        }
    }
}