using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization options for stockchart row.
    /// </summary>
    public partial class StockChartRow : SfBaseComponent
    {
        internal StockChartRowBorder Border { get; set; } = new StockChartRowBorder();

        [CascadingParameter]
        internal StockChartRows Parent { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The height of the row as a string accept input both as '100px' and '100%'.
        /// If specified as '100%, row renders to the full height of its chart.
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "100%";

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Rows.Add(this);
        }

        internal override void ComponentDispose()
        {
            Border = null;
            Parent = null;
            ChildContent = null;
        }
    }
}