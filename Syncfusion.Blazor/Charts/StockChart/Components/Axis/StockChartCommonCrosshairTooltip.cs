using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart common crosshair tooltip.
    /// </summary>
    public class StockChartCommonCrosshairTooltip : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartCommonAxis Parent { get; set; }

        /// <summary>
        /// If set to true, crosshair ToolTip will be visible.
        /// Default value is false.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// The fill color of the ToolTip accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        internal ChartCommonFont TextStyle { get; set; } = new ChartCommonFont();

        internal override void ComponentDispose()
        {
            Parent = null;
            TextStyle = null;
        }
    }
}