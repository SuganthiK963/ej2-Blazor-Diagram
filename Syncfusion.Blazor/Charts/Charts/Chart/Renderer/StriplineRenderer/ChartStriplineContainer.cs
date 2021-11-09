using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartStriplineContainer : ChartRendererContainer
    {
        protected int Seq { get; set; }

        protected Rect ClipRect { get; set; } = new Rect();

        public override void HandleChartSizeChange(Rect rect)
        {
            foreach (ChartStriplineRenderer renderer in Renderers)
            {
                renderer.InitStripline();
            }
        }

        internal void UpdateStriplineCollection()
        {
            foreach (ChartStriplineRenderer renderer in Renderers)
            {
                renderer.CalculateRenderingOptions();
                renderer.ProcessRenderQueue();
            }
        }

        private void UpdateClipRect()
        {
            RendererShouldRender = true;
            ClipRect = Owner.AxisContainer.AxisLayout.SeriesClipRect;
            InvokeAsync(StateHasChanged);
        }

        public override void AddRenderer(IChartElementRenderer renderer)
        {
            ContainerPrerender = false;
            RendererShouldRender = true;
            Renderers.Add(renderer);
            int index = Renderers.IndexOf(renderer);
            ChartStriplineRenderer striplineRenderer = renderer as ChartStriplineRenderer;
            if (striplineRenderer != null)
            {
                striplineRenderer.Stripline = Elements[index] as ChartStripline;
                striplineRenderer.Index = index;
            }
        }

        public override void ProcessRenderQueue()
        {
            UpdateClipRect();
            foreach (ChartStriplineRenderer renderer in Renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }
    }

    public class ChartStriplineBehindContainer : ChartStriplineContainer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.StriplineBehindContainer = this;
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            Seq = 0;
            string id = Owner.ID + "_stripline_" + "Behind" + "_";
            double width = ClipRect.Width > 0 ? ClipRect.Width : 0,
            height = ClipRect.Height > 0 ? ClipRect.Height : 0;
            RectOptions rectOption = new RectOptions(id + "ClipRect" + "_Rect", ClipRect.X, ClipRect.Y, width, height, 1, "transparent", "transparent", 0, 0, 1, "visible");
            builder.OpenElement(Seq++, "g");
            builder.AddAttribute(Seq++, "id", id + "collections");
            builder.AddAttribute(Seq++, "clip-path", "url(#" + id + "ClipRect" + ")");
            builder.OpenElement(Seq++, "defs");
            builder.OpenElement(Seq++, "clipPath");
            builder.AddAttribute(Seq++, "id", id + "ClipRect");
            builder.OpenComponent<SvgRect>(Seq++);
            builder.AddMultipleAttributes(Seq++, Owner.SvgRenderer.GetOptions(rectOption));
            builder.CloseComponent();
            builder.CloseElement();
            builder.CloseElement();
            foreach (IChartElement element in Elements)
            {
                if (element.RendererType != null)
                {
                    builder.OpenComponent(Seq++, element.RendererType);
                    builder.CloseComponent();
                }
            }
            builder.CloseElement();
        }
    }

    public class ChartStriplineOverContainer : ChartStriplineContainer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddToRenderQueue(this);
            Owner.StriplineOverContainer = this;
            Owner.AnnotationContainer?.AddContainerRenderer();
        }

        protected override void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            Seq = 0;
            string id = Owner.ID + "_stripline_" + "Over" + "_";
            double width = ClipRect.Width > 0 ? ClipRect.Width : 0,
            height = ClipRect.Height > 0 ? ClipRect.Height : 0;
            RectOptions rectOption = new RectOptions(id + "ClipRect" + "_Rect", ClipRect.X, ClipRect.Y, width, height, 1, "transparent", "transparent", 0, 0, 1, "visible");
            builder.OpenElement(Seq++, "g");
            builder.AddAttribute(Seq++, "id", id + "collections");
            builder.AddAttribute(Seq++, "clip-path", "url(#" + id + "ClipRect" + ")");
            builder.OpenElement(Seq++, "defs");
            builder.OpenElement(Seq++, "clipPath");
            builder.AddAttribute(Seq++, "id", id + "ClipRect");
            builder.OpenComponent<SvgRect>(Seq++);
            builder.AddMultipleAttributes(Seq++, Owner.SvgRenderer.GetOptions(rectOption));
            builder.CloseComponent();
            builder.CloseElement();
            builder.CloseElement();
            foreach (IChartElement element in Elements)
            {
                if (element.RendererType != null)
                {
                    builder.OpenComponent(Seq++, element.RendererType);
                    builder.CloseComponent();
                }
            }

            builder.CloseElement();
        }
    }
}
