using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// To specify customization values for stockchart axis major gridlines.
    /// </summary>
    public partial class StockChartAxisMajorGridLines : StockChartCommonMajorGridLines
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.MajorGridLines = this;
        }
    }
}