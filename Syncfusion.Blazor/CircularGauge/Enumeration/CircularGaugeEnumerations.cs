namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Specifies the direction of the circular gauge.
    /// </summary>
    public enum GaugeDirection
    {
        /// <summary>
        /// Renders the axis in clock wise direction.
        /// </summary>
        ClockWise,

        /// <summary>
        /// Renders the axis in anti-clock wise direction.
        /// </summary>
        AntiClockWise,
    }

    /// <summary>
    /// Defines the position of the axis range and pointers.
    /// </summary>
    public enum PointerRangePosition
    {
        /// <summary>
        /// Specifies the default position of the range and pointer in the axis.
        /// </summary>
        Auto,

        /// <summary>
        /// Specifies the position of the range and pointer inside the axis
        /// </summary>
        Inside,

        /// <summary>
        /// Specifies the position of the range and pointer outside the axis.
        /// </summary>
        Outside,

        /// <summary>
        /// Specifies the position of the range and pointer on the axis.
        /// </summary>
        Cross,
    }

    /// <summary>
    /// Specifies the shape of a marker in circular gauge.
    /// </summary>
    public enum GaugeShape
    {
        /// <summary>
        /// Renders a marker shape as circle.
        /// </summary>
        Circle,

        /// <summary>
        /// Renders the marker shape as rectangle.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Renders the marker shape as triangle.
        /// </summary>
        Triangle,

        /// <summary>
        /// Renders the marker shape as diamond.
        /// </summary>
        Diamond,

        /// <summary>
        /// Renders the marker shape as inverted triangle.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Renders the marker shape as an image.
        /// </summary>
        Image,

        /// <summary>
        /// Renders the marker as text.
        /// </summary>
        Text,
    }

    /// <summary>
    /// Defines the type of pointer in the axis.
    /// </summary>
    public enum PointerType
    {
        /// <summary>
        /// Specifies the pointer type as needle.
        /// </summary>
        Needle,

        /// <summary>
        /// Specifies the pointer type as marker.
        /// </summary>
        Marker,

        /// <summary>
        /// Specifies the pointer type as range bar.
        /// </summary>
        RangeBar,
    }

    /// <summary>
    /// Specifies the axis label to be hidden in the axis of circular gauge.
    /// </summary>
    public enum HiddenLabel
    {
        /// <summary>
        /// Specifies the first label to be hidden in circular gauge.
        /// </summary>
        First,

        /// <summary>
        /// Specifies the last label to be hidden in circular gauge.
        /// </summary>
        Last,

        /// <summary>
        /// No labels will be hidden in circular gauge.
        /// </summary>
        None,
    }

    /// <summary>
    /// Defines the position of the axis ticks and labels.
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// Specifies the position of the tick line and axis label inside the axis.
        /// </summary>
        Inside,

        /// <summary>
        /// Specifies the position of the tick line and axis label outside the axis.
        /// </summary>
        Outside,

        /// <summary>
        /// Specifies the position of the tick line and axis label on the axis.
        /// </summary>
        Cross,
    }

    /// <summary>
    /// Specifies the alignment of the legend in circular gauge component.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Places the legend near the circular gauge with respect to the position of legend.
        /// </summary>
        Near,

        /// <summary>
        /// Places the legend at the center of the circular gauge with respect to the position of legend.
        /// </summary>
        Center,

        /// <summary>
        /// Places the legend far from the circular gauge with respect to the position of legend.
        /// </summary>
        Far,
    }

    /// <summary>
    /// Specifies the position of legend for ranges in circular gauge component.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// Specifies the legend to be placed at the top of the circular gauge.
        /// </summary>
        Top,

        /// <summary>
        /// Specifies the legend to be placed at the left of the circular gauge.
        /// </summary>
        Left,

        /// <summary>
        /// Specifies the legend to be placed at the bottom of the circular gauge.
        /// </summary>
        Bottom,

        /// <summary>
        /// Specifies the legend to be placed at the right of the circular gauge.
        /// </summary>
        Right,

        /// <summary>
        /// Specifies the legend to be placed based on the custom x and y location.
        /// </summary>
        Custom,

        /// <summary>
        /// Specifies the legend to be placed based on the available space.
        /// </summary>
        Auto,
    }

    /// <summary>
    /// Specifies the export type of circular gauge component.
    /// </summary>
    public enum ExportType
    {
        /// <summary>
        /// Specifies the rendered circular gauge to be exported in the PNG format.
        /// </summary>
        PNG,

        /// <summary>
        /// Specifies the rendered circular gauge to be exported in the JPEG format.
        /// </summary>
        JPEG,

        /// <summary>
        /// Specifies the rendered circular gauge to be exported in the svg format.
        /// </summary>
        SVG,

        /// <summary>
        /// Specifies the rendered circular gauge to be exported in the pdf format.
        /// </summary>
        PDF,
    }
}