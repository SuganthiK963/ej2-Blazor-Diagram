using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart series datalabel.
    /// </summary>
    public partial class StockChartDataLabelSettings : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartMarkerSettings Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the alignment for data Label. They are,
        /// Near: Aligns the label to the left of the point.
        /// Center: Aligns the label to the center of the point.
        /// Far: Aligns the label to the right of the point.
        /// </summary>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Specifies angle for data label.
        /// </summary>
        [Parameter]
        public double Angle { get; set; }

        internal ChartCommonBorder Border { get; set; }

        /// <summary>
        /// Enables rotation for data label.
        /// </summary>
        [Parameter]
        public bool EnableRotation { get; set; }

        /// <summary>
        /// The background color of the data label accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = "transparent";

        internal ChartCommonFont Font { get; set; }

        /// <summary>
        /// Show Datalabel Even two Data Labels Are Overflow.
        /// </summary>
        [Parameter]
        public string LabelIntersectAction { get; set; } = "Hide";

        internal ChartCommonMargin Margin { get; set; }

        /// <summary>
        /// The DataSource field that contains the data label value.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// The opacity for the background.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Specifies the position of the data label. They are,
        /// Outer: Positions the label outside the point.
        /// top: Positions the label on top of the point.
        /// Bottom: Positions the label at the bottom of the point.
        /// Middle: Positions the label to the middle of the point.
        /// Auto: Positions the label based on series.
        /// </summary>
        [Parameter]
        public LabelPosition Position { get; set; } = LabelPosition.Auto;

        /// <summary>
        /// The roundedCornerX for the data label. It requires `Border` values not to be null.
        /// </summary>
        [Parameter]
        public double Rx { get; set; } = 5;

        /// <summary>
        /// The roundedCornerY for the data label. It requires `Border` values not to be null.
        /// </summary>
        [Parameter]
        public double Ry { get; set; } = 5;

        /// <summary>
        /// Custom template to show the data label. Use ${point.x} and ${point.y} as a placeholder
        /// text to display the corresponding data point.
        /// </summary>
        [Parameter]
        public string Template { get; set; }

        /// <summary>
        /// If set true, data label for series renders.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.DataLabel = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            Margin = null;
            Font = null;
        }
    }
}