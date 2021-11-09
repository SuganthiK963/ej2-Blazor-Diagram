using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Calendars.Internal
{
    /// <summary>
    /// The Calendar base is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    /// <typeparam name="TValue">Soecifies the type of CalendarBaseRender.</typeparam>
    public partial class CalendarBaseRender<TValue> : CalendarBase<TValue>
    {
        protected override string RootClass { get; set; }

        private DateTime TodayDate { get; set; }

        private string HeaderTitle { get; set; }

        private string TitleClass { get; set; }

        private bool ContentElement { get; set; }

        private string ContentElementClass { get; set; }

        private bool IsSelect { get; set; }

        private bool IsCellClicked { get; set; }

        private bool IsDeviceMode { get; set; }

        [CascadingParameter]
        internal CalendarBase<TValue> Parent { get; set; }

        /// <summary>
        /// Specifies the option to enable the multiple dates selection of the calendar.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool MultiSelection { get; set; }

        /// <summary>
        /// Specifies the cell click event.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public EventCallback<CellDetails> CellClickHandler { get; set; }

        /// <summary>
        /// Specifies whether the current day is focused or not.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public bool IsFocusTodayCell { get; set; } = true;

        /// <summary>
        /// Specifies current date value.
        /// </summary>
        /// <exclude/>
        [Parameter]
        public TValue CurrentDateValue { get; set; }

        ///// <summary>
        ///// Specifies the CSS class name that can be appended with the root element of the Calendar. One or more custom CSS classes can be added to a Calendar.
        ///// </summary>
        ///// <exclude/>
        //[Parameter]
        //public string CssClass { get; set; } = string.Empty;

        ///// <summary>
        ///// Specifies a boolean value that indicates whether the Calendar allows the user to interact with it.
        ///// </summary>
        //[Parameter]
        //public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets multiple selected dates of the calendar.
        /// </summary>
        [Parameter]
        public DateTime[] MultiValues { get; set; }

        private DateTime[] CalendarBase_MultiValues { get; set; }

        /// <summary>
        /// Gets or sets a callback of value.
        /// </summary>
        [Parameter]
        public EventCallback<DateTime[]> MultiValuesChanged { get; set; }

        internal CalendarDayCell<TValue> CalDayCell { get; set; }

        private AnimationSettings Animate { get; set; } = new AnimationSettings() { Duration = 400, Name = "ZoomIn", Delay = 0 };

        internal DateTime CurrentDate { get; set; }

        private Type PropertyType { get; set; }

        private int CellsCount { get; set; }

        private int WeekNumCell { get; set; } = 41;

        private int NumberCell { get; set; } = 35;

        private int OtherMonthCell { get; set; } = 6;

        private int Row { get; set; }

        private int RowIterator { get; set; }

        private int Count { get; set; }

        private string RowEleClass { get; set; }

        private DateTime LocalDate { get; set; }

        private int NumCells { get; set; }

        private bool IsNavigation { get; set; }

        private CalendarView CalendarView { get; set; }

        private List<DateTime> LocalMainDate { get; set; }

        private string ContentHeader { get; set; }

        /// <summary>
        /// Gets or sets a previous icon state.
        /// </summary>
        [Parameter]
        public string PrevIconClass { get; set; }

        /// <summary>
        /// Gets or sets a next icon state.
        /// </summary>
        [Parameter]
        public string NextIconClass { get; set; }

        private Dictionary<string, object> PrevIconAttr { get; set; } = new Dictionary<string, object>();

        private Dictionary<string, object> NextIconAttr { get; set; } = new Dictionary<string, object>();

        private Dictionary<string, object> RowAttr { get; set; } = new Dictionary<string, object>();

        private Dictionary<string, object> StyleDisplayNone { get; set; } = new Dictionary<string, object>() { { "style", "display:none;" } };

        internal string TodayEleClass { get; set; }

        private string TodayEleContent { get; set; }

        private bool IsKeyboardSelect { get; set; }

        private bool IsClientChanged { get; set; }
    }
}
