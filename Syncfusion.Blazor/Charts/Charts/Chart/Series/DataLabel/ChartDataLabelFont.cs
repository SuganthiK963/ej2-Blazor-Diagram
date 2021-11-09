using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the text style of datalabel.
    /// </summary>
    public partial class ChartDataLabelFont : ChartDefaultFont
    {
        private ChartDataLabel datalabel;

        /// <summary>
        /// Defines the size of the datalabel text.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "11px";

        /// <summary>
        /// Defind the color of the datalabel text.
        /// </summary>
        [Parameter]
        public override string Color { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            datalabel = (ChartDataLabel)Tracker;
            datalabel.UpdateDatalabelProperties("Font", this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            datalabel.UpdateDatalabelProperties("Font", this);
        }

        internal ChartDefaultFont GetChartDefaultFont()
        {
            return new ChartDefaultFont
            {
                Color = Color,
                FontFamily = FontFamily,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Opacity = Opacity,
                Size = Size,
                TextAlignment = TextAlignment,
                TextOverflow = TextOverflow
            };
        }

        internal override void ComponentDispose()
        {
            base.ComponentDispose();
            datalabel = null;
            ChildContent = null;
        }
    }
}