using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Gets and sets the events for the accumulation chart.
    /// </summary>
    public class AccumulationChartEvents : SfBaseComponent
    {
        [CascadingParameter]
        private IAccumulationChart AccumulationChartInstance { get; set; }

        /// <summary>
        /// Triggers after the print completed.
        /// </summary>
        [Parameter]
        public Action OnPrintComplete { get; set; }

        /// <summary>
        /// Triggers after the print completed.
        /// </summary>
        [Parameter]
        public Action<ExportEventArgs> OnExportComplete { get; set; }

        /// <summary>
        /// Triggers after resizing of chart.
        /// </summary>
        [Parameter]
        public Action<AccumulationResizeEventArgs> SizeChanged { get; set; }

        /// <summary>
        /// Triggers before the tooltip for series is rendered.
        /// </summary>
        [Parameter]
        public Action<TooltipRenderEventArgs> TooltipRender { get; set; }

        /// <summary>
        /// Triggers before series getting renderred.
        /// </summary>
        [Parameter]
        public Action<AccumulationTextRenderEventArgs> OnDataLabelRender { get; set; }

        /// <summary>
        /// Triggeres before the point rendering.
        /// </summary>
        [Parameter]
        public Action<AccumulationPointRenderEventArgs> OnPointRender { get; set; }

        /// <summary>
        /// Triggers before legend getting rendered.
        /// </summary>
        [Parameter]
        public Action<AccumulationLegendRenderEventArgs> OnLegendItemRender { get; set; }

        /// <summary>
        /// Triggers when the point click.
        /// </summary>
        [Parameter]
        public Action<AccumulationPointEventArgs> OnPointClick { get; set; }

        /// <summary>
        /// Triggers when the chart render completed.
        /// </summary>
        [Parameter]
        public Action<AccumulationLoadedEventArgs> Loaded { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnExportComplete event to achieve this.")]
        [Parameter]
        public EventCallback<IAfterExportEventArgs> AfterExport { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IAccAnimationCompleteEventArgs> OnAnimationComplete { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnPrintComplete event to achieve this.")]
        [Parameter]
        public EventCallback<IPrintEventArgs> OnPrint { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IMouseEventArgs> OnChartMouseClick { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IMouseEventArgs> OnChartMouseDown { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IMouseEventArgs> OnChartMouseLeave { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IMouseEventArgs> OnChartMouseMove { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IMouseEventArgs> OnChartMouseUp { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IAccLoadedEventArgs> Load { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<IPointEventArgs> PointMoved { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use SizeChanged event to achieve this.")]
        [Parameter]
        public EventCallback<IAccResizeEventArgs> Resized { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            AccumulationChartInstance.AccumulationChartEvents = this;
        }
    }
}