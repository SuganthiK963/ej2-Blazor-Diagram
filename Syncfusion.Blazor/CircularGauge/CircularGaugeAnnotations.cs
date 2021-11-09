using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Specifies the annotations that are to be added to the axis of the circular guage.
    /// </summary>
    public partial class CircularGaugeAnnotations : SfBaseComponent
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
        internal CircularGaugeAxis Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render annotaion.
        /// </summary>
        internal List<CircularGaugeAnnotation> Annotations { get; set; } = new List<CircularGaugeAnnotation>();

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="annotation">represent the annotation values.</param>
        internal void UpdateChildProperty(CircularGaugeAnnotation annotation)
        {
            Annotations.Add(annotation);
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Annotations = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Annotations", Annotations);
        }
    }
}
