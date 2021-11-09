using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class IndicatorLineSeriesRenderer : IndicatorLineBaseRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            CalculateDirection();
            options = new PathOptions(SeriesID(), Direction.ToString(), Series.DashArray, Series.Width, Series.Fill, Series.Opacity);

            if (Owner.ShouldAnimateSeries && Series.Animation.Enable)
            {
                AnimationOptions = new AnimationOptions(SeriesID(), AnimationType.Progressive);
            }
        }

        private void CalculateDirection()
        {
            string startPoint = "M";
            List<Point> visiblePoints = EnableComplexProperty();
            Direction = new System.Text.StringBuilder();
            double prevPointX = double.NaN, prevPointY = double.NaN, pointX, pointY;
            int count = visiblePoints.Count;
            for (int i = 0; i < count; i++)
            {
                Point point = visiblePoints[i];
                point.Regions = new List<Rect>();
                if (point.Visible && ChartHelper.WithInRange((i - 1 > -1) && (i - 1 < count) ? visiblePoints[i - 1] : null, point, i + 1 < count ? visiblePoints[i + 1] : null, XAxisRenderer))
                {
                    pointX = point.XValue;
                    pointY = point.YValue;
                    GetLineDirection(prevPointX, prevPointY, pointX, pointY, Owner.RequireInvertedAxis, startPoint);
                    startPoint = !double.IsNaN(prevPointX) ? "L" : startPoint;
                    prevPointX = pointX;
                    prevPointY = pointY;
                }
                else
                {
                    prevPointX = prevPointY = double.NaN;
                    startPoint = "M";
                    point.SymbolLocations = new List<ChartInternalLocation>();
                }
            }
        }

        internal override void UpdateDirection()
        {
            RendererShouldRender = true;            
            CalculateDirection();
            options.Direction = Direction.ToString();
            InvokeAsync(StateHasChanged);
        }

        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = true;
            switch (property)
            {
                case "Fill":
                    options.Stroke = Interior = Series.Fill;
                    break;
                case "DashArray":
                    options.StrokeDashArray = Series.DashArray;
                    break;
                case "Width":
                    options.StrokeWidth = Series.Width;
                    break;
            }
        }
    }
}
