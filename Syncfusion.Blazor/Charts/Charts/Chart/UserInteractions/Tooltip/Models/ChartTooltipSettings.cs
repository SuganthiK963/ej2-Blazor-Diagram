using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the tooltip.
    /// </summary>
    public class ChartTooltipSettings : ChartSubComponent
    {
        [CascadingParameter]
        private SfChart Parent { get; set; }

        /// <summary>
        /// Options to customize tooltip borders.
        /// </summary>
        [Parameter]
        public ChartTooltipBorder Border { get; set; } = new ChartTooltipBorder();

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
        public ChartTooltipTextStyle TextStyle { get; set; } = new ChartTooltipTextStyle();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent.Tooltip = this;
        }

        internal void UpdateTooltipProperties(string key, object keyValue)
        {
            if (key == nameof(Border))
            {
                Border = (ChartTooltipBorder)keyValue;
            }
            else if (key == nameof(TextStyle))
            {
                TextStyle = (ChartTooltipTextStyle)keyValue;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            TextStyle = null;
        }
    }
}