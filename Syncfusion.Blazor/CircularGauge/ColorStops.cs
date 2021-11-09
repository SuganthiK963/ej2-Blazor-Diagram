using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Defines the color range properties for the gradient.
    /// </summary>
    public partial class ColorStops
    {
        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of the color stop.
        /// </summary>
        internal List<ColorStop> ColorStop { get; set; } = new List<ColorStop>();

        /// <summary>
        /// Gets or sets the properties of linear gradient.
        /// </summary>
        [CascadingParameter]
        internal LinearGradient LinearParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of radial gradient.
        /// </summary>
        [CascadingParameter]
        internal RadialGradient RadialParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            LinearParent = null;
            RadialParent = null;
            BaseParent = null;
            ChildContent = null;
            ColorStop = null;
        }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="colorStop">color stop values.</param>
        internal void UpdateChildProperty(ColorStop colorStop)
        {
            ColorStop.Add(colorStop);
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (LinearParent != null)
            {
                LinearParent.UpdateChildProperties(ColorStop);
            }

            if (RadialParent != null)
            {
                RadialParent.UpdateChildProperties("ColorStop", ColorStop);
            }
        }
    }
}
