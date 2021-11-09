using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.BulletChart;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the legend of the bullet chart component.
    /// </summary>
    public partial class BulletChartLegendSettings : SfBaseComponent
    {
        private Alignment alignment;
        private string background;
        private double opacity;
        private double padding;
        private LegendPosition position;
        private double shapeHeight;
        private double shapePadding;
        private double shapeWidth;
        private bool visible;
        private string width;
        private string height;

        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        internal BulletChartLegendBorder Border { get; set; }

        internal BulletChartLegendTextStyle TextStyle { get; set; }

        internal BulletChartLegendLocation Location { get; set; }

        internal BulletChartLegendMargin Margin { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the legend in bullet chart can be aligned as follows:
        /// Near: Aligns the legend to the left of the bullet chart.
        /// Center: Aligns the legend to the center of the bullet chart.
        /// Far: Aligns the legend to the right of the bullet chart.
        /// </summary>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Sets and gets the background color of the bullet chart legend that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; } = "transparent";

        /// <summary>
        /// Sets and gets the opacity of the bullet chart legend.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets option to customize the padding between legend items.
        /// </summary>
        [Parameter]
        public double Padding { get; set; } = 8;

        /// <summary>
        /// Position of the legend in the bullet chart are,
        /// Auto: Places the legend based on area type.
        /// Top: Displays the legend at the top of the bullet chart.
        /// Left: Displays the legend at the left of the bullet chart.
        /// Bottom: Displays the legend at the bottom of the bullet chart.
        /// Right: Displays the legend at the right of the bullet chart.
        /// Custom: Displays the legend  based on the given x and y values.
        /// </summary>
        [Parameter]
        public LegendPosition Position { get; set; } = LegendPosition.Auto;

        /// <summary>
        /// Sets and gets the shape height of the bullet chart legend.
        /// </summary>
        [Parameter]
        public double ShapeHeight { get; set; } = 10;

        /// <summary>
        /// Sets and gets the padding between the bullet chart legend shape and text.
        /// </summary>
        [Parameter]
        public double ShapePadding { get; set; } = 5;

        /// <summary>
        /// Sets and gets the shape width of the bullet chart legend.
        /// </summary>
        [Parameter]
        public double ShapeWidth { get; set; } = 10;

        /// <summary>
        /// Sets and gets the tabIndex value for the bullet chart legend.
        /// </summary>
        [Parameter]
        public double TabIndex { get; set; } = 3;

        /// <summary>
        /// Sets and gets the visible of the legend.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Sets and gets the width of the legend.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = string.Empty;

        /// <summary>
        /// Sets and gets the height of the legend.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = string.Empty;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.LegendSettings = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            alignment = NotifyPropertyChanges(nameof(Alignment), Alignment, alignment);
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            shapeHeight = NotifyPropertyChanges(nameof(ShapeHeight), ShapeHeight, shapeHeight);
            shapeWidth = NotifyPropertyChanges(nameof(ShapeWidth), ShapeWidth, shapeWidth);
            shapePadding = NotifyPropertyChanges(nameof(ShapePadding), ShapePadding, shapePadding);
            padding = NotifyPropertyChanges(nameof(Padding), Padding, padding);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(BulletChartLegendSettings));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            TextStyle = null;
            Location = null;
            Margin = null;
        }
    }
}