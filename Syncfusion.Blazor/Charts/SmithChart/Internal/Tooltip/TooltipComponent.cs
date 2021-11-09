using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the rendering data of tooltip element in smith chart.
    /// </summary>
    public partial class TooltipComponent
    {
        private object templateContext;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Specifies the content of toottip.
        /// </summary>
        [Parameter]
        public RenderFragment<object> GivenContent { get; set; }

        /// <summary>
        /// Specifies the identification of tooltip.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Specifies the class of tooltip.
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Specifies the style of tooltip.
        /// </summary>
        [Parameter]
        public string Style { get; set; }

        private RenderFragment TemplateElements() => builder =>
        {
            int seq = 0;
            if (GivenContent != null)
            {
                builder.OpenElement(seq++, "div");
                builder.AddContent(3, GivenContent(templateContext));
                builder.CloseElement();
            }
        };

        internal void ChangeContent(RenderFragment<object> data, Point location, SmithChartPoint chartData = null, bool visible = true)
        {
            templateContext = chartData;
            Style = "top:" + location.Y.ToString(culture) + "px;left:" + location.X.ToString(culture) + "px;pointer-events:none; position:absolute;z-index: 1;visibility:" + (visible ? "visible" : "hidden") + ";";
            GivenContent = data;
            InvokeAsync(StateHasChanged);
        }

        internal void TemplateFadeOut()
        {
            GivenContent = null;
            InvokeAsync(StateHasChanged);
        }
    }
}