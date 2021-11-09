using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Options to customize the minor tick lines of the axis.
    /// </summary>
    public partial class ChartCommonMinorTickLines : SfBaseComponent
    {
        /// <summary>
        /// The color of the minor tick line that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        private string color { get; set; }

        /// <summary>
        /// The height of the ticks in pixels.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 5;

        private double height { get; set; }

        /// <summary>
        /// The width of the tick line in pixels.
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
            height = Height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
        }
    }
}