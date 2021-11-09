using System.ComponentModel;
using Microsoft.JSInterop;
using System;
using Syncfusion.Blazor.Internal;
using System.Threading.Tasks;
using System.Drawing;
using Syncfusion.Blazor.Sparkline.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue>
    {
        private double x;
        private double y;
        private string id;

        /// <summary>
        /// JS interopt while mouse move.
        /// </summary>
        /// <param name="mouseX">Represents the X axis of mouse.</param>
        /// <param name="mouseY">Represents the Y axis of mouse.</param>
        /// <param name="rectTop">Represents the top padding of element.</param>
        /// <param name="rectLeft">Represents the left padding of element.</param>
        /// <param name="svgTop">Represents the top padding of SVG element.</param>
        /// <param name="svgLeft">Represents the left padding of SVG element.</param>
        /// <param name="id">Represents the identification of element.</param>
        /// <param name="isIE">Represents whether the browser type is Internet Explorer.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnMouseMove(double mouseX, double mouseY, double rectTop, double rectLeft, double svgTop, double svgLeft, string id, bool isIE)
        {
            y = (mouseY - rectTop) - Math.Max(svgTop - rectTop, 0);
            x = (mouseX - rectLeft) - Math.Max(svgLeft - rectLeft, 0);
            this.id = id;
            if (Tooltip != null && !string.IsNullOrEmpty(id) && TooltipSettings != null && (TooltipSettings.Visible || (TooltipSettings.TrackLineSettings != null && TooltipSettings.TrackLineSettings.Visible)))
            {
                Tooltip.Rendertooltip(x, y, id, isIE);
            }
            else if (Tooltip != null)
            {
                Tooltip.RemoveTooltip();
            }
        }

        private PointRegion IsPointRegion(double mouseX, double mouseY, string elementId)
        {
            string[] id = elementId.Split("_");
            if (id[id.Length - 1].Equals(Type.ToString(), comparison) || elementId.Contains(ID + "_Sparkline_Marker_", comparison))
            {
                int index = elementId.Contains(ID + "_Sparkline_Marker_", comparison) ? Convert.ToInt32(id[id.Length - 1], null) : -1;
                if (index == -1 && (Type == SparklineType.Line || Type == SparklineType.Area))
                {
                    for (int i = 0; i < VisiblePoints.Count; i++)
                    {
                        if (SparklineHelper.WithInBounds(mouseX, mouseY, new RectInfo(10, 10, VisiblePoints[i].X - 5, VisiblePoints[i].Y - 5)))
                        {
                            return new PointRegion() { IsPointRegion = true, PointIndex = i };
                        }
                    }
                }

                return new PointRegion() { IsPointRegion = true, PointIndex = index };
            }

            return new PointRegion() { IsPointRegion = false, PointIndex = -1 };
        }

        /// <summary>
        /// JS interopt while mouse leave the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnMouseLeave()
        {
            if (Tooltip != null)
            {
                Tooltip.RemoveTooltip();
            }
        }

        /// <summary>
        /// JS interopt while mouse click on the component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async void OnClick()
        {
            PointRegion pointClick = IsPointRegion(x, y, id);
            if (pointClick.IsPointRegion)
            {
                await SfBaseUtils.InvokeEvent<PointRegionEventArgs>(Events?.OnPointRegionMouseClick, new PointRegionEventArgs() { PointIndex = pointClick.PointIndex });
            }
        }

        /// <summary>
        /// JS interopt while resize the window.
        /// </summary>
        /// <exclude />
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public async Task OnResize()
        {
            SparklineResizeEventArgs resizeArgs = new SparklineResizeEventArgs()
            {
                PreviousSize = new PointF((float)AvailableSize.Height, (float)AvailableSize.Width),
                CurrentSize = new PointF(0, 0)
            };
            await Refresh();
            resizeArgs.CurrentSize = new PointF((float)AvailableSize.Width, (float)AvailableSize.Height);
            await SfBaseUtils.InvokeEvent<SparklineResizeEventArgs>(Events?.OnResizing, resizeArgs);
        }
    }
}