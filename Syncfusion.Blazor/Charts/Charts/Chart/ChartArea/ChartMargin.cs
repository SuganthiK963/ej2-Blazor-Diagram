using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the margin of the chart.
    /// </summary>
    public class ChartMargin : ChartDefaultMargin
    {
        [CascadingParameter]
        internal SfChart Owner { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.Margin = this;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                Owner.OnLayoutChange();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Owner = null;
        }
    }
}