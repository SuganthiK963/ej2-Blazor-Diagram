using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.DataVizCommon;
using System;
using Syncfusion.Blazor.Charts.Chart.Internal;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ColumnSeriesRenderer : ColumnBaseRenderer
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

        private void CalculateColumnPathOption()
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, 0);
            string direction;
            string pointId = Series.Container.ID + "_Series_" + Index + "_Point_", id;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            PathOptions option;
            for (int i = 0; i < Points.Count; i++)
            {
                Point pointColumn = Points[i];
                pointColumn.SymbolLocations = new List<ChartInternalLocation>();
                pointColumn.Regions = new List<Rect>();
                Point previousPoint = i - 1 > -1 ? Points[i - 1] : null, nextPoint = i + 1 < Points.Count ? Points[i + 1] : null;
                if (pointColumn.Visible && ChartHelper.WithInRange(previousPoint, pointColumn, nextPoint, Series.Renderer.XAxisRenderer))
                {
                    Rect rect = GetRectangle(pointColumn.XValue + sideBySideInfo.Start, pointColumn.YValue, pointColumn.XValue + sideBySideInfo.End, origin);
                    PointRenderEventArgs argsData = TriggerEvent(pointColumn, Interior, new BorderModel() { Width = Series.Border.Width, Color = (YAxisRenderer.VisibleRange.Start >= pointColumn.YValue) ? string.Empty : Series.Border.Color });
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
}