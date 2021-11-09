using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the zoom settings of the chart.
    /// </summary>
    public class ChartZoomSettings: ChartSubComponent
    {
        [CascadingParameter]
        private SfChart chart { get; set; }

        /// <summary>
        /// If set to true, zooming will be performed on mouse up. It requires `EnableSelectionZooming` to be true.
        ///
        /// </summary>
        [Parameter]
        public bool EnableDeferredZooming { get; set; } = true;

        private bool enableDeferredZooming { get; set; }

        /// <summary>
        /// If set to true, chart can be zoomed by using mouse wheel.
        /// </summary>
        [Parameter]
        public bool EnableMouseWheelZooming { get; set; }

        private bool enableMouseWheelZooming { get; set; }

        /// <summary>
        /// Specifies whether chart needs to be panned by default.
        /// </summary>
        [Parameter]
        public bool EnablePan { get; set; }

        private bool enablePan { get; set; }

        /// <summary>
        /// If to true, chart can be pinched to zoom in / zoom out.
        /// </summary>
        [Parameter]
        public bool EnablePinchZooming { get; set; }

        private bool enablePinchZooming { get; set; }

        /// <summary>
        /// Specifies whether axis needs to have scrollbar.
        /// </summary>
        [Parameter]
        public bool EnableScrollbar { get; set; }

        private bool enableScrollbar { get; set; }

        /// <summary>
        /// If set to true, chart can be zoomed by a rectangular selecting region on the plot area.
        /// </summary>
        [Parameter]
        public bool EnableSelectionZooming { get; set; }

        private bool enableSelectionZooming { get; set; }

        /// <summary>
        /// Specifies whether to allow zooming vertically or horizontally or in both ways. They are,
        /// x,y: Chart can be zoomed both vertically and horizontally.
        /// x: Chart can be zoomed horizontally.
        /// y: Chart can be zoomed  vertically.
        /// It requires `EnableSelectionZooming` to be true.
        /// </summary>
        [Parameter]
        public ZoomMode Mode { get; set; } = ZoomMode.XY;

        private ZoomMode mode { get; set; }

        /// <summary>
        /// Specifies the toolkit options for the zooming as follows:
        /// Zoom
        /// ZoomIn
        /// ZoomOut
        /// Pan
        /// Reset.
        /// </summary>
        [Parameter]
        public List<ToolbarItems> ToolbarItems { get; set; } = new List<ToolbarItems>()
        {
            Syncfusion.Blazor.Charts.ToolbarItems.Zoom,
            Syncfusion.Blazor.Charts.ToolbarItems.ZoomIn,
            Syncfusion.Blazor.Charts.ToolbarItems.ZoomOut,
            Syncfusion.Blazor.Charts.ToolbarItems.Pan,
            Syncfusion.Blazor.Charts.ToolbarItems.Reset
        };

        private List<ToolbarItems> toolbarItems { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            chart.ZoomSettings = this;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        internal override void ComponentDispose()
        {
            chart = null;
            ChildContent = null;
            ToolbarItems = null;
        }
    }
}