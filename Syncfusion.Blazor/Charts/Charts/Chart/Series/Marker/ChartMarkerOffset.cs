using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartMarkerOffset : ChartDefaultLocation
    {
        [CascadingParameter]
        private ChartMarker marker { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            marker = (ChartMarker)Tracker;
            marker.UpdateMarkerProperties("Offset", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            marker.UpdateMarkerProperties("Offset", this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Chart = null;
            marker = null;
            ChildContent = null;
        }

        internal void SetOffsetValues(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
