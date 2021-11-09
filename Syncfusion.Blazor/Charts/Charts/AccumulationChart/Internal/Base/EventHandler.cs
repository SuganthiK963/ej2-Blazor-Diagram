using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts.AccumulationChart.Internal;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfAccumulationChart : SfDataBoundComponent, IAccumulationChart
    {
        private async void ResizeChart()
        {
            LegendClickRedraw = false;
#pragma warning disable CA2007
            Size previousSize = new Size(AvailableSize.Width, AvailableSize.Height);
            await GetContainerSize();
            SetContainerSize();
#pragma warning restore CA2007
            Size currentSize = AvailableSize;
            AccumulationResizeEventArgs argsData = new AccumulationResizeEventArgs(Constants.RESIZED, false, this, currentSize, previousSize);
            if (AccumulationChartEvents?.SizeChanged != null)
            {
                AccumulationChartEvents.SizeChanged.Invoke(argsData);
            }

            animateSeries = false;
            AvailableSize = argsData.Cancel ? argsData.PreviousSize : argsData.CurrentSize;
            RefreshChart();
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveElements()
        {
            animateSeries = false;
            LegendClickRedraw = false;
            ChartContent = null;
            InvokeAsync(StateHasChanged);
            Rendering.RefreshElementList();
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartMouseEnd(ChartInternalMouseEventArgs args)
        {
            AccumulationTooltipModule?.MouseUpHandler(args?.Target);
#pragma warning disable CA1062
            TitleTooltip(args, args.MouseX, args.MouseY);
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartMouseClick(ChartInternalMouseEventArgs args)
        {
            MouseX = args.ClientX;
            MouseY = args.ClientY;
            AccumulationLegendModule?.Click(args);
            AccumulationSelectionModule?.CalculateSelectedElements(args);
            if (EnableBorderOnMouseMove)
            {
                PieSeriesModule?.DrawHoverBorder(args);
            }

            if (VisibleSeries[0].Explode)
            {
                AccBaseModule.ProcessExplode(args);
            }

            if (AccumulationChartEvents?.OnPointClick != null)
            {
                TriggerPointEvent(args);
            }
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartMouseMove(ChartInternalMouseEventArgs args)
        {
            MouseX = args.MouseX;
#pragma warning restore CA1062
            MouseY = args.MouseY;
            AccumulationLegendModule?.LegendMouseMove(args, false);
            DataLabelModule?.DataLabelMouseMove(args, false);
            AccumulationTooltipModule?.MouseMoveHandler(args.Target);
            if (EnableBorderOnMouseMove && AccType == AccumulationType.Pie && ChartHelper.WithInBounds(args.MouseX, args.MouseY, InitialClipRect))
            {
                PieSeriesModule.DrawHoverBorder(args);
            }

            TitleTooltip(args, MouseX, MouseY);
            if (HighlightMode != AccumulationSelectionMode.None && !string.IsNullOrEmpty(args.Target))
            {
                if (args.Target.Contains("text", StringComparison.Ordinal))
                {
                    args.Target = args.Target.Replace("text", "shape", StringComparison.InvariantCulture);
                    if (args.Target.Contains("datalabel", StringComparison.InvariantCulture))
                    {
                        args.Target = args.Target.Replace("datalabel_", string.Empty, StringComparison.InvariantCulture).Replace("shape", "Point", StringComparison.InvariantCulture);
                    }
                }

                SvgPath selectedElement = Rendering.PathElementList.Find(item => item.Id == args.Target);
                if (selectedElement != null && !string.IsNullOrEmpty(selectedElement.Class) &&
                    (selectedElement.Class.Contains("highlight", StringComparison.Ordinal) || selectedElement.Class.Contains("selection", StringComparison.Ordinal)))
                {
                    return;
                }

                AccumulationSelectionModule.IsHighlight = true;
                AccumulationSelectionModule.CalculateSelectedElements(args);
                return;
            }
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartMouseLeave()
        {
            AccumulationTooltipModule?.MouseLeaveHandler();
            PieSeriesModule?.RemoveHoverBorder(1000);
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartResize()
        {
            ResizeChart();
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable CA1822
        public void OnChartLongPress()
        {
            // TODO:
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartRightClick()
        {
            // TODO:
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnGetCharSize()
        {
            // TODO:
        }

        /// <exclude />
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnChartMouseDown()
#pragma warning restore CA1822
        {
            // TODO:
        }

        private void TriggerPointEvent(ChartInternalMouseEventArgs args)
        {
            int[] data = ChartHelper.IndexFinder(args.Target, true);
            if (data[0] >= 0 && data[1] >= 0)
            {
                AccumulationPoints seriesPoints = Series[data[0]].Points[data[1]];
                AccumulationPointEventArgs eventArgs = new AccumulationPointEventArgs(Constants.ONPOINTCLICK, args.ClientX, args.ClientY, args.MouseX, args.MouseY, data[0], data[1], seriesPoints);
                if (AccumulationChartEvents?.OnPointClick != null)
                {
                    AccumulationChartEvents.OnPointClick.Invoke(eventArgs);
                }
            }
        }

        private void TitleTooltip(ChartInternalMouseEventArgs args, double x, double y)
        {
            bool id = args.Target == ID + "_title" || args.Target == ID + "_subTitle";
            if (id && Rendering.TextElementList.Find(item => item.Id == args.Target).Text.Contains("...", StringComparison.Ordinal))
            {
                string title = args.Target == (ID + "_title") ? Title : SubTitle;
                TooltipBase?.ShowTooltip(title, x, y, 0, 0, ID + "_Chart_Title_Tooltip");
            }
            else
            {
                TooltipBase?.ChangeContent(ID + "_Chart_Title_Tooltip");
            }
        }
    }
}