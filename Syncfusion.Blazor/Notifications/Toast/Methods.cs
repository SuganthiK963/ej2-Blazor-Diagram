using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications.Internal;

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
        /// To Hide Toast element on a document.
        /// To Hide all toast element when passing 'All'.
        /// </summary>
        /// <param name="element"> Specifies the particular element to Hide.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync(ElementReference element)
        {
            await InvokeMethod(JS_HIDE, ToastElement, element, GetInstance(), string.Empty);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(ElementReference element)
        {
            await InvokeMethod(JS_HIDE, ToastElement, element, GetInstance(), string.Empty);
        }

        /// <summary>
        /// To Hide Toast element on a document.
        /// To Hide all toast element when passing 'All'.
        /// </summary>
        /// <param name="hideAll"> Pass 'All' to hide all Toast.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync(string hideAll)
        {
            await InvokeMethod(JS_HIDE, ToastElement, hideAll, GetInstance(), string.Empty);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(string hideAll)
        {
            await InvokeMethod(JS_HIDE, ToastElement, hideAll, GetInstance(), string.Empty);
        }

        /// <summary>
        /// To Hide Toast element on a document.
        /// To Hide all toast element when passing 'All'.
        /// </summary>
        /// /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync()
        {
            await InvokeMethod(JS_HIDE, ToastElement, null, GetInstance(), string.Empty);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide()
        {
            await InvokeMethod(JS_HIDE, ToastElement, null, GetInstance(), string.Empty);
        }

        /// <summary>
        /// To Hide Toast element on a document.
        /// </summary>
        /// <param name="key"> Specifies the particular toast key to Hide.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task Hide(int key)
        {
            int index = Toastindex.IndexOf(key);
            await InvokeMethod(JS_HIDE, ToastElement, index.ToString(CultureInfo.CurrentCulture), GetInstance(), string.Empty);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task HideAsync(int key)
        {
            int index = Toastindex.IndexOf(key);
            await InvokeMethod(JS_HIDE, ToastElement, index.ToString(CultureInfo.CurrentCulture), GetInstance(), string.Empty);
        }

        /// <summary>
        /// To show Toast element on a document with the relative position.
        /// </summary>
        /// <param name="toastModel"> Specifies the ToastModel to show Toast element on screen.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task ShowAsync(ToastModel toastModel = null)
        {
            isShowToast = true;
            toastCount++;
            SetPosition();
            ToastShowModel model = toastModel == null ? CreateToast(false, null, this) : CreateToast(true, toastModel, null);
            if (ShowModels != null) {
                ShowModels.Add(model);
                await BeforeOpenEvent(ShowModels.Count - 1);
                await InvokeAsync(() => StateHasChanged());
            }
        }


        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Show(ToastModel toastModel = null)
        {
            isShowToast = true;
            toastCount++;
            SetPosition();
            ToastShowModel model = toastModel == null ? CreateToast(false, null, this) : CreateToast(true, toastModel, null);
            if (ShowModels != null)
            {
                ShowModels.Add(model);
                await BeforeOpenEvent(ShowModels.Count - 1);
                await InvokeAsync(() => StateHasChanged());
            }
        }
    }
}