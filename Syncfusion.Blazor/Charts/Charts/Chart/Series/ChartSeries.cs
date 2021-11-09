using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public class ChartSeries : ChartDataBoundComponent, IChartElement
    {
        private ChartSeriesRenderer renderer;
        private Type rendererType;
        private bool shouldWire = true;
        private bool needLayoutUpdate;
        private bool refreshRange;
        private bool refreshSeries;
        private bool shouldProcess = true;

        #region SERIES COMPONENT BACKING FIELDS
        private ChartSeriesType type = ChartSeriesType.Line;
        private bool visible = true;
        private double width = 1;
        private string x_AxisName = Constants.PRIMARYXAXIS;
        private string x_Name = string.Empty;
        private string y_AxisName = Constants.PRIMARYYAXIS;
        private string y_Name = string.Empty;
        private string high = string.Empty;
        private string low = string.Empty;
        private string open = string.Empty;
        private string close = string.Empty;
        private string volume = string.Empty;
        private string name = string.Empty;
        private int z_Order;
        private string fill;
        private IEnumerable<object> dataSource;
        private Syncfusion.Blazor.Data.Query query;
        private ChartDrawType drawType = ChartDrawType.Line;
        private bool enableComplexProperty;
        private bool enableTooltip = true;
        private string tooltipFormat;
        private string pointColorMapping = string.Empty;
        private string size = string.Empty;
        private string dashArray = "0";
        private double opacity = 1;

        private LegendShape legendShape = LegendShape.SeriesType;

        private ChartMarker marker = new ChartMarker();
        private ChartEmptyPointSettings emptyPointSettings = new ChartEmptyPointSettings();
        private double columnSpacing;

        private double columnWidth = double.NaN;
        private string stackingGroup = string.Empty;
        private Segment segmentAxis = Segment.X;
        private bool isClosed = true;
        private double cardinalSplineTension = 0.5;
        private SplineType splineType = Blazor.Charts.SplineType.Natural;
        private bool enableSolidCandles;
        private string bearFillColor = "#2ecd71";
        private string bullFillColor = "#e74c3d";
        private double maxRadius = 3;
        private double minRadius = 1;

        private ChartSeriesConnector connector = new ChartSeriesConnector();
        private string tooltipMappingName = string.Empty;
        private double[] intermediateSumIndexed;
        private double[] sumIndexes;
        private string summaryFillColor = "#4E81BC";
        private string negativeFillColor = "#C64E4A";
        private bool showNormalDistribution;
        private double binInterval = double.NaN;
        private BoxPlotMode boxPlotMode = BoxPlotMode.Normal;
        private bool showMean = true;

        private List<ChartTrendline> trendlines = new List<ChartTrendline>();
        private List<ChartSegment> segments = new List<ChartSegment>();
        private ChartSeriesAnimation animation = new ChartSeriesAnimation();

        private ChartErrorBarSettings errorBar = new ChartErrorBarSettings();

        private ChartErrorBarCapSettings errorBarCap = new ChartErrorBarCapSettings();

        private string selectionStyle;
        private string un_SelectedStyle;
        private string nonHighlightStyle;
        private string highlightStyle;

        private ChartDataEditSettings chartDataEditSettings = new ChartDataEditSettings();
        private int labelCurrentCount;
        private int labelPreviousCount;
        #endregion

        #region SERIES PUBLIC API

        /// <summary>
        /// Specifies the type of series
        /// The type of the series are
        /// Line
        /// Column
        /// Area
        /// Bar
        /// Histogram
        /// StackingColumn
        /// StackingArea
        /// StackingBar
        /// StepLine
        /// StepArea
        /// Scatter
        /// Spline
        /// StackingColumn100
        /// StackingBar100
        /// StackingArea100
        /// RangeColumn
        /// Hilo
        /// HiloOpenClose
        /// Waterfall
        /// RangeArea
        /// Bubble
        /// Candle
        /// Polar
        /// Radar
        /// BoxAndWhisker
        /// Pareto.
        /// </summary>
        [Parameter]
        public ChartSeriesType Type
        {
            get
            {
                return type;
            }

            set
            {
                if (type != value)
                {
                    if (Container == null)
                    {
                        type = value;
                    }

                    if (Container != null && Container.SeriesContainer != null)
                    {
                        if (value == ChartSeriesType.Pareto || type == ChartSeriesType.Pareto)
                        {
                            type = value;
                            RendererType = ChartSeriesRenderer.GetRendererType(type, DrawType);
                            if (value != ChartSeriesType.Pareto)
                            {
                                ChartSeriesRenderer renderer = Container.SeriesContainer.Renderers.Find(renderer => renderer.GetType().Equals(typeof(ParetoLineSeriesRenderer))) as ChartSeriesRenderer;
                                Container.SeriesContainer.RemoveRenderer(renderer);
                                Container.AxisContainer.RemoveParetoAxis(renderer.YAxisRenderer.Axis);
                                CurrentViewData = DataSource;
                            }

                            RendererType = ChartSeriesRenderer.GetRendererType(type, DrawType);
                            NeedRendererRemove = true;
                            Container.SeriesContainer.RendererShouldRender = true;
                            Container.SeriesContainer.Prerender();
                        }
                        else if (IsCartesianToPolar(type, value))
                        {
                            type = value;
                            RendererType = ChartSeriesRenderer.GetRendererType(type, DrawType);
                            NeedRendererRemove = true;
                            Container.SeriesContainer.RendererShouldRender = true;
                            Container.SeriesContainer.Prerender();
                        }
                        else if (value == ChartSeriesType.Polar || value == ChartSeriesType.Radar)
                        {
                            type = value;
                            Container.AxisContainer.AxisLayout.IsPolar = type == ChartSeriesType.Polar;
                            Container.AxisContainer.UpdateAxisRendering();
                            if (DrawType == ChartDrawType.Column || DrawType == ChartDrawType.StackingColumn || DrawType == ChartDrawType.RangeColumn)
                            {
                                Renderer.UpdateDirection();
                                Renderer.ProcessRenderQueue();
                            }
                        }
                        else if (value.ToString().Contains("Bar", StringComparison.InvariantCulture) || type.ToString().Contains("Bar", StringComparison.InvariantCulture))
                        {
                            type = value;
                            RendererType = ChartSeriesRenderer.GetRendererType(type, DrawType);
                            NeedRendererRemove = true;
                            Container.SeriesContainer.RendererShouldRender = true;
                            Container.SeriesContainer.Prerender();
                        }
                        else
                        {
                            type = value;
                            RendererType = ChartSeriesRenderer.GetRendererType(type, DrawType);
                            NeedRendererUpdate = true;
                            NeedRendererRemove = true;
                            Container.SeriesContainer.RendererShouldRender = true;
                            Container.SeriesContainer.Prerender();
                            if (Container.LegendRenderer != null)
                            {
                                Container.LegendRenderer.RendererShouldRender = visible;
                                Container.LegendRenderer.UpdateLegendShape(Renderer);
                                Container.LegendRenderer.ProcessRenderQueue();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the visibility of series.
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
                    if (Renderer != null)
                    {
                        if (FindLayoutChange())
                        {
                            needLayoutUpdate = true;
                        }
                        else
                        {
                            refreshRange = true;
                        }

                        if (Renderer.IsCategoryAxis())
                        {
                            labelPreviousCount = Renderer.XAxisRenderer.Labels.Count;
                            Renderer.UpdateCategoryData();
                            labelCurrentCount = Renderer.XAxisRenderer.Labels.Count;
                        }

                        if (Container != null && Container.LegendRenderer != null)
                        {
                            Container.LegendRenderer.RendererShouldRender = true;
                            Container.LegendRenderer.UpdateLegendFill(Renderer);
                            Container.LegendRenderer.ProcessRenderQueue();
                        }

                        Container.SeriesContainer.UpdateStackingValues();
                    }
                }
            }
        }

        /// <summary>
        /// The stroke width for the series that is applicable only for `Line` type series.
        /// It also represent the stroke width of the signal lines in technical indicators.
        /// </summary>
        [Parameter]
        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width != value)
                {
                    width = value;
                    Renderer?.UpdateCustomization(nameof(Width));
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// The name of the horizontal axis associated with the series. It requires `Axes` of the chart.
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string XAxisName
        {
            get
            {
                return x_AxisName;
            }

            set
            {
                if (x_AxisName != value)
                {
                    x_AxisName = value;
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the x value.
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string XName
        {
            get
            {
                return x_Name;
            }

            set
            {
                if (x_Name != value)
                {
                    x_Name = value;
                    if (renderer != null)
                    {
                        Renderer?.UpdateSeriesData();
                    }
                }
            }
        }

        /// <summary>
        /// The name of the vertical axis associated with the series. It requires `Axes` of the chart.
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string YAxisName
        {
            get
            {
                return y_AxisName;
            }

            set
            {
                if (y_AxisName != value)
                {
                    y_AxisName = value;
                    if (Renderer != null)
                    {
                        Renderer.YAxisName = y_AxisName;
                        Container.ProcessOnLayoutChange();
                    }
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the y value.
        /// </summary>
        [Parameter]
        public string YName
        {
            get
            {
                return y_Name;
            }

            set
            {
                if (y_Name != value)
                {
                    y_Name = value;
                    if (Renderer != null)
                    {
                        Renderer?.UpdateSeriesData();
                    }
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the high value for financial type series.
        /// </summary>
        [Parameter]
        public string High
        {
            get
            {
                return high;
            }

            set
            {
                if (high != value)
                {
                    high = value;
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the low value for financial type series.
        /// </summary>
        [Parameter]
        public string Low
        {
            get
            {
                return low;
            }

            set
            {
                if (low != value)
                {
                    low = value;
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the open value for financial type series.
        /// </summary>
        [Parameter]
        public string Open
        {
            get
            {
                return open;
            }

            set
            {
                if (open != value)
                {
                    open = value;
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the close value for financial type series.
        /// </summary>
        [Parameter]
        public string Close
        {
            get
            {
                return close;
            }

            set
            {
                if (close != value)
                {
                    close = value;
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the volume value for financial type series.
        /// </summary>
        [Parameter]
        public string Volume
        {
            get
            {
                return volume;
            }

            set
            {
                if (volume != value)
                {
                    volume = value;
                }
            }
        }

        /// <summary>
        /// Specifies the name of the series.
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
                    name = value;
                }
            }
        }

        /// <summary>
        /// Specifies the z order of the series.
        /// </summary>
        [Parameter]
        public int ZOrder
        {
            get
            {
                return z_Order;
            }

            set
            {
                if (z_Order != value)
                {
                    z_Order = value;
                }
            }
        }

        /// <summary>
        /// The fill color for the series that accepts value in hex and rgba as a valid CSS color string.
        /// It also represents the color of the signal lines in technical indicators.
        /// For technical indicators, the default value is 'blue' and for series, it has null.
        /// </summary>
        [Parameter]
        public string Fill
        {
            get
            {
                return fill;
            }

            set
            {
                if (fill != value)
                {
                    fill = value;
                    if (Renderer != null)
                    {
                        Renderer.Interior = Fill;
                        Renderer.UpdateCustomization(nameof(Fill));
                        Renderer.Container?.AddToRenderQueue(Renderer);
                        if (Container != null && Container.LegendRenderer != null)
                        {
                            Container.LegendRenderer.RendererShouldRender = true;
                            Container.LegendRenderer.UpdateLegendFill(Renderer, Fill);
                            Container.LegendRenderer.ProcessRenderQueue();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the DataSource for the chart. It can be an array of JSON objects or an instance of DataManager.
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource
        {
            get
            {
                return dataSource;
            }

            set
            {
                if (value != null && shouldProcess)
                {
                    if (Renderer == null)
                    {
                        dataSource = (value is INotifyCollectionChanged) ? value : value.ToList();
                        SetDataManager<object>(DataSource);
                        if (dataSource != null && shouldWire && dataSource is INotifyCollectionChanged)
                        {
                            shouldWire = false;
                            ((INotifyCollectionChanged)dataSource).CollectionChanged += DataCollectionChanged;
                        }
                        shouldProcess = false;
                    }
                    else if (Renderer != null && Container != null && Container.IsChartFirstRender && (dataSource?.Count() != value.Count() || !SfBaseUtils.Equals(dataSource, value)))
                    {
                        dataSource = (value is INotifyCollectionChanged) ? value : value.ToList();
                        SetDataManager<object>(DataSource);
                        UpdateDataSource = true;
                        shouldProcess = false;
                    }
                    else if (!Container.IsChartFirstRender)
                    {
                        dataSource = (value is INotifyCollectionChanged) ? value : value.ToList();
                        shouldProcess = false;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies query to select data from DataSource. This property is applicable only when the DataSource is `SfDataManager`.
        /// </summary>
        [Parameter]
        public Syncfusion.Blazor.Data.Query Query
        {
            get
            {
                return query;
            }

            set
            {
                if (query != value)
                {
                    query = value;
                    if (Renderer != null)
                    {
                        Renderer?.UpdateSeriesData();
                    }
                }
            }
        }

        /// <summary>
        /// Type of series to be drawn in radar or polar series. They are
        /// 'Line'
        /// 'Column'
        /// 'Area'
        /// 'Scatter'
        /// 'Spline'
        /// 'StackingColumn'
        /// 'StackingArea'
        /// 'RangeColumn'
        /// 'SplineArea'.
        /// </summary>
        [Parameter]
        public ChartDrawType DrawType
        {
            get
            {
                return drawType;
            }

            set
            {
                if (drawType != value)
                {
                    if (Container == null)
                    {
                        drawType = value;
                    }

                    if (Container != null && Container.SeriesContainer != null)
                    {
                        drawType = value;
                        RendererType = ChartSeriesRenderer.GetRendererType(type, DrawType);
                        NeedRendererUpdate = true;
                        NeedRendererRemove = true;
                        Container.SeriesContainer.RendererShouldRender = true;
                        Container.SeriesContainer.Prerender();
                        Container.LegendRenderer.RendererShouldRender = true;
                        Container.LegendRenderer.UpdateLegendShape(Renderer);
                        Container.LegendRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// This property used to improve chart performance via data mapping for series dataSource.
        /// </summary>
        [Parameter]
        public bool EnableComplexProperty
        {
            get
            {
                return enableComplexProperty;
            }

            set
            {
                if (enableComplexProperty != value)
                {
                    enableComplexProperty = value;
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// If set true, the Tooltip for series will be visible.
        /// </summary>
        [Parameter]
        public bool EnableTooltip
        {
            get
            {
                return enableTooltip;
            }

            set
            {
                if (enableTooltip != value)
                {
                    enableTooltip = value;
                }
            }
        }

        /// <summary>
        /// Specifies the format of the tooltip for the series.
        /// </summary>
        [Parameter]
        public string TooltipFormat
        {
            get
            {
                return tooltipFormat;
            }

            set
            {
                if (tooltipFormat != value)
                {
                    tooltipFormat = value;
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the color value of point
        /// It is applicable for series.
        /// </summary>
        [Parameter]
        public string PointColorMapping
        {
            get
            {
                return pointColorMapping;
            }

            set
            {
                if (pointColorMapping != value)
                {
                    pointColorMapping = value;
                    Renderer?.InitSeriesRendererFields();
                    Renderer?.ProcessData();
                    Renderer?.UpdateDirection();
                }
            }
        }

        /// <summary>
        /// The DataSource field that contains the size value for the bubble series.
        /// </summary>
        [Parameter]
        public string Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size != value)
                {
                    size = value;
                }
            }
        }

        /// <summary>
        /// Defines the pattern of dashes and gaps to stroke the lines in `Line` type series.
        /// </summary>
        [Parameter]
        public string DashArray
        {
            get
            {
                return dashArray;
            }

            set
            {
                if (dashArray != value)
                {
                    dashArray = value;
                    Renderer?.UpdateCustomization(nameof(DashArray));
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Defines the opacity of the series fill.
        /// </summary>
        [Parameter]
        public double Opacity
        {
            get
            {
                return opacity;
            }

            set
            {
                if (opacity != value)
                {
                    opacity = value;
                    Renderer?.UpdateCustomization(nameof(Opacity));
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Defines the border of the rectangle shaped series.
        /// </summary>
        [Parameter]
        public ChartSeriesBorder Border { get; set; } = new ChartSeriesBorder();

        /// <summary>
        /// Specifies the legend shape of the series.
        /// </summary>
        [Parameter]
        public LegendShape LegendShape
        {
            get
            {
                return legendShape;
            }

            set
            {
                if (legendShape != value)
                {
                    legendShape = value;
                    if (Container != null && Container.LegendRenderer != null)
                    {
                        Container.LegendRenderer.RendererShouldRender = visible;
                        Container.LegendRenderer.UpdateLegendShape(Renderer);
                        Container.LegendRenderer.ProcessRenderQueue();
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the customization of the marker of the series.
        /// </summary>
        [Parameter]
        public ChartMarker Marker
        {
            get
            {
                return marker;
            }

            set
            {
                if (marker != value)
                {
                    marker = value;
                }
            }
        }

        /// <summary>
        /// Specifies the customization of the empty point settins for the series.
        /// </summary>
        [Parameter]
        public ChartEmptyPointSettings EmptyPointSettings
        {
            get
            {
                return emptyPointSettings;
            }

            set
            {
                if (emptyPointSettings != value)
                {
                    emptyPointSettings = value;
                }
            }
        }

        /// <summary>
        /// Defines the space between adjacent series for the rectangle shaped series.
        /// </summary>
        [Parameter]
        public double ColumnSpacing
        {
            get
            {
                return columnSpacing;
            }

            set
            {
                if (columnSpacing != value)
                {
                    columnSpacing = value;
                    Renderer?.UpdateDirection();
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Specifies the corner radius of the rectangle shaped series.
        /// </summary>
        [Parameter]
        public ChartCornerRadius CornerRadius { get; set; } = new ChartCornerRadius();

        /// <summary>
        /// Specifies the column width of the rectangle typed series.
        /// </summary>
        [Parameter]
        public double ColumnWidth
        {
            get
            {
                return columnWidth;
            }

            set
            {
                if (columnWidth != value)
                {
                    columnWidth = value;
                    Renderer?.UpdateDirection();
                    Renderer?.Container?.AddToRenderQueue(Renderer);
                }
            }
        }

        /// <summary>
        /// Based on this value stacking series are grouped together.
        /// </summary>
        [Parameter]
        public string StackingGroup
        {
            get
            {
                return stackingGroup;
            }

            set
            {
                if (stackingGroup != value)
                {
                    stackingGroup = value;
                }
            }
        }

        /// <summary>
        /// Specifies in which axis segment will be done.
        /// </summary>
        [Parameter]
        public Segment SegmentAxis
        {
            get
            {
                return segmentAxis;
            }

            set
            {
                if (segmentAxis != value)
                {
                    segmentAxis = value;
                }
            }
        }

        /// <summary>
        /// Enables the polar line based series to be closed.
        /// </summary>
        [Parameter]
        public bool IsClosed
        {
            get
            {
                return isClosed;
            }

            set
            {
                if (isClosed != value)
                {
                    isClosed = value;
                    if (Renderer != null)
                    {
                        refreshRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the CardinalSplineTension for spline series.
        /// </summary>
        [Parameter]
        public double CardinalSplineTension
        {
            get
            {
                return cardinalSplineTension;
            }

            set
            {
                if (cardinalSplineTension != value)
                {
                    cardinalSplineTension = value;
                }
            }
        }

        /// <summary>
        /// Specifies the splineType for spline series.
        /// </summary>
        [Parameter]
        public SplineType SplineType
        {
            get
            {
                return splineType;
            }

            set
            {
                if (splineType != value)
                {
                    splineType = value;
                }
            }
        }

        /// <summary>
        /// Enables SolidCandles for candle series.
        /// </summary>
        [Parameter]
        public bool EnableSolidCandles
        {
            get
            {
                return enableSolidCandles;
            }

            set
            {
                if (enableSolidCandles != value)
                {
                    enableSolidCandles = value;
                }
            }
        }

        /// <summary>
        /// Specifies BearFillColor for candle series.
        /// </summary>
        [Parameter]
        public string BearFillColor
        {
            get
            {
                return bearFillColor;
            }

            set
            {
                if (bearFillColor != value)
                {
                    bearFillColor = value;
                }
            }
        }

        /// <summary>
        /// Specifies the BullFillColor for candle series.
        /// </summary>
        [Parameter]
        public string BullFillColor
        {
            get
            {
                return bullFillColor;
            }

            set
            {
                if (bullFillColor != value)
                {
                    bullFillColor = value;
                }
            }
        }

        /// <summary>
        /// Denoted the maximum radius for bubble series.
        /// </summary>
        [Parameter]
        public double MaxRadius
        {
            get
            {
                return maxRadius;
            }

            set
            {
                if (maxRadius != value)
                {
                    maxRadius = value;
                }
            }
        }

        /// <summary>
        /// Denoted the minimum radius for bubble series.
        /// </summary>
        [Parameter]
        public double MinRadius
        {
            get
            {
                return minRadius;
            }

            set
            {
                if (minRadius != value)
                {
                    minRadius = value;
                }
            }
        }

        ///// <summary>
        ///// Specifies the customization option for the connector lines
        ///// </summary>
        [Parameter]
        public ChartSeriesConnector Connector
        {
            get
            {
                return connector;
            }

            set
            {
                if (connector != value)
                {
                    connector = value;
                }
            }
        }

        /// <summary>
        /// Specifies the intermediateSumIndexes for waterfall series
        /// The provided value will be considered as a Tooltip Mapping name.
        /// </summary>
        [Parameter]
        public string TooltipMappingName
        {
            get
            {
                return tooltipMappingName;
            }

            set
            {
                if (tooltipMappingName != value)
                {
                    tooltipMappingName = value;
                }
            }
        }

        /// <summary>
        /// IntermediateSumIndexes for waterfall series.
        /// </summary>
        [Parameter]
        public double[] IntermediateSumIndexes
        {
            get
            {
                return intermediateSumIndexed;
            }

            set
            {
                if (intermediateSumIndexed != value)
                {
                    intermediateSumIndexed = value;
                }
            }
        }

        /// <summary>
        /// Specifies the sumIndexes for waterfall series.
        /// </summary>
        [Parameter]
        public double[] SumIndexes
        {
            get
            {
                return sumIndexes;
            }

            set
            {
                if (sumIndexes != value)
                {
                    sumIndexes = value;
                }
            }
        }

        /// <summary>
        /// Specifies the summaryFillColor for waterfall series.
        /// </summary>
        [Parameter]
        public string SummaryFillColor
        {
            get
            {
                return summaryFillColor;
            }

            set
            {
                if (summaryFillColor != value)
                {
                    summaryFillColor = value;
                    if (Renderer != null)
                    {
                        refreshRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the NegativeFillColor for waterfall series.
        /// </summary>
        [Parameter]
        public string NegativeFillColor
        {
            get
            {
                return negativeFillColor;
            }

            set
            {
                if (negativeFillColor != value)
                {
                    negativeFillColor = value;
                    if (Renderer != null)
                    {
                        refreshRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the ShowNormalDistribution for Histogram series.
        /// </summary>
        [Parameter]
        public bool ShowNormalDistribution
        {
            get
            {
                return showNormalDistribution;
            }

            set
            {
                if (showNormalDistribution != value)
                {
                    showNormalDistribution = value;
                }
            }
        }

        /// <summary>
        /// Specifies the BinInterval for Histogram series.
        /// </summary>
        [Parameter]
        public double BinInterval
        {
            get
            {
                return binInterval;
            }

            set
            {
                if (binInterval != value)
                {
                    binInterval = value;
                }
            }
        }

        /// <summary>
        /// Specifies the  BoxPlotMode for box and whisker series.
        /// </summary>
        [Parameter]
        public BoxPlotMode BoxPlotMode
        {
            get
            {
                return boxPlotMode;
            }

            set
            {
                if (boxPlotMode != value)
                {
                    boxPlotMode = value;
                    if (Renderer != null)
                    {
                        refreshRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the Showmean for box and whisker series.
        /// </summary>
        [Parameter]
        public bool ShowMean
        {
            get
            {
                return showMean;
            }

            set
            {
                if (showMean != value)
                {
                    showMean = value;
                    if (Renderer != null)
                    {
                        refreshRange = true;
                    }
                }
            }
        }

        /// <summary>
        /// Specifies the trendlines for the series.
        /// </summary>
        [Parameter]
        public List<ChartTrendline> Trendlines
        {
            get
            {
                return trendlines;
            }

            set
            {
                if (trendlines != value)
                {
                    trendlines = value;
                }
            }
        }

        /// <summary>
        /// Specified the segments of the multicolor series.
        /// </summary>
        [Parameter]
        public List<ChartSegment> Segments
        {
            get
            {
                return segments;
            }

            set
            {
                if (segments != value)
                {
                    segments = value;
                }
            }
        }

        /// <summary>
        /// Specifies the configuration of the  animation settings for series.
        /// </summary>
        [Parameter]
        public ChartSeriesAnimation Animation
        {
            get
            {
                return animation;
            }

            set
            {
                if (animation != value)
                {
                    animation = value;
                }
            }
        }

        //// <summary>
        ///// Specifies the configuration of the error bar settings for the series
        ///// </summary>
        [Parameter]
        public ChartErrorBarSettings ErrorBar
        {
            get
            {
                return errorBar;
            }

            set
            {
                if (errorBar != value)
                {
                    errorBar = value;
                }
            }
        }

        /// <summary>
        /// Spectifies class name when the series is selected.
        /// </summary>
        [Parameter]
        public string SelectionStyle
        {
            get
            {
                return selectionStyle;
            }

            set
            {
                if (selectionStyle != value)
                {
                    selectionStyle = value;
                }
            }
        }

        /// <summary>
        /// Spectifies class name when the series is deselected.
        /// </summary>
        [Parameter]
        public string UnSelectedStyle
        {
            get
            {
                return un_SelectedStyle;
            }

            set
            {
                if (un_SelectedStyle != value)
                {
                    un_SelectedStyle = value;
                }
            }
        }

        /// <summary>
        /// Specfies the class name when the series is  non-highlighted.
        /// </summary>
        [Parameter]
        public string NonHighlightStyle
        {
            get
            {
                return nonHighlightStyle;
            }

            set
            {
                if (nonHighlightStyle != value)
                {
                    nonHighlightStyle = value;
                }
            }
        }

        /// <summary>
        /// Specfies the class name when the series is  highlighted.
        /// </summary>
        [Parameter]
        public string HighlightStyle
        {
            get
            {
                return highlightStyle;
            }

            set
            {
                if (highlightStyle != value)
                {
                    highlightStyle = value;
                }
            }
        }

        /// <summary>
        /// Configuration of drag settings for the series.
        /// </summary>
        [Parameter]
        public ChartDataEditSettings ChartDataEditSettings
        {
            get
            {
                return chartDataEditSettings;
            }

            set
            {
                if (chartDataEditSettings != value)
                {
                    chartDataEditSettings = value;
                    chartDataEditSettings.IsPropertyChanged = false;
                }
            }
        }
        #endregion

        [CascadingParameter]
        internal SfChart Container { get; set; }

        internal bool NeedRendererUpdate { get; set; }

        internal bool NeedRendererRemove { get; set; }

        internal bool UpdateDataSource { get; set; }

        public string RendererKey { get; set; } = SfBaseUtils.GenerateID("chartseries");

        public Type RendererType
        {
            get
            {
                return rendererType;
            }

            set
            {
                rendererType = value;
            }
        }

        internal IEnumerable<object> CurrentViewData { get; set; } = new List<object>();

        internal ChartSeriesRenderer Renderer
        {
            get
            {
                return renderer;
            }

            set
            {
                if (renderer != value)
                {
                    renderer = value;
                    renderer?.OnParentParameterSet();
                }
            }
        }

        private static bool IsCartesianToPolar(ChartSeriesType previousType, ChartSeriesType currentType)
        {
            bool previousPolar = previousType.ToString().Contains("Polar", StringComparison.InvariantCulture) || previousType.ToString().Contains("Radar", StringComparison.InvariantCulture);
            bool currentCartesian = !(currentType.ToString().Contains("Polar", StringComparison.InvariantCulture) || currentType.ToString().Contains("Radar", StringComparison.InvariantCulture));
            bool previousCartesian = !(previousType.ToString().Contains("Polar", StringComparison.InvariantCulture) || previousType.ToString().Contains("Radar", StringComparison.InvariantCulture));
            bool currentPolar = currentType.ToString().Contains("Polar", StringComparison.InvariantCulture) || currentType.ToString().Contains("Radar", StringComparison.InvariantCulture);
            return (previousPolar && currentCartesian) || (previousCartesian && currentPolar);
        }

        private static void UpdateSeries(ChartSeriesRenderer seriesRenderer)
        {
            if (seriesRenderer.Series.Visible)
            {
                seriesRenderer.UpdateDirection();
            }
            else
            {
                seriesRenderer.RendererShouldRender = true;
                if (seriesRenderer.Series.Marker.Renderer != null)
                {
                   seriesRenderer.Series.Marker.Renderer.RendererShouldRender = seriesRenderer.Series.Marker.Visible;
                }

                if (seriesRenderer.Series.Marker.DataLabel.Renderer != null)
                {
                   seriesRenderer.Series.Marker.DataLabel.Renderer.RendererShouldRender = seriesRenderer.Series.Marker.DataLabel.Visible;
                }
            }

            seriesRenderer.ProcessRenderQueue();
        }

        private bool IsUpdateDataSource()
        {
            return Container.IsChartFirstRender && Renderer != null && Renderer.IsSeriesRender && Renderer.XAxisRenderer != null && Renderer.YAxisRenderer != null;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Container.AddSeries(this);
            rendererType = ChartSeriesRenderer.GetRendererType(Type, DrawType);
            await UpdateSeriesData();
        }

        internal void UpdateSeriesProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Marker):
                    Marker = (ChartMarker)keyValue;
                    break;

                case nameof(Trendlines):
                    Trendlines = trendlines = (List<ChartTrendline>)keyValue;
                    break;
                case nameof(Segments):
                    Segments = segments = (List<ChartSegment>)keyValue;
                    break;
                case nameof(Border):
                    Border = (ChartSeriesBorder)keyValue;
                    break;
                case nameof(CornerRadius):
                    CornerRadius = (ChartCornerRadius)keyValue;
                    break;
                case nameof(Animation):
                    Animation = (ChartSeriesAnimation)keyValue;
                    break;
                case nameof(ChartDataEditSettings):
                    ChartDataEditSettings = (ChartDataEditSettings)keyValue;
                    break;
                case nameof(EmptyPointSettings):
                    EmptyPointSettings = emptyPointSettings = (ChartEmptyPointSettings)keyValue;
                    break;
                case nameof(Connector):
                    Connector = connector = (ChartSeriesConnector)keyValue;
                    break;

                case nameof(ErrorBar):
                    ErrorBar = errorBar = (ChartErrorBarSettings)keyValue;
                    break;
            }
        }

        internal async Task<IEnumerable<object>> UpdateSeriesData()
        {
            if (DataSource != null && DataManager == null)
            {
                SetDataManager<object>(DataSource);
            }
            else if (Container.DataSource != null)
            {
                SetDataManager<object>(Container.DataSource);
            }

            if (DataManager == null)
            {
                CurrentViewData = new List<object>();
                return CurrentViewData;
            }

            DataManager.DataAdaptor.SetRunSyncOnce(true);
            CurrentViewData = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(new Data.Query());
            return CurrentViewData;
        }

        protected override async Task OnParametersSetAsync()
        {
            shouldProcess = true;
            await base.OnParametersSetAsync();
            if (UpdateDataSource && NeedRendererUpdate)
            {
                NeedRendererUpdate = UpdateDataSource = false;
                Container.ProcessOnLayoutChange();
            }
            else if (UpdateDataSource && !NeedRendererRemove)
            {
                Renderer?.UpdateSeriesData();
                UpdateDataSource = false;
            }

            if (needLayoutUpdate)
            {
                needLayoutUpdate = false;
                Renderer.Container.Owner.DelayLayoutChange();
            }
            else if (refreshRange)
            {
                if (renderer.Series.Type == ChartSeriesType.BoxAndWhisker)
                {
                    Container.SvgRenderer.PathElementList.Clear();
                }
                Renderer.XAxisRenderer.ChangeAxisRange(refreshRange);
                Renderer.YAxisRenderer.ChangeAxisRange(refreshRange);
                refreshRange = false;
                UpdateSeriesCollection();
            }
            else if (refreshSeries)
            {
                refreshSeries = false;
                UpdateSeries(Renderer);
            }
        }

        internal void UpdateSeriesCollection()
        {
            List<ChartSeriesRenderer> seriesCollection = ChartSeriesRendererContainer.FindAxisToSeriesCollection(Renderer.XAxisRenderer, Renderer.YAxisRenderer);
            Container.SeriesContainer.DataLabelCollection.Clear();
            foreach (ChartSeriesRenderer seriesRenderer in seriesCollection)
            {
                UpdateSeries(seriesRenderer);
            }
        }

        private bool FindLayoutChange()
        {
            int x_Digits = Math.Max(Renderer.XMax.ToString(CultureInfo.InvariantCulture).Length, Renderer.XMin.ToString(CultureInfo.InvariantCulture).Length);
            int y_Digits = Math.Max(Renderer.YMax.ToString(CultureInfo.InvariantCulture).Length, Renderer.YMin.ToString(CultureInfo.InvariantCulture).Length);
            bool needLayoutUpdate = false;
            List<ChartSeriesRenderer> seriesCollection = ChartSeriesRendererContainer.FindAxisToSeriesCollection(Renderer.XAxisRenderer, Renderer.YAxisRenderer);
            foreach (ChartSeriesRenderer seriesRenderer in seriesCollection)
            {
                if (seriesRenderer != Renderer)
                {
                    int x_OtherDigits = Math.Max(seriesRenderer.XMax.ToString(CultureInfo.InvariantCulture).Length, seriesRenderer.XMin.ToString(CultureInfo.InvariantCulture).Length);
                    int y_OtherDigits = Math.Max(seriesRenderer.YMax.ToString(CultureInfo.InvariantCulture).Length, seriesRenderer.YMin.ToString(CultureInfo.InvariantCulture).Length);
                    if (Renderer.XAxisRenderer.Orientation == Orientation.Vertical)
                    {
                        needLayoutUpdate = x_Digits != x_OtherDigits ? x_Digits != x_OtherDigits : needLayoutUpdate;
                    }
                     else
                    {
                        needLayoutUpdate = y_Digits != y_OtherDigits ? y_Digits != y_OtherDigits : needLayoutUpdate;
                    }
                }
            }

            return needLayoutUpdate;
        }

        internal override void ComponentDispose()
        {
            if (dataSource is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)dataSource).CollectionChanged -= DataCollectionChanged;
            }

            Container?.SeriesContainer?.RemoveElement(this);
            animation.ComponentDispose();
            emptyPointSettings.ComponentDispose();
            errorBar.ComponentDispose();
            errorBarCap.ComponentDispose();
        }

        internal async void DataCollectionChanged(object source, NotifyCollectionChangedEventArgs e)
        {
            if (IsUpdateDataSource())
            {
                Renderer.IsSeriesRender = false;
                SetDataManager<object>((DataSource != null) ? DataSource : Container.DataSource);
                Container.SeriesContainer.AddToRenderQueue(Renderer);
                UpdateDataSource = true;
                await Renderer.UpdateSeriesData();
                UpdateDataSource = false;
            }
        }

        internal void SetParetoSeriesValues(IEnumerable<object> data)
        {
            Name = "Pareto";
            XName = "X";
            YName = "Y";
            DataSource = data;
            YAxisName = "SecondaryAxis";
            Type = ChartSeriesType.Line;
            Fill = Container.ChartThemeStyle.ErrorBar;
            Width = 2;
        }

        internal void SetTrendlineValues(string name, string xname, string yname, string dashArray, double width, string fill, LegendShape legendShape, bool tooltip, ChartSeriesBorder border, ChartSeriesConnector connector)
        {
            SetName(name);
            XName = xname;
            YName = yname;
            Fill = fill;
            Width = width;
            DashArray = dashArray;
            EnableTooltip = tooltip;
            LegendShape = legendShape;
            Border = border;
            Connector = connector;
        }

        internal void SetName(string name)
        {
            Name = name;
        }

        internal void SetTrendlineType(ChartSeriesType type)
        {
            Type = type;
        }

        internal void SetLegendShape(LegendShape shape)
        {
            LegendShape = shape;
        }

        internal void OnLegendClick(bool value)
        {
            if (renderer.IsRectSeries())
            {
                Container.SvgRenderer.PathElementList.Clear();
            }

            Visible = value;
            if (needLayoutUpdate)
            {
                needLayoutUpdate = false;
                Container.UpdateRenderers();
            }
            else if (refreshRange)
            {
                if (Renderer.IsCategoryAxis() && ((labelPreviousCount > 0 && labelCurrentCount == 0) || (labelPreviousCount == 0 && labelCurrentCount > 0)))
                {
                    refreshRange = false;
                    Container.UpdateRenderers();
                    return;
                }

                Renderer.XAxisRenderer.ChangeAxisRange(refreshRange);
                Renderer.YAxisRenderer.ChangeAxisRange(refreshRange);
                refreshRange = false;
                UpdateSeriesCollection();
                Container.AnnotationContainer.UpdateRenderers();
            }
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (!firstRender)
            {
                shouldProcess = false;
            }
        }
    }

    public class ChartSeriesCollection : ChartSubComponent
    {
        // TODO: ChartSeriesCollection
    }
}
