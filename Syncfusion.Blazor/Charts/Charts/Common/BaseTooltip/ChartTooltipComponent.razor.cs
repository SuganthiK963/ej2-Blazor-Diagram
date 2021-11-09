using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class ChartTooltipComponent
    {
        private object templateContext;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        [Parameter]
        public RenderFragment<object> GivenContent { get; set; }

        [Parameter]
        public string ID { get; set; }

        [Parameter]
        public string Class { get; set; }

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

        internal void ChangeContent(RenderFragment<object> data, ChartInternalLocation location, ChartDataPointInfo chartdata = null, AccumulationChartDataPointInfo accData = null, bool visible = true)
        {
            templateContext = chartdata != null ? chartdata : (object)accData;
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