using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Calendars.Internal;
using System.Linq;
using System.ComponentModel;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The Calendar is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        /// <summary>
        /// Gets the properties to be maintained upon browser refresh.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task<string> GetPersistData()
        {

            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
        }
        /// <summary>
        /// Gets the properties to be maintained upon browser refresh.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<String> GetPersistDataAsync()
        {
            return await GetPersistData();
        }
        /// <summary>
        /// Gets the current view of the Calendar.
        /// </summary>
        /// <returns>Current view of the calendar.</returns>
        public string CurrentView()
        {
            return CalendarBase.CurrentView();
        }

        /// <summary>
        /// To navigate to the month or year or decade view of the calendar.
        /// </summary>
        /// <param name="view">Specifies the view of the calendar.</param>
        /// <param name="date">Specifies the focused date in a view.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task NavigateTo(CalendarView view, TValue date)
        {
            await CalendarBase.NavigateTo(view, date);
        }
        /// <summary>
        /// To navigate to the month or year or decade view of the calendar.
        /// </summary>
        /// <param name="view">Specifies the view of the calendar.</param>
        /// <param name="date">Specifies the focused date in a view.</param>
        /// <returns>Task.</returns>
        public async Task NavigateAsync(CalendarView view, TValue date)
        {
            await NavigateTo(view, date);
        }
        /// <summary>
        /// To adds the single or multiple dates to the Values property of the calendar.
        /// </summary>
        /// <param name="dates">Specifies the dates to be added to the Values property of the Calendar.</param>
        /// <returns>Task.</returns>
        public async Task AddDatesAsync(DateTime[] dates = null)
        {
            await AddDate(dates);
        }

        /// <summary>
        /// To adds the single or multiple dates to the Values property of the calendar.
        /// </summary>
        /// <param name="dates">Specifies the dates to be added to the Values property of the Calendar.</param>
        /// <returns>Task.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddDate(DateTime[] dates = null)
        {
            if (IsMultiSelection)
            {
                List<DateTime> copyValues = CopyValues(Values).ToList();
                if (dates != null && dates.Length > 0)
                {
                    DateTime[] tempDates = dates;
                    for (int index = 0; index < tempDates.Length; index++)
                    {
                        if (!CheckPresentDate(tempDates[index], copyValues.ToArray()))
                        {
                            copyValues.Add(tempDates[index]);
                        }
                    }
                }
                NotifyPropertyChanges(nameof(Value), GenericValue(copyValues.LastOrDefault()), CalendarBase_Value);
                await UpdateDateValues(copyValues.ToArray());
            }
        }

        private async Task UpdateDateValues(DateTime[] copyDateValue)
        {
            await UpdateCalendarProperty(CALENDAR_VALUES, copyDateValue);
            await UpdateCalendarProperty(VALUE, GenericValue(copyDateValue[copyDateValue.Length - 1]));
            TValue tempValue = Value == null ? default(TValue) : (TValue)SfBaseUtils.ChangeType(Value, PropertyType);
            ChangedArgs = new ChangedEventArgs<TValue> { Value = tempValue, Values = Values };
            ChangeHandler(null, Values, IsMultiSelection);
            await InvokeAsync(() => StateHasChanged());
        }
        /// <summary>
        /// To removes the single or multiple dates from the Values property of the calendar.
        /// <param name="dates">Specifies the dates which need to be removed from the values property of the Calendar.</param>
        /// </summary>
        public async Task RemoveDatesAsync(DateTime[] dates = null)
        {
            await RemoveDate(dates);
        }

        /// <summary>
        /// To removes the single or multiple dates from the Values property of the calendar.
        /// <param name="dates">Specifies the dates which need to be removed from the values property of the Calendar.</param>
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task RemoveDate(DateTime[] dates = null)
        {
            if (IsMultiSelection && Values != null && Values.Length > 0)
            {
                List<DateTime> copyValues = CopyValues(Values).ToList();
                if (dates != null && dates.Length > 0)
                {
                    DateTime[] tempDates = dates;
                    for (int index = 0; index < tempDates.Length; index++)
                    {
                        for (int tempIndex = 0; tempIndex < copyValues.Count; tempIndex++)
                        {
                            if (copyValues[tempIndex].Date == tempDates[index].Date)
                            {
                                copyValues.RemoveAt(tempIndex);
                            }
                        }
                    }
                }

                await UpdateDateValues(copyValues.ToArray());
            }
        }

        /// <summary>
        /// Invoke the component dispose.
        /// </summary>
        internal override void ComponentDispose()
        {
            ChangedArgs = null;
            containerAttributes = null;
            containerAttr = null;
        }
    }
}