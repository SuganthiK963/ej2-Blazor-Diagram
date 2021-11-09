using System.Collections.Generic;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// DropDownButton component is used to toggle contextual overlays for displaying list of action items.
    /// It can contain both text and images.
    /// </summary>
    public partial class SfDropDownButton : SfBaseComponent
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
            if (string.IsNullOrEmpty(text))
            {
                Items.AddRange(items);
            }
            else
            {
                var index = -1;
                for (var i = 0; i < Items.Count; i++)
                {
                    if (text == (isUniqueId ? Items[i].Id : Items[i].Text))
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                {
                    Items.InsertRange(Items.Count, items);
                }
                else
                {
                    Items.InsertRange(index, items);
                }
            }
        }

        /// <summary>
        /// Removes the items from the menu.
        /// </summary>
        /// <param name="items">Specifies the list of items to be removed.</param>
        /// <param name="isUniqueId">Set true if text parameter is a unique id.</param>
        public void RemoveItems(List<string> items, bool isUniqueId = false) 
        {
            if (items != null)
            {
                foreach (var text in items)
                {
                    for (var i = 0; i < Items.Count; i++)
                    {
                        if (text == (isUniqueId ? Items[i].Id : Items[i].Text))
                        {
                            Items.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To open/close DropDownButton popup based on current state of the DropDownButton.
        /// </summary>
        public void Toggle()
        {
            if (renderPopup)
            {
                Close();
            }
            else
            {
                Open();
            }
            StateHasChanged();
        }
    }
}