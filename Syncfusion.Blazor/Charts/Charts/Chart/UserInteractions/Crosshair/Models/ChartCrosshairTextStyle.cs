using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to cusomize the tooltip text style of the crosshair tooltip.
    /// </summary>
    public partial class ChartCrosshairTextStyle : ChartDefaultFont
    {
        [CascadingParameter]
        private ChartAxisCrosshairTooltip Parent { get; set; }

        /// <summary>
        /// Color for the text.
        /// </summary>
        [Parameter]
        public override string Color { get; set; }

        /// <summary>
        /// Font size for the text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "13px";

        protected override void OnInitialized()
        {
            base.OnInitializedAsync();
            Parent = (ChartAxisCrosshairTooltip)Tracker;
            Parent.UpdateChildProperties("TextStyle", this);
        }
    }
}