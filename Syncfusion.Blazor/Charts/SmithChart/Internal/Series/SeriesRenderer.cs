using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the series rendering of smith chart.
    /// </summary>
    public class SeriesRenderer
    {
        private const string SPACE = " ";

        private SfSmithChart smithchart;
        private List<double> xvalues = new List<double>();
        private List<double> yvalues = new List<double>();
        private List<LineSegment> lineSegments = new List<LineSegment>();
        private List<List<Point>> location = new List<List<Point>>();
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal SeriesRenderer(SfSmithChart chart)
        {
            smithchart = chart;
        }

        internal List<List<PointRegion>> PointsRegion { get; set; } = new List<List<PointRegion>>();

        internal void RenderSeries(RenderTreeBuilder treeBuilder)
        {
            double resistantCx, reactanceCy, chartAreaRadius = smithchart.AxisModule.AreaRadius,
            chartAreaCx = smithchart.AxisModule.CircleCenterX, chartAreaCy = smithchart.AxisModule.CircleCenterY, epsilon,
            diameter = chartAreaRadius * 2, circleLeftX = smithchart.AxisModule.CircleLeftX;
            Point reactanceStartPoint = new Point(chartAreaCx + ((smithchart.RenderType == RenderType.Impedance) ? chartAreaRadius : -chartAreaRadius), chartAreaCy);
            double resistantCy = chartAreaCy, reactanceCx = reactanceStartPoint.X, resistance, resistantR, reactance, reactanceR;
            PointsRegion.Clear();
            foreach (SmithChartSeries series in smithchart.VisibleSeries)
            {
                int index = series.Index;
                PointsRegion.Add(new List<PointRegion>());
                location.Add(new List<Point>());
                if (series.Visible)
                {
                    List<SmithChartPoint> points = series.ActualPoints;
                    for (int j = 0; j < points.Count; j++)
                    {
                        xvalues.Add(default(double));
                        yvalues.Add(default(double));
                        xvalues[j] = points[j].Resistance;
                        yvalues[j] = points[j].Reactance;
                    }

                    for (int k = 0; k < points.Count; k++)
                    {
                        resistance = xvalues[k];
                        resistantR = (diameter * (1 / (resistance + 1))) / 2;
                        reactance = yvalues[k];
                        reactanceR = Math.Abs(((1 / reactance) * diameter) / 2);
                        if (smithchart.RenderType == RenderType.Impedance)
                        {
                            reactanceCy = reactance > 0 ? chartAreaCy - reactanceR : chartAreaCy + reactanceR;
                            resistantCx = circleLeftX + diameter - resistantR;
                        }
                        else
                        {
                            reactanceCy = reactance < 0 ? chartAreaCy - reactanceR : chartAreaCy + reactanceR;
                            resistantCx = circleLeftX + resistantR;
                        }

                        Point interSectPoint = AxisRenderer.IntersectingCirclePoints(
                            reactanceCx, reactanceCy, reactanceR, resistantCx, resistantCy, resistantR, smithchart.RenderType);
                        epsilon = SmithChartHelper.GetEpsilonValue();
                        if (Math.Abs(reactance) < epsilon)
                        {
                            interSectPoint.X = (smithchart.RenderType == RenderType.Impedance) ? resistantCx - resistantR : resistantCx + resistantR;
                            interSectPoint.Y = chartAreaCy;
                        }

                        PointsRegion[index].Add(new PointRegion());
                        PointsRegion[index][k] = new PointRegion { Point = interSectPoint, X = resistance, Y = reactance };
                        location[index].Add(new Point(0, 0));
                        location[index][k] = new Point(interSectPoint.X, interSectPoint.Y);
                    }

                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        lineSegments.Add(new LineSegment());
                        lineSegments[i] = new LineSegment { X1 = xvalues[i], Y1 = yvalues[i], X2 = xvalues[i + 1], Y2 = yvalues[i + 1] };
                    }

                    DrawSeries(treeBuilder, series, index);
                }
            }
        }

        private void DrawSeries(RenderTreeBuilder treeBuilder, SmithChartSeries series, int seriesIndex)
        {
            string clipPathId = smithchart.ID + SmithChartConstants.SERIESCLIPRECTID + series.Index,
            seriesClipPath = "url(#" + clipPathId + ")";
            treeBuilder.OpenElement(SvgRendering.Seq++, "g");
            treeBuilder.AddAttribute(SvgRendering.Seq++, "id", smithchart.ID + "_SeriesGroup_" + seriesIndex);
            treeBuilder.AddAttribute(SvgRendering.Seq++, "style", "clip-path:" + seriesClipPath + "; -webkit-clip-path:" + seriesClipPath + "; ");
            string sb = string.Empty, path;
            SmithChartSeriesMarker marker = series.Marker;
            smithchart.Rendering.RenderClipPath(treeBuilder, clipPathId, new Rect { X = smithchart.Bounds.X, Y = smithchart.Bounds.Y, Width = smithchart.AvailableSize.Width, Height = smithchart.AvailableSize.Height }, smithchart.AnimateSeries && series.EnableAnimation ? "hidden" : "visible");
            for (int i = 0; i < series.ActualPoints.Count - 1; i++)
            {
                Point point1 = PointsRegion[seriesIndex][i].Point;
                Point point2 = PointsRegion[seriesIndex][i + 1].Point;
                sb = string.Concat(sb, "M " + SPACE + Convert.ToString(point1.X, culture) + SPACE + Convert.ToString(point1.Y, culture) + SPACE + "L" + SPACE + Convert.ToString(point2.X, culture) + SPACE + Convert.ToString(point2.Y, culture) + SPACE);
            }

            path = sb.ToString();
            SmithChartSeriesRenderEventArgs seriesEventArgs = new SmithChartSeriesRenderEventArgs()
            {
                EventName = "SeriesRendering",
                Fill = series.Interior,
                Text = series.Name,
                Index = seriesIndex
            };
            SfSmithChart.InvokeEvent<SmithChartSeriesRenderEventArgs>(smithchart.SmithChartEvents?.SeriesRender, seriesEventArgs);
            if (!seriesEventArgs.Cancel)
            {
                smithchart.Rendering.RenderPath(treeBuilder, smithchart.ID + "_Series_" + seriesIndex, path, string.Empty, series.Width, seriesEventArgs.Fill);
            }

            if (marker.Visible)
            {
                DrawMarker(marker, series.ActualPoints.Count - 1, seriesIndex, PointsRegion[seriesIndex], treeBuilder);
            }

            smithchart.DataLabelModule?.GetDataLabelCollection(seriesIndex, PointsRegion[seriesIndex]);
            if (marker.DataLabel.Visible)
            {
                smithchart.DataLabelModule?.Render(treeBuilder, series, series.Marker.DataLabel);
            }

            treeBuilder.CloseElement();
        }

        private void DrawMarker(SmithChartSeriesMarker marker, double pointsCount, int seriesindex, List<PointRegion> pointsRegion, RenderTreeBuilder treeBuilder)
        {
            treeBuilder.OpenElement(SvgRendering.Seq++, "g");
            treeBuilder.AddAttribute(SvgRendering.Seq++, "id", smithchart.ID + "_svg" + "_Series_" + seriesindex + "_Marker");
            for (int i = 0; i < pointsCount + 1; i++)
            {
                smithchart.Helper.DrawSymbol(treeBuilder, smithchart.Rendering, new Point(pointsRegion[i].Point.X, pointsRegion[i].Point.Y), marker.Shape.ToString(), new Size(marker.Width, marker.Height), new PathOptions(smithchart.ID + "_Series_" + seriesindex + "_Points" + i + "_Marker" + i, string.Empty, string.Empty, marker.Border.Width, marker.Border.Color, marker.Opacity, !string.IsNullOrEmpty(marker.Fill) ? marker.Fill : smithchart.Series[seriesindex].Interior));
            }

            treeBuilder.CloseElement();
        }

        internal Point GetLocation(int seriesindex, int pointIndex)
        {
            return new Point(location[seriesindex][pointIndex].X, location[seriesindex][pointIndex].Y);
        }

        internal void Dispose()
        {
            smithchart = null;
            lineSegments = null;
            PointsRegion = null;
            location = null;
            xvalues = null;
            yvalues = null;
        }
    }
}