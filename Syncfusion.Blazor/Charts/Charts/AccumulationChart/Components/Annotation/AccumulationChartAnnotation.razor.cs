using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using Newtonsoft.Json;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Annotation is a user defined HTML element that can be placed on chart
    /// We can use annotations to pile up the visual elegance of the chart.
    /// Specifies the customization of annotation.
    /// </summary>
    public partial class AccumulationChartAnnotation
    {
        [CascadingParameter]
        private AccumulationChartAnnotations annotationCollection { get; set; }

        [CascadingParameter]
        internal SfAccumulationChart ChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets and sets the Template content of this annotation.
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Content of the annotation, which accepts the id of the custom element.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Specifies the coordinate units of the annotation. They are
        /// Pixel - Annotation renders based on x and y pixel value.
        /// Point - Annotation renders based on x and y axis value.
        /// </summary>
        [Parameter]
        public Units CoordinateUnits { get; set; }

        private Units coordinateUnits { get; set; }

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
        public Regions Region { get; set; }

        private Regions region { get; set; }

        /// <summary>
        /// if set coordinateUnit as `Pixel` X specifies the axis value
        /// else is specifies pixel or percentage of coordinate.
        /// </summary>
        [Parameter]
        public string X { get; set; } = "0";

        private string x { get; set; }

        /// <summary>
        /// if set coordinateUnit as `Pixel` Y specifies the axis value
        /// else is specifies pixel or percentage of coordinate.
        /// </summary>
        [Parameter]
        public string Y { get; set; } = "0";

        private string y { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
            annotationCollection.Annotations.Add(this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            coordinateUnits = CoordinateUnits = NotifyPropertyChanges(nameof(CoordinateUnits), CoordinateUnits, coordinateUnits);
            region = Region = NotifyPropertyChanges(nameof(Region), Region, region);
            x = X = NotifyPropertyChanges(nameof(X), X, x);
            y = Y = NotifyPropertyChanges(nameof(Y), Y, y);
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary("Annotations", this, ChartInstance.PropertyChanges);
                PropertyChanges.Clear();
                await ChartInstance.OnAccumulationChartParametersSet();
#pragma warning restore CA2007
            }
        }

        internal override void ComponentDispose()
        {
            annotationCollection = null;
            ChildContent = null;
            ChartInstance = null;
            ContentTemplate = null;
        }
    }
}