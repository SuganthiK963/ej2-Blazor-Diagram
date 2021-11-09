using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// ColorPicker component is a user interface to select and adjust color values.
    /// It provides supports for various color specification like Red Green Blue, Hue Saturation Value and Hex codes.
    /// </summary>
    public partial class SfColorPicker : SfBaseComponent
    {
        /// <summary>
        /// Sets ID attribute for color picker element.
        /// </summary>
        [Parameter]
        [Obsolete("This property has been deprecated. Use @attributes to set ID for color picker element.")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as id, title etc., to the dropdown button element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [Obsolete("This property has been deprecated. Use @attributes to set additional attributes for dropdown button element.")]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes; }
            set { htmlAttributes = value; }
        }

        /// <summary>
        /// It is used to render the ColorPicker palette with specified columns.
        /// </summary>
        [Parameter]
        public double Columns { get; set; } = 10;

        /// <summary>
        /// This property sets the CSS classes to root element of the ColorPicker
        ///  which helps to customize the UI styles.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// It is used to enable / disable ColorPicker component. If it is disabled the ColorPicker popup won’t open.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// It is used to enable / disable the opacity option of ColorPicker component.
        /// </summary>
        [Parameter]
        public bool EnableOpacity { get; set; } = true;

        /// <summary>
        /// To enable or disable persisting component's state between page reloads and it is extended from component class.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// It is used to render the ColorPicker component as inline.
        /// </summary>
        [Parameter]
        public bool Inline { get; set; }

        /// <summary>
        /// Overrides the global culture and localization value for this component. Default global culture is 'en-US'.
        /// </summary>
        [Parameter]
        [Obsolete("The Locale is deprecated and will no longer be used. Hereafter, the Locale works based on the current culture.")]
        public string Locale { get; set; }

        /// <summary>
        /// It is used to render the ColorPicker with the specified mode.
        /// </summary>
        [Parameter]
        public ColorPickerMode Mode { get; set; } = ColorPickerMode.Picker;

        /// <summary>
        /// It is used to show / hide the mode switcher button of ColorPicker component.
        /// </summary>
        [Parameter]
        public bool ModeSwitcher { get; set; } = true;

        /// <summary>
        /// It is used to enable / disable the no color option of ColorPicker component.
        /// </summary>
        [Parameter]
        public bool NoColor { get; set; }

        /// <summary>
        /// It is used to load custom colors to palette.
        /// </summary>
        [Parameter]
        public Dictionary<string, string[]> PresetColors { get; set; }

        /// <summary>
        /// It is used to show / hide the control buttons (apply / cancel) of  ColorPicker component.
        /// </summary>
        [Parameter]
        public bool ShowButtons { get; set; } = true;

        /// <summary>
        /// It is used to set the color value for ColorPicker. It should be specified as Hex code.
        /// </summary>
        [Parameter]
        public string Value { get; set; } = DEFAULT_COLOR;

        /// <summary>
        /// Specifies the callback to trigger when the value changes.
        /// </summary>
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Specifies the expression for defining the value of the bound.
        /// </summary>
        [Parameter]
        public Expression<Func<string>> ValueExpression { get; set; }

        [CascadingParameter]
        protected EditContext ColorPickerEditContext { get; set; }

        /// <summary>
        /// Triggers before opening the ColorPicker popup.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenCloseEventArgs> OnOpen { get; set; }

        /// <summary>
        /// Triggers while opening the ColorPicker popup.
        /// </summary>
        [Parameter]
        public EventCallback<OpenEventArgs> Opened { get; set; }

        /// <summary>
        /// Triggers before closing the ColorPicker popup.
        /// </summary>
        [Parameter]
        public EventCallback<BeforeOpenCloseEventArgs> OnClose { get; set; }

        /// <summary>
        /// Triggers before Switching between ColorPicker mode.
        /// </summary>
        [Parameter]
        public EventCallback<ModeSwitchEventArgs> OnModeSwitch { get; set; }

        /// <summary>
        /// Triggers after the ColorPicker mode is switched.
        /// </summary>
        [Parameter]
        public EventCallback<ModeSwitchEventArgs> ModeSwitched { get; set; }

        /// <summary>
        /// Triggers while rendering each palette tile.
        /// </summary>
        [Parameter]
        public EventCallback<PaletteTileEventArgs> OnTileRender { get; set; }

        /// <summary>
        /// Triggers while changing the colors. It will be triggered based on the showButtons property.
        /// If the property is false, the event will be triggered while selecting the colors.
        /// If the property is true, the event will be triggered while apply the selected color.
        /// </summary>
        [Parameter]
        public EventCallback<ColorPickerEventArgs> ValueChange { get; set; }

        /// <summary>
        /// Triggers while selecting the color in picker / palette, when ShowButtons property is enabled.
        /// </summary>
        [Parameter]
        public EventCallback<ColorPickerEventArgs> Selected { get; set; }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Parent component of ColorPicker.
        /// </summary>

        // </exclude>
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic ColorPickerParent { get; set; }
    }
}