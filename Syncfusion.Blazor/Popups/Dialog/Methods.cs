using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;

namespace Syncfusion.Blazor.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        /// <summary>
        /// Returns the dialog button instances by index.
        /// Based on that, you can dynamically change the button states.
        /// </summary>
        /// <param name="index">Specifies the button index.</param>
        /// <returns>Returns the dialog button instances by index.</returns>
        public DialogButton GetButton(int index)
        {
            if (ButtonsValue != null)
            {
                return ButtonsValue[index];
            }

            return null;
        }

        /// <summary>
        /// Returns all the dialog button’s instances which are rendered in the dialog’s footer.
        /// Based on that, you can dynamically change the buttons states.
        /// </summary>
        /// <returns>Returns all the dialog button’s instances.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
        public List<DialogButton> GetButtonItems()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            List<DialogButton> button = ButtonsValue;
            return button;
        }

        /// <summary>
        /// Overloaded.
        /// Closes the dialog if it is in a visible state.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync()
        {
            await HideDialog(null);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide()
        {
            await HideDialog(null);
        }

        /// <summary>
        /// Overloaded.
        /// Closes the dialog if it is in a visible state.
        /// </summary>
        /// <param name="args">Specifies the interaction type.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync(string args = null)
        {
            await HideDialog(args, null);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(string args = null)
        {
            await HideDialog(args, null);
        }

        /// <summary>
        /// Overloaded.
        /// Closes the dialog if it is in a visible state.
        /// </summary>
        /// <param name="args">Specifies the interaction type.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync(MouseEventArgs args)
        {
            await HideDialog(null, new BeforeCloseEventArgs() { Event = args });
        }


        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(MouseEventArgs args)
        {
            await HideDialog(null, new BeforeCloseEventArgs() { Event = args });
        }

        /// <summary>
        /// Overloaded.
        /// Closes the dialog if it is in a visible state.
        /// </summary>
        /// <param name="args">Specifies the keyboard arguments.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync(KeyboardEventArgs args)
        {
            await HideDialog(null, new BeforeCloseEventArgs() { Event = args });
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(KeyboardEventArgs args)
        {
            await HideDialog(null, new BeforeCloseEventArgs() { Event = args });
        }

        /// <summary>
        /// Refreshes the dialog's position when the user changes its height and width dynamically.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task RefreshPositionAsync()
        {
            await InvokeMethod(JS_REFRESH_POSITION, GetElementRef());
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RefreshPosition()
        {
            await InvokeMethod(JS_REFRESH_POSITION, GetElementRef());
        }

        /// <summary>
        /// Opens the dialog if it is in a hidden state.
        /// To open the dialog with full-screen width, set the parameter as true.
        /// </summary>
        /// <param name="isFullScreen">Specifies dialog will open on full screen or not.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task ShowAsync(bool? isFullScreen = null)
        {
            await ShowDialog(isFullScreen);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Show(bool? isFullScreen = null)
        {
            await ShowDialog(isFullScreen);
        }
    }
}