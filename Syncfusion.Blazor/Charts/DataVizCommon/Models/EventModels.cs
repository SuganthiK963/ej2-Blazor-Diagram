using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Defines the border values.
    /// </summary>
    public class BorderModel
    {
        /// <summary>
        /// The color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        public string Color { get; set; } = string.Empty;
        /// <summary>
        /// The width of the border in pixels.
        /// </summary>
        public double Width { get; set; } = 1;
    }

    /// <summary>
    /// Defines the axis visible range model.
    /// </summary>
    public class VisibleRangeModel
    {
        /// <summary>
        /// axis delta value
        /// </summary>
        public double Delta { get; set; }
        /// <summary>
        /// axis interval value
        /// </summary>
        public double Interval { get; set; }
        /// <summary>
        /// axis maximum value
        /// </summary>
        public double Max { get; set; }
        /// <summary>
        /// axis minimum value
        /// </summary>
        public double Min { get; set; }
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class IPrintEventArgs
    {
        /// <summary>
        /// Defines the event cancel status
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool Cancel { get; set; }
        /// <summary>
        /// Defines the name of the event
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments of the print event.
    /// </summary>
    public class PrintEventArgs
    {
        /// <summary>
        /// Defines the event cancel status.
        /// </summary>
        public bool Cancel { get; set; }
    }
}