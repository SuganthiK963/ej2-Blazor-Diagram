using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the options to customize the legend margin.
    /// </summary>
    public class CircularGaugeLegendMargin : CircularGaugeMarginSettings
    {
        private double bottom;
        private double left;
        private double right;
        private double top;

        /// <summary>
        /// Gets or sets the properties of legends.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeLegendSettings DynamicParent { get; set; }

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
            DynamicParent.UpdateChildProperties("Margin", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            bottom = NotifyPropertyChanges(nameof(Bottom), Bottom, bottom);
            left = NotifyPropertyChanges(nameof(Left), Left, left);
            right = NotifyPropertyChanges(nameof(Right), Right, right);
            top = NotifyPropertyChanges(nameof(Top), Top, top);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
