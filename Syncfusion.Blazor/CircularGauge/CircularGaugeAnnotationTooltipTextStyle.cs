﻿using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets and sets the tooltip text style of annotation.
    /// </summary>
    public class CircularGaugeAnnotationTooltipTextStyle : CircularGaugeFontSettings
    {
        private string color;
        private string fontFamily;
        private string fontStyle;
        private string fontWeight;
        private double opacity;
        private string size;

        /// <summary>
        /// Gets or sets the properties to render tooltip for annotation.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugeAnnotationTooltipSettings DynamicParent { get; set; }

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
