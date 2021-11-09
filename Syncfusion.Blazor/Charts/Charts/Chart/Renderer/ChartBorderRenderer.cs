using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartBorderRenderer : ChartRenderer
    {
        private Rect availableRect;

        internal ChartBorder ChartBorder { get; set; }

        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            Owner.ChartBorderRenderer = this;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            if (availableRect != rect)
            {
                availableRect = rect;
                RendererShouldRender = true;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            SetDefaultBorder();
            if (availableRect != null)
            {
                RenderChartBorder(builder);
            }

            RendererShouldRender = false;
        }

        private void SetDefaultBorder()
        {
            if (ChartBorder != null)
            {
                return;
            }

            ChartBorder = new ChartBorder();
        }

        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            ProcessRenderQueue();
        }

        private void RenderChartBorder(RenderTreeBuilder builder)
        {
            double width = Owner.AvailableSize.Width - ChartBorder.Width;
            double height = Owner.AvailableSize.Height - ChartBorder.Width;
            Owner.SvgRenderer.RenderRect(builder, new RectOptions
            {
                Id = Owner.ID + "_ChartBorder",
                Fill = !string.IsNullOrEmpty(Owner.BackgroundImage) ? "transparent" : (!string.IsNullOrEmpty(Owner.Background) ? Owner.Background : Owner.ChartThemeStyle.Background),
                Stroke = ChartBorder.Color,
                StrokeWidth = ChartBorder.Width,
                Opacity = 1,
                X = ChartBorder.Width * 0.5,
                Y = ChartBorder.Width * 0.5,
                Width = width,
                Height = height
            });
            if (!string.IsNullOrEmpty(Owner.BackgroundImage))
            {
                ImageOptions image = new ImageOptions
                {
                    Id = Owner.ID + "_ChartBackground",
                    Width = width,
                    Height = height,
                    Href = Owner.BackgroundImage,
                    X = 0,
                    Y = 0,
                    Visibility = "visible",
                    PreserveAspectRatio = "none"
                };
                Owner.SvgRenderer.RenderImage(builder, image);
            }
        }
    }
}