using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Interface for focus out event.
    /// </summary>
    public class MaskBlurEventArgs
    {
        /// <summary>
        /// Returns the MaskedTextBox container element.
        /// </summary>
        public ElementReference Container { get; set; }

        /// <summary>
        /// Returns the original event arguments.
        /// </summary>
        [Obsolete("Event is deprecated and will no longer be used.")]
        public EventArgs Event { get; set; }

        /// <summary>
        /// Returns the value of the MaskedTextBox with the masked format.
        /// </summary>
        public string MaskedValue { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [Obsolete("Name is deprecated and will no longer be used.")]
        public string Name { get; set; }

        /// <summary>
        /// Returns the value of MaskedTextBox.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Interface for changed event.
    /// </summary>
    public class MaskChangeEventArgs : MaskBlurEventArgs
    {
        /// <summary>
        /// Returns true when the value of MaskedTextBox is changed by user interaction. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }
    }

    /// <summary>
    /// Interface for focus event.
    /// </summary>
    public class MaskFocusEventArgs : MaskBlurEventArgs
    {
        /// <summary>
        /// Returns selectionEnd value depends on mask length.
        /// </summary>
        public int SelectionEnd { get; set; }

        /// <summary>
        /// Returns selectionStart value as zero by default.
        /// </summary>
        public int SelectionStart { get; set; }
    }

    /// <summary>
    /// Specifies the client properties of MaskedTextBox component.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MaskClientProps
    {
        /// <summary>
        /// Specifies the read only property.
        /// </summary>
        /// <exclude/>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies wheter the component in disabled state or not.
        /// </summary>
        /// <exclude/>
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies the locale property of the component.
        /// </summary>
        /// <exclude/>
        public string Locale { get; set; }

        /// <summary>
        /// Specifies the selection range end property of the component.
        /// </summary>
        /// <exclude/>
        public int SelectionEnd { get; set; }

        /// <summary>
        /// Specifies the selection start property of the component.
        /// </summary>
        /// <exclude/>
        public int SelectionStart { get; set; }

        /// <summary>
        /// Specifies the value property of the component.
        /// </summary>
        /// <exclude/>
        public string Value { get; set; }

        /// <summary>
        /// Specifies the mask property of the component.
        /// </summary>
        /// <exclude/>
        public string Mask { get; set; }

        /// <summary>
        /// Specifies the key value  provided.
        /// </summary>
        /// <exclude/>
        public string keyValue { get; set; }

        /// <summary>
        /// Specifies whether the single charcater or multiple characters get deleted.
        /// </summary>
        /// <exclude/>
        public bool IsMultipleDelete { get; set; }

        /// <summary>
        /// Specifies the clipboard value.
        /// </summary>
        /// <exclude/>
        public string PasteValue { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the prompt character of the component.
        /// </summary>
        /// <exclude/>
        public string PromptCharacter { get; set; }

        /// <summary>
        /// Specifies the placeholder of the component.
        /// </summary>
        /// <exclude/>
        public string PlaceHolder { get; set; }

        /// <summary>
        /// SPecifies the value with mask literals.
        /// </summary>
        /// <exclude/>
        public string MaskedValue { get; set; }

        /// <summary>
        /// Specifies the floatlabel type of the component.
        /// </summary>
        /// <exclude/>
        public string FloatLabelType { get; set; }

        /// <summary>
        /// Specifies the custom regex.
        /// </summary>
        /// <exclude/>
        public List<string> CustomRegExpCollec { get; set; }

        /// <summary>
        /// Specifies the mask literals with escape sequence and casing characters.
        /// </summary>
        /// <exclude/>
        public string HiddenMask { get; set; }

        /// <summary>
        /// Specifies the mask literals.
        /// </summary>
        /// <exclude/>
        public string PromptMask { get; set; }
    }
}