using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the properties to render a radial gradient in the circular gauge.
    /// </summary>
    public partial class CircularGaugeRadialGradient : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the radius of the radial gradient in percentage.
        /// </summary>
        [Parameter]
        public string Radius { get; set; } = "0%";

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal object Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of ciruclar gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of the color stop.
        /// </summary>
        internal List<ColorStop> ColorStop { get; set; }

        /// <summary>
        /// Gets or sets the inner circle of the radial gradient.
        /// </summary>
        internal InnerPosition InnerPosition { get; set; }

        /// <summary>
        /// Gets or sets the outer circle of the radial gradient.
        /// </summary>
        internal OuterPosition OuterPosition { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            if (key == "ColorStop")
            {
                ColorStop = (List<ColorStop>)keyValue;
            }
            else if (key == "OuterPosition")
            {
                OuterPosition = (OuterPosition)keyValue;
            }
            else if (key == "InnerPosition")
            {
                InnerPosition = (InnerPosition)keyValue;
            }
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            BaseParent = null;
            Parent = null;
            ColorStop = null;
            OuterPosition = null;
            InnerPosition = null;
        }
    }
}
