using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Calendars.Internal
{
    /// <summary>
    /// The Calendar base is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    public partial class CalendarBaseRender<TValue> : CalendarBase<TValue>
    {
        private const string OTHER_MONTH = "e-other-month";
        private const string HEADER = "e-header";
        private const string CONTENT = "e-content";
        private const string YEAR = "e-year";
        private const string MONTH = "e-month";
        private const string DECADE = "e-decade";
        private const string ICON = "e-icons";
        private const string PREV_ICON = "e-prev";
        private const string NEXT_ICON = "e-next";
        private const string PREV_SPAN = "e-date-icon-prev";
        private const string NEXT_SPAN = "e-date-icon-next ";
        private const string ICON_CONTAINER = "e-icon-container";
        private const string DISABLED = "e-disabled";
        private const string OVERLAY = "e-overlay";
        private const string OTHER_MONTH_ROW = "e-month-hide";
        private const string TODAY = "e-today";
        private const string TITLE = "e-title";
        private const string LINK = "e-day";
        private const string FOOTER = "e-footer-container";
        private const string BTN = "e-btn";
        private const string FLAT = "e-flat";
        private const string CSS = "e-css";
        private const string PRIMARY = "e-primary";
        private const string MONTH_VIEW = "Month";
        private const string YEAR_VIEW = "Year";
        private const string DECADE_VIEW = "Decade";
        private const string VALUE = "Value";
        private const string CALENDAR_BASE_VALUES = "Values";
        private const string NAVIGATED = "Navigated";
        private const string MONTHS = "months";
        private const string FORMAT_YEAR = "yyyy";
        private const string FORMAT_SHORT_DATE = "M/d/yy";
        private const string FORMAT_FULL_DATE = "dddd, MMMM dd, yyyy hh:mm";
        private const string FORMAT_MONTHS = "MMMM yyyy";
        private const string DAYS = "days";
        private const string TODAY_LOCALE_KEY = "Calendar_Today";
        private const string TODAY_LOCALE_VALUE = "Today";
        private const string FALSE = "false";
        private const string TRUE = "true";
        private const string MOVE_LEFT = "moveLeft";
        private const string MOVE_RIGHT = "moveRight";
        private const string MOVE_UP = "moveUp";
        private const string MOVE_DOWN = "moveDown";
        private const string SELECT = "select";
        private const string CONTROL_UP = "controlUp";
        private const string CONTROL_DOWN = "controlDown";
        private const string HOME = "home";
        private const string END = "end";
        private const string PAGE_UP = "pageUp";
        private const string PAGE_DOWN = "pageDown";
        private const string SHIFT_PAGE_UP = "shiftPageUp";
        private const string SHIFT_PAGE_DOWN = "shiftPageDown";
        private const string CONTROL_HOME = "controlHome";
        private const string CONTROL_END = "controlEnd";
        private const int CELLCOUNT = 42;
        private const int WEEK_NUMBER = 7;
        private const int YEAR_NUMBER = 12;
        private const int CELL_ROW = 4;
        private const string TITLE_SEPARATOR = " - ";
        private const int MONTH_VIEW_VAL = (int)CalendarView.Month;
        private const int YEAR_VIEW_VAL = (int)CalendarView.Year;
        private const int DECADE_VIEW_VAL = (int)CalendarView.Decade;
    }

    /// <summary>
    /// Specifies the DatePicker popup arguments.
    /// </summary>
    public class DatePickerPopupArgs
    {
        /// <summary>
        /// Specifies the node to which the popup element to be appended.
        /// </summary>
        [JsonPropertyName("appendTo")]
        public string AppendTo { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonPropertyName("event")]
        public object Event { get; set; }

        /// <summary>
        /// Prevents the default action.
        /// </summary>
        [JsonPropertyName("preventDefault")]
        public object PreventDefault { get; set; }
    }
}