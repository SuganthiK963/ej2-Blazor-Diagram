using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        private bool allowDragging;

        private bool closeOnEscape;

        private string cssClass;

        private bool enableResize;

        private bool enableRtl;

        private double zIndex;

        private string width;

        private bool visible;

        private string target;

        private string minHeight;

        private bool isModal;

        private string height;

        private bool isLoadOnDemand;

        /// <summary>
        /// Specifies the unique identifier.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Sets content for the Dialog element including HTML support and its customizations.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the value whether the dialog component can be dragged by the end-user.
        /// The dialog allows a user to drag by selecting the header and dragging it for re-positioning the dialog.
        /// </summary>
        [Parameter]
        public bool AllowDragging { get; set; }

        /// <summary>
        /// Specifies the animation settings of the dialog component.
        /// The animation effect can be applied to open and close the dialog with duration and delay.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used. Use DialogAnimationSettings tag helper to configure the animation for Dialog")]
        [Parameter]
        public DialogAnimationSettings AnimationSettings
        {
            get { return AnimationSettingsValue; }
            set { AnimationSettingsValue = value; }
        }

        internal DialogAnimationSettings AnimationSettingsValue { get; set; }

        /// <summary>
        /// Configures the action `Buttons` that contains button properties with primary attributes and the click events.
        /// One or more action buttons can be configured to the dialog.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used. Use DialogButtons tag helper to configure the collection of Dialog action `buttons`.")]
        [Parameter]
        public List<DialogButton> Buttons
        {
            get { return ButtonsValue; }
            set { ButtonsValue = value; }
        }

        internal List<DialogButton> ButtonsValue { get; set; }

        /// <summary>
        /// Specifies the Boolean value whether the dialog can be closed on pressing the escape (ESC) key.
        /// that is used to control the dialog's closing behavior.
        /// </summary>
        [Parameter]
        public bool CloseOnEscape { get; set; } = true;

        /// <summary>
        /// Specifies the value that can be displayed in the dialog's content section.
        /// </summary>
        [Parameter]
        public string Content { get; set; }

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the dialog.
        /// One or more custom CSS classes can be added to a dialog.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies the value whether the dialog component can be resized by the end-user.
        /// If the enableResize is true, the dialog component creates a grip to resize it in a diagonal direction.
        /// </summary>
        [Parameter]
        public bool EnableResize { get; set; }

        /// <summary>
        /// Specifies the resize handles direction in the dialog component that can be resized by the end-user.
        /// </summary>
        [Parameter]
        public ResizeDirection[] ResizeHandles { get; set; } = new ResizeDirection[] { ResizeDirection.SouthEast };

        /// <summary>
        /// Enable or disable rendering component in the right to left (RTL) direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the template value that can be displayed with the dialog's footer area.
        /// This is an optional property and can be used only when the footer is occupied with information or custom components.
        /// By default, the footer is configured with the action buttons.
        /// If the footer template is configured to the dialog, the buttons property will be disabled.
        /// </summary>
        [Parameter]
        public string FooterTemplate { get; set; }

        /// <summary>
        /// Specifies the value that can be displayed in the dialog's title area that can be configured with a plain text.
        /// The dialog will be displayed without the header if the header property is null.
        /// </summary>
        [Parameter]
        public string Header { get; set; }

        /// <summary>
        /// Specifies the height of the dialog component.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// You can add the additional Html attributes such as id, title, etc., to the dialog element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [Obsolete("This property is deprecated. Use @attributes to set additional attributes for dialog element.")]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return HtmlAttributesValue; }
            set { HtmlAttributesValue = value; }
        }

        internal Dictionary<string, object> HtmlAttributesValue { get; set; }

        /// <summary>
        /// Specifies the Boolean value whether the dialog can be displayed as modal or non-modal.
        /// `Modal`: It creates an overlay that disables interaction with the parent application and
        ///  the user who should respond with modal before continuing with other applications.
        /// `Modeless`: It does not prevent user interaction with the parent application.
        /// </summary>
        [Parameter]
        public bool IsModal { get; set; }

        /// <summary>
        /// Overrides the global culture and localization value for this component. Default global culture is 'en-US'.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the min-height of the dialog component.
        /// </summary>
        [Parameter]
        public string MinHeight { get; set; }

        /// <summary>
        /// Specifies the value where the dialog can be positioned within the document or target.
        /// The position can be represented with pre-configured positions or specific X and Y values.
        /// `X value`: left, center, right, or offset value.
        /// `Y value`: top, center, bottom, or offset value.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used. Use DialogPositionData tag helper to configure the position for Dialog")]
        [Parameter]
        public DialogPositionData Position
        {
            get { return PositionValue; }
            set { PositionValue = value; }
        }

        internal DialogPositionData PositionValue { get; set; }

        /// <summary>
        /// Specifies the value that represents whether the close icon can be shown in the dialog’s title section.
        /// </summary>
        [Parameter]
        public bool ShowCloseIcon { get; set; }

        /// <summary>
        /// Specifies the target element in which the dialog should be displayed.
        /// The default value is null, which refers to the `Document.body` element.
        /// </summary>
        [Parameter]
        public string Target { get; set; }

        /// <summary>
        /// Specifies the value that represents whether the dialog component is visible.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets a callback of the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary>
        /// Specifies the width of the dialog.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        /// <summary>
        /// Specifies the z-order for rendering that determines whether the dialog is displayed
        /// in front or behind of another component.
        /// </summary>
        [Parameter]
        public double ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specifies the value that represents whether the Dialog element re-render or not.
        /// </summary>
        [Parameter]
        public bool IsLoadOnDemand { get; set; }
    }
}