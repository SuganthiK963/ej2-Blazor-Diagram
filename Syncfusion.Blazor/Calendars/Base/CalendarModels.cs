using Microsoft.AspNetCore.Components.Web;
using System;

namespace Syncfusion.Blazor.Calendars.Internal
{
    /// <summary>
    /// Specifies the key actions.
    /// </summary>
    public class KeyActions
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        public string SelectDate { get; set; }

        /// <summary>
        /// Gets or sets the focused date.
        /// </summary>
        public string FocusedDate { get; set; }

        /// <summary>
        /// Gets or sets the class list.
        /// </summary>
        public string ClassList { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the mouse event arguments.
        /// </summary>
        public MouseEventArgs Events { get; set; }

        /// <summary>
        /// Gets or sets the class list of target.
        /// </summary>
        public string TargetClassList { get; set; }

        /// <summary>
        /// Gets or sets the key code.
        /// </summary>
        public int KeyCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a left calendar.
        /// </summary>
        public bool IsLeftCalendar { get; set; }
    }

    /// <summary>
    /// Specifies the list options.
    /// </summary>
    /// <typeparam name="T">.</typeparam>
    public class ListOptions<T>
    {
        /// <summary>
        /// Gets or sets the date and time value.
        /// </summary>
        public T DateTimeValue { get; set; }

        /// <summary>
        /// Gets or sets the item date.
        /// </summary>
        public string ItemData { get; set; }

        /// <summary>
        /// Gets or sets the list class.
        /// </summary>
        public string ListClass { get; set; }
    }

    /// <summary>
    /// Specifies the preset options.
    /// </summary>
    public class PresetsOptions
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Gets or sets the list class.
        /// </summary>
        public string ListClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is a custom range.
        /// </summary>
        public bool IsCustomRange { get; set; }
    }
}
