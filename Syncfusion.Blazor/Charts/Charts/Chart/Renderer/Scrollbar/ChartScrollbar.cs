using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Internal;
using System;
using System.Globalization;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class ChartScrollbar : OwningComponentBase
    {
        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public SfChart Chart { get; set; }

        [Parameter]
        public double StartY { get; set; }

        private const string SPACE = " ";

        /// <summary>
        /// Sets and gets the chart scrollbar.
        /// </summary>
        [Parameter]
        public Scrollbar ScrollBar { get; set; }

        internal Dictionary<string, object> SvgAttributes { get; set; }

        internal Dictionary<string, object> RootGroupAttributes { get; set; }

        internal Dictionary<string, object> BackRectGroupAttributes { get; set; }

        internal Dictionary<string, object> ThumbRectGroupAttributes { get; set; }

        internal Dictionary<string, object> BackRectAttributes { get; set; }

        internal Dictionary<string, object> ThumbRectAttributes { get; set; }

        internal Dictionary<string, object> LeftCircleAttributes { get; set; }

        internal Dictionary<string, object> RightCircleAttributes { get; set; }

        internal Dictionary<string, object> ThumbShadowGroupAttributes { get; set; }

        internal Dictionary<string, object> LeftArrowAttributes { get; set; }

        internal Dictionary<string, object> RightArrowAttributes { get; set; }

        internal Dictionary<string, object> GripCircleGroupAttributes { get; set; }

        internal Dictionary<string, object> GripCircle1Attributes { get; set; }

        internal Dictionary<string, object> GripCircle2Attributes { get; set; }

        internal Dictionary<string, object> GripCircle3Attributes { get; set; }

        internal Dictionary<string, object> GripCircle4Attributes { get; set; }

        internal Dictionary<string, object> GripCircle5Attributes { get; set; }

        internal Dictionary<string, object> GripCircle6Attributes { get; set; }

        internal string ChartId { get; set; }

        internal double ThumbRectWidth { get; set; }

        internal double ThumbRectX { get; set; }

        internal string LeftCircleId { get; set; }

        internal string RightCircleId { get; set; }

        private string leftArrowId { get; set; }

        private string rightArrowId { get; set; }

        private string gripCircleId { get; set; }

        private string scrollCursor { get; set; } = "auto";

        private string scrollStyle { get; set; }

        private string sliderId { get; set; }

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        protected override Task OnInitializedAsync()
        {
            ChartId = Chart.ID + "_";
            UpdateInternalProps();
            return base.OnInitializedAsync();
        }

        internal bool IsScrollbarDisposed
        {
            get { return IsDisposed; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void UpdateInternalProps()
        {
            SetThumbSize(ScrollBar);
            SetSvgAttributes(ScrollBar);
            SetRootGroupAttributes(ScrollBar);
            SetBackThumbGroupAttributes();
            SetBackRectAttributes(ScrollBar);
            SetThumbRectAttributes(ScrollBar);
            SetCircleAttributes(ScrollBar);
            SetArrowAttributes(ScrollBar);
            SetThumbGripAttributes(ScrollBar);
        }

        private void SetThumbSize(Scrollbar scrollBar)
        {
            ThumbRectX = scrollBar.ThumbRectX;
            ThumbRectWidth = scrollBar.ThumbRectWidth;
        }

        private void SetThumbGripAttributes(Scrollbar scrollBar)
        {
            ScrollbarThemeStyle style = scrollBar.ScrollbarThemeStyle;
            string transform = "translate(" + Convert.ToString((ThumbRectX + (ThumbRectWidth / 2)) + ((scrollBar.IsVertical ? 1 : -1) * 5), culture) +
                ',' + (scrollBar.IsVertical ? "10" : "5") + ") rotate(" + (scrollBar.IsVertical ? "180" : "0") + ')';
            string id = ChartId + "scrollBar_gripCircle_" + scrollBar.Axis.GetName();
            gripCircleId = id;
            GripCircleGroupAttributes = new Dictionary<string, object>()
            {
                { "id", id },
                { "transform", transform }
            };
            GripCircle1Attributes = GetGripCircleAttributes(1, style.Grip, 0, 0);
            GripCircle2Attributes = GetGripCircleAttributes(2, style.Grip, 5, 0);
            GripCircle3Attributes = GetGripCircleAttributes(3, style.Grip, 10, 0);
            GripCircle4Attributes = GetGripCircleAttributes(4, style.Grip, 0, 5);
            GripCircle5Attributes = GetGripCircleAttributes(5, style.Grip, 5, 5);
            GripCircle6Attributes = GetGripCircleAttributes(6, style.Grip, 10, 5);
        }

        private Dictionary<string, object> GetGripCircleAttributes(int i, string color, int cx, int cy)
        {
            return new Dictionary<string, object>()
            {
                { "id", ChartId + "scrollBar_gripCircle" + i + '_' + ScrollBar.Axis.GetName() },
                { "cx", cx },
                { "cy", cy },
                { "stroke-width", 1 },
                { "opacity", 1 },
                { "stroke", color },
                { "fill", color },
                { "r", 1 }
            };
        }

        private void SetArrowAttributes(Scrollbar scrollBar)
        {
            ScrollbarThemeStyle style = scrollBar.ScrollbarThemeStyle;
            string leftDirection, rightDirection;
            SetArrowDirection(ThumbRectX, ThumbRectWidth, scrollBar.Height, out leftDirection, out rightDirection);
            leftArrowId = ChartId + "scrollBar_leftArrow_" + scrollBar.Axis.GetName();
            LeftArrowAttributes = new Dictionary<string, object>()
            {
                { "id", leftArrowId },
                { "d", leftDirection },
                { "stroke-width", 1 },
                { "opacity", 1 },
                { "stroke", style.Arrow },
                { "fill", style.Arrow }
            };
            rightArrowId = ChartId + "scrollBar_rightArrow_" + scrollBar.Axis.GetName();
            RightArrowAttributes = new Dictionary<string, object>()
            {
                { "id", rightArrowId },
                { "d", rightDirection },
                { "stroke-width", 1 },
                { "opacity", 1 },
                { "stroke", style.Arrow },
                { "fill", style.Arrow }
            };
        }

#pragma warning disable CA1822
        internal void SetArrowDirection(double thumbRectX, double thumbRectWidth, double height, out string leftDirection, out string rightDirection)
#pragma warning restore CA1822
        {
            leftDirection = "M " + Convert.ToString((thumbRectX - 4) + 1, culture) + SPACE + Convert.ToString(height / 2, culture) + SPACE + "L " + Convert.ToString(thumbRectX + 2, culture) + SPACE + 11 + SPACE + "L " + Convert.ToString(thumbRectX + 2, culture) + SPACE + 5 + " Z";
            rightDirection = "M " + Convert.ToString((thumbRectX + thumbRectWidth + 4) - 0.5, culture) + SPACE + Convert.ToString(height / 2, culture) + SPACE + "L " + Convert.ToString(thumbRectX + thumbRectWidth - 2, culture) + SPACE + "11.5" + SPACE + "L " + Convert.ToString(thumbRectX + thumbRectWidth - 2, culture) + SPACE + "4.5" + " Z";
        }

        private void SetCircleAttributes(Scrollbar scrollBar)
        {
            ScrollbarThemeStyle style = scrollBar.ScrollbarThemeStyle;
            LeftCircleId = ChartId + "scrollBar_leftCircle_" + scrollBar.Axis.GetName();
            LeftCircleAttributes = new Dictionary<string, object>()
            {
                { "id", LeftCircleId },
                { "cx", ThumbRectX.ToString(culture) },
                { "cy", (scrollBar.Height / 2).ToString(culture) },
                { "stroke-width", 1 },
                { "stroke", style.Circle },
                { "fill", style.Circle },
                { "r", 8 }
            };
            RightCircleId = ChartId + "scrollBar_rightCircle_" + scrollBar.Axis.GetName();
            RightCircleAttributes = new Dictionary<string, object>()
            {
                { "id", RightCircleId },
                { "cx", (ThumbRectX + ThumbRectWidth).ToString(culture) },
                { "cy", (scrollBar.Height / 2).ToString(culture) },
                { "stroke-width", 1 },
                { "stroke", style.Circle },
                { "fill", style.Circle },
                { "r", 8 }
            };
            ThumbShadowGroupAttributes = new Dictionary<string, object>()
            {
                { "id", ChartId + scrollBar.Axis.GetName() + "_thumb_shadow" }
            };
        }

        private void SetThumbRectAttributes(Scrollbar scrollBar)
        {
            scrollBar.StartX = ThumbRectX;
            ScrollbarThemeStyle style = scrollBar.ScrollbarThemeStyle;
            sliderId = ChartId + "scrollBarThumb_" + scrollBar.Axis.GetName();
            ThumbRectAttributes = new Dictionary<string, object>()
            {
                { "id", sliderId },
                { "fill", style.Thumb },
                { "stroke-width", 1 },
                { "stroke", Constants.TRANSPARENT },
                { "opacity", 1 },
                { "rx", 0 },
                { "ry", 0 },
                { "x", ThumbRectX.ToString(culture) },
                { "y", 0 },
                { "height", scrollBar.Height.ToString(culture) },
                { "width", ThumbRectWidth.ToString(culture) }
            };
        }

        private void SetBackRectAttributes(Scrollbar scrollBar)
        {
            ScrollbarThemeStyle style = scrollBar.ScrollbarThemeStyle;
            BackRectAttributes = new Dictionary<string, object>()
            {
                { "id", ChartId + "scrollBarBackRect_" + scrollBar.Axis.GetName() },
                { "fill", style.BackRect },
                { "stroke-width", 1 },
                { "stroke", style.BackRect },
                { "opacity", 1 },
                { "rx", 0 },
                { "ry", 0 },
                { "x", 0 },
                { "y", 0 },
                { "height", scrollBar.Height.ToString(culture) },
                { "width", scrollBar.Width.ToString(culture) },
            };
        }

        private void SetBackThumbGroupAttributes()
        {
            BackRectGroupAttributes = new Dictionary<string, object>()
            {
                { "id", ChartId + "scrollBar_backRect_" + ScrollBar.Axis.GetName() }
            };
            ThumbRectGroupAttributes = new Dictionary<string, object>()
            {
                { "id", ChartId + "scrollBar_thumb_" + ScrollBar.Axis.GetName() },
                { "transform", "translate(0,0)" }
            };
        }

        private void SetSvgAttributes(Scrollbar scrollbar)
        {
            Rect rect = scrollbar.Axis.Renderer.Rect;
            bool isHorizontalAxis = scrollbar.Axis.Renderer.Orientation == Orientation.Horizontal;
            bool enablePadding = false;
            double markerHeight = 5;
            foreach (ChartSeries tempSeries in scrollbar.Axis.Renderer.Series)
            {
                if (tempSeries.Marker.Height > markerHeight)
                {
                    markerHeight = tempSeries.Marker.Height;
                }
            }

            string id = scrollbar.Component.ID + "_scrollBar_svg" + scrollbar.Axis.GetName();
            scrollStyle = "position: absolute; top: " + ((scrollbar.Axis.OpposedPosition && isHorizontalAxis ? -16 : (enablePadding ? markerHeight : 0)) + StartY + rect.Y).ToString(culture) + "px;left: " +
            (((scrollbar.Axis.OpposedPosition && !isHorizontalAxis ? 16 : 0) + rect.X) - (scrollbar.IsVertical ? scrollbar.Height : 0)).ToString(culture) + "px;";
            SvgAttributes = new Dictionary<string, object>
            {
                { "id", id },
                { "width", (scrollbar.IsVertical ? scrollbar.Height : scrollbar.Width).ToString(culture) },
                { "height", (scrollbar.IsVertical ? scrollbar.Width : scrollbar.Height).ToString(culture) },
                { "style", scrollStyle + " cursor:" + scrollCursor + ";" },
                { Constants.ONMOUSEDOWN, Constants.SCROLLMOUSEDOWN },
                { Constants.ONTOUCHSTART, Constants.SCROLLMOUSEDOWN },
                { Constants.ONMOUSEMOVE, Constants.SCROLLMOUSEMOVE },
                { Constants.ONTOUCHMOVE, Constants.SCROLLMOUSEMOVE },
                { Constants.ONMOUSEUP, Constants.SCROLLMOUSEUP },
                { Constants.ONTOUCHEND,  Constants.SCROLLMOUSEUP },
                { Constants.ONMOUSEWHEEL, Constants.SCROLLMOUSEWHEEL },
                { Constants.ONWHEEL, Constants.SCROLLMOUSEWHEEL }
            };
            scrollbar.SvgObject = id;
        }

        internal void SetCursor(string cursorStyle)
        {
            scrollCursor = cursorStyle;
            SfBaseUtils.UpdateDictionary("style", scrollStyle + " cursor:" + cursorStyle + ";", SvgAttributes);
            InvokeAsync(StateHasChanged);
        }

        private void SetRootGroupAttributes(Scrollbar scrollBar)
        {
            string transform = "translate(" + ((scrollBar.IsVertical && scrollBar.Axis.IsInversed) ? scrollBar.Height : scrollBar.Axis.IsInversed ?
                scrollBar.Width : 0).ToString(culture) + "," + (scrollBar.IsVertical && scrollBar.Axis.IsInversed ? 0 : scrollBar.Axis.IsInversed ?
                scrollBar.Height : scrollBar.IsVertical ? scrollBar.Width : 0).ToString(culture) + ") rotate(" + (scrollBar.IsVertical && scrollBar.Axis.IsInversed ?
                "90" : scrollBar.IsVertical ? "270" : scrollBar.Axis.IsInversed ? "180" : "0") + ")";
            RootGroupAttributes = new Dictionary<string, object>()
            {
                { "id", ChartId + "scrollBar_" + scrollBar.Axis.GetName() },
                { "transform", transform },
            };
        }

        internal void SetTheme(string leftCircleStyle, string rightCircleStyle, string leftArrowStyle = null, string rightArrowStyle = null)
        {
            SfBaseUtils.UpdateDictionary("fill", leftCircleStyle, LeftCircleAttributes);
            SfBaseUtils.UpdateDictionary("stroke", leftCircleStyle, LeftCircleAttributes);
            SfBaseUtils.UpdateDictionary("fill", rightCircleStyle, RightCircleAttributes);
            SfBaseUtils.UpdateDictionary("stroke", rightCircleStyle, RightCircleAttributes);
            if (leftArrowStyle != null && rightArrowStyle != null)
            {
                SfBaseUtils.UpdateDictionary("fill", leftArrowStyle, LeftArrowAttributes);
                SfBaseUtils.UpdateDictionary("stroke", leftArrowStyle, LeftArrowAttributes);
                SfBaseUtils.UpdateDictionary("fill", rightArrowStyle, RightArrowAttributes);
                SfBaseUtils.UpdateDictionary("stroke", rightArrowStyle, RightArrowAttributes);
            }
        }

        internal void PositionThumb(double currentX, double currentWidth, string transform)
        {
            SfBaseUtils.UpdateDictionary("x", currentX.ToString(culture), ThumbRectAttributes);
            SfBaseUtils.UpdateDictionary("width", currentWidth.ToString(culture), ThumbRectAttributes);
            SfBaseUtils.UpdateDictionary("cx", currentX.ToString(culture), LeftCircleAttributes);
            SfBaseUtils.UpdateDictionary("cx", (currentX + currentWidth).ToString(culture), RightCircleAttributes);
            string leftArrow, rightArrow;
            SetArrowDirection(currentX, currentWidth, ScrollBar.Height, out leftArrow, out rightArrow);
            SfBaseUtils.UpdateDictionary("d", leftArrow, LeftArrowAttributes);
            SfBaseUtils.UpdateDictionary("d", rightArrow, RightArrowAttributes);
            SfBaseUtils.UpdateDictionary("transform", transform, GripCircleGroupAttributes);
            UpdateScrollBarPosition();
            InvokeAsync(StateHasChanged);
        }

        internal void UpdateScrollBarPosition(bool shouldUpdate = false)
        {
            Rect rect = ScrollBar.Axis.Renderer.Rect;
            bool isHorizontalAxis = ScrollBar.Axis.Renderer.Orientation == Orientation.Horizontal;
            bool enablePadding = false;
            scrollStyle = "position: absolute; top: " + ((ScrollBar.Axis.OpposedPosition && isHorizontalAxis ? -16 : (enablePadding ? 5 : 0)) + StartY + rect.Y).ToString(culture) + "px;left: " +
            (((ScrollBar.Axis.OpposedPosition && !isHorizontalAxis ? 16 : 0) + rect.X) - (ScrollBar.IsVertical ? ScrollBar.Height : 0)).ToString(culture) + "px;";
            SfBaseUtils.UpdateDictionary("style", scrollStyle + " cursor:" + scrollCursor + ";", SvgAttributes);
            if (shouldUpdate)
            {
                InvokeAsync(StateHasChanged);
            }
        }
    }
}