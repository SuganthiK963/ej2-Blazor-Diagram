using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Navigations;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System.Globalization;
using Syncfusion.Blazor.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the period selector which allows to select a range with specified periods.
    /// </summary>
    public class PeriodSelector
    {
        private PeriodSelectorControl control;
        private SfRangeNavigator chart;
        private double selectedIndex = double.NaN;
        private List<ToolbarItem> rangeToolbarItems = new List<ToolbarItem>();
        private List<RangeNavigatorPeriod> buttons = new List<RangeNavigatorPeriod>();
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodSelector"/> class.
        /// </summary>
        /// <param name="rangeNavigator">Specifies the instance of component.</param>
        public PeriodSelector(SfRangeNavigator rangeNavigator)
        {
            chart = rangeNavigator;
            PeriodSelectorSize = new Rect();
        }

        internal Rect PeriodSelectorSize { get; set; }

        internal bool TriggerChange { get; set; }

        internal PeriodSelectorItems SelectorItems { get; set; }

        internal ElementReference Element { get; set; }

        // internal SfStockChart StockChart { get; set; }
        internal static void OnCreated()
        {
            // TODO:
        }

        internal static DateTime ChangedRange(RangeIntervalType type, double end, double interval)
        {
            DateTime result = new DateTime(1970, 1, 1).AddMilliseconds(end);
            switch (type)
            {
                case RangeIntervalType.Quarter:
                    return result.AddMonths(-(int)(3 * interval));
                case RangeIntervalType.Months:
                    return result.AddMonths(-(int)interval);
                case RangeIntervalType.Weeks:
                    return result.AddDays(-(interval * 7));
                case RangeIntervalType.Days:
                    return result.AddDays(-interval);
                case RangeIntervalType.Hours:
                    return result.AddHours(-interval);
                case RangeIntervalType.Minutes:
                    return result.AddMinutes(-interval);
                case RangeIntervalType.Seconds:
                    return result.AddSeconds(-interval);
                default:
                    return result.AddYears(-(int)interval);
            }
        }

        private void SetControlValues(SfRangeNavigator chart)
        {
            control = new PeriodSelectorControl()
            {
                Periods = chart.PeriodSelectorSettings.Periods,
                SeriesXMax = chart.ChartSeries.XMax,
                SeriesXMin = chart.ChartSeries.XMin,
                RangeSlider = chart.RangeSliderModule,
                DisableRangeSelector = chart.DisableRangeSelector,
                EndValue = chart.EndValue,
                StartValue = chart.StartValue,
                RangeNavigatorControl = chart
            };
        }

        internal void RenderSelector(RenderTreeBuilder builder)
        {
            int seq = 0;
            SetControlValues(chart);
            buttons = control.Periods;
            rangeToolbarItems.Clear();
            rangeToolbarItems = chart.UpdateCustomElement?.Invoke() ?? new List<ToolbarItem>();
            for (int i = 0; i < buttons.Count; i++)
            {
#pragma warning disable BL0005
                rangeToolbarItems.Add(new ToolbarItem { Align = ItemAlign.Left, Text = buttons[i].Text, CssClass = chart.Value == null && buttons[i].Selected ? "e-active" : string.Empty });
            }

            if (!chart.DisableRangeSelector && !chart.IsStockChart)
            {
                rangeToolbarItems.Add(new ToolbarItem { Align = ItemAlign.Right });
            }
            else if (chart.IsStockChart)
            {
                chart.UpdateDropdownElement?.Invoke(rangeToolbarItems);
            }

            RangeSelectorRenderEventArgs selctorArgs = new RangeSelectorRenderEventArgs
            {
                Selector = rangeToolbarItems,
                EventName = "RangeSelector",
                Content = "Date Range"
            };
            if (chart.RangeNavigatorEvents?.SelectorRender != null)
            {
                SfRangeNavigator.InvokeEvent<RangeSelectorRenderEventArgs>(chart.RangeNavigatorEvents.SelectorRender, selctorArgs);
            }

            builder.OpenComponent<PeriodSelectorItems>(seq++);
            builder.AddMultipleAttributes(seq++, new Dictionary<string, object>()
            {
                { "rangeToolbarItems", selctorArgs.Selector },
                { "min", new DateTime(1970, 1, 1).AddMilliseconds(control.SeriesXMin == double.PositiveInfinity ? 0 : control.SeriesXMin) },
                { "max", new DateTime(1970, 1, 1).AddMilliseconds(control.SeriesXMax == double.NegativeInfinity ? 0 : control.SeriesXMax) },
                { "startDate", new DateTime(1970, 1, 1).AddMilliseconds(control.StartValue) },
                { "endDate", new DateTime(1970, 1, 1).AddMilliseconds(control.EndValue) },
                { "height", PeriodSelectorSize.Height.ToString(CultureInfo.InvariantCulture) },
                { "content", selctorArgs.Content }
            });
            builder.AddComponentReferenceCapture(seq++, ins => { SelectorItems = (PeriodSelectorItems)ins; });
            builder.CloseComponent();
        }

        internal async Task OnToolbarClick(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            SfRangeNavigator parentControl = chart.GetRangeNavigator?.Invoke() ?? control.RangeNavigatorControl;
            RangeSlider slider = control.RangeNavigatorControl.IsStockChart ? parentControl.RangeSliderModule : control.RangeSlider;
            double updatedStart;
            double updatedEnd;
            RangeNavigatorPeriod button = buttons.Find(btn => (btn.Text == args.Item.TooltipText || btn.Text == args.Item.Text));
            selectedIndex = button != null ? Convert.ToDouble(buttons.FindIndex(item => item == button)) : selectedIndex;
            rangeToolbarItems.ForEach((ToolbarItem toolbarItem) =>
            {
                if (!string.IsNullOrEmpty(toolbarItem.TooltipText) && toolbarItem.TooltipText == args.Item.TooltipText)
                {
                    toolbarItem.CssClass = "e-active";
                }
                else
                {
                    toolbarItem.CssClass = string.Empty;
                }
            });
            if (args.Item.TooltipText.ToUpper(culture) == "ALL" || args.Item.Text.ToUpper(culture) == "ALL")
            {
                ClearSelectedItem();
                button.Selected = true;
                updatedStart = control.SeriesXMin;
                updatedEnd = control.SeriesXMax;
                if (slider != null)
                {
                    await slider.PerformAnimation(updatedStart, updatedEnd);
                }

                chart.UpdateChartData?.Invoke(updatedStart, updatedEnd, button);
            }
            else if (args.Item.TooltipText.ToUpper(culture) == "YTD" || (args.Item.Text.ToUpper(culture) == "YTD" && slider != null))
            {
                ClearSelectedItem();
                button.Selected = true;
                updatedStart = chart.RangeAxisModule.GetTime(new DateTime(new DateTime(1970, 1, 1).AddMilliseconds(slider.CurrentEnd).Year, 1, 1));
                updatedEnd = slider.CurrentEnd;
                await slider.PerformAnimation(updatedStart, updatedEnd);
                chart.UpdateChartData?.Invoke(updatedStart, updatedEnd, button);
            }
            else if (!string.IsNullOrEmpty(args.Item.TooltipText) || (!string.IsNullOrEmpty(args.Item.Text) && slider != null))
            {
                ClearSelectedItem();
                button.Selected = true;
                updatedStart = chart.RangeAxisModule.GetTime(PeriodSelector.ChangedRange(button.IntervalType, slider.CurrentEnd, button.Interval));
                updatedEnd = slider.CurrentEnd;
                await slider.PerformAnimation(updatedStart, updatedEnd);
                chart.UpdateChartData?.Invoke(updatedStart, updatedEnd, button);
            }

            if (parentControl.IsStockChart && button != null)
            {
                chart.UpdatePeriodEvent?.Invoke();
            }
        }

        private void ClearSelectedItem()
        {
            buttons.ForEach(x => x.Selected = false);
#pragma warning restore BL0005
        }

        internal void OnToolbarCreated()
        {
            if (double.IsNaN(selectedIndex))
            {
                buttons.ForEach((RangeNavigatorPeriod period) =>
                {
                    if (period.Selected)
                    {
                        double startValue = (period.Text.ToUpper(culture) == "YTD") ? chart.RangeAxisModule.GetTime(new DateTime(new DateTime(1970, 1, 1).AddMilliseconds(control.EndValue).Year, 1, 1)) :
                        (period.Text.ToUpper(culture) == "ALL" ? control.SeriesXMin : chart.RangeAxisModule.GetTime(PeriodSelector.ChangedRange(period.IntervalType, control.EndValue, period.Interval)));
                        control.StartValue = chart.StartValue = startValue;
                        chart.EndValue = period.Text.ToUpper(culture) == "ALL" ? control.SeriesXMax : control.EndValue;
                        selectedIndex = Convert.ToDouble(buttons.FindIndex(item => item == period));
                        chart.RangeSliderModule.SetSlider(chart.StartValue, chart.EndValue, true, chart.Tooltip.DisplayMode == TooltipDisplayMode.OnDemand);
                    }
                });
            }
        }

        internal async Task OnChanged(RangePickerEventArgs<DateTime?> args)
        {
            double currentStart = chart.RangeAxisModule.GetTime((DateTime)args.StartDate);
            double currentEnd = chart.RangeAxisModule.GetTime((DateTime)args.EndDate);
            if (TriggerChange && control.RangeSlider != null)
            {
                SfRangeNavigator parentControl = chart.GetRangeNavigator?.Invoke() ?? control.RangeNavigatorControl;
                RangeSlider slider = control.RangeNavigatorControl.IsStockChart ? parentControl.RangeSliderModule : control.RangeSlider;
                await slider.PerformAnimation(currentStart, currentEnd);
            }

            chart.UpdateChartData?.Invoke(currentStart, currentEnd, null);
        }

        internal void Dispose()
        {
            SelectorItems?.Dispose();
            SelectorItems = null;
            PeriodSelectorSize = null;
            control = null;
            chart = null;
            rangeToolbarItems = null;
            buttons = null;
        }
    }
}