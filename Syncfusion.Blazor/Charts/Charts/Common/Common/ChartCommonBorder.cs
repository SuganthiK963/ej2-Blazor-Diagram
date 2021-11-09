using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the color and width of the border.
    /// </summary>
    public partial class ChartCommonBorder : SfBaseComponent
    {
        /// <summary>
        /// Sets and gets the color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public virtual string Color { get; set; }

        private string color { get; set; }

        /// <summary>
        /// Sets and gets the the width of the border in pixels.
        /// </summary>
        [Parameter]
        public virtual double Width { get; set; } = 1;

        private double width { get; set; } = 1;

        internal static ChartCommonBorder CreateBorderInstance(string color, double width)
        {
            return new ChartCommonBorder() { Color = color, Width = width };
        }

        internal void SetBorderValue(string color, double width)
        {
            Color = color;
            Width = width;
        }

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CA2007
            await base.OnParametersSetAsync();
#pragma warning restore CA2007
            color = Color = NotifyPropertyChanges(nameof(Color), Color, color);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
        }
    }
}