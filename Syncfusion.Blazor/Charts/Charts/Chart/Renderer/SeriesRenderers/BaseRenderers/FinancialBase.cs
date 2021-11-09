using System;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public abstract class FinancialBaseRenderer : ColumnBaseRenderer
    {
        protected override void ProcessExpandoObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                IDictionary<string, object> expandoData = (IDictionary<string, object>)data;
                object x, y, high, low, open, close, color;
                expandoData.TryGetValue(x_Name, out x);
                expandoData.TryGetValue(y_Name, out y);
                expandoData.TryGetValue(Series.High, out high);
                expandoData.TryGetValue(Series.Low, out low);
                expandoData.TryGetValue(Series.Open, out open);
                expandoData.TryGetValue(Series.Close, out close);
                expandoData.TryGetValue(Series.PointColorMapping, out color);
                point = new FinancialPoint()
                {
                    X = x,
                    Y = y,
                    High = high,
                    Low = low,
                    Open = open,
                    Close = close,
                    Interior = Convert.ToString(color, CultureInfo.InvariantCulture),
                    Text = Convert.ToString(GetTextMapping(), CultureInfo.InvariantCulture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        internal override bool ShouldRenderMarker()
        {
            return false;
        }

        protected override void ProcessDynamicObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            int index = 0;
            FinancialPoint point;
            foreach (object data in CurrentViewData)
            {
                point = new FinancialPoint()
                {
                    X = ChartHelper.GetDynamicMember(data, x_Name),
                    Y = ChartHelper.GetDynamicMember(data, y_Name),
                    High = ChartHelper.GetDynamicMember(data, Series.High),
                    Low = ChartHelper.GetDynamicMember(data, Series.Low),
                    Open = ChartHelper.GetDynamicMember(data, Series.Open),
                    Close = ChartHelper.GetDynamicMember(data, Series.Close),
                    Interior = Convert.ToString(ChartHelper.GetDynamicMember(data, Series.PointColorMapping), CultureInfo.InvariantCulture),
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
                    Open = jsonObject.GetValue(Series.Open, StringComparison.Ordinal),
                    Close = jsonObject.GetValue(Series.Close, StringComparison.Ordinal),
                    Interior = Convert.ToString(jsonObject.GetValue(Series.PointColorMapping, StringComparison.Ordinal), CultureInfo.InvariantCulture),
                    Text = Convert.ToString(jsonObject.GetValue(GetTextMapping(), StringComparison.Ordinal), CultureInfo.InvariantCulture),
                    Tooltip = Convert.ToString(jsonObject.GetValue(Series.TooltipMappingName, StringComparison.Ordinal), CultureInfo.InvariantCulture)
                };
                GetSetXValue(point, index);
                SetEmptyPoint(point, index, firstDataType);
                index++;
            }
        }

        protected override void ProcessObjectData(Type firstDataType, string x_Name, string y_Name, IEnumerable<object> currentViewData)
        {
            if (firstDataType != null)
            {
                PropertyAccessor x = new PropertyAccessor(firstDataType.GetProperty(x_Name));
                PropertyAccessor y = new PropertyAccessor(firstDataType.GetProperty(y_Name));
                PropertyAccessor high = new PropertyAccessor(firstDataType.GetProperty(Series.High));
                PropertyAccessor low = new PropertyAccessor(firstDataType.GetProperty(Series.Low));
                PropertyAccessor open = new PropertyAccessor(firstDataType.GetProperty(Series.Open));
                PropertyAccessor close = new PropertyAccessor(firstDataType.GetProperty(Series.Close));
                PropertyAccessor volume = new PropertyAccessor(firstDataType.GetProperty(Series.Volume));
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
                        Open = open.GetValue(data),
                        Close = close.GetValue(data),
                        Volume = volume.GetValue(data),
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

        internal override List<string> GetLabelText(Point currentPoint, ChartSeriesRenderer seriesRenderer)
        {
            FinancialPoint rangepoint = currentPoint as FinancialPoint;
            List<string> text = new List<string>();
            SeriesValueType type = Series.Renderer.SeriesType();
            switch (type)
            {
                case SeriesValueType.HighLow:
                    text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Max((double)rangepoint.High, (double)rangepoint.Low).ToString(CultureInfo.InvariantCulture));
                    text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Min((double)rangepoint.High, (double)rangepoint.Low).ToString(CultureInfo.InvariantCulture));
                    break;
                case SeriesValueType.HighLowOpenClose:
                    text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Max((double)rangepoint.High, (double)rangepoint.Low).ToString(CultureInfo.InvariantCulture));
                    text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Min((double)rangepoint.High, (double)rangepoint.Low).ToString(CultureInfo.InvariantCulture));
                    text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Max((double)rangepoint.Open, (double)rangepoint.Close).ToString(CultureInfo.InvariantCulture));
                    text.Add(!string.IsNullOrEmpty(rangepoint.Text) ? rangepoint.Text : Math.Min((double)rangepoint.Open, (double)rangepoint.Close).ToString(CultureInfo.InvariantCulture));
                    break;
            }

            return text;
        }

        internal override bool IsRectSeries()
        {
            return true;
        }

        internal void RenderSeriesElement(RenderTreeBuilder builder, PathOptions options)
        {
            SvgRenderer.RenderPath(builder, options);
            DynamicOptions.PathId.Add(options.Id);
            DynamicOptions.CurrentDirection.Add(options.Direction);
        }

        internal void SetHiloMinMax(double x, double high, double low)
        {
            XMin = Math.Min(XMin, x);
            XMax = Math.Max(XMax, x);
            YMin = Math.Min(YMin, Math.Min(double.IsNaN(low) ? YMin : low, double.IsNaN(high) ? YMin : high));
            YMax = Math.Max(YMax, Math.Max(double.IsNaN(low) ? YMax : low, double.IsNaN(high) ? YMax : high));
        }

        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            ColumnPathOptions.ForEach(option => option.Visibility = visibility);
            switch (property)
            {
                case "Fill":
                    ColumnPathOptions.ForEach(option => option.Fill = Interior);
                    break;
                case "DashArray":
                    ColumnPathOptions.ForEach(option => option.StrokeDashArray = Series.DashArray);
                    break;
                case "Width":
                    ColumnPathOptions.ForEach(option => option.StrokeWidth = Series.Border.Width);
                    break;
                case "Opacity":
                    ColumnPathOptions.ForEach(option => option.Opacity = Series.Opacity);
                    break;
            }
        }
    }
}