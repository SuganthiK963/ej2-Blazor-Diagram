using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of border for the striplines.
    /// </summary>
    public class ChartStriplineBorder : ChartDefaultBorder
    {
        [CascadingParameter]
        private ChartStripline Parent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartStripline)Tracker;
            Parent.SetBorderValues(this);
        }
    }
}