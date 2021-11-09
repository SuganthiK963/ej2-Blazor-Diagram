using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the location.
    /// </summary>
    public partial class ChartCommonLocation : SfBaseComponent
    {
        /// <summary>
        ///  Sets and gets the x coordinate of the legend in pixels.
        /// </summary>
        [Parameter]
        public virtual double X { get; set; }

        private double x { get; set; }

        /// <summary>
        ///  Sets and gets the y coordinate of the legend in pixels.
        /// </summary>
        [Parameter]
        public virtual double Y { get; set; }

        private double y { get; set; }

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CA2007
            await base.OnParametersSetAsync();
#pragma warning restore CA2007
            X = x = NotifyPropertyChanges(nameof(X), X, x);
            Y = y = NotifyPropertyChanges(nameof(Y), Y, y);
        }
    }
}