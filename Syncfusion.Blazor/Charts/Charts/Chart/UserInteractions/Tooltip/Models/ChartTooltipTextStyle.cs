using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the text style of the tooltip.
    /// </summary>
    public class ChartTooltipTextStyle : ChartDefaultFont
    {
        [CascadingParameter]
        private ChartTooltipSettings Parent { get; set; }

        /// <summary>
        /// Options to customize the tooltip.
        /// </summary>
        public override string Size { get; set; } = "13px";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartTooltipSettings)Tracker;
            Parent.UpdateTooltipProperties("TextStyle", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
            ChildContent = null;
        }
    }
}