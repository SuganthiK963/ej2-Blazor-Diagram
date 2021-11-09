using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options to customize the border for range navigator tooltip.
    /// </summary>
    public partial class RangeNavigatorTooltipBorder : ChartCommonBorder
    {
        [CascadingParameter]
        internal RangeNavigatorRangeTooltipSettings Tooltip { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Tooltip.UpdateTooltipProperties("Border", this);
        }

        internal override void ComponentDispose()
        {
            Tooltip = null;
        }
    }
}