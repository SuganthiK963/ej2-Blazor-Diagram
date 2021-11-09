using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart selected data index.
    /// </summary>
    public partial class StockChartSelectedDataIndex : SfBaseComponent
    {
        [CascadingParameter]
        internal StockChartSelectedDataIndexes Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies index of point.
        /// </summary>
        [Parameter]
        public int Point { get; set; }

        /// <summary>
        /// Specifies index of series.
        /// </summary>
        [Parameter]
        public int Series { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.SelectedDataIndexes.Add(this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}