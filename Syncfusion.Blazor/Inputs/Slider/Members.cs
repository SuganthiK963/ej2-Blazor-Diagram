using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the partial class SfSlider.
    /// </summary>
    public partial class SfSlider<TValue> : SfBaseComponent
    {
        /// <summary>
        /// Sets id attribute for the slider element.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Specifies the ChildContent.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public Expression<Func<TValue>> ValueExpression { get; set; }

        /// <summary>
        /// Specifies the ChildContent.
        /// </summary>
        [CascadingParameter]
        protected EditContext SliderEditContext { get; set; }

        /// <summary>
        /// Specifies the color to the slider based on given value.
        /// </summary>
        [Obsolete("This property is deprecated, Use SliderColorRanges tag as replacement")]
        [Parameter]
        public List<ColorRangeDataModel> ColorRange { get { return SfSliderColorRange; } set { SfSliderColorRange = value; } }
        internal List<ColorRangeDataModel> SfSliderColorRange { get; set; }

        /// <summary>
        /// Specifies the custom classes to be added to the element used to customize the slider.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;
        private string SliderCssClass { get; set; }

        /// <summary>
        /// Specifies an array of slider values in number or string type.
        /// The min and max step values are not considered.
        /// </summary>
        [Parameter]
        public string[] CustomValues { get; set; }

        /// <summary>
        /// Enables/Disables the animation for slider movement.
        /// </summary>
        [Parameter]
        public bool EnableAnimation { get; set; } = true;

        /// <summary>
        /// Defines whether to allow the cross-scripting site or not.
        /// </summary>
        [Obsolete("This property is deprecated and no longer be available")]
        [Parameter]
        public bool EnableHtmlSanitizer { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Enables or disables the slider.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;
        private bool SliderEnabled { get; set; }

        /// <summary>
        /// Specified the limit within which the slider to be moved.
        /// </summary>
        [Obsolete("This property is deprecated, Use SliderLimit tag as replacement")]
        [Parameter]
        public SliderLimits Limits { get { return SfSliderLimits; } set { SfSliderLimits = value; } }
        private SliderLimits SliderLimits { get; set; }
        internal SliderLimits SfSliderLimits { get; set; }

        /// <summary>
        /// Overrides the global culture and localization value for this component. Default global culture is 'en-US'.
        /// </summary>
        [Obsolete("This property is deprecated and no longer be available")]
        [Parameter]
        public string Locale { get { return SliderLocale; } set { SliderLocale = value; } }
        private string SliderLocale { get; set; } = string.Empty;
        internal string SfSliderLocale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the maximum value of the slider.
        /// </summary>
        [Parameter]
        public double Max { get; set; } = 100;
        private double SliderMax { get; set; }

        /// <summary>
        /// Specifies the minimum value of the slider.
        /// </summary>
        [Parameter]
        public double Min { get; set; } = 0;
        private double SliderMin { get; set; }

        /// <summary>
        ///  Specifies whether to render the slider in vertical or horizontal orientation.
        /// </summary>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SliderOrientation Orientation { get; set; }

        /// <summary>
        /// Specifies whether the render the slider in read-only mode to restrict any user interaction.
        /// The slider rendered with user defined values and can’t be interacted with user actions.
        /// </summary>
        [Parameter]
        public bool ReadOnly { get; set; }
        private bool SliderReadOnly { get; set; }

        /// <summary>
        /// Specifies whether to show or hide the increase/decrease buttons
        /// of Slider to change the slider value.
        /// </summary>
        [Parameter]
        public bool ShowButtons { get; set; }
        private bool SliderShowButtons { get; set; }

        /// <summary>
        /// Specifies the step value for each value change when the increase / decrease
        ///  button is clicked or on arrow keys press or on dragging the thumb.
        /// </summary>
        [Parameter]
        public double Step { get; set; } = 1;
        private double SliderStep { get; set; }

        /// <summary>
        /// It is used to render the slider ticks options such as placement and step values.
        /// </summary>
        [Obsolete("This property is deprecated, Use SliderTicks tag as replacement")]
        [Parameter]
        public SliderTicks Ticks { get { return SfSliderTicks; } set { SfSliderTicks = value; } }
        private SliderTicks SliderTicks { get; set; }

        internal SliderTicks SfSliderTicks { get; set; }

        /// <summary>
        /// Specifies the visibility, position of the tooltip over the slider element.
        /// </summary>
        [Obsolete("This property is deprecated, Use SliderTooltip tag as replacement")]
        [Parameter]
        public SliderTooltip Tooltip { get {return SfSliderTooltip; } set { SfSliderTooltip =value; } }
        private SliderTooltip SliderTooltip { get; set; }

        internal SliderTooltip SfSliderTooltip { get; set; }

        /// <summary>
        /// Defines the type of the Slider. The available options are:
        ///   Default - Allows to a single value in the Slider.
        ///   MinRange - Allows to select a single value in the Slider. It display’s a shadow from the start to the current value.
        ///   Range - Allows to select a range of values in the Slider. It displays shadow in-between the selection range.
        /// </summary>
        [Parameter]
        public SliderType Type { get; set; }

        /// <summary>
        /// It is used to denote the current value of the Slider.
        /// The value should be specified in array of number when render Slider type as range.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }
        private TValue SliderValue { get; set; }

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Specifies the width of the Slider.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Specifies whether the value need to be updated at the time of dragging slider handle.
        /// </summary>
        [Parameter]
        public bool IsImmediateValue { get; set; } = true;
        private bool SliderIsImmediateValue { get; set; }

        /// <summary>
        /// Used to specify an additional html attributes such as styles, class, and more to the root element.
        /// </summary>
        [Obsolete("This property is deprecated, Use @attribute as replacement")]
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes { get { return SliderHtmlAttributes; } set { SliderHtmlAttributes = value; } }

        internal Dictionary<string, object> SliderHtmlAttributes { get; set; }
    }
}
