using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class ParetoLineDataSouce
    {
        public object X { get; set; }

        public object Y { get; set; }
    }

    public class ParetoSeriesRenderer : ColumnSeriesRenderer
    {
        internal static List<ChartAxis> ParetoAxes { get; set; } = new List<ChartAxis>();

        protected override void RenderSeries()
        {
            base.RenderSeries();
            Animate();
        }

        public static List<object> PerformCumulativeCalculation(List<object> targetData, string x_Name, string y_Name)
        {
            if (targetData == null)
            {
                return null;
            }

            List<ParetoLineDataSouce> result = new List<ParetoLineDataSouce>();
            double sum = 0, count = 0;
            Type type = targetData.First().GetType();
            PropertyAccessor x = new PropertyAccessor(type.GetProperty(x_Name));
            PropertyAccessor y = new PropertyAccessor(type.GetProperty(y_Name));
            for (int i = 0; i < targetData.Count; i++)
            {
                sum += Convert.ToDouble(y.GetValue(targetData[i]), null);
            }

            for (int i = 0; i < targetData.Count; i++)
            {
                count += Convert.ToDouble(y.GetValue(targetData[i]), null);
                result.Add(new ParetoLineDataSouce { X = x.GetValue(targetData[i]), Y = Math.Round((count / sum) * 100) });
            }

            return result.Cast<object>().ToList();
        }

        internal override void ProcessData()
        {
            Point point = new Point();
            SeriesRenderEventArgs eventArgs = new SeriesRenderEventArgs("OnSeriesRender", false, Interior, CurrentViewData, Series);
            SfChart.InvokeEvent(Owner.ChartEvents?.OnSeriesRender, eventArgs);
            CurrentViewData = eventArgs.Data;
            Type datasourceType = CurrentViewData.First().GetType();
            CurrentViewData = ChartHelper.Sort(CurrentViewData.ToList(), Series.YName, datasourceType);
            Interior = eventArgs.Fill;
            int length = CurrentViewData.Count();
            XData = new double[length].ToList();
            string dataType = DataVizCommonHelper.FindDataType(datasourceType);
            string x_Name = Series.XName;
            string y_Name = Series.YName;
            switch (dataType)
            {
                case Constants.JOBJECT:
                    ProcessJObjectData(datasourceType, x_Name, y_Name, CurrentViewData);
                    break;
                case Constants.EXPANDOOBJECT:
                    ProcessExpandoObjectData(datasourceType, x_Name, y_Name, CurrentViewData);
                    break;
                case Constants.DYNAMICOBJECT:
                    ProcessDynamicObjectData(datasourceType, x_Name, y_Name, CurrentViewData);
                    break;
                default:
                    ProcessObjectData(datasourceType, x_Name, y_Name, CurrentViewData);
                    break;
            }

            ProcessInternalData();
        }

        internal virtual void ProcessInternalData()
        {
            List<object> paretoData = CurrentViewData.ToList();
            if (paretoData.Count == 0)
            {
                return;
            }

            ChartSeries paretoSeries = (Owner.SeriesContainer.Renderers[1] as ChartSeriesRenderer).Series;
            paretoSeries.CurrentViewData = PerformCumulativeCalculation(paretoData, Series.XName, Series.YName);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null)
            {
                return;
            }
        }
    }
}