using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the crosshair line of the chart.
    /// </summary>
    public class ChartCrosshairLine : ChartDefaultBorder
    {
        [CascadingParameter]
        private ChartCrosshairSettings Parent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.UpdateCrosshairProperties("Line", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}