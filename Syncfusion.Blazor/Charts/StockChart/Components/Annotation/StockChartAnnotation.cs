using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies annotation for stockchart.
    /// </summary>
    public partial class StockChartAnnotation : SfBaseComponent
    {
        private Units coordinateUnits;
        private string description;
        private Regions region;
        private object x;
        private string xaxisname;
        private string yaxisname;
        private string y;

        [CascadingParameter]
        internal StockChartAnnotations Parent { get; set; }

        /// <summary>
        /// Gets and sets the template of the annotation.
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Specifies the coordinate units of the annotation. They are
        /// Pixel - Annotation renders based on x and y pixel value.
        /// Point - Annotation renders based on x and y axis value.
        /// </summary>
        [Parameter]
        public Units CoordinateUnits { get; set; } = Units.Pixel;

        /// <summary>
        /// Information about annotation for assistive technology.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Specifies the regions of the annotation. They are
        /// Chart - Annotation renders based on chart coordinates.
        /// Series - Annotation renders based on series coordinates.
        /// </summary>
        [Parameter]
        public Regions Region { get; set; } = Regions.Chart;

        /// <summary>
        /// if set coordinateUnit as `Pixel` X specifies the axis value
        /// else is specifies pixel or percentage of coordinate.
        /// </summary>
        [Parameter]
        public object X { get; set; } = "0";

        /// <summary>
        /// The name of horizontal axis associated with the annotation.
        /// It requires `Axes` of chart.
        /// </summary>
        [Parameter]
        public string XAxisName { get; set; }

        /// <summary>
        /// if set coordinateUnit as `Pixel` Y specifies the axis value
        /// else is specifies pixel or percentage of coordinate.
        /// </summary>
        [Parameter]
        public string Y { get; set; } = "0";

        /// <summary>
        /// The name of vertical axis associated with the annotation.
        /// It requires `Axes` of chart.
        /// </summary>
        [Parameter]
        public string YAxisName { get; set; }

        /// <summary>
        /// Specifies the content of annotation.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Specifies the horizontal alignment of annotation.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public Alignment HorizontalAlignment { get; set; } = Alignment.Center;

        /// <summary>
        /// Specifies the vertical alignment of annotation.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public Position VerticalAlignment { get; set; } = Position.Middle;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent?.Annotations?.Add(this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            coordinateUnits = NotifyPropertyChanges(nameof(CoordinateUnits), CoordinateUnits, coordinateUnits);
            description = NotifyPropertyChanges(nameof(Description), Description, description);
            region = NotifyPropertyChanges(nameof(Region), Region, region);
            x = NotifyPropertyChanges(nameof(X), X, x);
            xaxisname = NotifyPropertyChanges(nameof(XAxisName), XAxisName, xaxisname);
            y = NotifyPropertyChanges(nameof(Y), Y, y);
            yaxisname = NotifyPropertyChanges(nameof(YAxisName), YAxisName, yaxisname);
            if (PropertyChanges.Any() && IsRendered)
            {
                PropertyChanges.Clear();
                Parent?.StockChartInstance?.OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ContentTemplate = null;
        }
    }
}