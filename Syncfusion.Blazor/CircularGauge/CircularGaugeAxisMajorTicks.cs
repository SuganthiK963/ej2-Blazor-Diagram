using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the major tick lines of an axis in circular gauge component.
    /// </summary>
    public class CircularGaugeAxisMajorTicks : CircularGaugeTickSettings
    {
        private string color;
        private string dashArray;
        private double height;
        private double interval;
        private double offset;
        private Position position;
        private bool useRangeColor;
        private double width;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the height of the major ticks.
        /// </summary>
        public override double Height { get; set; } = 10;

        /// <summary>
        ///  Gets or sets the properties of the circular gauge axis.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAxis DynamicParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            DynamicParent = null;
            ChildContent = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.UpdateChildProperties("MajorTicks", this);
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
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            interval = NotifyPropertyChanges(nameof(Interval), Interval, interval);
            offset = NotifyPropertyChanges(nameof(Offset), Offset, offset);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            useRangeColor = NotifyPropertyChanges(nameof(UseRangeColor), UseRangeColor, useRangeColor);
            width = NotifyPropertyChanges(nameof(Width), Width, width);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
