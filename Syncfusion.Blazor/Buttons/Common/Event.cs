using System;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Arguments for `ValueChange` event.
    /// </summary>
    public class ChangeEventArgs<TChecked>
    {
        /// <summary>
        /// Returns the checked value of the component.
        /// </summary>
        public TChecked Checked { get; set; }

        /// <summary>
        /// Returns the event parameters of the component.
        /// </summary>
        public MouseEventArgs Event { get; set; }
    }

    /// <summary>
    /// Interface for Radio Button change event arguments.
    /// </summary>
    public class ChangeArgs<TChecked>
    {
        /// <summary>
        /// Returns the value of the RadioButton.
        /// </summary>
        public TChecked Value { get; set; }

        /// <summary>
        /// Returns the event parameters of the RadioButton.
        /// </summary>
        public EventArgs Event { get; set; }
    }
}