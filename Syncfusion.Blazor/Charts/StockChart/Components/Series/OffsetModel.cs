using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Set and gets the stockchart series marker offset position.
    /// </summary>
    public class OffsetModel : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartSeriesMarker Parent { get; set; }

        /// <summary>
        /// x value of the marker position.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// y value of the marker position.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Offset = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}