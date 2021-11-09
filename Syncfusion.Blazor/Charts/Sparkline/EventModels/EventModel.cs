using System;
using System.ComponentModel;
using System.Drawing;
using Syncfusion.Blazor.Sparkline.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the event arguments which has common for sparkline component.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SparklineBaseEventArgs
    {
        /// <summary>
        /// Specifies the cancel state for the event. The default value is false.
        /// If set as true, the event progress will be stopped.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the name of the event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies the axis render event arguments.
    /// </summary>
    public class AxisRenderingEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the maximum X point of the axis.
        /// </summary>
        public double MaxX { get; set; }

        /// <summary>
        /// Defines the maximum X point of the axis.
        /// </summary>
        public double MinX { get; set; }

        /// <summary>
        /// Defines the maximum Y point of the axis.
        /// </summary>
        public double MinY { get; set; }

        /// <summary>
        /// Defines the maximum Y point of the axis.
        /// </summary>
        public double MaxY { get; set; }

        /// <summary>
        /// Defines the value point of the axis.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Defines the line color of the axis.
        /// </summary>
        public string LineColor { get; set; }

        /// <summary>
        /// Defines the line width of the axis.
        /// </summary>
        public double LineWidth { get; set; }
    }

    /// <summary>
    /// Specifies the data label rendering event arguments.
    /// </summary>
    public class DataLabelRenderingEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the color and width of the label border.
        /// </summary>
        public Border Border { get; set; }

        /// <summary>
        /// Defines the fill color of the label.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the point index of the label.
        /// </summary>
        public int PointIndex { get; set; }

        /// <summary>
        /// Defines the x position of the label.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines the y position of the label.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Defines the text of the label.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the color of the label.
        /// </summary>
        public string Color { get; set; }
    }

    /// <summary>
    /// Specifies the market rendering event arguments.
    /// </summary>
    public class MarkerRenderingEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the color and width of the marker border.
        /// </summary>
        public Border Border { get; set; }

        /// <summary>
        /// Defines the fill color of the marker.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the point index of the marker.
        /// </summary>
        public int PointIndex { get; set; }

        /// <summary>
        /// Defines the x position of the marker.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines the y position of the marker.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Defines the size of the marker.
        /// </summary>
        public double Size { get; set; }
    }

    /// <summary>
    /// Specifies the point event arguments in mouse click action.
    /// </summary>
    public class PointRegionEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the index of the point in mouse click action.
        /// </summary>
        public int PointIndex { get; set; }
    }

    /// <summary>
    /// Specifies the point rendering event arguments.
    /// </summary>
    public class SparklinePointEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the index of the point.
        /// </summary>
        public int PointIndex { get; set; }

        /// <summary>
        /// Defines the fill color of the point.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the color and width of the point border.
        /// </summary>
        public Border Border { get; set; }
    }

    /// <summary>
    /// Specifies the sparkline resize event arguments.
    /// </summary>
    public class SparklineResizeEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the previous size of the sparkline.
        /// </summary>
        public PointF PreviousSize { get; set; }

        /// <summary>
        /// Defines the current size of the sparkline.
        /// </summary>
        public PointF CurrentSize { get; set; }
    }

    /// <summary>
    /// Specifies the series rendering event arguments.
    /// </summary>
    public class SeriesRenderingEventArgs : SparklineBaseEventArgs
    {
        /// <summary>
        /// Defines the fill color of the series.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the line width of the series.
        /// </summary>
        public double LineWidth { get; set; }

        /// <summary>
        /// Defines the color and width of the series border.
        /// </summary>
        public Border Border { get; set; }
    }

    /// <summary>
    /// Specifies the mouse event arguments.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class SparklineMouseEventArgs : SparklineBaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the tooltip rendering event arguments.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class TooltipRenderingEventArgs : SparklineBaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the sparkline before rendering event arguments.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class SparklineLoadEventArgs : SparklineBaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the sparkline after rendering event arguments.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class SparklineLoadedEventArgs : SparklineBaseEventArgs
    {
    }
}