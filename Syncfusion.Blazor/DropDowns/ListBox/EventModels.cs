using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Interface for change event.
    /// </summary>
    public class ListBoxChangeEventArgs<TValue, TItem>
    {
        /// <summary>
        /// Specifies the list data.
        /// </summary>
        public IEnumerable<TItem> Items { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the selected values.
        /// </summary>
        public TValue Value { get; set; }
    }

    /// <summary>
    /// Interface for drag and drop event.
    /// </summary>
    public class DragEventArgs<T>
    {
        /// <summary>
        /// Used to prevent the drag action.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the dropped ListBox data.
        /// </summary>
        public SourceDestinationModel<T> Destination { get; set; }

        /// <summary>
        /// Specifies the dragged items data.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Specifies the dropped ListBox data.
        /// </summary>
        public SourceDestinationModel<T> Source { get; set; }

        /// <summary>
        /// Specifies the drag item index.
        /// </summary>
        public double PreviousIndex { get; set; }

        /// <summary>
        /// Specifies the drop item index.
        /// </summary>
        public double CurrentIndex { get; set; }
    }

    /// <summary>
    /// Interface for drag and drop event.
    /// </summary>
    public class SourceDestinationModel<T>
    {
        /// <summary>
        /// Specifies the data after the drag and drop operation.
        /// </summary>
        public IEnumerable<T> CurrentData { get; set; }

        /// <summary>
        /// Specifies the data before the drag and drop operation.
        /// </summary>
        public IEnumerable<T> PreviousData { get; set; }
    }

    /// <summary>
    /// Interface for drop event.
    /// </summary>
    public class DropEventArgs<T>
    {
        /// <summary>
        /// Used to prevent the drop action.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Specifies the drop item index.
        /// </summary>
        public double CurrentIndex { get; set; }

        /// <summary>
        /// Specifies the dragged items data.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Specifies the drag item index.
        /// </summary>
        public double PreviousIndex { get; set; }

        /// <summary>
        /// Specifies the target element.
        /// </summary>
        public ElementReference Target { get; set; }
    }

    /// <summary>
    /// Interface for before item render event.
    /// </summary>
    public class BeforeItemRenderEventArgs<T>
    {
        /// <summary>
        /// Specifies the item data.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }
}