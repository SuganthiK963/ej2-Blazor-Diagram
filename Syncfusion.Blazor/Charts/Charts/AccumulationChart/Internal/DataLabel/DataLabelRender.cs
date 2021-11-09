using System;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Syncfusion.Blazor.DataVizCommon;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Globalization;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal partial class DataLabelModule : AccumulationBase
    {
        private const string SPACE = " ";
        private const string ID = "id";
        private const string DIV = "div";

        internal void DrawDataLabels(AccumulationChartSeries series, RenderTreeBuilder builder)
        {
            if (IsCircular() && AccumulationChartInstance.EnableSmartLabels)
            {
                ExtendedLabelsCalculation();
            }

            List<AccumulationPoints> modifiedPoints = new List<AccumulationPoints>();
            modifiedPoints.AddRange(series.RightSidePoints);
            modifiedPoints.AddRange(series.LeftSidePoints);
#pragma warning disable CA1806
            modifiedPoints.OrderBy(dataPoint => dataPoint.Index);
#pragma warning restore CA1806
            string labelId = AccumulationChartInstance.ID + "_datalabel_Series_0_",
            groupId = labelId + "g_",
            rectId = labelId + "Shape_",
            connectorId = labelId + "connector_",
            textId = labelId + "text_";
            foreach (AccumulationPoints point in modifiedPoints)
            {
                if (!double.IsNaN((double)point.Y))
                {
                    FinalizeDatalabels(point, series.DataLabel);
                    if (point.LabelVisible && point.Visible && point.LabelRegion != null)
                    {
                        ChartInternalLocation location = new ChartInternalLocation(point.LabelRegion.X + marginValue, point.LabelRegion.Y + (point.TextSize.Height * 3 / 4) + marginValue);
                        AccumulationChartInstance.Rendering.OpenGroupElement(builder, groupId + point.Index, string.Empty, string.Empty, "cursor:default");

                        if (series.DataLabel.Template == null)
                        {
                            RectOptions rectOptions = new RectOptions(rectId + point.Index, point.LabelRegion.X, point.LabelRegion.Y, point.LabelRegion.Width, point.LabelRegion.Height,
                                series.DataLabel.Border.Width, series.DataLabel.Border.Color, series.DataLabel.Fill, series.DataLabel.Rx, series.DataLabel.Ry);
                            AccumulationChartInstance.Rendering.RenderRect(builder, rectOptions);
                            string rotate = string.Empty;
                            double degree;
                            double angle = series.DataLabel.Angle;
                            if (angle != 0 && series.DataLabel.EnableRotation)
                            {
                                if (point.LabelPosition == AccumulationLabelPosition.Outside)
                                {
                                    degree = 0;
                                }
                                else if (point.MidAngle >= 90 && point.MidAngle <= 270)
                                {
                                    degree = point.MidAngle + 180;
                                }
                                else
                                {
                                    degree = point.MidAngle;
                                }

                                rotate = "rotate(" + degree.ToString(culture) + "," + (location.X + (point.TextSize.Width / 2)).ToString(culture) + "," + (location.Y - (point.TextSize.Height / 4)).ToString(culture) + ")";
                            }
                            else
                            {
                                if (angle != 0)
                                {
                                    degree = (angle > 360) ? angle - 360 : (angle < -360) ? angle + 360 : angle;
                                }
                                else
                                {
                                    degree = 0;
                                }

                                rotate = "rotate(" + degree.ToString(culture) + "," + (location.X + (point.TextSize.Width / 2)).ToString(culture) + "," + location.Y.ToString(culture) + ")";
                            }

                            ChartCommonFont textFont = point.AccTextArgs == null || point.AccTextArgs.Point == null ? series.DataLabel.Font : point.AccTextArgs.Font;
                            TextOptions textOptions = new TextOptions()
                            {
#pragma warning disable CA1305
                                X = location.X.ToString(culture),
                                Y = location.Y.ToString(culture),
#pragma warning restore CA1305
                                Fill = !string.IsNullOrEmpty(textFont.Color) ? textFont.Color : GetSaturatedColor(point, point.AccTextArgs != null ? point.AccTextArgs.Color : point.Color),
                                Text = point.Label,
                                TextAnchor = AccumulationChartInstance.EnableRTL? "end" : "start",
                                Id = textId + point.Index,
                                Transform = rotate,
                                FontFamily = textFont.FontFamily,
                                FontSize = textFont.Size,
                                FontStyle = textFont.FontStyle,
                                FontWeight = textFont.FontWeight
                            };
                            string[] locations = AppendPieTextElements(textOptions.Id, location.X, location.Y);
                            textOptions.X = locations[0];
                            textOptions.Y = locations[1];
                            AccumulationChartInstance.Rendering.RenderText(builder, textOptions);
                        }

                        if (AccumulationChartInstance.AccumulationLegendModule != null && (series.DataLabel.Position == AccumulationLabelPosition.Outside || AccumulationChartInstance.EnableSmartLabels))
                        {
                            AccumulationChartInstance.VisibleSeries[0].FindMaxBounds(AccumulationChartInstance.VisibleSeries[0].LabelBound, point.LabelRegion);
                        }

                        if (point.LabelPosition == AccumulationLabelPosition.Outside)
                        {
                            PathOptions path = new PathOptions(connectorId + point.Index, GetConnectorPath(point.LabelRegion, point, series.DataLabel, point.LabelAngle), series.DataLabel.ConnectorStyle.DashArray, series.DataLabel.ConnectorStyle.Width, point.Color);
                            AccumulationChartInstance.Rendering.RenderPath(builder, path);
                        }

                        builder.CloseElement();
                    }
                }
            }
        }

        private static string TextTrim(double maxWidth, string text, ChartCommonFont font)
        {
            string label = text;
            double size = ChartHelper.MeasureText(text, SfAccumulationChart.GetFontOptions(font)).Width;
            if (size > maxWidth)
            {
                for (int i = text.Length - 1; i >= 0; --i)
                {
                    label = text.Substring(0, i) + "...";
                    size = ChartHelper.MeasureText(label, SfAccumulationChart.GetFontOptions(font)).Width;
                    if (size <= maxWidth)
                    {
                        return label;
                    }
                }
            }

            return label;
        }

        private static ChartInternalLocation GetPerpendicularDistance(ChartInternalLocation startPoint, AccumulationPoints point)
        {
            ChartInternalLocation increasedLocation;
            double height = 20 * Math.Sin(point.MidAngle * Math.PI / 360);
            if (point.MidAngle > 270 && point.MidAngle < 360)
            {
                increasedLocation = new ChartInternalLocation(startPoint.X + (height * Math.Cos((360 - point.MidAngle) * Math.PI / 180)), startPoint.Y - (height * Math.Sin((360 - point.MidAngle) * Math.PI / 180)));
            }
            else if (point.MidAngle > 0 && point.MidAngle < 90)
            {
                increasedLocation = new ChartInternalLocation(startPoint.X + (height * Math.Cos(point.MidAngle * Math.PI / 180)), startPoint.Y + (height * Math.Sin(point.MidAngle * Math.PI / 180)));
            }
            else if (point.MidAngle > 0 && point.MidAngle < 90)
            {
                increasedLocation = new ChartInternalLocation(startPoint.X - (height * Math.Cos((point.MidAngle - 90) * Math.PI / 180)), startPoint.Y + (height * Math.Sin((point.MidAngle - 90) * Math.PI / 180)));
            }
            else
            {
                increasedLocation = new ChartInternalLocation(startPoint.X - (height * Math.Cos((point.MidAngle - 180) * Math.PI / 180)), startPoint.Y - (height * Math.Sin((point.MidAngle - 180) * Math.PI / 180)));
            }

            return increasedLocation;
        }

        private string[] AppendPieTextElements(string id, double locationX, double locationY, string x = "x", string y = "y")
        {
            DynamicAccTextAnimationOptions existElement = AccumulationChartInstance.AnimateTextElements.Find(item => item.Id == id);
            if (existElement != null)
            {
                double preLocationX = existElement.CurLocationX != locationX ? existElement.CurLocationX : locationX,
                preLocationY = existElement.CurLocationY != locationY ? existElement.CurLocationY : locationY;
                existElement.PreLocationX = preLocationX;
                existElement.PreLocationY = preLocationY;
                existElement.CurLocationX = locationX;
                existElement.CurLocationY = locationY;
                locationX = AccumulationChartInstance.LegendClickRedraw ? existElement.PreLocationX : locationX;
                locationY = AccumulationChartInstance.LegendClickRedraw ? existElement.PreLocationY : locationY;
            }
            else
            {
                AccumulationChartInstance.AnimateTextElements.Add(new DynamicAccTextAnimationOptions { CurLocationX = locationX, CurLocationY = locationY, Id = id, X = x, Y = y });
            }

            return new string[] { locationX.ToString(CultureInfo.InvariantCulture), locationY.ToString(CultureInfo.InvariantCulture) };
#pragma warning restore CA1305
        }

        internal void DataLabelMouseMove(ChartInternalMouseEventArgs args, bool isTouch)
        {
            string tooltipId = AccumulationChartInstance.ID + "_Blazor_Datalabel_Tooltip";
            if (args.Target.Contains("text", StringComparison.InvariantCulture) && AccumulationChartInstance.Rendering.TextElementList.Find(item => item.Id == args.Target)?.Text.Contains("...", StringComparison.InvariantCulture) == true)
            {
                string[] targetId = args.Target.Split(AccumulationChartInstance.ID + "_datalabel_Series_");
                if (targetId.Length == 2)
                {
                    int seriesIndex = short.Parse(targetId[1].Split("_text_")[0], null);
                    int pointIndex = short.Parse(targetId[1].Split("_text_")[1], null);
                    if (!double.IsNaN(seriesIndex) && !double.IsNaN(pointIndex))
                    {
                        if (isTouch)
                        {
                            AccumulationChartInstance.TooltipBase.FadeOutTooltip(0);
                        }

                        AccumulationPoints point = AccumulationChartInstance.VisibleSeries[seriesIndex].Points[pointIndex];
#pragma warning disable CA1305
                        AccumulationChartInstance.TooltipBase?.ShowTooltip(!string.IsNullOrEmpty(point.Text) ? point.Text : point.Y.ToString(), args.MouseX, args.MouseY, areaRect.Width, areaRect.Height, AccumulationChartInstance.ID + "_Blazor_Datalabel_Tooltip");
#pragma warning restore CA1305
                    }
                }
            }
            else
            {
                AccumulationChartInstance.TooltipBase?.ChangeContent(tooltipId, true);
            }

            if (isTouch)
            {
                AccumulationChartInstance.TooltipBase.FadeOutTooltip(1000);
            }
        }

        private void FinalizeDatalabels(AccumulationPoints point, AccumulationDataLabelSettings dataLabel)
        {
            if (point.LabelVisible && point.LabelRegion != null)
            {
                Rect rect = AccumulationChartInstance.AccumulationLegendModule.LegendBounds;
                double padding = AccumulationChartInstance.LegendSettings.Border.Width / 2;
                TextTrimming(point, new Rect(rect.X - padding, rect.Y - padding, rect.Width + (2 * padding), rect.Height + (2 * padding)), dataLabel.Font, AccumulationChartInstance.AccumulationLegendModule.Position.ToString());
            }

            if (point.LabelVisible && point.LabelRegion != null)
            {
                TextTrimming(point, areaRect, dataLabel.Font, IsCircular() ? (point.LabelRegion.X >= Center.X) ? "InsideRight" : "InsideLeft" : (point.LabelRegion.X >= point.Region.X) ? "InsideRight" : "InsideLeft");
            }

            if (point.LabelVisible && point.LabelRegion != null && (point.LabelRegion.Y + point.LabelRegion.Height > areaRect.Y + areaRect.Height
                || point.LabelRegion.Y < areaRect.Y || point.LabelRegion.X < areaRect.X || point.LabelRegion.X + point.LabelRegion.Width > areaRect.X + areaRect.Width))
            {
                SetPointVisibileFalse(point);
            }
        }

        private void TextTrimming(AccumulationPoints point, Rect rect, ChartCommonFont font, string position)
        {
            if (IsOverlap(point.LabelRegion, rect))
            {
                double size = point.LabelRegion.Width;
                if (position == "Right")
                {
                    size = rect.X - point.LabelRegion.X;
                }
                else if (position == "Left")
                {
                    size = point.LabelRegion.X - (rect.X + rect.Width);
                    if (size < 0)
                    {
                        size += point.LabelRegion.Width;
                        point.LabelRegion.X = rect.X + rect.Width;
                    }
                }
                else if (position == "InsideRight")
                {
                    size = (rect.X + rect.Width) - point.LabelRegion.X;
                }
                else if (position == "InsideLeft")
                {
                    size = point.LabelRegion.X + point.LabelRegion.Width - rect.X;
                    if (size < point.LabelRegion.Width)
                    {
                        point.LabelRegion.X = rect.X;
                    }
                }
                else
                {
                    SetPointVisibileFalse(point);
                }

                if (point.LabelVisible && point.LabelRegion != null && size < point.LabelRegion.Width)
                {
                    point.Label = TextTrim(size - (marginValue * 2), point.Label, font);
                    point.LabelRegion.Width = size;
                }

                if (point.LabelVisible && point.LabelRegion != null && point.Label.Length == 3 && point.Label.Contains("...", StringComparison.Ordinal))
                {
                    SetPointVisibileFalse(point);
                }
            }
        }

        private string GetConnectorPath(Rect label, AccumulationPoints point, AccumulationDataLabelSettings dataLabel, double end = 0)
        {
            double labelRadius = IsCircular() ? !IsVariableRadius() ? LabelRadius : AccumulationChartInstance.PieSeriesModule.GetLabelRadius(AccumulationChartInstance.VisibleSeries.First(), point) : GetLabelDistance(point, dataLabel);
            ChartInternalLocation start = GetConnectorStartPoint(point, dataLabel.ConnectorStyle);
            double labelAngle = AccumulationChartInstance.EnableSmartLabels ? point.MidAngle : end != 0 ? end : point.MidAngle;
            ChartInternalLocation middle = new ChartInternalLocation(0, 0);
            ChartInternalLocation endPoint = GetEdgeOfLabel(label, labelAngle, middle, point, dataLabel.ConnectorStyle.Width);
            if (dataLabel.ConnectorStyle.Type == ConnectorType.Curve)
            {
                if (IsCircular())
                {
                    double radius = labelRadius - (IsVariableRadius() ? DataVizCommonHelper.StringToNumber(point.SliceRadius, AccumulationChartInstance.PieSeriesModule.SeriesRadius) : Radius);
                    if (point.IsLabelUpdated == 1)
                    {
                        middle = GetPerpendicularDistance(start, point);
                    }
                    else
                    {
                        middle = ChartHelper.DegreeToLocation(labelAngle, labelRadius - (radius / 2), Center);
                        if (point.LabelPosition == AccumulationLabelPosition.Outside && dataLabel.Position == AccumulationLabelPosition.Inside)
                        {
                            middle = ChartHelper.DegreeToLocation(labelAngle, labelRadius - (radius * 1.25), Center);
                        }
                    }

                    return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " Q " + middle.X.ToString(culture) + SPACE + middle.Y.ToString(culture) + SPACE + endPoint.X.ToString(culture) + SPACE + endPoint.Y.ToString(culture);
                }
                else
                {
                    return GetPolyLinePath(start, endPoint);
                }
            }
            else
            {
                return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " L " + middle.X.ToString(culture) + SPACE + middle.Y.ToString(culture) + " L " + endPoint.X.ToString(culture) + SPACE + endPoint.Y.ToString(culture);
            }
        }

        private ChartInternalLocation GetConnectorStartPoint(AccumulationPoints point, AccumulationChartConnector connector)
        {
            return IsCircular() ? ChartHelper.DegreeToLocation(point.MidAngle, (IsVariableRadius() ? DataVizCommonHelper.StringToNumber(point.SliceRadius, AccumulationChartInstance.PieSeriesModule.SeriesRadius) : AccumulationChartInstance.PieSeriesModule.Radius) - connector.Width,
                Center) : GetLabelLocation(point, !IsCircular() && point.Region.X > point.LabelRegion.X ? "OutsideLeft" : string.Empty);
        }

        private string GetPolyLinePath(ChartInternalLocation start, ChartInternalLocation end)
        {
            if (start.Y == end.Y)
            {
                return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " L " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture);
            }

            string path = "M";
            for (var i = 0; i <= 16; i++)
            {
                ChartInternalLocation points = GetBezierPoint(i / 16, new ChartInternalLocation[] { start, end }, 0, 2);
                path += points.X.ToString(culture) + "," + points.Y.ToString(culture);
                if (i != 16)
                {
                    path += " L";
                }
            }

            return path;
        }

        private ChartInternalLocation GetBezierPoint(double temp, ChartInternalLocation[] controlPoints, int index, int count)
        {
            if (count == 1)
            {
                return controlPoints[index];
            }

            ChartInternalLocation p0 = GetBezierPoint(temp, controlPoints, index, count - 1);
            ChartInternalLocation p1 = GetBezierPoint(temp, controlPoints, index + 1, count - 1);
            return new ChartInternalLocation(((1 - temp) * p0.X) + (temp * p1.X), ((1 - temp) * p0.Y) + (temp * p1.Y));
        }

        private string GetSaturatedColor(AccumulationPoints point, string color)
        {
            string saturatedColor;
            if (marginValue >= 1)
            {
                saturatedColor = color == "transparent" ? GetLabelBackground(point) : color;
            }
            else
            {
                saturatedColor = GetLabelBackground(point);
            }

            saturatedColor = saturatedColor == "transparent" ? AccumulationChartInstance.Background : saturatedColor;
            Color rgbValue = ChartHelper.GetRBGValue(saturatedColor);
            return Math.Round(Convert.ToDouble(((rgbValue.R * 299) + (rgbValue.G * 587) + (rgbValue.B * 114)) / 1000, AccumulationChartInstance.NumberFormatter)) >= 128 ? "black" : "white";
        }

        private string GetLabelBackground(AccumulationPoints point)
        {
            return point.LabelPosition == AccumulationLabelPosition.Outside ? string.IsNullOrEmpty(AccumulationChartInstance.Background) ? AccumulationChartInstance.AccChartThemeStyle.Background : AccumulationChartInstance.Background : point.AccPointArgs != null ? point.AccPointArgs.Fill : point.Color;
        }

        internal void RenderDataLabelTemplates(RenderTreeBuilder builder)
        {
            AccumulationChartSeries dataSeries = AccumulationChartInstance.VisibleSeries.First();
            string divId = AccumulationChartInstance.ID + "_Series_0_DataLabel_";
            if (dataSeries.DataLabel.Template != null)
            {
                foreach (AccumulationPoints point in dataSeries.Points)
                {
                    builder.OpenElement(SvgRendering.Seq++, DIV);
                    if (point.LabelRegion != null)
                    {
                        point.TemplateID = divId + point.Index;
                        builder.AddAttribute(SvgRendering.Seq++, "style", "left: " + point.LabelRegion.X.ToString(culture) + "px; top: " + point.LabelRegion.Y.ToString(culture) + "px; position: absolute;");
                        builder.AddAttribute(SvgRendering.Seq++, ID, point.TemplateID);
                        builder.AddContent(SvgRendering.Seq++, dataSeries.DataLabel.Template(new AccumulationChartDataPointInfo(point.X, point.Y, point.Percentage, point.Label)));
                    }

                    builder.CloseElement();
                }
            }
        }
    }
}