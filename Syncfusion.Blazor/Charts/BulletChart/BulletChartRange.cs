using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets for customizing the range color properties of the bullet chart component.
    /// </summary>
    public class BulletChartRange : SfBaseComponent
    {
        private double end;
        private double opacity;
        private LegendShape shape;
        private string color;

        [CascadingParameter]
        internal BulletChartRangeCollection Parent { get; set; }

        [CascadingParameter]
        internal IBulletChart BaseParent { get; set; }

        /// <summary>
        /// Sets and gets the color of the range.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Sets and gets the end value of the range.
        /// </summary>
        [Parameter]
        public double End { get; set; }

        /// <summary>
        /// Sets and gets the opacity of the range for the color.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets the name of the range.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Sets and gets the shape of the legend. Each ranges has its own legend shape. They are,
        /// Circle
        /// Rectangle
        /// Triangle
        /// Diamond
        /// Cross
        /// HorizontalLine
        /// VerticalLine
        /// Pentagon
        /// InvertedTriangle.
        /// </summary>
        [Parameter]
        public LegendShape Shape { get; set; } = LegendShape.Rectangle;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Ranges.Add(this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            end = NotifyPropertyChanges(nameof(End), End, end);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            shape = NotifyPropertyChanges(nameof(Shape), Shape, shape);
            if (PropertyChanges.Count > 0 && IsRendered)
            {
                await BaseParent.OnPropertyChanged(PropertyChanges, nameof(BulletChartRange));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }
    }
}