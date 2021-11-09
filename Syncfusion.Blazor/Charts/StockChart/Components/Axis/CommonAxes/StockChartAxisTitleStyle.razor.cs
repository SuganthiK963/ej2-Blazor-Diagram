using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization values for stockchart axis title.
    /// </summary>
    public partial class StockChartAxisTitleStyle : ChartCommonFont
    {
        [CascadingParameter]
        internal StockChartCommonAxis Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Unique size of the axis labels.
        /// </summary>
        [Parameter]
        public override string Size { get; set; } = "14px";

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.TitleStyle = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}