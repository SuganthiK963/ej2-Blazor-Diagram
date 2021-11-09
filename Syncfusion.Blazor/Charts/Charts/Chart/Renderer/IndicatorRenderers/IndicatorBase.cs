using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class IndicatorBase
    {
        internal ChartIndicator Indicator { get; set; }

        internal List<FinancialPoint> Points { get; set; }

        internal int Index { get; set; }

        internal SfChart Chart { get; set; }

        internal ChartAxis XAxis { get; set; }

        private static Type GetSeriesRendererType(ChartSeriesType type)
        {
            switch (type)
            {
                case ChartSeriesType.Line:
                    return typeof(IndicatorLineSeriesRenderer);
                case ChartSeriesType.RangeArea:
                    return typeof(IndicatorRangeAreaSeriesRenderer);
                case ChartSeriesType.Column:
                    return typeof(IndicatorColumnSeriesRenderer);
            }

            return null;
        }

        private static bool FindVisibility(FinancialPoint point)
        {
            return point.X == null || point.Low == null || double.IsNaN(Convert.ToDouble(point.Low, null)) || point.Open == null || double.IsNaN(Convert.ToDouble(point.Open, null)) || point.Close == null || double.IsNaN(Convert.ToDouble(point.Close, null)) || point.High == null || double.IsNaN(Convert.ToDouble(point.High, null));
        }

        private static void SetEmptyPoint(FinancialPoint point)
        {
            if (!FindVisibility(point))
            {
                point.Visible = true;
                return;
            }

            point.IsEmpty = true;
            point.Visible = false;
        }

        internal virtual void InitSeriesCollection()
        {
            Indicator.TargetSeries = new List<ChartSeries>();
            SetSeriesProperties(new ChartSeries(), Indicator.Type.ToString(), Indicator.Fill, Indicator.Width);
        }

        protected void SetSeriesProperties(ChartSeries series, string name, string fill, double width)
        {
            if (series == null)
            {
                return;
            }

#pragma warning disable BL0005
            series.Name = name != null && name.Length <= 4 ? name.ToUpper(CultureInfo.InvariantCulture) : name;
            series.XName = Indicator.XName;
            series.YName = "Y";
            series.Fill = !string.IsNullOrEmpty(fill) ? fill : "#606eff";
            series.DashArray = Indicator.DashArray;
            series.Width = width;
            if (Indicator.XAxisName != null)
            {
               series.XAxisName = Indicator.XAxisName;
            }

            series.Animation.Enable = Indicator.Animation.Enable;
            series.Animation.Duration = Indicator.Animation.Duration;
            series.Animation.Delay = Indicator.Animation.Delay;
            if (Indicator.YAxisName != null)
            {
              series.YAxisName = Indicator.YAxisName;
            }

            series.Container = Chart;
            series.EnableTooltip = true;
            series.RendererKey += Indicator.RendererKey;
            series.RendererType = GetSeriesRendererType(series.Type);
#pragma warning restore BL0005
            Indicator.TargetSeries.Add(series);
        }

        internal void InitiateAxis()
        {
            ChartAxisRenderer x_AxisRenderer, y_AxisRenderer;
            string x_AxisName = Indicator.XAxisName != null ? Indicator.XAxisName : Constants.PRIMARYXAXIS;
            string y_AxisName = Indicator.YAxisName != null ? Indicator.YAxisName : Constants.PRIMARYYAXIS;
            XAxis = Chart.AxisContainer.Axes[x_AxisName];
            x_AxisRenderer = XAxis.Renderer;
            x_AxisRenderer.Orientation = (!Chart.RequireInvertedAxis) ? Orientation.Horizontal : Orientation.Vertical;
            y_AxisRenderer = Chart.AxisContainer.Axes[y_AxisName].Renderer;
            y_AxisRenderer.Orientation = (!Chart.RequireInvertedAxis) ? Orientation.Vertical : Orientation.Horizontal;
            foreach (ChartSeries series in Indicator.TargetSeries)
            {
                x_AxisRenderer.SeriesRenderer.Add(series.Renderer);
                y_AxisRenderer.SeriesRenderer.Add(series.Renderer);
                series.Renderer.XAxisRenderer = x_AxisRenderer;
                series.Renderer.YAxisRenderer = y_AxisRenderer;
                series.Renderer.InitSeriesRendererFields();
            }
        }

        internal virtual void InitDataSource()
        {
            if (Indicator.DataSource != null)
            {
                Points = new List<FinancialPoint>();
                ProcessJsonData((IEnumerable<object>)Indicator.DataSource);
            }
            else
            {
                foreach (ChartSeriesRenderer renderer in Chart.SeriesContainer.Renderers)
                {
                    if (Indicator.SeriesName == renderer.Series.Name)
                    {
                        Points = renderer.Points.Cast<FinancialPoint>().ToList();
                    }
                }
            }
        }

        protected Point GetDataPoint(object x, object y, double pointX, ChartSeries series, int index)
        {
            if (series == null)
            {
                return null;
            }

            Point point = new Point()
            {
                X = x,
                Y = y,
                XValue = pointX,
                Interior = series.Fill,
                Index = index,
                YValue = Convert.ToDouble(y, null),
                Visible = true
            };
            series.Renderer.XMin = Math.Min(series.Renderer.XMin, point.XValue);
            series.Renderer.YMin = Math.Min(series.Renderer.YMin, point.YValue);
            series.Renderer.XMax = Math.Max(series.Renderer.XMax, point.XValue);
            series.Renderer.YMax = Math.Max(series.Renderer.YMax, point.YValue);
            series.Renderer.XData.Add(point.XValue);
            if (Indicator != null && Indicator.Type == TechnicalIndicators.Macd && series.Type == ChartSeriesType.Column)
            {
                if (Convert.ToDouble(point.Y, null) >= 0)
                {
                    point.Interior = Indicator.MacdPositiveColor;
                }
                else
                {
                    point.Interior = Indicator.MacdNegativeColor;
                }
            }

            return point;
        }

        protected virtual void SetSeriesRange(List<Point> points, ChartSeries series)
        {
            if (series != null)
            {
                series.Renderer.Points = points;
            }
        }

        private void ProcessJsonData(IEnumerable<object> currentViewData)
        {
            SeriesRenderEventArgs eventArgs = new SeriesRenderEventArgs("OnSeriesRender", false, string.Empty, currentViewData, Indicator.TargetSeries[0]);
            SfChart.InvokeEvent(Chart.ChartEvents?.OnSeriesRender, eventArgs);
            currentViewData = eventArgs.Data;
            int len = currentViewData.Count();
            if (len == 0)
            {
                return;
            }

            Type productDataType = currentViewData.First().GetType();
            PropertyAccessor x = new PropertyAccessor(productDataType.GetProperty(Indicator.XName));
            PropertyAccessor high = new PropertyAccessor(productDataType.GetProperty(Indicator.High));
            PropertyAccessor low = new PropertyAccessor(productDataType.GetProperty(Indicator.Low));
            PropertyAccessor open = new PropertyAccessor(productDataType.GetProperty(Indicator.Open));
            PropertyAccessor close = new PropertyAccessor(productDataType.GetProperty(Indicator.Close));
            PropertyAccessor volume = new PropertyAccessor(productDataType.GetProperty(Indicator.Volume));
            int index = 0;
            FinancialPoint point;
            foreach (object data in currentViewData)
            {
                point = new FinancialPoint()
                {
                    X = x.GetValue(data),
                    High = high.GetValue(data),
                    Low = low.GetValue(data),
                    Open = open.GetValue(data),
                    Close = close.GetValue(data),
                    Volume = volume.GetValue(data)
                };
                if (XAxis.ValueType == ValueType.Category)
                {
                    PushCategoryData(point, index, point.X.ToString());
                }
                else if (XAxis.ValueType == ValueType.DateTime || XAxis.ValueType == ValueType.DateTimeCategory)
                {
                    if (XAxis.ValueType == ValueType.DateTime)
                    {
                        point.XValue = ChartHelper.GetTime(Convert.ToDateTime(point.X, null));
                    }
                    else
                    {
                        PushCategoryData(point, index, ChartHelper.GetTime(Convert.ToDateTime(point.X, null)).ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    point.XValue = Convert.ToDouble(point.X, null);
                }

                point.Index = index;
                Points.Add(point);
                SetEmptyPoint(point);
                index++;
            }
        }

        private void PushCategoryData(Point point, int index, string pointX)
        {
            if (Indicator.Visible)
            {
                if (!XAxis.IsIndexed)
                {
                    if (XAxis.Renderer.Labels.IndexOf(pointX) < 0)
                    {
                        XAxis.Renderer.Labels.Add(pointX);
                    }

                    point.XValue = XAxis.Renderer.Labels.IndexOf(pointX);
                }
                else
                {
                    if (!string.IsNullOrEmpty(XAxis.Renderer.Labels[index]))
                    {
                        XAxis.Renderer.Labels.Add(XAxis.Renderer.Labels[index] + ", " + pointX);
                    }
                    else
                    {
                        XAxis.Renderer.Labels.Add(pointX);
                    }

                    point.XValue = index;
                }
            }
        }
    }
}