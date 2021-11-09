using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the properties of a range of an axis in circular gauge component.
    /// </summary>
    public partial class CircularGaugeRange : SfBaseComponent
    {
        private string color;
        private double end;
        private string endWidth;
        private string legendText;
        private string offset;
        private double opacity;
        private PointerRangePosition position;
        private string radius;
        private double roundedCornerRadius;
        private double start;
        private string startWidth;
        private int rangeIndex;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the color of the ranges in circular gauge component.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the end value of the range in circular gauge component.
        /// </summary>
        [Parameter]
        public double End { get; set; }

        /// <summary>
        ///   Gets or sets the width for the end of the range in the circular gauge component.
        /// </summary>
        [Parameter]
        public string EndWidth { get; set; } = "10";

        /// <summary>
        /// Gets or sets the text for the legend in the circular gauge component.
        /// </summary>
        [Parameter]
        public string LegendText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the offset value of range in circular gauge component.
        /// </summary>
        [Parameter]
        public string Offset { get; set; } = "0";

        /// <summary>
        /// Gets or sets the opacity for the ranges in circular gauge component.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the position of the range and pointer for an axis in circular gauge component.
        /// </summary>
        [Parameter]
        public PointerRangePosition Position { get; set; } = PointerRangePosition.Auto;

        /// <summary>
        /// Gets or sets the radius of the range for circular gauge component.
        /// </summary>
        [Parameter]
        public string Radius { get; set; }

        /// <summary>
        /// Gets or sets the corner radius for ranges in circular gauge component.
        /// </summary>
        [Parameter]
        public double RoundedCornerRadius { get; set; }

        /// <summary>
        /// Gets or sets the start value of the range in circular gauge component.
        /// </summary>
        [Parameter]
        public double Start { get; set; }

        /// <summary>
        ///  Gets or sets the width for the start of the range in the circular gauge component.
        /// </summary>
        [Parameter]
        public string StartWidth { get; set; } = "10";

        /// <summary>
        /// Gets or sets the properties of ranges.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeRanges Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the linear gradient to the ranges.
        /// </summary>
        internal LinearGradient LinearGradient { get; set; }

        /// <summary>
        /// Gets or sets the radial gradient to the ranges.
        /// </summary>
        internal RadialGradient RadialGradient { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            if (key == "LinearGradient")
            {
                LinearGradient = (LinearGradient)keyValue;
            }
            else if (key == "RadialGradient")
            {
                RadialGradient = (RadialGradient)keyValue;
            }
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            LinearGradient = null;
            RadialGradient = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            rangeIndex = Parent.Ranges.Count - 1;
            Parent.Parent.AxisValues.RangeStart.Add(Start);
            Parent.Parent.AxisValues.RangeEnd.Add(End);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            end = NotifyPropertyChanges(nameof(End), End, end);
            endWidth = NotifyPropertyChanges(nameof(EndWidth), EndWidth, endWidth);
            legendText = NotifyPropertyChanges(nameof(LegendText), LegendText, legendText);
            offset = NotifyPropertyChanges(nameof(Offset), Offset, offset);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);
            roundedCornerRadius = NotifyPropertyChanges(nameof(RoundedCornerRadius), RoundedCornerRadius, roundedCornerRadius);
            start = NotifyPropertyChanges(nameof(Start), Start, start);
            startWidth = NotifyPropertyChanges(nameof(StartWidth), StartWidth, startWidth);

            if (PropertyChanges.Count > 0)
            {
                if (PropertyChanges.ContainsKey("Start"))
                {
                    Parent.Parent.AxisValues.RangeStart[rangeIndex] = Start;
                }

                if (PropertyChanges.ContainsKey("End"))
                {
                    Parent.Parent.AxisValues.RangeEnd[rangeIndex] = End;
                }

                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
