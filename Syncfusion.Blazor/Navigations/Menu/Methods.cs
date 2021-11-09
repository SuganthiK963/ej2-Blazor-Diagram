using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Navigations.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Menu is a graphical user interface that serve as navigation headers for your application.
    /// </summary>
    public partial class SfMenu<TValue> : SfMenuBase<TValue>
    {
        /// <summary>
        /// This method is used to get the index of the menu item in the Menu based on the argument.
        /// </summary>
        /// <param name = "item">Item be passed to get the index.</param>
        /// <param name = "isUniqueId">Set `true` if it is a unique id.</param>
        public List<int> GetItemIndex(TValue item, bool isUniqueId = false)
        {
            var itemtext = Utils.GetItemProperties<string, TValue>(item, isUniqueId ? Fields.ItemId : Fields.Text);
            return GetIndex(itemtext, Items, new List<int>(), isUniqueId);
        }

        /// <summary>
        /// Used to open the Menu in hamburger mode.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Open()
        {
            await HeaderClickHandler(true);
        }

        /// <summary>
        /// Used to open the Menu in hamburger mode.
        /// </summary>
        public async Task OpenAsync()
        {
            await Open();
        }

        /// <summary>
        /// Closes the Menu if it is opened in hamburger mode.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Close()
        {
            await HeaderClickHandler();
        }

        /// <summary>
        /// Closes the Menu if it is opened in hamburger mode.
        /// </summary>
        public async Task CloseAsync()
        {
            await Close();
        }
    }
}
