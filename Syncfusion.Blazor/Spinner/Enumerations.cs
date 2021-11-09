using System.Runtime.Serialization;

namespace Syncfusion.Blazor.Spinner
{
    /// <summary>
    /// Specify the theme that the Spinner to be rendered.
    /// </summary>
    public enum SpinnerType
    {
        /// <summary>
        /// Default value is None
        /// Automatically, picks the application-level theme.
        /// </summary>
        [EnumMember(Value = "None")]
        None,

        /// <summary>
        /// Choose to render the Material spinner.
        /// </summary>
        [EnumMember(Value = "Material")]
        Material,

        /// <summary>
        /// Choose to render the Bootstrap 4 spinner.
        /// </summary>
        [EnumMember(Value = "Bootstrap4")]
        Bootstrap4,

        /// <summary>
        /// Choose to render the Bootstrap 5 spinner.
        /// </summary>
        [EnumMember(Value = "Bootstrap5")]
        Bootstrap5,

        /// <summary>
        /// Choose to render the Fabric spinner.
        /// </summary>
        [EnumMember(Value = "Fabric")]
        Fabric,

        /// <summary>
        /// Choose to render the Bootstrap spinner.
        /// </summary>
        [EnumMember(Value = "Bootstrap")]
        Bootstrap,

        /// <summary>
        /// Choose to render the High-contrast spinner.
        /// </summary>
        [EnumMember(Value = "HighContrast")]
        HighContrast,

        /// <summary>
        /// Choose to render the Tailwind spinner.
        /// </summary>
        [EnumMember(Value = "Tailwind")]
        Tailwind
    }
}