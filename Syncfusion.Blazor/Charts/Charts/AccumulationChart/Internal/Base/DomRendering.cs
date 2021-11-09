using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Microsoft.JSInterop;
using System.Globalization;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        internal SvgRendering Rendering { get; set; }

        internal AccumulationBase AccBaseModule { get; set; }

        internal PieSeries PieSeriesModule { get; set; }

        internal FunnelSeries FunnelSeriesModule { get; set; }

        internal PyramidSeries PyramidSeriesModule { get; set; }

        internal DataLabelModule DataLabelModule { get; set; }

        internal AccumulationChartLegend AccumulationLegendModule { get; set; }

        internal AccumulationChartSelection AccumulationSelectionModule { get; set; }

        internal ChartThemeStyle AccChartThemeStyle { get; set; }

        internal AccumulationChartTooltip AccumulationTooltipModule { get; set; }

        internal SvgPrintExport PrintExportModule { get; set; }

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        internal string SvgId { get; set; } = string.Empty;

        private void InstanceInitialization()
        {
            Rendering?.PathElementList?.Clear();
            Rendering = new SvgRendering();
            AccBaseModule = new AccumulationBase(this);
            AccumulationChartSeries series = VisibleSeries[0];
            if (series != null)
            {
                if (series.Type == AccumulationType.Funnel)
                {
                    FunnelSeriesModule = new FunnelSeries(this);
                }
                else if (series.Type == AccumulationType.Pyramid)
                {
                    PyramidSeriesModule = new PyramidSeries(this);
                }
                else
                {
                    PieSeriesModule = new PieSeries(this);
                }

                if (series.DataLabel.Visible)
                {
                    DataLabelModule = new DataLabelModule(this);
                }
            }

            PrintExportModule = new SvgPrintExport(JSRuntime);
            if (SelectionMode != AccumulationSelectionMode.None || HighlightMode != AccumulationSelectionMode.None)
            {
                AccumulationSelectionModule = new AccumulationChartSelection(this);
            }

            if (Tooltip.Enable)
            {
                AccumulationTooltipModule = new AccumulationChartTooltip(this);
            }

            InitSeries();
        }

        internal void CreateChart()
        {
            ChartContent = RenderElements();
            AnnotationContent = RenderAnnotationElement();
            TrimTooltip = RenderTrimTooltip();
            DatalabelTemplate = RenderTemplateDatalabels();
            if (Tooltip.Enable)
            {
                TooltipContent = RenderTooltipElements();
            }

            if (AccumulationSelectionModule != null)
            {
                int count = 0;
                StyleFragment = (builder) =>
                {
                    builder.OpenComponent<StyleElement>(count++);
                    builder.AddComponentReferenceCapture(count++, ins => { StyleElementInstance = (StyleElement)ins; });
                    builder.CloseComponent();
                };
            }

            InvokeAsync(StateHasChanged);
        }

        private void DoAfterRender()
        {
            if (Series[0].DataLabel.Template != null)
            {
                datalabelTemplate?.RenderTemplate(this);
            }

            if (Annotations.Count != 0)
            {
                annotationRender?.RenderAnnotations(this);
            }

            ProcessExplode();
            ProcessSelection();
            if (AccType == AccumulationType.Pie && Series[0].Animation.Enable && animateSeries)
            {
                DoAnimateSeries();
            }

            if (SelectedDataIndexes.Count != 0 || AccumulationSelectionModule?.SelectedDataIndexes.Count != 0)
            {
                AccumulationSelectionModule?.ProcessSelectedData();
            }

            if (EnableAnimation && LegendClickRedraw)
            {
                if (AccType == AccumulationType.Pie)
                {
#pragma warning disable CA2012
                    JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, AccumulationChartConstants.PIECHANGEPATH, new object[]
                    {
                           PieSeriesModule.LegendAnimateOptions,
                           PieSeriesModule.CenterLoc,
                           legendChangeDuration != 0 ? legendChangeDuration : 300,
                    });
                }

                foreach (DynamicAccTextAnimationOptions options in AnimateTextElements)
                {
                    if (!double.IsNaN(options.PreLocationX) && !double.IsNaN(options.PreLocationY))
                    {
                        JSRuntimeExtensions.InvokeVoidAsync(JSRuntime, AccumulationChartConstants.ANIMATEREDRAWELEMENT, new object[] { options.Id, 300, options.PreLocationX, options.PreLocationY, options.CurLocationX, options.CurLocationY, options.X, options.Y });
                    }
                }
            }

            TriggerLoadedEvent();
        }

        private async void DoAnimateSeries()
        {
            animateSeries = false;
            await InvokeMethod(AccumulationChartConstants.PIEANIMATE, 
                new object[]
                {
                    ID + "_Series_" + "0_slice",
                    Series[0].StartAngle,
                    PieSeriesModule.TotalAngle,
                    Series[0].Animation.Duration,
                    Series[0].Animation.Delay,
                    0,
                    Math.Max(AvailableSize.Height, AvailableSize.Width) * 0.75,
                    PieSeriesModule.CenterLoc
                });
        }
