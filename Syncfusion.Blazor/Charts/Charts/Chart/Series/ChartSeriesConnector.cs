using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of the series animation.
    /// </summary>
    public class ChartSeriesConnector : ChartDefaultConnector
    {
        private bool isPropertyChanged;

        [CascadingParameter]
        private ChartSeries Series { get; set; }

        /// <summary>
        /// Specifies the width of the connector line.
        /// </summary>
        [Parameter]
        public override double Width { get; set; } = 2;

        /// <summary>
        /// Specifies the color of the connector line.
        /// </summary>
        [Parameter]
        public override string Color { get; set; } = "black";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series = (ChartSeries)Tracker;
            Series.UpdateSeriesProperties("Connector", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                Series.Renderer.ProcessRenderQueue();
            }
        }

        internal override void ComponentDispose()
        {
            Series = null;
            ChildContent = null;
        }
    }
}