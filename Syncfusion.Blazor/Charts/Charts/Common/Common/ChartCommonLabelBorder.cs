using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Options to customize the label border of the axis.
    /// </summary>
    public partial class ChartCommonLabelBorder : SfBaseComponent
    {
        /// <summary>
        /// The color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        private string color { get; set; }

        /// <summary>
        /// Border type for labels
        /// Rectangle
        /// Without Top Border
        /// Without Top and BottomBorder
        /// Without Border
        /// Brace
        /// CurlyBrace.
        /// </summary>
        [Parameter]
        public BorderType Type { get; set; }

        private BorderType type { get; set; }

        /// <summary>
        /// The width of the border in pixels.
        /// </summary>
        [Parameter]
        public virtual double Width { get; set; } = 1;

        private double width { get; set; }

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CA2007
            await base.OnParametersSetAsync();
#pragma warning restore CA2007
            color = color = NotifyPropertyChanges(nameof(Color), Color, color);
            type = Type = NotifyPropertyChanges(nameof(Type), Type, type);
            width = Width = NotifyPropertyChanges(nameof(Width), Width, width);
        }
    }
}