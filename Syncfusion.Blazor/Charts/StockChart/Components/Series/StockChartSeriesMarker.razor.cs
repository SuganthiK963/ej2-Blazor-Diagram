using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart series marker.
    /// </summary>
    public partial class StockChartSeriesMarker : SfBaseComponent
    {
        internal StockChartDataLabel DataLabel { get; set; } = new StockChartDataLabel();

        internal StockChartMarkerBorder Border { get; set; } = new StockChartMarkerBorder();

        internal OffsetModel Offset { get; set; } = new OffsetModel();

        [CascadingParameter]
        internal StockChartSeries Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        ///  The fill color of the marker that accepts value in hex and rgba as a valid CSS color string. By default, it will take series' color.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// The height of the marker in pixels.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 5;

        /// <summary>
        /// The URL for the Image that is to be displayed as a marker.  It requires marker `Shape` value to be an `Image`.
        /// </summary>
        [Parameter]
#pragma warning disable CA1056
        public string ImageUrl { get; set; } = string.Empty;
#pragma warning restore CA1056

        /// <summary>
        /// The opacity of the marker.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// The different shape of a marker:
        /// Circle
        /// Rectangle
        /// Triangle
        /// Diamond
        /// HorizontalLine
        /// VerticalLine
        /// Pentagon
        /// InvertedTriangle
        /// Image.
        /// </summary>
        [Parameter]
        public ChartShape Shape { get; set; } = ChartShape.Circle;

        /// <summary>
        /// If set to true the marker for series is rendered. This is applicable only for line and area type series.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// The width of the marker in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 5;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Marker = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            DataLabel = null;
        }
    }
}