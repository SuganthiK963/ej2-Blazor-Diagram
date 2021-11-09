using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.BulletChart.Internal
{
    /// <summary>
    /// Specifies scale rendering of the bullet chart.
    /// </summary>
    public class BulletChartScaleRender
    {
        private IBulletChart bulletChart;
        private BulletChartRender render;
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulletChartScaleRender"/> class.
        /// </summary>
        /// <param name="chart">Represents the <seealso cref="IBulletChart"/> class.</param>
        /// <param name="render">Represents the <seealso cref="BulletChartRender"/> class.</param>
        public BulletChartScaleRender(IBulletChart chart, BulletChartRender render)
        {
            bulletChart = chart;
            this.render = render;
        }

        internal List<BarInfo> FeatureBars { get; set; }

        internal List<BarInfo> RectMeasureBars { get; set; }

        internal List<BarInfo> CrossMeasureBars { get; set; }

        internal List<BarInfo> CircleMeasureBars { get; set; }

        internal List<LabelModel> CategoryLabel { get; set; }

        internal List<ColorRange> RangeInfo { get; set; }

        internal void RenderFeatureBar(List<ExpandoObject> processedData)
        {
            double initialBoundsStart = bulletChart.Orientation == OrientationType.Horizontal ? render.InitialClipRect.Y + render.InitialClipRect.Height : render.InitialClipRect.X,
            featureBarSize = (bulletChart.Orientation == OrientationType.Horizontal ? render.InitialClipRect.Height : render.InitialClipRect.Width) / processedData.Count, point;
            FeatureBars = new List<BarInfo>();
            CategoryLabel = new List<LabelModel>();
            for (int i = 0; i < processedData.Count; i++)
            {
                IDictionary<string, object> data = processedData[i];
                data.TryGetValue(bulletChart.CategoryField, out object categoryValue);
                data.TryGetValue(bulletChart.ValueField, out object valueField);
                string strokeColor = string.IsNullOrEmpty(render.CategoryStyle.Color) ? bulletChart.Style.CategoryFontColor : render.CategoryStyle.Color;
                string categoryText = (categoryValue ?? string.Empty).ToString();
                categoryText = !bulletChart.EnableGroupSeparator ? categoryText.Replace(",", string.Empty, StringComparison.InvariantCulture) : categoryText;
                double fieldValue = double.Parse((valueField ?? string.Empty).ToString(), null);
                point = bulletChart.Orientation == OrientationType.Horizontal ? initialBoundsStart - (featureBarSize * i) - ((featureBarSize + bulletChart.ValueHeight) / 2) :
                    initialBoundsStart + (featureBarSize * i) + (featureBarSize / 2) - (bulletChart.ValueHeight / 2);
                MeasureModel bounds = CalculateFeatureMeasureBounds(fieldValue, categoryText);
                if (data != null && bounds != null && bulletChart.Type == FeatureType.Dot)
                {
                    bounds.PointX = bulletChart.Orientation == OrientationType.Horizontal ? bounds.PointX + ((fieldValue > 0 && !bulletChart.IsRtlEnabled()) ||
                            (fieldValue < 0 && bulletChart.IsRtlEnabled()) ? bounds.Width : 0) - (6 / 2) : bounds.PointX + ((fieldValue > 0 && bulletChart.IsRtlEnabled()) ||
                            (fieldValue < 0 && !bulletChart.IsRtlEnabled()) ? bounds.Width : 0) - (6 / 2);
                    bounds.Width = 6;
                }

                if (bounds != null)
                {
                    if (bulletChart.Orientation == OrientationType.Horizontal)
                    {
                        AddFeatureBarInfo(bounds.PointX, point, bounds.Width, i);
                    }
                    else
                    {
                        AddFeatureBarInfo(point, bounds.PointX, bounds.Width, i);
                    }

                    if (!string.IsNullOrEmpty(categoryText))
                    {
                        SizeInfo categoryTextSize = bulletChart.Helper.MeasureTextSize(categoryText, render.CategoryStyle);
                        double x, categorySize = bulletChart.Orientation == OrientationType.Horizontal ? categoryTextSize.Width : categoryTextSize.Height;
                        Rect initialRect = render.InitialClipRect;
                        if (bulletChart.Orientation == OrientationType.Horizontal)
                        {
                            x = bulletChart.IsRtlEnabled() ? initialRect.X + initialRect.Width + 5 + (categorySize / 2) : initialRect.X - 5 - (categorySize / 2);
                            CategoryLabel.Add(new LabelModel() { X = x, Y = point + bulletChart.ValueHeight, Text = categoryText, Color = strokeColor });
                        }
                        else
                        {
                            x = bulletChart.IsRtlEnabled() ? initialRect.Y - 5 - (categorySize / 2) : initialRect.Y + initialRect.Height + 5 + (categorySize / 2);
                            CategoryLabel.Add(new LabelModel() { X = point + (bulletChart.ValueHeight / 2), Y = x, Text = categoryText, Color = strokeColor });
                        }
                    }
                }
            }
        }

        private void AddFeatureBarInfo(double pointX, double pointY, double width, int index)
        {
            BarInfo feature = new BarInfo()
            {
                ID = bulletChart.ID + "_FeatureMeasure_" + index,
                Fill = bulletChart.ValueFill ?? string.Empty,
                X = pointX,
                Y = pointY,
                Width = bulletChart.Orientation == OrientationType.Vertical ? bulletChart.ValueHeight : width,
                Height = bulletChart.Orientation == OrientationType.Vertical ? width : bulletChart.ValueHeight,
                Border = new Border() { Stroke = bulletChart.ValueBorder != null ? bulletChart.ValueBorder.Color : "transparent", StrokeWidth = bulletChart.ValueBorder != null ? bulletChart.ValueBorder.Width : 1 },
                Style = bulletChart.Animation != null ? "visibility : hidden" : string.Empty,
            };
            FeatureBars.Add(feature);
        }

        private MeasureModel CalculateFeatureMeasureBounds(double fieldValue, string categoryValue)
        {
            double minValue = render.MinValue;
            fieldValue = fieldValue < minValue && minValue < 0 ? minValue : fieldValue;
            if (fieldValue >= minValue)
            {
                double pointX, lastPointX, width,
                loc = bulletChart.Orientation == OrientationType.Horizontal ? render.InitialClipRect.X : render.InitialClipRect.Y,
                scaleLength = bulletChart.Orientation == OrientationType.Horizontal ? render.InitialClipRect.Width : render.InitialClipRect.Height,
                delta = render.MaxValue - minValue, valueDiff = render.MaxValue - fieldValue;
                string orientation = (!bulletChart.IsRtlEnabled() ? "forward" : "backward") + bulletChart.Orientation.ToString().ToLower(culture);
                double stringLength = bulletChart.Helper.MeasureTextSize(categoryValue, render.CategoryStyle).Width;
                switch (orientation)
                {
                    case "forwardhorizontal":
                    case "backwardvertical":
                        pointX = loc + (minValue > 0 ? 0 : scaleLength / delta * Math.Abs(minValue));
                        width = scaleLength / (delta / ((minValue > 0) ? delta - valueDiff : fieldValue));
                        if (fieldValue < 0)
                        {
                            width = Math.Abs(width);
                            pointX -= width;
                        }

                        width = (pointX + width < loc + scaleLength) ? width : loc + scaleLength - pointX;
                        lastPointX = loc - (orientation.Equals("forwardhorizontal", StringComparison.Ordinal) ? ((stringLength / 2) + 5) : 15);
                        break;
                    default:
                        pointX = loc + (scaleLength - (scaleLength / (delta / (delta - valueDiff))));
                        width = minValue > 0 ? scaleLength / (delta / (delta - valueDiff)) : scaleLength / (delta / fieldValue);
                        if (fieldValue < 0)
                        {
                            width = Math.Abs(width);
                            pointX -= width;
                        }

                        if (pointX < loc)
                        {
                            width = pointX + width - loc;
                            pointX = loc;
                        }

                        lastPointX = loc + scaleLength + (orientation.Equals("backwardhorizontal", StringComparison.Ordinal) ? ((stringLength / 2) + 5) : 5);
                        break;
                }

                return new MeasureModel() { PointX = pointX, Width = width, LastPointX = lastPointX };
            }

            return null;
        }

        internal void DrawScaleGroup()
        {
            Rect rect = bulletChart.Render.InitialClipRect;
            bool isHorizontal = bulletChart.Orientation == OrientationType.Horizontal;
            double range = isHorizontal ? rect.Width : rect.Height,
            fillRange = isHorizontal ? rect.Height : rect.Width,
            locX = rect.X + ((bulletChart.IsRtlEnabled() && isHorizontal) ? rect.Width : 0),
            locY = rect.Y + ((!bulletChart.IsRtlEnabled() && bulletChart.Orientation == OrientationType.Vertical) ? rect.Height : 0),
            start = 0;
            RangeInfo = new List<ColorRange>();
            render.RangeCollection = new List<double>();
            for (int i = 0; i < bulletChart?.Ranges?.Count; i++)
            {
                double area = range * ((bulletChart.Ranges[i].End - start) / bulletChart.Maximum);
                if (isHorizontal)
                {
                    locX -= bulletChart.IsRtlEnabled() ? area : 0;
                }
                else
                {
                    locY -= !bulletChart.IsRtlEnabled() ? area : 0;
                }

                RangeInfo.Add(new ColorRange()
                {
                    ID = bulletChart.ID + "_range_" + i,
                    Stroke = !string.IsNullOrEmpty(bulletChart.Ranges[i].Color) ? bulletChart.Ranges[i].Color : bulletChart.Style.RangeStrokes[i],
                    StrokeWidth = 1,
                    Opacity = bulletChart.Ranges[i].Opacity,
                    RangeRect = new Rect(bulletChart.Orientation == OrientationType.Horizontal ? fillRange : area, bulletChart.Orientation == OrientationType.Horizontal ? area : fillRange, locX, locY)
                });
                if (isHorizontal)
                {
                    locX += bulletChart.IsRtlEnabled() ? 0 : area;
                }
                else
                {
                    locY += !bulletChart.IsRtlEnabled() ? 0 : area;
                }

                render.RangeCollection.Add(area);
                start = bulletChart.Ranges[i].End;
            }
        }

        internal void RenderComparativeSymbol(List<ExpandoObject> processedData)
        {
            List<double> targetCollection = new List<double>();
            Rect rect = render.InitialClipRect;
            bool isHorizontal = bulletChart.Orientation == OrientationType.Horizontal;
            double target, pointY = isHorizontal ? rect.Y + rect.Height : rect.X, temp, x1, delta = bulletChart.Maximum - bulletChart.Minimum,
            featureBarSize = (isHorizontal ? rect.Height : rect.Width) / processedData.Count,
            pointX = isHorizontal ? rect.X - (bulletChart.TargetWidth / 2) : rect.Y + rect.Height,
            scaleLength = isHorizontal ? rect.Width : rect.Height;
            RectMeasureBars = new List<BarInfo>();
            CrossMeasureBars = new List<BarInfo>();
            CircleMeasureBars = new List<BarInfo>();
            for (int i = 0; i < processedData.Count; i++)
            {
                IDictionary<string, object> data = processedData[i];
                data.TryGetValue(bulletChart.TargetField, out object targetValue);
                if (targetValue is IEnumerable)
                {
                    IEnumerator iterator = (targetValue as IEnumerable).GetEnumerator();
                    while (iterator.MoveNext())
                    {
                        target = double.Parse((iterator.Current ?? string.Empty).ToString(), null);
                        targetCollection.Add(target);
                    }
                }
                else
                {
                    target = double.Parse((targetValue ?? string.Empty).ToString(), null);
                    targetCollection.Add(target);
                }

                for (int j = 0; j < targetCollection.Count; j++)
                {
                    if (targetCollection[j] >= bulletChart.Minimum && targetCollection[j] <= bulletChart.Maximum)
                    {
                        temp = isHorizontal ? pointY - (featureBarSize * i) - (featureBarSize / 2) : pointY + (featureBarSize * i) + (featureBarSize / 2);
                        double y1 = temp - (bulletChart.TargetWidth * 1.5),
                        y2 = temp + (bulletChart.TargetWidth * 1.5);
                        temp = scaleLength / (delta / (delta - (bulletChart.Maximum - targetCollection[j])));
                        x1 = isHorizontal ? pointX + (bulletChart.IsRtlEnabled() ? scaleLength - temp : temp) : pointX - (bulletChart.IsRtlEnabled() ? scaleLength - temp : temp);
                        GetTargetElement(bulletChart.TargetTypes[j % bulletChart.TargetTypes.Count], isHorizontal, x1, y1, y2, targetCollection[j], string.Format(null, "{0}", j) + "_" + string.Format(null, "{0}", i));
                    }
                }

                targetCollection.Clear();
            }
        }

        private void GetTargetElement(TargetType targetType, bool isHorizontal, double x1, double y1, double y2, double target, string index)
        {
            double strokeWidth = targetType == TargetType.Cross ? bulletChart.TargetWidth - 1 : 1,
            size = targetType == TargetType.Circle ? bulletChart.TargetWidth - 1 : bulletChart.TargetWidth,
            lx = isHorizontal ? x1 + (size / 2) : y1 + ((y2 - y1) / 2),
            ly = isHorizontal ? y1 + ((y2 - y1) / 2) : x1;
            string id = bulletChart.ID + "_ComparativeMeasure_" + index;
            if (targetType == TargetType.Rect)
            {
                if (isHorizontal)
                {
                    AddCompareMeasureInfo(target == bulletChart.Maximum ? x1 - (bulletChart.TargetWidth / 2) : target == bulletChart.Minimum ? x1 + (bulletChart.TargetWidth / 2) : x1, y1, y2, id);
                }
                else
                {
                    AddCompareMeasureInfo(y1, y2, x1, id);
                }
            }
            else if (targetType == TargetType.Circle)
            {
                CircleMeasureBars.Add(new BarInfo()
                {
                    ID = id,
                    X1 = lx,
                    Y1 = ly,
                    StrokeWidth = size,
                    Stroke = !string.IsNullOrEmpty(bulletChart.TargetColor) ? bulletChart.TargetColor : "black",
                    Style = bulletChart.Animation != null ? "visibility : hidden" : string.Empty,
                });
            }
            else
            {
                string crossDirection = "M " + (lx - size).ToString(culture) + " " + (ly - size).ToString(culture) + " L " + (lx + size).ToString(culture) + " " + (ly + size).ToString(culture) + " M " + (lx - size).ToString(culture) + " " + (ly + size).ToString(culture) + " L " + (lx + size).ToString(culture) + " " + (ly - size).ToString(culture);
                CrossMeasureBars.Add(new BarInfo()
                {
                    ID = id,
                    X1 = lx,
                    Y1 = ly,
                    StrokeWidth = strokeWidth,
                    Stroke = !string.IsNullOrEmpty(bulletChart.TargetColor) ? bulletChart.TargetColor : string.Empty,
                    Path = crossDirection,
                    Style = bulletChart.Animation != null ? "visibility : hidden" : string.Empty,
                });
            }
        }

        private void AddCompareMeasureInfo(double x1, double y1, double y2, string id)
        {
            RectMeasureBars.Add(new BarInfo()
            {
                ID = id,
                X1 = x1,
                Y1 = bulletChart.Orientation == OrientationType.Horizontal ? y1 : y2,
                X2 = bulletChart.Orientation == OrientationType.Horizontal ? x1 : y1,
                Y2 = y2,
                StrokeWidth = bulletChart.TargetWidth,
                Stroke = !string.IsNullOrEmpty(bulletChart.TargetColor) ? bulletChart.TargetColor : "black",
                Style = bulletChart.Animation != null ? "visibility : hidden" : string.Empty,
            });
        }

        internal void SetValueBorder()
        {
            for (int i = 0; i < FeatureBars.Count; i++)
            {
                FeatureBars[i].Border = new Border() { Stroke = bulletChart.ValueBorder != null ? bulletChart.ValueBorder.Color : "transparent", StrokeWidth = bulletChart.ValueBorder != null ? bulletChart.ValueBorder.Width : 1 };
            }
        }

        internal void Dispose()
        {
            bulletChart = null;
            render = null;
            FeatureBars = null;
            RectMeasureBars = null;
            CrossMeasureBars = null;
            CircleMeasureBars = null;
            CategoryLabel = null;
            RangeInfo = null;
        }
    }
}