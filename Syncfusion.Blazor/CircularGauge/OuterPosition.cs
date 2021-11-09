using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Defines the outer circle of the radial gradient.
    /// </summary>
    public class OuterPosition : GradientPosition
    {
        private string x;

        private string y;

        /// <summary>
        /// Gets or sets the properties of radial gradients.
        /// </summary>
        [CascadingParameter]
        internal RadialGradient DynamicParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            DynamicParent = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.UpdateChildProperties("OuterPosition", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            x = NotifyPropertyChanges(nameof(X), X, x);
            y = NotifyPropertyChanges(nameof(Y), Y, y);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
