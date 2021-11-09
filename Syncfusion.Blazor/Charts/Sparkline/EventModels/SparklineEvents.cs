using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the events to be triggered in the sparkline component.
    /// </summary>
    public class SparklineEvents : SfBaseComponent
    {
        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Triggers before sparkline axis render.
        /// </summary>
        [Parameter]
        public EventCallback<AxisRenderingEventArgs> OnAxisRendering { get; set; }

        /// <summary>
        /// Triggers while mouse click on the sparkline point region.
        /// </summary>
        [Parameter]
        public EventCallback<PointRegionEventArgs> OnPointRegionMouseClick { get; set; }

        /// <summary>
        /// Triggers while mouse move on point region.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<PointRegionEventArgs> OnPointRegionMouseMove { get; set; }

        /// <summary>
        /// Triggers before the sparkline rendered.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public EventCallback<SparklineLoadEventArgs> OnLoad { get; set; }

        /// <summary>
        /// Triggers while mouse click on sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<SparklineMouseEventArgs> OnSparklineMouseClick { get; set; }

        /// <summary>
        /// Triggers while mouse move on sparkline.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<SparklineMouseEventArgs> OnSparklineMouseMove { get; set; }

        /// <summary>
        /// Triggers while rendering the sparkline tooltip.
        /// </summary>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This event is deprecated and will no longer be used.")]
        [Parameter]
        public Action<TooltipRenderingEventArgs> OnTooltipInitialize { get; set; }

        /// <summary>
        /// Triggers before the sparkline datalabel render.
        /// </summary>
        [Parameter]
        public Action<DataLabelRenderingEventArgs> OnDataLabelRendering { get; set; }

        /// <summary>
        /// Triggers after the sparkline component is rendered.
        /// </summary>
        [Parameter]
        public Action<System.EventArgs> OnLoaded { get; set; }

        /// <summary>
        /// Triggers before the sparkline marker render.
        /// </summary>
        [Parameter]
        public Action<MarkerRenderingEventArgs> OnMarkerRendering { get; set; }

        /// <summary>
        /// Triggers before sparkline points render.
        /// </summary>
        [Parameter]
        public Action<SparklinePointEventArgs> OnPointRendering { get; set; }

        /// <summary>
        /// Triggers on resizing the sparkline.
        /// </summary>
        [Parameter]
        public EventCallback<SparklineResizeEventArgs> OnResizing { get; set; }

        /// <summary>
        /// Triggers before sparkline series render.
        /// </summary>
        [Parameter]
        public Action<SeriesRenderingEventArgs> OnSeriesRendering { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Events = this;
        }
    }
}