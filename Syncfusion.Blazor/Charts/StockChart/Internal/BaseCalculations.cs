using System;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfStockChart : SfDataBoundComponent
    {
        private const string TITLEID = "_stockChart_Title";
        private const double DEFAULTWIDTH = 600;
        private const double DEFAULTHEIGHT = 450;
        private SfChart chartSettings;
        internal const string CHARTID = "_stockChart_chart";
        internal const double TOOLBARHEIGHT = 43;
        internal const double SELECTORHEIGHT = 80;
        internal const double TITLEPADDING = 15;
        internal const string RANGESELECTORID = "_stockChart_rangeSelector";
        internal const string SELECTORID = "_selector";

        internal SfChart ChartSettings
        {
            get
            {
                return chartSettings;
            }

            set
            {
                if (value != null)
                {
                    chartSettings = value;
                    UpdateSecondaryElements();
                }
            }
        }

        internal DomRect ElementOffset { get; set; }

        internal Size AvailableSize { get; set; } = new Size(0, 0);

        internal ChartHelper Helper { get; set; }

        internal Size ChartSize { get; set; }

        internal Size TitleSize { get; set; }

        internal bool TrendlineTriggered { get; set; } = true;

        internal RenderFragment TitleContent { get; set; }

        internal RenderFragment StockEventTooltipContent { get; set; }

        internal SvgRendering SvgRenderer { get; set; }

        internal SfRangeNavigator PeriodSelectorSettings { get; set; }

        internal PeriodSelector PeriodSelector { get; set; }

        internal SfRangeNavigator RangeSelectorSettings { get; set; }

        internal RenderFragment TooltipContent { get; set; }

        internal RenderFragment PeriodSelectorContent { get; set; }

        internal RenderFragment RangeTooltipContent { get; set; }

        internal RenderFragment AnnotationContent { get; set; }

        internal object SelectedValue { get; set; }

        internal StockEvents StockEventInstance { get; set; }

        internal ElementReference Element { get; set; }

        private static string GetToolbarHeight()
        {
            return TOOLBARHEIGHT.ToString(CultureInfo.InvariantCulture) + "px;";
        }

        internal static string GetYName(StockChartSeries series)
        {
            return series.Close != series.YName && series.YName == "close" ? series.Close : series.YName;
        }

        private static List<StockChartPeriod> FindRange(double min, double max)
        {
            List<StockChartPeriod> defaultPeriods = new List<StockChartPeriod>();
            if (((max - min) / 3.154e+10) >= 1)
            {
#pragma warning disable BL0005
                defaultPeriods.Add(new StockChartPeriod() { Text = "1M", Interval = 1, IntervalType = RangeIntervalType.Months });
                defaultPeriods.Add(new StockChartPeriod() { Text = "3M", Interval = 3, IntervalType = RangeIntervalType.Months });
                defaultPeriods.Add(new StockChartPeriod() { Text = "6M", Interval = 6, IntervalType = RangeIntervalType.Months });
                defaultPeriods.Add(new StockChartPeriod() { Text = "1Y", Interval = 1, IntervalType = RangeIntervalType.Years });
            }
            else if ((max - min) / 1.577e+10 >= 1)
            {
                defaultPeriods.Add(new StockChartPeriod() { Text = "1M", Interval = 1, IntervalType = RangeIntervalType.Months });
                defaultPeriods.Add(new StockChartPeriod() { Text = "3M", Interval = 3, IntervalType = RangeIntervalType.Months });
                defaultPeriods.Add(new StockChartPeriod() { Text = "6M", Interval = 6, IntervalType = RangeIntervalType.Months });
            }
            else if ((max - min) / 2.628e+9 >= 1)
            {
                defaultPeriods.Add(new StockChartPeriod() { Text = "1D", Interval = 1, IntervalType = RangeIntervalType.Days });
                defaultPeriods.Add(new StockChartPeriod() { Text = "3W", Interval = 3, IntervalType = RangeIntervalType.Weeks });
                defaultPeriods.Add(new StockChartPeriod() { Text = "1M", Interval = 1, IntervalType = RangeIntervalType.Months });
            }
            else if ((max - min) / 8.64e+7 >= 1)
            {
                defaultPeriods.Add(new StockChartPeriod() { Text = "1H", Interval = 1, IntervalType = RangeIntervalType.Hours });
                defaultPeriods.Add(new StockChartPeriod() { Text = "12H", Interval = 12, IntervalType = RangeIntervalType.Hours });
                defaultPeriods.Add(new StockChartPeriod() { Text = "1D", Interval = 1, IntervalType = RangeIntervalType.Days });
            }

            return defaultPeriods;
        }

        internal List<StockChartPeriod> CalculateAutoPeriods()
        {
            List<StockChartPeriod> defaultPeriods = FindRange(SeriesXMin, SeriesXMax);
            defaultPeriods.Add(new StockChartPeriod() { Text = "YTD", Selected = true });
            defaultPeriods.Add(new StockChartPeriod() { Text = "All" });
#pragma warning restore BL0005
            return defaultPeriods;
        }

        private void SetContainerSize()
        {
            CalculateAvailableSize();
            CalculateTitleSize();
            CalculateChartSize();
        }

        private void CalculateTitleSize()
        {
            TitleSize = new Size(0, 0);
            if (!string.IsNullOrEmpty(Title))
            {
                TitleSize = ChartHelper.MeasureText(Title, TitleStyle.GetCommonFont());
                TitleSize.Height += TITLEPADDING;
            }
        }

        private void CalculateChartSize()
        {
            double height = (EnablePeriodSelector ? TOOLBARHEIGHT : 0) + TitleSize.Height;
            height = EnableSelector ? height + SELECTORHEIGHT : height;
            ChartSize = new Size(AvailableSize.Width, AvailableSize.Height - height);
        }

        private void InitPrivateVariables()
        {
            Helper = new ChartHelper();
            SvgRenderer = new SvgRendering();
        }

        internal string GetId(string componentId)
        {
            return ID + componentId;
        }

        private string GetSvgHeight()
        {
            return Convert.ToString(AvailableSize.Height - ((EnablePeriodSelector ? TOOLBARHEIGHT : 0) + TitleSize.Height), Culture);
        }

        private string GetTooltipPosition()
        {
            return (ChartSize.Height + ((EnablePeriodSelector ? TOOLBARHEIGHT : 0) + TitleSize.Height)) + "px;";
        }

        internal double GetSeriesTooltipTop()
        {
            return (EnablePeriodSelector ? TOOLBARHEIGHT : 0) + TitleSize.Height;
        }

        private void CalculateAvailableSize()
        {
            double containerWidth = ElementOffset.Width,
            containerHeight = ElementOffset.Height,
            height = DataVizCommonHelper.StringToNumber(Height, containerHeight),
            width = DataVizCommonHelper.StringToNumber(Width, containerWidth);
            width = double.IsNaN(width) ? containerWidth : width;
            width = ChartHelper.IsNaNOrZero(width) ? DEFAULTWIDTH : width;
            height = double.IsNaN(height) ? containerHeight : height;
            height = ChartHelper.IsNaNOrZero(height) ? DEFAULTHEIGHT : height;
            AvailableSize = new Size(width, height);
        }

        internal void CalculateStockEvents(RenderTreeBuilder builder)
        {
            if (StockEvents.Count == 0)
            {
                return;
            }

            StockEventInstance = new StockEvents(this);
            StockEventInstance.RenderStockEvents(builder);
        }

        private void UpdateSecondaryElements()
        {
            periodSelectorResize = rangeSelectorResize = ShouldStockChartRender = true;
            StateHasChanged();
            ShouldStockChartRender = false;
        }
    }
}