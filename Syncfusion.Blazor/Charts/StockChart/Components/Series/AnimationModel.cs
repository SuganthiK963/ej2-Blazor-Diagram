using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Set and gets the trend line animation.
    /// </summary>
    public class AnimationModel : StockChartCommonAnimation
    {
        [CascadingParameter]
        internal StockChartTrendline Parent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Animation = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}