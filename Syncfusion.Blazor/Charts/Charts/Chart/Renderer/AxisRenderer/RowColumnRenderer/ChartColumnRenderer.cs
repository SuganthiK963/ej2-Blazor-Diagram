using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartColumnRenderer : ChartRenderer, IChartElementRenderer
    {
        [CascadingParameter]
        internal ChartRendererContainer Container { get; set; }

        public ChartColumn ChartColumn { get; set; }
        
        internal ObservableCollection<ChartAxis> Axes { get; set; } = new ObservableCollection<ChartAxis>();
        
        internal double ComputedWidth { get; set; }
        
        internal double ComputedLeft { get; set; }
        
        internal List<double> NearSizes { get; set; }
        
        internal List<double> FarSizes { get; set; }

        protected override void OnInitialized()
        {
            Owner.ColumnContainer.AddRenderer(this);
            SvgRenderer = Owner.SvgRenderer;
            ChartColumn.Renderer = this;
        }

        public void ComputeSize(ChartAxis axis, double scrollBarHeight = 0)
        {
            double height = 0;
            if (axis != null && axis.Visible)
            {
                height += axis.Renderer.FindTickSize() + scrollBarHeight + axis.Renderer.FindLabelSize(5, axis.Renderer.Rect.Width) + axis.LineStyle.Width * 0.5;
            }

            if (axis != null && axis.OpposedPosition)
            {
                FarSizes.Add(height);
            }
            else
            {
                NearSizes.Add(height);
            }
        }

        internal void MeasureColumnDefinition(Size size)
        {
            NearSizes = new List<double>();
            FarSizes = new List<double>();
            foreach (ChartAxis axis in Axes)
            {
                axis.Renderer.ComputeSize(size);
                ComputeSize(axis, axis.ScrollBarHeight);
            }

            if (FarSizes.Count > 0)
            {
                FarSizes[FarSizes.Count - 1] -= 10;
            }

            if (NearSizes.Count > 0)
            {
                NearSizes[NearSizes.Count - 1] -= 10;
            }
        }

        public void HandleChartSizeChange(Rect rect, double remainingWidth, double columnLeft, bool isLast)
        {
            if (rect == null)
            {
               return;
            }
            
            double width;
            
            if (ChartColumn.Width.IndexOf('%', StringComparison.InvariantCulture) != -1)
            {
                 width = Math.Min(remainingWidth, rect.Width * double.Parse(ChartColumn.Width.Replace("%", string.Empty, StringComparison.InvariantCulture), null) / 100);
            }
            else
            {
                 width = Math.Min(remainingWidth, Convert.ToInt32(ChartColumn.Width, null));
            }
            
            width = isLast ? width : remainingWidth;
            ComputedWidth = width;
            ComputedLeft = columnLeft;
        }

        public override void HandleChartSizeChange(Rect rect)
        {
        }

        public void HandleLayoutChange()
        {
        }

        public void InvalidateRender()
        {
            StateHasChanged();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
        }
    }
}
