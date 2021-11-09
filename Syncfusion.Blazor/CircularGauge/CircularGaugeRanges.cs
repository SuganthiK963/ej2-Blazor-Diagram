using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the ranges of an axis in circular gauge component.
    /// </summary>
    public partial class CircularGaugeRanges : SfBaseComponent
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
        /// Gets or sets the properties to render ranges.
        /// </summary>
        internal List<CircularGaugeRange> Ranges { get; set; } = new List<CircularGaugeRange>();

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="range">represents the range properties.</param>
        internal void UpdateChildProperty(CircularGaugeRange range)
        {
            Ranges.Add(range);
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Ranges = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Ranges", Ranges);
        }
    }
}
