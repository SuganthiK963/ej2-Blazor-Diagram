using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgPattern
    {
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public double Height { get; set; }
        [Parameter]
        public double Width { get; set; }
        [Parameter]
        public string PatternUnits { get; set; }
        [Parameter]
#pragma warning disable CA2227
        public List<object> ShapeOptions { get; set; }
#pragma warning restore CA2227
    }
}