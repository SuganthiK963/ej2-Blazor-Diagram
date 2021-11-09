using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the tooltip settings of the range in circular gauge.
    /// </summary>
    public partial class CircularGaugeRangeTooltipSettings
    {
        private bool enableAnimation;
        private string fill;
        private string format;
        private bool showAtMousePosition;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the animation for the range tooltip. The animation is set as true by default.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Gets or sets the fill color of the range tooltip. This property accepts value in hex code, rgba string as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets the format of the range tooltip in circular gauge.
        /// </summary>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the range tooltip to be shown at mouse position. By default, it set as false.
        /// </summary>
        [Parameter]
        public bool ShowAtMousePosition { get; set; }

        /// <summary>
        /// Gets or sets the custom template to render the tooltip content.
        /// </summary>
        [Parameter]
        public RenderFragment Template { get; set; }

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeTooltipSettings Parent { get; set; }

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the border of the tooltip.
        /// </summary>
        internal CircularGaugeRangeTooltipBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the styles of the tooltip text.
        /// </summary>
        internal CircularGaugeRangeTooltipTextStyle TextStyle { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        internal void UpdateChildProperties(string key, object keyValue)
        {
            if (key == "Border")
            {
                Border = (CircularGaugeRangeTooltipBorder)keyValue;
            }
            else if (key == "TextStyle")
            {
                TextStyle = (CircularGaugeRangeTooltipTextStyle)keyValue;
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
            TextStyle = null;
            Template = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("RangeSettings", this);
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
            showAtMousePosition = NotifyPropertyChanges(nameof(ShowAtMousePosition), ShowAtMousePosition, showAtMousePosition);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
