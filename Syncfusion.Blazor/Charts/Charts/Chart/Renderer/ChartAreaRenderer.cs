using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartAreaRenderer : ChartRenderer
    {
        internal Rect ChartAreaRect { get; private set; }

        internal ChartArea Area { get; set; }

        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            Owner.ChartAreaRenderer = this;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            RendererShouldRender = true;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            SetDefaultArea();
            RenderChartAreaBorder(builder);
            RendererShouldRender = false;
        }

        private void SetDefaultArea()
        {
            if (Area != null)
            {
                return;
            }

            Area = new ChartArea();
        }

        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            ProcessRenderQueue();
        }

        private void RenderChartAreaBorder(RenderTreeBuilder builder)
        {
            // TODO: Need to skip if it is not a cartesian axis. Need to add the border color based on the theme
            if (Owner.GetAreaType() == ChartAreaType.PolarAxes || builder == null || (Owner.AxisContainer?.AxisLayout as CartesianAxisLayout)?.SeriesClipRect == null)
            {
                return;
            }

            ChartAreaRect = (Owner.AxisContainer.AxisLayout as CartesianAxisLayout).SeriesClipRect;
            Owner.SvgRenderer.RenderRect(
            builder,
            new RectOptions
            {
                Id = Owner.ID + Constants.AREABORDERID,
                Fill = Area.Background,
                Stroke = Area.Border.Color ?? Owner.ChartThemeStyle.AreaBorder,
                StrokeWidth = Area.Border.Width,
                Opacity = Area.Opacity,
                X = ChartAreaRect.X,
                Y = ChartAreaRect.Y,
                Width = ChartAreaRect.Width > 0 ? ChartAreaRect.Width : 0,
                Height = ChartAreaRect.Height > 0 ? ChartAreaRect.Height : 0
            });
        }
    }
}