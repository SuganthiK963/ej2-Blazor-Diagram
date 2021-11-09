using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Specifies the view of the calendar.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CalendarView
    {
        /// <summary>
        /// Specifies the Month view of the calendar.
        /// </summary>
        [EnumMember(Value = "Month")]
        Month,

        /// <summary>
        /// Specifies the Year view of the calendar.
        /// </summary>
        [EnumMember(Value = "Year")]
        Year,

        /// <summary>
        /// Specifies the Decade view of the calendar.
        /// </summary>
        [EnumMember(Value = "Decade")]
        Decade,
    }

    /// <summary>
    /// Specifies the type of the calendar.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CalendarType
    {
        /// <summary>
        /// Specifies the Gregorian calendar.
        /// </summary>
        [EnumMember(Value = "Gregorian")]
        Gregorian,

        /// <summary>
        /// Specifies the Islamic calendar.
        /// </summary>
        [Obsolete("Islamic calendar mode is deprecated and will no longer be used")]
        [EnumMember(Value = "Islamic")]
        Islamic,
    }

    /// <summary>
    /// Specifies the day header formats.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DayHeaderFormats
    {
        /// <summary>
        /// Sets the short format of day name (like Su ) in day header.
        /// </summary>
        [EnumMember(Value = "Short")]
        Short,

        /// <summary>
        /// Sets the single character of day name (like S ) in day header.
        /// </summary>
        [EnumMember(Value = "Narrow")]
        Narrow,

        /// <summary>
        /// Sets the min format of day name (like Sun ) in day header.
        /// </summary>
        [EnumMember(Value = "Abbreviated")]
        Abbreviated,

        /// <summary>
        /// Sets the long format of day name (like Sunday ) in day header.
        /// </summary>
        [EnumMember(Value = "Wide")]
        Wide,
    }
}