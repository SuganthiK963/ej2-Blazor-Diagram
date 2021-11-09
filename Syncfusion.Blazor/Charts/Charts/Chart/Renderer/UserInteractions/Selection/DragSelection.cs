using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal partial class Selection : BaseSelection
    {
        private const string SPACE = " ";

        private void DragSelectionProcess(ChartInternalMouseEventArgs e)
        {
            if (ChartHelper.WithInBounds(chartInstance.MouseDownX, chartInstance.MouseDownY, seriesClipRect))
            {
                if (rectGrabbing && !resizing)
                {
                    DraggedRectMoved(dragRect, true);
                }
                else if (dragging && !resizing && lassoDownCompleted)
                {
                    if (chartInstance.SelectionMode == SelectionMode.Lasso)
                    {
#pragma warning disable CA1305
                        GetPath(chartInstance.MouseDownX, chartInstance.MouseDownY, chartInstance.MouseX, chartInstance.MouseY, (chartInstance.AllowMultiSelection ? count : 0).ToString());
                        DrawDraggingRect(dragRect);
                    }
                    else
                    {
                        dragRect = GetDraggedRectLocation(chartInstance.MouseDownX, chartInstance.MouseDownY, chartInstance.MouseX, chartInstance.MouseY, seriesClipRect);
                        DrawDraggingRect(dragRect);
                    }
                }

                if (rectPoints != null && !chartInstance.AllowMultiSelection)
                {
                    ResizingSelectionRect(e, new ChartInternalLocation(chartInstance.MouseX, chartInstance.MouseY), null);
                }
                else if ((chartInstance.AllowMultiSelection && !dragging) || resizing)
                {
                    ResizingSelectionRect(e, new ChartInternalLocation(chartInstance.MouseX, chartInstance.MouseY), null);
                }
            }
            else
            {
                CompleteSelection(e);
            }
        }

        private void CalculateDragSelectedElements(Rect dragRect, [Optional] bool isClose)
        {
            RemoveSelectedElements(SelectedDataIndexes, chartInstance.SeriesContainer.Renderers);
            Rect rect = new Rect(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
            ChartInternalLocation axisOffset = new ChartInternalLocation(seriesClipRect.X, seriesClipRect.Y);
            RemoveOffset(rect, axisOffset);
            List<PointXY> selectedPointValues = new List<PointXY>();
            IsSeriesMode = false;
            bool isDragResize = chartInstance.AllowMultiSelection && (rectGrabbing || resizing);
            rectPoints = new Rect(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
            dragRectArray[draggedRect + (isDragResize ? targetIndex : count)] = rectPoints;
            if (dragRect.Width > 0 && dragRect.Height > 0 && !isClose)
            {
                Rect rt = new Rect(dragRect.X, dragRect.Y, dragRect.Width, dragRect.Height);
                RemoveOffset(rt, axisOffset);
                filterArray[draggedRect + (isDragResize ? targetIndex : count)] = rt;
            }

            foreach (ChartSeriesRenderer seriesRenderer in chartInstance.SeriesContainer.Renderers)
            {
                ChartSeries series = seriesRenderer.Series;
                if (series.Visible)
                {
                    selectedPointValues.Clear();
                    double x_AxisOffset, y_AxisOffset;
                    if ((chartInstance.IsTransposed || series.Type.ToString().Contains("Bar", StringComparison.InvariantCulture)) && !(chartInstance.IsTransposed && series.Type.ToString().Contains("Bar", StringComparison.InvariantCulture)))
                    {
                        x_AxisOffset = seriesRenderer.XAxisRenderer.Rect.Y - axisOffset.Y;
                        y_AxisOffset = seriesRenderer.YAxisRenderer.Rect.X - axisOffset.X;
                    }
                    else
                    {
                        x_AxisOffset = seriesRenderer.XAxisRenderer.Rect.X - axisOffset.X;
                        y_AxisOffset = seriesRenderer.YAxisRenderer.Rect.Y - axisOffset.Y;
                    }

                    foreach (Point currentPoint in seriesRenderer.Points)
                    {
                        bool isCurrentPoint;
                        object selectedPointX = currentPoint.XValue;
                        if (chartInstance.AxisContainer.Axes["PrimaryXAxis"].ValueType == ValueType.Category)
                        {
                            selectedPointX = currentPoint.X.ToString();
                        }
                        else if (chartInstance.AxisContainer.Axes["PrimaryXAxis"].ValueType == ValueType.DateTime)
                        {
                            selectedPointX = Convert.ToDateTime(currentPoint.X, null);
                        }

                        if (series.Type == ChartSeriesType.BoxAndWhisker)
                        {
                            isCurrentPoint = currentPoint.Regions.Exists(region => ChartHelper.WithInBounds(region.X + x_AxisOffset, region.Y + y_AxisOffset, rect));
                        }
                        else
                        {
                            if (chartInstance.SelectionMode == SelectionMode.Lasso)
                            {
                                isCurrentPoint = currentPoint.IsSelected;
                            }
                            else
                            {
                                isCurrentPoint = chartInstance.AllowMultiSelection ? IsPointSelect(currentPoint, x_AxisOffset, y_AxisOffset, filterArray.Values.ToList()) :
                                    currentPoint.SymbolLocations.Exists(location => location != null && ChartHelper.WithInBounds(location.X + x_AxisOffset, location.Y + y_AxisOffset, rect));
                            }
                        }

                        if (isCurrentPoint && seriesRenderer.Category() != SeriesCategories.Indicator)
                        {
#pragma warning disable CA2000
                            ChartSelectedDataIndex index = ChartSelectedDataIndex.CreateSelectedData(currentPoint.Index, seriesRenderer.Index);
#pragma warning restore CA2000
                            SelectionChart(index, FindElements(seriesRenderer, index));
                            selectedPointValues.Add(new PointXY() { X = selectedPointX, Y = series.Type != ChartSeriesType.RangeArea ? currentPoint.YValue : currentPoint.Regions[0].Y, SeriesIndex = seriesRenderer.Index, PointIndex = currentPoint.Index });
                        }

                        if (isCurrentPoint && series.Type == ChartSeriesType.RangeArea)
                        {
                            selectedPointValues.Add(new PointXY() { X = selectedPointX, Y = currentPoint.Regions[0].Y, SeriesIndex = seriesRenderer.Index, PointIndex = currentPoint.Index });
                        }
                    }
                }
            }

            BlurEffect(chartInstance.SeriesContainer.Renderers);
            if (!isClose)
            {
                CreateCloseButton(chartInstance.SelectionMode == SelectionMode.Lasso ? chartInstance.MouseDownX : (dragRect.X + dragRect.Width), chartInstance.SelectionMode == SelectionMode.Lasso ? chartInstance.MouseDownY : dragRect.Y);
            }

            if (chartInstance.ChartEvents?.OnSelectionChanged != null)
            {
                chartInstance.ChartEvents.OnSelectionChanged.Invoke(new SelectionCompleteEventArgs() { SelectedDataValues = selectedPointValues, Name = Constants.ONSELECTIONCHANGED });
            }
        }

        private void CreateCloseButton(double x, double y)
        {
            bool isMultiDrag = chartInstance.AllowMultiSelection, isDragResize = isMultiDrag && (rectGrabbing || resizing), isDrag = rectGrabbing || resizing;
            CircleOptions circle = new CircleOptions(
                closeIconId + "circle_" + (isMultiDrag ? (isDrag ? targetIndex : count).ToString() : string.Empty),
                x.ToString(culture),
                y.ToString(culture),
                "10",
                string.Empty,
                2,
                chartInstance.ChartThemeStyle.SelectionCircleStroke,
                1,
                "#FFFFFF");
            PathOptions path = new PathOptions(
                closeIconId + "cross_" + (isMultiDrag ? (isDrag ? targetIndex : count).ToString() : string.Empty),
                "M " + (x - 4).ToString(culture) + SPACE + (y - 4).ToString(culture) + " L " + (x + 4).ToString(culture) + SPACE + (y + 4).ToString(culture) + " M " + (x - 4).ToString(culture) + SPACE + (y + 4).ToString(culture) + " L " + (x + 4).ToString(culture) + SPACE + (y - 4).ToString(culture), string.Empty, 2, chartInstance.ChartThemeStyle.SelectionCircleStroke);
            SvgSelectionRect element = chartInstance.ParentRect.RectsReference.Find(item => item.Id == draggedRect + ((isDragResize || !chartInstance.AllowMultiSelection) ? targetIndex : count));
            SvgSelectionPath elementPath = chartInstance.ParentRect.PathsReference.Find(item => item.Id == draggedRect + ((isDragResize || !chartInstance.AllowMultiSelection) ? targetIndex : count));
#pragma warning restore CA1305
            if (!chartInstance.AllowMultiSelection)
            {
                element = chartInstance.ParentRect.RectsReference.Count > 0 ? chartInstance.ParentRect.RectsReference[0] : null;
                elementPath = chartInstance.ParentRect.PathsReference.Count > 0 ? chartInstance.ParentRect.PathsReference[0] : null;
            }

            if (chartInstance.SelectionMode == SelectionMode.Lasso)
            {
                elementPath?.DrawCloseIcon(circle, path);
            }
            else
            {
                element?.DrawCloseIcon(circle, path);
            }
        }

        private void DraggedRectMoved(Rect grabbedPoint, [Optional] bool doDrawing)
        {
            Rect rect;
            if ((resizing || rectGrabbing) && chartInstance.AllowMultiSelection)
            {
                Rect r = dragRectArray[draggedRect + targetIndex];
                rect = new Rect(r.X, r.Y, r.Width, r.Height);
            }
            else
            {
                rect = new Rect(rectPoints.X, rectPoints.Y, rectPoints.Width, rectPoints.Height);
            }

            rect.X -= grabbedPoint.X - chartInstance.MouseX;
            rect.Y -= grabbedPoint.Y - chartInstance.MouseY;
            rect = GetDraggedRectLocation(rect.X, rect.Y, rect.X + rect.Width, rect.Height + rect.Y, seriesClipRect);
            if (doDrawing)
            {
                DrawDraggingRect(rect);
            }
            else
            {
                CalculateDragSelectedElements(rect, false);
            }
        }

        internal void DragStart(Rect seriesClipRect, double mouseDownX, double mouseDownY, ChartInternalMouseEventArgs events)
        {
            string mode = chartInstance.SelectionMode.ToString();
            CurrentMode = chartInstance.SelectionMode;
            dragging = lassoDownCompleted = (mode.Contains("Drag", StringComparison.InvariantCulture) || mode == "Lasso") && (chartInstance.IsDoubleTap || !chartInstance.IsTouch) && chartInstance.ChartAreaType != ChartAreaType.PolarAxes;
            if (dragging)
            {
                count = dragRectArray.ContainsKey(events.Target) ? count : count + 1;
                dragRect = new Rect(chartInstance.MouseDownX, chartInstance.MouseDownY, 0, 0);
                if (chartInstance.MouseDownX < seriesClipRect.X || chartInstance.MouseDownX > (seriesClipRect.X + seriesClipRect.Width) || chartInstance.MouseDownY < seriesClipRect.Y || chartInstance.MouseDownY > (seriesClipRect.Y + seriesClipRect.Height))
                {
                    dragging = false;
                }
            }

            if (CurrentMode == SelectionMode.Lasso)
            {
                foreach (ChartSeriesRenderer seriesRenderer in chartInstance.SeriesContainer.Renderers)
                {
                    if (seriesRenderer.Series.Visible)
                    {
                        seriesRenderer.Points.Where(x => !chartInstance.AllowMultiSelection).ToList().ForEach(y => y.IsSelected = false);
                    }
                }
            }
            else
            {
                if (rectPoints != null && !chartInstance.AllowMultiSelection)
                {
                    dragRect = new Rect(chartInstance.MouseDownX, chartInstance.MouseDownY, 0, 0);
                    ResizingSelectionRect(events, new ChartInternalLocation(mouseDownX, mouseDownY), true);
                    rectGrabbing = ChartHelper.WithInBounds(mouseDownX, mouseDownY, rectPoints);
                }

                if (chartInstance.AllowMultiSelection)
                {
                    int index = GetIndex(events.Target);
                    targetIndex = IsDragRect(events.Target) ? index : 0;
                    if (dragRectArray.Count != 0 && IsDragRect(events.Target))
                    {
                        ResizingSelectionRect(events, new ChartInternalLocation(mouseDownX, mouseDownY), true);
                        rectGrabbing = ChartHelper.WithInBounds(mouseDownX, mouseDownY, dragRectArray[draggedRect + index]);
                    }
                }
            }
        }

        private void ResizingSelectionRect(ChartInternalMouseEventArgs e, ChartInternalLocation location, bool? tapped)
        {
            Rect rect = new Rect();
            if ((chartInstance.AllowMultiSelection && IsDragRect(e.Target)) || dragRectArray.ContainsKey(draggedRect + targetIndex))
            {
                if (IsDragRect(e.Target))
                {
                    targetIndex = GetIndex(e.Target);
                }

                if (dragRectArray.ContainsKey(draggedRect + targetIndex))
                {
                    rect = dragRectArray[draggedRect + targetIndex];
                }
            }

            if (!chartInstance.AllowMultiSelection)
            {
                rect = new Rect(rectPoints.X, rectPoints.Y, rectPoints.Width, rectPoints.Height);
            }

            if (rect.Width > 0 && rect.Height > 0)
            {
                if (resizing)
                {
                    rect = GetDraggedRectLocation(rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height, seriesClipRect);
                    DrawDraggingRect(rect);
                    dragRect = rect;
                }

                if (tapped == true)
                {
                    resizing = FindResizeMode(rect, location);
                }
            }
            else
            {
                return;
            }
        }

        private bool FindResizeMode(Rect rect, ChartInternalLocation location)
        {
            string cursorStyle = "se-resize";
            bool resize = false;
            if (!resizing)
            {
                List<Rect> resizeEdges = new List<Rect>()
                {
                    new Rect(rect.X, rect.Y, rect.Width - 5, 5),
                    new Rect(rect.X, rect.Y, 5, rect.Height),
                    new Rect(rect.X, rect.Y + rect.Height - 5, rect.Width - 5, 5),
                    new Rect(rect.X + rect.Width - 5, rect.Y + 5, 5, rect.Height - 15),
                    new Rect(rect.X + rect.Width - 10, rect.Y + rect.Height - 10, 10, 10)
                };
                for (int i = 0; i < resizeEdges.Count; i++)
                {
                    if (ChartHelper.WithInBounds(location.X, location.Y, resizeEdges[i]))
                    {
                        cursorStyle = (i == 4) ? cursorStyle : (i % 2 == 0) ? "ns-resize" : "ew-resize";
                        resize = true;
                        resizeMode = i;
                        break;
                    }
                }
            }
            else
            {
                double x = rect.X, y = rect.Y, width = location.X - x, height = location.Y - y;
                switch (resizeMode)
                {
                    case 0:
                        height = Math.Abs((rect.Height + rect.Y) - location.Y);
                        rect.Y = Math.Min(rect.Height + rect.Y, location.Y);
                        rect.Height = height;
                        break;
                    case 1:
                        width = Math.Abs((rect.Width + rect.X) - location.X);
                        rect.X = Math.Min(rect.Width + rect.X, location.X);
                        rect.Width = width;
                        break;
                    case 2:
                        rect.Height = Math.Abs(height);
                        rect.Y = Math.Min(location.Y, y);
                        break;
                    case 3:
                        rect.Width = Math.Abs(width);
                        rect.X = Math.Min(location.X, x);
                        break;
                    case 4:
                        rect.Width = Math.Abs(width);
                        rect.Height = Math.Abs(height);
                        rect.X = Math.Min(location.X, x);
                        rect.Y = Math.Min(location.Y, y);
                        break;
                }
            }

            if (CurrentMode != SelectionMode.Lasso)
            {
                ChangeCursorStyle(resize, GetRectangleElement(chartInstance.AllowMultiSelection ? draggedRect + targetIndex : draggedRect + 0), cursorStyle);
            }

            return resize;
        }

        private async void DrawDraggingRect(Rect dragRect)
        {
            Rect cartesianLayout = seriesClipRect;
            double border = chartInstance.ChartAreaRenderer.Area.Border.Width;
            string rectFill = chartInstance.ChartThemeStyle.SelectionRectFill;
            string rectStroke = chartInstance.ChartThemeStyle.SelectionRectStroke;
            bool isLasso = chartInstance.SelectionMode == SelectionMode.Lasso;
            if (isdrawRect)
            {
                cartesianLayout.X = cartesianLayout.X - (border / 2);
                cartesianLayout.Y = cartesianLayout.Y - (border / 2);
                cartesianLayout.Width = cartesianLayout.Width + border;
                cartesianLayout.Height = cartesianLayout.Height + border;
                isdrawRect = false;
            }

            switch (chartInstance.SelectionMode)
            {
                case SelectionMode.DragX:
                    dragRect.Y = cartesianLayout.Y;
                    dragRect.Height = cartesianLayout.Height;
                    break;
                case SelectionMode.DragY:
                    dragRect.X = cartesianLayout.X;
                    dragRect.Width = cartesianLayout.Width;
                    break;
            }

            if ((dragRect.Width < 5 || dragRect.Height < 5) && !isLasso)
            {
                return;
            }

            if (chartInstance.AllowMultiSelection && (chartInstance.SelectionMode.ToString().Contains("Drag", StringComparison.InvariantCulture) || isLasso))
            {
                SvgSelectionRect rectElement = null;
                if (rectGrabbing || resizing)
                {
                    if (resizing)
                    {
                        rectElement = GetRectangleElement(draggedRect + targetIndex);
                    }
                    else
                    {
                        rectElement = GetRectangleElement(draggedRect + targetIndex);
                    }
#pragma warning disable CA2007
                    await rectElement?.ChangeRectangle(dragRect);
                }
                else
                {
                    if (!isLasso)
                    {
                        if (chartInstance.ParentRect.RectsReference.Find(item => item.Id == draggedRect + count) == null)
                        {
                            chartInstance.ParentRect.DrawNewRectangle(new SelectionOptions()
                            {
                                DragRect = dragRect,
                                Fill = rectFill,
                                Stroke = rectStroke,
                                StrokeWidth = "1",
                                Id = draggedRect + count
                            });
                        }
                        else
                        {
                            await chartInstance.ParentRect.RectsReference.Find(item => item.Id == draggedRect + count)?.ChangeRectangle(dragRect);
                        }
                    }
                    else
                    {
                        if (chartInstance.ParentRect.PathsReference.Find(item => item.Id == draggedRect + count) == null)
                        {
                            chartInstance.ParentRect.DrawNewRectangle(new SelectionOptions()
                            {
                                Fill = rectFill,
                                Stroke = rectStroke,
                                StrokeWidth = "3",
                                Id = draggedRect + count,
#pragma warning disable CA1305
                                Path = lassoPaths[count.ToString()],
                                IsLasso = true
                            });
                        }
                        else
                        {
                            await chartInstance.ParentRect.PathsReference.Find(item => item.Id == draggedRect + count)?.ChangePath(lassoPaths[count.ToString()]);
                        }
                    }
                }
            }
            else
            {
                if (!isLasso)
                {
                    if (chartInstance.ParentRect.RectsReference.Count == 0)
                    {
                        chartInstance.ParentRect.DrawNewRectangle(new SelectionOptions()
                        {
                            DragRect = dragRect,
                            Fill = rectFill,
                            Stroke = rectStroke,
                            StrokeWidth = "1",
                            Id = draggedRect + count
                        });
                    }
                    else
                    {
                        await chartInstance.ParentRect.RectsReference[0]?.ChangeRectangle(dragRect);
                    }
                }
                else
                {
                    if (chartInstance.ParentRect.PathsReference.Count == 0)
                    {
                        lassoPaths["0"] = string.Empty;
                        chartInstance.ParentRect.DrawNewRectangle(new SelectionOptions()
                        {
                            Fill = rectFill,
                            Stroke = rectStroke,
                            StrokeWidth = "3",
                            Id = draggedRect + count,
                            IsLasso = isLasso,
                            Path = string.Empty
                        });
                    }
                    else
                    {
                        await chartInstance.ParentRect.PathsReference[0]?.ChangePath(lassoPaths["0"]);
                    }
                }
            }
        }

        private async void CompleteSelection(ChartInternalMouseEventArgs e)
        {
            if (chartInstance.SelectionMode == SelectionMode.None)
            {
                return;
            }

            CurrentMode = chartInstance.SelectionMode;
            Rect previousRect = new Rect();
            if (dragRectArray.ContainsKey(draggedRect + targetIndex))
            {
                previousRect = dragRectArray[draggedRect + targetIndex];
            }

            if ((dragging || resizing) && dragRect.Width > 5 && dragRect.Height > 5)
            {
                CalculateDragSelectedElements(dragRect);
            }
            else if (!chartInstance.AllowMultiSelection && rectGrabbing && rectPoints.Width > 0 && rectPoints.Height > 0)
            {
                DraggedRectMoved(dragRect);
            }
            else if (rectGrabbing && previousRect.Width > 0 && previousRect.Height > 0)
            {
                DraggedRectMoved(dragRect);
            }

            if (chartInstance.SelectionMode == SelectionMode.Lasso && dragging && lassoDownCompleted && lassoPaths.Count > 0)
            {
                lassoDownCompleted = false;
                if (!chartInstance.AllowMultiSelection && lassoPaths["0"].Contains("L", StringComparison.InvariantCulture) && !e.Target.Contains("close", StringComparison.InvariantCulture))
                {
                    SvgSelectionPath lassoEle = chartInstance.ParentRect.PathsReference.First();
                    if (lassoEle != null)
                    {
                        lassoPaths["0"] += " Z";
                        await lassoEle.ChangePath(lassoPaths["0"]);
                        await LassoChecking(lassoEle.Id);
                    }
                }
                else
                {
                    SvgSelectionPath lassoEle = chartInstance.ParentRect.PathsReference.Find(item => item.Id == draggedRect + count);
                    if (lassoEle != null && lassoPaths[count.ToString()].Contains("L", StringComparison.InvariantCulture))
                    {
                        await lassoEle.ChangePath(lassoPaths[count.ToString()] + "Z");
#pragma warning restore CA1305
                        await LassoChecking(draggedRect + GetIndex(lassoEle.Id));
                    }
                }

                if (dragging || resizing)
                {
                    CalculateDragSelectedElements(dragRect);
                }
            }

            dragging = rectGrabbing = resizing = false;
            RemoveDraggedElements(e);
        }

        private async Task LassoChecking(string lassoPathId)
        {
            DomRect elementOffset = await JSRuntimeExtensions.InvokeAsync<DomRect>(chartInstance.JSRuntime, Constants.GETELEMENTBOUNDSBYID, new object[] { chartInstance.ID });
            double offsetX = seriesClipRect.X + Math.Max(elementOffset.Left, 0);
            double offsetY = seriesClipRect.Y + Math.Max(elementOffset.Top, 0);
            foreach (ChartSeriesRenderer series in chartInstance.SeriesContainer.Renderers)
            {
                foreach (Point dataPoint in series.Points)
                {
                    string selectedId = await chartInstance.InvokeMethod<string>(Constants.ISLASSOID, false, new object[] { dataPoint.SymbolLocations[0].X + offsetX, dataPoint.SymbolLocations[0].Y + offsetY });
#pragma warning restore CA2007
                    if (lassoPathId == selectedId)
                    {
                        dataPoint.IsSelected = true;
                        if (chartInstance.AllowMultiSelection && CurrentMode == SelectionMode.Lasso)
                        {
                            if (selectedLassoPoints.ContainsKey((count, series.Index)))
                            {
                                selectedLassoPoints[(count, series.Index)].Add(dataPoint);
                            }
                            else
                            {
                                selectedLassoPoints.Add((count, series.Index), new List<Point>());
                                selectedLassoPoints[(count, series.Index)].Add(dataPoint);
                            }
                        }
                    }
                    else if (!chartInstance.AllowMultiSelection)
                    {
                        dataPoint.IsSelected = false;
                    }
                }
            }
        }

        private SvgSelectionRect GetRectangleElement(string id)
        {
            return chartInstance.ParentRect.RectsReference.Find(item => item.Id == id);
        }

        internal void CreateDragElements(RenderTreeBuilder renderTreeBuilder)
        {
            renderTreeBuilder.OpenComponent<SvgSelectionRectCollection>(SvgRendering.Seq++);
            renderTreeBuilder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { chartInstance.ParentRect = (SvgSelectionRectCollection)ins; });
            renderTreeBuilder.CloseComponent();
        }

        private Rect GetDraggedRectLocation(double x1, double y1, double x2, double y2, Rect outerRect)
        {
            double width = Math.Abs(x1 - x2);
            double height = Math.Abs(y1 - y2);
            return new Rect(Math.Max(CheckBounds(Math.Min(x1, x2), width, outerRect.X, outerRect.Width), outerRect.X), Math.Max(CheckBounds(Math.Min(y1, y2), height, outerRect.Y, outerRect.Height), outerRect.Y), Math.Min(width, outerRect.Width), Math.Min(height, outerRect.Height));
        }

#pragma warning disable CA1822
        private void RemoveOffset(Rect rect, ChartInternalLocation clip)
        {
            rect.X -= clip.X;
            rect.Y -= clip.Y;
        }

        private bool IsPointSelect(Point point, double x_AxisOffset, double y_AxisOffset, List<Rect> rectCollection)
        {
            ChartInternalLocation location = point.SymbolLocations[0];
            foreach (Rect rect in rectCollection)
            {
                if (rect != null && location != null && ChartHelper.WithInBounds(location.X + x_AxisOffset, location.Y + y_AxisOffset, rect))
                {
                    return true;
                }
            }

            return false;
        }

        private void ChangeCursorStyle(bool isResize, SvgSelectionRect rectElement, string cursorStyle)
        {
            cursorStyle = isResize ? cursorStyle : "move";
            if (rectElement != null)
            {
                rectElement.ChangeCursor(cursorStyle);
            }
        }

        private double CheckBounds(double start, double size, double min, double max)
        {
            if (start < min)
            {
                start = min;
            }
            else if ((start + size) > (max + min))
            {
                start = (max + min) - size;
            }

            return start;
        }

        private int GetIndex(string id)
        {
            if (id.Contains("_drag_", StringComparison.InvariantCulture))
            {
                return int.Parse(id.Split("_drag_")[1].Split("_")[1], null);
            }

            return -1;
        }

        private bool IsDragRect(string id)
#pragma warning restore CA1822
        {
            return id.Contains("_ej2_drag_rect", StringComparison.InvariantCulture);
        }

        private void GetPath(double startX, double startY, double endX, double endY, string id)
        {
            if (dragging)
            {
                if (lassoPaths.Count > 0 && lassoPaths.ContainsKey(id) && !string.IsNullOrEmpty(lassoPaths[id]) && !lassoPaths[id].Contains("Z", StringComparison.InvariantCulture))
                {
                    lassoPaths[id] = lassoPaths[id] + " L" + endX.ToString(culture) + SPACE + endY.ToString(culture);
                }
                else
                {
                    lassoPaths[id] = "M " + startX.ToString(culture) + SPACE + startY.ToString(culture);
                }
            }
        }

        private void RemoveDraggedElements(ChartInternalMouseEventArgs events)
        {
            if (!string.IsNullOrEmpty(closeIconId) && events.Target.Contains(closeIconId, StringComparison.InvariantCulture) && !events.Type.Contains("move", StringComparison.InvariantCulture))
            {
                bool isSelectedvalues = true;
                if (chartInstance.AllowMultiSelection)
                {
                    int index = GetIndex(events.Target);
                    dragRectArray.Remove(draggedRect + index);
                    filterArray.Remove(draggedRect + index);
                    if (chartInstance.ParentRect.RectsReference.Count == 0)
                    {
                        dragRectArray.Clear();
                        filterArray.Clear();
                    }

                    if (CurrentMode == SelectionMode.Lasso)
                    {
                        for (int s = 0; s < chartInstance.SeriesContainer.Renderers.Count; s++)
                        {
                            if (selectedLassoPoints.ContainsKey((index, s)))
                            {
                                selectedLassoPoints[(index, s)].ForEach(x => x.IsSelected = false);
                                selectedLassoPoints[(index, s)].Clear();
                            }

                            foreach (List<Point> data in selectedLassoPoints.Values)
                            {
                                if (data.Count != 0)
                                {
                                    isSelectedvalues = false;
                                    data.ForEach(x => x.IsSelected = true);
                                }
                            }
                        }

                        CalculateDragSelectedElements(dragRect, true);
                    }
                    else if (filterArray.Count != 0)
                    {
                        List<Rect> items = filterArray.Values.ToList();
                        foreach (Rect item in items)
                        {
                            if (item != null)
                            {
                                isSelectedvalues = false;
                                CalculateDragSelectedElements(item, true);
                            }
                        }
                    }
                    else
                    {
                        CalculateDragSelectedElements(new Rect(0, 0, 0, 0), true);
                    }
                }
                else
                {
                    RemoveSelectedElements(SelectedDataIndexes, chartInstance.SeriesContainer.Renderers);
                }

                BlurEffect(chartInstance.SeriesContainer.Renderers);
                if (!chartInstance.AllowMultiSelection || isSelectedvalues)
                {
                    rectPoints = null;
                }
            }
        }
    }

    public class SelectionOptions
    {
        public string Id { get; set; } = "Default_Blazor_Selection";

        public Rect DragRect { get; set; } = new Rect();

        public string Stroke { get; set; } = string.Empty;

        public string StrokeWidth { get; set; } = string.Empty;

        public string Fill { get; set; } = "transparent";

        public bool IsLasso { get; set; }

        public string Path { get; set; }

        public CloseOptions Close { get; set; } = new CloseOptions();
    }

    public class CloseOptions
    {
        public PathOptions Path { get; set; }

        public CircleOptions Circle { get; set; }
    }
}