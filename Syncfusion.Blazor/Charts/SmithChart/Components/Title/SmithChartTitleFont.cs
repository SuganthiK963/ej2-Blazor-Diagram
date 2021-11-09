using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the font for title of the chart.
    /// </summary>
    public class SmithChartTitleFont : SmithChartCommonFont
    {
        [CascadingParameter]
        internal SmithChartTitle DynamicParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Font size of the text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "15px";

        internal override void ComponentDispose()
        {
            DynamicParent = null;
            ChildContent = null;
        }
    }
}