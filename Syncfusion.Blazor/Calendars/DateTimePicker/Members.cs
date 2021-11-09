using System;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The DateTimePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    /// <typeparam name="TValue">Specifies the type of SfDateTimePicker.</typeparam>
#pragma warning disable CA1501 // Mark members as hierarchy
    public partial class SfDateTimePicker<TValue> : SfDatePicker<TValue>
#pragma warning restore CA1501 // Mark members as hierarchy
    {
        /// <summary>
        /// Specifies the scroll bar position, if there is no value is selected in the DateTimePicker popup list or
        /// the given value is not present in the DateTimePicker popup list.
        /// </summary>
        [Parameter]
        public DateTime? ScrollTo { get; set; }

        private DateTime? DateTimeScrollTo { get; set; }

        /// <summary>
        /// Gets or sets the maximum time value that can be allowed to select in DateTimePicker.
        /// </summary>
        [Parameter]
        public override DateTime Max { get; set; } = new DateTime(2099, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets or sets the minimum time value that can be allowed to select in DateTimePicker.
        /// </summary>
        [Parameter]
        public override DateTime Min { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00);

        /// <summary>
        /// Specifies the time interval between the two adjacent time values in the time popup list .
        /// </summary>
        [Parameter]
        public int Step { get; set; } = 30;

        private int DateTimeStep { get; set; }

        /// <summary>
        /// Specifies the format of the time value that to be displayed in time popup list.
        /// </summary>
        [Parameter]
        public string TimeFormat { get; set; }

        /// <summary>
        /// Parent component of DateTimePicker.
        /// </summary>
        /// <exclude />
        [CascadingParameter(Name = "InPlaceEditor")]
        protected dynamic DateTimePickerParent { get; set; }
    }
}
