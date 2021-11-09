using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the options for customizing the legend of the circular gauge.
    /// </summary>
    public partial class CircularGaugeLegendSettings
    {
        private Alignment alignment;
        private string background;
        private string height;
        private double opacity;
        private double padding;
        private LegendPosition position;
        private GaugeShape shape;
        private double shapeHeight;
        private double shapePadding;
        private double shapeWidth;
        private bool toggleVisibility;
        private bool visible;
        private string width;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the legend in the circular gauge.
        /// </summary>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Gets or sets the background color of the legend in circular gauge.
        /// </summary>
        [Parameter]
        public string Background { get; set; } = "transparent";

        /// <summary>
        /// Gets or sets the height of the legend in the circular gauge.
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the legend.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the options to customize the padding between legend items.
        /// </summary>
        [Parameter]
        public double Padding { get; set; } = 8;

        /// <summary>
        /// Gets or sets the position of the legend in the circular gauge.
        /// </summary>
        [Parameter]
        public LegendPosition Position { get; set; } = LegendPosition.Auto;

        /// <summary>
        /// Gets or sets the shape of the legend in circular gauge.
        /// </summary>
        [Parameter]
        public GaugeShape Shape { get; set; } = GaugeShape.Circle;

        /// <summary>
        /// Gets or sets the height of the legend shape in circular gauge.
        /// </summary>
        [Parameter]
        public double ShapeHeight { get; set; } = 10;

        /// <summary>
        /// Gets or sets the padding for the legend shape in circular gauge.
        /// </summary>
        [Parameter]
        public double ShapePadding { get; set; } = 5;

        /// <summary>
        /// Gets or sets the width of the legend shape in circular gauge.
        /// </summary>
        [Parameter]
        public double ShapeWidth { get; set; } = 10;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the ranges visibility collapses based on the legend visibility.
        /// </summary>
        [Parameter]
        public bool ToggleVisibility { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the visibility of the legend in circular gauge.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the width of the legend in the circular gauge.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets the border of the legend in circular gauge.
        /// </summary>
        internal CircularGaugeLegendBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the location of the legend.
        /// </summary>
        internal LegendLocation Location { get; set; }

        /// <summary>
        /// Gets or sets the margin of the legend.
        /// </summary>
        internal CircularGaugeLegendMargin Margin { get; set; }

        /// <summary>
        /// Gets or sets the border of the legend shape.
        /// </summary>
        internal CircularGaugeLegendShapeBorder ShapeBorder { get; set; }

        /// <summary>
        /// Gets or sets the styles of the legend text.
        /// </summary>
        internal CircularGaugeLegendTextStyle TextStyle { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            switch (key)
            {
                case "Border":
                    Border = (CircularGaugeLegendBorder)keyValue;
                    break;
                case "ShapeBorder":
                    ShapeBorder = (CircularGaugeLegendShapeBorder)keyValue;
                    break;
                case "TextStyle":
                    TextStyle = (CircularGaugeLegendTextStyle)keyValue;
                    break;
                case "Margin":
                    Margin = (CircularGaugeLegendMargin)keyValue;
                    break;
                case "Location":
                    Location = (LegendLocation)keyValue;
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
            Border = null;
            Margin = null;
            ShapeBorder = null;
            TextStyle = null;
            Location = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("LegendSettings", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            alignment = NotifyPropertyChanges(nameof(Alignment), Alignment, alignment);
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            padding = NotifyPropertyChanges(nameof(Padding), Padding, padding);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            shape = NotifyPropertyChanges(nameof(Shape), Shape, shape);
            shapeHeight = NotifyPropertyChanges(nameof(ShapeHeight), ShapeHeight, shapeHeight);
            shapePadding = NotifyPropertyChanges(nameof(ShapePadding), ShapePadding, shapePadding);
            shapeWidth = NotifyPropertyChanges(nameof(ShapeWidth), ShapeWidth, shapeWidth);
            toggleVisibility = NotifyPropertyChanges(nameof(ToggleVisibility), ToggleVisibility, toggleVisibility);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            width = NotifyPropertyChanges(nameof(Width), Width, width);

            if (PropertyChanges.Count > 0)
            {
                if (PropertyChanges.ContainsKey("ToggleVisibility"))
                {
                    await BaseParent.UpdateToggleVisibility();
                }

                if (PropertyChanges.ContainsKey("Height") || PropertyChanges.ContainsKey("Width"))
                {
                    await BaseParent.PropertyChangesHandle();
                }
                else
                {
                    BaseParent.PropertyChangeHandler();
                }

                PropertyChanges.Clear();
            }
        }
    }
}
