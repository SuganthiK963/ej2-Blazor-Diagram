using System.Threading.Tasks;
using Syncfusion.PdfExport;
using System.IO;
using System;
using Syncfusion.Blazor.Internal;
using System.ComponentModel;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfBulletChart<TValue>
    {
        /// <summary>
        /// The method is used to re-render the bullet chart.
        /// </summary>
        /// <exclude />
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This method is deprecated and will no longer be used.")]
        public async Task Refresh()
        {
            isRender = true;
            resize = true;
            await InvokeAsync(() => StateHasChanged());
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered bullet chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered bullet chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <returns>Returns base64 string.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Export(ExportType type, string fileName, PdfPageOrientation? orientation = null)
        {
            if (type != ExportType.PDF)
            {
                await InvokeMethod<string>("sfExport.exportToImage", false, new object[] { type.ToString(), fileName, ID + "_svg", true });
            }
            else
            {
                if (orientation == null)
                {
                    orientation = PdfPageOrientation.Portrait;
                }

                await ExportToPdf(fileName, (PdfPageOrientation)orientation);
            }
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered bullet chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered bullet chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <returns>Returns base64 string.</returns>
        public async Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null)
        {
            await Export(type, fileName, orientation);
        }

        /// <summary>
        /// The method is used to perform the print functionality in bullet chart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Print()
        {
            PrintEventArgs args = new PrintEventArgs();
            await SfBaseUtils.InvokeEvent(Events?.OnPrintComplete, args);
            if (!args.Cancel)
            {
                await InvokeMethod("sfExport.print", new object[] { element });
            }
        }

        /// <summary>
        /// The method is used to perform the print functionality in bullet chart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PrintAsync()
        {
            await Print();
        }

        private async Task ExportToPdf(string fileName, PdfPageOrientation orientation)
        {
            string base64String = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.exportToImage", new object[] { "PNG", fileName, ID + "_svg", false });
            byte[] data = Convert.FromBase64String(base64String.Split("base64,")[1]);
            using (MemoryStream initialStream = new MemoryStream(data))
            {
                Stream stream = initialStream as Stream;
                using (PdfDocument document = new PdfDocument())
                {
                    document.PageSettings.Orientation = orientation;
                    PdfPage page = document.Pages.Add();
                    PdfGraphics graphics = page.Graphics;
                    using (PdfBitmap image = new PdfBitmap(stream))
                    {
                        graphics.DrawImage(image, 0, 0);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            document.Save(memoryStream);
                            memoryStream.Position = 0;
                            base64String = Convert.ToBase64String(memoryStream.ToArray());
                            await JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, "sfExport.downloadPdf", new object[] { base64String, fileName });
                            base64String = string.Empty;
                            document.Dispose();
                        }
                    }
                }
            }
        }
    }
}
