using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class BarSeriesRenderer : ColumnBaseRenderer
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
                Point pointBar = Points[i];
                pointBar.SymbolLocations = new List<ChartInternalLocation>();
                pointBar.Regions = new List<Rect>();
                Point previousPoint = i - 1 > -1 ? Points[i - 1] : null, nextPoint = i + 1 < Points.Count ? Points[i + 1] : null;
                if (pointBar.Visible && ChartHelper.WithInRange(previousPoint, pointBar, nextPoint, XAxisRenderer))
                {
                    Rect rect = GetRectangle(pointBar.XValue + sideBySideInfo.Start, pointBar.YValue, pointBar.XValue + sideBySideInfo.End, origin);
                    PointRenderEventArgs argsData = TriggerEvent(pointBar, Interior, new BorderModel() { Width = Series.Border.Width, Color = (YAxisRenderer.VisibleRange.Start >= pointBar.YValue) ? string.Empty : Series.Border.Color });
                    if (!argsData.Cancel)
                    {
                        UpdateSymbolLocation(pointBar, rect);
                        id = pointId + pointBar.Index;
                        direction = CalculateRectangle(pointBar, rect, id);
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

        internal override bool IsRectSeries()
        {
            return true;
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
