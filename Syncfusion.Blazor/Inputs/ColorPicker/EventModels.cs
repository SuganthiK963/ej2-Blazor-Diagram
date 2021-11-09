using System;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Interface for before open / close event.
    /// </summary>
    public class BeforeOpenCloseEventArgs
    {
        /// <summary>
        /// Used to prevent color picker popup open.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the color picker popup element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the Event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public EventArgs Event { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for change / select event.
    /// </summary>
    public class ColorPickerEventArgs
    {
        /// <summary>
        /// Specifies the current color value details.
        /// </summary>
        public ColorPickerValue CurrentValue { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the previous color value details.
        /// </summary>
        public ColorPickerValue PreviousValue { get; set; }
    }

    /// <summary>
    /// Interface for Color picker values.
    /// </summary>
    public class ColorPickerValue
    {
        /// <summary>
        /// Specifies the color value as HEX format without opacity.
        /// </summary>
        public string Hex { get; set; }

        /// <summary>
        /// Specifies the color value as RGBA format.
        /// </summary>
        public string Rgba { get; set; }
    }

    /// <summary>
    /// Interface for mode switching event.
    /// </summary>
    public class ModeSwitchEventArgs
    {
        /// <summary>
        /// Specifies the mode switcher element.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Specifies the color picker mode.
        /// </summary>
        public ColorPickerMode Mode { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for open event.
    /// </summary>
    public class OpenEventArgs
    {
        /// <summary>
        /// Specifies the color picker popup element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for before change event.
    /// </summary>
    public class PaletteTileEventArgs
    {
        /// <summary>
        /// Specifies the palette tile element.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name { get; set; }

        /// <summary>
        /// Specifies key name of the custom preset colors.
        /// </summary>
        public string PresetName { get; set; }

        /// <summary>
        /// Specifies the color value.
        /// </summary>
        public string Value { get; set; }
    }

    /// <exclude/>
    public class Offset
    {
        public double Left { get; set; }

        public double Right { get; set; }

        public double Top { get; set; }

        public double ClientLeft { get; set; }

        public double ClientWidth { get; set; }

        public double ClientTop { get; set; }

        public double ClientHeight { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public bool IsDevice { get; set; }
    }
}