using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using Microsoft.AspNetCore.Components;
using System;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class IndicatorColumnSeriesRenderer : ColumnBaseRenderer
    {
        [Parameter]
        public ChartSeries Indicatorseries { get; set; }

        protected override void OnInitialized()
        {
            InitSeriesRendererFields();
            Series = Indicatorseries;
            Series.Renderer = this;
            SvgRenderer = Owner.SvgRenderer;
            Owner.IndicatorContainer.AddRenderer(this);
            InitDynamicAnimationProperty();
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
            SeriesRenderer();
            OnLayoutChange();
        }

        protected override string SeriesID()
        {
            return Owner.ID + "_Indicator_" + Index + "_" + Series.Name + "_Point_";
        }

        internal override string ClipPathId()
        {
            return Owner.ID + "_ChartIndicatorClipRect_" + SourceIndex;
        }

        internal override string SeriesElementId()
        {
            return SeriesID() + "_Group";
        }

        protected override void RenderSeries()
        {
            base.RenderSeries();
            ColumnPathOptions = new List<PathOptions>();
            if (Series.Visible)
            {
                GetSetColumnPathOption();
            }

            Animate();
        }

        internal override void UpdateDirection()
        {
            RendererShouldRender = true;
            GetSetColumnPathOption(true);
            InvokeAsync(StateHasChanged);
        }

        internal override void UpdateCustomization(string property)
        {
            RendererShouldRender = true;
            string visibility = Series.Animation.Enable && Owner.ShouldAnimateSeries ? "hidden" : "visible";
            ColumnPathOptions.ForEach(option => option.Visibility = visibility);
            switch (property)
            {
                case "Fill":
                    ColumnPathOptions.ForEach(option => option.Fill = Series.Fill);
                    break;
            }
        }

        private void GetSetColumnPathOption(bool isUpdateDirection = false)
        {
            DoubleRange sideBySideInfo = GetSideBySideInfo();
            double origin = Math.Max(YAxisRenderer.VisibleRange.Start, 0);
            string direction;
            string pointId = SeriesID(), id;
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
                    UpdateSymbolLocation(pointColumn, rect);
                    id = pointId + pointColumn.Index;
                    direction = CalculateRectangle(pointColumn, rect, id);
                    if (direction != null && !isUpdateDirection)
                    {
                        option = new PathOptions(id, direction, Series.DashArray, Series.Border.Width, Series.Border.Color, Series.Opacity, pointColumn.Interior, string.Empty, string.Empty, AccessText);
                        option.Visibility = visibility;
                        ColumnPathOptions.Add(option);
                    }
                    else if (direction != null)
                    {
                        option = ColumnPathOptions.Find(element => element.Id == id);
                        option.Visibility = visibility;
                        option.Direction = direction;
                    }
                }
            }
        }

        private void CreateIndicatorElement(RenderTreeBuilder builder)
        {
            int seq = 0;
            builder.OpenElement(seq++, "g");
            builder.AddAttribute(seq++, "id", SeriesElementId());
            foreach (PathOptions option in ColumnPathOptions)
            {
                SvgRenderer.RenderPath(builder, option);
            }

            builder.CloseElement();
        }

        internal override SeriesCategories Category()
        {
            return SeriesCategories.Indicator;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ClipRect == null || builder == null)
            {
                return;
            }

            CreateIndicatorElement(builder);
        }
    }
}
