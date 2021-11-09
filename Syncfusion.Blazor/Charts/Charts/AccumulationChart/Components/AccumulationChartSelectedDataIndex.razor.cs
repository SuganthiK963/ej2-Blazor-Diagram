using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// SelectedData in the accumulation chart appears inimitable from the rest of the data points.
    /// </summary>
#pragma warning disable CA1067
    public partial class AccumulationChartSelectedDataIndex
#pragma warning restore CA1067
    {
        [CascadingParameter]
        private AccumulationChartSelectedDataIndexes selectedDataCollection { get; set; }

        /// <summary>
        /// Gets and sets the content of the UI element.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
#pragma warning disable CA2007
            await base.OnInitializedAsync();
#pragma warning restore CA2007
            selectedDataCollection.SelectedData.Add(this);
        }

        public override void Dispose()
        {
            ChildContent = null;
            selectedDataCollection = null;
        }

        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(AccumulationChartSelectedDataIndex other)
        {
            return other != null ? other.Point == Point && other.Series == Series : false;
        }

        internal static AccumulationChartSelectedDataIndex CreateSelectedData(int point, int series)
        {
            return new AccumulationChartSelectedDataIndex() { Point = point, Series = series };
        }
    }
}