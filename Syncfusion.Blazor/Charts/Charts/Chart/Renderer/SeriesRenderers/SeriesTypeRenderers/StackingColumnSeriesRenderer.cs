using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class StackingColumnSeriesRenderer : ColumnBaseRenderer
    {
        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                CalculateColumnPathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            ColumnPathOptions.Clear();
            CalculateColumnPathOption();
            base.UpdateDirection();
        }

        internal void CalculateColumnPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            List<Point> visiblePoints = ChartHelper.GetVisiblePoints(Points);
            string direction;
            string pointId = Series.Container.ID + "_Series_" + Index + "_Point_", id;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            PathOptions option;
            foreach (Point pointColumn in visiblePoints)
            {
                pointColumn.SymbolLocations = new List<ChartInternalLocation>();
                pointColumn.Regions = new List<Rect>();
                Point previousPoint = pointColumn.Index - 1 > -1 ? Points[pointColumn.Index - 1] : null, nextPoint = pointColumn.Index + 1 < Points.Count ? Points[pointColumn.Index + 1] : null;
                if (pointColumn.Visible && ChartHelper.WithInRange(previousPoint, pointColumn, nextPoint, Series.Renderer.XAxisRenderer))
                {
                    Rect rect = GetRectangle(pointColumn.XValue + sideBySideInfo.Start, Series.Renderer.StackedValues.EndValues[pointColumn.Index], pointColumn.XValue + sideBySideInfo.End, Series.Renderer.StackedValues.StartValues[pointColumn.Index]);
                    PointRenderEventArgs argsData = TriggerEvent(pointColumn, Interior, new BorderModel() { Width = Series.Border.Width, Color = Series.Border.Color });
                    if (!argsData.Cancel)
                    {
                        UpdateSymbolLocation(pointColumn, rect);
                        id = pointId + pointColumn.Index;
                        direction = CalculateRectangle(pointColumn, rect, id);
                        if (direction != null)
                        {
                            option = new PathOptions(id, direction, Series.DashArray, argsData.Border.Width, argsData.Border.Color, Series.Opacity, argsData.Fill, string.Empty, string.Empty, AccessText);
                            option.Visibility = visibility;
                            ColumnPathOptions.Add(option);
                        }
                    }
                }
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
            foreach (PathOptions option in ColumnPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }
    }

    public class StackingColumn100SeriesRenderer : StackingColumnSeriesRenderer
    {
    }
}