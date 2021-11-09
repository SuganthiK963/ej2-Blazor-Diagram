using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the collection of technical indicators, that are used in financial markets.
    /// </summary>
    public class ChartIndicators : ChartSubComponent
    {
        [CascadingParameter]
        internal SfChart Chart { get; set; }

        internal override void ComponentDispose()
        {
            Chart = null;
            ChildContent = null;
        }
    }
}