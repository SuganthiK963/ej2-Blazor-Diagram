namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the position of the legend.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// Specifies to place the legend based on width and height.
        /// </summary>
        Auto,
        /// <summary>
        /// Specifies to place the legend below of the component.
        /// </summary>
        Bottom,
        /// <summary>
        /// Specifies to place the legend above of the component.
        /// </summary>
        Top,
        /// <summary>
        /// Specifies to place the legend on the left of the component.
        /// </summary>
        Left,
        /// <summary>
        /// Specifies to place the legend on the right of the component.
        /// </summary>
        Right,
        /// <summary>
        /// Specifies the legend position based on given X and Y coordinate.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the export type for the component.
    /// </summary>
    public enum ExportType
    {
        /// <summary>
        /// Specifies the rendered component to be exported in the PNG format.
        /// </summary>
        PNG,
        /// <summary>
        /// Specifies the rendered component to be exported in the JPEG format.
        /// </summary>
        JPEG,
        /// <summary>
        /// Specifies the rendered component to be exported in the SVG format.
        /// </summary>
        SVG,
        /// <summary>
        /// Specifies the rendered component to be exported in the PDF format.
        /// </summary>
        PDF
    }

    /// <summary>
    /// Defines the text overflow of the title.
    /// </summary>
    public enum TextOverflow
    {
        /// <summary>
        /// Specifies that the text will shown as it is.
        /// </summary>
        None,
        /// <summary>
        /// Specifies that the text will trim if exceeded the defined margins.
        /// </summary>
        Trim,
        /// <summary>
        /// Specifies to wrap the text if exceed the defined margins.
        /// </summary>
        Wrap
    }

    /// <summary>
    /// Specifies the alignment of the elements in the chart.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Defines the alignment is near of the chart.
        /// </summary>
        Near,
        /// <summary>
        /// Defines the alignment is at center of the chart.
        /// </summary>
        Center,
        /// <summary>
        /// Defines the alignment is far from the chart.
        /// </summary>
        Far
    }

    /// <summary>
    /// Defines the shape of legend in the component.
    /// </summary>
    public enum LegendShape
    {
        /// <summary>
        /// Defines the legend shape as circle.
        /// </summary>
        Circle,
        /// <summary>
        /// Defines the legend shape as rectangle.
        /// </summary>
        Rectangle,
        /// <summary>
        /// Defines the legend shape as triangle.
        /// </summary>
        Triangle,
        /// <summary>
        /// Defines the legend shape as diamond.
        /// </summary>
        Diamond,
        /// <summary>
        /// Defines the legend shape as cross.
        /// </summary>
        Cross,
        /// <summary>
        /// Defines the legend shape as multiply.
        /// </summary>
        Multiply,
        /// <summary>
        /// Defines the legend shape as actual rect.
        /// </summary>
        ActualRect,
        /// <summary>
        /// Defines the legend shape as target rect.
        /// </summary>
        TargetRect,
        /// <summary>
        /// Defines the legend shape as horizontal line.
        /// </summary>
        HorizontalLine,
        /// <summary>
        /// Defines the legend shape as vertical line.
        /// </summary>
        VerticalLine,
        /// <summary>
        /// Defines the legend shape as pentagon.
        /// </summary>
        Pentagon,
        /// <summary>
        /// Defines the legend shape as inverted triangle.
        /// </summary>
        InvertedTriangle,
        /// <summary>
        /// Defines the legend shape as series type.
        /// </summary>
        SeriesType
    }


    /// <summary>
    /// Defines the Position. They are
    /// Top - Align the element to the top.
    /// Middle - Align the element to the center.
    /// Bottom - Align the element to the bottom.
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// Top - Align the element to the top.
        /// </summary>
        Top,
        /// <summary>
        /// Middle - Align the element to the center.
        /// </summary>
        Middle,
        /// <summary>
        /// Bottom - Align the element to the bottom.
        /// </summary>
        Bottom,
    }
}