using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the tick line settings of an axis in circular gauge component.
    /// </summary>
    public partial class CircularGaugeTickSettings : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the color of the tick line. This property accepts value in hex code, rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the dash-array for the ticks in circular gauge component.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = "0";

        /// <summary>
        /// Gets or sets the height of the ticks in circular gauge component.
        /// </summary>
        [Parameter]
        public virtual double Height { get; set; }

        /// <summary>
        /// Gets or sets the interval between the tick lines in circular gauge component.
        /// </summary>
        [Parameter]
        public double Interval { get; set; }

        /// <summary>
        /// Gets or sets the distance of the ticks from axis in circular gauge component.
        /// </summary>
        [Parameter]
        public double Offset { get; set; }

        /// <summary>
        /// Gets or sets the position of the ticks in circular gauge component.
        /// </summary>
        [Parameter]
        public Position Position { get; set; } = Position.Inside;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the tick lines to take the range color.
        /// </summary>
        [Parameter]
        public bool UseRangeColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the ticks in circular gauge component.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 2;

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal object Parent { get; set; }

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
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
