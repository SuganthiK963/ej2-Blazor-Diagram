using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Syncfusion.PdfExport;
using System.IO;
using System.Linq;
using System.Reflection;
using Syncfusion.Blazor.SplitButtons;
using Syncfusion.Blazor.Internal;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Syncfusion.Blazor.DataVizCommon;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    internal static class StockChartConstants
    {
        internal const string STOCKCHARTINTROP = "sfBlazor.StockChart.";
        internal const string INITIALIZE = STOCKCHARTINTROP + "initialize";
        internal const string RENDERTOOLTIP = STOCKCHARTINTROP + "renderTooltip";
        internal const string REMOVETOOLTIP = STOCKCHARTINTROP + "fadeOut";
        internal const string SETATTRIBUTE = STOCKCHARTINTROP + "setAttribute";
        internal const string STOCKEVENTSTOOLTIP = "_StockEvents_Tooltip";
        internal const string INITAILELEMENTBOUNDSBYID = "sfBlazor.getElementBoundsById";
        internal const string GETELEMENTBOUNDSBYID = STOCKCHARTINTROP + "getElementBoundsById";
        internal const string GETPARENTELEMENTBOUNDSBYID = STOCKCHARTINTROP + "getParentElementBoundsById";
    }

    public partial class SfStockChart : SfDataBoundComponent
    {
        internal static void InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                var eventHandler = (Action<T>)eventFn;
                eventHandler.Invoke(eventArgs);
            }
        }

        private static RangeValueType GetRangeType(ValueType valueType)
        {
            switch (valueType)
            {
                case ValueType.DateTime:
                    return RangeValueType.DateTime;
                case ValueType.Double:
                    return RangeValueType.Double;
                case ValueType.Logarithmic:
                    return RangeValueType.Logarithmic;
            }

            return RangeValueType.Double;
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered stock chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered stock chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Export(ExportType type, string fileName, PdfPageOrientation? orientation = null)
        {
            await ExportAsync(type, fileName, orientation);
        }

        /// <summary>
        /// The method is used to perform the export functionality for the rendered stock chart.
        /// </summary>
        /// <param name="type">Specifies the export type for the rendered stock chart.</param>
        /// <param name="fileName">Specifies the filename.</param>
        /// <param name="orientation">Specifies the portrait or landscape orientation of the page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ExportAsync(ExportType type, string fileName, PdfPageOrientation? orientation = null)
        {
            if (type != Syncfusion.Blazor.Charts.ExportType.PDF)
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
        /// The method is used to perform the print functionality in stock chart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Print()
        {
            await PrintAsync();
        }

        /// <summary>
        /// The method is used to perform the print functionality in stock chart.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PrintAsync()
        {
            PrintEventArgs args = new PrintEventArgs();
            await SfBaseUtils.InvokeEvent(Events?.OnPrintComplete, args);
            if (!args.Cancel)
            {
                await InvokeMethod("sfExport.print", new object[] { Element });
            }
        }

        private async Task ExportToPdf(string fileName, PdfPageOrientation orientation)
        {
            string base64String = await InvokeMethod<string>("sfExport.exportToImage", false, new object[] { "PNG", fileName, ID + "_svg", false });
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
                            await InvokeMethod("sfExport.downloadPdf", new object[] { base64String, fileName });
                            base64String = string.Empty;
                            document.Dispose();
                        }
                    }
                }
            }
        }

        internal async Task InvokePrintAsync()
        {
            await Print();
        }

        internal async Task ExportStockChartAsync(MenuEventArgs args)
        {
            await Export((Blazor.Charts.ExportType)Enum.Parse(typeof(Blazor.Charts.ExportType), args.Item.Text), "StockChart" + DateTime.Now.Ticks.ToString(Culture));
        }

        private async void UpdateData()
        {
            foreach (StockChartSeries chartSeries in Series)
            {
                chartSeries.CurrentViewData = await chartSeries.UpdatedSeriesData();
            }
        }

        internal void OnChartLoaded(LoadedEventArgs args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (rangeFound)
            {
                return;
            }

            InvokeEvent(Events.OnLoaded, new StockChartEventArgs { Name = "OnLoaded" });
            ShouldStockChartRender = false;
            if (Annotations.Count > 0)
            {
                ChartSettings.ProcessOnLayoutChange();
            }
            StockEventsRender?.UpdateRenderer();
        }

        private double GetStart()
        {
            return StartValue.GetType() == typeof(DateTime) ? ChartHelper.GetTime((DateTime)StartValue) : (double)StartValue;
        }

        private double GetEnd()
        {
            return EndValue.GetType() == typeof(DateTime) ? ChartHelper.GetTime((DateTime)EndValue) : (double)EndValue;
        }

        private static double GetDouble(object data)
        {
            return data.GetType() == typeof(DateTime) ? ChartHelper.GetTime((DateTime)data) : (double)data;
        }

        internal void OnSeriesRender(SeriesRenderEventArgs args)
        {
            if (StartValue != null && EndValue != null)
            {
                Type firstDataType = args.Data.ToList().First().GetType();
                string dataType = DataVizCommonHelper.FindDataType(firstDataType);
                double localStart = GetStart(),
                localEnd = GetEnd();
                if (localStart != localEnd)
                {
                    args.Data = args.Data.Where(data => IsDataWithInRange(data, firstDataType, dataType, args.Series.XName, localStart, localEnd)).ToList();
                }
            }
        }

        private bool IsDataWithInRange(object data, Type firstDataType, string dataType, string x_Name, double localStart, double localEnd)
        {
            object x;
            double xValue;
            switch (dataType)
            {
                case Constants.JOBJECT:
                    JObject jsonObject = (JObject)data;
                    x = jsonObject.GetValue(x_Name, StringComparison.Ordinal);
                    break;
                case Constants.EXPANDOOBJECT:
                    IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                    expandoData.TryGetValue(x_Name, out x);
                    break;
                case Constants.DYNAMICOBJECT:
                    x = ChartHelper.GetDynamicMember(data, x_Name);
                    break;
                default:
                    x = firstDataType.GetProperty(x_Name).GetValue(data);
                    break;
            }
            xValue = PrimaryXAxis.ValueType == ValueType.DateTime ? ChartHelper.GetTime(Convert.ToDateTime(x, null)) : Convert.ToDouble(x, null);
            return ( xValue>= localStart && xValue <= localEnd);
        }

        private void OnRangeChange(ChangedEventArgs args)
        {
            InvokeEvent(Events.RangeChange, new StockChartRangeChangeEventArgs
            {
                Data = new object { },
                SelectedData = args.SelectedData,
                End = args.End,
                Name = "RangeChange",
                Start = args.Start,
                ZoomFactor = args.ZoomFactor,
                ZoomPosition = args.ZoomPosition
            });
            if (GetDouble(args.Start) != GetDouble(args.End))
            {
                StartValue = args.Start;
                EndValue = args.End;
            }
            if (PeriodSelector?.SelectorItems != null)
            {
#pragma warning disable BL0005
                PeriodSelector.SelectorItems.RangePicker.StartDate = (DateTime)args.Start;
                PeriodSelector.SelectorItems.RangePicker.EndDate = (DateTime)args.End;
#pragma warning restore BL0005
            }
        }

        internal void OnPointClick(PointEventArgs args)
        {
            InvokeEvent(Events.OnPointClick, new StockChartPointEventArgs
            {
                Name = "OnPointClick",
                PageX = args.PageX,
                PageY = args.PageY,
                Point = args.Point,
                PointIndex = args.PointIndex,
                SeriesIndex = args.SeriesIndex,
                X = args.X,
                Y = args.Y
            });
        }

        internal void OnZooming(ZoomingEventArgs args)
        {
            List<StockChartAxisData> axisCollection = new List<StockChartAxisData>();
            foreach (AxisData axis in args.AxisCollection)
            {
                axisCollection.Add(new StockChartAxisData()
                {
                    AxisName = axis.AxisName,
                    AxisRange = axis.AxisRange,
                    ZoomFactor = axis.ZoomFactor,
                    ZoomPosition = axis.ZoomPosition,
                });
            }

            InvokeEvent(Events.OnZooming, new StockChartZoomingEventArgs
            {
                AxisCollection = axisCollection,
                Name = "OnZooming"
            });
        }

        internal void OnRangeSelectorLoaded()
        {
            rangeSelectorResize = false;
            RangeSelectorSettings?.InvokeThumbTooltip();
        }

        internal void OnPeriodSelectorLoaded()
        {
            periodSelectorResize = false;
        }
    }
}