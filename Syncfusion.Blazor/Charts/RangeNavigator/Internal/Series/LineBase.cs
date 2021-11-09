using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System.Runtime.InteropServices;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    /// <summary>
    /// Sets and gets the options for customizing the range navigator line series.
    /// </summary>
    public class RangeNavigatorLineBase
    {
        /// <summary>
        /// Specifies the space notation.
        /// </summary>
        protected const string SPACE = " ";

        internal RangeNavigatorLineBase([Optional] SfRangeNavigator chart)
        {
            Chart = chart;
        }

        internal SfRangeNavigator Chart { get; set; }

        /// <summary>
        /// Specifies the culture info.
        /// </summary>
        protected CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        internal static void AppendLinePath(RenderTreeBuilder builder, PathOptions options, RangeNavigatorSeries series)
        {
            series.ChartInstance.SvgRenderer.RenderPath(builder, options);
            series.IsRectSeries = false;
        }

        internal List<DataPoint> EnableComplexProperty(RangeNavigatorSeries series)
        {
            List<DataPoint> tempPoints = new List<DataPoint>();
            List<DataPoint> tempPoints2 = new List<DataPoint>();
            Rect areaBounds = series?.ClipRect;
            bool xvalid = series.Points.Count > 0 && IsValid(series.Points[0].X), yvalid = series.Points.Count > 0 && IsValid(series.Points[0].Y);
            double xtolerance = Math.Abs(series.XAxisRenderer.VisibleRange.Delta / areaBounds.Width), ytolerance = Math.Abs(series.YAxisRenderer.VisibleRange.Delta / areaBounds.Height),
            prevXValue = series.Points.Count != 0 && series.Points[0] != null && (xvalid ? (Convert.ToDouble(series.Points[0].X, null) > xtolerance) : xvalid) ? 0 : xtolerance,
            prevYValue = series.Points.Count != 0 && series.Points[0] != null && (yvalid ? (Convert.ToDouble(series.Points[0].Y, null) > xtolerance) : yvalid) ? 0 : ytolerance;
            foreach (DataPoint currentPoint in series.Points)
            {
                double xval = currentPoint.XValue != 0 ? currentPoint.XValue : series.XAxisRenderer.VisibleRange.Start,
                yval = currentPoint.YValue != 0 ? currentPoint.YValue : series.YAxisRenderer.VisibleRange.Start;
                if (Math.Abs(prevXValue - xval) >= xtolerance || Math.Abs(prevYValue - yval) >= ytolerance)
                {
                    tempPoints.Add(currentPoint);
                    prevXValue = xval;
                    prevYValue = yval;
                }
            }

            for (int i = 0; i < tempPoints.Count; i++)
            {
                if (tempPoints[i].X == null || string.IsNullOrEmpty(Convert.ToString(tempPoints[i].X, Culture)))
                {
                    continue;
                }
                else
                {
                    tempPoints2.Add(tempPoints[i]);
                }
            }

            return tempPoints2;
        }

        internal string GetAreaPathDirection(double xvalue, double yvalue, RangeNavigatorSeries series, bool isInverted, ChartInternalLocation startPoint, string startPath)
        {
            if (startPoint == null)
            {
                ChartInternalLocation firstPoint = RangeNavigatorHelper.GetPoint(xvalue, yvalue, series.XAxisRenderer, series.YAxisRenderer, isInverted);
                return startPath + SPACE + firstPoint.X.ToString(Culture) + SPACE + firstPoint.Y.ToString(Culture) + SPACE;
            }

            return string.Empty;
        }

        internal string GetLineDirection(in DataPoint firstPoint, in DataPoint secondPoint, RangeNavigatorSeries series, bool isInverted, string startPoint)
        {
            string direction = string.Empty;
            if (firstPoint != null)
            {
                ChartInternalLocation point1 = RangeNavigatorHelper.GetPoint(firstPoint.XValue, firstPoint.YValue, series?.XAxisRenderer, series.YAxisRenderer, isInverted);
                ChartInternalLocation point2 = RangeNavigatorHelper.GetPoint(secondPoint.XValue, secondPoint.YValue, series.XAxisRenderer, series.YAxisRenderer, isInverted);
                direction = startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE + 'L' + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE;
            }

            return direction;
        }

        private bool IsValid(object obj)
        {
            if (obj != null)
            {
                return double.TryParse(Convert.ToString(obj, Culture), out double x);
            }

            return false;
        }

        internal void Dispose()
        {
            Chart = null;
        }
    }
}