using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.SmithChart.Internal;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the event arguments which has common for smith chart component.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the event cancel status.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Defines the name of the event.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        public string Name
        {
            get
            {
                return EventName;
            }
        }

        internal string EventName { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for animation.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class AnimationCompleteEventArgs
    {
    }

    /// <summary>
    /// Specifies the event arguments available for export event of the chart component.
    /// </summary>
    public class SmithChartExportEventArgs : SmithChartBaseEventArgs
    {
        internal SmithChartExportEventArgs(string dataUrl)
        {
            DataUrl = dataUrl;
        }

        /// <summary>
        /// Defines the DataUrl for export file.
        /// </summary>
#pragma warning disable CA1056
        public string DataUrl { get; }
#pragma warning restore CA1056
    }

    /// <summary>
    /// Specifies the tooltip render event arguments.
    /// </summary>
    public class SmithChartTooltipEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the headerText of tooltip.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Defines point of the tooltip.
        /// </summary>
        public SmithChartPoint Point { get; set; }

        /// <summary>
        /// Defines the template.
        /// </summary>
        public RenderFragment<object> Template { get; set; }

        /// <summary>
        /// Defines the tooltip text.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Specifies the event arguments available for resize event of the chart component.
    /// </summary>
    public class SmithChartResizeEventArgs : SmithChartBaseEventArgs
    {
        internal SmithChartResizeEventArgs(string name, bool cancel, Size current, Size previous)
        {
            EventName = name;
            Cancel = cancel;
            CurrentSize = current;
            PreviousSize = previous;
        }

        /// <summary>
        /// Defines the current size of the  chart.
        /// </summary>
        public Size CurrentSize { get; }

        /// <summary>
        /// Defines the previous size of the  chart.
        /// </summary>
        public Size PreviousSize { get; }
    }

    /// <summary>
    /// Specifies the animation complete event arguments.
    /// </summary>
    public class SmithChartAnimationCompleteEventArgs : SmithChartBaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the Axis Label Render Event arguments.
    /// </summary>
    public class SmithChartAxisLabelRenderEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the current axis label text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the current axis label x location.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines the current axis label y location.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Specifies the Legend Render Event arguments.
    /// </summary>
    public class SmithChartLegendRenderEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the current legend fill color.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the current legend shape.
        /// </summary>
        public Shape Shape { get; set; }

        /// <summary>
        /// Defines the current legend text.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Specifies the Loaded Event arguments.
    /// </summary>
    public class SmithChartLoadedEventArgs : SmithChartBaseEventArgs
    {
    }

    /// <summary>
    /// Specifies the Series Render Event arguments.
    /// </summary>
    public class SmithChartSeriesRenderEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the current series fill.
        /// </summary>
        public string Fill { get; set; }

        /// <summary>
        /// Defines the current series index.
        /// </summary>
        public double Index { get; set; }

        /// <summary>
        /// Defines name of the event.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Specifies the Text Render Event arguments.
    /// </summary>
    public class SmithChartTextRenderEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the current datalabel pointIndex.
        /// </summary>
        public double PointIndex { get; set; }

        /// <summary>
        /// Defines the current datalabel seriesIndex.
        /// </summary>
        public double SeriesIndex { get; set; }

        /// <summary>
        /// Defines the current datalabel text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the current datalabel text x location.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines the current datalabel text y location.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Defines the text color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Defines the information of the border.
        /// </summary>
        public SmithChartSeriesDataLabelBorder Border { get; set; }

        /// <summary>
        /// Defines the template.
        /// </summary>
        public RenderFragment<SmithChartPoint> Template { get; set; }
    }

    /// <summary>
    /// Specifies the SubTitle Render Event arguments.
    /// </summary>
    public class SubTitleRenderEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the current subtitle text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the current subtitle text x location.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines the current subtitle text y location.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Specifies the Title Render Event arguments.
    /// </summary>
    public class TitleRenderEventArgs : SmithChartBaseEventArgs
    {
        /// <summary>
        /// Defines the current title text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Defines the current title text x location.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines the current title text y location.
        /// </summary>
        public double Y { get; set; }
    }
}