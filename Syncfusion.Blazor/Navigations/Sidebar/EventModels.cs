using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Interface for open and close events.
    /// </summary>
    public class EventArgs
    {
        /// <summary>
        /// Determines whether the current action needs to be prevented or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the element reference.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Returns the event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines the boolean that returns true when the Sidebar is closed by user interaction, otherwise returns false.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies the clientY position of the target element.
        /// </summary>
        public double? Top { get; set; }

        /// <summary>
        /// Specifies the clientX position of the target element.
        /// </summary>
        public double? Left { get; set; }

    }

    /// <summary>
    /// Defines the event arguments for the change event.
    /// </summary>
    public class ChangeEventArgs
    {
        /// <summary>
        /// Returns the element reference.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Returns event name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines the boolean that returns true when the Sidebar is closed by user interaction, otherwise returns false.
        /// </summary>
        public bool IsInteracted { get; set; }
    }

    /// <summary>
    /// Interface for persistence values.
    /// </summary>
    internal class PersistenceValues
    {
        /// <summary>
        /// Gets or sets the Sidebar component is open or close.
        /// </summary>
        public bool IsOpen { get; set; }
    }
}