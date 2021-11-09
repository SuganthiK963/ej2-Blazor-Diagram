using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart crosshair.
    /// </summary>
    public partial class StockChartCrosshairSettings : SfBaseComponent
    {
        internal StockChartCrosshairLine Line { get; set; } = new StockChartCrosshairLine();

        [CascadingParameter]
        internal SfStockChart Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// DashArray for crosshair.
        /// </summary>
        [Parameter]
        public string DashArray { get; set; } = "5";

        /// <summary>
        /// If set to true, crosshair line becomes visible.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// Specifies the line type. Horizontal mode enables the horizontal line and Vertical mode enables the vertical line. They are,
        /// None: Hides both vertical and horizontal crosshair lines.
        /// Both: Shows both vertical and horizontal crosshair lines.
        /// Vertical: Shows the vertical line.
        /// Horizontal: Shows the horizontal line.
        /// </summary>
        [Parameter]
        public LineType LineType { get; set; } = LineType.Both;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Crosshair = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Line = null;
        }
    }
}