using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the options for customizing the text style of the chart component.
    /// </summary>
    public partial class SmithChartCommonFont : SfBaseComponent
    {
        private string color;
        private double opacity;
        private string size;

        /// <summary>
        /// Gets and sets the color for the text.
        /// </summary>
        [Parameter]
        public virtual string Color { get; set; }

        /// <summary>
        /// Gets and sets the font family for the text.
        /// </summary>
        [Parameter]
        public virtual string FontFamily { get; set; } = "Segoe UI";

        /// <summary>
        /// Gets and sets the font style for the text.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary>
        /// Gets and sets the font weight for the text.
        /// </summary>
        [Parameter]
        public virtual string FontWeight { get; set; } = "Normal";

        /// <summary>
        /// Gets and sets the opacity for the text.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets and sets the size for the text.
        /// </summary>
        [Parameter]
        public virtual string Size { get; set; } = "16px";

        internal string GetFontKey()
        {
            return FontWeight + '_' + FontStyle + '_' + FontFamily;
        }

        internal FontOptions GetFontOptions()
        {
            return new FontOptions { Color = Color, Size = Size, FontFamily = FontFamily, FontWeight = FontWeight, FontStyle = FontStyle };
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            size = NotifyPropertyChanges(nameof(Size), Size, size);
        }
    }
}