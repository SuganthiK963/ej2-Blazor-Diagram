namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Toast is a small, nonblocking notification pop-up and it is shown to users with readable message content
    /// at the bottom of the screen or at a specific target and disappears automatically after a few seconds (time-out)
    /// with different animation effects.
    /// </summary>
    public partial class SfToast : SfBaseComponent
    {
        #region Common string constants
        private const string WRAPPER_CLASS = "e-control e-toast e-lib e-toast-container";
        private const string AUTO = "auto";
        private const string PERCENTAGE = "%";
        private const string PX = "px";
        private const string LEFT = "left:";
        private const string TOP = "top:";
        private const string TOAST_PREFIX = "e-toast-";
        private const string ELEMENT = "element";
        private const string NEWEST_ON_TOP = "newestOnTop";
        private const string TARGET = "target";
        private const string EFFECT = "effect";
        private const string DURATION = "duration";
        private const string EASING = "easing";
        private const string SHOW_ANIMATION = "showAnimation";
        private const string HIDE_ANIMATION = "hideAnimation";
        private const string FADE_IN = "FadeIn";
        private const string FADE_OUT = "FadeOut";
        private const string EASE = "ease";
        private int DEFAULT_ANIMATION_TIME = 600;
        private const string BODY = "body";
        private const string POSITION_FIXED = "position: fixed;";
        private const string POSITION_ABSOLUTE = "position: absolute;";
        private const string RTL = " e-rtl";
        private const string POS_TOP = "Top";
        private const string POS_LEFT = "Left";
        private const string TYPE = "type";
        private const string BTN = "button";
        private const string TITLE = "title";
        private const string TOAST_CLOSE = "Toast_Close";
        private const string CLOSE = "Close";
        #endregion

        #region JS invoke method string constants
        private const string SF_BLAZOR = "sfBlazor.";
        private const string SF_BLAZOR_TOAST = SF_BLAZOR + "Toast.";
        private const string JS_HIDE = SF_BLAZOR_TOAST + "hide";
        private const string JS_INITIALIZE = SF_BLAZOR_TOAST + "initialize";
        private const string JS_DESTROY = SF_BLAZOR_TOAST + "destroy";
        private const string JS_HIDE_TOAST_ANIMATION = SF_BLAZOR_TOAST + "hideAnimationToast";
        #endregion
    }
}
