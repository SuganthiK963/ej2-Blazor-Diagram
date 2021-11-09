using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Sets and gets the options for the text style of the tooltip text for ranges in circular Gauge.
    /// </summary>
    public class CircularGaugeRangeTooltipTextStyle : CircularGaugeFontSettings
    {
        private string color;
        private string fontFamily;
        private string fontStyle;
        private string fontWeight;
        private double opacity;
        private string size;

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        private CircularGaugeRangeTooltipSettings DynamicParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            DynamicParent = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DynamicParent.UpdateChildProperties("TextStyle", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            fontFamily = NotifyPropertyChanges(nameof(FontFamily), FontFamily, fontFamily);
            fontStyle = NotifyPropertyChanges(nameof(FontStyle), FontStyle, fontStyle);
            fontWeight = NotifyPropertyChanges(nameof(FontWeight), FontWeight, fontWeight);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            size = NotifyPropertyChanges(nameof(Size), Size, size);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
