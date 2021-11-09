using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the tooltip text style of range navigator tooltip.
    /// </summary>
    public partial class RangeNavigatorTooltipTextStyle : ChartCommonFont
    {
        [CascadingParameter]
        internal RangeNavigatorRangeTooltipSettings Tooltip { get; set; }

        /// <summary>
        /// Gets and sets the size of the tooltip text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "12px";

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Tooltip.UpdateTooltipProperties("TextStyle", this);
        }

        internal override void ComponentDispose()
        {
            Tooltip = null;
        }
    }
}