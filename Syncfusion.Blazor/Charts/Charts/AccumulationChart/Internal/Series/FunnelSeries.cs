using System;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class FunnelSeries : TriangularBase
    {
        internal FunnelSeries(SfAccumulationChart Chart)
            : base(Chart)
        {
        }

        internal void RenderPoint(AccumulationPoints point, AccumulationChartSeries series, PathOptions options, RenderTreeBuilder treeBuilder)
        {
            if (!point.Visible)
            {
                return;
            }

            point.MidAngle = 0;
            options.Direction = GetSegmentData(point, series, AccChartInstance);
            AccChartInstance.Rendering.RenderPath(treeBuilder, options);
        }

        private static string GetSegmentData(AccumulationPoints point, AccumulationChartSeries series, SfAccumulationChart chart)
        {
            double lineWidth = 0, topRadius = 0, bottomRadius = 0, endTop = 0, endBottom = 0, minRadius = 0, endMin = 0, bottomY = double.NaN;
            Size area = series.TriangleSize;
            double emptySpaceAtLeft = ((chart.InitialClipRect.Width - series.TriangleSize.Width) * 0.5) + chart.InitialClipRect.X;
            double seriesTop = chart.InitialClipRect.Y + ((chart.InitialClipRect.Height - area.Height) / 2);
            double top = point.YRatio * area.Height;
            double bottom = top + (point.HeightRatio * area.Height);
            Size neckSize = series.NeckSize;
            lineWidth = neckSize.Width + ((area.Width - neckSize.Width) * ((area.Height - neckSize.Height - top) / (area.Height - neckSize.Height)));
            topRadius = (area.Width / 2) - (lineWidth / 2);
            endTop = topRadius + lineWidth;
            if (bottom > area.Height - neckSize.Height || area.Height == neckSize.Height)
            {
                lineWidth = neckSize.Width;
            }
            else
            {
                lineWidth = neckSize.Width + ((area.Width - neckSize.Width) * ((area.Height - neckSize.Height - bottom) / (area.Height - neckSize.Height)));
            }

            bottomRadius = (area.Width / 2) - (lineWidth / 2);
            endBottom = bottomRadius + lineWidth;
            if (top >= area.Height - neckSize.Height)
            {
                topRadius = bottomRadius = minRadius = (area.Width / 2) - (neckSize.Width / 2);
                endTop = endBottom = endMin = (area.Width / 2) + (neckSize.Width / 2);
            }
            else if (bottom > (area.Height - neckSize.Height))
            {
                minRadius = bottomRadius = (area.Width / 2) - (lineWidth / 2);
                endMin = endBottom = minRadius + lineWidth;
                bottomY = area.Height - neckSize.Height;
            }

            top += seriesTop;
            bottom += seriesTop;
            bottomY += seriesTop;
            ChartInternalLocation line1 = new ChartInternalLocation(emptySpaceAtLeft + topRadius, top);
            ChartInternalLocation line2 = new ChartInternalLocation(emptySpaceAtLeft + endTop, top);
            ChartInternalLocation line4 = new ChartInternalLocation(emptySpaceAtLeft + endBottom, bottom);
            ChartInternalLocation line5 = new ChartInternalLocation(emptySpaceAtLeft + bottomRadius, bottom);
            ChartInternalLocation line3 = new ChartInternalLocation(emptySpaceAtLeft + endBottom, bottom);
            ChartInternalLocation line6 = new ChartInternalLocation(emptySpaceAtLeft + bottomRadius, bottom);
            if (!double.IsNaN(bottomY))
            {
                line3 = new ChartInternalLocation(emptySpaceAtLeft + endMin, bottomY);
                line6 = new ChartInternalLocation(emptySpaceAtLeft + minRadius, bottomY);
            }

            ChartInternalLocation[] polygon = new ChartInternalLocation[] { line1, line2, line3, line4, line5, line6 };
            SetLabelLocation(series, point, polygon);
            return FindPath(polygon);
        }
    }
}