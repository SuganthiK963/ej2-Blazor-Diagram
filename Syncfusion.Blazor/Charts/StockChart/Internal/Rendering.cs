using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.SplitButtons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Reflection;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfStockChart : SfDataBoundComponent
    {
        private const string RESETID = "_reset";
        private const string PRINTID = "_print";
        private const string CALENDARID = "_calendar";
        private const string SERIES = "Series";
        private const string INDICATORS = "Indicators";
        private const string TRENDLINE = "Trendline";
        private const string RESET = "Reset";
        private const string PRINT = "Print";
        private const string EXPORT = "Export";
        private const string LONGSPACE = "   ";
        private const string TICK = "✔ "; // "&#10004";
        private string selectedSeries = string.Empty;
        private string selectedIndicator = string.Empty;
        private string selectedTrendLine = string.Empty;
        private string trendline;
        private List<TechnicalIndicators> indicators = new List<TechnicalIndicators>();
        private bool isSingleAxis;
        private double currentEnd;
        private bool rangeFound;
        private bool periodSelectorResize { get; set; }
        private bool rangeSelectorResize { get; set; }
        private List<StockChartPeriod> tempPeriods = new List<StockChartPeriod>();
        private StringComparison comparison = StringComparison.InvariantCulture;
        private bool isStockChartRendered;

        internal CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        internal ChartTooltipComponent TemplateTooltip { get; set; }

        internal object StartValue { get; set; }

        internal object EndValue { get; set; }

        internal double SeriesXMin { get; set; }

        internal double SeriesXMax { get; set; }

        internal List<ChartSeriesType> SeriesTypeList { get; set; } = new List<ChartSeriesType>();

        internal RangeIntervalType CurrentInterval { get; set; }

        private static RenderFragment CreateButtonFragment(string id, string content, EventCallback<MouseEventArgs> eventCallback) => builder =>
        {
            builder.OpenComponent<SfButton>(0);
            builder.AddAttribute(1, "Content", content);
            builder.AddAttribute(2, "id", id);
            builder.AddAttribute(3, "OnClick", eventCallback);
            builder.CloseComponent();
        };

        private static object GetPointX(Type type, object currentData, string xName)
        {
            object pointX = 0;
            if (type.Name.Equals(typeof(JObject)))
            {
                pointX = ((JObject)currentData).GetValue(xName, StringComparison.InvariantCulture);
            }
            else if (type.Equals(typeof(ExpandoObject)))
            {
                foreach (KeyValuePair<string, object> property in (IDictionary<string, object>)currentData)
                {
                    if (xName.Equals(property.Key, StringComparison.Ordinal))
                    {
                        pointX = property.Value;
                    }
                }
            }
            else if (type.BaseType.Equals(typeof(DynamicObject)))
            {
                pointX = ChartHelper.GetDynamicMember(currentData, xName);
            }
            else
            {
                pointX = GetMember(type, currentData, xName);
            }

            return pointX;
        }

        private static object GetMember(Type type, object data, string memberName)
        {
            PropertyInfo info = type.GetProperty(memberName);
            return info != null ? info.GetValue(data) : null;
        }

        private RenderFragment RenderTitle() => builder =>
        {
            if (string.IsNullOrEmpty(Title))
            {
                return;
            }

            builder.OpenElement(0, "svg");
            builder.AddMultipleAttributes(1, new Dictionary<string, object>
            {
                { "id", GetId(TITLEID) },
                { "width", Convert.ToString(ChartSize.Width, Culture) },
                { "height", Convert.ToString(TitleSize.Height, Culture) },
                { "fill", GetBackgroundColor() }
            });
            SvgRenderer.RenderRect(builder, new RectOptions
            {
                Fill = GetBackgroundColor(),
                Id = GetId("_title_background"),
                Opacity = 1,
                Width = ChartSize.Width,
                Height = TitleSize.Height,
                X = 0,
                Y = 0
            });
            TextOptions titleOptions = new TextOptions(Convert.ToString(ChartHelper.TitlePositionX(new Rect(0, 0, AvailableSize.Width, 0), TitleStyle.TextAlignment), Culture), Convert.ToString(TitleSize.Height * (3.0 / 4.0), Culture), FindTitleColor(), TitleStyle.GetFontOptions(), Title, TitleStyle.TextAlignment == Alignment.Near ? "start" : TitleStyle.TextAlignment == Alignment.Far ? "end" : "middle", ID + "_ChartTitle", string.Empty, string.Empty, "auto", Title);
            ChartHelper.TextElement(builder, SvgRenderer, titleOptions);
            builder.CloseElement();
        };

        internal void UpdatePeriodEvent()
        {
            if (Events.PeriodChanged != null)
            {
                Events.PeriodChanged.Invoke(new StockChartPeriodChangedEventArgs("PeriodChanged", tempPeriods));
            }
        }

        private string GetBackgroundColor()
        {
            if (!string.IsNullOrEmpty(Background))
            {
                return Background;
            }

            switch (Theme)
            {
                case Theme.HighContrast:
                case Theme.HighContrastLight:
                    return "#000000";
                case Theme.MaterialDark:
                    return "#303030";
                case Theme.BootstrapDark:
                    return "#1A1A1A";
                case Theme.Bootstrap5Dark:
                    return "#212529";
                case Theme.FabricDark:
                    return "#201F1F";
                case Theme.TailwindDark:
                    return "#1F2937";
                default:
                    return "#FFFFFF";
            }
        }

        private string FindTitleColor()
        {
            if (!string.IsNullOrEmpty(TitleStyle.Color))
            {
                return TitleStyle.Color;
            }

            switch (Theme)
            {
                case Theme.HighContrast:
                case Theme.HighContrastLight:
                case Theme.BootstrapDark:
                case Theme.FabricDark:
                case Theme.MaterialDark:
                case Theme.TailwindDark:
                case Theme.Bootstrap5Dark:
                    return "#ffffff";
                default:
                    return "#424242";
            }
        }

        private string GetBorderStyle()
        {
            return "width: " + Convert.ToString(AvailableSize.Width, Culture) + "px; height: " + Convert.ToString(AvailableSize.Height, Culture)
                + "px; position: absolute; border: " + Border.Width + "px solid " + Border.Color + "; pointer-events: none;";
        }

        private RenderFragment RenderTooltipElements() => builder =>
        {
            builder.OpenComponent<ChartTooltipComponent>(SvgRendering.Seq++);
            Dictionary<string, object> tooltipDivAttributes = new Dictionary<string, object>
            {
                { "ID", GetId(StockChartConstants.STOCKEVENTSTOOLTIP) },
                { "Class", "ejSVGTooltip" },
                { "Style", "pointer-events:none; position: relative;z-index: 1; height: 0px; width: 0px;" + "left: " + ElementOffset.Left + "px; top: " + ElementOffset.Top + "px;" }
            };
            builder.AddMultipleAttributes(SvgRendering.Seq++, tooltipDivAttributes);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { TemplateTooltip = (ChartTooltipComponent)ins; });
            builder.CloseComponent();
        };

        /// <summary>
        /// JS interopt while mouse move.
        /// </summary>
        /// <param name="args">Represents the mouse events arguments.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnStockChartMouseMove(ChartInternalMouseEventArgs args)
        {
            if (args != null && args.Target.Contains("StockEvents", comparison))
            {
                StockEventInstance.RenderStockEventTooltip(args.Target);
            }
            else if (StockEventInstance != null)
            {
                StockEventInstance.RemoveStockEventTooltip();
            }
        }

        /// <summary>
        /// JS interopt while mouse leave.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnStockChartMouseLeave()
        {
            StockEventInstance?.RemoveStockEventTooltip();
        }

        /// <summary>
        /// JS interopt while window get resized.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async void OnStockChartResize()
        {
            rangeSelectorResize = periodSelectorResize = true;
            RenderedSelectors.Clear();
            await RenderStockChart(false);
            await InvokeAsync(StateHasChanged);
        }

        internal void OnStockChartPropertyChanged()
        {
            if (!isStockChartRendered)
            {
                return;
            }

            foreach (string property in PropertyChanges?.Keys)
            {
                switch (property)
                {
                    case "Height":
                    case "Width":
                        CalculateAvailableSize();
                        CalculateChartSize();
                        UpdateStockChart();
                        break;
                    case "DataSource":
                        UpdateData();
                        UpdateStockChart();
                        break;
                    case "Tooltip":
                    case "Crosshair":
                        UpdateStockChart();
                        break;
                }
            }
        }

        private List<ToolbarItem> UpdateCustomElement()
        {
            List<ToolbarItem> selector = new List<ToolbarItem>();
            string controlId = ID;
            if (SeriesType.Count > 0)
            {
                selector.Add(new ToolbarItem
                {
#pragma warning disable BL0005
                    Template = CreateDropDownFragment(SERIES, EventCallback.Factory.Create<MenuEventArgs>(this, SeriesItemSelected)),
                    Align = ItemAlign.Left
                });
            }

            if (IndicatorType.Count > 0)
            {
                selector.Add(new ToolbarItem
                {
                    Template = CreateDropDownFragment(INDICATORS, EventCallback.Factory.Create<MenuEventArgs>(this, IndicatorItemSelected)),
                    Align = ItemAlign.Left
                });
            }

            if (TrendlineType.Count > 0)
            {
                selector.Add(new ToolbarItem
                {
                    Template = CreateDropDownFragment(TRENDLINE, EventCallback.Factory.Create<MenuEventArgs>(this, TrendlineItemSelected)),
                    Align = ItemAlign.Left
                });
            }

            return selector;
        }

        private RenderFragment CreateDropDownFragment(string content, EventCallback<MenuEventArgs> eventCallback) => builder =>
        {
            List<DropDownMenuItem> list = GetDropDownList(content);
            builder.OpenComponent<StockChartDropDown>(0);
            builder.AddAttribute(1, "Content", content);
            builder.AddAttribute(2, "Items", list);
            builder.AddAttribute(3, "ItemSelected", eventCallback);
            builder.CloseComponent();
        };

        private void SeriesItemSelected(MenuEventArgs args)
        {
            selectedSeries = args.Item.Text;
            string text = TickMark(args);
            AddSeries(text);
            ChartSettings.ShouldAnimateSeries = false;
            ChartSettings.OnLayoutChange();
            ChartSettings.InvalidateRender();
        }

        private void AddSeries(string seriesType)
        {
            SeriesTypeList.Clear();
            List<StockChartSeries> seriesList = Series;
            foreach (StockChartSeries series in seriesList)
            {
                if (series.YName == "Volume")
                {
                    continue;
                }

                ChartSeriesType currentType = seriesType.Contains("Candle", comparison) ? ChartSeriesType.Candle :
                (seriesType.Contains("OHLC", comparison) ? ChartSeriesType.HiloOpenClose : (ChartSeriesType)Enum.Parse(typeof(ChartSeriesType), seriesType));
                series.Type = currentType;
                SeriesTypeList.Add(currentType);
                series.EnableSolidCandles = seriesType == "Candle";
                foreach (StockChartTrendline trendLine in series.Trendlines)
                {
                    trendLine.Animation.Enable = false;
                    trendLine.EnableTooltip = false;
                }
            }
        }

        private string TickMark(MenuEventArgs args)
        {
            string text;
            List<DropDownMenuItem> items = args.Item.Parent.DropDownButton.Items;
            foreach (DropDownMenuItem item in items)
            {
                item.Text = item.Text.Contains(TICK, comparison) ? item.Text.Substring(item.Text.IndexOf(TICK, comparison) + 1) : item.Text;
                if (!item.Text.Contains(LONGSPACE, comparison))
                {
                    item.Text = LONGSPACE + item.Text;
                }
            }

            if (args.Item.Text.Contains(LONGSPACE, comparison))
            {
                text = args.Item.Text.Replace(LONGSPACE, string.Empty, comparison);
                args.Item.Text = args.Item.Text.Replace(LONGSPACE, TICK, comparison);
            }
            else
            {
                text = args.Item.Text.Replace(TICK, string.Empty, comparison);
            }

            return text;
        }

        private void IndicatorItemSelected(MenuEventArgs args)
        {
            ChartSettings.ShouldAnimateSeries = false;
            List<StockChartSeries> seriesList = Series;
            foreach (StockChartSeries currentSeries in seriesList)
            {
                if (currentSeries.Trendlines.Count != 0)
                {
                    currentSeries.Trendlines[0].Animation.Enable = false;
                }
            }

            args.Item.Text = args.Item.Text.Contains(TICK, comparison) ? args.Item.Text.Substring(args.Item.Text.IndexOf(TICK, comparison) + 1) : args.Item.Text;
            string text = args.Item.Text.Replace(LONGSPACE, string.Empty, comparison);
            TechnicalIndicators type = (TechnicalIndicators)Enum.Parse(typeof(TechnicalIndicators), text);
            selectedIndicator = !selectedIndicator.Contains(text, comparison) ? selectedIndicator + ' ' + type : selectedIndicator.Replace(text, string.Empty, comparison);
            if (type == TechnicalIndicators.Tma || type == TechnicalIndicators.BollingerBands || type == TechnicalIndicators.Sma || type == TechnicalIndicators.Ema)
            {
                if (indicators.IndexOf(type) == -1)
                {
                    args.Item.Text = TICK + args.Item.Text.Replace(LONGSPACE, string.Empty, comparison);
                    List<StockChartIndicator> indicator = GetIndicator(type, Series[0].YAxisName);
                    indicators.Add(type);
                    indicator.ForEach(x => Indicators.Add(x));
                    ChartSettings.InvalidateRender();
                    ChartSettings.IndicatorContainer.Prerender();
                    ChartSettings.OnLayoutChange();
                }
                else
                {
                    args.Item.Text = LONGSPACE + args.Item.Text;
                    for (int z = 0; z < Indicators.Count; z++)
                    {
                        if (Indicators[z].Type == type)
                        {
                            Indicators.RemoveAt(z);
                        }
                    }

                    indicators.Remove(type);
                    ChartSettings.InvalidateRender();
                    ChartSettings.IndicatorContainer.Prerender();
                    ChartSettings.OnLayoutChange();
                }
            }
            else
            {
                CreateIndicatorAxes(type, args);
            }
        }

        private void CreateIndicatorAxes(TechnicalIndicators type, MenuEventArgs args)
        {
            if (indicators.IndexOf(type) == -1)
            {
                args.Item.Text = TICK + args.Item.Text.Replace(LONGSPACE, string.Empty, comparison);
                indicators.Add(type);
                List<StockChartAxis> axis;
                List<StockChartRow> row;
                List<StockChartIndicator> indicator;
                int len = Rows.Count;
                if (len > 0)
                {
                    Rows[len - 1].Height = "15%";
                }

                row = new List<StockChartRow>() { new StockChartRow { Height = string.Empty + (100 - (len * 15)) + "px" } };
                if (Rows.Count == 1)
                {
                    isSingleAxis = true;
                }

                row.ForEach(x => Rows.Add(x));
                if (!isSingleAxis)
                {
                    Axes[0].RowIndex += 1;
                }
                else
                {
                    for (int i = 0; i < Axes.Count; i++)
                    {
                        Axes[i].RowIndex += 1;
                    }
                }

                axis = new List<StockChartAxis>()
                {
                    new StockChartAxis()
                    {
                        PlotOffset = 10, OpposedPosition = true,
                        RowIndex = !isSingleAxis ? Axes.Count : 0,
                        DesiredIntervals = 1,
                        LabelFormat = "n2",
                        MajorGridLines = PrimaryYAxis.MajorGridLines,
                        LineStyle = PrimaryYAxis.LineStyle,
                        LabelPosition = PrimaryYAxis.LabelPosition,
                        MajorTickLines = PrimaryYAxis.MajorTickLines,
                        RangePadding = ChartRangePadding.None,  Name = type.ToString(),
                        RendererKey = type.ToString()
                    }
                };
                PrimaryYAxis.RowIndex = !isSingleAxis ? 0 : len + 1;
                indicator = GetIndicator(type, type.ToString());
                indicator.ForEach(x => Indicators.Add(x));
                axis.ForEach(x => Axes.Add(x));
                ChartSettings.InvalidateRender();
            }
            else
            {
                args.Item.Text = LONGSPACE + args.Item.Text;
                for (int i = 0; i < Indicators.Count; i++)
                {
                    if (Indicators[i].Type == type)
                    {
                        Indicators.RemoveAt(i);
                    }
                }

                indicators.Remove(type);
                int removedIndex = 0;
                for (int z = 0; z < Axes.Count; z++)
                {
                    if (Axes[z].Name == type.ToString())
                    {
                        removedIndex = Convert.ToInt32(Axes[z].RowIndex);
                        Rows.RemoveAt(z);
                        Axes.RemoveAt(z);
                    }
                }

                for (int z = 0; z < Axes.Count; z++)
                {
                    if (Axes[z].RowIndex != 0 && Axes[z].RowIndex > removedIndex)
                    {
                        Axes[z].RowIndex = Axes[z].RowIndex - 1;
                    }
                }
                Rows.Last().Height = (100 - (Rows.Count - 1 * 15)) + "px";

                ChartSettings.InvalidateRender();
            }
        }

        private List<StockChartIndicator> GetIndicator(TechnicalIndicators type, string yaxisname)
        {
            StockChartSeries currentSeries = Series[0];
            List<StockChartIndicator> indicator = new List<StockChartIndicator>()
            {
                new StockChartIndicator()
                {
                    Type = type, Period = 3, YAxisName = yaxisname,
                    DataSource = currentSeries.CurrentViewData,
                    XName = currentSeries.XName,
                    Open = currentSeries.Open,
                    Close = currentSeries.Close,
                    High = currentSeries.High,
                    Low = currentSeries.Low,
                    Volume = currentSeries.Volume,
                    Fill = type == TechnicalIndicators.Sma ? "#32CD32" : "#6063ff",
                    Animation = new StockChartIndicatorAnimation() { Enable = false },
                    UpperLine = new StockChartUpperLine() { Color = "#FFE200", Width = 1 },
                    PeriodLine = new StockChartPeriodLine() { Width = 2 },
                    LowerLine = new StockChartLowerLine() { Color = "#FAA512", Width = 1 },
                    FastPeriod = 8, SlowPeriod = 5, MacdType = MacdType.Both, Width = 1,
                    MacdPositiveColor = "#6EC992", MacdNegativeColor = "#FF817F",
                    BandColor = "rgba(245, 203, 35, 0.12)",
                    RendererKey = type.ToString()
                }
            };
            return indicator;
        }

        private void TrendlineItemSelected(MenuEventArgs args)
        {
            ChartSettings.ShouldAnimateSeries = false;
            string text = TickMark(args);
            TrendlineTypes type = (TrendlineTypes)Enum.Parse(typeof(TrendlineTypes), text);
            selectedTrendLine = string.IsNullOrEmpty(selectedTrendLine) ? type.ToString() : selectedTrendLine + ',' + type;
            if (trendline != type.ToString())
            {
                trendline = type.ToString();
                int index = 0;
                foreach (StockChartSeries series in Series)
                {
                    if (series.YName.ToUpper(Culture) == "VOLUME")
                    {
                        continue;
                    }

                    if (series.Trendlines.Count == 0)
                    {
                        TrendlineTriggered = false;
                        series.Trendlines = new List<StockChartTrendline>() { new StockChartTrendline() { Type = type, Width = 1, EnableTooltip = false, RendererKey = type.ToString() } };
                    }
                    else
                    {
                        List<StockChartTrendline> chartTrendlines = series.Trendlines;
                        StockChartTrendline chartTrendline;
                        for (int i = 0; i < chartTrendlines.Count; i++)
                        {
                            chartTrendline = chartTrendlines[i];
                            series.Trendlines[i] = new StockChartTrendline()
                            {
                                BackwardForecast = chartTrendline.BackwardForecast,
                                EnableTooltip = chartTrendline.EnableTooltip,
                                Fill = chartTrendline.Fill,
                                ForwardForecast = chartTrendline.ForwardForecast,
                                Intercept = chartTrendline.Intercept,
                                LegendShape = chartTrendline.LegendShape,
                                Name = chartTrendline.Name,
                                Period = chartTrendline.Period,
                                PolynomialOrder = chartTrendline.PolynomialOrder,
                                Type = type,
                                Width = chartTrendline.Width != 0 ? chartTrendline.Width : 1,
                                Marker = new StockChartMarkerSettings()
                                {
                                    Fill = chartTrendline.Marker.Fill,
                                    Height = chartTrendline.Marker.Height,
                                    ImageUrl = chartTrendline.Marker.ImageUrl,
                                    Opacity = chartTrendline.Marker.Opacity,
                                    Shape = chartTrendline.Marker.Shape,
                                    Width = chartTrendline.Marker.Width,
                                    Visible = chartTrendline.Marker.Visible
                                },
                                Animation = new AnimationModel() { Enable = TrendlineTriggered },
                                RendererKey = i + "_" + type.ToString()
                            };
                        }
                    }

                    index++;
                }

                ChartSettings.InvalidateRender();
            }
            else
            {
                args.Item.Text = LONGSPACE + args.Item.Text.Replace(TICK, string.Empty, comparison);
                Series[0].Trendlines.Clear();
                trendline = null;
                ChartSettings.InvalidateRender();
            }
        }

        private List<DropDownMenuItem> GetDropDownList(string content)
        {
            ChartSeriesType type;
            List<DropDownMenuItem> list = new List<DropDownMenuItem>();
            switch (content)
            {
                case SERIES:
                    foreach (ChartSeriesType seriesType in SeriesType)
                    {
                        list.Add(new DropDownMenuItem()
                        {
                            Text = LONGSPACE + ((seriesType == ChartSeriesType.HiloOpenClose) ? "OHLC" : seriesType.ToString())
                        });
                    }

                    for (int i = 0; i < Series.Count; i++)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            type = Series[i].Type;
                            if (list[j].Text == LONGSPACE + ((type == ChartSeriesType.HiloOpenClose) ? "OHLC" : type.ToString()))
                            {
                                list[j].Text = list[j].Text.Replace(LONGSPACE, TICK, comparison);
                            }
                        }
                    }

                    break;
                case TRENDLINE:
                    foreach (TrendlineTypes trendType in TrendlineType)
                    {
                        string prefix = trendType.ToString() == trendline ? TICK : LONGSPACE;
                        list.Add(new DropDownMenuItem()
                        {
                            Text = prefix + trendType.ToString()
                        });
                    }

                    break;
                case INDICATORS:
                    foreach (TechnicalIndicators indicatorType in IndicatorType)
                    {
                        list.Add(new DropDownMenuItem()
                        {
                            Text = LONGSPACE + indicatorType.ToString()
                        });
                    }

                    for (int i = 0; i < Indicators.Count; i++)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (list[j].Text == LONGSPACE + Indicators[i].Type.ToString())
                            {
                                list[j].Text = list[j].Text.Replace(LONGSPACE, TICK, comparison);
                            }
                        }
                    }

                    break;
                case EXPORT:
                    List<ExportType> stockChartExport = ExportType;
                    foreach (ExportType exportType in stockChartExport)
                    {
                        list.Add(new DropDownMenuItem()
                        {
                            Text = LONGSPACE + exportType.ToString()
                        });
                    }

                    break;
            }

            return list;
        }

        internal async Task ResetStockChart(MouseEventArgs args)
        {
            double updatedStart = RangeSelectorSettings != null ? RangeSelectorSettings.RangeAxisModule.GetTime(new DateTime(new DateTime(1970, 1, 1).AddMilliseconds(SeriesXMax).Year, 1, 1)) : SeriesXMin,
            updatedEnd = SeriesXMax;
            RangeSlider slider = RangeSelectorSettings?.RangeSliderModule;
            if (slider != null)
            {
                await slider.PerformAnimation(updatedStart, updatedEnd);
            }
            foreach (StockChartIndicator chartIndicator in Indicators)
            {
                StockChartAxis axis = Axes.Find(axis => axis.Name.ToLowerInvariant() == chartIndicator.Type.ToString().ToLowerInvariant());
                if (axis != null)
                {
                    Rows.RemoveAt(axis.RowIndex);
                    Axes.Remove(axis);
                }
            }
            Indicators.Clear();
            SeriesTypeList.Clear();
            selectedIndicator = selectedTrendLine = selectedSeries = trendline = string.Empty;
            indicators = new List<TechnicalIndicators>();
            int count = 0;
            foreach (StockChartSeries chartSeries in Series)
            {
                chartSeries.Type = ChartSeriesType.Candle;
                if (chartSeries.Trendlines.Count > 0)
                {
                    chartSeries.Trendlines.Clear();
                }

                count += 1;
            }
            CurrentInterval = RangeIntervalType.Auto;
            tempPeriods.ForEach(period => { period.Selected = false; });
            tempPeriods.Find(period => period.Text == "YTD").Selected = true;
            rangeSelectorResize = periodSelectorResize = true;
            PeriodSelectorSettings?.CallStateHasChanged();
            UpdateStockChart();
            UpdateChartData(updatedStart, updatedEnd);
            rangeSelectorResize = periodSelectorResize = false;
        }

        internal void UpdateChartData(double updatedStart, double updatedEnd, RangeNavigatorPeriod selectedPeriod = null)
        {
            object[] previousValue = SelectedValue != null ? ((Array)SelectedValue).Cast<object>().ToArray() : null;
            SelectedValue = new object[] { new DateTime(1970, 1, 1).AddMilliseconds(updatedStart), new DateTime(1970, 1, 1).AddMilliseconds(updatedEnd) };
            StartValue = updatedStart;
            EndValue = updatedEnd;
            if (selectedPeriod != null)
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                StockChartPeriod chartPeriod = tempPeriods.Find(period => period.Text == selectedPeriod.Text && !string.IsNullOrEmpty(period.Text)) ?? new StockChartPeriod();
#pragma warning restore CA2000 // Dispose objects before losing scope
                ClearSelectedItem();
                chartPeriod.Selected = true;
                CurrentInterval = selectedPeriod.IntervalType;
            }

            foreach (ChartIndicator indicator in ChartSettings.IndicatorContainer.Elements)
            {
                indicator.DataSource = ((ChartSeries)ChartSettings.SeriesContainer.Elements[0]).CurrentViewData;
            }

            if (previousValue == null || !StartValue.Equals(ChartHelper.GetTime((DateTime)previousValue[0])) || !EndValue.Equals(ChartHelper.GetTime((DateTime)previousValue[1])))
            {
                ChartSettings.ProcessOnLayoutChange();
                UpdateStockChartElements();
            }

            rangeFound = true;
        }
        internal async void UpdateStockChartElements()
        {
            if (StockEventsRender != null)
            {
                await Task.Delay(150);
                StockEventsRender.UpdateRenderer();
            }
            if (Annotations.Count > 0)
            {
                ChartSettings.AnnotationContainer?.ProcessRenderQueue();
            }
        }

        internal void CalculateSeriesXMinMax()
        {
            double pointXValue;
            object pointX;
            Type type;
            object[] currentViewData;
            int length = 0;
            SeriesXMin = double.PositiveInfinity;
            SeriesXMax = double.NegativeInfinity;
            foreach (StockChartSeries series in Series)
            {
                currentViewData = series.CurrentViewData.ToArray();
                length = currentViewData.Length;
                if (length == 0)
                {
                    return;
                }

                type = currentViewData.First().GetType();
                pointX = GetPointX(type, currentViewData[0], series.XName);
                if (!pointX.GetType().Equals(typeof(DateTime)))
                {
                    return;
                }

                for (int i = 0; i < length; i++)
                {
                    pointX = GetPointX(type, currentViewData[i], series.XName);
                    if (!string.IsNullOrEmpty(pointX.ToString()))
                    {
                        pointXValue = ChartHelper.GetTime(Convert.ToDateTime(pointX, null));
                        SeriesXMin = Math.Min(SeriesXMin, pointXValue);
                        SeriesXMax = Math.Max(SeriesXMax, pointXValue);
                    }
                }
            }

            CalculateStartEndValue();
        }

        private void CalculateStartEndValue()
        {
            EndValue = currentEnd = SeriesXMax;
            if (EnablePeriodSelector)
            {
                tempPeriods = tempPeriods.Count > 0 ? tempPeriods : (Periods.Count > 0 ? Periods : CalculateAutoPeriods());
                for (int index = 0; index < tempPeriods.Count; index++)
                {
                    StockChartPeriod period = tempPeriods[index];
#pragma warning disable CA1304
                    if (period.Selected && period.Text.ToLower(Culture).Equals("ytd", comparison))
                    {
                        StartValue = ChartHelper.GetTime(new DateTime(new DateTime(1970, 1, 1).AddMilliseconds(currentEnd).Year, 1, 1));
                    }
                    else if (period.Selected && period.Text.ToLower(Culture).Equals("all", comparison))
#pragma warning restore CA1304
                    {
                        StartValue = SeriesXMin;
                    }
                    else if (period.Selected)
                    {
                        StartValue = ChartHelper.GetTime(PeriodSelector.ChangedRange(period.IntervalType, (double)EndValue, period.Interval));
                    }
                }
            }
            else
            {
                StartValue = SeriesXMin;
            }

            if (EnableSelector)
            {
                DateTime localStart = StartValue.GetType() == typeof(DateTime) ? (DateTime)StartValue : new DateTime(1970, 1, 1).AddMilliseconds((double)StartValue),
                localEnd = EndValue.GetType() == typeof(DateTime) ? (DateTime)EndValue : new DateTime(1970, 1, 1).AddMilliseconds((double)EndValue);
                SelectedValue = new object[] { localStart, localEnd };
            }
        }

        private void ClearSelectedItem()
        {
            tempPeriods.ForEach(x => x.Selected = false);
        }

        internal void UpdateDropdownElement(List<ToolbarItem> rangeToolbarItems)
        {
            string controlId = ID;
            if (EnableCustomRange)
            {
                rangeToolbarItems.Add(new ToolbarItem { Align = ItemAlign.Right, Id = controlId + CALENDARID });
            }

            rangeToolbarItems.Add(new ToolbarItem { Align = ItemAlign.Right, Template = CreateButtonFragment(controlId + PRINTID, PRINT, EventCallback.Factory.Create<MouseEventArgs>(this, InvokePrintAsync)) });
            rangeToolbarItems.Add(new ToolbarItem { Align = ItemAlign.Right, Template = CreateButtonFragment(controlId + RESETID, RESET, EventCallback.Factory.Create<MouseEventArgs>(this, ResetStockChart)) });
            rangeToolbarItems.Add(new ToolbarItem { Align = ItemAlign.Right, Template = CreateDropDownFragment(EXPORT, EventCallback.Factory.Create<MenuEventArgs>(this, ExportStockChartAsync)) });
#pragma warning restore BL0005
        }

        internal SfRangeNavigator GetRangeNavigator()
        {
            return RangeSelectorSettings;
        }
    }
}