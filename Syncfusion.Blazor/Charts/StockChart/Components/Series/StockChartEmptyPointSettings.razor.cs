using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart empty point settings.
    /// </summary>
    public partial class StockChartEmptyPointSettings : SfBaseComponent
    {
        internal StockChartEmptyPointBorder Border { get; set; } = new StockChartEmptyPointBorder();

        [CascadingParameter]
        internal StockChartSeries Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// To customize the fill color of empty points.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// To customize the mode of empty points.
        /// </summary>
        [Parameter]
        public EmptyPointMode Mode { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.EmptyPointSettings = this;
        }

        internal override void ComponentDispose()
        {
            Border = null;
            Parent = null;
            ChildContent = null;
        }
    }
}