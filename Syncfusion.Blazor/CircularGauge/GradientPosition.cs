using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Defines the circle positions of the radial gradient.
    /// </summary>
    public partial class GradientPosition : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the horizontal position in percentage.
        /// </summary>
        [Parameter]
        public string X { get; set; } = "0%";

        /// <summary>
        /// Gets or sets the vertical position in percentage.
        /// </summary>
        [Parameter]
        public string Y { get; set; } = "0%";

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
