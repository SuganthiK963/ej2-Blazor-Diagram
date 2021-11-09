using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgLine
    {
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;
        [Parameter]
        public override string Id { get; set; }
        [Parameter]
        public string Stroke { get; set; } = "transparent";
        [Parameter]
        public double StrokeWidth { get; set; } = 1;
        [Parameter]
        public double X1 { get; set; }
        [Parameter]
        public double Y1 { get; set; }
        [Parameter]
        public double X2 { get; set; }
        [Parameter]
        public double Y2 { get; set; }
        [Parameter]
        public double Opacity { get; set; } = 1;
        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;
    }
}