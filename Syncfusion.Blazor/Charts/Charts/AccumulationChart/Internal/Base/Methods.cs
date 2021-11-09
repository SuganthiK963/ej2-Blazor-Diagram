using System;
using System.Threading.Tasks;
using Syncfusion.PdfExport;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        /// <summary>
        /// Method to set the annotation content dynamically for accumulation.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task SetAnnotationValue(double annotationIndex, string content)
        {
            await SetAnnotationValueAsync(annotationIndex, content);
        }

        /// <summary>
        /// Method to set the annotation content dynamically for accumulation.
        /// </summary>
        public async Task SetAnnotationValueAsync(double annotationIndex, string content)
        {
#pragma warning disable CA2007
            await InvokeMethod("setAnnotationValue", null, annotationIndex, content);
        }

        /// <summary>
        /// Handles the print method for accumulation chart control.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Print()
        {
            await PrintAsync();
        }

        /// <summary>
        /// Handles the print method for accumulation chart control.
        /// </summary>
        public async Task PrintAsync()
        {
            await PrintExportModule.Print(Element);
            if (AccumulationChartEvents?.OnPrintComplete != null)
            {
                AccumulationChartEvents.OnPrintComplete.Invoke();
            }
        }

        /// <summary>
        /// The method is used to perform the export functionality for the Chart.
        /// </summary>
        /// <param name="type">Specifies the format of the file to export the circular gauge.</param>
        /// <param name="fileName">Specifies the name of the file for the exported circular gauge.</param>
        /// <param name="orientation">Specifies the orientation of the exported PDF document when the type parameter is PDF.</param>
        /// <param name="allowDownload">Specifies whether the exported file is to be downloaded or not.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Export(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            await ExportAsync(type, fileName, orientation, allowDownload);
        }

        /// <summary>
        /// The method is used to perform the export functionality for the Chart.
        /// </summary>
        /// <param name="type">Specifies the format of the file to export the circular gauge.</param>
        /// <param name="fileName">Specifies the name of the file for the exported circular gauge.</param>
        /// <param name="orientation">Specifies the orientation of the exported PDF document when the type parameter is PDF.</param>
        /// <param name="allowDownload">Specifies whether the exported file is to be downloaded or not.</param>
        public async Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            await PrintExportModule.Export(type, fileName, SvgId, orientation, allowDownload);
#pragma warning restore CA2007
            if (AccumulationChartEvents?.OnExportComplete != null)
            {
                PrintExportModule.TriggerOnExportCompleted(null, this, type, fileName, SvgId);
            }
        }

        /// <summary>
        /// This re renders the accumulation chart.
        /// </summary>
        public async void Refresh(bool shouldAnimate = true)
        {
            try
            {
                animateSeries = shouldAnimate;
                CalculateVisibleSeries();
                await GetContainerSize();
                SetContainerSize();
                ProcessData();
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }
    }
}