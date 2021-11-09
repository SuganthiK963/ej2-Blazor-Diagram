using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class BoxAndWhiskerSeriesRenderer : ColumnBaseRenderer
    {
        protected List<PathOptions> BoxPathOptions { get; set; }

        private static void GetQuartileValues(double[] y_Values, int count, BoxPlotQuartile quartile)
        {
            if (count == 1)
            {
                quartile.LowerQuartile = y_Values[0];
                quartile.UpperQuartile = y_Values[0];
                return;
            }

            int halfLength = count / 2;
            quartile.LowerQuartile = ChartHelper.GetMedian(y_Values.Take(halfLength).ToArray());
            quartile.UpperQuartile = ChartHelper.GetMedian(y_Values.Skip(count % 2 == 0 ? halfLength : halfLength + 1).Take(count).ToArray());
        }

        private static double GetExclusiveQuartileValue(double[] y_Values, int count, double percentile)
        {
            if (count == 0)
            {
                return 0;
            }
            else if (count == 1)
            {
                return y_Values[0];
            }

            double region, rank = percentile * (count + 1), integerRank = Math.Floor(Math.Abs(rank)),
            fractionRank = rank - integerRank;
            int index = Convert.ToInt32(integerRank);
            if (integerRank == 0)
            {
                region = y_Values[0];
            }
            else if (integerRank > count - 1)
            {
                region = y_Values[count - 1];
            }
            else
            {
                region = (fractionRank * (y_Values[index] - y_Values[index - 1])) + y_Values[index - 1];
            }

            return region;
        }

        private static void GetMinMaxOutlier(double[] y_Values, int count, BoxPlotQuartile quartile)
        {
            double rangeIQR = 1.5 * (quartile.UpperQuartile - quartile.LowerQuartile);
            for (int i = 0; i < count; i++)
            {
                if (y_Values[i] < quartile.LowerQuartile - rangeIQR)
                {
                    quartile.Outliers.Add(y_Values[i]);
                }
                else
                {
                    quartile.Minimum = y_Values[i];
                    break;
                }
            }

            for (int i = count - 1; i >= 0; i--)
            {
                if (y_Values[i] > quartile.UpperQuartile + rangeIQR)
                {
                    quartile.Outliers.Add(y_Values[i]);
                }
                else
                {
                    quartile.Maximum = y_Values[i];
                    break;
                }
            }
        }

        private static double GetInclusiveQuartileValue(double[] y_Values, int count, double percentile)
        {
            if (count == 0)
            {
                return 0;
            }
            else if (count == 1)
            {
                return y_Values[0];
            }

            double rank = percentile * (count - 1),
            integerRank = Math.Floor(Math.Abs(rank)),
            fractionRank = rank - integerRank;
            int index = Convert.ToInt32(integerRank);
            return (fractionRank * (y_Values[index + 1] - y_Values[index])) + y_Values[index];
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            BoxPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                CalculateBoxAndWhiskerPathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            BoxPathOptions.Clear();
            CalculateBoxAndWhiskerPathOption();
            base.UpdateDirection();
        }

        private static void FindBoxPlotValues(double[] y_Values, BoxPoint point, BoxPlotMode mode)
        {
            int y_Count = y_Values.Length;
            BoxPlotQuartile quartile = new BoxPlotQuartile
            {
                Average = y_Values.Sum() / y_Count,
                LowerQuartile = 0,
                UpperQuartile = 0,
                Maximum = 0,
                Minimum = 0,
                Median = 0,
                Outliers = new List<double>()
            };
            if (mode == BoxPlotMode.Exclusive)
            {
                quartile.LowerQuartile = GetExclusiveQuartileValue(y_Values, y_Count, 0.25);
                quartile.UpperQuartile = GetExclusiveQuartileValue(y_Values, y_Count, 0.75);
                quartile.Median = GetExclusiveQuartileValue(y_Values, y_Count, 0.5);
            }
            else if (mode == BoxPlotMode.Inclusive)
            {
                quartile.LowerQuartile = GetInclusiveQuartileValue(y_Values, y_Count, 0.25);
                quartile.UpperQuartile = GetInclusiveQuartileValue(y_Values, y_Count, 0.75);
                quartile.Median = GetInclusiveQuartileValue(y_Values, y_Count, 0.5);
            }
            else
            {
                quartile.Median = ChartHelper.GetMedian(y_Values);
                GetQuartileValues(y_Values, y_Count, quartile);
            }

            GetMinMaxOutlier(y_Values, y_Count, quartile);
            point.Minimum = quartile.Minimum;
            point.Maximum = quartile.Maximum;
            point.LowerQuartile = quartile.LowerQuartile;
            point.UpperQuartile = quartile.UpperQuartile;
            point.Median = quartile.Median;
            point.Outliers = quartile.Outliers.ToArray();
            point.Average = quartile.Average;
        }

        protected override void ProcessExpandoObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string pointColor = Series.PointColorMapping;
            int index = 0;
            BoxPoint point;
            if (currentViewData != null)
            {
                foreach (object data in currentViewData)
                {
                    IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                    object x, y, color;
                    expandoData.TryGetValue(x_Name, out x);
                    expandoData.TryGetValue(y_Name, out y);
                    expandoData.TryGetValue(pointColor, out color);
                    point = new BoxPoint()
                    {
                        X = x,
                        Y = y,
                        Interior = Convert.ToString(color, CultureInfo.InvariantCulture),
                        Text = Convert.ToString(GetTextMapping(), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        protected override void ProcessDynamicObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string pointColor = Series.PointColorMapping;
            int index = 0;
            BoxPoint point;
            if (currentViewData != null)
            {
                foreach (object data in currentViewData)
                {
                    point = new BoxPoint()
                    {
                        X = ChartHelper.GetDynamicMember(data, x_Name),
                        Y = ChartHelper.GetDynamicMember(data, y_Name),
                        Interior = Convert.ToString(ChartHelper.GetDynamicMember(data, pointColor), CultureInfo.InvariantCulture),
                        Text = Convert.ToString(ChartHelper.GetDynamicMember(data, GetTextMapping()), CultureInfo.InvariantCulture),
                        Tooltip = Convert.ToString(ChartHelper.GetDynamicMember(data, Series.TooltipMappingName), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        protected override void ProcessJObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string pointColor = Series.PointColorMapping;
            int index = 0;
            BoxPoint point;
            if (currentViewData != null)
            {
                foreach (object data in currentViewData)
                {
                    JObject jsonObject = (JObject)data;
                    point = new BoxPoint()
                    {
                        X = jsonObject.GetValue(x_Name, StringComparison.Ordinal),
                        Y = jsonObject.GetValue(y_Name, StringComparison.Ordinal),
                        Interior = Convert.ToString(jsonObject.GetValue(pointColor, StringComparison.Ordinal), CultureInfo.InvariantCulture),
                        Text = Convert.ToString(jsonObject.GetValue(GetTextMapping(), StringComparison.Ordinal), CultureInfo.InvariantCulture),
                        Tooltip = Convert.ToString(jsonObject.GetValue(Series.TooltipMappingName, StringComparison.Ordinal), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        protected override void ProcessObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            if (firstDataType != null && currentViewData != null)
            {
                PropertyAccessor x = new PropertyAccessor(firstDataType.GetProperty(x_Name));
                PropertyAccessor y = new PropertyAccessor(firstDataType.GetProperty(y_Name));
                PropertyAccessor pointColor = new PropertyAccessor(firstDataType.GetProperty(Series.PointColorMapping));
                PropertyAccessor textMapping = new PropertyAccessor(firstDataType.GetProperty(GetTextMapping()));
                PropertyAccessor tooltipMapping = new PropertyAccessor(firstDataType.GetProperty(Series.TooltipMappingName));
                int index = 0;
                BoxPoint point;
                foreach (object data in currentViewData)
                {
                    point = new BoxPoint()
                    {
                        X = x.GetValue(data),
                        Y = y.GetValue(data),
                        Interior = Convert.ToString(pointColor.GetValue(data), CultureInfo.InvariantCulture),
                        Text = Convert.ToString(textMapping.GetValue(data), CultureInfo.InvariantCulture),
                        Tooltip = Convert.ToString(tooltipMapping.GetValue(data), CultureInfo.InvariantCulture)
                    };
                    GetSetXValue(point, index);
                    SetEmptyPoint(point, index, firstDataType);
                    index++;
                }
            }
        }

        private void CalculateBoxAndWhiskerPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            string direction = string.Empty;
            PathOptions option;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            string symbolId = Series.Container.ID + "_Series_" + Index + "_Point_";
            string id;
            foreach (BoxPoint point in Points)
            {
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? Points[point.Index - 1] : null, point, point.Index + 1 < Points.Count ? Points[point.Index + 1] : null, XAxisRenderer))
                {
                    FindBoxPlotValues((double[])point.Y, point, Series.BoxPlotMode);
                    UpdateTipRegion(point, sideBySideInfo);
                    Rect centerRegion = GetRectangle(point.XValue + sideBySideInfo.Start, point.UpperQuartile, point.XValue + sideBySideInfo.End, point.LowerQuartile);
                    point.Regions.Add(centerRegion);
                    PointRenderEventArgs argsData = TriggerEvent(point, Interior, new BorderModel
                        {
                            Color = (!string.IsNullOrEmpty(Series.Border.Color) && Series.Border.Color != "transparent") ? Series.Border.Color : ChartHelper.GetSaturationColor(Interior, -0.6),
                            Width = !double.IsNaN(Series.Border.Width) ? Series.Border.Width : 1
                        });
                    if (!argsData.Cancel)
                    {
                        direction = GetPathString(
                                point, Series, ChartHelper.GetPoint(point.XValue, point.Median, XAxisRenderer, YAxisRenderer, Owner.RequireInvertedAxis), ChartHelper.GetPoint(point.XValue + sideBySideInfo.Median, point.Average, XAxisRenderer, YAxisRenderer, Owner.RequireInvertedAxis));
                        id = symbolId + point.Index + "_BoxPath";
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, argsData.Border.Width, argsData.Border.Color, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText)
                            {
                                Visibility = visibility
                            };
                            BoxPathOptions.Add(option);
                        }

                        for (int i = 0; i < point.Outliers.Length; i++)
                        {
                            ChartInternalLocation location = ChartHelper.GetPoint(point.XValue + sideBySideInfo.Median, point.Outliers[i], XAxisRenderer, YAxisRenderer, Series.Container.RequireInvertedAxis);
                            Size size = new Size(Series.Marker.Width, Series.Marker.Height);
                            point.SymbolLocations.Add(location);
                            UpdateTipSize(point, new Rect { X = location.X - (size.Width / 2), Y = location.Y - (size.Height / 2), Width = size.Width, Height = size.Height }, true);
                        }
                    }
                }
            }
        }

        internal override List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            BoxPoint boxPoint = currentPoint as BoxPoint;
            List<string> text = new List<string>();
            text.Add(!string.IsNullOrEmpty(boxPoint.Text) ? boxPoint.Text : boxPoint.Median.ToString(Culture));
            text.Add(!string.IsNullOrEmpty(boxPoint.Text) ? boxPoint.Text : boxPoint.Maximum.ToString(Culture));
            text.Add(!string.IsNullOrEmpty(boxPoint.Text) ? boxPoint.Text : boxPoint.Minimum.ToString(Culture));
            text.Add(!string.IsNullOrEmpty(boxPoint.Text) ? boxPoint.Text : boxPoint.UpperQuartile.ToString(Culture));
            text.Add(!string.IsNullOrEmpty(boxPoint.Text) ? boxPoint.Text : boxPoint.LowerQuartile.ToString(Culture));
            foreach (double liers in boxPoint.Outliers)
            {
                text.Add(!string.IsNullOrEmpty(boxPoint.Text) ? boxPoint.Text : liers.ToString(Culture));
            }

            return text;
        }

        internal override object GetMarkerY(Point point)
        {
            BoxPoint boxPoint = point as BoxPoint;
            object y = boxPoint.Outliers[0];
            return y;
        }

        private void UpdateTipSize(BoxPoint point, Rect region, bool isInverted)
        {
            double borderWidth = Series.Border.Width != 0 && !double.IsNaN(Series.Border.Width) ? Series.Border.Width : 1;
            if (!isInverted)
            {
                region.X -= borderWidth / 2;
                region.Width = region.Width != 0 && !double.IsNaN(region.Width) ? region.Width : borderWidth;
            }
            else
            {
                region.Y -= borderWidth / 2;
                region.Height = region.Height != 0 && !double.IsNaN(region.Height) ? region.Height : borderWidth;
            }

            point.Regions.Add(region);
        }

        private void UpdateTipRegion(BoxPoint point, DoubleRange sideBySideInfo)
        {
            UpdateTipSize(point, GetRectangle(point.XValue + sideBySideInfo.Median, point.Maximum, point.XValue + sideBySideInfo.Median, point.Minimum), Series.Container.RequireInvertedAxis);
        }

        private string GetPathString(BoxPoint point, ChartSeries series, ChartInternalLocation median, ChartInternalLocation average)
        {
            Rect topRect = point.Regions[0];
            Rect midRect = point.Regions[1];
            string direction = string.Empty;
            double center = series.Container.RequireInvertedAxis ? topRect.Y + (topRect.Height / 2) : topRect.X + (topRect.Width / 2),
            midWidth = midRect.X + midRect.Width, midHeight = midRect.Y + midRect.Height, topWidth = topRect.X + topRect.Width, topHeight = topRect.Y + topRect.Height;
            if (!series.Container.RequireInvertedAxis)
            {
                UpdateTipSize(point, new Rect { X = midRect.X, Y = topRect.Y, Width = midWidth - midRect.X, Height = 0 }, true);
                UpdateTipSize(point, new Rect { X = midRect.X, Y = topHeight, Width = midWidth - midRect.X, Height = 0 }, true);
                direction += "M " + midRect.X.ToString(Culture) + SPACE + topRect.Y.ToString(Culture) + SPACE + " L " + midWidth.ToString(Culture) + SPACE + topRect.Y.ToString(Culture);
                direction += " M " + center.ToString(Culture) + SPACE + topRect.Y.ToString(Culture) + SPACE + " L " + center.ToString(Culture) + SPACE + midRect.Y.ToString(Culture);
                direction += " M " + midRect.X.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + SPACE + " L " + midWidth.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + " L " + midWidth.ToString(Culture) + SPACE + midHeight.ToString(Culture) + " L " + midRect.X.ToString(Culture) + SPACE + midHeight.ToString(Culture) + " Z";
                direction += " M " + center.ToString(Culture) + SPACE + midHeight.ToString(Culture) + " L " + center.ToString(Culture) + SPACE + topHeight.ToString(Culture);
                direction += " M " + midRect.X.ToString(Culture) + SPACE + topHeight.ToString(Culture) + " L " + midWidth.ToString(Culture) + SPACE + topHeight.ToString(Culture);
                direction += " M " + midRect.X.ToString(Culture) + SPACE + median.Y.ToString(Culture) + " L " + midWidth.ToString(Culture) + SPACE + median.Y.ToString(Culture);
                direction += series.ShowMean ? " M " + (average.X - 5).ToString(Culture) + SPACE + (average.Y - 5).ToString(Culture) + " L " + (average.X + 5).ToString(Culture) + SPACE + (average.Y + 5).ToString(Culture) +
                    " M " + (average.X + 5).ToString(Culture) + SPACE + (average.Y - 5).ToString(Culture) + " L " + (average.X - 5).ToString(Culture) + SPACE + (average.Y + 5).ToString(Culture) : string.Empty;
            }
            else
            {
                UpdateTipSize(point, new Rect { X = topRect.X, Y = midRect.Y, Width = 0, Height = midHeight - midRect.Y }, false);
                UpdateTipSize(point, new Rect { X = topWidth, Y = midRect.Y, Width = 0, Height = midHeight - midRect.Y }, true);
                direction += "M " + topRect.X.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + " L " + topRect.X.ToString(Culture) + SPACE + midHeight.ToString(Culture);
                direction += "M " + topRect.X.ToString(Culture) + SPACE + center.ToString(Culture) + SPACE + " L " + midRect.X.ToString(Culture) + SPACE + center.ToString(Culture);
                direction += " M " + midRect.X.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + SPACE + " L " + midWidth.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + " L " + midWidth.ToString(Culture) + SPACE + midHeight.ToString(Culture) + " L " + midRect.X.ToString(Culture) + SPACE + midHeight.ToString(Culture) + " Z";
                direction += " M " + midWidth.ToString(Culture) + SPACE + center.ToString(Culture) + " L " + topWidth.ToString(Culture) + SPACE + center.ToString(Culture);
                direction += " M " + topWidth.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + " L " + topWidth.ToString(Culture) + SPACE + midHeight.ToString(Culture);
                direction += " M " + median.X.ToString(Culture) + SPACE + midRect.Y.ToString(Culture) + SPACE + " L " + median.X.ToString(Culture) + SPACE + midHeight.ToString(Culture);
                direction += series.ShowMean ? "M " + (average.X + 5).ToString(Culture) + SPACE + (average.Y - 5).ToString(Culture) + " L " + (average.X - 5).ToString(Culture) + SPACE + (average.Y + 5).ToString(Culture) +
                    "M " + (average.X - 5).ToString(Culture) + SPACE + (average.Y - 5).ToString(Culture) + " L " + (average.X + 5).ToString(Culture) + SPACE + (average.Y + 5).ToString(Culture) : string.Empty;
            }

            return direction;
        }

        internal override SeriesValueType SeriesType()
        {
            return SeriesValueType.BoxPlot;
        }

        internal override bool FindVisibility(Point point)
        {
            double[] boxPlotValue;
            boxPlotValue = ((double[])point.Y).Where((x) =>
            {
                return !double.IsNaN(x);
            }).ToArray();
            Array.Sort(boxPlotValue);
            point.Y = boxPlotValue;
            if (boxPlotValue.Length != 0)
            {
                YMin = Math.Min(YMin, boxPlotValue.Min());
                YMax = Math.Max(YMax, boxPlotValue.Max());
            }

            SetXYMinMax(point.XValue, point.YValue);
            return boxPlotValue.Length == 0;
        }

        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            BoxPathOptions.ForEach(option => option.Visibility = visibility);
            switch (property)
            {
                case "Fill":
                    BoxPathOptions.ForEach(option => option.Fill = Interior);
                    break;
                case "DashArray":
                    BoxPathOptions.ForEach(option => option.StrokeDashArray = Series.DashArray);
                    break;
                case "Width":
                    BoxPathOptions.ForEach(option => option.StrokeWidth = Series.Border.Width);
                    break;
                case "Color":
                    BoxPathOptions.ForEach(option => option.Stroke = Series.Border.Color);
                    break;
                case "Opacity":
                    BoxPathOptions.ForEach(option => option.Opacity = Series.Opacity);
                    break;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in BoxPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
    }
}