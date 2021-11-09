using System;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Specifies the event arguments of Toast before open.
    /// </summary>
    public class ToastBeforeOpenArgs
    {
        /// <summary>
        /// Defines the prevent action for before opening toast.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the Toast element.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Defines current Toast model properties as options.
        /// </summary>
        public ToastModel Options { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments of Toast before close.
    /// </summary>
    public class ToastBeforeCloseArgs
    {
        /// <summary>
        /// Defines the prevent action for before closing toast.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the Toast element.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Defines the current Toast Key.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Defines the current Toast is interacted or not.
        /// </summary>
        public string RequestType { get; set; }

        /// <summary>
        /// Defines current Toast model properties as options.
        /// </summary>
        public ToastModel Options { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments of Toast click.
    /// </summary>
    public class ToastClickEventArgs
    {
        /// <summary>
        /// Defines the prevent action for Toast click event.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the close action for click or tab on the Toast.
        /// </summary>
        public bool ClickToClose { get; set; }

        /// <summary>
        /// Defines the Toast element.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Defines current Toast model properties as options.
        /// </summary>
        public ToastModel Options { get; set; }

    }

    /// <summary>
    /// Specifies the event arguments of Toast close.
    /// </summary>
    public class ToastCloseArgs
    {
        /// <summary>
        /// Defines current Toast model properties as options.
        /// </summary>
        public ToastModel Options { get; set; }

        /// <summary>
        /// Defines the Toast container element.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM ToastContainer { get; set; }

        /// <summary>
        /// Defines the current Toast Key.
        /// </summary>
        public int Key { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments of Toast open.
    /// </summary>
    public class ToastOpenArgs
    {
        /// <summary>
        /// Defines the Toast element.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Defines current Toast model properties as options.
        /// </summary>
        public ToastModel Options { get; set; }

        /// <summary>
        /// Defines current Toast Key.
        /// </summary>
        public int Key { get; set; }
    }

    /// <summary>
    /// Interface for a class ToastAnimationSettings.
    /// </summary>
    public class ToastAnimationSettingsModel
    {
        /// <summary>
        /// Specifies the animation to appear while hiding the Toast.
        /// </summary>
        public ToastAnimationsModel Hide { get; set; }

        /// <summary>
        /// Specifies the animation to appear while showing the Toast.
        /// </summary>
        public ToastAnimationsModel Show { get; set; }
    }

    /// <summary>
    /// Interface for a class ToastAnimations.
    /// </summary>
    public class ToastAnimationsModel
    {
        /// <summary>
        /// Specifies the duration to animate.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Specifies the animation timing function.
        /// </summary>
        public ToastEasing Easing { get; set; }

        /// <summary>
        /// Specifies the animation name that should be applied on while opening and closing the toast.
        /// If the user sets Fade animation, the toast will open with the `FadeIn` effect and close with the `FadeOut` effect.
        /// The following are the list of animation effects available to configure to the toast:
        /// 1. Fade
        /// 2. FadeZoom
        /// 3. FlipLeftDown
        /// 4. FlipLeftUp
        /// 5. FlipRightDown
        /// 6. FlipRightUp
        /// 7. FlipXDown
        /// 8. FlipXUp
        /// 9. FlipYLeft
        /// 10. FlipYRight
        /// 11. SlideBottom
        /// 12. SlideLeft
        /// 13. SlideRight
        /// 14. SlideTop
        /// 15. Zoom
        /// 16. None.
        /// </summary>
        public ToastEffect Effect { get; set; }
    }

    /// <summary>
    /// Interface for a class Toast.
    /// </summary>
    public class ToastModel
    {
        /// <summary>
        /// Specifies the animation configuration settings for showing and hiding the Toast.
        /// </summary>
        public ToastAnimationSettings Animation { get; set; }

        /// <summary>
        /// Specifies the content to be displayed on the Toast.
        /// Accepts selectors, string values and HTML elements.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Defines single/multiple classes (separated by space) to be used for customization of Toast.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the Toast display time duration after interacting with the Toast.
        /// </summary>
        public int ExtendedTimeout { get; set; } = 1000;

        /// <summary>
        /// Specifies the height of the Toast in pixels/number/percentage. Number value is considered as pixels.
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Defines CSS classes to specify an icon for the Toast which is to be displayed at top left corner of the Toast.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Overrides the global culture and localization value for this component. Default global culture is 'en-US'.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the newly created Toast message display order while multiple toast's are added to page one after another.
        /// By default, newly added Toast will be added after old Toast's.
        /// </summary>
        public bool NewestOnTop { get; set; } = true;

        /// <summary>
        /// Specifies the position of the Toast message to be displayed within target container.
        /// In the case of multiple Toast display, new Toast position will not update on dynamic change of property values
        /// until the old Toast messages removed.
        /// X values are: Left , Right ,Center
        /// Y values are: Top , Bottom.
        /// </summary>
        public ToastPosition Position { get; set; }

        /// <summary>
        ///  Specifies the direction for the Toast progressBar.
        /// </summary>
        public ProgressDirection ProgressDirection { get; set; }

        /// <summary>
        /// Specifies whether to show the close button in Toast message to close the Toast.
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Specifies whether to show the progress bar to denote the Toast message display timeout.
        /// </summary>
        public bool ShowProgressBar { get; set; }

        /// <summary>
        /// Specifies the target container where the Toast to be displayed.
        /// Based on the target, the positions such as `Left`, `Top` will be applied to the Toast.
        /// The default value is null, which refers the `document.body` element.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Specifies the HTML element/element ID as a string that can be displayed as a Toast content.
        /// </summary>
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Specifies the Toast display time duration on the page in milliseconds.
        /// - Once the time expires, Toast message will be removed.
        /// - Setting 0 as a time out value displays the Toast on the page until the user closes it manually.
        /// </summary>
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// Specifies the title to be displayed on the Toast.
        /// Works only with string values.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Specifies the width of the Toast in pixels/numbers/percentage. Number value is considered as pixels.
        /// In mobile devices, default width is considered as `100%`.
        /// </summary>
        public string Width { get; set; } = "300px;";

        /// <summary>
        /// Defines current Toast Key.
        /// </summary>
        public int Key { get; set; }
    }

    /// <summary>
    /// Interface for a class ToastPosition.
    /// </summary>
    public class ToastPositionModel
    {
        /// <summary>
        /// Specifies the position of the Toast notification with respect to the target container's left edge.
        /// </summary>
        public string X { get; set; }

        /// <summary>
        /// Specifies the position of the Toast notification with respect to the target container's top edge.
        /// </summary>
        public string Y { get; set; }
    }
}