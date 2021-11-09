using Microsoft.AspNetCore.Components.Rendering;
using System.Text;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.RangeNavigator.Internal;
using System;

namespace Syncfusion.Blazor.Charts.Chart.Internal
{
    internal class RangeNavigatorLineSeries : RangeNavigatorLineBase
    {
        internal void Render(RenderTreeBuilder builder, RangeNavigatorSeries series, bool isInverted)
        {
            StringBuilder direction = new StringBuilder();
            DataPoint prevPoint = null;
            string startPoint = "M";
            List<DataPoint> visiblePoints = EnableComplexProperty(series);
            int count = visiblePoints.Count;
            foreach (DataPoint point in visiblePoints)
            {
                if (point.Visible && RangeNavigatorHelper.WithInRange(((point.Index - 1) < 0 || (point.Index - 1) >= count) ? null : visiblePoints[point.Index - 1], point, point.Index + 1 >= count ? null : visiblePoints[point.Index + 1], series))
                {
                    direction.Append(GetLineDirection(prevPoint, point, series, isInverted, startPoint));
                    startPoint = prevPoint != null ? "L" : startPoint;
                    prevPoint = point;
                }
                else
                {
                    prevPoint = null;
                    startPoint = "M";
                }
            }

            series.ChartInstance.SvgRenderer.RenderPath(builder, new PathOptions(series.ChartInstance.Id + "_Series_" + (series.Index == default ? series.Category.ToString() : series.Index.ToString(Culture)), Convert.ToString(direction, Culture), series.DashArray, series.Width, series.Interior, series.Opacity));
            builder.CloseElement();
        }
    }
}