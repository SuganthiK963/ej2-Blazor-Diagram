using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    /// <summary>
    /// Specifies the range navigator step line series.
    /// </summary>
    public class RangeStepLineSeries : RangeNavigatorLineBase
    {
        internal void Render(RenderTreeBuilder builder, RangeNavigatorSeries series, ChartAxisRenderer xaxis, ChartAxisRenderer yaxis, bool isInverted)
        {
            string direction = string.Empty, startPoint = "M";
            DataPoint prevPoint = null;
            ChartInternalLocation point1, point2;
            List<DataPoint> visiblePoints = EnableComplexProperty(series);
            foreach (DataPoint point in visiblePoints)
            {
                if (point.Visible && RangeNavigatorHelper.WithInRange(point.Index - 1 > -1 && point.Index - 1 < visiblePoints.Count ? visiblePoints[point.Index - 1] : null, point, point.Index + 1 < visiblePoints.Count ? visiblePoints[point.Index + 1] : null, series))
                {
                    if (prevPoint != null)
                    {
                        point2 = RangeNavigatorHelper.GetPoint(point.XValue, point.YValue, xaxis, yaxis, isInverted);
                        point1 = RangeNavigatorHelper.GetPoint(prevPoint.XValue, prevPoint.YValue, xaxis, yaxis, isInverted);
                        direction = string.Concat(direction, startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE + "L" + SPACE + point2.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE + "L" + SPACE + point2.X.ToString(Culture) + SPACE + point2.Y.ToString(Culture) + SPACE);
                        startPoint = "L";
                    }
                    else
                    {
                        point1 = RangeNavigatorHelper.GetPoint(point.XValue, point.YValue, xaxis, yaxis, isInverted);
                        direction = string.Concat(direction, startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
                        startPoint = "L";
                    }

                    prevPoint = point;
                }
                else
                {
                    prevPoint = null;
                    startPoint = "M";
                }
            }

            if (visiblePoints.Count > 0)
            {
                point1 = RangeNavigatorHelper.GetPoint(visiblePoints[visiblePoints.Count - 1].XValue, visiblePoints[visiblePoints.Count - 1].YValue, xaxis, yaxis, isInverted);
                direction = string.Concat(direction, startPoint + SPACE + point1.X.ToString(Culture) + SPACE + point1.Y.ToString(Culture) + SPACE);
            }

            AppendLinePath(builder, new PathOptions(series.ChartInstance.Id + "_Series_" + series.Index, direction, series.DashArray, series.Width, series.Interior, series.Opacity, Constants.TRANSPARENT), series);
            builder?.CloseElement();
        }
    }
}