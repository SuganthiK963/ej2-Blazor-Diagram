using Microsoft.AspNetCore.Components;
using System;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class ChartColumn : ChartSubComponent, IChartElement
    {
        [CascadingParameter]
        internal SfChart Container { get; set; }

        /// <summary>
        /// Options to customize the border of the columns.
        /// </summary>
        [Parameter]
        public ChartBorder Border { get; set; } = new ChartBorder();

        /// <summary>
        /// The width of the column as a string accepts input both as like '100px' or '100%'.
        /// If specified as '100%, column renders to the full width of its chart.
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "100%";

        public string RendererKey { get; set; }

        public Type RendererType { get; set; }

        internal ChartColumnRenderer Renderer { get; set; }

        protected override void OnInitialized()
        {
            RendererType = typeof(ChartColumnRenderer);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Container != null)
            {
                Container.AddColumn(this);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Container = null;
            ChildContent = null;
        }
    }
}
