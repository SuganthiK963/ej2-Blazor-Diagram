using System;
using System.ComponentModel;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Calendars.Internal;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// TimePicker is an intuitive component which provides an options to select a time value from popup list or to set a desired time value.
    /// </summary>
    public partial class SfTimePicker<TValue> : SfInputTextBase<TValue>
    {
        private const string MODEL = "model";
        private const string BODY = "body";
        private const string OPEN = "Open";
        private const string CLOSE = "Close";

        /// <summary>
        /// Sets the focus to the TimePicker component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.TimePicker.focusIn", new object[] { InputElement });
        }
        /// <summary>
        /// Sets the focus to the TimePicker component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
        /// <summary>
        /// Remove the focus from the TimePicker component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Remove the focus from the TimePicker component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.TimePicker.focusOut", new object[] { InputElement });
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
        /// Opens the popup to show the list items.
        /// <returns>Task.</returns>
        /// </summary>
        public async Task ShowPopupAsync()
        {
            await Show();
        }
        /// <summary>
        /// Opens the popup to show the list items.
        /// <param name="args">Specifies the event arguments </param>
        /// <returns>Task.</returns>
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Show(EventArgs args = null)
        {
            if (!((Enabled && Readonly) || !Enabled))
            {
                await GenerateList();
                var openEventArgs = new PopupEventArgs
                {
                    Cancel = false,
                    Event = args,
                    Name = OPEN
                };
                if (!string.IsNullOrEmpty(CurrentValueAsString))
                {
                    UpdateListSelection(CurrentValueAsString, SELECTED);
                }

                PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = args, PreventDefault = false };
                await SfBaseUtils.InvokeEvent<PopupEventArgs>(TimepickerEvents?.OnOpen, openEventArgs);
                if (!openEventArgs.Cancel)
                {
                    IsListRendered = true;
                    await Task.Delay(10);
                    ShowPopupList = true;
                    SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, inputAttr);
                    IsTimeIconClicked = false;
                    await InvokeAsync(() => StateHasChanged());
                }
            }
        }
        /// <summary>
        /// Hides the TimePicker popup.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task HidePopupAsync()
        {
            await Hide();
        }

        /// <summary>
        /// Hides the TimePicker popup.
        /// </summary>
        /// <param name="args">Specifies the event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(EventArgs args = null)
        {
            var closeEventArgs = new PopupEventArgs
            {
                Cancel = false,
                Event = args,
                Name = CLOSE
            };
            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = args, PreventDefault = false };
            await SfBaseUtils.InvokeEvent<PopupEventArgs>(TimepickerEvents?.OnClose, closeEventArgs);
            if (!closeEventArgs.Cancel)
            {
                var options = new TimePickerClientProps<TValue>
                {
                    EnableRtl = EnableRtl,
                    ZIndex = ZIndex,
                    KeyConfigs = KeyConfigs,
                    Value = Value,
                    Width = Width,
                    Step = Step,
                    ScrollTo = ScrollTo
                };
                if (IsDevice)
                {
                    BaseInputAttributes = RemoveAttributes(READ_ONLY, BaseInputAttributes);
                }
                TimeIcon = SfBaseUtils.RemoveClass(TimeIcon, ACTIVE);
                await InvokeMethod("sfBlazor.TimePicker.closePopup", new object[] { InputElement, PopupEventArgs, options });
                IsListRender = false;
            }
        }

        /// <summary>
        /// Shows the TimePicker popup.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> arguments.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ShowPopup(EventArgs args = null)
        {
            await Show(args);
        }

        /// <summary>
        /// Hides the TimePicker popup.
        /// </summary>
        /// <returns>Task.</returns>
        /// <param name="args">The<see cref="EventArgs"></see>/>.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task HidePopup(EventArgs args = null)
        {
            await Hide(args);
        }

        /// <summary>
        /// Invoke the before the popup close.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopup()
        {
            await ClosePopupAction();
        }

        /// <summary>
        /// Invoke the keyboard action handler.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        /// <param name="args">The <see cref="KeyActions"/> arguments.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task KeyboardHandler(KeyActions args)
        {
            if (args != null)
            {
                await KeyboardActions(args);
            }
        }
    }
}