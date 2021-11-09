using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The Calendar is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        ///// <summary>
        ///// Specifies the id of the Calendar component.
        ///// </summary>
        //[Parameter]
        //public string ID { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the expression for defining the values of the bound.
        /// </summary>
        [Parameter]
        public Expression<Func<DateTime[]>> ValuesExpression { get; set; }

        ///// <summary>
        ///// Enable or disable rendering component in right to left direction.
        ///// </summary>
        //[Parameter]
        //public bool EnableRtl { get; set; }

        /// <summary>
        /// Specifies the boolean value whether the calendar allows the user to select the multiple dates.
        /// </summary>
        [Parameter]
        public bool IsMultiSelection { get; set; }

        /// <summary>
        /// Specifies the tab order of the component.
        /// </summary>
        [Parameter]
        public int TabIndex { get; set; }

        /// <summary>
        /// Sets multiple selected dates of the calendar.
        /// </summary>
        [Parameter]
        public DateTime[] Values { get; set; }

        private DateTime[] Calendar_Values { get; set; }

        /// <summary>
        /// Specifies the callback to trigger when the values changes.
        /// </summary>
        [Parameter]
        public EventCallback<DateTime[]> ValuesChanged { get; set; }

        /// <summary>
        /// You can add the additional html attributes such as styles, class etc., to the root element.
        /// <para>If you configured both property and equivalent html attribute, then the component considers the property value.</para>
        /// </summary>
        [Parameter]
        public Dictionary<string, object> HtmlAttributes { get; set; }

        ///// <summary>
        ///// Specifies the CSS class name that can be appended with the root element of the Calendar. One or more custom CSS classes can be added to a Calendar.
        ///// </summary>
        //[Parameter]
        //public string CssClass { get; set; } = string.Empty;

        private string Calendar_CssClass { get; set; } = string.Empty;

        ///// <summary>
        ///// Specifies a boolean value that indicates whether the Calendar allows the user to interact with it.
        ///// </summary>
        //[Parameter]
        //public bool Enabled { get; set; } = true;

        internal bool Calendar_Enabled { get; set; } = true;
    }
}
