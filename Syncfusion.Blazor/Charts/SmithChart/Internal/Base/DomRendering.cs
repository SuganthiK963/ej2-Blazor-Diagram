using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.SmithChart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSmithChart : SfDataBoundComponent
    {
        private bool shouldUpdateDatalabelTemplate;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal SvgRendering Rendering { get; set; }

        internal SmithChartLegend SmithChartLegendModule { get; set; }

        internal SmithChartThemeStyle SmithChartThemeStyle { get; set; }

        internal AxisRenderer AxisModule { get; set; }

        internal SeriesRenderer SeriesModule { get; set; }

        internal SmithChartDataLabel DataLabelModule { get; set; }

        internal SmithChartTooltip TooltipModule { get; set; }

        internal bool IsTooltipEnabled { get; set; }

        internal bool IsDataLabelEnabled { get; set; }

        private void InstanceInitialization()
        {
            SmithChartSeries series;
            Rendering?.PathElementList?.Clear();
            Rendering = new SvgRendering();
            AxisModule = new AxisRenderer(this);
            SeriesModule = new SeriesRenderer(this);
            series = VisibleSeries.Find(series => series.Tooltip.Visible);
            IsTooltipEnabled = series != null ? series.Tooltip.Visible : false;
            series = VisibleSeries.Find(series => series.Marker.DataLabel.Visible);
            IsDataLabelEnabled = series != null ? series.Marker.DataLabel.Visible : false;
            if (IsDataLabelEnabled)
            {
                DataLabelModule = new SmithChartDataLabel(this);
            }

            if (IsTooltipEnabled)
            {
                TooltipModule = new SmithChartTooltip(this);
            }
        }

        internal void ClearSvgElements()
        {
            Rendering.PathElementList.Clear();
            Rendering.TextElementList.Clear();
            Rendering.EllipseElementList.Clear();
            Rendering.RectElementList.Clear();
        }

        internal async void CreateChart()
        {
            ClearSvgElements();
            ChartContent = RenderElements();
            RenderSecondaryElements();
            if (IsTooltipEnabled)
            {
                TooltipContent = RenderTooltipElements();
            }

            await InvokeAsync(StateHasChanged);
            await UpdateDatalabelTemplate();
        }

        private RenderFragment RenderSecondaryElements() => builder =>
        {
            builder.AddContent(SvgRendering.Seq++, DatalabelTemplate);
        };

        private async Task UpdateDatalabelTemplate()
        {
            if (shouldUpdateDatalabelTemplate)
            {
                SmithChartPoint points;
                Point region;
                double left, top;
                DatalabelTemplate = RenderTemplateDatalabels();
                await InvokeAsync(StateHasChanged);
                DomRect templateSize = new DomRect();
                foreach (SmithChartSeries series in VisibleSeries)
                {
                    if (series.Visible && series.Marker.DataLabel.Visible)
                    {
                        for (int j = 0; j < series.ActualPoints.Count; j++)
                        {
                            points = series.ActualPoints[j];
                            for (int i = 0; i < points.TemplateID.Count; i++)
                            {
                                templateSize = await InvokeMethod<DomRect>(SmithChartConstants.GETELEMENTBOUNDSBYID, false, new object[] { points.TemplateID[i] });
                                points.TemplateSize.Add(new Size(templateSize.Width, templateSize.Height));
                            }

                            region = SeriesModule?.PointsRegion.Count > 0 ? SeriesModule?.PointsRegion[series.Index][j].Point : new Point(0, 0);
                            left = region.X - (templateSize.Width / 2);
                            top = region.Y - (templateSize.Height / 2);
                            series.LabelOption.TextOptions[j] = new DataLabelTextOptions
                            {
                                Id = ID + "_Series_" + series.Index.ToString(culture) + "_DataLabelPoint_" + j + "_Label_" + j,
                                X = left,
                                Y = top,
                                Fill = "black",
                                Text = string.Empty,
                                Font = series.Marker.DataLabel.TextStyle,
                                XPosition = left,
                                YPosition = top,
                                Width = templateSize.Width,
                                Height = templateSize.Height,
                                Visible = true,
                                Location = new Point(region.X, region.Y),
                                LabelOptions = new SmithChartLabelPosition { TextX = left, TextY = top, X = left, Y = top }
                            };
                        }
                    }
                }

                DatalabelTemplate = RenderTemplateDatalabels();
                await InvokeAsync(StateHasChanged);
            }
        }

        private RenderFragment RenderTooltipElements() => builder =>
        {
            builder.OpenComponent<TooltipComponent>(SvgRendering.Seq++);
            Dictionary<string, object> tooltipDivAttributes = new Dictionary<string, object>
            {
                { "ID", ID + "_smithchart_tooltip_div" },
                { "Class", "ejSVGTooltip" },
                { "Style", "pointer-events:none; position:absolute;z-index: 1" }
            };
            builder.AddMultipleAttributes(SvgRendering.Seq++, tooltipDivAttributes);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { TooltipModule.TemplateTooltip = (TooltipComponent)ins; });
            builder.CloseComponent();
        };

        private async Task PerformAnimation()
        {
            if (VisibleSeries?.Count > 0 && AnimateSeries && AvailableSize != null)
            {
                AnimateSeries = false;
                foreach (SmithChartSeries series in VisibleSeries)
                {
                    if (series.Visible && series.EnableAnimation)
                    {
                        AnimateSeries = await InvokeMethod<bool>(SmithChartConstants.DOLINEARANIMATION, false, new object[] { ID + "_ChartSeriesClipRect_" + series.Index + "_Rect", series.AnimationDuration, renderType == RenderType.Impedance });
                    }
                }
            }
        }

        private RenderFragment RenderTemplateDatalabels() => builder =>
        {
            foreach (SmithChartSeries series in VisibleSeries)
            {
                if (series.Visible && series.Marker.DataLabel.Visible && series.Marker.DataLabel.Template != null)
                {
                    string seriesClipPath = "url(#" + ID + SmithChartConstants.SERIESCLIPRECTID + series.Index + ")";
                    builder.OpenElement(SvgRendering.Seq++, "div");
                    builder.AddAttribute(SvgRendering.Seq++, "id", ID + "_DataLabel_Collections");
                    builder.AddAttribute(SvgRendering.Seq++, "style", "clip-path:" + seriesClipPath + "; -webkit-clip-path:" + seriesClipPath + "; ");
                    DataLabelModule.Render(builder, series, series.Marker.DataLabel);
                    builder.CloseElement();
                }
            }
        };

        private RenderFragment RenderElements() => treebuilder =>
        {
            ClearSvgElements();
            treebuilder.OpenElement(SvgRendering.Seq++, "svg");
            Dictionary<string, object> svgattributes = new Dictionary<string, object>
            {
                { "height", AvailableSize.Height },
                { "width", AvailableSize.Width },
                { "id", ID + "_svg" },
                { "style", "cursor: default" }
            };
            treebuilder.AddMultipleAttributes(SvgRendering.Seq++, svgattributes);
            RenderChartBorder(treebuilder);
            RenderTitle(treebuilder);
            AxisModule?.RenderArea(treebuilder);
            RenderSeriesCollection(treebuilder);
            RenderLegend(treebuilder);
            treebuilder.CloseElement();
        };

        private void RenderChartBorder(RenderTreeBuilder builder)
        {
            Rendering.RenderRect(builder, ID + SmithChartConstants.BORDERID, 0, 0, AvailableSize.Width, AvailableSize.Height, Border.Width, string.IsNullOrEmpty(Border.Color) ? "transparent" : Border.Color, string.IsNullOrEmpty(Background) ? SmithChartThemeStyle.Background : Background);
        }

        private void RenderLegend(RenderTreeBuilder builder)
        {
            if (LegendSettings.Visible && SmithChartLegendModule.LegendSeries.Count > 0)
            {
                SmithChartLegendModule.RenderLegend(builder);
            }
        }

        private void RenderTitle(RenderTreeBuilder builder)
        {
            if (Title.Visible && !string.IsNullOrEmpty(Title.Text))
            {
                double maxTitleWidth = double.IsNaN(Title.MaximumWidth) ? Math.Abs(Margin.Left + Margin.Right - AvailableSize.Width) : Title.MaximumWidth, x, y;
                Size textSize = SmithChartHelper.MeasureText(Title.Text, Title.TextStyle);
                if (Title.EnableTrim && textSize.Width > maxTitleWidth)
                {
                    titleText = SmithChartHelper.TextTrim(maxTitleWidth, Title.Text, Title.TextStyle);
                    textSize = SmithChartHelper.MeasureText(titleText, Title.TextStyle);
                }

                if (textSize.Width > AvailableSize.Width)
                {
                    x = Margin.Left + Border.Width;
                }
                else
                {
                    x = Title.TextAlignment == SmithChartAlignment.Center ? (AvailableSize.Width / 2) - (textSize.Width / 2) :
                          (Title.TextAlignment == SmithChartAlignment.Near ? (Margin.Left + ElementSpacing + Border.Width) : (AvailableSize.Width
                              - textSize.Width - (Margin.Right + ElementSpacing + Border.Width)));
                }

                y = Margin.Top + (textSize.Height / 2) + ElementSpacing;
                TitleRenderEventArgs titleEventArgs = new TitleRenderEventArgs()
                {
                    X = x,
                    Y = y,
                    Text = titleText,
                    EventName = "TitleRender"
                };
                InvokeEvent<TitleRenderEventArgs>(SmithChartEvents?.TitleRendering, titleEventArgs);
                TextOptions titleOptions = new TextOptions()
                {
                    X = Convert.ToString(titleEventArgs.X, culture),
                    Y = Convert.ToString(titleEventArgs.Y, culture),
                    Text = titleEventArgs.Text,
                    TextAnchor = "start",
                    Id = ID + "_title",
                    FontFamily = Title.TextStyle.FontFamily,
                    FontSize = Title.TextStyle.Size,
                    FontStyle = Title.TextStyle.FontStyle,
                    FontWeight = Title.TextStyle.FontWeight,
                    AccessibilityText = !string.IsNullOrEmpty(Title.Description) ? Title.Description : titleEventArgs.Text,
                    Fill = DataVizCommonHelper.FindThemeColor(Title.TextStyle.Color, SmithChartThemeStyle.ChartTitle)
                };
                if (!titleEventArgs.Cancel)
                {
                    Rendering.RenderText(builder, titleOptions);
                }

                if (Title.Subtitle.Visible && !string.IsNullOrEmpty(Title.Subtitle.Text))
                {
                    RenderSubtitle(builder, x, y, textSize);
                }
            }
        }

        private void RenderSubtitle(RenderTreeBuilder builder, double titleXLocation, double titleYLocation, Size titleSize)
        {
            SmithChartSubtitle subTitle = Title.Subtitle;
            SmithChartAlignment alignment = subTitle.TextAlignment;
            string subTitleText = subTitle.Text;
            Size subTitleSize = SmithChartHelper.MeasureText(subTitleText, subTitle.TextStyle);
            double maxSubTitleWidth = double.IsNaN(subTitle.MaximumWidth) ? (Bounds.Width * 0.75) : subTitle.MaximumWidth;
            if (subTitle.EnableTrim && subTitleSize.Width > maxSubTitleWidth)
            {
                subTitleText = SmithChartHelper.TextTrim(maxSubTitleWidth, subTitleText, subTitle.TextStyle);
            }

            SubTitleRenderEventArgs subTitleEventArgs = new SubTitleRenderEventArgs()
            {
                X = alignment == SmithChartAlignment.Far ? (titleXLocation + titleSize.Width - (ElementSpacing / 2)) :
                (alignment == SmithChartAlignment.Near) ? titleXLocation : (titleXLocation + (titleSize.Width / 2)),
                Y = titleYLocation + (2 * ElementSpacing),
                Text = subTitleText,
                EventName = "SubTitleRendering"
            };
            InvokeEvent<SubTitleRenderEventArgs>(SmithChartEvents?.SubtitleRendering, subTitleEventArgs);
            TextOptions subTitleOptions = new TextOptions()
            {
                X = Convert.ToString(subTitleEventArgs.X, culture),
                Y = Convert.ToString(subTitleEventArgs.Y, culture),
                Text = subTitleEventArgs.Text,
                TextAnchor = alignment == SmithChartAlignment.Near ? "start" : alignment == SmithChartAlignment.Far ? "end" : "middle",
                Id = ID + "_subTitle",
                FontSize = subTitle.TextStyle.Size,
                AccessibilityText = !string.IsNullOrEmpty(subTitle.Description) ? subTitle.Description : subTitleEventArgs.Text,
                Fill = DataVizCommonHelper.FindThemeColor(subTitle.TextStyle.Color, SmithChartThemeStyle.ChartTitle)
            };
            if (!subTitleEventArgs.Cancel)
            {
                Rendering.RenderText(builder, subTitleOptions);
            }
        }

        private void RenderSeriesCollection(RenderTreeBuilder builder)
        {
            builder.OpenElement(SvgRendering.Seq++, "g");
            builder.AddAttribute(SvgRendering.Seq++, "id", ID + "_SeriesCollection");
            SeriesModule.RenderSeries(builder);
            builder.CloseElement();
            Rendering.OpenGroupElement(builder, ID + "_DataLabelCollection");
            foreach (SmithChartSeries series in VisibleSeries)
            {
                if (series.Visible && series.Marker.DataLabel.Visible && series.Marker.DataLabel.Template != null)
                {
                    shouldUpdateDatalabelTemplate = true;
                }
            }

            builder.CloseElement();
        }
    }
}