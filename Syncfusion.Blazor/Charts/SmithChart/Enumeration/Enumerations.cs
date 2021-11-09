using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines render type of smithchart. They are
    ///  Impedance - Render a smithchart with Impedance type.
    ///  Admittance - Render a smithchart with Admittance type.
    /// </summary>
    public enum RenderType
    {
        /// <summary>
        /// Impedance - Render a smithchart with Impedance type.
        /// </summary>
        Impedance,

        /// <summary>
        /// Admittance - Render a smithchart with Admittance type.
        /// </summary>
        Admittance
    }

    /// <summary>
    /// Defines the Alignment. They are
    ///  Near - Align the element to the left.
    ///  Center - Align the element to the center.
    ///  Far - Align the element to the right.
    /// </summary>
    public enum SmithChartAlignment
    {
        /// <summary>
        /// Near - Align the element to the left.
        /// </summary>
        Near,

        /// <summary>
        /// Center - Align the element to the center.
        /// </summary>
        Center,

        /// <summary>
        /// Far - Align the element to the right.
        /// </summary>
        Far
    }

    /// <summary>
    /// Defines the label intersect action.
    /// </summary>
    public enum SmithChartLabelIntersectAction
    {
        /// <summary>
        /// Hide - hide the labels when intersecting.
        /// </summary>
        Hide,

        /// <summary>
        /// None - place all labels when intersecting.
        /// </summary>
        None
    }

    /// <summary>
    /// Defines axis label position.
    /// </summary>
    public enum AxisLabelPosition
    {
        /// <summary>
        /// Outside - axis label position is outside.
        /// </summary>
        Outside,

        /// <summary>
        /// Inside - axis label position is inside.
        /// </summary>
        Inside
    }

    /// <summary>
    /// Defines the shape of legend. They are
    ///  Circle - Renders a circle.
    ///  Rectangle - Renders a rectangle.
    ///  Triangle - Renders a triangle.
    ///  Diamond - Renders a diamond.
    ///  Pentagon - Renders a pentagon.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Shape
    {
        /// <summary>
        /// Circle - Renders a circle.
        /// </summary>
        [EnumMember(Value = "Circle")]
        Circle,

        /// <summary>
        /// Rectangle - Renders a rectangle.
        /// </summary>
        [EnumMember(Value = "Rectangle")]
        Rectangle,

        /// <summary>
        /// Triangle - Renders a triangle.
        /// </summary>
        [EnumMember(Value = "Triangle")]
        Triangle,

        /// <summary>
        /// Diamond - Renders a diamond.
        /// </summary>
        [EnumMember(Value = "Diamond")]
        Diamond,

        /// <summary>
        /// Pentagon - Renders a pentagon.
        /// </summary>
        [EnumMember(Value = "Pentagon")]
        Pentagon
    }
}