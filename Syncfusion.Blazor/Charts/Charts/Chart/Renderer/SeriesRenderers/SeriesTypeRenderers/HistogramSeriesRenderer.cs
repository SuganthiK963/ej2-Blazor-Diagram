using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Linq;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class HistogramSeriesRenderer : ColumnSeriesRenderer
    {
        protected List<PathOptions> HistogramPathOptions { get; set; }

        private static void CalculateBinInterval(List<double> y_Values, ChartSeries series)
        {
            double mean = y_Values.Sum() / y_Values.Count, sumValue = 0;
            foreach (double y_Value in y_Values)
            {
                sumValue += (y_Value - mean) * (y_Value - mean);
            }

            series.Renderer.HistogramValues.Mean = mean;
            series.Renderer.HistogramValues.SDValue = Math.Round(Math.Sqrt((sumValue / y_Values.Count) - 1));
            series.Renderer.HistogramValues.BinWidth = !double.IsNaN(series.BinInterval) ? series.BinInterval :
                Math.Round((3.5 * series.Renderer.HistogramValues.SDValue) / Math.Pow(y_Values.Count, 0.3333));
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            HistogramPathOptions = new List<PathOptions>();
            CalculateHistogramPathOption();
            Animate();
        }

        internal override void UpdateDirection()
        {
            HistogramPathOptions.Clear();
            CalculateHistogramPathOption();
            base.UpdateDirection();
        }

        private void CalculateHistogramPathOption()
        {
            if (Series.ShowNormalDistribution)
            {
                ChartInternalLocation pointLocation;
                string direction = string.Empty, startPoint = "M";
                PathOptions option;
                string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
                string id = Series.Container.ID + "_Series_" + Index + "_NDLine";
                double mean = HistogramValues.Mean;
                double sd_Value = HistogramValues.SDValue;
                double del = (XAxisRenderer.ActualRange.End - XAxisRenderer.ActualRange.Start) / 499;
                for (int i = 0; i < 500; i++)
                {
                    double x_Value = XAxisRenderer.ActualRange.Start + (i * del);
                    double y_Value = Math.Exp(-(x_Value - mean) * (x_Value - mean) / (2 * sd_Value * sd_Value)) / (sd_Value * Math.Sqrt(2 * Math.PI));
                    pointLocation = ChartHelper.GetPoint(x_Value, y_Value * HistogramValues.BinWidth * HistogramValues.YValues.Count, XAxisRenderer, YAxisRenderer, Series.Container.RequireInvertedAxis);
                    direction += startPoint + SPACE + pointLocation.X.ToString(Culture) + SPACE + pointLocation.Y.ToString(Culture) + SPACE;
                    startPoint = "L";
                }

                if (direction != null)
                {
                    option = new PathOptions(id, direction, Series.DashArray, 2, Series.Container.ChartThemeStyle.ErrorBar, Series.Opacity, Constants.TRANSPARENT, string.Empty, string.Empty, AccessText);
                    option.Visibility = "visible";
                    HistogramPathOptions.Add(option);
                }
            }
        }

        internal override void ProcessData()
            {
                Point point = new Point();
                SeriesRenderEventArgs eventArgs = new SeriesRenderEventArgs("OnSeriesRender", false, Interior, CurrentViewData, Series);
                SfChart.InvokeEvent(Owner.ChartEvents?.OnSeriesRender, eventArgs);
                CurrentViewData = eventArgs.Data;
                Interior = eventArgs.Fill;
                ProcessInternalData(out Type datasourceType, out IEnumerable<object> data);
                int length = data.Count();
                XData = new double[length].ToList();
                IEnumerable<object> histogramData = data.ToArray();
                string dataType = DataVizCommonHelper.FindDataType(datasourceType);
                switch (dataType)
                {
                    case Constants.JOBJECT:
                        ProcessJObjectData(datasourceType, "X", "Y", histogramData);
                        break;
                    case Constants.EXPANDOOBJECT:
                        ProcessExpandoObjectData(datasourceType, "X", "Y", histogramData);
                        break;
                    case Constants.DYNAMICOBJECT:
                        ProcessDynamicObjectData(datasourceType, "X", "Y", histogramData);
                        break;
                    default:
                        ProcessObjectData(datasourceType, "X", "Y", histogramData);
                        break;
                }
        }

        internal void ProcessInternalData(out Type type, out IEnumerable<object> currentViewData)
        {
            type = CurrentViewData.ToArray().First().GetType();
            object[] data = CurrentViewData.ToArray();
            List<SymbolLocation> updatedData = new List<SymbolLocation>();
            List<double> y_Values = new List<double>();
            for (int i = 0; i < data.Length; i++)
            {
               y_Values.Add(Convert.ToDouble(type.GetProperty(Series.YName).GetValue(data[i]), Culture));
            }

            HistogramValues = new HistogramValues
            {
                YValues = y_Values
            };
            double min = HistogramValues.YValues.Min();
            CalculateBinInterval(HistogramValues.YValues, Series);
            double binWidth = HistogramValues.BinWidth;
            for (int j = 0; j < data.Length;)
            {
                int y_Count = y_Values.Where(y => y >= min && y < (min + binWidth)).ToArray().Length;
                updatedData.Add(new SymbolLocation { X = min + (binWidth / 2), Y = y_Count });
                min = min + binWidth;
                j += y_Count;
            }

            currentViewData = updatedData.ToArray();
            type = currentViewData.ToArray().First().GetType();
        }

        internal override void CalculateAverageValue(Point point, int i, Type type)
        {
            point.Y = point.YValue = YData[i] = GetAverage(type, "Y", i);
            point.Visible = true;
        }

        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            HistogramPathOptions.ForEach(option => option.Visibility = visibility);
            switch (property)
            {
                case "Fill":
                    Interior = Series.Fill;
                    HistogramPathOptions.ForEach(option => option.Fill = Series.Fill);
                    break;
                case "DashArray":
                    HistogramPathOptions.ForEach(option => option.StrokeDashArray = Series.DashArray);
                    break;
                case "Width":
                    HistogramPathOptions.ForEach(option => option.StrokeWidth = Series.Border.Width);
                    break;
                case "Color":
                    HistogramPathOptions.ForEach(option => option.Stroke = Series.Border.Color);
                    break;
                case "Opacity":
                    HistogramPathOptions.ForEach(option => option.Opacity = Series.Opacity);
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
            foreach (PathOptions option in HistogramPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
    }
}