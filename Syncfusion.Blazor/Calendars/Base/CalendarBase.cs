using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Calendars.Internal;
using Syncfusion.Blazor.Inputs;
using System.Globalization;
using System.ComponentModel;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The Calendar is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    /// <typeparam name="T">Specifies the type of CalendarBase.</typeparam>
    public class CalendarBase<T> : SfInputTextBase<T>
    {
        internal const string SHORTDATE = "M/d/yy";

        internal CellDetails CurrentCellDetail = new CellDetails();

        ///// <summary>
        ///// Specifies the expression for defining the value of the bound.
        ///// </summary>
        //[Parameter]
        //public Expression<Func<T>> ValueExpression { get; set; }

        /// <summary>
        /// Specifies the editcontext of the Calendar.
        /// </summary>
        [CascadingParameter]
        protected EditContext CalendarEditContext { get; set; }

        /// <summary>
        /// Specifies a maximum date that is allowed a user can select in the calendar.
        /// </summary>
        [Parameter]
        public virtual DateTime Max { get; set; } = new DateTime(2099, 12, 31);

        internal DateTime CalendarBase_Max { get; set; }

        /// <summary>
        /// Specifies a minimum date that is allowed a user can select in the calendar.
        /// </summary>
        [Parameter]
        public virtual DateTime Min { get; set; } = new DateTime(1900, 01, 01);

        internal DateTime CalendarBase_Min { get; set; }

        /// <summary>
        /// Sets the calendar's first day of the week. By default, the first day of the week will be based on the current culture.
        /// </summary>
        [Parameter]
        public int FirstDayOfWeek { get; set; }

        internal int CalendarBase_FirstDayOfWeek { get; set; }

        /// <summary>
        /// Sets the calendar's type like Gregorian.
        /// </summary>
        [Parameter]
        public CalendarType CalendarMode { get; set; }

        /// <summary>
        /// Specifies the format of the day that to be displayed in the header. By default, the format is short.
        /// <para>Possible formats are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Short</term>
        /// <description>Sets the short format of day name (like Su ) in day header.</description>
        /// </item>
        /// <item>
        /// <term>Narrow</term>
        /// <description>Sets the single character of day name (like S ) in day header.</description>
        /// </item>
        /// <item>
        /// <term>Abbreviated</term>
        /// <description>Sets the min format of day name (like Sun ) in day header.</description>
        /// </item>
        /// <item>
        /// <term>Wide</term>
        /// <description>Sets the long format of day name (like Sunday ) in day header.</description>
        /// </item>
        /// </list>
        /// </summary>
        [Parameter]
        public DayHeaderFormats DayHeaderFormat { get; set; }

        /// <summary>
        /// Sets the maximum level of views such as a month, year, and decade in the calendar.
        /// <para>The depth view should be smaller than the start view to restrict its view navigation.</para>
        /// </summary>
        [Parameter]
        public CalendarView Depth { get; set; }

        internal CalendarView CalendarBase_Depth { get; set; }

        /// <summary>
        /// Customizes the key actions in the calendar.
        /// <para>For example, when using a German keyboard, the key actions can be customized using these shortcuts.</para>
        /// </summary>
        [Parameter]
        public Dictionary<string, object> KeyConfigs { get; set; }

        /// <summary>
        /// Specifies the initial view of the calendar when it is opened. With the help of this property, the initial view can be changed to the year or decade view.
        /// </summary>
        [Parameter]
        public CalendarView Start { get; set; }

        internal CalendarView CalendarBase_Start { get; set; }

        /// <summary>
        /// By default, the date value will be processed based on the system time zone.
        /// <para>If you want to process the initial date value using the server time zone then specify the time zone value to the ServerTimezoneOffset property.</para>
        /// </summary>
        [Parameter]
        public double ServerTimezoneOffset { get; set; }
        internal string CalendarLocale { get; set; }

        /// <summary>
        /// Specifies whether the today button will be displayed in the calendar.
        /// </summary>
        [Parameter]
        public bool ShowTodayButton { get; set; } = true;

        /// <summary>
        /// Specifies whether the week number of the year will be displayed in the calendar.
        /// </summary>
        [Parameter]
        public bool WeekNumber { get; set; }

        /// <summary>
        /// Specifies the rule for defining the first week of the year.
        /// </summary>
        [Parameter]
        public CalendarWeekRule WeekRule { get; set; }

        private List<string> DirectParamKeys { get; set; } = new List<string>();

        internal T CalendarBase_Value { get; set; }

        [CascadingParameter]
        internal CalendarBaseRender<T> BaseParent { get; set; }

        internal ChangedEventArgs<T> ChangedArgs { get; set; } = new ChangedEventArgs<T>();

        internal List<CellDetails> CellListData { get; set; }

        internal List<CellDetails> CellDetailsData { get; set; } = new List<CellDetails>();

        internal T PreviousDate { get; set; }

        internal T PreviousSelectedDate { get; set; }

        internal T PreviousDeSelectedDate { get; set; }

        internal int PreviousValues { get; set; }

        internal string Effect { get; set; } = string.Empty;

        internal bool IsTodayClick { get; set; }

        internal ElementReference Element { get; set; }

        internal virtual async Task UpdateCalendarProperty(string key, object dateValue)
        {
            await Task.CompletedTask;
        }

#pragma warning disable CA1822 // Mark members as static
        internal string ToUpperFirstChar(string str)
        {
            return !string.IsNullOrEmpty(str) ? (str.Length == 1) ? char.ToUpper(str[0], CultureInfo.CurrentCulture).ToString() : char.ToUpper(str[0], CultureInfo.CurrentCulture) + str.Substring(1) : string.Empty;
        }

        internal DateTime[] CopyValues(DateTime[] values)
        {
            List<DateTime> copyValues = new List<DateTime>();
            if (values != null && values.Length > 0)
            {
                for (int index = 0; index < values.Length; index++)
                {
                    copyValues.Add(new DateTime(values[index].Ticks));
                }
            }

            return copyValues.ToArray();
        }

        internal bool CheckPresentDate(DateTime dates, DateTime[] values)
        {
            bool previousValue = false;
            if (values != null && values.Length > 0)
            {
                for (int checkPrevious = 0; checkPrevious < values.Length; checkPrevious++)
                {
                    string localDateString = Intl.GetDateFormat(dates, SHORTDATE, CalendarLocale);
                    string tempDateString = Intl.GetDateFormat(values[checkPrevious], SHORTDATE, CalendarLocale);
                    if (localDateString == tempDateString)
                    {
                        previousValue = true;
                    }
                }
            }

            return previousValue;
        }

        internal void ChangeHandler(MouseEventArgs args = null, DateTime[] values = null, bool? isMultiSelection = null)
        {
            Type propertyType = typeof(T);
            ChangedArgs.Event = args;
            ChangedArgs.IsInteracted = args != null;
            T dateValue = (Value == null) ? default(T) : (T)SfBaseUtils.ChangeType(Value, propertyType);
            T previousDateVal = (PreviousDate == null) ? default(T) : (T)SfBaseUtils.ChangeType(PreviousDate, propertyType);
            if (!(bool)isMultiSelection && (!SfBaseUtils.Equals(dateValue, previousDateVal)))
            {
                ChangeEvent(args);
            }
            else if (values != null && PreviousValues != values.Length)
            {
                ChangeEvent(args);
                PreviousValues = values.Length;
            }
        }

        internal DateTime ConvertDate(T dateValue)
        {
            Type propertyType = typeof(T);
            bool isNullable = Nullable.GetUnderlyingType(propertyType) != null;
            DateTime dateVal = DateTime.Now;
            if ((dateValue != null && propertyType == typeof(DateTime)) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(propertyType)))
            {
                dateVal = (DateTime)SfBaseUtils.ChangeType(dateValue, propertyType);
            }
            else if ((dateValue != null && propertyType == typeof(DateTimeOffset)) || (isNullable && typeof(DateTimeOffset) == Nullable.GetUnderlyingType(propertyType)))
            {
                DateTimeOffset dt = (DateTimeOffset)SfBaseUtils.ChangeType(dateValue, propertyType);
                dateVal = dt.DateTime;
            }

            return dateVal;
        }

        internal virtual void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
        }

        internal virtual async Task HoverSelection(CellDetails args)
        {
            await Task.CompletedTask;
        }

        internal virtual async Task BindRenderDayEvent(RenderDayCellEventArgs eventArgs)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Triggers when the value get changed.
        /// </summary>
        /// <param name="args">Specifies the <see cref="EventArgs"> arguments</see>.</param>
        protected virtual void ChangeEvent(EventArgs args)
        {
        }

        internal T GenericValue(DateTime dateValue)
