using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxis : ChartSubComponent, IChartElement
    {
        #region Axis COMPONENT BACKING FIELDS

        private bool needLayoutUpdate;

        private bool updateLabels;

        private bool needAxisRefresh;

        private bool needSeriesRefresh;

        private bool updateRange;

        private double interval = double.NaN;

        private object minimum;

        private object maximum;

        private IntervalType intervalType = IntervalType.Auto;

        private double zoomFactor = 1;

        private double desiredIntervals = double.NaN;

        private double zoomPosition;

        private object crossesAt;

        private ChartAxisMajorGridLines majorGridLines = new ChartAxisMajorGridLines();

        private ChartAxisLabelStyle labelStyle = new ChartAxisLabelStyle();

        private LabelPlacement labelPlacement = LabelPlacement.BetweenTicks;

        private EdgeLabelPlacement edgeLabelPlacement = EdgeLabelPlacement.None;

        private LabelIntersectAction labelIntersectAction = LabelIntersectAction.Trim;

        private ChartRangePadding rangePadding = ChartRangePadding.Auto;

        private bool isIndexed;

        private bool isInversed;

        private bool enableTrim;

        private bool opposedPosition;

        private bool visible = true;

        private string labelFormat = string.Empty;

        private string format = string.Empty;

        private double labelRotation;

        private ChartAxisScrollbarSettings scrollbarSettings = new ChartAxisScrollbarSettings();

        private double startAngle;

        private double maximumLabelWidth = 34;

        private AxisPosition labelPosition = AxisPosition.Outside;

        private AxisPosition tickPosition = AxisPosition.Outside;

        private bool placeNextToAxisLine = true;

        private string name = string.Empty;

        #endregion

        [CascadingParameter]
        internal SfChart Container { get; set; }

        /// <summary>
        /// Specifies indexed category  axis.
        /// </summary>
        [Parameter]
        public bool IsIndexed
        {
            get
            {
                return isIndexed;
            }

            set
            {
                if (isIndexed != value)
                {
                    isIndexed = value;
                    if (Renderer != null)
                    {
                        Renderer.Labels.Clear();
                        foreach (ChartSeriesRenderer seriesRenderer in Renderer.SeriesRenderer)
                        {
                            seriesRenderer.InitSeriesRendererFields();
                            seriesRenderer.ProcessData();
                        }

                        updateRange = !needLayoutUpdate;
                    }
                }
            }
        }

        /// <summary>
        /// It specifies whether the axis to be rendered in inversed manner or not.
        /// </summary>
        [Parameter]
        public bool IsInversed
        {
            get
            {
                return isInversed;
            }

            set
            {
                if (isInversed != value)
                {
                    isInversed = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needSeriesRefresh = true;
                    }
                }
            }
        }

        /// <summary>
        /// Unique identifier of an axis.
        /// To associate an axis with the series, set this name to the xAxisName/yAxisName properties of the series.
        /// </summary>
        [Parameter]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (name != value)
                {
                    if (Renderer != null && Container.AxisContainer.Axes.ContainsKey(name))
                    {
                        Container.AxisContainer.Axes.Remove(name);
                        Container.AxisContainer.Axes.Add(value, this);
                        name = value;
                    }
                    else
                    {
                        name = value;
                    }
                }
            }
        }

        /// <summary>
        /// If set to true, the axis will render at the opposite side of its default position.
        /// </summary>
        [Parameter]
        public bool OpposedPosition
        {
            get
            {
                return opposedPosition;
            }

            set
            {
                if (opposedPosition != value)
                {
                    opposedPosition = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needLayoutUpdate = true;
                    }
                }
            }
        }

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
        public ValueType ValueType { get; set; }

        /// <summary>
        /// If set to true, axis label will be visible.
        /// </summary>
        [Parameter]
        public bool Visible
        {
            get
            {
                return visible;
            }

            set
            {
                if (visible != value)
                {
                    visible = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary>
        /// The polar radar radius position.
        /// </summary>
        [Parameter]
        public double Coefficient { get; set; } = 100;

        /// <summary>
        /// Specifies the index of the column where the axis is associated,
        /// when the chart area is divided into multiple plot areas by using `Columns`.
        /// </summary>
        [Parameter]
        public double ColumnIndex { get; set; }

        /// <summary>
        /// Specifies the value at which the axis line has to be intersect with the vertical axis or vice versa.
        /// </summary>
        [Parameter]
        public object CrossesAt
        {
            get
            {
                return crossesAt;
            }

            set
            {
                if (crossesAt == null || !crossesAt.Equals(value))
                {
                    crossesAt = value;
                    if (Renderer != null && Renderer.CrossInAxis != null && !needLayoutUpdate)
                    {
                        Renderer.UpdateCrossValue();
                        if (Renderer.IsInside(Renderer.CrossInAxis.Renderer.VisibleRange, Renderer.CrossAt))
                        {
                            needAxisRefresh = true;
                        }
                        else
                        {
                            needLayoutUpdate = true;
                        }
                    }
                }
            }
        }

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
        public double DesiredIntervals
        {
            get
            {
                return desiredIntervals;
            }

            set
            {
                if (desiredIntervals != value && !(double.IsNaN(desiredIntervals) && double.IsNaN(value)))
                {
                    desiredIntervals = value;
                    if (Renderer != null && !needLayoutUpdate && !double.IsNaN(Interval))
                    {
                        Renderer.VisibleInterval = Renderer.ActualInterval = Renderer.CalculateActualInterval(Renderer.ActualRange);
                        updateLabels = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the position of labels at the edge of the axis.They are,
        ///  None: No action will be performed.
        ///  Hide: Edge label will be hidden.
        ///  Shift: Shifts the edge labels.
        /// </summary>
        [Parameter]
        public EdgeLabelPlacement EdgeLabelPlacement
        {
            get
            {
                return edgeLabelPlacement;
            }

            set
            {
                if (edgeLabelPlacement != value)
                {
                    edgeLabelPlacement = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needAxisRefresh = true;
                    }
                }
            }
        }

        /// <summary>
        /// If set to true, axis interval will be calculated automatically with respect to the zoomed range.
        /// </summary>
        [Parameter]
        public bool EnableAutoIntervalOnZooming { get; set; } = true;

        /// <summary>
        /// Enables the scrollbar for zooming.
        /// </summary>
        [Parameter]
        public bool EnableScrollbarOnZooming { get; set; } = true;

        /// <summary>
        /// Specifies the Trim property for an axis.
        /// </summary>
        [Parameter]
        public bool EnableTrim
        {
            get
            {
                return enableTrim;
            }

            set
            {
                if (enableTrim != value)
                {
                    enableTrim = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        updateLabels = true;
                    }
                }
            }
        }

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
        public IntervalType IntervalType
        {
            get
            {
                return intervalType;
            }

            set
            {
                if (intervalType != value)
                {
                    intervalType = value;
                    if (Renderer != null)
                    {
                        updateRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the interval for an axis.
        /// </summary>
        [Parameter]
        public double Interval
        {
            get
            {
                return interval;
            }

            set
            {
                if (interval != value && !(double.IsNaN(interval) && double.IsNaN(value)))
                {
                    interval = value;
                    if (Renderer != null)
                    {
                        Renderer.VisibleInterval = value;
                        updateLabels = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the minimum range of an axis.
        /// </summary>
        [Parameter]
        public object Minimum
        {
            get
            {
                return minimum;
            }

            set
            {
                if (minimum == null || !minimum.Equals(value))
                {
                    minimum = value;
                    if (Renderer != null && value != null)
                    {
                        double start = (Renderer.Axis.ValueType == ValueType.DateTime) ? (Renderer as DateTimeAxisRenderer).GetTime((DateTime)value) : Convert.ToDouble(value, null);
                        if (Renderer.NeedAxisLayoutChange(start, Renderer.VisibleRange.End))
                        {
                            needLayoutUpdate = true;
                        }
                        else
                        {
                            Renderer.VisibleRange = new DoubleRange(start, Renderer.VisibleRange.End);
                            updateLabels = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the placement of a label for category axis. They are,
        /// betweenTicks: Renders the label between the ticks.
        /// onTicks: Renders the label on the ticks.
        /// </summary>
        [Parameter]
        public LabelPlacement LabelPlacement
        {
            get
            {
                return labelPlacement;
            }

            set
            {
                if (labelPlacement != value && !needLayoutUpdate)
                {
                    labelPlacement = value;
                    if (Renderer != null)
                    {
                        updateRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the maximum range of an axis.
        /// </summary>
        [Parameter]
        public object Maximum
        {
            get
            {
                return maximum;
            }

            set
            {
                if (maximum == null || !maximum.Equals(value))
                {
                    maximum = value;
                    if (Renderer != null && value != null)
                    {
                        double end = (Renderer.Axis.ValueType == ValueType.DateTime) ? (Renderer as DateTimeAxisRenderer).GetTime((DateTime)value) : Convert.ToDouble(value, null);
                        if (Renderer.NeedAxisLayoutChange(Renderer.VisibleRange.Start, end))
                        {
                            needLayoutUpdate = true;
                        }
                        else
                        {
                            Renderer.VisibleRange = new DoubleRange(Renderer.VisibleRange.Start, end);
                            updateLabels = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The axis is scaled by this factor. When zoomFactor is 0.5, the chart is scaled by 200% along this axis. Value ranges from 0 to 1.
        /// </summary>
        [Parameter]
        public double ZoomFactor
        {
            get
            {
                return zoomFactor;
            }

            set
            {
                if (zoomFactor != value)
                {
                    zoomFactor = value;
                    if (Renderer != null && Container.IsChartFirstRender)
                    {
                        Renderer.VisibleRange = Renderer.CalculateVisibleRange(Renderer.ActualRange);
                        updateLabels = true;
                    }
                }
            }
        }

        /// <summary>
        /// Position of the zoomed axis. Value ranges from 0 to 1.
        /// </summary>
        [Parameter]
        public double ZoomPosition
        {
            get
            {
                return zoomPosition;
            }

            set
            {
                if (zoomPosition != value)
                {
                    zoomPosition = value;
                    if (Renderer != null && Container.IsChartFirstRender)
                    {
                        Renderer.VisibleRange = Renderer.CalculateVisibleRange(Renderer.ActualRange);
                        updateLabels = true;
                    }
                }
            }
        }

        /// <summary>
        /// Used to format the axis label that accepts any global string format like 'C', 'n1', 'P' etc.
        /// It also accepts placeholder like '{value}°C' in which value represent the axis label, e.g, 20°C.
        /// </summary>
        [Parameter]
        public string LabelFormat
        {
            get
            {
                return labelFormat;
            }

            set
            {
                if (labelFormat != value)
                {
                    labelFormat = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        if (Renderer.Orientation == Orientation.Vertical)
                        {
                            needLayoutUpdate = true;
                        }
                        else
                        {
                            updateLabels = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the actions like `None`, `Hide`, `Trim`, `Wrap`, `MultipleRows`, `Rotate45`, and `Rotate90`
        /// when the axis labels intersect with each other.They are,
        /// None: Shows all the labels.
        /// Hide: Hides the label when it intersects.
        /// Trim: Trim the label when it intersects.
        /// Wrap: Wrap the label when it intersects.
        /// MultipleRows: Shows the label in MultipleRows when it intersects.
        /// Rotate45: Rotates the label to 45 degree when it intersects.
        /// Rotate90: Rotates the label to 90 degree when it intersects.
        /// </summary>
        [Parameter]
        public LabelIntersectAction LabelIntersectAction
        {
            get
            {
                return labelIntersectAction;
            }

            set
            {
                if (labelIntersectAction != value)
                {
                    labelIntersectAction = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the labelPadding from axis.
        /// </summary>
        [Parameter]
        public double LabelPadding { get; set; } = 5;

        /// <summary>
        /// Specifies the placement of a labels to the axis line. They are,
        /// inside: Renders the labels inside to the axis line.
        /// outside: Renders the labels outside to the axis line.
        /// </summary>
        [Parameter]
        public AxisPosition LabelPosition
        {
            get
            {
                return labelPosition;
            }

            set
            {
                if (labelPosition != value && !needLayoutUpdate)
                {
                    labelPosition = value;
                    needLayoutUpdate = Renderer != null;
                }
            }
        }

        /// <summary>
        /// The angle to which the axis label gets rotated.
        /// </summary>
        [Parameter]
        public double LabelRotation
        {
            get
            {
                return labelRotation;
            }

            set
            {
                if (labelRotation != value)
                {
                    labelRotation = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary>
        /// Options to customize the axis label.
        /// </summary>
        [Parameter]
        public ChartAxisLabelStyle LabelStyle
        {
            get
            {
                return labelStyle;
            }

            set
            {
                if (labelStyle != value)
                {
                    labelStyle = value;
                    Renderer?.CustomizeGridRenderingOptions(nameof(LabelStyle));
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Options for customizing axis lines.
        /// </summary>
        [Parameter]
        public ChartAxisLineStyle LineStyle { get; set; } = new ChartAxisLineStyle();

        /// <summary>
        /// The base value for logarithmic axis. It requires `ValueType` to be `Logarithmic`.
        /// </summary>
        [Parameter]
        public double LogBase { get; set; } = 10;

        /// <summary>
        /// Options for customizing major grid lines.
        /// </summary>
        [Parameter]
        public ChartAxisMajorGridLines MajorGridLines
        {
            get
            {
                return majorGridLines;
            }

            set
            {
                if (majorGridLines != value)
                {
                    majorGridLines = value;
                    Renderer?.CustomizeGridRenderingOptions(nameof(MajorGridLines));
                    Renderer?.ProcessRenderQueue();
                }
            }
        }

        /// <summary>
        /// Options for customizing major tick lines.
        /// </summary>
        [Parameter]
        public ChartAxisMajorTickLines MajorTickLines { get; set; } = new ChartAxisMajorTickLines();

        /// <summary>
        /// Specifies the maximum width of an axis label.
        /// </summary>
        [Parameter]
        [DefaultValue(34)]
        public double MaximumLabelWidth
        {
            get
            {
                return maximumLabelWidth;
            }

            set
            {
                if (maximumLabelWidth != value)
                {
                    maximumLabelWidth = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        updateLabels = true;
                    }
                }
            }
        }

        /// <summary>
        /// The maximum number of label count per 100 pixels with respect to the axis length.
        /// </summary>
        [Parameter]
        public double MaximumLabels { get; set; } = 3;

        /// <summary>
        /// Options for customizing minor grid lines.
        /// </summary>
        [Parameter]
        public ChartAxisMinorGridLines MinorGridLines { get; set; } = new ChartAxisMinorGridLines();

        /// <summary>
        /// Options for customizing minor tick lines.
        /// </summary>
        [Parameter]
        public ChartAxisMinorTickLines MinorTickLines { get; set; } = new ChartAxisMinorTickLines();

        /// <summary>
        /// Specifies the number of minor ticks per interval.
        /// </summary>
        [Parameter]
        public double MinorTicksPerInterval { get; set; }

        /// <summary>
        /// Specifies whether axis elements like axis labels, axis title, etc has to be crossed with axis line.
        /// </summary>
        [Parameter]
        public bool PlaceNextToAxisLine
        {
            get
            {
                return placeNextToAxisLine;
            }

            set
            {
                if (placeNextToAxisLine != value)
                {
                    placeNextToAxisLine = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        needLayoutUpdate = true;
                    }
                }
            }
        }

        /// <summary>
        /// Left and right padding for the plot area in pixels.
        /// </summary>
        [Parameter]
        public double PlotOffset { get; set; }

        /// <summary>
        /// Bottom padding for the plot area in pixels.
        /// </summary>
        [Parameter]
        public double PlotOffsetBottom { get; set; } = double.NaN;

        /// <summary>
        /// Left padding for the plot area in pixels.
        /// </summary>
        [Parameter]
        public double PlotOffsetLeft { get; set; } = double.NaN;

        /// <summary>
        /// Right padding for the plot area in pixels.
        /// </summary>
        [Parameter]
        public double PlotOffsetRight { get; set; } = double.NaN;

        /// <summary>
        /// Top padding for the plot area in pixels.
        /// </summary>
        [Parameter]
        public double PlotOffsetTop { get; set; } = double.NaN;

        /// <summary>
        /// Specifies the padding for the axis range in terms of interval.They are,
        /// none: Padding cannot be applied to the axis.
        /// normal: Padding is applied to the axis based on the range calculation.
        /// additional: Interval of the axis is added as padding to the minimum and maximum values of the range.
        /// round: Axis range is rounded to the nearest possible value divided by the interval.
        /// </summary>
        [Parameter]
        public ChartRangePadding RangePadding
        {
            get
            {
                return rangePadding;
            }

            set
            {
                if (rangePadding != value)
                {
                    rangePadding = value;
                    if (Renderer != null)
                    {
                        updateRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Options for customizing the axis title.
        /// </summary>
        [Parameter]
        public ChartAxisTitleStyle TitleStyle { get; set; } = new ChartAxisTitleStyle();

        /// <summary>
        /// Specifies the index of the row where the axis is associated, when the chart area is divided into multiple plot areas by using `Rows`.
        /// </summary>
        [Parameter]
        public double RowIndex { get; set; }

        /// <summary>
        /// Specifies the skeleton format in which the dateTime format will process.
        /// </summary>
        [Parameter]
        public string Format
        {
            get
            {
                return format;
            }

            set
            {
                if (format != value)
                {
                    format = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        updateLabels = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the skeleton in which the dateTime will process.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated. Use Format property to achieve this.")]
        [Parameter]
        public string Skeleton { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the number of `Columns` or `rows` an axis has to span horizontally or vertically.
        /// </summary>
        [Parameter]
        public int Span { get; set; } = 1;

        /// <summary>
        /// The start angle for the series.
        /// </summary>
        [Parameter]
        public double StartAngle
        {
            get
            {
                return startAngle;
            }

            set
            {
                if (startAngle != value)
                {
                    startAngle = value;
                    if (Renderer != null && !needLayoutUpdate)
                    {
                        foreach (ChartAxisRenderer renderer in Container.AxisContainer.Renderers)
                        {
                            renderer.ClearAxisInfo();
                            renderer.UpdateAxisRendering();
                        }

                        foreach (ChartSeriesRenderer seriesRenderer in Renderer.SeriesRenderer)
                        {
                            seriesRenderer.UpdateDirection();
                            seriesRenderer.ProcessRenderQueue();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// It specifies whether the axis to be start from zero.
        /// </summary>
        [Parameter]
        public bool StartFromZero { get; set; } = true;

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
        public AxisPosition TickPosition
        {
            get
            {
                return tickPosition;
            }

            set
            {
                if (tickPosition != value && !needLayoutUpdate)
                {
                    tickPosition = value;
                    needLayoutUpdate = Renderer != null;
                }
            }
        }

        /// <summary>
        /// Specifies the stripLine collection for the axis.
        /// </summary>
        [Parameter]
        public List<ChartStripline> StripLines { get; set; } = new List<ChartStripline>();

        /// <summary>
        /// Specifies the multi level labels collection for the axis.
        /// </summary>
        [Parameter]
        public List<ChartMultiLevelLabel> MultiLevelLabels { get; set; } = new List<ChartMultiLevelLabel>();

        /// <summary>
        /// Border of the multi level labels.
        /// </summary>
        [Parameter]
        public ChartAxisLabelBorder Border { get; set; } = new ChartAxisLabelBorder();

        /// <summary>
        /// Option to customize scrollbar with lazy loading.
        /// </summary>
        [Parameter]
        public ChartAxisScrollbarSettings ScrollbarSettings
        {
            get
            {
                return scrollbarSettings;
            }

            set
            {
                if (scrollbarSettings != value)
                {
                    scrollbarSettings = value;
                }
            }
        }

        /// <summary>
        /// Options to customize the crosshair ToolTip.
        /// </summary>
        [Parameter]
        public ChartAxisCrosshairTooltip CrosshairTooltip { get; set; } = new ChartAxisCrosshairTooltip();

        internal ChartAxisRenderer Renderer { get; set; }

        public string RendererKey { get; set; }

        public Type RendererType { get; set; }

        internal double ScrollBarHeight { get; set; }

        public virtual string GetName()
        {
            return Name;
        }

        protected override void OnInitialized()
        {
            RendererType = ChartAxisRenderer.GetRendererType(ValueType);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Container != null)
            {
                Container.AddAxis(this);
            }

            if (needLayoutUpdate)
            {
                needLayoutUpdate = false;
                Renderer.Chart.OnLayoutChange();
            }
            else if (updateLabels || updateRange)
            {
                Renderer.ChangeAxisRange(updateRange);
                updateLabels = updateRange = false;
                UpdateSeries();
            }
            else if (needSeriesRefresh || needAxisRefresh)
            {
                Renderer.ClearAxisInfo();
                Renderer.UpdateAxisRendering();
                if (needSeriesRefresh)
                {
                    UpdateSeries();
                }

                needAxisRefresh = needSeriesRefresh = false;
            }
        }

        private void UpdateSeries()
        {
            Container.SeriesContainer.DataLabelCollection.Clear();
            foreach (ChartSeriesRenderer seriesRenderer in Renderer.SeriesRenderer)
            {
                seriesRenderer.UpdateDirection();
                seriesRenderer.ProcessRenderQueue();
            }
        }

        internal void UpdateZoomValues(double axisZoomFactor, double axisZoomPosition)
        {
            zoomFactor = axisZoomFactor;
            zoomPosition = axisZoomPosition;
        }

        internal void SetInverse()
        {
            IsInversed = Container.EnableRTL || IsInversed;
            needSeriesRefresh = false;
        }

        internal void SetOpposedPosition()
        {
            OpposedPosition = Container.EnableRTL || OpposedPosition;
            needLayoutUpdate = false;
        }

        internal void UpdateAxisProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(LabelStyle):
                    labelStyle = new ChartAxisLabelStyle();
                    LabelStyle = (ChartAxisLabelStyle)keyValue;
                    break;
                case nameof(LineStyle):
                    LineStyle = (ChartAxisLineStyle)keyValue;
                    break;
                case nameof(MajorGridLines):
                    majorGridLines = new ChartAxisMajorGridLines();
                    MajorGridLines = (ChartAxisMajorGridLines)keyValue;
                    break;
                case nameof(MajorTickLines):
                    MajorTickLines = (ChartAxisMajorTickLines)keyValue;
                    break;
                case nameof(MinorGridLines):
                    MinorGridLines = (ChartAxisMinorGridLines)keyValue;
                    break;
                case nameof(MinorTickLines):
                    MinorTickLines = (ChartAxisMinorTickLines)keyValue;
                    break;
                case nameof(MultiLevelLabels):
                    MultiLevelLabels = (List<ChartMultiLevelLabel>)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartAxisLabelBorder)keyValue;
                    break;
                case nameof(ScrollbarSettings):
                    ScrollbarSettings = (ChartAxisScrollbarSettings)keyValue;
                    break;
                case nameof(CrosshairTooltip):
                    CrosshairTooltip = (ChartAxisCrosshairTooltip)keyValue;
                    break;
                case nameof(TitleStyle):
                    TitleStyle = (ChartAxisTitleStyle)keyValue;
                    break;
            }
        }

        internal override void ComponentDispose()
        {
            Container?.RemoveAxis(this);
            ChildContent = null;
        }
    }
}
