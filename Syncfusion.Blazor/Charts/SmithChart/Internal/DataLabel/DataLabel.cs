using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.SmithChart.Internal
{
    /// <summary>
    /// Specifies the data label rendering of smith chart.
    /// </summary>
    public partial class SmithChartDataLabel
    {
        private SfSmithChart smithchart;
        private SmithChartMargin margin;
        private bool connectorFlag;
        private DataLabelTextOptions prevLabel;
        private List<DataLabelTextOptions> allPoints = new List<DataLabelTextOptions>();
        private CultureInfo culture = CultureInfo.InvariantCulture;

        internal SmithChartDataLabel(SfSmithChart chart)
        {
            smithchart = chart;
            margin = smithchart.Margin;
        }

        internal List<DataLabelTextOptions> TextOptions { get; set; } = new List<DataLabelTextOptions>();

        private static bool IsCollide(DataLabelTextOptions dataLabel1, DataLabelTextOptions dataLabel2)
        {
            if (dataLabel1 != dataLabel2)
            {
                return !(((dataLabel1.Y + dataLabel1.Height) < dataLabel2.Y) ||
                (dataLabel1.Y > (dataLabel2.Y + dataLabel2.Height)) ||
                ((dataLabel1.X + (dataLabel1.Width / 2)) < dataLabel2.X - (dataLabel2.Width / 2)) ||
                (dataLabel1.X - (dataLabel1.Width / 2) > (dataLabel2.X + (dataLabel2.Width / 2))));
            }

            return false;
        }

        internal static string GetFontStyle(SmithChartCommonFont font)
        {
            return "font-size:" + font.Size + "; font-style:" + font.FontStyle + "; font-weight:" + font.FontWeight + "; font-family:" + font.FontFamily + ";opacity:" + font.Opacity + "; color:" + font.Color + ";";
        }

        private static void ResetValues(DataLabelTextOptions currentPoint)
        {
            currentPoint.XPosition = currentPoint.LabelOptions.X;
            currentPoint.YPosition = currentPoint.LabelOptions.Y;
            currentPoint.X = currentPoint.LabelOptions.TextX;
            currentPoint.Y = currentPoint.LabelOptions.TextY;
        }

        internal void GetDataLabelCollection(int seriesindex, List<PointRegion> pointsRegion)
        {
            SmithChartSeries series = smithchart.Series[seriesindex];
            TextOptions = new List<DataLabelTextOptions>();
            allPoints = new List<DataLabelTextOptions>();
            SmithChartSeriesMarker marker = series.Marker;
            double pointIndex, count = pointsRegion.Count, xpos, ypos, width, height;
            LabelOption labelOption = new LabelOption();
            if (marker.DataLabel.Visible)
            {
                for (int i = 0; i < count; i++)
                {
                    string labelText = Convert.ToString(series.ActualPoints[i].Reactance, null);
                    Size textSize = SmithChartHelper.MeasureText(labelText, marker.DataLabel.TextStyle);
                    Point region = pointsRegion[i].Point;
                    xpos = region.X - (textSize.Width / 2);
                    ypos = region.Y - (textSize.Height + marker.Height + margin.Top);
                    width = textSize.Width + (margin.Left / 2) + (margin.Right / 2);
                    height = textSize.Height + (margin.Top / 2) + (margin.Bottom / 2);
                    pointIndex = i;
                    SmithChartLabelPosition labelPosition = new SmithChartLabelPosition
                    {
                        TextX = xpos + (margin.Left / 2),
                        TextY = ypos + (height / 2) + (margin.Top / 2),
                        X = xpos,
                        Y = ypos
                    };
                    TextOptions.Add(new DataLabelTextOptions
                    {
                        Id = smithchart.ID + "_Series" + seriesindex + "_Points" + pointIndex + "_dataLabel" + "_displayText" + i,
                        X = labelPosition.TextX,
                        Y = labelPosition.TextY,
                        Fill = "black",
                        Text = labelText,
                        Font = marker.DataLabel.TextStyle,
                        XPosition = xpos,
                        YPosition = ypos,
                        Width = width,
                        Height = height,
                        Location = region,
                        LabelOptions = labelPosition,
                        Visible = true
                    });
                }
            }

            labelOption.TextOptions = TextOptions;
            series.LabelOption = labelOption;
        }

        private void CreateDataLabelTemplate(RenderTreeBuilder builder, SmithChartSeries series, SmithChartSeriesDatalabel dataLabel, SmithChartPoint point, SmithChartTextRenderEventArgs data, int labelIndex)
        {
            int seq = 0;
            Point region = smithchart.SeriesModule?.PointsRegion.Count > 0 ? smithchart.SeriesModule?.PointsRegion[series.Index][labelIndex].Point : new Point(0, 0);
            Size templateSize = point.TemplateSize.Count > 0 ? point.TemplateSize[0] : new Size(0, 0);
            double height = templateSize.Height, width = templateSize.Width;
            string id = smithchart.ID + "_Series_" + Convert.ToString(series.Index, culture) + "_DataLabelPoint_" + labelIndex + "_Label_" + labelIndex;
            DataLabelTextOptions option = series.LabelOption != null ? series.LabelOption.TextOptions[labelIndex] : new DataLabelTextOptions { X = 0, Y = 0 };
            string leftPos = Convert.ToString(option.XPosition, culture) + "px",
            topPos = Convert.ToString(option.YPosition, culture) + "px",
            visibility = !(point.TemplateSize.Count > 0) ? "hidden" : "visible";
            point.TemplateSize = new List<Size>();
            point.TemplateID.Add(id);
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "id", id);
            builder.AddAttribute(seq++, "style", "position: absolute;background-color:" + data.Color + ";" + GetFontStyle(dataLabel.TextStyle) + "border:" + Convert.ToString(dataLabel.Border.Width, culture) + "px solid " + dataLabel.Border.Color + ";left:" + leftPos + ";top:" + topPos + ";visibility:" + visibility + ";");
            SmithChartPoint templatedata = new SmithChartPoint { Reactance = point.Reactance, Resistance = point.Resistance };
            builder.AddContent(seq++, data.Template(templatedata));
            builder.CloseElement();
        }

        internal void Render(RenderTreeBuilder builder, SmithChartSeries series, SmithChartSeriesDatalabel dataLabel)
        {
            if (series.EnableSmartLabels && dataLabel.Visible && series.LabelOption != null)
            {
                SmartLabelRendering(builder, series.Index);
            }

            int seriesIndex = series.Index;
            string templateId = smithchart.ID + "_Series_" + seriesIndex.ToString(culture) + "_DataLabelCollections", id;
            SmithChartDataLabelTextStyle font = dataLabel.TextStyle;
            DataLabelTextOptions options;
            RectOptions rectOption;
            TextOptions textOption;
            SmithChartSeriesDataLabelBorder border = dataLabel.Border;
            string fill = string.IsNullOrEmpty(dataLabel.Fill) ? smithchart.Series[seriesIndex].Interior : dataLabel.Fill;
            smithchart.Rendering.OpenGroupElement(builder, templateId);
            SmithChartTextRenderEventArgs argsData;
            for (int k = 0; k < series.ActualPoints.Count; k++)
            {
                options = series.LabelOption.TextOptions[k];
                id = smithchart.ID + "_Series" + seriesIndex + "_Points" + k + "_dataLabel" + "_symbol" + k;
                argsData = new SmithChartTextRenderEventArgs()
                {
                    X = options.X,
                    Y = options.Y,
                    SeriesIndex = seriesIndex,
                    PointIndex = k,
                    EventName = "TextRendering",
                    Text = options.Text,
                    Template = dataLabel.Template
                };
                if (dataLabel.Template == null && options != null && options.Visible)
                {
                    SfSmithChart.InvokeEvent<SmithChartTextRenderEventArgs>(smithchart.SmithChartEvents?.TextRendering, argsData);
                    if (!argsData.Cancel)
                    {
                        rectOption = new RectOptions(id, options.XPosition, options.YPosition, options.Width, options.Height, border.Width, border.Color, fill, 0, 0, dataLabel.Opacity);
                        smithchart.Rendering.RenderRect(builder, rectOption);
                        textOption = new TextOptions(argsData.X.ToString(culture), argsData.Y.ToString(culture), !string.IsNullOrEmpty(font.Color) ? font.Color : smithchart.SmithChartThemeStyle.DataLabel, font.GetFontOptions(), argsData.Text, "start", options.Id);
                        smithchart.Rendering.RenderText(builder, textOption);
                    }
                }
                else if (dataLabel.Template != null)
                {
                    CreateDataLabelTemplate(builder, series, dataLabel, series.ActualPoints[k], argsData, k);
                }
            }

            builder.CloseElement();
        }

        private void SmartLabelRendering(RenderTreeBuilder builder, int seriesIndex)
        {
            DataLabelTextOptions currentPoint;
            LabelOption labelOption = smithchart.Series[seriesIndex].LabelOption;
            smithchart.Rendering.OpenGroupElement(builder, smithchart.ID + "_svg" + "_series" + seriesIndex + "_Datalabel" + "_connectorLines");
            CalculateSmartLabels(labelOption, seriesIndex);
            for (int k = 0; k < smithchart.Series[seriesIndex].ActualPoints.Count; k++)
            {
                currentPoint = labelOption.TextOptions[k];
                if (currentPoint.ConnectorFlag)
                {
                    DrawConnectorLines(seriesIndex, k, currentPoint, builder);
                }
            }

            builder.CloseElement();
        }

        private void DrawConnectorLines(int seriesIndex, double index, DataLabelTextOptions currentPoint, RenderTreeBuilder builder)
        {
            Point location = currentPoint.Location;
            double endY;
            if (location.Y > currentPoint.Y)
            {
                endY = currentPoint.Y;
            }
            else
            {
                endY = currentPoint.Y - (currentPoint.Height / 2);
            }

            SmithChartDataLabelConnectorLine connectorLineValues = smithchart.Series[seriesIndex].Marker.DataLabel.ConnectorLine;
            string stroke = !string.IsNullOrEmpty(connectorLineValues.Color) ? connectorLineValues.Color : smithchart.Series[seriesIndex].Interior;
            smithchart.Rendering.RenderPath(builder, smithchart.ID + "_dataLabelConnectorLine" + "_series" + seriesIndex + "_point" + index, "M " + " " + location.X.ToString(culture) + " " + location.Y.ToString(culture) + " " + "L" + " " + currentPoint.X.ToString(culture) + " " + endY.ToString(culture), string.Empty, connectorLineValues.Width, stroke);
        }

        private void CalculateSmartLabels(LabelOption points, int seriesIndex)
        {
            List<DataLabelTextOptions> textOptions = points.TextOptions;
            double length = textOptions.Count, count = 0;
            for (int k = 0; k < length; k++)
            {
                allPoints.Add(textOptions[k]);
                connectorFlag = false;
                CompareDataLabels(k, points, count, seriesIndex);
                textOptions[k] = points.TextOptions[k];
                textOptions[k].ConnectorFlag = connectorFlag;
            }
        }

        private void CompareDataLabels(double i, LabelOption points, double count, double m)
        {
            int length = allPoints.Count, padding = 10;
            DataLabelTextOptions currentLabel, prevLabel;
            for (int j = 0; j < length; j++)
            {
                prevLabel = allPoints[j];
                currentLabel = allPoints[length - 1];
                if (IsCollide(prevLabel, currentLabel))
                {
                    connectorFlag = true;
                    switch (count)
                    {
                        case 0:
                            ResetValues(currentLabel);
                            this.prevLabel = prevLabel;
                            currentLabel.XPosition = this.prevLabel.XPosition + ((this.prevLabel.Width / 2) + (currentLabel.Width / 2) + padding);
                            currentLabel.X = currentLabel.XPosition + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 1:
                            ResetValues(currentLabel);
                            currentLabel.XPosition = this.prevLabel.XPosition + (this.prevLabel.Width / 2) + (currentLabel.Width / 2) + padding;
                            currentLabel.X = currentLabel.XPosition + (padding / 2);
                            currentLabel.YPosition = currentLabel.Location.Y + (currentLabel.Height / 2) + (padding / 2);
                            currentLabel.Y = currentLabel.YPosition + (currentLabel.Height / 2) + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 2:
                            ResetValues(currentLabel);
                            currentLabel.YPosition = currentLabel.Location.Y + (currentLabel.Height / 2) + (padding / 2);
                            currentLabel.Y = currentLabel.YPosition + (currentLabel.Height / 2) + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 3:
                            ResetValues(currentLabel);
                            currentLabel.XPosition = this.prevLabel.XPosition - (this.prevLabel.Width / 2) - (currentLabel.Width / 2) - padding;
                            currentLabel.X = currentLabel.XPosition + (padding / 2);
                            currentLabel.YPosition = (currentLabel.Height / 2) + currentLabel.Location.Y + (padding / 2);
                            currentLabel.Y = currentLabel.YPosition + (currentLabel.Height / 2) + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 4:
                            ResetValues(currentLabel);
                            currentLabel.XPosition = this.prevLabel.XPosition - (this.prevLabel.Width / 2) - (currentLabel.Width / 2) - padding;
                            currentLabel.X = currentLabel.XPosition + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 5:
                            ResetValues(currentLabel);
                            currentLabel.XPosition = this.prevLabel.XPosition - (this.prevLabel.Width / 2) - (currentLabel.Width / 2) - padding;
                            currentLabel.X = currentLabel.XPosition + (padding / 2);
                            currentLabel.YPosition = this.prevLabel.YPosition - currentLabel.Height - padding;
                            currentLabel.Y = currentLabel.YPosition + (currentLabel.Height / 2) + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 6:
                            ResetValues(currentLabel);
                            currentLabel.YPosition = this.prevLabel.YPosition - (currentLabel.Height + padding);
                            currentLabel.Y = currentLabel.YPosition + (currentLabel.Height / 2) + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 7:
                            ResetValues(currentLabel);
                            currentLabel.XPosition = this.prevLabel.XPosition + (this.prevLabel.Width / 2) + (currentLabel.Width / 2) + padding;
                            currentLabel.X = currentLabel.XPosition + (padding / 2);
                            currentLabel.YPosition = this.prevLabel.YPosition - currentLabel.Height - padding;
                            currentLabel.Y = currentLabel.YPosition + (currentLabel.Height / 2) + (padding / 2);
                            count += 1;
                            CompareDataLabels(i, points, count, m);
                            break;
                        case 8:
                            count = 0;
                            CompareDataLabels(i, points, count, m);
                            break;
                    }
                }
            }
        }

        internal void Dispose()
        {
            smithchart = null;
            TextOptions = null;
            allPoints = null;
            margin = null;
            prevLabel = null;
        }
    }
}