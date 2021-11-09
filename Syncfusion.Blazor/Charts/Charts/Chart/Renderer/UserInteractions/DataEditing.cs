using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class DataEditing
    {
        internal DataEditing(SfChart sfChart)
        {
            chart = sfChart;
        }

        private double dragY;
        private string previousCursor;

        private SfChart chart { get; set; }

        private int? seriesIndex { get; set; }

        private int? pointIndex { get; set; }

        internal bool IsPointDragging { get; set; }

        internal void PointMouseDown()
        {
            ChartData data = new ChartData(chart);
            PointData pointData = data.GetData();
            if (pointData.Point != null && (data.InsideRegion || !pointData.Series.Renderer.IsRectSeries()))
            {
                seriesIndex = pointData.Series.Renderer.Index;
                pointIndex = pointData.Point.Index;
                ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)chart.SeriesContainer.Renderers[(int)seriesIndex];
                if (seriesRenderer.Series.ChartDataEditSettings.Enable)
                {
                    if (chart.ChartEvents?.OnDataEdit != null)
                    {
                        chart.ChartEvents?.OnDataEdit.Invoke(new DataEditingEventArgs(
                           Constants.ONDATAEDIT,
                           seriesRenderer.Points[(int)pointIndex].YValue,
                           seriesRenderer.YData.Count > 0 ? seriesRenderer.YData[(int)pointIndex] : double.NaN,
                           pointData.Point,
                           (int)pointIndex,
                           pointData.Series,
                           (int)seriesIndex));
                    }

                    chart.IsPointMouseDown = true;
#pragma warning disable BL0005
                    chart.ZoomSettings.EnableDeferredZooming = false;
#pragma warning restore BL0005
                }
            }
        }

        internal void PointMouseMove()
        {
            ChartData data = new ChartData(chart);
            PointData pointData = data.GetData();
            string cursor;
            if (pointData.Series != null && pointData.Series.ChartDataEditSettings.Enable && pointData.Point != null && (data.InsideRegion || !pointData.Series.Renderer.IsRectSeries()))
            {
                cursor = GetCursorStyle(pointData);
            }
            else
            {
                cursor = "null";
            }

            if (previousCursor != cursor)
            {
                chart.SetSvgCursor(cursor);
                previousCursor = cursor;
            }

            if (chart.IsPointMouseDown)
            {
                ((ChartSeriesRenderer)chart.SeriesContainer.Renderers[(int)seriesIndex]).FindSplinePoint();
                PointDragging((int)seriesIndex, (int)pointIndex);
            }
        }

        private string GetCursorStyle(PointData pointData)
        {
            if (pointData.Series.Type == ChartSeriesType.Bar && chart.IsTransposed)
            {
                return "ns-resize";
            }
            else if (chart.IsTransposed || pointData.Series.Type == ChartSeriesType.Bar)
            {
                return "ew-resize";
            }
            else
            {
                return "ns-resize";
            }
        }

        internal void PointMouseUp()
        {
            if (chart.IsPointMouseDown)
            {
                ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)chart.SeriesContainer.Renderers[(int)seriesIndex];
                if (seriesRenderer.Series.ChartDataEditSettings.Enable)
                {
                    if (chart.ChartEvents?.OnDataEditCompleted != null)
                    {
                        chart.ChartEvents?.OnDataEditCompleted.Invoke(new DataEditingEventArgs(
                           Constants.ONDATAEDITCOMPLETED,
                           seriesRenderer.Points[(int)pointIndex].YValue,
                           seriesRenderer.YData.Count > 0 ? seriesRenderer.YData[(int)pointIndex] : double.NaN,
                           seriesRenderer.Points[(int)pointIndex],
                           (int)pointIndex,
                           seriesRenderer.Series,
                           (int)seriesIndex));
                    }

                    seriesRenderer.Points[(int)pointIndex].Y = seriesRenderer.Points[(int)pointIndex].YValue;
                    chart.IsPointMouseDown = false;
                    IsPointDragging = false;
                    seriesIndex = pointIndex = null;
                }
            }
        }

        private void PointDragging(int s_Index, int p_Index)
        {
            List<double> y_ValueArray = new List<double>();
            double y, y_Size;
            ChartSeriesRenderer seriesRenderer = (ChartSeriesRenderer)chart.SeriesContainer.Renderers[s_Index];
            ChartDataEditSettings pointDrag = seriesRenderer.Series.ChartDataEditSettings;
            ChartAxisRenderer y_Axis = seriesRenderer.YAxisRenderer;
            int extra = seriesRenderer.IsRectSeries() ? 1 : 0;
            Rect axis = ChartHelper.GetTransform(seriesRenderer.XAxisRenderer.Rect, y_Axis.Rect, chart.RequireInvertedAxis);
            if (seriesRenderer.Series.Type == ChartSeriesType.Bar)
            {
                y = chart.IsTransposed ? axis.Y + axis.Height - chart.MouseY : chart.MouseX - axis.X;
                y_Size = chart.IsTransposed ? axis.Height : axis.Width;
            }
            else
            {
                y = chart.IsTransposed ? chart.MouseX - axis.X : axis.Y + axis.Height - chart.MouseY;
                y_Size = chart.IsTransposed ? axis.Width : axis.Height;
            }

            double y_Value = y_Axis.Axis.IsInversed ? 1 - (y / y_Size) : y / y_Size;
            y_Value = (y_Value * y_Axis.VisibleRange.Delta) + y_Axis.VisibleRange.Start;
            double minRange = y_Axis.Axis.Minimum != null ? y_Axis.VisibleRange.Start + extra : (double.IsNaN(pointDrag.MinY) ? y_Value : pointDrag.MinY),
            maxRange = y_Axis.Axis.Maximum != null ? y_Axis.VisibleRange.End + extra : (double.IsNaN(pointDrag.MaxY) ? y_Value : pointDrag.MaxY);
            if (maxRange >= y_Value && minRange <= y_Value)
            {
#pragma warning disable CA1305
                dragY = y_Axis.Axis.ValueType == ValueType.Logarithmic ? Math.Pow(y_Axis.Axis.LogBase, y_Value) : Convert.ToDouble(y_Value.ToString("N2"), null);
#pragma warning restore CA1305
                seriesRenderer.Points[p_Index].YValue = dragY;
                seriesRenderer.Points[p_Index].Y = (double)dragY;
                seriesRenderer.Points[p_Index].Interior = pointDrag.Fill;
                for (int i = 0; i < seriesRenderer.Points.Count; i++)
                {
                    y_ValueArray.Add(seriesRenderer.Points[i].YValue);
                }

                seriesRenderer.YMin = y_ValueArray.Min();
                seriesRenderer.YMax = y_ValueArray.Max();
                IsPointDragging = true;
                chart.OnLayoutChange();
            }
        }

        internal void Dispose()
        {
            chart = null;
        }
    }
}