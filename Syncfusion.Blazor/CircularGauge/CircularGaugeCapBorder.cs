using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the border of the pointer cap in the circular gauge component.
    /// </summary>
    public class CircularGaugeCapBorder : CircularGaugeBorderSettings
    {
        private string color;
        private double width;

        /// <summary>
        ///  Gets or sets the properties of the cap.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeCap DynamicParent { get; set; }

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
            DynamicParent.UpdateChildProperties("Border", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            width = NotifyPropertyChanges(nameof(Width), Width, width);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
