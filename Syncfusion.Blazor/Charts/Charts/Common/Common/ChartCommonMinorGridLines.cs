using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Options to customize the minor grid lines of the axis.
    /// </summary>
    public partial class ChartCommonMinorGridLines : SfBaseComponent
    {
        /// <summary>
        /// The color of the minor grid line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        private string color { get; set; }

        /// <summary>
        /// The dash array of grid lines.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        private string dashArray { get; set; }

        /// <summary>
        /// The width of the line in pixels.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 0.7;

        private double width { get; set; }

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CA2007
            await base.OnParametersSetAsync();
#pragma warning restore CA2007
            color = Color = NotifyPropertyChanges(nameof(Color), Color, color);
            dashArray = DashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
        }
    }
}