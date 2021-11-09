using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Models;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal partial class DataLabelModule : AccumulationBase
    {
        private AccumulationChartSeries series { get; set; }

        private Rect areaRect { get; set; }

        private double marginValue { get; set; }

        private ChartHelper helper { get; set; }

        internal DataLabelModule(SfAccumulationChart chart)
            : base(chart)
        {
            helper = AccumulationChartInstance.ChartHelper;
        }

        internal void FindAreaRect()
        {
            areaRect = new Rect(0, 0, AccumulationChartInstance.AvailableSize.Width, AccumulationChartInstance.AvailableSize.Height);
            ChartHelper.SubtractThickness(areaRect, new Thickness(AccumulationChartInstance.Margin.Left, AccumulationChartInstance.Margin.Right, AccumulationChartInstance.Margin.Top, AccumulationChartInstance.Margin.Bottom));
        }

        internal void RenderDataLabel(AccumulationPoints point, AccumulationDataLabelSettings dataLabel, List<AccumulationPoints> points, AccumulationChartSeries seriesValue)
        {
            const double padding = 4;
            series = seriesValue;
            if (point.AccTextArgs == null)
            {
#pragma warning disable CA1305
                point.Label = !string.IsNullOrEmpty(point.Text) ? point.Text : !string.IsNullOrEmpty(point.OriginalText) ? point.OriginalText : point.Y?.ToString(culture);
            }
            else
            {
                point.Label = !string.IsNullOrEmpty(point.AccTextArgs.Text) ? point.AccTextArgs.Text : !string.IsNullOrEmpty(point.OriginalText) ? point.OriginalText : point.Y?.ToString(culture);
            }
#pragma warning restore CA1305
            marginValue = point.AccTextArgs != null && !string.IsNullOrEmpty(point.AccTextArgs.Border.Color) ? 5 + point.AccTextArgs.Border.Width : 1;
            if (dataLabel.Template == null)
            {
                point.TextSize = ChartHelper.MeasureText(point.Label, SfAccumulationChart.GetFontOptions(dataLabel.Font));
                point.TextSize.Height += padding;
                point.TextSize.Width += padding;
            }
            else if (string.IsNullOrEmpty(point.TemplateID))
            {
                point.TextSize = new Size(0, 0);
            }

            GetDataLabelPosition(point, dataLabel, points, point.TextSize);
            if (point.LabelRegion != null)
            {
                CorrectLabelRegion(point.LabelRegion, point.TextSize);
            }
        }

        private void GetDataLabelPosition(AccumulationPoints point, AccumulationDataLabelSettings dataLabel, List<AccumulationPoints> points, Size textSize)
        {
            point.LabelPosition = null;
            GetLabelRegion(point, dataLabel.Position, textSize,
                IsCircular() ? !IsVariableRadius() ? AccumulationChartInstance.PieSeriesModule.LabelRadius : AccumulationChartInstance.PieSeriesModule.GetLabelRadius(series, point) : GetLabelDistance(point, dataLabel), marginValue);
            point.LabelAngle = point.MidAngle;
            point.LabelPosition = dataLabel.Position;
            if (AccumulationChartInstance.EnableSmartLabels)
            {
                GetSmartLabel(point, dataLabel, textSize, points);
            }
        }

        private double GetLabelDistance(AccumulationPoints point, AccumulationDataLabelSettings datalabel)
        {
            if (point.LabelPosition != null && datalabel.Position != point.LabelPosition || (datalabel.ConnectorStyle.Length != null && point.LabelPosition != AccumulationLabelPosition.Inside))
            {
                double length = DataVizCommonHelper.StringToNumber(datalabel.ConnectorStyle.Length != null ? datalabel.ConnectorStyle.Length : "70px", AccumulationChartInstance.InitialClipRect.Width);
                if (length < AccumulationChartInstance.InitialClipRect.Width)
                {
                    return length;
                }
            }

            return (AccumulationLabelPosition)(!string.IsNullOrEmpty(point.LabelPosition.ToString()) ? point.LabelPosition : datalabel.Position) == AccumulationLabelPosition.Outside ? AccumulationChartInstance.InitialClipRect.Width - (point.SymbolLocation.X + point.LabelOffset.X) - ((AccumulationChartInstance.InitialClipRect.Width - series.TriangleSize.Width) * 0.5) : 0;
        }

        private void GetLabelRegion(AccumulationPoints point, AccumulationLabelPosition position, Size textSize, double labelRadius, double margin, double endAngle = 0)
        {
            double labelAngle = endAngle != 0 ? endAngle : point.MidAngle;
            ChartInternalLocation location = ChartHelper.DegreeToLocation(labelAngle, labelRadius, IsCircular() ? Center : GetLabelLocation(point, position.ToString()));
            location.X = position == AccumulationLabelPosition.Inside ? location.X - (textSize.Width * 0.5) : location.X;
            location.Y = position == AccumulationLabelPosition.Inside ? location.Y - (textSize.Height * 0.5) : location.Y;
            point.LabelRegion = new Rect(location.X, location.Y, textSize.Width + (margin * 2), textSize.Height + (margin * 2));
            if (position == AccumulationLabelPosition.Outside)
            {
                point.LabelRegion.Y -= point.LabelRegion.Height / 2;
                if (labelAngle >= 90 && labelAngle <= 270)
                {
                    point.LabelRegion.X -= point.LabelRegion.Width + 10;
                }
                else
                {
                    point.LabelRegion.X += 10;
                }
            }
        }

        private ChartInternalLocation GetLabelLocation(AccumulationPoints point, string position = "Outside")
        {
            if (!IsCircular())
            {
                position = position == "OutsideLeft" ? "OutsideLeft" : !string.IsNullOrEmpty(point.LabelPosition.ToString()) ? point.LabelPosition.ToString() : position;
                ChartInternalLocation location = new ChartInternalLocation(point.SymbolLocation.X, point.SymbolLocation.Y - point.LabelOffset.Y);
                switch (position)
                {
                    case "Inside":
                        location.Y = point.Region.Y + (point.Region.Height / 2);
                        break;
                    case "Outside":
                        location.X += point.LabelOffset.X;
                        break;
                    case "OutsideLeft":
                        location.X -= point.LabelOffset.X;
                        break;
                }

                return location;
            }
            else
            {
                return ChartHelper.DegreeToLocation(point.MidAngle, !IsVariableRadius() ? Radius : DataVizCommonHelper.StringToNumber(point.SliceRadius, AccumulationChartInstance.PieSeriesModule.SeriesRadius), Center);
            }
        }

        private static void CorrectLabelRegion(Rect labelRegion, Size textSize, double padding = 4)
        {
            labelRegion.Height -= padding;
            labelRegion.Width -= padding;
            labelRegion.X += padding / 2;
            labelRegion.Y += padding / 2;
            textSize.Height -= padding;
            textSize.Width -= padding;
        }

        private static AccumulationPoints FindPreviousPoint(List<AccumulationPoints> points, int index, AccumulationLabelPosition position)
        {
            for (var i = index - 1; i >= 0; i--)
            {
                if (points[i].Visible && points[i].LabelVisible && points[i].LabelRegion != null && points[i].LabelPosition == position)
                {
                    return points[i];
                }
            }

            return null;
        }

        private static bool IsOverlapping(AccumulationPoints currentPoint, List<AccumulationPoints> points)
        {
            for (var i = currentPoint.Index - 1; i >= 0; i--)
            {
                if (points[i].Visible && points[i].LabelVisible && points[i].LabelRegion != null && currentPoint.LabelRegion != null && currentPoint.LabelVisible && IsOverlap(currentPoint.LabelRegion, points[i].LabelRegion))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsRect(Rect currentRect, Rect rect)
        {
            return currentRect.X <= rect.X && currentRect.X + currentRect.Width >= rect.X + rect.Width && currentRect.Y <= rect.Y && currentRect.Height + currentRect.Y >= rect.Y + rect.Height;
        }

        private void GetSmartLabel(AccumulationPoints point, AccumulationDataLabelSettings dataLabel, Size textSize, List<AccumulationPoints> points)
        {
            bool circular = IsCircular();
            double labelRadius = circular ? Radius : GetLabelDistance(point, dataLabel);
            string connectorLength = circular ? (string.IsNullOrEmpty(dataLabel.ConnectorStyle.Length) ? "4%" : dataLabel.ConnectorStyle.Length) : "0px";
            labelRadius += DataVizCommonHelper.StringToNumber(connectorLength, labelRadius);
            AccumulationPoints previousPoint = FindPreviousPoint(points, point.Index, (AccumulationLabelPosition)point.LabelPosition);
            if (dataLabel.Position == AccumulationLabelPosition.Inside)
            {
                if (previousPoint != null && previousPoint.LabelRegion != null && (IsOverlap(point.LabelRegion, previousPoint.LabelRegion) || IsOverlapping(point, points)) || !circular && !ContainsRect(point.Region, point.LabelRegion))
                {
                    point.LabelPosition = AccumulationLabelPosition.Outside;
                    if (!circular)
                    {
                        labelRadius = GetLabelDistance(point, dataLabel);
                    }

                    GetLabelRegion(point, (AccumulationLabelPosition)point.LabelPosition, textSize, labelRadius, marginValue);
                    previousPoint = FindPreviousPoint(points, point.Index, (AccumulationLabelPosition)point.LabelPosition);
                    if (previousPoint != null && (IsOverlap(point.LabelRegion, previousPoint.LabelRegion) || IsConnectorLineOverlapping(point, previousPoint)))
                    {
                        SetOuterSmartLabel(previousPoint, point, dataLabel.Border.Width, labelRadius, textSize, marginValue);
                    }
                }
            }
            else if (previousPoint != null && previousPoint.LabelRegion != null && (IsOverlap(point.LabelRegion, previousPoint.LabelRegion) || IsOverlapping(point, points) || IsConnectorLineOverlapping(point, previousPoint)))
            {
                SetOuterSmartLabel(previousPoint, point, dataLabel.Border.Width, labelRadius, textSize, marginValue);
            }

            if (IsOverlapping(point, points) && !IsCircular())
            {
                double labelRadiusL = circular ? Radius : GetLabelDistance(point, dataLabel);
                ChartInternalLocation location = ChartHelper.DegreeToLocation(double.IsNaN(point.MidAngle) ? 0 : point.MidAngle, -labelRadiusL, IsCircular() ? Center : GetLabelLocation(point, "OutsideLeft"));
                point.LabelRegion = new Rect(
                    location.X,
                    location.Y,
                    textSize.Width + (marginValue * 2),
                    textSize.Height + (marginValue * 2));
                point.LabelRegion.Y -= point.LabelRegion.Height / 2;
                point.LabelRegion.X = point.LabelRegion.X - 10 - point.LabelRegion.Width;
                if (previousPoint != null && previousPoint.LabelRegion != null && (IsOverlap(point.LabelRegion, previousPoint.LabelRegion) || IsOverlapping(point, points) || IsConnectorLineOverlapping(point, previousPoint)))
                {
                    SetOuterSmartLabel(previousPoint, point, dataLabel.Border.Width, labelRadiusL, textSize, marginValue);
                }
            }
        }

        private bool IsConnectorLineOverlapping(AccumulationPoints point, AccumulationPoints previous)
        {
            string position = string.Empty;
            if (!IsCircular() && point.LabelRegion.X < point.Region.X)
            {
                position = "outsideLeft";
            }

            ChartInternalLocation startLocation = GetLabelLocation(point, position);
            ChartInternalLocation endLocation = new ChartInternalLocation(0, 0);
            GetEdgeOfLabel(point.LabelRegion, point.LabelAngle, endLocation, point, 0);
            ChartInternalLocation previousend = new ChartInternalLocation(0, 0);
            GetEdgeOfLabel(previous.LabelRegion, previous.LabelAngle, previousend, point, 0);
            return IsLineRectangleIntersect(startLocation, endLocation, point.LabelRegion) || IsLineRectangleIntersect(startLocation, endLocation, previous.LabelRegion) || IsLineRectangleIntersect(GetLabelLocation(previous), previousend, point.LabelRegion);
        }

        private static ChartInternalLocation GetEdgeOfLabel(Rect labelshape, double angle, ChartInternalLocation middle, in AccumulationPoints point, double border = 1)
        {
            ChartInternalLocation edge = new ChartInternalLocation(labelshape.X, labelshape.Y);
            if (angle >= 90 && angle <= 270)
            {
                edge.X += labelshape.Width + (border / 2);
                edge.Y += labelshape.Height / 2;
                middle.X = edge.X + 10;
            }
            else if (point != null && point.Region != null && point.Region.X > point.LabelRegion.X)
            {
                edge.X += (border * 2) + labelshape.Width;
                edge.Y += labelshape.Height / 2;
                middle.X = edge.X + 10;
            }
            else
            {
                edge.X -= border / 2;
                edge.Y += labelshape.Height / 2;
                middle.X = edge.X - 10;
            }

            middle.Y = edge.Y;
            return edge;
        }

        private static bool IsLineRectangleIntersect(ChartInternalLocation line1, ChartInternalLocation line2, Rect rect)
        {
            ChartInternalLocation[] rectPoints = new ChartInternalLocation[]
            {
                new ChartInternalLocation(Math.Round(rect.X), Math.Round(rect.Y)),
                new ChartInternalLocation(Math.Round(rect.X + rect.Width), Math.Round(rect.Y)),
                new ChartInternalLocation(Math.Round(rect.X + rect.Width), Math.Round(rect.Y + rect.Height)),
                new ChartInternalLocation(Math.Round(rect.X), Math.Round(rect.Y + rect.Height))
            };
            line1.X = Math.Round(line1.X);
            line1.Y = Math.Round(line1.Y);
            line2.X = Math.Round(line2.X);
            line2.Y = Math.Round(line2.Y);
            for (var i = 0; i < rectPoints.Length; i++)
            {
                if (IsLinesIntersect(line1, line2, rectPoints[i], rectPoints[(i + 1) % rectPoints.Length]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsLinesIntersect(ChartInternalLocation point1, ChartInternalLocation point2, ChartInternalLocation point11, ChartInternalLocation point12)
        {
            double a1 = point2.Y - point1.Y;
            double b1 = point1.X - point2.X;
            double c1 = (a1 * point1.X) + (b1 * point1.Y);
            double a2 = point12.Y - point11.Y;
            double b2 = point11.X - point12.X;
            double c2 = (a2 * point11.X) + (b2 * point11.Y);
            double delta = (a1 * b2) - (a2 * b1);
            if (delta != 0)
            {
                double x = ((b2 * c1) - (b1 * c2)) / delta;
                double y = ((a1 * c2) - (a2 * c1)) / delta;
                bool lies = Math.Min(point1.X, point2.X) <= x && x <= Math.Max(point1.X, point2.X);
                lies = lies && Math.Min(point1.Y, point2.Y) <= y && y <= Math.Max(point1.Y, point2.Y);
                lies = lies && Math.Min(point11.X, point12.X) <= x && x <= Math.Max(point11.X, point12.X);
                lies = lies && Math.Min(point11.Y, point12.Y) <= y && y <= Math.Max(point11.Y, point12.Y);
                return lies;
            }

            return false;
        }

        private void SetOuterSmartLabel(AccumulationPoints previousPoint, AccumulationPoints point, double border, double labelRadius, Size textsize, double margin)
        {
            if (!IsCircular())
            {
                SetSmartLabelForSegments(point, previousPoint);
            }
            else
            {
                double labelAngle = GetOverlappedAngle(previousPoint.LabelRegion, point.LabelRegion, point.MidAngle, border * 2);
                GetLabelRegion(point, AccumulationLabelPosition.Outside, textsize, labelRadius, margin, labelAngle);
                if (labelAngle > point.EndAngle)
                {
                    labelAngle = point.MidAngle;
                }

                point.LabelAngle = labelAngle;
                while (point.LabelVisible && (IsOverlap(previousPoint.LabelRegion, point.LabelRegion) || labelAngle <= previousPoint.LabelAngle || IsConnectorLineOverlapping(point, previousPoint)))
                {
                    if (labelAngle > point.EndAngle)
                    {
                        break;
                    }

                    point.LabelAngle = labelAngle;
                    GetLabelRegion(point, AccumulationLabelPosition.Outside, textsize, labelRadius, margin, labelAngle);
                    labelAngle += 0.1;
                }
            }
        }

        private void SetSmartLabelForSegments(AccumulationPoints point, AccumulationPoints prevPoint)
        {
            double overlapHeight = AccumulationChartInstance.AccType == AccumulationType.Funnel ? prevPoint.LabelRegion.Y - (point.LabelRegion.Y + point.LabelRegion.Height) : point.LabelRegion.Y - (prevPoint.LabelRegion.Y + prevPoint.LabelRegion.Height);
            if (overlapHeight < 0)
            {
                point.LabelRegion.Y += AccumulationChartInstance.AccType == AccumulationType.Funnel ? overlapHeight : -overlapHeight;
            }
        }

        private double GetOverlappedAngle(Rect first, Rect second, double angle, double padding)
        {
            double x = first.X;
            if (angle >= 90 && angle <= 270)
            {
                second.Y = first.Y - (padding + (second.Height / 2));
                x = first.X + first.Width;
            }
            else
            {
                second.Y = first.Y + first.Height + padding;
            }

            return GetAngle(Center, new ChartInternalLocation(x, second.Y));
        }

        private static double GetAngle(ChartInternalLocation center, ChartInternalLocation point)
        {
            double angle = Math.Atan2(point.Y - center.Y, point.X - center.X);
            angle = angle < 0 ? (6.283 + angle) : angle;
            return angle * (180 / Math.PI);
        }

        internal override void Dispose()
        {
            base.Dispose();
            series = null;
            areaRect = null;
            helper = null;
            rightSideRenderingPoints = null;
            leftSideRenderingPoints = null;
        }
    }
}