using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Toast is a small, nonblocking notification pop-up and it is shown to users with readable message content
    /// at the bottom of the screen or at a specific target and disappears automatically after a few seconds (time-out)
    /// with different animation effects.
    /// </summary>
    public partial class SfToast : SfBaseComponent
    {
        /// <summary>
        /// Specifies the animation configuration settings for showing and hiding the Toast.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used. Use ToastAnimationSettings tag helper to configure the animation for showing and hiding the Toast")]
        [Parameter]
        public ToastAnimationSettings Animation
        {
            get { return AnimationValue; }
            set { AnimationValue = value; }
        }

        internal ToastAnimationSettings AnimationValue { get; set; }

        /// <summary>
        /// Specifies the content to be displayed on the Toast.
        /// Accepts selectors, string values and HTML elements.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Defines single/multiple classes (separated by space) to be used for customization of Toast.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the Toast display time duration after interacting with the Toast.
        /// </summary>
        [Parameter]
        public int ExtendedTimeout { get; set; } = 1000;

        /// <summary>
        /// Specifies the height of the Toast in pixels/number/percentage. Number value is considered as pixels.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// Defines CSS classes to specify an icon for the Toast which is to be displayed at top left corner of the Toast.
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Overrides the global culture and localization value for this component. Default global culture is 'en-US'.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the newly created Toast message display order while multiple toast's are added to page one after another.
        /// By default, newly added Toast will be added after old Toast's.
        /// </summary>
        [Parameter]
        public bool NewestOnTop { get; set; } = true;

        /// <summary>
        /// Specifies the position of the Toast message to be displayed within target container.
        /// In the case of multiple Toast display, new Toast position will not update on dynamic change of property values
        /// until the old Toast messages removed.
        /// X values are: Left , Right ,Center.
        /// Y values are: Top , Bottom.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used. Use ToastPosition tag helper to configure the position for toast message.")]
        [Parameter]
        public ToastPosition Position
        {
            get { return PositionValue; }
            set { PositionValue = value; }
        }

        internal ToastPosition PositionValue { get; set; }

        /// <summary>
        ///  Specifies the direction for the Toast progressBar.
        /// </summary>
        [Parameter]
        public ProgressDirection ProgressDirection { get; set; }

        /// <summary>
        /// Specifies whether to show the close button in Toast message to close the Toast.
        /// </summary>
        [Parameter]
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Specifies whether to show the progress bar to denote the Toast message display timeout.
        /// </summary>
        [Parameter]
        public bool ShowProgressBar { get; set; }

        /// <summary>
        /// Specifies the target container where the Toast to be displayed.
        /// Based on the target, the positions such as `Left`, `Top` will be applied to the Toast.
        /// The default value is null, which refers the `document.body` element.
        /// </summary>
        [Parameter]
        public string Target { get; set; }

        /// <summary>
        /// Specifies the HTML element/element ID as a string that can be displayed as a Toast content.
        /// </summary>
        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Specifies the Toast display time duration on the page in milliseconds.
        /// - Once the time expires, Toast message will be removed.
        /// - Setting 0 as a time out value displays the Toast on the page until the user closes it manually.
        /// </summary>
        [Parameter]
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// Specifies the title to be displayed on the Toast.
        /// Works only with string values.
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Specifies the width of the Toast in pixels/numbers/percentage. Number value is considered as pixels.
        /// In mobile devices, default width is considered as `100%`.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "300px";

        /// <summary>
        /// Specifies the collection of Toast action `buttons` to be rendered with the given
        /// Button model properties and its click action handler.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used. Use ToastButtons tag helper to configure the collection of Toast action `buttons`.")]
        [Parameter]
        public List<ToastButton> ActionButtons
        {
            get { return ActionButtonsValue; }
            set { ActionButtonsValue = value; }
        }

        internal List<ToastButton> ActionButtonsValue { get; set; }
    }
}