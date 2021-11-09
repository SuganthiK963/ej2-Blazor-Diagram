using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart datalabel font.
    /// </summary>
    public partial class StockChartFont : SfBaseComponent
    {
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

        internal override void ComponentDispose()
        {
            ChildContent = null;
        }
    }
}