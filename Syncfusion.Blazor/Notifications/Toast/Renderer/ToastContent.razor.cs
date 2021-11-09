using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications.Internal
{
    /// <summary>
    /// Specifies the content to be displayed on the Toast.
    /// </summary>
    public partial class ToastContent : SfBaseComponent
    {
        #region Common string constants
        private const string BTN_DEFAULT_CLS = "e-small e-primary e-flat ";
        private const string ROOT_CLASS = "e-toast e-blazor-toast-hidden";
        private const string ALERT = "alert";
        private const string TOAST_PROGRESS = "e-toast-progress";
        private const string TOAST_ICON = "e-toast-icon e-icons";
        private const string TOAST_MESSAGE = "e-toast-message";
        private const string TOAST_TITLE = "e-toast-title";
        private const string TOAST_CONTENT_CLASS = "e-toast-content";
        private const string TOAST_ACTIONS = "e-toast-actions";
        private const string TOAST_CLOSE_ICON = "e-toast-close-icon e-icons e-blazor-toast-close-icon";
        private const string WIDTH = "width";
        private const string STYLE = "style";
        private const string FULL_WIDTH = "e-toast-full-width";
        private const string ROOT_ELEMENT = "rootElement";
        private const string ELEMENT = "element";
        private const string INDEX = "index";
        private const string TIMEOUT = "timeOut";
        private const string EXTENDED_TIMEOUT = "extendedTimeout";
        private const string NEWEST_ON_TOP = "newestOnTop";
        private const string SHOW_PROGRESS_BAR = "showProgressBar";
        private const string TARGET = "target";
        private const string SHOW_ANIMATION = "showAnimation";
        private const string HIDE_ANIMATION = "hideAnimation";
        private const string HUN_PERCENT = "100%";
        private const string PROGRESS_DIRECTION = "progressDirection";
        private const string JS_SHOW = "sfBlazor.Toast.show";
        private const string TOAST_HEADER_ICON = "e-toast-header-icon";
        private const string TOAST_HEADER_CLOSE_ICON = "e-toast-header-close-icon";
        #endregion

        #region JS invoke method string constants
        private const string JS_HIDE = "sfBlazor.Toast.hide";
        #endregion

        #region Internal variables
        private ElementReference toastContentElement;
        private string toastWidth;
        private string toastHeight;
        private Dictionary<string, object> progressBarAttr = new Dictionary<string, object>();
        private double percentage = 100;
        private int progressTimeout;
        private Dictionary<string, object> buttonAttribute = new Dictionary<string, object>() { { "type", "button" } };
        private int intervalTime = 10;
        private int count;
        private Timer timeoutTimer;
        private bool isOpen;
        private string headerIconClass = string.Empty;
        private string closeIconClass = string.Empty;
        #endregion

        #region Element/Module reference
        [CascadingParameter]
        private SfToast Parent { get; set; }

        [CascadingParameter(Name = "ToastContent")]
        private ToastContentModel ShowModel { get; set; }
        #endregion

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            SetWidthHeight();
            Parent.Toastindex.Add(ShowModel.Model.Key);
            if (!string.IsNullOrEmpty(ShowModel.Model.Height))
            {
                toastHeight = ShowModel.Model.Height;
            }

            string width = $"{WIDTH}: {percentage}%;";
            progressBarAttr.Clear();
            progressBarAttr.Add(STYLE, width);
            if (ShowModel.Model.Icon != null)
            {
                this.headerIconClass = TOAST_HEADER_ICON;
            }
            if (ShowModel.Model.ShowCloseButton)
            {
                this.closeIconClass = TOAST_HEADER_CLOSE_ICON;
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender"> Set to true if this is the first time OnAfterRender(Boolean) has been invoked.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!ShowModel.Model.IsRendered && !isOpen)
            {
                ShowModel.Model.IsRendered = true;
                isOpen = true;
                Parent.CurrentToast = toastContentElement;
                if (!Parent.ArgsCancelList[ShowModel.Index])
                {
                    await InvokeMethod(JS_SHOW, GetInstance());
                }

                if (ShowModel == null) {
                    return;
                }

                ToastProgressBar progressObj = new ToastProgressBar()
                {
                    MaxHideTime = ShowModel.Model.Timeout,
                    HideEstimatedTimeOfArrival = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds + ShowModel.Model.Timeout // Any time since that date is calculated based on the number of seconds elapsed.
                };
                Parent.ProgressObj.Add(progressObj);
                progressTimeout = ShowModel.Model.Timeout > 0 ? ShowModel.Model.Timeout : Timeout.Infinite;
                UpdateProgressBar();
                timeoutTimer = new Timer(new TimerCallback(DetachToast), null, progressTimeout, 0);
                Parent.TimeoutTimer.Insert(ShowModel.Index, timeoutTimer);
            }
        }

        private void UpdateWidth()
        {
            if (Parent == null || Parent.ProgressObj.Count <= ShowModel.Index)
            {
                return;
            }

            double time = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            double percentage = ((Parent.ProgressObj[ShowModel.Index].HideEstimatedTimeOfArrival - time) / Parent.ProgressObj[ShowModel.Index].MaxHideTime) * 100;
            percentage = ShowModel.Model.ProgressDirection == ProgressDirection.LTR ? 100 - percentage : percentage;
            string percentageValue = Convert.ToString(percentage, System.Globalization.CultureInfo.InvariantCulture);
            string width = $"{WIDTH}: {percentageValue}%;";
            progressBarAttr.Clear();
            progressBarAttr.Add(STYLE, width);
            StateHasChanged();
        }

        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> toastObj = new Dictionary<string, object>()
            {
                { ROOT_ELEMENT, Parent.ToastElement },
                { ELEMENT, toastContentElement },
                { INDEX, ShowModel.Index },
                { TIMEOUT, ShowModel.Model.Timeout },
                { EXTENDED_TIMEOUT, ShowModel.Model.ExtendedTimeout },
                { NEWEST_ON_TOP, ShowModel.Model.NewestOnTop },
                { SHOW_PROGRESS_BAR, ShowModel.Model.ShowProgressBar },
                { TARGET, ShowModel.Model.Target },
                { SHOW_ANIMATION, Parent.GetShowAnimation() },
                { HIDE_ANIMATION, Parent.GetHideAnimation() },
                { PROGRESS_DIRECTION, ShowModel.Model.ProgressDirection.ToString() }
            };
            return toastObj;
        }

        private async void CloseButtonClick()
        {
            if (ShowModel != null) {
                Parent?.DestroyTimer(ShowModel.Index);
                await Parent?.ClickEvent(ShowModel.Index, toastContentElement, true);
            }
        }

        private async void DetachToast(object state)
        {
            if (ShowModel != null && ShowModel.Model != null && Parent != null && Parent.ShowModels[ShowModel.Index].IsRendered)
            {
                Parent.DestroyTimer(ShowModel.Index);
                await InvokeMethod(JS_HIDE, Parent.ToastElement, toastContentElement, GetInstance(), string.Empty);
            }
        }

        private void SetTimer(object state)
        {
            if (Parent == null)
            {
                return;
            }

            intervalTime += 10;
            count++;
            if (intervalTime >= progressTimeout)
            {
                Parent.ProgressBarTimer[ShowModel.Index].Dispose();
            }
            else
            {
                InvokeAsync(() => UpdateWidth());
            }
        }

        private async Task ToastClickAsync()
        {
            if (ShowModel != null) {
                await Parent?.ClickEvent(ShowModel.Index, toastContentElement, false);
            }
        }

        internal void UpdateProgressBar()
        {
            if (ShowModel.Model.ShowProgressBar)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, 0);
                TimeSpan timeInterval = new TimeSpan(0, 0, 0, 0, 10);
                Parent.ProgressBarTimer.Insert(ShowModel.Index, null);
                Parent.ProgressBarTimer[ShowModel.Index] = new Timer(new TimerCallback(SetTimer), new { Max = 100 }, timeSpan, timeInterval);
            }
            else
            {
                Parent.ProgressBarTimer.Insert(ShowModel.Index, null);
            }
        }

        private void SetWidthHeight()
        {
            if (ShowModel.Model.Width == HUN_PERCENT)
            {
                Parent.FullWidth = FULL_WIDTH;
            }
            else if (!string.IsNullOrEmpty(ShowModel.Model.Width) && new Regex(@"^[0-9]+$").IsMatch(ShowModel.Model.Width))
            {
                toastWidth = SfToast.FormatUnit(ShowModel.Model.Width);
                Parent.FullWidth = string.Empty;
            }
            else
            {
                toastWidth = ShowModel.Model.Width;
                Parent.FullWidth = string.Empty;
            }

            toastHeight = !string.IsNullOrEmpty(ShowModel.Model.Height) ? SfToast.FormatUnit(ShowModel.Model.Height) : null;
        }

        internal override void ComponentDispose()
        {
            if (timeoutTimer != null)
            {
                timeoutTimer.Dispose();
            }

            progressBarAttr.Clear();
            buttonAttribute.Clear();
            ShowModel = null;
            Parent = null;
        }
    }
}
