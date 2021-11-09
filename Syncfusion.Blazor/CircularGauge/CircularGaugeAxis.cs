using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.Blazor.CircularGauge.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the options for customizing a axis of circular gauge.
    /// </summary>
    public partial class CircularGaugeAxis : SfBaseComponent
    {
        private string background;
        private GaugeDirection direction;
        private double endAngle;
        private bool hideIntersectingLabel;
        private double maximum;
        private double minimum;
        private string radius;
        private double rangeGap;
        private int roundingPlaces;
        private bool showLastLabel;
        private bool startAndEndRangeGap;
        private double startAngle;

        /// <summary>
        /// Gets or sets the start angle of an axis in circular gauge component.
        /// </summary>
        internal double AngleStart { get; set; }

        /// <summary>
        /// Gets or sets the end angle of an axis in circular gauge component.
        /// </summary>
        internal double AngleEnd { get; set; }

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the background color of an axis. This property accepts value in hex code, rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the direction of an axis.
        /// </summary>
        [Parameter]
        public GaugeDirection Direction { get; set; } = GaugeDirection.ClockWise;

        /// <summary>
        /// Gets or sets the end angle of an axis in circular gauge component.
        /// </summary>
        [Parameter]
        public double EndAngle { get; set; } = 160;

        /// <summary>
        /// Gets or sets a value indicating whether or not the intersecting labels to be hidden in axis.
        /// </summary>
        [Parameter]
        public bool HideIntersectingLabel { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of an axis in the circular gauge component.
        /// </summary>
        [Parameter]
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of an axis in the circular gauge component.
        /// </summary>
        [Parameter]
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the radius of an axis in circular gauge.
        /// </summary>
        [Parameter]
        public string Radius { get; set; }

        /// <summary>
        /// Gets or sets the gap between the ranges by specified value in circular gauge component.
        /// </summary>
        [Parameter]
        public double RangeGap { get; set; }

        /// <summary>
        /// Gets or sets the rounding Off value in the label in an axis.
        /// </summary>
        [Parameter]
        public int RoundingPlaces { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to display the last label of axis when it is hidden in a circular gauge component.
        /// </summary>
        [Parameter]
        public bool ShowLastLabel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the start and end gap between the ranges and axis in circular gauge.
        /// </summary>
        [Parameter]
        public bool StartAndEndRangeGap { get; set; }

        /// <summary>
        /// Gets or sets the start angle of an axis in circular gauge component.
        /// </summary>
        [Parameter]
        public double StartAngle { get; set; } = 200;

        /// <summary>
        /// Gets or sets the values of the ranges.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<CircularGaugeRange> Ranges { get; set; }

        /// <summary>
        ///  Gets or sets the properties of the circular gauge axis.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAxes Parent { get; set; }

        /// <summary>
        ///  Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the styles of the labels in the axis.
        /// </summary>
        internal CircularGaugeAxisLabelStyle LabelStyle { get; set; }

        /// <summary>
        /// Gets or sets the styles of the lines in the axis.
        /// </summary>
        internal CircularGaugeAxisLineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the values of the major ticks in the axis.
        /// </summary>
        internal CircularGaugeAxisMajorTicks MajorTicks { get; set; }

        /// <summary>
        /// Gets or sets the values of the minor ticks in the axis.
        /// </summary>
        internal CircularGaugeAxisMinorTicks MinorTicks { get; set; }

        /// <summary>
        /// Gets or sets the values of the annotations.
        /// </summary>
        internal List<CircularGaugeAnnotation> Annotations { get; set; }

        /// <summary>
        /// Gets or sets the values of the pointers.
        /// </summary>
        internal List<CircularGaugePointer> Pointers { get; set; }

        /// <summary>
        /// Gets or sets the properties to render axis.
        /// </summary>
        internal AxisInternal AxisValues { get; set; } = new AxisInternal();

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represent the keys.</param>
        /// <param name="keyValue">represent the key values.</param>
        internal void UpdateChildProperties(string key, object keyValue)
        {
            switch (key)
            {
                case "LineStyle":
                    LineStyle = (CircularGaugeAxisLineStyle)keyValue;
                    break;
                case "MajorTicks":
                    MajorTicks = (CircularGaugeAxisMajorTicks)keyValue;
                    break;
                case "MinorTicks":
                    MinorTicks = (CircularGaugeAxisMinorTicks)keyValue;
                    break;
                case "LabelStyle":
                    LabelStyle = (CircularGaugeAxisLabelStyle)keyValue;
                    break;
                case "Ranges":
                    Ranges = (List<CircularGaugeRange>)keyValue;
                    break;
                case "Pointers":
                    Pointers = (List<CircularGaugePointer>)keyValue;
                    break;
                case "Annotations":
                    Annotations = (List<CircularGaugeAnnotation>)keyValue;
                    break;
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
            Annotations = null;
            LabelStyle = null;
            LineStyle = null;
            MajorTicks = null;
            MinorTicks = null;
            Pointers = null;
            Ranges = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            AxisValues = new AxisInternal()
            {
                PointerValue = new List<double>(), RangeStart = new List<double>(),
                RangeEnd = new List<double>(),
                AnnotationContent = new List<string>(),
            };
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            direction = NotifyPropertyChanges(nameof(Direction), Direction, direction);
            endAngle = NotifyPropertyChanges(nameof(EndAngle), EndAngle, endAngle);
            hideIntersectingLabel = NotifyPropertyChanges(nameof(HideIntersectingLabel), HideIntersectingLabel, hideIntersectingLabel);
            maximum = NotifyPropertyChanges(nameof(Maximum), Maximum, maximum);
            minimum = NotifyPropertyChanges(nameof(Minimum), Minimum, minimum);
            radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);
            rangeGap = NotifyPropertyChanges(nameof(RangeGap), RangeGap, rangeGap);
            roundingPlaces = NotifyPropertyChanges(nameof(RoundingPlaces), RoundingPlaces, roundingPlaces);
            showLastLabel = NotifyPropertyChanges(nameof(ShowLastLabel), ShowLastLabel, showLastLabel);
            startAndEndRangeGap = NotifyPropertyChanges(nameof(StartAndEndRangeGap), StartAndEndRangeGap, startAndEndRangeGap);
            startAngle = NotifyPropertyChanges(nameof(StartAngle), StartAngle, startAngle);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
