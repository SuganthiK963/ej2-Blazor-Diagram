using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the options for customizing the tooltip of gauge.
    /// </summary>
    public partial class CircularGaugeTooltipSettings
    {
        private bool enable;
        private bool enableAnimation;
        private string fill;
        private string format;
        private bool showAtMousePosition;
        private string[] type;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the custom template to render the tooltip content.
        /// </summary>
        [Parameter]
        public RenderFragment TooltipTemplate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the visibility of tooltip.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the animation to take place in circular gauge.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Gets or sets the fill color of the tooltip. This property accepts value in hex code, rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets the format for the tooltip content in circular gauge.
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the tooltip of the circular gauge at mouse position. By default, it set as false.
        /// </summary>
        [Parameter]
        public bool ShowAtMousePosition { get; set; }

        /// <summary>
        /// Gets or sets the options to select the type of tooltip for range, annotation and pointer.
        /// </summary>
        [Parameter]
        public string[] Type { get; set; }

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
        /// Gets or sets the properties for displaying the annotation tooltip.
        /// </summary>
        internal CircularGaugeAnnotationTooltipSettings AnnotationSettings { get; set; }

        /// <summary>
        /// Gets or sets the properties for displaying the border of the tooltip.
        /// </summary>
        internal CircularGaugeTooltipBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the properties for displaying the range tooltip.
        /// </summary>
        internal CircularGaugeRangeTooltipSettings RangeSettings { get; set; }

        /// <summary>
        /// Gets or sets the properties for displaying the text styles of the tooltip.
        /// </summary>
        internal CircularGaugeTooltipTextStyle TextStyle { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        internal void UpdateChildProperties(string key, object keyValue)
        {
            switch (key)
            {
                case "Border":
                    Border = (CircularGaugeTooltipBorder)keyValue;
                    break;
                case "TextStyle":
                    TextStyle = (CircularGaugeTooltipTextStyle)keyValue;
                    break;
                case "RangeSettings":
                    RangeSettings = (CircularGaugeRangeTooltipSettings)keyValue;
                    break;
                case "AnnotationSettings":
                    AnnotationSettings = (CircularGaugeAnnotationTooltipSettings)keyValue;
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
            AnnotationSettings = null;
            Border = null;
            TooltipTemplate = null;
            RangeSettings = null;
            TextStyle = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Tooltip", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enable = NotifyPropertyChanges(nameof(Enable), Enable, enable);
            enableAnimation = NotifyPropertyChanges(nameof(EnableAnimation), EnableAnimation, enableAnimation);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            format = NotifyPropertyChanges(nameof(Format), Format, format);
            showAtMousePosition = NotifyPropertyChanges(nameof(ShowAtMousePosition), ShowAtMousePosition, showAtMousePosition);
            type = NotifyPropertyChanges(nameof(Type), Type, type);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
