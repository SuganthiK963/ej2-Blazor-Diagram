using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DataVizCommon
{
    public partial class SvgPolygon
    {
        [Parameter]
        public override string Id { get; set; }
        [Parameter]
        public string points { get; set; }
        [Parameter]
        public string Fill { get; set; }
    }
}