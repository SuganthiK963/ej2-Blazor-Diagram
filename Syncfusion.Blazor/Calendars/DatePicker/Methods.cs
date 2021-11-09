using Microsoft.JSInterop;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Internal;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Defines the <see cref="SfDatePicker{TValue}" />.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of SfDatePicker.</typeparam>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Sets focus to the DatePicker component for interaction.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.DatePicker.focusIn", new object[] { InputElement });
        }
        /// <summary>
        /// Sets focus to the DatePicker component for interaction.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task FocusAsync()
        {
            await FocusIn();
        }
        /// <summary>
        /// Remove focus from the DatePicker component, if the component is in focus state.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task FocusOutAsync()
        {
            await FocusOut();
        }
        /// <summary>
        /// Remove focus from the DatePicker component, if the component is in focus state.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusOut()
        {
            await InvokeMethod("sfBlazor.DatePicker.focusOut", new object[] { InputElement });
        }

        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>The <see cref="Task{String}"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<string> GetPersistData()
        {
            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
        }
        /// <summary>
        /// Gets the properties to be maintained in the persisted state.
        /// </summary>
        /// <returns>The <see cref="Task{String}"/>.</returns>
        public async Task<String> GetPersistDataAsync()
        {
            return await GetPersistData();
        }
        /// <summary>
        /// Opens the popup to show the calendar.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task ShowPopupAsync()
        {
            await Show();
        }
        /// <summary>
        /// Opens the popup to show the calendar.
        /// </summary>
        /// <param name="args">Specifies the event arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Show(EventArgs args = null)
        {
            if (!((Enabled && Readonly) || !Enabled))
            {
                if (IsDevice)
                {
                    CalendarClass = SfBaseUtils.AddClass(CalendarClass, DEVICE);
                    var modelValue = Value != null ? (DateTime)SfBaseUtils.ChangeType(Value, typeof(DateTime)) : DateTime.Now;
                    ModelYear = Intl.GetDateFormat(modelValue, FORMAT_YEAR, CalendarLocale);
                    ModelDay = Intl.GetDateFormat(modelValue, FORMAT_DAY, CalendarLocale);
                    ModelMonth = Intl.GetDateFormat(modelValue, FORMAT_MONTH, CalendarLocale);
                }
                else
                {
                    CalendarClass = SfBaseUtils.RemoveClass(CalendarClass, DEVICE);
                }

                TValue outOfRange;
                if (Value != null && !(CompareValue(Value, ConvertGeneric(Min)) == 1 && CompareValue(ConvertGeneric(Max), Value) == 1))
                {
                    outOfRange = Value;
                    Value = CalendarBase_Value = await SfBaseUtils.UpdateProperty(default(TValue), CalendarBase_Value, ValueChanged, CalendarEditContext, ValueExpression);
                }
                else
                {
                    outOfRange = Value;
                }

                if (!IsCalendarRender)
                {
                    Value = CalendarBase_Value = await SfBaseUtils.UpdateProperty(outOfRange, CalendarBase_Value, ValueChanged, CalendarEditContext, ValueExpression);
                    PreviousDate = outOfRange;
                }

                PopupObjectArgs openEventArgs = await InvokeOpenEvent(true, args);
                if (!openEventArgs.Cancel)
                {
                    if (!IsDatePickerPopup)
                    {
                        UpdateDateTimePopupState(true);
                    }
                    else
                    {
                        IsCalendarRendered = true;
                        SetPopupVisibility(true);
                    }

                    SfBaseUtils.UpdateDictionary(ARIA_EXPANDED, TRUE, inputAttr);
                }

                await InvokeAsync(() => StateHasChanged());
            }
        }
        /// <summary>
        /// Hide the calendar popup.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task HidePopupAsync()
        {
            await Hide();
        }

        /// <summary>
        /// Hide the calendar popup.
        /// </summary>
        /// <param name="args">Specifies the event arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Hide(EventArgs args = null)
        {
            PopupObjectArgs closeEventArgs = await InvokeOpenEvent(false, args);
            if (!closeEventArgs.Cancel)
            {
                var options = new DatePickerClientProps<TValue>
                {
                    Readonly = Readonly,
                    Enabled = Enabled,
                    Locale = CalendarLocale,
                    EnableRtl = EnableRtl,
                    ZIndex = ZIndex,
                    KeyConfigs = KeyConfigs,
                    ShowClearButton = ShowClearButton,
                    Value = Value,
                    Width = Width,
                    IsDatePopup = IsDatePickerPopup,
                    AllowEdit = AllowEdit,
                    Depth = Depth.ToString()
                };
                if (DateIcon != null)
                {
                    DateIcon = SfBaseUtils.RemoveClass(DateIcon, ACTIVE);
                }

                await InvokeMethod("sfBlazor.DatePicker.closePopup", new object[] { InputElement, PopupElement, PopupHolderEle, PopupEventArgs, options });
                IsCalendarRender = false;
            }
        }

        /// <summary>
        /// Gets the current view of the calendar.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string CurrentView()
        {
            if (CalendarBaseInstance != null)
            {
                return CalendarBaseInstance.CurrentView();
            }
            else
            {
                return Start.ToString();
            }
        }
        /// <summary>
        /// To navigate to the month or year or decade view of the calendar.
        /// </summary>
        /// <param name="view">Specifies the view of the calendar.</param>
        /// <param name="date">Specifies the focused date in a view.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task NavigateAsync(CalendarView view, TValue date)
        {
            await NavigateTo(view, date);
        }

        /// <summary>
        /// To navigate to the month or year or decade view of the calendar.
        /// </summary>
        /// <param name="view">Specifies the view of the calendar.</param>
        /// <param name="date">Specifies the focused date in a view.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task NavigateTo(CalendarView view, TValue date)
        {
            if (CalendarBaseInstance != null)
            {
                await CalendarBaseInstance.NavigateTo(view, date);
            }
            else
            {
                CurrentValueAsString = Intl.GetDateFormat(date, GetDefaultFormat(), CalendarLocale);
                Start = view;
            }
        }

        /// <summary>
        /// Hides the calenar popup.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
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
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task ClosePopup()
        {
            if (IsDevice)
            {
                IsDateIconClicked = false;
            }

            await ClosePopupElement();
        }

        /// <summary>
        /// Invoke the keyboard action handler.
        /// </summary>
        /// <param name="args">The args<see cref="KeyActions"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task InputKeyActionHandle(KeyActions args)
        {
            if (args != null)
            {
                await InputKeyActionHandler(args);
            }
        }
    }
}
