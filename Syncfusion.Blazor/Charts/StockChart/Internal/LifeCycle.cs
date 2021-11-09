using System;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfStockChart : SfDataBoundComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            ScriptModules = SfScriptModules.SfStockChart;
            InitPrivateVariables();
            await base.OnInitializedAsync();
            DependentScripts.Add(Blazor.Internal.ScriptModules.SfSvgExport);
        }

        internal override async Task OnAfterScriptRendered()
        {
            await InvokeMethod<bool>(StockChartConstants.INITIALIZE, false, new object[] { Element, DotnetObjectReference });
            await RenderStockChart(isStockChartRendered);
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Represents the first render.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RenderStockChart();
            }
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender)
            {
                StockEventsRender?.InvalidateRenderer();
            }
        }

        private async Task RenderStockChart()
        {
            try
            {
                ElementOffset = await InvokeMethod<DomRect>(StockChartConstants.INITAILELEMENTBOUNDSBYID, false, new object[] { ID });
                if (ElementOffset == null)
                {
                    return;
                }

                SetContainerSize();
                TitleContent = RenderTitle();
                if (Series.Count > 0)
                {
                    CalculateSeriesXMinMax();
                }

                if (StockEvents.Count > 0)
                {
                    StockEventTooltipContent = RenderTooltipElements();
                }

                isStockChartRendered = true;
                UpdateStockChart();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        private async Task RenderStockChart(bool isRender)
        {
            if (isRender)
            {
                return;
            }

            try
            {
                await InvokeMethod<DomRect>(StockChartConstants.SETATTRIBUTE, false, new object[] { ID, "style", "display: none;" });
                ElementOffset = await InvokeMethod<DomRect>(StockChartConstants.GETPARENTELEMENTBOUNDSBYID, false, new object[] { ID });
                SetContainerSize();
                TitleContent = RenderTitle();
                if (Series.Count > 0)
                {
                    CalculateSeriesXMinMax();
                }
                if (StockEvents.Count > 0)
                {
                    StockEventTooltipContent = RenderTooltipElements();
                }

                isStockChartRendered = true;
                await InvokeMethod<DomRect>(StockChartConstants.SETATTRIBUTE, false, new object[] { ID, "style", "display: block;" });
                RangeSelectorSettings?.ResizeChart();
                UpdateStockChart();
                ShouldStockChartRender = false;
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            dataSource = NotifyPropertyChanges(nameof(DataSource), DataSource, dataSource);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            if (PropertyChanges.Any() && IsRendered)
            {
                OnStockChartPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
            tempPeriods.Clear();
            ChartSettings?.Dispose();
            PeriodSelectorSettings?.Dispose();
            RangeSelectorSettings?.Dispose();
            StockEventInstance = null;
            AnnotationContent = null;
            RangeTooltipContent = null;
            PeriodSelectorContent = null;
            TooltipContent = null;
            RangeSelectorSettings = null;
            PeriodSelector = null;
            SvgRenderer = null;
            TitleContent = null;
            TitleStyle = null;
        }

        /// <summary>
        /// Method helps to re-render the stock chart.
        /// </summary>
        public void Refresh()
        {
            if (isStockChartRendered)
            {
                ShouldStockChartRender = true;
            }
            UpdateStockChartData();
            InvokeAsync(StateHasChanged);
        }

        private async void UpdateStockChartData()
        {
            foreach (StockChartSeries chartSeries in Series)
            {
                chartSeries.CurrentViewData = await chartSeries.UpdatedSeriesData();
                chartSeries.FilteredData = chartSeries.CurrentViewData;
            }
        }

        /// <summary>
        /// Method helps to re-render the stock chart UI element.
        /// </summary>
        public void UpdateStockChart()
        {
            ShouldStockChartRender = true;
            StateHasChanged();
        }
    }
}