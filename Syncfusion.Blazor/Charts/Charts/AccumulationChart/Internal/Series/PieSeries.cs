using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    internal class PieSeries : PieBase
    {
        private Timer timer { get; set; } = new Timer();

        internal List<PieLegendAnimation> LegendAnimateOptions { get; set; } = new List<PieLegendAnimation>();

        private const string SPACE = " ";

        internal PieSeries(SfAccumulationChart chart)
            : base(chart)
        {
        }

        internal void RenderPoint(AccumulationPoints point, AccumulationChartSeries series, PathOptions options, RenderTreeBuilder treeBuilder)
        {
            point.StartAngle = StartAngle;
            double yValue = point.Visible ? (double)point.Y : 0;
            double totalAngle = (TotalAngle == 360) ? TotalAngle - 0.001 : TotalAngle;
            double degree = series.SumOfPoints != 0 ? ((Math.Abs(yValue) / series.SumOfPoints) * totalAngle) : double.NaN;
            double start = Math.PI / 180 * ((90 - (360 - StartAngle)) - 90);
            Radius = IsRadiusMapped ? DataVizCommonHelper.StringToNumber(point.SliceRadius, SeriesRadius) : Radius;
            options.Direction = GetPathOption(point, degree);
            point.MidAngle = (StartAngle - (double.IsNaN(degree) ? 0 : degree * 0.5)) % 360;
            point.EndAngle = StartAngle % 360;
            point.SymbolLocation = ChartHelper.DegreeToLocation(point.MidAngle, (Radius + InnerRadius) * 0.5, CenterLoc);
            string preDirection = options.Direction;
            PieLegendAnimation existElement = LegendAnimateOptions.Find(Item => Item.PathOption.Id == options.Id);
            if (existElement != null && AccChart.LegendClickRedraw)
            {
                preDirection = point.Visible ? existElement.PathOption.Direction : string.Empty;
                existElement.Degree = degree;
                existElement.Point = new AccumulationPoints() { X = point.X, Y = point.Y, Percentage = double.IsNaN(point.Percentage) ? 0 : point.Percentage, Start = point.Start, Degree = point.Degree, Visible = point.Visible };
                existElement.Start = start;
                existElement.PathOption = new PathOptions(options.Id, options.Direction, options.StrokeDashArray, options.StrokeWidth, options.Stroke, options.Opacity, options.Fill);
                existElement.Radius = Radius;
                existElement.InnerRadius = InnerRadius;
            }
            else
            {
                LegendAnimateOptions.Add(new PieLegendAnimation()
                {
                    Degree = degree,
                    Point = new AccumulationPoints() { X = point.X, Y = point.Y, Percentage = point.Percentage, Start = point.Start, Degree = point.Degree, Visible = point.Visible },
                    Start = start,
                    PathOption = new PathOptions(options.Id, options.Direction, options.StrokeDashArray, options.StrokeWidth, options.Stroke, options.Opacity, options.Fill),
                    Radius = Radius,
                    InnerRadius = InnerRadius
                });
            }

            if (!string.IsNullOrEmpty(preDirection))
            {
                options.Direction = preDirection;
            }

            AccumulationChartInstance.Rendering.RenderPath(treeBuilder, options);
            if (point == series.Points.Last())
            {
                AccumulationChartInstance.Rendering.RenderPath(treeBuilder, new PathOptions() { Id = AccumulationChartInstance.ID + "PointHoverBorder", Opacity = 0 });
            }

            point.Degree = degree;
            point.Start = start;
        }

        private static string GetPathFromArc(ChartInternalLocation center, double start, double end, double radius, double innerRadius)
        {
            double degree = end - start;
            degree = degree < 0 ? degree + 360 : degree;
            int flag = degree < 180 ? 0 : 1;
            if (innerRadius == 0)
            {
                return GetPiePath(center, ChartHelper.DegreeToLocation(start, radius, center), ChartHelper.DegreeToLocation(end, radius, center), radius, flag);
            }
            else
            {
                return GetDoughnutPath(ChartHelper.DegreeToLocation(start, radius, center), ChartHelper.DegreeToLocation(end, radius, center), radius,
                    ChartHelper.DegreeToLocation(start, innerRadius, center), ChartHelper.DegreeToLocation(end, innerRadius, center), innerRadius, flag);
            }
        }

        private static string GetPiePath(ChartInternalLocation center, ChartInternalLocation start, ChartInternalLocation end, double radius, int clockWise)
        {
            return "M " + center.X.ToString(culture) + SPACE + center.Y.ToString(culture) + " L " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " A " + radius.ToString(culture) + SPACE + radius.ToString(culture) + " 0 " + clockWise + " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture) + " Z";
        }

        private static string GetDoughnutPath(ChartInternalLocation start, ChartInternalLocation end, double radius, ChartInternalLocation innerStart, ChartInternalLocation innerEnd, double InnerRadius, int clockWise)
        {
            return "M " + start.X.ToString(culture) + SPACE + start.Y.ToString(culture) + " A " + radius.ToString(culture) + SPACE + radius.ToString(culture) + " 0 " + clockWise + " 1 " + end.X.ToString(culture) + SPACE + end.Y.ToString(culture) + " L " + innerEnd.X.ToString(culture) + SPACE + innerEnd.Y.ToString(culture) + " A " + InnerRadius.ToString(culture) + SPACE + InnerRadius.ToString(culture) + " 0 " + clockWise + ",0 " + innerStart.X.ToString(culture) + SPACE + innerStart.Y.ToString(culture) + " Z";
        }

        private string GetPathOption(AccumulationPoints point, double degree)
        {
            if (double.IsNaN(degree) || degree == 0)
            {
                return string.Empty;
            }

            string path = GetPathFromArc(CenterLoc, StartAngle % 360, (StartAngle + degree) % 360, IsRadiusMapped ? DataVizCommonHelper.StringToNumber(point.SliceRadius, SeriesRadius) : Radius, InnerRadius);
            StartAngle += degree;
            return path;
        }

        internal void DrawHoverBorder(ChartInternalMouseEventArgs args)
        {
            double borderRadius = InnerRadius == 0 ? Radius + 3 : InnerRadius - 3;
            double borderInnerRadius = InnerRadius == 0 ? borderRadius + 2 : borderRadius - 2;
            ToggleInnerPoint(args, borderRadius, borderInnerRadius);
        }

        private async void ToggleInnerPoint(ChartInternalMouseEventArgs args, double radius, double innerRadius)
        {
            int seriesIndex = -1, pointIndex = -1;
            string target = args.Target.Replace(AccumulationChartInstance.ID, string.Empty, StringComparison.InvariantCulture);
            if (args.Target.Contains("_Point_", StringComparison.InvariantCulture))
            {
                seriesIndex = int.Parse(target.Split('_')[2], AccumulationChartInstance.NumberFormatter);
                pointIndex = Convert.ToInt32(target.Split('_').Last(), AccumulationChartInstance.NumberFormatter);
            }

            HoverBorderElement = AccumulationChartInstance.Rendering.PathElementList.Find(item => item.Id == AccumulationChartInstance.ID + "PointHoverBorder");
            if (seriesIndex > -1 && pointIndex > -1 && HoverBorderElement != null)
            {
                AccumulationPoints point = AccumulationChartInstance.VisibleSeries.First().Points[pointIndex];
                SvgPath pointElement = AccumulationChartInstance.Rendering.PathElementList.Find(item => item.Id == AccumulationChartInstance.ID + "_Series_0_Point_" + point.Index);
                pointElement.ChangeClass(pointElement?.Class?.Trim(), null, true);
                double opacity = pointElement.Class == AccumulationChartInstance.ID + "_blazor_deselected" ? AccumulationChartInstance.Tooltip.Enable ? 0.5 : 0.3 : AccumulationChartInstance.Tooltip.Enable ? 0.5 : AccumulationChartInstance.VisibleSeries[0].Opacity;
                string innerPie = GetPathFromArc(AccumulationChartInstance.PieSeriesModule.CenterLoc, point.StartAngle % 360, point.EndAngle % 360, radius, innerRadius);
                if (!point.IsExplode)
                {
                    HoverBorderElement.ChangePathAttributes(innerPie, point.Color, point.Color, opacity);
                    if (AccumulationChartInstance.AccumulationSelectionModule == null && !string.IsNullOrEmpty(pointElement.Class?.Trim()))
                    {
                        pointElement.ChangeClass(string.Empty, null, true);
                        AccumulationChartInstance.AccumulationSelectionModule.AddSvgClass(HoverBorderElement, pointElement.Class);
                    }
                }
                else if (point.IsExplode && args.Type != "click")
                {
#pragma warning disable CA2007
                    string transform = await AccChart.InvokeMethod<string>(AccumulationChartConstants.GETATTRIBUTE, false, new object[] { AccChart.ID + "_Series_" + seriesIndex + "_Point_" + pointIndex, "transform" });
#pragma warning restore CA2007
                    HoverBorderElement.ChangePathAttributes(innerPie, point.Color, point.Color, opacity, transform);
                }
            }
            else
            {
                RemoveHoverBorder(1000);
            }
        }

        internal void AnimateSeries(RenderTreeBuilder builder)
        {
            AccChart.Rendering.OpenClipPath(builder, AccChart.ID + "_Series_" + "0" + "_clippath");
            AccChart.Rendering.RenderPath(builder, AccChart.ID + "_Series_" + "0_slice", string.Empty, string.Empty, 1, "transparent");
            builder.CloseElement();
        }

        internal void RemoveHoverBorder(double fadeOutDuration)
        {
            timer.Stop();
            timer.Interval = fadeOutDuration;
            timer.AutoReset = false;
            if (HoverBorderElement != null)
            {
                timer.Elapsed += OnTimeOut;
                timer.Start();
            }
        }

        private void OnTimeOut(object source, ElapsedEventArgs e)
        {
            HoverBorderElement?.ChangeDirection(string.Empty);
            (source as Timer).Stop();
        }

        internal override void Dispose()
        {
            base.Dispose();
            LegendAnimateOptions = null;
            timer = null;
        }
    }
}