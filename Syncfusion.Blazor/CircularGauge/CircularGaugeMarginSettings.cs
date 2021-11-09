using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the options to customize the left, right, top and bottom margins of the circular gauge.
    /// </summary>
    public partial class CircularGaugeMarginSettings : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the bottom margin value of the gauge.
        /// </summary>
        [Parameter]
        public double Bottom { get; set; } = 10;

        /// <summary>
        /// Gets or sets the left margin value of the gauge.
        /// </summary>
        [Parameter]
        public double Left { get; set; } = 10;

        /// <summary>
        /// Gets or sets the right margin value of the gauge.
        /// </summary>
        [Parameter]
        public double Right { get; set; } = 10;

        /// <summary>
        /// Gets or sets the top margin value of the gauge.
        /// </summary>
        [Parameter]
        public double Top { get; set; } = 10;

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal object Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
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
