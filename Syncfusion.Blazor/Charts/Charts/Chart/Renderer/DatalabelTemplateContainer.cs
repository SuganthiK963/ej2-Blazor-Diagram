using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class DataLabelTemplateContainer: OwningComponentBase
    {
        [CascadingParameter]
        internal SfChart Owner { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Owner.DatalabelTemplateContainer = this;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            base.BuildRenderTree(builder);
            foreach (ChartSeries series in Owner.SeriesContainer?.Elements)
            {
                if (series.Marker.DataLabel.Renderer != null)
                {
                    int seq = 0;
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "id", GetDatalabelTemplateParentId(series.Renderer.Index));
                    foreach (DatalabelTemplateOptions templateOption in series.Marker.DataLabel.Renderer?.templateOptions)
                    {
                        builder.OpenElement(seq++, "div");
                        builder.AddAttribute(seq++, "id", templateOption.Id);
                        builder.AddAttribute(seq++, "style", templateOption.style);
                        builder.AddContent(seq++, templateOption.Template);
                        builder.CloseElement();
                    }

                    builder.CloseElement();
                }
            }
        }

        internal void InvalidateRender()
        {
            InvokeAsync(StateHasChanged);
        }

        private string GetDatalabelTemplateParentId(int index)
        {
            return Owner.ID + "_Series_" + index + "_DataLabelCollections";
        }
    }
}
