using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the Animation property for the Accumulation chart's series.
    /// </summary>
    public partial class AccumulationChartAnimation
    {
        [CascadingParameter]
        private AccumulationChartSeries series { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The option to delay animation of the series.
        /// </summary>
        [Parameter]
        public double Delay { get; set; }

        private double delay { get; set; }

        /// <summary>
        /// The duration of animation in milliseconds.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 1000;

        private double duration { get; set; }

        /// <summary>
        /// If set to true, series gets animated on initial loading.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; } = true;

        private bool enable { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            series.UpdateSeriesProperties("Animation", this);
        }

        internal override void ComponentDispose()
        {
            series = null;
            ChildContent = null;
        }
    }
}