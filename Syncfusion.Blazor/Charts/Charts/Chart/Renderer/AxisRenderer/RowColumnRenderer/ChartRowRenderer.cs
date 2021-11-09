using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System.Collections.ObjectModel;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartRowRenderer : ChartRenderer, IChartElementRenderer
    {
        [CascadingParameter]
        internal ChartRendererContainer Container { get; set; }

        public ChartRow ChartRow { get; set; }
        
        internal ObservableCollection<ChartAxis> Axes { get; set; } = new ObservableCollection<ChartAxis>();
        
        internal double ComputedHeight { get; set; }
        
        internal double ComputedTop { get; set; }
        
        internal List<double> NearSizes { get; set; }
        
        internal List<double> FarSizes { get; set; }

        protected override void OnInitialized()
        {
            Owner.RowContainer.AddRenderer(this);
            SvgRenderer = Owner.SvgRenderer;
            ChartRow.Renderer = this;
        }

        public void HandleChartSizeChange(Rect rect, double remainingHeight, double rowTop, bool isLast)
        {
            if (rect == null)
            {
                return;
            }

            double height;
            if (ChartRow.Height.IndexOf('%', StringComparison.InvariantCulture) != -1)
            {
                height = Math.Min(remainingHeight, rect.Height * double.Parse(ChartRow.Height.Replace("%", string.Empty, StringComparison.InvariantCulture), null) / 100);
            }
            else
            {
                height = Math.Min(remainingHeight, double.Parse(ChartRow.Height.Replace("px", string.Empty, StringComparison.InvariantCulture), null));
            }

            height = isLast ? height : remainingHeight;
            ComputedHeight = height;
            ComputedTop = rowTop - height;
        }

        public void ComputeSize(ChartAxis axis, double scrollBarHeight = 0)
        {
            double width = 0;
            double innerPadding = 5;
            if (axis != null && axis.Visible)
            {
                width += axis.Renderer.FindTickSize() + scrollBarHeight + axis.Renderer.FindLabelSize(innerPadding, axis.Renderer.Rect.Height) + axis.LineStyle.Width * 0.5;
            }

            if (axis != null && axis.OpposedPosition)
            {
                FarSizes.Add(width);
            }
            else
            {
                NearSizes.Add(width);
            }
        }

        internal void MeasureRowDefinition(Size size)
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

        public void HandleLayoutChange()
        {
        }

        public void InvalidateRender()
        {
            StateHasChanged();
        }
    }
}
