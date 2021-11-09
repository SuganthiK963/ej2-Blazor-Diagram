using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxisTitleStyle : ChartDefaultFont
    {
        [CascadingParameter]
        private ChartAxis axis { get; set; }

        /// <summary>
        /// Unique size of the axis labels.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "14px";

        /// <summary>
        /// Font color for the axis title.
        /// </summary>
        [Parameter]
        public override string Color { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            axis = (ChartAxis)Tracker;
            axis.UpdateAxisProperties("TitleStyle", this);
        }
    }
}
