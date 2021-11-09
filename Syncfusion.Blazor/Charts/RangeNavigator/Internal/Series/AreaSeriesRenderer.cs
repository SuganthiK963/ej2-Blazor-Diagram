using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    /// <summary>
    /// Specifies the series of range navigator.
    /// </summary>
    public class RNAreaSeries : RangeNavigatorLineBase
    {
        internal void Render(RenderTreeBuilder builder, RangeNavigatorSeries series, bool isInverted)
        {
            ChartInternalLocation startPoint = null;
            double origin = Math.Max(series.YAxisRenderer.VisibleRange.Start, 0), currentXValue, borderWidth = series.Border != null ? series.Border.Width : 0;
            string direction = string.Empty, borderColor = series.Border != null ? series.Border.Color : Constants.TRANSPARENT;
            List<DataPoint> visiblePoints = EnableComplexProperty(series);
            DataPoint previousPoint, nextPoint, point;
            for (int i = 0; i < visiblePoints.Count; i++)
            {
                point = visiblePoints[i];
                currentXValue = point.XValue;
                previousPoint = i - 1 > -1 ? visiblePoints[i - 1] : null;
                nextPoint = i + 1 < visiblePoints.Count ? visiblePoints[i + 1] : null;
                if (point.Visible && RangeNavigatorHelper.WithInRange(previousPoint, point, nextPoint, series))
                {
                    direction += GetAreaPathDirection(currentXValue, origin, series, isInverted, startPoint, "M");
                    startPoint = startPoint != null ? startPoint : new ChartInternalLocation(currentXValue, origin);
                    direction += GetAreaPathDirection(currentXValue, point.YValue, series, isInverted, null, "L");
                }
            }

            AppendLinePath(builder, new PathOptions(series.ChartInstance.Id + "_Series_" + series.Index, (series.Points.Count > 1 && !string.IsNullOrEmpty(direction)) ? (direction + GetAreaPathDirection(series.Points[series.Points.Count - 1].XValue, origin, series, isInverted, null, "L")) : string.Empty, series.DashArray, borderWidth, borderColor, series.Opacity, series.Interior), series);
            builder.CloseElement();
        }
    }
}