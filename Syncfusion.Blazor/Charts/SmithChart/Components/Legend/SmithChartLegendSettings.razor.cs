using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the legend in the chart.
    /// </summary>
    public partial class SmithChartLegendSettings
    {
        private SmithChartAlignment alignment;
        private int columnCount;
        private string height;
        private double itemPadding;
        private LegendPosition position;
        private int rowCount;
        private Shape shape;
        private double shapePadding;
        private bool toggleVisibility;
        private bool visible;
        private string width;

        [CascadingParameter]
        internal SfSmithChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Alignment for legend.
        /// </summary>
        [Parameter]
        public SmithChartAlignment Alignment { get; set; } = SmithChartAlignment.Center;

        /// <summary>
        /// Column count for legend.
        /// </summary>
        [Parameter]
        public int ColumnCount { get; set; } = 0;

        /// <summary>
        /// Description for legend.
        /// </summary>
        [Parameter]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Height of the legend.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = string.Empty;

        /// <summary>
        /// Spacing between legend items.
        /// </summary>
        [Parameter]
        public double ItemPadding { get; set; } = 8;

        /// <summary>
        /// Position of the legend.
        /// </summary>
        [Parameter]
        public LegendPosition Position { get; set; } = LegendPosition.Bottom;

        /// <summary>
        /// Row count for legend.
        /// </summary>
        [Parameter]
        public int RowCount { get; set; } = 0;

        /// <summary>
        /// Shape of the legend.
        /// </summary>
        [Parameter]
        public Shape Shape { get; set; } = Shape.Circle;

        /// <summary>
        /// Padding between the legend shape and text.
        /// </summary>
        [Parameter]
        public double ShapePadding { get; set; } = 5;

        /// <summary>
        /// If set to true, series visibility collapses based on the legend visibility.
        /// </summary>
        [Parameter]
        public bool ToggleVisibility { get; set; } = true;

        /// <summary>
        /// Visibility of the legend.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Width of the legend.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = string.Empty;

        internal SmithChartLegendItemStyle ItemStyle { get; set; } = new SmithChartLegendItemStyle();

        internal SmithChartLegendLocation Location { get; set; } = new SmithChartLegendLocation();

        internal SmithChartLegendBorder Border { get; set; } = new SmithChartLegendBorder();

        internal SmithChartLegendTextStyle TextStyle { get; set; } = new SmithChartLegendTextStyle();

        internal SmithChartLegendTitle Title { get; set; } = new SmithChartLegendTitle();

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
            alignment = NotifyPropertyChanges(nameof(Alignment), Alignment, alignment);
            position = NotifyPropertyChanges(nameof(Position), Position, position);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            shapePadding = NotifyPropertyChanges(nameof(ShapePadding), ShapePadding, shapePadding);
            toggleVisibility = NotifyPropertyChanges(nameof(ToggleVisibility), ToggleVisibility, toggleVisibility);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            columnCount = NotifyPropertyChanges(nameof(ColumnCount), ColumnCount, columnCount);
            itemPadding = NotifyPropertyChanges(nameof(ItemPadding), ItemPadding, itemPadding);
            rowCount = NotifyPropertyChanges(nameof(RowCount), RowCount, rowCount);
            shape = NotifyPropertyChanges(nameof(Shape), Shape, shape);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Any() && IsRendered)
            {
                await Parent.PropertyChanged(PropertyChanges, "LegendSettings");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            ItemStyle = null;
            TextStyle = null;
            Location = null;
            Title = null;
        }
    }
}