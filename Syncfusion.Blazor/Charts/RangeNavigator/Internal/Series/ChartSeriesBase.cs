using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using System.Reflection;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the range navigator series.
    /// </summary>
    public partial class RangeNavigatorSeries : SfDataBoundComponent
    {
        private CultureInfo culture = CultureInfo.InvariantCulture;
        private StringComparison comparison = StringComparison.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeNavigatorSeries"/> class.
        /// </summary>
        public RangeNavigatorSeries()
        {
        }

        internal RangeNavigatorSeries(SfRangeNavigator chart)
        {
            ChartInstance = chart;
            XMin = double.PositiveInfinity;
            XMax = double.NegativeInfinity;
            YMin = double.PositiveInfinity;
            YMax = double.NegativeInfinity;
        }

        internal int Index { get; set; }

        internal string Interior { get; set; }

        internal bool IsRectSeries { get; set; }

        internal SeriesCategories Category { get; set; } = SeriesCategories.Series;

        internal ChartAxisRenderer XAxisRenderer { get; set; }

        internal ChartAxisRenderer YAxisRenderer { get; set; }

        internal List<double> XData { get; set; } = new List<double>();

        internal List<double> YData { get; set; } = new List<double>();

        internal List<DataPoint> Points { get; set; }

        internal IEnumerable<object> CurrentViewData { get; set; }

        internal List<DataPoint> VisiblePoints { get; set; }

        internal int RecordsCount { get; set; }

        internal double XMin { get; set; } = double.NaN;

        internal double YMin { get; set; } = double.NaN;

        internal double XMax { get; set; } = double.NaN;

        internal double YMax { get; set; } = double.NaN;

        internal Rect ClipRect { get; set; } = new Rect();

        internal double SeriesLength { get; set; }

        private static void GetObjectValue(object obj, DataPoint point, PropertyInfo x, PropertyInfo y)
        {
            point.X = x != null ? x.GetValue(obj) : null;
            point.Y = y != null ? y.GetValue(obj) : null;
        }

        internal void RenderChart()
        {
            SeriesLength = 0;
            ChartInstance.RangeSliderModule.Points = new List<DataPoint>();
            if (ChartInstance.Series.Count != 0)
            {
                ChartInstance.Series.ForEach((RangeNavigatorSeries series) =>
                {
                    series.XData = new List<double>();
                    series.YData = new List<double>();
                    series.Points = new List<DataPoint>();
                    SeriesLength += 1;
                    DataManagerSuccess(series.CurrentViewData, series);
                });
            }
            else
            {
                DataManagerSuccess(ChartInstance.FinalData);
            }

            if (ChartInstance.Series.Count == 0 || SeriesLength == ChartInstance.Series.Count)
            {
                ChartInstance.RangeAxisModule.ProcessXAxis(ChartInstance);
                ChartInstance.RangeAxisModule.CalculateGroupingBounds();
                ChartInstance.RangeAxisModule.ProcessYAxis(ChartInstance);
            }
        }

        internal void DataManagerSuccess(IEnumerable<object> dataSource, RangeNavigatorSeries series = null)
        {
            RecordsCount = dataSource != null ? dataSource.Count() : 0;
            if (series != null)
            {
                series.RecordsCount = RecordsCount;
            }

            if (RecordsCount != 0)
            {
                ProcessData(dataSource, RecordsCount, series);
            }
        }

        internal void ProcessData(IEnumerable<object> currentData, int len, RangeNavigatorSeries series = null)
        {
            string xname = series?.XName ?? ChartInstance.XName;
            string yname = series?.YName ?? ChartInstance.YName;
            Points = new DataPoint[len].ToList();
            XData = new double[len].ToList();
            YData = new double[len].ToList();
            object[] currentViewData = currentData.ToArray();
            Type type = currentViewData.First().GetType();
            for (int i = 0; i < len; i++)
            {
                DataPoint point = RNDataPoint(type, i, currentViewData[i], xname, yname);
                point.YValue = Convert.ToDouble(point.Y, null);
                point.Index = i;
                point.Visible = true;
                point.XValue = ChartInstance.ValueType == RangeValueType.DateTime ? point.XValue = ChartHelper.GetTime(Convert.ToDateTime(point.X, null)) : point.X.GetType() == typeof(string) ? i : Convert.ToDouble(point.X, null);
                if (series != null)
                {
                    series.Points.Add(point);
                }

                XMin = Math.Min(XMin, point.XValue);
                YMin = Math.Min(YMin, point.YValue);
                XMax = Math.Max(XMax, point.XValue);
                YMax = Math.Max(YMax, point.YValue);
                ChartInstance.RangeSliderModule.Points.Add(point);
            }
        }

        private DataPoint RNDataPoint(Type type, int i, object currentViewData, string xname, string yname)
        {
            Points[i] = new DataPoint();
            if (type.Name.Equals("JObject", comparison))
            {
                JObject jsonObject = (JObject)currentViewData;
                Points[i].X = jsonObject.GetValue(xname, comparison);
                Points[i].Y = jsonObject.GetValue(yname, comparison);
            }
            else if (type.Name.Equals("ExpandoObject", comparison))
            {
                foreach (KeyValuePair<string, object> property in (IDictionary<string, object>)currentViewData)
                {
                    if (xname.Equals(property.Key, comparison))
                    {
                        Points[i].X = property.Value;
                    }

                    if (yname.Equals(property.Key, comparison))
                    {
                        Points[i].Y = property.Value;
                    }
                }
            }
            else if (type.BaseType.Equals(typeof(DynamicObject)))
            {
                Points[i].X = ChartHelper.GetDynamicMember(currentViewData, xname);
                Points[i].Y = ChartHelper.GetDynamicMember(currentViewData, yname);
            }
            else
            {
                PropertyInfo xnameProp = type.GetProperty(xname), ynameProp = type.GetProperty(yname);
                if (xnameProp == null || ynameProp == null)
                {
                    Points = null;
                }
                else
                {
                    GetObjectValue(currentViewData, Points[i], xnameProp, ynameProp);
                }
            }

            return Points[i];
        }

        internal void RenderSeries(RenderTreeBuilder builder)
        {
            VisiblePoints = RangeNavigatorHelper.GetVisiblePoints(this);
            RangeNavigatorLineSeries lineSeries = new RangeNavigatorLineSeries();
            RNAreaSeries areaSeries = new RNAreaSeries();
            RangeStepLineSeries stepLineSeries = new RangeStepLineSeries();
            if (RecordsCount != 0)
            {
                CreateSeriesElements(builder);
                if (Type == RangeNavigatorType.Line)
                {
                    lineSeries.Render(builder, this, false);
                }
                else if (Type == RangeNavigatorType.Area)
                {
                    areaSeries.Render(builder, this, false);
                }
                else
                {
                    stepLineSeries.Render(builder, this, XAxisRenderer, YAxisRenderer, false);
                }

                ChartInstance.SvgRenderer.RenderRect(builder, ChartInstance.Id + "_SeriesBorder", ChartInstance.InitialClipRect.X, ChartInstance.InitialClipRect.Y, ClipRect.Width, ClipRect.Height, ChartInstance.NavigatorBorder.Width, ChartInstance.NavigatorBorder.Color ?? "#DDDDDD", "transparent");
            }
        }

        internal void CreateSeriesElements(RenderTreeBuilder builder)
        {
            string seriesClipRectId = ChartInstance.Id + Constants.SERIESCLIPRECTID + Index;
            ChartInstance.SvgRenderer.OpenGroupElement(builder, ChartInstance.Id + "SeriesGroup" + Index, "translate(" + ClipRect.X.ToString(culture) + ',' + ClipRect.Y.ToString(culture) + ")", "url(#" + seriesClipRectId + ")");
            ChartInstance.SvgRenderer.RenderClipPath(builder, seriesClipRectId, new Rect { X = 0, Y = 0, Width = ClipRect.Width, Height = ClipRect.Height });
        }

        internal override void ComponentDispose()
        {
            Parent?.Series?.Remove(this);
            ChartInstance?.VisibleSeries?.Remove(this);
            Parent = null;
            ChartInstance = null;
            ChildContent = null;
            Animation = null;
            Border = null;
            XAxisRenderer = null;
            YAxisRenderer = null;
            XData = null;
            YData = null;
            Points = null;
            CurrentViewData = null;
            VisiblePoints = null;
            ClipRect = null;
        }
    }
}