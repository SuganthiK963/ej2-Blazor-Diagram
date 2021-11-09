using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart indicator.
    /// </summary>
    public partial class StockChartIndicator : SfBaseComponent
    {
        internal string RendererKey;
        internal StockChartIndicatorAnimation Animation { get; set; } = new StockChartIndicatorAnimation();

        internal StockChartLowerLine LowerLine { get; set; } = new StockChartLowerLine();

        internal StockChartMacdLine MacdLine { get; set; } = new StockChartMacdLine();

        internal StockChartPeriodLine PeriodLine { get; set; } = new StockChartPeriodLine();

        internal StockChartUpperLine UpperLine { get; set; } = new StockChartUpperLine();

        [CascadingParameter]
        internal StockChartIndicators Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Options for customizing the BollingerBand in the indicator.
        /// </summary>
        [Parameter]
        public string BandColor { get; set; } = "rgba(211,211,211,0.25)";

        /// <summary>
        /// The DataSource field that contains the close value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string Close { get; set; } = "close";

        /// <summary>
        /// Defines the period, the price changes over which will define the %D value in stochastic indicators.
        /// </summary>
        [Parameter]
        public double DPeriod { get; set; } = 3;

        /// <summary>
        /// Defines the pattern of dashes and gaps to stroke the lines in `Line` type series.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = "0";

        /// <summary>
        /// Specifies the DataSource for the series. It can be an array of JSON objects or an instance of DataManager.
        /// </summary>
        [Parameter]
        public object DataSource { get; set; }

        /// <summary>
        /// Sets the fast period to define the Macd line.
        /// </summary>
        [Parameter]
        public double FastPeriod { get; set; } = 26;

        /// <summary>
        /// Defines the field to compare the current value with previous values.
        /// </summary>
        [Parameter]
        public FinancialDataFields Field { get; set; } = FinancialDataFields.Close;

        /// <summary>
        /// The fill color for the series that accepts value in hex and rgba as a valid CSS color string.
        /// It also represents the color of the signal lines in technical indicators.
        /// For technical indicators, the default value is 'blue' and for series, it has null.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// The DataSource field that contains the high value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string High { get; set; } = "high";

        /// <summary>
        /// Defines the look back period, the price changes over which will define the %K value in stochastic indicators.
        /// </summary>
        [Parameter]
        public double KPeriod { get; set; } = 14;

        /// <summary>
        /// The DataSource field that contains the low value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string Low { get; set; } = "low";

        /// <summary>
        /// Defines the color of the negative bars in Macd indicators.
        /// </summary>
        [Parameter]
        public string MacdNegativeColor { get; set; } = "#e74c3d";

        /// <summary>
        /// Defines the color of the positive bars in Macd indicators.
        /// </summary>
        [Parameter]
        public string MacdPositiveColor { get; set; } = "#2ecd71";

        /// <summary>
        /// Defines the type of the Macd indicator.
        /// </summary>
        [Parameter]
        public MacdType MacdType { get; set; } = MacdType.Both;

        /// <summary>
        /// The DataSource field that contains the open value of y
        /// It is applicable for series and technical indicators.
        /// </summary>
        [Parameter]
        public string Open { get; set; } = "open";

        /// <summary>
        /// Defines the over-bought(threshold) values. It is applicable for RSI and stochastic indicators.
        /// </summary>
        [Parameter]
        public double OverBought { get; set; } = 80;

        /// <summary>
        /// Defines the over-sold(threshold) values. It is applicable for RSI and stochastic indicators.
        /// </summary>
        [Parameter]
        public double OverSold { get; set; } = 20;

        /// <summary>
        /// Defines the period, the price changes over which will be considered to predict the trend.
        /// </summary>
        [Parameter]
        public double Period { get; set; } = 14;

        /// <summary>
        /// The DataSource field that contains the color value of point.
        /// It is applicable for series.
        /// </summary>
        [Parameter]
        public string PointColorMapping { get; set; } = string.Empty;

        /// <summary>
        /// Specifies query to select data from DataSource. This property is applicable only when the DataSource is `Ej.DataManager`.
        /// </summary>
        [Parameter]
        public string Query { get; set; }

        /// <summary>
        /// Defines the name of the series, the data of which has to be depicted as indicator.
        /// </summary>
        [Parameter]
        public string SeriesName { get; set; } = string.Empty;

        /// <summary>
        /// Enables/Disables the over-bought and over-sold regions.
        /// </summary>
        [Parameter]
        public bool ShowZones { get; set; } = true;

        /// <summary>
        /// Sets the slow period to define the Macd line.
        /// </summary>
        [Parameter]
        public double SlowPeriod { get; set; } = 12;

        /// <summary>
        /// Sets the standard deviation values that helps to define the upper and lower bollinger bands.
        /// </summary>
        [Parameter]
        public double StandardDeviation { get; set; } = 2;

        /// <summary>
        /// Defines the type of the technical indicator.
        /// </summary>
        [Parameter]
        public TechnicalIndicators Type { get; set; } = TechnicalIndicators.Sma;

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
        public string XAxisName { get; set; }

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
        public string YAxisName { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Indicators.Add(this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Animation = null;
            LowerLine = null;
            MacdLine = null;
            PeriodLine = null;
            UpperLine = null;
        }
    }
}