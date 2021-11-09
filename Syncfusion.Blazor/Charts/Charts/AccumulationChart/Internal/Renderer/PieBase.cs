using Syncfusion.Blazor.Data;
using System;
using System.Linq;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class PieBase : AccumulationBase
    {
        internal SfAccumulationChart AccChart { get; set; }

        internal double StartAngle { get; set; }

        internal double TotalAngle { get; set; }

        internal double InnerRadius { get; set; }

        internal ChartInternalLocation CenterLoc { get; set; }

        internal new double Radius { get; set; }

        internal new double LabelRadius { get; set; }

        internal double SeriesRadius { get; set; }

        internal double Size { get; set; }

        internal bool IsRadiusMapped { get; set; }

        internal PieBase(SfAccumulationChart chart)
            : base(chart)
        {
        }

        internal void InitProperties(SfAccumulationChart chart, AccumulationChartSeries series)
        {
            AccChart = chart;
            Size = Math.Min(chart.InitialClipRect.Width, chart.InitialClipRect.Height);
            InitAngles(series);
            bool isRadiusValid = double.TryParse(series.Radius.Replace("%", string.Empty, StringComparison.Ordinal), out double x);
            double radius = string.IsNullOrEmpty(series.Radius) || !isRadiusValid ? double.NaN : Convert.ToDouble(series.Radius.Replace("%", string.Empty, StringComparison.Ordinal), chart.NumberFormatter);
            if ((series.Radius.Contains("%", StringComparison.Ordinal) || radius.GetType() == typeof(double)) && !double.IsNaN(radius))
            {
                IsRadiusMapped = false;
                Radius = DataVizCommonHelper.StringToNumber(series.Radius, Size * 0.5);
                InnerRadius = DataVizCommonHelper.StringToNumber(series.InnerRadius, Radius);
                LabelRadius = series.DataLabel.Position == AccumulationLabelPosition.Inside ? (((Radius - InnerRadius) / 2) + InnerRadius) :
                    (Radius + DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(series.DataLabel.ConnectorStyle.Length) ? series.DataLabel.ConnectorStyle.Length : "4%", Size / 2));
            }
            else if (series.Points.Count > 0)
            {
                double[] radiusCollection = new double[series.Points.Count];
                IsRadiusMapped = true;
                foreach (AccumulationPoints point in series.Points)
                {
                    if (point.SliceRadius.Contains("%", StringComparison.Ordinal))
                    {
                        radiusCollection[point.Index] = DataVizCommonHelper.StringToNumber(point.SliceRadius, Size * 0.5);
                    }
                    else
                    {
                        radiusCollection[point.Index] = double.Parse(point.SliceRadius, culture);
                    }
                }

                Radius = SeriesRadius = radiusCollection.Max();
                InnerRadius = DataVizCommonHelper.StringToNumber(series.InnerRadius, SeriesRadius);
                InnerRadius = InnerRadius > radiusCollection.Min() ? InnerRadius * 0.5 : InnerRadius;
            }

            chart.ExplodeDistance = series.Explode ? DataVizCommonHelper.StringToNumber(series.ExplodeOffset, Radius) : 0;
            FindCenter(chart, series);
            DefaultLabelBound(series, series.DataLabel.Visible, series.DataLabel.Position);
            TotalAngle -= 0.001;
        }

        private void InitAngles(AccumulationChartSeries series)
        {
            double endAngle = double.IsNaN(series.EndAngle) ? series.StartAngle : series.EndAngle;
            TotalAngle = (endAngle - series.StartAngle) % 360;
            StartAngle = series.StartAngle - 90;
            TotalAngle = TotalAngle <= 0 ? 360 + TotalAngle : TotalAngle;
            StartAngle = (StartAngle < 0 ? 360 + StartAngle : StartAngle) % 360;
        }

        private void FindCenter(SfAccumulationChart chart, AccumulationChartSeries series)
        {
            CenterLoc = new ChartInternalLocation(
                DataVizCommonHelper.StringToNumber(chart.Center.X, chart.InitialClipRect.Width) + chart.InitialClipRect.X,
                DataVizCommonHelper.StringToNumber(chart.Center.Y, chart.InitialClipRect.Height) + chart.InitialClipRect.Y);
            Rect accumulationRect = GetSeriesBounds(series);
            series.AccumulationBound = accumulationRect;
            ChartInternalLocation accRectCenter = new ChartInternalLocation(
                accumulationRect.X + (accumulationRect.Width * 0.5),
                accumulationRect.Y + (accumulationRect.Height * 0.5));
            CenterLoc.X += CenterLoc.X - accRectCenter.X;
            CenterLoc.Y += CenterLoc.Y - accRectCenter.Y;
            chart.Origin = CenterLoc;
        }

        private Rect GetSeriesBounds(AccumulationChartSeries series)
        {
            Rect result = new Rect(double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            InitAngles(series);
            double start = StartAngle;
            double total = TotalAngle;
            double end = (StartAngle + total) % 360;
            end = end == 0 ? 360 : end;
            series.FindMaxBounds(result, GetRectFromAngle(start));
            series.FindMaxBounds(result, GetRectFromAngle(end));
            series.FindMaxBounds(result, new Rect(CenterLoc.X, CenterLoc.Y, 0, 0));
            double nextQuadrant = ((Math.Floor(start / 90) * 90) + 90) % 360;
            double lastQuadrant = (Math.Floor(end / 90) * 90) % 360;
            lastQuadrant = lastQuadrant == 0 ? 360 : lastQuadrant;
            if (total >= 90 || lastQuadrant == nextQuadrant)
            {
                series.FindMaxBounds(result, GetRectFromAngle(nextQuadrant));
                series.FindMaxBounds(result, GetRectFromAngle(lastQuadrant));
            }

            if (start == 0 || (start + total) >= 360)
            {
                series.FindMaxBounds(result, GetRectFromAngle(0));
            }

            double length = nextQuadrant == lastQuadrant ? 0 : Math.Floor(total / 90);
            for (var i = 1; i < length; i++)
            {
                nextQuadrant = nextQuadrant + 90;
                if (nextQuadrant < lastQuadrant || end < start || total == 360)
                {
                    series.FindMaxBounds(result, GetRectFromAngle(nextQuadrant));
                }
            }

            result.Width -= result.X;
            result.Height -= result.Y;
            return result;
        }

        private Rect GetRectFromAngle(double angle)
        {
            ChartInternalLocation location = ChartHelper.DegreeToLocation(angle, Radius, CenterLoc);
            return new Rect(location.X, location.Y, 0, 0);
        }

        internal void DefaultLabelBound(AccumulationChartSeries series, bool visible, AccumulationLabelPosition labelPosition)
        {
            series.AccumulationBound = GetSeriesBounds(series);
            series.LabelBound = new Rect(
                series.AccumulationBound.X,
                series.AccumulationBound.Y,
                series.AccumulationBound.X + series.AccumulationBound.Width,
                series.AccumulationBound.Y + series.AccumulationBound.Height);
            if (visible && labelPosition == AccumulationLabelPosition.Outside)
            {
                series.LabelBound = new Rect(double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            }
        }

        internal double GetLabelRadius(AccumulationChartSeries series, AccumulationPoints point)
        {
            return series.DataLabel.Position == AccumulationLabelPosition.Inside ? ((DataVizCommonHelper.StringToNumber(point.SliceRadius, Radius) - InnerRadius) * 0.5) + InnerRadius :
                (DataVizCommonHelper.StringToNumber(point.SliceRadius, SeriesRadius) + DataVizCommonHelper.StringToNumber(string.IsNullOrEmpty(series.DataLabel.ConnectorStyle.Length) ? "4%" : series.DataLabel.ConnectorStyle.Length, Size * 0.5));
        }

        internal override void Dispose()
        {
            base.Dispose();
            AccChart = null;
            CenterLoc = null;
        }
    }
}