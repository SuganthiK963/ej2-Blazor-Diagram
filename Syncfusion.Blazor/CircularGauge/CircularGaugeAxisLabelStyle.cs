using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the style of the axis label in circular gauge component.
    /// </summary>
    public partial class CircularGaugeAxisLabelStyle : SfBaseComponent
    {
        private bool autoAngle;
        private string format;
        private HiddenLabel hiddenLabel;
        private double offset;
        private Position position;
        private bool shouldMaintainPadding;
        private bool useRangeColor;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the labels get rotated along the axis in the circular gauge component.
        /// </summary>
        [Parameter]
        public bool AutoAngle { get; set; }

        /// <summary>
        /// Gets or sets the format for the axis label. This property accepts any global string format like 'C', 'n1', 'P' etc.
        /// Also accepts placeholder like '{value}°C' in which value represent the axis label e.g. 20°C.
        /// </summary>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label of an axis, which gets hidden when an axis makes a complete circle.
        /// </summary>
        [Parameter]
        public HiddenLabel HiddenLabel { get; set; } = HiddenLabel.None;

        /// <summary>
        /// Gets or sets the value to place the labels from the axis in circular gauge.
        /// </summary>
        [Parameter]
        public double Offset { get; set; }

        /// <summary>
        /// Gets or sets the position type to place the labels in the axis in the circular gauge component.
        /// </summary>
        [Parameter]
        public Position Position { get; set; } = Position.Inside;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the default padding value of axis labels in circular gauge component.
        /// </summary>
        [Parameter]
        public bool ShouldMaintainPadding { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to use the range color in the axis labels.
        /// </summary>
        [Parameter]
        public bool UseRangeColor { get; set; }

        /// <summary>
        /// Gets or sets the properties of the circular gauge axis.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAxis Parent { get; set; }

        /// <summary>
        ///  Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the font styles for the labels.
        /// </summary>
        internal CircularGaugeAxisLabelFont Font { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="font">represent the font properties.</param>
        internal void UpdateChildProperties(CircularGaugeAxisLabelFont font)
        {
            Font = font;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            Font = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("LabelStyle", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            autoAngle = NotifyPropertyChanges(nameof(AutoAngle), AutoAngle, autoAngle);
            format = NotifyPropertyChanges(nameof(Format), Format, format);
            hiddenLabel = NotifyPropertyChanges(nameof(HiddenLabel), HiddenLabel, hiddenLabel);
            offset = NotifyPropertyChanges(nameof(Offset), Offset, offset);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            shouldMaintainPadding = NotifyPropertyChanges(nameof(ShouldMaintainPadding), ShouldMaintainPadding, shouldMaintainPadding);
            useRangeColor = NotifyPropertyChanges(nameof(UseRangeColor), UseRangeColor, useRangeColor);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
