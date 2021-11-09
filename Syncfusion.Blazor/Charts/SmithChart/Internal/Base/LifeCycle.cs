using Syncfusion.Blazor.Charts.SmithChart.Internal;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Syncfusion.PdfExport;
using System;
using System.IO;
using System.ComponentModel;
using Microsoft.JSInterop;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSmithChart : SfDataBoundComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            ScriptModules = SfScriptModules.SfSmithChart;
            DependentScripts = new List<ScriptModules>() { Blazor.Internal.ScriptModules.SvgBase, Blazor.Internal.ScriptModules.SfSvgExport };
            SmithChartThemeStyle = SmithChartHelper.GetSmithChartThemeStyle(Theme);
        }

        /// <summary>
        /// The method is used to perform the print functionality in smith chart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Print()
        {
            InvokeEvent<EventArgs>(SmithChartEvents?.OnPrintComplete, new EventArgs());
            await InvokeMethod("sfExport.print", new object[] { Element });
        }

        /// <summary>
        /// The method is used to perform the print functionality in smith chart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PrintAsync()
        {
            await Print();
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered smith chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered smith chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Export(ExportType type, string fileName, PdfPageOrientation? orientation = null)
        {
            if (type != ExportType.PDF)
            {
                await InvokeMethod<string>("sfExport.exportToImage", false, new object[] { type.ToString(), fileName, ID + "_svg", true });
            }

            if (orientation == null)
            {
                orientation = PdfPageOrientation.Portrait;
            }

            await ExportToPdf(fileName, (PdfPageOrientation)orientation);
            AnimateSeries = false;
            if (SmithChartEvents?.OnExportComplete != null)
            {
                string base64String = await InvokeMethod<string>("sfExport.exportToImage", false, new object[] { type.ToString(), fileName, ID + "_svg", false });
                SmithChartExportEventArgs args = new SmithChartExportEventArgs(base64String);
                InvokeEvent<SmithChartExportEventArgs>(SmithChartEvents?.OnExportComplete, args);
            }
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered smith chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered smith chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null)
        {
            await Export(type, fileName, orientation);
        }

        private async Task ExportToPdf(string fileName, PdfPageOrientation orientation)
        {
            string base64String = await JSRuntimeExtensions.InvokeAsync<string>(JSRuntime, "sfExport.exportToImage", new object[] { "PNG", fileName, ID + "_svg", false });
            byte[] data = Convert.FromBase64String(base64String?.Split("base64,")[1]);
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

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            background = NotifyPropertyChanges(nameof(Background), Background, background);
            elementSpacing = NotifyPropertyChanges(nameof(ElementSpacing), ElementSpacing, elementSpacing);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);
            renderType = NotifyPropertyChanges(nameof(RenderType), RenderType, renderType);
            theme = NotifyPropertyChanges(nameof(Theme), Theme, theme);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            if (PropertyChanges.Any() && IsRendered)
            {
                await PropertyChanged(PropertyChanges, string.Empty);
            }
        }

        internal async Task PropertyChanged(Dictionary<string, object> properties, string parent)
        {
            bool renderer = false;
            if (properties.Any() && IsRendered && ChartContent != null && AvailableSize != null)
            {
                AnimateSeries = false;
                List<string> propKeys = PropertyChanges?.Keys.ToList();
                if (!string.IsNullOrEmpty(parent))
                {
                    propKeys.Clear();
                    propKeys.Add(parent);
                }

                foreach (string property in propKeys)
                {
                    switch (property)
                    {
                        case "Background":
                        case "Border":
                        case "ElementSpacing":
                        case "HorizontalAxis":
                        case "RadialAxis":
                        case "Margin":
                        case "Title":
                        case "Font":
                        case "LegendSettings":
                        case "Radius":
                            renderer = true;
                            break;
                        case "Height":
                        case "Width":
                            isResize = true;
                            await SetContainerSize();
                            renderer = true;
                            break;
                        case "Theme":
                            AnimateSeries = true;
                            renderer = true;
                            SmithChartThemeStyle = SmithChartHelper.GetSmithChartThemeStyle(Theme);
                            break;
                        case "Series":
                            if (properties.ContainsKey("EnableAnimation"))
                            {
                                AnimateSeries = true;
                            }

                            CalculateVisibleSeries();
                            renderer = true;
                            break;
                        case "RenderType":
                            AnimateSeries = true;
                            renderer = true;
                            break;
                    }
                }

                if (renderer)
                {
                    ChartContent = null;
                    ProcessData();
                }
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            DomRect elementData = await InvokeMethod<DomRect>("sfBlazor.SmithChart.initialize", false, new object[] { ID, DotnetObjectReference, Height, Width, Element });
            if (elementData != null && AvailableSize == null)
            {
                ElementOffset = elementData;
                string result = await InvokeMethod<string>(SmithChartConstants.GETCHARSIZEBYFONT, false, new object[] { fontKeys });
                SmithChartHelper.SizePerCharacter = JsonSerializer.Deserialize<Dictionary<string, Size>>(result);
                await ComponentRender();
            }

            await PerformAnimation();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Specifies the first render of the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ElementOffset = await InvokeMethod<DomRect>(SmithChartConstants.GETELEMENTBOUNDS, false, new object[] { ID });
                if (ElementOffset != null)
                {
                    ElementOffset.Height = SmithChartHelper.StringToNumber(Height, ElementOffset.Height);
                    ElementOffset.Width = SmithChartHelper.StringToNumber(Width, ElementOffset.Width);
                    SetFontKeys();
                    await GetCharSizeList();
                    await ComponentRender();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
            await PerformAnimation();
        }

        private async Task GetCharSizeList()
        {
            List<string> uniqueKeys = new List<string>();
            foreach (string fontKey in fontKeys)
            {
                if (!chartFontKeys.Contains(fontKey))
                {
                    uniqueKeys.Add(fontKey);
                    chartFontKeys.Add(fontKey);
                }
            }

            if (uniqueKeys.Count == 0)
            {
                return;
            }

            string result = await InvokeMethod<string>("sfBlazor.getCharCollectionSize", false, new object[] { uniqueKeys });
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            List<Dictionary<string, Point>> charSizeList = JsonSerializer.Deserialize<List<Dictionary<string, Point>>>(result);
            for (int i = 0; i < charSizeList.Count; i++)
            {
                charSizeList[i].ToList().ForEach(keyValue => SmithChartHelper.SizePerCharacter.TryAdd(keyValue.Key + "_" + fontKeys[i], new Size { Width = keyValue.Value.X, Height = keyValue.Value.Y }));
            }
        }

        private async Task ComponentRender()
        {
            CalculateVisibleSeries();
            await SetContainerSize();
            ProcessData();
        }

        internal override async void ComponentDispose()
        {
            ChildContent = null;
            ChartContent = null;
            TooltipContent = null;
            DatalabelTemplate = null;
            Series = null;
            HorizontalAxis = null;
            RadialAxis = null;
            Title = null;
            LegendSettings = null;
            Border = null;
            Margin = null;
            Rendering?.Dispose();
            Rendering = null;
            SmithChartLegendModule?.Dispose();
            SmithChartLegendModule = null;
            AxisModule?.Dispose();
            AxisModule = null;
            SeriesModule?.Dispose();
            SeriesModule = null;
            TooltipModule?.Dispose();
            TooltipModule = null;
            DataLabelModule?.Dispose();
            DataLabelModule = null;
            Helper = null;
            Bounds = null;
            LegendBounds = null;
            NumberFormatter = null;
            AvailableSize = null;
            ChartArea = null;
            VisibleSeries = null;
            SmithChartEvents = null;
            fontKeys = null;
            if (IsRendered && !IsDisposed)
            {
                await InvokeMethod("sfBlazor.SmithChart.destroy", new object[] { Element });
            }
        }
    }
}