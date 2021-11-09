using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the options for customizing the color and width of the border.
    /// </summary>
    public partial class SmithChartCommonBorder : SfBaseComponent
    {
        private string color;
        private double width;

        /// <summary>
        /// Gets and sets the color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public virtual string Color { get; set; }

        /// <summary>
        /// Gets and sets the the width of the border in pixels.
        /// </summary>
        [Parameter]
        public virtual double Width { get; set; } = 1;

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
        }
    }
}