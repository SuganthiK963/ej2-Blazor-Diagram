using System.Collections.Generic;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// SplitButton component has primary and secondary button. Primary button is used to select 
    /// default action and secondary button is used to toggle contextual overlays for displaying list of 
    /// action items. It can contain both text and images.
    /// </summary>
    public partial class SfSplitButton : SfBaseComponent
    {
        /// <summary>
        /// Adds a new item to the menu. By default, new item appends to the list as the last item,
        /// but you can insert based on the text parameter.
        /// </summary>
        /// <param name="items">Specifies the list of items to be added.</param>
        /// <param name="text">Specifies the existing item text. If specified, adds the items of the collection before this item.
        /// If not specified, adds the items of the collection to the end of the list.</param>
        /// <param name="isUniqueId">Set true if text parameter is a unique id.</param>
        public void AddItems(List<DropDownMenuItem> items, string text = null, bool isUniqueId = false)
        {
            secondryBtn.AddItems(items, text, isUniqueId);
        }

        /// <summary>
        /// Removes the items from the menu.
        /// </summary>
        /// <param name="items">Specifies the list of items to be removed.</param>
        /// <param name="isUniqueId">Set true if text parameter is a unique id.</param>
        public void RemoveItems(List<string> items, bool isUniqueId = false)
        {
            secondryBtn.RemoveItems(items, isUniqueId);
        }

        /// <summary>
        /// To open/close Split Button popup based on current state of the Split Button.
        /// </summary>
        public void Toggle()
        {
            secondryBtn.Toggle();
        }
    }
}