using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Specifies the option to expand single or multiple panel at a time.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExpandMode
    {
        /// <summary>
        /// Denotes single panel expansion.
        /// </summary>
        [EnumMember(Value = "Single")]
#pragma warning disable CA1720 // Identifier contains type name
        Single,
#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Denotes multiple panel expansion.
        /// </summary>
        [EnumMember(Value = "Multiple")]
        Multiple
    }
}