#pragma warning restore CA2012

        private void TriggerLoadedEvent()
        {
            AccumulationLoadedEventArgs argsData = new AccumulationLoadedEventArgs
            {
                Chart = this,
                Name = Constants.LOADED
            };
            InvokeEvent<AccumulationLoadedEventArgs>(AccumulationChartEvents?.Loaded, argsData);
        }

        internal static void InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                Action<T> eventHandler = (Action<T>)eventFn;
                eventHandler.Invoke(eventArgs);
            }
        }

        internal void ClearSvgElements()
        {
            Rendering.PathElementList.Clear();
            Rendering.TextElementList.Clear();
            Rendering.EllipseElementList.Clear();
            Rendering.RectElementList.Clear();
        }

        private RenderFragment RenderAnnotationElement() => builder =>
        {
            builder.OpenComponent<AnnotationElements>(SvgRendering.Seq++);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { annotationRender = (AnnotationElements)ins; });
            builder.CloseComponent();
        };

        private RenderFragment RenderTrimTooltip() => builder =>
        {
            builder.OpenComponent<TrimTooltipBase>(SvgRendering.Seq++);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { TooltipBase = (TrimTooltipBase)ins; });
            builder.CloseComponent();
        };

        private RenderFragment RenderTemplateDatalabels() => builder =>
        {
            builder.OpenComponent<TemplateDataLabel>(SvgRendering.Seq++);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { datalabelTemplate = (TemplateDataLabel)ins; });
            builder.CloseComponent();
        };

        private RenderFragment RenderTooltipElements() => builder =>
        {
            builder.OpenComponent<ChartTooltipComponent>(SvgRendering.Seq++);
            Dictionary<string, object> tooltipDivAttributes = new Dictionary<string, object>
            {
                { "ID", ID + "_tooltip" },
                { "Class", "ejSVGTooltip" },
                { "Style", "pointer-events:none; position:absolute;z-index: 1" }
            };
            builder.AddMultipleAttributes(SvgRendering.Seq++, tooltipDivAttributes);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { AccumulationTooltipModule.TemplateTooltip = (ChartTooltipComponent)ins; });
            builder.CloseComponent();
        };

        internal void InitSeries()
        {
            foreach (AccumulationChartSeries accSeries in VisibleSeries)
            {
                if (accSeries.Visible)
                {
                    if (accSeries.Type == AccumulationType.Pie)
                    {
                        PieSeriesModule.InitProperties(this, accSeries);
                    }
                    else if (accSeries.Type == AccumulationType.Funnel)
                    {
                        FunnelSeriesModule.InitProperties(this, accSeries);
                    }
                    else
                    {
                        PyramidSeriesModule.InitProperties(this, accSeries);
                    }
                }
            }
        }

        private RenderFragment RenderElements() => treebuilder =>
        {
            treebuilder.OpenElement(SvgRendering.Seq++, "svg");
            Dictionary<string, object> svgattributes = new Dictionary<string, object> {
                { "height", AvailableSize.Height.ToString(CultureInfo.InvariantCulture) },
                { "width", AvailableSize.Width.ToString(CultureInfo.InvariantCulture) },
                {"id", SvgId },
                { "style", "cursor: default" }
            };
            treebuilder.AddMultipleAttributes(SvgRendering.Seq++, svgattributes);
            RenderChartBorder(treebuilder);
            RenderSeriesCollection(treebuilder);
            RenderTitle(treebuilder);
            RenderLegend(treebuilder);
            if (AccumulationSelectionModule != null)
            {
                BaseSelection.CreateStylePatternComponent(treebuilder, ID);
            }

            treebuilder.CloseElement();
            shouldRender = false;
        };

        private void ProcessExplode()
        {
            if (!VisibleSeries[0].Explode)
            {
                return;
            }

            AccBaseModule?.InvokeExplode();
        }

        private void ProcessSelection()
        {
            if (AccumulationSelectionModule != null && (SelectionMode != AccumulationSelectionMode.None || HighlightMode != AccumulationSelectionMode.None))
            {
                AccumulationSelectionModule.InvokeSelection();
            }
        }

        private void RenderChartBorder(RenderTreeBuilder builder)
        {
            Rendering.RenderRect(builder, ID + Constants.BORDERID, 0, 0, AvailableSize.Width, AvailableSize.Height, Border.Width, string.IsNullOrEmpty(Border.Color) ? "transparent" : Border.Color,
                string.IsNullOrEmpty(Background) ? AccChartThemeStyle.Background : Background);
        }

        private void RenderTitle(RenderTreeBuilder builder)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                Rect rect = new Rect(Margin.Left, 0, AvailableSize.Width - Margin.Left - Margin.Right, 0);
                TextOptions TitleOptions = new TextOptions()
                {
                    X = Convert.ToString(ChartHelper.TitlePositionX(rect, TitleStyle.TextAlignment), culture),
                    Y = Convert.ToString(Margin.Top + (ChartHelper.MeasureText(Title, GetFontOptions(TitleStyle)).Height * 0.75), culture),
                    Text = titleCollection[0],
                    TextAnchor = ChartHelper.GetTextAnchor(TitleStyle.TextAlignment, EnableRTL),
                    Id = ID + "_title",
                    FontFamily = TitleStyle.FontFamily,
                    FontSize = TitleStyle.Size,
                    FontStyle = TitleStyle.FontStyle,
                    FontWeight = TitleStyle.FontWeight,
                    Fill = ChartHelper.FindThemeColor(TitleStyle.Color, AccChartThemeStyle.ChartTitle)
                };
                Rendering.RenderText(builder, TitleOptions);
                if (!string.IsNullOrEmpty(SubTitle))
                {
                    RenderSubTitle(builder, TitleOptions);
                }
            }
        }

        private void RenderSubTitle(RenderTreeBuilder builder, TextOptions options)
        {
            double maxWidth = 0, titleWidth = 0;
            foreach (string TitleText in titleCollection)
            {
                titleWidth = ChartHelper.MeasureText(TitleText, GetFontOptions(TitleStyle)).Width;
                maxWidth = titleWidth > maxWidth ? titleWidth : maxWidth;
            }

            Rect rect = new Rect(TitleStyle.TextAlignment == Alignment.Center ? (Convert.ToDouble(options.X, culture) - (maxWidth * 0.5)) : TitleStyle.TextAlignment == Alignment.Far ? Convert.ToDouble(options.X, culture) - maxWidth : Convert.ToDouble(options.X, culture), 0, maxWidth, 0);
            TextOptions SubTitleOptions = new TextOptions()
            {
                X = Convert.ToString(ChartHelper.TitlePositionX(rect, SubTitleStyle.TextAlignment), culture),
                Y = Convert.ToString(Convert.ToDouble(options.Y, null) + (ChartHelper.MeasureText(SubTitle, GetFontOptions(SubTitleStyle)).Height * 3 / 4) + 10, culture),
                Text = subTitleCollection[0],
                TextAnchor = ChartHelper.GetTextAnchor(SubTitleStyle.TextAlignment, EnableRTL),
                Id = ID + "_subTitle",
                Fill = ChartHelper.FindThemeColor(SubTitleStyle.Color, AccChartThemeStyle.ChartTitle)
            };
            Rendering.RenderText(builder, SubTitleOptions);
        }

        private void RenderSeriesCollection(RenderTreeBuilder builder)
        {
            builder.OpenElement(SvgRendering.Seq++, "g");
            builder.AddAttribute(SvgRendering.Seq++, "id", ID + "_SeriesCollection");
            RenderSeries(builder);
            builder.CloseElement();
        }

        private void RenderSeries(RenderTreeBuilder treebuilder)
        {
            foreach (AccumulationChartSeries AccSeries in VisibleSeries)
            {
                if (AccSeries.Visible)
                {
                    string grpId = ID + "_Series_" + "0";
                    treebuilder.OpenElement(SvgRendering.Seq++, "g");
                    treebuilder.AddAttribute(SvgRendering.Seq++, "id", grpId);
                    if (AccSeries.Animation.Enable && AccSeries.Type == AccumulationType.Pie && animateSeries)
                    {
                        treebuilder.AddAttribute(SvgRendering.Seq++, "style", "clip-path:url(#" + grpId + "_clippath" + "); -webkit-clip-path:url(#" + grpId + "_clippath" + ");");
                    }

                    RenderPoints(treebuilder, AccSeries);
                    if (AccSeries.DataLabel.Visible)
                    {
                        InitiateDataLabelProcess(treebuilder, AccSeries);
                    }

                    if (AccType == AccumulationType.Pie)
                    {
                        PieSeriesModule.DefaultLabelBound(AccSeries, AccSeries.DataLabel.Visible, AccSeries.DataLabel.Position);
                        AccSeries.FindMaxBounds(AccSeries.LabelBound, AccSeries.AccumulationBound);
                        if (!LegendClickRedraw && animateSeries)
                        {
                            PieSeriesModule.AnimateSeries(treebuilder);
                        }
                    }

                    if (AccumulationLegendModule != null && LegendSettings.Visible)
                    {
                        AccSeries.LabelBound.X -= ExplodeDistance;
                        AccSeries.LabelBound.Y -= ExplodeDistance;
                        AccSeries.LabelBound.Height += ExplodeDistance - AccSeries.LabelBound.Y;
                        AccSeries.LabelBound.Width += ExplodeDistance - AccSeries.LabelBound.X;
                    }

                    treebuilder.CloseElement();
                }
            }
        }

        private void RenderLegend(RenderTreeBuilder builder)
        {
            if (AccumulationLegendModule != null && LegendSettings.Visible)
            {
                Rect legendBounds = AccumulationLegendModule.LegendBounds;
                if (AccumulationLegendModule.LegendCollection.Count != 0)
                {
                    if (VisibleSeries[0].Type == AccumulationType.Pie)
                    {
                        AccumulationLegendModule.GetSmartLegendLocation(VisibleSeries[0].LabelBound, legendBounds, Margin);
                    }

                    AccumulationLegendModule.RenderLegend(builder, Rendering, LegendSettings, legendBounds, LegendSettings.Border, LegendSettings.TextStyle, null, VisibleSeries);
                }
            }
        }

        private void InitiateDataLabelProcess(RenderTreeBuilder treebuilder, AccumulationChartSeries series)
        {
            string labelsVisibility;
            treebuilder.OpenElement(SvgRendering.Seq++, "g");
            treebuilder.AddAttribute(SvgRendering.Seq++, "id", ID + "_datalabel_Series_" + "0");
            if (series.Type == AccumulationType.Pie)
            {
                labelsVisibility = !animateSeries ? "visible" : "hidden";
                treebuilder.AddAttribute(SvgRendering.Seq++, "style", "visibility:" + labelsVisibility);
            }

            series.ProcessingDataLabels(treebuilder);
            treebuilder.CloseElement();
        }

        private void RenderPoints(RenderTreeBuilder treeBuilder, AccumulationChartSeries series)
        {
            foreach (AccumulationPoints point in series.Points)
            {
                AccumulationPointRenderEventArgs argsData = point.AccPointArgs;
                point.Id = ID + "_Series_0_Point_" + point.Index;
                PathOptions option = new PathOptions()
                {
                    Id = point.Id,
                    Stroke = ChartHelper.FindThemeColor(
                        argsData == null ? point.Color : argsData.Border.Color,
                        argsData == null ? point.Color : argsData.Fill),
                    StrokeWidth = argsData == null ? 0 : argsData.Border.Width,
                    Fill = argsData == null ? point.Color : argsData.Fill,
                    Opacity = series.Opacity
                };
                if (series.Type == AccumulationType.Pie)
                {
                    PieSeriesModule.RenderPoint(point, series, option, treeBuilder);
                }
                else if (series.Type == AccumulationType.Funnel)
                {
                    FunnelSeriesModule.RenderPoint(point, series, option, treeBuilder);
                }
                else
                {
                    PyramidSeriesModule.RenderPoint(point, series, option, treeBuilder);
                }
            }
        }
    }
}