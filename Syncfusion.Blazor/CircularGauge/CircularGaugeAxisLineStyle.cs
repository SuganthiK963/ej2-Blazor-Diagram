using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the style of the line in axis of circular gauge component.
    /// </summary>
    public class CircularGaugeAxisLineStyle : SfBaseComponent
    {
        private string color;
        private string dashArray;
        private double width;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the color of the axis line in the circular gauge component. This property accepts the value in hex code,
        /// rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the dash-array of the axis line in circular gauge component.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the width of the line in circular gauge component.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 2;

        /// <summary>
        ///  Gets or sets the properties of the circular gauge axis.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAxis Parent { get; set; }

        /// <summary>
        ///  Gets or sets the properties of the circular gauge.
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
            ChildContent = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("LineStyle", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            dashArray = NotifyPropertyChanges(nameof(DashArray), DashArray, dashArray);
            width = NotifyPropertyChanges(nameof(Width), Width, width);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
