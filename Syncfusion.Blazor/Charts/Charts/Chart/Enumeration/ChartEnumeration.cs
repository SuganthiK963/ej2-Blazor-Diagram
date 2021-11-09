namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the chart component highlight mode type.
    /// </summary>
    public enum HighlightMode
    {
        /// <summary>
        /// Defines the none highlight mode of the chart component.
        /// </summary>
        None,

        /// <summary>
        /// Defines the series highlight mode of the chart component.
        /// </summary>
        Series,

        /// <summary>
        /// Defines the point highlight mode of the chart component.
        /// </summary>
        Point,

        /// <summary>
        /// Defines the cluster highlight mode of the chart component.
        /// </summary>
        Cluster
    }

    /// <summary>
    /// Specifies the highlighting or selecting patterns.
    /// </summary>
    public enum SelectionPattern
    {
        /// <summary>
        /// Defines the none as highlighting or selecting pattern.
        /// </summary>
        None,

        /// <summary>
        /// Defines the chessboard as highlighting or selecting pattern.
        /// </summary>
        Chessboard,

        /// <summary>
        /// Defines the dots as highlighting or selecting pattern.
        /// </summary>
        Dots,

        /// <summary>
        /// Defines the diagonal forward as highlighting or selecting pattern.
        /// </summary>
        DiagonalForward,

        /// <summary>
        /// Defines the crosshatch as highlighting or selecting pattern.
        /// </summary>
        Crosshatch,

        /// <summary>
        /// Defines the pacman highlighting or selecting pattern.
        /// </summary>
        Pacman,

        /// <summary>
        /// Defines the diagonal backward as highlighting or selecting pattern.
        /// </summary>
        DiagonalBackward,

        /// <summary>
        /// Defines the grid as highlighting or selecting pattern.
        /// </summary>
        Grid,

        /// <summary>
        /// Defines the turquoise as highlighting or selecting pattern.
        /// </summary>
        Turquoise,

        /// <summary>
        /// Defines the star as highlighting or selecting pattern.
        /// </summary>
        Star,

        /// <summary>
        /// Defines the triangle as highlighting or selecting pattern.
        /// </summary>
        Triangle,

        /// <summary>
        /// Defines the circle as highlighting or selecting pattern.
        /// </summary>
        Circle,

        /// <summary>
        /// Defines the tile as highlighting or selecting pattern.
        /// </summary>
        Tile,

        /// <summary>
        /// Defines the horizontal dash as highlighting or selecting pattern.
        /// </summary>
        HorizontalDash,

        /// <summary>
        /// Defines the vertical dash as highlighting or selecting pattern.
        /// </summary>
        VerticalDash,

        /// <summary>
        /// Defines the rectangle as highlighting or selecting pattern.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Defines the box as highlighting or selecting pattern.
        /// </summary>
        Box,

        /// <summary>
        /// Defines the vertical stripe as highlighting or selecting pattern.
        /// </summary>
        VerticalStripe,

        /// <summary>
        /// Defines the horizontal stripe as highlighting or selecting pattern.
        /// </summary>
        HorizontalStripe,

        /// <summary>
        /// Defines the bubble as highlighting or selecting pattern.
        /// </summary>
        Bubble
    }

    /// <summary>
    /// Specifies the selection mode.
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// Defines the disable the selection.
        /// </summary>
        None,

        /// <summary>
        /// Defines to select an series.
        /// </summary>
        Series,

        /// <summary>
        /// Defines to select a point.
        /// </summary>
        Point,

        /// <summary>
        /// Defines too select a cluster of point.
        /// </summary>
        Cluster,

        /// <summary>
        /// Defines to select points, by dragging with respect to both horizontal and vertical axis.
        /// </summary>
        DragXY,

        /// <summary>
        /// Defines to select points, by dragging with respect to horizontal axis.
        /// </summary>
        DragY,

        /// <summary>
        ///  Defines to select points, by dragging with respect to vertical axis.
        /// </summary>
        DragX,

        /// <summary>
        /// Defines to select points, by dragging with respect to free form.
        /// </summary>
        Lasso
    }

    /// <summary>
    /// Specifies the box plot mode for box and whisker chart series.
    /// </summary>
    public enum BoxPlotMode
    {
        /// <summary>
        /// Defines to series render based on exclusive mode.
        /// </summary>
        Exclusive,

        /// <summary>
        /// Defines to series render based on inclusive mode.
        /// </summary>
        Inclusive,

        /// <summary>
        /// Defines to series render based on normal mode.
        /// </summary>
        Normal
    }

    /// <summary>
    /// Specifies the type of series to be drawn in radar or polar series.
    /// </summary>
    public enum ChartDrawType
    {
        /// <summary>
        /// Defines to renders the line series.
        /// </summary>
        Line,

        /// <summary>
        /// Defines to renders the column series.
        /// </summary>
        Column,

        /// <summary>
        /// Defines to renders the stacking column series.
        /// </summary>
        StackingColumn,

        /// <summary>
        /// Defines to renders the area series.
        /// </summary>
        Area,

        /// <summary>
        /// Defines to renders the scatter series.
        /// </summary>
        Scatter,

        /// <summary>
        /// Defines to renders the range column series.
        /// </summary>
        RangeColumn,

        /// <summary>
        /// Defines to renders the spline series.
        /// </summary>
        Spline,

        /// <summary>
        /// Defines to renders the spline area series.
        /// </summary>
        SplineArea,

        /// <summary>
        /// Defines to renders the stacking area series.
        /// </summary>
        StackingArea,

        /// <summary>
        /// Defines to renders the stacking line series.
        /// </summary>
        StackingLine
    }

    /// <summary>
    /// Specifies the segment axis.
    /// </summary>
    public enum Segment
    {
        /// <summary>
        /// Defines segment to be rendered based on horizontal axis.
        /// </summary>
        X,

        /// <summary>
        /// Defines calculation rendered based on vertical axis.
        /// </summary>
        Y
    }

    /// <summary>
    /// Specifies the type of spline.
    /// </summary>
    public enum SplineType
    {
        /// <summary>
        /// Defines to render Natural spline.
        /// </summary>
        Natural,

        /// <summary>
        /// Defines to render monotonic spline.
        /// </summary>
        Monotonic,

        /// <summary>
        /// Defines to render cardinal spline.
        /// </summary>
        Cardinal,

        /// <summary>
        /// Defines to render clamped spline.
        /// </summary>
        Clamped
    }

    /// <summary>
    /// Specifies the type series in chart.
    /// </summary>
    public enum ChartSeriesType
    {
        /// <summary>
        /// Defines to renders the line series.
        /// </summary>
        Line,

        /// <summary>
        /// Defines to renders the column series.
        /// </summary>
        Column,

        /// <summary>
        /// Defines to renders the area series.
        /// </summary>
        Area,

        /// <summary>
        /// Defines to renders the stacking column series.
        /// </summary>
        Bar,

        /// <summary>
        /// Defines to renders the histogram series.
        /// </summary>
        Histogram,

        /// <summary>
        /// Defines to renders the stacking column series.
        /// </summary>
        StackingColumn,

        /// <summary>
        /// Defines to renders the stacking area series.
        /// </summary>
        StackingArea,

        /// <summary>
        /// Defines to renders the stacking line series.
        /// </summary>
        StackingLine,

        /// <summary>
        /// Defines to renders the stacking bar series.
        /// </summary>
        StackingBar,

        /// <summary>
        /// Defines to renders the stacking step area series.
        /// </summary>
        StackingStepArea,

        /// <summary>
        /// Defines to renders the step line series.
        /// </summary>
        StepLine,

        /// <summary>
        /// Defines to renders the step area series.
        /// </summary>
        StepArea,

        /// <summary>
        /// Defines to renders the spline area series.
        /// </summary>
        SplineArea,

        /// <summary>
        /// Defines to renders the scatter series.
        /// </summary>
        Scatter,

        /// <summary>
        /// Defines to renders the spline series.
        /// </summary>
        Spline,

        /// <summary>
        ///  Defines to renders the stacking column series.
        /// </summary>
        StackingColumn100,

        /// <summary>
        /// Defines to renders the stacking bar 100 percent series.
        /// </summary>
        StackingBar100,

        /// <summary>
        /// Defines to renders the stacking line 100 percent series.
        /// </summary>
        StackingLine100,

        /// <summary>
        /// Defines to renders the stacking area 100 percent series.
        /// </summary>
        StackingArea100,

        /// <summary>
        /// Defines to renders the range column series.
        /// </summary>
        RangeColumn,

        /// <summary>
        /// Defines to renders the hilo series.
        /// </summary>
        Hilo,

        /// <summary>
        /// Defines to renders the hilo open close series.
        /// </summary>
        HiloOpenClose,

        /// <summary>
        /// Defines to renders the waterfall series.
        /// </summary>
        Waterfall,

        /// <summary>
        /// Defines to renders the range area series.
        /// </summary>
        RangeArea,

        /// <summary>
        /// Defines to renders the bubble series.
        /// </summary>
        Bubble,

        /// <summary>
        /// Defines to renders the candle series.
        /// </summary>
        Candle,

        /// <summary>
        /// Defines to renders the polar series.
        /// </summary>
        Polar,

        /// <summary>
        /// Defines to renders the radar series.
        /// </summary>
        Radar,

        /// <summary>
        /// Defines to renders the box and whisker series.
        /// </summary>
        BoxAndWhisker,

        /// <summary>
        /// Defines to renders the multicolored line series.
        /// </summary>
        MultiColoredLine,

        /// <summary>
        /// Defines to renders the multicolored area series.
        /// </summary>
        MultiColoredArea,

        /// <summary>
        /// Defines to renders the pareto series.
        /// </summary>
        Pareto
    }

    /// <summary>
    /// Specifies the type of trendlines.
    /// </summary>
