using System;
using System.IO;
using Syncfusion.PdfExport;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Charts
{
    public class SvgPrintExport
    {
        internal SvgPrintExport(IJSRuntime js_Runtime)
        {
            JSRuntime = js_Runtime;
        }

        internal IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Handles the print method for accumulation chart control.
        /// </summary>
        internal async Task Print(ElementReference element)
        {
            await JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, "sfExport.print", new object[] { element });
        }

        /// <summary>
        /// The method is used to perform the export functionality for the chart components.
        /// </summary>
        /// <param name="type">Specifies the format of the file to export the chart.</param>
        /// <param name="fileName">Specifies the name of the file for the exported chart.</param>
        /// <param name="id">Specifies the chart svg id.</param>
        /// <param name="orientation">Specifies the orientation of the exported PDF document when the type parameter is PDF.</param>
        /// <param name="allowDownload">Specifies whether the exported file is to be downloaded or not.</param>
        internal async Task<string> Export(ExportType type, string fileName, string id, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            if (type != ExportType.PDF)
            {
                return await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.exportToImage", new object[] { type.ToString(), fileName, id, allowDownload });
            }
            else
            {
                if (orientation == null)
                {
                    orientation = PdfPageOrientation.Portrait;
                }

                return await ExportToPdf(fileName, (PdfPageOrientation)orientation, allowDownload, id);
            }
        }

        private async Task<string> ExportToPdf(string fileName, PdfPageOrientation orientation, bool allowDownload, string id)
        {
            string base64String = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.exportToImage", new object[] { "PNG", fileName, id, false });
            using (MemoryStream initialStream = new MemoryStream(Convert.FromBase64String(base64String.Split("base64,")[1])))
            {
                Stream stream = initialStream as Stream;
                PdfDocument document = new PdfDocument();
                document.PageSettings.Orientation = orientation;
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;
#pragma warning disable CA2000
                PdfBitmap image = new PdfBitmap(stream);
#pragma warning restore CA2000
                graphics.DrawImage(image, 0, 0);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    document.Save(memoryStream);
                    memoryStream.Position = 0;
                    base64String = Convert.ToBase64String(memoryStream.ToArray());
                    if (allowDownload)
                    {
                        await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.downloadPdf", new object[] { base64String, fileName });
                        base64String = string.Empty;
                    }
                    else
                    {
                        base64String = "data:application/pdf;base64," + base64String;
                    }

                    document.Dispose();
                }
            }

            return base64String;
        }

        internal async void TriggerOnExportCompleted(SfChart chart, SfAccumulationChart accChart, ExportType type, string fileName, string id)
        {
            string base64String = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.exportToImage", new object[] { type.ToString(), fileName, id, false });
            ExportEventArgs argsData = new ExportEventArgs(Constants.ONEXPORTCOMPLETED, base64String);
            if (chart != null)
            {
                chart.ChartEvents.OnExportComplete.Invoke(argsData);
            }

            if (accChart != null)
            {
                accChart.AccumulationChartEvents.OnExportComplete.Invoke(argsData);
            }
        }

        internal void Dispose()
        {
            JSRuntime = null;
        }
    }
}