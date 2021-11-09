using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets Center of the Accumulation chart.
    /// </summary>
    public partial class AccumulationChartCenter
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
        /// X value of the center.
        /// </summary>
        [Parameter]
        public string X { get; set; } = "50%";

        private string x { get; set; }

        /// <summary>
        /// Y value of the center.
        /// </summary>
        [Parameter]
        public string Y { get; set; } = "50%";

        private string y { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            accumulationChart.UpdateChildProperties("Center", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            x = X = NotifyPropertyChanges(nameof(X), X, x);
            y = Y = NotifyPropertyChanges(nameof(Y), Y, y);
            if (PropertyChanges.Any())
            {
                ((SfBaseComponent)accumulationChart).PropertyChanges.TryAdd(nameof(IAccumulationChart.Center), this);
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