using System;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// The MaskedTextBox is an input element that allows to get input from the user.
    /// </summary>
    public partial class SfMaskedTextBox : SfBaseComponent
    {
        /// <summary>
        /// Returns the value of MaskedTextBox with respective mask.
        /// </summary>
        /// <returns>The value with mask literals.</returns>
        public string GetMaskedValue()
        {
            return MaskedValue;
        }

        /// <summary>
        /// Sets the focus to SfMaskedTextBox component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.MaskedTextBox.focusIn", new object[] { InputBaseObj.InputElement });
        }
        /// <summary>
        /// Sets the focus to SfMaskedTextBox component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }

        /// <summary>
        /// Remove the focus from SfMaskedTextBox component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.MaskedTextBox.focusOut", new object[] { InputBaseObj.InputElement });
        }
        /// <summary>
        /// Remove the focus from SfMaskedTextBox component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<string> GetPersistData()
        {
            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
        }
        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<String> GetPersistDataAsync()
        {
            return await GetPersistData();
        }

        /// <summary>
        /// Gets the clipboard values and its related properties from client.
        /// </summary>
        /// <param name="args">Specifies the mask client properties.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task<Dictionary<string, object>> UpdatePasteValue(MaskClientProps args)
        {
            if (args != null)
            {
                IsMultipleDelete = args.IsMultipleDelete;
                SelectionStart = args.SelectionStart;
                SelectionEnd = args.SelectionEnd;
                await PasteHandler(args);
            }

            return new Dictionary<string, object> { { "inputValue", MaskedValue }, { "cursorPosition", SelectionStart } };
        }
    }
}