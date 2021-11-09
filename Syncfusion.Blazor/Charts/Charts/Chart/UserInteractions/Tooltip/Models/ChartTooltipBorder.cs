using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the border of the tooltip.
    /// </summary>
    public class ChartTooltipBorder : ChartDefaultBorder
    {
        [CascadingParameter]
        private ChartTooltipSettings Parent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartTooltipSettings)Tracker;
            Parent.UpdateTooltipProperties("Border", this);
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            Parent = null;
            ChildContent = null;
        }
    }
}