using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    public interface IChartElementRenderer
    {
        void InvalidateRender();

        void HandleLayoutChange();
    }

    public class ChartRenderer : OwningComponentBase
    {
        internal bool RendererShouldRender { get; set; }

        protected SvgRendering SvgRenderer { get; set; }

        [CascadingParameter]
        public SfChart Owner { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public virtual void HandleChartSizeChange(Rect rect)
        {
        }

        internal virtual void OnParentParameterSet()
        {
            ProcessRenderQueue();
        }

        protected override bool ShouldRender()
        {
            return RendererShouldRender;
        }

        public virtual void ProcessRenderQueue()
        {
            InvokeAsync(StateHasChanged);
        }

        internal void AddToRenderQueue(ChartRenderer renderer)
        {
            Owner.Renderers.Add(renderer);

           // RendererQueue.Enqueue(renderer);
        }

        internal void RemoveFromRenderQueue(ChartRenderer renderer)
        {
            Owner.Renderers.Remove(renderer);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            this.CreateCascadingValue(
               builder,
               0,
               1,
               this,
               2,
               (builder2) =>
               {
                   if (ChildContent != null)
                   {
                       ChildContent(builder2);
                   }
               });

            RendererShouldRender = false;
        }
    }
}
