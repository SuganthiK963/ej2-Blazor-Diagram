using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart stockevents text style.
    /// </summary>
    public partial class StockChartStockEventsTextStyle : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartStockEvent Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Color for the text.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// FontFamily for the text.
        /// </summary>
        [Parameter]
        public string FontFamily { get; set; }

        /// <summary>
        /// FontStyle for the text.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary>
        /// FontWeight for the text.
        /// </summary>
        [Parameter]
        public string FontWeight { get; set; } = "Normal";

        /// <summary>
        /// Opacity for the text.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Font size for the text.
        /// </summary>
        [Parameter]
        public string Size { get; set; } = "16px";

        /// <summary>
        /// Text alignment.
        /// </summary>
        [Parameter]
        public Alignment TextAlignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Specifies the chart title text overflow.
        /// </summary>
        [Parameter]
        public TextOverflow TextOverflow { get; set; } = TextOverflow.Trim;

        internal ChartFontOptions GetCommonFont()
        {
#pragma warning disable BL0005
            return new ChartFontOptions() { Color = Color, Size = Size, FontFamily = FontFamily, FontWeight = FontWeight, FontStyle = FontStyle };
#pragma warning restore BL0005
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.TextStyle = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}