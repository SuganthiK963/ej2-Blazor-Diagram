using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ZoomToolkit : OwningComponentBase
    {
        private bool shouldRender;

        [CascadingParameter]
        private SfChart chart { get; set; }

        private string selectionColor { get; set; }

        private string fillColor { get; set; }

        private string elementOpacity { get; set; }

        private string elementId { get; set; }

        private string zoomInElements { get; set; }

        private string zoomOutElements { get; set; }

        private string zoomElements { get; set; }

        private string panElements { get; set; }

        private Rect iconRect { get; set; }

        private string hoveredID { get; set; }

        private string selectedID { get; set; }

        private string iconRectOverFill { get; set; } = Constants.TRANSPARENT;

        private string iconRectSelectionFill { get; set; } = Constants.TRANSPARENT;

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        private string zoomingKitCollection { get; set; }

        private double zoomkitOpacity { get; set; }

        internal bool Visible { get; set; }

        protected override void OnInitialized()
        {
            elementId = chart.ID;
            selectionColor = chart.Theme == Theme.Bootstrap4 ? "#FFFFFF" : (chart.Theme == Theme.Tailwind || chart.Theme == Theme.Bootstrap5 || chart.Theme == Theme.Bootstrap5Dark || chart.Theme == Theme.TailwindDark) ? "#374151" : "#ff4081";
            fillColor = chart.Theme == Theme.Bootstrap4 ? "#495057" : "#737373";
            iconRectOverFill = chart.Theme == Theme.Bootstrap4 ? "#5A6268" : iconRectOverFill;
            iconRectSelectionFill = chart.Theme == Theme.Bootstrap4 ? "#5B6269" : iconRectSelectionFill;
            iconRect = chart.Theme == Theme.Bootstrap4 ? new Rect(-5, -5, 26, 26) : new Rect(0, 0, 16, 16);
            zoomkitOpacity = 0.3;

        }

        private bool IsDevice()
        {
            return chart.ZoomingModule.IsDevice();
        }

        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        internal void InvalidateRenderer()
        {
            StateHasChanged();
        }

        internal void ShowZoomingKit()
        {
            Visible = shouldRender = true;
            InvalidateRenderer();
        }

        internal void HideZoomingKit()
        {
            Visible = false;
            shouldRender = true;
            InvalidateRenderer();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            shouldRender = false;
            if (Visible)
            {
                ShowZoomingToolkit(builder);
            }
        }

        private void ShowZoomingToolkit(RenderTreeBuilder builder)
        {
            List<ToolbarItems> toolboxItems = chart.ZoomSettings.ToolbarItems;
            Rect areaBounds = chart.AxisContainer.AxisLayout.SeriesClipRect;
            int length = IsDevice() ? 1 : toolboxItems.Count;
            Size size = ChartHelper.MeasureText("Reset Zoom", new ChartFontOptions { Size = "12px" });
            double iconSize = IsDevice() ? size.Width : 16, height = IsDevice() ? size.Height : 22, width = (length * iconSize) + ((length + 1) * 5) + ((length - 1) * 5),
            transX = areaBounds.X + areaBounds.Width - width - 5, transY = areaBounds.Y + 5;
            SvgRendering renderer = chart.SvgRenderer;
            zoomingKitCollection = elementId + "_Zooming_KitCollection";
            if (length == 0 || !renderer.GroupCollection.Find(item => item.Id == zoomingKitCollection).Equals(new ElementReference()))
            {
                return;
            }

            toolboxItems = IsDevice() ? new List<ToolbarItems>() { ToolbarItems.Reset } : toolboxItems;
            RectOptions rectOptions = new RectOptions(elementId + "_Zooming_Rect", 0, 0, width, height + 10, 1, Constants.TRANSPARENT, "#fafafa", 0, 0, 1);
            RenderZoomKit(builder, transX, transY, rectOptions, length, toolboxItems, iconSize);
        }

        private void RenderZoomKit(RenderTreeBuilder builder, double transX, double transY, RectOptions rectOptions, int length, List<ToolbarItems> toolboxItems, double iconSize)
        {
            SvgRendering renderer = chart.SvgRenderer;
            MarkupString shadowElement = (MarkupString)((MarkupString)"<filter id='chart_shadow' height='130%'><feGaussianBlur in='SourceAlpha' stdDeviation='5'/>" + "<feOffset dx='-3' dy='4' result='offsetblur'/><feComponentTransfer><feFuncA type='linear' slope='1'/>" + "</feComponentTransfer><feMerge><feMergeNode/><feMergeNode in='SourceGraphic'/></feMerge></filter>");
            builder.OpenElement(SvgRendering.Seq++, "g");
            builder.AddAttribute(SvgRendering.Seq++, "id", zoomingKitCollection);
            builder.AddAttribute(SvgRendering.Seq++, "transform", "translate(" + transX.ToString(culture) + ',' + transY.ToString(culture) + ')');
            builder.AddAttribute(SvgRendering.Seq++, "opacity", IsDevice() ? 1 : zoomkitOpacity);
            builder.AddAttribute(SvgRendering.Seq++, "cursor", "auto");
            if (!IsDevice())
            {
                builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEMOVE, EventCallback.Factory.Create<MouseEventArgs>(this, ZoomToolkitMove));
                builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHSTART, EventCallback.Factory.Create<TouchEventArgs>(this, ZoomToolkitMove));
                builder.AddAttribute(SvgRendering.Seq++, Constants.ONPOINTERMOVE, EventCallback.Factory.Create<PointerEventArgs>(this, ZoomToolkitMove));
                builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOUT, EventCallback.Factory.Create<MouseEventArgs>(this, ZoomToolkitLeave));
                builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHEND, EventCallback.Factory.Create<TouchEventArgs>(this, ZoomToolkitLeave));
                builder.AddAttribute(SvgRendering.Seq++, Constants.ONPOINTEROUT, EventCallback.Factory.Create<PointerEventArgs>(this, ZoomToolkitLeave));
                if (chart.ZoomingModule.IsPanning)
                {
                    Pan();
                }
            }

            builder.AddElementReferenceCapture(SvgRendering.Seq++, ins => { renderer.GroupCollection.Add((ElementReference)ins); });
            builder.OpenElement(SvgRendering.Seq++, "defs");
            builder.AddMarkupContent(SvgRendering.Seq++, shadowElement.ToString());
            builder.CloseElement();
            renderer.RenderRect(builder, rectOptions);
            rectOptions.Opacity = 0.1;
            rectOptions.Filter = "url(#chart_shadow)";
            renderer.RenderRect(builder, rectOptions);
            double x_Position = 5;
            double space = IsDevice() ? 5 : 8;
            for (int i = 1; i <= length; i++)
            {
                builder.OpenElement(SvgRendering.Seq++, "g");
                builder.AddAttribute(SvgRendering.Seq++, "transform", "translate(" + x_Position.ToString(culture) + ',' + space.ToString(culture) + ')');
                switch (toolboxItems[i - 1])
                {
                    case ToolbarItems.Pan: CreatePanButton(builder); break;
                    case ToolbarItems.Zoom: CreateZoomButton(builder, chart); break;
                    case ToolbarItems.ZoomIn: CreateZoomInButton(builder, chart); break;
                    case ToolbarItems.ZoomOut: CreateZoomOutButton(builder, chart); break;
                    case ToolbarItems.Reset: CreateResetButton(builder, IsDevice()); break;
                }

                x_Position += iconSize + 10;
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        private void ZoomToolkitLeave()
        {
            zoomkitOpacity = 0.3;
            SetAttribute(zoomingKitCollection, "opacity", zoomkitOpacity.ToString(culture));
        }

        private void ZoomToolkitMove()
        {
            zoomkitOpacity = 1;
            SetAttribute(zoomingKitCollection, "opacity", zoomkitOpacity.ToString(culture));
        }

        internal void CreatePanButton(RenderTreeBuilder builder)
        {
            SvgRendering render = chart.SvgRenderer;
            string fillColor = chart.ZoomingModule.IsPanning ? selectionColor : this.fillColor;
            string direction = "M5,3h2.3L7.275,5.875h1.4L8.65,3H11L8,0L5,3z M3,11V8.7l2.875,0.025v-1.4L3,7.35V5L0,8L3,11z M11,13H8.7l0.025-2.875h-1.4L7.35,13H5l3,3L11,13z M13,5v2.3l-2.875-0.025v1.4L13,8.65V11l3-3L13,5z";
            panElements = elementId + "_Zooming_Pan";
            builder.AddAttribute(SvgRendering.Seq++, "id", elementId + "_Zooming_Pan");
            builder.AddAttribute(SvgRendering.Seq++, "aria-label", chart.GetLocalizedLabel("Chart_Pan"));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEDOWN, EventCallback.Factory.Create<MouseEventArgs>(this, Pan));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHSTART, EventCallback.Factory.Create<TouchEventArgs>(this, Pan));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOVER, EventCallback.Factory.Create<MouseEventArgs>(this, ShowPanTooltip));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOUT, EventCallback.Factory.Create<MouseEventArgs>(this, RemoveTooltip));
            render.RenderRect(builder, new RectOptions(elementId + "_Zooming_Pan_1", iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height, 0, Constants.TRANSPARENT, Constants.TRANSPARENT, 0, 0, 1));
            render.RenderPath(builder, new PathOptions(elementId + "_Zooming_Pan_2", direction, null, 0, Constants.TRANSPARENT, 1, fillColor));
        }

        internal void CreateZoomButton(RenderTreeBuilder builder, SfChart chart)
        {
            SvgRendering render = chart.SvgRenderer;
            string fillColor = chart.ZoomingModule.IsPanning ? this.fillColor : selectionColor;
            string rectColor = chart.ZoomingModule.IsPanning ? Constants.TRANSPARENT : iconRectSelectionFill;
            string direction = "M0.001,14.629L1.372,16l4.571-4.571v-0.685l0.228-0.274c1.051,0.868,2.423,1.417,3.885,1.417c3.291,0,5.943-2.651,5.943-5.943S13.395,0,10.103,0S4.16,2.651,4.16,5.943c0,1.508,0.503,2.834,1.417,3.885l-0.274,0.228H4.571L0.001,14.629L0.001,14.629z M5.943,5.943c0-2.285,1.828-4.114,4.114-4.114s4.114,1.828,4.114,";
            zoomElements = elementId + "_Zooming_Zoom";
            builder.AddAttribute(SvgRendering.Seq++, "id", elementId + "_Zooming_Zoom");
            builder.AddAttribute(SvgRendering.Seq++, "aria-label", this.chart.GetLocalizedLabel("Chart_Zoom"));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEDOWN, EventCallback.Factory.Create<MouseEventArgs>(this, Zoom));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHSTART, EventCallback.Factory.Create<TouchEventArgs>(this, Zoom));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOVER, EventCallback.Factory.Create<MouseEventArgs>(this, ShowZoomTooltip));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOUT, EventCallback.Factory.Create<MouseEventArgs>(this, RemoveTooltip));
            selectedID = this.chart.ZoomingModule.IsPanning ? this.chart.ID + "_Zooming_Pan_1" : elementId + "_Zooming_Zoom_1";
            render.RenderRect(builder, new RectOptions(elementId + "_Zooming_Zoom_1", iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height, 0, Constants.TRANSPARENT, rectColor, 0, 0, 1));
            render.RenderPath(builder, new PathOptions(elementId + "_Zooming_Zoom_3", direction + "4.114s-1.828,4.114-4.114,4.114S5.943,8.229,5.943,5.943z", null, 0, Constants.TRANSPARENT, 1, fillColor));
        }

        internal void CreateZoomInButton(RenderTreeBuilder builder, SfChart chart)
        {
            SvgRendering render = chart.SvgRenderer;
            string direction = "M10.103,0C6.812,0,4.16,2.651,4.16,5.943c0,1.509,0.503,2.834,1.417,3.885l-0.274,0.229H4.571L0,14.628l0,0L1.372,16l4.571-4.572v-0.685l0.228-0.275c1.052,0.868,2.423,1.417,3.885,1.417c3.291,0,5.943-2.651,5.943-5.943C16,2.651,13.395,0,10.103,0z M10.058,10.058c-2.286,0-4.114-1.828-4.114-4.114c0-2.286,1.828-4.114,4.114-4.114c2.286,0,4.114,1.828,4.114,4.114C14.172,8.229,12.344,10.058,10.058,10.058z";
            string zoomingZoomIn = elementId + "_Zooming_ZoomIn";
            string polygonDirection = "12.749,5.466 10.749,5.466 10.749,3.466 9.749,3.466 9.749,5.466 7.749,5.466 7.749,6.466 9.749,6.466 9.749,8.466 10.749,8.466 10.749,6.466 12.749,6.466";
            zoomInElements = zoomingZoomIn;
            builder.AddAttribute(SvgRendering.Seq++, "id", zoomingZoomIn);
            builder.AddAttribute(SvgRendering.Seq++, "aria-label", this.chart.GetLocalizedLabel("Chart_ZoomIn"));
            elementOpacity = chart.ZoomingModule.IsPanning ? "0.2" : "1";
            builder.AddAttribute(SvgRendering.Seq++, "opacity", elementOpacity);
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEDOWN, EventCallback.Factory.Create<MouseEventArgs>(this, ZoomIn));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHSTART, EventCallback.Factory.Create<TouchEventArgs>(this, ZoomIn));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOVER, EventCallback.Factory.Create<MouseEventArgs>(this, ShowZoomInTooltip));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOUT, EventCallback.Factory.Create<MouseEventArgs>(this, RemoveTooltip));
            render.RenderRect(builder, new RectOptions(zoomingZoomIn + "_1", iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height, 0, Constants.TRANSPARENT, Constants.TRANSPARENT, 0, 0, 1));
            render.RenderPath(builder, new PathOptions(zoomingZoomIn + "_2", direction, null, 0, Constants.TRANSPARENT, 1, fillColor));
            render.RenderPolygon(builder, zoomingZoomIn + "_3", fillColor, polygonDirection);
        }

        internal void CreateZoomOutButton(RenderTreeBuilder builder, SfChart chart)
        {
            SvgRendering render = chart.SvgRenderer;
            string fillColor = this.fillColor;
            string direction = "M0,14.622L1.378,16l4.533-4.533v-0.711l0.266-0.266c1.022,0.889,2.4,1.422,3.866,1.422c3.289,0,5.955-2.666,5.955-5.955S13.333,0,10.044,0S4.089,2.667,4.134,5.911c0,1.466,0.533,2.844,1.422,3.866l-0.266,0.266H4.578L0,14.622L0,14.622z M5.911,5.911c0-2.311,1.822-4.133,4.133-4.133s4.133,1.822,4.133,4.133s-1.866,4.133-4.133,4.133S5.911,8.222,5.911,5.911z M12.567,6.466h-5v-1h5V6.466z";
            string zooming_ZoomOut = elementId + "_Zooming_ZoomOut";
            zoomOutElements = zooming_ZoomOut;
            builder.AddAttribute(SvgRendering.Seq++, "id", zooming_ZoomOut);
            builder.AddAttribute(SvgRendering.Seq++, "aria-label", this.chart.GetLocalizedLabel("Chart_ZoomOut"));
            elementOpacity = chart.ZoomingModule.IsPanning ? "0.2" : "1";
            builder.AddAttribute(SvgRendering.Seq++, "opacity", elementOpacity);
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEDOWN, EventCallback.Factory.Create<MouseEventArgs>(this, ZoomOut));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHSTART, EventCallback.Factory.Create<TouchEventArgs>(this, ZoomOut));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOVER, EventCallback.Factory.Create<MouseEventArgs>(this, ShowZoomOutTooltip));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOUT, EventCallback.Factory.Create<MouseEventArgs>(this, RemoveTooltip));
            render.RenderRect(builder, new RectOptions(zooming_ZoomOut + "_1", iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height, 0, Constants.TRANSPARENT, Constants.TRANSPARENT, 0, 0, 1));
            render.RenderPath(builder, new PathOptions(zooming_ZoomOut + "_2", direction, null, 0, Constants.TRANSPARENT, 1, fillColor));
        }

        private void ZoomInOutCalculation(int scale, SfChart chart, List<IChartElementRenderer> axes, ZoomMode mode)
        {
            if (!chart.ZoomingModule.IsPanning && elementOpacity != "0.2")
            {
                ZoomingEventArgs args = chart.ZoomingModule.TriggerZoomingEvent(chart.ChartEvents?.OnZoomStart, Constants.ONZOOMSTART);
                if (args.Cancel)
                {
                    return;
                }

                chart.DisableTrackTooltip = chart.DelayRedraw = true;
                List<AxisData> zoomedAxisCollection = new List<AxisData>();
                foreach (ChartAxisRenderer axisRenderer in axes)
                {
                    ChartAxis axis = axisRenderer.Axis;
                    if ((axisRenderer.Orientation == Orientation.Horizontal && mode != ZoomMode.Y) || (axisRenderer.Orientation == Orientation.Vertical && mode != ZoomMode.X))
                    {
                        double cumulative = Math.Max(Math.Max(1 / ChartHelper.MinMax(axis.ZoomFactor, 0, 1), 1) + (0.25 * scale), 1),
                        zoomFactor = cumulative == 1 ? 1 : ChartHelper.MinMax(1 / cumulative, 0, 1),
                        zoomPosition = cumulative == 1 ? 0 : axis.ZoomPosition + ((axis.ZoomFactor - zoomFactor) * 0.5);
                        if (axis.ZoomPosition != zoomPosition || axis.ZoomFactor != zoomFactor)
                        {
                            zoomFactor = (zoomPosition + zoomFactor) > 1 ? (1 - zoomPosition) : zoomFactor;
                        }

                        zoomedAxisCollection.Add(new AxisData
                        {
                            ZoomFactor = zoomFactor,
                            ZoomPosition = zoomPosition,
                            AxisName = axis.GetName(),
                            AxisRange = ChartHelper.GetVisibleRangeModel(axisRenderer.VisibleRange, axisRenderer.VisibleInterval)
                        });
                    }
                    else
                    {
                        zoomedAxisCollection.Add(new AxisData
                        {
                            ZoomFactor = axis.ZoomFactor,
                            ZoomPosition = axis.ZoomPosition,
                            AxisName = axis.GetName(),
                            AxisRange = ChartHelper.GetVisibleRangeModel(axisRenderer.VisibleRange, axisRenderer.VisibleInterval)
                        });
                    }
                }

                ZoomingEventArgs zoomingEventArgs = new ZoomingEventArgs
                {
                    AxisCollection = zoomedAxisCollection,
                    Name = Constants.ONZOOMING
                };
                SfChart.InvokeEvent<ZoomingEventArgs>(chart.ChartEvents?.OnZooming, zoomingEventArgs);
                if (!zoomingEventArgs.Cancel)
                {
                    for (int i = 0; i < zoomingEventArgs.AxisCollection.Count; i++)
                    {
                        ChartAxisRenderer axisRenderer = (ChartAxisRenderer)axes[i];
                        ChartAxis axis = axisRenderer.Axis;
                        axis.UpdateZoomValues(zoomingEventArgs.AxisCollection[i].ZoomFactor, zoomingEventArgs.AxisCollection[i].ZoomPosition);
                    }
                }
            }
        }

        internal void Reset()
        {
            SfChart chart = this.chart;
            if (!chart.ZoomingModule.IsDevice())
            {
                chart.ZoomingToolkitContent.HideZoomingKit();
            }

            RemoveTooltip();
            chart.SetSvgCursor("auto");
            List<AxisData> zoomedAxisCollection = new List<AxisData>();
            chart.AxisContainer.IsScrollExist = false;
            foreach (ChartAxisRenderer axisRenderer in chart.AxisContainer.Renderers)
            {
                ChartAxis axis = axisRenderer.Axis;
                axis.UpdateZoomValues(1, 0);
                if (axisRenderer.ZoomingScrollBar != null)
                {
                    axisRenderer.ZoomingScrollBar.IsScrollUI = false;
                }

                axis.UpdateZoomValues(axis.ZoomFactor, axis.ZoomPosition);
                zoomedAxisCollection.Add(new AxisData
                {
                    ZoomFactor = axis.ZoomFactor,
                    ZoomPosition = axis.ZoomFactor,
                    AxisName = axis.GetName(),
                    AxisRange = ChartHelper.GetVisibleRangeModel(axisRenderer.VisibleRange, axisRenderer.VisibleInterval)
                });
            }

            ZoomingEventArgs zoomingEventArgs = new ZoomingEventArgs
            {
                AxisCollection = zoomedAxisCollection,
                Name = Constants.ONZOOMING
            };
            if (!zoomingEventArgs.Cancel)
            {
                SfChart.InvokeEvent<ZoomingEventArgs>(chart.ChartEvents?.OnZoomEnd, zoomingEventArgs);
                if (!zoomingEventArgs.Cancel)
                {
                    SetDeferredZoom(chart);
                }
            }
            else
            {
                SetDeferredZoom(chart);
            }
        }

        private void ShowTooltip(string currentTarget, string text, MouseEventArgs args)
        {
            text = chart.GetLocalizedLabel(text);
#pragma warning disable CA2000
            double left = args.ClientX - (ChartHelper.MeasureText(text, new ChartFontOptions { Size = "10px" }).Width + 5);
            string rect = currentTarget + "_1";
            hoveredID = rect;
            SetAttribute(rect, "fill", iconRectOverFill);
            SetAttribute(currentTarget + "_2", "fill", selectionColor);
            SetAttribute(currentTarget + "_3", "fill", selectionColor);
            if (!chart.IsTouch)
            {
                CreateTooltip(Constants.EJ2_CHART_ZOOMTIP, text, args.ClientY + 10, left, "10px");
            }
        }

        internal void CreateResetButton(RenderTreeBuilder builder, bool isDevice)
        {
            SvgRendering render = chart.SvgRenderer;
            string direction = "M12.364,8h-2.182l2.909,3.25L16,8h-2.182c0-3.575-2.618-6.5-5.818-6.5c-1.128,0-2.218,0.366-3.091,1.016l1.055,1.178C6.581,3.328,7.272,3.125,8,3.125C10.4,3.125,12.363,5.319,12.364,8L12.364,8z M11.091,13.484l-1.055-1.178C9.419,12.672,8.728,12.875,8,12.875c-2.4,0-4.364-2.194-4.364-4.875h2.182L2.909,4.75L0,8h2.182c0,3.575,2.618,6.5,5.818,6.5C9.128,14.5,10.219,14.134,11.091,13.484L11.091,13.484z";
            string zooming_Reset = elementId + "_Zooming_Reset";
            builder.AddAttribute(SvgRendering.Seq++, "id", zooming_Reset);
            builder.AddAttribute(SvgRendering.Seq++, "aria-label", chart.GetLocalizedLabel("Chart_Reset"));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEDOWN, EventCallback.Factory.Create<MouseEventArgs>(this, Reset));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONTOUCHSTART, EventCallback.Factory.Create<TouchEventArgs>(this, Reset));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOVER, EventCallback.Factory.Create<MouseEventArgs>(this, ShowResetTooltip));
            builder.AddAttribute(SvgRendering.Seq++, Constants.ONMOUSEOUT, EventCallback.Factory.Create<MouseEventArgs>(this, RemoveTooltip));
            if (!isDevice)
            {
                render.RenderRect(builder, new RectOptions(zooming_Reset + "_1", iconRect.X, iconRect.Y, iconRect.Width, iconRect.Height, 0, Constants.TRANSPARENT, Constants.TRANSPARENT, 0, 0, 1));
                render.RenderPath(builder, new PathOptions(zooming_Reset + "_2", direction, null, 0, Constants.TRANSPARENT, 1, fillColor));
            }
            else
            {
                Size size = ChartHelper.MeasureText(chart.GetLocalizedLabel("Chart_ResetZoom"), new ChartFontOptions { Size = "12px" });
#pragma warning restore BL0005
#pragma warning restore CA2000
                render.RenderRect(builder, new RectOptions(zooming_Reset + "_1", 0, 0, size.Width, size.Height, 0, Constants.TRANSPARENT, Constants.TRANSPARENT, 0, 0, 1));
                render.RenderText(builder, new TextOptions()
                {
                    Id = zooming_Reset + "_2",
#pragma warning disable CA1305
                    X = (size.Width / 2).ToString(culture),
                    Y = (size.Height * (3 / 4)).ToString(culture),
#pragma warning restore CA1305
                    TextAnchor = "middle",
                    Text = chart.GetLocalizedLabel("Chart_ResetZoom"),
                    Transform = "rotate(0," + 0 + ',' + 0 + ')',
                    DominantBaseline = "auto",
                    FontSize = "12px",
                    Fill = "black"
                });
            }
        }

        private void ShowPanTooltip(MouseEventArgs args)
        {
            ShowTooltip(elementId + "_Zooming_Pan", "Chart_Pan", args);
        }

        private void ShowZoomTooltip(MouseEventArgs args)
        {
            ShowTooltip(elementId + "_Zooming_Zoom", "Chart_Zoom", args);
        }

        private void ShowZoomInTooltip(MouseEventArgs args)
        {
            ShowTooltip(elementId + "_Zooming_ZoomIn", "Chart_ZoomIn", args);
        }

        private void ShowZoomOutTooltip(MouseEventArgs args)
        {
            ShowTooltip(elementId + "_Zooming_ZoomOut", "Chart_ZoomOut", args);
        }

        private void ShowResetTooltip(MouseEventArgs args)
        {
            ShowTooltip(elementId + "_Zooming_Reset", "Chart_Reset", args);
        }

        private async void CreateTooltip(string id, string text, double top, double left, string fontSize)
        {
#pragma warning disable CA2007
            await chart.InvokeMethod(Constants.CREATETOOLTIP, new object[] { id, text, top, left, fontSize });
        }

        private async void ApplySelection(string id, string fill)
        {
            await chart.InvokeMethod(Constants.APPLYSELECTION, new object[] { id, fill });
        }

        private async void RemoveElement(string id)
        {
            await chart.InvokeMethod<bool>(Constants.REMOVEELEMENT, false, new object[] { id });
        }

        private void SetAttribute(string id, string key, string fill)
        {
            chart.SetAttribute(id, key, fill);
        }

        internal void RemoveTooltip()
        {
            if (!string.IsNullOrEmpty(hoveredID))
            {
                SetAttribute(hoveredID, "fill", chart.ZoomingModule.IsPanning ? hoveredID.Contains("_Pan_", StringComparison.InvariantCulture) ? iconRectSelectionFill : Constants.TRANSPARENT : hoveredID.Contains("_Zoom_", StringComparison.InvariantCulture) ? iconRectSelectionFill : Constants.TRANSPARENT);
                SetAttribute(hoveredID.Replace("_1", "_2", StringComparison.InvariantCulture), "fill", chart.ZoomingModule.IsPanning ? hoveredID.Contains("_Pan_", StringComparison.InvariantCulture) ? selectionColor : fillColor : hoveredID.Contains("_Zoom_", StringComparison.InvariantCulture) ? selectionColor : fillColor);
                SetAttribute(hoveredID.Replace("_1", "_3", StringComparison.InvariantCulture), "fill", chart.ZoomingModule.IsPanning ? fillColor : hoveredID.Contains("_Zoom_", StringComparison.InvariantCulture) ? selectionColor : fillColor);
            }

            RemoveElement(Constants.EJ2_CHART_ZOOMTIP);
        }

        private bool SetDeferredZoom(SfChart chart)
        {
            chart.DisableTrackTooltip = false;
            chart.ZoomingModule.IsZoomed = chart.ZoomingModule.IsPanning = chart.IsChartDrag = chart.DelayRedraw = false;
            chart.ZoomingModule.TouchMoveList = chart.ZoomingModule.TouchStartList = new List<Touches>();
            chart.ZoomingModule.PinchTarget = null;
            if (chart.EnableAutoIntervalOnBothAxis)
            {
                chart.SeriesContainer.ProcessData();
            }

            chart.ProcessOnLayoutChange();
            elementOpacity = "1";
            return false;
        }

        private void ZoomIn()
        {
            ZoomInOutCalculation(1, chart, chart.AxisContainer.Renderers, chart.ZoomSettings.Mode);
        }

        private void ZoomOut()
        {
            ZoomInOutCalculation(-1, chart, chart.AxisContainer.Renderers, chart.ZoomSettings.Mode);
        }

        private void Zoom()
        {
            chart.ZoomingModule.IsPanning = false;
            elementOpacity = "1";
            chart.SetSvgCursor("auto");
            SetAttribute(zoomInElements, "opacity", elementOpacity);
            SetAttribute(zoomOutElements, "opacity", elementOpacity);
            ApplySelection(zoomElements, selectionColor);
            ApplySelection(panElements, "#737373");
            if (!string.IsNullOrEmpty(selectedID))
            {
                SetAttribute(selectedID, "fill", Constants.TRANSPARENT);
            }

            selectedID = chart.ID + "_Zooming_Zoom_1";
            SetAttribute(selectedID, "fill", iconRectSelectionFill);
        }

        internal void Pan()
        {
            chart.ZoomingModule.IsPanning = true;
            chart.SetSvgCursor("pointer");
            elementOpacity = "0.2";
            SetAttribute(zoomInElements, "opacity", elementOpacity);
            SetAttribute(zoomOutElements, "opacity", elementOpacity);
            ApplySelection(panElements, selectionColor);
            ApplySelection(zoomElements, "#737373");
            SetAttribute(selectedID, "fill", Constants.TRANSPARENT);
            selectedID = chart.ID + "_Zooming_Pan_1";
            SetAttribute(selectedID, "fill", iconRectSelectionFill);
        }
    }
}
