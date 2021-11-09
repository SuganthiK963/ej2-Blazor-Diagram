using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the axis rendering of smith chart.
    /// </summary>
    public class AxisRenderer
    {
        private const string SPACE = " ";
        private SfSmithChart smithchart;
        private double[] radialLabels = new double[] { -50, -20, -10, -5, -4, -3, -2, -1.5, -1, -0.8, -0.6, -0.4, -0.2, 0, 0.2, 0.4, 0.6, 0.8, 1, 1.5, 2, 3, 4, 5, 10, 20, 50 };
        private List<LabelCollection> radialLabelCollection = new List<LabelCollection>();
        private List<HorizontalLabelCollection> horizontalLabelCollection = new List<HorizontalLabelCollection>();
        private List<GridArcPoints> majorHGridArcPoints = new List<GridArcPoints>();
        private List<GridArcPoints> minorHGridArcPoints = new List<GridArcPoints>();
        private List<GridArcPoints> majorRGridArcPoints = new List<GridArcPoints>();
        private List<GridArcPoints> minorGridArcPoints = new List<GridArcPoints>();
        private List<RadialLabelCollection> labelCollection = new List<RadialLabelCollection>();
        private Direction direction = new Direction();
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisRenderer"/> class.
        /// </summary>
        /// <param name="chart">Represents the <see cref="SfSmithChart"/> instance.</param>
        public AxisRenderer(SfSmithChart chart)
        {
            smithchart = chart;
        }

        internal double AreaRadius { get; set; }

        internal double CircleLeftX { get; set; }

        internal double CircleTopY { get; set; }

        internal double CircleCenterX { get; set; }

        internal double CircleCenterY { get; set; }

        private static LabelRegion CalculateRegion(string label, Size textSize, double textPositionX, double textPositionY)
        {
            return new LabelRegion { Bounds = new Rect(textPositionX, textPositionY, textSize.Width, textSize.Height), LabelText = label };
        }

        private static Point SetLabelsInsidePosition(double angle, double px, double py, Size textSize)
        {
            double x = px, y = py;
            if (angle == 0 || angle == 360)
            {
                x -= textSize.Width;
                y -= textSize.Height / 2;
            }
            else if (angle == 90)
            {
                x -= textSize.Width;
                y += textSize.Height / 8;
            }
            else if (angle == 180)
            {
                y += textSize.Height;
            }
            else if (angle == 270)
            {
                y += textSize.Height / 2;
            }
            else if (angle > 0 && angle <= 20)
            {
                x -= textSize.Width;
            }
            else if (angle > 20 && angle <= 60)
            {
                x -= textSize.Width + (textSize.Width / 2);
                y += textSize.Height / 2;
            }
            else if (angle > 60 && angle < 90)
            {
                x -= textSize.Width + (textSize.Width / 4);
                y += textSize.Height / 4;
            }
            else if (angle > 90 && angle <= 135)
            {
                x -= textSize.Width / 2;
                y += textSize.Height / 16;
            }
            else if (angle > 135 && angle <= 150)
            {
                x += textSize.Width / 2;
                y += textSize.Height / 2;
            }
            else if (angle > 150 && angle < 180)
            {
                x += textSize.Width / 2;
                y += textSize.Height;
            }
            else if (angle > 180 && angle <= 210)
            {
                x += textSize.Width / 6;
                y += textSize.Height / 6;
            }
            else if (angle > 210 && angle < 240)
            {
                y += textSize.Height / 4;
            }
            else if (angle > 225 && angle < 270)
            {
                y += textSize.Height / 3;
            }
            else if (angle > 270 && angle <= 300)
            {
                x -= textSize.Width + (textSize.Width / 4);
                y += textSize.Height / 4;
            }
            else if (angle > 300 && angle <= 330)
            {
                x -= textSize.Width + (textSize.Width / 3);
                y += textSize.Height / 4;
            }
            else if (angle > 330 && angle <= 340)
            {
                x -= textSize.Width + (textSize.Width / 2);
                y += textSize.Height / 4;
            }
            else if (angle > 340)
            {
                x -= textSize.Width;
                y += textSize.Height / 8;
            }

            return new Point(x, y);
        }

        private static Point SetLabelsOutsidePosition(double angle, double px, double py, Size textSize)
        {
            double x = px, y = py;
            if (angle == 90)
            {
                x -= textSize.Width / 2;
                y += textSize.Height;
            }
            else if (angle == 180)
            {
                x -= textSize.Width + 5;
                y -= textSize.Height / 4;
            }
            else if (angle == 270)
            {
                x -= textSize.Width / 2;
                y -= textSize.Height / 4;
            }
            else if (angle == 360)
            {
                x += 5;
                y -= textSize.Height / 2;
            }
            else if (angle > 0 && angle <= 30)
            {
                x += textSize.Width / 4;
                y += textSize.Height / 8;
            }
            else if (angle > 30 && angle <= 60)
            {
                x += textSize.Width / 2;
                y += textSize.Height / 4;
            }
            else if (angle > 60 && angle <= 90)
            {
                x -= textSize.Width / 2;
                y += textSize.Height;
            }
            else if (angle > 90 && angle <= 135)
            {
                x -= textSize.Width;
                y += textSize.Height;
            }
            else if (angle > 135 && angle <= 180)
            {
                x -= textSize.Width + (textSize.Width / 4);
                y += textSize.Height / 4;
            }
            else if (angle > 180 && angle <= 210)
            {
                x -= textSize.Width + (textSize.Width / 4);
                y -= textSize.Height / 4;
            }
            else if (angle > 210 && angle <= 270)
            {
                x -= textSize.Width;
                y -= textSize.Height / 4;
            }
            else if (angle > 270 && angle <= 340)
            {
                y -= textSize.Height / 4;
            }
            else if (angle > 340)
            {
                y += textSize.Height / 4;
                x += textSize.Width / 6;
            }

            return new Point(x, y);
        }

        private static double ArcRadius(Point startPoint, Point endPoint, double angle)
        {
            double radian = angle > 180 ? (90 * Math.PI / 180) : (270 * Math.PI / 180),
            mx = (endPoint.X - startPoint.X) / 2,
            my = (endPoint.Y - startPoint.Y) / 2,
            cy = startPoint.Y + (Math.Sin(radian) * (my - (mx * ((Math.Cos(radian) * my) - (Math.Sin(radian) * mx)) / ((Math.Cos(radian) * mx) + (Math.Sin(radian) * my)))) / Math.Sin(radian));
            return Math.Abs(startPoint.Y - cy);
        }

        private static Point CirclePointPosition(double cx, double cy, double angle, double r)
        {
            double radian = angle * (Math.PI / 180);
            return new Point(cx + (r * Math.Cos(radian)), cy + (r * Math.Sin(radian)));
        }

        private static double CircleXYRadianValue(double centerX, double centerY, double outterX, double outterY)
        {
            double radian = Math.Atan2(outterY - centerY, outterX - centerX);
            return radian < 0 ? (radian + (360 * Math.PI / 180)) : radian;
        }

        internal static Point IntersectingCirclePoints(double x1, double y1, double r1, double x2, double y2, double r2, RenderType renderType)
        {
            Point point = new Point(0, 0);
            double cx = x1 - x2,
            cy = y1 - y2,
            midRadius = Math.Sqrt((cx * cx) + (cy * cy)),
            radiusSquare = midRadius * midRadius,
            a = ((r1 * r1) - (r2 * r2)) / (2 * radiusSquare),
            radiusSquare2 = (r1 * r1) - (r2 * r2),
            c = Math.Sqrt((2 * ((r1 * r1) + (r2 * r2)) / radiusSquare) - (radiusSquare2 * radiusSquare2 / (radiusSquare * radiusSquare)) - 1),
            fx = ((x1 + x2) / 2) + (a * (x2 - x1)),
            gx = c * (y2 - y1) / 2,
            ix1 = fx + gx,
            ix2 = fx - gx,
            fy = ((y1 + y2) / 2) + (a * (y2 - y1)),
            gy = c * (x1 - x2) / 2,
            iy1 = fy + gy,
            iy2 = fy - gy;
            if (renderType == RenderType.Impedance)
            {
                if (ix2 < ix1)
                {
                    point.X = ix2;
                    point.Y = iy2;
                }
                else
                {
                    point.X = ix1;
                    point.Y = iy1;
                }
            }
            else
            {
                if (ix1 > ix2)
                {
                    point.X = ix1;
                    point.Y = iy1;
                }
                else
                {
                    point.X = ix2;
                    point.Y = iy2;
                }
            }

            return point;
        }

        private static bool IsOverlap(double x, double d, double previousR, double spacingBetweenGridLines)
        {
            double coeff = 1 / (x + 1),
            radius = ((d * coeff) / 2) * 2;
            return previousR - radius < spacingBetweenGridLines;
        }

        internal void RenderArea(RenderTreeBuilder builder)
        {
            CalculateChartArea();
            CalculateCircleMargin();
            CalculateXAxisRange();
            CalculateRAxisRange();
            MeasureHorizontalAxis();
            MeasureRadialAxis();
            if (smithchart.HorizontalAxis.Visible)
            {
                UpdateHAxis(builder);
            }

            if (smithchart.RadialAxis.Visible)
            {
                UpdateRAxis(builder);
            }

            if (smithchart.HorizontalAxis.Visible)
            {
                DrawHAxisLabels(builder);
            }

            if (smithchart.RadialAxis.Visible)
            {
                DrawRAxisLabels(builder);
            }
        }

        private void CalculateChartArea()
        {
            Rect bounds = smithchart.Bounds;
            double width = bounds.Width,
            height = bounds.Height,
            chartAreaWidth = Math.Min(width, height),
            chartAreaHeight = Math.Min(width, height);
            smithchart.ChartArea = new Rect(bounds.X + ((width / 2) - (chartAreaWidth / 2)), bounds.Y + ((height - chartAreaHeight) / 2 > 0 ? (height - chartAreaHeight) / 2 : 0), chartAreaWidth, chartAreaHeight);
        }

        private void DrawHAxisLabels(RenderTreeBuilder builder)
        {
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + "_HAxisLabels");
            for (int i = 0; i < horizontalLabelCollection.Count; i++)
            {
                HorizontalLabelCollection circleAxis = horizontalLabelCollection[i];
                string label = Math.Round(horizontalLabelCollection[i].Value, 1).ToString(culture);
                if (circleAxis.Value != 0.0)
                {
                    double x = (smithchart.RenderType == RenderType.Impedance) ?
                        circleAxis.CenterX - circleAxis.Radius : circleAxis.CenterX + circleAxis.Radius,
                    y = circleAxis.CenterY;
                    Size textSize = SmithChartHelper.MeasureText(label, smithchart.HorizontalAxis.LabelStyle);
                    x = (smithchart.RenderType == RenderType.Impedance) ? x - textSize.Width : x;
                    if (smithchart.HorizontalAxis.LabelPosition == AxisLabelPosition.Outside)
                    {
                        y -= textSize.Height / 4;
                    }
                    else
                    {
                        y += textSize.Height;
                    }

                    horizontalLabelCollection[i].Region = CalculateRegion(label, textSize, x, y);
                    if (smithchart.HorizontalAxis.LabelIntersectAction == SmithChartLabelIntersectAction.Hide)
                    {
                        HorizontalLabelCollection curLabel = horizontalLabelCollection[i];
                        double curWidth = curLabel.Region.Bounds.Width,
                        curX = curLabel.Region.Bounds.X;
                        for (int j = 1; j < i; j++)
                        {
                            Rect preLabelBounds = horizontalLabelCollection[j].Region?.Bounds;
                            double preX = preLabelBounds != null ? preLabelBounds.X : 0;
                            if (((smithchart.RenderType == RenderType.Impedance) && (preX + (preLabelBounds != null ? preLabelBounds.Width : 0)) > curX) || ((smithchart.RenderType == RenderType.Admittance) && preX < curX + curWidth))
                            {
                                label = string.Empty;
                            }
                        }
                    }

                    SmithChartAxisLabelRenderEventArgs argsData = new SmithChartAxisLabelRenderEventArgs()
                    {
                        EventName = "AxisLabelRendering",
                        Text = label,
                        X = x,
                        Y = y
                    };
                    SfSmithChart.InvokeEvent<SmithChartAxisLabelRenderEventArgs>(smithchart.SmithChartEvents?.AxisLabelRendering, argsData);
                    if (!argsData.Cancel)
                    {
                        TextOptions options = new TextOptions(argsData.X.ToString(culture), argsData.Y.ToString(culture), smithchart.HorizontalAxis.LabelStyle.Color ?? smithchart.SmithChartThemeStyle.AxisLabel, smithchart.HorizontalAxis.LabelStyle.GetFontOptions(), argsData.Text, "none", smithchart.ID + "_HLabel_" + i, string.Empty, "0", "undefined", string.Empty, string.Empty, "opacity: " + smithchart.HorizontalAxis.LabelStyle.Opacity);
                        SmithChartHelper.TextElement(builder, options, smithchart.HorizontalAxis.LabelStyle, smithchart.Rendering);
                    }
                }
            }

            builder.CloseElement();
        }

        private void DrawRAxisLabels(RenderTreeBuilder builder)
        {
            double paddingRadius = 2, angle, curX, curY, curWidth, curHeight, preX, preY, preWidth, preHeight;
            SmithChartRadialAxisLabelStyle font = smithchart.RadialAxis.LabelStyle;
            SmithChartRadialAxis raxis = smithchart.RadialAxis;
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + "_RAxisLabels");
            for (int i = 0; i < labelCollection.Count; i++)
            {
                Point position, textPosition;
                RadialLabelCollection interSectPoint = labelCollection[i];
                string label = interSectPoint.Value.ToString(culture);
                Size textSize = SmithChartHelper.MeasureText(label, font);
                angle = Math.Round(interSectPoint.Angle * 100) / 100;
                if (raxis.LabelPosition == AxisLabelPosition.Outside)
                {
                    position = CirclePointPosition(CircleCenterX, CircleCenterY, interSectPoint.Angle, AreaRadius + paddingRadius);
                    textPosition = SetLabelsOutsidePosition(angle, position.X, position.Y, textSize);
                }
                else
                {
                    position = CirclePointPosition(CircleCenterX, CircleCenterY, interSectPoint.Angle, AreaRadius - paddingRadius);
                    textPosition = SetLabelsInsidePosition(angle, position.X, position.Y, textSize);
                }

                labelCollection[i].Region = CalculateRegion(label, textSize, textPosition.X, textPosition.Y);
                if (raxis.LabelIntersectAction == SmithChartLabelIntersectAction.Hide)
                {
                    Rect curLabelBounds = labelCollection[i].Region.Bounds;
                    curWidth = curLabelBounds.Width;
                    curHeight = curLabelBounds.Height;
                    curX = curLabelBounds.X;
                    curY = curLabelBounds.Y;
                    for (int j = 0; j < i; j++)
                    {
                        Rect preLabelBounds = labelCollection[j].Region.Bounds;
                        preWidth = preLabelBounds.Width;
                        preHeight = preLabelBounds.Height;
                        preX = preLabelBounds.X;
                        preY = preLabelBounds.Y;
                        if ((preX <= curX + curWidth) && (curX <= preX + preWidth) && (preY <= curY + curHeight) && (curY <= preY + preHeight))
                        {
                            label = SPACE;
                        }
                    }
                }

                SmithChartAxisLabelRenderEventArgs argsData = new SmithChartAxisLabelRenderEventArgs()
                {
                    EventName = "AxisLabelRendering",
                    Text = label.ToString(),
                    X = textPosition.X,
                    Y = textPosition.Y
                };
                SfSmithChart.InvokeEvent<SmithChartAxisLabelRenderEventArgs>(smithchart.SmithChartEvents?.AxisLabelRendering, argsData);
                if (!argsData.Cancel)
                {
                    TextOptions options = new TextOptions(argsData.X.ToString(culture), argsData.Y.ToString(culture), font.Color ?? smithchart.SmithChartThemeStyle.AxisLabel, font.GetFontOptions(), argsData.Text, "none", smithchart.ID + "_RLabel_" + i, string.Empty, "0", "undefined", string.Empty, string.Empty, "opacity: " + smithchart.RadialAxis.LabelStyle.Opacity);
                    SmithChartHelper.TextElement(builder, options, font, smithchart.Rendering);
                }
            }

            builder.CloseElement();
        }

        private void CalculateCircleMargin()
        {
            double padding = 10, width = smithchart.ChartArea.Width, radius = smithchart.Radius,
            labelMargin = (smithchart.RadialAxis.LabelPosition == AxisLabelPosition.Outside) ? (MaximumLabelLength() + padding) : padding,
            diameter = width - (labelMargin * 2) > 0 ? width - (labelMargin * 2) : 0, actualRadius = diameter / 2,
            circleCoefficient = radius > 1 ? 1 : (radius < 0.1 ? 0.1 : radius);
            AreaRadius = actualRadius * circleCoefficient;
            CircleLeftX = smithchart.ChartArea.X + labelMargin + (actualRadius * (1 - circleCoefficient));
            CircleTopY = smithchart.ChartArea.Y + labelMargin + (actualRadius * (1 - circleCoefficient));
            CircleCenterX = CircleLeftX + AreaRadius;
            CircleCenterY = smithchart.Bounds.Y + (smithchart.Bounds.Height / 2);
        }

        private double MaximumLabelLength()
        {
            double maximumLabelLength = 0;
            for (int i = 0; i < radialLabels.Length; i++)
            {
                string label = radialLabels[i].ToString(culture);
                Size textSize = SmithChartHelper.MeasureText(label, smithchart.HorizontalAxis.LabelStyle);
                if (maximumLabelLength < textSize.Width)
                {
                    maximumLabelLength = textSize.Width;
                }
            }

            return maximumLabelLength;
        }

        private void CalculateXAxisRange()
        {
            double x, coeff, leftX = CircleLeftX, radius, cx, diameter = AreaRadius * 2, cy = CircleCenterY, circleStartX = CircleLeftX;
            double[] horizontalAxisLabels = CalculateAxisLabels();
            for (int i = 0; i < horizontalAxisLabels.Length; i++)
            {
                x = horizontalAxisLabels[i];
                coeff = 1 / (x + 1);
                radius = (diameter * coeff) / 2;
                if (smithchart.RenderType == RenderType.Impedance)
                {
                    leftX = circleStartX + diameter - (radius * 2);
                }

                cx = leftX + radius;
                horizontalLabelCollection.Add(new HorizontalLabelCollection
                {
                    CenterX = cx,
                    CenterY = cy,
                    Radius = radius,
                    Value = x,
                    Region = null
                });
            }
        }

        private double[] CalculateAxisLabels()
        {
            double spacingBetweenGridLines = 30, previousR = 0, diameter = AreaRadius * 2, coeff, radius;
            List<double> labels = new List<double>();
            double[] staticlabels = new double[] { 2, 3, 4, 5, 10, 20, 50 };
            for (float i = 0; i < 2f; i += 0.1f)
            {
                i = (float)Math.Round((decimal)i * 10) / 10;
                coeff = 1 / (i + 1);
                radius = (diameter * coeff / 2) * 2;
                if (previousR == 0.0 || i == 1)
                {
                    previousR = radius;
                    labels.Add(i);
                    continue;
                }

                if ((i < 1) && IsOverlap(1, diameter, radius, spacingBetweenGridLines))
                {
                    continue;
                }

                if ((i > 1) && IsOverlap(2, diameter, radius, spacingBetweenGridLines))
                {
                    continue;
                }

                if (previousR - radius >= spacingBetweenGridLines)
                {
                    labels.Add(i);
                    previousR = radius;
                }
            }

            for (int k = 0; k < staticlabels.Length; k++)
            {
                labels.Add(staticlabels[k]);
            }

            return labels.ToArray();
        }

        private void CalculateRAxisRange()
        {
            double arcCy, arcRadius, diameter = AreaRadius * 2, y;
            Point point = new Point(0, 0);
            radialLabelCollection = new List<LabelCollection>();
            if (smithchart.RenderType == RenderType.Impedance)
            {
                point.X = CircleLeftX + diameter;
                point.Y = CircleTopY + AreaRadius;
            }
            else
            {
                point.X = CircleLeftX;
                point.Y = CircleTopY + AreaRadius;
            }

            for (int i = 0; i < radialLabels.Length; i++)
            {
                y = radialLabels[i];
                arcRadius = Math.Abs(1 / y * diameter / 2);
                if (smithchart.RenderType == RenderType.Impedance)
                {
                    arcCy = y > 0 ? point.Y - arcRadius : point.Y + arcRadius;
                }
                else
                {
                    arcCy = y < 0 ? point.Y - arcRadius : point.Y + arcRadius;
                }

                radialLabelCollection.Add(new RadialLabelCollection
                {
                    CenterX = point.X,
                    CenterY = arcCy,
                    Radius = arcRadius,
                    Value = y
                });
            }
        }

        private void MeasureHorizontalAxis()
        {
            if (smithchart.HorizontalAxis.MajorGridLines.Visible)
            {
                MeasureHMajorGridLines();
            }

            if (smithchart.HorizontalAxis.MinorGridLines.Visible)
            {
                MeasureHMinorGridLines();
            }
        }

        private void MeasureHMinorGridLines()
        {
            List<LabelCollection> radialPoint1 = new List<LabelCollection>(),
            radialPoint2 = new List<LabelCollection>();
            List<List<LabelCollection>> arcPoints = new List<List<LabelCollection>>();
            bool isLargeArc;
            Point startPoint, endPoint;
            HorizontalLabelCollection previous, next;
            double maxCount = smithchart.HorizontalAxis.MinorGridLines.Count, cx, k = 0, diameter = AreaRadius * 2, space, count, interval, radius, leftX;
            minorHGridArcPoints = new List<GridArcPoints>();
            for (int i = 0; i < horizontalLabelCollection.Count - 3; i++)
            {
                previous = horizontalLabelCollection[i];
                next = horizontalLabelCollection[i + 1];
                space = (previous.Radius - next.Radius) * 2;
                count = Math.Floor(maxCount / 100 * space);
                interval = space / count;
                for (int j = 0; j < count; j++)
                {
                    radius = next.Radius + (j * interval / 2);
                    leftX = (smithchart.RenderType == RenderType.Impedance) ? CircleLeftX + diameter - (radius * 2) : CircleLeftX;
                    cx = leftX + radius;
                    isLargeArc = next.Value > 5;
                    arcPoints = CalculateMinorArcStartEndPoints(next.Value);
                    if (smithchart.RenderType == RenderType.Impedance)
                    {
                        radialPoint1 = arcPoints[0];
                        radialPoint2 = arcPoints[1];
                    }
                    else
                    {
                        radialPoint1 = arcPoints[1];
                        radialPoint2 = arcPoints[0];
                    }

                    startPoint = IntersectingCirclePoints(radialPoint1[0].CenterX, radialPoint1[0].CenterY, radialPoint1[0].Radius, cx, previous.CenterY, radius, smithchart.RenderType);
                    endPoint = IntersectingCirclePoints(radialPoint2[0].CenterX, radialPoint2[0].CenterY, radialPoint2[0].Radius, cx, previous.CenterY, radius, smithchart.RenderType);
                    minorHGridArcPoints.Add(new GridArcPoints
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint,
                        RotationAngle = 2 * Math.PI,
                        SweepDirection = (smithchart.RenderType == RenderType.Impedance) ? direction.CounterClockwise : direction.Clockwise,
                        IsLargeArc = isLargeArc,
                        Size = new Size(radius, radius)
                    });
                    k++;
                }
            }
        }

        private List<List<LabelCollection>> CalculateMinorArcStartEndPoints(double point)
        {
            double calValue1, calValue2;
            List<List<LabelCollection>> marcHPoints = new List<List<LabelCollection>>();
            if (point <= 0.1)
            {
                calValue1 = 1.0;
                calValue2 = -1.0;
            }
            else if (point <= 0.2)
            {
                calValue1 = 0.8;
                calValue2 = -0.8;
            }
            else if (point <= 0.3)
            {
                calValue1 = 0.4;
                calValue2 = -0.4;
            }
            else if (point <= 0.6)
            {
                calValue1 = 1.0;
                calValue2 = -1.0;
            }
            else if (point <= 1.0)
            {
                calValue1 = 1.5;
                calValue2 = -1.5;
            }
            else if (point <= 1.5)
            {
                calValue1 = 2.0;
                calValue2 = -2.0;
            }
            else if (point <= 2.0)
            {
                calValue1 = 1.0;
                calValue2 = -1.0;
            }
            else if (point <= 5.0)
            {
                calValue1 = 3.0;
                calValue2 = -3.0;
            }
            else
            {
                calValue1 = 10.0;
                calValue2 = -10.0;
            }

            marcHPoints.Add(radialLabelCollection.Where(item => item.Value == calValue1).ToList());
            marcHPoints.Add(radialLabelCollection.Where(item => item.Value == calValue2).ToList());
            return marcHPoints;
        }

        private void MeasureRadialAxis()
        {
            if (smithchart.RadialAxis.MajorGridLines.Visible)
            {
                MeasureRMajorGridLines();
            }

            if (smithchart.RadialAxis.MinorGridLines.Visible)
            {
                MeasureRMinorGridLines();
            }
        }

        private void MeasureRMinorGridLines()
        {
            double maxCount = smithchart.RadialAxis.MinorGridLines.Count, distance, count, interval, centerValue, radius, sweepDirection,
            arcCx, nextAngle, k = 0, betweenAngle, circumference = Math.PI * (AreaRadius * 2), arcStartX, arcStartY = CircleCenterY, outterInterSectAngle, arcCy;
            List<LabelCollection> circlePoint;
            Point arcStartPoint, outerInterSectPoint, innerInterSectPoint, startPoint, endPoint;
            RadialLabelCollection previous, next;
            arcStartX = arcCx = (smithchart.RenderType == RenderType.Impedance) ? CircleCenterX + AreaRadius : CircleCenterX - AreaRadius;
            minorGridArcPoints = new List<GridArcPoints>();
            arcStartPoint = new Point(arcStartX, arcStartY);
            for (int i = 2; i < labelCollection.Count - 3; i++)
            {
                previous = labelCollection[i];
                next = labelCollection[i + 1];
                if (smithchart.RenderType == RenderType.Impedance)
                {
                    nextAngle = next.Angle == 360 ? 0 : next.Angle;
                    betweenAngle = Math.Abs(nextAngle - previous.Angle);
                }
                else
                {
                    nextAngle = previous.Angle == 360 ? 0 : previous.Angle;
                    betweenAngle = Math.Abs(nextAngle - next.Angle);
                }

                distance = (circumference / 360) * betweenAngle;
                count = Math.Floor((maxCount / 100) * distance);
                interval = betweenAngle / count;
                centerValue = next.Value > 0 ? next.Value : previous.Value;
                circlePoint = MinorGridLineArcIntersectCircle(Math.Abs(centerValue));
                for (int j = 1; j < count; j++)
                {
                    outterInterSectAngle = (interval * j) + (previous.Angle == 360 ? nextAngle : previous.Angle);
                    outerInterSectPoint = CirclePointPosition(CircleCenterX, CircleCenterY, outterInterSectAngle, AreaRadius);
                    radius = ArcRadius(arcStartPoint, outerInterSectPoint, outterInterSectAngle);
                    arcCy = outterInterSectAngle > 180 ? CircleCenterY - radius : CircleCenterY + radius;
                    innerInterSectPoint = IntersectingCirclePoints(arcCx, arcCy, radius, circlePoint[0].CenterX, circlePoint[0].CenterY, circlePoint[0].Radius, smithchart.RenderType);
                    startPoint = new Point(innerInterSectPoint.X, y: innerInterSectPoint.Y);
                    endPoint = new Point(outerInterSectPoint.X, y: outerInterSectPoint.Y);
                    Size size = new Size(radius, radius);
                    sweepDirection = previous.Value >= 0 ? direction.Clockwise : direction.CounterClockwise;
                    minorGridArcPoints.Add(new GridArcPoints
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint,
                        RotationAngle = 2 * Math.PI,
                        SweepDirection = sweepDirection,
                        IsLargeArc = false,
                        Size = size
                    });
                    k++;
                }
            }
        }

        private List<LabelCollection> MinorGridLineArcIntersectCircle(double centerValue)
        {
            List<LabelCollection> circlePoint;
            double calValue;
            if (centerValue >= 3)
            {
                calValue = 20;
            }
            else if (centerValue >= 1.5)
            {
                calValue = 10;
            }
            else if (centerValue >= 0.6)
            {
                calValue = 3;
            }
            else
            {
                calValue = 2;
            }

            circlePoint = horizontalLabelCollection.Where(item => item.Value == calValue).Cast<LabelCollection>().ToList();
            return circlePoint;
        }

        private void MeasureRMajorGridLines()
        {
            LabelCollection radialPoint;
            Point innerInterSectPoint, outerInterSectPoint;
            double epsilon, y, outterInterSectRadian, outterInterSectAngle, sweepDirection;
            Point startPoint, endPoint;
            Size size;
            majorRGridArcPoints = new List<GridArcPoints>();
            labelCollection = new List<RadialLabelCollection>();
            epsilon = SmithChartHelper.GetEpsilonValue();
            for (int i = 0; i < radialLabelCollection.Count; i++)
            {
                radialPoint = radialLabelCollection[i];
                if (radialPoint.Radius <= epsilon)
                {
                    continue;
                }

                y = radialPoint.Value;
                List<Point> arcPoints = CalculateMajorArcStartEndPoints(radialPoint, Math.Abs(y));
                innerInterSectPoint = arcPoints[0];
                outerInterSectPoint = arcPoints[1];
                outterInterSectRadian = CircleXYRadianValue(CircleCenterX, CircleCenterY, outerInterSectPoint.X, outerInterSectPoint.Y);
                outterInterSectAngle = outterInterSectRadian * (180 / Math.PI);
                if (y != 0.0)
                {
                    startPoint = new Point(innerInterSectPoint.X, innerInterSectPoint.Y);
                    endPoint = new Point(outerInterSectPoint.X, outerInterSectPoint.Y);
                    size = new Size(radialPoint.Radius, radialPoint.Radius);
                    sweepDirection = y > 0 ? direction.Clockwise : direction.CounterClockwise;
                    majorRGridArcPoints.Add(new GridArcPoints
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint,
                        Size = size,
                        RotationAngle = 2 * Math.PI,
                        IsLargeArc = false,
                        SweepDirection = sweepDirection
                    });
                    labelCollection.Add(new RadialLabelCollection
                    {
                        CenterX = outerInterSectPoint.X,
                        CenterY = outerInterSectPoint.Y,
                        Angle = outterInterSectAngle,
                        Value = y,
                        Radius = AreaRadius
                    });
                }
                else
                {
                    startPoint = new Point(CircleLeftX, CircleCenterY);
                    endPoint = new Point(CircleCenterX + AreaRadius, CircleCenterY);
                    majorRGridArcPoints.Add(new GridArcPoints
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint
                    });
                    labelCollection.Add(new RadialLabelCollection
                    {
                        CenterX = (smithchart.RenderType == RenderType.Impedance) ? (CircleCenterX - AreaRadius) : (CircleCenterX + AreaRadius),
                        CenterY = CircleCenterY,
                        Angle = (smithchart.RenderType == RenderType.Impedance) ? 180 : 360,
                        Value = y,
                        Radius = AreaRadius
                    });
                }
            }
        }

        private List<Point> CalculateMajorArcStartEndPoints(LabelCollection radialPoint, double point)
        {
            List<Point> arcPoints = new List<Point>();
            List<HorizontalLabelCollection> circlePoint = new List<HorizontalLabelCollection>();
            double cx = CircleCenterX, cy = CircleCenterY;
            if (point >= 10)
            {
                arcPoints.Add((smithchart.RenderType == RenderType.Impedance) ? new Point(cx + AreaRadius, cy) : new Point(cx - AreaRadius, cy));
            }
            else if (point >= 3)
            {
                circlePoint = horizontalLabelCollection.Where(item => item.Value == 10).ToList();
            }
            else if (point >= 1)
            {
                circlePoint = horizontalLabelCollection.Where(item => item.Value == 5).ToList();
            }
            else
            {
                circlePoint = horizontalLabelCollection.Where(item => item.Value == 3).ToList();
            }

            if (circlePoint.Count > 0)
            {
                arcPoints.Add(IntersectingCirclePoints(radialPoint.CenterX, radialPoint.CenterY, radialPoint.Radius, circlePoint[0].CenterX, circlePoint[0].CenterY, circlePoint[0].Radius, smithchart.RenderType));
            }

            arcPoints.Add(IntersectingCirclePoints(radialPoint.CenterX, radialPoint.CenterY, radialPoint.Radius, cx, cy, AreaRadius, smithchart.RenderType));

            return arcPoints;
        }

        private List<List<LabelCollection>> CalculateHMajorArcStartEndPoints(double point)
        {
            List<List<LabelCollection>> arcHPoints = new List<List<LabelCollection>>();
            double calValue1, calValue2;
            if (point <= 0.3)
            {
                calValue1 = 2.0;
                calValue2 = -2.0;
            }
            else if (point <= 1.0)
            {
                calValue1 = 3.0;
                calValue2 = -3.0;
            }
            else if (point <= 2.0)
            {
                calValue1 = 5.0;
                calValue2 = -5.0;
            }
            else if (point <= 5.0)
            {
                calValue1 = 10.0;
                calValue2 = -10.0;
            }
            else
            {
                calValue1 = 50.0;
                calValue2 = -50.0;
            }

            arcHPoints.Add(radialLabelCollection.Where(item => item.Value == calValue1).ToList());
            arcHPoints.Add(radialLabelCollection.Where(item => item.Value == calValue2).ToList());
            return arcHPoints;
        }

        private void MeasureHMajorGridLines()
        {
            List<List<LabelCollection>> arcPoints = new List<List<LabelCollection>>();
            Point startPoint, endPoint;
            List<LabelCollection> radialPoint1 = new List<LabelCollection>(),
            radialPoint2 = new List<LabelCollection>();
            majorHGridArcPoints = new List<GridArcPoints>();
            for (int i = 0; i < horizontalLabelCollection.Count; i++)
            {
                HorizontalLabelCollection circlePoint = new HorizontalLabelCollection();
                circlePoint = horizontalLabelCollection[i];
                arcPoints = CalculateHMajorArcStartEndPoints(circlePoint.Value);
                if (smithchart.RenderType == RenderType.Impedance)
                {
                    radialPoint1 = arcPoints[0];
                    radialPoint2 = arcPoints[1];
                }
                else
                {
                    radialPoint1 = arcPoints[1];
                    radialPoint2 = arcPoints[0];
                }

                Size size = new Size(circlePoint.Radius, circlePoint.Radius);
                if (circlePoint.Value != 0.0 && circlePoint.Value != 50.0)
                {
                    startPoint = IntersectingCirclePoints(radialPoint1[0].CenterX, radialPoint1[0].CenterY, radialPoint1[0].Radius, circlePoint.CenterX, circlePoint.CenterY, circlePoint.Radius, smithchart.RenderType);
                    endPoint = IntersectingCirclePoints(radialPoint2[0].CenterX, radialPoint2[0].CenterY, radialPoint2[0].Radius, circlePoint.CenterX, circlePoint.CenterY, circlePoint.Radius, smithchart.RenderType);
                    majorHGridArcPoints.Add(new GridArcPoints
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint,
                        RotationAngle = 2 * Math.PI,
                        SweepDirection = (smithchart.RenderType == RenderType.Impedance) ?
                        direction.CounterClockwise : direction.Clockwise,
                        IsLargeArc = true,
                        Size = size
                    });
                }
                else
                {
                    startPoint = new Point(circlePoint.CenterX + circlePoint.Radius, circlePoint.CenterY);
                    endPoint = new Point(circlePoint.CenterX + circlePoint.Radius, circlePoint.CenterY - 0.05);
                    majorHGridArcPoints.Add(new GridArcPoints
                    {
                        StartPoint = startPoint,
                        EndPoint = endPoint,
                        RotationAngle = 2 * Math.PI,
                        SweepDirection = direction.Clockwise,
                        IsLargeArc = true,
                        Size = size
                    });
                }
            }
        }

        private void UpdateHAxis(RenderTreeBuilder builder)
        {
            if (smithchart.HorizontalAxis.MajorGridLines.Visible)
            {
                SmithChartHorizontalMajorGridLines majorGridLine = smithchart.HorizontalAxis.MajorGridLines;
                PathOptions options = new PathOptions(smithchart.ID + "_horizontalAxisMajorGridLines", string.Empty, majorGridLine.DashArray, majorGridLine.Width, majorGridLine.Color ?? smithchart.SmithChartThemeStyle.MajorGridLine, majorGridLine.Opacity);
                UpdateGridLines(builder, "_svg_horizontalAxisMajorGridLines", options, majorHGridArcPoints);
            }

            if (smithchart.HorizontalAxis.MinorGridLines.Visible)
            {
                SmithChartHorizontalMinorGridLines minorGridLine = smithchart.HorizontalAxis.MinorGridLines;
                PathOptions options = new PathOptions(smithchart.ID + "_horizontalAxisMinorGridLines", string.Empty, minorGridLine.DashArray, minorGridLine.Width, minorGridLine.Color ?? smithchart.SmithChartThemeStyle.MinorGridLine);
                UpdateGridLines(builder, "_svg_horizontalAxisMinorGridLines", options, minorHGridArcPoints);
            }

            if (smithchart.HorizontalAxis.AxisLine.Visible)
            {
                double radius = AreaRadius;
                SmithChartHorizontalAxisLine axisLine = smithchart.HorizontalAxis.AxisLine;
                PathOptions options = new PathOptions(smithchart.ID + "_horizontalAxisLine", string.Empty, axisLine.DashArray, axisLine.Width, axisLine.Color ?? smithchart.SmithChartThemeStyle.AxisLine);
                UpdateAxisLine(builder, options, "_svg_h_AxisLine", 1, Math.PI * 2, direction.Clockwise, new Size(radius, radius), new Point(CircleCenterX + radius, CircleCenterY), new Point(CircleCenterX + radius, CircleCenterY - 0.05));
            }
        }

        private void UpdateRAxis(RenderTreeBuilder builder)
        {
            SmithChartRadialAxis raxis = smithchart.RadialAxis;
            if (raxis.MajorGridLines.Visible)
            {
                SmithChartRadialMajorGridLines majorGridLine = smithchart.RadialAxis.MajorGridLines;
                PathOptions options = new PathOptions(smithchart.ID + "_radialAxisMajorGridLines", string.Empty, majorGridLine.DashArray, majorGridLine.Width, majorGridLine.Color ?? smithchart.SmithChartThemeStyle.MajorGridLine, majorGridLine.Opacity);
                UpdateGridLines(builder, "_svg_radialAxisMajorGridLines", options, majorRGridArcPoints);
            }

            if (raxis.MinorGridLines.Visible)
            {
                SmithChartRadialMinorGridLines minorGridLine = smithchart.RadialAxis.MinorGridLines;
                PathOptions options = new PathOptions(smithchart.ID + "_radialAxisMinorGridLines", string.Empty, minorGridLine.DashArray, minorGridLine.Width, minorGridLine.Color ?? smithchart.SmithChartThemeStyle.MinorGridLine);
                UpdateGridLines(builder, "_svg_radialAxisMinorGridLines", options, minorGridArcPoints);
            }

            if (raxis.AxisLine.Visible)
            {
                double radius = AreaRadius;
                SmithChartRadialAxisLine axisLine = smithchart.RadialAxis.AxisLine;
                PathOptions options = new PathOptions(smithchart.ID + "_radialAxisLine", string.Empty, axisLine.DashArray, axisLine.Width, axisLine.Color ?? smithchart.SmithChartThemeStyle.AxisLine);
                UpdateAxisLine(builder, options, "_svg_r_AxisLine", 0, 0, direction.CounterClockwise, new Size(0, 0), new Point(CircleCenterX - radius, CircleCenterY), new Point(CircleCenterX + radius, CircleCenterY));
            }
        }

        private void UpdateAxisLine(RenderTreeBuilder builder, PathOptions options, string id, double isLargeArc, double angle, double sweep, Size size, Point point1, Point point2)
        {
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + id);
            options.Direction = "M " + SPACE + point1.X.ToString(culture) + SPACE + point1.Y.ToString(culture) + SPACE + "A" + SPACE + size.Width.ToString(culture) +
                SPACE + size.Height.ToString(culture) + SPACE + angle.ToString(culture) + SPACE + isLargeArc.ToString(culture) + SPACE + sweep.ToString(culture) + SPACE + point2.X.ToString(culture) + SPACE + point2.Y.ToString(culture) + SPACE;
            smithchart.Rendering.RenderPath(builder, options);
            builder.CloseElement();
        }

        private void UpdateGridLines(RenderTreeBuilder builder, string groupElementId, PathOptions options, List<GridArcPoints> gridPoints)
        {
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + groupElementId);
            options.Direction = CalculateGridLinesPath(gridPoints);
            smithchart.Rendering.RenderPath(builder, options);
            builder.CloseElement();
        }

        private string CalculateGridLinesPath(List<GridArcPoints> points)
        {
            double r1, r2, angle, isLargeArc, sweep;
            string sb = string.Empty;
            for (int i = 0; i < points.Count; i++)
            {
                GridArcPoints pathSegment = points[i];
                r1 = pathSegment.Size != null ? pathSegment.Size.Width : 0;
                r2 = pathSegment.Size != null ? pathSegment.Size.Height : 0;
                angle = !double.IsNaN(pathSegment.RotationAngle) ? pathSegment.RotationAngle : 0;
                isLargeArc = pathSegment.IsLargeArc ? 1 : 0;
                sweep = !double.IsNaN(pathSegment.SweepDirection) ? pathSegment.SweepDirection : 0;
                sb = string.Concat(sb, "M " + SPACE + pathSegment.StartPoint.X.ToString(culture) + SPACE + pathSegment.StartPoint.Y.ToString(culture) + SPACE + "A" + SPACE + r1.ToString(culture) + SPACE + r2.ToString(culture) + SPACE + angle.ToString(culture) + SPACE + isLargeArc.ToString(culture) + SPACE + sweep.ToString(culture) + SPACE + pathSegment.EndPoint.X.ToString(culture) + SPACE + pathSegment.EndPoint.Y.ToString(culture) + SPACE);
            }

            return sb.ToString(culture);
        }

        internal void Dispose()
        {
            smithchart = null;
            radialLabelCollection = null;
            horizontalLabelCollection = null;
            majorHGridArcPoints = null;
            minorHGridArcPoints = null;
            majorRGridArcPoints = null;
            minorGridArcPoints = null;
            labelCollection = null;
            direction = null;
        }
    }
}