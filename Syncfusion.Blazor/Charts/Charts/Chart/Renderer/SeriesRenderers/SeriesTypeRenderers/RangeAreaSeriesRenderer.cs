using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Globalization;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class RangeAreaSeriesRenderer : LineBaseSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            string name = SeriesID();
            options = new PathOptions()
            {
                Id = name,
                Fill = Interior,
                StrokeWidth = Series.Border.Width,
                Stroke = Series.Border.Color,
                Opacity = Series.Opacity,
                StrokeDashArray = Series.DashArray,
                Direction = Direction.ToString()
            };

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(ClipRectId(), AnimationType.Linear);
            }
        }

        private void CalculateDirection()
        {
            bool isInverted = Owner.RequireInvertedAxis;
            string command = "M";
            bool? closed = null;
            Direction = new System.Text.StringBuilder();
            foreach (FinancialPoint point in Points)
            {
                ChartInternalLocation lowPoint = new ChartInternalLocation(0, 0);
                ChartInternalLocation highPoint = new ChartInternalLocation(0, 0);
                if (point.Visible)
                {
                    point.SymbolLocations = new List<ChartInternalLocation>();
                    point.Regions = new List<Rect>();
                    double low = Math.Min(Convert.ToDouble(point.Low, Culture), Convert.ToDouble(point.High, Culture)), high = Math.Max(Convert.ToDouble(point.Low, Culture), Convert.ToDouble(point.High, Culture)), temp;
                    if (YAxisRenderer.Axis.IsInversed)
                    {
                        temp = low;
                        low = high;
                        high = temp;
                    }

                    lowPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(low), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    highPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(point.XValue), YAxisRenderer.GetPointValue(high), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                    point.SymbolLocations.Add(highPoint);
                    point.SymbolLocations.Add(lowPoint);
                    Rect rect = new Rect(Math.Min(lowPoint.X, highPoint.X), Math.Min(lowPoint.Y, highPoint.Y), Math.Max(Math.Abs(highPoint.X - lowPoint.X), Series.Marker.Width), Math.Max(Math.Abs(highPoint.Y - lowPoint.Y), Series.Marker.Width));
                    if (!isInverted)
                    {
                        rect.X -= Series.Marker.Width / 2;
                    }
                    else
                    {
                        rect.Y -= Series.Marker.Width / 2;
                    }

                    point.Regions.Add(rect);
                }

                Point prevPoint = point.Index - 1 > -1 ? Points[point.Index - 1] : null;
                Point nextPoint = point.Index + 1 < Points.Count ? Points[point.Index + 1] : null;
                if (point.Visible && ChartHelper.WithInRange(prevPoint, point, nextPoint, XAxisRenderer))
                {
                    Direction.Append(command + SPACE + lowPoint.X.ToString(Culture) + SPACE + lowPoint.Y.ToString(Culture) + SPACE);
                    closed = false;
                    if ((point.Index + 1 < Points.Count && (nextPoint != null) ? !nextPoint.Visible : false) || point.Index == Points.Count - 1)
                    {
                        Direction = CloseRangeAreaPath(Points, point, point.Index);
                        command = "M";
                        Direction.Append(SPACE + "Z");
                        closed = true;
                    }

                    command = "L";
                }
                else
                {
                    if (closed == false && point.Index != 0)
                    {
                        Direction = CloseRangeAreaPath(Points, point, point.Index);
                        closed = true;
                    }

                    command = "M";
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }
            }
        }

        internal override void UpdateDirection()
        {
            CalculateDirection();
            options.Direction = Direction.ToString();
            base.UpdateDirection();
        }

        protected StringBuilder CloseRangeAreaPath(List<Point> points, Point point, int i)
        {
            for (int j = i; j >= 0; j--)
            {
                if (points[j].Visible && (points[j].SymbolLocations.Count > 0) ? points[j].SymbolLocations[0] != null : false)
                {
                    point = points[j];
                    Direction.Append("L" + SPACE + point.SymbolLocations[0].X.ToString(Culture) + SPACE + point.SymbolLocations[0].Y.ToString(Culture) + SPACE);
                }
                else
                {
                    break;
                }
            }

            return Direction;
        }

        protected override void ProcessExpandoObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string tempHigh = Series.High;
            string tempLow = Series.Low;
            string pointColor = Series.PointColorMapping;
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                object x, y, high, low, color;
                expandoData.TryGetValue(x_Name, out x);
                expandoData.TryGetValue(y_Name, out y);
                expandoData.TryGetValue(tempHigh, out high);
                expandoData.TryGetValue(tempLow, out low);
                expandoData.TryGetValue(pointColor, out color);
                point = new FinancialPoint()
                {
                    X = x,
                    Y = y,
                    High = high,
                    Low = low,
                    Interior = Convert.ToString(color, CultureInfo.InvariantCulture),
                    Text = Convert.ToString(GetTextMapping(), CultureInfo.InvariantCulture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessDynamicObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string high = Series.High;
            string low = Series.Low;
            string pointColor = Series.PointColorMapping;
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                point = new FinancialPoint()
                {
                    X = ChartHelper.GetDynamicMember(data, x_Name),
                    Y = ChartHelper.GetDynamicMember(data, y_Name),
                    High = ChartHelper.GetDynamicMember(data, high),
                    Low = ChartHelper.GetDynamicMember(data, low),
                    Interior = Convert.ToString(ChartHelper.GetDynamicMember(data, pointColor), CultureInfo.InvariantCulture),
                    Text = Convert.ToString(ChartHelper.GetDynamicMember(data, GetTextMapping()), CultureInfo.InvariantCulture),
                    Tooltip = Convert.ToString(ChartHelper.GetDynamicMember(data, Series.TooltipMappingName), CultureInfo.InvariantCulture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessJObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            string high = Series.High;
            string low = Series.Low;
            string pointColor = Series.PointColorMapping;
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                JObject jsonObject = (JObject)data;
                point = new FinancialPoint()
                {
                    X = jsonObject.GetValue(x_Name, StringComparison.InvariantCulture),
                    Y = jsonObject.GetValue(y_Name, StringComparison.InvariantCulture),
                    High = jsonObject.GetValue(high, StringComparison.InvariantCulture),
                    Low = jsonObject.GetValue(low, StringComparison.InvariantCulture),
                    Interior = Convert.ToString(jsonObject.GetValue(pointColor, StringComparison.InvariantCulture), CultureInfo.InvariantCulture),
                    Text = Convert.ToString(jsonObject.GetValue(GetTextMapping(), StringComparison.InvariantCulture), CultureInfo.InvariantCulture),
                    Tooltip = Convert.ToString(jsonObject.GetValue(Series.TooltipMappingName, StringComparison.Ordinal), Culture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            PropertyAccessor x = new PropertyAccessor(firstDataType.GetProperty(x_Name));
            PropertyAccessor y = new PropertyAccessor(firstDataType.GetProperty(y_Name));
            PropertyAccessor high = new PropertyAccessor(firstDataType.GetProperty(Series.High));
            PropertyAccessor low = new PropertyAccessor(firstDataType.GetProperty(Series.Low));
            PropertyAccessor pointColor = new PropertyAccessor(firstDataType.GetProperty(Series.PointColorMapping));
            PropertyAccessor textMapping = new PropertyAccessor(firstDataType.GetProperty(GetTextMapping()));
            PropertyAccessor tooltipMapping = new PropertyAccessor(firstDataType.GetProperty(Series.TooltipMappingName));
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                point = new FinancialPoint()
                {
                    X = x.GetValue(data),
                    Y = y.GetValue(data),
                    High = high.GetValue(data),
                    Low = low.GetValue(data),
                    Interior = Convert.ToString(pointColor.GetValue(data), CultureInfo.InvariantCulture),
                    Text = Convert.ToString(textMapping.GetValue(data), CultureInfo.InvariantCulture),
                    Tooltip = Convert.ToString(tooltipMapping.GetValue(data), CultureInfo.InvariantCulture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        internal override SeriesValueType SeriesType()
        {
            return SeriesValueType.HighLow;
        }

        internal override bool FindVisibility(Point point)
        {
            FinancialPoint rangepoint = point as FinancialPoint;
            if (rangepoint.High == null || rangepoint.Low == null)
            {
                YMin = Math.Min(YMin, 0);
                YMax = Math.Max(YMax, 0);
                XMin = Math.Min(XMin, (double)rangepoint.XValue);
                XMax = Math.Max(XMax, (double)rangepoint.XValue);
                return rangepoint.High == null || rangepoint.Low == null;
            }

            SetHiloMinMax((double)rangepoint.High, (double)rangepoint.Low, (double)rangepoint.XValue);
            return rangepoint.X.Equals(null) || double.IsNaN((double)rangepoint.Low) || double.IsNaN((double)rangepoint.High);
        }

        private void SetHiloMinMax(double high, double low, double x)
        {
            XMin = Math.Min(XMin, x);
            XMax = Math.Max(XMax, x);
            YMin = Math.Min(YMin, Math.Min((double.IsNaN(low) || double.IsNaN(low)) ? YMin : low, (double.IsNaN(high) || double.IsNaN(high)) ? YMin : high));
            YMax = Math.Max(YMax, Math.Max((double.IsNaN(low) || double.IsNaN(low)) ? YMax : low, (double.IsNaN(high) || double.IsNaN(high)) ? YMax : high));
        }

        internal override void SetEmptyPoint(Point point, int i, Type type)
        {
            FinancialPoint rangepoint = point as FinancialPoint;
            if (!FindVisibility(rangepoint))
            {
                rangepoint.Visible = true;
                return;
            }

            rangepoint.IsEmpty = true;
            switch (Series.EmptyPointSettings.Mode)
            {
                case EmptyPointMode.Zero:
                    rangepoint.Visible = true;
                    rangepoint.High = rangepoint.Low = 0;
                    break;
                case EmptyPointMode.Average:
                    rangepoint.High = rangepoint.High == null || double.IsNaN((double)rangepoint.High) ? GetAverage(type, Series.High, i) : rangepoint.High;
                    rangepoint.Low = rangepoint.Low == null || double.IsNaN((double)rangepoint.Low) ? GetAverage(type, Series.Low, i) : rangepoint.Low;
                    rangepoint.Visible = true;
                    break;
                case EmptyPointMode.Drop:
                case EmptyPointMode.Gap:
                    point.Visible = false;
                    break;
            }
        }

        internal override List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            FinancialPoint rangepoint = currentPoint as FinancialPoint;
            List<string> text = new List<string>();
            text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Max((double)rangepoint.High, (double)rangepoint.Low).ToString(CultureInfo.InvariantCulture));
            text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Min((double)rangepoint.High, (double)rangepoint.Low).ToString(CultureInfo.InvariantCulture));
            return text;
        }

        internal override bool IsRectSeries()
        {
            return true;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            RenderSeriesElement(builder, options);
            builder.CloseElement();
        }
    }
}
