using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.PdfExport;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System.Globalization;
using System.ComponentModel;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Internal;
using System.Linq;
using Syncfusion.Blazor.Charts.Internal;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfRangeNavigator : SfDataBoundComponent
    {
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal SvgRendering Rendering { get; set; } = new SvgRendering();

        internal SvgPrintExport PrintExport { get; set; }

        internal ElementReference Element { get; set; }

        private static void FindClipRect(RangeNavigatorSeries series)
        {
            Rect rect = series.ClipRect;
            rect.X = series.XAxisRenderer.Rect.X;
            rect.Y = series.YAxisRenderer.Rect.Y;
            rect.Width = series.XAxisRenderer.Rect.Width;
            rect.Height = series.YAxisRenderer.Rect.Height;
        }

        private void ClearSvgReferences()
        {
            SvgRenderer.PathElementList.RemoveAll(item => item != null);
            SvgRenderer.LineElementList.RemoveAll(item => item != null);
            SvgRenderer.GroupCollection.RemoveAll(item => item.Id != null);
            SvgRenderer.ImageCollection.RemoveAll(item => item != null);
            SvgRenderer.LineElementList.RemoveAll(item => item != null);
            SvgRenderer.PathElementList.RemoveAll(item => item != null);
            SvgRenderer.RectElementList.RemoveAll(item => item != null);
            SvgRenderer.TextElementList.RemoveAll(item => item != null);
        }

        private void CreateChart()
        {
            ClearSvgReferences();
            ChartContent = RenderElements();
            if (Tooltip.Enable)
            {
                TooltipContent = RenderTooltipElements();
            }

            InvokeAsync(StateHasChanged);
        }

        private RenderFragment RenderTooltipElements() => builder =>
        {
            builder.OpenElement(SvgRendering.Seq++, "div");
            builder.AddAttribute(SvgRendering.Seq++, "id", Id + "_rightTooltip");
            builder.AddAttribute(SvgRendering.Seq++, "class", "ejSVGTooltip");
            builder.AddAttribute(SvgRendering.Seq++, "style", "pointer-events:none; position:absolute;z-index: 1");
            builder.AddElementReferenceCapture(SvgRendering.Seq++, ins => { TooltipModule.RightElement = ins; });
            builder.CloseElement();
            builder.OpenElement(SvgRendering.Seq++, "div");
            builder.AddAttribute(SvgRendering.Seq++, "id", Id + "_leftTooltip");
            builder.AddAttribute(SvgRendering.Seq++, "class", "ejSVGTooltip");
            builder.AddAttribute(SvgRendering.Seq++, "style", "pointer-events:none; position:absolute;z-index: 1");
            builder.AddElementReferenceCapture(SvgRendering.Seq++, ins => { TooltipModule.LeftElement = ins; });
            builder.CloseElement();
        };

        internal void InvokeThumbTooltip()
        {
            Chart.TooltipModule?.RenderThumbTooltip(RangeSliderModule);
        }

        /// <summary>
        /// The method is used to perform the print functionality in range navigator.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Print()
        {
            await PrintAsync();
        }

        /// <summary>
        /// The method is used to perform the print functionality in range navigator.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PrintAsync()
        {
            await PrintExport.Print(Element);
            if (RangeNavigatorEvents?.OnPrintCompleted != null)
            {
                await SfBaseUtils.InvokeEvent<System.EventArgs>(RangeNavigatorEvents.OnPrintCompleted, new EventArgs() { });
            }
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered range navigator.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered range navigator.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <param name="allowDownload">Specifies whether to download or not.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void Export(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            await PrintExport.Export(type, fileName, Id + RangeConstants.SVG, orientation, allowDownload);
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered range navigator.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered range navigator.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <param name="allowDownload">Specifies whether to download or not.</param>
        public async Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null, bool allowDownload = true)
        {
            await PrintExport.Export(type, fileName, Id + RangeConstants.SVG, orientation, allowDownload);
        }

        private void InstanceInitialization()
        {
            RangeAxisModule = new RangeAxis(Chart);
            RangeSliderModule = new RangeSlider(Chart);
            ChartSeries = new RangeNavigatorSeries(Chart);
            PrintExport = new SvgPrintExport(JSRuntime);
            if (Tooltip.Enable)
            {
                TooltipModule = new RangeTooltip(this);
            }
        }

        private RenderFragment RenderElements() => treebuilder =>
        {
            ClearSvgReferences();
            SetSliderValue();
            if (!IsStockChart)
            {
                treebuilder.OpenElement(SvgRendering.Seq++, "svg");
                Dictionary<string, object> svgattributes = new Dictionary<string, object>
                {
                    { "id", Id + "_svg" },
                    { "height", AvailableSize.Height },
                    { "width", AvailableSize.Width },
                };
                treebuilder.AddMultipleAttributes(SvgRendering.Seq++, svgattributes);
            }

            RenderChartBorder(treebuilder);
            if (!DisableRangeSelector)
            {
                RangeAxisModule.RenderGridLines(treebuilder, Chart.XAxisRenderer);
                RangeAxisModule.RenderAxisLabels(treebuilder);
            }

            RenderSeriesElements(treebuilder);
            RangeSliderModule.Render(Chart, treebuilder);
            if (!IsStockChart)
            {
                treebuilder.CloseElement();
            }
        };

        /// <summary>
        /// JS interop for remove the existing element.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void RemoveElements()
        {
            ChartContent = null;
            StateHasChanged();
            SvgRenderer?.RefreshElementList();
        }

        private void RenderSeriesElements(RenderTreeBuilder builder)
        {
            SvgRenderer.OpenGroupElement(builder, Id + "SeriesCollection");
            RenderSeries(builder);
            builder.CloseElement();
        }

        internal void RenderSeries(RenderTreeBuilder builder)
        {
            foreach (RangeNavigatorSeries series in VisibleSeries)
            {
                series.XAxisRenderer = XAxisRenderer;
                series.YAxisRenderer = YAxisRenderer;
#pragma warning disable BL0005
                series.XAxisRenderer.Axis.IsInversed = IsRtlEnabled();
#pragma warning restore BL0005
                FindClipRect(series);
                series.RenderSeries(builder);
            }
        }

        private void SetSliderValue()
        {
            bool isDateTime = ValueType == RangeValueType.DateTime;
            DoubleRange range = ChartSeries.XAxisRenderer.ActualRange;
            object[] sliderRange = Value != null ? ((Array)Value).Cast<object>().ToArray() : null;
            StartValue = !double.IsNaN(StartValue) ? StartValue : (sliderRange != null && sliderRange[0] != null ? (isDateTime ? Chart.RangeAxisModule.GetTime(Convert.ToDateTime(sliderRange[0], null)) : Convert.ToDouble(sliderRange[0], null)) : range.Start);
            EndValue = !double.IsNaN(EndValue) ? EndValue : (sliderRange != null && sliderRange[1] != null ? (isDateTime ? Chart.RangeAxisModule.GetTime(Convert.ToDateTime(sliderRange[1], null)) : Convert.ToDouble(sliderRange[1], null)) : range.End);
        }

        private void RenderChartBorder(RenderTreeBuilder builder)
        {
            Rendering.RenderRect(builder, Id + Constants.BORDERID, 0, 0, AvailableSize.Width, AvailableSize.Height, 0, "transparent", Chart.ThemeStyle.Background);
        }
    }
}