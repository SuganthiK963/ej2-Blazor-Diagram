using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// Interface for Chip Events.
    /// </summary>
    public class ChipEventArgs
    {
        /// <summary>
        /// It denotes whether the item can be deleted or not.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// It denotes the deleted Item element.
        /// </summary>
        public ElementReference Element { get; set; }

        /// <summary>
        /// It denotes the deleted item index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// It denotes whether the clicked item is selected or not.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// It denotes the deleted item text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// It denotes the deleted item value.
        /// </summary>
        public string Value { get; set; }
    }
}