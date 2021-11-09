using System;
using Syncfusion.Blazor.DataVizCommon;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartColumnRendererContainer : ChartRendererContainer
    {
        internal override List<Type> DefaultRendererType { get; set; } = new List<Type>() { typeof(ChartColumnRenderer) };

        protected override void OnInitialized()
        {
            AddToRenderQueue(this);
            Owner.ColumnContainer = this;
            SvgRenderer = Owner.SvgRenderer;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        public override void ProcessRenderQueue()
        {
            StateHasChanged();
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            if (rect != null)
            {
                double remainingWidth = Math.Max(0, rect.Width);
                double columnLeft = rect.X;
                ChartColumnRenderer renderer;
                Rect initialRect = rect;
                for (int i = 0, len = Renderers.Count; i < len; i++)
                {
                    renderer = Renderers[i] as ChartColumnRenderer;
                    renderer.HandleChartSizeChange(initialRect, remainingWidth, columnLeft, i != len - 1);
                    remainingWidth -= renderer.ComputedWidth;
                    columnLeft += renderer.ComputedWidth;
                }
            }
        }

        internal void AssignAxisToColumn()
        {
            int actualIndex;
            ChartColumnRenderer columnRenderer;
            ChartAxis axis;
            Renderers.ForEach(renderer => (renderer as ChartColumnRenderer).Axes.Clear());
            foreach (ChartAxisRenderer axisRenderer in Owner.AxisContainer.Renderers)
            {
                if (axisRenderer.Orientation == Orientation.Horizontal)
                {
                    axis = axisRenderer.Axis;
                    axis.SetInverse();
                    actualIndex = GetActualColumn(axis);
                    columnRenderer = Renderers[actualIndex] as ChartColumnRenderer;
                    columnRenderer.Axes.Add(axis);
                }
            }
        }

        private int GetActualColumn(ChartAxis axis)
        {
            int actualLength = Renderers.Count;
            int pos = Convert.ToInt32(axis.ColumnIndex);
            int result = pos >= actualLength ? actualLength - 1 : (pos < 0 ? 0 : pos);
            return result;
        }

        protected override void OnRendererAdded(IChartElementRenderer renderer, IChartElement element)
        {
            if (renderer != null)
            {
                (renderer as ChartColumnRenderer).ChartColumn = element != null ? element as ChartColumn : new ChartColumn();
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            int seq = 0;
            if (ContainerUpdate && Elements.Count > 0)
            {
                base.BuildRenderTree(builder);
            }
            else if (ContainerUpdate && Renderers.Count == 0) {
                foreach (Type defaultRenderer in DefaultRendererType)
                {
                    builder.OpenComponent(seq++, defaultRenderer);
                    builder.CloseComponent();
                }
            }

            RendererShouldRender = false;
        }
    }
}
