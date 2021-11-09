using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Linq;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options to customize the border for range navigator series.
    /// </summary>
    public partial class RangeNavigatorSeriesBorder : ChartCommonBorder
    {
        [CascadingParameter]
        internal RangeNavigatorSeries Series { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Series.UpdateSeriesProperties("Border", this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary(nameof(Series.Border), this, Series.PropertyChanges);
                PropertyChanges.Clear();
                await Series.SeriesPropertyChanged();
            }
        }

        internal override void ComponentDispose()
        {
            Series = null;
        }
    }
}