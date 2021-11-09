using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the location of the legend, relative to the circular gauge.
    /// If x is 20, legend moves by 20 pixels to the right of the gauge. It requires the `Position` to be `Custom` in the legend settings.
    /// </summary>
    public class LegendLocation : SfBaseComponent
    {
        private double x;
        private double y;

        /// <summary>
        /// Gets or sets the X coordinate of the legend in the circular gauge.
        /// </summary>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the legend in the circular gauge.
        /// </summary>
        [Parameter]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the properties of legends.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeLegendSettings Parent { get; set; }

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
            X = x = Y = y = 0;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Location", this);
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
