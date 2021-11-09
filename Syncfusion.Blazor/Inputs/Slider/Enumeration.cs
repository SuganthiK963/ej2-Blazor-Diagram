using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    ///  Holds slider component's different orientation options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SliderOrientation
    {
        /// <summary>
        /// Specifies the slider rendering position as Horizontal.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Specifies the slider rendering position as Vertical.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Holds slider component's different options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SliderType
    {
        /// <summary>
        /// Specifies the slider type as Default.
        /// </summary>
        Default,

        /// <summary>
        /// Specifies the slider type as MinRange.
        /// </summary>
        MinRange,

        /// <summary>
        /// Specifies the slider type as Range.
        /// </summary>
        Range
    }

    /// <summary>
    /// Holds slider component's Ticks Placement options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Placement
    {
        /// <summary>
        /// This enum value will restrict the ticks element rendering.
        /// </summary>
        None,

        /// <summary>
        /// This is used to rendering tiks element before the slider track.
        /// </summary>
        Before,

        /// <summary>
        /// This is used to rendering tiks element after the slider track.
        /// </summary>
        After,

        /// <summary>
        /// This is used to rendering tiks element before and after the slider track.
        /// </summary>
        Both
    }

    /// <summary>
    /// Holds slider component's Tooltip Placement options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TooltipPlacement
    {
        /// <summary>
        /// This is used to render the tooltip element before the slider track.
        /// </summary>
        Before,

        /// <summary>
        /// This is used to render the tooltip element after the slider track.
        /// </summary>
        After
    }

    /// <summary>
    /// Holds slider component's Tooltip ShowOn options.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TooltipShowOn
    {
        /// <summary>
        /// This is used to display the tooltip while click / focus the slider handle elemenet.
        /// </summary>
        Auto,

        /// <summary>
        /// This is used to display the tooltip while focus the slider handle elemenet.
        /// </summary>
        Focus,

        /// <summary>
        /// This is used to display the tooltip while hover the slider handle elemenet.
        /// </summary>
        Hover,

        /// <summary>
        /// This is used to display the tooltip until new actions occurred in slider elemenet.
        /// </summary>
        Always
    }
}