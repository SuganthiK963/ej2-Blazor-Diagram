namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// ColorPicker Mode.
    /// </summary>
    public enum ColorPickerMode
    {
        /// <summary>
        /// To set the color picker mode as picker.
        /// </summary>
        Picker,

        /// <summary>
        /// To set the color picker mode as palette.
        /// </summary>
        Palette,
    }

    /// <summary>
    /// Color value types.
    /// </summary>
    public enum ColorValueType
    {
        /// <summary>
        /// Specifies the hex code value without opacity.
        /// </summary>
        Hex,

        /// <summary>
        /// Specifies the hex code value with opacity.
        /// </summary>
        Hexa,

        /// <summary>
        /// Specifies the red green blue value without opacity.
        /// </summary>
        Rgb,

        /// <summary>
        /// Specifies the red green blue value with opacity.
        /// </summary>
        Rgba,

        /// <summary>
        /// Specifies the hue saturation value without opacity.
        /// </summary>
        Hsv,

        /// <summary>
        /// Specifies the hue saturation value with opacity.
        /// </summary>
        Hsva,

        /// <summary>
        /// Specifies the opacity value.
        /// </summary>
        Opacity,
    }
}