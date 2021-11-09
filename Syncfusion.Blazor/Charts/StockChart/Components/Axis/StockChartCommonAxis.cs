using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart axis.
    /// </summary>
    public class StockChartCommonAxis : SfDataBoundComponent
    {
        private object crossesAt;
        private string crossesInAxis;
        private string description;
        private double desiredIntervals;
        private EdgeLabelPlacement edgeLabelPlacement;
        private bool enableAutoIntervalOnZooming;
        private bool enableTrim;
        private double interval;
        private IntervalType intervalType;
        private bool isInversed;
        private string labelFormat;
        private LabelIntersectAction labelIntersectAction;
        private LabelPlacement labelPlacement;
        private double labelRotation;
        private AxisPosition labelPosition;
        private ValueType valueType;
        private double zoomFactor;
        private double zoomPosition;
        private bool visible;
        private double logBase;
        private object maximum;
        private double maximumLabelWidth;
        private double maximumLabels;
        private object minimum;
        private double minorTicksPerInterval;
        private string name;
        private bool opposedPosition;
        private bool placeNextToAxisLine;
        private ChartRangePadding rangePadding;
        private double plotOffset;
        private int rowIndex;
        private double startAngle;
        private double tabIndex;
        private double span;
        private string title;
        private AxisPosition tickPosition;

        internal StockChartCommonCrosshairTooltip CrosshairTooltip { get; set; } = new StockChartCommonCrosshairTooltip();

        internal StockChartAxisLabelStyle LabelStyle { get; set; } = new StockChartAxisLabelStyle();

        internal StockChartCommonLineStyle LineStyle { get; set; } = new StockChartCommonLineStyle();

        internal StockChartCommonMajorGridLines MajorGridLines { get; set; } = new StockChartCommonMajorGridLines();

        internal StockChartCommonMajorTickLines MajorTickLines { get; set; } = new StockChartCommonMajorTickLines();

        internal StockChartCommonMinorGridLines MinorGridLines { get; set; } = new StockChartCommonMinorGridLines();

        internal List<StockChartCommonStripLine> StripLines { get; set; } = new List<StockChartCommonStripLine>();

        internal StockChartAxisTitleStyle TitleStyle { get; set; } = new StockChartAxisTitleStyle();

        internal StockChartCommonMinorTickLines MinorTickLines { get; set; } = new StockChartCommonMinorTickLines();

        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Specifies the value at which the axis line has to be intersect with the vertical axis or vice versa.
        /// </summary>
        [Parameter]
        public object CrossesAt { get; set; }

        /// <summary>
        /// Specifies axis name with which the axis line has to be crossed.
        /// </summary>
        [Parameter]
        public string CrossesInAxis { get; set; }

        /// <summary>
        /// Description for axis and its element.
        /// </summary>
        [Parameter]

        public string Description { get; set; }

        /// <summary>
        /// With this property, you can request axis to calculate intervals approximately equal to your specified interval.
        /// </summary>
        [Parameter]
        public double DesiredIntervals { get; set; } = double.NaN;

        /// <summary>
        /// Specifies the position of labels at the edge of the axis.They are,
        /// None: No action will be performed.
        /// Hide: Edge label will be hidden.
        /// Shift: Shifts the edge labels.
        /// </summary>
        [Parameter]
        public EdgeLabelPlacement EdgeLabelPlacement { get; set; }

        /// <summary>
        /// If set to true, axis interval will be calculated automatically with respect to the zoomed range.
        /// </summary>
        [Parameter]
        public bool EnableAutoIntervalOnZooming { get; set; } = true;

        /// <summary>
        /// Specifies the Trim property for an axis.
        /// </summary>
        [Parameter]
        public bool EnableTrim { get; set; }

        /// <summary>
        /// Specifies the interval for an axis.
        /// </summary>
        [Parameter]
        public double Interval { get; set; } = double.NaN;

        /// <summary>
        /// Specifies the types like `Years`, `Months`, `Days`, `Hours`, `Minutes`, `Seconds` in date time axis.They are,
        /// Auto: Defines the interval of the axis based on data.
        /// Years: Defines the interval of the axis in years.
        /// Months: Defines the interval of the axis in months.
        /// Days: Defines the interval of the axis in days.
        /// Hours: Defines the interval of the axis in hours.
        /// Minutes: Defines the interval of the axis in minutes.
        /// </summary>
        [Parameter]
        public IntervalType IntervalType { get; set; }

        /// <summary>
        /// It specifies whether the axis to be rendered in inversed manner or not.
        /// </summary>
        [Parameter]
        public bool IsInversed { get; set; }

        /// <summary>
        /// Used to format the axis label that accepts any global string format like 'C', 'n1', 'P' etc.
        /// It also accepts placeholder like '{value}°C' in which value represent the axis label, e.g, 20°C.
        /// </summary>
        [Parameter]
        public string LabelFormat { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the actions like `Hide`, `Rotate45`, and `Rotate90` when the axis labels intersect with each other.They are,
        /// None: Shows all the labels.
        /// Hide: Hides the label when it intersects.
        /// Rotate45: Rotates the label to 45 degree when it intersects.
        /// Rotate90: Rotates the label to 90 degree when it intersects.
        /// </summary>
        [Parameter]
        public LabelIntersectAction LabelIntersectAction { get; set; } = LabelIntersectAction.Hide;

        /// <summary>
        /// Specifies the placement of a label for category axis. They are,
        /// betweenTicks: Renders the label between the ticks.
        /// onTicks: Renders the label on the ticks.
        /// </summary>
        [Parameter]
        public LabelPlacement LabelPlacement { get; set; }

        /// <summary>
        /// Specifies the placement of a labels to the axis line. They are,
        /// inside: Renders the labels inside to the axis line.
        /// outside: Renders the labels outside to the axis line.
        /// </summary>
        [Parameter]
        public virtual AxisPosition LabelPosition { get; set; } = AxisPosition.Outside;

        /// <summary>
        /// The angle to which the axis label gets rotated.
        /// </summary>
        [Parameter]
        public double LabelRotation { get; set; }

        /// <summary>
        /// The base value for logarithmic axis. It requires `ValueType` to be `Logarithmic`.
        /// </summary>
        [Parameter]
        public double LogBase { get; set; } = 10;

        /// <summary>
        /// Specifies the maximum range of an axis.
        /// </summary>
        [Parameter]
        public object Maximum { get; set; }

        /// <summary>
        /// Specifies the maximum width of an axis label.
        /// </summary>
        [Parameter]
        public double MaximumLabelWidth { get; set; } = 34;

        /// <summary>
        /// The maximum number of label count per 100 pixels with respect to the axis length.
        /// </summary>
        [Parameter]
        public double MaximumLabels { get; set; } = 3;

        /// <summary>
        /// Specifies the minimum range of an axis.
        /// </summary>
        [Parameter]
        public object Minimum { get; set; }

        /// <summary>
        /// Specifies the number of minor ticks per interval.
        /// </summary>
        [Parameter]
        public double MinorTicksPerInterval { get; set; }

        /// <summary>
        /// Unique identifier of an axis.
        /// To associate an axis with the series, set this name to the xAxisName/yAxisName properties of the series.
        /// </summary>
        [Parameter]
        public virtual string Name { get; set; } = string.Empty;

        /// <summary>
        /// If set to true, the axis will render at the opposite side of its default position.
        /// </summary>
        [Parameter]
        public virtual bool OpposedPosition { get; set; }

        /// <summary>
        /// Specifies whether axis elements like axis labels, axis title, etc has to be crossed with axis line.
        /// </summary>
        [Parameter]
        public bool PlaceNextToAxisLine { get; set; } = true;

        /// <summary>
        /// Left and right padding for the plot area in pixels.
        /// </summary>
        [Parameter]
        public double PlotOffset { get; set; }

        /// <summary>
        /// Specifies the padding for the axis range in terms of interval.They are,
        ///  none: Padding cannot be applied to the axis.
        ///  normal: Padding is applied to the axis based on the range calculation.
        ///  additional: Interval of the axis is added as padding to the minimum and maximum values of the range.
        ///  round: Axis range is rounded to the nearest possible value divided by the interval.
        /// </summary>
        [Parameter]
        public ChartRangePadding RangePadding { get; set; }

        /// <summary>
        /// Specifies the index of the row where the axis is associated, when the chart area is divided into multiple plot areas by using `Rows`.
        ///
        /// </summary>
        [Parameter]
        public int RowIndex { get; set; }

        /// <summary>
        /// Specifies the skeleton format in which the dateTime format will process.
        /// </summary>
        [Parameter]
        public string Skeleton { get; set; } = string.Empty;

        /// <summary>
        /// It specifies the type of format to be used in dateTime format process.
        /// </summary>
        [Parameter]
        public SkeletonType SkeletonType { get; set; } = SkeletonType.DateTime;

        /// <summary>
        /// Specifies the number of `Columns` or `rows` an axis has to span horizontally or vertically.
        /// </summary>
        [Parameter]
        public double Span { get; set; } = 1;

        /// <summary>
        /// The start angle for the series.
        /// </summary>
        [Parameter]
        public double StartAngle { get; set; }

        /// <summary>
        /// TabIndex value for the axis.
        /// </summary>
        [Parameter]
        public double TabIndex { get; set; } = 2;

        /// <summary>
        /// Specifies the placement of a ticks to the axis line. They are,
        /// inside: Renders the ticks inside to the axis line.
        /// outside: Renders the ticks outside to the axis line.
        /// </summary>
        [Parameter]
        public AxisPosition TickPosition { get; set; } = AxisPosition.Outside;

        /// <summary>
        /// Specifies the title of an axis.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the type of data the axis is handling.
        /// Double:  Renders a numeric axis.
        /// DateTime: Renders a dateTime axis.
        /// Category: Renders a category axis.
        /// Logarithmic: Renders a log axis.
        /// </summary>
        [Parameter]
        public virtual ValueType ValueType { get; set; }

        /// <summary>
        /// If set to true, axis label will be visible.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// The axis is scaled by this factor. When zoomFactor is 0.5, the chart is scaled by 200% along this axis. Value ranges from 0 to 1.
        /// </summary>
        [Parameter]
        public double ZoomFactor { get; set; } = 1;

        /// <summary>
        /// Position of the zoomed axis. Value ranges from 0 to 1.
        /// </summary>
        [Parameter]
        public double ZoomPosition { get; set; }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            crossesAt = NotifyPropertyChanges(nameof(CrossesAt), CrossesAt, crossesAt);
            crossesInAxis = NotifyPropertyChanges(nameof(CrossesInAxis), CrossesInAxis, crossesInAxis);
            description = NotifyPropertyChanges(nameof(Description), Description, description);
            desiredIntervals = NotifyPropertyChanges(nameof(DesiredIntervals), DesiredIntervals, desiredIntervals);
            edgeLabelPlacement = NotifyPropertyChanges(nameof(EdgeLabelPlacement), EdgeLabelPlacement, edgeLabelPlacement);
            enableAutoIntervalOnZooming = NotifyPropertyChanges(nameof(EnableAutoIntervalOnZooming), EnableAutoIntervalOnZooming, enableAutoIntervalOnZooming);
            enableTrim = NotifyPropertyChanges(nameof(EnableTrim), EnableTrim, enableTrim);
            interval = NotifyPropertyChanges(nameof(Interval), Interval, interval);
            intervalType = NotifyPropertyChanges(nameof(IntervalType), IntervalType, intervalType);
            labelFormat = NotifyPropertyChanges(nameof(LabelFormat), LabelFormat, labelFormat);
            labelIntersectAction = NotifyPropertyChanges(nameof(LabelIntersectAction), LabelIntersectAction, labelIntersectAction);
            labelPlacement = NotifyPropertyChanges(nameof(LabelPlacement), LabelPlacement, labelPlacement);
            labelPosition = NotifyPropertyChanges(nameof(LabelPosition), LabelPosition, labelPosition);
            labelRotation = NotifyPropertyChanges(nameof(LabelRotation), LabelRotation, labelRotation);
            logBase = NotifyPropertyChanges(nameof(LogBase), LogBase, logBase);
            maximum = NotifyPropertyChanges(nameof(Maximum), Maximum, maximum);
            minimum = NotifyPropertyChanges(nameof(Minimum), Minimum, minimum);
            maximumLabels = NotifyPropertyChanges(nameof(MaximumLabels), MaximumLabels, maximumLabels);
            maximumLabelWidth = NotifyPropertyChanges(nameof(MaximumLabelWidth), MaximumLabelWidth, maximumLabelWidth);
            minorTicksPerInterval = NotifyPropertyChanges(nameof(MinorTicksPerInterval), MinorTicksPerInterval, minorTicksPerInterval);
            name = NotifyPropertyChanges(nameof(Name), Name, name);
            opposedPosition = NotifyPropertyChanges(nameof(OpposedPosition), OpposedPosition, opposedPosition);
            placeNextToAxisLine = NotifyPropertyChanges(nameof(PlaceNextToAxisLine), PlaceNextToAxisLine, placeNextToAxisLine);
            plotOffset = NotifyPropertyChanges(nameof(PlotOffset), PlotOffset, plotOffset);
            rangePadding = NotifyPropertyChanges(nameof(RangePadding), RangePadding, rangePadding);
            rowIndex = NotifyPropertyChanges(nameof(RowIndex), RowIndex, rowIndex);
            span = NotifyPropertyChanges(nameof(Span), Span, span);
            startAngle = NotifyPropertyChanges(nameof(StartAngle), StartAngle, startAngle);
            tabIndex = NotifyPropertyChanges(nameof(TabIndex), TabIndex, tabIndex);
            tickPosition = NotifyPropertyChanges(nameof(TickPosition), TickPosition, tickPosition);
            title = NotifyPropertyChanges(nameof(Title), Title, title);
            valueType = NotifyPropertyChanges(nameof(ValueType), ValueType, valueType);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            zoomFactor = NotifyPropertyChanges(nameof(ZoomFactor), ZoomFactor, zoomFactor);
            zoomPosition = NotifyPropertyChanges(nameof(ZoomPosition), ZoomPosition, zoomPosition);
            isInversed = NotifyPropertyChanges(nameof(IsInversed), IsInversed, isInversed);
            if (PropertyChanges.Any() && IsRendered)
            {
                StockChartInstance.PropertyChanges.TryAdd(Name, this);
                PropertyChanges.Clear();
                StockChartInstance.OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            StockChartInstance = null;
            CrossesAt = crossesAt = null;
            CrosshairTooltip = null;
            LabelStyle = null;
            LineStyle = null;
            MajorGridLines = null;
            MajorTickLines = null;
            Maximum = maximum = null;
            Minimum = minimum = null;
            MinorGridLines = null;
            MinorTickLines = null;
            StripLines = null;
            TitleStyle = null;
        }
    }
}