using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgImage
    {
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public double X { get; set; }
        [Parameter]
        public double Y { get; set; }
        [Parameter]
        public double Width { get; set; }
        [Parameter]
        public double Height { get; set; }
        [Parameter]
        public string Href { get; set; }
        [Parameter]
        public string Visibility { get; set; }

        [Parameter]
        public string PreserveAspectRatio { get; set; }
        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;
    }
}