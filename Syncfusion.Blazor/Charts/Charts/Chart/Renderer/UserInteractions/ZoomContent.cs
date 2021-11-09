using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.DataVizCommon;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ZoomContent: OwningComponentBase
    {
        [CascadingParameter]
        internal SfChart Chart { get; set; }

        internal RectOptions Options { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetRectOption();
        }

        internal void SetRectOption()
        {
            Options = new RectOptions(Chart.ID + "_ZoomArea", 0, 0, 0, 0, 1, Chart.ChartThemeStyle.SelectionRectStroke, Chart.ChartThemeStyle.SelectionRectFill, 0, 0, 1);
            Options.Transform = string.Empty;
            Options.DashArray = "3";
        }

        internal void UpdateRectSize(Rect rect)
        {
            Options.X = rect.X;
            Options.Y = rect.Y;
            Options.Height = rect.Height;
            Options.Width = rect.Width;
            InvalidateRenderer();
        }
        internal void InvalidateRenderer()
        {
            StateHasChanged();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            Chart.SvgRenderer.RectElementList.Remove(Chart.SvgRenderer.RectElementList.Find(item => item.Id == Options.Id));
            Chart.SvgRenderer.RenderRect(builder, Options);
        }
    }
}
