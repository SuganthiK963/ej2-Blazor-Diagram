using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class Scrollbar
    {
        private DateTime previousRequestTime = DateTime.Now;
        internal Scrollbar(SfChart sfchart)
        {
            Component = sfchart;
        }

        internal Scrollbar(SfChart sfChart, ChartAxis axis = null)
        {
            Component = sfChart;
            Axis = axis;
        }

        internal bool IsScrollUI { get; set; }

        internal ChartScrollbar ScrollbarRef { get; set; }

        internal bool IsVertical { get; set; }

        internal double StartX { get; set; }

        internal ScrollbarThemeStyle ScrollbarThemeStyle { get; set; }

        internal double ThumbRectWidth { get; set; }

        internal double ThumbRectX { get; set; }

        private bool isResizeLeft { get; set; }

        private bool isResizeRight { get; set; }

        private double previousXY { get; set; }

        private double previousWidth { get; set; }

        private double previousRectX { get; set; }

        private ValueType valueType { get; set; }

        private VisibleRangeModel scrollRange { get; set; } = new VisibleRangeModel { Max = double.NaN, Min = double.NaN, Interval = double.NaN, Delta = double.NaN };

        private bool isLazyLoad { get; set; }

        private double previousStart { get; set; }

        private double previousEnd { get; set; }

        private double startZoomPosition { get; set; }

        private double startZoomFactor { get; set; }

        private DoubleRange startRange { get; set; }

        private bool scrollStarted { get; set; }

        private string scrollCursor { get; set; } = "auto";

        private double mouseX { get; set; }

        private double mouseY { get; set; }

        private bool isThumbDrag { get; set; }

        internal Dictionary<string, ChartAxis> Axes { get; set; }

        internal ChartAxis Axis { get; set; }

        internal SfChart Component { get; set; }

        internal double ZoomFactor { get; set; }

        internal double ZoomPosition { get; set; }

        internal string SvgObject { get; set; }

        internal double Width { get; set; }

        internal double Height { get; set; }

        private static string WebkitGrabbing(string name)
        {
            return BrowserName() == Constants.MOZILLA ? name : BrowserName() == Constants.MSIE ? "move" : " - webkit-" + name;
        }

        internal static string BrowserName()
        {
            return string.Empty; // Component.Browser.BrowserName;
        }

        internal static bool IsPointer()
        {
            return false; // Component.Browser.IsPointer;
        }

        internal void Render(RenderTreeBuilder builder, bool isScrollExist, double startY)
        {
            if (Component.ZoomingModule != null || (isScrollExist && Axis.ScrollbarSettings.Enable))
            {
                GetDefaults();
            }

            GetTheme();
            builder.OpenComponent<ChartScrollbar>(SvgRendering.Seq++);
            builder.AddAttribute(SvgRendering.Seq++, "chart", Component);
            builder.AddAttribute(SvgRendering.Seq++, "scrollBar", this);
            builder.AddAttribute(SvgRendering.Seq++, "StartY", startY);
            builder.AddComponentReferenceCapture(SvgRendering.Seq++, ins => { ScrollbarRef = (ChartScrollbar)ins; });
            builder.CloseComponent();
        }

        internal void ScrollMouseDown(ChartInternalMouseEventArgs mouseEvent)
        {
            string id = mouseEvent.Target;
            ChartScrollbar elem = ScrollbarRef;
            GetMouseXY(mouseEvent);
            isResizeLeft = IsExist(id, Constants.LEFTCIRCLE) || IsExist(id, Constants.LEFTARROW);
            isResizeRight = IsExist(id, Constants.RIGHTCIRCLE) || IsExist(id, Constants.RIGHTARROW);
            previousXY = (IsVertical && Axis.IsInversed) ? mouseY : IsVertical ? Width - mouseY : Axis.IsInversed ? Width - mouseX : mouseX;
            previousWidth = elem.ThumbRectWidth;
            previousRectX = elem.ThumbRectX;
            startZoomPosition = Axis.ZoomPosition;
            startZoomFactor = Axis.ZoomFactor;
            startRange = Axis.Renderer.VisibleRange;
            scrollStarted = true;
            SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, GetScrollArguments(Constants.ONSCROLLSTART));
            if (IsExist(id, Constants.SCROLLBARTHUMB))
            {
                isThumbDrag = true;
                scrollCursor = WebkitGrabbing("grabbing");
                ScrollbarRef.SetCursor(scrollCursor);
            }
            else if (IsExist(id, Constants.SCROLLBARBACKRECT))
            {
                double currentX = MoveLength(previousXY, previousRectX);
                elem.ThumbRectX = IsWithIn(currentX) ? currentX : elem.ThumbRectX;
                PositionThumb(elem.ThumbRectX, elem.ThumbRectWidth);
                SetZoomFactorPosition(elem.ThumbRectX, elem.ThumbRectWidth, false);
                if (isLazyLoad)
                {
                    ScrollEventArgs scrollStartArgs = CalculateLazyRange(Constants.ONSCROLLSTART, elem.ThumbRectX, elem.ThumbRectWidth, elem.ThumbRectX > this.previousRectX ? Constants.RIGHTMOVE : Constants.LEFTMOVE);
                    if (scrollStartArgs != null)
                    {
                        SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, scrollStartArgs);
                    }
                }
            }
        }

        internal void ScrollMouseMove(ChartInternalMouseEventArgs mouseEventArgs)
        {
            string target = mouseEventArgs.Target;
            ChartScrollbar elem = ScrollbarRef;
            ScrollEventArgs scrollChangedArgs = null;
            GetMouseXY(mouseEventArgs);
            SetCursor(target);
            SetTheme(target);
            double mouseXY = (IsVertical && Axis.IsInversed) ? Width - mouseY : IsVertical ? mouseY : mouseX;
            if (isLazyLoad && (isThumbDrag || isResizeLeft || isResizeRight))
            {
                scrollChangedArgs = CalculateLazyRange(Constants.ONSCROLLCHANGED, elem.ThumbRectX, elem.ThumbRectWidth, previousRectX - elem.ThumbRectX < 0 ? Constants.RIGHTMOVE : Constants.LEFTMOVE);
            }

            if (isThumbDrag)
            {
                Component.AxisContainer.IsScrolling = isThumbDrag;
                mouseXY = IsVertical || Axis.IsInversed ? Width - mouseXY : mouseXY;
                double currentX = elem.ThumbRectX + (mouseXY - previousXY);
                if (mouseXY >= currentX + elem.ThumbRectWidth)
                {
                    SetCursor(target);
                }
                else
                {
                    scrollCursor = WebkitGrabbing("grabbing");
                    ScrollbarRef.SetCursor(scrollCursor);
                }

                if (mouseXY >= 0 && mouseXY <= currentX + elem.ThumbRectWidth)
                {
                    elem.ThumbRectX = IsWithIn(currentX) ? currentX : elem.ThumbRectX;
                    PositionThumb(elem.ThumbRectX, elem.ThumbRectWidth);
                    previousXY = mouseXY;
                    SetZoomFactorPosition(currentX, elem.ThumbRectWidth);
                }
                if ((DateTime.Now - previousRequestTime).TotalMilliseconds > 300)
                {
                    SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, GetScrollArguments(Constants.ONSCROLLCHANGED, Axis.Renderer.VisibleRange, ZoomPosition, ZoomFactor, (scrollChangedArgs != null) ? scrollChangedArgs.CurrentRange : null, Axis.Renderer.VisibleInterval));
                    previousRequestTime = DateTime.Now;
                }
            }
            else if (isResizeLeft || isResizeRight)
            {
                if ((DateTime.Now - previousRequestTime).TotalMilliseconds > 300)
                {
                    ResizeThumb();
                    SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, GetScrollArguments(Constants.ONSCROLLCHANGED, Axis.Renderer.VisibleRange, ZoomPosition, ZoomFactor, (scrollChangedArgs != null) ? scrollChangedArgs.CurrentRange : null, Axis.Renderer.VisibleInterval));
                }
            }
        }

        private ScrollEventArgs CalculateMouseWheelRange(double scrollThumbX, double scrollThumbWidth)
        {
            double zoomFactor = double.NaN, zoomPosition = double.NaN;
            VisibleRangeModel range = scrollRange;
#pragma warning disable CA2000
            ChartAxisScrollbarSettingsRange previousRange = GetStartEnd(previousStart, previousEnd, false);
#pragma warning restore CA2000
            if ((scrollThumbX + scrollThumbWidth + 8) <= Width)
            {
                zoomPosition = (scrollThumbX - 8) / Width;
                zoomFactor = scrollThumbWidth / Width;
            }

            double currentStart = range.Min + (zoomPosition * range.Delta),
            currentEnd = currentStart + (zoomFactor * range.Delta);
            if (!double.IsNaN(currentEnd))
            {
                return new ScrollEventArgs
                {
                    Axis = Axis,
                    CurrentRange = GetStartEnd(currentStart, currentEnd, true),
                    PreviousAxisRange = previousRange
                };
            }

            return null;
        }

        private ScrollEventArgs CalculateLazyRange(string eventName, double scrollThumbX, double scrollThumbWidth, string thumbMove = "")
        {
            double currentScrollWidth = scrollThumbWidth, zoomFactor, zoomPosition, currentStart = double.NaN, currentEnd = double.NaN;
            VisibleRangeModel range = scrollRange;
            if (isResizeRight || thumbMove == Constants.RIGHTMOVE)
            {
                currentScrollWidth = isResizeRight ? currentScrollWidth + 16 : currentScrollWidth;
                zoomFactor = currentScrollWidth / Width;
                zoomPosition = thumbMove == Constants.RIGHTMOVE ? (scrollThumbX + 8) / Width : Axis.ZoomPosition;
                currentStart = thumbMove == Constants.RIGHTMOVE ? (range.Min + (zoomPosition * range.Delta)) : previousStart;
                currentEnd = currentStart + (zoomFactor * range.Delta);
            }
            else if (isResizeLeft || thumbMove == Constants.LEFTMOVE)
            {
                zoomPosition = (scrollThumbX - 8) / Width;
                zoomFactor = currentScrollWidth / Width;
                currentStart = range.Min + (zoomPosition * range.Delta);
                currentStart = currentStart >= range.Min ? currentStart : range.Min;
                currentEnd = thumbMove == Constants.LEFTMOVE ? (currentStart + (zoomFactor * range.Delta)) : previousEnd;
            }
            else if (isThumbDrag)
            {
                zoomPosition = thumbMove == Constants.RIGHTMOVE ? (scrollThumbX + 8) / Width : (scrollThumbX - 8) / Width;
                zoomFactor = ScrollbarRef.ThumbRectWidth / Width;
                currentStart = range.Min + (zoomPosition * range.Delta);
                currentStart = currentStart >= range.Min ? currentStart : range.Min;
                currentEnd = currentStart + (zoomFactor * range.Delta);
            }

            if (!double.IsNaN(currentEnd))
            {
                return new ScrollEventArgs
                {
                    Name = eventName,
                    Axis = Axis,
                    CurrentRange = GetStartEnd(currentStart, currentEnd, true),
                    PreviousAxisRange = GetStartEnd(previousStart, previousEnd, false)
                };
            }

            return null;
        }

        internal void ScrollMouseWheel(ChartMouseWheelArgs wheelEvent)
        {
            mouseX = wheelEvent.MouseX;
            mouseY = wheelEvent.MouseY;
            ChartScrollbar elem = ScrollbarRef;
            ChartAxis axis = Axis;
            double direction = (BrowserName() == "mozilla" && !IsPointer()) ? -wheelEvent.Detail / 3 > 0 ? 1 : -1 : (wheelEvent.WheelDelta / 120) > 0 ? 1 : -1;
            double cumulative = Math.Max(Math.Max(1 / ChartHelper.MinMax(axis.ZoomFactor, 0, 1), 1) + (0.25 * direction), 1);
            DoubleRange range = Axis.Renderer.VisibleRange;
            double zoomPosition = ZoomPosition, zoomFactor = ZoomFactor;
            ScrollEventArgs args;
            if (cumulative >= 1)
            {
                double origin = axis.Renderer.Orientation == Orientation.Horizontal ? mouseX / axis.Renderer.Rect.Width : 1 - (mouseY / axis.Renderer.Rect.Height);
                origin = origin > 1 ? 1 : origin < 0 ? 0 : origin;
                ZoomFactor = (cumulative == 1) ? 1 : ChartHelper.MinMax(1 / cumulative, 0, 1);
                ZoomPosition = (cumulative == 1) ? 0 : axis.ZoomPosition + ((axis.ZoomFactor - ZoomFactor) * origin);
            }

            elem.ThumbRectX = IsWithIn(ZoomPosition * Width) ? ZoomPosition * Width : elem.ThumbRectX;
            IsScrollUI = true;
            PositionThumb(elem.ThumbRectX, elem.ThumbRectWidth);
            if (isLazyLoad)
            {
                SetZoomFactorPosition(elem.ThumbRectX, elem.ThumbRectWidth);
            }

            if (!(axis.ZoomFactor == ZoomFactor && axis.ZoomPosition == ZoomPosition))
            {
                axis.UpdateZoomValues(ZoomFactor, ZoomPosition);
                Component.AxisContainer.IsScrollExist = ZoomFactor != 1 && ZoomPosition != 0;
                Component.UpdateRenderers();
            }

            if (isLazyLoad)
            {
                args = CalculateMouseWheelRange(elem.ThumbRectX, elem.ThumbRectWidth);
                if (args != null && ((args.CurrentRange.Minimum != args.PreviousAxisRange.Minimum) && (args.CurrentRange.Maximum != args.PreviousAxisRange.Maximum)))
                {
                    SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, args);
                }
            }

            if (!isLazyLoad)
            {
                SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, GetScrollArguments(Constants.ONSCROLLCHANGED, range, zoomPosition, zoomFactor, null, Axis.Renderer.VisibleInterval));
            }
        }

        private ChartAxisScrollbarSettingsRange GetStartEnd(object start, object end, bool isCurrentStartEnd)
        {
            if ((valueType == ValueType.DateTime || valueType == ValueType.DateTimeCategory) && isCurrentStartEnd)
            {
                previousStart = (double)start;
                previousEnd = (double)end;
            }
            else if (isCurrentStartEnd)
            {
                previousStart = Math.Round((double)start);
                previousEnd = Math.Ceiling((double)end);
            }

            switch (valueType)
            {
                case ValueType.Double:
                case ValueType.Category:
                case ValueType.Logarithmic:
                    start = Math.Round((double)start);
                    end = Math.Ceiling((double)end);
                    break;
                case ValueType.DateTime:
                case ValueType.DateTimeCategory:
                    start = new DateTime(1970, 1, 1).AddMilliseconds((double)start);
                    end = new DateTime(1970, 1, 1).AddMilliseconds((double)end);
                    break;
            }

            ChartAxisScrollbarSettingsRange range = new ChartAxisScrollbarSettingsRange();
            range.SetMinMax(Convert.ToString(start, null), Convert.ToString(end, null));
            return range;
        }

        private void SetZoomFactorPosition(double currentX, double currentWidth, bool isScrollUI = true)
        {
            IsScrollUI = isScrollUI;
            Component.AxisContainer.IsScrollExist = true;
            double currentScrollWidth = currentX + currentWidth + 9;
            ZoomPosition = (currentX - (currentX - 8.5 <= 0 ? 8.5 : 0)) / (IsVertical ? Axis.Renderer.Rect.Height : Width);
            ZoomFactor = (currentWidth + (currentScrollWidth >= Width ? 9 : 0)) / (IsVertical ? Axis.Renderer.Rect.Height : Width);
            Axis.UpdateZoomValues(ZoomFactor, ZoomPosition);
            Component.UpdateRenderers();
        }

        internal void ScrollMouseUp()
        {
            scrollCursor = Constants.AUTO;
            ScrollbarRef.SetCursor(scrollCursor);
            ScrollEventArgs args;
            StartX = ScrollbarRef.ThumbRectX;
            double currentScrollWidth = StartX + ScrollbarRef.ThumbRectWidth + 9;
            if ((isResizeLeft || isResizeRight) && !isLazyLoad)
            {
                double zoomFactor = (currentScrollWidth >= Width - 1 && (StartX - 8.5) <= 0) ? 1 : ZoomFactor;

                Axis.UpdateZoomValues(zoomFactor, Axis.ZoomPosition);
            }

            if (isLazyLoad)
            {
                double moveLength = previousRectX - StartX;
                if ((moveLength > 0 || moveLength < 0) && isThumbDrag)
                {
                    string thumbMove = moveLength < 0 ? Constants.RIGHTMOVE : Constants.LEFTMOVE;
                    if (thumbMove == Constants.RIGHTMOVE)
                    {
                        StartX = (StartX + Math.Abs(moveLength)) < Width - 8 ? StartX : Width - 8 - ScrollbarRef.ThumbRectWidth;
                    }
                    else
                    {
                        StartX = (StartX + ScrollbarRef.ThumbRectWidth - Math.Abs(moveLength)) > 8 ? StartX : 8;
                    }

                    args = CalculateLazyRange(Constants.ONSCROLLEND, StartX, ScrollbarRef.ThumbRectWidth, thumbMove);
                    if (args != null)
                    {
                        SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, args);
                        scrollStarted = false;
                    }
                }

                if (isResizeLeft || isResizeRight)
                {
                    args = CalculateLazyRange(Constants.ONSCROLLEND, StartX, ScrollbarRef.ThumbRectWidth);
                    if (args != null)
                    {
                        SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, args);
                        scrollStarted = false;
                    }
                }
            }

            isThumbDrag = isResizeLeft = isResizeRight = Component.AxisContainer.IsScrolling = false;
            if (scrollStarted && !isLazyLoad)
            {
                SfChart.InvokeEvent<ScrollEventArgs>(Component.ChartEvents?.OnScrollChanged, GetScrollArguments(Constants.ONSCROLLCHANGED, startRange, startZoomPosition, startZoomFactor, null, Axis.Renderer.VisibleInterval));
                scrollStarted = false;
            }

            ScrollbarRef.UpdateScrollBarPosition(true);
        }

        private void PositionThumb(double currentX, double currentWidth)
        {
            ScrollbarRef.PositionThumb(currentX, currentWidth, "translate(" + (currentX + (currentWidth / 2) + ((IsVertical ? 1 : -1) * 5)).ToString(CultureInfo.InvariantCulture) + ',' + (IsVertical ? "10" : "5") + ") rotate(" + (IsVertical ? "180" : "0") + ')');
        }

        private bool IsWithIn(double currentX)
        {
            return currentX - 8 >= 0 && currentX + ScrollbarRef.ThumbRectWidth + 8 <= Width;
        }

        private double MoveLength(double mouseXY, double thumbX, double circleRadius = 8)
        {
            double moveLength = (10 / 100) * (Width - (circleRadius * 2));
            if (mouseXY < thumbX)
            {
                moveLength = thumbX - (thumbX - moveLength > circleRadius ? moveLength : circleRadius);
            }
            else
            {
                moveLength = thumbX + (thumbX + ScrollbarRef.ThumbRectWidth + moveLength < Width - circleRadius ? moveLength : circleRadius);
            }

            return moveLength;
        }

        private ScrollEventArgs GetScrollArguments(string eventName, DoubleRange range = new DoubleRange(), double zoomPosition = double.NaN, double zoomFactor = double.NaN, ChartAxisScrollbarSettingsRange currentRange = null, double previousInterval = double.NaN)
        {
            return new ScrollEventArgs
            {
                Axis = Axis,
                Name = eventName,
                Range = ChartHelper.GetVisibleRangeModel(Axis.Renderer.VisibleRange, Axis.Renderer.VisibleInterval),
                ZoomFactor = Axis.ZoomFactor,
                ZoomPosition = Axis.ZoomPosition,
                PreviousRange = ChartHelper.GetVisibleRangeModel(range, previousInterval),
                PreviousZoomFactor = zoomFactor,
                PreviousZoomPosition = zoomPosition,
                CurrentRange = currentRange
            };
        }

        private void GetMouseXY(ChartInternalMouseEventArgs mouseEvent)
        {
            mouseX = mouseEvent.MouseX;
            mouseY = mouseEvent.MouseY;
        }

        private void SetTheme(string id)
        {
            bool isLeftHover = id.Contains(Constants.LEFTCIRCLE, StringComparison.InvariantCulture) || id.Contains(Constants.LEFTARROW, StringComparison.InvariantCulture), isRightHover = id.Contains(Constants.RIGHTCIRCLE, StringComparison.InvariantCulture) || id.Contains(Constants.RIGHTARROW, StringComparison.InvariantCulture);
            ScrollbarThemeStyle style = ScrollbarThemeStyle;
            object leftArrowEleId;
            ScrollbarRef.LeftArrowAttributes.TryGetValue("id", out leftArrowEleId);
            bool isAxis = IsCurrentAxis(id, Convert.ToString(leftArrowEleId, null));
            isLeftHover = isLeftHover && isAxis;
            isRightHover = isRightHover && isAxis;
            string leftCircleStyle = isLeftHover ? style.CircleHover : style.Circle, rightCircleStyle = isRightHover ? style.CircleHover : style.Circle, leftArrowStyle = null, rightArrowStyle = null;
            if (Component.Theme == Theme.HighContrastLight)
            {
                leftArrowStyle = isLeftHover ? style.ArrowHover : style.Arrow;
                rightArrowStyle = isRightHover ? style.ArrowHover : style.Arrow;
            }

            ScrollbarRef.SetTheme(leftCircleStyle, rightCircleStyle, leftArrowStyle, rightArrowStyle);
        }

        private void SetCursor(string id)
        {
            scrollCursor = id.Contains(Constants.SCROLLBARTHUMB, StringComparison.InvariantCulture) || id.Contains(Constants.GRIPCIRCLE, StringComparison.InvariantCulture) ? WebkitGrabbing("grab") : (id.Contains(Constants.CIRCLE, StringComparison.InvariantCulture) || id.Contains(Constants.ARROW, StringComparison.InvariantCulture)) ? IsVertical ? Constants.NS_RESIZE : Constants.EW_RESIZE : Constants.AUTO;
            ScrollbarRef.SetCursor(scrollCursor);
        }

        private void ResizeThumb()
        {
            double currentWidth, thumbX = previousRectX,
            mouseXY = (IsVertical && Axis.IsInversed) ? mouseY : IsVertical ? Width - mouseY : Axis.IsInversed ? Width - mouseX : mouseX,
            diff = Math.Abs(previousXY - mouseXY);
            if (isResizeLeft && mouseXY >= 0)
            {
                double currentX = thumbX + (mouseXY > previousXY ? diff : -diff);
                currentWidth = currentX - 8 >= 0 ? previousWidth + (mouseXY > previousXY ? -diff : diff) : previousWidth;
                currentX = currentX - 8 >= 0 ? currentX : thumbX;
                if (currentWidth >= 40 && mouseXY < currentX + currentWidth)
                {
                    ScrollbarRef.ThumbRectX = previousRectX = currentX;
                    ScrollbarRef.ThumbRectWidth = previousWidth = currentWidth;
                    previousXY = mouseXY;
                    PositionThumb(currentX, currentWidth);
                    SetZoomFactorPosition(currentX, currentWidth, false);
                }
            }
            else if (isResizeRight)
            {
                currentWidth = mouseXY >= 40 + ScrollbarRef.ThumbRectX && mouseXY <= Width - 8 ? mouseXY - ScrollbarRef.ThumbRectX : previousWidth;
                ScrollbarRef.ThumbRectWidth = previousWidth = currentWidth;
                previousXY = mouseXY;
                PositionThumb(StartX, currentWidth);
                SetZoomFactorPosition(StartX, currentWidth, false);
                if (!isLazyLoad)
                {
                    SetZoomFactorPosition(StartX, currentWidth, false);
                }
            }
        }

        private void GetTheme()
        {
            ScrollbarThemeStyle = ChartHelper.GetScrollbarThemeColor(Component.Theme);
        }

        private void GetDefaults()
        {
            ChartAxis axis = Axis;
            if (Axis.ScrollbarSettings.Enable)
            {
                isLazyLoad = true;
                GetLazyDefaults(axis);
            }

            IsVertical = axis.Renderer.Orientation == Orientation.Vertical;
            ZoomFactor = isLazyLoad ? ZoomFactor : axis.ZoomFactor;
            ZoomPosition = isLazyLoad ? ZoomPosition : axis.ZoomPosition;
            double currentWidth = ZoomFactor * (IsVertical ? axis.Renderer.Rect.Height : axis.Renderer.Rect.Width);
            currentWidth = currentWidth > 40 ? currentWidth : 40;
            Width = IsVertical ? axis.Renderer.Rect.Height : axis.Renderer.Rect.Width;
            Height = 16;
            double currentX = ZoomPosition * (IsVertical ? axis.Renderer.Rect.Height : Width),
            minThumbX = Width - 48;
            ThumbRectX = currentX > minThumbX ? minThumbX : currentX < 8 ? 8 : currentX;
            ThumbRectWidth = ((currentWidth + ThumbRectX) < Width - 16) ? currentWidth : Width - ThumbRectX - 8;
        }

        private void GetLazyDefaults(ChartAxis axis)
        {
#pragma warning disable CA1062
            ValueType valueType = axis.ValueType;
#pragma warning restore CA1062
            this.valueType = valueType = (!string.IsNullOrEmpty(axis.ScrollbarSettings.Range.Minimum) || !string.IsNullOrEmpty(axis.ScrollbarSettings.Range.Maximum)) && !ChartHelper.IsNaNOrZero(axis.ScrollbarSettings.PointsLength) ? ValueType.Double : valueType;
            ChartAxisScrollbarSettingsRange range = axis.ScrollbarSettings.Range;
            DoubleRange visibleRange = axis.Renderer.VisibleRange;
            double start = visibleRange.Start, end = visibleRange.End, pointsLength = axis.ScrollbarSettings.PointsLength, zoomFactor, zoomPosition;
            switch (valueType)
            {
                case ValueType.Double:
                case ValueType.Category:
                case ValueType.Logarithmic:
                    start = !string.IsNullOrEmpty(range.Minimum) ? Convert.ToDouble(range.Minimum, null) : !ChartHelper.IsNaNOrZero(pointsLength) ? 0 : visibleRange.Start;
                    end = !string.IsNullOrEmpty(range.Maximum) ? Convert.ToDouble(range.Maximum, null) : !ChartHelper.IsNaNOrZero(pointsLength) ? (pointsLength - 1) : visibleRange.End;
                    break;
                case ValueType.DateTime:
                case ValueType.DateTimeCategory:
                    start = !string.IsNullOrEmpty(range.Minimum) ? ChartHelper.GetTime(DateTime.Parse(range.Minimum, null)) : visibleRange.Start;
                    end = !string.IsNullOrEmpty(range.Maximum) ? ChartHelper.GetTime(DateTime.Parse(range.Maximum, null)) : visibleRange.End;
                    break;
            }

            start = Math.Min(start, visibleRange.Start);
            end = Math.Max(end, visibleRange.End);
            zoomFactor = (visibleRange.End - visibleRange.Start) / (end - start);
            zoomPosition = (visibleRange.Start - start) / (end - start);
            ZoomFactor = !string.IsNullOrEmpty(range.Minimum) || !string.IsNullOrEmpty(range.Maximum) ? zoomFactor : (Axis.Renderer.MaxPointLength / axis.ScrollbarSettings.PointsLength);
            ZoomPosition = !string.IsNullOrEmpty(range.Minimum) || !string.IsNullOrEmpty(range.Maximum) ? zoomPosition : axis.ZoomPosition;
            scrollRange.Min = start;
            scrollRange.Max = end;
            scrollRange.Delta = end - start;
            previousStart = visibleRange.Start;
            previousEnd = visibleRange.End;
        }

#pragma warning disable CA1822
        internal void InjectTo(ChartAxisRenderer axisRenderer, SfChart component)
        {
            axisRenderer.ZoomingScrollBar = new Scrollbar(component, axisRenderer.Axis);
        }

        private bool IsExist(string id, string match)
        {
            return id.Contains(match, StringComparison.InvariantCulture);
        }

        private bool IsCurrentAxis(string id, string eleId)
        {
            return id.Split('_')[2] == eleId.Split('_')[2];
        }

        internal void Destroy()
#pragma warning restore CA1822
        {
            // TODO: Need to complete the method here.
            // elements = new List<string>();
        }

        internal void Dispose()
        {
            Component = null;
            ScrollbarRef = null;
            Axis = null;
        }
    }
}