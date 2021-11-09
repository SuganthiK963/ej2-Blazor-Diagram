using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// SelectedData in the chart appears inimitable from the rest of the data points.
    /// </summary>
    public class ChartSelectedDataIndex : ChartDefaultSelectedData
    {
        [CascadingParameter]
        internal ChartSelectedDataIndexes SelectedDataCollection { get; set; }

        internal static ChartSelectedDataIndex CreateSelectedData(int point, int series)
        {
            return new ChartSelectedDataIndex() { Point = point, Series = series };
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SelectedDataCollection = (ChartSelectedDataIndexes)Tracker;
            SelectedDataCollection.SelectedData.Add(this);
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
            SelectedDataCollection = null;
        }
    }
}