#pragma warning disable CA1717
    public enum TrendlineTypes
    {
        /// <summary>
        /// Defines the linear trendline.
        /// </summary>
        Linear,

        /// <summary>
        /// Defines the exponential trendline.
        /// </summary>
        Exponential,

        /// <summary>
        /// Defines the polynomial trendline.
        /// </summary>
        Polynomial,

        /// <summary>
        /// Defines the power trendline.
        /// </summary>
        Power,

        /// <summary>
        /// Defines the logarithmic trendline.
        /// </summary>
        Logarithmic,

        /// <summary>
        /// Defines the moving average trendline.
        /// </summary>
        MovingAverage
    }

    /// <summary>
    /// Specifies the shape of marker.
    /// </summary>
    public enum ChartShape
    {
        /// <summary>
        /// Defines the marker shape as circle.
        /// </summary>
        Circle,

        /// <summary>
        /// Defines the marker shape as rectangle.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Defines the marker shape as triangle.
        /// </summary>
        Triangle,

        /// <summary>
        /// Defines the marker shape as diamond.
        /// </summary>
        Diamond,

        /// <summary>
        /// Defines the marker shape as cross.
        /// </summary>
        Cross,

        /// <summary>
        /// Defines the marker shape as horizontal line.
        /// </summary>
        HorizontalLine,

        /// <summary>
        /// Defines the marker shape as vertical line.
        /// </summary>
        VerticalLine,

        /// <summary>
        /// Defines the marker shape as pentagon.
        /// </summary>
        Pentagon,

        /// <summary>
        /// Defines the marker shape as inverted triangle.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Defines the marker shape as image.
        /// </summary>
        Image
    }

    /// <summary>
    /// Specifies the label position.
    /// </summary>
    public enum LabelPosition
    {
        /// <summary>
        /// Defines label position on outside of the point.
        /// </summary>
        Outer,

        /// <summary>
        /// Defines label position on top of the point.
        /// </summary>
        Top,

        /// <summary>
        /// Defines label position on bottom of the point.
        /// </summary>
        Bottom,

        /// <summary>
        /// Defines label position on middle of the point.
        /// </summary>
        Middle,

        /// <summary>
        /// Defines label position base on series.
        /// </summary>
        Auto
    }

    /// <summary>
    /// Specifies the direction of error bar.
    /// </summary>
    public enum ErrorBarDirection
    {
        /// <summary>
        /// Defines to renders both direction of the error bar.
        /// </summary>
        Both,

        /// <summary>
        /// Defines to renders minus direction of the error bar.
        /// </summary>
        Minus,

        /// <summary>
        /// Defines to renders plus direction of the error bar.
        /// </summary>
        Plus
    }

    /// <summary>
    /// Specifies the modes of error bar.
    /// </summary>
    public enum ErrorBarMode
    {
        /// <summary>
        /// Defines to renders the vertical error bar.
        /// </summary>
        Vertical,

        /// <summary>
        /// Defines to renders the horizontal error bar.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Defines to renders both side error bar.
        /// </summary>
        Both
    }

    /// <summary>
    /// Specifies the type of error bar. They are.
    /// </summary>
    public enum ErrorBarType
    {
        /// <summary>
        /// Defines to renders a fixed type error bar.
        /// </summary>
        Fixed,

        /// <summary>
        /// Defines to renders a percentage type error bar.
        /// </summary>
        Percentage,

        /// <summary>
        /// Defines to renders a standard deviation type error bar.
        /// </summary>
        StandardDeviation,

        /// <summary>
        /// Defines to renders a standard error type bar.
        /// </summary>
        StandardError,

        /// <summary>
        /// Defines to renders a custom type error bar.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the edge Label Placement for an axis.
    /// </summary>
    public enum EdgeLabelPlacement
    {
        /// <summary>
        /// Defines no action will be perform.
        /// </summary>
        None,

        /// <summary>
        /// Defines edge label will be hidden.
        /// </summary>
        Hide,

        /// <summary>
        /// Defines Shift the edge labels.
        /// </summary>
        Shift
    }

    /// <summary>
    /// Specifies the interval type of datetime axis.
    /// </summary>
    public enum IntervalType
    {
        /// <summary>
        /// Defines the interval of the axis based on data.
        /// </summary>
        Auto,

        /// <summary>
        /// Defines the interval of the axis in years.
        /// </summary>
        Years,

        /// <summary>
        /// Defines the interval of the axis in months.
        /// </summary>
        Months,

        /// <summary>
        /// Defines the interval of the axis in days.
        /// </summary>
        Days,

        /// <summary>
        /// Defines the interval of the axis in hours.
        /// </summary>
        Hours,

        /// <summary>
        /// Defines the interval of the axis in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// Defines the interval of the axis in seconds.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the alignment.
    /// </summary>
    public enum LabelIntersectAction
    {
        /// <summary>
        /// Defines to shows all the labels.
        /// </summary>
        None,

        /// <summary>
        /// Defines to hide the label when it intersect.
        /// </summary>
        Hide,

        /// <summary>
        /// Defines to trim the label when it intersect.
        /// </summary>
        Trim,

        /// <summary>
        /// Defines to wrap the label when it intersect.
        /// </summary>
        Wrap,

        /// <summary>
        /// Defines to set the multiple rows the label when it intersect.
        /// </summary>
        MultipleRows,

        /// <summary>
        /// Defines to rotate the label at 45 degree when it intersect.
        /// </summary>
        Rotate45,

        /// <summary>
        /// Defines to rotate the label at 90 degree when it intersect.
        /// </summary>
        Rotate90
    }

    /// <summary>
    /// Specifies the label placement for category axis.
    /// </summary>
    public enum LabelPlacement
    {
        /// <summary>
        /// Defines to render the label between the ticks.
        /// </summary>
        BetweenTicks,

        /// <summary>
        /// Defines to render the label on the ticks.
        /// </summary>
        OnTicks
    }

    /// <summary>
    /// Specifies the position.
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// Defines the ticks or labels inside to the axis line.
        /// </summary>
        Inside,

        /// <summary>
        /// Defines the ticks or labels outside to the axis line.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Specifies the range padding of axis.
    /// </summary>
    public enum ChartRangePadding
    {
        /// <summary>
        /// Defines auto padding to be applied to the axis.
        /// </summary>
        Auto,

        /// <summary>
        /// Defines padding cannot be applied to the axis.
        /// </summary>
        None,

        /// <summary>
        /// Defines padding is applied to the axis based on the range calculation.
        /// </summary>
        Normal,

        /// <summary>
        /// Defines interval of the axis is added as padding to the minimum and maximum values of the range.
        /// </summary>
        Additional,

        /// <summary>
        ///  Defines axis range is rounded to the nearest possible value divided by the interval.
        /// </summary>
        Round
    }

    /// <summary>
    /// Specifies the type of axis.
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// Defines to renders a numeric axis.
        /// </summary>
#pragma warning disable CA1720
        Double,

#pragma warning restore CA1720
        /// <summary>
        /// Defines to renders a date time axis.
        /// </summary>
        DateTime,

        /// <summary>
        /// Defines to renders a category axis.
        /// </summary>
        Category,

        /// <summary>
        /// Defines renders a log axis.
        /// </summary>
        Logarithmic,

        /// <summary>
        /// Defines renders a date time category axis.
        /// </summary>
        DateTimeCategory
    }

    /// <summary>
    /// Specifies the strip line text position.
    /// </summary>
    public enum Anchor
    {
        /// <summary>
        /// Defines the strip line text at the start.
        /// </summary>
        Start,

        /// <summary>
        /// Defines the strip line text in the middle.
        /// </summary>
        Middle,

        /// <summary>
        /// Defines the strip line text at the end.
        /// </summary>
        End
    }

    /// <summary>
    /// Specifies the unit of strip line size.
    /// </summary>
    public enum SizeType
    {
        /// <summary>
        /// Defines auto type.
        /// </summary>
        Auto,

        /// <summary>
        /// Defines pixel type.
        /// </summary>
        Pixel,

        /// <summary>
        /// Defines years type.
        /// </summary>
        Years,

        /// <summary>
        /// Defines months type.
        /// </summary>
        Months,

        /// <summary>
        /// Defines days type.
        /// </summary>
        Days,

        /// <summary>
        /// Defines hours type.
        /// </summary>
        Hours,

        /// <summary>
        /// Defines minutes type.
        /// </summary>
        Minutes,

        /// <summary>
        /// Defines seconds type.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the order of the strip line.
    /// </summary>
    public enum ZIndex
    {
        /// <summary>
        /// Defines the strip line over the series elements.
        /// </summary>
        Over,

        /// <summary>
        /// Defines the strip line behind the series elements.
        /// </summary>
        Behind
    }

    /// <summary>
    /// Specifies border type for multi level labels.
    /// </summary>
    public enum BorderType
    {
        /// <summary>
        /// Defines rectangle type.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Defines brace type.
        /// </summary>
        Brace,

        /// <summary>
        /// Defines without border type.
        /// </summary>
        WithoutBorder,

        /// <summary>
        /// Defines without top border type.
        /// </summary>
        WithoutTopBorder,

        /// <summary>
        /// Defines without top and bottom border type.
        /// </summary>
        WithoutTopandBottomBorder,

        /// <summary>
        /// Defines curly brace type.
        /// </summary>
        CurlyBrace,

        /// <summary>
        /// Define Default border type.
        /// </summary>
        Auto
    }

    /// <summary>
    /// Specifies coordinate units of an annotation.
    /// </summary>
    public enum Units
    {
        /// <summary>
        /// Defines pixel units.
        /// </summary>
        Pixel,

        /// <summary>
        /// Defines point units.
        /// </summary>
        Point
    }

    /// <summary>
    /// Specifies regions of an annotation.
    /// </summary>
    public enum Regions
    {
        /// <summary>
        /// Defines chart region.
        /// </summary>
        Chart,

        /// <summary>
        /// Defines series region.
        /// </summary>
        Series
    }

    /// <summary>
    /// Specifies the financial data fields.
    /// </summary>
    public enum FinancialDataFields
    {
        /// <summary>
        /// Defines to the highest price in the stocks over time.
        /// </summary>
        High,

        /// <summary>
        /// Defines to the lowest price in the stocks over time.
        /// </summary>
        Low,

        /// <summary>
        /// Defines to the opening price in the stocks over time.
        /// </summary>
        Open,

        /// <summary>
        /// Defines to the closing price in the stocks over time.
        /// </summary>
        Close
    }

    /// <summary>
    /// Specifies the type of technical indicators.
    /// </summary>
    public enum MacdType
    {
        /// <summary>
        /// Defines to predicts the trend using simple moving average approach.
        /// </summary>
        Line,

        /// <summary>
        /// Defines to predicts the trend using simple moving average approach.
        /// </summary>
        Histogram,

        /// <summary>
        /// Defines to predicts the trend using simple moving average approach.
        /// </summary>
        Both
    }

    /// <summary>
    /// Specifies the type of technical indicators.
    /// </summary>
    public enum TechnicalIndicators
    {
        /// <summary>
        /// Defines to predicts the trend using simple moving average approach.
        /// </summary>
        Sma,

        /// <summary>
        /// Defines to predicts the trend using exponential moving average approach.
        /// </summary>
        Ema,

        /// <summary>
        /// Defines to predicts the trend using triangle moving average approach.
        /// </summary>
        Tma,

        /// <summary>
        /// Defines to predicts the trend using momentum approach.
        /// </summary>
        Momentum,

        /// <summary>
        /// Defines to predicts the trend using average true range approach.
        /// </summary>
        Atr,

        /// <summary>
        /// Defines to predicts the trend using accumulation distribution approach.
        /// </summary>
        AccumulationDistribution,

        /// <summary>
        /// Defines to predicts the trend using bollinger approach.
        /// </summary>
        BollingerBands,

        /// <summary>
        /// Defines to predicts the trend using moving average convergence divergence approach.
        /// </summary>
        Macd,

        /// <summary>
        /// Defines to predicts the trend using stochastic approach.
        /// </summary>
        Stochastic,

        /// <summary>
        /// Defines to predicts the trend using RSI approach.
        /// </summary>
        Rsi
    }

    /// <summary>
    /// Specifies the connector type.
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// Defines accumulation series connector line type as straight line.
        /// </summary>
        Line,

        /// <summary>
        /// Defines accumulation series connector line type as curved line.
        /// </summary>
        Curve
    }

    /// <summary>
    /// Specifies the mode of line in crosshair.
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Defines to hides both vertical and horizontal crosshair line.
        /// </summary>
        None,

        /// <summary>
        /// Defines to shows both vertical and horizontal crosshair line.
        /// </summary>
        Both,

        /// <summary>
        /// Defines to shows the vertical line.
        /// </summary>
        Vertical,

        /// <summary>
        /// Defines to shows the horizontal line.
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// Specifies the zooming mode.
    /// </summary>
    public enum ZoomMode
    {
        /// <summary>
        /// Defines chart will be zoomed with respect to both vertical and horizontal axis.
        /// </summary>
        XY,

        /// <summary>
        /// Defines chart will be zoomed with respect to horizontal axis.
        /// </summary>
        X,

        /// <summary>
        /// Defines chart will be zoomed with respect to vertical axis.
        /// </summary>
        Y
    }

    /// <summary>
    /// Specifies the selection mode.
    /// </summary>
    public enum AccumulationSelectionMode
    {
        /// <summary>
        /// Defines to disable the selection.
        /// </summary>
        None,

        /// <summary>
        /// Defines to select a point.
        /// </summary>
        Point
    }

    /// <summary>
    /// Specifies the mode of the group mode.
    /// </summary>
    public enum GroupMode
    {
        /// <summary>
        /// Defines selected points get grouped.
        /// </summary>
        Point,

        /// <summary>
        /// Defines the points, which less then values get grouped.
        /// </summary>
        Value
    }

    /// <summary>
    /// Specifies the mode of the pyramid.
    /// </summary>
    public enum PyramidMode
    {
        /// <summary>
        /// Defines height of the pyramid segments reflects the values.
        /// </summary>
        Linear,

        /// <summary>
        /// Defines surface or Area of the pyramid segments reflects the values.
        /// </summary>
        Surface
    }

    /// <summary>
    /// Specifies the accumulation chart series type.
    /// </summary>
    public enum AccumulationType
    {
        /// <summary>
        /// Defines pie type.
        /// </summary>
        Pie,

        /// <summary>
        /// Defines funnel type.
        /// </summary>
        Funnel,

        /// <summary>
        /// Defines pyramid type.
        /// </summary>
        Pyramid
    }

    /// <summary>
    /// Specifies the accumulation label position.
    /// </summary>
    public enum AccumulationLabelPosition
    {
        /// <summary>
        /// Defines the label position at accumulation series in-side.
        /// </summary>
        Inside,

        /// <summary>
        /// Defines the label position at accumulation series out-side.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Specifies the interval type of datetime axis.
    /// </summary>
    public enum RangeIntervalType
    {
        /// <summary>
        /// Defines the interval of the axis based on data.
        /// </summary>
        Auto,

        /// <summary>
        ///  Defines the interval of the axis in years.
        /// </summary>
        Years,

        /// <summary>
        /// Defines the interval of the axis based on data.
        /// </summary>
        Quarter,

        /// <summary>
        /// Defines the interval of the axis in months.
        /// </summary>
        Months,

        /// <summary>
        /// Defines the interval of the axis in weeks.
        /// </summary>
        Weeks,

        /// <summary>
        /// Defines the interval of the axis in days.
        /// </summary>
        Days,

        /// <summary>
        ///  Defines the interval of the axis in hours.
        /// </summary>
        Hours,

        /// <summary>
        /// Defines the interval of the axis in minutes.
        /// </summary>
        Minutes,

        /// <summary>
        /// Defines the interval of the axis in seconds.
        /// </summary>
        Seconds
    }

    /// <summary>
    /// Specifies the intersect action.
    /// </summary>
    public enum RangeLabelIntersectAction
    {
        /// <summary>
        /// Defines that the label will appear as like.
        /// </summary>
        None,

        /// <summary>
        /// Defines that the label will hide when it intersect.
        /// </summary>
        Hide
    }

    /// <summary>
    /// Specifies the label alignment of the axis.
    /// </summary>
    public enum LabelAlignment
    {
        /// <summary>
        /// Defines the lable alignment at near position.
        /// </summary>
        Near,

        /// <summary>
        /// Defines the lable alignment at middle position.
        /// </summary>
        Middle,

        /// <summary>
        /// Defines the lable alignment at far position.
        /// </summary>
        Far
    }

    /// <summary>
    /// Specifies the skeleton type for the axis.
    /// </summary>
    public enum SkeletonType
    {
        /// <summary>
        /// Defines the format as date.
        /// </summary>
        Date,

        /// <summary>
        /// Defines the format as  date and time.
        /// </summary>
        DateTime,

        /// <summary>
        /// Defines the format as time only.
        /// </summary>
        Time
    }

    /// <summary>
    /// Specifies the value Type for the axis.
    /// </summary>
    public enum RangeValueType
    {
        /// <summary>
        /// Defines the double type.
        /// </summary>
#pragma warning disable CA1720
        Double,

#pragma warning restore CA1720
        /// <summary>
        /// Defines the date and time type.
        /// </summary>
        DateTime,

        /// <summary>
        /// Defines the logarithmic type.
        /// </summary>
        Logarithmic
    }

    /// <summary>
    /// Specifies the type of series in the range navigator.
    /// </summary>
    public enum RangeNavigatorType
    {
        /// <summary>
        /// Defines the line type.
        /// </summary>
        Line,

        /// <summary>
        /// Defines the area type.
        /// </summary>
        Area,

        /// <summary>
        /// Defines the stepLine type.
        /// </summary>
        StepLine
    }

    /// <summary>
    /// Specifies the display mode for the range navigator tooltip.
    /// </summary>
    public enum TooltipDisplayMode
    {
        /// <summary>
        /// Defines the range navigator tooltip display mode as always.
        /// </summary>
        Always,

        /// <summary>
        /// Defines the range navigator tooltip display mode as on-demand.
        /// </summary>
        OnDemand
    }

    /// <summary>
    /// Specifies the type of thump in the range navigator.
    /// </summary>
    public enum ThumbType
    {
        /// <summary>
        /// Defines the circle position.
        /// </summary>
        Circle,

        /// <summary>
        /// Defines the rectangle position.
        /// </summary>
        Rectangle
    }

    /// <summary>
    /// Specifies the period of selector position.
    /// </summary>
    public enum PeriodSelectorPosition
    {
        /// <summary>
        /// Defines the top position.
        /// </summary>
        Top,

        /// <summary>
        /// Defines the bottom position.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Specifies the flag type for stock events.
    /// </summary>
    public enum FlagType
    {
        /// <summary>
        /// Defines the circle type.
        /// </summary>
        Circle,

        /// <summary>
        /// Defines the square type.
        /// </summary>
        Square,

        /// <summary>
        /// Defines the flag type.
        /// </summary>
        Flag,

        /// <summary>
        /// Defines the pin type.
        /// </summary>
        Pin,

        /// <summary>
        /// Defines the triangle type.
        /// </summary>
        Triangle,

        /// <summary>
        /// Defines the inverted triangle type.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Defines the triangle right type.
        /// </summary>
        TriangleRight,

        /// <summary>
        /// Defines the triangle left type.
        /// </summary>
        TriangleLeft,

        /// <summary>
        /// Defines the arrow up type.
        /// </summary>
        ArrowUp,

        /// <summary>
        /// Defines the arrow down type.
        /// </summary>
        ArrowDown,

        /// <summary>
        /// Defines the arrow left type.
        /// </summary>
        ArrowLeft,

        /// <summary>
        /// Defines the arrow right type.
        /// </summary>
        ArrowRight,

        /// <summary>
        /// Defines the flag type as text.
        /// </summary>
        Text
    }

    /// <summary>
    /// Specifies the area type of chart.
    /// </summary>
    public enum ChartAreaType
    {
        /// <summary>
        /// Defines to cartesian axes area type.
        /// </summary>
        CartesianAxes,

        /// <summary>
        /// Defines to polar axes area type.
        /// </summary>
        PolarAxes
    }

    /// <summary>
    /// Specifies the empty point mode of the chart.
    /// </summary>
    public enum EmptyPointMode
    {
        /// <summary>
        /// Defines to display empty points as space.
        /// </summary>
        Gap,

        /// <summary>
        /// Defines to display empty points as zero.
        /// </summary>
        Zero,

        /// <summary>
        /// Defines to ignore the empty point while rendering.
        /// </summary>
        Drop,

        /// <summary>
        /// Defines to display empty points as previous and next point average.
        /// </summary>
        Average
    }

    /// <summary>
    /// Specifies the orientation of chart axis.
    /// </summary>
    public enum Orientation
    {
        /// <summary>
        /// Defines the null orientation.
        /// </summary>
        Null,

        /// <summary>
        /// Defines the horizontal orientation.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Defines the vertical orientation.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies the series type of chart.
    /// </summary>
    public enum SeriesValueType
    {
        /// <summary>
        /// Defines the xy series type of chart.
        /// </summary>
        XY,

        /// <summary>
        /// Defines the high low series type of chart.
        /// </summary>
        HighLow,

        /// <summary>
        /// Defines the high low open close series type of chart.
        /// </summary>
        HighLowOpenClose,

        /// <summary>
        /// Defines the box plot series type of chart.
        /// </summary>
        BoxPlot
    }

    /// <summary>
    /// Specifies the zooming toolkit types.
    /// </summary>
    public enum ToolbarItems
    {
        /// <summary>
        /// Defines the zoom button to be render.
        /// </summary>
        Zoom,

        /// <summary>
        /// Defines the zoom in button to be render.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// Defines the zoom out button to be render.
        /// </summary>
        ZoomOut,

        /// <summary>
        /// Defines the pan button to be render.
        /// </summary>
        Pan,

        /// <summary>
        /// Defines the reset button to be render.
        /// </summary>
        Reset
    }

    /// <summary>
    /// Specifies the series categories type.
    /// </summary>
    public enum SeriesCategories
    {
        /// <summary>
        /// Defines the trend line type of series categories.
        /// </summary>
        TrendLine,

        /// <summary>
        ///  Defines the iondicator type of series categories.
        /// </summary>
        Indicator,

        /// <summary>
        /// Defines the series type of series categories.
        /// </summary>
        Series,

        /// <summary>
        /// Defines the pareto type of series categories.
        /// </summary>
        Pareto
    }

    /// <summary>
    /// Specifies the type of animation.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// Defines the progressive animation type.
        /// </summary>
        Progressive,

        /// <summary>
        /// Defines the linear animation type.
        /// </summary>
        Linear,

        /// <summary>
        /// Defines the rect animation type.
        /// </summary>
        Rect,

        /// <summary>
        /// Defines the marker animation type.
        /// </summary>
        Marker,

        /// <summary>
        /// Defines the polarRadar animation type.
        /// </summary>
        PolarRadar
    }
}