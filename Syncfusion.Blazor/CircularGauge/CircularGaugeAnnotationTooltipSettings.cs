using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets and sets the tooltip settings for the annotation in circular gauge.
    /// </summary>
    public partial class CircularGaugeAnnotationTooltipSettings : SfBaseComponent
    {
        private bool enableAnimation;
        private string fill;
        private string format;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the animation of the annotation tooltip. By default, the animation is set as true.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Gets or sets the fill color of the annotation tooltip. This property accepts value in hex code,
        /// rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets the format of annotation in tooltip.
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the custom template to render the tooltip content.
        /// </summary>
        [Parameter]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Gets or sets the properties to render tooltip.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeTooltipSettings Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the border of the tooltip annotations.
        /// </summary>
        internal CircularGaugeAnnotationTooltipBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the text styles for tooltip annotations.
        /// </summary>
        internal CircularGaugeAnnotationTooltipTextStyle TextStyle { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        internal void UpdateChildProperties(string key, object keyValue)
        {
            if (key == "Border")
            {
                Border = (CircularGaugeAnnotationTooltipBorder)keyValue;
            }
            else if (key == "TextStyle")
            {
                TextStyle = (CircularGaugeAnnotationTooltipTextStyle)keyValue;
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
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("AnnotationSettings", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableAnimation = NotifyPropertyChanges(nameof(EnableAnimation), EnableAnimation, enableAnimation);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            format = NotifyPropertyChanges(nameof(Format), Format, format);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
