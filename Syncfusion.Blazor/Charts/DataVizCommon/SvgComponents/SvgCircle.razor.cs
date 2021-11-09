using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgCircle
    {
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;
        [Parameter]
        public override string Id { get; set; }
        [Parameter]
        public string Cx { get; set; }
        [Parameter]
        public string Cy { get; set; }
        [Parameter]
        public string R { get; set; }
        [Parameter]
        public string StrokeDashArray { get; set; }
        [Parameter]
        public string Stroke { get; set; }
        [Parameter]
        public double StrokeWidth { get; set; } = 1;
        [Parameter]
        public double Opacity { get; set; } = 1;
        [Parameter]
        public string Fill { get; set; } = "none";
        [Parameter]
        public string Visibility { get; set; } = string.Empty;
        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;
    }
}