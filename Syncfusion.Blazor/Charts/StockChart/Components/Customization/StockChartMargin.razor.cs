using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart datalabel margin.
    /// </summary>
    public partial class StockChartMargin : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartDataLabel Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Bottom margin in pixels.
        /// </summary>
        [Parameter]
        public double Bottom { get; set; } = 10;

        /// <summary>
        /// Left margin in pixels.
        /// </summary>
        [Parameter]
        public double Left { get; set; } = 10;

        /// <summary>
        /// Right margin in pixels.
        /// </summary>
        [Parameter]
        public double Right { get; set; } = 10;

        /// <summary>
        /// Top margin in pixels.
        /// </summary>
        [Parameter]
        public double Top { get; set; } = 10;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Margin = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}