using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartRowRendererContainer : ChartRendererContainer
    {
        internal override List<Type> DefaultRendererType { get; set; } = new List<Type>() { typeof(ChartRowRenderer) };

        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            Owner.RowContainer = this;
            SvgRenderer = Owner.SvgRenderer;
        }

        protected override void OnElementAdded(IChartElement element)
        {
            StateHasChanged();
        }

        protected override void OnElementRemoved(IChartElement element)
        {
            if (!IsDisposed)
            {
                Renderers.Remove((element as ChartRow)?.Renderer);
                if (!Owner.ChartDisposed())
                {
                    StateHasChanged();
                }
            }
        }

        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer != null)
            {
                if (element == null)
                {
                    element = new ChartRow();
                    Elements.Add(element);
                }

                (renderer as ChartRowRenderer).ChartRow = element as ChartRow;
                if (Owner.InitialRect != null)
                {
                    Owner.ProcessOnLayoutChange();
                }
            }
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            if (rect != null)
            {
                double remainingHeight = Math.Max(0, rect.Height);
                double rowTop = rect.Y + rect.Height;
                ChartRowRenderer renderer;
                for (int i = 0, len = Renderers.Count; i < len; i++)
                {
                    renderer = Renderers[i] as ChartRowRenderer;
                    renderer.HandleChartSizeChange(rect, remainingHeight, rowTop, i != len - 1);
                    remainingHeight -= renderer.ComputedHeight;
                    rowTop -= renderer.ComputedHeight;
                }
            }
        }

        internal void AssignAxisToRow()
        {
            int actualIndex, span;
            ChartRowRenderer rowRenderer;
            ChartAxis axis;
            Renderers.ForEach(rowRenderer => (rowRenderer as ChartRowRenderer).Axes.Clear());
            foreach (ChartAxisRenderer axisRenderer in Owner.AxisContainer.Renderers)
            {
                if (axisRenderer.Orientation == Orientation.Vertical)
                {
                    axis = axisRenderer.Axis;
                    axis.SetOpposedPosition();
                    actualIndex = GetActualRow(axis);
                    rowRenderer = Renderers[actualIndex] as ChartRowRenderer;
                    rowRenderer.Axes.Add(axis);
                    span = (actualIndex + axisRenderer.Axis.Span) > Renderers.Count ? Renderers.Count : actualIndex + axis.Span;
                }
            }
        }

        private int GetActualRow(ChartAxis axis)
        {
            int actualLength = Renderers.Count;
            int pos = Convert.ToInt32(axis.RowIndex);
            return pos >= actualLength ? actualLength - 1 : (pos < 0 ? 0 : pos);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            int seq = 0;
            if (ContainerUpdate && Elements.Count > 0)
            {
                base.BuildRenderTree(builder);
            }
            else if (ContainerUpdate && Renderers.Count == 0)
            {
                foreach (Type defaultRenderer in DefaultRendererType)
                {
                    builder?.OpenComponent(seq++, defaultRenderer);
                    builder?.CloseComponent();
                }
            }
        }
    }
}
