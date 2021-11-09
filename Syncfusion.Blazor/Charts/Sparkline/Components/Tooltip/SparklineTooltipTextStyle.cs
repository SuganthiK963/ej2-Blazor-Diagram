using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the tooltip text style of the sparkline component.
    /// </summary>
    public partial class SparklineTooltipTextStyle : FontSettings
    {
        [CascadingParameter]
        internal ISparklineTooltip Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties(nameof(SparklineTooltipTextStyle), this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}