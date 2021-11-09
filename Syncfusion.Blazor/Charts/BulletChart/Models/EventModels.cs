using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the event arguments which has common for bullet chart component.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class BulletChartBaseEventArgs
    {
        /// <summary>
        /// Specifies the cancel state for the event. The default value is false.
        /// If set as true, the event progress will be stopped.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Defines the label render event arguments.
    /// </summary>
    public class BulletChartLabelRenderEventArgs : BulletChartBaseEventArgs
    {
        /// <summary>
        /// Specifies the text of label.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the X position of label.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Specifies the Y position of label.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Defines the legend render event arguments.
    /// </summary>
    public class BulletChartLegendRenderEventArgs : BulletChartBaseEventArgs
    {
        /// <summary>
        /// Specifies the current legend fill color.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name { get; set; }

        /// <summary>
        /// Specifies the current legend shape.
        /// </summary>
        public LegendShape Shape { get; set; }

        /// <summary>
        /// Specifies the current legend text.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Defines the tooltip render event arguments.
    /// </summary>
    public class BulletChartTooltipEventArgs : BulletChartBaseEventArgs
    {
        /// <summary>
        /// Specifies the name of the Event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name { get; set; }

        /// <summary>
        /// Specifies the target value of the comparative bar.
        /// </summary>
        public List<string> Target { get; set; }

        /// <summary>
        /// Specifies the tooltip template.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Template { get; set; }

        /// <summary>
        /// Specifies the tooltip text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Specifies the actual value of the feature bar.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Defines the component loaded event arguments.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class BulletChartLoadedEventArgs : BulletChartBaseEventArgs
    {
        /// <summary>
        /// Defines the theme of the bullet chart.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public Theme Theme { get; set; }
    }

    /// <summary>
    /// Defines the mouse event arguments.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This event argument is deprecated and will no longer be used.")]
    public class BulletChartMouseEventArgs : BulletChartBaseEventArgs
    {
        /// <summary>
        /// Defines current mouse event target id.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Target { get; set; }

        /// <summary>
        /// Defines current mouse x location.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public double X { get; set; }

        /// <summary>
        /// Defines current mouse y location.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public double Y { get; set; }
    }
}