using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DateRangePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDateRangePicker<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Sets focus to the DateRangePicker component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.DateRangePicker.focusIn", new object[] { InputElement });

        }
        /// <summary>
        /// Sets focus to the DateRangePicker component for interaction.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }

        /// <summary>
        /// Remove focus from the DateRangePicker component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.DateRangePicker.focusOut", new object[] { InputElement });

        }
        /// <summary>
        /// Remove focus from the DateRangePicker component, if the component is in focus state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Opens the popup to show the calendar.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task ShowPopupAsync()
        {
            await Show();
        }
        /// <summary>
        /// Hide the calendar popup.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task HidePopupAsync()
        {
            await Hide();
        }

        /// <summary>
        /// Opens the popup to show the calendar.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Show()
        {
            if (!((Enabled && Readonly) || !Enabled))
            {
                if (Presets.Count > 0)
                {
                    IsCustomPopup = (StartDate != null && EndDate != null) ? IsCustomPopup : true;
                    ProcessPresets();
                    SetPresetHeight();
                }

                CalendarClass = IsDevice ? SfBaseUtils.AddClass(CalendarClass, DEVICE) : SfBaseUtils.RemoveClass(CalendarClass, DEVICE);
                CellDetailsData = new List<CellDetails>();
                var openEventArgs = new RangePopupEventArgs
                {
                    Date = FormatDateValue,
                };
                PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY };
                await SfBaseUtils.InvokeEvent<RangePopupEventArgs>(DaterangepickerEvents?.OnOpen, openEventArgs);
                this.PreventOpen = openEventArgs.Cancel;
                if (!PreventOpen)
                {
                    if (IsDevice)
                    {
                        StartBtnClass = START_BUTTON + SPACE + ACTIVE;
                        EndBtnClass = END_BUTTON;
                    }

                    SelectStartMonth();
                    if (Start == CalendarView.Month)
                    {
                        SelectNextMonth();
                    }
                    else if (Start == CalendarView.Year)
                    {
                        SelectNextYear();
                    }
                    else if (Start == CalendarView.Decade)
                    {
                        SelectNextDecade();
                    }

                    IsCalendarRendered = true;
                    SetPopupVisibility(true);
                    SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, inputAttr);
                    UpdateHeaders();
                }

                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                await this.FocusOut();
            }
        }

        /// <summary>
        /// Gets the current view of the calendar.
        /// </summary>
        /// <returns>returns current view of the calendar.</returns>
        public string CurrentView()
        {
            return Start.ToString();
        }

        /// <summary>
        /// Hide the calendar popup.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide()
        {
            if (!IsCalendarRender)
            {
                return;
            }

            UpdateStartEndValue();
            var closeEventArgs = new RangePopupEventArgs()
            {
                Date = FormatDateValue,
            };
            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY };
            await SfBaseUtils.InvokeEvent<RangePopupEventArgs>(DaterangepickerEvents?.OnClose, closeEventArgs);
            if (!closeEventArgs.Cancel)
            {
                RightCalFocusToday = false;
                LeftCalFocusToday = true;
                var options = new DateRangePickerClientProps<TValue>
                {
                    EnableRtl = EnableRtl,
                    ZIndex = ZIndex,
                    Presets = Presets,
                };
                if (StartValue == null || EndValue == null)
                {
                    RemoveSelection();
                }

                DateRangeIcon = SfBaseUtils.RemoveClass(DateRangeIcon, ACTIVE);
                IsCalendarRender = false;
                await InvokeMethod("sfBlazor.DateRangePicker.closePopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, options });
            }
        }

        /// <summary>
        /// Hides the range popup.
        /// </summary>
        /// <param name="args">Specifies the event arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task HidePopup(EventArgs args = null)
        {
            var eventargs = args != null ? args : new MouseEventArgs();
            await ApplyFunction((MouseEventArgs)eventargs);
        }

        /// <summary>
        /// Invoke the before the popup close.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopup()
        {
            if (IsDevice)
            {
                IsDateRangeIconClicked = false;
            }

            await ClosePopupAction();
        }

        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<string> GetPersistData()
        {
            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
        }

        /// <summary>
        /// Invoke the keyboard action handler.
        /// </summary>
        /// <param name="args">Specifies key action arguments.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task InputKeyActionHandle(KeyActions args)
        {
            if (args != null)
            {
                if (IsCalendarRender)
                {
                    if (IsPresetRender && (!IsCustomPopup || PresetFocus))
                    {
                        await KeyActionHandler(args);
                    }
                    else
                    {
                        await InputKeyActionHandler(args);
                    }
                }
                else
                {
                    await InputHandler(args);
                }
            }
        }
    }
}