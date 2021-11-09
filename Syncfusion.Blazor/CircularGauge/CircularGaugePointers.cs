using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the pointers of an axis in circular gauge component.
    /// </summary>
    public partial class CircularGaugePointers
    {
        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the properties of axis.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAxis Parent { get; set; }

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        internal List<CircularGaugePointer> Pointers { get; set; } = new List<CircularGaugePointer>();

        /// <summary>
        /// UpdateChildProperty is used to update the child properties.
        /// </summary>
        /// <param name="pointer">represents the pointer properties.</param>
        internal void UpdateChildProperty(CircularGaugePointer pointer)
        {
            Pointers.Add(pointer);
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Pointers = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Pointers", Pointers);
        }
    }
}
