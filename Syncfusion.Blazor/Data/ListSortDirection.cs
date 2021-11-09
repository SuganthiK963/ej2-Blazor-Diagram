using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    ///  Specifies the direction of a sort operation.
    /// </summary>
    public enum ListSortDirection
    {
        /// <summary>
        /// Sorts in ascending order.
        /// </summary>
        Ascending = 0,

        /// <summary>
        /// Sorts in descending order.
        /// </summary>
        Descending = 1,
    }

    /// <summary>
    /// Sepcifies the sort order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// No sort order.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Sorts in ascending order.
        /// </summary>
        [EnumMember(Value = "Ascending")]
        Ascending,

        /// <summary>
        /// Sorts in descending order.
        /// </summary>
        [EnumMember(Value = "Descending")]
        Descending,
    }

    /// <summary>
    /// Defines the sort column.
    /// </summary>
    public class SortedColumn
    {
        private SortOrder direction = SortOrder.Ascending;

        /// <summary>
        /// Specifies the field to sort.
        /// </summary>
        [JsonProperty("field")]
        [DefaultValue(null)]
        public string Field { get; set; }

        /// <summary>
        /// Specifies the sort order.
        /// </summary>
        [JsonProperty("direction")]
        [DefaultValue(SortOrder.Ascending)]
        [JsonConverter(typeof(StringEnumConverter))]

        public SortOrder Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// Gets the sort comparer
        /// </summary>
        public object Comparer { get; set; }
    }
}
