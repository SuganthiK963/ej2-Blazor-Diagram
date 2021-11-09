using System;
using System.ComponentModel;
using System.Drawing;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the event arguments for the Linear Gauge's events. The common arguments of the events in the Linear Gauge are stored in this class.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BaseEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not an event can be canceled.
        /// If true, the event progress will be canceled. By default, the value is false.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for the animation complete event in circular gauge.
    /// </summary>
    public class AnimationCompleteEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the event arguments for the annotation render event in circular gauge.
    /// </summary>
    public class AnnotationRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the content of the annotation in circular gauge.
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for the axis label render event in circular gauge.
    /// </summary>
    public class AxisLabelRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the text of the axis labels in the axis of the circular gauge.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value of the axis labels in the axis of the circular gauge.
        /// </summary>
        public double Value { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for rendering a legend in circular gauge.
    /// </summary>
    public class LegendRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the fill color of the legend in circular gauge.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Gets or sets the shape of the legend in circular gauge.
        /// </summary>
        public GaugeShape Shape { get; set; }

        /// <summary>
        /// Gets or sets the text of the legend in circular gauge.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments of the loaded event in circular gauge.
    /// </summary>
    public class LoadedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the event arguments for the mouse events in circular gauge.
    /// </summary>
    public class MouseEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the x position of the target element in circular gauge.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y position of the target element in circular gauge.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for the drag start, drag move and drag end events in circular gauge.
    /// </summary>
    public class PointerDragEventArgs
    {
        /// <summary>
        /// Gets or sets the index of the axis in circular gauge.
        /// </summary>
        public int AxisIndex { get; set; }

        /// <summary>
        /// Gets or sets the value of the pointer before it gets dragged.
        /// </summary>
        public float CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the index of the pointer in circular gauge.
        /// </summary>
        public double PointerIndex { get; set; }

        /// <summary>
        /// Gets or sets the value of the pointer after it gets dragged.
        /// </summary>
        public double PreviousValue { get; set; }

        /// <summary>
        /// Gets or sets the index of the range in circular gauge.
        /// </summary>
        public double RangeIndex { get; set; }

        /// <summary>
        /// Gets or sets the type of the pointer in circular gauge.
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments of the print event.
    /// </summary>
    public class PrintEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the event argument for the radius calculate event in circular gauge.
    /// </summary>
    public class RadiusCalculateEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets the current radius of the circular gauge.
        /// </summary>
        public double CurrentRadius { get; set; }

        /// <summary>
        /// Gets or sets the location of the circular gauge.
        /// </summary>
        public PointF MidPoint { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for the resize event in circular gauge.
    /// </summary>
    public class ResizeEventArgs
    {
        /// <summary>
        /// Gets or sets the size of the circular gauge after it gets resized.
        /// </summary>
        public Size CurrentSize { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the circular gauge before it gets resized.
        /// </summary>
        public Size PreviousSize { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for the tooltip render event in circular gauge.
    /// </summary>
    public class TooltipRenderEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the tooltip element to append in body.
        /// </summary>
        public bool AppendInBodyTag { get; set; }

        /// <summary>
        /// Gets or sets the content for the tooltip in circular gauge.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the location of the tooltip in circular gauge.
        /// </summary>
        public PointF Location { get; set; }

        /// <summary>
        /// Gets or sets the element type in which the tooltip is rendered. The element types are
        /// range, annotation, and pointer of the circular gauge.
        /// </summary>
        public string Type { get; set; }
    }
}