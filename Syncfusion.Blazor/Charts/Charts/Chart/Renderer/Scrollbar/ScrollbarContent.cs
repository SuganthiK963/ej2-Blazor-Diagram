using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.Internal
{
    public partial class ScrollbarContent : OwningComponentBase
    {
        [CascadingParameter]
        private SfChart parent { get; set; }

        [Parameter]
        public SfChart Chart { get; set; }

        [Parameter]
        public double StartY { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            parent = Chart != null ? Chart : parent;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
#pragma warning disable CA2007
            await base.OnAfterRenderAsync(firstRender);
#pragma warning restore CA2007
            if (firstRender)
            {
                ChildContent = RenderScrollbarElements();
            }
        }

        protected override bool ShouldRender()
        {
            bool ShouldUpdate = true;
            bool isScrollbar = true;
            if (parent?.AxisContainer?.ScrollbarAxes != null)
            {
                foreach (ChartAxis axis in parent.AxisContainer.ScrollbarAxes)
                {
                    axis.Renderer.ZoomingScrollBar?.ScrollbarRef?.UpdateScrollBarPosition(true);
                    ShouldUpdate = ShouldUpdate && (axis.ZoomFactor == 1 && axis.ZoomPosition == 0);
                    isScrollbar = isScrollbar || (axis.Renderer.ZoomingScrollBar?.ScrollbarRef != null ? (bool)!axis.Renderer.ZoomingScrollBar?.ScrollbarRef.IsScrollbarDisposed : true);
                }
            }

            return (parent != null && !parent.AxisContainer.IsScrollExist) || ShouldUpdate || !isScrollbar || parent.ZoomingModule != null && ((bool)parent.ZoomingModule.IsWheelZoom || ((bool)!parent.ZoomingModule.IsWheelZoom && !parent.AxisContainer.IsScrolling && (bool)parent.ZoomingModule.IsPanning));
        }

        private RenderFragment RenderScrollbarElements() => builder =>
        {
            if (parent?.AxisContainer.ScrollbarAxes.Count > 0)
            {
                RenderScrollbar(parent, parent.AxisContainer.ScrollbarAxes, builder);
            }
        };

#pragma warning disable CA1822
        internal void RenderScrollbar(SfChart chart, List<ChartAxis> axes, RenderTreeBuilder builder)
#pragma warning restore CA1822
        {
            bool isZoomed = chart.ZoomingModule == null ? false : chart.ZoomingModule.IsZoomed;
            string referenceId = chart.ID + "_scrollElement";
            chart.SvgRenderer.CreateElement(builder, "div", referenceId, false);
            foreach (ChartAxis axis in axes)
            {
                if (((isZoomed && (axis.ZoomFactor < 1 || axis.ZoomPosition > 0)) || (axis.ScrollbarSettings.Enable &&
                    (axis.ZoomFactor <= 1 || axis.ZoomPosition >= 0))) && !axis.Renderer.ZoomingScrollBar.IsScrollUI)
                {
                    if (chart.ScrollElement != null)
                    {
                        axis.Renderer.ZoomingScrollBar.Render(builder, true, StartY);
                    }
                }
                else if (axis.ZoomFactor == 1 && axis.ZoomPosition == 0 && axis.Renderer.ZoomingScrollBar?.SvgObject != null && !axis.ScrollbarSettings.Enable)
                {
                    axis.Renderer.ZoomingScrollBar.Destroy();
                }

                if (axis.Renderer.ZoomingScrollBar.IsScrollUI)
                {
                    axis.Renderer.ZoomingScrollBar.IsScrollUI = false;
                }
            }

            builder.CloseElement();
        }

        public void CallStateHasChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            parent = null;
            ChildContent = null;
        }
    }
}