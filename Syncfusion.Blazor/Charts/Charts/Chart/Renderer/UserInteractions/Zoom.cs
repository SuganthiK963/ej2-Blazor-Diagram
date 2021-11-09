using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class Zoom
    {
        internal Zoom(SfChart sfChart)
        {
            chart = sfChart;
            Browser = chart.Browser;
            AddEventListener();
            ChartZoomSettings zooming = chart.ZoomSettings;
            this.zooming = zooming;
            elementId = chart.ID;
            zoomingRect = new Rect(0, 0, 0, 0);
            zoomAxes = new List<ZoomAxisRange>();
            IsZoomed = PerformedUI = this.zooming.EnablePan && this.zooming.EnableSelectionZooming;
            if (zooming.EnableScrollbar)
            {
                chart.ScrollElement = chart.AxisContainer.IsScrollExist ? chart.ScrollElement : chart.AxisContainer.CreateScrollbarDiv();
            }
        }

        private SfChart chart { get; set; }

        private ChartZoomSettings zooming { get; set; }

        private string elementId { get; set; }

        private Rect zoomingRect { get; set; }

        private Rect offset { get; set; }

        private List<ZoomAxisRange> zoomAxes { get; set; } = new List<ZoomAxisRange>();

        private bool isZoomStart { get; set; } = true;

        private SvgRect rectEle { get; set; }

        internal ElementReference ToolkitElements { get; set; }

        internal bool IsPanning { get; set; }

        internal bool IsZoomed { get; set; }

        internal bool IsWheelZoom { get; set; }

        internal Browser Browser { get; set; }

        internal string PinchTarget { get; set; }

        internal List<Touches> TouchStartList { get; set; }

        internal List<Touches> TouchMoveList { get; set; }

        internal bool PerformedUI { get; set; }

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        private static void ClearSelectionRect(SfChart chart)
        {
            if (chart.ZoomingContent != null)
            {
                chart.ZoomingContent.UpdateRectSize(new Rect(0, 0, 0, 0));
            }
        }

        internal bool IsPointer()
        {
            return Browser.IsPointer;
        }

        internal bool IsDevice()
        {
            return Browser != null ? (Browser.BrowserName != "msie" && Browser.IsPointer) : chart.SyncfusionService.IsDeviceMode;
        }

        internal string BrowserName()
        {
            return chart.Browser.BrowserName;
        }

        internal bool IsIOS()
        {
            return Browser.IsIos;
        }

        private void AddEventListener()
        {
            chart.MouseMove += MouseMoveHandler;
            chart.MouseDown += MouseDownHandler;
            chart.MouseUp += MouseUpHandler;
            chart.MouseCancel += MouseCancelHandler;
            chart.WheelEvent += ChartMouseWheel;
        }

        private void MouseMoveHandler(object source, ChartInternalMouseEventArgs e)
        {
            List<Touches> touches = new List<Touches>();
            if (e?.Type == "touchmove")
            {
                if (e.PreventDefault && IsIOS() && (IsPanning || chart.IsDoubleTap || (zooming.EnablePinchZooming && TouchStartList.Count > 1)))
                {
                    // e.preventDefault = true;
                }

                touches = e.Touches;
            }

            if (chart.IsChartDrag)
            {
                ZoomingEventArgs args = TriggerZoomingEvent(chart.ChartEvents?.OnZoomStart, Constants.ONZOOMSTART, !isZoomStart);
                if (chart.IsTouch)
                {
                    TouchMoveList = AddTouchPointer(TouchMoveList, e, touches);
                    if (zooming.EnablePinchZooming && TouchMoveList.Count > 1 && TouchStartList.Count > 1)
                    {
                        PerformPinchZooming(chart);
                    }
                }

                if (args == null || !args.Cancel)
                {
                    RenderZooming(e, chart, chart.IsTouch);
                }
            }
        }

        internal ZoomingEventArgs TriggerZoomingEvent(Action<ZoomingEventArgs> zoomingEvent, string eventName, bool preventEvent = false)
        {
            if (preventEvent)
            {
                return null;
            }

            List<AxisData> zoomedAxisCollection = new List<AxisData>();
            foreach (ChartAxisRenderer axisRender in chart.AxisContainer.Renderers)
            {
                ChartAxis axis = axisRender.Axis;
                zoomedAxisCollection.Add(new AxisData
                {
                    ZoomFactor = axis.ZoomFactor,
                    ZoomPosition = axis.ZoomPosition,
                    AxisName = axis.GetName(),
                    AxisRange = ChartHelper.GetVisibleRangeModel(axisRender.VisibleRange, axisRender.VisibleInterval)
                });
            }

            ZoomingEventArgs zoomingEventArgs = new ZoomingEventArgs
            {
                Cancel = false,
                AxisCollection = zoomedAxisCollection,
                Name = eventName
            };
            SfChart.InvokeEvent<ZoomingEventArgs>(zoomingEvent, zoomingEventArgs);
            isZoomStart = false;
            return zoomingEventArgs;
        }

        private void CalculateZoomAxesRange(SfChart chart)
        {
            for (int index = 0; index < chart.AxisContainer.Renderers.Count; index++)
            {
                ChartAxisRenderer axis = (ChartAxisRenderer)chart.AxisContainer.Renderers[index];
                DoubleRange axisRange = axis.VisibleRange;
                if (zoomAxes.Count > index && zoomAxes[index] != null && !chart.DelayRedraw)
                {
                    zoomAxes[index].Min = axisRange.Start;
                    zoomAxes[index].Delta = axisRange.Delta;
                }
                else
                {
                    zoomAxes.Insert(index, new ZoomAxisRange
                    {
                        ActualMin = axis.ActualRange.Start,
                        ActualDelta = axis.ActualRange.Delta,
                        Min = axisRange.Start,
                        Delta = axisRange.Delta
                    });
                }
            }
        }

        private bool PerformPinchZooming(SfChart chart)
        {
            if ((zoomingRect.Width > 0 && zoomingRect.Height > 0) || (chart.StartMove && chart.Crosshair.Enable))
            {
                return false;
            }

            CalculateZoomAxesRange(chart);
            IsZoomed = IsPanning = PerformedUI = true;
            offset = !chart.DelayRedraw ? chart.AxisContainer.AxisLayout.SeriesClipRect : offset;
            chart.DelayRedraw = chart.DisableTrackTooltip = true;
            DomRect elementOffset = chart.ElementOffset;
            List<Touches> touchDown = TouchStartList;
            List<Touches> touchMove = TouchMoveList;
            double touch0StartX = touchDown[0].PageX - elementOffset.Left,
                   touch0StartY = touchDown[0].PageY - elementOffset.Top,
                   touch0EndX = touchMove[0].PageX - elementOffset.Left,
                   touch0EndY = touchMove[0].PageY - elementOffset.Top,
                   touch1StartX = touchDown[1].PageX - elementOffset.Left,
                   touch1StartY = touchDown[1].PageY - elementOffset.Top,
                   touch1EndX = touchMove[1].PageX - elementOffset.Left,
                   touch1EndY = touchMove[1].PageY - elementOffset.Top,
                   scaleX = Math.Abs(touch0EndX - touch1EndX) / Math.Abs(touch0StartX - touch1StartX),
                   scaleY = Math.Abs(touch0EndY - touch1EndY) / Math.Abs(touch0StartY - touch1StartY),
                   clipX = ((offset.X - touch0EndX) / scaleX) + touch0StartX,
                   clipY = ((offset.Y - touch0EndY) / scaleY) + touch0StartY,
                   translateXValue = touch0EndX - (scaleX * touch0StartX),
                   translateYValue = touch0EndY - (scaleY * touch0StartY);
            Rect pinchRect = new Rect(clipX, clipY, offset.Width / scaleX, offset.Height / scaleY);
            if (!double.IsNaN(scaleX - scaleX) && !double.IsNaN(scaleY - scaleY))
            {
                switch (zooming.Mode)
                {
                    case ZoomMode.XY:
                        SetTransform(translateXValue, translateYValue, scaleX, scaleY, chart, true);
                        break;
                    case ZoomMode.X:
                        SetTransform(translateXValue, 0, scaleX, 1, chart, true);
                        break;
                    case ZoomMode.Y:
                        SetTransform(0, translateYValue, 1, scaleY, chart, true);
                        break;
                }
            }

            CalculatePinchZoomFactor(chart, pinchRect);
            RefreshAxis(chart.AxisContainer.AxisLayout, chart, chart.AxisContainer.Renderers);
            return true;
        }

        private void SetTransform(double transX, double transY, double scaleX, double scaleY, SfChart chart, bool isPinch)
        {
#pragma warning disable CA2012
            chart.JSRuntime.InvokeVoidAsync(Constants.SETZOOMINGCIPPATH, new object[] { elementId + "SeriesCollection", elementId + "IndicatorCollection", "url(#" + elementId + "_ChartAreaClipRect_)" });
            bool isScale = !ChartHelper.IsNaNOrZero(scaleX) || !ChartHelper.IsNaNOrZero(scaleY);
            if (!ChartHelper.IsNaNOrZero(transX) && !ChartHelper.IsNaNOrZero(transY))
            {
                foreach (ChartSeries seriesVal in chart.SeriesContainer.Elements)
                {
                    double x_AxisLoc = chart.RequireInvertedAxis ? seriesVal.Renderer.YAxisRenderer.Rect.X : seriesVal.Renderer.XAxisRenderer.Rect.X,
                    y_AxisLoc = chart.RequireInvertedAxis ? seriesVal.Renderer.XAxisRenderer.Rect.Y : seriesVal.Renderer.YAxisRenderer.Rect.Y;
                    string translate = "translate(" + (transX + (isPinch ? (scaleX * x_AxisLoc) : x_AxisLoc)).ToString(culture) + ',' + (transY + (isPinch ? (scaleY * y_AxisLoc) : y_AxisLoc)).ToString(culture) + ')';
                    translate = isScale ? translate + " scale(" + scaleX.ToString(culture) + ' ' + scaleY.ToString(culture) + ')' : translate;
                    if (seriesVal.Visible)
                    {
                        seriesVal.Renderer.UpdateElementRef();
                        chart.JSRuntime.InvokeVoidAsync(Constants.SETZOOMINGELEMENTATTRIBUTES, new object[] { translate, seriesVal.Renderer.Category(), seriesVal.Renderer.SeriesElement, seriesVal.Renderer.ErrorBarElement, seriesVal.Renderer.SymbolElement, seriesVal.Renderer.TextElement, seriesVal.Renderer.ShapeElement, chart.SvgRenderer.GroupCollection.Find(item => item.Id == chart.ID + "_Series_" + seriesVal.Renderer.Index + "_DataLabelCollections") });
                    }
#pragma warning restore CA2012
                }
            }
        }

        private void RenderZooming(ChartInternalMouseEventArgs e, SfChart chart, bool isTouch)
        {
            CalculateZoomAxesRange(chart);
            if (zooming.EnableSelectionZooming && (!isTouch || (chart.IsDoubleTap && TouchStartList.Count == 1)) && (!IsPanning || chart.IsDoubleTap))
            {
                IsPanning = IsDevice() ? true : IsPanning;
                PerformedUI = true;
                DrawZoomingRectangle(chart);
            }
            else if (IsPanning && chart.IsChartDrag && (!isTouch || (isTouch && TouchStartList.Count == 1)))
            {
                PinchTarget = isTouch ? e.Target : null;
                DoPan(chart, chart.AxisContainer.Renderers);
            }
        }

        private void PerformDeferredZoom(SfChart chart)
        {
            double translateX, translateY;
            if (zooming.EnableDeferredZooming)
            {
                translateX = chart.MouseX - chart.MouseDownX;
                translateY = chart.MouseY - chart.MouseDownY;
                switch (zooming.Mode)
                {
                    case ZoomMode.X:
                        translateY = 0;
                        break;
                    case ZoomMode.Y:
                        translateX = 0;
                        break;
                }

                SetTransform(translateX, translateY, 0, 0, chart, false);
                RefreshAxis(chart.AxisContainer.AxisLayout, chart, chart.AxisContainer.Renderers);
            }
            else
            {
                PerformZoomRedraw(chart);
            }

            chart.PreviousMouseMoveX = chart.MouseX;
            chart.PreviousMouseMoveY = chart.MouseY;
        }

        private void DrawZoomingRectangle(SfChart chart)
        {
            Rect areaBounds = chart.AxisContainer.AxisLayout.SeriesClipRect;
            Rect rect = zoomingRect = GetRectLocation(new ChartInternalLocation(chart.PreviousMouseMoveX, chart.PreviousMouseMoveY), new ChartInternalLocation(chart.MouseX, chart.MouseY), areaBounds);
            if (rect.Width > 0 && rect.Height > 0)
            {
                IsZoomed = true;
                chart.DisableTrackTooltip = true;
                chart.SetSvgCursor("crosshair");
                if (zooming.Mode == ZoomMode.X)
                {
                    rect.Height = areaBounds.Height;
                    rect.Y = areaBounds.Y;
                }
                else if (zooming.Mode == ZoomMode.Y)
                {
                    rect.Width = areaBounds.Width;
                    rect.X = areaBounds.X;
                }
                chart.ZoomingToolkitContent.HideZoomingKit();
                chart.ZoomingContent.UpdateRectSize(rect);
            }
        }

        internal RenderFragment RenderZoomingElement(RectOptions options) => builder =>
        {
            chart.SvgRenderer.RenderRect(builder, options);
        };

#pragma warning disable CA1822
        private Rect GetRectLocation(ChartInternalLocation startLocation, ChartInternalLocation endLocation, Rect outerRect)
        {
            double x = endLocation.X < outerRect.X ? outerRect.X : (endLocation.X > (outerRect.X + outerRect.Width)) ? outerRect.X + outerRect.Width : endLocation.X,
            y = endLocation.Y < outerRect.Y ? outerRect.Y : (endLocation.Y > (outerRect.Y + outerRect.Height)) ? outerRect.Y + outerRect.Height : endLocation.Y;
            return new Rect(x > startLocation.X ? startLocation.X : x, y > startLocation.Y ? startLocation.Y : y, Math.Abs(x - startLocation.X), Math.Abs(y - startLocation.Y));
        }

        private void RefreshAxis(AxisLayout layout, SfChart chart, List<IChartElementRenderer> axes)
        {
            ZoomMode mode = chart.ZoomSettings.Mode;
            layout.ComputePlotAreaBounds(new Rect(chart.InitialRect.X, chart.InitialRect.Y, chart.InitialRect.Width, chart.InitialRect.Height));
            for (int index = 0; index < axes.Count; index++)
            {
                ChartAxisRenderer axisRenderer = (ChartAxisRenderer)axes[index];
                if (axisRenderer.Orientation == Orientation.Horizontal && mode != ZoomMode.Y)
                {
                    chart.UpdateRenderers();
                }

                if (axisRenderer.Orientation == Orientation.Vertical && mode != ZoomMode.X)
                {
                    chart.UpdateRenderers();
                }
            }
        }

        private List<Touches> AddTouchPointer(List<Touches> touchList, ChartInternalMouseEventArgs e, List<Touches> touches)
#pragma warning restore CA1822
        {
            if (touches.Count > 0)
            {
                touchList = e.Touches;
            }
            else
            {
                touchList = touchList.Count > 0 ? touchList : new List<Touches>();
                if (touchList.Count == 0)
                {
                    touchList.Add(new Touches { PageX = e.ClientX, PageY = e.ClientY, PointerId = e.PointerId });
                }
                else
                {
                    for (int i = 0, length = touchList.Count; i < length; i++)
                    {
                        if (touchList[i].PointerId == e.PointerId)
                        {
                            touchList[i] = new Touches { PageX = e.ClientX, PageY = e.ClientY, PointerId = e.PointerId };
                        }
                        else
                        {
                            touchList.Add(new Touches { PageX = e.ClientX, PageY = e.ClientY, PointerId = e.PointerId });
                        }
                    }
                }
            }

            return touchList;
        }

        private void MouseDownHandler(object source, ChartInternalMouseEventArgs args)
        {
            if (!args.Target.Contains(chart.ID + Constants.ZOOMID, StringComparison.InvariantCulture) && ChartHelper.WithInBounds(chart.PreviousMouseMoveX, chart.PreviousMouseMoveY, chart.AxisContainer.AxisLayout.SeriesClipRect))
            {
                chart.IsChartDrag = true;
            }

            if (chart.IsTouch)
            {
                TouchStartList = AddTouchPointer(TouchStartList, args, args.Touches);
            }
        }

        private void MouseUpHandler(object source, ChartInternalMouseEventArgs args)
        {
            bool performZoomRedraw = !args.Target.Contains(chart.ID + "_ZoomOut_", StringComparison.InvariantCulture) || args.Target.Contains(chart.ID + "_ZoomIn_", StringComparison.InvariantCulture);
            if (chart.IsChartDrag || performZoomRedraw)
            {
                ClearSelectionRect(chart);
                PerformZoomRedraw(chart);
                TriggerZoomingEvent(chart.ChartEvents?.OnZoomEnd, Constants.ONZOOMEND);
                isZoomStart = true;
            }

            if (chart.IsTouch)
            {
                if (chart.IsDoubleTap && ChartHelper.WithInBounds(chart.MouseX, chart.MouseY, chart.AxisContainer.AxisLayout.SeriesClipRect) && TouchStartList.Count == 1 && IsZoomed)
                {
                    chart.ZoomingToolkitContent.Reset();
                    isZoomStart = true;
                }

                TouchStartList = new List<Touches>();
                chart.IsDoubleTap = false;
            }
        }

        private void PerformZoomRedraw(SfChart chart)
        {
            if (IsZoomed)
            {
                if (zoomingRect.Width > 0 && zoomingRect.Height > 0)
                {
                    PerformedUI = true;
                    DoZoom(chart, chart.AxisContainer.Renderers, chart.AxisContainer.AxisLayout.SeriesClipRect);
                    chart.IsDoubleTap = false;
                    chart.SetSvgCursor("auto");
                }
                else if (chart.DisableTrackTooltip)
                {
                    chart.DisableTrackTooltip = chart.DelayRedraw = false;
                    chart.ProcessOnLayoutChange();
                }
            }
        }

        private void MouseCancelHandler(object source, ChartInternalMouseEventArgs args)
        {
            if (IsZoomed)
            {
                ClearSelectionRect(chart);
                PerformZoomRedraw(chart);
            }

            PinchTarget = null;
            TouchStartList = new List<Touches>();
            TouchMoveList = new List<Touches>();
        }

        private void ChartMouseWheel(object source, ChartMouseWheelArgs args)
        {
#pragma warning disable CA1304
            if (zooming.EnableMouseWheelZooming && ChartHelper.WithInBounds(args.MouseX, args.MouseY, chart.AxisContainer.AxisLayout.SeriesClipRect) && !args.Target.ToLower().Contains("scrollbar", StringComparison.InvariantCulture))
#pragma warning restore CA1304
            {
                PerformMouseWheelZooming(args, args.MouseX, args.MouseY, chart, chart.AxisContainer.Renderers);
            }
        }

        private void DoPan(SfChart chart, List<IChartElementRenderer> axes)
        {
            if (chart.StartMove && chart.Crosshair.Enable)
            {
                return;
            }

            IsZoomed = true;
            offset = !chart.DelayRedraw ? chart.AxisContainer.AxisLayout.SeriesClipRect : offset;
            chart.DelayRedraw = chart.DisableTrackTooltip = true;
            List<AxisData> zoomedAxisCollection = new List<AxisData>();
            double zoomFactor, zoomPosition;
            foreach (ChartAxisRenderer axisRenderer in axes)
            {
                ChartAxis axis = axisRenderer.Axis;
                zoomFactor = axis.ZoomFactor;
                zoomPosition = axis.ZoomPosition;
                double currentScale = Math.Max(1 / ChartHelper.MinMax(axis.ZoomFactor, 0, 1), 1);
                if (axis.Renderer.Orientation == Orientation.Horizontal)
                {
#pragma warning disable BL0005
                    zoomPosition = ChartHelper.MinMax(axis.ZoomPosition + ((chart.PreviousMouseMoveX - chart.MouseX) / axis.Renderer.Rect.Width / currentScale), 0, 1 - axis.ZoomFactor);
                }
                else
                {
                    zoomPosition = ChartHelper.MinMax(axis.ZoomPosition - ((chart.PreviousMouseMoveY - chart.MouseY) / axis.Renderer.Rect.Height / currentScale), 0, 1 - axis.ZoomFactor);
                }

                if (axis.Renderer.ZoomingScrollBar != null)
                {
                    axis.Renderer.ZoomingScrollBar.IsScrollUI = false;
                }

                zoomedAxisCollection.Add(new AxisData
                {
                    ZoomFactor = zoomFactor,
                    ZoomPosition = zoomPosition,
                    AxisName = axis.GetName(),
                    AxisRange = ChartHelper.GetVisibleRangeModel(axis.Renderer.VisibleRange, axis.Renderer.VisibleInterval)
                });
            }

            ZoomingEventArgs zoomingEventArgs = new ZoomingEventArgs
            {
                AxisCollection = zoomedAxisCollection,
                Name = Constants.ONZOOMING
            };
            SfChart.InvokeEvent<ZoomingEventArgs>(chart.ChartEvents?.OnZooming, zoomingEventArgs);
            if (!zoomingEventArgs.Cancel)
            {
                foreach (AxisData axisData in zoomedAxisCollection)
                {
                    ((ChartAxisRenderer)axes.Find(item => ((ChartAxisRenderer)item).Axis.GetName() == axisData.AxisName))?.Axis.UpdateZoomValues(axisData.ZoomFactor, axisData.ZoomPosition);
                }

                PerformDeferredZoom(chart);
            }
        }

        private void DoZoom(SfChart chart, List<IChartElementRenderer> axes, Rect bounds)
        {
            Rect zoomRect = zoomingRect;
            IsPanning = chart.ZoomSettings.EnablePan || IsPanning;
            List<AxisData> zoomedAxisCollections = new List<AxisData>();
            double zoomFactor, zoomPosition;
            foreach (ChartAxisRenderer axisRenderer in axes)
            {
                zoomFactor = axisRenderer.Axis.ZoomFactor;
                zoomPosition = axisRenderer.Axis.ZoomPosition;
                if (axisRenderer.Orientation == Orientation.Horizontal && zooming.Mode != ZoomMode.Y)
                {
                    zoomPosition += Math.Abs((zoomRect.X - bounds.X) / bounds.Width) * axisRenderer.Axis.ZoomFactor;
                    zoomFactor *= zoomRect.Width / bounds.Width;
                }
                else if (axisRenderer.Orientation == Orientation.Vertical && zooming.Mode != ZoomMode.X)
                {
                    zoomPosition += (1 - Math.Abs((zoomRect.Height + (zoomRect.Y - bounds.Y)) / bounds.Height)) * axisRenderer.Axis.ZoomFactor;
                    zoomFactor *= zoomRect.Height / bounds.Height;
                }

                zoomedAxisCollections.Add(new AxisData
                {
                    ZoomFactor = zoomFactor,
                    ZoomPosition = zoomPosition,
                    AxisName = axisRenderer.Axis.GetName(),
                    AxisRange = ChartHelper.GetVisibleRangeModel(axisRenderer.VisibleRange, axisRenderer.VisibleInterval)
                });
            }

            ZoomingEventArgs onZoomingEventArg = new ZoomingEventArgs
            {
                AxisCollection = zoomedAxisCollections,
                Name = Constants.ONZOOMING
            };
            SfChart.InvokeEvent<ZoomingEventArgs>(chart.ChartEvents?.OnZooming, onZoomingEventArg);
            if (!onZoomingEventArg.Cancel)
            {
                foreach (AxisData axis in zoomedAxisCollections)
                {
                    ((ChartAxisRenderer)axes.Find(item => ((ChartAxisRenderer)item).Axis.GetName() == axis.AxisName)).Axis?.UpdateZoomValues(axis.ZoomFactor, axis.ZoomPosition);
                }

                zoomingRect = new Rect(0, 0, 0, 0);
                PerformZoomRedraw(chart);
            }
        }

        private void CalculatePinchZoomFactor(SfChart chart, Rect pinchRect)
        {
            double rangeMin, rangeMax, pinchValue, axisTrans;
            List<AxisData> zoomedAxisCollection = new List<AxisData>();
            double zoomFactor, zoomPosition;
            for (int index = 0; index < chart.AxisContainer.Renderers.Count; index++)
            {
                ChartAxisRenderer axisRenderer = (ChartAxisRenderer)chart.AxisContainer.Renderers[index];
                ChartAxis axis = axisRenderer.Axis;
                zoomFactor = axis.ZoomFactor;
                zoomPosition = axis.ZoomPosition;
                if ((axisRenderer.Orientation == Orientation.Horizontal && zooming.Mode != ZoomMode.Y) || (axisRenderer.Orientation == Orientation.Vertical && zooming.Mode != ZoomMode.X))
                {
                    if (axisRenderer.Orientation == Orientation.Horizontal)
                    {
                        pinchValue = pinchRect.X - offset.X;
                        axisTrans = axisRenderer.Rect.Width / zoomAxes[index].Delta;
                        rangeMin = (pinchValue / axisTrans) + zoomAxes[index].Min;
                        pinchValue = pinchRect.X + pinchRect.Width - offset.X;
                        rangeMax = (pinchValue / axisTrans) + zoomAxes[index].Min;
                    }
                    else
                    {
                        pinchValue = pinchRect.Y - offset.Y;
                        axisTrans = axisRenderer.Rect.Height / zoomAxes[index].Delta;
                        rangeMin = (((pinchValue * -1) + axisRenderer.Rect.Height) / axisTrans) + zoomAxes[index].Min;
                        pinchValue = pinchRect.Y + pinchRect.Height - offset.Y;
                        rangeMax = (((pinchValue * -1) + axisRenderer.Rect.Height) / axisTrans) + zoomAxes[index].Min;
                    }

                    double selectionMin = Math.Min(rangeMin, rangeMax), selectionMax = Math.Max(rangeMin, rangeMax),
                    currentZP = (selectionMin - zoomAxes[index].ActualMin) / zoomAxes[index].ActualDelta,
                    currentZF = (selectionMax - selectionMin) / zoomAxes[index].ActualDelta;
                    zoomPosition = currentZP < 0 ? 0 : currentZP;
                    zoomFactor = currentZF > 1 ? 1 : currentZF;
                    zoomedAxisCollection.Add(new AxisData()
                    {
                        ZoomFactor = zoomFactor,
                        ZoomPosition = zoomPosition,
                        AxisName = axis.GetName(),
                        AxisRange = ChartHelper.GetVisibleRangeModel(axisRenderer.VisibleRange, axisRenderer.VisibleInterval)
                    });
                }
            }

            ZoomingEventArgs onZoomingEventArgs = new ZoomingEventArgs
            {
                AxisCollection = zoomedAxisCollection,
                Name = Constants.ONZOOMING
            };
            SfChart.InvokeEvent<ZoomingEventArgs>(chart.ChartEvents?.OnZooming, onZoomingEventArgs);
            if (!onZoomingEventArgs.Cancel)
            {
                foreach (AxisData axisData in zoomedAxisCollection)
                {
                    ((ChartAxisRenderer)chart.AxisContainer.Renderers.Find(item => ((ChartAxisRenderer)item).Axis.GetName() == axisData.AxisName))?.Axis.UpdateZoomValues(axisData.ZoomFactor, axisData.ZoomPosition);
                }
            }
        }

        private void PerformMouseWheelZooming(ChartMouseWheelArgs args, double mouseX, double mouseY, SfChart chart, List<IChartElementRenderer> axisCollection)
        {
            int direction = BrowserName() == "mozilla" && !IsPointer() ? -args.Detail / 3 > 0 ? 1 : -1 : (args.WheelDelta / 120) > 0 ? 1 : -1;
            IsZoomed = true;
            CalculateZoomAxesRange(chart);
            chart.DisableTrackTooltip = true;
            PerformedUI = true;
            IsPanning = chart.ZoomSettings.EnablePan || IsPanning;
            List<AxisData> zoomedAxisCollection = new List<AxisData>();
            IsWheelZoom = true;
            double zoomFactor, zoomPosition;
            foreach (ChartAxisRenderer axisRenderer in axisCollection)
            {
                ChartAxis axis = axisRenderer.Axis;
                zoomFactor = axis.ZoomFactor;
                zoomPosition = axis.ZoomPosition;
                if ((axisRenderer.Orientation == Orientation.Vertical && zooming.Mode != ZoomMode.X) || (axisRenderer.Orientation == Orientation.Horizontal && zooming.Mode != ZoomMode.Y))
                {
                    double cumulative = Math.Max(Math.Max(1 / ChartHelper.MinMax(axis.ZoomFactor, 0, 1), 1) + (0.25 * direction), 1);
                    if (cumulative >= 1)
                    {
                        double origin = axisRenderer.Orientation == Orientation.Horizontal ? mouseX / axisRenderer.Rect.Width : 1 - (mouseY / axisRenderer.Rect.Height);
                        origin = origin > 1 ? 1 : origin < 0 ? 0 : origin;
                        zoomFactor = (cumulative == 1) ? 1 : ChartHelper.MinMax(1 / cumulative, 0, 1);
                        zoomPosition = (cumulative == 1) ? 0 : axis.ZoomPosition + ((axis.ZoomFactor - zoomFactor) * origin);
                        if (axis.ZoomPosition != zoomPosition || axis.ZoomFactor != zoomFactor)
                        {
                            zoomFactor = (zoomPosition + zoomFactor) > 1 ? (1 - zoomPosition) : zoomFactor;
                        }
                    }
                }

                zoomedAxisCollection.Add(new AxisData
                {
                    ZoomFactor = zoomFactor,
                    ZoomPosition = zoomPosition,
                    AxisName = axis.GetName(),
                    AxisRange = ChartHelper.GetVisibleRangeModel(axisRenderer.VisibleRange, axisRenderer.VisibleInterval)
                });
            }

            ZoomingEventArgs onZoomingEventArgs = new ZoomingEventArgs
            {
                AxisCollection = zoomedAxisCollection,
                Name = Constants.ONZOOMING
            };
            SfChart.InvokeEvent<ZoomingEventArgs>(chart.ChartEvents?.OnZooming, onZoomingEventArgs);
            if (!onZoomingEventArgs.Cancel)
            {
                foreach (AxisData axis in zoomedAxisCollection)
                {
                    ((ChartAxisRenderer)axisCollection.Find(item => ((ChartAxisRenderer)item).Axis.GetName() == axis.AxisName))?.Axis.UpdateZoomValues(axis.ZoomFactor, axis.ZoomPosition);
                }

                PerformZoomRedraw(chart);
            }

            IsWheelZoom = false;
        }

        internal void ApplyZoomToolkit(SfChart chart, List<IChartElementRenderer> axes)
        {
            if (IsAxisZoomed(axes))
            {
                chart.ZoomingToolkitContent.ShowZoomingKit();
                IsZoomed = true;
            }
            else
            {
                chart.ZoomingToolkitContent.RemoveTooltip();
                IsPanning = IsZoomed = false;
                chart.ZoomingToolkitContent.HideZoomingKit();
                chart.SetSvgCursor("auto");
            }
        }

        internal bool IsAxisZoomed(List<IChartElementRenderer> axes)
        {
            bool showToolkit = false;
            foreach (ChartAxisRenderer axisRenderer in axes)
            {
                showToolkit = IsZoomed = showToolkit || axisRenderer.Axis.ZoomFactor != 1 || axisRenderer.Axis.ZoomPosition != 0;
            }

            return showToolkit;
        }

        internal void Dispose()
        {
            chart = null;
            Browser = null;
            zooming = null;
            zoomingRect = null;
            TouchMoveList = null;
            TouchStartList = null;
            zoomAxes = null;
            rectEle = null;
            offset = null;
        }
    }
}