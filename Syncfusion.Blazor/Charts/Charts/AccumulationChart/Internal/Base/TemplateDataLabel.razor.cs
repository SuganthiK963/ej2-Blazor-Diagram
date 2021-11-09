using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.Charts.Chart.Models;
using System;

namespace Syncfusion.Blazor.Charts.AccumulationChart.Internal
{
    public partial class TemplateDataLabel
    {
        private RenderFragment templateContent { get; set; }

        private bool allowDatalabel { get; set; }

        private static RenderFragment TemplateDataLabels(SfAccumulationChart accChart) => builder =>
        {
            accChart.DataLabelModule.RenderDataLabelTemplates(builder);
        };

        internal async void RenderTemplate(SfAccumulationChart accChart)
        {
            allowDatalabel = true;
            templateContent = TemplateDataLabels(accChart);
#pragma warning disable CA2007
            await InvokeAsync(StateHasChanged);
            const double padding = 4;
            foreach (var accSeries in accChart.VisibleSeries)
            {
                foreach (var point in accSeries.Points)
                {
                    DomRect templateSize = await accChart.InvokeMethod<DomRect>(AccumulationChartConstants.GETELEMENTBOUNDSBYID, false, new object[] { point.TemplateID });
                    point.TextSize = new Size(templateSize.Width, templateSize.Height);
                    point.TextSize.Height += padding;
                    point.TextSize.Width += padding;
                }

                accSeries.ProcessingDataLabels(null);
            }

            templateContent = TemplateDataLabels(accChart);
            await InvokeAsync(StateHasChanged);
#pragma warning restore CA2007
        }
    }
}