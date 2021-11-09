using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgRect
    {
        [Parameter]
        public override string Id { get; set; }
        [Parameter]
        public double Height { get; set; } = 450;
        [Parameter]
        public double Width { get; set; } = 100;
        [Parameter]
        public string Stroke { get; set; } = "transparent";
        [Parameter]
        public string Fill { get; set; } = "transparent";
        [Parameter]
        public string Filter { get; set; }
        [Parameter]
        public string DashArray { get; set; }
        [Parameter]
        public string Transform { get; set; }
        [Parameter]
        public double StrokeWidth { get; set; } = 1;
        [Parameter]
        public double X { get; set; }
        [Parameter]
        public double Y { get; set; }
        [Parameter]
        public double Rx { get; set; }
        [Parameter]
        public double Ry { get; set; }
        [Parameter]
        public double Opacity { get; set; } = 1;
        [Parameter]
        public string Style { get; set; }
        [Parameter]
        public string Visibility { get; set; } = "visible";

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal void ChangeSize(Rect rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
            InvokeAsync(StateHasChanged);
        }

        public void ChangeFill(string color)
        {
            Fill = color;
            StateHasChanged();
        }
        public void ChangeX(double x)
        {
            X = x;
            StateHasChanged();
        }
        public void ChangeStyle(string style)
        {
            Style = style;
            StateHasChanged();
        }
        public void ChangeWidth(double width)
        {
            Width = width;
            StateHasChanged();
        }
    }
}