using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart title.
    /// </summary>
    public partial class StockChartTitleStyle : SfBaseComponent
    {
        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

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
        /// text alignment.
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
            return new ChartFontOptions() { Color = Color, Size = Size, FontFamily = FontFamily, FontStyle = FontStyle, FontWeight = FontWeight, /*Opacity = Opacity,*/ TextAlignment = TextAlignment, TextOverflow = TextOverflow };
#pragma warning restore BL0005
        }

        internal FontOptions GetFontOptions()
        {
            return new FontOptions { Color = Color, Size = Size, FontFamily = FontFamily, FontWeight = FontWeight, FontStyle = FontStyle };
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.TitleStyle = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}