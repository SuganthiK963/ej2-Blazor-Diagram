using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Chart events are defined by this class.
    /// </summary>
    public class SmithChartEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal SfSmithChart BaseParent { get; set; }

        /// <summary>
        /// Triggers after the animation gets completed.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<AnimationCompleteEventArgs> AnimationCompleted { get; set; }

        /// <summary>
        /// Triggers before each axis label is rendered.
        /// </summary>
        [Parameter]
        public Action<SmithChartAxisLabelRenderEventArgs> AxisLabelRendering { get; set; }

        /// <summary>
        /// Triggers while print the smith chart component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnPrintComplete event to achieve this.")]
        [Parameter]
        public Action<IPrintEventArgs> OnPrint { get; set; }

        /// <summary>
        /// Triggers after print is completed.
        /// </summary>
        [Parameter]
        public Action OnPrintComplete { get; set; }

        /// <summary>
        /// Triggers after export is completed.
        /// </summary>
        [Parameter]
        public Action<SmithChartExportEventArgs> OnExportComplete { get; set; }

        /// <summary>
        /// Triggers before the legend is rendered.
        /// </summary>
        [Parameter]
        public Action<SmithChartLegendRenderEventArgs> LegendRendering { get; set; }

        /// <summary>
        /// Triggers before render the smith chart component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<SmithChartLoadedEventArgs> OnLoad { get; set; }

        /// <summary>
        /// Triggers when the chart rendering completed.
        /// </summary>
        [Parameter]
        public Action<SmithChartLoadedEventArgs> Loaded { get; set; }

        /// <summary>
        /// Triggers when the window is re-sizing.
        /// </summary>
        [Parameter]
        public Action<SmithChartResizeEventArgs> SizeChanged { get; set; }

        /// <summary>
        /// Triggers when the series is rendering.
        /// </summary>
        [Parameter]
        public Action<SmithChartSeriesRenderEventArgs> SeriesRender { get; set; }

        /// <summary>
        /// Triggers before the data label text is rendered.
        /// </summary>
        [Parameter]
        public Action<SmithChartTextRenderEventArgs> TextRendering { get; set; }

        /// <summary>
        /// Triggers before the tooltip rendering.
        /// </summary>
        [Parameter]
        public Action<SmithChartTooltipEventArgs> TooltipRender { get; set; }

        /// <summary>
        ///  Triggers before the sub-title is rendered.
        /// </summary>
        [Parameter]
        public Action<SubTitleRenderEventArgs> SubtitleRendering { get; set; }

        /// <summary>
        /// Triggers before the title is rendered.
        /// </summary>
        [Parameter]
        public Action<TitleRenderEventArgs> TitleRendering { get; set; }

        /// <summary>
        /// Triggers before export the smith chart component.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use 'OnExportComplete' event to achieve the same.")]
        [Parameter]
        public Action<SmithChartExportEventArgs> BeforeExport { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            BaseParent.SmithChartEvents = this;
        }
    }
}