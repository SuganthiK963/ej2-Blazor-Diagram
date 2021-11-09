using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// SelectedData in the chart appears inimitable from the rest of the data points.
    /// </summary>
    public class ChartSelectedDataIndexes : ChartSubComponent
    {
        [CascadingParameter]
        internal SfChart Chart { get; set; }

        internal List<ChartSelectedDataIndex> SelectedData { get; set; } = new List<ChartSelectedDataIndex>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Chart.SelectedDataIndexes = SelectedData;
        }

        internal override void ComponentDispose()
        {
            Chart = null;
            ChildContent = null;
            SelectedData = null;
        }
    }
}