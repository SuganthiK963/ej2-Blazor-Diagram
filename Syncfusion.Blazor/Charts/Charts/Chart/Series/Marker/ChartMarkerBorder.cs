using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartMarkerBorder: ChartDefaultBorder
    {
        [CascadingParameter]
        private ChartMarker marker { get; set; }

        /// <summary>
        /// Specifies the width of the marker border.
        /// </summary>
        [Parameter]
        public override double Width { get; set; } = 2;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            marker = (ChartMarker)Tracker;
            marker.UpdateMarkerProperties("Border", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            marker.UpdateMarkerProperties("Border", this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Owner = null;
            marker = null;
            ChildContent = null;
        }

        internal void SetBorderValues(string color, double width)
        {
            Color = color;
            Width = width;
        }
    }
}
