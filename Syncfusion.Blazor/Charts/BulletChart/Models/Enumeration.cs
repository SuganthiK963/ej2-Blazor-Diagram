namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the rext anchor of bullet chart.
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// Specifies target type is rect.
        /// </summary>
        Rect,

        /// <summary>
        /// Specifies target type is circle.
        /// </summary>
        Circle,

        /// <summary>
        /// Specifies target type is cross.
        /// </summary>
        Cross
    }

    /// <summary>
    /// Defines the label placement of bullet chart.
    /// </summary>
    public enum LabelsPlacement
    {
        /// <summary>
        /// Specifies the label placed outside of the chart.
        /// </summary>
        Outside,

        /// <summary>
        /// Specifies the label placed inside the chart.
        /// </summary>
        Inside
    }

    /// <summary>
    /// Defines the orientation of the bullet chart.
    /// </summary>
    public enum OrientationType
    {
        /// <summary>
        /// Specifies the render bullet chart in horizontal orientation.
        /// </summary>
        Horizontal,

        /// <summary>
        ///  Specifies the render bullet chart in vertical orientation.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Defines the tick placement of bullet chart.
    /// </summary>
    public enum TickPosition
    {
        /// <summary>
        /// outside - ticks placed outside of bullet chart
        /// </summary>
        Outside,

        /// <summary>
        /// inside - ticks placed inside the bullet chart
        /// </summary>
        Inside
    }

    /// <summary>
    /// Defines the text anchor of bullet chart.
    /// </summary>
    public enum TextPosition
    {
        /// <summary>
        /// Specifies the bullet chart title placed in left.
        /// </summary>
        Left,

        /// <summary>
        /// Specifies the bullet chart title placed in right.
        /// </summary>
        Right,

        /// <summary>
        /// Specifies the bullet chart title placed in top.
        /// </summary>
        Top,

        /// <summary>
        /// Specifies the bullet chart title placed in bottom.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Defines the flow direction of the bullet chart.
    /// </summary>
    public enum FeatureType
    {
        /// <summary>
        /// Specifies the rectangle feature type.
        /// </summary>
        Rect,

        /// <summary>
        /// Specifies the dot feature type.
        /// </summary>
        Dot
    }
}