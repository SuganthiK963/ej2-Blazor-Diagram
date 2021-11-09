using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the animation options of chart component.
    /// </summary>
    public partial class ChartCommonAnimation : SfDataBoundComponent
    {
        /// <summary>
        /// Sets and gets the option for animation delay of the series.
        /// </summary>
        [Parameter]
        public double Delay { get; set; } = 0;

        /// <summary>
        /// Sets and gets the duration of animation in milliseconds.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 1000;

        /// <summary>
        /// Option to series gets animated on initial loading.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; } = true;
    }
}