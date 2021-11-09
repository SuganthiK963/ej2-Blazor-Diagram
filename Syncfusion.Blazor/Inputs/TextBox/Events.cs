using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The TextBox is an input element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextBox : SfInputTextBase<string>
    {
        /// <summary>
        /// Triggers when the TextBox has focus-out.
        /// </summary>
        [Parameter]
        public EventCallback<FocusOutEventArgs> Blur { get; set; }

        /// <summary>
        /// Triggers when the content of TextBox has changed and gets focus-out.
        /// </summary>
        [Parameter]
        public EventCallback<ChangedEventArgs> ValueChange { get; set; }

        /// <summary>
        /// Triggers when the TextBox component is created.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the TextBox component is destroyed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the TextBox gets focus.
        /// </summary>
        [Parameter]
        public EventCallback<FocusInEventArgs> Focus { get; set; }

        /// <summary>
        /// Triggers each time when the value of TextBox has changed.
        /// </summary>
        [Parameter]
        public EventCallback<InputEventArgs> Input { get; set; }
    }
}