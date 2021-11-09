using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class PyramidSeries : TriangularBase
    {
        internal PyramidSeries(SfAccumulationChart Chart)
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
            Size area = series.TriangleSize;
            double seriesTop = chart.InitialClipRect.Y + ((chart.InitialClipRect.Height - area.Height) / 2);
            double emptySpaceAtLeft = ((chart.InitialClipRect.Width - series.TriangleSize.Width) / 2) + chart.InitialClipRect.X;
            double top = point.YRatio;
            double bottom = point.YRatio + point.HeightRatio;
            double bottomRadius = 0.5 * (1 - bottom);
            double topRadius = 0.5 * (1 - point.YRatio);
            top += seriesTop / area.Height;
            bottom += seriesTop / area.Height;
            ChartInternalLocation line1 = new ChartInternalLocation(emptySpaceAtLeft + (topRadius * area.Width), top * area.Height);
            ChartInternalLocation line2 = new ChartInternalLocation(emptySpaceAtLeft + ((1 - topRadius) * area.Width), top * area.Height);
            ChartInternalLocation line3 = new ChartInternalLocation(emptySpaceAtLeft + ((1 - bottomRadius) * area.Width), bottom * area.Height);
            ChartInternalLocation line4 = new ChartInternalLocation(emptySpaceAtLeft + (bottomRadius * area.Width), bottom * area.Height);
            ChartInternalLocation[] polygon = new ChartInternalLocation[] { line1, line2, line3, line4 };
            SetLabelLocation(series, point, polygon);
            return FindPath(polygon);
        }

        protected override void InitializeSizeRatio(List<AccumulationPoints> points, AccumulationChartSeries series, bool reverse = false)
        {
            if (series.PyramidMode == PyramidMode.Linear)
            {
                base.InitializeSizeRatio(points, series, true);
            }
            else
            {
                CalculateSurfaceSegments(series);
            }
        }

        private static void CalculateSurfaceSegments(AccumulationChartSeries series)
        {
            int count = series.Points.Count;
            double[] y = new double[count];
            double[] height = new double[count];
            double gapHeight = Math.Min(0, Math.Max(series.GapRatio, 1)) / (count - 1);
            double preSum = GetSurfaceHeight(0, series.SumOfPoints);
            double currY = 0;
            for (var i = 0; i < count; i++)
            {
                if (series.Points[i].Visible)
                {
                    y[i] = currY;
                    height[i] = GetSurfaceHeight(currY, Math.Abs((double)series.Points[i].Y));
                    currY += height[i] + (gapHeight * preSum);
                }
            }

            double coef = 1 / (currY - (gapHeight * preSum));
            for (var i = 0; i < count; i++)
            {
                if (series.Points[i].Visible)
                {
                    series.Points[i].YRatio = coef * y[i];
                    series.Points[i].HeightRatio = coef * height[i];
                }
            }
        }

        private static double GetSurfaceHeight(double y, double surface)
        {
            return SolveQuadraticEquation(1, 2 * y, -surface);
        }

        private static double SolveQuadraticEquation(double a, double b, double c)
        {
            double d = (b * b) - (4 * a * c);
            if (d >= 0)
            {
                double sd = Math.Sqrt(d);
                double root1 = (-b - sd) / (2 * a);
                double root2 = (-b + sd) / (2 * a);
                return Math.Max(root1, root2);
            }

            return 0;
        }
    }
}