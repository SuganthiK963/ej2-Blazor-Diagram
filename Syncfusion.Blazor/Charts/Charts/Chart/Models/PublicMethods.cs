using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.PdfExport;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfChart : SfDataBoundComponent, ISubcomponentTracker
    {
        /// <summary>
        /// The method is used to add the axes in chart.
        /// </summary>
        /// <param name="axisCollection">Specifies the chart axis collection.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void AddAxes(List<ChartAxis> axisCollection)
        {
            if (axisCollection == null)
            {
                return;
            }
        }

        /// <summary>
        /// The method is used to perform the print functionality in chart.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Print()
        {
            await PrintAsync();
        }

        /// <summary>
        /// The method is used to perform the print functionality in chart.
        /// </summary>
        /// <returns>Print the chart.</returns>
        public async Task PrintAsync()
        {
            await printExport.Print(Element);
            if (ChartEvents?.OnPrintComplete != null)
            {
                ChartEvents.OnPrintComplete.Invoke();
            }
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <param name="allowDownload">Specifies whether to download or not.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Export(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            await ExportAsync(type, fileName, orientation, allowDownload);
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <param name="allowDownload">Specifies whether to download or not.</param>
        /// <returns>Export the chart with sepcifies export type.</returns>
        public async Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            await printExport.Export(type, fileName, SvgId(), orientation, allowDownload);
            if (ChartEvents?.OnExportComplete != null)
            {
                printExport.TriggerOnExportCompleted(this, null, type, fileName, SvgId());
            }
        }

        /// <summary>
        /// The method is used to refresh the chart.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Refresh(bool shouldAnimate = true)
        {
            await RefreshAsync(shouldAnimate);
        }

        /// <summary>
        /// The method is used to re-render the chart.
        /// </summary>
        public async Task RefreshAsync(bool shouldAnimate = true)
        {
            ShouldAnimateSeries = shouldAnimate;
            await GetElementOffset(Constants.GETELEMENTBOUNDSBYID);
            CalculateAvailableSize();
            SetSvgDimension(Constants.SETSVGDIMENSION);
            UpdateData();
            await GetRemoteData();
            InitPrivateVariable();
            InitModules();
            InitPrivateModules();
            Prerender();
            RefreshChart();
            ApplyZoomkit();
            ChartScrollBarContent?.CallStateHasChanged();
            await PerformDelayAnimation();
        }

        /// <summary>
        /// The method is used to refresh the chart data live updates.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RefreshLiveData()
        {
            RefreshChart();
        }

        /// <summary>
        /// The method is used to add the series collection in chart.
        /// </summary>
        /// <param name="seriesCollection">Specifies the chart series collection.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddSeries(List<ChartSeries> seriesCollection)
        {
            await AddSeriesAsync(seriesCollection);
        }

        /// <summary>
        /// The method is used to add the series collection in chart.
        /// </summary>
        /// <param name="seriesCollection">Specifies the chart series collection.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task AddSeriesAsync(List<ChartSeries> seriesCollection)
        {
            if (seriesCollection == null)
            {
                return;
            }

            if (seriesCollection.Count > 0)
            {
                foreach (ChartSeries series in seriesCollection)
                {
                    series.Container = this;
                    series.RendererType = ChartSeriesRenderer.GetRendererType(series.Type, series.DrawType);
                    await series.UpdateSeriesData();
                    if (series.Marker != null && series.Marker.Visible)
                    {
                        series.Marker.RendererType = typeof(ChartMarkerRenderer);
                    }

                    if (series.Marker.DataLabel != null && series.Marker.DataLabel.Visible)
                    {
                        series.Marker.DataLabel.RendererType = typeof(ChartDataLabelRenderer);
                    }

                    AddSeries(series);
                    foreach (ChartTrendline trendline in series.Trendlines)
                    {
                        trendline.Parent = new ChartTrendlines();
                        trendline.Parent.Series = series;
                        trendline.InitTrendline();
                    }
                }

                SeriesContainer.RendererShouldRender = true;
                SeriesContainer.Prerender();
                ProcessOnLayoutChange();
            }
        }

        /// <summary>
        /// The method is used to remove the specific series in chart.
        /// </summary>
        /// <param name="index">Specifies the index of the series collection.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveSeries(int index)
        {
            if (SeriesContainer.Elements.Count > 0)
            {
                ChartSeries series = SeriesContainer.Elements[index] as ChartSeries;
                SeriesContainer.RemoveElement(series);
                ProcessOnLayoutChange();
            }
        }

        /// <summary>
        ///  The method is used to clear the series in chart.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ClearSeries()
        {
            foreach (ChartSeries series in SeriesContainer.Elements.ToList())
            {
                SeriesContainer.RemoveElement(series);
            }

            ProcessOnLayoutChange();
        }
    }
}