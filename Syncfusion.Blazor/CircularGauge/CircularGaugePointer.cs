using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and Gets the properties of the pointer in an axis of circular gauge component.
    /// </summary>
    public partial class CircularGaugePointer : SfBaseComponent
    {
        private string color;
        private string description;
        private string imageUrl;
        private double markerHeight;
        private GaugeShape markerShape;
        private double markerWidth;
        private double needleEndWidth;
        private double needleStartWidth;
        private string offset;
        private double pointerWidth;
        private PointerRangePosition position;
        private string radius;
        private double roundedCornerRadius;
        private string text;
        private PointerType type;
        private double value;
        private int pointerIndex;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the color of the pointer in an axis.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the information about pointer for assistive technology.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the url for the image that is to be displayed as pointer.
        /// It requires marker shape value to be Image.
        /// </summary>
        [Parameter]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the height of the marker pointer in an axis.
        /// </summary>
        [Parameter]
        public double MarkerHeight { get; set; } = 5;

        /// <summary>
        /// Gets or sets the shape of the marker type pointer in an axis.
        /// </summary>
        [Parameter]
        public GaugeShape MarkerShape { get; set; } = GaugeShape.Circle;

        /// <summary>
        /// Gets or sets the width of the marker pointer in an axis.
        /// </summary>
        [Parameter]
        public double MarkerWidth { get; set; } = 5;

        /// <summary>
        /// Gets or sets the end width of the needle pointer in an axis.
        /// </summary>
        [Parameter]
        public double NeedleEndWidth { get; set; }

        /// <summary>
        /// Gets or sets the start width of the needle pointer in an axis.
        /// </summary>
        [Parameter]

        public double NeedleStartWidth { get; set; }

        /// <summary>
        /// Gets or sets the offset value of pointer from scale.
        /// </summary>
        [Parameter]
        public string Offset { get; set; } = "0";

        /// <summary>
        /// Gets or sets the width of the pointer in axis.
        /// </summary>
        [Parameter]

        public double PointerWidth { get; set; } = 20;

        /// <summary>
        /// Gets or sets the position of pointer for an axis.
        /// </summary>
        [Parameter]
        public PointerRangePosition Position { get; set; } = PointerRangePosition.Auto;

        /// <summary>
        /// Gets or sets the radius of pointer for marker and range type pointer and fix length of pointer for needle pointer.
        /// </summary>
        [Parameter]
        public string Radius { get; set; }

        /// <summary>
        /// Gets or sets the corner radius for pointer in axis.
        /// </summary>
        [Parameter]
        public double RoundedCornerRadius { get; set; }

        /// <summary>
        /// Gets or sets the text in pointer.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of pointer for an axis in Circular gauge component.
        /// </summary>
        [Parameter]
        public PointerType Type { get; set; } = PointerType.Needle;

        /// <summary>
        /// Gets or sets the value of the pointer in circular gauge component.
        /// </summary>
        [Parameter]
        public double Value { get; set; } = 0;

        /// <summary>
        /// Gets or sets the properties of pointers.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugePointers Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauges.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the properties of pointer animation.
        /// </summary>
        internal CircularGaugePointerAnimation Animation { get; set; }

        /// <summary>
        /// Gets or sets the border of the pointer.
        /// </summary>
        internal CircularGaugePointerBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the cap for the pointer.
        /// </summary>
        internal CircularGaugeCap Cap { get; set; }

        /// <summary>
        /// Gets or sets the linear gradient for the pointer.
        /// </summary>
        internal LinearGradient LinearGradient { get; set; }

        /// <summary>
        /// Gets or sets the needle tail for the pointer.
        /// </summary>
        internal CircularGaugeNeedleTail NeedleTail { get; set; }

        /// <summary>
        /// Gets or sets the radial gradient for the pointer.
        /// </summary>
        internal RadialGradient RadialGradient { get; set; }

        /// <summary>
        /// Gets or sets the styles for the marker text.
        /// </summary>
        internal MarkerTextStyle TextStyle { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the key.</param>
        /// <param name="keyValue">represents the key value.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            switch (key)
            {
                case "Animation":
                    Animation = (CircularGaugePointerAnimation)keyValue;
                    break;
                case "Border":
                    Border = (CircularGaugePointerBorder)keyValue;
                    break;
                case "LinearGradient":
                    LinearGradient = (LinearGradient)keyValue;
                    break;
                case "RadialGradient":
                    RadialGradient = (RadialGradient)keyValue;
                    break;
                case "Cap":
                    Cap = (CircularGaugeCap)keyValue;
                    break;
                case "NeedleTail":
                    NeedleTail = (CircularGaugeNeedleTail)keyValue;
                    break;
                case "TextStyle":
                    TextStyle = (MarkerTextStyle)keyValue;
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
            Animation = null;
            Border = null;
            Cap = null;
            LinearGradient = null;
            RadialGradient = null;
            NeedleTail = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            pointerIndex = Parent.Pointers.Count - 1;
            Parent.Parent.AxisValues.PointerValue.Add(Value);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            description = NotifyPropertyChanges(nameof(Description), Description, description);
            imageUrl = NotifyPropertyChanges(nameof(ImageUrl), ImageUrl, imageUrl);
            markerHeight = NotifyPropertyChanges(nameof(MarkerHeight), MarkerHeight, markerHeight);
            markerShape = NotifyPropertyChanges(nameof(MarkerShape), MarkerShape, markerShape);
            markerWidth = NotifyPropertyChanges(nameof(MarkerWidth), MarkerWidth, markerWidth);
            needleEndWidth = NotifyPropertyChanges(nameof(NeedleEndWidth), NeedleEndWidth, needleEndWidth);
            needleStartWidth = NotifyPropertyChanges(nameof(NeedleStartWidth), NeedleStartWidth, needleStartWidth);
            offset = NotifyPropertyChanges(nameof(Offset), Offset, offset);
            pointerWidth = NotifyPropertyChanges(nameof(PointerWidth), PointerWidth, pointerWidth);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);
            roundedCornerRadius = NotifyPropertyChanges(nameof(RoundedCornerRadius), RoundedCornerRadius, roundedCornerRadius);
            text = NotifyPropertyChanges(nameof(Text), Text, text);
            type = NotifyPropertyChanges(nameof(Type), Type, type);
            value = NotifyPropertyChanges(nameof(Value), Value, value);

            if (PropertyChanges.Count > 0)
            {
                if (PropertyChanges.ContainsKey("Value"))
                {
                    Parent.Parent.AxisValues.PointerValue[pointerIndex] = Value;
                }

                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
