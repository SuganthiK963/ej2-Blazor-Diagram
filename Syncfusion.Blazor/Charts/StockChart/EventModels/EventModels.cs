using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Event argument class for stockchart Range Change Event.
    /// </summary>
    public class StockChartRangeChangeEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the data source.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Defines the end value.
        /// </summary>
        public object End { get; set; }

        /// <summary>
        /// Defines the selected data.
        /// </summary>
        public object SelectedData { get; set; }

        /// <summary>
        /// Defines the start value.
        /// </summary>
        public object Start { get; set; }

        /// <summary>
        /// Defined the zoom factor of the stock chart.
        /// </summary>
        public double ZoomFactor { get; set; }

        /// <summary>
        /// Defined the zoom position of the stock chart.
        /// </summary>
        public double ZoomPosition { get; set; }
    }

    /// <summary>
    /// Event argument class for stockchart.
    /// </summary>
    public class StockChartEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines stock chart instance.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public SfStockChart StockChart { get; set; }

        /// <summary>
        /// Defines theme of the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public Theme Theme { get; set; }
    }

    /// <summary>
    /// Event argument class for stockchart Mouse Events.
    /// </summary>
    public class StockChartMouseEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines current mouse event target id.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Defines current mouse x location.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines current mouse y location.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Event argument class for stockchart Point Event.
    /// </summary>
    public class StockChartPointEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines stock chart instance.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public SfStockChart Chart { get; set; }

        /// <summary>
        /// Defines current window page x location.
        /// </summary>
        public double PageX { get; set; }

        /// <summary>
        /// Defines current window page y location.
        /// </summary>
        public double PageY { get; set; }

        /// <summary>
        /// Defines the current point.
        /// </summary>
        public object Point { get; set; }

        /// <summary>
        /// Defines the point index.
        /// </summary>
        public double PointIndex { get; set; }

        /// <summary>
        /// Defines series of the stock chart.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public StockChartSeries Series { get; set; }

        /// <summary>
        /// Defines the series index.
        /// </summary>
        public double SeriesIndex { get; set; }

        /// <summary>
        /// Defines current mouse x location.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Defines current mouse y location.
        /// </summary>
        public double Y { get; set; }
    }

    /// <summary>
    /// Event argument class for stockchart Zooming Event.
    /// </summary>
    public class StockChartZoomingEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Defines the axis collection.
        /// </summary>
        public List<StockChartAxisData> AxisCollection { get; set; }
    }

    /// <summary>
    /// Event argument class for stockchart zooming axis details.
    /// </summary>
    public class StockChartAxisData
    {
        /// <summary>
        /// Defines the axis name.
        /// </summary>
        public string AxisName { get; set; }

        /// <summary>
        ///  Defines the axis range.
        /// </summary>
        public VisibleRangeModel AxisRange { get; set; }

        /// <summary>
        ///  Defines the axis zoom factor.
        /// </summary>
        public double ZoomFactor { get; set; }

        /// <summary>
        ///  Defines the axis zoom position.
        /// </summary>
        public double ZoomPosition { get; set; }
    }

    /// <summary>
    /// Event argument class for stockchart Period Changed Event.
    /// </summary>
    public class StockChartPeriodChangedEventArgs : BaseEventArgs
    {
        internal StockChartPeriodChangedEventArgs(string name, List<StockChartPeriod> periods)
        {
            Name = name;
            Periods = periods;
        }

        /// <summary>
        /// Defines the periods.
        /// </summary>
        public List<StockChartPeriod> Periods { get; private set; }
    }
}