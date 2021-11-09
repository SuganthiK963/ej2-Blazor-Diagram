using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Defines the color range properties for the gradient.
    /// </summary>
    public class ColorStop : SfBaseComponent
    {
        private string color;
        private string offset;
        private double opacity;
        private string style;

        /// <summary>
        /// Gets or sets the color to be used in the gradient.
        /// </summary>
        [Parameter]
        public string Color { get; set; } = "#000000";

        /// <summary>
        ///  Gets or sets the gradient color begin and end in percentage.
        /// </summary>
        [Parameter]
        public string Offset { get; set; } = "0%";

        /// <summary>
        ///  Gets or sets the opacity to be used in the gradient.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the style of the color stop in the gradient element.
        /// </summary>
        [Parameter]
        public string Style { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal ColorStops Parent { get; set; }

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
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
            Parent.UpdateChildProperty(this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            offset = NotifyPropertyChanges(nameof(Offset), Offset, offset);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            style = NotifyPropertyChanges(nameof(Style), Style, style);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
