using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Animation effects that are applicable to the Tooltip. A different animation can be set for tooltip open and close action.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Effect
    {
        /// <summary>
        /// Tooltip open/close actions occur with the FadeIn animation effect.
        /// </summary>
        [EnumMember(Value = "FadeIn")]
        FadeIn,

        /// <summary>
        /// Tooltip open/close actions occur with the FadeOut animation effect.
        /// </summary>
        [EnumMember(Value = "FadeOut")]
        FadeOut,

        /// <summary>
        /// Tooltip open/close actions occur with the FadeZoomIn animation effect.
        /// </summary>
        [EnumMember(Value = "FadeZoomIn")]
        FadeZoomIn,

        /// <summary>
        /// Tooltip open/close actions occur with the FadeZoomOut animation effect.
        /// </summary>
        [EnumMember(Value = "FadeZoomOut")]
        FadeZoomOut,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipXDownIn animation effect.
        /// </summary>
        [EnumMember(Value = "FlipXDownIn")]
        FlipXDownIn,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipXDownOut animation effect.
        /// </summary>
        [EnumMember(Value = "FlipXDownOut")]
        FlipXDownOut,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipXUpIn animation effect.
        /// </summary>
        [EnumMember(Value = "FlipXUpIn")]
        FlipXUpIn,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipXUpOut animation effect.
        /// </summary>
        [EnumMember(Value = "FlipXUpOut")]
        FlipXUpOut,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipYLeftIn animation effect.
        /// </summary>
        [EnumMember(Value = "FlipYLeftIn")]
        FlipYLeftIn,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipYLeftOut animation effect.
        /// </summary>
        [EnumMember(Value = "FlipYLeftOut")]
        FlipYLeftOut,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipYRightIn animation effect.
        /// </summary>
        [EnumMember(Value = "FlipYRightIn")]
        FlipYRightIn,

        /// <summary>
        /// Tooltip open/close actions occur with the FlipYRightOut animation effect.
        /// </summary>
        [EnumMember(Value = "FlipYRightOut")]
        FlipYRightOut,

        /// <summary>
        /// Tooltip open/close actions occur with the ZoomIn animation effect.
        /// </summary>
        [EnumMember(Value = "ZoomIn")]
        ZoomIn,

        /// <summary>
        /// Tooltip open/close actions occur with the ZoomOut animation effect.
        /// </summary>
        [EnumMember(Value = "ZoomOut")]
        ZoomOut,

        /// <summary>
        /// Tooltip open/close actions occur without any animation effect.
        /// </summary>
        [EnumMember(Value = "None")]
        None
    }

    /// <summary>
    /// To set the open modes available for the Tooltip.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OpenMode
    {
        /// <summary>
        /// In Auto mode, the tooltip appears when you hover over the target or when the target element receives the focus.
        /// In mobile devices, the tooltip opens on tap and hold of the target element.
        /// </summary>
        [EnumMember(Value = "Auto")]
        Auto,

        /// <summary>
        /// In Hover mode, the tooltip appears when you hover over the target on the desktop.
        /// In mobile devices, the tooltip opens on a tap and hold of the target element.
        /// </summary>
        [EnumMember(Value = "Hover")]
        Hover,

        /// <summary>
        /// In Click mode, the tooltip appears when you click a target element on the desktop.
        /// In mobile devices, Tooltip appears with a single tap on the target element.
        /// </summary>
        [EnumMember(Value = "Click")]
        Click,

        /// <summary>
        /// In Focus mode, Tooltip appears when you focus on a target element in desktop.
        /// In mobile devices, Tooltip appears with a single tap on the target element.
        /// </summary>
        [EnumMember(Value = "Focus")]
        Focus,

        /// <summary>
        /// In Custom mode, the tooltip will not appear on any default action. You have to bind your custom events and use either open or close public methods in both desktop and mobile devices.
        /// </summary>
        [EnumMember(Value = "Custom")]
        Custom
    }

    /// <summary>
    /// To set the applicable positions where the Tooltip can be displayed over specific target elements.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Position
    {
        /// <summary>
        /// Positions the Tooltip to the top-center of the target element.
        /// </summary>
        [EnumMember(Value = "TopCenter")]
        TopCenter,

        /// <summary>
        /// Positions the Tooltip to the top-left of the target element.
        /// </summary>
        [EnumMember(Value = "TopLeft")]
        TopLeft,

        /// <summary>
        /// Positions the Tooltip to the top-right of the target element.
        /// </summary>
        [EnumMember(Value = "TopRight")]
        TopRight,

        /// <summary>
        /// Positions the Tooltip to the bottom-left of the target element.
        /// </summary>
        [EnumMember(Value = "BottomLeft")]
        BottomLeft,

        /// <summary>
        /// Positions the Tooltip to the bottom-center of the target element.
        /// </summary>
        [EnumMember(Value = "BottomCenter")]
        BottomCenter,

        /// <summary>
        /// Positions the Tooltip to the bottom-right of the target element.
        /// </summary>
        [EnumMember(Value = "BottomRight")]
        BottomRight,

        /// <summary>
        /// Positions the Tooltip to the left-top of the target element.
        /// </summary>
        [EnumMember(Value = "LeftTop")]
        LeftTop,

        /// <summary>
        /// Positions the Tooltip to the left-center of the target element.
        /// </summary>
        [EnumMember(Value = "LeftCenter")]
        LeftCenter,

        /// <summary>
        /// Positions the Tooltip to the left-bottom of the target element.
        /// </summary>
        [EnumMember(Value = "LeftBottom")]
        LeftBottom,

        /// <summary>
        /// Positions the Tooltip to the right-top of the target element.
        /// </summary>
        [EnumMember(Value = "RightTop")]
        RightTop,

        /// <summary>
        /// Positions the Tooltip to the right-center of the target element.
        /// </summary>
        [EnumMember(Value = "RightCenter")]
        RightCenter,

        /// <summary>
        /// Positions the Tooltip to the right-bottom of the target element.
        /// </summary>
        [EnumMember(Value = "RightBottom")]
        RightBottom
    }

    /// <summary>
    /// Applicable tip positions are attached to the Tooltip.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipPointerPosition
    {
        /// <summary>
        /// Automatically adjusts the tip pointer position.
        /// </summary>
        [EnumMember(Value = "Auto")]
        Auto,

        /// <summary>
        /// Positions the tip pointer at the start of the Tooltip element.
        /// </summary>
        [EnumMember(Value = "Start")]
        Start,

        /// <summary>
        /// Positions the tip pointer in the middle of the Tooltip element.
        /// </summary>
        [EnumMember(Value = "Middle")]
        Middle,

        /// <summary>
        /// Positions the tip pointer at the end of the Tooltip element.
        /// </summary>
        [EnumMember(Value = "End")]
        End
    }
}