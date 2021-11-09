using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    public partial class AnnotationElements
    {
        private RenderFragment templateContent { get; set; }

        private string annotationParentId { get; set; }

        private bool allowRender { get; set; }

        private string style { get; set; }

        private static RenderFragment AddAnnotation(SfAccumulationChart accChart) => builder =>
        {
            accChart.Annotations.ForEach(x => x.RenderAnnotationElement(builder));
        };

        internal void RenderAnnotations(SfAccumulationChart accChart)
        {
            allowRender = true;
            annotationParentId = accChart.ID + "_Annotation_Collections";
            templateContent = AddAnnotation(accChart);
            style = GetStyle(accChart);
            InvokeAsync(StateHasChanged);
        }

        internal static string GetStyle(SfAccumulationChart accChart)
        {
            return "position: relative;left: " + accChart.SecondaryElementOffset.Left.ToString(CultureInfo.InvariantCulture) + "px;top: " + accChart.SecondaryElementOffset.Top.ToString(CultureInfo.InvariantCulture) + "px;";
        }
    }
}