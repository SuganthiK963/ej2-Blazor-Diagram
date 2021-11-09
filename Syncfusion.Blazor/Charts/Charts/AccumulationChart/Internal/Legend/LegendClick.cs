using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal partial class AccumulationChartLegend : BaseLegend
    {
        internal void Click(ChartInternalMouseEventArgs args)
        {
            if (accChart.AccumulationLegendModule != null && accChart.LegendSettings.Visible)
            {
                LegendItemsClick(args.Target, new List<string>() { LegendID + "_text_", LegendID + "_shape_", LegendID + "_shape_marker_" }, args);
            }

            if (args.Target.Contains("chart_legend_pageup", StringComparison.Ordinal))
            {
                ChangePage(true);
            }
            else if (args.Target.Contains("_chart_legend_pagedown", StringComparison.Ordinal))
            {
                ChangePage(false);
            }
        }

        private void ChangePage(bool pageUp)
        {
            SvgText pageNumberElement = accChart.Rendering.TextElementList.Find(element => element.Id == PageNumberID);
            int page = int.Parse(pageNumberElement.Text.Split('/')[0], null);
            if (pageUp && page > 1)
            {
                TranslatePage(page - 2, page - 1, pageNumberElement);
            }
            else if (!pageUp && page < TotalPageCount)
            {
                TranslatePage(page, page + 1, pageNumberElement);
            }
        }

        private async void TranslatePage(double page, double pageNumber, SvgText pageNumberEle = null)
        {
            double size = ClipPathHeight * page, pageX;
            Transform = "translate(0,-" + size + ")";
            CurrentPageNumber = pageNumber;
            if (IsVertical)
            {
                pageX = PageXCollections[Convert.ToInt32(page * MaxColumns, accChart.NumberFormatter)];
                size = (!IsInverse) ? pageX - LegendBounds.X : (LegendBounds.X + MaxWidth) - pageX;
                size = size < 0 ? 0 : size;
                Transform = ((!IsInverse) ? "translate(-" : "translate(") + size + ",0)";
            }
#pragma warning disable CA2007
            await accChart.InvokeMethod(AccumulationChartConstants.SETATTRIBUTE, new object[] { LegendTranslateID, "transform", Transform });
#pragma warning restore CA2007
            pageNumberEle.ChangeText(CurrentPageNumber + "/" + TotalPageCount);
        }

        private void LegendItemsClick(string targetID, List<string> legendItems, ChartInternalMouseEventArgs args)
        {
            foreach (string item in legendItems)
            {
                if (targetID.Contains(item, StringComparison.Ordinal))
                {
                    accChart.animateSeries = false;
                    double pointIndex = Convert.ToInt32(targetID.Split(item).Last(), accChart.NumberFormatter);
                    if (accChart.LegendSettings.ToggleVisibility && !double.IsNaN(pointIndex))
                    {
                        accChart.LegendClickRedraw = accChart.EnableAnimation;
                        AccumulationChartSeries currentSeries = accChart.Series.First();
                        AccumulationPoints point = currentSeries.Points.Find(item => item.Index == pointIndex);
                        LegendOption legendOption = LegendCollection.Find(item => item.PointIndex == pointIndex);
                        point.Visible = !point.Visible;
                        legendOption.Visible = !legendOption.Visible;
                        currentSeries.SumOfPoints += point.Visible ? (double)point.Y : -(double)point.Y;
                        currentSeries.RefreshPoints(currentSeries.Points);
                        accChart.ClearSvgElements();
                        accChart.InitSeries();
                        accChart.CreateChart();
                    }
                    else if (accChart.AccumulationSelectionModule != null && !double.IsNaN(pointIndex))
                    {
                        accChart.AccumulationSelectionModule.LegendSelection((int)pointIndex, args);
                    }
                }
            }
        }

        internal void LegendMouseMove(ChartInternalMouseEventArgs args, bool isTouch)
        {
            SvgText element = accChart.Rendering.TextElementList.Find(item => item.Id == args.Target);
            if (element != null && element.Text.Contains("...", StringComparison.InvariantCulture))
            {
                string[] targetIds = args.Target.Split(LegendID + "_text_");
                if (targetIds.Length == 2)
                {
                    int legendIndex = int.Parse(targetIds[1], accChart.NumberFormatter);
                    if (!double.IsNaN(legendIndex))
                    {
                        if (isTouch)
                        {
                            accChart.TooltipBase.FadeOutTooltip(0);
                        }

                        accChart.TooltipBase?.ShowTooltip(
                            accChart.VisibleSeries[0].Points[legendIndex].X.ToString(), accChart.MouseX + 10, accChart.MouseY + 10, accChart.AvailableSize.Width, accChart.AvailableSize.Height, accChart.ID + "_Blazor_Legend_Tooltip");
                    }
                }
            }
            else
            {
                accChart.TooltipBase?.ChangeContent(string.Empty, true);
            }

            if (isTouch)
            {
                accChart.TooltipBase?.FadeOutTooltip(1000);
            }

            if (accChart.LegendSettings.Visible && !accChart.LegendSettings.ToggleVisibility && !isTouch && accChart.AccumulationSelectionModule != null && accChart.HighlightMode != AccumulationSelectionMode.None)
            {
                Click(args);
            }
        }
    }
}