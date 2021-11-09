using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Stock Chart is a well-crafted, easy-to-use financial charting package to track and visualize the stock price of any
    /// company over a specific period using charting (such as Candlestick chart, OHLC, etc) and range tools.
    /// </summary>
    public partial class SfStockChart : SfDataBoundComponent
    {
        internal bool ShouldStockChartRender = true;
        internal Dictionary<string, bool> RenderedSelectors = new Dictionary<string, bool>();
        private string width;
        private string height;
        private IEnumerable<object> dataSource;

        internal List<StockChartAnnotation> Annotations { get; set; } = new List<StockChartAnnotation>();

        internal List<StockChartAxis> Axes { get; set; } = new List<StockChartAxis>();

        internal StockChartPrimaryXAxis PrimaryXAxis { get; set; } = new StockChartPrimaryXAxis();

        internal StockChartPrimaryYAxis PrimaryYAxis { get; set; } = new StockChartPrimaryYAxis();

        internal StockChartChartArea ChartArea { get; set; } = new StockChartChartArea();

        internal StockChartChartMargin Margin { get; set; } = new StockChartChartMargin();

        internal StockChartTitleStyle TitleStyle { get; set; } = new StockChartTitleStyle();

        internal StockChartChartBorder Border { get; set; } = new StockChartChartBorder();

        internal StockChartZoomSettings ZoomSettings { get; set; } = new StockChartZoomSettings();

        internal StockChartTooltipSettings Tooltip { get; set; } = new StockChartTooltipSettings();

        internal StockChartCrosshairSettings Crosshair { get; set; } = new StockChartCrosshairSettings();

        internal List<StockChartStockEvent> StockEvents { get; set; } = new List<StockChartStockEvent>();

        internal List<StockChartSeries> Series { get; set; } = new List<StockChartSeries>();

        internal List<StockChartPeriod> Periods { get; set; } = new List<StockChartPeriod>();

        internal List<StockChartSelectedDataIndex> SelectedDataIndexes { get; set; } = new List<StockChartSelectedDataIndex>();

        internal List<StockChartIndicator> Indicators { get; set; } = new List<StockChartIndicator>();

        internal List<StockChartRow> Rows { get; set; } = new List<StockChartRow>() { new StockChartRow() };

        internal StockChartEvents Events { get; set; } = new StockChartEvents();
        internal StockEventsRender StockEventsRender { get; set; }

        /// <summary>
        /// Gets and sets the identification of the component.
        /// </summary>
        [Parameter]
        public string ID { get; set; } = SfBaseUtils.GenerateID("stockchart");

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The background color of the stock chart that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Background { get; set; }

        /// <summary>
        /// Specifies the data source for the stock chart. It can be an array of JSON objects or an instance of DataManager.
        /// </summary>
        [Parameter]
        public IEnumerable<object> DataSource { get; set; }

        /// <summary>
        /// Defines the custom range.
        /// </summary>
        [Parameter]
        public bool EnableCustomRange { get; set; } = true;

        /// <summary>
        /// It specifies whether the period selector to be rendered in financial chart.
        /// </summary>
        [Parameter]
        public bool EnablePeriodSelector { get; set; } = true;

        /// <summary>
        /// specifies whether to enable persistence of the stock chart instance.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// specifies locale of the stock chart instance.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        public string Locale { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// It specifies whether the range navigator to be rendered in financial chart.
        /// </summary>
        [Parameter]
        public bool EnableSelector { get; set; } = true;

        /// <summary>
        /// It specifies the types of export options in financial chart.
        /// </summary>
        [Parameter]
        public List<ExportType> ExportType { get; set; } = new List<ExportType>() { Blazor.Charts.ExportType.PNG, Blazor.Charts.ExportType.JPEG, Blazor.Charts.ExportType.SVG, Blazor.Charts.ExportType.PDF };

        /// <summary>
        /// It specifies the types of indicators in financial chart.
        /// </summary>
        [Parameter]
        public List<TechnicalIndicators> IndicatorType { get; set; } = new List<TechnicalIndicators>() { TechnicalIndicators.Ema, TechnicalIndicators.Tma, TechnicalIndicators.Sma, TechnicalIndicators.Momentum, TechnicalIndicators.Atr, TechnicalIndicators.AccumulationDistribution, TechnicalIndicators.BollingerBands, TechnicalIndicators.Macd, TechnicalIndicators.Stochastic, TechnicalIndicators.Rsi };

        /// <summary>
        /// It specifies the types of series in financial chart.
        /// </summary>
        [Parameter]
        public List<ChartSeriesType> SeriesType { get; set; } = new List<ChartSeriesType> { ChartSeriesType.Line, ChartSeriesType.Hilo, ChartSeriesType.HiloOpenClose, ChartSeriesType.Candle, ChartSeriesType.Spline };

        /// <summary>
        /// It specifies the types of trendline types in financial chart.
        /// </summary>
        [Parameter]
        public List<TrendlineTypes> TrendlineType { get; set; } = new List<TrendlineTypes>() { TrendlineTypes.Linear, TrendlineTypes.Exponential, TrendlineTypes.Polynomial, TrendlineTypes.Logarithmic, TrendlineTypes.MovingAverage };

        /// <summary>
        /// The height of the stock chart as a string accepts input both as '100px' or '100%'.
        /// If specified as '100%, stockChart renders to the full height of its parent element.
        /// </summary>
        [Parameter]
        public string Height { get; set; }

        /// <summary>
        /// If set true, enables the multi-selection in chart. It requires `SelectionMode` set to be `Point` | `Series` | or `Cluster`.
        /// </summary>
        [Parameter]
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// If set true, enables the animation in chart.
        /// </summary>
        [Parameter]
        public bool IsSelect { get; set; }

        /// <summary>
        /// It specifies whether the stock chart should be render in transposed manner or not.
        /// </summary>
        [Parameter]
        public bool IsTransposed { get; set; }

        /// <summary>
        /// Specifies whether series or data point has to be selected. They are,
        ///  None: Disables the selection.
        ///  Series: Selects a series.
        ///  Point: Selects a point.
        ///  Cluster: Selects a cluster of point.
        ///  DragXY: Selects points by dragging with respect to both horizontal and vertical axes.
        ///  DragX: Selects points by dragging with respect to horizontal axis.
        ///  DragY: Selects points by dragging with respect to vertical axis.
        /// </summary>
        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.None;

        /// <summary>
        /// Specifies the theme for the stock chart.
        /// </summary>
        [Parameter]
        public Theme Theme { get; set; } = Theme.Bootstrap4;

        /// <summary>
        /// Title of the stock chart.
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The width of the stock chart as a string accepts input as both like '100px' or '100%'. If specified as '100%, stock chart renders to the full width of its parent element.
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        protected override bool ShouldRender()
        {
            return ShouldStockChartRender;
        }

        internal bool ShouldPeriodSelectorRender(string selectorId)
        {
            bool shouldSelectorsRender;
            if (!RenderedSelectors.TryGetValue(selectorId, out shouldSelectorsRender))
            {
                RenderedSelectors.TryAdd(selectorId, true);
            }

            return periodSelectorResize || !shouldSelectorsRender;
        }

        internal bool ShouldRangeSelectorRender(string selectorId)
        {
            bool shouldSelectorsRender;
            if (!RenderedSelectors.TryGetValue(selectorId, out shouldSelectorsRender))
            {
                RenderedSelectors.TryAdd(selectorId, true);
            }

            return rangeSelectorResize || !shouldSelectorsRender;
        }
    }
}