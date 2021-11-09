using System;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class TriangularBase : AccumulationBase
    {
        internal SfAccumulationChart AccChartInstance { get; set; }

        internal TriangularBase(SfAccumulationChart chart)
            : base(chart)
        {
            AccChartInstance = chart;
        }

        internal void InitProperties(SfAccumulationChart chart, AccumulationChartSeries series)
        {
            series.TriangleSize = new Size(
                DataVizCommonHelper.StringToNumber(series.Width, chart.InitialClipRect.Width),
                DataVizCommonHelper.StringToNumber(series.Height, chart.InitialClipRect.Height));
            series.NeckSize = new Size(
                DataVizCommonHelper.StringToNumber(series.NeckWidth, chart.InitialClipRect.Width),
                DataVizCommonHelper.StringToNumber(series.NeckHeight, chart.InitialClipRect.Height));
            DefaultLabelBound(series, series.DataLabel.Visible, series.DataLabel.Position, chart);
            chart.ExplodeDistance = DataVizCommonHelper.StringToNumber(series.ExplodeOffset == "30%" ? "25px" : series.ExplodeOffset, chart.InitialClipRect.Width);
            InitializeSizeRatio(series.Points, series);
        }

        protected virtual void InitializeSizeRatio(List<AccumulationPoints> points, AccumulationChartSeries series, bool reverse = false)
        {
            double gapRatio = Math.Min(Math.Max(series.GapRatio, 0), 1);
            double coeff = 1 / (series.SumOfPoints * (1 + (gapRatio / (1 - gapRatio))));
            double spacing = gapRatio / (points.Count - 1);
            double y = 0;
            for (var j = points.Count - 1; j >= 0; j--)
            {
                int index = reverse ? points.Count - 1 - j : j;
                if (points[index].Visible)
                {
                    double height = coeff * (double)points[index].Y;
                    points[index].YRatio = y;
                    points[index].HeightRatio = height;
                    y += height + spacing;
                }
            }
        }

        protected static void SetLabelLocation(AccumulationChartSeries series, AccumulationPoints point, ChartInternalLocation[] locations)
        {
            int bottom = series.Type == AccumulationType.Funnel ? locations.Length - 2 : locations.Length - 1;
            double x = (locations[0].X + locations[bottom].X) * 0.5;
            point.Region = new Rect(x, locations[0].Y, ((locations[1].X + locations[bottom - 1].X) * 0.5) - x, locations[bottom].Y - locations[0].Y);
            point.SymbolLocation = new ChartInternalLocation(
                point.Region.X + (point.Region.Width * 0.5),
                point.Region.Y + (point.Region.Height * 0.5));
            point.LabelOffset = new ChartInternalLocation(
                point.SymbolLocation.X - ((locations[0].X + locations[locations.Length - 1].X) * 0.5),
                point.SymbolLocation.Y - ((locations[0].Y + locations[locations.Length - 1].Y) * 0.5));
        }

        private static void DefaultLabelBound(AccumulationChartSeries series, bool labelVisible, AccumulationLabelPosition position, SfAccumulationChart chart)
        {
            series.AccumulationBound = new Rect((chart.InitialClipRect.Width - series.TriangleSize.Width) * 0.5, (chart.InitialClipRect.Height - series.TriangleSize.Height) * 0.5, series.TriangleSize.Width, series.TriangleSize.Height);
            if (labelVisible && position == AccumulationLabelPosition.Outside)
            {
                series.LabelBound = new Rect(double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            }
            else
            {
                series.LabelBound = new Rect(
                    series.AccumulationBound.X,
                    series.AccumulationBound.Y,
                    series.AccumulationBound.X + series.AccumulationBound.Width,
                    series.AccumulationBound.Y + series.AccumulationBound.Height);
            }
        }

        protected static string FindPath(ChartInternalLocation[] locations)
        {
            string path = "M";
            for (var k = 0; k < locations.Length; k++)
            {
                path += locations[k].X.ToString(culture) + " " + locations[k].Y.ToString(culture);
                if (k != locations.Length - 1)
                {
                    path += " L";
                }
            }

            return path + " Z";
        }

        internal override void Dispose()
        {
            base.Dispose();
            AccChartInstance = null;
        }
    }
}