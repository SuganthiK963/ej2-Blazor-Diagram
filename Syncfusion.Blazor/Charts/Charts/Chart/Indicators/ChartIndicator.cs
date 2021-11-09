using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    ///  Defines the technical indicator, that are used in financial markets.
    /// </summary>
    public class ChartIndicator : ChartSubComponent, IChartElement
    {
        [CascadingParameter]
        private ChartIndicators Parent { get; set; }

        /// <summary>
        /// Defines the type of the technical indicator.
        /// </summary>
        [Parameter]
        public TechnicalIndicators Type { get; set; } = TechnicalIndicators.Sma;

        /// <summary>
        /// Defines the period, the price changes over which will be considered to predict the trend.
        /// </summary>
        [Parameter]
        public double Period { get; set; } = 14;

        /// <summary>
        /// Defines the look back period, the price changes over which will define the %K value in stochastic indicators.
        /// </summary>
        [Parameter]
        public double KPeriod { get; set; } = 14;

        /// <summary>
        /// Defines the period, the price changes over which will define the %D value in stochastic indicators.
        /// </summary>
        [Parameter]
        public double DPeriod { get; set; } = 3;

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
        /// Sets the standard deviation values that helps to define the upper and lower bollinger bands.
        /// </summary>
        [Parameter]
        public double StandardDeviation { get; set; } = 2;

        /// <summary>
        /// Defines the field to compare the current value with previous values.
        /// </summary>
        [Parameter]
        public FinancialDataFields Field { get; set; } = FinancialDataFields.Close;

        /// <summary>
        /// Defines the field to compare the current value with previous values.
        /// </summary>
        [Parameter]
        public double SlowPeriod { get; set; } = 12;

        /// <summary>
        /// Sets the fast period to define the Macd line.
        /// </summary>
        [Parameter]
        public double FastPeriod { get; set; } = 26;

        /// <summary>
        /// Enables/Disables the over-bought and over-sold regions.
        /// </summary>
        [Parameter]
        public bool ShowZones { get; set; } = true;

        /// <summary>
        /// Defines the type of the Macd indicator.
        /// </summary>
        [Parameter]
        public MacdType MacdType { get; set; } = MacdType.Both;

        /// <summary>
        /// Defines the color of the positive bars in Macd indicators.
        /// </summary>
        [Parameter]
        public string MacdPositiveColor { get; set; } = "#2ecd71";

        /// <summary>
        /// Defines the color of the negative bars in Macd indicators.
        /// </summary>
        [Parameter]
        public string MacdNegativeColor { get; set; } = "#e74c3d";

        /// <summary>
        /// Options for customizing the BollingerBand in the indicator.
        /// </summary>
        [Parameter]
        public string BandColor { get; set; } = "rgba(211,211,211,0.25)";

        /// <summary>
        /// Defines the appearance of the the MacdLine of Macd indicator.
        /// </summary>
        [Parameter]
        public ChartIndicatorMacdLine MacdLine { get; set; } = new ChartIndicatorMacdLine();

        /// <summary>
        /// Defines the appearance of the upper line in technical indicators.
        /// </summary>
        [Parameter]
        public ChartIndicatorUpperLine UpperLine { get; set; } = new ChartIndicatorUpperLine();

        /// <summary>
        /// Defines the appearance of lower line in technical indicators.
        /// </summary>
        [Parameter]
        public ChartIndicatorLowerLine LowerLine { get; set; } = new ChartIndicatorLowerLine();

        /// <summary>
        /// Defines the appearance of Period Line in technical indicators.
        /// </summary>
        [Parameter]
        public ChartIndicatorPeriodLine PeriodLine { get; set; } = new ChartIndicatorPeriodLine();

        /// <summary>
        /// Defines the name of the series, the data of which has to be depicted as indicator.
        /// </summary>
        [Parameter]
        public string SeriesName { get; set; } = string.Empty;

        /// <summary>
        /// Animation settings for indicator.
        /// </summary>
        [Parameter]
        public ChartIndicatorAnimation Animation { get; set; } = new ChartIndicatorAnimation();

        /// <summary>
        /// SPecifies the fill color of the indicator.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Specifies the close value of the indicator.
        /// </summary>
        [Parameter]
        public string Close { get; set; } = string.Empty;

        /// <summary>
        /// Specifies the dashArray of the indicator.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = "0";

        /// <summary>
        /// Defines the datasource for the indicator.
        /// </summary>
        [Parameter]
        public object DataSource { get; set; }

        /// <summary>
        /// Enables the complex property data binding for the indicator.
        /// </summary>
        [Parameter]
        public bool EnableComplexProperty { get; set; }

        /// <summary>
        /// Defines the high value of the indicator.
        /// </summary>
        [Parameter]
        public string High { get; set; } = string.Empty;

        /// <summary>
        /// Defines the low value of the indicator.
        /// </summary>
        [Parameter]
        public string Low { get; set; } = string.Empty;

        /// <summary>
        /// Defines the open value of the indicator.
        /// </summary>
        [Parameter]
        public string Open { get; set; } = string.Empty;

        /// <summary>
        /// Enables the visiblity of the indicator.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Defines the volume value of the indicator.
        /// </summary>
        [Parameter]
        public string Volume { get; set; } = string.Empty;

        /// <summary>
        /// Defines the width of the indicator.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = 1;

        /// <summary>
        /// Defines the corresponding horizontal axis in which indicator to be plotted.
        /// </summary>
        [Parameter]
        public string XAxisName { get; set; }

        /// <summary>
        /// Defines the xValue of the indicator.
        /// </summary>
        [Parameter]
        public string XName { get; set; } = string.Empty;

        /// <summary>
        ///  Defines the corresponding vertical axis in which indicator to be plotted.
        /// </summary>
        [Parameter]
        public string YAxisName { get; set; }

        public string RendererKey { get; set; } = SfBaseUtils.GenerateID("chartindicator");

        public Type RendererType { get; set; }

        internal List<ChartSeries> TargetSeries { get; set; } = new List<ChartSeries>();

        internal IndicatorBase IndicatorRenderer { get; set; }

        internal Rect ClipRect { get; set; } = new Rect();

        private static IndicatorBase GetIndicatorType(TechnicalIndicators type)
        {
            switch (type)
            {
                case TechnicalIndicators.AccumulationDistribution:
                    return new AccumulationDistributionIndicatorRenderer();
                case TechnicalIndicators.Atr:
                    return new AtrIndicatorRenderer();
                case TechnicalIndicators.BollingerBands:
                    return new BollingerBandsIndicatorRenderer();
                case TechnicalIndicators.Ema:
                    return new EmaIndicatorRenderer();
                case TechnicalIndicators.Macd:
                    return new MacdIndicatorRenderer();
                case TechnicalIndicators.Momentum:
                    return new MomentumIndicatorRenderer();
                case TechnicalIndicators.Rsi:
                    return new RSIIndicatorRenderer();
                case TechnicalIndicators.Sma:
                    return new SmaIndicatorRenderer();
                case TechnicalIndicators.Stochastic:
                    return new StochasticIndicatorRenderer();
                case TechnicalIndicators.Tma:
                    return new TmaIndicatorRenderer();
            }

            return null;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartIndicators)Tracker;
            IndicatorRenderer = GetIndicatorType(Type);
            IndicatorRenderer.Indicator = this;
            IndicatorRenderer.Chart = Parent.Chart;
            IndicatorRenderer.InitSeriesCollection();
            Parent.Chart.AddIndicator(this);
        }

        internal void UpdateIndicatorProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Animation):
                    Animation = (ChartIndicatorAnimation)keyValue;
                    break;
                case nameof(UpperLine):
                    UpperLine = (ChartIndicatorUpperLine)keyValue;
                    break;
                case nameof(LowerLine):
                    LowerLine = (ChartIndicatorLowerLine)keyValue;
                    break;
                case nameof(MacdLine):
                    MacdLine = (ChartIndicatorMacdLine)keyValue;
                    break;
                case nameof(PeriodLine):
                    PeriodLine = (ChartIndicatorPeriodLine)keyValue;
                    break;
            }
        }

        internal override void ComponentDispose()
        {
            Parent?.Chart?.RemoveIndicator(this);
            Parent = null;
            ChildContent = null;
            MacdLine = null;
            LowerLine = null;
            PeriodLine = null;
            UpperLine = null;
            Animation = null;
        }
    }
}