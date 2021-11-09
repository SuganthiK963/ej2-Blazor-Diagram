using Microsoft.AspNetCore.Components.Rendering;
using System;
using Syncfusion.Blazor.Charts.Chart.Models;
using System.Runtime.InteropServices;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Drawing;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Point = Syncfusion.Blazor.Charts.Chart.Internal.Point;
using Size = Syncfusion.Blazor.Charts.Chart.Internal.Size;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class DatalabelTemplateOptions
    {
        public string Id { get; set; }

        public string style { get; set; }

        public RenderFragment Template { get; set; }
    }

    public class ChartDataLabelRenderer : ChartRenderer, IChartElementRenderer
    {
        [Parameter]
        public ChartSeries Series { get; set; }

        internal ChartSeriesRenderer SeriesRenderer { get; set; }

        private bool inverted { get; set; }

        private bool yAxisInversed { get; set; }

        private string fontBackground { get; set; }

        private bool isShape { get; set; }

        private double borderWidth { get; set; }

        private double markerHeight { get; set; }

        private string commonId { get; set; }

        private string chartBackground { get; set; }

        private double errorHeight { get; set; }

        private double locationX { get; set; }

        private double locationY { get; set; }

        private ChartEventMargin margin { get; set; } = new ChartEventMargin();

        private CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        private List<RectOptions> rectOptions = new List<RectOptions>();
        private List<TextOptions> textOptions = new List<TextOptions>();
        internal List<DatalabelTemplateOptions> templateOptions = new List<DatalabelTemplateOptions>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Series.Marker.DataLabel.Renderer = this;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = Series.Marker.DataLabel.Visible;
            SeriesRenderer = Series.Renderer;
            if (RendererShouldRender && SeriesRenderer.Series.Visible)
            {
                CalculateRenderTreeBuilderOptions(Series, Series.Marker.DataLabel);
            }
        }

        private string SeriesIndex()
        {
            return double.IsNaN(SeriesRenderer.Index) ? Convert.ToString(SeriesRenderer.Category(), null) : Convert.ToString(SeriesRenderer.Index, null);
        }

        internal string SeriesTextId()
        {
            return Owner.ID + "TextGroup" + SeriesIndex();
        }

        internal string SeriesShapeId()
        {
            return Owner.ID + "ShapeGroup" + SeriesIndex();
        }

        private void InitPrivateVariables(ChartSeries series, ChartMarker marker)
        {
            markerHeight = series.Type == ChartSeriesType.Scatter || marker.Visible ? (marker.Height / 2) : 0;
            commonId = Owner.ID + "_Series_" + SeriesIndex() + "_Point_";
            CalculateErrorHeight(series, series.Marker.DataLabel.Position);
            chartBackground = Owner.ChartAreaRenderer.Area.Background == Constants.TRANSPARENT ? Owner.Background ?? Owner.ChartThemeStyle.Background : Owner.ChartAreaRenderer.Area.Background;
            textOptions.Clear();
            rectOptions.Clear();
            templateOptions.Clear();
        }

        private void CalculateErrorHeight(ChartSeries series, LabelPosition position)
        {
            if (!series.ErrorBar.Visible)
            {
                return;
            }
            else if (series.ErrorBar.Visible && Owner.ChartAreaType != ChartAreaType.PolarAxes)
            {
                ErrorBarDirection direction = series.ErrorBar.Direction;
                double positiveHeight = Series.ErrorBar.Renderer.PositiveHeight;
                double negativeHeight = Series.ErrorBar.Renderer.NegativeHeight;
                if (IsRectSeries())
                {
                    if (position == LabelPosition.Top || position == LabelPosition.Auto)
                    {
                        if (direction == ErrorBarDirection.Both || direction == ErrorBarDirection.Minus)
                        {
                            errorHeight = negativeHeight;
                        }
                        else
                        {
                            errorHeight = 0;
                        }
                    }

                    if (position == LabelPosition.Outer || position == LabelPosition.Auto)
                    {
                        if (direction == ErrorBarDirection.Both || direction == ErrorBarDirection.Plus)
                        {
                            errorHeight = positiveHeight;
                        }
                        else
                        {
                            errorHeight = 0;
                        }
                    }
                }
                else
                {
                    if (position == LabelPosition.Top || position == LabelPosition.Outer || position == LabelPosition.Auto)
                    {
                        if ((direction == ErrorBarDirection.Both || direction == ErrorBarDirection.Plus) && (!series.Container.IsTransposed))
                        {
                            errorHeight = positiveHeight;
                        }
                        else
                        {
                            errorHeight = 0;
                        }
                    }

                    if (position == LabelPosition.Bottom || position == LabelPosition.Auto)
                    {
                        if (direction == ErrorBarDirection.Both || direction == ErrorBarDirection.Minus)
                        {
                            errorHeight = negativeHeight;
                        }
                        else
                        {
                            errorHeight = 0;
                        }
                    }
                }
            }
            else
            {
                errorHeight = 0;
            }
        }

        internal void CalculateRenderTreeBuilderOptions(ChartSeries series, ChartDataLabel dataLabel)
        {
            InitPrivateVariables(series, series.Marker);
            LabelLocation labelLocation = new LabelLocation(0, 0);
            inverted = Owner.RequireInvertedAxis;
            yAxisInversed = series.Renderer.YAxisRenderer.Axis.IsInversed;
            string templateId = Owner.ID + "_Series_" + (SeriesRenderer.Index != 0 ? SeriesRenderer.Category().ToString() : SeriesRenderer.Index.ToString(CultureInfo.InvariantCulture)) + "_DataLabelCollections";
            List<Point> visiblePoints = ChartHelper.GetVisiblePoints(SeriesRenderer.Points);
            SeriesRenderer.DynamicOptions.DataLabelParentId = SeriesTextId();
            SeriesRenderer.SeriesTemplateID = new List<string>();
            for (int i = 0; i < visiblePoints.Count; i++)
            {
                Point point = visiblePoints[i];
                margin = new ChartEventMargin { Left = dataLabel.Margin.Left, Right = dataLabel.Margin.Right, Bottom = dataLabel.Margin.Bottom, Top = dataLabel.Margin.Top };
                double x_Pos, y_Pos, x_Value, y_Value;
                bool isRender = true;
                Rect clip = SeriesRenderer.ClipRect;
                double angle = dataLabel.Angle, degree = dataLabel.Angle;
                string rotation;
                if ((point.SymbolLocations.Count != 0 && point.SymbolLocations[0] != null) || (series.Type == ChartSeriesType.BoxAndWhisker && point.Regions.Count != 0))
                {
                    List<string> labelText = ChartHelper.GetLabelText(point, SeriesRenderer);
                    point.TemplateID = new List<string>();
                    for (int j = 0; j < labelText.Count; j++)
                    {
                        TextRenderEventArgs argsData = new TextRenderEventArgs(Constants.TEXTRENDER, false, series, point, labelLocation, labelText[j], dataLabel.Fill, new BorderModel() { Width = dataLabel.Border.Width, Color = dataLabel.Border.Color }, dataLabel.Template, dataLabel.Font.GetChartDefaultFont(), SeriesRenderer.Index);
                        if (Owner.ChartEvents?.OnDataLabelRender != null)
                        {
                            Owner.ChartEvents?.OnDataLabelRender.Invoke(argsData);
                        }

                        if (!argsData.Cancel && point.Visible)
                        {
                            fontBackground = argsData.Color;
                            IsDataLabelShape(argsData);
                            markerHeight = (series.Type == ChartSeriesType.Bubble) ? (point.Regions[0].Height / 2) : markerHeight;
                            if (argsData.Template != null)
                            {
                                CreateDataLabelTemplate(series, dataLabel, point, argsData, j);
                            }
                            else
                            {
                                Size textSize = ChartHelper.MeasureText(argsData.Text, GetFontOptions(dataLabel.Font));
                                Rect rect = CalculateTextPosition(point, series, textSize, dataLabel, j);
                                if (Owner.ChartAreaType == ChartAreaType.PolarAxes)
                                {
                                    List<Rect> visibleAxisLabelRect = (Owner.AxisContainer.AxisLayout as PolarRadarAxisLayout).VisibleAxisLabelRect;
                                    foreach (Rect rectRegion in visibleAxisLabelRect)
                                    {
                                        if (ChartHelper.IsOverlap(new Rect(rect.X, rect.Y, rect.Width, rect.Height), rectRegion))
                                        {
                                            isRender = false;
                                            break;
                                        }
                                    }
                                }

                                Rect actualRect = new Rect(rect.X + clip.X, rect.Y + clip.Y, rect.Width, rect.Height);
                                bool isNotOverlapping;
                                List<List<ChartInternalLocation>> rectPoints = new List<List<ChartInternalLocation>>();
                                if (dataLabel.EnableRotation && angle != 0)
                                {
                                    List<ChartInternalLocation> rectCoordinates = GetRectanglePoints(actualRect);
                                    rectPoints.Add(ChartHelper.GetRotatedRectangleCoordinates(rectCoordinates, actualRect.X + (actualRect.Width * 0.5), actualRect.Y - (actualRect.Height / 2), angle));
                                    isNotOverlapping = true;
                                    for (int index = j; index > 0; index--)
                                    {
                                        if (rectPoints[j] != null && rectPoints[index - 1] != null && ChartHelper.IsRotatedRectIntersect(rectPoints[j], rectPoints[index - 1]))
                                        {
                                            isNotOverlapping = false;
                                            rectPoints[j] = null;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    isNotOverlapping = !ChartHelper.IsCollide(rect, Owner.SeriesContainer.DataLabelCollection, clip, Owner.ChartAreaType == ChartAreaType.CartesianAxes);
                                }

                                if (isNotOverlapping && isRender)
                                {
                                    Owner.SeriesContainer.DataLabelCollection.Add(actualRect);
                                    Color rgbValue = ChartHelper.GetRBGValue(fontBackground);
                                    double contrast = Math.Round(Convert.ToDouble(((rgbValue.R * 299) + (rgbValue.G * 587) + (rgbValue.B * 114)) / 1000, culture), 1);
                                    x_Pos = (rect.X + margin.Left + (textSize.Width / 2)) + labelLocation.X;
                                    y_Pos = (rect.Y + margin.Top + (textSize.Height * 3 / 4)) + labelLocation.Y;
                                    labelLocation.X = 0;
                                    labelLocation.Y = 0;
                                    if (angle != 0 && dataLabel.EnableRotation)
                                    {
                                        x_Value = x_Pos - (dataLabel.Margin.Left / 2) + (dataLabel.Margin.Right / 2);
                                        y_Value = y_Pos - (dataLabel.Margin.Top / 2) - (textSize.Height / dataLabel.Margin.Top) + (dataLabel.Margin.Bottom / 2);
                                        degree = (angle > 360) ? angle - 360 : (angle < -360) ? angle + 360 : angle;
                                    }
                                    else
                                    {
                                        degree = 0;
                                        x_Value = rect.X;
                                        y_Value = rect.Y;
                                    }

                                    rotation = "rotate(" + degree.ToString(culture) + "," + x_Value.ToString(culture) + "," + y_Value.ToString(culture) + ")";
                                    if (isShape)
                                    {
                                        RectOptions rectOption = new RectOptions(commonId + point.Index + "_TextShape_" + i, rect.X, rect.Y, rect.Width, rect.Height, argsData.Border.Width, argsData.Border.Color, argsData.Color, dataLabel.Rx, dataLabel.Ry);
                                        rectOption.Transform = rotation;
                                        rectOptions.Add(rectOption);
                                    }

                                    string color = string.IsNullOrEmpty(argsData.Font.Color) ? ((contrast >= 128 || series.Type == ChartSeriesType.Hilo) ? "black" : "white") : argsData.Font.Color;
                                    TextOptions option = new TextOptions(x_Pos.ToString(culture), y_Pos.ToString(culture), color, argsData.Font.GetFontOptions(), argsData.Text, "middle", commonId + point.Index + "_Text_" + i, rotation, degree.ToString(culture), "auto");
                                    string[] locations = ChartHelper.AppendTextElements(Owner, option.Id, Convert.ToDouble(option.X, culture), Convert.ToDouble(option.Y, culture));
                                    option.X = locations[0];
                                    option.Y = locations[1];
                                    textOptions.Add(option);
                                    SeriesRenderer.DynamicOptions.DataLabelTextId.Add(option.Id);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static ChartFontOptions GetFontOptions(ChartDataLabelFont font)
        {
            return new ChartFontOptions { Color = font.Color, Size = font.Size, FontFamily = font.FontFamily, FontWeight = font.FontWeight, FontStyle = font.FontStyle, TextAlignment = font.TextAlignment, TextOverflow = font.TextOverflow };
        }

        private Rect CalculateTextPosition(Point point, ChartSeries series, Size textSize, ChartDataLabel dataLabel, int labelIndex)
        {
            Rect labelRegion = (point.Regions.Count > 0 ? true : false) ? labelIndex > 1 ? (series.Type == ChartSeriesType.Candle) ? point.Regions[1] : point.Regions[0] : point.Regions[0] : null;
            if (labelIndex > 1 && series.Type == ChartSeriesType.HiloOpenClose)
            {
                labelRegion = (labelIndex == 2) ? point.Regions[1] : point.Regions[2];
            }

            ChartInternalLocation location = GetLabelLocation(point, series, labelIndex);
            Rect clipRect = SeriesRenderer.ClipRect;
            double alignmentValue;
            if (!Owner.RequireInvertedAxis || !IsRectSeries() || series.Type == ChartSeriesType.BoxAndWhisker)
            {
                locationX = location.X;
                alignmentValue = textSize.Height + (borderWidth * 2) + markerHeight + margin.Bottom + margin.Top + 5;
                location.Y = dataLabel.Position == LabelPosition.Auto ? location.Y : CalculateAlignment(alignmentValue, location.Y, dataLabel.Alignment, IsRectSeries() ? point.YValue < 0 : false);
                location.Y = (!IsRectSeries() || series.Type == ChartSeriesType.BoxAndWhisker) ? CalculatePathPosition(location.Y, labelRegion, point.YValue < 0 != yAxisInversed, dataLabel.Position, series, point, textSize, labelIndex) :
                    CalculateRectPosition(location.Y, labelRegion, point.YValue < 0 != yAxisInversed, dataLabel.Position, series, textSize, labelIndex, point);
                if (IsRectSeries() && Owner.ChartAreaType == ChartAreaType.PolarAxes)
                {
                    location = CalculatePolarRectPosition(location, dataLabel.Position, series, point, textSize, labelIndex, dataLabel.Alignment, alignmentValue);
                }
            }
            else
            {
                locationY = location.Y;
                alignmentValue = textSize.Width + borderWidth + margin.Left + margin.Right - 5;
                location.X = dataLabel.Position == LabelPosition.Auto ? location.X : CalculateAlignment(alignmentValue, location.X, dataLabel.Alignment, point.YValue < 0);
                location.X = CalculateRectPosition(location.X, labelRegion, point.YValue < 0 != yAxisInversed, dataLabel.Position, series, textSize, labelIndex, point);
            }

            Rect rect = ChartHelper.CalculateRect(location, textSize, margin);
            if (!(dataLabel.EnableRotation && dataLabel.Angle != 0) && !((rect.Y > (clipRect.Y + clipRect.Height)) || (rect.X > (clipRect.X + clipRect.Width)) || (rect.X + rect.Width < 0) || (rect.Y + rect.Height < 0)))
            {
                rect.X = rect.X < 0 ? 5 : rect.X;
                rect.Y = rect.Y < 0 ? 5 : rect.Y;
                rect.X -= (rect.X + rect.Width) > (clipRect.X + clipRect.Width) ? (rect.X + rect.Width) - (clipRect.X + clipRect.Width) + 5 : 0;
                rect.Y -= (rect.Y + rect.Height) > (clipRect.Y + clipRect.Height) ? (rect.Y + rect.Height) - (clipRect.Y + clipRect.Height) + 5 : 0;
                fontBackground = fontBackground == Constants.TRANSPARENT ? chartBackground : fontBackground;
            }

            return rect;
        }

        private ChartInternalLocation GetLabelLocation(Point point, ChartSeries series, int labelIndex)
        {
            ChartInternalLocation location = new ChartInternalLocation(0, 0);
            Rect labelRegion = (point.Regions.Count > 0 ? true : false) ? (series.Type == ChartSeriesType.Candle && labelIndex > 1) ? point.Regions[1] : point.Regions[0] : null;
            if (series.Type == ChartSeriesType.HiloOpenClose)
            {
                labelRegion = (labelIndex == 2) ? point.Regions[1] : point.Regions[2];
            }

            ChartAxisRenderer x_Axis = SeriesRenderer.XAxisRenderer;
            ChartAxisRenderer y_Axis = SeriesRenderer.YAxisRenderer;
            bool isInverted = Owner.RequireInvertedAxis;
            if (series.Type == ChartSeriesType.BoxAndWhisker)
            {
                markerHeight = 0;
                BoxPoint boxPoint = point as BoxPoint;
                double x_Value = x_Axis.GetPointValue(point.XValue);
                switch (labelIndex)
                {
                    case 0: location = ChartHelper.GetPoint(x_Value, y_Axis.GetPointValue(boxPoint.Median), x_Axis, y_Axis, isInverted); break;
                    case 1: location = ChartHelper.GetPoint(x_Value, y_Axis.GetPointValue(boxPoint.Maximum), x_Axis, y_Axis, isInverted); break;
                    case 2: location = ChartHelper.GetPoint(x_Value, y_Axis.GetPointValue(boxPoint.Minimum), x_Axis, y_Axis, isInverted); break;
                    case 3: location = ChartHelper.GetPoint(x_Value, y_Axis.GetPointValue(boxPoint.UpperQuartile), x_Axis, y_Axis, isInverted); break;
                    case 4: location = ChartHelper.GetPoint(x_Value, y_Axis.GetPointValue(boxPoint.LowerQuartile), x_Axis, y_Axis, isInverted); break;
                    default:
                        {
                            location = ChartHelper.GetPoint(point.XValue, boxPoint.Outliers[labelIndex - 5], x_Axis, y_Axis, isInverted);
                            markerHeight = series.Marker.Height / 2;
                            break;
                        }
                }

                if (isInverted)
                {
                    location.Y = point.Regions[0].Y + (point.Regions[0].Height / 2);
                }
                else
                {
                    location.X = point.Regions[0].X + (point.Regions[0].Width / 2);
                }
            }
            else if (labelIndex == 0 || labelIndex == 1)
            {
                location = new ChartInternalLocation(point.SymbolLocations[0].X, point.SymbolLocations[0].Y);
            }
            else if ((labelIndex == 2 || labelIndex == 3) && series.Type == ChartSeriesType.Candle)
            {
                location = new ChartInternalLocation(point.SymbolLocations[1].X, point.SymbolLocations[1].Y);
            }
            else if (isInverted)
            {
                location.X = labelRegion.X + (labelRegion.Width / 2);
                location.Y = labelRegion.Y;
            }
            else
            {
                location.X = labelRegion.X + labelRegion.Width;
                location.Y = labelRegion.Y + (labelRegion.Height / 2);
            }

            if (labelIndex > 1 && series.Type == ChartSeriesType.HiloOpenClose)
            {
                if (Owner.RequireInvertedAxis)
                {
                    location.Y = labelRegion.Y + (labelRegion.Height / 2) + (2 * (labelIndex == 2 ? 1 : -1));
                }
                else
                {
                    location.X = labelRegion.X + (labelRegion.Width / 2) + (2 * (labelIndex == 2 ? 1 : -1));
                }
            }

            return location;
        }

        private double CalculateAlignment(double align, double labelLocation, Alignment alignment, bool isMinus)
        {
            switch (alignment)
            {
                case Alignment.Far:
                    labelLocation = !inverted ? (isMinus ? labelLocation + align : labelLocation - align) : (isMinus ? labelLocation - align : labelLocation + align); break;
                case Alignment.Near:
                    labelLocation = !inverted ? (isMinus ? labelLocation - align : labelLocation + align) : (isMinus ? labelLocation + align : labelLocation - align); break;
                case Alignment.Center:
                    return labelLocation;
            }

            return labelLocation;
        }

        private double CalculatePathPosition(double labelLocation, Rect rect, bool isminus, LabelPosition position, ChartSeries series, Point point, Size size, int labelIndex)
        {
            bool isAreaSeries = series.Type.ToString().Contains("Area", StringComparison.InvariantCulture);
            if ((isAreaSeries && series.Type != ChartSeriesType.RangeArea) && yAxisInversed && series.Marker.DataLabel.Position != LabelPosition.Auto)
            {
                position = position == LabelPosition.Top ? LabelPosition.Bottom : position == LabelPosition.Bottom ? LabelPosition.Top : position;
            }

            switch (position)
            {
                case LabelPosition.Top:
                case LabelPosition.Outer:
                    labelLocation = labelLocation - markerHeight - borderWidth - (size.Height / 2) - margin.Bottom - 5 - errorHeight;
                    break;
                case LabelPosition.Bottom:
                    labelLocation = labelLocation + markerHeight + borderWidth + (size.Height / 2) + margin.Top + 5 + errorHeight;
                    break;
                case LabelPosition.Auto:
                    labelLocation = CalculatePathActualPosition(
                        labelLocation, rect, isminus, series, point, size, labelIndex);
                    break;
            }

            bool withInRegion = !inverted ? isminus ? (labelLocation < rect.Y) : (labelLocation > rect.Y) : isminus ? (labelLocation > rect.X) : (labelLocation < rect.X);
            fontBackground = (withInRegion && isAreaSeries) ?
                fontBackground == Constants.TRANSPARENT ? (!string.IsNullOrEmpty(point.Interior) ? point.Interior : SeriesRenderer.Interior) : fontBackground :
                (fontBackground == Constants.TRANSPARENT ? chartBackground : fontBackground);
            return labelLocation;
        }

        private double CalculatePathActualPosition(double y, Rect rect, bool isminus, ChartSeries series, Point point, Size size, int labelIndex)
        {
            int index = point.Index;
            double y_Value = SeriesRenderer.Points[index].YValue;
            LabelPosition position;
            Point nextPoint = SeriesRenderer.Points.Count - 1 > index ? SeriesRenderer.Points[index + 1] : null;
            Point previousPoint = index > 0 ? SeriesRenderer.Points[index - 1] : null;
            double y_Location = 0;
            bool isOverLap = true;
            List<Rect> collection = Owner.SeriesContainer.DataLabelCollection;
            if (series.Type == ChartSeriesType.Bubble)
            {
                position = LabelPosition.Top;
            }
            else if (series.Type.ToString().Contains("Step", StringComparison.InvariantCulture))
            {
                position = LabelPosition.Top;
                if (index != 0)
                {
                    position = (previousPoint == null || !previousPoint.Visible || (y_Value > previousPoint.YValue != yAxisInversed) || y_Value == previousPoint.YValue) ? LabelPosition.Top : LabelPosition.Bottom;
                }
            }
            else if (series.Type == ChartSeriesType.BoxAndWhisker)
            {
                if (labelIndex == 1 || labelIndex == 3 || labelIndex > 4)
                {
                    position = SeriesRenderer.YAxisRenderer.Axis.IsInversed ? LabelPosition.Bottom : LabelPosition.Top;
                }
                else if (labelIndex == 2 || labelIndex == 4)
                {
                    position = SeriesRenderer.YAxisRenderer.Axis.IsInversed ? LabelPosition.Top : LabelPosition.Bottom;
                }
                else
                {
                    isOverLap = false;
                    position = LabelPosition.Middle;
                    y_Location = CalculatePathPosition(y, rect, isminus, position, series, point, size, labelIndex);
                }
            }
            else
            {
                if (index == 0)
                {
                    position = (nextPoint == null || !nextPoint.Visible || y_Value > nextPoint.YValue || (y_Value < nextPoint.YValue && yAxisInversed)) ? LabelPosition.Top : LabelPosition.Bottom;
                }
                else if (index == SeriesRenderer.Points.Count - 1)
                {
                    position = (previousPoint == null || !previousPoint.Visible || y_Value > previousPoint.YValue || (y_Value < previousPoint.YValue && yAxisInversed)) ? LabelPosition.Top : LabelPosition.Bottom;
                }
                else
                {
                    if (!nextPoint.Visible && !(previousPoint != null && previousPoint.Visible))
                    {
                        position = LabelPosition.Top;
                    }
                    else if (!nextPoint.Visible || previousPoint == null)
                    {
                        position = nextPoint.YValue > y_Value || (previousPoint != null && previousPoint.YValue > y_Value) ? LabelPosition.Bottom : LabelPosition.Top;
                    }
                    else
                    {
                        double slope = (nextPoint.YValue - previousPoint.YValue) / 2;
                        double intersectY = (slope * index) + (nextPoint.YValue - (slope * (index + 1)));
                        position = !yAxisInversed ? intersectY < y_Value ? LabelPosition.Top : LabelPosition.Bottom : intersectY < y_Value ? LabelPosition.Bottom : LabelPosition.Top;
                    }
                }
            }

            bool isBottom = position == LabelPosition.Bottom;
            LabelPosition[] Test = { LabelPosition.Outer, LabelPosition.Top, LabelPosition.Bottom, LabelPosition.Middle, LabelPosition.Auto };
            int positionIndex = Array.IndexOf(Test, position);
            while (isOverLap && positionIndex < 4)
            {
                y_Location = CalculatePathPosition(y, rect, isminus, GetPosition(positionIndex), series, point, size, labelIndex);
                Rect labelRect = ChartHelper.CalculateRect(new ChartInternalLocation(locationX, y_Location), size, margin);
                isOverLap = labelRect.Y < 0 || ChartHelper.IsCollide(labelRect, collection, SeriesRenderer.ClipRect, Owner.ChartAreaType == ChartAreaType.CartesianAxes) || (labelRect.Y + labelRect.Height) > SeriesRenderer.ClipRect.Height;
                positionIndex = isBottom ? positionIndex - 1 : positionIndex + 1;
                isBottom = false;
            }

            return y_Location;
        }

        private double CalculateRectPosition(double labelLocation, Rect rect, bool isMinus, LabelPosition position, ChartSeries series, Size textSize, int labelIndex, Point point)
        {
            if (Owner.ChartAreaType == ChartAreaType.PolarAxes)
            {
                return 0;
            }

            double extraSpace = borderWidth + ((!inverted ? textSize.Height : textSize.Width) / 2) + 5;
            if (series.Type.ToString().Contains("Stacking", StringComparison.InvariantCulture))
            {
                position = position == LabelPosition.Outer ? LabelPosition.Top : position;
            }
            else if (series.Type.ToString().Contains("Range", StringComparison.InvariantCulture))
            {
                position = (position == LabelPosition.Outer || position == LabelPosition.Top) ? position : LabelPosition.Auto;
            }
            else if (series.Type == ChartSeriesType.Waterfall)
            {
                position = position == LabelPosition.Auto ? LabelPosition.Middle : position;
            }

            switch (position)
            {
                case LabelPosition.Bottom:
                    labelLocation = !inverted ? isMinus ? (labelLocation - rect.Height + extraSpace + margin.Top) : (labelLocation + rect.Height - extraSpace - margin.Bottom) : isMinus ? (labelLocation + rect.Width - extraSpace - margin.Left) : (labelLocation - rect.Width + extraSpace + margin.Right);
                    break;
                case LabelPosition.Middle:
                    labelLocation = !inverted ? isMinus ? labelLocation - (rect.Height / 2) : labelLocation + (rect.Height / 2) : isMinus ? labelLocation + (rect.Width / 2) : labelLocation - (rect.Width / 2);
                    break;
                case LabelPosition.Auto:
                    labelLocation = CalculateRectActualPosition(labelLocation, rect, isMinus, series, textSize, labelIndex, point);
                    break;
                default:
                    extraSpace += errorHeight;
                    labelLocation = CalculateTopAndOuterPosition(labelLocation, rect, position, series, labelIndex, extraSpace, isMinus);
                    break;
            }

            fontBackground = (!inverted ? (labelLocation < rect.Y || labelLocation > rect.Y + rect.Height) : (labelLocation < rect.X || labelLocation > rect.X + rect.Width)) ? (fontBackground == Constants.TRANSPARENT ? chartBackground : fontBackground) :
                fontBackground == Constants.TRANSPARENT ? (!string.IsNullOrEmpty(point.Interior) ? point.Interior : SeriesRenderer.Interior) : fontBackground;
            return labelLocation;
        }

        private double CalculateRectActualPosition(double labelLocation, Rect rect, bool isMinus, ChartSeries series, Size size, int labelIndex, Point point)
        {
            double location = 0;
            Rect labelRect;
            bool isOverLap = true;
            int position = 0;
            List<Rect> collection = Owner.SeriesContainer.DataLabelCollection;
            while (isOverLap && position < (series.Type.ToString().Contains("Range", StringComparison.InvariantCulture) || series.Type == ChartSeriesType.Hilo ? 2 : 4))
            {
                location = CalculateRectPosition(labelLocation, rect, isMinus, GetPosition(position), series, size, labelIndex, point);
                if (!inverted)
                {
                    labelRect = ChartHelper.CalculateRect(new ChartInternalLocation(locationX, location), size, margin);
                    isOverLap = labelRect.Y < 0 || ChartHelper.IsCollide(labelRect, collection, SeriesRenderer.ClipRect, Owner.ChartAreaType == ChartAreaType.CartesianAxes) || labelRect.Y + labelRect.Height > SeriesRenderer.ClipRect.Height;
                }
                else
                {
                    labelRect = ChartHelper.CalculateRect(new ChartInternalLocation(location, locationY), size, margin);
                    isOverLap = labelRect.X < 0 || ChartHelper.IsCollide(labelRect, collection, SeriesRenderer.ClipRect, Owner.ChartAreaType == ChartAreaType.CartesianAxes) || labelRect.X + labelRect.Width > SeriesRenderer.ClipRect.Width;
                }

                position++;
            }

            return location;
        }

        private double CalculateTopAndOuterPosition(double location, Rect rect, LabelPosition position, ChartSeries series, int index, double extraSpace, bool isMinus)
        {
            bool top;
            switch (series.Type)
            {
                case ChartSeriesType.RangeColumn:
                case ChartSeriesType.RangeArea:
                case ChartSeriesType.Hilo:
                    top = (index == 0 && !yAxisInversed) || (index == 1 && yAxisInversed);
                    location = UpdateLabelLocation(position, location, extraSpace, margin, rect, top);
                    break;
                case ChartSeriesType.Candle:
                    top = ((index == 0 || index == 2) && !yAxisInversed) || ((index == 1 || index == 3) && yAxisInversed);
                    location = UpdateLabelLocation(position, location, extraSpace, margin, rect, top, index > 1);
                    break;
                case ChartSeriesType.HiloOpenClose:
                    if (index <= 1)
                    {
                        top = (index == 0 && !yAxisInversed) || (index == 1 && yAxisInversed);
                        location = UpdateLabelLocation(position, location, extraSpace, margin, rect, top);
                    }
                    else
                    {
                        if (yAxisInversed)
                        {
                            location = !inverted ? location + extraSpace + margin.Top : location - extraSpace - margin.Right;
                        }
                        else
                        {
                            location = !inverted ? location - extraSpace - margin.Bottom : location + extraSpace + margin.Left;
                        }
                    }

                    break;
                default:
                    if ((isMinus && position == LabelPosition.Top) || (!isMinus && position == LabelPosition.Outer))
                    {
                        location = !inverted ? location - extraSpace - margin.Bottom - markerHeight : location + extraSpace + margin.Left + markerHeight;
                    }
                    else
                    {
                        location = !inverted ? location + extraSpace + margin.Top + markerHeight : location - extraSpace - margin.Right - markerHeight;
                    }

                    break;
            }

            return location;
        }

        private double UpdateLabelLocation(LabelPosition position, double location, double extraSpace, ChartEventMargin margin, Rect rect, bool top, bool inside = false)
        {
            if (!inverted)
            {
                if (top)
                {
                    location = (position == LabelPosition.Outer && !inside) ? location - extraSpace - margin.Bottom - markerHeight : location + extraSpace + margin.Top + markerHeight;
                }
                else
                {
                    location = (position == LabelPosition.Outer && !inside) ? location + rect.Height + extraSpace + margin.Top + markerHeight : location + rect.Height - extraSpace - margin.Bottom - markerHeight;
                }
            }
            else
            {
                if (top)
                {
                    location = (position == LabelPosition.Outer && !inside) ? location + extraSpace + margin.Left + markerHeight : location - extraSpace - margin.Right - markerHeight;
                }
                else
                {
                    location = (position == LabelPosition.Outer && !inside) ? location - rect.Width - extraSpace - margin.Right - markerHeight : location - rect.Width + extraSpace + margin.Left + markerHeight;
                }
            }

            return location;
        }

        private ChartInternalLocation CalculatePolarRectPosition(ChartInternalLocation location, LabelPosition position, ChartSeries series, Point point, Size size, int labelIndex, Alignment alignment, double alignmentValue)
        {
            double columnRadius;
            double angle = (point.RegionData.StartAngle - (0.5 * Math.PI)) + ((point.RegionData.EndAngle - point.RegionData.StartAngle) / 2);
            if (labelIndex == 0)
            {
                columnRadius = point.RegionData.Radius < point.RegionData.InnerRadius ? point.RegionData.InnerRadius : point.RegionData.Radius;
            }
            else
            {
                columnRadius = point.RegionData.Radius > point.RegionData.InnerRadius ? point.RegionData.InnerRadius : point.RegionData.Radius;
            }

            fontBackground = fontBackground == "transparent" ? chartBackground : fontBackground;
            if (series.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
            {
                position = position == LabelPosition.Outer ? LabelPosition.Top : position;
            }
            else if (series.DrawType.ToString().Contains("Range", StringComparison.InvariantCulture))
            {
                position = position == LabelPosition.Outer || position == LabelPosition.Top ? position : LabelPosition.Auto;
            }

            if (position == LabelPosition.Outer)
            {
                columnRadius = labelIndex == 0 ? columnRadius + 10 + markerHeight : columnRadius - 10 - markerHeight;
            }
            else if (position == LabelPosition.Middle)
            {
                columnRadius = (columnRadius / 2) + 5;
                if (series.DrawType == ChartDrawType.StackingColumn)
                {
                    columnRadius = point.RegionData.InnerRadius + ((point.RegionData.Radius - point.RegionData.InnerRadius) / 2) + 5 - (size.Height / 2);
                }
            }
            else if (position == LabelPosition.Top)
            {
                columnRadius = labelIndex == 0 ? columnRadius - 10 - markerHeight : columnRadius + 10 + markerHeight;
            }
            else if (position == LabelPosition.Bottom)
            {
                columnRadius = 10;
                columnRadius += (series.DrawType == ChartDrawType.StackingColumn) ? (point.RegionData.InnerRadius + markerHeight) : 0;
            }
            else
            {
                if (labelIndex == 0)
                {
                    columnRadius = columnRadius >= Series.Renderer.Owner.AxisContainer.AxisLayout.Radius ? columnRadius - 5 : series.DrawType == ChartDrawType.StackingColumn ? columnRadius - 10 : columnRadius + 10;
                }
                else
                {
                    columnRadius = columnRadius >= Series.Renderer.Owner.AxisContainer.AxisLayout.Radius ? columnRadius + 5 : columnRadius - 10;
                }
            }

            columnRadius += alignmentValue * (alignment == Alignment.Center ? 0 : alignment == Alignment.Far ? 1 : -1);
            location.X = (SeriesRenderer.ClipRect.Width / 2) + SeriesRenderer.ClipRect.X + (columnRadius * Math.Cos(angle));
            if (series.DrawType == ChartDrawType.StackingColumn)
            {
                location.X = location.X < Owner.AvailableSize.Width / 2 ? location.X + (size.Width / 2) : (location.X > Owner.AvailableSize.Width / 2 ? location.X - (size.Width / 2) : location.X);
            }
            else if (series.DrawType == ChartDrawType.Column)
            {
                location.X = location.X < Owner.AvailableSize.Width / 2 ? location.X - (size.Width / 2) : (location.X > Owner.AvailableSize.Width / 2 ? location.X + (size.Width / 2) : location.X);
            }

            location.Y = (SeriesRenderer.ClipRect.Height / 2) + SeriesRenderer.ClipRect.Y + (columnRadius * Math.Sin(angle));

            return location;
        }

        private static LabelPosition GetPosition(int index)
        {
            LabelPosition[] pos = { LabelPosition.Outer, LabelPosition.Top, LabelPosition.Bottom, LabelPosition.Middle, LabelPosition.Auto };
            return pos[index];
        }

        private static List<ChartInternalLocation> GetRectanglePoints(Rect rect)
        {
            return new List<ChartInternalLocation>() { new ChartInternalLocation(rect.X, rect.Y), new ChartInternalLocation(rect.X + rect.Width, rect.Y), new ChartInternalLocation(rect.X + rect.Width, rect.Y + rect.Height), new ChartInternalLocation(rect.X, rect.Y + rect.Height) };
        }

        private bool IsRectSeries()
        {
            return SeriesRenderer.IsRectSeries();
        }

        private void IsDataLabelShape(TextRenderEventArgs style)
        {
            isShape = style.Color != Constants.TRANSPARENT || style.Border.Width > 0;
            borderWidth = !double.IsNaN(style.Border.Width) ? style.Border.Width : 0;
            if (!isShape)
            {
                margin = new ChartEventMargin() { Left = 0, Right = 0, Bottom = 0, Top = 0 };
            }
        }

        private void CreateDataLabelTemplate(ChartSeries series, ChartDataLabel dataLabel, Point point, TextRenderEventArgs data, int labelIndex)
        {
            ChartAxisRenderer v_Axis = Owner.RequireInvertedAxis ? SeriesRenderer.XAxisRenderer : SeriesRenderer.YAxisRenderer;
            ChartAxisRenderer h_Axis = Owner.RequireInvertedAxis ? SeriesRenderer.YAxisRenderer : SeriesRenderer.XAxisRenderer;
            margin = new ChartEventMargin() { Left = 0, Right = 0, Bottom = 0, Top = 0 };
            Rect clip = SeriesRenderer.ClipRect;
            Size templateSize = point.TemplateSize.Count > 0 ? point.TemplateSize[labelIndex] : new Size(0, 0);
            Rect rect = CalculateTextPosition(point, series, templateSize, dataLabel, labelIndex);
            double posX = ((Owner.ChartAreaType == ChartAreaType.PolarAxes) ? 0 : SeriesRenderer.ClipRect.X) + rect.X,
            posY = ((Owner.ChartAreaType == ChartAreaType.PolarAxes) ? 0 : SeriesRenderer.ClipRect.Y) + rect.Y;
            string left = Convert.ToString(posX, culture) + "px";
            string top = Convert.ToString(posY, culture) + "px";
            Color rgbValue = Color.FromName(fontBackground);
            string color = string.IsNullOrEmpty(dataLabel.Font.Color) ? dataLabel.Font.Color : (Math.Round(Convert.ToDouble(((rgbValue.R * 299) + (rgbValue.G * 587) + (rgbValue.B * 114)) / 1000, culture), 1) >= 128 ? "black" : "white");
            bool isAnimation = series.Animation.Enable && Owner.ShouldAnimateSeries;
            string visibility = !(point.TemplateSize.Count > 0) ? "hidden" : isAnimation ? "hidden" : "visible";
            string id;
            if ((!ChartHelper.IsCollide(rect, Owner.SeriesContainer.DataLabelCollection, clip, Owner.ChartAreaType == ChartAreaType.CartesianAxes) || dataLabel.LabelIntersectAction == "None") &&
                (SeriesRenderer.SeriesType() != SeriesValueType.XY || point.YValue.Equals(double.NaN) ||
                ChartHelper.WithIn(point.YValue, SeriesRenderer.YAxisRenderer.VisibleRange) || (series.Type.ToString().Contains("100", StringComparison.InvariantCulture) &&
                ChartHelper.WithIn(SeriesRenderer.StackedValues.EndValues[point.Index], SeriesRenderer.YAxisRenderer.VisibleRange))) &&
                ChartHelper.WithIn(point.XValue, SeriesRenderer.XAxisRenderer.VisibleRange) && posY >= v_Axis.Rect.Y &&
                posX >= h_Axis.Rect.X && posY <= v_Axis.Rect.Y + v_Axis.Rect.Height && posX <= h_Axis.Rect.X + h_Axis.Rect.Width)
            {
                id = Owner.ID + "_Series_" + (string.IsNullOrEmpty(Convert.ToString(SeriesRenderer.Index, null)) ? Convert.ToString(SeriesRenderer.Category(), null) : Convert.ToString(SeriesRenderer.Index, null)) + "_DataLabelPoint_" + point.Index + "_Label_" + labelIndex;
                point.TemplateID.Add(id);
                Owner.SeriesContainer.DataLabelCollection.Add(new Rect(rect.X + clip.X, rect.Y + clip.Y, rect.Width, rect.Height));
                ChartDataPointInfo templatedata = new ChartDataPointInfo()
                {
                    X = point.X,
                    Y = point.Y,

                    // Close = point.Close,
                    // High = point.High,
                    // Low = point.Low,
                    // Open = point.Open,
                    PointX = point.X,
                    PointY = point.Y,
                    PointIndex = point.Index,
                    PointText = point.Text,
                    Text = point.Text,
                    SeriesIndex = SeriesRenderer.Index,
                    SeriesName = series.Name,

                    // Volume = point.Volume
                };
                templateOptions.Add(new DatalabelTemplateOptions()
                {
                    Id = id,
                    style = "position: absolute;background-color:" + data.Color + ";" + ChartHelper.GetFontStyle(dataLabel.Font) + "border:" + data.Border.Width.ToString(culture) + "px solid " + data.Border.Color + ";left:" + left + ";top:" + top + ";visibility:" + visibility + ";",
                    Template = data.Template(templatedata)
                });
                if (isAnimation)
                {
                    SeriesRenderer.SeriesTemplateID.Add(id);
                }
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            if (SeriesRenderer != null && SeriesRenderer.Series.Visible)
            {
                Owner.SvgRenderer.OpenGroupElement(builder, Owner.ID + "_DataLabelCollection");
                RenderDatalabels(builder, Series.Marker);
                builder.CloseElement();
            }
        }

        private void RenderDatalabels(RenderTreeBuilder builder, ChartMarker marker)
        {
            if (!(marker.DataLabel != null && marker.DataLabel.Visible && marker.DataLabel.Template == null))
            {
                return;
            }

            string transform = string.Empty, clipPath = string.Empty;
            if (Owner.ChartAreaType == ChartAreaType.CartesianAxes)
            {
                transform = "translate(" + SeriesRenderer.ClipRect.X.ToString(culture) + "," + SeriesRenderer.ClipRect.Y.ToString(culture) + ")";
                clipPath = "url(#" + Owner.ID + "_ChartSeriesClipRect_" + SeriesIndex() + ')';
            }

            Owner.SvgRenderer.OpenGroupElement(builder, SeriesShapeId(), transform, clipPath);

            foreach (RectOptions rectOption in rectOptions)
            {
                Owner.SvgRenderer.RenderRect(builder, rectOption);
            }

            builder.CloseElement();

            Owner.SvgRenderer.OpenGroupElement(builder, SeriesTextId(), transform, clipPath);

            foreach (TextOptions textOption in textOptions)
            {
                Owner.SvgRenderer.RenderText(builder, textOption);
            }

            builder.CloseElement();
        }

        internal void Dispose()
        {
            margin = null;
        }

        public void HandleLayoutChange()
        {
            OnLayoutChange();
        }

        protected virtual void OnLayoutChange()
        {
            // ProcessRenderQueue();
        }

        internal void ToggleVisibility()
        {
            if (Series.Marker.DataLabel.Visible)
            {
                DatalabelValueChanged();
            }
            else
            {
                RendererShouldRender = true;
                ClearRenderingOptions();
                InvalidateRender();
            }
        }

        internal void DatalabelValueChanged()
        {
            RendererShouldRender = Series.Marker.DataLabel.Visible;
            if (RendererShouldRender)
            {
                ClearRenderingOptions();
                CalculateRenderTreeBuilderOptions(Series, Series.Marker.DataLabel);
            }

            InvalidateRender();
        }

        private void ClearRenderingOptions()
        {
            templateOptions.Clear();
            textOptions.Clear();
            rectOptions.Clear();
        }

        internal void UpdateDatalabelTemplatePosition()
        {
            if (Series.Marker.DataLabel.Template == null || !Series.Marker.DataLabel.Visible)
            {
                return;
            }

            ClearRenderingOptions();
            CalculateRenderTreeBuilderOptions(Series, Series.Marker.DataLabel);
            Owner.DatalabelTemplateContainer?.InvalidateRender();
        }

        public void InvalidateRender()
        {
            InvokeAsync(StateHasChanged);
        }

        public override void ProcessRenderQueue()
        {
            InvokeAsync(StateHasChanged);
            Owner.DatalabelTemplateContainer?.InvalidateRender();
        }
    }
}