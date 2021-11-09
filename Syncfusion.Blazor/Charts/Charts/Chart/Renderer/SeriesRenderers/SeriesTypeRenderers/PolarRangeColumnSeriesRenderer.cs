using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class PolarRangeColumnSeriesRenderer : PolarRadarColumnSeriesRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            ColumnDrawTypeRender(Series, XAxisRenderer.Axis, YAxisRenderer.Axis);
            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions = new List<PathOptions>();
            ColumnDrawTypeRender(Series, XAxisRenderer.Axis, YAxisRenderer.Axis);
            base.UpdateDirection();
        }

        public void ColumnDrawTypeRender(ChartSeries series, ChartAxis x_Axis, ChartAxis y_Axis)
        {
            double startAngle, endAngle, itemCurrentXPos, radius, inversedValue, pointStartAngle, pointEndAngle,
               x1, x2, y1, y2, startValue, endValue, innerRadius, d_StartX, d_StartY, d_EndX, d_EndY;
            double min = XAxisRenderer.ActualRange.Start;
            double centerX = (series.Renderer.ClipRect.Width / 2) + series.Renderer.ClipRect.X;
            double centerY = (series.Renderer.ClipRect.Height / 2) + series.Renderer.ClipRect.Y;
            string direction, arcValue;
            double sumofYValues = 0;
            double interval = (series.Renderer.Points.Count > 1 ? series.Renderer.Points[1].XValue : 2 * series.Renderer.Points[0].XValue) - series.Renderer.Points[0].XValue;
            double ticks = (x_Axis.ValueType == ValueType.Category && x_Axis.LabelPlacement == LabelPlacement.BetweenTicks) ? 0 : x_Axis.IsInversed ? -interval / 2 : interval / 2;
            double rangeInterval = x_Axis.ValueType == ValueType.DateTime ? XAxisRenderer.DateTimeInterval : 1;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            GetSeriesPosition();
            do
            {
                sumofYValues += rangeInterval;
                min += rangeInterval;
            }
            while (min <= XAxisRenderer.ActualRange.End - (x_Axis.ValueType == ValueType.Category ? 0 : 1));
            PathOptions option;
            foreach (FinancialPoint point in series.Renderer.Points)
            {
                point.SymbolLocations = new List<ChartInternalLocation>();
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange(point.Index - 1 > -1 ? series.Renderer.Points[point.Index - 1] : null, point, point.Index + 1 < series.Renderer.Points.Count ? series.Renderer.Points[point.Index + 1] : null, Series.Renderer.XAxisRenderer))
                {
                    inversedValue = x_Axis.IsInversed ? (XAxisRenderer.VisibleRange.End - point.XValue) : point.XValue - XAxisRenderer.VisibleRange.Start;
                    itemCurrentXPos = inversedValue + (((interval / series.Renderer.RectCount) * (x_Axis.IsInversed ? (series.Renderer.RectCount - 1 - series.Renderer.Position) : series.Renderer.Position)) - ticks) + (sumofYValues / 360 * x_Axis.StartAngle);
                    itemCurrentXPos = itemCurrentXPos / sumofYValues;
                    startAngle = 2 * Math.PI * (itemCurrentXPos + x_Axis.StartAngle);
                    endAngle = 2 * Math.PI * (itemCurrentXPos + x_Axis.StartAngle + ((interval / series.Renderer.RectCount) / sumofYValues));
                    if (startAngle == 0 && endAngle == 0)
                    {
                        endAngle = 2 * Math.PI;
                        arcValue = "1";
                    }
                    else
                    {
                        arcValue = "0";
                    }

                    pointStartAngle = startAngle;
                    pointEndAngle = endAngle;
                    startAngle = (startAngle - (0.5 * Math.PI)) + (series.ColumnSpacing / 2);
                    endAngle = ((endAngle - (0.5 * Math.PI)) - 0.000001) - (series.ColumnSpacing / 2);

                    startValue = (double)point.Low;
                    endValue = (double)point.High;
                    endValue = y_Axis.ValueType == ValueType.Logarithmic ? ChartHelper.LogBase(endValue == 0 ? 1 : endValue, y_Axis.LogBase) : endValue;
                    endValue = endValue > YAxisRenderer.ActualRange.End ? YAxisRenderer.ActualRange.End : endValue;

                    radius = startValue == endValue ? 0 : Series.Renderer.Owner.AxisContainer.AxisLayout.Radius * ChartHelper.ValueToCoefficient(endValue, YAxisRenderer);
                    x1 = centerX + (radius * Math.Cos(startAngle));
                    x2 = centerX + (radius * Math.Cos(endAngle));
                    y1 = centerY + (radius * Math.Sin(startAngle));
                    y2 = centerY + (radius * Math.Sin(endAngle));
                    innerRadius = Series.Renderer.Owner.AxisContainer.AxisLayout.Radius * ChartHelper.ValueToCoefficient(startValue == 0 && YAxisRenderer.VisibleRange.Start != 0 ? YAxisRenderer.VisibleRange.Start : startValue, YAxisRenderer);
                    d_StartX = centerX + (innerRadius * Math.Cos(startAngle));
                    d_StartY = centerY + (innerRadius * Math.Sin(startAngle));
                    d_EndX = centerX + (innerRadius * Math.Cos(endAngle));
                    d_EndY = centerY + (innerRadius * Math.Sin(endAngle));
                    if (series.Type == ChartSeriesType.Polar)
                    {
                        direction = "M" + SPACE + x1.ToString(Culture) + SPACE + y1.ToString(Culture) + SPACE + "A" + SPACE + radius.ToString(Culture) + SPACE + radius.ToString(Culture) + SPACE + "0" + SPACE + arcValue.ToString(Culture) + SPACE + 1 + SPACE + x2.ToString(Culture) + SPACE + y2.ToString(Culture) + SPACE + "L" + SPACE + d_EndX.ToString(Culture) + SPACE + d_EndY.ToString(Culture) + SPACE +
                            "A" + SPACE + innerRadius.ToString(Culture) + SPACE + innerRadius.ToString(Culture) + SPACE + "1" + SPACE + "0" + SPACE + "0" + SPACE + d_StartX.ToString(Culture) + SPACE + d_StartY.ToString(Culture) + SPACE + "Z";
                    }
                    else
                    {
                        direction = "M" + SPACE + x1.ToString(Culture) + SPACE + y1.ToString(Culture) + SPACE + "L" + SPACE + x2.ToString(Culture) + SPACE + y2.ToString(Culture) + SPACE + "L " + d_EndX.ToString(Culture) + SPACE + d_EndY.ToString(Culture) + SPACE + "L" + SPACE + d_StartX.ToString(Culture) + SPACE + d_StartY.ToString(Culture) + SPACE + "Z";
                    }

                    point.RegionData = new PolarArc(pointStartAngle, pointEndAngle, innerRadius, radius, itemCurrentXPos);
                    PointRenderEventArgs argsData = TriggerEvent(point, Interior, new BorderModel() { Width = Series.Border.Width, Color = Series.Border.Color });
                    if (!argsData.Cancel)
                    {
                        if (series.Type == ChartSeriesType.Polar)
                        {
                            point.SymbolLocations.Add(new ChartInternalLocation(centerX + (radius * Math.Cos(startAngle + ((endAngle - startAngle) / 2))), centerY + (radius * Math.Sin(startAngle + ((endAngle - startAngle) / 2)))));
                            point.SymbolLocations.Add(new ChartInternalLocation(centerX + (innerRadius * Math.Cos(startAngle + ((endAngle - startAngle) / 2))), centerY + (innerRadius * Math.Sin(startAngle + ((endAngle - startAngle) / 2)))));
                        }
                        else
                        {
                            point.SymbolLocations.Add(new ChartInternalLocation((x1 + x2) / 2, (y1 + y2) / 2));
                            point.SymbolLocations.Add(new ChartInternalLocation((d_EndX + d_StartX) / 2, (d_EndY + d_StartY) / 2));
                        }

                        option = new PathOptions(Owner.ID + "_Series_" + series.Renderer.Index + "_Point_" + point.Index, direction, series.DashArray, argsData.Border.Width, argsData.Border.Color, series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText);
                        option.Visibility = visibility;
                        ColumnPathOptions.Add(option);
                    }
                }
            }
        }

        private void GetSeriesPosition()
        {
            List<ChartSeries> seriesCollection = new List<ChartSeries>();
            Dictionary<string, double> stackingGroup = new Dictionary<string, double>();
            RectPosition v_Series = new RectPosition { RectCount = 0, Position = double.NaN };
            foreach (ChartSeries series1 in Owner.SeriesContainer.Elements)
            {
                if (series1.Visible && (series1.Type == ChartSeriesType.Polar || series1.Type == ChartSeriesType.Radar) && series1.DrawType.ToString().Contains("Column", StringComparison.InvariantCulture))
                {
                    seriesCollection.Add(series1);
                }
            }

            for (int i = 0; i < seriesCollection.Count; i++)
            {
                ChartSeries series2 = seriesCollection[i];
                if (series2.DrawType.ToString().Contains("Stacking", StringComparison.InvariantCulture))
                {
                    if (!string.IsNullOrEmpty(series2.StackingGroup))
                    {
                        if (!stackingGroup.ContainsKey(series2.StackingGroup))
                        {
                            series2.Renderer.Position = v_Series.RectCount;
                            stackingGroup.Add(series2.StackingGroup, v_Series.RectCount++);
                        }
                        else
                        {
                            series2.Renderer.Position = stackingGroup[series2.StackingGroup];
                        }
                    }
                    else
                    {
                        if (double.IsNaN(v_Series.Position))
                        {
                            series2.Renderer.Position = v_Series.RectCount;
                            v_Series.Position = v_Series.RectCount++;
                        }
                        else
                        {
                            series2.Renderer.Position = v_Series.Position;
                        }
                    }
                }
                else
                {
                    series2.Renderer.Position = v_Series.RectCount++;
                }
            }

            for (int i = 0; i < seriesCollection.Count; i++)
            {
                seriesCollection[i].Renderer.RectCount = v_Series.RectCount;
            }
        }

        protected override void ProcessExpandoObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                object x, y, high, low, color;
                expandoData.TryGetValue(x_Name, out x);
                expandoData.TryGetValue(y_Name, out y);
                expandoData.TryGetValue(Series.High, out high);
                expandoData.TryGetValue(Series.Low, out low);
                expandoData.TryGetValue(Series.PointColorMapping, out color);
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
                    Tooltip = Convert.ToString(ChartHelper.GetDynamicMember(data, Series.TooltipMappingName), Culture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessJObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                JObject jsonObject = (JObject)data;
                point = new FinancialPoint()
                {
                    X = jsonObject.GetValue(x_Name, StringComparison.Ordinal),
                    Y = jsonObject.GetValue(y_Name, StringComparison.Ordinal),
                    High = jsonObject.GetValue(Series.High, StringComparison.Ordinal),
                    Low = jsonObject.GetValue(Series.Low, StringComparison.Ordinal),
                    Interior = Convert.ToString(jsonObject.GetValue(Series.PointColorMapping, StringComparison.Ordinal), CultureInfo.InvariantCulture),
                    Text = Convert.ToString(jsonObject.GetValue(GetTextMapping(), StringComparison.Ordinal), CultureInfo.InvariantCulture),
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

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null || !Series.Visible)
            {
                return;
            }

            CreateSeriesElements(builder);
            foreach (PathOptions option in ColumnPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
    }
}
