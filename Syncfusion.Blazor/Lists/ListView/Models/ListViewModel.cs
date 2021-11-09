using System.Collections.Generic;

namespace Syncfusion.Blazor.Lists.Internal
{
    internal class CheckedItemDetails
    {
        /// <summary>
        /// It is used to denote checked Element id details.
        /// </summary>
        public string[] ElementId { get; set; }

        /// <summary>
        /// It is used to denote the datasource key.
        /// </summary>
        public string Key { get; set; }
    }

    /// <exclude/>
    /// <summary>
    /// Referring given li element data details.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class ListElementDetails<T>
    {
        /// <summary>
        /// Denotes the nested child data of the list item.
        /// </summary>
        public IEnumerable<T> Child { get; set; }

        /// <summary>
        /// Denotes the id of the item element.
        /// </summary>
        public List<string> Id { get; set; }

        /// <summary>
        /// Denotes the data of the list item.
        /// </summary>
        public T ItemData { get; set; }

        /// <summary>
        /// Denotes the list item text.
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// Denotes index of the list element.
        /// </summary>
        public List<int> Index { get; set; }
    }
}
