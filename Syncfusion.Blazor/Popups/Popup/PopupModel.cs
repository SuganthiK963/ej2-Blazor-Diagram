using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Defines the types of target.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        /// <summary>
        /// Defines the TargetType as Relative.
        /// </summary>
        Relative,

        /// <summary>
        /// Defines the TargetType as Container.
        /// </summary>
        Container
    }

    /// <summary>
    /// Defines the types of collision.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CollisionType
    {
        /// <summary>
        /// Defines the CollisionType as None.
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// Defines the CollisionType as Flip.
        /// </summary>
        [EnumMember(Value = "flip")]
        Flip,

        /// <summary>
        /// Defines the CollisionType as Fit.
        /// </summary>
        [EnumMember(Value = "fit")]
        Fit
    }

    /// <summary>
    /// Provides data for the PopupModel.
    /// </summary>
    public class PopupModel
    {
        /// <summary>
        /// Specifies the collision handler settings of the component.
        /// </summary>
        [JsonPropertyName("collision")]
        public CollisionAxis Collision { get; set; }

        /// <summary>
        /// Specifies the popup element position, respective to the relative element.
        /// </summary>
        [JsonPropertyName("position")]
        public PositionDataModel Position { get; set; }

        /// <summary>
        /// Specifies the element selector for relative container element of the popup element .Based on the relative element, popup element will be positioned.
        /// </summary>
        [JsonPropertyName("relateTo")]
        public ElementReference RelateTo { get; set; }

        /// <summary>
        /// Specifies the relative element type of the component.
        /// </summary>
        [JsonPropertyName("targetType")]
        public TargetType TargetType { get; set; } = TargetType.Container;

        /// <summary>
        /// specifies the popup element offset-x value, respective to the relative element.
        /// </summary>
        public int OffsetX { get; set; }

        /// <summary>
        /// specifies the popup element offset-y value, respective to the relative element.
        /// </summary>
        public int OffsetY { get; set; }
    }

    /// <summary>
    ///  Provides information about a CollisionAxis.
    /// </summary>
    public class CollisionAxis
    {
        /// <summary>
        /// Specify the collision handler for a X-Axis.
        /// </summary>
        [JsonPropertyName("X")]
        public CollisionType X { get; set; }

        /// <summary>
        /// specify the collision handler for a Y-Axis.
        /// </summary>
        [JsonPropertyName("Y")]
        public CollisionType Y { get; set; }
    }

    /// <summary>
    ///  Provides information about a PositionDataModel.
    /// </summary>
    public class PositionDataModel
    {
        /// <summary>
        /// Specify the offset left value.
        /// </summary>
        [JsonPropertyName("X")]
        public string X { get; set; } = null;

        /// <summary>
        /// Specify the offset top value.
        /// </summary>
        [JsonPropertyName("Y")]
        public string Y { get; set; } = null;
    }
}