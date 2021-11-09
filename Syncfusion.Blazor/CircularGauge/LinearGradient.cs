using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the properties to render a linear gradient in the circular gauge.
    /// If both linear and radial gradient is set, then the linear gradient will be rendered.
    /// </summary>
    public partial class LinearGradient
    {
        private string endValue;
        private string startValue;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeRange RangeParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of pointer.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugePointer PointerParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeCap CapParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of needle tail.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeNeedleTail TailParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            RangeParent = null;
            PointerParent = null;
            TailParent = null;
            CapParent = null;
            ChildContent = null;
            ColorStop = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (RangeParent != null)
            {
                RangeParent.UpdateChildProperties("LinearGradient", this);
            }

            if (PointerParent != null)
            {
                PointerParent.UpdateChildProperties("LinearGradient", this);
            }

            if (TailParent != null)
            {
                TailParent.UpdateChildProperties("LinearGradient", this);
            }

            if (CapParent != null)
            {
                CapParent.UpdateChildProperties("LinearGradient", this);
            }
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            endValue = NotifyPropertyChanges(nameof(EndValue), EndValue, endValue);
            startValue = NotifyPropertyChanges(nameof(StartValue), StartValue, startValue);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
