using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the minor grid lines of the axis.
    /// </summary>
    public partial class SmithChartMinorGridLines : SfBaseComponent
    {
        private string color;
        private string dashArray;
        private double width;

        /// <summary>
        /// The color of the minor grid line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// The dash array of grid lines.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// The width of the line in pixels.
        /// </summary>
        [Parameter]
        public virtual double Width { get; set; } = 0.7;

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            dashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
        }
    }
}