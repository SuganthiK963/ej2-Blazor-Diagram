using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the text style of the chart component.
    /// </summary>
    public partial class ChartCommonFont : SfBaseComponent
    {
        /// <summary>
        /// Sets and gets the color for the text.
        /// </summary>
        [Parameter]
        public virtual string Color { get; set; } = null;

        private string color { get; set; }

        /// <summary>
        /// Sets and gets the font family for the text.
        /// </summary>
        [Parameter]
        public virtual string FontFamily { get; set; } = "Segoe UI";

        private string fontFamily { get; set; }

        /// <summary>
        /// Sets and gets the font style for the text.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        private string fontStyle { get; set; }

        /// <summary>
        /// Sets and gets the font weight for the text.
        /// </summary>
        [Parameter]
        public virtual string FontWeight { get; set; } = "Normal";

        private string fontWeight { get; set; }

        /// <summary>
        /// Sets and gets the opacity for the text.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        private double opacity { get; set; }

        /// <summary>
        /// Sets and gets the size for the text.
        /// </summary>
        [Parameter]
        public virtual string Size { get; set; } = "16px";

        private string size { get; set; }

        /// <summary>
        /// Sets and gets the text alignment.
        /// </summary>
        [Parameter]
        public Alignment TextAlignment { get; set; } = Alignment.Center;

        private Alignment textAlignment { get; set; }

        /// <summary>
        /// Sets and gets the chart text overflow.
        /// </summary>
        [Parameter]
        public TextOverflow TextOverflow { get; set; } = TextOverflow.Trim;

        private TextOverflow textOverflow { get; set; }

        internal string GetFontKey()
        {
            return FontWeight + Constants.UNDERSCORE + FontStyle + Constants.UNDERSCORE + FontFamily;
        }

        internal FontOptions GetFontOptions()
        {
            return new FontOptions { Color = Color, Size = Size, FontFamily = FontFamily, FontWeight = FontWeight, FontStyle = FontStyle };
        }

        internal void SetSize(string size)
        {
            Size = size;
        }

        protected override async Task OnParametersSetAsync()
        {
#pragma warning disable CA2007
            await base.OnParametersSetAsync();
#pragma warning restore CA2007
            color = Color = NotifyPropertyChanges(nameof(Color), Color, color);
            fontFamily = FontFamily = NotifyPropertyChanges(nameof(FontFamily), FontFamily, fontFamily);
            fontStyle = FontStyle = NotifyPropertyChanges(nameof(FontStyle), FontStyle, fontStyle);
            fontWeight = FontWeight = NotifyPropertyChanges(nameof(FontWeight), FontWeight, fontWeight);
            opacity = Opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            size = Size = NotifyPropertyChanges(nameof(Size), Size, size);
            textAlignment = TextAlignment = NotifyPropertyChanges(nameof(TextAlignment), TextAlignment, textAlignment);
            textOverflow = TextOverflow = NotifyPropertyChanges(nameof(TextOverflow), TextOverflow, textOverflow);
        }
    }
}