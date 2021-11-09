using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    public class ChartEvents : ChartSubComponent
    {
        [CascadingParameter]
        private SfChart Container { get; set; }

        /// <summary>
        /// Triggers before each axis label is rendered.
        /// </summary>
        [Parameter]
        public Action<AxisLabelRenderEventArgs> OnAxisLabelRender { get; set; }

        /// <summary>
        /// Triggers before each axis range is rendered.
        /// </summary>
        [Parameter]
        public Action<AxisRangeCalculatedEventArgs> OnAxisActualRangeCalculated { get; set; }

        /// <summary>
        ///  Triggers before each points for the series is rendered.
        /// </summary>
        [Parameter]
        public Action<PointRenderEventArgs> OnPointRender { get; set; }

        /// <summary>
        /// Triggers before the data label for series is rendered.
        /// </summary>
        [Parameter]
        public Action<TextRenderEventArgs> OnDataLabelRender { get; set; }

        /// <summary>
        ///  Triggers before each series is rendered.
        /// </summary>
        [Parameter]
        public Action<SeriesRenderEventArgs> OnSeriesRender { get; set; }

        /// <summary>
        ///  Triggers before each legend item is rendered.
        /// </summary>
        [Parameter]
        public Action<LegendRenderEventArgs> OnLegendItemRender { get; set; }

        /// <summary>
        /// Triggers while render multiLevelLabels.
        /// </summary>
        [Parameter]
        public Action<AxisMultiLabelRenderEventArgs> OnAxisMultiLevelLabelRender { get; set; }

        /// <summary>
        /// Triggers when change the scroll.
        /// </summary>
        [Parameter]
        public Action<ScrollEventArgs> OnScrollChanged { get; set; }

        /// <summary>
        /// Triggers after the zoom selection is triggered.
        /// </summary>
        [Parameter]
        public Action<ZoomingEventArgs> OnZooming { get; set; }

        /// <summary>
        /// Triggers after the zoom selection is triggered.
        /// </summary>
        [Parameter]
        public Action<ZoomingEventArgs> OnZoomStart { get; set; }

        /// <summary>
        /// Triggers after the zoom selection is triggered.
        /// </summary>
        [Parameter]
        public Action<ZoomingEventArgs> OnZoomEnd { get; set; }

        /// <summary>
        /// Triggers when the legend item is clicked.
        /// </summary>
        [Parameter]
        public Action<LegendClickEventArgs> OnLegendClick { get; set; }

        /// <summary>
        /// Triggers when the crosshair axis value updated.
        /// </summary>
        [Parameter]
        public Action<CrosshairMoveEventArgs> OnCrosshairMove { get; set; }

        /// <summary>
        /// Triggers when the point drag end.
        /// </summary>
        [Parameter]
        public Action<DataEditingEventArgs> OnDataEditCompleted { get; set; }

        /// <summary>
        /// Triggers when the point drag start.
        /// </summary>
        [Parameter]
        public Action<DataEditingEventArgs> OnDataEdit { get; set; }

        /// <summary>
        /// Triggers after the selection is completed.
        /// </summary>
        [Parameter]
        public Action<SelectionCompleteEventArgs> OnSelectionChanged { get; set; }

        /// <summary>
        /// Triggers after the print completed.
        /// </summary>
        [Parameter]
        public Action<ExportEventArgs> OnExportComplete { get; set; }

        /// <summary>
        /// Triggers after the print completed.
        /// </summary>
        [Parameter]
        public Action OnPrintComplete { get; set; }

        /// <summary>
        /// Triggers after resizing of chart.
        /// </summary>
        [Parameter]
        public Action<ResizeEventArgs> SizeChanged { get; set; }

        /// <summary>
        /// Triggers before the tooltip for series is rendered.
        /// </summary>
        [Parameter]
#pragma warning disable CA1041
        [Obsolete]
        public Action<TooltipRenderEventArgs> TooltipRender { get; set; }

        /// <summary>
        /// Triggers before the tooltip for series is rendered.
        /// </summary>
        [Parameter]
        [Obsolete]
#pragma warning restore CA1041
        public Action<SharedTooltipRenderEventArgs> SharedTooltipRender { get; set; }

        /// <summary>
        /// Triggers on point click.
        /// </summary>
        [Parameter]
        public Action<PointEventArgs> OnPointClick { get; set; }

        /// <summary>
        /// Triggers after click on multiLevelLabelClick.
        /// </summary>
        [Parameter]
        public Action<MultiLevelLabelClickEventArgs> OnMultiLevelLabelClick { get; set; }

        /// <summary>
        /// Triggers when the chart rendering completed.
        /// </summary>
        [Parameter]
        public Action<LoadedEventArgs> Loaded { get; set; }

        /// <summary>
        /// Triggers when x axis label clicked.
        /// </summary>
        [Parameter]
        public Action<AxisLabelClickEventArgs> OnAxisLabelClick { get; set; }
        
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnExportComplete event to achieve this.")]
        [Parameter]
        public Action<IAfterExportEventArgs> AfterExport { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IAnimationCompleteEventArgs> OnAnimationComplete { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IExportEventArgs> BeforeExport { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnPrintComplete event to achieve this.")]
        [Parameter]
        public Action<IPrintEventArgs> OnPrint { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IMouseEventArgs> OnChartMouseClick { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IMouseEventArgs> OnChartMouseDown { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IMouseEventArgs> OnChartMouseLeave { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IMouseEventArgs> OnChartMouseMove { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IMouseEventArgs> OnChartMouseUp { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnDataEdit event to achieve this.")]
        [Parameter]
        public Action<IDataEditingEventArgs> Drag { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnSelectionChanged event to achieve this.")]
        [Parameter]
        public Action<IDragCompleteEventArgs> OnDragComplete { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnDataEditCompleted event to achieve this.")]
        [Parameter]
        public Action<IDataEditingEventArgs> DragEnd { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnDataEdit event to achieve this.")]
        [Parameter]
        public Action<IDataEditingEventArgs> DragStart { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnLegendClick event to achieve this.")]
        [Parameter]
        public Action<ILegendClickEventArgs> LegendClick { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<ILoadedEventArgs> Load { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnMultiLevelLabelClick event to achieve this.")]
        [Parameter]
        public Action<IMultiLevelLabelClickEventArgs> MultiLevelLabelClick { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IPointEventArgs> OnPointDoubleClick { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<IPointEventArgs> PointMoved { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use SizeChanged event to achieve this.")]
        [Parameter]
        public Action<IResizeEventArgs> Resized { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnScrollChanged event to achieve this.")]
        [Parameter]
        public Action<IScrollEventArgs> ScrollChanged { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnScrollChanged event to achieve this.")]
        [Parameter]
        public Action<IScrollEventArgs> OnScrollEnd { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnScrollChanged event to achieve this.")]
        [Parameter]
        public Action<IScrollEventArgs> OnScrollStart { get; set; }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated. Use OnSelectionChanged event to achieve this.")]
        [Parameter]
        public Action<ISelectionCompleteEventArgs> OnSelectionComplete { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Container != null)
            {
                Container.ChartEvents = this;
            }
        }
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IMouseEventArgs
    {
    }

    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This class is deprecated and will no longer be used.")]
    public class IPointEventArgs
    {
    }
}
