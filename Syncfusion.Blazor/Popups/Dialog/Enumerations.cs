using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Defines the resize handles in the dialog.
    /// </summary>
    public enum ResizeDirection
    {
        /// <summary>
        /// Defines edge resize of the dialog in the south east direction.
        /// </summary>
        [EnumMember(Value = "SouthEast")]
        SouthEast,

        /// <summary>
        /// Defines border resize of the dialog in the south direction.
        /// </summary>
        [EnumMember(Value = "South")]
        South,

        /// <summary>
        /// Defines border resize of the dialog in the south direction.
        /// </summary>
        [EnumMember(Value = "North")]
        North,

        /// <summary>
        /// Defines border resize of the dialog in the south direction.
        /// </summary>
        [EnumMember(Value = "East")]
        East,

        /// <summary>
        /// Defines border resize of the dialog in the south direction.
        /// </summary>
        [EnumMember(Value = "West")]
        West,

        /// <summary>
        /// Defines edge resize of the dialog in the north east direction.
        /// </summary>
        [EnumMember(Value = "NorthEast")]
        NorthEast,

        /// <summary>
        /// Defines edge resize of the dialog in the north west direction.
        /// </summary>
        [EnumMember(Value = "NorthWest")]
        NorthWest,

        /// <summary>
        /// Defines edge resize of the dialog in the south west direction.
        /// </summary>
        [EnumMember(Value = "SouthWest")]
        SouthWest,

        /// <summary>
        /// Defines border resize of the dialog in all the direction.
        /// </summary>
        [EnumMember(Value = "All")]
        All
    }

    /// <summary>
    /// Defines the type of a button in the dialog.
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// Defines ButtonType as Button.
        /// </summary>
        [EnumMember(Value = "Button")]
        Button,

        /// <summary>
        /// Defines ButtonType as Submit.
        /// </summary>
        [EnumMember(Value = "Submit")]
        Submit,

        /// <summary>
        /// Defines ButtonType as Reset.
        /// </summary>
        [EnumMember(Value = "Reset")]
        Reset
    }

    /// <summary>
    /// Specifies the Dialog animation effects.
    /// </summary>
    public enum DialogEffect
    {
        /// <summary>
        /// Defines DialogEffect as Fade.
        /// </summary>
        [EnumMember(Value = "Fade")]
        Fade,

        /// <summary>
        /// Defines DialogEffect as FadeZoom.
        /// </summary>
        [EnumMember(Value = "FadeZoom")]
        FadeZoom,

        /// <summary>
        /// Defines DialogEffect as FlipLeftDown.
        /// </summary>
        [EnumMember(Value = "FlipLeftDown")]
        FlipLeftDown,

        /// <summary>
        /// Defines DialogEffect as FlipLeftUp.
        /// </summary>
        [EnumMember(Value = "FlipLeftUp")]
        FlipLeftUp,

        /// <summary>
        /// Defines DialogEffect as FlipRightDown.
        /// </summary>
        [EnumMember(Value = "FlipRightDown")]
        FlipRightDown,

        /// <summary>
        /// Defines DialogEffect as FlipRightUp.
        /// </summary>
        [EnumMember(Value = "FlipRightUp")]
        FlipRightUp,

        /// <summary>
        /// Defines DialogEffect as FlipXDown.
        /// </summary>
        [EnumMember(Value = "FlipXDown")]
        FlipXDown,

        /// <summary>
        /// Defines DialogEffect as FlipXUp.
        /// </summary>
        [EnumMember(Value = "FlipXUp")]
        FlipXUp,

        /// <summary>
        /// Defines DialogEffect as FlipYLeft.
        /// </summary>
        [EnumMember(Value = "FlipYLeft")]
        FlipYLeft,

        /// <summary>
        /// Defines DialogEffect as FlipYRight.
        /// </summary>
        [EnumMember(Value = "FlipYRight")]
        FlipYRight,

        /// <summary>
        /// Defines DialogEffect as SlideBottom.
        /// </summary>
        [EnumMember(Value = "SlideBottom")]
        SlideBottom,

        /// <summary>
        /// Defines DialogEffect as SlideLeft.
        /// </summary>
        [EnumMember(Value = "SlideLeft")]
        SlideLeft,

        /// <summary>
        /// Defines DialogEffect as SlideRight.
        /// </summary>
        [EnumMember(Value = "SlideRight")]
        SlideRight,

        /// <summary>
        /// Defines DialogEffect as SlideTop.
        /// </summary>
        [EnumMember(Value = "SlideTop")]
        SlideTop,

        /// <summary>
        /// Defines DialogEffect as Zoom.
        /// </summary>
        [EnumMember(Value = "Zoom")]
        Zoom,

        /// <summary>
        /// Defines DialogEffect as None.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }
}