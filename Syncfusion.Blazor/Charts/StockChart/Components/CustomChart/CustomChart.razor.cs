using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Models;
using System.Collections.Generic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    /// <summary>
    /// To specify customization options for stockchart internal chart component.
    /// </summary>
    public partial class CustomChart
    {
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Specifies the instance of the component.
        /// </summary>
        [Parameter]
        public SfStockChart StockChart { get; set; }

        /// <summary>
        /// Specifies the characters size.
        /// </summary>
        [Parameter]
        public Size ChartSize { get; set; }

        /// <summary>
        /// Specifies the series of stock chart.
        /// </summary>
        [Parameter]
        public List<StockChartSeries> Series { get; set; }

        /// <summary>
        /// Specifies the area of stock chart.
        /// </summary>
        [Parameter]
        public StockChartChartArea ChartArea { get; set; }

        /// <summary>
        /// Specifies the tooltip of stock chart.
        /// </summary>
        [Parameter]
        public StockChartTooltipSettings Tooltip { get; set; }

        /// <summary>
        /// Specifies the cross hair of stock chart.
        /// </summary>
        [Parameter]
        public StockChartCrosshairSettings Crosshair { get; set; }

        /// <summary>
        /// Specifies the X axis.
        /// </summary>
        [Parameter]
        public StockChartPrimaryXAxis PrimaryXAxis { get; set; }

        /// <summary>
        /// Specifies the Y axis.
        /// </summary>
        [Parameter]
        public StockChartPrimaryYAxis PrimaryYAxis { get; set; }

        /// <summary>
        /// Specifies the zoom settings.
        /// </summary>
        [Parameter]
        public StockChartZoomSettings ZoomSettings { get; set; }

        private static bool GetTooltipVisibility(StockChartSeries series)
        {
            return !(series.Type != ChartSeriesType.HiloOpenClose && series.Type != ChartSeriesType.Candle && series.YName.ToUpper(CultureInfo.InvariantCulture) == "VOLUME");
        }

        private string GetTooltipHeader()
        {
            if (Tooltip.HeaderContent == null)
            {
                Tooltip.HeaderContent = "<b>${point.x}</b>";
            }

            return Tooltip.HeaderContent;
        }

        private string GetTooltipFormat()
        {
            if (Tooltip.FormatContent == null)
            {
                Tooltip.FormatContent = "High : <b>${point.high}</b><br/>Low :" +
                    " <b>${point.low}</b><br/>Open : <b>${point.open}</b><br/>Close : <b>${point.close}</b>";
                if (Series.Count > 0 && !string.IsNullOrEmpty(Series[0].Volume))
                {
                    Tooltip.FormatContent += "<br/>Volume : <b>${point.volume}</b>";
                }
            }

            return Tooltip.FormatContent;
        }

        private IntervalType GetXAxisIntervalType()
        {
            if (StockChart.CurrentInterval != RangeIntervalType.Auto && PrimaryXAxis.IntervalType == IntervalType.Auto)
            {
                return StockChart.CurrentInterval == RangeIntervalType.Weeks ? IntervalType.Days : (IntervalType)StockChart.CurrentInterval;
            }
            else
            {
                return PrimaryXAxis.IntervalType;
            }
        }

        private ChartSeriesType GetSeriesType(ChartSeriesType seriesType)
        {
            if (StockChart.SeriesTypeList.Count > 0)
            {
                return StockChart.SeriesTypeList[0];
            }
            else
            {
                return seriesType;
            }
        }
    }
}