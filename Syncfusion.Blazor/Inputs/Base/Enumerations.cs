using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Defines floating label type of the input and decides how the label should float on the input.
    /// </summary>
    public enum FloatLabelType
    {
        /// <summary>
        /// The placeholder text should not be float ever.
        /// </summary>
        [EnumMember(Value = "Never")]
        Never,

        /// <summary>
        /// The placeholder text floats above the input always.
        /// </summary>
        [EnumMember(Value = "Always")]
        Always,

        /// <summary>
        /// The placeholder text floats above the input while focusing or enter a value in input.
        /// </summary>
        [EnumMember(Value = "Auto")]
        Auto,
    }

    /// <summary>
    /// Specifies whether the browser is allow to automatically enter or select a value for the textbox.
    /// </summary>
    public enum AutoComplete
    {
        /// <summary>
        /// Specifies that autocomplete is enabled.
        /// </summary>
        [EnumMember(Value = "on")]
        On,

        /// <summary>
        /// Specifies that autocomplete is disabled.
        /// </summary>
        [EnumMember(Value = "off")]
        Off
    }

    /// <summary>
    /// Define the attribute specifies the type of textbox.
    /// </summary>
    public enum InputType
    {
        /// <summary>
        /// The text field is a single-line text.
        /// </summary>
        [EnumMember(Value = "text")]
        Text,

        /// <summary>
        /// Set an e-mail type to TextBox component.
        /// </summary>
        [EnumMember(Value = "email")]
        Email,

        /// <summary>
        /// Set the password type to TextBox component.
        /// </summary>
        [EnumMember(Value = "password")]
        Password,

        /// <summary>
        /// Set the number type to TextBox component.
        /// </summary>
        [EnumMember(Value = "number")]
        Number,

        /// <summary>
        /// Set the search type to TextBox component.
        /// </summary>
        [EnumMember(Value = "search")]
        Search,

        /// <summary>
        /// Set the tel type to TextBox component.
        /// </summary>
        [EnumMember(Value = "tel")]
        Tel,

        /// <summary>
        /// Set the URL type to TextBox component.
        /// </summary>
        [EnumMember(Value = "url")]
        URL
    }
}