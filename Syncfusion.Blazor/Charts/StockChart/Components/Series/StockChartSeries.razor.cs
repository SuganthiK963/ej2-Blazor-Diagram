using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart series.
    /// </summary>
    public partial class StockChartSeries : SfDataBoundComponent
    {
        private string dataSourceType;
        private ChartSeriesType type = ChartSeriesType.Candle;

        internal StockChartCornerRadius CornerRadius { get; set; } = new StockChartCornerRadius();

        internal StockChartEmptyPointSettings EmptyPointSettings { get; set; } = new StockChartEmptyPointSettings();

        internal StockChartSeriesAnimation Animation { get; set; } = new StockChartSeriesAnimation();

        internal StockChartSeriesMarker Marker { get; set; } = new StockChartSeriesMarker();

        internal StockChartSeriesBorder Border { get; set; } = new StockChartSeriesBorder();

        internal List<StockChartTrendline> Trendlines { get; set; } = new List<StockChartTrendline>();

        [CascadingParameter]
        internal StockChartSeriesCollection Parent { get; set; }

        [CascadingParameter]
        internal SfStockChart StockChartInstance { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal IEnumerable<object> CurrentViewData { get; set; }

        internal IEnumerable<object> FilteredData { get; set; }

        /// <summary>
        /// This property is used in stock charts to visualize the price movements in stock.
        /// It defines the color of the candle/point, when the opening price is less than the closing price.
        /// </summary>
        [Parameter]
        public string BearFillColor { get; set; } = "#2ecd71";

        /// <summary>
        /// This property is used in financial charts to visualize the price movements in stock.
        /// It defines the color of the candle/point, when the opening price is higher than the closing price.
        /// </summary>
        [Parameter]
        public string BullFillColor { get; set; } = "#e74c3d";

        /// <summary>
        /// It defines tension of cardinal spline types.
        /// </summary>
        [Parameter]
        public double CardinalSplineTension { get; set; } = 0.5;

        /// <summary>
        /// The DataSource field that contains the close value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string Close { get; set; } = "close";

        /// <summary>
        /// To render the column series points with particular column spacing. It takes value from 0 - 1.
        /// </summary>
        [Parameter]
        public double ColumnSpacing { get; set; }

        /// <summary>
        /// To render the column series points with particular column width. If the series type is histogram the
        /// default value is 1 otherwise 0.7.
        /// </summary>
        [Parameter]
        public double ColumnWidth { get; set; }

        /// <summary>
        /// Defines the pattern of dashes and gaps to stroke the lines in `Line` type series.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = "0";

        /// <summary>
        /// Specifies the DataSource for the series. It can be an array of JSON objects or an instance of DataManager.
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource { get; set; }

        /// <summary>
        /// This property is applicable for candle series.
        /// It enables/disables to visually compare the current values with the previous values in stock.
        /// </summary>
        [Parameter]
        public bool EnableSolidCandles { get; set; }

        /// <summary>
        /// If set true, the Tooltip for series will be visible.
        /// </summary>
        [Parameter]
        public bool EnableTooltip { get; set; } = true;

        /// <summary>
        /// The fill color for the series that accepts value in hex and rgba as a valid CSS color string.
        /// It also represents the color of the signal lines in technical indicators.
        /// For technical indicators, the default value is 'blue' and for series, it has null.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// The DataSource field that contains the high value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string High { get; set; } = "high";

        /// <summary>
        /// The DataSource field that contains the low value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string Low { get; set; } = "low";

        /// <summary>
        /// The name of the series visible in legend.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The opacity of the series.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// The DataSource field that contains the open value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string Open { get; set; } = "open";

        /// <summary>
        /// The DataSource field that contains the color value of point
        /// It is applicable for series.
        /// </summary>
        [Parameter]
        public string PointColorMapping { get; set; } = string.Empty;

        /// <summary>
        /// Specifies query to select data from DataSource. This property is applicable only when the DataSource is `Ej.DataManager`.
        /// </summary>
        [Parameter]
        public Query Query { get; set; }

        /// <summary>
        /// Custom style for the selected series or points.
        /// </summary>
        [Parameter]
        public string SelectionStyle { get; set; }

        /// <summary>
        /// The provided value will be considered as a Tooltip name.
        /// </summary>
        [Parameter]
        public string TooltipMappingName { get; set; } = string.Empty;

        /// <summary>
        /// The type of the series are
        /// Line
        /// Column
        /// Area
        /// Spline
        /// Hilo
        /// HiloOpenClose
        /// Candle.
        /// </summary>
        [Parameter]
        public ChartSeriesType Type
        {
            get {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Specifies the visibility of series.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Defines the data source field that contains the volume value in candle charts
        /// It is applicable for financial series and technical indicators.
        /// </summary>
        [Parameter]
        public string Volume { get; set; } = "volume";

        /// <summary>
        /// The stroke width for the series that is applicable only for `Line` type series.
        /// It also represents the stroke width of the signal lines in technical indicators.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// The name of the horizontal axis associated with the series. It requires `Axes` of the chart.
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string XAxisName { get; set; } = Constants.PRIMARYXAXIS;

        /// <summary>
        /// The DataSource field that contains the x value.
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string XName { get; set; } = "date";

        /// <summary>
        /// The name of the vertical axis associated with the series. It requires `Axes` of the chart.
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string YAxisName { get; set; } = Constants.PRIMARYYAXIS;

        /// <summary>
        /// The DataSource field that contains the y value.
        /// </summary>
        [Parameter]
        public string YName { get; set; } = "close";

        /// <summary>
        /// The provided format will be considered as a Tooltip format.
        /// </summary>
        [Parameter]
        public string TooltipFormat { get; set; } = string.Empty;

        internal async Task<IEnumerable<object>> UpdatedSeriesData()
        {
            if (DataSource != null && DataManager == null)
            {
                SetDataManager<object>(DataSource);
                dataSourceType = DataSource.GetType().Name;
            }
            else if (StockChartInstance != null && StockChartInstance.DataManager == null)
            {
                SetDataManager<object>(StockChartInstance.DataSource);
                dataSourceType = StockChartInstance.DataSource?.GetType().Name;
            }
            else if (StockChartInstance != null)
            {
                DataManager = StockChartInstance.DataManager;
                dataSourceType = StockChartInstance.DataSource?.GetType().Name;
            }

            DataManager.DataAdaptor.SetRunSyncOnce(true);
            IEnumerable<object> data = (IEnumerable<object>)await DataManager.ExecuteQuery<object>(Query ?? new Query());
            return data;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            CurrentViewData = await UpdatedSeriesData();
            FilteredData = FilteredData == null ? CurrentViewData : FilteredData;
            Parent?.Series?.Add(this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                PropertyChanges.Clear();
                StockChartInstance.OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            if (Parent != null && Parent.Series != null)
            {
                int index = Parent.Series.FindIndex(x => x == this);
                Parent.Series.Remove(this);
                if (index > -1)
                {
                    StockChartInstance.ChartSettings?.RemoveSeries(index);
                    StockChartInstance.RangeSelectorSettings?.Series?.RemoveAt(index);
                }
                StockChartInstance.RangeSelectorSettings?.ResizeChart();
            }

            StockChartInstance = null;
            Parent = null;
            ChildContent = null;
            CurrentViewData = null;
            FilteredData = null;
            Animation = null;
            Border = null;
            CornerRadius = null;
            DataSource = null;
            EmptyPointSettings = null;
            Marker = null;
            Trendlines = null;
        }
    }
}