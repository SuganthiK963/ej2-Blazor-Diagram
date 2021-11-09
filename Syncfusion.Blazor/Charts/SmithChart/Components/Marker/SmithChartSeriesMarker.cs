using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the marker in the chart.
    /// </summary>
    public partial class SmithChartSeriesMarker
    {
        private string fill;
        private double height;
        private double opacity;
        private Shape shape;
        private bool visible;
        private double width;

        [CascadingParameter]
        internal SmithChartSeries Parent { get; set; }

        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Color for marker.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Height of the marker.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = 6;

        /// <summary>
        /// Specifies the image for series marker.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
#pragma warning disable CA1056
        public string ImageUrl { get; set; } = string.Empty;
#pragma warning restore CA1056

        /// <summary>
        /// Opacity of the marker.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Shape of the marker.
        /// </summary>
        [Parameter]
        public Shape Shape { get; set; } = Shape.Circle;

        /// <summary>
        /// Visibility of the marker.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Width of the marker.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 6;

        internal SmithChartSeriesMarkerBorder Border { get; set; } = new SmithChartSeriesMarkerBorder();

        internal SmithChartSeriesDatalabel DataLabel { get; set; } = new SmithChartSeriesDatalabel();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Marker = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            shape = NotifyPropertyChanges(nameof(Shape), Shape, shape);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            if (PropertyChanges.Any() && IsRendered)
            {
                await BaseParent.PropertyChanged(PropertyChanges, "Series");
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
            DataLabel = null;
            BaseParent = null;
        }
    }
}