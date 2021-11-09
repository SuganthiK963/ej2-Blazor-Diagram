using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart animation.
    /// </summary>
    public partial class StockChartCommonAnimation : SfBaseComponent
    {
        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The option to delay animation of the series.
        /// </summary>
        [Parameter]
        public double Delay { get; set; }

        /// <summary>
        /// The duration of animation in milliseconds.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 1000;

        /// <summary>
        /// If set to true, series gets animated on initial loading.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; } = true;

        internal override void ComponentDispose()
        {
            ChildContent = null;
        }
    }
}