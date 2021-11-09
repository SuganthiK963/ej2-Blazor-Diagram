using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization values for stockchart axis major tick lines.
    /// </summary>
    public partial class StockChartAxisMajorTickLines : StockChartCommonMajorTickLines
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.MajorTickLines = this;
        }
    }
}