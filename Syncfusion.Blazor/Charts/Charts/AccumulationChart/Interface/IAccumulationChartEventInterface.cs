using System;
using System.ComponentModel;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class AccumulationRenderingEventArgs
    {
        /// <summary>
        /// Defines the event cancel status.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available on accumulation chart point render.
    /// </summary>
    public class AccumulationPointRenderEventArgs : AccumulationRenderingEventArgs
    {
        /// <summary>
        /// Defines the Series.
        /// </summary>
#pragma warning disable CA1051
        public readonly AccumulationChartSeries Series;

        /// <summary>
        /// Defines the point.
        /// </summary>
        public readonly AccumulationPoints Point;

        /// <summary>
        /// Defines the color for the dataLabel text.
        /// </summary>
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Defines the border for the datalabel.
        /// </summary>
        public ChartCommonBorder Border { get; set; }

        internal AccumulationPointRenderEventArgs(string name, bool cancel, AccumulationChartSeries series, AccumulationPoints point, string fill, ChartCommonBorder border)
        {
            Name = name;
            Cancel = cancel;
            Series = series;
            Point = point;
            Fill = fill;
            Border = border;
        }
    }

    /// <summary>
    /// Specifies the event arguments available on point render of the accumulation chart component.
    /// </summary>
    public class AccumulationPointEventArgs : AccumulationRenderingEventArgs
    {
        /// <summary>
        /// Defines the x point of page.
        /// </summary>
        public readonly double PageX;

        /// <summary>
        /// Defines the y point of page.
        /// </summary>
        public readonly double PageY;

        /// <summary>
        /// Defines the x point.
        /// </summary>
        public readonly double X;

        /// <summary>
        /// Defines the y point.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Defines the index of the point.
        /// </summary>
        public readonly int PointIndex;

        /// <summary>
        /// Defines the series index of the point.
        /// </summary>
        public readonly int SeriesIndex;

        /// <summary>
        /// Defines the Series Point.
        /// </summary>
        public readonly AccumulationPoints Point;

        internal AccumulationPointEventArgs(string name, double pageX, double pageY, double x, double y, int seriesIndex, int pointIndex, AccumulationPoints point)
        {
            Name = name;
            PageX = pageX;
            PageY = pageY;
            X = x;
            Y = y;
            PointIndex = pointIndex;
            SeriesIndex = seriesIndex;
            Point = point;
        }
    }

    /// <summary>
    /// Specifies the event arguments available for resize event of the accumulation chart component.
    /// </summary>
    public class AccumulationResizeEventArgs : AccumulationRenderingEventArgs
    {
        //// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public readonly SfAccumulationChart Chart;

        /// <summary>
        /// Defines the current size of the  chart.
        /// </summary>
        public readonly Size CurrentSize;

        /// <summary>
        /// Defines the previous size of the  chart.
        /// </summary>
        public readonly Size PreviousSize;

        internal AccumulationResizeEventArgs(string name, bool cancel, SfAccumulationChart chart, Size current, Size previous)
        {
            Name = name;
            Cancel = cancel;
            Chart = chart;
            CurrentSize = current;
            PreviousSize = previous;
        }
    }

    /// <summary>
    /// Specifies the event arguments available on accumulation chart text render.
    /// </summary>
    public class AccumulationTextRenderEventArgs : AccumulationRenderingEventArgs
    {
        /// <summary>
        /// Defines the Series.
        /// </summary>
        public readonly AccumulationChartSeries Series;

        /// <summary>
        /// Defines the point.
        /// </summary>
        public readonly AccumulationPoints Point;
#pragma warning restore CA1051

        /// <summary>
        /// Defines the text for the point.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the color for the data label text.
        /// </summary>
        public string Color { get; set; } = "transparent";

        /// <summary>
        /// Defines the border for the data label.
        /// </summary>
        public ChartCommonBorder Border { get; set; }

        /// <summary>
        /// Defines the font for the data label.
        /// </summary>
        public ChartCommonFont Font { get; set; }

        internal AccumulationTextRenderEventArgs(string name, bool cancel, AccumulationChartSeries series, AccumulationPoints point, string text, string color, ChartCommonBorder border, ChartCommonFont font)
        {
            Name = name;
            Cancel = cancel;
            Series = series;
            Point = point;
            Text = text;
            Color = color;
            Font = font;
            Border = border;
        }

        internal AccumulationTextRenderEventArgs()
        {
        }
    }

    /// <summary>
    /// Specifies the event arguments available when rendering legend item of the accumulation chart component.
    /// </summary>
    public class AccumulationLegendRenderEventArgs : AccumulationRenderingEventArgs
    {
        /// <summary>
        /// Defines the legend fill color.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the legend text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the legend shape.
        /// </summary>
        public LegendShape Shape { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments for loaded event in accumulation chart component.
    /// </summary>
    public class AccumulationLoadedEventArgs : AccumulationRenderingEventArgs
    {
        //// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public SfAccumulationChart Chart { get; set; }
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IAccAnimationCompleteEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated. Use AccumulationResizeEventArgs class to achieve this.")]
    public class IAccResizeEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated. Use AccumulationTextRenderEventArgs class to achieve this.")]
    public class IAccTextRenderEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated. Use AccumulationPointRenderEventArgs class to achieve this.")]
    public class IAccPointRenderEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated. Use AccumulationLegendRenderEventArgs class to achieve this.")]
    public class IAccLegendRenderEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated. Use AccumulationPointEventArgs class to achieve this.")]
    public class IAccPointEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated. Use AccumulationLoadedEventArgs class to achieve this.")]
    public class IAccLoadedEventArgs
    {
    }
}