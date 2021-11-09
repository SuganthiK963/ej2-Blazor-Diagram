using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the font style.
    /// </summary>
    public class FontSettings : SfBaseComponent
    {
        private string color;
        private double opacity;
        private string size;

        /// <summary>
        ///  Sets and gets the color for the text.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#000000";

        /// <summary>
        ///  Sets and gets the font family for the text.
        /// </summary>
        [Parameter]
        public string FontFamily { get; set; } = "Segoe UI";

        /// <summary>
        ///  Sets and gets the font style for the text.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary>
        ///  Sets and gets the font weight for the text.
        /// </summary>
        [Parameter]
        public string FontWeight { get; set; } = "Normal";

        /// <summary>
        ///  Sets and gets the opacity for the text.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        ///  Sets and gets the font size for the text.
        /// </summary>
        [Parameter]
        public string Size { get; set; } = "13px";

        internal string GetFontKey()
        {
            return FontWeight + '_' + FontStyle + '_' + FontFamily;
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