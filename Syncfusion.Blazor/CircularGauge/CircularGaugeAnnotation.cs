using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the annotation element for an axis in circular gauge component.
    /// </summary>
    public partial class CircularGaugeAnnotation : SfBaseComponent
    {
        private double angle;
        private bool autoAngle;
        private string content;
        private string radius;
        private string zindex;
        private int annotationIndex;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the annotation.
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Gets or sets the angle for annotation with respect to axis in circular gauge component.
        /// </summary>
        [Parameter]
        public double Angle { get; set; } = 90;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the annotation rotation along the axis.
        /// </summary>
        [Parameter]
        public bool AutoAngle { get; set; }

        /// <summary>
        /// Gets or sets the content of the annotation.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the information about annotation for assistive technology.
        /// </summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the radius for annotation with respect to axis in circular gauge component.
        /// </summary>
        [Parameter]
        public string Radius { get; set; } = "50%";

        /// <summary>
        /// Gets or sets the z-index of an annotation in an axis in the circular gauge component.
        /// </summary>
        [Parameter]
        public string ZIndex { get; set; } = "-1";

        /// <summary>
        /// Gets or sets the properties of the annotaions.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAnnotations Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the style of the text in annotations.
        /// </summary>
        internal CircularGaugeAnnotationTextStyle TextStyle { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="value">represents the annotation text styles.</param>
        internal void UpdateChildProperties(CircularGaugeAnnotationTextStyle value)
        {
            TextStyle = value;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            ContentTemplate = null;
            TextStyle = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            annotationIndex = Parent.Annotations.Count - 1;
            Parent.Parent.AxisValues.AnnotationContent.Add(Content);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            angle = NotifyPropertyChanges(nameof(Angle), Angle, angle);
            autoAngle = NotifyPropertyChanges(nameof(AutoAngle), AutoAngle, autoAngle);
            content = NotifyPropertyChanges(nameof(Content), Content, content);
            radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);
            zindex = NotifyPropertyChanges(nameof(ZIndex), ZIndex, zindex);

            if (PropertyChanges.Count > 0)
            {
                if (PropertyChanges.ContainsKey("Content"))
                {
                    Parent.Parent.AxisValues.AnnotationContent[annotationIndex] = Content;
                }

                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
