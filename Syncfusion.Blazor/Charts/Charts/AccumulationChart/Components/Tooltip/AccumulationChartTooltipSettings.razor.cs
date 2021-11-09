using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    ///  Tooltip shows the information about the data points when users hover over data points on your chart.
    /// </summary>
    public partial class AccumulationChartTooltipSettings
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
        /// Options to customize tooltip borders.
        /// </summary>
        [Parameter]
        public AccumulationChartTooltipBorder Border { get; set; } = new AccumulationChartTooltipBorder();

        /// <summary>
        /// Duration for the ToolTip animation.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 300;

        /// <summary>
        /// Enables / Disables the visibility of the tooltip.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// If set to true, ToolTip will animate while moving from one point to another.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Enables / Disables the visibility of the marker.
        /// </summary>
        [Parameter]
        public bool EnableMarker { get; set; } = true;

        /// <summary>
        /// To wrap the tooltip long text based on available space.
        /// This is only application for chart tooltip.
        /// </summary>
        [Parameter]
        public bool EnableTextWrap { get; set; }

        /// <summary>
        /// Fade Out duration for the ToolTip hide.
        /// </summary>
        [Parameter]
        public double FadeOutDuration { get; set; } = 1000;

        /// <summary>
        /// The fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Format the ToolTip content.
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// Header for tooltip.
        /// </summary>
        [Parameter]
        public string Header { get; set; }

        /// <summary>
        /// The fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 0.75;

        /// <summary>
        /// If set to true, a single ToolTip will be displayed for every index.
        /// </summary>
        [Parameter]
        public bool Shared { get; set; }

        /// <summary>
        /// Custom template to format the ToolTip content. Use ${x} and ${y} as the placeholder text to display the corresponding data point.
        /// </summary>
        [Parameter]
        public RenderFragment<object> Template { get; set; }

        /// <summary>
        /// Options to customize the ToolTip text.
        /// </summary>
        [Parameter]
        public AccumulationChartTooltipTextStyle TextStyle { get; set; } = new AccumulationChartTooltipTextStyle();

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            accumulationChart.UpdateChildProperties("Tooltip", this);
        }

        internal override void ComponentDispose()
        {
            accumulationChart = null;
            ChildContent = null;
            Border = null;
            TextStyle = null;
        }

        internal void UpdateTooltipProperties(string key, object keyValue)
        {
            if (key == nameof(Border))
            {
                Border = (AccumulationChartTooltipBorder)keyValue;
            }
            else if (key == nameof(TextStyle))
            {
                TextStyle = (AccumulationChartTooltipTextStyle)keyValue;
            }
        }
    }
}