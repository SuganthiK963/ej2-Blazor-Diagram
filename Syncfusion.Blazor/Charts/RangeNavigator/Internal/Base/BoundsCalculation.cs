using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfRangeNavigator : SfDataBoundComponent
    {
        internal ChartHelper ChartHelper { get; set; } = new ChartHelper();

        internal Rect InitialClipRect { get; set; } = new Rect();

        internal DomRect ElementOffset { get; set; }

        internal Size AvailableSize { get; set; }

        internal List<RangeNavigatorSeries> VisibleSeries { get; set; } = new List<RangeNavigatorSeries>();

        internal async Task SetContainerSize()
        {
            ElementOffset = await InvokeMethod<DomRect>(RangeConstants.GETELEMENTSIZE, false, new object[] { Element, Height, Width });
            CalculateAvailableSize();
        }

        internal async void ResizeChart()
        {
            try
            {
                ChartContent = null;
                PeriodSelectorContent = null;
                TooltipContent = null;
                await InvokeAsync(StateHasChanged);
                Size previousSize = new Size(AvailableSize.Width, AvailableSize.Height);
                await SetContainerSize();
                if (RangeNavigatorEvents?.Resized != null)
                {
                    await SfBaseUtils.InvokeEvent<RangeResizeEventArgs>(RangeNavigatorEvents.Resized, new RangeResizeEventArgs
                    {
                        EventName = RangeConstants.RESIZED,
                        PreviousSize = previousSize,
                        CurrentSize = AvailableSize
                    });
                }

                await ProcessData();
                if (Tooltip != null && Tooltip.Enable && Tooltip.DisplayMode == TooltipDisplayMode.Always)
                {
                    if (IsStockChart)
                    {
                        await Task.Delay(100);
                    }
                    await TooltipModule.RenderThumbTooltip(RangeSliderModule);
                }
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private async Task TriggerLoadedEvent()
        {
            await SfBaseUtils.InvokeEvent<RangeLoadedEventArgs>(RangeNavigatorEvents?.Loaded, new RangeLoadedEventArgs { Navigator = this, EventName = RangeConstants.LOADED });
        }

        private void CalculateAvailableSize()
        {
            double width = Width.Contains("%", System.StringComparison.InvariantCulture) ? ElementOffset.Width : double.Parse(Width.ToLower(culture).Replace("px", string.Empty, System.StringComparison.InvariantCulture), culture);
            double height = Height.Contains("%", System.StringComparison.InvariantCulture) ? ElementOffset.Height : double.Parse(Height.ToLower(culture).Replace("px", string.Empty, System.StringComparison.InvariantCulture), culture);
            height = ChartHelper.IsNaNOrZero(height) ? 120 : height;
            double periodHeight = PeriodSelectorSettings.Periods.Count > 0 ? PeriodSelectorSettings.Height : 0;
            height = (Series.Count > 0 ? height : ((EnableGrouping ? (55 + ChartHelper.MeasureText("tempString", GetFontOptions(LabelStyle)).Height) : 40) + (Margin.Top + Margin.Bottom + (Tooltip.Enable ? 35 : 0)))) + periodHeight;
            if (DisableRangeSelector)
            {
                height = periodHeight;
            }

            AvailableSize = new Size(width > 0 ? width : 600, height > 0 ? height : 120);
        }

        internal static ChartFontOptions GetFontOptions(ChartCommonFont font)
        {
            return new ChartFontOptions { Color = font.Color, Size = font.Size, FontFamily = font.FontFamily, FontWeight = font.FontWeight, FontStyle = font.FontStyle, TextAlignment = font.TextAlignment, TextOverflow = font.TextOverflow };
        }

        internal async Task ProcessData()
        {
            await UpdateData();
            CalculateBounds();
            InstanceInitialization();
            ChartSeries.RenderChart();
            CreateChart();
            RangeSliderModule?.TriggerEvent(ChartSeries.XAxisRenderer.ActualRange);
            await TriggerLoadedEvent();
        }

        private async Task UpdateData()
        {
            if (DataSource != null && DataManager == null)
            {
                SetDataManager<object>(DataSource);
            }

            if (DataManager == null)
            {
                return;
            }

            DataManager.DataAdaptor.SetRunSyncOnce(true);
            FinalData = (IEnumerable<object>)await DataManager.ExecuteQuery<object>((Query != null) ? Query : new Data.Query());
        }

        private void CalculateVisibleSeries()
        {
            VisibleSeries.Clear();
            string[] colors = ChartHelper.GetSeriesColor(Theme.ToString());
            int count = colors.Length;
            RangeNavigatorSeries series;
            for (int i = 0; i < Series.Count; i++)
            {
                series = Series[i];
                series.Index = i;
                series.Interior = !string.IsNullOrEmpty(series.Fill) ? series.Fill : colors[series.Index % count];
                VisibleSeries.Add(series);
            }
        }

        private void CalculateBounds()
        {
            double labelPadding = EnableGrouping ? 15 : 8, labelSize = ChartHelper.MeasureText("tempString", GetFontOptions(LabelStyle)).Height;
            RangeNavigatorThumbSettings thumb = NavigatorStyleSettings.Thumb;
            RangeNavigatorMargin margin = Margin;
            bool isLeightWeight = Series.Count == 0;
            double tooltipSpace = !DisableRangeSelector && isLeightWeight && Tooltip.Enable ? 35 : 0;
            PeriodSelectorModule = new PeriodSelector(this);
            if (PeriodSelectorSettings.Periods.Count > 0)
            {
                PeriodSelectorModule.PeriodSelectorSize = new Rect(0, 0, 0, 0);
                PeriodSelectorModule.PeriodSelectorSize.Width = AvailableSize.Width;
                PeriodSelectorModule.PeriodSelectorSize.Height = PeriodSelectorSettings.Height;
                PeriodSelectorModule.PeriodSelectorSize.Y = PeriodSelectorSettings.Position == PeriodSelectorPosition.Bottom ? AvailableSize.Height - PeriodSelectorModule.PeriodSelectorSize.Height : 0;
            }

            double periodSelectorY = PeriodSelectorSettings.Position == PeriodSelectorPosition.Top ? PeriodSelectorModule.PeriodSelectorSize.Y + PeriodSelectorModule.PeriodSelectorSize.Height : 0;
            InitialClipRect = new Rect((ThemeStyle.ThumbWidth / 2) + thumb.Border.Width + margin.Left, margin.Top + tooltipSpace + periodSelectorY, AvailableSize.Width - ThemeStyle.ThumbWidth - (thumb.Border.Width * 2) - margin.Left - margin.Right, AvailableSize.Height - margin.Top - margin.Bottom - tooltipSpace - PeriodSelectorModule.PeriodSelectorSize.Height);
            double deductHeight = (LabelPosition == AxisPosition.Outside || isLeightWeight ? (labelSize + labelPadding) : 0) + ((TickPosition == AxisPosition.Outside || isLeightWeight) ? MajorTickLines.Height : 0);
            InitialClipRect.Height -= deductHeight;
            if (isLeightWeight)
            {
                double height = EnableGrouping ? InitialClipRect.Height - (labelSize + labelPadding) : InitialClipRect.Height;
                InitialClipRect.Y += ThemeStyle.ThumbHeight > height ? (ThemeStyle.ThumbHeight - height) / 2 : 0;
            }

            if (DisableRangeSelector)
            {
                InitialClipRect.Y = 0;
                InitialClipRect.Height = PeriodSelectorSettings.Height;
            }
        }
    }
}