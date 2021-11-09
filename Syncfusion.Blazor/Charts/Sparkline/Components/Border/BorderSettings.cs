using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the color and width of the border.
    /// </summary>
    public class BorderSettings : SfBaseComponent
    {
        private double width;
        private string color;

        /// <summary>
        /// Sets and gets the color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the width of the border.
        /// </summary>
        [Parameter]
        public double Width { get; set; }

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