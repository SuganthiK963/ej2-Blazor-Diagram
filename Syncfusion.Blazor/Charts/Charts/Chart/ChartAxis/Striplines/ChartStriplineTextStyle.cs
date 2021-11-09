using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of textstyle for the stripline text.
    /// </summary>
    public class ChartStriplineTextStyle : ChartDefaultFont
    {
        [CascadingParameter]
        private ChartStripline Parent { get; set; }

        /// <summary>
        /// Unique size of the axis labels.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "12px";

        /// <summary>
        /// Font color for the axis title.
        /// </summary>
        [Parameter]
        public override string Color { get; set; } = "#353535";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartStripline)Tracker;
            Parent.SetTextStyleValue(this);
        }
    }
}