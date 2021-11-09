using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart zooming.
    /// </summary>
    public partial class StockChartZoomSettings : SfBaseComponent
    {
        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// If set to true, zooming will be performed on mouse up. It requires `EnableSelectionZooming` to be true.
        /// </summary>
        [Parameter]
        public bool EnableDeferredZooming { get; set; } = true;

        /// <summary>
        /// If set to true, chart can be zoomed by using mouse wheel.
        /// </summary>
        [Parameter]
        public bool EnableMouseWheelZooming { get; set; }

        /// <summary>
        /// Specifies whether chart needs to be panned by default.
        /// </summary>
        [Parameter]
        public bool EnablePan { get; set; }

        /// <summary>
        /// If to true, chart can be pinched to zoom in / zoom out.
        /// </summary>
        [Parameter]
        public bool EnablePinchZooming { get; set; }

        /// <summary>
        /// Specifies whether axis needs to have scrollbar.
        /// </summary>
        [Parameter]
        public bool EnableScrollbar { get; set; }

        /// <summary>
        /// If set to true, chart can be zoomed by a rectangular selecting region on the plot area.
        /// </summary>
        [Parameter]
        public bool EnableSelectionZooming { get; set; }

        /// <summary>
        /// Specifies whether to allow zooming vertically or horizontally or in both ways. They are,
        /// x,y: Chart can be zoomed both vertically and horizontally.
        /// x: Chart can be zoomed horizontally.
        /// y: Chart can be zoomed  vertically.
        /// It requires `EnableSelectionZooming` to be true.
        /// </summary>
        [Parameter]
        public ZoomMode Mode { get; set; } = ZoomMode.XY;

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
            Blazor.Charts.ToolbarItems.Zoom,
            Blazor.Charts.ToolbarItems.ZoomIn,
            Blazor.Charts.ToolbarItems.ZoomOut,
            Blazor.Charts.ToolbarItems.Pan,
            Blazor.Charts.ToolbarItems.Reset
        };

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.ZoomSettings = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            ToolbarItems = null;
        }
    }
}