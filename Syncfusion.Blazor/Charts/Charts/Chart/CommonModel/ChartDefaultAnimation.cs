using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the animation options of chart component.
    /// </summary>
    public partial class ChartDefaultAnimation : ChartSubComponent
    {
        [CascadingParameter]
        internal SfChart Container { get; set; }

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

        internal void SetTrendlineAnimation(bool enable, double duration, double delay)
        {
            Enable = enable;
            Duration = duration;
            Delay = delay;
        }
    }
}