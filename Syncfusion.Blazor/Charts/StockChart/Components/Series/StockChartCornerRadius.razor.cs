using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart series corner radius.
    /// </summary>
    public partial class StockChartCornerRadius : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartSeries Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the bottom left corner radius value.
        /// </summary>
        [Parameter]
        public double BottomLeft { get; set; }

        /// <summary>
        /// Specifies the bottom right corner radius value.
        /// </summary>
        [Parameter]
        public double BottomRight { get; set; }

        /// <summary>
        /// Specifies the top left corner radius value.
        /// </summary>
        [Parameter]
        public double TopLeft { get; set; }

        /// <summary>
        /// Specifies the top right corner radius value.
        /// </summary>
        [Parameter]
        public double TopRight { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.CornerRadius = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}