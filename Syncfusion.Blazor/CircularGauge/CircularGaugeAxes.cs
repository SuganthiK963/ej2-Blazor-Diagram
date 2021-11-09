using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets the collection of axes to the circular gauge.
    /// </summary>
    public partial class CircularGaugeAxes : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render axis.
        /// </summary>
        internal List<CircularGaugeAxis> Axes { get; set; } = new List<CircularGaugeAxis>();

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="axis">represents the axis values.</param>
        internal void UpdateChildProperty(CircularGaugeAxis axis)
        {
            Axes.Add(axis);
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Axes = null;
        }

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Parent.UpdateChildProperties("Axes", Axes);
        }
    }
}
