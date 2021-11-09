using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Defines the options for customizing the fonts.
    /// </summary>
    public partial class CircularGaugeFontSettings : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the font color of the text in annotation, label and tooltip etc.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the font family for the given text in annotation, tooltip etc.
        /// </summary>
        [Parameter]
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the style of the font, which is in annotation, tooltip etc.
        /// </summary>
        [Parameter]
        public string FontStyle { get; set; } = "Normal";

        /// <summary>
        /// Gets or sets the font weight for the text in annotation, tooltip etc.
        /// </summary>
        [Parameter]
        public string FontWeight { get; set; } = "Normal";

        /// <summary>
        /// Gets or sets the opacity for the annotation or tooltip text.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the font size of the text in annotation, label, and tooltip, etc. The default of the size is '16px'.
        /// </summary>
        [Parameter]
        public virtual string Size { get; set; } = "16px";

        /// <summary>
        /// Gets or sets the cascading value and parameters for all of its descendants of the class.
        /// </summary>
        [CascadingParameter]
        internal object Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            BaseParent = null;
            Parent = null;
        }
    }
}
