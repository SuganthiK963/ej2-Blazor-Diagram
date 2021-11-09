using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization values for stockchart Axis.
    /// </summary>
    public partial class StockChartAxis : StockChartCommonAxis
    {
        internal string RendererKey;
        [CascadingParameter]
        internal StockChartAxes Parent { get; set; }

        /// <summary>
        /// If set to true, the axis will render at the opposite side of its default position.
        /// </summary>
        [Parameter]
        public override bool OpposedPosition { get; set; } = true;

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent?.Axes?.Add(this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}