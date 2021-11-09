using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Defines the argument for the blur event.
    /// </summary>
    public class BlurEventArgs
    {
        /// <summary>
        /// Gets or sets the Model.
        /// </summary>
        [JsonProperty("model")]
        public object Model { get; set; }
    }

    /// <summary>
    /// Defines the event argument for Selected event.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class SelectedEventArgs<T>
    {
        /// <summary>
        /// Returns the selected date.
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// Defines the event argument for DeSelected event.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class DeSelectedEventArgs<T>
    {
        /// <summary>
        /// Returns the selected date.
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Change event.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ChangedEventArgs<T>
    {
        /// <summary>
        /// Defines the element.
        /// </summary>
        [JsonProperty("element")]
        public object Element { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines the selected date of the Calendar.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }

        /// <summary>
        /// Defines the multiple selected date of the Calendar.
        /// </summary>
        [JsonProperty("values")]
        public DateTime[] Values { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Clear event.
    /// </summary>
    public class ClearedEventArgs
    {
        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Focus event.
    /// </summary>
    public class FocusEventArgs
    {
        /// <summary>
        /// Gets or sets the Model.
        /// </summary>
        [JsonProperty("model")]
        public object Model { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Navigation event.
    /// </summary>
    public class NavigatedEventArgs
    {
        /// <summary>
        /// Defines the focused date in a view.
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines the current view of the Calendar.
        /// </summary>
        [JsonProperty("view")]
        public string View { get; set; }
    }

    /// <summary>
    /// Defines the argument for the RenderDayCell event.
    /// </summary>
    public class RenderDayCellEventArgs
    {
        /// <summary>
        /// Defines the current date of the Calendar.
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Specifies the day cell element.
        /// </summary>
        [JsonProperty("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Specifies whether to disable the current date or not.
        /// </summary>
        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Defines whether the current date is out of range (less than min or greater than max) or not.
        /// </summary>
        [JsonProperty("isOutOfRange")]
        public bool IsOutOfRange { get; set; }

        /// <summary>
        /// Gets or sets the cell data.
        /// </summary>
        public CellDetails CellData { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// Specifies name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Open and Close event.
    /// </summary>
    public class PopupObjectArgs
    {
        /// <summary>
        /// Specifies the node to which the popup element to be appended.
        /// </summary>
        [JsonProperty("appendTo")]
        public DOM AppendTo { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Prevents the default action.
        /// </summary>
        [JsonProperty("preventDefault")]
        public object PreventDefault { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Range event.
    /// </summary>
    public class RangeEventArgs
    {
        /// <summary>
        /// Defines the day span between the range.
        /// </summary>
        [JsonProperty("daySpan")]
        public double DaySpan { get; set; }

        /// <summary>
        /// Specifies the element.
        /// </summary>
        [JsonProperty("element")]
        public object Element { get; set; }

        /// <summary>
        /// Defines the end date.
        /// </summary>
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines the start date.
        /// </summary>
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Defines the value string in the input element.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Defines the value.
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Range event.
    /// </summary>
    /// <typeparam name="TValue">.</typeparam>
    public class RangePickerEventArgs<TValue>
    {
        /// <summary>
        /// Defines the day span between the range.
        /// </summary>
        public double DaySpan { get; set; }

        /// <summary>
        /// Specifies the element.
        /// </summary>
        public object Element { get; set; }

        /// <summary>
        /// Defines the end date.
        /// </summary>
        public TValue EndDate { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object Event { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines the start date.
        /// </summary>
        public TValue StartDate { get; set; }

        /// <summary>
        /// Defines the value string in the input element.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the value.
        /// </summary>
        [Obsolete("The Value is deprecated and will no longer be used. Hereafter, StartDate and EndDate properties will show the changed values.")]
        public object Value { get; set; }
    }

    /// <summary>
    /// Defines the argument for the DateRangePicker Popup event.
    /// </summary>
    public class RangePopupEventArgs
    {
        /// <summary>
        /// Specifies the node to which the popup element to be appended.
        /// </summary>
        public string AppendTo { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the range string in the input element.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        public object Event { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Change event.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ChangeEventArgs<T>
    {
        /// <summary>
        /// Defines the element.
        /// </summary>
        [JsonProperty("element")]
        public object Element { get; set; }

        /// <summary>
        /// Defines the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Defines the boolean that returns true when the value is changed by user interaction, otherwise returns false.
        /// </summary>
        [JsonProperty("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Defines the selected time value as string.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Defines the selected time value of the TimePicker.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }
    }

    /// <summary>
    /// Interface for before list item render .
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ItemEventArgs<T>
    {
        /// <summary>
        /// Defines the created LI element.
        /// </summary>
        [JsonProperty("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Specifies whether to disable the current time value or not.
        /// </summary>
        [JsonProperty("isDisabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Defines the displayed text value in a popup list.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Defines the Date object of displayed text in a popup list.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }
    }

    /// <summary>
    /// Defines the argument for the Open and Close event.
    /// </summary>
    public class PopupEventArgs
    {
        /// <summary>
        /// Specifies the node to which the popup element to be appended.
        /// </summary>
        [JsonProperty("appendTo")]
        public DOM AppendTo { get; set; }

        /// <summary>
        /// Illustrates whether the current action needs to be prevented or not.
        /// </summary>
        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the original event arguments.
        /// </summary>
        [JsonProperty("event")]
        public object Event { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for a class DateRangePicker.
    /// </summary>
    /// <typeparam name="TValue">.</typeparam>
    public class DateRangePickerModel<TValue>
    {
        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Triggers when the DateRangePicker value is changed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when daterangepicker value is cleared using clear button.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("cleared")]
        public EventCallback<object> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("close")]
        public RangePopupEventArgs Close { get; set; } = null;

        /// <summary>
        /// Triggers when the DateRangePicker is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the DateRangePicker is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the Calendar is navigated to another view or within the same level of view.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("navigated")]
        public EventCallback<object> Navigated { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("open")]
        public RangePopupEventArgs Open { get; set; } = null;

        /// <summary>
        /// Gets or sets the RenderDayCell
        /// Triggers when each day cell of the Calendar is rendered.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("renderDayCell")]
        public EventCallback<object> RenderDayCell { get; set; }

        /// <summary>
        /// Triggers on selecting the start and end date.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("select")]
        public RangePickerEventArgs<TValue> Select { get; set; } = null;

        /// <summary>
        /// Specifies a boolean value whether the DateRangePicker allows user to change the value via typing. When set as false, the DateRangePicker allows user to change the value via picker only.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("allowEdit")]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DateRangePicker. One or more custom CSS classes can be added to a DateRangePicker.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Sets the maximum level of view (Month, Year, Decade) in the Calendar.
        /// <para>Depth view should be smaller than the Start view to restrict its view navigation.</para>.
        /// </summary>
        [DefaultValue(CalendarView.Month)]
        [JsonProperty("depth")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarView Depth { get; set; } = CalendarView.Month;

        /// <summary>
        /// Enable or disable the persisting DateRangePicker's state between the page reloads. If enabled, following list of states will be persisted.
        /// <list type="bullet">
        /// <item>
        /// <term>StartDate</term>
        /// </item>
        /// <item>
        /// <term>EndDate</term>
        /// </item>
        /// <item>
        /// <term>Value</term>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Enable or disable rendering DateRangePicker in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the DateRangePicker allows the user to interact with it.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the end date of the date range selection.
        /// </summary>
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Sets the calendar's first day of the week. By default, the first day of the week will be based on the current culture.
        /// </summary>
        [DefaultValue(default(double))]
        [JsonProperty("firstDayOfWeek")]
        public double FirstDayOfWeek { get; set; } = default;

        /// <summary>
        /// Specifies the floating label behavior of the DateRangePicker that the placeholder text floats above the DateRangePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DateRangePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DateRangePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DateRangePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(Syncfusion.Blazor.Inputs.FloatLabelType.Never)]
        [JsonProperty("floatLabelType")]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; } = Syncfusion.Blazor.Inputs.FloatLabelType.Never;

        /// <summary>
        /// Specifies the format of the value that to be displayed in DateRangePicker.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("format")]
        public string Format { get; set; } = null;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the DateRangePicker considers the property value.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public object HtmlAttributes { get; set; } = null;

        /// <summary>
        /// Customizes the key actions in DateRangePicker.
        /// For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("keyConfigs")]
        public object KeyConfigs { get; set; } = null;

        /// <summary>
        /// Specifies the global culture and localization of the DateRangePicker.
        /// </summary>
        [DefaultValue("en-US")]
        [JsonProperty("locale")]
        public string Locale { get; set; } = "en-US";

        /// <summary>
        /// Gets or sets the maximum date that can be selected in the calendar-popup.
        /// </summary>
        [JsonProperty("max")]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31);

        /// <summary>
        /// Specifies the maximum span of days that can be allowed in a date range selection.
        /// </summary>
        [DefaultValue(default(int))]
        [JsonProperty("maxDays")]
        public int MaxDays { get; set; } = default;

        /// <summary>
        /// Gets or sets the minimum date that can be selected in the calendar-popup.
        /// </summary>
        [JsonProperty("min")]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01);

        /// <summary>
        /// Specifies the minimum span of days that can be allowed in date range selection.
        /// </summary>
        [DefaultValue(default(int))]
        [JsonProperty("minDays")]
        public int MinDays { get; set; } = default;

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DateRangePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null;

        /// <summary>
        /// Set the predefined ranges which let the user pick required range easily in a DateRangePicker.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("presets")]

        public List<PresetsModel> Presets { get; set; } = null;

        /// <summary>
        /// Specifies a boolean value whether the DateRangePicker allows the user to change the text.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// Sets or gets the string that used between the start and end date string.
        /// </summary>
        [DefaultValue("-")]
        [JsonProperty("separator")]
        public string Separator { get; set; } = "-";

        /// <summary>
        /// Specifies whether to show or hide the clear icon in DateRangePicker.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies the initial view of the Calendar when it is opened.
        /// With the help of this property, initial view can be changed to year or decade view.
        /// </summary>
        [DefaultValue(CalendarView.Month)]
        [JsonProperty("start")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarView Start { get; set; } = CalendarView.Month;

        /// <summary>
        /// Gets or sets the start date of the date range selection.
        /// </summary>
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether StrictMode
        /// Specifies the DateRangePicker to act as strict. So that, it allows to enter only a valid date value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("strictMode")]
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the start and end date of the Calendar.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("value")]
        public object Value { get; set; } = null;

        /// <summary>
        /// Determines whether the week number of the Calendar is to be displayed or not.
        /// The week number is displayed in every week row.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("weekNumber")]
        public bool WeekNumber { get; set; } = false;

        /// <summary>
        /// Specifies the width of the DateRangePicker component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("width")]
        public object Width { get; set; } = null;

        /// <summary>
        /// specifies the z-index value of the DateRangePicker popup element.
        /// </summary>
        [DefaultValue(1000)]
        [JsonProperty("zIndex")]
        public int ZIndex { get; set; } = 1000;
    }

    /// <summary>
    /// Interface for a class Presets.
    /// </summary>
    public class PresetsModel
    {
        /// <summary>
        /// Defines the end date of the preset range.
        /// </summary>
        [JsonProperty("end")]
        public DateTime End { get; set; }

        /// <summary>
        /// Defines the label string of the preset range.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Defines the start date of the preset range.
        /// </summary>
        [JsonProperty("start")]
        public DateTime Start { get; set; }
    }

    /// <summary>
    /// Interface for a class DateTimePicker.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class DateTimePickerModel<T>
    {
        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Triggers when the date or time value is changed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when DateTimePicker value is cleared using clear button.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("cleared")]
        public EventCallback<object> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("close")]
        public PopupObjectArgs Close { get; set; } = null;

        /// <summary>
        /// Triggers when the DateTimePicker is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the DateTimePicker is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers when the Calendar is navigated to another view or within the same level of view.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("navigated")]
        public EventCallback<object> Navigated { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("open")]
        public PopupObjectArgs Open { get; set; } = null;

        /// <summary>
        /// Triggers when each day cell of the Calendar is rendered.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("renderDayCell")]
        public EventCallback<object> RenderDayCell { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the DateTimePicker allows user to change the value via typing. When set as false, the DateTimePicker allows user to change the value via picker only.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("allowEdit")]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Gets or sets the Calendar's Type like gregorian or islamic.
        /// </summary>
        [DefaultValue(CalendarType.Gregorian)]
        [JsonProperty("calendarMode")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarType CalendarMode { get; set; } = CalendarType.Gregorian;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DateTimePicker. One or more custom CSS classes can be added to a DateTimePicker.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = null;

        /// <summary>
        /// Specifies the format of the day that to be displayed in the header. By default, the format is Short.
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
        /// </list>.
        /// </summary>
        [DefaultValue(DayHeaderFormats.Short)]
        [JsonProperty("dayHeaderFormat")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DayHeaderFormats DayHeaderFormat { get; set; } = DayHeaderFormats.Short;

        /// <summary>
        /// Sets the maximum level of view (Month, Year, Decade) in the Calendar.
        /// <para>Depth view should be smaller than the Start view to restrict its view navigation.</para>.
        /// </summary>
        [DefaultValue(CalendarView.Month)]
        [JsonProperty("depth")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarView Depth { get; set; } = CalendarView.Month;

        /// <summary>
        /// Enable or disable persisting DateTimePicker's state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Enable or disable rendering DateTimePicker in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the DateTimePicker allows the user to interact with it.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Sets the calendar's first day of the week. By default, the first day of the week will be based on the current culture.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty("firstDayOfWeek")]
        public int FirstDayOfWeek { get; set; }

        /// <summary>
        /// Specifies the floating label behavior of the DateTimePicker that the placeholder text floats above the DateTimePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DateTimePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DateTimePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DateTimePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(Syncfusion.Blazor.Inputs.FloatLabelType.Never)]
        [JsonProperty("floatLabelType")]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; } = Syncfusion.Blazor.Inputs.FloatLabelType.Never;

        /// <summary>
        /// Specifies the format of the value that to be displayed in DateTimePicker.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("format")]
        public string Format { get; set; } = null;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the DateTimePicker considers the property value.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public object HtmlAttributes { get; set; } = null;

        /// <summary>
        /// Customizes the key actions in DateTimePicker.
        /// For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("keyConfigs")]
        public object KeyConfigs { get; set; } = null;

        /// <summary>
        /// Specifies the global culture and localization of the DateTimePicker.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("locale")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum date that can be selected in the DateTimePicker.
        /// </summary>
        [JsonProperty("max")]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31);

        /// <summary>
        /// Gets or sets the minimum date that can be selected in the DateTimePicker.
        /// </summary>
        [JsonProperty("min")]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01);

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DateTimePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null;

        /// <summary>
        /// Specifies a boolean value whether the DateTimePicker allows the user to change the text.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the timepicker popup list or
        /// the given value is not present in the timepicker popup list.
        /// </summary>
        [JsonProperty("scrollTo")]
        public DateTime ScrollTo { get; set; }

        /// <summary>
        /// By default, the date value will be processed based on system time zone.
        /// If you want to process the initial date value using server time zone
        /// then specify the time zone value to `ServerTimezoneOffset` property.
        /// </summary>
        [DefaultValue(default(double))]
        [JsonProperty("serverTimezoneOffset")]
        public double ServerTimezoneOffset { get; set; } = default;

        /// <summary>
        /// Specifies whether to show or hide the clear icon in textbox.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies whether the today button is to be displayed or not.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showTodayButton")]
        public bool ShowTodayButton { get; set; } = true;

        /// <summary>
        /// Specifies the initial view of the Calendar when it is opened.
        /// With the help of this property, initial view can be changed to year or decade view.
        /// </summary>
        [DefaultValue(CalendarView.Month)]
        [JsonProperty("start")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalendarView Start { get; set; } = CalendarView.Month;

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the time popup list .
        /// </summary>
        [DefaultValue(30)]
        [JsonProperty("step")]
        public int Step { get; set; } = 30;

        /// <summary>
        /// Specifies the DateTimePicker to act as strict. So that, it allows to enter only a valid date value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("strictMode")]
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// Specifies the format of the time value that to be displayed in time popup list.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("timeFormat")]
        public string TimeFormat { get; set; } = null;

        /// <summary>
        /// Gets or sets the selected date of the Calendar.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }

        /// <summary>
        /// Determines whether the week number of the year is to be displayed in the calendar or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("weekNumber")]
        public bool WeekNumber { get; set; } = false;

        /// <summary>
        /// Specifies the width of the DateTimePicker component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("width")]
        public object Width { get; set; } = null;

        /// <summary>
        /// Specifies the z-index value of the DateTimePicker popup element.
        /// </summary>
        [DefaultValue(1000)]
        [JsonProperty("zIndex")]
        public int ZIndex { get; set; } = 1000;
    }

    /// <summary>
    /// Interface for a class DatePicker.
    /// </summary>
    public class DatePickerModel
    {
        /// <summary>
        /// Specifies a boolean value whether the DatePicker allows user to change the value via typing. When set as false, the DatePicker allows user to change the value via picker only.
        /// </summary>
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the DatePicker. One or more custom CSS classes can be added to a DatePicker.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        public RenderFragment<object> ChildContent { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the DatePicker allows the user to interact with it.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Specifies the floating label behavior of the DatePicker that the placeholder text floats above the DatePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the DatePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the DatePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the DatePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// Specifies the format of the value that to be displayed in component.
        /// <para>By default, the format is based on the culture.</para>
        /// <para>You can set the format to "format:'dd/MM/yyyy hh:mm'".</para>.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as styles, class, and more to the root element.
        /// <para>If you configured both the property and equivalent html attribute, then the component considers the property value.</para>.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the component considers the property value.</para>.
        /// </summary>
        public Dictionary<string, object> InputAttributes { get; set; }

        /// <summary>
        /// Specifies the global culture and localization of the calendar.
        /// </summary>
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in DatePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the DatePicker allows the user to change the text.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Specifies a boolean value that indicates whether the clear button is displayed in DatePicker.
        /// </summary>
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies the component to act as strict. So that, it allows to enter only a valid date  value within a specified range or else it will resets to previous value.
        /// </summary>
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range date value with highlighted error class.</para>
        public bool StrictMode { get; set; }

        /// <summary>
        /// Specifies the width of the DatePicker component.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// specifies the z-index value of the DatePicker popup element.
        /// </summary>
        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// Specifies the tab order of the DatePicker component.
        /// </summary>
        public int TabIndex { get; set; }
    }

    /// <summary>
    /// Interface for a class TimePicker.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class TimePickerModel<T>
    {
        /// <summary>
        /// Triggers when the input loses the focus.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("blur")]
        public EventCallback<object> Blur { get; set; }

        /// <summary>
        /// Triggers when the time value is changed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("change")]
        public EventCallback<object> Change { get; set; }

        /// <summary>
        /// Triggers when TimePicker value is cleared using clear button.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("cleared")]
        public EventCallback<object> Cleared { get; set; }

        /// <summary>
        /// Triggers when the popup is closed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("close")]
        public EventCallback<object> Close { get; set; }

        /// <summary>
        /// Triggers when the TimePicker is created.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("created")]
        public EventCallback<object> Created { get; set; }

        /// <summary>
        /// Triggers when the TimePicker is destroyed.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("destroyed")]
        public EventCallback<object> Destroyed { get; set; }

        /// <summary>
        /// Triggers when the input gets focus.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("focus")]
        public EventCallback<object> Focus { get; set; }

        /// <summary>
        /// Triggers while rendering the each popup list item.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("itemRender")]
        public EventCallback<object> ItemRender { get; set; }

        /// <summary>
        /// Triggers when the popup is opened.
        /// </summary>
        [JsonIgnore]
        [JsonProperty("open")]
        public EventCallback<object> Open { get; set; }

        /// <summary>
        /// Specifies a boolean value whether the TimePicker allows user to change the value via typing. When set as false, the TimePicker allows user to change the value via picker only.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("allowEdit")]
        public bool AllowEdit { get; set; } = true;

        /// <summary>
        /// Specifies the CSS class name that can be appended with the root element of the TimePicker. One or more custom CSS classes can be added to a TimePicker.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("cssClass")]
        public string CssClass { get; set; } = null;

        /// <summary>
        /// Enable or disable persisting TimePicker's state between page reloads. If enabled, the `Value` state will be persisted.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enablePersistence")]
        public bool EnablePersistence { get; set; } = false;

        /// <summary>
        /// Enable or disable rendering TimePicker in right to left direction.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("enableRtl")]
        public bool EnableRtl { get; set; } = false;

        /// <summary>
        /// Specifies a boolean value that indicates whether the TimePicker allows the user to interact with it.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Specifies the floating label behavior of the TimePicker that the placeholder text floats above the TimePicker based on the following values.
        /// <para>Possible values are:</para>
        /// <list type="bullet">
        /// <item>
        /// <term>Never</term>
        /// <description>Never floats the label in the TimePicker when the placeholder is available.</description>
        /// </item>
        /// <item>
        /// <term>Always</term>
        /// <description>The floating label always floats above the TimePicker.</description>
        /// </item>
        /// <item>
        /// <term>Auto</term>
        /// <description>The floating label floats above the TimePicker after focusing it or when enters the value in it.</description>
        /// </item>
        /// </list>.
        /// </summary>
        [DefaultValue(Syncfusion.Blazor.Inputs.FloatLabelType.Never)]
        [JsonProperty("floatLabelType")]
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; } = Syncfusion.Blazor.Inputs.FloatLabelType.Never;

        /// <summary>
        /// Specifies the format of the value that to be displayed in TimePicker.
        /// <para>By default, the format is based on the culture.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("format")]
        public string Format { get; set; } = null;

        /// <summary>
        /// You can add the additional input attributes such as disabled, value, and more to the root element.
        /// <para>If you configured both the property and equivalent input attribute, then the TimePicker considers the property value.</para>.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("htmlAttributes")]
        public object HtmlAttributes { get; set; } = null;

        /// <summary>
        /// Customizes the key actions in TimePicker.
        /// For example, when using German keyboard, the key actions can be customized using these shortcuts.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("keyConfigs")]
        public object KeyConfigs { get; set; } = null;

        /// <summary>
        /// Specifies the global culture and localization of the TimePicker.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("locale")]
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the maximum time value that can be allowed to select in TimePicker.
        /// </summary>
        [JsonProperty("max")]
        public DateTime Max { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum time value that can be allowed to select in TimePicker.
        /// </summary>
        [JsonProperty("min")]
        public DateTime Min { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);

        /// <summary>
        /// Specifies the text that is shown as a hint or placeholder until the user focuses or enter a value in TimePicker. The property is depending on the FloatLabelType property.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null;

        /// <summary>
        /// Specifies a boolean value whether the TimePicker allows the user to change the text.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("readonly")]
        public bool Readonly { get; set; } = false;

        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the popup list or
        /// the given value is not present in the popup list.
        /// </summary>
        [JsonProperty("scrollTo")]
        public DateTime ScrollTo { get; set; }

        /// <summary>
        /// Specifies whether to show or hide the clear icon.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("showClearButton")]
        public bool ShowClearButton { get; set; } = true;

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the popup list.
        /// </summary>
        [DefaultValue(30)]
        [JsonProperty("step")]
        public int Step { get; set; } = 30;

        /// <summary>
        /// Specifies the TimePicker to act as strict. So that, it allows to enter only a valid time value within a specified range or else it will resets to previous value.
        /// <para> By default, StrictMode is in false. It allows invalid or out-of-range time value with highlighted error class.</para>
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty("strictMode")]
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the value of the TimePicker. The value is parsed based on the culture specific time format.
        /// </summary>
        [JsonProperty("value")]
        public T Value { get; set; }

        /// <summary>
        /// Specifies the width of the TimePicker component.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty("width")]
        public object Width { get; set; } = null;

        /// <summary>
        /// specifies the z-index value of the timePicker popup element.
        /// </summary>
        [DefaultValue(1000)]
        [JsonProperty("zIndex")]
        public int ZIndex { get; set; } = 1000;
    }
}
