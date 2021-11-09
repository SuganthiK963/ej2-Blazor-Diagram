using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    internal class IndicatorLineBaseRenderer : LineBaseSeriesRenderer
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
            return Owner.ID + "_Indicator_" + Index + "_" + Series.Name;
        }

        internal override string ClipPathId()
        {
            return Owner.ID + "_ChartIndicatorClipRect_" + SourceIndex;
        }

        internal override string SeriesElementId()
        {
            return SeriesID() + "_Group";
        }

        private void CreateIndicatorElement(RenderTreeBuilder builder)
        {
            int seq = 0;
            builder.OpenElement(seq++, "g");
            builder.AddAttribute(seq++, "id", SeriesElementId());
            SvgRenderer.RenderPath(builder, options);
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
