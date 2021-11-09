using System;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class AreaBaseSeriesRenderer : LineBaseSeriesRenderer
    {
        internal virtual void GetAreaPathDirection(double x, double y, ChartSeries series, bool isInverted, ChartInternalLocation startPoint, string startPath)
        {
            if (startPoint == null)
            {
                ChartInternalLocation firstPoint = ChartHelper.GetPoint(XAxisRenderer.GetPointValue(x), YAxisRenderer.GetPointValue(y), XAxisRenderer, YAxisRenderer, XLength, YLength, isInverted);
                Direction.Append(string.Join(string.Empty, startPath, SPACE, Convert.ToInt32(firstPoint.X).ToString(Culture), SPACE, Convert.ToInt32(firstPoint.Y).ToString(Culture), SPACE));
            }
        }

        internal virtual void GetPolarAreaPathDirection(double x, double y, ChartSeries series, bool isInverted, ChartInternalLocation startPoint, string startPath)
        {
            if (startPoint == null)
            {
                ChartInternalLocation firstPoint = ChartHelper.TransformToVisible(x, y, XAxisRenderer.Axis, YAxisRenderer.Axis, Series);
                Direction.Append(string.Join(string.Empty, startPath, SPACE, Convert.ToInt32(firstPoint.X).ToString(Culture), SPACE, Convert.ToInt32(firstPoint.Y).ToString(Culture), SPACE));
            }
        }

        internal virtual void GetAreaEmptyDirection(ChartInternalLocation firstPoint, ChartInternalLocation secondPoint, ChartSeries series, bool isInverted)
        {
            GetAreaPathDirection(firstPoint.X, firstPoint.Y, series, isInverted, null, "L");
            GetAreaPathDirection(secondPoint.X, secondPoint.Y, series, isInverted, null, "L");
        }

        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = true;
            switch (property)
            {
                case "Fill":
                    options.Fill = Interior;
                    Series.Marker.Renderer?.MarkerColorChanged();
                    break;
                case "Color":
                    options.Stroke = Series.Border.Color;
                    break;
                case "DashArray":
                    options.StrokeDashArray = Series.DashArray;
                    break;
                case "Width":
                    options.StrokeWidth = Series.Width;
                    break;
                case "Opacity":
                    options.Opacity = Series.Opacity;
                    break;
            }
        }
    }
}