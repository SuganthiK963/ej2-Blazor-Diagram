using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// Interface for Split Button click event arguments.
    /// </summary>
    public class ClickEventArgs
    {
        /// <summary>
        /// Specifies the primary split button element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// Specifies name of the event.
        /// </summary>
        public string Name { get; set; }
    }
}