using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Notifications.Internal;
using System.Threading;
using System;
using System.Globalization;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Toast is a small, nonblocking notification pop-up and it is shown to users with readable message content
    /// at the bottom of the screen or at a specific target and disappears automatically after a few seconds (time-out)
    /// with different animation effects.
    /// </summary>
    public partial class SfToast : SfBaseComponent
    {
        private bool isShowToast;

        private string toastPosition;

        private int toastCount;

        private string positionStyle;

        private string positionClass;

        private string rtlClass;

        private RenderFragment titleTemplate;

        private RenderFragment contentTemplate;

        private RenderFragment templates;

        #region Internal variables
        internal List<ToastShowModel> ShowModels { get; set; } = new List<ToastShowModel>();

        internal List<Timer> TimeoutTimer { get; set; } = new List<Timer>();

        internal List<Timer> ProgressBarTimer { get; set; } = new List<Timer>();

        internal List<bool> ArgsCancelList { get; set; } = new List<bool>();

        internal List<int> Toastindex { get; set; } = new List<int>();

        internal List<ToastProgressBar> ProgressObj { get; set; } = new List<ToastProgressBar>();

        internal Dictionary<string, object> CloseIconAttributes { get; set; } = new Dictionary<string, object>();

        internal ToastEvents Delegates { get; set; }

        internal ElementReference ToastElement { get; set; }

        internal ElementReference CurrentToast { get; set; }

        internal string FullWidth { get; set; }
        #endregion

        #region Element/Module reference

        /// <summary>
        /// Specifies the unique identifier.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <summary>
        /// Sets the content of the Toast.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        #endregion

        private static bool IsNumberValue(string positionValue, bool checkNumber)
        {
            return positionValue == AUTO || positionValue.Contains(PERCENTAGE, StringComparison.Ordinal) || positionValue.Contains(PX, StringComparison.Ordinal) || (checkNumber && float.TryParse(positionValue, out _));
        }

        internal static string FormatUnit(string posValue)
        {
            if (IsNumberValue(posValue, false))
            {
                return posValue;
            }

            return posValue + PX;
        }

        #region Internal methods
        internal void UpdateAnimation(ToastAnimationSettings animation)
        {
            AnimationValue = animation == null ? new ToastAnimationSettings() : animation;
        }

        internal void UpdatePosition(ToastPosition position)
        {
            PositionValue = (position == null) ? new ToastPosition() : position;
        }

        internal void UpdateTemplate(RenderFragment title, RenderFragment content, RenderFragment template)
        {
            titleTemplate = title;
            contentTemplate = content;
            templates = template;
        }

        private void SetPosition()
        {

            string x = PositionValue != null && !string.IsNullOrEmpty(PositionValue.X) ? PositionValue.X : POS_LEFT;
            string y = PositionValue != null && !string.IsNullOrEmpty(PositionValue.Y) ? PositionValue.Y : POS_TOP;

            bool isNumX = IsNumberValue(x, true);
            bool isNumY = IsNumberValue(y, true);
            string low_X = Convert.ToString(x, CultureInfo.InvariantCulture).ToLower(CultureInfo.CurrentCulture);
            string low_Y = Convert.ToString(y, CultureInfo.InvariantCulture).ToLower(CultureInfo.CurrentCulture);
            if (isNumX && isNumY)
            {
                positionStyle = LEFT + FormatUnit(x) + ";" + TOP + FormatUnit(y) + ";";
                positionClass = string.Empty;
            }
            else if (!isNumX && isNumY)
            {
                positionStyle = TOP + FormatUnit(y) + ";";
                positionClass = TOAST_PREFIX + low_X;
            }
            else if (isNumX && !isNumY)
            {
                positionStyle = LEFT + FormatUnit(x) + ";";
                positionClass = TOAST_PREFIX + low_Y;
            }
            else
            {
                positionStyle = string.Empty;
                positionClass = TOAST_PREFIX + low_Y + "-" + low_X;
            }
        }

        private ToastShowModel CreateToast(bool isModel, ToastModel model, SfToast toast)
        {
            ToastShowModel toastModel = new ToastShowModel
            {
                Content = isModel ? model.Content : toast.Content,
                Title = isModel ? model.Title : toast.Title,
                Timeout = isModel ? model.Timeout : toast.Timeout,
                CssClass = isModel ? model.CssClass : toast.CssClass,
                ChildContent = ChildContent,
                TitleTemplate = titleTemplate,
                ContentTemplate = isModel ? model.ContentTemplate : contentTemplate,
                Templates = templates,
                Animation = AnimationValue,
                Position = PositionValue,
                Locale = isModel ? model.Locale : toast.Locale,
                ProgressDirection = isModel ? model.ProgressDirection : toast.ProgressDirection,
                EnableRtl = isModel ? model.EnableRtl : toast.EnableRtl ? toast.EnableRtl : SyncfusionService.options.EnableRtl,
                ExtendedTimeout = isModel ? model.ExtendedTimeout : toast.ExtendedTimeout,
                Height = isModel ? model.Height : toast.Height,
                Icon = isModel ? model.Icon : toast.Icon,
                NewestOnTop = isModel ? model.NewestOnTop : toast.NewestOnTop,
                ShowCloseButton = isModel ? model.ShowCloseButton : toast.ShowCloseButton,
                ShowProgressBar = isModel ? model.ShowProgressBar : toast.ShowProgressBar,
                Target = isModel ? model.Target : toast.Target,
                Width = isModel ? model.Width : toast.Width,
                Key = isModel ? model.Key : 0,
                IsRendered = false
            };
            return toastModel;
        }

        internal void UpdateActionButtons(List<ToastButton> buttons)
        {
            ActionButtonsValue = buttons;
        }

        internal void UpdateLocale()
        {
            CloseIconAttributes.Clear();
            CloseIconAttributes.Add(TYPE, BTN);
            CloseIconAttributes.Add(TITLE, Localizer.GetText(TOAST_CLOSE) ?? CLOSE);
        }
        #endregion

        #region Event handler methods

        /// <summary>
        /// Method invoked when the component has been created.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task CreatedEvent()
        {
            await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, null);
        }

        /// <summary>
        /// Method invoked when the component has been created.
        /// </summary>
        /// <param name="index">Specifies the current toast index.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ClearTimeout(int index)
        {
            ProgressBarTimer[index]?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            ProgressBarTimer[index] = null;
        }

        /// <summary>
        /// Method invoked before toast open.
        /// </summary>
        /// <param name="index">Specifies the current toast index.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task BeforeOpenEvent(int index)
        {
            ToastBeforeOpenArgs eventArgs = new ToastBeforeOpenArgs()
            {
                Cancel = false,
                Element = new DOM() { ID = ShowModels[index].ID },
                Options = ShowModels[index]
            };
            await SfBaseUtils.InvokeEvent<ToastBeforeOpenArgs>(Delegates?.OnOpen, eventArgs);
            ArgsCancelList.Insert(index, eventArgs.Cancel);
        }

        /// <summary>
        /// Method invoked after toast opened.
        /// </summary>
        /// <param name="index">Specifies the current toast index.</param>
        /// <param name="element">Specifies the current toast element.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OpenEvent(int index, DOM element)
        {
            ToastOpenArgs eventArgs = new ToastOpenArgs()
            {
                Element = element,
                Options = ShowModels[index],
                Key = ShowModels[index].Key
            };
            await SfBaseUtils.InvokeEvent<ToastOpenArgs>(Delegates?.Opened, eventArgs);
        }

        /// <summary>
        /// Method invoked after toast closed.
        /// </summary>
        /// <param name="index">Specifies the current toast index.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task CloseEvent(int index)
        {
            ToastCloseArgs eventArgs = new ToastCloseArgs()
            {
                ToastContainer = new DOM() { ID = ID },
                Key = ShowModels[index].Key
            };
            await SfBaseUtils.InvokeEvent<ToastCloseArgs>(Delegates?.Closed, eventArgs);
            await SfBaseUtils.InvokeEvent<ToastCloseArgs>(Delegates?.Closed, eventArgs);
            ShowModels[index].IsRendered = false;
        }

        /// <summary>
        /// Method invoked after toast clicked.
        /// </summary>
        /// <param name="id">Specifies the current toast id.</param>
        /// <param name="element">Specifies the current toast element.</param>
        /// <param name="isCloseIcon">Specifies whether close icon is clicked or not.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClickEvent(int id, ElementReference element, bool isCloseIcon)
        {
            if (ShowModels.Count > 0 && ShowModels[id].IsRendered)
            {
                ToastClickEventArgs eventArgs = new ToastClickEventArgs()
                {
                    Cancel = false,
                    ClickToClose = false,
                    Options = ShowModels[id]
                };
                await SfBaseUtils.InvokeEvent<ToastClickEventArgs>(Delegates?.OnClick, eventArgs);
                if ((isCloseIcon && !eventArgs.Cancel) || eventArgs.ClickToClose)
                {
                    await InvokeMethod(JS_HIDE, ToastElement, element, GetInstance(), "click");
                }
            }
        }

        /// <summary>
        /// Method invoked before toast close.
        /// </summary>
        /// <param name="id">Specifies the current toast id.</param>
        /// <param name="interactionType">Specifies the type of the interaction.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnCloseEvent(int id, string interactionType)
        {
            if (ShowModels[id].IsRendered)
            {
                ToastBeforeCloseArgs eventArgs = new ToastBeforeCloseArgs()
                {
                    Cancel = false,
                    RequestType = interactionType,
                    Element = new DOM() { ID = ShowModels[id].ID },
                    Key = ShowModels[id].Key,
                    Options = ShowModels[id]
                };
                await SfBaseUtils.InvokeEvent<ToastBeforeCloseArgs>(Delegates?.OnClose, eventArgs);
                if (!eventArgs.Cancel)
                {
                    ShowModels[id].IsRendered = false;
                    await InvokeMethod(JS_HIDE_TOAST_ANIMATION, ToastElement, id);
                }
            }
        }

        /// <summary>
        /// Method invoked when hover the toast element.
        /// </summary>
        /// <param name="id">Specifies the current toast id.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void MouseoverEvent(int id)
        {
            DestroyTimer(id);
        }

        /// <summary>
        /// Method invoked when hover the toast element.
        /// </summary>
        /// <param name="id">Specifies the current toast id.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void DestroyTimer(int id)
        {
            if (TimeoutTimer.Count > id && TimeoutTimer[id] != null)
            {
                TimeoutTimer[id].Dispose();
                TimeoutTimer[id] = null;
            }

            if (ShowProgressBar && TimeoutTimer.Count > id && ProgressBarTimer[id] != null)
            {
                ProgressBarTimer[id].Dispose();
                ProgressBarTimer[id] = null;
            }
        }
        #endregion
    }
}