using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the properties to render a linear gradient in the circular gauge.
    /// </summary>
    public partial class CircularGaugeLinearGradient : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the end value of the linear gradient.
        /// </summary>
        [Parameter]
        public string EndValue { get; set; } = "100%";

        /// <summary>
        /// Gets or sets the start value of the linear gradient.
        /// </summary>
        [Parameter]
        public string StartValue { get; set; } = "0%";

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
        /// Gets or sets the color stop color for the linear gradient.
        /// </summary>
        internal List<ColorStop> ColorStop { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="colorStop">represents the color stop properties.</param>
        internal void UpdateChildProperties(List<ColorStop> colorStop)
        {
            ColorStop = colorStop;
        }
    }
}
