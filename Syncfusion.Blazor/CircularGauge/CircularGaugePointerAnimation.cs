using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the animation of pointers while rendering the axis in circular gauge.
    /// </summary>
    public class CircularGaugePointerAnimation : SfBaseComponent
    {
        private bool enable;

        private double duration;

        /// <summary>
        /// Gets or sets the duration of animation in milliseconds in circular gauge component.
        /// </summary>
        [Parameter]
        public double Duration { get; set; } = 1000;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the pointer animation during initial rendering in circular gauge component.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Gets or sets the properties of pointers.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugePointer Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Animation", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            duration = NotifyPropertyChanges(nameof(Duration), Duration, duration);
            enable = NotifyPropertyChanges(nameof(Enable), Enable, enable);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
