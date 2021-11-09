using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Charts.Internal
{
    public interface IChartElement
    {
        string RendererKey { get; set; }

        Type RendererType { get; set; }
    }

    public class ChartRendererContainer : ChartRenderer
    {
        private Queue<IChartElementRenderer> rendererQueue = new Queue<IChartElementRenderer>();

        internal bool ContainerPrerender { get; set; } = true;

        internal bool ContainerUpdate { get; set; }

        internal virtual List<Type> DefaultRendererType { get; set; } = new List<Type>();

        internal List<IChartElement> Elements { get; private set; } = new List<IChartElement>();

        internal List<IChartElementRenderer> Renderers { get; private set; } = new List<IChartElementRenderer>();

        internal void AddToRenderQueue(IChartElementRenderer renderer)
        {
            rendererQueue.Enqueue(renderer);
        }

        // TODO: 1. Only add case is handled. Have to handle insert also.
        // TODO: 2. Set unique renderer key while building renderers in the render tree
        public void AddElement(IChartElement element)
        {
            if (!Elements.Contains(element))
            {
                ContainerPrerender = true;
                Elements.Add(element);
                OnElementAdded(element);
            }
        }

        protected virtual void OnElementAdded(IChartElement element)
        {
        }

        public void RemoveElement(IChartElement element)
        {
            if (Elements.Contains(element))
            {
                ContainerPrerender = true;
                Elements.Remove(element);
                OnElementRemoved(element);
            }
        }

        protected virtual void OnElementRemoved(IChartElement element)
        {
        }

        public virtual void AddRenderer(IChartElementRenderer renderer)
        {
            if (!Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Add(renderer);
                int index = Renderers.IndexOf(renderer);
                if (index <= Elements.Count - 1 && index != -1)
                {
                    OnRendererAdded(renderer, Elements[index]);
                }
                else
                {
                    OnRendererAdded(renderer, null);
                }
            }
        }

        protected virtual void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
        }

        public void RemoveRenderer(IChartElementRenderer renderer)
        {
            if (Renderers.Contains(renderer))
            {
                ContainerPrerender = false;
                RendererShouldRender = true;
                Renderers.Remove(renderer);
                OnRendererRemoved(renderer);
            }
        }

        protected virtual void OnRendererRemoved(IChartElementRenderer renderer)
        {
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
                   BuildRenderers(builder2);
               });
            RendererShouldRender = false;
        }

        protected virtual void BuildRenderers(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            int seq = 0;
            foreach (IChartElement element in Elements)
            {
                if (element.RendererType != null)
                {
                    builder.OpenComponent(seq++, element.RendererType);
                    builder.CloseComponent();
                }
            }
        }

        internal void Prerender()
        {
            ContainerUpdate = true;
            StateHasChanged();
        }

        public override void ProcessRenderQueue()
        {
            while (rendererQueue.Count != 0)
            {
                var renderer = rendererQueue.Dequeue();
                renderer.InvalidateRender();
            }
        }

        internal virtual void HandleLayoutChange()
        {
            foreach (IChartElementRenderer renderer in Renderers)
            {
                renderer.HandleLayoutChange();
            }
        }

        protected override bool ShouldRender()
        {
            return RendererShouldRender || ContainerPrerender;
        }

        protected void SortSeriesByZOrder()
        {
            Elements = Elements.OrderBy(element => (element as ChartSeries).ZOrder).ToList();
        }
    }
}
