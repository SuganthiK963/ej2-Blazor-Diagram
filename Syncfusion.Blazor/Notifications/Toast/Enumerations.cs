using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    ///  Defines the type of a button in the toast.
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// Defines ButtonType as Button.
        /// </summary>
        Button,

        /// <summary>
        /// Defines ButtonType as Submit.
        /// </summary>
        Submit,

        /// <summary>
        /// Defines ButtonType as Reset.
        /// </summary>
        Reset
    }

    /// <summary>
    /// Specifies the Toast Easing effects.
    /// </summary>
    public enum ToastEasing
    {
        /// <summary>
        /// Defines ToastEasing as Easing.
        /// </summary>
        Ease,

        /// <summary>
        /// Defines ToastEasing as Linear.
        /// </summary>
        Linear
    }

    /// <summary>
    /// Specifies the Toast animation effects.
    /// </summary>
    public enum ToastEffect
    {
        /// <summary>
        /// Defines ToastEffect as FadeIn.
        /// </summary>
        FadeIn,

        /// <summary>
        /// Defines ToastEffect as FadeOut.
        /// </summary>
        FadeOut,

        /// <summary>
        /// Defines ToastEffect as FadeZoomIn.
        /// </summary>
        FadeZoomIn,

        /// <summary>
        /// Defines ToastEffect as FadeZoomOut.
        /// </summary>
        FadeZoomOut,

        /// <summary>
        /// Defines ToastEffect as FlipLeftDownIn.
        /// </summary>
        FlipLeftDownIn,

        /// <summary>
        /// Defines ToastEffect as FlipLeftDownOut.
        /// </summary>
        FlipLeftDownOut,

        /// <summary>
        /// Defines ToastEffect as FlipLeftUpIn.
        /// </summary>
        FlipLeftUpIn,

        /// <summary>
        /// Defines ToastEffect as FlipLeftUpOut.
        /// </summary>
        FlipLeftUpOut,

        /// <summary>
        /// Defines ToastEffect as FlipRightDownIn.
        /// </summary>
        FlipRightDownIn,

        /// <summary>
        /// Defines ToastEffect as FlipRightDownOut.
        /// </summary>
        FlipRightDownOut,

        /// <summary>
        /// Defines ToastEffect as FlipRightUpIn
        /// </summary>
        FlipRightUpIn,

        /// <summary>
        /// Defines ToastEffect as FlipRightUpOut
        /// </summary>
        FlipRightUpOut,

        /// <summary>
        /// Defines ToastEffect as FlipXDownIn.
        /// </summary>
        FlipXDownIn,

        /// <summary>
        /// Defines ToastEffect as FlipXDownOut.
        /// </summary>
        FlipXDownOut,

        /// <summary>
        /// Defines ToastEffect as FlipXUpIn.
        /// </summary>
        FlipXUpIn,

        /// <summary>
        /// Defines ToastEffect as FlipXUpOut.
        /// </summary>
        FlipXUpOut,

        /// <summary>
        /// Defines ToastEffect as FlipYLeftIn.
        /// </summary>
        FlipYLeftIn,

        /// <summary>
        /// Defines ToastEffect as FlipYLeftOut.
        /// </summary>
        FlipYLeftOut,

        /// <summary>
        /// Defines ToastEffect as FlipYRightIn.
        /// </summary>
        FlipYRightIn,

        /// <summary>
        /// Defines ToastEffect as FlipYRightOut.
        /// </summary>
        FlipYRightOut,

        /// <summary>
        /// Defines ToastEffect as SlideBottomIn.
        /// </summary>
        SlideBottomIn,

        /// <summary>
        /// Defines ToastEffect as SlideBottomOut.
        /// </summary>
        SlideBottomOut,

        /// <summary>
        /// Defines ToastEffect as SlideDown.
        /// </summary>
        SlideDown,

        /// <summary>
        /// Defines ToastEffect as SlideLeft.
        /// </summary>
        SlideLeft,

        /// <summary>
        /// Defines ToastEffect as SlideLeftIn.
        /// </summary>
        SlideLeftIn,

        /// <summary>
        /// Defines ToastEffect as SlideLeftOut.
        /// </summary>
        SlideLeftOut,

        /// <summary>
        /// Defines ToastEffect as SlideRight.
        /// </summary>
        SlideRight,

        /// <summary>
        /// Defines ToastEffect as SlideRightIn.
        /// </summary>
        SlideRightIn,

        /// <summary>
        /// Defines ToastEffect as SlideRightOut.
        /// </summary>
        SlideRightOut,

        /// <summary>
        /// Defines ToastEffect as SlideTopIn.
        /// </summary>
        SlideTopIn,

        /// <summary>
        /// Defines ToastEffect as SlideTopOut.
        /// </summary>
        SlideTopOut,

        /// <summary>
        /// Defines ToastEffect as SlideUp.
        /// </summary>
        SlideUp,

        /// <summary>
        /// Defines ToastEffect as ZoomIn.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// Defines ToastEffect as ZoomOut.
        /// </summary>
        ZoomOut,

        /// <summary>
        /// Defines ToastEffect as None.
        /// </summary>
        None
    }

    /// <summary>
    /// Specifies the ProgressBar direction types.
    /// </summary>
    public enum ProgressDirection
    {
        /// <summary>
        /// Defines progressDirection as RTL.
        /// </summary>
        RTL,

        /// <summary>
        /// Defines progressDirection as LTR.
        /// </summary>
        LTR
    }
}