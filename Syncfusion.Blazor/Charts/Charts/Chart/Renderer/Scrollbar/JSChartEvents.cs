using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfChart : SfDataBoundComponent, ISubcomponentTracker
    {
        private DateTime threshold;
        private bool isLayoutChange;

        internal bool IsPointMouseDown { get; set; }

        private Scrollbar GetAxisScrollbar(string id)
        {
            string axisName;
            string[] splitId;
            if (!id.Contains("_scrollBar_svg", StringComparison.InvariantCulture))
            {
                splitId = id.Split("_");
                axisName = splitId[splitId.Length - 1];
            }
            else
            {
                splitId = id.Split("_scrollBar_svg");
                axisName = splitId[splitId.Length - 1];
            }

            foreach (ChartAxisRenderer axis in AxisContainer.Renderers)
            {
                if (axisName == axis.Axis.GetName())
                {
                    return axis.ZoomingScrollBar;
                }
            }

            return null;
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ScrollMouseDown(ChartInternalMouseEventArgs args)
        {
            GetAxisScrollbar(args?.Target).ScrollMouseDown(args);
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ScrollMouseMove(ChartInternalMouseEventArgs args)
        {
            GetAxisScrollbar(args?.Target).ScrollMouseMove(args);
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ScrollMouseUp(ChartInternalMouseEventArgs args)
        {
            GetAxisScrollbar(args?.Target).ScrollMouseUp();
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void ScrollMouseWheel(ChartMouseWheelArgs args)
        {
            if (!ZoomSettings.EnableMouseWheelZooming)
            {
                GetAxisScrollbar(args?.Target).ScrollMouseWheel(args);
            }
        }

        private void IsTouchEnabled(ChartInternalMouseEventArgs args)
        {
            IsTouch = args.Type.Contains("touch", StringComparison.InvariantCulture) ? true : args.PointerType == "touch" || args.PointerType == "2";
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseDown(ChartInternalMouseEventArgs args)
        {
            if (args == null || isLayoutChange)
            {
                return;
            }

            IsTouchEnabled(args);
            double offset = Browser != null && Browser.IsDevice ? 20 : 30;
            MouseDownX = PreviousMouseMoveX = args.MouseX;
            MouseDownY = PreviousMouseMoveY = args.MouseY;
            if (IsTouch)
            {
                IsDoubleTap = DateTime.Now < threshold && !args.Target.Contains(ID + "_Zooming_", StringComparison.InvariantCulture) &&
                    (MouseDownX - offset >= MouseX || MouseDownX + offset >= MouseX) &&
                    (MouseDownY - offset >= MouseY || MouseDownY + offset >= MouseY) &&
                    (MouseX - offset >= MouseDownX || MouseX + offset >= MouseDownX) &&
                    (MouseY - offset >= MouseDownY || MouseY + offset >= MouseDownY);
            }

            MouseDown?.Invoke(this, args);
            DataEditingModule?.PointMouseDown();
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseMove(ChartInternalMouseEventArgs args)
        {
            if (args == null || isLayoutChange)
            {
                return;
            }

            IsTouchEnabled(args);
            MouseX = args.MouseX;
            MouseY = args.MouseY;
            MouseMove?.Invoke(this, args);
            TooltipModule?.MouseMoveHandler();
            MarkerExplode?.MarkerMove(false);
            crosshairModule?.MouseMoveHandler(args);
            LegendRenderer?.MouseMove(args);
            // TODO: Need to remove  && !IsStockChart condition
            if (!IsTouch && !IsStockChart)
            {
                TitleTooltip(args);
                AxisTooltip(args);
            }

            DataEditingModule?.PointMouseMove();
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseEnd(ChartInternalMouseEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            IsTouchEnabled(args);
            IsChartDrag = false;
            MouseX = args.MouseX;
            MouseY = args.MouseY;

            // TODO: Need to remove  && !IsStockChart condition
            if (IsTouch && !IsStockChart)
            {
                TitleTooltip(args);
                AxisTooltip(args);
                TooltipModule?.MouseLeaveHandler();
                threshold = DateTime.Now.AddMilliseconds(300);
            }

            crosshairModule?.MouseUpHandler();
            MouseUp?.Invoke(this, args);
            DataEditingModule?.PointMouseUp();
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseClick(ChartInternalMouseEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            IsTouchEnabled(args);
            MouseClick?.Invoke(this, args);
            LegendRenderer?.Click(args);
            if (ChartEvents?.OnPointClick != null)
            {
                TriggerPointEvent(Constants.ONPOINTCLICK, ChartEvents.OnPointClick, args);
            }

            if (ChartEvents?.OnMultiLevelLabelClick != null)
            {
                TriggerMultilevelLabelClick(args);
            }

            if (ChartEvents?.OnAxisLabelClick != null)
            {
                TriggerAxisLabelClickEvent(args);
            }
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseRightClick(ChartInternalMouseEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            IsTouchEnabled(args);
            if (ChartEvents?.OnPointClick != null)
            {
                TriggerPointEvent(Constants.ONPOINTCLICK, ChartEvents.OnPointClick, args, true);
            }
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseLeave(ChartInternalMouseEventArgs args)
        {
            if (args == null)
            {
                return;
            }

            IsTouchEnabled(args);
            TooltipModule?.MouseLeaveHandler();
            crosshairModule?.MouseLeaveHandler();
            MouseCancel?.Invoke(this, args);
            IsChartDrag = IsPointMouseDown = false;
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartMouseWheel(ChartMouseWheelArgs args)
        {
            WheelEvent?.Invoke(this, args);
        }

        /// <summary>
        /// The method is invoke from js while resize.
        /// </summary>
        /// <param name="size">Specifies the format of the offset size of the chart.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartResize(string size)
        {
            Size availabelSize = JsonSerializer.Deserialize<Size>(size);
            ResizeChart(availabelSize);
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void OnChartLongPress(ChartInternalMouseEventArgs args)
        {
            StartMove = true;
            MouseX = args != null ? args.MouseX : 0;
            MouseY = args != null ? args.MouseY : 0;
            TooltipModule?.LongPress();
            crosshairModule?.LongPress();
        }

        private void ResizeChart(Size availabelSize)
        {
            Size previousSize = new Size(AvailableSize.Width, AvailableSize.Height);
            ElementOffset.Width = availabelSize.Width;
            ElementOffset.Height = availabelSize.Height;
            CalculateAvailableSize();
            ResizeEventArgs argsData = new ResizeEventArgs(Constants.RESIZED, false, this, AvailableSize, previousSize);
            if (ChartEvents?.SizeChanged != null)
            {
                ChartEvents.SizeChanged.Invoke(argsData);
            }

            if (!argsData.Cancel)
            {
                AvailableSize = argsData.CurrentSize;
                SetInitialRect();
                SetSvgDimension(Constants.SETSVGDIMENSION);
                ParentRect?.ClearElements();
                OnLayoutChange();
            }
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [JSInvokable]
        public void DisposeDotnetRef()
        {
            ChartDotNetReference?.Dispose();
        }

        private void TriggerPointEvent(string eventName, Action<PointEventArgs> action, ChartInternalMouseEventArgs evt, bool isRightClick = false)
        {
            PointData pointData = new ChartData(this).GetData();
            if (pointData.Series != null && pointData.Point != null)
            {
                PointEventArgs pointEvent = new PointEventArgs(eventName, false, this, evt.ClientX, evt.ClientY, pointData.Point, pointData.Point.Index, pointData.Series, pointData.Series.Renderer.Index, evt.MouseX, evt.MouseY, isRightClick);
                action.Invoke(pointEvent);
            }
        }

        private void TriggerAxisLabelClickEvent(ChartInternalMouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Target.Contains("_AxisLabel_", StringComparison.InvariantCulture))
            {
                string[] index = mouseEventArgs.Target.Split("_AxisLabel_");
                int axisIndex = Convert.ToInt32(index[0].Split(ID)[1], null);
                int labelIndex = Convert.ToInt32(index[1], null);
                ChartAxisRenderer currentAxisRenderer = AxisContainer.Renderers[axisIndex] as ChartAxisRenderer;
                ChartAxis currentAxis = currentAxisRenderer.Axis;
                if (currentAxis.Visible && (axisIndex == 0 || axisIndex == 1))
                {
                    AxisLabelClickEventArgs argsData = new AxisLabelClickEventArgs(Constants.ONAXISLABELCLICK,
                        this, 
                        currentAxis,
                        currentAxisRenderer.VisibleLabels[labelIndex].Text,
                        mouseEventArgs.Target,
                        labelIndex,
                        new ChartInternalLocation(mouseEventArgs.ClientX, mouseEventArgs.ClientY),
                        currentAxisRenderer.VisibleLabels[labelIndex].Value);
                    if (ChartEvents?.OnAxisLabelClick != null)
                    {
                        ChartEvents.OnAxisLabelClick.Invoke(argsData);
                    }
                }
            }
        }

        private void TriggerMultilevelLabelClick(ChartInternalMouseEventArgs mouseEventArgs)
        {
            string multiLevelID = "_Axis_MultiLevelLabel_Level_";
            if (mouseEventArgs.Target.Contains(multiLevelID, StringComparison.InvariantCulture))
            {
                string elementId = mouseEventArgs.Target.Split(multiLevelID)[0];
                MultiLevelLabelClick(mouseEventArgs.Target.Split(multiLevelID)[1], int.Parse(elementId.Substring(elementId.Length - 1), null));
            }
        }

        private void MultiLevelLabelClick(string labelIndex, int axisIndex)
        {
            int textElement = int.Parse(labelIndex.Substring(7), null);
            ChartAxis axis = (AxisContainer.Renderers[axisIndex] as ChartAxisRenderer).Axis;
            List<ChartCategory> categories = axis.MultiLevelLabels[int.Parse(labelIndex.Substring(0, 1), null)].Categories;
            MultiLevelLabelClickEventArgs multilevelclickArgs = new MultiLevelLabelClickEventArgs(Constants.MULTILEVELLABELCLICK, false, categories[textElement].Text, axis, categories[textElement].CustomAttributes, categories[textElement].End, int.Parse(labelIndex.Substring(0, 1), null), categories[textElement].Start);
            ChartEvents.OnMultiLevelLabelClick.Invoke(multilevelclickArgs);
        }

        private void TitleTooltip(ChartInternalMouseEventArgs args)
        {
            string targetId = args.Target, title;
            int index = 0;
            SvgText titleElement = SvgRenderer.TextElementList.Find(element => element.Id == targetId);
            if (targetId.Contains("_AxisTitle", StringComparison.InvariantCulture))
            {
                index = Convert.ToInt32(targetId.Replace(ID, string.Empty, StringComparison.InvariantCulture).Replace("AxisLabel_", string.Empty, StringComparison.InvariantCulture).Split("_")[2], 10);
            }

            if ((targetId == (ID + "_ChartTitle") || targetId == (ID + "_ChartSubTitle") || targetId.Contains("_AxisTitle", StringComparison.InvariantCulture) || targetId.Contains("_legend_title", StringComparison.InvariantCulture)) && (titleElement != null ? titleElement.Text : string.Empty).Contains("...", StringComparison.InvariantCulture))
            {
                title = (AxisContainer.Renderers[index] as ChartAxisRenderer).Axis.Title;
                ChartBorder border = ChartBorderRenderer.ChartBorder;
                TrimTooltip.ShowTooltip((targetId == (ID + "_ChartTitle")) ? Title : targetId.Contains("_AxisTitle", StringComparison.InvariantCulture) ? title : targetId.Contains("_ChartSubTitle", StringComparison.InvariantCulture) ? SubTitle : string.Empty, args.MouseX, args.MouseY, AvailableSize.Width - border.Width, AvailableSize.Height - border.Width, ID + "_EJ2_Title_Tooltip");
            }
            else
            {
                TrimTooltip.ChangeContent(ID + "_EJ2_Title_Tooltip");
            }
        }

        private void AxisTooltip(ChartInternalMouseEventArgs args)
        {
            SvgText axisElement = SvgRenderer.TextElementList.Find(element => element.Id == args.Target);
            if ((args.Target.Contains("AxisLabel", StringComparison.InvariantCulture) || args.Target.Contains("Axis_MultiLevelLabel", StringComparison.InvariantCulture)) && ((axisElement != null) ? axisElement.Text : string.Empty).Contains("...", StringComparison.InvariantCulture))
            {
                ChartBorder border = ChartBorderRenderer.ChartBorder;
                TrimTooltip.ShowTooltip(FindAxisLabel(args.Target), args.MouseX, args.MouseY, AvailableSize.Width - border.Width, AvailableSize.Height - border.Width, ID + "_EJ2_AxisLabel_Tooltip");
            }
            else
            {
                TrimTooltip.ChangeContent(ID + "_EJ2_AxisLabel_Tooltip");
            }
        }

        private string FindAxisLabel(string text)
        {
            string[] texts;
            string label;
            if (text.Contains("AxisLabel", StringComparison.InvariantCulture))
            {
                texts = text.Replace(ID, string.Empty, StringComparison.InvariantCulture).Replace("AxisLabel_", string.Empty, StringComparison.InvariantCulture).Split("_");
                label = (AxisContainer.Renderers[Convert.ToInt32(texts[0], 10)] as ChartAxisRenderer).VisibleLabels[Convert.ToInt32(texts[1], 10)].OriginalText;
                return label;
            }
            else
            {
                texts = text.Replace(ID, string.Empty, StringComparison.InvariantCulture).Replace("Axis_MultiLevelLabel_Level_", string.Empty, StringComparison.InvariantCulture).Replace("Text_", string.Empty, StringComparison.InvariantCulture).Split("_");
                label = (AxisContainer.Renderers[Convert.ToInt32(texts[0], 10)] as ChartAxisRenderer).Axis.MultiLevelLabels[Convert.ToInt32(texts[1], 10)].Categories[Convert.ToInt32(texts[2], 10)].Text;
                return label;
            }
        }
    }
}