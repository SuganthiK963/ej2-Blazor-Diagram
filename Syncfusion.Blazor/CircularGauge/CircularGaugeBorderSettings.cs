using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the options for customizing the color and width of the gauge border.
    /// </summary>
    public partial class CircularGaugeBorderSettings : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the color of the border in the circular gauge. This property accepts value in hex code, rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the width of the border in circular gauge.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        ///  Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal object Parent { get; set; }

        /// <summary>
        ///  Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            BaseParent = null;
            Parent = null;
        }
    }
}
