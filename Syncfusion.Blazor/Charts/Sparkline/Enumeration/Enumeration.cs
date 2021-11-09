namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the range padding of the sparkline series.
    /// </summary>
    public enum SparklineRangePadding
    {
        /// <summary>
        /// Specifies the none type of range paddding.
        /// </summary>
        None,

        /// <summary>
        /// Specifies the normal type of range paddding.
        /// </summary>
        Normal,

        /// <summary>
        /// Specifies the additional type of range paddding.
        /// </summary>
        Additional
    }

    /// <summary>
    /// Defines the sparkline types.
    /// </summary>
    public enum SparklineType
    {
        /// <summary>
        /// Specifies the line type of the sparkline.
        /// </summary>
        Line,

        /// <summary>
        /// Specifies the column type of the sparkline.
        /// </summary>
        Column,

        /// <summary>
        /// Specifies the winLoss type of the sparkline.
        /// </summary>
        WinLoss,

        /// <summary>
        /// Specifies the pie type of the sparkline.
        /// </summary>
        Pie,

        /// <summary>
        /// Specifies the area type of the sparkline.
        /// </summary>
        Area
    }

    /// <summary>
    /// Defines the sparkline data value types.
    /// </summary>
    public enum SparklineValueType
    {
        /// <summary>
        /// Specifies the numeric type of the data value.
        /// </summary>
        Numeric,

        /// <summary>
        /// Specifies the category type of the data value..
        /// </summary>
        Category,

        /// <summary>
        /// Specifies the date time type of the data value..
        /// </summary>
        DateTime
    }

    /// <summary>
    /// Defines the edge of the data label placement, if exceeds the sparkline area horizontally.
    /// </summary>
    public enum EdgeLabelMode
    {
        /// <summary>
        /// Specifies the edge data label shown as clipped text.
        /// </summary>
        None,

        /// <summary>
        /// Specifies the edge data label moved inside the sparkline area.
        /// </summary>
        Shift,

        /// <summary>
        /// Specifies edge data label will hide, if exceeds the sparkline area.
        /// </summary>
        Hide
    }

    /// <summary>
    /// Defines the sparkline marker | datalabel visible types.
    /// </summary>
    public enum VisibleType
    {
        /// <summary>
        /// Specifies the visible at all points.
        /// </summary>
        All,

        /// <summary>
        /// Specifies the visible at high points.
        /// </summary>
        High,

        /// <summary>
        /// Specifies the visible at low points.
        /// </summary>
        Low,

        /// <summary>
        /// Specifies the visible at start points.
        /// </summary>
        Start,

        /// <summary>
        /// Specifies the visible at end points.
        /// </summary>
        End,

        /// <summary>
        /// Specifies the visible at negative points.
        /// </summary>
        Negative,

        /// <summary>
        /// Specifies the visible at none points.
        /// </summary>
        None
    }
}