#pragma warning restore CA1822 // Mark members as static
        {
            Type propertyType = typeof(T);
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }
            if (propertyType == typeof(DateTime))
            {
                return (T)SfBaseUtils.ChangeType(dateValue, propertyType);
            }
            else
            {
                return (T)SfBaseUtils.ChangeType(new DateTimeOffset(dateValue), propertyType);
            }
        }

        /// <exclude/>
        internal async Task KeyBoardEventHandler(KeyActions args)
        {
            await BaseParent.KeyActionHandler(args);
        }

        internal async Task SetLocalStorage(string persistId, T dataValue)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, dataValue });
        }

        internal virtual async Task InvokeDeSelectEvent(DeSelectedEventArgs<T> args)
        {
            await Task.CompletedTask;
        }
        internal override async Task OnAfterScriptRendered()
        {
            await Task.CompletedTask;
        }
        /// <summary>
        /// Triggers while properties get dynamically changed in the component.
        /// </summary>
        /// <returns>System.Threading.Tasks.</returns>
        /// <param name="parameters"><see cref="ParameterView"/> parameters.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (DirectParamKeys.Count == 0)
            {
                foreach (var parameter in parameters)
                {
                    if (!parameter.Cascading)
                    {
                        DirectParamKeys.Add(parameter.Name);
                    }
                }
            }

            var currentCulture = CultureInfo.CurrentCulture;
            if (currentCulture != null && !DirectParamKeys.Contains("WeekRule"))
            {
                WeekRule = currentCulture.DateTimeFormat.CalendarWeekRule;
            }

            if (currentCulture != null && !DirectParamKeys.Contains("FirstDayOfWeek"))
            {
                FirstDayOfWeek = (int)currentCulture.DateTimeFormat.FirstDayOfWeek;
            }

            return base.SetParametersAsync(parameters);
        }

        internal virtual async Task InvokeSelectEvent(SelectedEventArgs<T> args)
        {
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Specifies the cell details and its respective properties.
    /// </summary>
    public class CellDetails
    {
        /// <summary>
        /// Gets or sets the cell Id.
        /// </summary>
        public string CellID { get; set; }

        /// <summary>
        /// Gets or sets the cell Id.
        /// </summary>
        public string ClassList { get; set; }

        /// <summary>
        /// Gets or sets the element reference.
        /// </summary>
        public ElementReference? Element { get; set; }

        /// <summary>
        /// Gets or sets the event arguments.
        /// </summary>
        public MouseEventArgs EventArgs { get; set; }

        /// <summary>
        /// Gets or sets the current date.
        /// </summary>
        public DateTime CurrentDate { get; set; }
    }
}
