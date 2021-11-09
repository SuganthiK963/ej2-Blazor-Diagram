using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgPath
    {
        [Parameter]
        public override string Id { get; set; }
        [Parameter]
        public string Direction { get; set; }
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
        public string Transform { get; set; } = null;
        [Parameter]
        public string StrokeMiterLimit { get; set; }
        [Parameter]
        public string ClipPath { get; set; } = string.Empty;
        [Parameter]
        public string Style { get; set; } = string.Empty;
        [Parameter]
        public string AccessibilityText { get; set; } = string.Empty;
        [Parameter]
        public string Visibility { get; set; } = string.Empty;

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal void ChangePathAttributes(string direction, string fill, string stroke, double opacity = 1, string transform = "translate(0,0)")
        {
            Stroke = stroke;
            Direction = direction;
            Fill = fill;
            Opacity = opacity;
            if (!string.IsNullOrEmpty(transform))
            {
                Transform = transform;
            }
            StateHasChanged();
        }
        
        /// <exclude />
        public void ChangeDirection(string direction)
        {
            Direction = direction;
            InvokeAsync(StateHasChanged);
        }

        internal void ChangeOpacity(double opacity)
        {
            Opacity = opacity;
            InvokeAsync(StateHasChanged);
        }
    }
}