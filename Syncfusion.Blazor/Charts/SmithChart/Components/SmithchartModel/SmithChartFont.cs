using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the font style of the chart.
    /// </summary>
    public partial class SmithChartFont
    {
        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Font size for text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "12px";

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}