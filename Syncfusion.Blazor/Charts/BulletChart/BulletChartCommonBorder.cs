using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the color and width of the buller chart component border.
    /// </summary>
    public partial class BulletChartCommonBorder : SfBaseComponent
    {
        /// <summary>
        /// Sets and gets the color of the border. This property accepts the value in hex code and rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Sets and gets the width of the border in the bullet chart component.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;
    }
}