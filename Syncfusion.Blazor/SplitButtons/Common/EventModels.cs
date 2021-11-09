using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.StockChart")]

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// Interface for before open / close event.
    /// </summary>
    public class BeforeOpenCloseMenuEventArgs
    {
        /// <summary>
        /// Used to prevent dropdown menu open.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the button or dropdown element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the Event.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Specifies the dropdown items.
        /// </summary>
        public List<DropDownMenuItem> Items { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for before item render / select event.
    /// </summary>
    public class MenuEventArgs
    {
        /// <summary>
        /// Specifies the selected list element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the selected dropdown item.
        /// </summary>
        public DropDownMenuItem Item { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the selected dropdown item.
        /// </summary>
        public EventArgs Event { get; set; }
    }

    /// <summary>
    /// Interface for open/close event.
    /// </summary>
    public class OpenCloseMenuEventArgs
    {
        /// <summary>
        /// Specifies the dropdown element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies the button or dropdown items.
        /// </summary>
        public List<DropDownMenuItem> Items { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Interface for progress event.
    /// </summary>
    public class ProgressEventArgs
    {
        /// <summary>
        /// Indicates the current duration of the progress.
        /// </summary>
        public double CurrentDuration { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates the current state of progress in percentage.
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// Specifies the interval.
        /// </summary>
        public double Step { get; set; }
    }
}