using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Syncfusion.Blazor.DropDowns
{
    /// <summary>
    /// Determines on which filter type, the component needs to be considered on search action.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FilterType
    {
        /// <summary>
        /// Defines the StartsWith filter type.
        /// </summary>
        [EnumMember(Value = "StartsWith")]
        StartsWith,

        /// <summary>
        /// Defines the EndsWith filter type.
        /// </summary>
        [EnumMember(Value = "EndsWith")]
        EndsWith,

        /// <summary>
        /// Defines the Contains filter type.
        /// </summary>
        [EnumMember(Value = "Contains")]
        Contains,
    }

    /// <summary>
    /// Configures visibility mode for component interaction.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VisualMode
    {
        /// <summary>
        /// Defines the Default visual mode.
        /// </summary>
        [EnumMember(Value = "Default")]
        Default,

        /// <summary>
        /// Defines the Delimiter visual mode.
        /// </summary>
        [EnumMember(Value = "Delimiter")]
        Delimiter,

        /// <summary>
        /// Defines the Box visual mode.
        /// </summary>
        [EnumMember(Value = "Box")]
        Box,

        /// <summary>
        /// Defines the CheckBox visual mode.
        /// </summary>
        [EnumMember(Value = "CheckBox")]
        CheckBox,
    }

    /// <summary>
    /// Specifies the SortOrder to sort the data source.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        /// <summary>
        /// Defines the None sort order.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Defines the Ascending sort order.
        /// </summary>
        [EnumMember(Value = "Ascending")]
        Ascending,

        /// <summary>
        /// Defines the Descending sort order.
        /// </summary>
        [EnumMember(Value = "Descending")]
        Descending,
    }

    /// <summary>
    ///  Highlight the searched characters on suggested list items.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HightLightType
    {
        /// <summary>
        /// Defines the Contains highlight type.
        /// </summary>
        [EnumMember(Value = "Contains")]
        Contains,

        /// <summary>
        /// Defines the StartsWith highlight type.
        /// </summary>
        [EnumMember(Value = "StartsWith")]
        StartsWith,

        /// <summary>
        /// Defines the EndsWith highlight type.
        /// </summary>
        [EnumMember(Value = "EndsWith")]
        EndsWith,
    }

    /// <summary>
    /// Defines the SearchType.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SearchType
    {
        /// <summary>
        /// Defines the StartsWith search type.
        /// </summary>
        [EnumMember(Value = "StartsWith")]
        StartsWith,

        /// <summary>
        /// Defines the Equal search type.
        /// </summary>
        [EnumMember(Value = "Equal")]
        Equal,
    }

    /// <summary>
    /// Defines the checkbox position of List Box.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CheckBoxPosition
    {
        /// <summary>
        /// Defines the Left check box position.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Defines the Right check box position.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
    }

    /// <summary>
    /// Defines the selection mode of List Box.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SelectionMode
    {
        /// <summary>
        /// Defines the Multiple selection mode.
        /// </summary>
        [EnumMember(Value = "Multiple")]
        Multiple,

        /// <summary>
        /// Defines the Single selection mode.
        /// </summary>
        [EnumMember(Value = "Single")]
        Single,
    }

    /// <summary>
    /// Defines the toolbar position of List Box.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ToolBarPosition
    {
        /// <summary>
        /// Defines the Left tool bar position.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Defines the Right tool bar position.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
    }
}
