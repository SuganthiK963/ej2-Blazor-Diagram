using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Legend is used to help readers understand the plotted data.
    /// The chart legend shows information about the datasets that are appearing on the chart.
    /// </summary>
    public partial class AccumulationChartLegendSettings
    {
        [CascadingParameter]
        private IAccumulationChart chart { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Legend in chart can be aligned as follows:
        ///  Near: Aligns the legend to the left of the chart.
        ///  Center: Aligns the legend to the center of the chart.
        ///  Far: Aligns the legend to the right of the chart.
        /// </summary>
        [Parameter]
        public Alignment Alignment { get; set; } = Alignment.Center;

        private Alignment alignment { get; set; }

        /// <summary>
        /// The background color of the legend that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; } = Constants.TRANSPARENT;

        private string background { get; set; }

        /// <summary>
        /// Options to customize the border of the legend.
        /// </summary>
        [Parameter]
        public AccumulationChartLegendBorder Border { get; set; } = new AccumulationChartLegendBorder();

        private AccumulationChartLegendBorder border { get; set; }

        /// <summary>
        /// Description for legends.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The height of the legend in pixels.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = string.Empty;

        private string height { get; set; }

        /// <summary>
        /// Specifies the location of the legend, relative to the chart.
        /// If x is 20, legend moves by 20 pixels to the right of the chart. It requires the `Position` to be `Custom`.
        /// </summary>
        [Parameter]
        public AccumulationChartLocation Location { get; set; } = new AccumulationChartLocation();

        private AccumulationChartLocation location { get; set; }

        /// <summary>
        ///  Options to customize left, right, top and bottom margins of the chart.
        /// </summary>
        [Parameter]
        public AccumulationChartLegendMargin Margin { get; set; } = new AccumulationChartLegendMargin();

        private AccumulationChartLegendMargin margin { get; set; }

        /// <summary>
        /// Opacity of the legend.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        private double opacity { get; set; }

        /// <summary>
        /// Option to customize the padding between legend items.
        /// </summary>
        [Parameter]
        public double Padding { get; set; } = 8;

        private double padding { get; set; }

        /// <summary>
        /// Position of the legend in the chart are,
        /// Auto: Places the legend based on area type.
        /// Top: Displays the legend at the top of the chart.
        /// Left: Displays the legend at the left of the chart.
        /// Bottom: Displays the legend at the bottom of the chart.
        /// Right: Displays the legend at the right of the chart.
        /// Custom: Displays the legend  based on the given x and y value.
        /// </summary>
        [Parameter]
        public LegendPosition Position { get; set; }

        private LegendPosition position { get; set; }

        /// <summary>
        /// Shape height of the legend in pixels.
        /// </summary>
        [Parameter]
        public double ShapeHeight { get; set; } = 10;

        private double shapeHeight { get; set; }

        /// <summary>
        /// Padding between the legend shape and text.
        /// </summary>
        [Parameter]
        public double ShapePadding { get; set; } = 5;

        private double shapePadding { get; set; }

        /// <summary>
        /// Shape width of the legend in pixels.
        /// </summary>
        [Parameter]
        public double ShapeWidth { get; set; } = 10;

        private double shapeWidth { get; set; }

        /// <summary>
        /// TabIndex legendItem for the legend.
        /// </summary>
        [Parameter]
        public double TabIndex { get; set; } = 3;

        /// <summary>
        /// Options to customize the legend text.
        /// </summary>
        [Parameter]
        public AccumulationChartLegendFont TextStyle { get; set; } = new AccumulationChartLegendFont();

        private AccumulationChartLegendFont textStyle { get; set; }

        /// <summary>
        /// If set to true, series' visibility collapses based on the legend visibility.
        /// </summary>
        [Parameter]
        public bool ToggleVisibility { get; set; } = true;

        private bool toggleVisibility { get; set; }

        /// <summary>
        /// If set to true, legend will be visible.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        private bool visible { get; set; }

        /// <summary>
        /// The width of the legend in pixels.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = string.Empty;

        /// <summary>
        /// Enables / Disables the inverse rendering of the legend symbol and text.
        /// </summary>
        [Parameter]
        public bool IsInversed { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            chart.UpdateChildProperties("LegendSettings", this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            alignment = Alignment = NotifyPropertyChanges(nameof(Alignment), Alignment, alignment);
            background = Background = NotifyPropertyChanges(nameof(Background), Background, background);
            border = Border = NotifyPropertyChanges(nameof(Border), Border, border);
            position = Position = NotifyPropertyChanges(nameof(Position), Position, position);
            height = Height = NotifyPropertyChanges(nameof(Height), Height, height);
            location = Location = NotifyPropertyChanges(nameof(Location), Location, location);
            margin = Margin = NotifyPropertyChanges(nameof(Margin), Margin, margin);
            opacity = Opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            padding = Padding = NotifyPropertyChanges(nameof(Padding), Padding, padding);
            shapeHeight = ShapeHeight = NotifyPropertyChanges(nameof(ShapeHeight), ShapeHeight, shapeHeight);
            shapePadding = ShapePadding = NotifyPropertyChanges(nameof(ShapePadding), ShapePadding, shapePadding);
            shapeWidth = ShapeWidth = NotifyPropertyChanges(nameof(ShapeWidth), ShapeWidth, shapeWidth);
            toggleVisibility = ToggleVisibility = NotifyPropertyChanges(nameof(ToggleVisibility), ToggleVisibility, toggleVisibility);
            visible = Visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            if (PropertyChanges.Any() && IsRendered)
            {
                ((SfBaseComponent)chart).PropertyChanges.TryAdd(nameof(IAccumulationChart.LegendSettings), this);
                PropertyChanges.Clear();
                await chart.OnAccumulationChartParametersSet();
            }
        }

        internal async Task LegendPropertyChanged()
        {
            await OnParametersSetAsync();
#pragma warning restore CA2007
        }

        internal void UpdateLegendProperties(string key, object legendItem)
        {
            if (key == nameof(Border))
            {
                Border = border = (AccumulationChartLegendBorder)legendItem;
            }
            else if (key == nameof(TextStyle))
            {
                TextStyle = textStyle = (AccumulationChartLegendFont)legendItem;
            }
            else if (key == nameof(Margin))
            {
                Margin = margin = (AccumulationChartLegendMargin)legendItem;
            }
            else if (key == nameof(Location))
            {
                Location = location = (AccumulationChartLocation)legendItem;
            }
        }

        internal override void ComponentDispose()
        {
            chart = null;
            ChildContent = null;
            Border = null;
            TextStyle = null;
            Margin = null;
            Location = null;
        }
    }
}