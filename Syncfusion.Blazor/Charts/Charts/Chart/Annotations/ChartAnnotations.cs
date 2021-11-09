using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Annotation is a user defined HTML element that can be placed on chart
    /// We can use annotations to pile up the visual elegance of the chart.
    /// </summary>
    public class ChartAnnotations : ChartSubComponent, ISubcomponentTracker
    {
        private int pendingParametersSetCount;

        [CascadingParameter]
        internal SfChart Chart { get; set; }

        internal List<ChartAnnotation> Annotations { get; set; } = new List<ChartAnnotation>();

        void ISubcomponentTracker.PushSubcomponent()
        {
            pendingParametersSetCount++;
        }

        void ISubcomponentTracker.PopSubcomponent()
        {
            pendingParametersSetCount--;
            if (pendingParametersSetCount == 0)
            {
                Chart.AnnotationContainer.Prerender();
            }
        }
    }

    public class ChartAnnotationRendererContainer : ChartRendererContainer
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.AnnotationContainer = this;
            Elements.AddRange(Owner.StockChartAnnotations);
        }

        internal void AddContainerRenderer()
        {
            AddToRenderQueue(this);
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            foreach (ChartAnnotationRenderer renderer in Renderers)
            {
                renderer.CalculateRenderingOption();
            }
        }

        public override void ProcessRenderQueue()
        {
            foreach (ChartRenderer renderer in Renderers)
            {
                renderer.ProcessRenderQueue();
            }
        }

        internal void UpdateRenderers()
        {
            foreach (ChartAnnotationRenderer renderer in Renderers)
            {
                renderer.CalculateRenderingOption();
                renderer.ProcessRenderQueue();
            }
        }

        protected override void OnElementAdded(IChartElement element)
        {
            if (Owner.InitialRect != null)
            {
                RendererShouldRender = true;
                StateHasChanged();
            }
        }

        public override void AddRenderer(IChartElementRenderer renderer)
        {
            ContainerPrerender = false;
            RendererShouldRender = true;
            Renderers.Add(renderer);
            int index = Renderers.IndexOf(renderer);
            ChartAnnotationRenderer annotationRenderer = renderer as ChartAnnotationRenderer;
            if (annotationRenderer != null)
            {
               annotationRenderer.Annotation = Elements[index] as ChartAnnotation;
            }

            if (Owner.InitialRect != null && annotationRenderer != null)
            {
                annotationRenderer.CalculateRenderingOption();
                annotationRenderer.ProcessRenderQueue();
            }
        }

        internal void InvalidateRenderer()
        {
            RendererShouldRender = true;
            StateHasChanged();
        }
    }
}