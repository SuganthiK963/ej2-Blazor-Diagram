using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The MaskedTextBox is an input element that allows to get input from the user.
    /// </summary>
    public partial class SfMaskedTextBox : SfBaseComponent
    {
        /// <summary>
        /// Triggers when the SfMaskedTextBox has focus-out.
        /// </summary>
        [Parameter]
        public EventCallback<MaskBlurEventArgs> Blur { get; set; }

        /// <summary>
        /// Triggers when the content of SfMaskedTextBox has changed and gets focus-out.
        /// </summary>
        [Parameter]
        public EventCallback<MaskChangeEventArgs> ValueChange { get; set; }

        /// <summary>
        /// Triggers when the SfMaskedTextBox component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the SfMaskedTextBox component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the SfMaskedTextBox gets focus.
        /// </summary>
        [Parameter]
        public EventCallback<MaskFocusEventArgs> Focus { get; set; }

        /// <summary>
        /// Triggers when the content of input has changed and gets focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnChange { get; set; }

        /// <summary>
        /// Triggers each time when the value of input has changed.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<ChangeEventArgs> OnInput { get; set; }

        /// <summary>
        /// Triggers when the input has focus-out.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<Microsoft.AspNetCore.Components.Web.FocusEventArgs> OnFocus { get; set; }
    }
}